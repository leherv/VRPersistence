using System.Collections.Generic;

namespace VRPersistence.DTO
{
    public class DeleteSubscriptionsDTO
    {
        public string NotificationEndpointIdentifier { get; set; }
        public List<DeleteSubscriptionDTO> Subscriptions { get; set; }
    }
}