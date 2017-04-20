using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpLinkNetMonitor
{
    public class EntityHelper
    {
        /// <summary>
        /// 登陆的服务器响应
        /// </summary>
        public class LoginResponseEntity
        {
            public string error_code;
            public string stok;
        }

        public class WanStatusReponseEntity
        {
            public string error_code;
            public NetworkStatusEntity network;
        }

        public class NetworkStatusEntity
        {
            public WanStatusEntity wan_status;
        }

        public class WanStatusEntity
        {
            public int down_speed;
            public int up_speed;
        }

        /// <summary>
        /// 解析匿名json对象HostInfo
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<HostInfoEntity> AnalysHostInfoJson(string json)
        {
            List<HostInfoEntity> hostsInfo = new List<HostInfoEntity>();
            string jsonArrayStr = GetJsonArray(json);

            var jsonArray = JArray.Parse(jsonArrayStr);
            foreach (var jtoken in jsonArray)
            {
                JToken jobj = jtoken.First.First;
                hostsInfo.Add(new HostInfoEntity
                {
                    hostname = System.Web.HttpUtility.UrlDecode(jobj["hostname"].ToString()),
                    ip = jobj["ip"].ToString(),
                    down_speed = jobj["down_speed"].ToString(),
                    mac = jobj["mac"].ToString(),
                    up_speed = jobj["up_speed"].ToString()
                });
            }

            return hostsInfo;
        }

        /// <summary>
        /// 截取json数组
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string GetJsonArray(string json)
        {
            string startStr = "[";
            string endStr = "]";
            int start = json.IndexOf(startStr);
            int end;
            if (start != -1)
            {
                end = json.LastIndexOf(endStr);
                if (end != -1)
                {
                    string result = json.Substring(start, end - start + 1);
                    return result;
                }
            }
            return string.Empty;
        }

        public class HostInfoEntity
        {
            public string mac;
            public string ip;
            public string hostname;
            public string up_speed;
            public string down_speed;
        }
    }
}
