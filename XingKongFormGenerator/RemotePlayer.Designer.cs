namespace XingKongFormGenerator
{
    partial class RemotePlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemotePlayer));
            this.pbAlbum = new System.Windows.Forms.PictureBox();
            this.lbNowPlaying = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbSinger = new System.Windows.Forms.Label();
            this.lbAlbum = new System.Windows.Forms.Label();
            this.lbExit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbAlbum)).BeginInit();
            this.SuspendLayout();
            // 
            // pbAlbum
            // 
            this.pbAlbum.Image = ((System.Drawing.Image)(resources.GetObject("pbAlbum.Image")));
            this.pbAlbum.Location = new System.Drawing.Point(12, 139);
            this.pbAlbum.Name = "pbAlbum";
            this.pbAlbum.Size = new System.Drawing.Size(300, 300);
            this.pbAlbum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAlbum.TabIndex = 0;
            this.pbAlbum.TabStop = false;
            // 
            // lbNowPlaying
            // 
            this.lbNowPlaying.AutoSize = true;
            this.lbNowPlaying.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNowPlaying.Location = new System.Drawing.Point(335, 138);
            this.lbNowPlaying.Name = "lbNowPlaying";
            this.lbNowPlaying.Size = new System.Drawing.Size(212, 48);
            this.lbNowPlaying.TabIndex = 1;
            this.lbNowPlaying.Text = "正在播放";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(339, 272);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(225, 35);
            this.lbName.TabIndex = 2;
            this.lbName.Text = "決意のダイヤ";
            // 
            // lbSinger
            // 
            this.lbSinger.AutoSize = true;
            this.lbSinger.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSinger.Location = new System.Drawing.Point(339, 342);
            this.lbSinger.Name = "lbSinger";
            this.lbSinger.Size = new System.Drawing.Size(346, 24);
            this.lbSinger.TabIndex = 3;
            this.lbSinger.Text = "烏丸千歳、久我山八重、片倉京";
            // 
            // lbAlbum
            // 
            this.lbAlbum.AutoSize = true;
            this.lbAlbum.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAlbum.Location = new System.Drawing.Point(339, 395);
            this.lbAlbum.Name = "lbAlbum";
            this.lbAlbum.Size = new System.Drawing.Size(70, 24);
            this.lbAlbum.TabIndex = 4;
            this.lbAlbum.Text = "Bloom";
            // 
            // lbExit
            // 
            this.lbExit.AutoSize = true;
            this.lbExit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbExit.Location = new System.Drawing.Point(12, 565);
            this.lbExit.Name = "lbExit";
            this.lbExit.Size = new System.Drawing.Size(70, 14);
            this.lbExit.TabIndex = 5;
            this.lbExit.Text = "退出：Esc";
            // 
            // RemotePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 588);
            this.Controls.Add(this.lbExit);
            this.Controls.Add(this.lbAlbum);
            this.Controls.Add(this.lbSinger);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lbNowPlaying);
            this.Controls.Add(this.pbAlbum);
            this.Name = "RemotePlayer";
            this.Text = "RemotePlayer";
            this.Load += new System.EventHandler(this.RemotePlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbAlbum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAlbum;
        private System.Windows.Forms.Label lbNowPlaying;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbSinger;
        private System.Windows.Forms.Label lbAlbum;
        private System.Windows.Forms.Label lbExit;
    }
}