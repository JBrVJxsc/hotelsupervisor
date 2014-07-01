using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Managers;
using HotelSupervisorClient.Objects.Commands;

namespace HotelSupervisorClient.Objects.Holders
{
    [XmlInclude(typeof(KnockResponseHolder))]
    [XmlInclude(typeof(UpdateResponseHolder))]
    public class BaseResponseHolder : IHolder
    {
        private List<Response> responseList = new List<Response>();
        private XmlManager xmlManager = new XmlManager();

        public List<Response> ResponseList
        {
            get
            {
                return responseList;
            }
            set
            {
                responseList = value;
            }
        }

        protected virtual string GetBackUpFileFullName()
        {
            return string.Empty;
        }

        protected virtual string GetSaveToFileErrorCode()
        {
            return string.Empty;
        }

        protected virtual string GetInitErrorCode()
        {
            return string.Empty;
        }

        public void SaveToFile()
        {
            string backUpFileFullName = GetBackUpFileFullName();
            if (responseList.Count == 0)
            {
                return;
            }
            StreamWriter streamWriter = null;
            try
            {
                if (File.Exists(backUpFileFullName))
                {
                    File.Delete(backUpFileFullName);
                }
                string str = xmlManager.SerializeResponseHolder(this);
                str = EncryptionManager.Encrypt(str);
                streamWriter = new StreamWriter(backUpFileFullName);
                streamWriter.Write(str);
                streamWriter.Close();
                File.SetAttributes(backUpFileFullName, FileAttributes.Hidden);
            }
            catch (Exception e)
            {
                throw new Exception(GetSaveToFileErrorCode() + e.Message);
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }

        public void Init()
        {
            string backUpFileFullName = GetBackUpFileFullName();
            StreamReader streamReader = null;
            try
            {
                if (!File.Exists(backUpFileFullName))
                {
                    return;
                }
                streamReader = new StreamReader(backUpFileFullName);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                str = EncryptionManager.Decrypt(str);
                BaseResponseHolder baseResponseHolder = xmlManager.DeserializeResponseHolder(str);
                ResponseList = baseResponseHolder.ResponseList;
                File.Delete(backUpFileFullName);
            }
            catch (Exception e)
            {
                throw new Exception(GetInitErrorCode() + e.Message);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
        }
    }
}
