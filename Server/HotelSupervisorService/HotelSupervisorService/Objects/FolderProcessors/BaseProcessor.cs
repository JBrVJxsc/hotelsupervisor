using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Communication;

namespace HotelSupervisorService.Objects.FolderProcessors
{
    public class BaseProcessor
    {
        protected CommunicationManager communicationManager = new CommunicationManager();
        protected CommunicationParameter communicationParameter = new CommunicationParameter(EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessageUserName), EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessagePassword));
    }
}
