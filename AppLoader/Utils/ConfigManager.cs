using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLoader.Utils
{
    public class ConfigManager
    {
        private static ConfigManager m_Instance;
        private ConfigEntity config;

        private ConfigManager()
        {
            if (File.Exists("Config.json"))
            {
                string jsonText = File.ReadAllText("Config.json");
                config = JsonConvert.DeserializeObject<ConfigEntity>(jsonText);
            }
            else
            {
                Console.WriteLine("Config.json doesn't exist.");
            }
        }

        public static ConfigManager GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new ConfigManager();
            }
            return m_Instance;
        }

        public ConfigEntity Config
        {
            get
            {
                return config;
            }
        }
    }

    public class ConfigEntity
    {
        public string PortName;
        public string BackupPortName;
    }
}
