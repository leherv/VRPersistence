namespace VRPersistence.BO
{
    public class Media
    {
        public long Id { get; set; }
        public string MediaName { get; set; }
        public string Description { get; set; }


        public Media(DAO.Media media)
        {
            Id = media.Id;
            MediaName = media.MediaName;
            Description = media.Description;
        }

        public Media(string mediaName)
        {
            MediaName = mediaName.ToLower();
        }
    }
}