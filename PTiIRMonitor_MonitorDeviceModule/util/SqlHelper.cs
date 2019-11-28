using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.util
{
    public class SqlHelper
    {
        private SqlHelper() { }

        private const String ip = "localhost";
        private const int port = 3306;
        private const String username = "root";
        private const String password = "root";
        private const String database = "newsystem";

        /// <summary>
        /// 初始化连接数据库,本机测试
        /// </summary>
        /// <returns>MySqlConnection</returns>
        public static MySqlConnection getConnection()
        {
            // SslMode = none;
            string connectStr = "server =" + ip + "; port =" + port + "; user =" + username + "; password =" + password + " ;database =" + database + ";;CharSet=gb2312;Allow User Variables=True";
            MySqlConnection connenction = null;
            try
            {
               connenction = new MySqlConnection(connectStr);
            }catch(Exception e)
            {
                connenction = null;
            }
            
            return connenction;

        }

        /// <summary>
        ///自定义初始化连接数据库
        /// </summary>
        /// <returns>MySqlConnection</returns>
        public static MySqlConnection GetConnection(string ip, int port, string username, string password, string database)
        {
            MySqlConnection connenction = null;
            try
            {
                string connectStr = "server =" + ip + "; port =" + port + "; user =" + username + "; password =" + password + " ;database =" + database + ";;CharSet=gb2312;Allow User Variables=True";
                connenction = new MySqlConnection(connectStr);
            }
            catch(Exception ex)
            {
                connenction = null;
            }
            return connenction;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sqlStr"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static DataTable QueryData(MySqlConnection conn, String sqlStr, params SqlParameter[] parameter)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                if (parameter != null)
                {
                    cmd.Parameters.AddRange(parameter);
                }

               // MySqlDataReader reader = cmd.ExecuteReader();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                return dt;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("查询数据异常" + ex.Message);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool UpdateData(MySqlConnection conn, string sqlStr, params MySqlParameter[] parameter)
        {
            try
            {
                conn.Open();
                //sqlStr = "update cruise_spotset set  Emissivity = 0.69, Humitidy   = 0.35 where Spot_Index = 2";
                parameter = null;
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                if (parameter != null)
                {
                    cmd.Parameters.AddRange(parameter);
                    //foreach (MySqlParameter p in parameter)
                    //{
                    //    cmd.Parameters.Add(p);
                    //}

                }
                var row = cmd.ExecuteNonQuery();
                conn.Close();
                if (row > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("更新数据异常" + ex.Message);
            }
        }
        public static bool UpdateData(MySqlConnection conn, string sqlStr)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                var row = cmd.ExecuteNonQuery();
                conn.Close();
                if (row > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("更新数据异常" + ex.Message);
            }
        }

        /// <summary>
        ///添加
        /// </summary>
        /// <param name="sqlStr">删除语句</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public static bool AddData(MySqlConnection conn, string sqlStr, MySqlParameter[] parameter)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                if (parameter != null)
                {
                    cmd.Parameters.AddRange(parameter);
                }
                var row = cmd.ExecuteNonQuery();
                conn.Close();
                if (row > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("添加数据异常" + ex.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sqlStr"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static bool DeleteData(MySqlConnection conn, string sqlStr, MySqlParameter[] parameter)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                if (parameter != null)
                {
                    cmd.Parameters.AddRange(parameter);
                }
                var row = cmd.ExecuteNonQuery();
                conn.Close();
                if (row > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("删除数据异常" + ex.Message);
            }

        }

        /// <summary>
        /// sql拼接
        /// </summary>
        /// <param name="startSql"></param>
        /// <param name="appendList"></param>
        /// <param name="endSql"></param>
        /// <returns></returns>
        public static string SqlAppend(string startSql, List<string> appendList, string endSql)
        {
            for (var i = 0; i < appendList.Count; i++)
            {
                if (i != appendList.Count - 1)
                {
                    startSql += " " + appendList[i] + ",";
                }
                else
                {
                    startSql += appendList[i] + " ";
                }
            }
            return startSql + endSql;
        }
    }
}

