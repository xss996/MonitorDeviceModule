namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class IRAnaParamItem
    {
        public int uNO { get; set; }
        public bool uActive { get; set; }
        public bool uUseLocal { get; set; }
        public double fEmiss { get; set; }
        public double fDis { get; set; }
        public string result { get; set; }

        public override string ToString()
        {
            return string.Format("IRAnaParamItem=[uNO={0},uActive ={1},uUseLocal={2},fEmiss={3},fDis={4}, result={5}]", uNO, uActive, uUseLocal, fEmiss, fDis, result);
        }

    }
}
