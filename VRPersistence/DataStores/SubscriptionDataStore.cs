using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public class SubscriptionDataStore : ISubscriptionDataStore
    {
        private readonly VRPersistenceDbContext _dbContext;
        private readonly ILogger<SubscriptionDataStore> _logger;

        public SubscriptionDataStore(VRPersistenceDbContext dbContext, ILogger<SubscriptionDataStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> AddSubscription(Subscription subscription)
        {
            try
            {
                if (!SubscriptionExists(subscription.Media.Id, subscription.NotificationEndpoint.Id))
                {
                    await _dbContext.Subscriptions.AddAsync(subscription);
                    await _dbContext.SaveChangesAsync();
                }
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "Adding subscription for notificationEndpoint with identifier {identifier} for media {mediaName} failed due to: {exceptionMessage}. ",
                    subscription.NotificationEndpoint.Identifier,
                    subscription.Media.MediaName,
                    e.Message);
                return Result.Failure("Adding the subscription to the database failed.");
            }
        }

        private bool SubscriptionExists(long mediaId, long notificationEndpointId)
        {
            return GetSubscription(mediaId, notificationEndpointId).IsSuccess;
        }

        private Result<Subscription> GetSubscription(long mediaId, long notificationEndpointId)
        {
            var subscription = _dbContext.Subscriptions.FirstOrDefault(s =>
                s.MediaId == mediaId &&
                s.NotificationEndpointId == notificationEndpointId);
            if (subscription == null)
            {
                return Result.Failure<Subscription>(
                    $"No subscription for mediaId {mediaId} and notificationEndpointId {notificationEndpointId} found.");
            }

            return Result.Success(subscription);
        }

        public async Task<Result<IEnumerable<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName)
        {
            try
            {
                var notificationEndpoints = _dbContext.Subscriptions
                    .Where(s => s.Media.MediaName.ToLower().Equals(mediaName.ToLower()))
                    .Include(s => s.NotificationEndpoint)
                    .AsEnumerable()
                    .Select(s => s.NotificationEndpoint);
                return Result.Success(notificationEndpoints);
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    "Fetching subscribed endpoints for media {mediaName} failed due to: {exceptionMessage}. ",
                    mediaName,
                    e.Message);
                return Result.Failure<IEnumerable<NotificationEndpoint>>("Adding the subscription to the database failed.");
            }
        }
    }
}