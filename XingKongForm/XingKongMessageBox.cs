using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongMessageBox
    {
        private XingKongLabel lbCaption;
        private XingKongLabel lbTitle;
        public XingKongButton btOk;
        public XingKongButton btCancel;

        private XingKongForm.Keyboard keyboard;

        private DialogResult result;
        private bool canExit;

        public Style DialogStyle;

        public enum DialogResult
        {
            OK,
            Cancel
        }

        public enum Style
        {
            OkCancel,
            OK
        }

        public string Caption
        {
            set
            {
                lbCaption.Text = value; 
            }

            get
            {
                return lbCaption.Text;
            }
        }

        public string Title
        {
            set
            {
                lbTitle.Text = value;
            }

            get
            {
                return lbTitle.Text;
            }
        }

        public XingKongMessageBox()
        {
            lbCaption = new XingKongLabel();
            lbTitle = new XingKongLabel();
            btOk = new XingKongButton();
            btCancel = new XingKongButton();
            if (XingKongScreen.IsRunningOnMono())
            {
                btOk.TextXoffset = -9;
                btCancel.TextXoffset = -9;
            }
            else
            {
                btOk.TextXoffset = -2;
                btCancel.TextXoffset = -2;
            }
            btOk.Text = "确定";
            btCancel.Text = "取消";
            btOk.Width = 128;
            btOk.Height = 60;
            btCancel.Width = 128;
            btCancel.Height = 60;

            btOk.IsChecked = true; ;
            btCancel.IsChecked = false;

            DialogStyle = Style.OkCancel;
        }
        
        /*
         ______________________
         |_Caption____________|
         |                    |
         |        Title       |
         |  | Ok |   |Cancel| |
         |____________________|
             */

        public async Task<DialogResult> ShowAsync()
        {
            Func<DialogResult> fun = new Func<DialogResult>(Show);
            return await Task.Run<DialogResult>(fun);
        }

        public DialogResult Show()
        {
            canExit = false;

            int[] widths = new int[3];
            widths[0] = XingKongScreen.MeasureStringWidth(Caption, lbCaption.FontSize);
            widths[1] = XingKongScreen.MeasureStringWidth(Title, lbTitle.FontSize);
            widths[2] = 128 * 2 + 40;

            int maxWidth = widths[0];
            foreach (int w in widths)
            {
                if (w > maxWidth)
                {
                    maxWidth = w;
                }
            }

            int actWidth = maxWidth + 150;
            int actHeight = actWidth * 3 / 4;
            int left = (800 - actWidth) / 2;
            int top = (600 - actHeight) / 2;

            lbCaption.Left = (short)(20 + left);
            lbCaption.Top = (short)(20 + top);

            Point pLeftTopOfBox = new Point(left, top);
            Point pRightButtomOfBox = new Point(left + actWidth, top + actHeight);

            XingKongScreen.SetColor(XingKongScreen.XColor.White);
            XingKongScreen.FillSquare(pLeftTopOfBox, pRightButtomOfBox);
            XingKongScreen.SetColor();
            XingKongScreen.DrawSquare(pLeftTopOfBox, pRightButtomOfBox);

            int lbCaptionFontHeight = getFontHeightPixel(lbCaption.FontSize);
            int lbTitleFontHeight = getFontHeightPixel(lbTitle.FontSize);
            XingKongScreen.DrawLine(new Point(left, 40 + top + lbCaptionFontHeight), new Point(left + actWidth, 40 + top + lbTitleFontHeight));

            lbTitle.Top = (short)((actHeight / 5) + 40 + top + lbCaptionFontHeight);
            lbTitle.Left = (short)(((actWidth - XingKongScreen.MeasureStringWidth(lbTitle.Text, lbTitle.FontSize)) / 2) + left);

            int btOkFontHeight = getFontHeightPixel(btOk.FontSize);
            int btCancelFontHeight = getFontHeightPixel(btCancel.FontSize);
            if (DialogStyle == Style.OkCancel)
            {
                btOk.Left = (actWidth - widths[2]) / 2 + left;
                btOk.Top = (short)((actHeight * 3 / 5) + top + btCancelFontHeight);

                btCancel.Left = btOk.Left + btOk.Width + 40;
                btCancel.Top = btOk.Top;

                btOk.Draw();
                btCancel.Draw();
            }
            else
            {
                btOk.Left = (actWidth - btOk.Width) / 2 + left;
                btOk.Top = (short)((actHeight * 3 / 5) + top + btCancelFontHeight);
                btOk.Draw();
            }


            lbCaption.Draw();
            lbTitle.Draw();
            XingKongScreen.FreshScreen();

            if (btOk.IsChecked)
            {
                result = DialogResult.OK;
            }
            else
            {
                result = DialogResult.Cancel;
            }

            keyboard = XingKongScreen.GetKeyboard();
            keyboard.KeyPressed += Keyboard_KeyPressed;
            while (!canExit)
            {
                Thread.Sleep(10);
            }
            keyboard.KeyPressed -= Keyboard_KeyPressed;
            return result;
        }

        private void Keyboard_KeyPressed(System.Windows.Forms.Keys pressedKey)
        {
            switch (pressedKey)
            {
                case System.Windows.Forms.Keys.Left:
                case System.Windows.Forms.Keys.Right:
                    if (DialogStyle == Style.OkCancel)
                    {
                        if (btOk.IsChecked)
                        {
                            btOk.IsChecked = false;
                            btCancel.IsChecked = true;
                        }
                        else
                        {
                            btOk.IsChecked = true;
                            btCancel.IsChecked = false;
                        }
                        btOk.Draw();
                        btCancel.Draw();
                        XingKongScreen.FreshScreen();
                    }
                    break;
                case System.Windows.Forms.Keys.Enter:
                    canExit = true;
                    if (btOk.IsChecked)
                    {
                        result = DialogResult.OK;
                    }
                    else
                    {
                        result = DialogResult.Cancel;
                    }
                    break;
                case System.Windows.Forms.Keys.Escape:
                    canExit = true;
                    result = DialogResult.Cancel;
                    break;
                default:
                    break;
            }
        }

        private int getFontHeightPixel(XingKongScreen.XFontSize fontSize)
        {
            int height = 32;
            switch (fontSize)
            {
                case XingKongScreen.XFontSize.Normal:
                    height = 32;
                    break;
                case XingKongScreen.XFontSize.Large:
                    height = 48;
                    break;
                case XingKongScreen.XFontSize.ExtraLarge:
                    height = 64;
                    break;
                default:
                    break;
            }
            return height;
        }
    }
}
