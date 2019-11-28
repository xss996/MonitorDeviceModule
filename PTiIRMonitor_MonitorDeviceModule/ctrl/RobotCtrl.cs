using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
   public class RobotCtrl
    {
        public void GetWorkState()
        {
            Debug.WriteLine("机器人当前工作状态:xxx");
        }

        public void Stop(bool flag)
        {
            Debug.WriteLine("机器人急停...");
        }

        public void Move(double iForVelo,double iRotaVelo)
        {
            Debug.WriteLine("机器人移动设置:{0},{1}", iForVelo, iRotaVelo);
        }

        public void GetCurrentPos()
        {
            Debug.WriteLine("获取机器人当前位置...");
        }

        public void SetManualMovePosParam(double fRefPosX,double fRefPosY,double fAbsAngle)
        {
            Debug.WriteLine("机器人当前位置参数:{0},{1},{2}", fRefPosX, fRefPosY, fAbsAngle);
        }

        public void SetMoveSpePos(string posType)
        {
            Debug.WriteLine("机器人当前特定位模式:{0}",posType);
        }

        public void SetPower(bool flag)
        {
            if (flag)
            {
                Debug.WriteLine("机器人充电开关打开");
            }
            else
            {
                Debug.WriteLine("机器人充电开关关闭");
            }
        }

    }
}
