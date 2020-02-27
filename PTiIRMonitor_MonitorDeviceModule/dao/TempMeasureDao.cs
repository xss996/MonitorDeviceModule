using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.dao
{
    public class TempMeasureDao
    {
        private string ip;
        private int port;
        private string username;
        private string password;
        private string databaseName;

        public TempMeasureDao()
        {
            ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            port = Convert.ToInt32(INIUtil.Read("DATABASE", "port", Constant.IniFilePath));
            username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
        }

        public bool AddOneTempMeasureRecord(TempMeasure oneTempMeaRecord)
        {
            string sql = "insert into record_tempmeasure(MeasureTask_Index,MonDev_Index,MeasureValueMin,MeasureValueAvg,MeasureValueMax,AirTemperature,Humidity,Electricty,RecordTime,TVFilePath," +
                "IRFilePath,VTVFilePath,IRAFilePath,VIRFilePath,TVVImgPath,IRVImgPath,IRHImgPath,Readed) " +
                " values(@MeasureTask_Index,@MonDev_Index,@MeasureValueMin,@MeasureValueAvg,@MeasureValueMax,@AirTemperature,@Humidity,@Electricty,@RecordTime,@TVFilePath," +
                "@IRFilePath,@VTVFilePath,@IRAFilePath,@VIRFilePath,@TVVImgPath,@IRVImgPath,@IRHImgPath,@Readed)";
            List<MySqlParameter> parmList = new List<MySqlParameter>();
            parmList.Add(new MySqlParameter("@MeasureTask_Index", oneTempMeaRecord.MeasureTask_Index));
            parmList.Add(new MySqlParameter("@MonDev_Index", oneTempMeaRecord.MonDev_Index));
            parmList.Add(new MySqlParameter("@MeasureValueMin", oneTempMeaRecord.MeasureValueMin));
            parmList.Add(new MySqlParameter("@MeasureValueAvg", oneTempMeaRecord.MeasureValueAvg));
            parmList.Add(new MySqlParameter("@MeasureValueMax", oneTempMeaRecord.MeasureValueMax));
            parmList.Add(new MySqlParameter("@AirTemperature", oneTempMeaRecord.AirTemperature));
            parmList.Add(new MySqlParameter("@Humidity", oneTempMeaRecord.Humidity));
            parmList.Add(new MySqlParameter("@Electricty", oneTempMeaRecord.Electricty));
            parmList.Add(new MySqlParameter("@RecordTime", oneTempMeaRecord.RecordTime));
            parmList.Add(new MySqlParameter("@TVFilePath", oneTempMeaRecord.TVFilePath));
            parmList.Add(new MySqlParameter("@IRFilePath", oneTempMeaRecord.IRFilePath));
            parmList.Add(new MySqlParameter("@VTVFilePath", oneTempMeaRecord.VTVFilePath));
            parmList.Add(new MySqlParameter("@IRAFilePath", oneTempMeaRecord.IRAFilePath));
            parmList.Add(new MySqlParameter("@VIRFilePath", oneTempMeaRecord.VIRFilePath));
            parmList.Add(new MySqlParameter("@TVVImgPath", oneTempMeaRecord.TVVImgPath));
            parmList.Add(new MySqlParameter("@IRVImgPath", oneTempMeaRecord.IRVImgPath));
            parmList.Add(new MySqlParameter("@IRHImgPath", oneTempMeaRecord.IRHImgPath));
            parmList.Add(new MySqlParameter("@Readed", oneTempMeaRecord.Readed));
            MySqlConnection conn = SqlHelper.GetConnection(ip, port, username, password, databaseName);
            return SqlHelper.AddData(conn, sql, parmList.ToArray());          
        }
       
        
        
    }
}
