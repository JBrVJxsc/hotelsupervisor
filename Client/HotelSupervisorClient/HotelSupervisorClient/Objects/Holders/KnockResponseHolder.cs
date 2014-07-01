using HotelSupervisorClient.Managers;

namespace HotelSupervisorClient.Objects.Holders
{
    public class KnockResponseHolder : BaseResponseHolder
    {
        protected override string GetBackUpFileFullName()
        {
            return EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandResponseKnockBackUpFileName);
        }

        protected override string GetSaveToFileErrorCode()
        {
            return "40。";
        }

        protected override string GetInitErrorCode()
        {
            return "41。";
        }
    }
}
