using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XingKongApp;
using XingKongForm;
using XingKongUtils;

namespace TpLinkNetMonitor
{
    public class TpLinkNetMonitor : App
    {
        private string appPath;
        private XingKongWindow currentWindow;
        private XingKongLabel lbGlobalDownload;
        private XingKongLabel lbGlobalUpload;
        private Keyboard keyboard;

        /* Area0 */
        private XingKongLabel lbArea0PcName;
        private XingKongLabel lbArea0Ip;
        private XingKongLabel lbArea0Up;
        private XingKongLabel lbArea0Down;

        /* Area1 */
        private XingKongLabel lbArea1PcName;
        private XingKongLabel lbArea1Ip;
        private XingKongLabel lbArea1Up;
        private XingKongLabel lbArea1Down;

        /* Area2 */
        private XingKongLabel lbArea2PcName;
        private XingKongLabel lbArea2Ip;
        private XingKongLabel lbArea2Up;
        private XingKongLabel lbArea2Down;

        /* Area3 */
        private XingKongLabel lbArea3PcName;
        private XingKongLabel lbArea3Ip;
        private XingKongLabel lbArea3Up;
        private XingKongLabel lbArea3Down;

        private XingKongLabel lbPage;
        private XingKongButton btPrev;
        private XingKongButton btNext;

        private System.Timers.Timer timer;

        private int buttonIndex;
        private string token;
        private string request_url;
        private const string wan_status_args = "{\"network\":{\"name\":[\"wan_status\"]},\"method\":\"get\"}";
        private const string hosts_info_args = "{\"hosts_info\":{\"table\":\"online_host\"},\"method\":\"get\"}";
        private int upSpeed;
        private int downSpeed;
        private bool isUpdating = false;//标记是否正在刷新内容
        private PageManager pageManager;
        private int pageIndex = 0;
        private int pageCount = 0;
        private int remaningSeconds = 7;//每7秒刷新一次

        public override string GetName()
        {
            return "TP-Link流量监控";
        }

        public override void Init()
        {
            appPath = Path.Combine("Apps", "TpLinkNetMonitor");
            currentWindow = new XingKongWindow();
            XingKongWindow.Entity entity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(File.ReadAllText(Path.Combine(this.appPath, "MainForm.json")));
            currentWindow.SetEntity(entity);

            XingKongPanel plGlobal = currentWindow.ControlsSet["plGlobal"] as XingKongPanel;
            lbGlobalDownload = plGlobal.ControlsSet["lbGlobalDownload"] as XingKongLabel;
            lbGlobalUpload = plGlobal.ControlsSet["lbGlobalUpload"] as XingKongLabel;

            XingKongPanel plArea0 = currentWindow.ControlsSet["plArea0"] as XingKongPanel;
            lbArea0PcName = plArea0.ControlsSet["lbArea0PcName"] as XingKongLabel;
            lbArea0Ip = plArea0.ControlsSet["lbArea0Ip"] as XingKongLabel;
            lbArea0Up = plArea0.ControlsSet["lbArea0Up"] as XingKongLabel;
            lbArea0Down = plArea0.ControlsSet["lbArea0Down"] as XingKongLabel;

            XingKongPanel plArea1 = currentWindow.ControlsSet["plArea1"] as XingKongPanel;
            lbArea1PcName = plArea1.ControlsSet["lbArea1PcName"] as XingKongLabel;
            lbArea1Ip = plArea1.ControlsSet["lbArea1Ip"] as XingKongLabel;
            lbArea1Up = plArea1.ControlsSet["lbArea1Up"] as XingKongLabel;
            lbArea1Down = plArea1.ControlsSet["lbArea1Down"] as XingKongLabel;

            XingKongPanel plArea2 = currentWindow.ControlsSet["plArea2"] as XingKongPanel;
            lbArea2PcName = plArea2.ControlsSet["lbArea2PcName"] as XingKongLabel;
            lbArea2Ip = plArea2.ControlsSet["lbArea2Ip"] as XingKongLabel;
            lbArea2Up = plArea2.ControlsSet["lbArea2Up"] as XingKongLabel;
            lbArea2Down = plArea2.ControlsSet["lbArea2Down"] as XingKongLabel;

            XingKongPanel plArea3 = currentWindow.ControlsSet["plArea3"] as XingKongPanel;
            lbArea3PcName = plArea3.ControlsSet["lbArea3PcName"] as XingKongLabel;
            lbArea3Ip = plArea3.ControlsSet["lbArea3Ip"] as XingKongLabel;
            lbArea3Up = plArea3.ControlsSet["lbArea3Up"] as XingKongLabel;
            lbArea3Down = plArea3.ControlsSet["lbArea3Down"] as XingKongLabel;

            XingKongPanel plPage = currentWindow.ControlsSet["plPage"] as XingKongPanel;
            lbPage = plPage.ControlsSet["lbPage"] as XingKongLabel;
            btPrev = currentWindow.ControlsSet["btPrev"] as XingKongButton;
            btNext = currentWindow.ControlsSet["btNext"] as XingKongButton;

            //显示初始内容
            lbGlobalDownload.Text = "下载：N/A";
            lbGlobalUpload.Text = "上传：N/A";
            for (int i = 0; i < 4; i++)
            {
                setAreaContent(i, null);
            }
            lbPage.Text = "退出：Esc";

            pageManager = new PageManager();

            XingKongScreen.ClearScreen();
            currentWindow.HardworkDraw();
            XingKongScreen.FreshScreen();

            //登陆并启动定时刷新计时器
            if (Login(out token))
            {
                LogData("登陆成功，token:" + token);

                request_url = string.Format("http://192.168.0.1/stok={0}/ds", token);

                if (timer == null)
                {
                    timer = new System.Timers.Timer();
                    timer.Interval = 1000;
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                }
            }
            else
            {
                LogData("登陆失败");
                XingKongMessageBox msgBox = new XingKongMessageBox();
                msgBox.Title = "登陆TP-Link后台失败";
                msgBox.Caption = "提示";
                msgBox.DialogStyle = XingKongMessageBox.Style.OK;
                XingKongMessageBox.DialogResult result = msgBox.ShowAsync().Result;
                LogData("user click : " + result.ToString());
                Quit();
            }

            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            remaningSeconds--;
            if (remaningSeconds < 0)
            {
                //重置剩余刷新时间
                remaningSeconds = 7;

                if (isUpdating)
                {
                    return;
                }
                isUpdating = true;
                try
                {
                    bool needDraw = false;

                    //获取全局速度
                    string response = HttpUtils.Post(request_url, wan_status_args, HttpUtils.RequestType.Raw);
                    EntityHelper.WanStatusReponseEntity wanStatusEntity = JsonConvert.DeserializeObject<EntityHelper.WanStatusReponseEntity>(response);
                    if (upSpeed != wanStatusEntity.network.wan_status.up_speed || downSpeed != wanStatusEntity.network.wan_status.down_speed)
                    {
                        lbGlobalDownload.Text = string.Format("下载：{0}KB/s", wanStatusEntity.network.wan_status.down_speed);
                        lbGlobalUpload.Text = string.Format("上传：{0}KB/s", wanStatusEntity.network.wan_status.up_speed);

                        needDraw = true;
                        upSpeed = wanStatusEntity.network.wan_status.up_speed;
                        downSpeed = wanStatusEntity.network.wan_status.down_speed;
                    }

                    //获取各个机器的速度
                    response = HttpUtils.Post(request_url, hosts_info_args, HttpUtils.RequestType.Raw);
                    List<EntityHelper.HostInfoEntity> hostsInfo = EntityHelper.AnalysHostInfoJson(response);
                    pageManager.SetDataSource(hostsInfo);
                    pageCount = pageManager.GetPageCount();
                    if (pageCount > 0)
                    {
                        lbPage.Text = string.Format("第{0}页，共{1}页", pageIndex + 1, pageCount);
                        List<EntityHelper.HostInfoEntity> page = pageManager.GetPageByIndex(pageIndex);
                        for (int i = 0; i < 4; i++)
                        {
                            if (i > (page.Count - 1))
                            {
                                setAreaContent(i, null);
                            }
                            else
                            {
                                setAreaContent(i, page[i]);
                            }
                        }
                    }
                    else
                    {
                        lbPage.Text = "路由器智障了(╯‵□′)╯︵┻━┻";
                    }

                    if (needDraw)
                    {
                        currentWindow.Draw();
                        XingKongScreen.FreshScreen();
                    }
                }
                catch (Exception ex)
                {
                    LogData(ex.Message);
                    LogData(ex.StackTrace);
                }
                isUpdating = false;
            }

            
        }

        private void Keyboard_KeyPressed(System.Windows.Forms.Keys pressedKey)
        {
            switch (pressedKey)
            {
                case System.Windows.Forms.Keys.Left:
                    buttonIndex = (--buttonIndex % 3);
                    break;
                case System.Windows.Forms.Keys.Right:
                    buttonIndex = (++buttonIndex % 3);
                    break;
                default:
                    break;
            }
            if (buttonIndex < 0)
            {
                buttonIndex += 3;
            }

            switch (pressedKey)
            {
                case System.Windows.Forms.Keys.Left:
                case System.Windows.Forms.Keys.Right:
                    switchButton(buttonIndex);
                    XingKongScreen.FreshScreen();
                    //防止过多刷新导致眼花……，重置剩余刷新时间
                    remaningSeconds = 7;
                    break;
                case System.Windows.Forms.Keys.Enter:
                    if (btNext.IsChecked)
                    {
                        pageIndex++;
                        pageIndex = pageIndex % pageCount;
                    }
                    else if (btPrev.IsChecked)
                    {
                        pageIndex--;
                        if (pageIndex < 0)
                        {
                            pageIndex += pageCount;
                        }
                    }
                    LogData("PageIndex: " + pageIndex);
                    //切换页
                    List<EntityHelper.HostInfoEntity> page = pageManager.GetPageByIndex(pageIndex);
                    for (int i = 0; i < 4; i++)
                    {
                        if (i > (page.Count - 1))
                        {
                            setAreaContent(i, null);
                        }
                        else
                        {
                            setAreaContent(i, page[i]);
                        }
                    }
                    //防止过多刷新导致眼花……，重置剩余刷新时间
                    remaningSeconds = 7;
                    currentWindow.Draw();
                    XingKongScreen.FreshScreen();
                    break;
                case System.Windows.Forms.Keys.Escape:
                    Suspend();
                    Quit();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 登陆TP-Link后台
        /// </summary>
        /// <param name="token">登陆成功的token</param>
        /// <returns>是否登陆成功</returns>
        private bool Login(out string token)
        {
            //请求登陆的url
            string url = @"http://192.168.0.1/";
            //请求登陆的字符串
            string payload = "{\"method\":\"do\",\"login\":{\"password\":\"WaQ7xPr3L41vMwK\"}}";
            //获得服务器响应
            try
            {
                string response = HttpUtils.Post(url, payload, HttpUtils.RequestType.Raw);
                EntityHelper.LoginResponseEntity loginResponseEntity = JsonConvert.DeserializeObject<EntityHelper.LoginResponseEntity>(response);
                if (loginResponseEntity.error_code.Equals("0"))
                {
                    token = loginResponseEntity.stok;
                    return true;
                }
                else
                {
                    token = "";
                    LogData(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogData("登录失败：\r\n" + ex.Message);
                token = "";
                return false;
            }

        }

        public override void Resume()
        {
            timer.Start();
            this.keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
        }

        public override void Suspend()
        {
            timer.Stop();
            this.keyboard.KeyPressed -= new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
        }

        private void setAreaContent(int areaIndex, EntityHelper.HostInfoEntity hostInfo)
        {
            XingKongLabel lbPcName;
            XingKongLabel lbIp;
            XingKongLabel lbUp;
            XingKongLabel lbDown;

            switch (areaIndex)
            {
                case 0:
                    lbPcName = lbArea0PcName;
                    lbIp = lbArea0Ip;
                    lbUp = lbArea0Up;
                    lbDown = lbArea0Down;
                    break;
                case 1:
                    lbPcName = lbArea1PcName;
                    lbIp = lbArea1Ip;
                    lbUp = lbArea1Up;
                    lbDown = lbArea1Down;
                    break;
                case 2:
                    lbPcName = lbArea2PcName;
                    lbIp = lbArea2Ip;
                    lbUp = lbArea2Up;
                    lbDown = lbArea2Down;
                    break;
                case 3:
                    lbPcName = lbArea3PcName;
                    lbIp = lbArea3Ip;
                    lbUp = lbArea3Up;
                    lbDown = lbArea3Down;
                    break;
                default:
                    return;
            }

            if (hostInfo == null)
            {
                lbPcName.Text = "";
                lbIp.Text = "";
                lbUp.Text = "";
                lbDown.Text = "";
            }
            else
            {
                string hostName = hostInfo.hostname + "";
                if (string.IsNullOrWhiteSpace(hostName))
                {
                    hostName = "匿名主机";
                }

                lbPcName.Text = hostName;
                lbIp.Text = "" + hostInfo.ip;
                lbUp.Text = string.Format("上传：{0}", FormatNetSpeed("" + hostInfo.up_speed));
                lbDown.Text = string.Format("下载：{0}", FormatNetSpeed("" + hostInfo.down_speed));
            }
        }

        private string FormatNetSpeed(string speed)
        {
            try
            {
                float iSpeed = float.Parse(speed) / 1024;//kb/s
                if (iSpeed > 1024)
                {
                    //换算为mb/s
                    iSpeed = iSpeed / 1024;
                    return string.Format("{0:N2}MB/s", iSpeed);
                }
                else
                {
                    return string.Format("{0:N2}KB/s", iSpeed);
                }
            }
            catch (Exception ex)
            {
                LogData("[FormatNetSpeed]\t" + ex.Message);
                return "";
            }
        }

        private void switchButton(int btIndex)
        {
            switch (btIndex)
            {
                case 0:
                    btPrev.IsChecked = false;
                    btNext.IsChecked = false;
                    break;
                case 1:
                    btPrev.IsChecked = true;
                    btNext.IsChecked = false;
                    break;
                case 2:
                    btPrev.IsChecked = false;
                    btNext.IsChecked = true;
                    break;
                default:
                    break;
            }
            currentWindow.Draw();
        }

        private void LogData(string data)
        {
            Console.WriteLine("[TpLinkNetMonitor]\t");
            Console.WriteLine(data);
        }
    }
}
