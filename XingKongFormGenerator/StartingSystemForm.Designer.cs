namespace XingKongFormGenerator
{
    partial class StartingSystemForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingSystemForm));
            this.lbStarting = new System.Windows.Forms.Label();
            this.lbCompany = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pbCopyright = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).BeginInit();
            this.SuspendLayout();
            // 
            // lbStarting
            // 
            this.lbStarting.AutoSize = true;
            this.lbStarting.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStarting.Location = new System.Drawing.Point(296, 332);
            this.lbStarting.Name = "lbStarting";
            this.lbStarting.Size = new System.Drawing.Size(155, 35);
            this.lbStarting.TabIndex = 1;
            this.lbStarting.Text = "正在启动";
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCompany.Location = new System.Drawing.Point(280, 547);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(230, 21);
            this.lbCompany.TabIndex = 2;
            this.lbCompany.Text = "XingKong Corporation";
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(136, 154);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(540, 93);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            // 
            // pbCopyright
            // 
            this.pbCopyright.Image = ((System.Drawing.Image)(resources.GetObject("pbCopyright.Image")));
            this.pbCopyright.Location = new System.Drawing.Point(249, 550);
            this.pbCopyright.Name = "pbCopyright";
            this.pbCopyright.Size = new System.Drawing.Size(28, 29);
            this.pbCopyright.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbCopyright.TabIndex = 4;
            this.pbCopyright.TabStop = false;
            // 
            // StartingSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 601);
            this.Controls.Add(this.pbCopyright);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lbCompany);
            this.Controls.Add(this.lbStarting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StartingSystemForm";
            this.Text = "StartingSystemForm";
            this.Load += new System.EventHandler(this.StartingSystemForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCopyright)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbStarting;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.PictureBox pbCopyright;
    }
}