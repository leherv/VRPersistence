using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using VRPersistence.BO;
using VRPersistence.DataStores;

namespace VRPersistence.Services
{
    public class ReleaseService: IReleaseService
    {
        private readonly IReleaseDataStore _releaseDataStore;
        private readonly IMediaDataStore _mediaDataStore;
        private readonly ILogger<ReleaseService> _logger;

        public ReleaseService(IReleaseDataStore releaseDataStore, ILogger<ReleaseService> logger, IMediaDataStore mediaDataStore)
        {
            _releaseDataStore = releaseDataStore;
            _logger = logger;
            _mediaDataStore = mediaDataStore;
        }

        public async Task<Result<IEnumerable<Release>>> GetNotNotified(string mediaName)
        {
            var result = await _releaseDataStore.GetNotNotified(mediaName);
            return result.IsSuccess 
                ? Result.Success(result.Value.Select(releaseDao => new Release(releaseDao)))
                : Result.Failure<IEnumerable<Release>>($"Failed to load non notified releases for media ${mediaName}");
        }
        
        public async Task<Result> AddRelease(Release release)
        {
            if (await IsNewNewest(release))
            {
                var releaseDao = new DAO.Release(release);
                var mediaResult = await _mediaDataStore.GetMedia(release.Media.MediaName);
                if (mediaResult.IsSuccess)
                {
                    releaseDao.Media = mediaResult.Value;
                }
                _logger.LogInformation("Release with releaseNumber {releaseNumber} is the newest for {mediaName} so it will be added" , release.ReleaseNumber.ToString(), release.Media.MediaName);
                return await _releaseDataStore.AddRelease(releaseDao);
            }
            _logger.LogInformation("Release with releaseNumber {releaseNumber} is not newer for {mediaName} so it will be discarded", release.ReleaseNumber.ToString(), release.Media.MediaName);
            return Result.Failure($"Release with releaseNumber {release.ReleaseNumber.ToString()} is not newer for {release.Media.MediaName}");
        }

        private async Task<bool> IsNewNewest(Release release)
        {
            var currentNewestResult = await _releaseDataStore.GetNewestReleaseForMedia(release.Media.MediaName);
            if (currentNewestResult.IsSuccess)
            {
                // no release yet for this media
                if (currentNewestResult.Value == null) return true;
                return release.IsNewerThan(currentNewestResult.Value);
            }

            return false;
        }

        public async Task<List<Result>> AddReleases(IEnumerable<Release> releases)
        {
            var results = new List<Result>();
            foreach (var release in releases)
            {
                results.Add(await AddRelease(release));
            }
            return results;
        }

        public async Task<List<Result>> SetNotified(IEnumerable<SetNotified> setNotified)
        {
            var results = new List<Result>();
            foreach (var s in setNotified)
            {
                var result = await _releaseDataStore.GetRelease(s.ReleaseId);
                if (result.IsSuccess)
                {
                    var releaseDao = result.Value;
                    _logger.LogInformation("Found release to set notified for id {id}", s.ReleaseId.ToString());
                    if (releaseDao.Notified)
                    {
                        _logger.LogInformation("Release was already notified, nothing to do.");
                    }
                    results.Add(await _releaseDataStore.SetNotified(releaseDao));
                }
                else
                {
                    results.Add(Result.Failure($"Could not find release with id {s.ReleaseId.ToString()}"));
                }
            }
            return results;
        }
    }
}