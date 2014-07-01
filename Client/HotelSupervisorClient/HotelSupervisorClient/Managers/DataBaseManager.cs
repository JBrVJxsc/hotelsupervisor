using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using HotelSupervisorClient.Objects;
using HotelSupervisorClient.Plus;

namespace HotelSupervisorClient.Managers
{
    /// <summary>
    /// 数据库操作类。
    /// </summary>
    internal class DataBaseManager
    {
        public static OleDbConnection connection = null;
        public static DataBaseManager GlobalDataBaseManager = new DataBaseManager();
        private RegistryManager registryManager = new RegistryManager(); 

        public DataBaseManager()
        {
            string connectionString = string.Empty;
            if (connection == null)
            {
                connectionString = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseConnectionString);
                string dataSource = FileSystemWatcherPlus.DataBaseFileFullName;
                string userID = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseFileUserName);
                string password = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseFilePassword);
                connectionString = string.Format(connectionString, dataSource, userID, password);
                connection = new OleDbConnection(connectionString);
            }
        }

        /// <summary>
        /// 打开数据库连接。
        /// </summary>
        public void OpenDataBase()
        {
            try
            {
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("12。"+e.Message);
            }
        }

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        public void CloseDataBase()
        {
            try
            {
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("13。"+e.Message);
            }
        }

        public List<LocalGuest> GetLastLocalGuest()
        {
            List<LocalGuest> localGuestList;
            string lastGuestNumber = registryManager.GetLastGuestNumber();
            if (lastGuestNumber == null)
            {
                localGuestList = GetLastLocalGuestByTop(1);
            }
            else
            {
                localGuestList = GetLastLocalGuestByGuestNumber(lastGuestNumber);
            }

            if (localGuestList.Count == 0)
            {
                return null;
            }
            localGuestList.Sort();
            registryManager.SetLastGuestNumber(localGuestList[localGuestList.Count-1].GuestNumber.ToString());
            if (lastGuestNumber == null)
            {
                return null;
            }
            return localGuestList;
        }

        private List<LocalGuest> GetLastLocalGuestByTop(int number)
        {
            OleDbDataReader reader = null;
            List<LocalGuest> localGuestList = new List<LocalGuest>();
            string sql = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SQL_QUERY_LAST_LOCALGUEST_BY_TOP);
            sql = string.Format(sql, number.ToString());
            reader = ExecuteReader(connection, sql, null);
            try
            {
                while (reader.Read())
                {
                    LocalGuest localGuest = new LocalGuest();
                    localGuest.GuestNumber =reader["F_GuestNum"].ToString();
                    localGuest.CardNumber = reader["F_CardNum"].ToString();
                    localGuest.Name = reader["F_Name"].ToString();
                    localGuest.LoginTime = reader["F_LoginTime"].ToString();
                    localGuest.LoginRoom = reader["F_LoginRoom"].ToString();
                    localGuestList.Add(localGuest);
                }
            }
            catch (Exception e)
            {
                throw new Exception("15。"+e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return localGuestList;
        }

        private List<LocalGuest> GetLastLocalGuestByGuestNumber(string guestNumber)
        {
            OleDbDataReader reader = null;
            List<LocalGuest> localGuestList = new List<LocalGuest>();
            string sql = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.SQL_QUERY_LAST_LOCALGUEST_BY_GUESTNUMBER);
            sql = string.Format(sql, guestNumber);
            reader = ExecuteReader(connection, sql, null);
            try
            {
                while (reader.Read())
                {
                    LocalGuest localGuest = new LocalGuest();
                    localGuest.GuestNumber = reader["F_GuestNum"].ToString();
                    localGuest.CardNumber = reader["F_CardNum"].ToString();
                    localGuest.Name = reader["F_Name"].ToString();
                    localGuest.LoginTime = reader["F_LoginTime"].ToString();
                    localGuest.LoginRoom = reader["F_LoginRoom"].ToString();
                    localGuestList.Add(localGuest);
                }
            }
            catch(Exception e)
            {
                throw new Exception("18。"+e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return localGuestList;
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
            try
            {
                PrepareCommand(command, connection, null, commandString, commandParameters);
                OleDbDataReader reader = command.ExecuteReader();
                command.Parameters.Clear();
                return reader;
            }
            catch(Exception e)
            {
                throw new Exception("14。"+e.Message);
            }
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
            try
            {
                dataAdapter.Fill(dataSet);
                command.Parameters.Clear();
                return dataSet;
            }
            catch
            {
                throw;
            }
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
            command.CommandType = CommandType.Text;
            if (commandParameters != null)
            {
                foreach (OleDbParameter parameter in commandParameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
}
