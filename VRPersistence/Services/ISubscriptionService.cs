using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.Services
{
    public interface ISubscriptionService
    {
        Task<Result> AddSubscription(Subscription subscription);
        Task<Result<IEnumerable<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName);

    }
}