using System;
using System.Collections.Generic;
using System.IO;

namespace XingKongApp
{
    public class AppFinder
    {
        public const string AppFolder = "Apps";

        public static List<AppInfo> GetAppList()
        {
            List<AppInfo> list = new List<AppInfo>();
            if (Directory.Exists("Apps"))
            {
                string[] files = Directory.GetFiles("Apps", "*.dll");
                for (int i = 0; i < files.Length; i++)
                {
                    string moduleName = files[i].Replace(".dll", "");
                    App app = App.GetApp(moduleName);
                    if (app != null)
                    {
                        list.Add(new AppInfo(app.GetName(), moduleName));
                    }
                }
                return list;
            }
            Console.Write("[AppFinder]\t");
            Console.WriteLine("Folder: Apps not exist.");
            return list;
        }
    }
}
