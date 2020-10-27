using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using VRPersistence.BO;

namespace VRPersistence.DataStores
{
    public class ReleaseDataStore: IReleaseDataStore
    {
        private readonly VRPersistenceDbContext _dbContext;

        public ReleaseDataStore(VRPersistenceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Result<Release>> GetRelease(string mediaName, int releaseNumber)
        {
            var releaseDao = await _dbContext.Releases
                .FirstOrDefaultAsync(r => r.Media.MediaName == mediaName && r.ReleaseNumber == releaseNumber);
            if (releaseDao == null) return Result.Failure<Release>("No release found");
            var releaseBo = new Release(releaseDao);
            return Result.Success(releaseBo);
        }

        public async Task<Result> AddRelease(Release release)
        {
            var releaseDao = new DAO.Release(release);
            await _dbContext.Releases.AddAsync(releaseDao);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> IsNewNewest(Release release)
        {
            var newerRelease = await _dbContext.Releases
                .FirstOrDefaultAsync(r => r.Media.MediaName == release.Media.MediaName && r.ReleaseNumber >= release.ReleaseNumber);
            return newerRelease == null
                ? Result.Success()
                : Result.Failure("The Release is not newer than the already added releases");
        }
     }
}