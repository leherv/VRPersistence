namespace VRPersistence.DAO
{
    public class NotificationEndpoint
    {
        public long Id { get; set; }
        public string Identifier { get; set; }

        public NotificationEndpoint(BO.NotificationEndpoint nEndpoint)
        {
            Identifier = nEndpoint.Identifier;
        }
        
        public NotificationEndpoint() {}
    }
}