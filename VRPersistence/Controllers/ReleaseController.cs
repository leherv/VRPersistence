using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VRPersistence.BO;
using VRPersistence.DTO;
using VRPersistence.Extensions;
using VRPersistence.Services;

namespace VRPersistence.Controllers
{
    [ApiController]
    [Route("api/release")]
    public class ReleaseController
    {
        private readonly ILogger<ReleaseController> _logger;
        private readonly IReleaseService _releaseService;

        public ReleaseController(ILogger<ReleaseController> logger, IReleaseService releaseService)
        {
            _logger = logger;
            _releaseService = releaseService;
        }
        
        [HttpPost]
        public async Task<JsonResult> AddRelease([FromBody]AddReleaseDTO addReleaseDto)
        {
            var release = new Release(addReleaseDto);
            var result = await _releaseService.AddRelease(release);
            return new JsonResult(result.AsSerializableResult());
        }
        
    }
}