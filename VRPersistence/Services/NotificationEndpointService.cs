using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.BO;
using VRPersistence.DataStores;

namespace VRPersistence.Services
{
    public class NotificationEndpointService : INotificationEndpointService
    {
        private readonly INotificationDataStore _notificationDataStore;
        private readonly ILogger<NotificationEndpointService> _logger;

        public NotificationEndpointService(INotificationDataStore notificationDataStore, ILogger<NotificationEndpointService> logger)
        {
            _notificationDataStore = notificationDataStore;
            _logger = logger;
        }

        public async Task<Result> AddNotificationEndpoint(NotificationEndpoint notificationEndpoint)
        {
            var notificationEndpointDao = new DAO.NotificationEndpoint(notificationEndpoint);
            return await _notificationDataStore.AddNotificationEndpoint(notificationEndpointDao);
        }
    }
}