using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFOPLoc.Text = openFileDialog1.FileName;
            }
        }

        private void btnSavePDF_Click(object sender, EventArgs e)
        {
            if (txtFOPLoc.Text == string.Empty || txtXMLLocation.Text == string.Empty || txtXSLLocation.Text == string.Empty)
            {
                throw new ArgumentException("Input not valid");
            }
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pdfLocation = saveFileDialog1.FileName;
                    FOPWrap.FOP.GeneratePDF(
                        txtFOPLoc.Text,
                        txtXMLLocation.Text,
                        txtXSLLocation.Text,
                        pdfLocation);
                }
            }
        }

        private void btnUploadXML_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtXMLLocation.Text = openFileDialog1.FileName;
            }
        }

        private void btnUploadXSL_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtXSLLocation.Text = openFileDialog1.FileName;
            }
        }
    }
}