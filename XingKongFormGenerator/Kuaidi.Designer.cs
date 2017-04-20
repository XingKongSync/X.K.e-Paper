namespace XingKongFormGenerator
{
    partial class Kuaidi
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
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbKuaidis = new System.Windows.Forms.ListBox();
            this.lbHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.Location = new System.Drawing.Point(20, 12);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(212, 48);
            this.lbTitle.TabIndex = 3;
            this.lbTitle.Text = "快递查询";
            // 
            // lbKuaidis
            // 
            this.lbKuaidis.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbKuaidis.FormattingEnabled = true;
            this.lbKuaidis.ItemHeight = 20;
            this.lbKuaidis.Location = new System.Drawing.Point(20, 89);
            this.lbKuaidis.Name = "lbKuaidis";
            this.lbKuaidis.Size = new System.Drawing.Size(752, 464);
            this.lbKuaidis.TabIndex = 2;
            // 
            // lbHint
            // 
            this.lbHint.AutoSize = true;
            this.lbHint.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHint.Location = new System.Drawing.Point(16, 570);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(99, 20);
            this.lbHint.TabIndex = 6;
            this.lbHint.Text = "退出：Esc";
            // 
            // Kuaidi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 588);
            this.Controls.Add(this.lbHint);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.lbKuaidis);
            this.Name = "Kuaidi";
            this.Text = "Kuaidi";
            this.Load += new System.EventHandler(this.Kuaidi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.ListBox lbKuaidis;
        private System.Windows.Forms.Label lbHint;
    }
}