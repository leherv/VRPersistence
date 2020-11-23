using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VRPersistence.Config;
using VRPersistence.Extensions;
using VRPersistence.Services;

namespace VRPersistence.Controllers
{
    [ApiController]
    [Route("api/notificationendpoint")]
    public class NotificationEndpointController
    {
        private readonly ILogger<NotificationEndpointController> _logger;
        private readonly TrackedMediaSettings _trackedMediaSettings;
        private readonly ISubscriptionService _subscriptionService;

        public NotificationEndpointController(ILogger<NotificationEndpointController> logger,
            IOptions<TrackedMediaSettings> trackedMediaSettings,
            ISubscriptionService subscriptionService)
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
            _trackedMediaSettings = trackedMediaSettings.Value;
        }


        [HttpGet("{mediaName}")]
        public async Task<JsonResult> GetSubscribedEndpoints(string mediaName)
        {
            try
            {
                var result = await _subscriptionService.GetSubscribedNotificationEndpoints(mediaName);
                return new JsonResult(result.AsSerializableResult());
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result.Failure("Getting subscribed endpoints went wrong.")
                    .AsSerializableResult());
            }
        }
    }
}