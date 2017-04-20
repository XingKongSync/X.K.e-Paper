namespace XingKongFormGenerator
{
    partial class ShutdownForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShutdownForm));
            this.pbCopyright = new System.Windows.Forms.PictureBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lbCompany = new System.Windows.Forms.Label();
            this.lbMessage = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbEarth = new System.Windows.Forms.PictureBox();
            this.pbDisconnect = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEarth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisconnect)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCopyright
            // 
            this.pbCopyright.Image = ((System.Drawing.Image)(resources.GetObject("pbCopyright.Image")));
            this.pbCopyright.Location = new System.Drawing.Point(249, 550);
            this.pbCopyright.Name = "pbCopyright";
            this.pbCopyright.Size = new System.Drawing.Size(28, 29);
            this.pbCopyright.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbCopyright.TabIndex = 8;
            this.pbCopyright.TabStop = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(136, 154);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(540, 93);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 7;
            this.pbLogo.TabStop = false;
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCompany.Location = new System.Drawing.Point(280, 547);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(230, 21);
            this.lbCompany.TabIndex = 6;
            this.lbCompany.Text = "XingKong Corporation";
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.lbMessage.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMessage.Location = new System.Drawing.Point(260, 357);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(199, 21);
            this.lbMessage.TabIndex = 5;
            this.lbMessage.Text = "已断开与世界的连接";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // pbEarth
            // 
            this.pbEarth.Image = ((System.Drawing.Image)(resources.GetObject("pbEarth.Image")));
            this.pbEarth.Location = new System.Drawing.Point(166, 347);
            this.pbEarth.Name = "pbEarth";
            this.pbEarth.Size = new System.Drawing.Size(54, 54);
            this.pbEarth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbEarth.TabIndex = 10;
            this.pbEarth.TabStop = false;
            // 
            // pbDisconnect
            // 
            this.pbDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("pbDisconnect.Image")));
            this.pbDisconnect.Location = new System.Drawing.Point(583, 347);
            this.pbDisconnect.Name = "pbDisconnect";
            this.pbDisconnect.Size = new System.Drawing.Size(51, 54);
            this.pbDisconnect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDisconnect.TabIndex = 11;
            this.pbDisconnect.TabStop = false;
            // 
            // ShutdownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 601);
            this.Controls.Add(this.pbDisconnect);
            this.Controls.Add(this.pbEarth);
            this.Controls.Add(this.pbCopyright);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lbCompany);
            this.Controls.Add(this.lbMessage);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ShutdownForm";
            this.Text = "ShutdownForm";
            this.Load += new System.EventHandler(this.ShutdownForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEarth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisconnect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCopyright;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbEarth;
        private System.Windows.Forms.PictureBox pbDisconnect;
    }
}