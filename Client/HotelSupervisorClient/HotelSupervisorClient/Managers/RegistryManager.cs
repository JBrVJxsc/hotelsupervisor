using System;
using System.Reflection;
using System.Runtime.InteropServices;
using HotelSupervisorClient.Exceptions;
using HotelSupervisorClient.Objects;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace HotelSupervisorClient.Managers
{
    internal class RegistryManager
    {
        private string _subkey;
        private RegDomain _domain;

        #region 构造函数

        public RegistryManager()
        {
            ///默认注册表项名称
            _subkey = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegBaseLocation);
            ///默认注册表基项域
            _domain = RegDomain.LocalMachine;
        }

        #endregion

        #region 公有方法

        #region FileSystemWatcherPlus

        /// <summary>
        /// 获得被监控数据库文件的路径。
        /// </summary>
        /// <returns>路径。</returns>
        public string GetDataBaseFilePath()
        {
            string path = string.Empty;
            if (IsSubKeyExist())
            {
                path = EncryptionManager.NormalDecryptOne(EncryptionManager.ReverseString(ReadRegeditKey(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegPath)).ToString()));
                path += EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseFileFolder);
            }
            else
            {
                //64位操作系统。
                if (IntPtr.Size == 8)
                {
                    try
                    {
                        path = EncryptionManager.NormalDecryptOne(EncryptionManager.ReverseString(Get64BitRegistryKeyValueFromLocalMachine(_subkey, EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegPath))));
                    }
                    catch(Exception e)
                    {
                        if (e.GetType() == typeof(RegeditKeyNotFoundException))
                        {
                            throw new Exception("10。");
                        }
                        else if (e.GetType() == typeof(SubKeyNotFoundException))
                        {
                            throw new Exception("19。");
                        }
                    }
                    path += EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseFileFolder);
                }
                else
                {
                    throw new Exception("10。");
                }
            }
            return path;
        }

        public void GetHotelInfo(ref string id,ref string name,ref string location,ref string tel)
        {
            if (IsSubKeyExist())
            {
                name = EncryptionManager.ReverseString(ReadRegeditKey(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelName)).ToString());
                location = EncryptionManager.ReverseString(ReadRegeditKey(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelLocation)).ToString());
                tel = EncryptionManager.NormalDecryptTwo(ReadRegeditKey(EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelTel)).ToString());
                tel = EncryptionManager.ReverseString(tel);
            }
            else
            {
                //64位操作系统。
                if (IntPtr.Size == 8)
                {
                    try
                    {
                        name = EncryptionManager.ReverseString(Get64BitRegistryKeyValueFromLocalMachine(_subkey, EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelName)));
                        location = EncryptionManager.ReverseString(Get64BitRegistryKeyValueFromLocalMachine(_subkey, EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelLocation)));
                        tel = EncryptionManager.NormalDecryptTwo(Get64BitRegistryKeyValueFromLocalMachine(_subkey, EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelTel)));
                        tel = EncryptionManager.ReverseString(tel);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(RegeditKeyNotFoundException))
                        {
                            throw new Exception("11。");
                        }
                        else if (e.GetType() == typeof(SubKeyNotFoundException))
                        {
                            throw new Exception("20。");
                        }
                    }
                }
                else
                {
                    throw new Exception("11。");
                }
            }
            string regeditKey = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegHotelID);
            if (IsRegeditKeyExist(regeditKey))
            {
                id = EncryptionManager.Decrypt(ReadRegeditKey(regeditKey).ToString());
            }
            else
            {
                id  = GuidManager.GetNewGuid();
                WriteRegeditKey(regeditKey, EncryptionManager.Encrypt(id));
                //第一次注册，则发送注册信息。
                CommunicationManager communicationManager = new CommunicationManager();
                HotelInfo hotelInfo = new HotelInfo();
                hotelInfo.ID = id;
                hotelInfo.Name = name;
                hotelInfo.Location = location;
                hotelInfo.Tel = tel;
                communicationManager.SendRegisterMessage(hotelInfo);
            }
        }

        /// <summary>
        /// 获得最近一次发送的旅客序列号。
        /// </summary>
        /// <returns>旅客序列号。</returns>
        public string GetLastGuestNumber()
        {
            if (IsSubKeyExist())
            {
                string regeditKey = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegLastGuestNumber);
                if (IsRegeditKeyExist(regeditKey))
                {
                    return EncryptionManager.Decrypt(ReadRegeditKey(regeditKey).ToString());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("16。");
            }
        }

        /// <summary>
        /// 设置最近一次发送的旅客序列号。
        /// </summary>
        /// <param name="guestNumber">旅客序列号。</param>
        public void SetLastGuestNumber(string guestNumber)
        {
            if (IsSubKeyExist())
            {
                string regeditKey = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegLastGuestNumber);
                WriteRegeditKey(regeditKey, EncryptionManager.Encrypt(guestNumber));
            }
            else
            {
                throw new Exception("17。");
            }
        }

        public string GetLastCommandCheckPoint(string regeditKey)
        {
            if (IsSubKeyExist())
            {
                if (IsRegeditKeyExist(regeditKey))
                {
                    return EncryptionManager.Decrypt(ReadRegeditKey(regeditKey).ToString());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("29。");
            }
        }

        public void SetLastCommandCheckPoint(string regeditKey, string value)
        {
            if (IsSubKeyExist())
            {
                WriteRegeditKey(regeditKey, EncryptionManager.Encrypt(value));
            }
            else
            {
                throw new Exception("30。");
            }
        }

        #endregion

        #region 创建注册表项

        /// <summary>
        /// 创建注册表项，默认创建在注册表基项 HKEY_LOCAL_MACHINE下面
        /// 虚方法，子类可进行重写
        /// 例子：如subkey是software\\higame\\，则将创建HKEY_LOCAL_MACHINE\\software\\higame\\注册表项
        /// </summary>
        /// <param name="subKey">注册表项名称</param>
        public virtual void CreateSubKey(string subKey)
        {
            ///判断注册表项名称是否为空，如果为空，返回false
            if (subKey == string.Empty || subKey == null)
            {
                return;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegDomain(_domain);

            ///要创建的注册表项的节点
            RegistryKey sKey;
            if (!IsSubKeyExist(subKey))
            {
                sKey = key.CreateSubKey(subKey);
            }
            //sKey.Close();
            ///关闭对注册表项的更改
            key.Close();
        }

        #endregion

        #region 判断注册表项是否存在

        /// <summary>
        /// 判断注册表项是否存在，默认是在注册表基项HKEY_LOCAL_MACHINE下判断（请先设置SubKey属性）
        /// 虚方法，子类可进行重写
        /// 例子：如果设置了Domain和SubKey属性，则判断Domain\\SubKey，否则默认判断HKEY_LOCAL_MACHINE\\software\\
        /// </summary>
        /// <returns>返回注册表项是否存在，存在返回true，否则返回false</returns>
        public virtual bool IsSubKeyExist()
        {
            ///判断注册表项名称是否为空，如果为空，返回false
            if (_subkey == string.Empty || _subkey == null)
            {
                return false;
            }

            ///检索注册表子项
            ///如果sKey为null,说明没有该注册表项不存在，否则存在
            RegistryKey sKey = OpenSubKey(_subkey, _domain);
            if (sKey == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断注册表项是否存在，默认是在注册表基项HKEY_LOCAL_MACHINE下判断
        /// 虚方法，子类可进行重写
        /// 例子：如subkey是software\\higame\\，则将判断HKEY_LOCAL_MACHINE\\software\\higame\\注册表项是否存在
        /// </summary>
        /// <param name="subKey">注册表项名称</param>
        /// <returns>返回注册表项是否存在，存在返回true，否则返回false</returns>
        public virtual bool IsSubKeyExist(string subKey)
        {
            ///判断注册表项名称是否为空，如果为空，返回false
            if (subKey == string.Empty || subKey == null)
            {
                return false;
            }

            ///检索注册表子项
            ///如果sKey为null,说明没有该注册表项不存在，否则存在
            RegistryKey sKey = OpenSubKey(subKey);
            if (sKey == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 判断键值是否存在

        /// <summary>
        /// 判断键值是否存在（请先设置SubKey属性）
        /// 虚方法，子类可进行重写
        /// 如果SubKey为空、null或者SubKey指定的注册表项不存在，返回false
        /// </summary>
        /// <param name="name">键值名称</param>
        /// <returns>返回键值是否存在，存在返回true，否则返回false</returns>
        public virtual bool IsRegeditKeyExist(string name)
        {
            ///返回结果
            bool result = false;

            ///判断是否设置键值属性
            if (name == string.Empty || name == null)
            {
                return false;
            }

            ///判断注册表项是否存在
            if (IsSubKeyExist())
            {
                ///打开注册表项
                RegistryKey key = OpenSubKey();
                ///键值集合
                string[] regeditKeyNames;
                ///获取键值集合
                regeditKeyNames = key.GetValueNames();
                ///遍历键值集合，如果存在键值，则退出遍历
                foreach (string regeditKey in regeditKeyNames)
                {
                    if (string.Compare(regeditKey, name, true) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                ///关闭对注册表项的更改
                key.Close();
            }
            return result;
        }

        #endregion

        #region 设置键值内容

        /// <summary>
        /// 设置指定的键值内容，不指定内容数据类型（请先设置SubKey属性）
        /// 存在改键值则修改键值内容，不存在键值则先创建键值，再设置键值内容
        /// </summary>
        /// <param name="name">键值名称</param>
        /// <param name="content">键值内容</param>
        /// <returns>键值内容设置成功，则返回true，否则返回false</returns>
        public virtual bool WriteRegeditKey(string name, object content)
        {
            ///返回结果
            bool result = false;

            ///判断键值是否存在
            if (name == string.Empty || name == null)
            {
                return false;
            }

            ///判断注册表项是否存在，如果不存在，则直接创建
            if (!IsSubKeyExist(_subkey))
            {
                CreateSubKey(_subkey);
            }

            ///以可写方式打开注册表项
            RegistryKey key = OpenSubKey(true);

            ///如果注册表项打开失败，则返回false
            if (key == null)
            {
                return false;
            }

            try
            {
                key.SetValue(name, content);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                ///关闭对注册表项的更改
                key.Close();
            }
            return result;
        }

        #endregion

        #region 读取键值内容

        /// <summary>
        /// 读取键值内容（请先设置SubKey属性）
        /// 1.如果SubKey为空、null或者SubKey指示的注册表项不存在，返回null
        /// 2.反之，则返回键值内容
        /// </summary>
        /// <param name="name">键值名称</param>
        /// <returns>返回键值内容</returns>
        public virtual object ReadRegeditKey(string name)
        {
            ///键值内容结果
            object obj = null;

            ///判断是否设置键值属性
            if (name == string.Empty || name == null)
            {
                return null;
            }

            ///判断键值是否存在
            if (IsRegeditKeyExist(name))
            {
                ///打开注册表项
                RegistryKey key = OpenSubKey();
                if (key != null)
                {
                    obj = key.GetValue(name);
                }
                ///关闭对注册表项的更改
                key.Close();
            }
            return obj;
        }

        #endregion

        #region 32位程序读取64位注册表。

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegOpenKeyEx")]
        static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int sam,
        out IntPtr phkResult);

        [Flags]
        public enum eRegWow64Options : int
        {
            None = 0x0000,
            KEY_WOW64_64KEY = 0x0100,
            KEY_WOW64_32KEY = 0x0200
        }

        [Flags]
        public enum eRegistryRights : int
        {
            ReadKey = 131097,
            WriteKey = 131078,
        }

        public string Get64BitRegistryKeyValueFromLocalMachine(string subKey, string valueName)
        {
            RegistryKey key;
            try
            {
                  key = OpenSubKey(Registry.LocalMachine, subKey, false,
                  eRegWow64Options.KEY_WOW64_32KEY);
            }
            catch
            {
                throw new SubKeyNotFoundException();
            }
            try
            {
                return key.GetValue(valueName).ToString();
            }
            catch
            {
                throw new RegeditKeyNotFoundException();
            }
        }

        public RegistryKey OpenSubKey(RegistryKey pParentKey, string pSubKeyName,
        bool pWriteable,
       eRegWow64Options pOptions)
        {
            if (pParentKey == null || GetRegistryKeyHandle(pParentKey).Equals(System.IntPtr.Zero))
                throw new System.Exception("OpenSubKey: Parent key is not open");

            eRegistryRights Rights = eRegistryRights.ReadKey;
            if (pWriteable)
                Rights = eRegistryRights.WriteKey;

            System.IntPtr SubKeyHandle;
            System.Int32 Result = RegOpenKeyEx(GetRegistryKeyHandle(pParentKey), pSubKeyName, 0,
            (int)Rights | (int)pOptions, out SubKeyHandle);
            if (Result != 0)
            {
                System.ComponentModel.Win32Exception W32ex =
                new System.ComponentModel.Win32Exception();
                throw new System.Exception("OpenSubKey: Exception encountered opening key",
                W32ex);
            }

            return PointerToRegistryKey(SubKeyHandle, pWriteable, false);
        }

        private System.IntPtr GetRegistryKeyHandle(RegistryKey pRegisteryKey)
        {
            Type Type = Type.GetType("Microsoft.Win32.RegistryKey");
            FieldInfo Info = Type.GetField("hkey", BindingFlags.NonPublic | BindingFlags.Instance);

            SafeHandle Handle = (SafeHandle)Info.GetValue(pRegisteryKey);
            IntPtr RealHandle = Handle.DangerousGetHandle();

            return Handle.DangerousGetHandle();
        }

        private RegistryKey PointerToRegistryKey(IntPtr hKey, bool pWritable,
        bool pOwnsHandle)
        {
            // Create a SafeHandles.SafeRegistryHandle from this pointer - this is a private class 
            BindingFlags privateConstructors = BindingFlags.Instance | BindingFlags.NonPublic;
            Type safeRegistryHandleType = typeof(
            SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType(
            "Microsoft.Win32.SafeHandles.SafeRegistryHandle");

            Type[] safeRegistryHandleConstructorTypes = new Type[] { typeof(System.IntPtr), 
 typeof(System.Boolean) };
            ConstructorInfo safeRegistryHandleConstructor =
            safeRegistryHandleType.GetConstructor(privateConstructors,
            null, safeRegistryHandleConstructorTypes, null);
            Object safeHandle = safeRegistryHandleConstructor.Invoke(new Object[] { hKey, 
 pOwnsHandle });

            // Create a new Registry key using the private constructor using the 
            // safeHandle - this should then behave like 
            // a .NET natively opened handle and disposed of correctly 
            Type registryKeyType = typeof(Microsoft.Win32.RegistryKey);
            Type[] registryKeyConstructorTypes = new Type[] { safeRegistryHandleType, 
 typeof(Boolean) };
            ConstructorInfo registryKeyConstructor =
            registryKeyType.GetConstructor(privateConstructors, null,
            registryKeyConstructorTypes, null);
            RegistryKey result = (RegistryKey)registryKeyConstructor.Invoke(new Object[] { 
 safeHandle, pWritable });
            return result;
        }

        #endregion

        #endregion

        #region 受保护方法

        /// <summary>
        /// 获取注册表基项域对应顶级节点
        /// 例子：如regDomain是ClassesRoot，则返回Registry.ClassesRoot
        /// </summary>
        /// <param name="regDomain">注册表基项域</param>
        /// <returns>注册表基项域对应顶级节点</returns>
        protected RegistryKey GetRegDomain(RegDomain regDomain)
        {
            ///创建基于注册表基项的节点
            RegistryKey key;

            #region 判断注册表基项域
            switch (regDomain)
            {
                case RegDomain.ClassesRoot:
                    key = Registry.ClassesRoot; break;
                case RegDomain.CurrentUser:
                    key = Registry.CurrentUser; break;
                case RegDomain.LocalMachine:
                    key = Registry.LocalMachine; break;
                case RegDomain.User:
                    key = Registry.Users; break;
                case RegDomain.CurrentConfig:
                    key = Registry.CurrentConfig; break;
                case RegDomain.DynDa:
                    key = Registry.DynData; break;
                case RegDomain.PerformanceData:
                    key = Registry.PerformanceData; break;
                default:
                    key = Registry.LocalMachine; break;
            }
            #endregion

            return key;
        }

        #region 打开注册表项

        /// <summary>
        /// 打开注册表项节点，以只读方式检索子项
        /// 虚方法，子类可进行重写
        /// </summary>
        /// <returns>如果SubKey为空、null或者SubKey指示注册表项不存在，则返回null，否则返回注册表节点</returns>
        protected virtual RegistryKey OpenSubKey()
        {
            ///判断注册表项名称是否为空
            if (_subkey == string.Empty || _subkey == null)
            {
                return null;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegDomain(_domain);

            ///要打开的注册表项的节点
            RegistryKey sKey = null;
            ///打开注册表项
            sKey = key.OpenSubKey(_subkey);
            ///关闭对注册表项的更改
            key.Close();
            ///返回注册表节点
            return sKey;
        }

        /// <summary>
        /// 打开注册表项节点
        /// 虚方法，子类可进行重写
        /// </summary>
        /// <param name="writable">如果需要项的写访问权限，则设置为 true</param>
        /// <returns>如果SubKey为空、null或者SubKey指示注册表项不存在，则返回null，否则返回注册表节点</returns>
        protected virtual RegistryKey OpenSubKey(bool writable)
        {
            ///判断注册表项名称是否为空
            if (_subkey == string.Empty || _subkey == null)
            {
                return null;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegDomain(_domain);

            ///要打开的注册表项的节点
            RegistryKey sKey = null;
            ///打开注册表项
            sKey = key.OpenSubKey(_subkey, writable);
            ///关闭对注册表项的更改
            key.Close();
            ///返回注册表节点
            return sKey;
        }

        /// <summary>
        /// 打开注册表项节点，以只读方式检索子项
        /// 虚方法，子类可进行重写
        /// </summary>
        /// <param name="subKey">注册表项名称</param>
        /// <returns>如果SubKey为空、null或者SubKey指示注册表项不存在，则返回null，否则返回注册表节点</returns>
        protected virtual RegistryKey OpenSubKey(string subKey)
        {
            ///判断注册表项名称是否为空
            if (subKey == string.Empty || subKey == null)
            {
                return null;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegDomain(_domain);

            ///要打开的注册表项的节点
            RegistryKey sKey = null;
            ///打开注册表项
            sKey = key.OpenSubKey(subKey);
            ///关闭对注册表项的更改
            key.Close();
            ///返回注册表节点
            return sKey;
        }

        /// <summary>
        /// 打开注册表项节点，以只读方式检索子项
        /// 虚方法，子类可进行重写
        /// </summary>
        /// <param name="subKey">注册表项名称</param>
        /// <param name="regDomain">注册表基项域</param>
        /// <returns>如果SubKey为空、null或者SubKey指示注册表项不存在，则返回null，否则返回注册表节点</returns>
        protected virtual RegistryKey OpenSubKey(string subKey, RegDomain regDomain)
        {
            ///判断注册表项名称是否为空
            if (subKey == string.Empty || subKey == null)
            {
                return null;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegDomain(regDomain);

            ///要打开的注册表项的节点
            RegistryKey sKey = null;
            ///打开注册表项
            sKey = key.OpenSubKey(subKey);
            ///关闭对注册表项的更改
            key.Close();
            ///返回注册表节点
            return sKey;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 注册表基项静态域
    /// 
    /// 主要包括：
    /// 1.Registry.ClassesRoot     对应于HKEY_CLASSES_ROOT主键
    /// 2.Registry.CurrentUser     对应于HKEY_CURRENT_USER主键
    /// 3.Registry.LocalMachine    对应于 HKEY_LOCAL_MACHINE主键
    /// 4.Registry.User            对应于 HKEY_USER主键
    /// 5.Registry.CurrentConfig   对应于HEKY_CURRENT_CONFIG主键
    /// 6.Registry.DynDa           对应于HKEY_DYN_DATA主键
    /// 7.Registry.PerformanceData 对应于HKEY_PERFORMANCE_DATA主键
    /// 
    /// 版本:1.0
    /// </summary>
    public enum RegDomain
    {
        ClassesRoot = 0,
        CurrentUser = 1,
        LocalMachine = 2,
        User = 3,
        CurrentConfig = 4,
        DynDa = 5,
        PerformanceData = 6,
    }

    public enum RegValueKind
    {
        Unknown = 0,
        String = 1,
        ExpandString = 2,
        Binary = 3,
        DWord = 4,
        MultiString = 5,
        QWord = 6,
    }
}
