using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMES_Reports.App_Code;
namespace IMES_Reports
{
    public partial class Add_new : Form
    {
        AppHelper AH = new AppHelper();
        public Add_new()
        {
            InitializeComponent();
            if (AH.Buttun_Check("Btn_For_Delivery"))
            {
                this.checkBox1.Checked = true;
                
            }
            if (AH.Buttun_Check("Btn_For_QC"))
            {
                this.checkBox2.Checked = true;
            }
            if (AH.Buttun_Check("Btn_For_Product"))
            {
                this.checkBox3.Checked = true;
            }
            if (AH.Buttun_Check("Btn_For_WIP"))
            {
                this.checkBox4.Checked = true;
            }
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                AH.WriteTryMessage("Btn_For_Delivery", "YES", "Btn_Check");
                
            }
            else 
            {
                AH.WriteTryMessage("Btn_For_Delivery", "NO", "Btn_Check");
            }
            if (this.checkBox2.Checked)
            {
                AH.WriteTryMessage("Btn_For_QC", "YES", "Btn_Check");
            }
            else
            {
                AH.WriteTryMessage("Btn_For_QC", "NO", "Btn_Check");
            }
            if (this.checkBox3.Checked)
            {
                AH.WriteTryMessage("Btn_For_Product", "YES","Btn_Check");
            }
            else
            {
                AH.WriteTryMessage("Btn_For_Product", "NO", "Btn_Check");
            }
            if (this.checkBox4.Checked)
            {
                AH.WriteTryMessage("Btn_For_WIP","YES","Btn_Check");
            }
            else
            {
                AH.WriteTryMessage("Btn_For_WIP", "NO", "Btn_Check");
            }
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
