﻿using System.Collections.Generic;

namespace VRPersistence.DTO.request
{
    public class AddSubscriptionsDTO
    {
        public string NotificationEndpointIdentifier { get; set; }
        public List<AddSubscriptionDTO> Subscriptions { get; set; }
    }
}