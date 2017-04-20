using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;

namespace XingKongFormGenerator
{
    public partial class KuaidiInfoForm : XingKongFormBase
    {
        public KuaidiInfoForm()
        {
            InitializeComponent();
        }

        private void KuaidiInfoForm_Load(object sender, EventArgs e)
        {
            PortName = "COM4";

            XingKongWindow window = GetXingKongWindow();
            LocalShow();
            ShowCode();
        }
    }
}
