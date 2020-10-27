using VRPersistence.DTO;

namespace VRPersistence.BO
{
    public class Release
    {
        public Media Media { get; set; } 
        public int ReleaseNumber { get; set; }
        public string Url { get; set; }
        public bool Notified { get; set; }

        public Release(AddReleaseDTO addReleaseDto)
        {
            Media = new Media {MediaName = addReleaseDto.MediaName.ToLower()};
            ReleaseNumber = addReleaseDto.ReleaseNumber;
            Url = addReleaseDto.Url;
            Notified = false;
        }

        public Release(DAO.Release release)
        {
            Media = new Media {MediaName = release.Media.MediaName};
            ReleaseNumber = release.ReleaseNumber;
            Url = release.Url;
            Notified = release.Notified;
        }
    }
}