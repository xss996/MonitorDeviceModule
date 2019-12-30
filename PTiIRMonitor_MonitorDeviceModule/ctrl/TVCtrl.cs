
using MySql.Data.MySqlClient;
using Peiport.Monitor.HKTVCtrl;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class TVCtrl
    {
        public int TVConnectStatus = 0;
        HKTVCtrl hktvCtrl = new HKTVCtrl();

        public string IP = "";
        public int port = 8000;
        public int channel = 1;
        public string username = "";
        public string password = "";

        /// <summary>
        /// 红外初始化,从配置文件读取参数或者数据库获取
        /// </summary>

        public void Init()
        {
            string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
            string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);

            MySqlConnection conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);

            string MonDev_Index = INIUtil.Read("MonDev", "Index", Constant.IniFilePath);
            string strSql = "select * from base_videoserver where MonDev_Index = " + MonDev_Index;
            DataTable data = SqlHelper.QueryData(conn, strSql, null);

            this.IP = data.Rows[0]["TVStreamIP"].ToString();
            if (!string.IsNullOrEmpty(data.Rows[0]["TVStreamPort"].ToString()))
            {
                this.port = Convert.ToInt32(data.Rows[0]["TVStreamPort"]);
            }
            conn.Close();
            this.username = data.Rows[0]["TVUserName"].ToString();
            this.password = data.Rows[0]["TVUserPwd"].ToString();
        }
        /// <summary>
        /// 登录预览视频
        /// </summary>
        /// <param name="isStartPlay">是否预览</param>
        public bool Connect(bool isStartPlay)
        {
            Init();
            bool flag = false;
            if (!string.IsNullOrEmpty(IP) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (isStartPlay)
                {               
                    flag = hktvCtrl.StartPlay(IP, username, password, port, channel);
                }
                else
                {
                    flag = hktvCtrl.ConnectTV(IP, username, password, port, channel);
                }
            }
            int i = hktvCtrl.QuestDevStatus();
            return flag;

        }

        public int GetTVStatus()
        {
            TVConnectStatus = hktvCtrl.QuestDevStatus();
            return TVConnectStatus;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (hktvCtrl.DisconnectTV())
            {
                TVConnectStatus = -1;
            }
        }
        /// <summary>
        /// 停止预览
        /// </summary>
        public void StopPlay()
        {
            hktvCtrl.StopPlay();
        }
        /// <summary>
        /// 设置变焦倍数
        /// </summary>
        /// <param name="zoomType">变焦倍数,>0放大<0缩小,0停止变焦</param>
        public bool SetZoomOpt(int zoomType)
        {
            return hktvCtrl.ZoomOpt(zoomType);
        }
        /// <summary>
        /// 设置变焦位置
        /// </summary>
        /// <param name="fValue">位置值,大于0</param>
        public bool SetZoomPos(float fValue)
        {
            return hktvCtrl.SetZoomPos(fValue);
        }
        /// <summary>
        /// 获取变焦位置
        /// </summary>
        /// <returns></returns>
        public float GetZoomPos()
        {
            // Debug.WriteLine(string.Format("变焦位置为:{0}", hktvCtrl.GetZoomPos()));
            return hktvCtrl.GetZoomPos();
        }
        /// <summary>
        /// 抓图 
        /// </summary>
        /// <param name="imgFileName">图片名称</param>
        /// <param name="imgType">图片类型:1 JPG,2 BMP,默认为1</param>
        public bool SaveImage(string imgFileName, int imgType = 1)
        {
            bool flag = false;
            Thread th = new Thread(()=>{
                flag= hktvCtrl.SnapTVPicture(imgFileName, imgType);
        });
            th.Start();
            return flag;
            
        }
        /// <summary>
        /// 焦距相关操作设置
        /// </summary>
        /// <param name="iOptType">取值:小于0:焦距拉远;等于0:停止聚焦;大于0:焦距拉近</param>
        public bool FacusOpt(int iOptType)
        {
            return hktvCtrl.FocusOpt(iOptType);
        }

        #region 云台相关操作
        public void PTZInit()
        {

        }
        /// <summary>
        /// 云台移动 
        /// </summary>
        /// <param name="xVole">水平速度</param>
        /// <param name="yVole">垂直速度</param>
        public void PTZMove(int xVole, int yVole)
        {
            hktvCtrl.PtzMove(xVole, yVole);
        }

        public void SetAngle(int honAngle, int verAngle)
        {
            Debug.WriteLine(string.Format("云台角度设置,水平角度:{0},垂直角度:{1}", honAngle, verAngle));
        }

        public void GetAngle(ref int honAngle, ref int verAngle)
        {
            Debug.WriteLine(string.Format("当前云台角度x:{0},y:{1}", honAngle, verAngle));
        }
        /// <summary>
        /// 设置预置位
        /// </summary>
        /// <param name="NO"></param>
        public void SetPrePos(int NO)
        {
            if (NO > 0 && NO < 81)
            {
                hktvCtrl.PresetCtrl(1, (uint)NO);
            }
        }
        /// <summary>
        /// 走预置位
        /// </summary>
        /// <param name="NO"></param>
        public void InvokePrePos(int NO)
        {
            if (NO > 0 && NO < 81)
            {
                hktvCtrl.PresetCtrl(2, (uint)NO);
            }
        }
        #endregion

    }
}
