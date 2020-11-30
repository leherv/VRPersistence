using FluentValidation;
using VRPersistence.Config;

namespace VRPersistence.DTO.request
{
    public class AddReleaseDTO
    {
        public string MediaName { get; set; }
        public int ReleaseNumber { get; set; }
        public int SubReleaseNumber { get; set; }
        public string Url { get; set; }
    }

    public class ReleaseValidator : AbstractValidator<AddReleaseDTO>
    {
        public ReleaseValidator(TrackedMediaSettings trackedMediaSettings)
        {
            RuleFor(r => r.MediaName).NotEmpty();
            RuleFor(r => r.MediaName)
                .Must(m => trackedMediaSettings.MediaNames.Contains(m.ToLower()))
                .WithMessage("Media of release to add is not included in the MediaSettings");
            RuleFor(r => r.Url).NotEmpty();
            RuleFor(r => r.ReleaseNumber).Must(n => n > -1);
        }
    }
}