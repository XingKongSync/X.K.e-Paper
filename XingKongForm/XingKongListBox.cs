using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongListBox : IDrawable
    {
        public string Name;
        public int Left;
        public int Top;
        private int width;
        private int height;
        private List<string> items;
        private bool NeedDraw = true;
        private XingKongScreen.XFontSize fontSize = XingKongScreen.XFontSize.Normal;
        private int selectedIndex = -1;//-1代表什么都没有选中
        private int scope;//可见区域内能容纳多少项目
        private List<XingKongButton> btItems;//利用button来组成列表内容
        private int firstVisibleItemIndex = 0;//第一个可见内容的索引

        //单个列表项的高度
        private int ItemHeight
        {
            get
            {
                return getTextHeight(fontSize) + 20;
            }
        }

        /// <summary>
        /// 列表中的项目
        /// </summary>
        public List<string> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
                NeedDraw = true;
                Refresh();
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
                calculateScope();//重新计算可见区域范围
            }
        }

        public XingKongScreen.XFontSize FontSize
        {
            get
            {
                return fontSize;
            }

            set
            {
                fontSize = value;
                NeedDraw = true;
                calculateScope();//重新计算可见区域范围
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                selectedIndex = value;
            }
        }

        public int getLeft()
        {
            return Left;
        }

        public string getName()
        {
            return Name;
        }

        public int getTop()
        {
            return Top;
        }


        public void setLeft(int left)
        {
            Left = left;
            NeedDraw = true;
        }

        public void setName(string name)
        {
            this.Name = name;
        }

        public void setTop(int top)
        {
            Top = top;
            NeedDraw = true;
        }
        public void HardworkDraw()
        {
            Draw();
        }

        public bool needDraw()
        {
            return NeedDraw;
        }

        /// <summary>
        /// 计算当前可见区域能容纳的项目数量
        /// </summary>
        private void calculateScope()
        {
            scope = height / ItemHeight;
            Logdata(string.Format("height:{0} itemHeight:{1} scope:{2}", height, ItemHeight, scope));
        }

        private int getTextHeight(XingKongScreen.XFontSize fontSize)
        {
            int fontsize = 32;
            switch (fontSize)
            {
                case XingKongScreen.XFontSize.Large:
                    fontsize = 48;
                    break;
                case XingKongScreen.XFontSize.ExtraLarge:
                    fontsize = 64;
                    break;
                default:
                    break;
            }
            return fontsize;
        }


        /// <summary>
        /// 使Items项目生效
        /// </summary>
        public void Refresh()
        {
            calculateScope();

            btItems = new List<XingKongButton>();
            for (int i = 0; i < scope; i++)
            {
                XingKongButton bt = new XingKongButton();
                bt.Name = "button" + i;
                bt.Left = Left;
                bt.Top = Top + i * ItemHeight;
                bt.Height = ItemHeight;
                bt.Width = width;
                bt.FontSize = FontSize;
                bt.TextAlign = XingKongScreen.TextAlign.Left;
                bt.IsChecked = false;
                btItems.Add(bt);
            }
        }

        public void ClearArea()
        {
            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                XingKongScreen.SetColor(XingKongScreen.XColor.White);
                XingKongScreen.FillSquare(new Point(Left, Top), new Point(Left + width, Top + height));
                XingKongScreen.SetColor();
            }
        }

        public void Draw()
        {
            XingKongScreen.DrawSquare(new Point(Left, Top), new Point(Left + width, Top + height));
            for (int i = 0; i < scope && i < items.Count; i++)
            {
                int itemIndex = i + firstVisibleItemIndex;
                btItems[i].Text = items[itemIndex];
                if (itemIndex == selectedIndex)
                {
                    btItems[i].IsChecked = true;
                }
                btItems[i].Draw();
                btItems[i].IsChecked = false;
            }
        }

        private void adjustVisibleWindow()
        {
            if (selectedIndex < firstVisibleItemIndex)
            {
                firstVisibleItemIndex = selectedIndex;
            }
            else if (selectedIndex >= (firstVisibleItemIndex + scope))
            {
                firstVisibleItemIndex = selectedIndex - scope + 1;
            }
            Logdata(string.Format("selectedIndex:{0} firstVisibleItemIndex:{1}", selectedIndex, firstVisibleItemIndex));
        }

        public void SelectPrevious()
        {
            selectedIndex--;
            SelectedIndex = selectedIndex % items.Count;
            if (selectedIndex < 0)
            {
                SelectedIndex += items.Count;
                selectedIndex++;
            }
            if (selectedIndex < 0)
            {
                SelectedIndex = -1;
                firstVisibleItemIndex = 0;
            }
            adjustVisibleWindow();
        }

        public void SelectNext()
        {
            selectedIndex++;
            SelectedIndex = selectedIndex % items.Count;

            adjustVisibleWindow();
        }

        private void Logdata(string data)
        {
            Console.Write("[XingKongListBox]\t");
            Console.WriteLine(data);
        }
    }
}
