using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongApp;
using XingKongForm;

namespace Kuaidi
{
    public class Kuaidi : App
    {
        private string appPath;
        private XingKongWindow currentWindow;
        private XingKongWindow mainWindow;
        private XingKongWindow kuaidiInfoWindow;
        private Keyboard keyboard;

        private XingKongLabel lbTitle;
        private XingKongListBox lbKuaidis;
        private KuaidiInfoFormControlGroup g1;
        private KuaidiInfoFormControlGroup g2;
        private KuaidiInfoFormControlGroup g3;
        private KuaidiInfoFormControlGroup g4;
        private KuaidiInfoFormControlGroup[] infoGroup = new KuaidiInfoFormControlGroup[4];
        private XingKongLabel lbg1g2;
        private XingKongLabel lbg2g3;
        private XingKongLabel lbg3g4;
        private XingKongLabel lbInfoTitle;

        private List<KuaidiListItem> KuaidiItemList;

        private static string urlFormat = "https://www.kuaidi100.com/query?type={0}&postid={1}&id=1&valicode=&temp=0.21497602685806494";

        public override string GetName()
        {
            return "快递查询";
        }

        public override void Init()
        {
            appPath = Path.Combine("Apps", "Kuaidi");
            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += new Keyboard.KeyPressedHandler(Keyboard_KeyPressed);

            LogData("1");

            mainWindow = new XingKongWindow();
            XingKongWindow.Entity entity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(File.ReadAllText(Path.Combine(appPath, "KuaidiMainForm.json")));
            mainWindow.SetEntity(entity);
            lbTitle = mainWindow.ControlsSet["lbTitle"] as XingKongLabel;
            lbKuaidis = mainWindow.ControlsSet["lbKuaidis"] as XingKongListBox;

            ShowMainWindow();
            LogData("2");

            kuaidiInfoWindow = new XingKongWindow();
            XingKongWindow.Entity entity2 = JsonConvert.DeserializeObject<XingKongWindow.Entity>(File.ReadAllText(Path.Combine(appPath, "KuaidiInfoForm.json")));
            kuaidiInfoWindow.SetEntity(entity2);
            g1 = new KuaidiInfoFormControlGroup(kuaidiInfoWindow, 1);
            g2 = new KuaidiInfoFormControlGroup(kuaidiInfoWindow, 2);
            g3 = new KuaidiInfoFormControlGroup(kuaidiInfoWindow, 3);
            g4 = new KuaidiInfoFormControlGroup(kuaidiInfoWindow, 4);
            infoGroup[0] = g1;
            infoGroup[1] = g2;
            infoGroup[2] = g3;
            infoGroup[3] = g4;
            lbg1g2 = kuaidiInfoWindow.ControlsSet["lbg1g2"] as XingKongLabel;
            lbg2g3 = kuaidiInfoWindow.ControlsSet["lbg2g3"] as XingKongLabel;
            lbg3g4 = kuaidiInfoWindow.ControlsSet["lbg3g4"] as XingKongLabel;
            lbInfoTitle = kuaidiInfoWindow.ControlsSet["lbInfoTitle"] as XingKongLabel;

            LogData("Controls Reference Load Completed!");
        }

        private void Keyboard_KeyPressed(Keys pressedKey)
        {
            if (currentWindow == mainWindow)
            {
                switch (pressedKey)
                {
                    case Keys.Escape:
                        Suspend();
                        Quit();
                        break;
                    case Keys.Up:
                        lbKuaidis.SelectPrevious();
                        currentWindow.Draw();
                        XingKongScreen.FreshScreen();
                        break;
                    case Keys.Down:
                        lbKuaidis.SelectNext();
                        currentWindow.Draw();
                        XingKongScreen.FreshScreen();
                        break;
                    case Keys.Enter:
                        int selectedIndex = lbKuaidis.SelectedIndex;
                        ShowInfoWindow(KuaidiItemList[selectedIndex]);
                        break;
                    default:
                        break;
                }
            }
            else if (currentWindow == kuaidiInfoWindow)
            {
                switch (pressedKey)
                {
                    case Keys.Escape:
                        ShowMainWindow();
                        break;
                    default:
                        break;
                }
            }
        }

        private void LogData(string data)
        {
            Console.Write("[Kuaidi]\t");
            Console.WriteLine(data);
        }

        public override void Resume()
        {
            this.keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
        }

        public override void Suspend()
        {
            this.keyboard.KeyPressed -= new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
        }

        private void ShowMainWindow()
        {
            currentWindow = mainWindow;
            List<string> kuaidiListSource = new List<string>();
            KuaidiItemList = GetKuaidiList();
            foreach (var item in KuaidiItemList)
            {
                kuaidiListSource.Add(item.DisplayName);
            }
            lbKuaidis.Items = kuaidiListSource;
            lbKuaidis.Refresh();

            XingKongScreen.ClearScreen();
            currentWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
        }

        private List<KuaidiListItem> GetKuaidiList()
        {
            List<KuaidiListItem> kuaidiList = new List<KuaidiListItem>();
            string kuaidiPath = Path.Combine(appPath, "kuaidi.txt");
            if (File.Exists(kuaidiPath))
            {
                string[] lines = File.ReadAllLines(kuaidiPath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (fields.Length == 4)
                        {
                            KuaidiListItem item = new KuaidiListItem();
                            item.DisplayName = fields[0];
                            item.ComPyName = fields[1];
                            item.ComCnName = fields[2];
                            item.KdNumber = fields[3];
                            kuaidiList.Add(item);
                        }
                    }
                }
            }
            return kuaidiList;
        }

        private void ShowInfoWindow(KuaidiListItem selectedItem)
        {
            currentWindow = kuaidiInfoWindow;
            LogData(selectedItem.ToString());
            try
            {
                string url = string.Format(urlFormat, selectedItem.ComPyName, selectedItem.KdNumber);
                string responseStr = XingKongUtils.HttpUtils.Get(url);
                LogData(responseStr);
                //Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
                //responseStr = reg.Replace(responseStr, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

                EntityHelper.ResponseEntity response = JsonConvert.DeserializeObject<EntityHelper.ResponseEntity>(responseStr);

                lbInfoTitle.Text = selectedItem.ComCnName + " " + selectedItem.KdNumber;
                if (response.data != null && response.data.Count > 0)
                {
                    ShowDashedLineByCount(response.data.Count);
                    for (int i = 0; i < 4; i++)
                    {
                        if (i + 1 <= response.data.Count)
                        {
                            LogData(string.Format("i={0}, desc={1}, time={2}", i, response.data[i].context, response.data[i].time));
                            infoGroup[i].Msg = response.data[i].context;
                            infoGroup[i].Time = response.data[i].time;
                            infoGroup[i].Visibility = true;
                        }
                        else
                        {
                            infoGroup[i].Visibility = false;
                        }
                    }
                }
                else
                {
                    g1.Visibility = true;
                    g2.Visibility = false;
                    g3.Visibility = false;
                    g4.Visibility = false;
                    g1.Msg = response.message + "";
                    g1.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                lbInfoTitle.Text = "错误";
                g1.Visibility = false;
                g2.Visibility = false;
                g3.Visibility = false;
                g4.Visibility = false;
                LogData(ex.Message);
            }


            XingKongScreen.ClearScreen();
            currentWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
        }

        private void ShowDashedLineByCount(int count)
        {
            if (count >= 4)
            {
                lbg1g2.Text = "|";
                lbg2g3.Text = "|";
                lbg3g4.Text = "|";
            }
            else if (count == 3)
            {
                lbg1g2.Text = "|";
                lbg2g3.Text = "|";
                lbg3g4.Text = "";
            }
            else if (count == 2)
            {
                lbg1g2.Text = "|";
                lbg2g3.Text = "";
                lbg3g4.Text = "";
            }

            else if (count == 1)
            {
                lbg1g2.Text = "";
                lbg2g3.Text = "";
                lbg3g4.Text = "";
            }
        }
    }
}
