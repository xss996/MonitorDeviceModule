using PTiIRMonitor_MonitorDeviceModule.ctrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public partial class TestForm : Form
    {
        GlobalCtrl globalCtrl = new GlobalCtrl();
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            globalCtrl.createCrusieThread();
        }
    }
}
