using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;

namespace XingKongFormGenerator
{
    public class XingKongFormBase : Form
    {
        public static string PortName;
        private Graphics graphics;
        private XingKongWindow window;

        protected XingKongWindow GetXingKongWindow(bool forceRefresh = false)
        {
            if (window == null || forceRefresh)
            {
                graphics = CreateGraphics();

                window = new XingKongWindow();
                window.Name = this.Name;
                foreach (var control in this.Controls)
                {
                    IDrawable xkControl = getXingKongControls(control);
                    if (xkControl != null)
                    {
                        window.AddChild(xkControl);
                    }
                }
            }
            return window;
        }

        private IDrawable getXingKongControls(object control)
        {
            if (control is Label)
            {
                Label l = control as Label;
                if (l.Visible)
                {
                    XingKongLabel xLabel = new XingKongLabel();
                    xLabel.Name = l.Name;
                    xLabel.Text = l.Text;
                    xLabel.Left = (short)l.Left;
                    xLabel.Top = (short)l.Top;
                    xLabel.FontSize = (XingKongScreen.XFontSize)getFontSizeIndex(l.Font);
                    return xLabel;
                }

            }
            else if (control is PictureBox)
            {
                PictureBox p = control as PictureBox;
                if (p.Visible)
                {
                    XingKongImageBox xImage = new XingKongImageBox();
                    xImage.Name = p.Name;
                    xImage.Left = p.Left;
                    xImage.Top = p.Top;
                    xImage.LoadPicture(new Bitmap(p.Image));
                    xImage.SkipPreProceed = false;
                    return xImage;
                }
            }
            else if (control is Button)
            {
                Button b = control as Button;
                if (b.Visible)
                {
                    XingKongButton xButton = new XingKongButton();
                    xButton.Name = b.Name;
                    xButton.FontSize = (XingKongScreen.XFontSize)getFontSizeIndex(b.Font);
                    xButton.Text = b.Text;
                    xButton.Left = b.Left;
                    xButton.Top = b.Top;
                    xButton.Width = b.Width;
                    xButton.Height = b.Height;
                    bool ischekced = false;
                    string tag = b.Tag as string;
                    if (tag != null && tag.ToLower().Equals("true"))
                    {
                        ischekced = true;
                    }
                    xButton.IsChecked = ischekced;
                    return xButton;
                }
            }
            else if (control is Panel)
            {
                Panel p = control as Panel;
                if (p.Visible)
                {
                    XingKongPanel xPanel = new XingKongPanel();
                    xPanel.Name = p.Name;
                    xPanel.Left = p.Left;
                    xPanel.Top = p.Top;
                    xPanel.Width = p.Width;
                    xPanel.Height = p.Height;

                    foreach (Control pcontrol in p.Controls)
                    {
                        IDrawable xkControl = getXingKongControls(pcontrol);
                        if (xkControl != null)
                        {
                            xkControl.setLeft(xkControl.getLeft() + xPanel.Left);
                            xkControl.setTop(xkControl.getTop() + xPanel.Top);
                            xPanel.AddChild(xkControl);
                        }
                    }

                    return xPanel;
                }
            }
            else if (control is ListBox)
            {
                ListBox l = control as ListBox;
                if (l.Visible)
                {
                    XingKongListBox xListBox = new XingKongListBox();
                    xListBox.setName(l.Name);
                    xListBox.setLeft(l.Left);
                    xListBox.setTop(l.Top);
                    xListBox.Width = l.Width;
                    xListBox.Height = l.Height;
                    xListBox.FontSize = (XingKongScreen.XFontSize)getFontSizeIndex(l.Font);
                    List<string> items = new List<string>();
                    foreach (string item in l.Items)
                    {
                        if (item != null)
                        {
                            items.Add(item);
                        }
                    }
                    xListBox.Items = items;
                    xListBox.SelectedIndex = l.SelectedIndex;
                    return xListBox;
                }
            }
            return null;
        }

        protected void LocalShow()
        {

            if (Program.Args != null && Program.Args.Length >= 2 && Program.Args[0].Equals("-ip"))
            {
                XingKongScreen.IsRemote = true;
                XingKongScreen.RemoteIp = Program.Args[1];
            }


            XingKongScreen.OpenScreen(PortName);

            XingKongScreen.ClearScreen();

            XingKongWindow window = GetXingKongWindow();
            window.Draw();

            XingKongScreen.FreshScreen();
            XingKongScreen.CloseScreen();
        }

        private int getFontSizeIndex(Font font)
        {
            SizeF sizeF = graphics.MeasureString("A", font);
            int height = (int)sizeF.Height;

            if (height <= 32)
            {
                return 1;
            }
            if (height > 32 && height <= 48)
            {
                return 2;
            }
            if (height > 48)
            {
                return 3;
            }
            return 1;
        }

        protected void ShowCode()
        {
            if (window == null)
            {
                GetXingKongWindow();
            }
            string json = JsonConvert.SerializeObject(window.GetEntity(), Formatting.Indented);
            TextForm f = new TextForm();
            f.ShowText(json);
            f.Show();
        }
    }
}
