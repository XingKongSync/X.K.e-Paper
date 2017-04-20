using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XingKongApp
{
    public class TcpListener
    {
        private Thread th;

        public Action<MusicInfo> MusicInfoReceived;

        private IPAddress remoteIp;

        public IPAddress RemoteIp
        {
            get
            {
                return remoteIp;
            }

            private set
            {
                remoteIp = value;
            }
        }

        public void Start()
        {
            th = new Thread(new ThreadStart(listen));
            th.Start();
        }

        public void Stop()
        {
            if (th != null)
            {
                th.Abort();
                th = null;
                Console.WriteLine("[Callback]\tTCP监听已停止.");
            }
        }

        private void listen()
        {
            int recvLength;
            byte[] recdata = new byte[1024];
            List<byte> l = new List<byte>();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 9500);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEnd);
            socket.Listen(10);
            Console.WriteLine("[Callback]\tTCP监听已启动.");
            while (true)
            {
                Socket client = socket.Accept();
                IPEndPoint ipEndClient = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("[Callback]\t成功与" + ipEndClient.Address + ":" + ipEndClient.Port + "建立连接.");
                RemoteIp = ipEndClient.Address;//记录远程计算机的ip
                l.Clear();
                while (true)
                {
                    try
                    {
                        recvLength = client.Receive(recdata);
                        if (recvLength == 0)
                        {
                            Console.WriteLine("[Callback]\t数据接收完成，连接已断开.");
                            break;
                        }
                        l.AddRange(recdata.Take(recvLength));
                    }
                    catch (Exception ex)
                    {
                        Console.Write("[Callback]\t");
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
                try
                {
                    messageHandler(l);
                }
                catch (Exception ex)
                {
                    Console.Write("[Callback]\t尝试处理PC发来的消息出现异常，原因：");
                    Console.WriteLine(ex.Message);
                }
                client.Close();
            }
        }

        private void messageHandler(List<byte> data)
        {
            if (data.Count > 2)
            {
                string msg = Encoding.UTF8.GetString(data.ToArray());
                MusicInfo musicInfo = JsonConvert.DeserializeObject<MusicInfo>(msg);
                MusicInfoReceived?.Invoke(musicInfo);
            }
        }
    }

    public class MusicInfo
    {
        public string Name;
        public string Singer;
        public string Album;
        public string picBase64;
    }
}
