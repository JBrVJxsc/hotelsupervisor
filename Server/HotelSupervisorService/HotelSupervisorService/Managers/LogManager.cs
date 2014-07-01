using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using HotelSupervisorService.Interfaces;
using Microsoft.VisualBasic.Logging;

namespace HotelSupervisorService.Managers
{
    public sealed class LogManager
    {
        #region 私有变量。

        private static readonly LogManager instance = new LogManager();
        private string fileName = EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.LogFileName);
        private string fileLocation = Application.StartupPath;
        private bool fileAppend = true;
        private static bool logType = false;
        private static bool loggingOn = true;
        private string nameSpace;
        private string methodName;
        private string className;
        private string methodArgs;
        private StreamWriter Writer = null;

        #endregion

        #region 事件。

        public event AddLogHandle OnAddLog;
        public event DeleteLogHandle OnDeleteLog;
        public event ClearLogHandle OnClearLog;
        public event SaveLogHandle OnSaveLog;
        public event ShowLogHandle OnShowLog;

        #endregion

        #region 公有属性。

        /// <summary>
        /// 文件名。
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// 文件地址。
        /// </summary>
        public string FileLocation
        {
            get
            {
                return fileLocation;
            }
            set
            {
                fileLocation = value;
            }
        }

        /// <summary>
        /// 是否持续添加。
        /// </summary>
        public bool FileAppend
        {
            get
            {
                return fileAppend;
            }
            set
            {
                fileAppend = value;
            }
        }
        /// <summary>
        /// 是否处于记录日志状态。
        /// </summary>
        public static bool LoggingOn
        {
            get
            {
                return loggingOn;
            }
            set
            {
                loggingOn = value;
            }
        }

        /// <summary>
        /// 记录日志的方式。设置为True时，使用FileLogTraceListener；设置为False时，使用StreamWriter。
        /// </summary>
        public static bool LogType
        {
            get
            {
                return logType;
            }
            set
            {
                logType = value;
            }
        }

        #endregion

        #region 私有属性。

        /// <summary>
        /// 命名空间。
        /// </summary>
        private string NameSpace
        {
            get
            {
                InitializeName();
                return nameSpace;
            }
            set
            {
                nameSpace = value;
            }
        }

        /// <summary>
        /// 方法名称。
        /// </summary>
        private string MethodName
        {
            get
            {
                InitializeName();
                return methodName;
            }
            set
            {
                methodName = value;
            }
        }

        /// <summary>
        /// 方法参数。
        /// </summary>
        private string MethodArguments
        {
            get
            {
                InitializeName();
                return methodArgs;
            }
            set
            {
                methodArgs = value;
            }
        }

        /// <summary>
        /// 类名。
        /// </summary>
        private string ClassName
        {
            get
            {
                InitializeName();
                return className;
            }
            set
            {
                className = value;
            }
        }

        /// <summary>
        /// 输入流。
        /// </summary>
        private StreamWriter MyWriter
        {
            get
            {
                return Writer;
            }
            set
            {
                Writer = value;
            }
        }
        #endregion

        #region 公有静态方法。

        public static LogManager GetLogManager()
        {
            return instance;
        }

        #endregion

        #region 公有方法。

        /// <summary>
        /// 开始记录。
        /// </summary>
        public void StartLog()
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " 记录开始。");
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry("Application", ex.Message);
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " 记录开始。");
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在开始记录时执行。
        /// </summary>
        /// <param name="strMsg">与记录一起显示的内容。</param>
        public void StartLog(string strMsg)
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " 记录开始，为" + strMsg + " 创建。");
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " 记录开始，为" + strMsg + " 创建。");
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建一条记录。
        /// </summary>
        /// <param name="strMsg">记录的内容。</param>
        public void CreateLog(string strMsg)
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = new FileLogTraceListener();
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " " + strMsg);
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
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
        }

        /// <summary>
        /// 为异常创建一条记录，通常在Catch段中使用。
        /// </summary>
        /// <param name="ex">异常实体。</param>
        public void CreateLog(Exception ex)
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = new FileLogTraceListener();
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " 异常记录开始。");
                        logInfo.WriteLine("命名空间： " + NameSpace);
                        logInfo.WriteLine("类名： " + ClassName);
                        logInfo.WriteLine("方法名： " + MethodName);
                        logInfo.WriteLine("方法参数： " + MethodArguments);
                        logInfo.WriteLine("源： " + ex.Source);
                        logInfo.WriteLine("类型： " + ex.GetType().FullName);
                        logInfo.WriteLine("消息： " + ex.Message);
                        logInfo.WriteLine("目标站点： " + ex.TargetSite);
                        logInfo.WriteLine("堆栈跟踪： " + ex.StackTrace);
                        if (ex.InnerException != null)
                        {
                            logInfo.WriteLine("内部异常： " + ex.InnerException);
                        }
                        logInfo.WriteLine("异常记录结束。");
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " 异常记录开始。");
                            MyWriter.WriteLine("命名空间： " + NameSpace);
                            MyWriter.WriteLine("类名： " + ClassName);
                            MyWriter.WriteLine("方法名： " + MethodName);
                            MyWriter.WriteLine("方法参数： " + MethodArguments);
                            MyWriter.WriteLine("源： " + ex.Source);
                            MyWriter.WriteLine("类型： " + ex.GetType().FullName);
                            MyWriter.WriteLine("消息： " + ex.Message);
                            MyWriter.WriteLine("目标站点： " + ex.TargetSite);
                            MyWriter.WriteLine("堆栈跟踪： " + ex.StackTrace);
                            if (ex.InnerException != null)
                            {
                                MyWriter.WriteLine("内部异常： " + ex.InnerException);
                            }
                            MyWriter.WriteLine("异常记录结束。");
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 为异常创建一条记录，通常在Catch段中使用。
        /// </summary>
        /// <param name="ex">异常实体。</param>
        /// <param name="strExceptionMsg">异常名称或者用户自定义内容。</param>
        public void CreateLog(Exception ex, string strExceptionMsg)
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = new FileLogTraceListener();
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " 异常记录开始。");
                        logInfo.WriteLine(strExceptionMsg);
                        logInfo.WriteLine("命名空间： " + NameSpace);
                        logInfo.WriteLine("类名： " + ClassName);
                        logInfo.WriteLine("方法名： " + MethodName);
                        logInfo.WriteLine("方法参数： " + MethodArguments);
                        logInfo.WriteLine("源： " + ex.Source);
                        logInfo.WriteLine("类型： " + ex.GetType().FullName);
                        logInfo.WriteLine("消息： " + ex.Message);
                        logInfo.WriteLine("目标站点： " + ex.TargetSite);
                        logInfo.WriteLine("堆栈跟踪： " + ex.StackTrace);
                        if (ex.InnerException != null)
                        {
                            logInfo.WriteLine("内部异常： " + ex.InnerException);
                        }
                        logInfo.WriteLine("异常记录结束。");
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " 异常记录开始。");
                            MyWriter.WriteLine(strExceptionMsg);
                            MyWriter.WriteLine("命名空间： " + NameSpace);
                            MyWriter.WriteLine("类名： " + ClassName);
                            MyWriter.WriteLine("方法名： " + MethodName);
                            MyWriter.WriteLine("方法参数： " + MethodArguments);
                            MyWriter.WriteLine("源： " + ex.Source);
                            MyWriter.WriteLine("类型： " + ex.GetType().FullName);
                            MyWriter.WriteLine("消息： " + ex.Message);
                            MyWriter.WriteLine("目标站点： " + ex.TargetSite);
                            MyWriter.WriteLine("堆栈跟踪： " + ex.StackTrace);
                            if (ex.InnerException != null)
                            {
                                MyWriter.WriteLine("内部异常： " + ex.InnerException);
                            }
                            MyWriter.WriteLine("异常记录结束。");
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建一条用户自定义记录。
        /// </summary>
        /// <param name="strMsg">内容。</param>
        public void CreateExMsgLog(string strMsg)
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " " + strMsg);
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " " + strMsg);
                            MyWriter.Close();
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 结束记录。
        /// </summary>
        public void EndLog()
        {
            if (LoggingOn)
            {
                if (LogType)
                {
                    FileLogTraceListener logInfo = new FileLogTraceListener();
                    try
                    {
                        logInfo = new FileLogTraceListener();
                        logInfo = (FileLogTraceListener)AssignProperty();
                        logInfo.WriteLine(DateTime.Now + " 记录结束。");
                        logInfo.Close();
                    }
                    finally
                    {
                        logInfo.Close();
                    }
                }
                else
                {
                    lock (this)
                    {
                        GetWriter();
                        try
                        {
                            MyWriter.WriteLine(DateTime.Now + " 记录结束。");
                        }
                        finally
                        {
                            MyWriter.Close();
                        }
                    }
                }
            }
        }

        #region ILogShower相关。

        /// <summary>
        /// 增加Log。
        /// </summary>
        /// <param name="iLog">Log实体。</param>
        public void AddLog(ILog iLog)
        {
            if (OnAddLog != null)
            {
                OnAddLog(iLog);
            }
        }

        /// <summary>
        /// 删除Log。
        /// </summary>
        /// <param name="iLog">Log实体。</param>
        public void DeleteLog(ILog iLog)
        {
            if (OnDeleteLog != null)
            {
                OnDeleteLog(iLog);
            }
        }

        /// <summary>
        /// 清除Log。
        /// </summary>
        public void ClearLog()
        {
            if (OnClearLog != null)
            {
                OnClearLog();
            }
        }

        /// <summary>
        /// 保存Log。
        /// </summary>
        public void SaveLog()
        {
            if (OnSaveLog != null)
            {
                OnSaveLog();
            }
        }

        /// <summary>
        /// 按照数量显示Log。
        /// </summary>
        /// <param name="number">数量。</param>
        public void ShowLog(int number)
        {
            if (OnShowLog != null)
            {
                OnShowLog(number);
            }
        }

        #endregion

        #endregion

        #region 私有方法。

        /// <summary>
        /// 根据文件名称与文件地址创建FileTraceListener实体。
        /// </summary>
        /// <returns>FileTraceListener实体。</returns>
        private FileLogTraceListener AssignProperty()
        {
            try
            {
                FileLogTraceListener logInfo;
                logInfo = new FileLogTraceListener();
                logInfo.Append = FileAppend;
                logInfo.BaseFileName = FileName;
                logInfo.CustomLocation = FileLocation;
                logInfo.Location = LogFileLocation.Custom;
                logInfo.AutoFlush = true;
                return logInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得输入流。
        /// </summary>
        private void GetWriter()
        {
            lock (this)
            {
                CloseFile();
                MyWriter = new StreamWriter(FileLocation + "/" + FileName, FileAppend);
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

        /// <summary>
        /// 获取命名空间、类、方法、方法参数的名称。
        /// </summary>
        private void InitializeName()
        {
            string strParam = string.Empty;
            StackTrace objSTrace = new StackTrace(true);
            if (objSTrace.FrameCount >= 3)
            {
                StackFrame objSFrame = objSTrace.GetFrame(3);
                MethodBase objMethodBase = objSFrame.GetMethod();
                ParameterInfo[] objParamInfo = objMethodBase.GetParameters();
                foreach (ParameterInfo objParam in objParamInfo)
                {
                    if (strParam != string.Empty)
                    {
                        strParam += objParam.ParameterType.Name;
                    }
                    else
                    {
                        strParam += " , " + objParam.ParameterType.Name;
                    }
                    strParam += " , " + objParam.Name;
                }
                NameSpace = objMethodBase.ReflectedType.Namespace;
                ClassName = objMethodBase.ReflectedType.Name;
                MethodName = objMethodBase.Name;
                MethodArguments = strParam == string.Empty ? "无参数" : strParam;
            }
        }

        #endregion
    }

    public delegate void AddLogHandle(ILog iLog);
    public delegate void DeleteLogHandle(ILog iLog);
    public delegate void ClearLogHandle();
    public delegate void SaveLogHandle();
    public delegate void ShowLogHandle(int number);
}
