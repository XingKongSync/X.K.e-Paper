using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string testJson = "{ \"hosts_info\": { \"online_host\": [ { \"host_info_14\": { \"mac\": \"28-b2-bd-71-5f-d9\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.110\", \"hostname\": \"liyanhai%2DPC\", \"up_speed\": \"0\", \"down_speed\": \"182\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_15\": { \"mac\": \"64-9a-be-4d-8c-ef\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.105\", \"hostname\": \"lizhu\", \"up_speed\": \"23847\", \"down_speed\": \"573967\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_21\": { \"mac\": \"b8-97-5a-d2-30-f2\", \"type\": \"0\", \"blocked\": \"0\", \"ip\": \"192.168.0.180\", \"hostname\": \"\", \"up_speed\": \"231\", \"down_speed\": \"201\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_4\": { \"mac\": \"e8-4e-06-1c-02-0c\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.115\", \"hostname\": \"raspberrypi\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_17\": { \"mac\": \"b8-97-5a-d2-31-15\", \"type\": \"0\", \"blocked\": \"0\", \"ip\": \"192.168.0.181\", \"hostname\": \"\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_16\": { \"mac\": \"78-44-76-c2-93-fd\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.109\", \"hostname\": \"%E6%98%9F%E7%A9%BA%2DDesktop\", \"up_speed\": \"0\", \"down_speed\": \"121\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"1\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_1\": { \"mac\": \"9c-4e-36-53-59-c4\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.100\", \"hostname\": \"%E6%98%9F%E7%A9%BA%2DPC\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_10\": { \"mac\": \"b8-97-5a-d0-2a-cf\", \"type\": \"0\", \"blocked\": \"0\", \"ip\": \"192.168.0.107\", \"hostname\": \"PCDALAO\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_11\": { \"mac\": \"98-90-96-af-1f-ed\", \"type\": \"0\", \"blocked\": \"0\", \"ip\": \"192.168.0.102\", \"hostname\": \"Server\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_20\": { \"mac\": \"44-00-10-7a-86-fd\", \"type\": \"1\", \"blocked\": \"0\", \"ip\": \"192.168.0.111\", \"hostname\": \"yangwen\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } }, { \"host_info_18\": { \"mac\": \"00-0c-29-c2-fa-d5\", \"type\": \"0\", \"blocked\": \"0\", \"ip\": \"192.168.0.121\", \"hostname\": \"\", \"up_speed\": \"0\", \"down_speed\": \"0\", \"up_limit\": \"0\", \"down_limit\": \"0\", \"is_cur_host\": \"0\", \"ssid\": \"\", \"wifi_mode\": \"0\", \"plan_rule\": [ ] } } ] }, \"error_code\": 0 }";
            List<TpLinkNetMonitor.EntityHelper.HostInfoEntity> list = TpLinkNetMonitor.EntityHelper.AnalysHostInfoJson(testJson);
            return;
        }

        [TestMethod]
        public void TestMethod2()
        {
            //请求登陆的url
            string url = @"http://192.168.0.1/";
            //请求登陆的字符串
            string payload = "{\"method\":\"do\",\"login\":{\"password\":\"WaQ7xPr3L41vMwK\"}}";


            Console.WriteLine(XingKongUtils.HttpUtils.Post(url, payload, XingKongUtils.HttpUtils.RequestType.Raw));
        }

        [TestMethod]
        public void TestGetKuaidi()
        {
            string requestUrl = @"https://sp0.baidu.com/9_Q4sjW91Qh3otqbppnN2DJv/pae/channel/data/asyncqury?cb=jQuery110204408993111524364_1489902564206&appid=4001&com=rufengda&nu=11703199808371&vcode=&token=&_=1489902564208";
            string response = XingKongUtils.HttpUtils.Get(requestUrl);

            Kuaidi.EntityHelper.ResponseEntity responseEntity = JsonConvert.DeserializeObject<Kuaidi.EntityHelper.ResponseEntity>(response);
            return;
        }
    }
}
