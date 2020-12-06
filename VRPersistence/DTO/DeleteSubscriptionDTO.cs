using FluentValidation;
using VRPersistence.Config;

namespace VRPersistence.DTO
{
    public class DeleteSubscriptionDTO
    {
        public string MediaName { get; set; }
    }

    public class DeleteSubscriptionValidator : AbstractValidator<DeleteSubscriptionDTO>
    {
        public DeleteSubscriptionValidator(TrackedMediaSettings trackedMediaSettings)
        {
            RuleFor(sub => trackedMediaSettings.MediaNames.Contains(sub.MediaName.ToLower()));
        }
    }
}