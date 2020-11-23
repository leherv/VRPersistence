using FluentValidation;
using VRPersistence.Config;

namespace VRPersistence.DTO.request
{
    public class AddSubscriptionDTO
    {
        public string MediaName { get; set; }
    }

    public class SubscriptionValidator : AbstractValidator<AddSubscriptionDTO>
    {
        public SubscriptionValidator(TrackedMediaSettings trackedMediaSettings)
        {
            RuleFor(sub => trackedMediaSettings.MediaNames.Contains(sub.MediaName.ToLower()));
        }
    }
}