using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace XingKongApp
{
    public class App
    {
        protected Action QuitAction;
        protected Action<string> LoadAppAction;

        public virtual void Init()
        {

        }

        public virtual string GetName()
        {
            return "DefaultApp";
        }

        public virtual void Quit()
        {
            QuitAction?.Invoke();
        }

        public virtual void Suspend()
        {

        }

        public virtual void Resume()
        {

        }

        public void SetQuitAction(Action action)
        {
            QuitAction = action;
        }

        public void SetLoadAppAction(Action<string> action)
        {
            LoadAppAction = action;
        }

        public static string Bash(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "sh";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.WriteLine("exit");
            string strResult = p.StandardOutput.ReadToEnd();
            p.Close();

            return strResult;
        }

        public static App GetApp(string moduleName)
        {
            Type type = null;
            foreach (Type type2 in Assembly.LoadFrom(moduleName + ".dll").GetTypes())
            {
                if (type2.IsSubclassOf(typeof(App)))
                {
                    type = type2;
                    break;
                }
            }
            try
            {
                if (type.GetMethod("Init") != null)
                {
                    return (Activator.CreateInstance(type) as App);
                }
            }
            catch (Exception exception1)
            {
                Console.WriteLine(exception1.Message);
                Console.WriteLine(exception1.StackTrace);
            }
            return null;
        }
    }
}
