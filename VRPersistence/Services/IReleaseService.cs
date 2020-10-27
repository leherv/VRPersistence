using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using VRPersistence.BO;

namespace VRPersistence.Services
{
    public interface IReleaseService
    {
        Task<Result> AddRelease(Release release);
    }
}