namespace RemoteController
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btLeft = new System.Windows.Forms.Button();
            this.btRight = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btConnect = new System.Windows.Forms.Button();
            this.tbKeybordInput = new System.Windows.Forms.TextBox();
            this.cbIpList = new System.Windows.Forms.ComboBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btUp
            // 
            this.btUp.Location = new System.Drawing.Point(103, 97);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(60, 60);
            this.btUp.TabIndex = 0;
            this.btUp.Text = "↑";
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btDown
            // 
            this.btDown.Location = new System.Drawing.Point(103, 163);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(60, 60);
            this.btDown.TabIndex = 1;
            this.btDown.Text = "↓";
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btLeft
            // 
            this.btLeft.Location = new System.Drawing.Point(37, 163);
            this.btLeft.Name = "btLeft";
            this.btLeft.Size = new System.Drawing.Size(60, 60);
            this.btLeft.TabIndex = 2;
            this.btLeft.Text = "←";
            this.btLeft.UseVisualStyleBackColor = true;
            this.btLeft.Click += new System.EventHandler(this.btLeft_Click);
            // 
            // btRight
            // 
            this.btRight.Location = new System.Drawing.Point(169, 163);
            this.btRight.Name = "btRight";
            this.btRight.Size = new System.Drawing.Size(60, 60);
            this.btRight.TabIndex = 3;
            this.btRight.Text = "→";
            this.btRight.UseVisualStyleBackColor = true;
            this.btRight.Click += new System.EventHandler(this.btRight_Click);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(289, 97);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(60, 60);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(355, 97);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(60, 60);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "IP：";
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(253, 18);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(75, 23);
            this.btConnect.TabIndex = 8;
            this.btConnect.Text = "连接";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // tbKeybordInput
            // 
            this.tbKeybordInput.Location = new System.Drawing.Point(289, 184);
            this.tbKeybordInput.Name = "tbKeybordInput";
            this.tbKeybordInput.Size = new System.Drawing.Size(100, 21);
            this.tbKeybordInput.TabIndex = 9;
            this.tbKeybordInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbKeybordInput_KeyUp);
            // 
            // cbIpList
            // 
            this.cbIpList.FormattingEnabled = true;
            this.cbIpList.Location = new System.Drawing.Point(66, 18);
            this.cbIpList.Name = "cbIpList";
            this.cbIpList.Size = new System.Drawing.Size(100, 20);
            this.cbIpList.TabIndex = 10;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(172, 18);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 11;
            this.btSearch.Text = "搜索";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 261);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.cbIpList);
            this.Controls.Add(this.tbKeybordInput);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btRight);
            this.Controls.Add(this.btLeft);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btUp);
            this.Name = "Form1";
            this.Text = "XingKongOS远程键盘";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btLeft;
        private System.Windows.Forms.Button btRight;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.TextBox tbKeybordInput;
        private System.Windows.Forms.ComboBox cbIpList;
        private System.Windows.Forms.Button btSearch;
    }
}

