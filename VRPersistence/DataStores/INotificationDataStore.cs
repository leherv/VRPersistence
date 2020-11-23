using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface INotificationDataStore
    {
        Task<Result> AddNotificationEndpoint(NotificationEndpoint notificationEndpoint);
        Result<NotificationEndpoint> GetNotificationEndpoint(string identifier);
    }
}