using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XingKongForm;

namespace XingKongApp
{
    public class SystemUI : App
    {
        private const string TodayWeatherUrl = @"http://api.k780.com/?app=weather.today&weaid=101190405&appkey=21431&sign=d47edad482fa3dccb030628ec922f0e3&format=json";
        private const string FutureWeatherUrl = @"http://api.k780.com/?app=weather.future&weaid=101190405&appkey=21431&sign=d47edad482fa3dccb030628ec922f0e3&format=json";
        private const string vbDllPath = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\Microsoft.VisualBasic.dll";

        private XingKongWindow currentForm;

        private XingKongLabel lbWeek;//星期
        private XingKongLabel lbDate;//日期
        private XingKongImageBox pbM1;//分钟第1位
        private XingKongImageBox pbM2;//分钟第2位
        private XingKongImageBox pbS;//冒号
        private XingKongImageBox pbH1;//小时第1位
        private XingKongImageBox pbH2;//小时第2位

        private XingKongImageBox pbWeatherIcon;//天气图标
        private XingKongLabel lbCity;//城市
        private XingKongLabel lbWeather;//总体天气
        private XingKongLabel lbTemp;//总体温度
        private XingKongLabel lbWind;//风力
        private XingKongLabel lbTempCurrDesc;//“实时温度”的文本
        private XingKongLabel lbTempCurr;//实时温度

        private XingKongLabel lbTomoWeatherAndWind;//明日天气和风向
        private XingKongLabel lbTomoTemp;//明日气温

        private XingKongLabel lbIp;//本机IP
        private XingKongLabel lbCpuStatus;//CPU占用和温度
        private XingKongLabel lbRamStatus;//内存占用

        private XingKongImageBox pbPower;//电源图标
        private XingKongImageBox pbApp;//应用图标
        private XingKongButton btPower;//电源按钮
        private XingKongButton btApp;//应用按钮
        private int buttonIndex;//当前选中的按钮，0表示未选中，1表示选中应用按钮，2表示选中电源按钮

        private System.Timers.Timer autoSyncMiniuteTimer;//让autoFreshTimer与分钟同步的计时器
        private System.Timers.Timer autoFreshTimer;
        private System.Timers.Timer autoWeatherTimer;

        private PerformanceCounter cpuCounter;//cpu性能计数器 .NET Mono通用
        private XingKongForm.Keyboard keyboard;

        /// <summary>
        /// 显示启动界面
        /// </summary>
        private void showStartingInterface()
        {
            string startingWindowJson = File.ReadAllText(@"StartingForm.json");
            XingKongWindow.Entity windowEntity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(startingWindowJson);
            XingKongWindow loadingWindow = new XingKongWindow();
            loadingWindow.SetEntity(windowEntity);
            XingKongImageBox pbLogo = loadingWindow.ControlsSet["pbLogo"] as XingKongImageBox;
            XingKongImageBox pbCopyright = loadingWindow.ControlsSet["pbCopyright"] as XingKongImageBox;
            pbLogo.SkipPreProceed = true;
            pbCopyright.SkipPreProceed = true;
            pbLogo.LoadPicture(Path.Combine("SystemUI", "Logo_cut_1.bmp"));
            pbCopyright.LoadPicture(Path.Combine("SystemUI", "Logo_cut_2.bmp"));
            loadingWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
            XingKongScreen.ClearScreen();
        }

        public override void Init()
        {
            //先显示启动画面
            showStartingInterface();

            if (!XingKongScreen.IsRunningOnMono())
            {
                Thread.Sleep(1000);
            }

            string appLoaderWindowJson = File.ReadAllText(@"MainForm.json");
            XingKongWindow.Entity windowEntity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(appLoaderWindowJson);
            currentForm = new XingKongWindow();
            currentForm.SetEntity(windowEntity);

            #region 初始化日期控件
            XingKongPanel panel1 = currentForm.ControlsSet["panel1"] as XingKongPanel;
            lbWeek = panel1.ControlsSet["lbWeek"] as XingKongLabel;
            lbDate = panel1.ControlsSet["lbDate"] as XingKongLabel;
            pbM1 = currentForm.ControlsSet["pbM1"] as XingKongImageBox;
            pbM2 = currentForm.ControlsSet["pbM2"] as XingKongImageBox;
            pbS = currentForm.ControlsSet["pbS"] as XingKongImageBox;
            pbH1 = currentForm.ControlsSet["pbH1"] as XingKongImageBox;
            pbH2 = currentForm.ControlsSet["pbH2"] as XingKongImageBox;

            pbM1.SkipPreProceed = true;
            pbM2.SkipPreProceed = true;
            pbS.SkipPreProceed = true;
            pbH1.SkipPreProceed = true;
            pbH2.SkipPreProceed = true;

            pbS.LoadPicture(getNumPic(':'));
            #endregion

            #region 初始化天气控件
            XingKongPanel panel2 = currentForm.ControlsSet["panel2"] as XingKongPanel;
            XingKongPanel panel3 = currentForm.ControlsSet["panel3"] as XingKongPanel;
            pbWeatherIcon = currentForm.ControlsSet["pbWeatherIcon"] as XingKongImageBox;
            lbCity = panel2.ControlsSet["lbCity"] as XingKongLabel;
            lbWeather = panel2.ControlsSet["lbWeather"] as XingKongLabel;
            lbTemp = panel2.ControlsSet["lbTemp"] as XingKongLabel;
            lbWind = panel2.ControlsSet["lbWind"] as XingKongLabel;
            lbTempCurrDesc = panel3.ControlsSet["lbTempCurrDesc"] as XingKongLabel;
            lbTempCurr = panel3.ControlsSet["lbTempCurr"] as XingKongLabel;

            pbWeatherIcon.SkipPreProceed = true;

            lbTomoWeatherAndWind = panel2.ControlsSet["lbTomoWeatherAndWind"] as XingKongLabel;
            lbTomoTemp = panel2.ControlsSet["lbTomoTemp"] as XingKongLabel;
            #endregion

            #region 初始化系统信息控件
            XingKongPanel panel4 = currentForm.ControlsSet["panel4"] as XingKongPanel;
            lbIp = panel4.ControlsSet["lbIp"] as XingKongLabel;
            lbCpuStatus = panel4.ControlsSet["lbCpuStatus"] as XingKongLabel;
            lbRamStatus = panel4.ControlsSet["lbRamStatus"] as XingKongLabel;
            #endregion

            #region 初始化按钮控件
            pbPower = currentForm.ControlsSet["pbPower"] as XingKongImageBox;
            pbApp = currentForm.ControlsSet["pbApp"] as XingKongImageBox;
            btPower = currentForm.ControlsSet["btPower"] as XingKongButton;
            btApp = currentForm.ControlsSet["btApp"] as XingKongButton;

            pbPower.SkipPreProceed = true;
            pbApp.SkipPreProceed = true;

            pbPower.LoadPicture(Path.Combine("SystemUI", "POWER.BMP"));
            pbApp.LoadPicture(Path.Combine("SystemUI", "APP.BMP"));
            if (XingKongScreen.IsRunningOnMono())
            {
                btPower.TextXoffset = -9;
                btApp.TextXoffset = -9;
            }
            else
            {
                btPower.TextXoffset = -2;
                btApp.TextXoffset = -2;
            }
            #endregion

            UpdateIp();
            UpdateTime();
            UpdateSystemInfo();
            UpdateWeather();
            currentForm.HardworkDraw();

            XingKongScreen.FreshScreen();

            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += Keyboard_KeyPressed;

            autoSyncMiniuteTimer = new System.Timers.Timer();//同步时钟间隔
            autoSyncMiniuteTimer.Elapsed += AutoSyncMiniuteTimer_Elapsed;
            SyncMiniuteTimer();

            autoFreshTimer = new System.Timers.Timer();
            autoFreshTimer.Interval = 1000 * 60;//一分钟刷新一次
            autoFreshTimer.Elapsed += AutoFreshTimer_Elapsed;

            autoWeatherTimer = new System.Timers.Timer();
            autoWeatherTimer.Interval = 1000 * 60 * 10;//十分钟刷新一次
            autoWeatherTimer.Elapsed += AutoWeatherTimer_Elapsed;
            autoWeatherTimer.Enabled = true;
        }

        private void SyncMiniuteTimer()
        {
            autoSyncMiniuteTimer.Interval = 1000 * (60 - DateTime.Now.Second ) + 500;
            autoSyncMiniuteTimer.Enabled = true;
        }

        private void AutoSyncMiniuteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            autoSyncMiniuteTimer.Enabled = false;
            autoFreshTimer.Enabled = true;
            AutoFreshTimer_Elapsed(null, null);
        }

        private void AutoWeatherTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateWeather();
        }

        private void AutoFreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            autoFreshTimer.Enabled = false;
            UpdateIp();
            UpdateTime();
            UpdateSystemInfo();
            currentForm.Draw();
            XingKongScreen.FreshScreen();
            SyncMiniuteTimer();//重新校时
        }

        private void UpdateTime()
        {
            DateTime now = DateTime.Now;
            switch (now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    lbWeek.Text = "星期日";
                    break;
                case DayOfWeek.Monday:
                    lbWeek.Text = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    lbWeek.Text = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    lbWeek.Text = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    lbWeek.Text = "星期四";
                    break;
                case DayOfWeek.Friday:
                    lbWeek.Text = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    lbWeek.Text = "星期六";
                    break;
                default:
                    break;
            }
            lbDate.Text = now.ToString("yyyy-MM-dd");
            string time = now.ToString("HH:mm");
            pbH1.LoadPicture(getNumPic(time[0]));
            pbH2.LoadPicture(getNumPic(time[1]));
            pbM1.LoadPicture(getNumPic(time[3]));
            pbM2.LoadPicture(getNumPic(time[4]));
        }

        private void UpdateIp()
        {
            string[] ips = getIpAddress();
            lbIp.Text = string.Empty;
            if (ips.Length >= 1)
            {
                lbIp.Text += ips[0];
                if (ips.Length >= 2)
                {
                    for (int i = 1; i < ips.Length; i++)
                    {
                        lbIp.Text += ("," + ips[i]);
                    }
                }
            }
        }

        private void UpdateSystemInfo()
        {
            if (cpuCounter == null)
            {
                cpuCounter = new PerformanceCounter();
                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";
            }
            string cpuStatus = cpuCounter.NextValue().ToString("F1") + "%  ";
            string memStatus = string.Empty;

            if (XingKongScreen.IsRunningOnMono())
            {
                string cmdResult = Bash(@"cat /sys/class/thermal/thermal_zone0/temp").Trim();
                float cpuTemp = float.Parse(cmdResult) / 1000;
                cpuStatus += cpuTemp.ToString("F1");
                cpuStatus += "℃";


                cmdResult = Bash(@"free");
                string[] lines = cmdResult.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 2)
                {
                    string[] fields = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length > 3)
                    {
                        memStatus = string.Format(@"{1}MB / {0}MB", int.Parse(fields[1]) / 1000, int.Parse(fields[2]) / 1000);
                    }
                }
            }
            else
            {
                Assembly vbDll = Assembly.LoadFrom(vbDllPath);
                if (vbDll != null)
                {
                    Type vbModule = vbDll.GetType("Microsoft.VisualBasic.Devices.ComputerInfo");
                    if (vbModule != null)
                    {
                        PropertyInfo AvailablePhysicalMemory = vbModule.GetProperty("AvailablePhysicalMemory");
                        PropertyInfo TotalPhysicalMemory = vbModule.GetProperty("TotalPhysicalMemory");

                        MethodInfo getAPM = AvailablePhysicalMemory.GetGetMethod();
                        MethodInfo getTPM = TotalPhysicalMemory.GetGetMethod();

                        object computerInfo = System.Activator.CreateInstance(vbModule);
                        ulong? avaMem = getAPM.Invoke(computerInfo, null) as ulong?;
                        ulong? totMem = getTPM.Invoke(computerInfo, null) as ulong?;
                        //Console.WriteLine("系统物理内存(M):" + totMem / 1024 / 1024);
                        //Console.WriteLine("可用物理内存(M):" + avaMem / 1024 / 1024);

                        memStatus = string.Format(@"{0}MB / {1}MB", (totMem - avaMem) / 1024 / 1024, totMem / 1024 / 1024);
                    }
                    else
                    {
                        Console.WriteLine("[SystemUI]\terror while finding class ComputerInfo.");
                    }
                }
                else
                {
                    Console.WriteLine("[SystemUI]\terror while loading VB moudle");
                }
            }

            lbCpuStatus.Text = cpuStatus;
            lbRamStatus.Text = memStatus;
        }

        private void switchButton(int btIndex)
        {
            switch (btIndex)
            {
                case 0:
                    btApp.IsChecked = false;
                    btPower.IsChecked = false;
                    break;
                case 1:
                    btApp.IsChecked = true;
                    btPower.IsChecked = false;
                    break;
                case 2:
                    btApp.IsChecked = false;
                    btPower.IsChecked = true;
                    break;
                default:
                    break;
            }
            currentForm.Draw();
        }

        public override void Suspend()
        {
            autoFreshTimer.Enabled = false;
            autoWeatherTimer.Enabled = false;
            autoSyncMiniuteTimer.Enabled = false;

            keyboard.KeyPressed -= Keyboard_KeyPressed;
        }

        public override void Resume()
        {
            XingKongScreen.ClearScreen();
            UpdateTime();
            UpdateIp();
            UpdateSystemInfo();
            currentForm.HardworkDraw();
            XingKongScreen.FreshScreen();

            SyncMiniuteTimer();
            UpdateWeather();
            autoWeatherTimer.Enabled = true;

            keyboard.KeyPressed += Keyboard_KeyPressed;
        }

        private async void Keyboard_KeyPressed(System.Windows.Forms.Keys pressedKey)
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
                    break;
                case System.Windows.Forms.Keys.Enter:
                    if (buttonIndex == 2)
                    {
                        //用户选择并点击了了关机按钮
                        XingKongMessageBox mb = new XingKongMessageBox();
                        mb.Caption = "提示";
                        mb.Title = "确定要关机吗？";
                        mb.btOk.IsChecked = false;
                        mb.btCancel.IsChecked = true;
                        Suspend();
                        XingKongMessageBox.DialogResult result = await mb.ShowAsync();
                        Console.WriteLine("user clicked: " + result.ToString());
                        switch (result)
                        {
                            case XingKongMessageBox.DialogResult.OK:
                                Thread t = new Thread(new ThreadStart(ShutdownComputer));
                                t.IsBackground = true;
                                t.Start();
                                break;
                            case XingKongMessageBox.DialogResult.Cancel:
                                Resume();
                                break;
                            default:
                                Resume();
                                break;
                        }
                    }
                    else if (buttonIndex == 1)
                    {
                        //用户选择并点击了应用按钮
                        Suspend();
                        AppForm appForm = new AppForm();
                        appForm.QuitAction = Resume;
                        appForm.LoadAppAction = LoadAppAction;

                        Thread t = new Thread(new ThreadStart(appForm.Init));
                        t.IsBackground = true;
                        t.Start();
                    }
                    break;
                case System.Windows.Forms.Keys.F5:
                    XingKongScreen.ClearScreen();
                    currentForm.HardworkDraw();
                    XingKongScreen.FreshScreen();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示关机界面
        /// </summary>
        private void showShutdownInterface()
        {
            string shutdownWindowJson = File.ReadAllText(@"ShutdownForm.json");
            XingKongWindow.Entity windowEntity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(shutdownWindowJson);
            XingKongWindow shutdownWindow = new XingKongWindow();
            shutdownWindow.SetEntity(windowEntity);
            XingKongImageBox pbLogo = shutdownWindow.ControlsSet["pbLogo"] as XingKongImageBox;
            XingKongImageBox pbCopyright = shutdownWindow.ControlsSet["pbCopyright"] as XingKongImageBox;
            XingKongImageBox pbEarth = shutdownWindow.ControlsSet["pbEarth"] as XingKongImageBox;
            XingKongImageBox pbDisconnect = shutdownWindow.ControlsSet["pbDisconnect"] as XingKongImageBox;

            pbLogo.SkipPreProceed = true;
            pbCopyright.SkipPreProceed = true;
            pbEarth.SkipPreProceed = true;
            pbDisconnect.SkipPreProceed = true;

            pbLogo.LoadPicture(Path.Combine("SystemUI", "Logo_cut_1.bmp"));
            pbCopyright.LoadPicture(Path.Combine("SystemUI", "Logo_cut_2.bmp"));
            pbEarth.LoadPicture(Path.Combine("SystemUI", "Icon_Earth.bmp"));
            pbDisconnect.LoadPicture(Path.Combine("SystemUI", "Icon_Disconnected.bmp"));
            XingKongScreen.ClearScreen();
            shutdownWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
        }

        private void ShutdownComputer()
        {
            if (XingKongScreen.IsRunningOnMono())
            {
                showShutdownInterface();
                Console.WriteLine(Bash("sudo shutdown -h now"));
            }
        }

        private string getNumPic(char num)
        {
            if (num == ':')
            {
                return Path.Combine("SystemUI", "NUMS.bmp");
            }
            else if (num >= '0' && num <= '9')
            {
                return Path.Combine("SystemUI", "NUM" + num + ".bmp");
            }
            else
            {
                Console.WriteLine("[SystemUI]\terror while getNumPic :" + num + " .");
                return Path.Combine("SystemUI", "NUMS.bmp");
            }
        }

        private string getWeatherPic(string weatherDesc)
        {
            string basePath = "SystemUI";
            if (weatherDesc.Contains("晴"))
            {
                return Path.Combine(basePath, "WQING.jpg");
            }
            if (weatherDesc.Contains("阴"))
            {
                return Path.Combine(basePath, "WYIN.jpg");
            }
            if (weatherDesc.Contains("多云"))
            {
                return Path.Combine(basePath, "WDYZQ.jpg");
            }
            if (weatherDesc.Contains("雷"))
            {
                return Path.Combine(basePath, "WLZYU.jpg");
            }
            if (weatherDesc.Contains("雪"))
            {
                return Path.Combine(basePath, "WXUE.jpg");
            }
            if (weatherDesc.Contains("雹"))
            {
                return Path.Combine(basePath, "WBBAO.jpg");
            }
            if (weatherDesc.Contains("霾") || weatherDesc.Contains("雾"))
            {
                return Path.Combine(basePath, "WWU.jpg");
            }
            if (weatherDesc.Contains("雨"))
            {
                if (weatherDesc.Contains("小雨"))
                {
                    return Path.Combine(basePath, "WXYU.jpg");
                }
                else
                {
                    return Path.Combine(basePath, "WYU.jpg");
                }
            }
            return Path.Combine(basePath, "WDYZQ.jpg");
        }

        private void UpdateWeather()
        {
            try
            {
                string todayJson = XingKongUtils.HttpUtils.Get(TodayWeatherUrl);
                WeatherTodayReponseEntity today = JsonConvert.DeserializeObject<WeatherTodayReponseEntity>(todayJson);

                pbWeatherIcon.LoadPicture(getWeatherPic(today.result.weather_curr));
                lbCity.Text = today.result.citynm;
                lbWeather.Text = today.result.weather;
                lbTemp.Text = today.result.temperature.Replace(@"/", " ~ ");
                lbWind.Text = today.result.wind + " " + today.result.winp;
                lbTempCurr.Text = today.result.temperature_curr;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SystemUI]\terror while getting today's weather.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                pbWeatherIcon.LoadPicture(getWeatherPic("雪"));
                lbCity.Text = "城市";
                lbWeather.Text = "天气未知";
                lbTemp.Text = "N/A℃ ~ N/A℃";
                lbWind.Text = "获取天气失败";
                lbTempCurr.Text = "N/A";
            }

            try
            {
                string futureJson = XingKongUtils.HttpUtils.Get(FutureWeatherUrl);
                WeatherFutureResponseEntity future = JsonConvert.DeserializeObject<WeatherFutureResponseEntity>(futureJson);

                var tomoEntity = future.result[1];
                lbTomoWeatherAndWind.Text = tomoEntity.weather + "  " + tomoEntity.winp;
                lbTomoTemp.Text = tomoEntity.temperature.Replace(@"/", " ~ ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SystemUI]\terror while getting future's weather.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                lbTomoWeatherAndWind.Text = "天气未知";
                lbTomoTemp.Text = "N/A℃ ~ N/A℃";
            }
        }

        private string[] getIpAddress()
        {
            List<string> ip = new List<string>();
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip.Add(_IPAddress.ToString());
                }
            }
            return ip.ToArray();
        }

        public class WeatherTodayReponseEntity
        {
            public string success;
            public WeatherResultEntity result;
        }

        public class WeatherFutureResponseEntity
        {
            public string success;
            public WeatherResultEntity[] result;
        }

        public class WeatherResultEntity
        {
            public string weaid;
            public string days;
            public string week;
            public string cityno;

            /// <summary>
            /// 城市
            /// </summary>
            public string citynm;
            public string cityid;

            /// <summary>
            /// 温度
            /// </summary>
            public string temperature;
            public string temperature_curr;

            /// <summary>
            /// 湿度
            /// </summary>
            public string humidity;
            public string weather;
            public string weather_curr;
            public string weather_icon;
            public string weather_icon1;

            /// <summary>
            /// 风向
            /// </summary>
            public string wind;

            /// <summary>
            /// 风力
            /// </summary>
            public string winp;

            /// <summary>
            /// 最高温度
            /// </summary>
            public string temp_high;

            /// <summary>
            /// 最低温度
            /// </summary>
            public string temp_low;
            public string temp_curr;
            public string humi_high;
            public string humi_low;
            public string weatid;
            public string weatid1;
            public string windid;
            public string winpid;
        }
    }
}
