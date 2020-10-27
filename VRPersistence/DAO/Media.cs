namespace VRPersistence.DAO
{
    public class Media
    {
        public long Id { get; set; }
        public string MediaName { get; set; }
        public string Description { get; set; }

        public Media(BO.Media media)
        {
            MediaName = media.MediaName;
            Description = media.Description;
        }
        
        public Media() {}
    }
}