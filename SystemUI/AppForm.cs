using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;

namespace XingKongApp
{
    public class AppForm
    {
        private List<AppInfo> apps;
        public Action<string> LoadAppAction;

        private XingKongWindow thisWindow;
        private XingKongButton btLaunch;
        private XingKongButton btBack;
        private XingKongImageBox pbIcon;
        private XingKongLabel lbAllApps;
        private XingKongListBox lbApps;

        private Keyboard keyboard;

        private bool isListHasFocus = true;

        public Action QuitAction;

        public void Init()
        {
            thisWindow = new XingKongWindow();
            string shutdownWindowJson = File.ReadAllText(@"AppForm.json");
            XingKongWindow.Entity windowEntity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(shutdownWindowJson);
            thisWindow.SetEntity(windowEntity);

            btLaunch = thisWindow.ControlsSet["btLaunch"] as XingKongButton;
            btBack = thisWindow.ControlsSet["btBack"] as XingKongButton;
            pbIcon = thisWindow.ControlsSet["pbIcon"] as XingKongImageBox;
            lbAllApps = thisWindow.ControlsSet["lbAllApps"] as XingKongLabel;
            lbApps = thisWindow.ControlsSet["lbApps"] as XingKongListBox;

            lbApps.Refresh();

            pbIcon.SkipPreProceed = true;
            pbIcon.LoadPicture(Path.Combine("SystemUI", "appstore.png"));

            apps = AppFinder.GetAppList();
            if ((apps == null) || (apps.Count == 0))
            {
                XingKongMessageBox box1 = new XingKongMessageBox
                {
                    Caption = "提示",
                    Title = "然而一个应用都没有。"
                };
                box1.btOk.IsChecked = true;
                box1.DialogStyle = XingKongMessageBox.Style.OK;
                Console.WriteLine("user clicked: " + box1.ShowAsync().Result);
            }
            else
            {
                if (lbApps.Items == null)
                {
                    lbApps.Items = new List<string>();
                }
                foreach (AppInfo info in apps)
                {
                    lbApps.Items.Add(info.AppName);
                }
                lbApps.Refresh();
            }
            XingKongScreen.ClearScreen();
            thisWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += new Keyboard.KeyPressedHandler(Keyboard_KeyPressed);
        }

        private void switchFocus()
        {
            if (isListHasFocus)
            {
                btBack.IsChecked = false;
                btLaunch.IsChecked = false;
            }
            else
            {
                btLaunch.IsChecked = true;
            }
        }

        private void Keyboard_KeyPressed(System.Windows.Forms.Keys pressedKey)
        {
            if (pressedKey == Keys.Left)
            {
                isListHasFocus = true;
                switchFocus();
            }
            else if (pressedKey == Keys.Right)
            {
                isListHasFocus = false;
                switchFocus();
            }
            else if (pressedKey == Keys.Up)
            {
                if (isListHasFocus)
                {
                    lbApps.SelectPrevious();
                }
                else
                {
                    btLaunch.IsChecked = true;
                    btBack.IsChecked = false;
                }
            }
            else if (pressedKey == Keys.Down)
            {
                if (isListHasFocus)
                {
                    lbApps.SelectNext();
                }
                else
                {
                    btLaunch.IsChecked = false;
                    btBack.IsChecked = true;
                }
            }
            else if (pressedKey == Keys.Enter)
            {
                if (!isListHasFocus)
                {
                    if (btBack.IsChecked)
                    {
                        Suspend();
                        QuitAction?.Invoke();
                        return;
                    }
                    if ((btLaunch.IsChecked && (lbApps.SelectedIndex >= 0)) && (lbApps.SelectedIndex <= apps.Count))
                    {
                        Suspend();
                        LoadAppAction?.Invoke(apps[lbApps.SelectedIndex].MoudlePath);
                        return;
                    }
                }
                else if ((lbApps.SelectedIndex >= 0) && (lbApps.SelectedIndex <= apps.Count))
                {
                    Suspend();
                    Console.WriteLine(apps[lbApps.SelectedIndex].MoudlePath);
                    LoadAppAction?.Invoke(apps[lbApps.SelectedIndex].MoudlePath);
                    return;
                }
            }
            thisWindow.Draw();
            XingKongScreen.FreshScreen();
        }

        public void Suspend()
        {
            keyboard.KeyPressed -= Keyboard_KeyPressed;
        }

        public void Resume()
        {
            keyboard.KeyPressed += Keyboard_KeyPressed;
        }

    }
}
