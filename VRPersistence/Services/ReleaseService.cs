using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.DAO;
using VRPersistence.DataStores;

namespace VRPersistence.Services
{
    public class ReleaseService: IReleaseService
    {
        private readonly IReleaseDataStore _releaseDataStore;
        private readonly ILogger<ReleaseService> _logger;

        public ReleaseService(IReleaseDataStore releaseDataStore, ILogger<ReleaseService> logger)
        {
            _releaseDataStore = releaseDataStore;
            _logger = logger;
        }
        
        public async Task<Result> AddRelease(BO.Release release)
        {
            var isNewNewestResult = await _releaseDataStore.IsNewNewest(release);
            if (isNewNewestResult.IsSuccess)
            {
                _logger.LogInformation("Release with {releaseNumber} is the newest for {mediaName} so it will be added" , release.ReleaseNumber.ToString(), release.Media.MediaName);
                return await _releaseDataStore.AddRelease(release);
            }
            _logger.LogInformation("Release with releaseNumber {releaseNumber} is not newer for {mediaName} so it will be discarded", release.ReleaseNumber.ToString(), release.Media.MediaName);
            return Result.Success();
        }

        public async Task<Result<BO.Release>> GetRelease(string mediaName, int releaseNumber)
        {
            var result = await _releaseDataStore.GetRelease(mediaName, releaseNumber);
            return result;
        }
        
    }
}