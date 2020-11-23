using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.BO;
using VRPersistence.DataStores;

namespace VRPersistence.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionDataStore _subscriptionDataStore;
        private readonly IMediaDataStore _mediaDataStore;
        private readonly INotificationDataStore _notificationDataStore;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(ILogger<SubscriptionService> logger, ISubscriptionDataStore subscriptionDataStore, IMediaDataStore mediaDataStore, INotificationDataStore notificationDataStore)
        {
            _logger = logger;
            _subscriptionDataStore = subscriptionDataStore;
            _mediaDataStore = mediaDataStore;
            _notificationDataStore = notificationDataStore;
        }

        public async Task<Result> AddSubscription(Subscription subscription)
        {
            var subscriptionDao = new DAO.Subscription(subscription);
            var mediaResult = _mediaDataStore.GetMedia(subscription.Media.MediaName);
            if (mediaResult.IsFailure)
            {
                _logger.LogError("Media {media} that should be subscribed to does not exist.", subscription.Media.MediaName);
                return Result.Failure(
                    $"Media {subscription.Media.MediaName} that should be subscribed to does not exist.");
            }
            var notificationEndpointResult =
                _notificationDataStore.GetNotificationEndpoint(subscription.NotificationEndpoint.Identifier);
            if (notificationEndpointResult.IsFailure)
            {
                _logger.LogError("NotificationEndpoint {endpoint} that should be used for the subscription to does not exist.", subscription.NotificationEndpoint.Identifier);
                return Result.Failure(
                    $"NotificationEndpoint {subscription.NotificationEndpoint.Identifier} that should be used for the subscription to does not exist.");
            }

            subscriptionDao.Media = mediaResult.Value;
            subscriptionDao.NotificationEndpoint = notificationEndpointResult.Value;
            return await _subscriptionDataStore.AddSubscription(subscriptionDao);
        }

        public async Task<Result<IEnumerable<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName)
        {
            var result = await _subscriptionDataStore.GetSubscribedNotificationEndpoints(mediaName);
            return result.IsSuccess
                ? Result.Success(result.Value.Select(e => new NotificationEndpoint(e)))
                : Result.Failure<IEnumerable<NotificationEndpoint>>(
                    $"Failed to get subscribed notificationEndpoints for media {mediaName}");
        }
    }
}