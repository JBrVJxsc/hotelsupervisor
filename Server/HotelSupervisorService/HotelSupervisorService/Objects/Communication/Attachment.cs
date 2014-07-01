using System.IO;
using LumiSoft.Net.MIME;

namespace HotelSupervisorService.Objects.Communication
{
    public class Attachment
    {
        private string text = string.Empty;
        private MIME_Entity mimeEntity;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public MIME_Entity MimeEntity
        {
            get
            {
                return mimeEntity;
            }
            set
            {
                mimeEntity = value;
            }
        }

        public bool SaveToFile(string fileFullName)
        {
            if (mimeEntity == null)
            {
                return false;
            }
            byte[] bytes = (MimeEntity.Body as MIME_b_SinglepartBase).Data;
            MemoryStream memoryStream = new MemoryStream(bytes);
            FileStream fileStream = new FileStream(fileFullName, FileMode.Create);
            memoryStream.WriteTo(fileStream);
            memoryStream.Close();
            fileStream.Close();
            memoryStream.Dispose();
            fileStream.Dispose();
            return true;
        }
    }
}
