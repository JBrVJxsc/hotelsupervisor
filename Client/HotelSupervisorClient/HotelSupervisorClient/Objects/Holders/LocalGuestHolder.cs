using System;
using System.Collections.Generic;
using System.IO;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Managers;

namespace HotelSupervisorClient.Objects.Holders
{
    public class LocalGuestHolder : IHolder
    {
        private List<LocalGuest> localGuestList = new List<LocalGuest>();
        private string backUpFileFullName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.LocalGuestBackUpFileFullName);
        private XmlManager xmlManager = new XmlManager();

        public List<LocalGuest> LocalGuestList
        {
            get
            {
                return localGuestList;
            }
            set
            {
                localGuestList = value;
            }
        }

        public void SaveToFile()
        {
            if (localGuestList.Count == 0)
            {
                return;
            }
            StreamWriter streamWriter=null;
            try
            {
                if (File.Exists(backUpFileFullName))
                {
                    File.Delete(backUpFileFullName);
                }
                string str = xmlManager.SerializeLocalGuestHolder(this);
                str = EncryptionManager.Encrypt(str);
                streamWriter = new StreamWriter(backUpFileFullName);
                streamWriter.Write(str);
                streamWriter.Close();
                File.SetAttributes(backUpFileFullName, FileAttributes.Hidden);
            }
            catch (Exception e)
            {
                throw new Exception("39。" + e.Message);
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
                LocalGuestHolder localGuestHolder = xmlManager.DeserializeLocalGuestHolder(str);
                LocalGuestList = localGuestHolder.LocalGuestList;
                File.Delete(backUpFileFullName);
            }
            catch (Exception e)
            {
                throw new Exception("37。" + e.Message);
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
