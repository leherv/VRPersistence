using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface IReleaseDataStore
    {
        Task<Result<List<Release>>> GetNotNotified(string mediaName);
        public Task<Result<Release>> GetRelease(string mediaName, int releaseNumber);
        public Task<Result<Release>> GetRelease(long id);
        public Task<Result> AddRelease(Release release);

        public Task<Result<Release>> GetNewestReleaseForMedia(string mediaName);
        public Task<Result> SetNotified(Release release);
    }
}