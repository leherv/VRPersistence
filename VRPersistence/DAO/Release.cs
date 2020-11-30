namespace VRPersistence.DAO
{
    public class Release
    {
        public long Id { get; set; }
        public Media Media { get; set; } 
        public int ReleaseNumber { get; set; }
        public int SubReleaseNumber { get; set; }
        public string Url { get; set; }
        public bool Notified { get; set; }

        public Release(BO.Release release)
        {
            Media = new Media(release.Media);
            ReleaseNumber = release.ReleaseNumber;
            SubReleaseNumber = release.SubReleaseNumber;
            Url = release.Url;
            Notified = release.Notified;
        }
        
        public Release() {}
    }
}