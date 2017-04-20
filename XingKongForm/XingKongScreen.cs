using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongScreen
    {
        private static string portName;
        private static SerialPort serialPort;
        private static Graphics graphics;
        private static Font font;
        private static Keyboard keyboardReceiver;

        public static bool IsRemote = false;
        public static string RemoteIp;
        public static int RemotePort = 9850;
        private static Socket socket;

        public static object AsyncLockObj = new object();

        public static string PortName
        {
            get
            {
                return portName;
            }
        }

        public static void OpenScreen(string PortName)
        {
            if (IsRemote)
            {
                IPAddress ip = null;
                try
                {
                    ip = IPAddress.Parse(RemoteIp);

                    IPEndPoint ipEnd = new IPEndPoint(ip, RemotePort);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipEnd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Remote Connect Error:" + ex.Message);
                    return;
                }
            }
            else
            {
                portName = PortName;
                if (serialPort == null)
                {
                    serialPort = new SerialPort();
                }
                else
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                lock (serialPort)
                {
                    serialPort.PortName = PortName;
                    serialPort.BaudRate = 115200;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Handshake = Handshake.None;
                    serialPort.Open();
                }
                Console.WriteLine("Screen: " + PortName + " has opened.");
            }
        }

        public static Keyboard GetKeyboard()
        {
            if (keyboardReceiver == null)
            {
                keyboardReceiver = new Keyboard();
                keyboardReceiver.StartKeyboardService();
            }
            return keyboardReceiver;
        }

        public static void CloseScreen()
        {
            if (IsRemote)
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Remote Debug Error While ShuttingDown: " + ex.Message);
                    }

                }
            }
            else
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
        }

        public static byte getCheckByte(IEnumerable<byte> data, bool includeLastByte = false)
        {
            byte result = data.First();
            int count = data.Count();
            if (includeLastByte)
            {
                count--;
            }
            for (int i = 1; i < count; i++)
            {
                result ^= data.ElementAt(i);
            }
            return result;
        }

        public static void ClearScreen()
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x09, 0x2E, 0xCC, 0x33, 0xC3, 0x3C, 0x82 };
            Write(command);
        }

        public static void FreshScreen()
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x09, 0x0A, 0xCC, 0x33, 0xC3, 0x3C, 0xA6 };
            Write(command);
        }

        public static void SetFontSize(int sizeIndex)
        {
            SetEngFontSize(sizeIndex);
            SetChineseFontSize(sizeIndex);
        }

        public static void SetEngFontSize(int sizeIndex)
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x0A, 0x1E, 0x01, 0xCC, 0x33, 0xC3, 0x3C, 0xB0 };
            if (sizeIndex == 2)
            {
                command[4] = 0x02;
            }
            else if (sizeIndex == 3)
            {
                command[4] = 0x03;
            }
            command[9] = getCheckByte(command, true);
            Write(command);
        }

        public static void SetChineseFontSize(int sizeIndex)
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x0A, 0x1F, 0x01, 0xCC, 0x33, 0xC3, 0x3C, 0xB1 };
            if (sizeIndex == 2)
            {
                command[4] = 0x02;
            }
            else if (sizeIndex == 3)
            {
                command[4] = 0x03;
            }
            command[9] = getCheckByte(command, true);
            Write(command);
        }

        public static void Write(IEnumerable<byte> data)
        {
            if (IsRemote)
            {
                lock (socket)
                {
                    socket.Send(data.ToArray());
                }
            }
            else
            {
                lock (serialPort)
                {
                    byte[] dataToWrite = data.ToArray();
                    serialPort.Write(dataToWrite, 0, dataToWrite.Length);
                    serialPort.DiscardInBuffer();
                }
            }

        }

        public enum XColor : byte
        {
            Black = 0x00,
            DarkGray = 0x01,
            Gray = 0x02,
            White = 0x03
        }

        public enum TextAlign
        {
            Left,
            Center,
            Right
        }

        public static void SetColor(XColor frontColor = 0x00, XColor backColor = XColor.White)
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x0B, 0x10, (byte)frontColor, (byte)backColor, 0xCC, 0x33, 0xC3, 0x3C, 0xBD };
            byte checkbyte = getCheckByte(command, true);
            command[10] = checkbyte;
            Write(command);
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        public enum XFontSize
        {
            /// <summary>
            /// 二号
            /// </summary>
            Normal = 1,

            /// <summary>
            /// 一号
            /// </summary>
            Large = 2,

            /// <summary>
            /// 初号
            /// </summary>
            ExtraLarge = 3
        }

        public static void DrawSquare(Point P1, Point P2)
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x11, 0x25, 0x00, 0x0A, 0x00, 0x0A, 0x00, 0xFF, 0x00, 0xFF, 0xCC, 0x33, 0xC3, 0x3C, 0x91 };
            IEnumerable<byte> x1 = BitConverter.GetBytes((short)P1.X);
            IEnumerable<byte> y1 = BitConverter.GetBytes((short)P1.Y);
            IEnumerable<byte> x2 = BitConverter.GetBytes((short)P2.X);
            IEnumerable<byte> y2 = BitConverter.GetBytes((short)P2.Y);

            command[4] = x1.ElementAt(1);
            command[5] = x1.ElementAt(0);

            command[6] = y1.ElementAt(1);
            command[7] = y1.ElementAt(0);

            command[8] = x2.ElementAt(1);
            command[9] = x2.ElementAt(0);

            command[10] = y2.ElementAt(1);
            command[11] = y2.ElementAt(0);

            byte checkbyte = getCheckByte(command, true);
            command[16] = checkbyte;

            Write(command);
        }


        public static void DrawLine(Point from, Point to)
        {
            byte[] command = new byte[] { 0xA5, 0x00, 0x11, 0x22, 0x00, 0x0A, 0x00, 0x0A, 0x00, 0xFF, 0x00, 0xFF, 0xCC, 0x33, 0xC3, 0x3C, 0x96 };
            IEnumerable<byte> fx = BitConverter.GetBytes((short)(from.X));
            IEnumerable<byte> fy = BitConverter.GetBytes((short)(from.Y));
            IEnumerable<byte> tx = BitConverter.GetBytes((short)(to.X));
            IEnumerable<byte> ty = BitConverter.GetBytes((short)(to.Y));

            command[4] = fx.ElementAt(1);
            command[5] = fx.ElementAt(0);
            command[6] = fy.ElementAt(1);
            command[7] = fy.ElementAt(0);

            command[8] = tx.ElementAt(1);
            command[9] = tx.ElementAt(0);
            command[10] = ty.ElementAt(1);
            command[11] = ty.ElementAt(0);

            byte checkbyte = XingKongScreen.getCheckByte(command, true);
            command[16] = checkbyte;

            XingKongScreen.Write(command);
        }

        public static void FillSquare(Point P1, Point P2)
        {
            if (P1.X < 0 || P1.Y < 0 || P2.X < 0 || P2.Y < 0)
            {
                return;
            }
            if (P2.X - P1.X > 800 | P2.X - P1.X < (-800))
            {
                return;
            }
            if (P2.Y - P1.Y > 600 | P2.Y - P1.Y < (-600))
            {
                return;
            }

            byte[] command = new byte[] { 0xA5, 0x00, 0x11, 0x24, 0x00, 0x0A, 0x00, 0x0A, 0x00, 0xFF, 0x00, 0xFF, 0xCC, 0x33, 0xC3, 0x3C, 0x90 };
            IEnumerable<byte> x1 = BitConverter.GetBytes((short)P1.X);
            IEnumerable<byte> y1 = BitConverter.GetBytes((short)P1.Y);
            IEnumerable<byte> x2 = BitConverter.GetBytes((short)P2.X);
            IEnumerable<byte> y2 = BitConverter.GetBytes((short)P2.Y);

            command[4] = x1.ElementAt(1);
            command[5] = x1.ElementAt(0);

            command[6] = y1.ElementAt(1);
            command[7] = y1.ElementAt(0);

            command[8] = x2.ElementAt(1);
            command[9] = x2.ElementAt(0);

            command[10] = y2.ElementAt(1);
            command[11] = y2.ElementAt(0);

            byte checkbyte = getCheckByte(command, true);
            command[16] = checkbyte;

            Write(command);
        }

        public static int MeasureStringWidth(string str, XFontSize fontSize)
        {
            int tempSize = 32;
            switch (fontSize)
            {
                case XFontSize.Normal:
                    tempSize = 32;
                    break;
                case XFontSize.Large:
                    tempSize = 48;
                    break;
                case XFontSize.ExtraLarge:
                    tempSize = 64;
                    break;
                default:
                    break;
            }
            if (graphics == null)
            {
                Bitmap b = new Bitmap(tempSize * str.Length, tempSize);
                Image i = Image.FromHbitmap(b.GetHbitmap());
                graphics = Graphics.FromImage(i);
            }
            if (font == null)
            {
                font = new Font("宋体", 10);
            }
            SizeF sizeF = graphics.MeasureString(str, font);
            float radio = sizeF.Width / sizeF.Height;
            return (int)(radio * tempSize);
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
