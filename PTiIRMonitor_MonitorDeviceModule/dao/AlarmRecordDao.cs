using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.dao
{
    public class AlarmRecordDao
    {
        private string ip;
        private int port;
        private string username;
        private string password;
        private string databaseName;

        public AlarmRecordDao()
        {
            ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            port = Convert.ToInt32(INIUtil.Read("DATABASE", "port", Constant.IniFilePath));
            username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
        }

        public bool AddOneAlarmRecord(AlarmRecord alarmRecord)
        {
            string sql = "insert into record_alarmrecord(TempMeasure_Index,Rule_Index,Rule_Type,AlarmType,AlarmRecordTime,RelMeaVal,RefPreAlarmVal,RefAlarmVal,RefSuperAlarmVal,Readed) " +
                "values(@TempMeasure_Index,@Rule_Index,@Rule_Type,@AlarmType,@AlarmRecordTime,@RelMeaVal,@RefPreAlarmVal,@RefAlarmVal,@RefSuperAlarmVal,@Readed)";
            List<MySqlParameter> parmList = new List<MySqlParameter>();
            parmList.Add(new MySqlParameter("@TempMeasure_Index", alarmRecord.TempMeasure_Index));
            parmList.Add(new MySqlParameter("@Rule_Index", alarmRecord.Rule_Index));
            parmList.Add(new MySqlParameter("@Rule_Type", alarmRecord.Rule_Type));
            parmList.Add(new MySqlParameter("@AlarmType", alarmRecord.AlarmType));
            parmList.Add(new MySqlParameter("@AlarmRecordTime", alarmRecord.AlarmRecordTime));
            parmList.Add(new MySqlParameter("@RelMeaVal", alarmRecord.RelMeaVal));
            parmList.Add(new MySqlParameter("@RefPreAlarmVal", alarmRecord.RefPreAlarmVal));
            parmList.Add(new MySqlParameter("@RefAlarmVal", alarmRecord.RefAlarmVal));
            parmList.Add(new MySqlParameter("@RefSuperAlarmVal", alarmRecord.RefSuperAlarmVal));
            parmList.Add(new MySqlParameter("@Readed", alarmRecord.Readed));
            MySqlConnection conn = SqlHelper.GetConnection(ip, port, username, password, databaseName);
            return SqlHelper.AddData(conn, sql, parmList.ToArray());
        }
    }
}
