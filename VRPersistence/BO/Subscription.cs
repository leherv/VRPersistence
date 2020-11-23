using VRPersistence.DTO.request;

namespace VRPersistence.BO
{
    public class Subscription
    {
        public Media Media { get; set; }
        public NotificationEndpoint NotificationEndpoint { get; set; }

        public Subscription(AddSubscriptionDTO addSubscriptionDto, string endpointIdentifier)
        {
            Media = new Media {MediaName = addSubscriptionDto.MediaName.ToLower()};
            NotificationEndpoint = new NotificationEndpoint(endpointIdentifier);
        }
    }
}