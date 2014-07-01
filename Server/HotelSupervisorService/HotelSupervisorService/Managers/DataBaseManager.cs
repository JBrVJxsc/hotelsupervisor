using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Objects;
using HotelSupervisorService.Objects.Command;
using HotelSupervisorService.Objects.Logs;
using HotelSupervisorService.Objects.Setting;

namespace HotelSupervisorService.Managers
{
    /// <summary>
    /// 数据库操作类。
    /// </summary>
    public class DataBaseManager
    {
        public static readonly string SQL_QUERY_SQL = "SELECT [SQL_BODY] FROM [SQL] WHERE [SQL_CODE]=?";
        public static readonly string connectionStringBase = "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=false;";
        public static OleDbConnection connectionOfCompare = null;
        public static OleDbConnection connectionOfLocal = null;
        public static DataBaseManager GlobalDataBaseManager = new DataBaseManager();

        public DataBaseManager()
        {
            try
            {
                DataBaseSetting dataBaseSetting = new DataBaseSetting();
                dataBaseSetting.InitSetting();
                string connectionString = string.Empty;
                if (connectionOfCompare == null)
                {
                    connectionString = connectionStringBase;
                    connectionString += "Data Source=" + dataBaseSetting.CompareDataBaseUrl + ";";
                    connectionString += "User ID=" + dataBaseSetting.CompareDataBaseUserID + ";";
                    connectionString += "Jet OLEDB:Database Password=" + dataBaseSetting.CompareDataBasePassword + ";";
                    connectionOfCompare = new OleDbConnection(connectionString);
                    connectionOfCompare.Open();
                }
                if (connectionOfLocal == null)
                {
                    connectionString = connectionStringBase;
                    connectionString += "Data Source=" + dataBaseSetting.LocalDataBaseUrl + ";";
                    connectionString += "User ID=" + dataBaseSetting.LocalDataBaseUserID + ";";
                    connectionString += "Jet OLEDB:Database Password=" + dataBaseSetting.LocalDataBasePassword + ";";
                    connectionOfLocal = new OleDbConnection(connectionString);
                    connectionOfLocal.Open();
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("64。", e);
            }
        }

        /// <summary>
        /// 打开数据库连接。
        /// </summary>
        public void OpenDataBase()
        {
            try
            {
                if (connectionOfCompare != null)
                {
                    if (connectionOfCompare.State != ConnectionState.Open)
                    {
                        connectionOfCompare.Open();
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("62。", e);
            }
            try
            {
                if (connectionOfLocal != null)
                {
                    if (connectionOfLocal.State != ConnectionState.Open)
                    {
                        connectionOfLocal.Open();
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("63。", e);
            }
        }

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        public void CloseDataBase()
        {
            try
            {
                if (connectionOfCompare != null)
                {
                    if (connectionOfCompare.State != ConnectionState.Closed)
                    {
                        connectionOfCompare.Close();
                    }
                }
            }
            catch(Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("49。", e);
            }
            try
            {
                if (connectionOfLocal != null)
                {
                    if (connectionOfLocal.State != ConnectionState.Closed)
                    {
                        connectionOfLocal.Close();
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("50。", e);
            }
        }

        public static string GetPassword(string fileName,string errorPassword)
        { 
            byte[] baseByte = { 0xbe, 0xec, 0x65, 0x9c, 0xfe, 0x28, 0x2b, 0x8a, 0x6c, 0x7b, 0xcd, 0xdf, 0x4f, 0x13, 0xf7, 0xb1, };
            byte flagByte = 0x0c;
            string password = "";
            try
            {
                FileStream fileStream = File.OpenRead(fileName);
                fileStream.Seek(0x14, SeekOrigin.Begin);
                byte ver = (byte)fileStream.ReadByte(); 
                fileStream.Seek(0x42, SeekOrigin.Begin);
                byte[] bytes = new byte[33];
                if (fileStream.Read(bytes, 0, 33) != 33) return "";
                byte flag = (byte)(bytes[32] ^ flagByte);
                for (int i = 0; i < 16; i++)
                {
                    byte b = (byte)(baseByte[i] ^ bytes[i * 2]);
                    if (i % 2 == 0 && ver == 1)
                    {
                        b ^= flag;
                    }
                    if (b > 0)
                    {
                        password += (char)b;
                    }
                }
            }
            catch
            {
                return errorPassword;
            }
            return password;
        } 

        /// <summary>
        /// 执行非查询语句。
        /// </summary>
        /// <param name="connection">连接。</param>
        /// <param name="commandString">命令字符串。</param>
        /// <param name="commandParameters">命令参数集。</param>
        /// <returns>非查询语言影响的行数。</returns>
        private int ExecuteNonQuery(OleDbConnection connection, string commandString, params OleDbParameter[] commandParameters)
        {
            OleDbCommand command = new OleDbCommand();
            PrepareCommand(command, connection, null, commandString, commandParameters);
            int result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result;
        }

        /// <summary>
        /// 执行非查询语句。
        /// </summary>
        /// <param name="transaction">事务。</param>
        /// <param name="commandString">命令字符串。</param>
        /// <param name="commandParameters">命令参数集。</param>
        /// <returns>非查询语言影响的行数。</returns>
        private int ExecuteNonQuery(OleDbTransaction transaction, string commandString, params OleDbParameter[] commandParameters)
        {
            OleDbCommand command = new OleDbCommand();
            PrepareCommand(command, transaction.Connection, transaction, commandString, commandParameters);
            int result = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return result;
        }

        /// <summary>
        /// 执行获得数据阅读器。
        /// </summary>
        /// <param name="connection">连接。</param>
        /// <param name="commandString">命令字符串。</param>
        /// <param name="commandParameters">命令参数集。</param>
        /// <returns>数据阅读器。</returns>
        private OleDbDataReader ExecuteReader(OleDbConnection connection, string commandString, params OleDbParameter[] commandParameters)
        {
            OleDbCommand command = new OleDbCommand();
            PrepareCommand(command, connection, null, commandString, commandParameters);
            OleDbDataReader reader = command.ExecuteReader();
            command.Parameters.Clear();
            return reader;
        }

        /// <summary>
        /// 执行查询数据集。
        /// </summary>
        /// <param name="connection">连接。</param>
        /// <param name="commandString">命令字符串。</param>
        /// <param name="commandParameters">命令参数集。</param>
        /// <returns>数据集。</returns>
        public DataSet ExecuteDataSet(OleDbConnection connection, string commandString, params OleDbParameter[] commandParameters)
        {
            OleDbCommand command = new OleDbCommand();
            PrepareCommand(command, connection, null, commandString, commandParameters);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            command.Parameters.Clear();
            return dataSet;
        }

        /// <summary>
        /// 预处理命令。
        /// </summary>
        /// <param name="command">命令。</param>
        /// <param name="connection">连接。</param>
        /// <param name="transaction">事务。</param>
        /// <param name="commandString">命令字符串。</param>
        /// <param name="commandParameters">命令参数集。</param>
        private void PrepareCommand(OleDbCommand command, OleDbConnection connection, OleDbTransaction transaction, string commandString, OleDbParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandString;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = System.Data.CommandType.Text;
            if (commandParameters != null)
            {
                foreach (OleDbParameter parameter in commandParameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
        }

        private string GetSql(string sqlCode)
        {
            string sql = string.Empty;
            OleDbParameter parameter = null;
            OleDbDataReader reader = null;
            try
            {
                parameter = new OleDbParameter("@SQL_CODE", sqlCode);
                reader = ExecuteReader(connectionOfLocal, SQL_QUERY_SQL, parameter);
                if (reader.Read())
                {
                    sql = reader["SQL_BODY"].ToString();
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("80。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return sql;
        }

        #region 旅店相关。

        /// <summary>
        /// 插入旅店信息。
        /// </summary>
        /// <param name="hotel">旅店实体。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int InsertHotelInfo(Hotel hotel)
        {
            try
            {
                string sql = GetSql("INSERT_HOTELINFO");
                OleDbParameter parameter0 = new OleDbParameter("@HOTEL_CODE", hotel.Code);
                OleDbParameter parameter1 = new OleDbParameter("@HOTEL_NAME", hotel.Name);
                OleDbParameter parameter2 = new OleDbParameter("@HOTEL_LOCATION", hotel.Location);
                OleDbParameter parameter3 = new OleDbParameter("@HOTEL_MAP_URL", hotel.MapUrl);
                OleDbParameter parameter4 = new OleDbParameter("@HOTEL_TEL", hotel.HotelTel);
                OleDbParameter parameter5 = new OleDbParameter("@HOTEL_OWNER", hotel.HotelOwner);
                OleDbParameter parameter6 = new OleDbParameter("@HOTEL_OWNER_TEL", hotel.HotelOwnerTel);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4, parameter5, parameter6);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("79。", e);
            }
        }

        /// <summary>
        /// 根据旅店编码删除旅店信息。
        /// </summary>
        /// <param name="hotelCode">旅店编码。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int DeleteHotelInfoByHotelCode(string hotelCode)
        {
            try
            {
                string sql = GetSql("DELETE_HOTELINFO_BY_HOTELCODE");
                OleDbParameter parameter = new OleDbParameter("@HOTEL_CODE", hotelCode);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("78。", e);
            }
        }

        /// <summary>
        /// 根据旅店编码更新旅店信息。
        /// </summary>
        /// <param name="hotel">旅店信息实体。</param>
        /// <param name="hotelCode">旅店编码。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int UpdateHotelInfoByHotelCode(Hotel hotel, string hotelCode)
        {
            try
            {
                string sql = GetSql("UPDATE_HOTELINFO_BY_HOTELCODE");
                OleDbParameter parameter0 = new OleDbParameter("@HOTEL_CODE", hotel.Code);
                OleDbParameter parameter1 = new OleDbParameter("@HOTEL_NAME", hotel.Name);
                OleDbParameter parameter2 = new OleDbParameter("@HOTEL_LOCATION", hotel.Location);
                OleDbParameter parameter3 = new OleDbParameter("@HOTEL_MAP_URL", hotel.MapUrl);
                OleDbParameter parameter4 = new OleDbParameter("@HOTEL_TEL", hotel.HotelTel);
                OleDbParameter parameter5 = new OleDbParameter("@HOTEL_OWNER", hotel.HotelOwner);
                OleDbParameter parameter6 = new OleDbParameter("@HOTEL_OWNER_TEL", hotel.HotelOwnerTel);
                OleDbParameter parameter7 = new OleDbParameter("@HOTEL_CODE", hotelCode);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("77。", e);
            }
        }

        /// <summary>
        /// 根据旅店编码获得旅店实体。
        /// </summary>
        /// <param name="hotelCode">旅店编码。</param>
        /// <returns>旅店实体。</returns>
        public Hotel GetHotelInfoByHotelCode(string hotelCode)
        {
            OleDbParameter parameter = null;
            OleDbDataReader reader = null;
            Hotel hotel = null;
            try
            {
                string sql = GetSql("QUERY_HOTELINFO_BY_HOTELCODE");
                parameter = new OleDbParameter("@HOTEL_CODE", hotelCode);
                reader = ExecuteReader(connectionOfLocal, sql, parameter); 
                if (reader.Read())
                {
                    hotel = new Hotel();
                    hotel.Code = reader["HOTEL_CODE"].ToString();
                    hotel.Name = reader["HOTEL_NAME"].ToString();
                    hotel.Location = reader["HOTEL_LOCATION"].ToString();
                    hotel.MapUrl = reader["HOTEL_MAP_URL"].ToString();
                    hotel.HotelTel = reader["HOTEL_TEL"].ToString();
                    hotel.HotelOwner = reader["HOTEL_OWNER"].ToString();
                    hotel.HotelOwnerTel = reader["HOTEL_OWNER_TEL"].ToString();
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("76。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return hotel;
        }

        /// <summary>
        /// 获得所有旅店实体。
        /// </summary>
        /// <returns>旅店实体列表。</returns>
        public List<Hotel> GetAllHotelInfo()
        {
            OleDbDataReader reader = null;
            List<Hotel> hotelList = null;
            try
            {
                string sql = GetSql("QUERY_ALL_HOTELINFO");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                hotelList = new List<Hotel>();
                while (reader.Read())
                {
                    Hotel hotel = new Hotel();
                    hotel.Code = reader["HOTEL_CODE"].ToString();
                    hotel.Name = reader["HOTEL_NAME"].ToString();
                    hotel.Location = reader["HOTEL_LOCATION"].ToString();
                    hotel.MapUrl = reader["HOTEL_MAP_URL"].ToString();
                    hotel.HotelTel = reader["HOTEL_TEL"].ToString();
                    hotel.HotelOwner = reader["HOTEL_OWNER"].ToString();
                    hotel.HotelOwnerTel = reader["HOTEL_OWNER_TEL"].ToString();
                    hotelList.Add(hotel);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("75。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return hotelList;
        }

        #endregion

        #region 警员相关。

        /// <summary>
        /// 插入警员信息。
        /// </summary>
        /// <param name="hotel">警员实体。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int InsertPoliceman(Policeman policeman)
        {
            try
            {
                string sql = GetSql("INSERT_POLICEMAN");
                OleDbParameter parameter0 = new OleDbParameter("@POLICEMAN_NAME", policeman.Name);
                OleDbParameter parameter1 = new OleDbParameter("@MOBILEPHONE", policeman.MobilePhone);
                OleDbParameter parameter2 = new OleDbParameter("@DUTY_TIME_BEGIN", policeman.DutyTimeBegin);
                OleDbParameter parameter3 = new OleDbParameter("@DUTY_TIME_END", policeman.DutyTimeEnd);
                OleDbParameter parameter4 = new OleDbParameter("@IS_ON_DUTY", policeman.IsOnDuty);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("74。", e);
            }
        }

        /// <summary>
        /// 根据移动手机删除警员信息。
        /// </summary>
        /// <param name="mobilephone">移动电话。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int DeletePolicemanByMobilephone(string mobilephone)
        {
            try
            {
                string sql = GetSql("DELETE_POLICEMAN_BY_MOBILEPHONE");
                OleDbParameter parameter = new OleDbParameter("@MOBILEPHONE", mobilephone);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("73。", e);
            }
        }

        /// <summary>
        /// 根据移动电话更新警员信息。
        /// </summary>
        /// <param name="policeman">警员实体。</param>
        /// <param name="mobilephone">移动电话。。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int UpdatePolicemanByMobilephone(Policeman policeman, string mobilephone)
        {
            try
            {
                string sql = GetSql("UPDATE_POLICEMAN_BY_MOBILEPHONE");
                OleDbParameter parameter0 = new OleDbParameter("@POLICEMAN_NAME", policeman.Name);
                OleDbParameter parameter1 = new OleDbParameter("@MOBILEPHONE", policeman.MobilePhone);
                OleDbParameter parameter2 = new OleDbParameter("@DUTY_TIME_BEGIN", policeman.DutyTimeBegin);
                OleDbParameter parameter3 = new OleDbParameter("@DUTY_TIME_END", policeman.DutyTimeEnd);
                OleDbParameter parameter4 = new OleDbParameter("@IS_ON_DUTY", policeman.IsOnDuty);
                OleDbParameter parameter5 = new OleDbParameter("@MOBILEPHONE", mobilephone);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4, parameter5);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("72。", e);
            }
        }

        /// <summary>
        /// 根据移动电话获得警员信息。
        /// </summary>
        /// <param name="mobilephone">移动电话。</param>
        /// <returns>警员实体。</returns>
        public Policeman GetPolicemanByMobilephone(string mobilephone)
        {
            OleDbParameter parameter = null;
            OleDbDataReader reader = null;
            Policeman policeman = null;
            try
            {
                string sql = GetSql("QUERY_POLICEMAN_BY_MOBILEPHONE");
                parameter = new OleDbParameter("@MOBILEPHONE", mobilephone);
                reader = ExecuteReader(connectionOfLocal, sql, parameter);
                if (reader.Read())
                {
                    policeman = new Policeman();
                    policeman.Name = reader["POLICEMAN_NAME"].ToString();
                    policeman.MobilePhone = reader["MOBILEPHONE"].ToString();
                    policeman.DutyTimeBegin = reader["DUTY_TIME_BEGIN"].ToString();
                    policeman.DutyTimeEnd = reader["DUTY_TIME_END"].ToString();
                    policeman.IsOnDuty = Convert.ToBoolean(reader["IS_ON_DUTY"]);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("71。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return policeman;
        }

        /// <summary>
        /// 获得所有警员实体。
        /// </summary>
        /// <returns>警员实体列表。</returns>
        public List<Policeman> GetAllPoliceman()
        {
            OleDbDataReader reader = null;
            List<Policeman> policemanList = null;
            try
            {
                string sql = GetSql("QUERY_ALL_POLICEMAN");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                policemanList = new List<Policeman>();
                while (reader.Read())
                {
                    Policeman policeman = new Policeman();
                    policeman.Name = reader["POLICEMAN_NAME"].ToString();
                    policeman.MobilePhone = reader["MOBILEPHONE"].ToString();
                    policeman.DutyTimeBegin = reader["DUTY_TIME_BEGIN"].ToString();
                    policeman.DutyTimeEnd = reader["DUTY_TIME_END"].ToString();
                    policeman.IsOnDuty = Convert.ToBoolean(reader["IS_ON_DUTY"]);
                    policemanList.Add(policeman);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("70。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return policemanList;
        }

        #endregion

        #region 特殊监控。

        /// <summary>
        /// 插入特殊监控。
        /// </summary>
        /// <param name="specialSupervised">特殊监控实体。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int InsertSpecialSupervised(SpecialSupervised specialSupervised)
        {
            try
            {
                string sql = GetSql("INSERT_SPECIALSUPERVISED");
                OleDbParameter parameter0 = new OleDbParameter("@CARD_NUMBER", specialSupervised.CardNumber);
                OleDbParameter parameter1 = new OleDbParameter("@SPECIALSUPERVISED_NAME", specialSupervised.Name);
                OleDbParameter parameter2 = new OleDbParameter("@FUZZY_COMPARE", specialSupervised.FuzzyCompare);
                OleDbParameter parameter3 = new OleDbParameter("@NEED_SMS_ALERT", specialSupervised.NeedSMSAlert);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("58。", e);
            }
        }

        /// <summary>
        /// 根据身份证号删除特殊监控。
        /// </summary>
        /// <param name="cardNumber">身份证号。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int DeleteSpecialSupervisedByCardNumber(string cardNumber)
        {
            try
            {
                string sql = GetSql("DELETE_SPECIALSUPERVISED_BY_CARDNUMBER");
                OleDbParameter parameter = new OleDbParameter("@CARD_NUMBER", cardNumber);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("59。", e);
            }
        }

        /// <summary>
        /// 根据身份证号更新特殊监控。
        /// </summary>
        /// <param name="specialSupervised">特殊监控实体。</param>
        /// <param name="cardNumber">身份证号。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int UpdateSpecialSupervisedByCardNumber(SpecialSupervised specialSupervised, string cardNumber)
        {
            try
            {
                string sql = GetSql("UPDATE_SPECIALSUPERVISED_BY_CARDNUMBER");
                OleDbParameter parameter0 = new OleDbParameter("@CARD_NUMBER", specialSupervised.CardNumber);
                OleDbParameter parameter1 = new OleDbParameter("@SPECIALSUPERVISED_NAME", specialSupervised.Name);
                OleDbParameter parameter2 = new OleDbParameter("@FUZZY_COMPARE", specialSupervised.FuzzyCompare);
                OleDbParameter parameter3 = new OleDbParameter("@NEED_SMS_ALERT", specialSupervised.NeedSMSAlert);
                OleDbParameter parameter4 = new OleDbParameter("@CARD_NUMBER", cardNumber);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("57。", e);
            }
        }

        /// <summary>
        /// 根据身份证号查询特殊监控。
        /// </summary>
        /// <param name="cardNumber">身份证号。</param>
        /// <returns>特殊监控实体。</returns>
        public SpecialSupervised GetSpecialSupervisedByCardNumber(string cardNumber)
        {
            OleDbDataReader reader = null;
            SpecialSupervised specialSupervised=null;
            try
            {
                string sql = GetSql("QUERY_SPECIALSUPERVISED_BY_CARDNUMBER");
                OleDbParameter parameter = new OleDbParameter("@CARD_NUMBER", cardNumber);
                reader = ExecuteReader(connectionOfLocal, sql, parameter);

                if (reader.Read())
                {
                    specialSupervised = new SpecialSupervised();
                    specialSupervised.CardNumber = reader["CARD_NUMBER"].ToString();
                    specialSupervised.Name = reader["SPECIALSUPERVISED_NAME"].ToString();
                    specialSupervised.FuzzyCompare = Convert.ToBoolean(reader["FUZZY_COMPARE"]);
                    specialSupervised.NeedSMSAlert = Convert.ToBoolean(reader["NEED_SMS_ALERT"]);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("60。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return specialSupervised;
        }

        /// <summary>
        /// 查询所有特殊监控。
        /// </summary>
        /// <returns>特殊监控列表。</returns>
        public List<SpecialSupervised> GetAllSpecialSupervised()
        {
            OleDbDataReader reader = null;
            List<SpecialSupervised> specialSupervisedList = new List<SpecialSupervised>();
            try
            {
                string sql = GetSql("QUERY_ALL_SPECIALSUPERVISED");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                while (reader.Read())
                {
                    SpecialSupervised specialSupervised = new SpecialSupervised();
                    specialSupervised.CardNumber = reader["CARD_NUMBER"].ToString();
                    specialSupervised.Name = reader["SPECIALSUPERVISED_NAME"].ToString();
                    specialSupervised.FuzzyCompare = Convert.ToBoolean(reader["FUZZY_COMPARE"]);
                    specialSupervised.NeedSMSAlert = Convert.ToBoolean(reader["NEED_SMS_ALERT"]);
                    specialSupervisedList.Add(specialSupervised);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("61。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return specialSupervisedList;
        }

        #endregion

        #region 日志相关。

        /// <summary>
        /// 插入旅客日志。
        /// </summary>
        /// <param name="specialSupervised">旅客日志实体。</param>
        /// <returns>1为成功；0为未查找到数据；-1为失败。</returns>
        public int InsertGuestLog(GuestLog guestLog)
        {
            try
            {
                string sql = GetSql("INSERT_GUESTLOG");
                OleDbParameter parameter0 = new OleDbParameter("@LOG_TIME", guestLog.Time.ToString());
                OleDbParameter parameter1 = new OleDbParameter("@GUEST_TYPE", guestLog.Guest.GuestType.ToString());
                OleDbParameter parameter2 = new OleDbParameter("@CARD_NUMBER", guestLog.Guest.CardNumber);
                OleDbParameter parameter3 = new OleDbParameter("@GUEST_NAME", guestLog.Guest.Name);
                OleDbParameter parameter4 = new OleDbParameter("@HOTEL_CODE", guestLog.Hotel.Code);
                OleDbParameter parameter5 = new OleDbParameter("@ROOM_CODE", guestLog.Guest.LogRoom);
                OleDbParameter parameter6 = new OleDbParameter("@LOGIN_TIME", guestLog.Guest.LogTime);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4, parameter5, parameter6);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("66。", e);
            }
        }

        public int InsertSystemLog(SystemLog systemLog)
        {
            try
            {
                string sql = GetSql("INSERT_SYSTEMLOG");
                OleDbParameter parameter0 = new OleDbParameter("@LOG_TIME", systemLog.Time.ToString());
                OleDbParameter parameter1 = new OleDbParameter("@SYSTEM_LOG_TYPE", systemLog.SystemLogType.ToString());
                OleDbParameter parameter2 = new OleDbParameter("@LOG_CONTENT", systemLog.Content);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("67。", e);
            }
        }

        public int InsertCommand(CommandObject commandObject)
        {
            try
            {
                string sql = GetSql("INSERT_COMMAND");
                OleDbParameter parameter0 = new OleDbParameter("@COMMANDID", commandObject.CommandID);
                OleDbParameter parameter1 = new OleDbParameter("@COMMANDTYPE", commandObject.CommandType.ToString());
                OleDbParameter parameter2 = new OleDbParameter("@SENDTIME", commandObject.SendTime.ToString());
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("98。", e);
            }
        }

        public int InsertCommandInfo(CommandInfoObject commandInfoObject)
        {
            try
            {
                string sql = GetSql("INSERT_COMMANDINFO");
                OleDbParameter parameter0 = new OleDbParameter("@COMMANDID", commandInfoObject.CommandID);
                OleDbParameter parameter1 = new OleDbParameter("@RESPONSEHOTELID", commandInfoObject.ResponseHotelID);
                OleDbParameter parameter2 = new OleDbParameter("@RESPONSEHOTELNAME", commandInfoObject.ResponseHotelName);
                OleDbParameter parameter3 = new OleDbParameter("@RESPONSECONTENT", commandInfoObject.ResponseContent);
                OleDbParameter parameter4 = new OleDbParameter("@RESPONSETIME", commandInfoObject.ResponseTime.ToString());
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("99。", e);
            }
        }

        public List<GuestLog> GetGuestLogByTop(int number)
        {
            OleDbDataReader reader = null;
            List<GuestLog> guestLogList = null;
            try
            {
                string sql = GetSql("QUERY_GUESTLOG_BY_TOP");
                sql = string.Format(sql, number.ToString());
                reader = ExecuteReader(connectionOfLocal, sql, null);
                guestLogList = new List<GuestLog>();
                while (reader.Read())
                {
                    GuestLog guestLog = new GuestLog();
                    guestLog.Time = Convert.ToDateTime(reader["LOG_TIME"]);
                    guestLog.Guest.GuestType = (GuestType)Enum.Parse(typeof(GuestType), reader["GUEST_TYPE"].ToString());
                    guestLog.Guest.CardNumber = reader["CARD_NUMBER"].ToString();
                    guestLog.Guest.Name = reader["GUEST_NAME"].ToString();
                    guestLog.Hotel.Code = reader["HOTEL_CODE"].ToString();
                    guestLog.Hotel.Name = reader["HOTEL_NAME"].ToString();
                    guestLog.Guest.LogRoom = reader["ROOM_CODE"].ToString();
                    guestLog.Guest.LogTime = reader["LOGIN_TIME"].ToString();
                    guestLogList.Add(guestLog);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("68。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return guestLogList;
        }

        public List<SystemLog> GetSystemLogByTop(int number)
        {
            OleDbDataReader reader = null;
            List<SystemLog> systemLogList = null;
            try
            {
                string sql = GetSql("QUERY_SYSTEMLOG_BY_TOP");
                sql = string.Format(sql, number.ToString());
                reader = ExecuteReader(connectionOfLocal, sql, null);
                systemLogList = new List<SystemLog>(); 
                while (reader.Read())
                {
                    SystemLog systemLog = new SystemLog();
                    systemLog.Time = Convert.ToDateTime(reader["LOG_TIME"]);
                    systemLog.SystemLogType = (SystemLogType)Enum.Parse(typeof(SystemLogType), reader["SYSTEM_LOG_TYPE"].ToString());
                    systemLog.Content = reader["LOG_CONTENT"].ToString();
                    systemLogList.Add(systemLog);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("69。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return systemLogList;
        }

        #endregion

        #region 嫌疑人相关。

        public int InsertSuspectHistory(Suspect suspect)
        {
            try
            {
                string sql = GetSql("INSERT_SUSPECTHISTORY");
                OleDbParameter parameter0 = new OleDbParameter("@SUSPECT_ID", suspect.ID);
                OleDbParameter parameter1 = new OleDbParameter("@CHECK_SOURCE", suspect.CheckSource.ToString());
                OleDbParameter parameter2 = new OleDbParameter("@SUSPECT_NAME", suspect.Name);
                OleDbParameter parameter3 = new OleDbParameter("@SUSPECT_NAMEOTHER", suspect.NameOther);
                OleDbParameter parameter4 = new OleDbParameter("@SUSPECT_CARDNUMBER", suspect.CardNumber);
                OleDbParameter parameter5 = new OleDbParameter("@HOME_LOCATION", suspect.HomeLocation);
                OleDbParameter parameter6 = new OleDbParameter("@HOME_LOCATIONNOW", suspect.HomeLocationNow);
                OleDbParameter parameter7 = new OleDbParameter("@CHARGE", suspect.Charge);
                OleDbParameter parameter8 = new OleDbParameter("@LAST_APPEAR_HOTEL_CODE", suspect.LastAppearHotelCode);
                OleDbParameter parameter9 = new OleDbParameter("@LAST_APPEAR_HOTEL_NAME", suspect.LastAppearHotelName);
                OleDbParameter parameter10 = new OleDbParameter("@LAST_APPEAR_HOTEL_ROOM", suspect.LastAppearHotelRoom);
                OleDbParameter parameter11 = new OleDbParameter("@LAST_APPEAR_TIME", suspect.LastAppearTime);
                OleDbParameter parameter12 = new OleDbParameter("@HANDLED", suspect.Handled);
                OleDbParameter parameter13 = new OleDbParameter("@HANDLE_TIME", suspect.HandleTime.ToString());
                OleDbParameter parameter14 = new OleDbParameter("@HISTORY_CREATE_TIME", suspect.HistoryCreateTime.ToString());
                OleDbParameter parameter15 = new OleDbParameter("@CONTACTS", suspect.Contacts);
                OleDbParameter parameter16 = new OleDbParameter("@CONTACTSTEL", suspect.ContactsTel);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9, parameter10, parameter11, parameter12, parameter13, parameter14, parameter15, parameter16);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("100。", e);
            }
        }

        public List<Suspect> GetAllSuspectHistory()
        {
            OleDbDataReader reader = null;
            List<Suspect> suspectList = null;
            try
            {
                string sql = GetSql("QUERY_ALL_SUSPECTHISTORY");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                suspectList = new List<Suspect>();
                while (reader.Read())
                {
                    Suspect suspect = new Suspect();
                    suspect.ID = reader["SUSPECT_ID"].ToString();
                    suspect.CheckSource = (CheckSource)Enum.Parse(typeof(CheckSource), reader["CHECK_SOURCE"].ToString());
                    suspect.Name = reader["SUSPECT_NAME"].ToString();
                    suspect.NameOther = reader["SUSPECT_NAMEOTHER"].ToString();
                    suspect.CardNumber = reader["SUSPECT_CARDNUMBER"].ToString();
                    suspect.HomeLocation = reader["HOME_LOCATION"].ToString();
                    suspect.HomeLocationNow = reader["HOME_LOCATIONNOW"].ToString();
                    suspect.Charge = reader["CHARGE"].ToString();
                    suspect.LastAppearHotelCode = reader["LAST_APPEAR_HOTEL_CODE"].ToString();
                    suspect.LastAppearHotelName = reader["LAST_APPEAR_HOTEL_NAME"].ToString();
                    suspect.LastAppearHotelRoom = reader["LAST_APPEAR_HOTEL_ROOM"].ToString();
                    suspect.LastAppearTime = reader["LAST_APPEAR_TIME"].ToString();
                    suspect.Handled = Convert.ToBoolean(reader["HANDLED"]);
                    suspect.HandleTime = Convert.ToDateTime(reader["HANDLE_TIME"]);
                    suspect.HistoryCreateTime = Convert.ToDateTime(reader["HISTORY_CREATE_TIME"]);
                    suspect.Contacts = reader["CONTACTS"].ToString();
                    suspect.ContactsTel = reader["CONTACTSTEL"].ToString();
                    suspectList.Add(suspect);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("101。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return suspectList;
        }

        public List<Suspect> GetHandledSuspectHistory()
        {
            OleDbDataReader reader = null;
            List<Suspect> suspectList = null;
            try
            {
                string sql = GetSql("QUERY_HANDLED_SUSPECTHISTORY");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                suspectList = new List<Suspect>();
                while (reader.Read())
                {
                    Suspect suspect = new Suspect();
                    suspect.ID = reader["SUSPECT_ID"].ToString();
                    suspect.CheckSource = (CheckSource)Enum.Parse(typeof(CheckSource), reader["CHECK_SOURCE"].ToString());
                    suspect.Name = reader["SUSPECT_NAME"].ToString();
                    suspect.CardNumber = reader["SUSPECT_CARDNUMBER"].ToString();
                    suspect.HomeLocation = reader["HOME_LOCATION"].ToString();
                    suspect.Charge = reader["CHARGE"].ToString();
                    suspect.LastAppearHotelCode = reader["LAST_APPEAR_HOTEL_CODE"].ToString();
                    suspect.LastAppearHotelName = reader["LAST_APPEAR_HOTEL_NAME"].ToString();
                    suspect.LastAppearHotelRoom = reader["LAST_APPEAR_HOTEL_ROOM"].ToString();
                    suspect.LastAppearTime = reader["LAST_APPEAR_TIME"].ToString();
                    suspect.Handled = Convert.ToBoolean(reader["HANDLED"]);
                    suspect.HandleTime = Convert.ToDateTime(reader["HANDLE_TIME"]);
                    suspect.HistoryCreateTime = Convert.ToDateTime(reader["HISTORY_CREATE_TIME"]);
                    suspectList.Add(suspect);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("102。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return suspectList;
        }

        public List<Suspect> GetUnHandledSuspectHistory()
        {
            OleDbDataReader reader = null;
            List<Suspect> suspectList = null;
            try
            {
                string sql = GetSql("QUERY_UN_HANDLED_SUSPECTHISTORY");
                reader = ExecuteReader(connectionOfLocal, sql, null);
                suspectList = new List<Suspect>();
                while (reader.Read())
                {
                    Suspect suspect = new Suspect();
                    suspect.ID = reader["SUSPECT_ID"].ToString();
                    suspect.CheckSource = (CheckSource)Enum.Parse(typeof(CheckSource), reader["CHECK_SOURCE"].ToString());
                    suspect.Name = reader["SUSPECT_NAME"].ToString();
                    suspect.NameOther = reader["SUSPECT_NAMEOTHER"].ToString();
                    suspect.CardNumber = reader["SUSPECT_CARDNUMBER"].ToString();
                    suspect.HomeLocation = reader["HOME_LOCATION"].ToString();
                    suspect.HomeLocationNow = reader["HOME_LOCATIONNOW"].ToString();
                    suspect.Charge = reader["CHARGE"].ToString();
                    suspect.LastAppearHotelCode = reader["LAST_APPEAR_HOTEL_CODE"].ToString();
                    suspect.LastAppearHotelName = reader["LAST_APPEAR_HOTEL_NAME"].ToString();
                    suspect.LastAppearHotelRoom = reader["LAST_APPEAR_HOTEL_ROOM"].ToString();
                    suspect.LastAppearTime = reader["LAST_APPEAR_TIME"].ToString();
                    suspect.Handled = Convert.ToBoolean(reader["HANDLED"]);
                    suspect.HandleTime = Convert.ToDateTime(reader["HANDLE_TIME"]);
                    suspect.HistoryCreateTime = Convert.ToDateTime(reader["HISTORY_CREATE_TIME"]);
                    suspect.Contacts = reader["CONTACTS"].ToString();
                    suspect.ContactsTel = reader["CONTACTSTEL"].ToString();
                    suspectList.Add(suspect);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("103。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return suspectList;
        }

        public List<Suspect> CheckSuspectFromCompare(string cardNumber)
        {
            OleDbDataReader reader = null;
            List<Suspect> suspectList = null;
            try
            {
                string sql = GetSql("QUERY_SUSPECT_BY_CARDNUMBER");
                OleDbParameter parameter = new OleDbParameter("@CARDNUMBER", cardNumber);
                reader = ExecuteReader(connectionOfCompare, sql, parameter);
                suspectList = new List<Suspect>();
                while (reader.Read())
                {
                    Suspect suspect = new Suspect();
                    suspect.CheckSource = CheckSource.天网追逃;
                    suspect.Name = reader["bt2"].ToString();
                    suspect.NameOther = reader["bt3"].ToString();
                    suspect.CardNumber = reader["bt7"].ToString();
                    suspect.HomeLocation = reader["bt9"].ToString();
                    suspect.HomeLocationNow = reader["bt11"].ToString();
                    suspect.Contacts = reader["bt25"].ToString();
                    suspect.ContactsTel = reader["bt26"].ToString();
                    suspectList.Add(suspect);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("104。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return suspectList;
        }

        public Suspect CheckSuspectFromLocal(string cardNumber)
        {
            OleDbDataReader reader = null;
            Suspect suspect = null;
            try
            {
                string sql = GetSql("QUERY_SPECIALSUPERVISED_BY_CARDNUMBER");
                OleDbParameter parameter = new OleDbParameter("@CARD_NUMBER", cardNumber);
                reader = ExecuteReader(connectionOfLocal, sql, parameter);
                while (reader.Read())
                {
                    suspect = new Suspect();
                    suspect.CardNumber = reader["CARD_NUMBER"].ToString();
                    suspect.Name = reader["SPECIALSUPERVISED_NAME"].ToString();
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("105。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return suspect;
        }

        public bool CheckSuspectFromLocalFuzzy(string cardNumber)
        {
            OleDbDataReader reader = null;
            List<Suspect> suspectList = null;
            try
            {
                string sql = GetSql("QUERY_SPECIALSUPERVISED_BY_CARDNUMBER_FUZZY");
                OleDbParameter parameter = new OleDbParameter("@CARD_NUMBER", cardNumber);
                reader = ExecuteReader(connectionOfLocal, sql, parameter);
                suspectList = new List<Suspect>();
                while (reader.Read())
                {
                    Suspect suspect = new Suspect();
                    suspect.CardNumber = reader["CARD_NUMBER"].ToString();
                    suspect.Name = reader["SPECIALSUPERVISED_NAME"].ToString();
                    suspectList.Add(suspect);
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("106。", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (suspectList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UpdateSuspectToHandled(string suspectID)
        {
            try
            {
                string sql = GetSql("UPDATE_SUSPECTHISTORY_TO_HANDLED");
                OleDbParameter parameter0 = new OleDbParameter("@HANDLED", true);
                OleDbParameter parameter1 = new OleDbParameter("@HANDLE_TIME", DateTime.Now.ToString());
                OleDbParameter parameter2 = new OleDbParameter("@SUSPECT_ID", suspectID);
                return ExecuteNonQuery(connectionOfLocal, sql, parameter0, parameter1, parameter2);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("109。", e);
            }
        }

        #endregion
    }
}
