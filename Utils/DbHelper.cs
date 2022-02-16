using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DbHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"] ; 


        #region 执行数据库操作(新增、更新或删除)，返回影响行数
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                PrepareCommand(cmd, con, commandType, commandText, null, null);
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, OleDbTransaction? trans = null, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection con = (trans != null ? trans.Connection : new OleDbConnection(connectionString));
            PrepareCommand(cmd, con, commandType, commandText, null, trans);
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, SqlParameter[]? cmdParms = null, OleDbTransaction? trans = null, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection con = (trans != null ? trans.Connection : new OleDbConnection(connectionString)))
            {
                PrepareCommand(cmd, con, commandType, commandText, cmdParms, trans);
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据

        /// <summary>
        /// 执行数据库操作同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string commandText, SqlParameter[]? cmdParms = null, CommandType commandType = CommandType.Text)
        {
            object result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                PrepareCommand(cmd, con, commandType, commandText, cmdParms);
                try
                {
                    result = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }
        /// <summary>
        /// 执行数据库操作同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string commandText, OleDbTransaction trans, SqlParameter[]? cmdParms = null, CommandType commandType = CommandType.Text)
        {
            object result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection con = (trans != null ? trans.Connection : new OleDbConnection(connectionString));

            PrepareCommand(cmd, con, commandType, commandText, cmdParms, trans);
            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region 执行数据库查询，返回OleDbDataReader对象
        /// <summary>
        /// 执行数据库查询，返回OleDbDataReader对象
        /// </summary>
        /// <param name="cmd">OleDbCommand对象</param>
        /// <returns>OleDbDataReader对象</returns>
        public static OleDbDataReader ExecuteReader(OleDbCommand cmd)
        {
            OleDbDataReader? reader = null;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");

            OleDbConnection con = new OleDbConnection(connectionString);
            PrepareCommand(cmd, con, cmd.CommandType, cmd.CommandText);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回OleDbDataReader对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型（默认语句）</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>OleDbDataReader对象</returns>
        public static OleDbDataReader ExecuteReader(string commandText, SqlParameter[]? cmdParms = null, CommandType commandType = CommandType.Text)
        {
            OleDbDataReader? reader = null;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            OleDbConnection con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, con, commandType, commandText, cmdParms);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<T> ExecuteList<T>(string commandText) where T : class, new()
        {
            DataTable Table = ExecuteDataTable(commandText);
            return ConvertToList<T>(Table);
        }
        /// <summary>
        /// 执行数据库查询，返回实体
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <returns>查询所得的第1行数据</returns>
        public static T ExecuteScalar<T>(string commandText) where T : class, new()
        {
            DataTable Table = ExecuteDataTable(commandText);
            return ConvertToList<T>(Table).FirstOrDefault();
        }

        public static List<T> ConvertToList<T>(DataTable dt) where T : class, new()
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        #endregion

        #region 执行数据库查询，返回DataSet对象
        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="cmd">OleDbCommand对象</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(OleDbCommand cmd)
        {
            DataSet ds = new DataSet();
            OleDbConnection con = new OleDbConnection(connectionString);
            PrepareCommand(cmd, con, cmd.CommandType, cmd.CommandText);
            try
            {
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string commandText, CommandType commandType = CommandType.Text)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            OleDbConnection con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, con, commandType, commandText);
            try
            {
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string commandText, SqlParameter[]? cmdParms = null, CommandType commandType = CommandType.Text)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            OleDbConnection con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, con, commandType, commandText, cmdParms);
            try
            {
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }
        #endregion

        #region 执行数据库查询，返回DataTable对象
        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType = CommandType.Text)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataTable dt = new DataTable();
            OleDbConnection con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, con, commandType, commandText);
            try
            {
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return dt;
        }

        #endregion

        #region 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// <summary>
        /// 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// </summary>
        /// <param name="cmd">Command对象</param>
        /// <param name="conn">Connection对象</param>
        /// <param name="cmdType">SQL字符串执行类型</param>
        /// <param name="cmdText">SQL Text</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, CommandType cmdType, string cmdText, SqlParameter[]? cmdParms = null, OleDbTransaction? trans = null)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            cmd.CommandType = cmdType;
            if (trans != null)
                cmd.Transaction = trans;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static OleDbTransaction BeginTransaction()
        {
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            return conn.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        #endregion
    }
}
