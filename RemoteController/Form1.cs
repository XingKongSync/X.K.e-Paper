using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteController
{
    public partial class Form1 : Form
    {
        private XingKongUtils.UdpUtils.UdpSend udpClient;
        private XingKongUtils.UdpUtils.UdpListen udpListen;
        private readonly byte[] BROADCAST_SEARCH_DEVICE = new byte[] {0x00, 0xFF, 0x00};
        private HashSet<string> ipSet;
        private HashSet<string> localIpSet;

        public Form1()
        {
            InitializeComponent();
            ipSet = new HashSet<string>();
            localIpSet = new HashSet<string>();
            udpListen = new XingKongUtils.UdpUtils.UdpListen(9850);
            udpListen.DataReceived2 += UdpListen_DataReceived2;
            udpListen.StartListen();

        }

        private void UdpListen_DataReceived2(byte[] data, System.Net.IPEndPoint addrSource)
        {
            if (data.Length == 3)
            {
                if (ipSet.Add(addrSource.Address.ToString()))
                {
                    cbIpList.Invoke(new Action(UpdateIpView));
                }
            }
        }

        private void UpdateIpView()
        {
            cbIpList.Items.Clear();
            foreach (string ip in ipSet)
            {
                if (!localIpSet.Contains(ip))
                {
                    cbIpList.Items.Add(ip);
                }
            }
            if (cbIpList.Text.Equals(string.Empty) && cbIpList.Items.Count > 0)
            {
                cbIpList.SelectedIndex = 0;
            }
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            udpClient = new XingKongUtils.UdpUtils.UdpSend(cbIpList.Text, 9849);
        }

        private void sendKey(Keys key)
        {
            byte[] data = BitConverter.GetBytes((int)key);
            udpClient.message(data);
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Up);
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Down);
        }

        private void btLeft_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Left);
        }

        private void btRight_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Right);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Enter);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            sendKey(Keys.Escape);
        }

        private void tbKeybordInput_KeyUp(object sender, KeyEventArgs e)
        {
            tbKeybordInput.Text = "";
            sendKey(e.KeyCode);
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            localIpSet.Clear();
            IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in ipe.AddressList)
            {
                localIpSet.Add(ip.ToString());
            }

            udpClient = new XingKongUtils.UdpUtils.UdpSend("255.255.255.255", 9849);
            udpClient.message(BROADCAST_SEARCH_DEVICE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btSearch_Click(null, null);
        }
    }
}
