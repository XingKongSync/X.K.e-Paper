using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongLabel : IDrawable
    {
        public XingKongScreen.XFontSize FontSize;
        private string text = "";
        private short left;
        private short top;

        public string Name;

        private bool NeedDraw = true;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (value != null && !text.Equals(value))
                {
                    text = value;
                    NeedDraw = true;
                }
            }
        }

        public short Left
        {
            get
            {
                return left;
            }

            set
            {
                left = value;
                NeedDraw = true;
            }
        }

        public short Top
        {
            get
            {
                return top;
            }

            set
            {
                top = value;
                NeedDraw = true;
            }
        }

        public XingKongLabel()
        {
            FontSize = XingKongScreen.XFontSize.Normal;
            Text = string.Empty;
            Left = 0;
            Top = 0;
        }

        public XingKongLabel(string text, XingKongScreen.XFontSize size, int left, int top)
        {
            FontSize = size;
            Text = text;
            Left = (short)left;
            Top = (short)top;

            NeedDraw = true;
        }

        public void Draw()
        {
            List<byte> byteList = new List<byte>();

            //设置字体大小
            XingKongScreen.SetFontSize((int)FontSize);

            //生成坐标数据
            IEnumerable<byte> left = BitConverter.GetBytes(Left).Reverse();
            IEnumerable<byte> top = BitConverter.GetBytes(Top).Reverse();

            //生成字符串数据
            byte[] str = Encoding.GetEncoding("GBK").GetBytes(Text);

            IEnumerable<byte> length = BitConverter.GetBytes((short)(9 + str.Length + 1 + 4)).Reverse();

            byteList.Add(0xA5);
            byteList.AddRange(length);
            byteList.Add(0x30);
            byteList.AddRange(left);
            byteList.AddRange(top);
            byteList.AddRange(str);
            byteList.Add(0x00);
            byteList.AddRange(new byte[] { 0xCC, 0x33, 0xC3, 0x3C });

            byte checkByte = XingKongScreen.getCheckByte(byteList);
            byteList.Add(checkByte);

            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                XingKongScreen.Write(byteList);
            }
            NeedDraw = false;
        }

        public void setName(string name)
        {
            Name = name;
        }

        public string getName()
        {
            return Name;
        }

        public bool needDraw()
        {
            return NeedDraw;
        }

        public void ClearArea()
        {
            
        }

        public void setLeft(int left)
        {
            this.Left = (short)left;
        }

        public int getLeft()
        {
            return left;
        }

        public void setTop(int top)
        {
            this.Top = (short)top;
        }

        public int getTop()
        {
            return top;
        }

        public void HardworkDraw()
        {
            Draw();
            NeedDraw = false;
        }
    }
}
