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
    /// <summary>
    /// 测量任务表数据库操作类
    /// </summary>
    public class MeasureTaskSetDao
    {
        private string ip;
        private int port;
        private string username;
        private string password;
        private string databaseName;
        public MeasureTaskSetDao()
        {

            ip =INIUtil.Read("DATABASE","ip",Constant.IniFilePath);
            port =Convert.ToInt32(INIUtil.Read("DATABASE", "port", Constant.IniFilePath));
            username =INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
        }
        /// <summary>
        /// 获取巡检任务点的索引
        /// </summary>
        /// <returns></returns>
        public List<int> GetMeasureTaskIndex()
        {
            List<int> taskIndexList= new List<int>();
            string sql = "select MeasureTask_Index from cruise_measuretaskset";
            MySqlConnection connenction =SqlHelper.GetConnection(ip, port, username, password, databaseName);
            DataTable dataTable = SqlHelper.QueryData(connenction, sql, null);
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                taskIndexList.Add(Convert.ToInt32(dataTable.Rows[i]["MeasureTask_Index"]));
            }
            return taskIndexList;
        }
    }
}
