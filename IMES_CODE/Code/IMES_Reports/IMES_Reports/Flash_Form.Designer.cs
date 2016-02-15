using IMES_Reports.App_Code;
namespace IMES_Reports
{
    partial class Flash_Form
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.BT_QC = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BT_AddButtun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Location = new System.Drawing.Point(36, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 56);
            this.button1.TabIndex = 1;
            this.button1.Text = "生產出貨報表";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            // 
            // BT_QC
            // 
            this.BT_QC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BT_QC.ForeColor = System.Drawing.Color.Black;
            this.BT_QC.Location = new System.Drawing.Point(133, 25);
            this.BT_QC.Name = "BT_QC";
            this.BT_QC.Size = new System.Drawing.Size(55, 56);
            this.BT_QC.TabIndex = 2;
            this.BT_QC.Text = "品質良率報表";
            this.BT_QC.UseVisualStyleBackColor = false;
            this.BT_QC.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.BT_QC.Click += new System.EventHandler(this.button2_Click);
            this.BT_QC.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button3.Location = new System.Drawing.Point(238, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(55, 56);
            this.button3.TabIndex = 3;
            this.button3.Text = "投入產出報表";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(347, 25);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(55, 56);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.button4.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            // 
            // BT_AddButtun
            // 
            this.BT_AddButtun.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BT_AddButtun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BT_AddButtun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BT_AddButtun.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_AddButtun.Location = new System.Drawing.Point(347, 175);
            this.BT_AddButtun.Name = "BT_AddButtun";
            this.BT_AddButtun.Size = new System.Drawing.Size(61, 54);
            this.BT_AddButtun.TabIndex = 6;
            this.BT_AddButtun.Text = "新功能添加";
            this.BT_AddButtun.UseVisualStyleBackColor = false;
            this.BT_AddButtun.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.BT_AddButtun.Click += new System.EventHandler(this.BT_AddButtun_Click);
            this.BT_AddButtun.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            // 
            // Flash_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::IMES_Reports.Properties.Resources.wra222_1;
            this.ClientSize = new System.Drawing.Size(490, 258);
            this.Controls.Add(this.BT_AddButtun);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.BT_QC);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Flash_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Flash_From";
            this.Load += new System.EventHandler(this.Flash_From_Load);
            this.MouseLeave += new System.EventHandler(this.Flash_From_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Flash_From_MouseMove);
            this.MouseHover += new System.EventHandler(this.Flash_From_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BT_QC;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BT_AddButtun;

    }
}