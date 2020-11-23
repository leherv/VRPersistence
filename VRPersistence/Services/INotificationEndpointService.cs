using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.Services
{
    public interface INotificationEndpointService
    {
        Task<Result> AddNotificationEndpoint(NotificationEndpoint notificationEndpoint);
    }
}