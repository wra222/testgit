namespace TestForm
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUploadFOP = new System.Windows.Forms.Button();
            this.txtFOPLoc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtXMLLocation = new System.Windows.Forms.TextBox();
            this.btnUploadXML = new System.Windows.Forms.Button();
            this.btnUploadXSL = new System.Windows.Forms.Button();
            this.txtXSLLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSavePDF = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FOP Location";
            // 
            // btnUploadFOP
            // 
            this.btnUploadFOP.Location = new System.Drawing.Point(321, 8);
            this.btnUploadFOP.Name = "btnUploadFOP";
            this.btnUploadFOP.Size = new System.Drawing.Size(75, 23);
            this.btnUploadFOP.TabIndex = 1;
            this.btnUploadFOP.Text = "Specify";
            this.btnUploadFOP.UseVisualStyleBackColor = true;
            this.btnUploadFOP.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFOPLoc
            // 
            this.txtFOPLoc.Location = new System.Drawing.Point(92, 8);
            this.txtFOPLoc.Name = "txtFOPLoc";
            this.txtFOPLoc.Size = new System.Drawing.Size(213, 20);
            this.txtFOPLoc.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "XML Location";
            // 
            // txtXMLLocation
            // 
            this.txtXMLLocation.Location = new System.Drawing.Point(92, 45);
            this.txtXMLLocation.Name = "txtXMLLocation";
            this.txtXMLLocation.Size = new System.Drawing.Size(213, 20);
            this.txtXMLLocation.TabIndex = 4;
            // 
            // btnUploadXML
            // 
            this.btnUploadXML.Location = new System.Drawing.Point(321, 45);
            this.btnUploadXML.Name = "btnUploadXML";
            this.btnUploadXML.Size = new System.Drawing.Size(75, 23);
            this.btnUploadXML.TabIndex = 5;
            this.btnUploadXML.Text = "Specify";
            this.btnUploadXML.UseVisualStyleBackColor = true;
            this.btnUploadXML.Click += new System.EventHandler(this.btnUploadXML_Click);
            // 
            // btnUploadXSL
            // 
            this.btnUploadXSL.Location = new System.Drawing.Point(321, 80);
            this.btnUploadXSL.Name = "btnUploadXSL";
            this.btnUploadXSL.Size = new System.Drawing.Size(75, 23);
            this.btnUploadXSL.TabIndex = 8;
            this.btnUploadXSL.Text = "Specify";
            this.btnUploadXSL.UseVisualStyleBackColor = true;
            this.btnUploadXSL.Click += new System.EventHandler(this.btnUploadXSL_Click);
            // 
            // txtXSLLocation
            // 
            this.txtXSLLocation.Location = new System.Drawing.Point(92, 80);
            this.txtXSLLocation.Name = "txtXSLLocation";
            this.txtXSLLocation.Size = new System.Drawing.Size(213, 20);
            this.txtXSLLocation.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "XSL Location";
            // 
            // btnSavePDF
            // 
            this.btnSavePDF.Location = new System.Drawing.Point(321, 116);
            this.btnSavePDF.Name = "btnSavePDF";
            this.btnSavePDF.Size = new System.Drawing.Size(75, 23);
            this.btnSavePDF.TabIndex = 11;
            this.btnSavePDF.Text = "Save";
            this.btnSavePDF.UseVisualStyleBackColor = true;
            this.btnSavePDF.Click += new System.EventHandler(this.btnSavePDF_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSavePDF;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 235);
            this.Controls.Add(this.btnSavePDF);
            this.Controls.Add(this.btnUploadXSL);
            this.Controls.Add(this.txtXSLLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnUploadXML);
            this.Controls.Add(this.txtXMLLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFOPLoc);
            this.Controls.Add(this.btnUploadFOP);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "FOP Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUploadFOP;
        private System.Windows.Forms.TextBox txtFOPLoc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtXMLLocation;
        private System.Windows.Forms.Button btnUploadXML;
        private System.Windows.Forms.Button btnUploadXSL;
        private System.Windows.Forms.TextBox txtXSLLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSavePDF;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}

