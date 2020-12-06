using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface IMediaDataStore
    {
        Task<Result<Media>> GetMedia(string mediaName);
    }
}