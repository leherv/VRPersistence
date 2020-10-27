using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.DataStores
{
    public interface IReleaseDataStore
    {
        public Task<Result<Release>> GetRelease(string mediaName, int releaseNumber);
        public Task<Result> AddRelease(Release release);

        public Task<Result> IsNewNewest(Release release);
    }
}