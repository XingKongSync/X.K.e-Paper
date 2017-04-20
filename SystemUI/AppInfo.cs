using System;

namespace XingKongApp
{

    public class AppInfo
    {
        public string AppName;
        public string MoudlePath;

        public AppInfo()
        {
        }

        public AppInfo(string appName, string moudlePath)
        {
            this.AppName = appName;
            this.MoudlePath = moudlePath;
        }
    }
}
