using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VRPersistence.BO;
using VRPersistence.Config;
using VRPersistence.DTO;
using VRPersistence.Extensions;
using VRPersistence.Services;

namespace VRPersistence.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly TrackedMediaSettings _trackedMediaSettings;
        private readonly INotificationEndpointService _notificationEndpointService;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ILogger<SubscriptionController> logger,
            IOptions<TrackedMediaSettings> trackedMediaSettings,
            INotificationEndpointService notificationEndpointService, ISubscriptionService subscriptionService)
        {
            _logger = logger;
            _notificationEndpointService = notificationEndpointService;
            _subscriptionService = subscriptionService;
            _trackedMediaSettings = trackedMediaSettings.Value;
        }


        [HttpPost]
        public async Task<JsonResult> AddSubscriptions([FromBody] AddSubscriptionsDTO addSubscriptionsDto)
        {
            try
            {
                var results = new List<Result>();
                var result =
                    await _notificationEndpointService.AddNotificationEndpoint(
                        new NotificationEndpoint(addSubscriptionsDto.NotificationEndpointIdentifier));
                if (result.IsSuccess)
                {
                    // we do not use the automatic integration of fluentValidation into ASP.NET Core (validating objects that are passed in to controller actions), as we want to add ALL valid releases and not stop and throw if one is invalid)
                    var subscriptionValidator = new SubscriptionValidator(_trackedMediaSettings);
                    foreach (var addSubscription in addSubscriptionsDto.Subscriptions)
                    {
                        var validationResult = await subscriptionValidator.ValidateAsync(addSubscription);
                        if (validationResult.IsValid)
                        {
                            results.Add(await _subscriptionService.AddSubscription(new Subscription(addSubscription,
                                addSubscriptionsDto.NotificationEndpointIdentifier)));
                        }
                        else
                        {
                            results.Add(Result.Failure(validationResult.GetMessage()));
                        }
                    }
                }
                else
                {
                    results.Add(result);
                }

                return new JsonResult(results.Select(r => r.AsSerializableResult()));
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result.Failure("Adding the subscriptions failed.").AsSerializableResult());
            }
        }

        [HttpGet("{notificationEndpointIdentifier}")]
        public async Task<JsonResult> GetSubscribedToMedia(string notificationEndpointIdentifier)
        {
            try
            {
                var result = await _subscriptionService.GetSubscribedToMedia(notificationEndpointIdentifier);
                return new JsonResult(result.AsSerializableResult());
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result
                    .Failure(
                        $"Fetching the media that noticationEndpoint with identifier {notificationEndpointIdentifier} is subscribed to failed.")
                    .AsSerializableResult());
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteSubscriptions([FromBody] DeleteSubscriptionsDTO deleteSubscriptionsDto)
        {
            try
            {
                var results = new List<Result>();
                // we do not use the automatic integration of fluentValidation into ASP.NET Core (validating objects that are passed in to controller actions), as we want to add ALL valid releases and not stop and throw if one is invalid)
                var subscriptionValidator = new DeleteSubscriptionValidator(_trackedMediaSettings);
                foreach (var deleteSubscription in deleteSubscriptionsDto.Subscriptions)
                {
                    var validationResult = await subscriptionValidator.ValidateAsync(deleteSubscription);
                    if (validationResult.IsValid)
                    {
                        results.Add(await _subscriptionService.DeleteSubscription(deleteSubscription.MediaName, deleteSubscriptionsDto.NotificationEndpointIdentifier));
                    }
                    else
                    {
                        results.Add(Result.Failure(validationResult.GetMessage()));
                    }
                }
                return new JsonResult(results.Select(r => r.AsSerializableResult()));
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result.Failure("Deleting the subscriptions failed.").AsSerializableResult());
            }
        }
    }
}