using HotelSupervisorClient.Managers;

namespace HotelSupervisorClient.Objects.Holders
{
    public class UpdateResponseHolder : BaseResponseHolder
    {
        protected override string GetBackUpFileFullName()
        {
            return EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandResponseUpdateBackUpFileName);
        }

        protected override string GetSaveToFileErrorCode()
        {
            return "42。";
        }

        protected override string GetInitErrorCode()
        {
            return "43。";
        }
    }
}
