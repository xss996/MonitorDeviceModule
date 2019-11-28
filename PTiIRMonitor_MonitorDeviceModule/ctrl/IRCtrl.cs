using MySql.Data.MySqlClient;
using Peiport.Monitor.IRTelnetCtrl;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;


namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class IRCtrl
    {
        //public IRCtrl(IRTelnetCtrl telnetCtrl)
        //{
        //    this.telnetCtrl = telnetCtrl;
        //}

        public bool IRConnectStatus = false;
        IRTelnetCtrl telnetCtrl;
        string IP;
        int port;
        int TimeOut = 50;

        List<IRTelnetCtrl.SpotInfo> spotInfoList = new List<IRTelnetCtrl.SpotInfo>();
        List<IRTelnetCtrl.LineInfo> lineInfoList = new List<IRTelnetCtrl.LineInfo>();
        List<IRTelnetCtrl.AreaInfo> areaInfoList = new List<IRTelnetCtrl.AreaInfo>();

        #region 红外控制
        /// <summary>
        /// 红外初始化,从配置文件读取参数或者数据库获取
        /// </summary>
        public void Init()
        {
            MySqlConnection conn = null ;
            try
            {
                string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
                string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
                string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
                string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
                string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);

                conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);

                string MonDev_Index = INIUtil.Read("Monitor", "Index", Constant.IniFilePath);
                string strSql = "select * from base_videoserver where MonDev_Index = " + MonDev_Index;
                DataTable data = SqlHelper.QueryData(conn, strSql, null);

                this.IP = data.Rows[0]["IRStreamIP"].ToString();
                if (!string.IsNullOrEmpty(data.Rows[0]["IRStreamPort"].ToString()))
                {
                    this.port = Convert.ToInt32(data.Rows[0]["IRStreamPort"]);
                }
                telnetCtrl = new IRTelnetCtrl(IP, port, TimeOut);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 连接红外
        /// </summary>
        public bool Connect()
        {                    
            return telnetCtrl.ConnectIRTelnet();    
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public bool DisConnect()
        {
            return telnetCtrl.DisconIRTelnet();
        }
        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <returns></returns>
        public bool GetIRConnectStatus()
        {
          return telnetCtrl.GetTelnetSta();    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iOptType">类型:0.停止 ,1.焦距拉近 ,2.焦距拉远 ,3.自动聚焦</param>
        /// <param name="iSpeed">聚焦速度,取值范围>=0</param>
        public bool SetManualFocus(int iOptType,int iSpeed=6)  
        {
          return telnetCtrl.FocusOpt(iOptType, iSpeed);
        }  

        public bool SetAutoFocus()
        {
           return telnetCtrl.FocusOpt(3, 0);
            
        }
        /// <summary>
        /// 设置红外聚焦位置
        /// </summary>
        /// <param name="fValue"></param>
        public bool SetFocusPos(int fValue)
        {
           return telnetCtrl.SetFocusPos(fValue);  
        }
        /// <summary>
        /// 获取红外聚焦位置
        /// </summary>
        /// <param name="iFocusOpt"></param>
        /// <returns></returns>
        public bool GetFocusPos(ref int iFocusOpt)
        {
           return telnetCtrl.GetFocusPos(ref iFocusOpt);          
            
        }


        int palType = 0;
        public bool SetPalette()  //四种类型：WhiteHot/BlackHot/Iron/Rain
        {
           
            if (telnetCtrl.SetPalette(palType))
            {
                Debug.WriteLine(string.Format("当前面板设置为:{0}", palType));
                palType++;
                if (palType >= 3)
                {
                    palType = 0;
                }
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public void SetDigZoom(int value)     //值1,2,4
        {
            Debug.WriteLine(string.Format("数字变焦设置值:{0}", value));
        }
        public void GetDigZoom()
        {
            Debug.WriteLine(string.Format("数字变焦获取值:"));
        }

        //public void SetAdjustMode(string mode)  //Auto/Manu
        //{
        //    telnetCtrl.SetImageAdjustMode();
        //    Debug.WriteLine(string.Format("设置云台调节模式为:{0}", mode));
        //}

        /// <summary>
        /// 设置是否自动调节,亮度,透明度
        /// </summary>
        /// <param name="Auto">是否自动调节</param>
        /// <param name="Brightness">亮度</param>
        /// <param name="Contrast">透明度</param>
        public bool SetImageAdjustMode(bool Auto,int Brightness, int Contrast)
        {
           return telnetCtrl.SetImageAdjustMode(Auto, Brightness, Contrast);   
        }

        /// <summary>
        /// 存红外热图
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool SaveIRHotImage(string fileName)
        {        
            bool flag = telnetCtrl.SaveHotPic(fileName);
            return flag;   
        }

        /// <summary>
        /// 保存红外视频图
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool SaveVideoImage(string fileName)
        {
           return telnetCtrl.SaveVisPic(fileName);            
        }
        #endregion

        #region 红外分析

        #region 参数设置
        public void GetMeaParam(ref float fRefTemp, ref float fEnvTemp, ref float fRelHum, ref float fWinTemp, ref float fWinTrans, ref float fDist, ref float fEmis)
        {
            telnetCtrl.GetMeasurePara( ref fRefTemp,ref fEnvTemp,ref fRelHum,ref fWinTemp,ref fWinTrans,ref fDist,ref fEmis);
        }
        public void SetEmissivity(double emiss)    //范围0.00-1.0
        {
            Debug.WriteLine(string.Format("当前辐射率设置为:{0}", emiss));
        }

        public void GetEmissivity()
        {
            Debug.WriteLine(string.Format("当前辐射率获取..."));
        }

        public void SetRefTemp(double fValue)
        {
            Debug.WriteLine(string.Format("当前反射温度设置为:{0}", fValue));
        }
        public void GetRefTemp()
        {
            Debug.WriteLine(string.Format("当前反射温度获取"));
        }

        public void SetAirTemp(double fValue)
        {
            Debug.WriteLine(string.Format("当前空气温度设置为:{0}", fValue));
        }
        public void GetAirTemp()
        {
            Debug.WriteLine(string.Format("当前空气温度获取"));
        }

        public void SetAirHum(double fValue)
        {
            Debug.WriteLine(string.Format("当前空气湿度设置为:{0}", fValue));
        }
        public void GetAirHum()
        {
            Debug.WriteLine(string.Format("当前空气湿度获取"));
        }

        public void SetDistance(double fValue)
        {
            Debug.WriteLine(string.Format("当前距离设置为:{0}", fValue));
        }
        public void GetDistance()
        {
            Debug.WriteLine(string.Format("当前距离获取"));
        }

        public void SetWinTemp(double fValue)
        {
            Debug.WriteLine(string.Format("当前窗口温度设置为:{0}", fValue));
        }
        public void GetWinTemp()
        {
            Debug.WriteLine(string.Format("当前窗口温度获取"));
        }

        public void SetWinTrmRate(double fValue)
        {
            Debug.WriteLine(string.Format("当前窗口穿透率设置为:{0}", fValue));
        }
        public void GetWinTrmRate()
        {
            Debug.WriteLine(string.Format("当前窗口穿透率获取"));
        }

        public void GetAnaState()                   //type,0-无操作，1点温，2-线温，3-矩形，4-多边形，5-圆形; NO,表示序号
        {
            Debug.WriteLine(string.Format("当前测温状态获取:{0}", 0));
        }

        public void ClearAnaAll()
        {
           telnetCtrl.ClearScreen();
        }
        #endregion

        #region 点温相关
        public bool SetAnaSpotPos(int uNO, int SpotX, int SpotY,double fDist,double fEmiss)
        {           
            IRTelnetCtrl.SpotInfo stuSpotInfo = new IRTelnetCtrl.SpotInfo();
            stuSpotInfo.iSpotIndex =(short) uNO;
            stuSpotInfo.iSpotX = SpotX;
            stuSpotInfo.iSpotY = SpotY;
            stuSpotInfo.fDist = fDist;
            stuSpotInfo.fEmiss = fEmiss;
          
            if (telnetCtrl.SetSpotInfo(stuSpotInfo))
            {
                if (spotInfoList.Count == 0)
                {
                    spotInfoList.Add(stuSpotInfo);
                }
                else
                {
                    bool flag = false;
                    for (var i = 0; i < spotInfoList.Count; i++)
                    {
                        if (spotInfoList[i].iSpotIndex == stuSpotInfo.iSpotIndex)
                        {
                            spotInfoList[i] = stuSpotInfo;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        spotInfoList.Add(stuSpotInfo);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public IRTelnetCtrl.SpotInfo GetAnaSpotPos(int uNO)
        {
            IRTelnetCtrl.SpotInfo spotInfo = new IRTelnetCtrl.SpotInfo();
            if (spotInfoList.Count != 0)
            {
                for (var i = 0; i < spotInfoList.Count; i++)
                {
                    if(spotInfoList[i].iSpotIndex == uNO)
                    {
                        spotInfo = spotInfoList[i];                     
                    }
                }
            }
            return spotInfo;
            
        }

        public void SetAnaSpotParam(IRAnaParamItem paramObj)
        {
            Debug.WriteLine("点温设置相关参数为:{0}", paramObj);
        }
        public void GetAnaSpotParam(int uNO)
        {
            IRAnaParamItem paramObj = new IRAnaParamItem();
            paramObj.uNO = uNO;
            Debug.WriteLine("点温获取相关参数为:{0}", paramObj);
        }

        public void GetAnaSpotTemp(int uNO)
        {
            double temp = 0;
            telnetCtrl.GetSpotTempInfo(uNO, ref temp);
            Debug.WriteLine("当前{0}编号的点温为:{1}", uNO,temp);
        }
        #endregion
        #region 线温相关
        public void SetAnaLinePos(int uNO,int x1, int y1, int x2, int y2, double fDist, double fEmiss)
        {
            IRTelnetCtrl.LineInfo lineInfo = new IRTelnetCtrl.LineInfo();
            lineInfo.iLineIndex =(short) uNO;
            lineInfo.iLineX1 = x1;
            lineInfo.iLineY1 = y1;
            lineInfo.iLineX2 = x2;
            lineInfo.iLineY2 = y2;
            lineInfo.fDist = fDist;
            lineInfo.fEmiss = fEmiss;
            
            if (telnetCtrl.SetLineInfo(lineInfo))
            {
                if (lineInfoList.Count == 0)
                {
                    lineInfoList.Add(lineInfo);
                }
                else
                {
                    bool flag = false;
                    for (var i = 0; i < lineInfoList.Count; i++)
                    {
                        if (lineInfoList[i].iLineIndex == lineInfo.iLineIndex)
                        {
                            lineInfoList[i] = lineInfo;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        lineInfoList.Add(lineInfo);
                    }
                }
                Debug.WriteLine("编号为{0}的线温位置设置成功", uNO);
            }
            else
            {
                Debug.WriteLine("编号为{0}的线温位置设置失败", uNO);
            }
            
        }

        public void GetAnaLinePos(int uNO)
        {
            IRTelnetCtrl.LineInfo lineInfo;
            if (lineInfoList.Count != 0)
            {
                for (var i = 0; i < lineInfoList.Count; i++)
                {
                    if (lineInfoList[i].iLineIndex == uNO)
                    {
                        lineInfo = lineInfoList[i];
                        Debug.WriteLine(string.Format("获取编号为{0}的线温坐标位置参数:({1},{2}),({3},{4})", lineInfo.iLineIndex,lineInfo.iLineX1,lineInfo.iLineY1,lineInfo.iLineX2,lineInfo.iLineY2));
                        return;
                    }
                }
            }
        }

        public void SetAnaLineParam(IRAnaParamItem paramObj)
        {
            Debug.WriteLine("线温设置相关参数为:{0}", paramObj);
        }
        public void GetAnaLineParam(int uNO)
        {
            IRAnaParamItem paramObj = new IRAnaParamItem();
            paramObj.uNO = uNO;
            Debug.WriteLine("线温获取相关参数为:{0}", paramObj);
        }

        public void GetAnaLineTemp(int uNO)
        {
            double maxTemp = -500;
            double minTemp = -500;
            double avgTemp = -500;
            int  maxLineX = 0;
            int maxLineY = 0;


            telnetCtrl.GetLineMaxTemp(uNO, ref maxTemp);
            telnetCtrl.GetLineAvgTemp(uNO, ref minTemp);
            telnetCtrl.GetLineAvgTemp(uNO,ref avgTemp);
            telnetCtrl.GetLineMaxPosX(uNO, ref maxLineX);
            telnetCtrl.GetLineMaxPosY(uNO, ref maxLineY);
            Debug.WriteLine("当前{0}编号的线温最大温度:{1},最低温度:{2},平均温度:{3},最高温度x坐标:{4}最高温度y坐标:{5}", uNO,maxTemp,minTemp,avgTemp,maxLineX,maxLineY);

        }

        #endregion
        #region 矩形测温相关
        public void SetAnaAreaPos(int uNO,int fStartX,int fStartY,int fWidth, int fHeight, double fDist, double fEmiss)
        {
            IRTelnetCtrl.AreaInfo areaInfo = new IRTelnetCtrl.AreaInfo();
            areaInfo.iAreaIndex =(short) uNO;
            areaInfo.iAreaX = fStartX;
            areaInfo.iAreaY = fStartY;
            areaInfo.iAreaW = fWidth;
            areaInfo.iAreaH = fHeight;
            areaInfo.fDist = fDist;
            areaInfo.fEmiss = fEmiss;

            if (telnetCtrl.SetAreaInfo(areaInfo))
            {
                if (areaInfoList.Count == 0)
                {
                    areaInfoList.Add(areaInfo);
                }
                else
                {
                    bool flag = false;
                    for (var i = 0; i < areaInfoList.Count; i++)
                    {
                        if (areaInfoList[i].iAreaIndex == areaInfo.iAreaIndex)
                        {
                            areaInfoList[i] = areaInfo;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        areaInfoList.Add(areaInfo);
                    }
                }
                Debug.WriteLine("编号为{0}的区域测温设置成功", uNO);
            }
            else
            {
                Debug.WriteLine("编号为{0}的区域测温设置失败", uNO);
            }

            // Debug.WriteLine("编号为{0}的矩形位置参数为:{1},{2},{3},{4}", uNO, fStartXPer, fStartYPer, fToWidthPer, fHeightPer, );
        }
        public void GetAnaAreaPos(int uNO)
        {
            IRTelnetCtrl.AreaInfo areaInfo;
            if (areaInfoList.Count != 0)
            {
                for (var i = 0; i < areaInfoList.Count; i++)
                {
                    if (areaInfoList[i].iAreaIndex == uNO)
                    {
                        areaInfo = areaInfoList[i];
                        Debug.WriteLine(string.Format("获取编号为{0}的区域测温参数:x={1},y={2},w={3},h={4}", areaInfo.iAreaIndex, areaInfo.iAreaX, areaInfo.iAreaY,areaInfo.iAreaW,areaInfo.iAreaY));
                        return;
                    }
                }
            }
 
        }

        public void SetAnaAreaParam(IRAnaParamItem paramObj)
        {
            Debug.WriteLine("矩形测温设置相关参数为:{0}", paramObj);
        }
        public void GetAnaAreaParam(int uNO)
        {
            IRAnaParamItem paramObj = new IRAnaParamItem();
            paramObj.uNO = uNO;
            Debug.WriteLine("矩形测温获取相关参数为:{0}", paramObj);
        }

        public void GetAnaAreaTemp(int uNO)
        {
            double maxTemp = -500;
            double minTemp = -500;
            double avgTemp = -500;
            int maxAreaX = 0;
            int maxAreaY = 0;

            telnetCtrl.GetAreaAvgTemp(uNO, ref avgTemp);
            telnetCtrl.GetAreaMaxPosX(uNO, ref maxAreaX);
            telnetCtrl.GetAreaMaxPosY(uNO, ref maxAreaY);
            telnetCtrl.GetAreaMaxTemp(uNO, ref maxTemp);

            Debug.WriteLine("当前{0}编号的区域测温最大温度:{1},最低温度:{2},平均温度:{3},最高温度x坐标:{4}最高温度y坐标:{5}", uNO, maxTemp, minTemp, avgTemp, maxAreaX, maxAreaY);

        }

        #endregion
        #region 多边形测温相关
        public void SetAnaPolyPos(int uNO, Dictionary<string, int> dictionary)
        {
            Debug.WriteLine("编号为{0}的多边形测温参数设置为:{1}", uNO, dictionary);
        }
        public void GetAnaPolyPos(int uNO)
        {
            Debug.WriteLine("编号为{0}的多边形测温参数设置为:xxx", uNO);
        }

        public void SetAnaPolyParam(IRAnaParamItem paramObj)
        {
            Debug.WriteLine("多边形测温设置相关参数为:{0}", paramObj);
        }
        public void GetAnaPolyParam(int uNO)
        {
            IRAnaParamItem paramObj = new IRAnaParamItem();
            paramObj.uNO = uNO;
            Debug.WriteLine("编号为{0}多边形测温获取相关参数为:{1}", uNO, paramObj);
        }
        public void GetAnaPolyTemp(int uNO)
        {
            Debug.WriteLine("当前{0}编号的多边形测温温度为:xxx", uNO);

        }
        #endregion
        #region 圆形测温
        public void SetAnaCirclePos(int uNO, int fCenterLeftPer, int fCenterTopPer, int fRadiusWidthPer)
        {
            Debug.WriteLine("编号为{0}的圆形测温位置参数为:{1},{2},{3}", uNO, fCenterLeftPer, fCenterTopPer, fRadiusWidthPer);
        }

        public void GetAnaCirclePos(int uNO)
        {
            Debug.WriteLine("获取编号为{0}的圆形测温位置参数...", uNO);
        }

        public void SetAnaCircleParam(IRAnaParamItem paramObj)
        {
            Debug.WriteLine("圆形测温设置相关参数为:{0}", paramObj);
        }
        public void GetAnaCircleParam(int uNO)
        {
            IRAnaParamItem paramObj = new IRAnaParamItem();
            paramObj.uNO = uNO;
            Debug.WriteLine("圆形测温获取相关参数为:{0}", paramObj);
        }

        public void GetAnaCircleTemp(int uNO)
        {
            Debug.WriteLine("当前{0}编号的圆形测温为:xxx", uNO);

        }
        #endregion
        #endregion 
    }
}
