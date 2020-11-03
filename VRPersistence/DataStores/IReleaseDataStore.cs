using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface IReleaseDataStore
    {
        public Task<Result<Release>> GetRelease(string mediaName, int releaseNumber);
        public Task<Result<Release>> GetRelease(long id);
        public Task<Result> AddRelease(Release release);

        public Task<Result> IsNewNewest(string mediaName, int releaseNumber);
        public Task<Result> SetNotified(Release release);
    }
}