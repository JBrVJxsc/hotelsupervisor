using System.IO;
using System.Xml.Serialization;
using HotelSupervisorClient.Objects.Holders;

namespace HotelSupervisorClient.Managers
{
    internal class XmlManager
    {
        private static XmlSerializer xmlSerializer;

        private void InitXmlManagerForLocalGuestHolder()
        {
            xmlSerializer = new XmlSerializer(typeof(LocalGuestHolder));
        }

        private void InitXmlManagerForResponseHolder()
        {
            xmlSerializer = new XmlSerializer(typeof(BaseResponseHolder));
        }

        public string SerializeLocalGuestHolder(LocalGuestHolder localGuestHolder)
        {
            InitXmlManagerForLocalGuestHolder();
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, localGuestHolder);
                return stringWriter.ToString();
            }
        }

        public LocalGuestHolder DeserializeLocalGuestHolder(string localGuestHolderXml)
        {
            InitXmlManagerForLocalGuestHolder();
            using (StringReader stringReader = new StringReader(localGuestHolderXml))
            {
                LocalGuestHolder localGuestHolder = xmlSerializer.Deserialize(stringReader) as LocalGuestHolder;
                return localGuestHolder;
            }
        }

        public string SerializeResponseHolder(BaseResponseHolder baseResponseHolder)
        {
            InitXmlManagerForResponseHolder();
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, baseResponseHolder);
                return stringWriter.ToString();
            }
        }

        public BaseResponseHolder DeserializeResponseHolder(string baseResponseHolderXml)
        {
            InitXmlManagerForResponseHolder();
            using (StringReader stringReader = new StringReader(baseResponseHolderXml))
            {
                BaseResponseHolder baseResponseHolder = xmlSerializer.Deserialize(stringReader) as BaseResponseHolder;
                return baseResponseHolder;
            }
        }
    }
}
