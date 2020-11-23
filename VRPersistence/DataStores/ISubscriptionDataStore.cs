using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface ISubscriptionDataStore
    {
        Task<Result> AddSubscription(Subscription subscription);
        Task<Result<IEnumerable<NotificationEndpoint>>> GetSubscribedNotificationEndpoints(string mediaName);
    }
}