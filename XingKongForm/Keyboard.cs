using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XingKongUtils;

namespace XingKongForm
{
    public class Keyboard
    {
        private XingKongUtils.UdpUtils.UdpListen udpClient;

        public delegate void KeyPressedHandler(Keys pressedKey);

        public event KeyPressedHandler KeyPressed;

        private readonly byte[] RETURN_BROADCAST_IP_ADDRESS = new byte[] { 0xFF, 0x00, 0xFF };

        public void StartKeyboardService()
        {
            if (udpClient == null)
            {
                udpClient = new XingKongUtils.UdpUtils.UdpListen(9849);
            }
            udpClient.DataReceived2 += UdpClient_DataReceived2;
            udpClient.StartListen();
        }

        private void UdpClient_DataReceived2(byte[] data, System.Net.IPEndPoint addrSource)
        {
            if (data.Length == 4)
            {
                Keys key = (Keys)BitConverter.ToInt32(data, 0);
                Console.WriteLine(string.Format("Key:{0} Pressed.", key.ToString()));
                KeyPressed?.Invoke(key);
            }
            else if (data.Length == 3)
            {
                XingKongUtils.UdpUtils.UdpSend udpSend = new XingKongUtils.UdpUtils.UdpSend(addrSource.Address.ToString(), 9850);
                udpSend.message(RETURN_BROADCAST_IP_ADDRESS);
                Console.WriteLine("Broadcast Received.");
            }
        }
    }
}
