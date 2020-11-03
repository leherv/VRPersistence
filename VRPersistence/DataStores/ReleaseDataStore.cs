using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public class ReleaseDataStore: IReleaseDataStore
    {
        private readonly VRPersistenceDbContext _dbContext;
        private readonly ILogger<ReleaseDataStore> _logger;

        public ReleaseDataStore(VRPersistenceDbContext dbContext, ILogger<ReleaseDataStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<Result<Release>> GetRelease(string mediaName, int releaseNumber)
        {
            var release = await _dbContext.Releases
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Media.MediaName == mediaName && r.ReleaseNumber == releaseNumber);
            return release == null ? Result.Failure<Release>("No release found") : Result.Success(release);
        }
        
        public async Task<Result<Release>> GetRelease(long id)
        {
            var release = await _dbContext.Releases
                .Include(r => r.Media)
                .FirstOrDefaultAsync(r => r.Id == id);
            return release == null ? Result.Failure<Release>("No release found") : Result.Success(release);
        }
        
        public async Task<Result> AddRelease(Release release)
        {
            try
            {
                await _dbContext.Releases.AddAsync(release);
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Adding release with releaseNumber {releaseNumber} for {mediaName} failed due to: {exceptionMessage}. ", release.ReleaseNumber.ToString(), release.Media.MediaName, e.Message);
                return Result.Failure("Adding the release to the database failed.");
            }
        }

        public async Task<Result> IsNewNewest(string mediaName, int releaseNumber)
        {
            var newerRelease = await _dbContext.Releases
                .FirstOrDefaultAsync(r => r.Media.MediaName == mediaName && r.ReleaseNumber >= releaseNumber);
            return newerRelease == null
                ? Result.Success()
                : Result.Failure("The Release is not newer than the already added releases");
        }

        public async Task<Result> SetNotified(Release release)
        {
            try
            {
                release.Notified = true;
                await _dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Setting release with releaseNumber {releaseNumber} for {mediaName} to notified failed due to: {exceptionMessage}. ", release.ReleaseNumber.ToString(), release.Media.MediaName, e.Message);
                return Result.Failure("Setting the release to notified failed.");
            }
            
        }
    }
}