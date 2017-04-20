using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;

namespace XingKongFormGenerator
{
    public partial class Form1 : XingKongFormBase
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PortName = "COM4";

            XingKongWindow window = GetXingKongWindow();
            XingKongButton btApp = window.ControlsSet["btApp"] as XingKongButton;
            XingKongButton btPower = window.ControlsSet["btPower"] as XingKongButton;
            btApp.TextXoffset = -2;
            btPower.TextXoffset = -2;
            XingKongPanel.IsDebugEnabled = true;
            LocalShow();
            ShowCode();
        }

    }
}
