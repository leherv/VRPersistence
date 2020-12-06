using VRPersistence.DTO;

namespace VRPersistence.BO
{
    public class SetNotified
    {
        public long ReleaseId { get; set; }

        public SetNotified(SetNotifiedMessageDTO setNotifiedMessageDto)
        {
            ReleaseId = setNotifiedMessageDto.ReleaseId;
        }
    }
}