using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.dao
{
    public class SpotSetDao
    {
        private string ip;
        private int port;
        private string username;
        private string password;
        private string databaseName;

        public SpotSetDao()
        {
            ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            port = Convert.ToInt32(INIUtil.Read("DATABASE", "port", Constant.IniFilePath));
            username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
        }

        public void GetSpotInfoByTaskIndex(int taskIndex)
        {
            string sql = "select * from cruise_spotset where MeasureTask_Index = @taskIndex and Active =1";
            List<MySqlParameter> parmList = new List<MySqlParameter>();
            parmList.Add(new MySqlParameter("@taskIndex", taskIndex));
            MySqlConnection conn = SqlHelper.GetConnection(ip, port, username, password, databaseName);
            DataTable dataTable = SqlHelper.QueryData(conn, sql, parmList.ToArray());
        }       
    }
}
