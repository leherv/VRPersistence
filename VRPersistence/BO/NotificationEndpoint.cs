﻿namespace VRPersistence.BO
{
    public class NotificationEndpoint
    {
        public string Identifier { get; set; }

        public NotificationEndpoint(string identifier)
        {
            Identifier = identifier;
        }

        public NotificationEndpoint(DAO.NotificationEndpoint notificationEndpoint)
        {
            Identifier = notificationEndpoint.Identifier;
        }
        
    }
}