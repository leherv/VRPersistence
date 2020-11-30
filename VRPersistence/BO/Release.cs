using VRPersistence.DTO.request;

namespace VRPersistence.BO
{
    public class Release
    {
        public Media Media { get; set; } 
        public int ReleaseNumber { get; set; }
        public int SubReleaseNumber { get; set; }
        public string Url { get; set; }
        public bool Notified { get; set; }

        public Release(AddReleaseDTO addReleaseDto)
        {
            Media = new Media {MediaName = addReleaseDto.MediaName.ToLower()};
            ReleaseNumber = addReleaseDto.ReleaseNumber;
            SubReleaseNumber = addReleaseDto.SubReleaseNumber;
            Url = addReleaseDto.Url;
            Notified = false;
        }

        public Release(DAO.Release release)
        {
            Media = new Media {MediaName = release.Media.MediaName};
            ReleaseNumber = release.ReleaseNumber;
            SubReleaseNumber = release.SubReleaseNumber;
            Url = release.Url;
            Notified = release.Notified;
        }

        public bool IsNewerThan(DAO.Release release)
        {
            if (ReleaseNumber > release.ReleaseNumber) return true;
            if (ReleaseNumber == release.ReleaseNumber)
            {
                return SubReleaseNumber > release.SubReleaseNumber;
            }

            return false;
        }
    }
}