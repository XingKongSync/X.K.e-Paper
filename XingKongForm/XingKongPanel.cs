using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongPanel : IDrawable
    {
        public string Name;
        private int width;
        private int height;
        private int left;
        private int top;
        private bool NeedDraw = true;

        public static bool IsDebugEnabled = false;

        public Dictionary<string, IDrawable> ControlsSet;

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

        public void AddChild(IDrawable control)
        {
            if (ControlsSet == null)
            {
                ControlsSet = new Dictionary<string, IDrawable>();
            }
            string name = control.getName();
            if (!string.IsNullOrWhiteSpace(name))
            {
                control.setLeft(control.getLeft());
                control.setTop(control.getTop());
                ControlsSet.Add(name, control);
                NeedDraw = true;
            }
            else
            {
                Console.WriteLine("Warning: ignored a non-named control.");
            }
        }

        public void ClearArea()
        {
            object asyncLockObj = XingKongScreen.AsyncLockObj;
            lock (asyncLockObj)
            {
                if (IsDebugEnabled)
                {
                    XingKongScreen.SetColor(XingKongScreen.XColor.Black);
                }
                else
                {
                    XingKongScreen.SetColor(XingKongScreen.XColor.White);
                }
                XingKongScreen.FillSquare(new Point(left, top), new Point(left + width, top + height));
                XingKongScreen.SetColor();
            }
        }

        public void HardworkDraw()
        {
            if (ControlsSet != null)
            {
                foreach (IDrawable control in ControlsSet.Values)
                {
                    control.HardworkDraw();
                }
            }
            NeedDraw = false;
        }

        public void LazyDraw()
        {
            if (ControlsSet != null)
            {
                foreach (IDrawable control in ControlsSet.Values)
                {
                    if (control.needDraw())
                    {
                        control.ClearArea();
                    }
                }
                foreach (IDrawable control in ControlsSet.Values)
                {
                    if (control.needDraw())
                    {
                        control.Draw();
                    }
                }
            }
        }

        public void Draw()
        {
            if (ControlsSet != null)
            {
                bool canLazyDraw = true;
                foreach (IDrawable control in ControlsSet.Values)
                {
                    if (control is XingKongLabel && control.needDraw())
                    {
                        canLazyDraw = false;
                        break;
                    }
                }
                if (canLazyDraw)
                {
                    LazyDraw();
                }
                else
                {
                    HardworkDraw();
                }
            }
            NeedDraw = false;
        }

        public bool needDraw()
        {
            if (NeedDraw)
            {
                return true;
            }
            else
            {
                foreach (IDrawable control in ControlsSet.Values)
                {
                    if (control.needDraw())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void setName(string name)
        {
            Name = name;
        }

        public string getName()
        {
            return Name;
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

        public class Entity
        {
            public string Name;
            public int Left;
            public int Top;
            public int Width;
            public int Height;
            public List<XingKongButton> buttonSet;
            public List<XingKongImageBox> imageSet;
            public List<XingKongLabel> labelSet;
        }

        public Entity GetEntity()
        {
            Entity entity = new Entity();
            entity.Name = this.Name;
            entity.Left = left;
            entity.Height = height;
            entity.Width = width;
            entity.Top = top;
            entity.buttonSet = new List<XingKongButton>();
            entity.imageSet = new List<XingKongImageBox>();
            entity.labelSet = new List<XingKongLabel>();

            foreach (IDrawable control in ControlsSet.Values)
            {
                if (control is XingKongButton)
                {
                    entity.buttonSet.Add(control as XingKongButton);
                }
                else if (control is XingKongImageBox)
                {
                    entity.imageSet.Add(control as XingKongImageBox);
                }
                else if (control is XingKongLabel)
                {
                    entity.labelSet.Add(control as XingKongLabel);
                }
            }
            return entity;
        }

        public void SetEntity(Entity entity)
        {
            this.Name = entity.Name;
            this.Top = entity.Top;
            this.Left = entity.Left;
            this.Height = entity.Height;
            this.Width = entity.Width;

            if (entity.buttonSet != null)
            {
                foreach (var button in entity.buttonSet)
                {
                    AddChild(button);
                }
            }
            if (entity.imageSet != null)
            {
                foreach (var image in entity.imageSet)
                {
                    AddChild(image);
                }
            }
            if (entity.labelSet != null)
            {
                foreach (var label in entity.labelSet)
                {
                    AddChild(label);
                }
            }
        }
    }
}
