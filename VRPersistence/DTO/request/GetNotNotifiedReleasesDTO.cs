using FluentValidation;
using Microsoft.Extensions.Options;
using VRPersistence.Config;

namespace VRPersistence.DTO.request
{
    public class GetNotNotifiedReleasesDTO
    {
        public string MediaName { get; set; }
    }

    public class GetNotNotifiedReleasesValidator : AbstractValidator<GetNotNotifiedReleasesDTO>
    {
        public GetNotNotifiedReleasesValidator(IOptions<TrackedMediaSettings> trackedMediaSettings)
        {
            var settings = trackedMediaSettings.Value;
            RuleFor(r => r.MediaName)
                .Must(m => settings.MediaNames.Contains(m.ToLower()))
                .WithMessage("Media of release to add is not included in the MediaSettings");
        }
    }
}