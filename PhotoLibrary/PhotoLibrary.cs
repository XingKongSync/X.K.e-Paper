using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XingKongApp;
using XingKongForm;
using XingKongUtils;

namespace PhotoLibrary
{
    namespace XingKongApp
    {
        public class PhotoLibrary : App
        {
            private string appPath;
            private XingKongWindow currentWindow;
            private bool inPicViewMode;
            private Keyboard keyboard;
            private XingKongLabel lbCaption;
            private XingKongLabel lbHint;
            private XingKongListBox lbPicList;

            public override string GetName()
            {
                return "照片图库";
            }

            public List<string> GetPicFileNames()
            {
                List<string> list = new List<string>();
                string[] files = Directory.GetFiles(this.appPath);
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    string str2 = fileName.ToLower();
                    if ((str2.EndsWith(".jpg") || str2.EndsWith(".bmp")) || str2.EndsWith(".png"))
                    {
                        list.Add(fileName);
                    }
                }
                return list;
            }

            public override void Init()
            {
                appPath = Path.Combine("Apps", "PhotoLibrary");
                currentWindow = new XingKongWindow();
                XingKongWindow.Entity entity = JsonConvert.DeserializeObject<XingKongWindow.Entity>(File.ReadAllText(Path.Combine(this.appPath, "MainForm.json")));
                currentWindow.SetEntity(entity);
                lbHint = this.currentWindow.ControlsSet["lbHint"] as XingKongLabel;
                lbCaption = this.currentWindow.ControlsSet["lbCaption"] as XingKongLabel;
                lbPicList = this.currentWindow.ControlsSet["lbPicList"] as XingKongListBox;
                lbPicList.Items = this.GetPicFileNames();
                lbPicList.Refresh();
                keyboard = XingKongScreen.GetKeyboard();
                keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
                XingKongScreen.ClearScreen();
                currentWindow.HardworkDraw();
                XingKongScreen.FreshScreen();
            }

            private void Keyboard_KeyPressed(Keys pressedKey)
            {
                if (pressedKey <= Keys.Escape)
                {
                    if (pressedKey != Keys.Enter)
                    {
                        if (pressedKey == Keys.Escape)
                        {
                            this.LogData("inPicViewMode: " + (this.inPicViewMode ? "true" : "false"));
                            if (this.inPicViewMode)
                            {
                                this.inPicViewMode = false;
                                XingKongScreen.ClearScreen();
                                this.currentWindow.HardworkDraw();
                                XingKongScreen.FreshScreen();
                            }
                            else
                            {
                                this.Suspend();
                                this.Quit();
                            }
                        }
                    }
                    else if (!this.inPicViewMode)
                    {
                        XingKongImageBox control = new XingKongImageBox();
                        string data = Path.Combine(this.appPath, this.lbPicList.Items[this.lbPicList.SelectedIndex]);
                        this.LogData(data);
                        ImageHelper.Size size = ImageHelper.getPictureSize(data);
                        this.LogData(string.Format("Width:{0} Height:{1}", size.Width, size.Height));
                        if ((size.Height > 600) || (size.Width > 800))
                        {
                            Bitmap pic = ImageHelper.Scale(new Bitmap(data), 600, 800);
                            control.LoadPicture(pic);
                        }
                        else
                        {
                            control.LoadPicture(data);
                        }
                        control.Name = "imageBox1";
                        control.Left = (800 - control.Width) / 2;
                        control.Top = (600 - control.Height) / 2;
                        //control.SkipPreProceed = true;
                        XingKongWindow window1 = new XingKongWindow();
                        window1.AddChild(control);
                        this.inPicViewMode = true;
                        XingKongScreen.ClearScreen();
                        window1.Draw();
                        XingKongScreen.FreshScreen();
                    }
                }
                else
                {
                    if (pressedKey != Keys.Up)
                    {
                        if (pressedKey != Keys.Down)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!this.inPicViewMode)
                        {
                            this.lbPicList.SelectPrevious();
                            this.lbPicList.Draw();
                            XingKongScreen.FreshScreen();
                        }
                        return;
                    }
                    if (!this.inPicViewMode)
                    {
                        this.lbPicList.SelectNext();
                        this.lbPicList.Draw();
                        XingKongScreen.FreshScreen();
                    }
                }
            }

            private void LogData(string data)
            {
                Console.Write("[PhotoLirary]\t");
                Console.WriteLine(data);
            }

            public override void Resume()
            {
                this.keyboard.KeyPressed += new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
            }

            public override void Suspend()
            {
                this.keyboard.KeyPressed -= new Keyboard.KeyPressedHandler(this.Keyboard_KeyPressed);
            }
        }
    }

}
