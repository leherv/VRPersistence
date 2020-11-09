using VRPersistence.DTO.request;

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