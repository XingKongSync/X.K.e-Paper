using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongButton : IDrawable
    {
        public string Name;

        private int left;
        private int top;
        private int width;
        private int height;
        private string text;
        private int textXoffset = 0;
        private int textYoffset = 0;

        public XingKongScreen.XFontSize FontSize;
        public XingKongScreen.TextAlign TextAlign = XingKongScreen.TextAlign.Center;

        private bool isChecked = false;

        private bool NeedDraw = true;

        public bool IsChecked
        {
            set
            {
                isChecked = value;
                NeedDraw = true;
            }
            get
            {
                return isChecked;
            }
        }

        public int Left
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

        public int Top
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

        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                NeedDraw = true;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                NeedDraw = true;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                NeedDraw = true;
            }
        }

        public int TextXoffset
        {
            get
            {
                return textXoffset;
            }

            set
            {
                textXoffset = value;
                NeedDraw = true;
            }
        }

        public int TextYoffset
        {
            get
            {
                return textYoffset;
            }

            set
            {
                textYoffset = value;
                NeedDraw = true;
            }
        }

        public void Draw()
        {
            //先生成文本
            int fontHeight = 32;
            switch (FontSize)
            {
                case XingKongScreen.XFontSize.Normal:
                    fontHeight = 32;
                    break;
                case XingKongScreen.XFontSize.Large:
                    fontHeight = 48;
                    break;
                case XingKongScreen.XFontSize.ExtraLarge:
                    fontHeight = 64;
                    break;
                default:
                    break;
            }
            int fontWidth = XingKongScreen.MeasureStringWidth(Text, FontSize);
            int fontLeft = 10;
            switch (TextAlign)
            {
                case XingKongScreen.TextAlign.Left:
                    fontLeft = 10 + left;
                    break;
                case XingKongScreen.TextAlign.Center:
                    fontLeft = (Width - fontWidth) / 2 + Left + TextXoffset;
                    break;
                case XingKongScreen.TextAlign.Right:
                    fontLeft = Width - fontWidth - 10;
                    break;
                default:
                    break;
            }
            int fontTop = (Height - fontHeight) / 2 + Top + TextYoffset;

            XingKongLabel ltemp = new XingKongLabel();
            ltemp.Text = Text;
            ltemp.Left = (short)fontLeft;
            ltemp.Top = (short)fontTop;
            ltemp.FontSize = FontSize;

            //计算矩形位置
            Point p1 = new Point(Left, Top);
            Point p2 = new Point(Left + Width, Top + Height);

            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                if (isChecked)
                {
                    XingKongScreen.SetColor(XingKongScreen.XColor.Gray);
                    XingKongScreen.FillSquare(p1, p2);

                    //设置文字为白色，背景为黑色
                    XingKongScreen.SetColor(XingKongScreen.XColor.White, XingKongScreen.XColor.Gray);
                    ltemp.Draw();

                    //恢复默认调色板
                    XingKongScreen.SetColor();
                }
                else
                {
                    XingKongScreen.SetColor(XingKongScreen.XColor.White);
                    XingKongScreen.FillSquare(p1, p2);
                    XingKongScreen.SetColor();
                    //先画文本
                    ltemp.Draw();
                    //再画矩形
                    XingKongScreen.DrawSquare(p1, p2);
                }
            }

            NeedDraw = false;
        }

        public string getName()
        {
            return Name;
        }

        public void setName(string name)
        {
            Name = name;
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
            this.Left = left;
        }

        public int getLeft()
        {
            return left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public int getTop()
        {
            return top;
        }

        public void HardworkDraw()
        {
            Draw();
        }
    }
}
