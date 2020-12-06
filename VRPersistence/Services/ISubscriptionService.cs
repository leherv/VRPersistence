using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.Services
{
    public interface ISubscriptionService
    {
        Task<Result> AddSubscription(Subscription subscription);
        Task<Result<List<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName);
        Task<Result<List<Media>>> GetSubscribedToMedia(string notificationEndpointId);
        Task<Result> DeleteSubscription(string mediaName, string notificationEndpointIdentifier);
    }
}