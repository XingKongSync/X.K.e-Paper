using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XingKongForm;

namespace Kuaidi
{
    class KuaidiInfoFormControlGroup
    {
        private XingKongLabel lbl1;
        private XingKongLabel lbl2;
        private XingKongLabel lbl3;
        private XingKongLabel lbr1;
        private XingKongLabel lbr2;

        private int index = 1;
        private bool visibility = true;

        private string time;
        private string msg;

        public bool Visibility
        {
            get
            {
                return visibility;
            }

            set
            {
                if (value)
                {
                    lbl1.Text = "|";
                    lbl2.Text = index == 1 ? "●" : "○";
                    lbl3.Text = "|";
                    lbr1.Text = time;
                    lbr2.Text = msg;
                }
                else
                {
                    lbl1.Text = string.Empty;
                    lbl2.Text = string.Empty;
                    lbl3.Text = string.Empty;
                    lbr1.Text = string.Empty;
                    lbr2.Text = string.Empty;
                }
            }
        }

        public string Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                if (Visibility)
                {
                    lbr1.Text = time;
                }
                else
                {
                    lbr1.Text = string.Empty;
                }
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
                if (visibility)
                {
                    lbr2.Text = msg;
                }
                else
                {
                    lbr2.Text = string.Empty;
                }
            }
        }

        public KuaidiInfoFormControlGroup(XingKongWindow kuaidiInfoWindow, int index)
        {
            lbl1 = kuaidiInfoWindow.ControlsSet["lbg" + index + "l1"] as XingKongLabel;
            lbl2 = kuaidiInfoWindow.ControlsSet["lbg" + index + "l2"] as XingKongLabel;
            lbl3 = kuaidiInfoWindow.ControlsSet["lbg" + index + "l3"] as XingKongLabel;
            lbr1 = kuaidiInfoWindow.ControlsSet["lbg" + index + "r1"] as XingKongLabel;
            lbr2 = kuaidiInfoWindow.ControlsSet["lbg" + index + "r2"] as XingKongLabel;

            LogData("group:" + index);
        }

        private void LogData(string data)
        {
            Console.Write("[Kuaidi]\t");
            Console.WriteLine(data);
        }

    }
}
