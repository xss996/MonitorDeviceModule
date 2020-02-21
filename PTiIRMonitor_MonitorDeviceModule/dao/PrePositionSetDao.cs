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
   public class PrePositionSetDao
    {
        private string ip;
        private int port;
        private string username;
        private string password;
        private string databaseName;
        public PrePositionSetDao()
        {
            ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            port = Convert.ToInt32(INIUtil.Read("DATABASE", "port", Constant.IniFilePath));
            username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
        }

        /// <summary>
        /// 根据任务点索引查询所对应的预置位信息
        /// </summary>
        /// <param name="taskIndex"></param>
        /// <returns></returns>
        public List<PrePositionSet> GetPrePosIndexByTaskIndex(int taskIndex)
        {
            List<PrePositionSet> prePosList = new List<PrePositionSet>();
            string sql = "SELECT m.MeasureTask_Index,m.PrePosSet_Index,m.DeviceInfo_Index,m.MeaType,m.TaskName,m.GetValueType," +
            "p.Position_Index,p.PrePositionNO,p.PrePositionName,p.PrePosType,p.TVZoom,p.IRFocus,p.GoPrePosDelays,p.PanAngle,p.TiltAngle,p.PTZHorAngle,p.PTZVerAngle,p.PaletteType  " +
            "from cruise_measuretaskset m, cruise_prepositionset p where m.PrePosSet_Index = p.PrePosSet_Index and p.EnbleMeasure = 1 and m.MeasureTask_Index = @taskIndex";

            List<MySqlParameter> parmList = new List<MySqlParameter>();
            parmList.Add(new MySqlParameter("@taskIndex", taskIndex));
            MySqlConnection conn = SqlHelper.GetConnection(ip, port, username, password, databaseName);
            DataTable dataTable = SqlHelper.QueryData(conn, sql, parmList.ToArray());
            if (dataTable.Rows.Count != 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    PrePositionSet prePos = new PrePositionSet();
                    if (!string.IsNullOrEmpty(dataTable.Rows[i]["IRFocus"].ToString()))
                    {
                        prePos.IRFocus = Convert.ToInt32(dataTable.Rows[i]["IRFocus"]);
                    }
                    prePos.PrePosSet_Index = Convert.ToInt32(dataTable.Rows[i]["PrePosSet_Index"]);

                    if (!string.IsNullOrEmpty(dataTable.Rows[i]["TVZoom"].ToString()))
                    {
                        prePos.TVZoom = Convert.ToInt32(dataTable.Rows[i]["TVZoom"]);
                    }
                    MeasureTaskSet oneTask = new MeasureTaskSet();
                    oneTask.MeasureTask_Index = taskIndex;
                    prePos.Position_Index = Convert.ToInt32(dataTable.Rows[i]["Position_Index"]);
                    prePos.PrePositionName = dataTable.Rows[i]["PrePositionName"].ToString();
                    oneTask.MeaType = Convert.ToInt32(dataTable.Rows[i]["MeaType"]);
                    prePos.OneTask = oneTask;
                    prePosList.Add(prePos);
                }     
            }

            return prePosList ;
        }
    }
}
