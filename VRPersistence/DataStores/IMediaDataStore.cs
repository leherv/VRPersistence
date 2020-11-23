using CSharpFunctionalExtensions;
using VRPersistence.DAO;

namespace VRPersistence.DataStores
{
    public interface IMediaDataStore
    {
        Result<Media> GetMedia(string mediaName);
    }
}