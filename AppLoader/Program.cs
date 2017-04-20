using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;
using XingKongUtils;

namespace AppLoader
{
    class Program
    {
        private static XingKongApp.App SystemUI;
        private static Utils.ConfigEntity config;
        private static System.Timers.Timer PortDemonTimer;//狗日的辣鸡pi有的时候会把ttyUSB0突然变成ttyUSB1

        static void Main(string[] args)
        {
            Utils.ConfigManager configManager = Utils.ConfigManager.GetInstance();
            config = configManager.Config;
            Console.WriteLine("PortName: " + config.PortName);

            if (args != null && args.Length >= 2 && args[0].Equals("-ip"))
            {
                XingKongScreen.IsRemote = true;
                XingKongScreen.RemoteIp = args[1];
            }
            else if (!string.IsNullOrWhiteSpace(config.BackupPortName) && XingKongScreen.IsRunningOnMono())
            {
                Console.WriteLine("BackupPortName: " + config.BackupPortName);
                PortDemonTimer = new System.Timers.Timer();
                PortDemonTimer.Elapsed += PortDemonTimer_Elapsed;
                PortDemonTimer.Interval = 1000 * 30;//30秒检查一次
                PortDemonTimer.Enabled = true;
            }

            XingKongScreen.OpenScreen(config.PortName);
            XingKongScreen.SetColor();
            XingKongScreen.ClearScreen();

            SystemUI = LoadAppAsync("SystemUI");

            if (args != null && args.Length >= 1 && args[0].Equals("-bg"))
            {
                //让程序在linux下在后台执行不退出
                //需要输入下列命令来启动本程序
                //nohup mono AppLoader.exe -bg &
                Thread.CurrentThread.Suspend();
            }
            else
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                while (!input.Equals("exit"))
                {
                    Console.Write("> ");
                    input = Console.ReadLine();
                }
            }
            XingKongScreen.CloseScreen();
        }

        private static void PortDemonTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!File.Exists(XingKongScreen.PortName))
            {
                if (XingKongScreen.PortName.Equals(config.PortName))
                {
                    XingKongScreen.OpenScreen(config.BackupPortName);
                }
                else
                {
                    XingKongScreen.OpenScreen(config.PortName);
                }
            }
        }

        static XingKongApp.App LoadAppAsync(string moduleName)
        {
            XingKongApp.App app = GetApp(moduleName);
            Task.Run(() => {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
                app.Init();
            });
            return app;
        }

        static XingKongApp.App GetApp(string moduleName)
        {
            Assembly pAsm = Assembly.LoadFrom(moduleName + ".dll");
            Type[] types = pAsm.GetTypes();
            Type mainModule = null;
            foreach (var item in types)
            {
                if (item.IsSubclassOf(typeof(XingKongApp.App)))
                {
                    //找到主模块
                    mainModule = item;
                    break;
                }
            }
            try
            {
                //找到入口方法
                MethodInfo entryPoint = mainModule.GetMethod("Init");
                if (entryPoint != null)
                {
                    //创建对象实例
                    XingKongApp.App app = System.Activator.CreateInstance(mainModule) as XingKongApp.App;
                    app.SetQuitAction(QuitFun);
                    app.SetLoadAppAction((string m)=> { LoadAppAsync(m); });
                    return app;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return null;
        }

        static void QuitFun()
        {
            if (SystemUI != null)
            {
                SystemUI.Resume();
            }
        }
    }
}
