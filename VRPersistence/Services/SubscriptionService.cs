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
            var mediaResult = await _mediaDataStore.GetMedia(subscription.Media.MediaName);
            if (mediaResult.IsFailure)
            {
                _logger.LogError("Media {media} that should be subscribed to does not exist.", subscription.Media.MediaName);
                return Result.Failure(
                    $"Media {subscription.Media.MediaName} that should be subscribed to does not exist.");
            }
            var notificationEndpointResult =
                await _notificationDataStore.GetNotificationEndpoint(subscription.NotificationEndpoint.Identifier);
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

        public async Task<Result> DeleteSubscription(string mediaName, string notificationEndpointIdentifier)
        {
            var mediaResult = await _mediaDataStore.GetMedia(mediaName);
            if (mediaResult.IsFailure)
            {
                _logger.LogError("Media with name {mediaName}, referenced in the subscription to delete, does not exist.", mediaName);
                return Result.Failure(
                    $"Media with name {mediaName}, referenced in the subscription to delete, does not exist.");
            }
            var notificationEndpointResult =
                await _notificationDataStore.GetNotificationEndpoint(notificationEndpointIdentifier);
            if (notificationEndpointResult.IsFailure)
            {
                _logger.LogError("NotificationEndpoint with identifier {identifier}, referenced in the subscription to delete, does not exist.", notificationEndpointIdentifier);
                return Result.Failure(
                    $"NotificationEndpoint with identifier {notificationEndpointIdentifier}, referenced in the subscription to delete, does not exist.");
            }

            var subscriptionResult = await _subscriptionDataStore.GetSubscription(mediaResult.Value.Id, notificationEndpointResult.Value.Id);
            if (subscriptionResult.IsFailure)
            {
                _logger.LogError("There is no subscription for mediaName {mediaName} and notificationEndpointIdentifier {notificationEndpointIdentifier} to delete", mediaName, notificationEndpointIdentifier);
                return Result.Failure($"There is no subscription for mediaName {mediaName} and notificationEndpointIdentifier {notificationEndpointIdentifier} to delete");
            }
            
            return await _subscriptionDataStore.DeleteSubscription(subscriptionResult.Value);
        }

        public async Task<Result<List<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName)
        {
            var result = await _subscriptionDataStore.GetSubscribedNotificationEndpoints(mediaName);
            return result.IsSuccess
                ? Result.Success(result.Value.Select(e => new NotificationEndpoint(e)).ToList())
                : Result.Failure<List<NotificationEndpoint>>(
                    $"Failed to get subscribed notificationEndpoints for media {mediaName}");
        }

        public async Task<Result<List<Media>>> GetSubscribedToMedia(string notificationEndpointId)
        {
            var result =  await _subscriptionDataStore.GetSubscribedToMedia(notificationEndpointId);
            return result.IsSuccess
                ? Result.Success(result.Value.Select(m => new Media(m)).ToList())
                : Result.Failure<List<Media>>(result.Error);
        }
    }
}