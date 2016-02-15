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
    public partial class Input_Output : Form
    {
        AppHelper AH = new AppHelper();
        public Input_Output()
        {
            InitializeComponent();

        }
        public void Form_Reflash()
        {

          
            string CmdText = AH.ReadConfig("appSettings", "Data_for_InputOutput");
            DataSet ds = new DataSet();

            ds = Func.DBbind(CmdText);
            int Y_height = int.Parse(ds.Tables[0].Rows[0][4].ToString());
            ds.Tables[0].Columns.Remove("Qty");
            int temp;
            if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) <= int.Parse(ds.Tables[0].Rows[0][1].ToString()))
            {
                temp = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                ds.Tables[0].Rows[0][0] = ds.Tables[0].Rows[0][1].ToString();
                ds.Tables[0].Rows[0][1] = temp;

                l_2.Text = "FA投入數";
                l_1.Text = "FA產出數";

            }
            else
            {
                l_1.Text = "FA投入數";
                l_2.Text = "FA產出數";
            }
            if (int.Parse(ds.Tables[0].Rows[0][2].ToString()) <= int.Parse(ds.Tables[0].Rows[0][3].ToString()))
            {
                temp = int.Parse(ds.Tables[0].Rows[0][2].ToString());
                ds.Tables[0].Rows[0][2] = ds.Tables[0].Rows[0][3].ToString();
                ds.Tables[0].Rows[0][3] = temp;
                l_4.Text = "PAK投入數";
                l_3.Text = "PAK產出數";
            }
            else
            {
                l_3.Text = "PAK投入數";
                l_4.Text = "PAK產出數";
            }
            ds.Tables[0].AcceptChanges();
            // ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
            //  Func.DrawingChart("HPIMES 生產出貨即時報表（未來3天）", this.panel1, ds, ChartType.Other, 0, 1000, 10, "");
            Func.DrawingChart("HPIMES 投入產出報表", this.panel1, ds, ChartType.Other, 0, Y_height, 100, "");
        }
        public void Input_Output_Load(object sender, EventArgs ex)
        {
            IsInOANet IsOANet = new IsInOANet();
            string Url = AH.ReadConfig("appSettings", "NetCheck");
            if (IsOANet.IsCanConnect(Url))
            {

                Form_Reflash();
                ContextMenuStrip docMenu = new ContextMenuStrip();
                ToolStripMenuItem ExitLable = new ToolStripMenuItem();
                ExitLable.Text = "刷新";
                ExitLable.Click += new EventHandler(Reflash_Click);
                docMenu.Items.AddRange(new ToolStripMenuItem[] { ExitLable });
                this.ContextMenuStrip = docMenu;
            }
            else
            {
                MessageBox.Show("無法連接到OA 網絡請檢查網絡連接", "系統提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void Reflash_Click(object sender, System.EventArgs e)
        {
            Form_Reflash();
        }
        public void RestoreWindow()
        {
            //this.WindowState = ;
            //this.Show();
            //this.ShowInTaskbar = true;
            Check_FormStatus();
        }
        public void Check_FormStatus()
        {
            
            Form singleForm = null;//初始化为空

                foreach (Form item in Application.OpenForms)
                {

                    if (item is Input_Output)
                    {

                        singleForm = item;
                        //object sender=new object();
                        //System.EventArgs e = System.EventArgs.Empty;
                        //this.Input_Output_Load( sender, e);
                        this.Close();
                        //singleForm.Show();
                        break;

                    }

                }
            
            if (singleForm == null) singleForm = new Input_Output();
           
            singleForm.Show();
           
        }
    }
}
