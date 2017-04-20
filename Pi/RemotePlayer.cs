using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongForm;

namespace XingKongApp
{
    public class RemotePlayer : App
    {

        private Keyboard keyboard;
        private string appPath;
        private string defaultAlubmPath;
        private TcpListener tcpListener;

        private XingKongWindow currentWindow;
        private XingKongImageBox pbAlbum;
        private XingKongLabel lbNowPlaying;
        private XingKongLabel lbName;
        private XingKongLabel lbSinger;
        private XingKongLabel lbAlbum;
        private XingKongLabel lbExit;

        //通过远程计算机的XingKongSync2来控制媒体播放
        private XingKongUtils.UdpUtils.UdpSend udpsend;

        public override string GetName()
        {
            return "远程播放器";
        }

        public override void Init()
        {
            appPath = Path.Combine("Apps", "RemotePlayer");
            currentWindow = new XingKongWindow();
            XingKongWindow.Entity entity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(File.ReadAllText(Path.Combine(this.appPath, "RemotePlayerForm.json")));
            currentWindow.SetEntity(entity);
            pbAlbum = currentWindow.ControlsSet["pbAlbum"] as XingKongImageBox;
            lbNowPlaying = currentWindow.ControlsSet["lbNowPlaying"] as XingKongLabel;
            lbName = currentWindow.ControlsSet["lbName"] as XingKongLabel;
            lbSinger = currentWindow.ControlsSet["lbSinger"] as XingKongLabel;
            lbAlbum = currentWindow.ControlsSet["lbAlbum"] as XingKongLabel;
            lbExit = currentWindow.ControlsSet["lbExit"] as XingKongLabel;

            lbName.Text = "暂无信息";
            lbSinger.Text = "暂无信息";
            lbAlbum.Text = "暂无信息";

            //pbAlbum.SkipPreProceed = true;
            defaultAlubmPath = Path.Combine(appPath, "album.png");
            pbAlbum.LoadPicture(defaultAlubmPath);

            XingKongScreen.ClearScreen();
            currentWindow.HardworkDraw();
            XingKongScreen.FreshScreen();

            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += new Keyboard.KeyPressedHandler(Keyboard_KeyPressed);
            tcpListener = new TcpListener();
            tcpListener.MusicInfoReceived = MusicInfoReceived;
            tcpListener.Start();
        }

        public void MusicInfoReceived(MusicInfo mi)
        {
            //Console.WriteLine(mi.Name);
            //Console.WriteLine(mi.Singer);
            //Console.WriteLine(mi.Album);
            //Console.WriteLine(mi.picBase64);
            udpsend = new XingKongUtils.UdpUtils.UdpSend(tcpListener.RemoteIp.ToString(), 7777);//每次建立连接后都重新初始化一下udp对象

            lbName.Text = mi.Name;
            lbSinger.Text = mi.Singer;
            lbAlbum.Text = mi.Album;
            if (!string.IsNullOrWhiteSpace(mi.picBase64))
            {
                pbAlbum.LoadPicture(ToImage(mi.picBase64));
            }

            XingKongScreen.ClearScreen();
            currentWindow.HardworkDraw();
            XingKongScreen.FreshScreen();
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Bitmap ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            MemoryStream memStream = new MemoryStream(bytes);
            BinaryFormatter binFormatter = new BinaryFormatter();
            Bitmap img = (Bitmap)binFormatter.Deserialize(memStream);
            return img;
        }

        private void Keyboard_KeyPressed(Keys pressedKey)
        {
            switch (pressedKey)
            {
                case Keys.Escape:
                    Suspend();
                    Quit();
                    break;
                case Keys.Left:
                    //上一曲
                    UdpSend("<pre>");
                    break;
                case Keys.Right:
                    //下一曲
                    UdpSend("<nex>");
                    break;
                case Keys.Up:
                    //音量+
                    UdpSend("<vo+>");
                    break;
                case Keys.Down:
                    //音量-
                    UdpSend("<vo->");
                    break;
                case Keys.Enter:
                    //播放、暂停
                    UdpSend("<pla>");
                    break;
                default:
                    break;
            }
        }

        private void UdpSend(string msg)
        {
            if (udpsend != null)
            {
                udpsend.message(Encoding.UTF8.GetBytes(msg));
            }
        }

        public override void Resume()
        {
            this.keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
            tcpListener.Start();
        }

        public override void Suspend()
        {
            this.keyboard.KeyPressed -= new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
            tcpListener.Stop();
        }
    }
}
