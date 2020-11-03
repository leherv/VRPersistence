using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VRPersistence.BO;
using VRPersistence.Config;
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
        private readonly TrackedMediaSettings _trackedMediaSettings;

        public ReleaseController(ILogger<ReleaseController> logger, IReleaseService releaseService, IOptions<TrackedMediaSettings> trackedMediaSettings)
        {
            _logger = logger;
            _releaseService = releaseService;
            _trackedMediaSettings = trackedMediaSettings.Value;
        }
        
        [HttpPost]
        public async Task<JsonResult> AddReleases([FromBody] AddReleasesDTO addReleaseDtos)
        {
            try
            {
                var results = new List<Result>();
                // we do not use the automatic integration of fluentValidation into ASP.NET Core (validating objects that are passed in to controller actions), as we want to add ALL valid releases and not stop and throw if one is invalid)
                var releaseValidator = new ReleaseValidator(_trackedMediaSettings);
                foreach (var addReleaseDto in addReleaseDtos.Releases)
                {
                    var validationResult = await releaseValidator.ValidateAsync(addReleaseDto);
                    if (validationResult.IsValid)
                    {
                        results.Add(await _releaseService.AddRelease(new Release(addReleaseDto)));
                    }
                    else
                    {
                        results.Add(Result.Failure("this one is invalid"));
                    }
                }
                return new JsonResult(results.Select(r => r.AsSerializableResult()));
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result.Failure("Adding the release failed.").AsSerializableResult());
            }
        }

        [HttpPatch]
        public async Task<JsonResult> SetNotified([FromBody] SetNotifiedMessagesDTO setNotifiedMessagesDto)
        {
            try
            {
                var setNotifiedMessages = setNotifiedMessagesDto.SetNotifiedMessages.Select(notifiedDto => new SetNotified(notifiedDto));
                var results = await _releaseService.SetNotified(setNotifiedMessages);
                return new JsonResult(results.Select(r => r.AsSerializableResult()));
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong. {exceptionMessage}", e.Message);
                return new JsonResult(Result.Failure("Setting the release notified failed.").AsSerializableResult());
            }
        }

        // [HttpGet("{mediaName}")]
        // public async Task<JsonResult> GetNotNotified() 
        // {
        //     return new JsonResult();
        // }
    }
}