using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RemoteSerialPort
{
    class Program
    {
        static string portName;
        static SerialPort serialPort;

        static void Main(string[] args)
        {
            if (args != null && args.Length >= 1)
            {
                portName = args[0];
                serialPort = new SerialPort();
                serialPort.PortName = portName;
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.Open();

                Console.WriteLine("TCP端口：" + 9850);
                Console.WriteLine("本机串口：" + portName);
            }

            Thread th = new Thread(new ThreadStart(listen));
            th.Start();
            Thread.CurrentThread.Suspend();
        }

        private static void listen()
        {

            int recv;
            byte[] recdata = new byte[1024];
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 9850);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEnd);
            socket.Listen(10);
            while (true)
            {
                Console.WriteLine("正在等待远程计算机的连接");
                Socket client = socket.Accept();
                IPEndPoint ipEndClient = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("提示:", "成功与" + ipEndClient.Address + ":" + ipEndClient.Port + "建立连接\n\n数据传输中......", false);
                while (true)
                {
                    recv = client.Receive(recdata);
                    if (recv == 0)
                    {
                        break;
                    }
                    //写串口
                    serialPort.Write(recdata, 0, recv);
                }
                Console.WriteLine("数据接收完成，连接已断开");
                client.Close();
            }
        }
    }
}
