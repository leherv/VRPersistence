using VRPersistence.DTO;

namespace VRPersistence.BO
{
    public class Subscription
    {
        public Media Media { get; set; }
        public NotificationEndpoint NotificationEndpoint { get; set; }

        public Subscription(AddSubscriptionDTO addSubscriptionDto, string endpointIdentifier)
        {
            Media = new Media(addSubscriptionDto.MediaName);
            NotificationEndpoint = new NotificationEndpoint(endpointIdentifier);
        }
    }
}