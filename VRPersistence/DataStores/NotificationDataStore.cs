using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public class NotificationDataStore : INotificationDataStore
    {
        private readonly VRPersistenceDbContext _dbContext;
        private readonly ILogger<NotificationDataStore> _logger;

        public NotificationDataStore(VRPersistenceDbContext dbContext, ILogger<NotificationDataStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> AddNotificationEndpoint(NotificationEndpoint notificationEndpoint)
        {
            try
            {
                if (!NotificationEndpointExists(notificationEndpoint.Identifier))
                {
                    await _dbContext.AddAsync(notificationEndpoint);
                    await _dbContext.SaveChangesAsync();
                }
               
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Adding notificationEndpoint with identifier {identifier} failed due to: {exceptionMessage}. ",
                    notificationEndpoint.Identifier,
                    e.Message);
                return Result.Failure("Adding the notificationEndpoint to the database failed.");
            }
        }
        
        private bool NotificationEndpointExists(string identifier)
        {
            return GetNotificationEndpoint(identifier).IsSuccess;
        }
        
        public Result<NotificationEndpoint> GetNotificationEndpoint(string identifier)
        {
            try
            {
                var endpoint = _dbContext.NotificationEndpoints
                    .FirstOrDefault(n => n.Identifier.Equals(identifier));
                 if (endpoint == null)
                 {
                     return Result.Failure<NotificationEndpoint>(
                         $"No notificationEndpoint for identifier {identifier} found.");
                 }

                 return Result.Success(endpoint);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Fetching notificationEndpoint with identifier {identifier} failed due to: {exceptionMessage}.",
                    identifier,
                    e.Message);
                return Result.Failure<NotificationEndpoint>("Fetching the notificationEndpoint failed.");
            }
        }
    }
}