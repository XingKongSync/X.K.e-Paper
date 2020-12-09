using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
        //private byte[] customColorPiovt = new byte[3];

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

        //private Bitmap getGrayBitmap()
        //{
        //    byte maxColor;
        //    byte minColor;
        //    if (bitmap != null)
        //    {
        //        maxColor = byte.MinValue;
        //        minColor = byte.MaxValue;
        //        for (int i = 0; i < bitmap.Width; i++)
        //        {
        //            for (int j = 0; j < bitmap.Height; j++)
        //            {
        //                //取图片当前的像素点
        //                var color = bitmap.GetPixel(i, j);
        //                var gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
        //                if (gray > maxColor)
        //                {
        //                    maxColor = (byte)gray;
        //                }
        //                if (gray < minColor)
        //                {
        //                    minColor = (byte)gray;
        //                }
        //                //重新设置当前的像素点
        //                bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
        //            }
        //        }
        //        byte temp = (byte)((maxColor - minColor) / 4);
        //        customColorPiovt[0] = (byte)(minColor + temp);
        //        customColorPiovt[1] = (byte)(minColor + 2 * temp);
        //        customColorPiovt[2] = (byte)(minColor + 3 * temp);
        //    }
        //    return bitmap;
        //}


        private byte GetPixelColor(byte color, bool useCustomColorPivot = false)
        {
            //if (useCustomColorPivot)
            //{
            //    if (color <= customColorPiovt[0])
            //    {
            //        color = 0x00;
            //    }
            //    else if (color > customColorPiovt[0] && color <= customColorPiovt[1])
            //    {
            //        color = 0x01;
            //    }
            //    else if (color > customColorPiovt[1] && color <= customColorPiovt[2])
            //    {
            //        color = 0x02;
            //    }
            //    else
            //    {
            //        color = 0x03;
            //    }
            //}
            //else
            //{
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
            //}
            return color;
        }

        private int colorCount = 0;
        private XingKongScreen.XColor lastColor = XingKongScreen.XColor.White;
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


            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                DitheringDraw();
            }
            else
            {
                TraditionalDraw(bitmap);
            }
        }

        #region Dithering

        private void DitheringDraw()
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            try
            {
                if (!SkipPreProceed)
                    Dithering(bmpData);
                InnerDraw(bmpData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[XingKongImageBox][Draw]{ex.Message}");
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
        }

        private XingKongScreen.XColor GetPixel(BitmapData image, int x, int y)
        {
            if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
            {
                return XingKongScreen.XColor.White;
            }
            int stride = image.Stride;
            int fuck = stride / image.Width;
            IntPtr ptr = image.Scan0 + y * stride + x * fuck;

            return (XingKongScreen.XColor)(byte)(Marshal.ReadByte(ptr) / 64);
        }

        private int GetGrayPixel(BitmapData image, int x, int y)
        {
            if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
            {
                return 255;
            }
            int stride = image.Stride;
            int fuck = stride / image.Width;
            IntPtr ptr = image.Scan0 + y * stride + x * fuck;
            int color = 0;
            color += (int)(Marshal.ReadByte(ptr + 0) * 0.114);
            color += (int)(Marshal.ReadByte(ptr + 1) * 0.587);
            color += (int)(Marshal.ReadByte(ptr + 2) * 0.299);

            return color;
        }

        private void SetPixel(BitmapData image, int x, int y, int c)
        {
            if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
            {
                return;
            }
            if (c > 255)
            {
                c = 255;
            }
            else if (c < 0)
            {
                c = 0;
            }
            int stride = image.Stride;
            int fuck = stride / image.Width;
            IntPtr ptr = image.Scan0 + y * stride + x * fuck;
            Marshal.WriteByte(ptr, (byte)c);
            Marshal.WriteByte(ptr + 1, (byte)c);
            Marshal.WriteByte(ptr + 2, (byte)c);
        }

        private void Dithering(BitmapData bmpData)
        {

            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    int oldpixel = GetGrayPixel(bmpData, x, y);
                    int newpixel = FindClosestPaletteColor(oldpixel);
                    SetPixel(bmpData, x, y, newpixel);
                    int quant_error = oldpixel - newpixel;
                    SetPixel(bmpData, x + 1, y, GetGrayPixel(bmpData, x + 1, y) + quant_error * 7 / 16);
                    SetPixel(bmpData, x - 1, y + 1, GetGrayPixel(bmpData, x - 1, y + 1) + quant_error * 3 / 16);
                    SetPixel(bmpData, x, y + 1, GetGrayPixel(bmpData, x, y + 1) + quant_error * 5 / 16);
                    SetPixel(bmpData, x + 1, y + 1, GetGrayPixel(bmpData, x + 1, y + 1) + quant_error / 16);
                }
            }

        }

        private int FindClosestPaletteColor(int oldpixel)
        {
            return oldpixel / 64 * 64;
        }

        private void InnerDraw(BitmapData bitmap)
        {
            setLastPoint();
            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                //初始化判断颜色是否相同的临时变量
                colorCount = 0;
                lastColor = XingKongScreen.XColor.White;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        XingKongScreen.XColor color = GetPixel(bitmap, x, y);

                        if (lastColor != color)
                        {
                            colorCount++;
                            if (colorCount % 2 == 1)
                            {
                                //线的开始，记录点
                                lastPoint = new Point(x, y);
                                lastColor = color;
                                XingKongScreen.SetColor(color);
                            }
                            else if (colorCount != 0 && lastColor != XingKongScreen.XColor.White)
                            {
                                //线的结束，画线
                                XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, y)));
                            }
                        }

                    }
                    if (colorCount % 2 != 0 && lastColor != XingKongScreen.XColor.White)
                    {
                        XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, bitmap.Height)));
                        colorCount = 0;
                        lastColor = XingKongScreen.XColor.White;
                    }
                }
                XingKongScreen.SetColor();//恢复初始调色板
                NeedDraw = false;
            }
        }
        #endregion

        private void TraditionalDraw(Bitmap bitmap)
        {
            setLastPoint();
            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                //初始化判断颜色是否相同的临时变量
                colorCount = 0;
                lastColor = XingKongScreen.XColor.White;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        XingKongScreen.XColor color = (XingKongScreen.XColor)GetPixelColor(bitmap.GetPixel(x, y).R);

                        if (lastColor != color)
                        {
                            colorCount++;
                            if (colorCount % 2 == 1)
                            {
                                //线的开始，记录点
                                lastPoint = new Point(x, y);
                                lastColor = color;
                                XingKongScreen.SetColor(color);
                            }
                            else if (colorCount != 0 && lastColor != XingKongScreen.XColor.White)
                            {
                                //线的结束，画线
                                XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, y)));
                            }
                        }

                    }
                    if (colorCount % 2 != 0 && lastColor != XingKongScreen.XColor.White)
                    {
                        XingKongScreen.DrawLine(getOffsetPoint(lastPoint), getOffsetPoint(new Point(x, bitmap.Height)));
                        colorCount = 0;
                        lastColor = XingKongScreen.XColor.White;
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
