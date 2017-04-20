using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XingKongForm
{
    public class XingKongWindow
    {
        public Dictionary<string, IDrawable> ControlsSet;

        public string Name;

        public void AddChild(IDrawable control)
        {
            if (ControlsSet == null)
            {
                ControlsSet = new Dictionary<string, IDrawable>();
            }
            string name = control.getName();
            if (!string.IsNullOrWhiteSpace(name))
            {
                ControlsSet.Add(name, control);
            }
            else
            {
                Console.WriteLine("Warning: ignored a non-named control.");
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
        }

        public class Entity
        {
            public string Name;
            public List<XingKongButton> buttonSet;
            public List<XingKongImageBox> imageSet;
            public List<XingKongLabel> labelSet;
            public List<XingKongListBox> listSet;
            public List<XingKongPanel.Entity> panelSet;
        }

        public Entity GetEntity()
        {
            Entity entity = new Entity();
            entity.Name = this.Name;
            entity.buttonSet = new List<XingKongButton>();
            entity.imageSet = new List<XingKongImageBox>();
            entity.labelSet = new List<XingKongLabel>();
            entity.listSet = new List<XingKongListBox>();
            entity.panelSet = new List<XingKongPanel.Entity>();

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
                else if (control is XingKongPanel)
                {
                    entity.panelSet.Add((control as XingKongPanel).GetEntity());
                }
                else if (control is XingKongListBox)
                {
                    entity.listSet.Add(control as XingKongListBox);
                }
            }
            return entity;
        }

        public void SetEntity(Entity entity)
        {
            this.Name = entity.Name;

            if (entity.panelSet != null)
            {
                foreach (var panelEntity in entity.panelSet)
                {
                    XingKongPanel panel = new XingKongPanel();
                    panel.SetEntity(panelEntity);
                    AddChild(panel);
                }
            }
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
            if (entity.listSet != null)
            {
                foreach (var list in entity.listSet)
                {
                    AddChild(list);
                }
            }
        }
    }
}
