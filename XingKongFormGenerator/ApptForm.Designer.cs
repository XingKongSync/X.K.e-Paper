namespace XingKongFormGenerator
{
    partial class ApptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApptForm));
            this.lbApps = new System.Windows.Forms.ListBox();
            this.lbAllApps = new System.Windows.Forms.Label();
            this.btBack = new System.Windows.Forms.Button();
            this.btLaunch = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lbApps
            // 
            this.lbApps.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbApps.FormattingEnabled = true;
            this.lbApps.ItemHeight = 20;
            this.lbApps.Location = new System.Drawing.Point(20, 89);
            this.lbApps.Name = "lbApps";
            this.lbApps.Size = new System.Drawing.Size(636, 484);
            this.lbApps.TabIndex = 0;
            // 
            // lbAllApps
            // 
            this.lbAllApps.AutoSize = true;
            this.lbAllApps.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAllApps.Location = new System.Drawing.Point(90, 15);
            this.lbAllApps.Name = "lbAllApps";
            this.lbAllApps.Size = new System.Drawing.Size(212, 48);
            this.lbAllApps.TabIndex = 1;
            this.lbAllApps.Text = "所有应用";
            // 
            // btBack
            // 
            this.btBack.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btBack.Location = new System.Drawing.Point(674, 527);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(113, 46);
            this.btBack.TabIndex = 2;
            this.btBack.Text = "返回";
            this.btBack.UseVisualStyleBackColor = true;
            // 
            // btLaunch
            // 
            this.btLaunch.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btLaunch.Location = new System.Drawing.Point(674, 475);
            this.btLaunch.Name = "btLaunch";
            this.btLaunch.Size = new System.Drawing.Size(113, 46);
            this.btLaunch.TabIndex = 3;
            this.btLaunch.Text = "启动";
            this.btLaunch.UseVisualStyleBackColor = true;
            // 
            // pbIcon
            // 
            this.pbIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbIcon.Image")));
            this.pbIcon.Location = new System.Drawing.Point(20, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(64, 64);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIcon.TabIndex = 4;
            this.pbIcon.TabStop = false;
            // 
            // ApptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 590);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.btLaunch);
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.lbAllApps);
            this.Controls.Add(this.lbApps);
            this.Name = "ApptForm";
            this.Text = "AppForm";
            this.Load += new System.EventHandler(this.TestListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbApps;
        private System.Windows.Forms.Label lbAllApps;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.Button btLaunch;
        private System.Windows.Forms.PictureBox pbIcon;
    }
}