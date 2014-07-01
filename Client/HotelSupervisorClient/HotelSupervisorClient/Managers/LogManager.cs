using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.Logging;

namespace HotelSupervisorClient.Managers
{
    internal sealed class LogManager
    {
        #region 私有变量。

        public static readonly LogManager GlobalLogManager = new LogManager();
        private string fileName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.LogFileName);
        private string fileLocation = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.LogFileLocation);
        private bool fileAppend = true;
        private static bool loggingOn = true;
        private StreamWriter MyWriter = null;

        #endregion

        #region 公有方法。

        /// <summary>
        /// 创建一条记录。
        /// </summary>
        /// <param name="strMsg">记录的内容。</param>
        public void CreateLog(string strMsg)
        {
            if (loggingOn)
            {
                lock (this)
                {
                    GetWriter();
                    try
                    {
                        MyWriter.WriteLine(DateTime.Now + " " + strMsg);
                    }
                    finally
                    {
                        MyWriter.Close();
                    }
                }
            }
        }

        #endregion

        #region 私有方法。

        /// <summary>
        /// 获得输入流。
        /// </summary>
        private void GetWriter()
        {
            lock (this)
            {
                CloseFile();
                MyWriter = new StreamWriter(fileLocation + "/" + fileName, fileAppend);
            }
        }

        /// <summary>
        /// 关闭输入流。
        /// </summary>
        private void CloseFile()
        {
            if (MyWriter != null)
            {
                try
                {
                    MyWriter.Close();
                }
                catch
                {

                }
            }
        }

        #endregion
    }
}
