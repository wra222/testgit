namespace IMES_Reports
{
    partial class Fly_Form
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
            this.L_Rate = new System.Windows.Forms.Label();
            this.L_Title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // L_Rate
            // 
            this.L_Rate.AutoSize = true;
            this.L_Rate.BackColor = System.Drawing.Color.SeaGreen;
            this.L_Rate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.L_Rate.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.L_Rate.ForeColor = System.Drawing.Color.White;
            this.L_Rate.Location = new System.Drawing.Point(6, 25);
            this.L_Rate.Name = "L_Rate";
            this.L_Rate.Size = new System.Drawing.Size(26, 16);
            this.L_Rate.TabIndex = 0;
            this.L_Rate.Text = "11";
            this.L_Rate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.L_Rate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.L_Rate.MouseHover += new System.EventHandler(this.Fly_From_MouseHover);
            this.L_Rate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // L_Title
            // 
            this.L_Title.AutoSize = true;
            this.L_Title.BackColor = System.Drawing.Color.Green;
            this.L_Title.Cursor = System.Windows.Forms.Cursors.Hand;
            this.L_Title.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.L_Title.ForeColor = System.Drawing.Color.White;
            this.L_Title.Location = new System.Drawing.Point(62, 24);
            this.L_Title.Name = "L_Title";
            this.L_Title.Size = new System.Drawing.Size(23, 15);
            this.L_Title.TabIndex = 1;
            this.L_Title.Text = "11";
            this.L_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.L_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.L_Title.MouseHover += new System.EventHandler(this.Fly_From_MouseHover);
            this.L_Title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // Fly_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(417, 105);
            this.Controls.Add(this.L_Title);
            this.Controls.Add(this.L_Rate);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "Fly_Form";
            this.Opacity = 0.8;
            this.ShowInTaskbar = false;
            this.Text = "Fly_From";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Fly_From_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_Rate;
        private System.Windows.Forms.Label L_Title;


    }
}