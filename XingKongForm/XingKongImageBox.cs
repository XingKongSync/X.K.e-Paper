using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;

namespace XingKongForm
{
    public class XingKongImageBox : IDrawable
    {
        private int left;
        private int top;
        public bool SkipPreProceed = false;
        private string PicFilePath;
        public int Width;
        public int Height;

        public string Name;
        private Bitmap bitmap;

        private bool NeedDraw = true;
        private Point lastP1;
        private Point lastP2;

        private byte[] defaultColorPiovt = { 93, 156, 189 };
        private byte[] customColorPiovt = new byte[3];

        private void setLastPoint()
        {
            if (bitmap != null)
            {
                lastP1 = new Point(Left, Top);
                lastP2 = new Point(Left + Width, Top + Height);
            }
            else
            {
                lastP1 = new Point(0, 0);
                lastP2 = new Point(0, 0);
            }
        }

        public void LoadPicture(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && !filePath.Equals(PicFilePath))
            {
                setLastPoint();

                PicFilePath = filePath;
                bitmap = new Bitmap(filePath);
                Width = bitmap.Width;
                Height = bitmap.Height;

                NeedDraw = true;
            }
        }

        public void LoadPicture(Bitmap pic)
        {
            setLastPoint();

            bitmap = pic;
            Width = bitmap.Width;
            Height = bitmap.Height;

            NeedDraw = true;
        }

        private Bitmap getGrayBitmap()
        {
            byte maxColor;
            byte minColor;
            if (bitmap != null)
            {
                maxColor = byte.MinValue;
                minColor = byte.MaxValue;
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        //取图片当前的像素点
                        var color = bitmap.GetPixel(i, j);
                        var gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                        if (gray > maxColor)
                        {
                            maxColor = (byte)gray;
                        }
                        if (gray < minColor)
                        {
                            minColor = (byte)gray;
                        }
                        //重新设置当前的像素点
                        bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    }
                }
                byte temp = (byte)((maxColor - minColor) / 4);
                customColorPiovt[0] = (byte)(minColor + temp);
                customColorPiovt[1] = (byte)(minColor + 2 * temp);
                customColorPiovt[2] = (byte)(minColor + 3 * temp);
            }
            return bitmap;
        }


        private byte GetPixelColor(byte color, bool useCustomColorPivot = false)
        {
            if (useCustomColorPivot)
            {
                if (color <= customColorPiovt[0])
                {
                    color = 0x00;
                }
                else if (color > customColorPiovt[0] && color <= customColorPiovt[1])
                {
                    color = 0x01;
                }
                else if (color > customColorPiovt[1] && color <= customColorPiovt[2])
                {
                    color = 0x02;
                }
                else
                {
                    color = 0x03;
                }
            }
            else
            {
                if (color <= defaultColorPiovt[0])
                {
                    color = 0x00;
                }
                else if (color > defaultColorPiovt[0] && color <= defaultColorPiovt[1])
                {
                    color = 0x01;
                }
                else if (color > defaultColorPiovt[1] && color <= defaultColorPiovt[2])
                {
                    color = 0x02;
                }
                else
                {
                    color = 0x03;
                }
            }
            return color;
        }

        private int colorCount = 0;
        private byte lastColor = 0x03;
        private Point lastPoint = new Point();

        public int Left
        {
            get
            {
                return left;
            }

            set
            {
                left = value;
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
            }
        }

        public void Draw()
        {
            if (bitmap == null)
            {
                return;
            }

            setLastPoint();

            if (!SkipPreProceed)
            {
                try
                {
                    getGrayBitmap();
                }
                catch (Exception)
                {
                    bitmap = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb);
                    getGrayBitmap();
                }
            }

            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                //初始化判断颜色是否相同的临时变量
                colorCount = 0;
                lastColor = 0x03;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte color = GetPixelColor(bitmap.GetPixel(x, y).R, !SkipPreProceed);

                        if (lastColor != color)
                        {
                            colorCount++;
                            if (colorCount % 2 == 1)
                            {
                                //线的开始，记录点
                                lastPoint = new Point(x, y);
                                lastColor = color;
                                XingKongScreen.SetColor((XingKongScreen.XColor)color);
                            }
                            else if (colorCount != 0 && lastColor != 0x03)
                            {
                                //线的结束，画线
                                XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, y)));
                            }
                        }

                    }
                    if (colorCount % 2 != 0 && lastColor != 0x03)
                    {
                        XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, bitmap.Height)));
                        colorCount = 0;
                        lastColor = 0x03;
                    }
                }
                XingKongScreen.SetColor();//恢复初始调色板
                NeedDraw = false;
            }
        }

        private Point getOffsetPoint(Point p1)
        {
            Point p = new Point(p1.X + Left, p1.Y + Top);
            return p;
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
            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                XingKongScreen.SetColor(XingKongScreen.XColor.White);
                XingKongScreen.FillSquare(lastP1, lastP2);
                XingKongScreen.SetColor();
            }
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
