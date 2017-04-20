using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC
{
    public partial class MainForm : Form
    {
        private RemoteWMP.RemotedWindowsMediaPlayer remotePlayer;
        private System.Timers.Timer timer;

        private string picPath;

        private bool startSync = false;

        private NotifyIcon ni;

        public MainForm()
        {
            InitializeComponent();
        }

        private async Task EnsureWMPRunning()
        {
            Process[] procs = Process.GetProcessesByName("wmplayer");
            if (procs.Length <= 0)
            {
                Process.Start(@"C:\Program Files (x86)\Windows Media Player\wmplayer.exe");
                await Task.Delay(1000);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await EnsureWMPRunning();

            remotePlayer = new RemoteWMP.RemotedWindowsMediaPlayer();
            timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            ni = new NotifyIcon();
            ni.Icon = new System.Drawing.Icon(@"Media.ico");
            ni.Text = "XingKongOS远程播放器";
            ni.Visible = true;
            MenuItem menu1 = new MenuItem();
            menu1.Text = @"显示/隐藏";
            MenuItem menuExit = new MenuItem();
            menuExit.Text = "退出";

            menu1.Click += MenuHide_Click;
            menuExit.Click += MenuExit_Click;
            ni.ContextMenu = new ContextMenu(new MenuItem[] { menu1, menuExit });

        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuHide_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            var currentPlaying = remotePlayer.Player.currentMedia;
            if (currentPlaying != null && !label1.Text.Equals(currentPlaying.name))
            {
                this.Invoke(new Action(() => {
                    MediaHolder mh = new MediaHolder(currentPlaying);
                    label1.Text = mh.Title;
                    label2.Text = mh.Artist;
                    label3.Text = mh.Album;
                    string folderPath = Path.GetDirectoryName(currentPlaying.sourceURL);
                    string newPicPath = Path.Combine(folderPath, "folder.jpg");

                    if (File.Exists(newPicPath))
                    {
                        picPath = newPicPath;
                        pictureBox1.Image = Image.FromFile(newPicPath);
                    }
                    else
                    {
                        picPath = string.Empty;
                    }
                    if (startSync)
                    {
                        btSend_Click(null, null);
                    }
                }));
            }

            timer.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (remotePlayer != null)
            {
                remotePlayer.Dispose();
                remotePlayer = null;
            }
            ni.Visible = false;
            base.OnClosed(e);
        }

        private void Send(string ip, int port, byte[] senddata)
        {
            try
            {
                //1.发送数据                
                TcpClient client = new TcpClient(ip, port);
                IPEndPoint ipendpoint = client.Client.RemoteEndPoint as IPEndPoint;
                NetworkStream stream = client.GetStream();
                stream.Write(senddata, 0, senddata.Length);
                Console.WriteLine("{0:HH:mm:ss}->发送数据(to {1})", DateTime.Now, ip);

                //3.关闭对象
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0:HH:mm:ss}->{1}", DateTime.Now, ex.Message);
            }
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            MusicInfo mi = new MusicInfo();
            mi.Name = label1.Text;
            mi.Singer = label2.Text;
            mi.Album = label3.Text;
            mi.picBase64 = string.Empty;
            if (!string.IsNullOrWhiteSpace(picPath))
            {
                Bitmap converetdBitmap = Scale(new Bitmap(pictureBox1.Image, pictureBox1.Image.Size), 300, 300);
                mi.picBase64 = ToBase64(converetdBitmap);
            }
            Send(textBox1.Text, 9500, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mi)));
        }

        /// <summary>
        /// 将图片数据转换为Base64字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string ToBase64(Bitmap bmp)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            binFormatter.Serialize(memStream, bmp);
            byte[] bytes = memStream.GetBuffer();
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        /// <summary>
        /// 图像按比例缩放
        /// </summary>
        /// <param name="b"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static Bitmap Scale(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;


            if ((sWidth * destHeight) > (sHeight * destWidth))
            {
                sW = destWidth;
                sH = (destWidth * sHeight) / sWidth;
            }
            else
            {
                sH = destHeight;
                sW = (sWidth * destHeight) / sHeight;
            }

            Bitmap outBmp = new Bitmap(sW, sH);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle(0, 0, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }

        private void btStartSync_Click(object sender, EventArgs e)
        {
            startSync = !startSync;
            if (startSync)
            {
                btStartSync.Text = "停止同步";
            }
            else
            {
                btStartSync.Text = "开始同步";
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
