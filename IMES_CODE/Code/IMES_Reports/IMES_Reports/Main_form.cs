using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Net;
//using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Collections;
using IMES_Reports.App_Code;

namespace IMES_Reports
{
    public partial class Main_From : Form
    {
        AppHelper AH = new AppHelper();
        public Main_From()
        {
            InitializeComponent();
            string Url = AH.ReadConfig("appSettings", "NetCheck");
            IsInOANet IsOANet = new IsInOANet();
            if (IsOANet.IsCanConnect(Url))
            {
                this.FormClosed += new FormClosedEventHandler(Main_From_FormClosed);
                
                //this.label1.Text = AH.ReadConfig("appSettings", "connectionstring");
                string CmdText = AH.ReadConfig("appSettings", "Data_for_Product");
                DataSet ds = new DataSet();
                ds = Func.DBbind(CmdText);
                // ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
                Func.DrawingChart("HPIMES 生產出貨即時報表（未來3天）", this.panel1, ds, ChartType.Histogram, 0, 100, 10, "");
                this.dataGridView1.DataSource = ds.Tables[0];
                for (int i = 0; i < 10; i++)
                {
                    this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            else
            {
                MessageBox.Show("無法連接到OA 網絡請檢查網絡連接","系統提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
            ds = Func.DBbind("exec dbo.rpt_Product_Generate");
            Func.DrawingChart("饼图测试", this.panel1, ds);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = Func.DBbind("exec dbo.rpt_Product_Generate");
          // ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
            Func.DrawingChart("HPIMES 生產出貨即時報表（未來3天）", this.panel1, ds, ChartType.Histogram, 0, 100, 10, "");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = Func.DBbind("select '一月' as 月份,'甲' as name,50 as value,'乙' as name2,60 as value2 union all select '二月' as 月份,'甲',70,'乙',80");
            Func.DrawingChart("直方图一测试", this.panel1, ds, ChartType.Histogram, 0, 100, 10, "万");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = Func.DBbind("select '一月' as 月份,50 as 甲,60 as 乙 union all select '二月', 60,40 union all select '三月',70,50 union all select '四月',30,60 union all select '五月',40,80 union all select '六月',70,90");
            Func.DrawingChart("曲线图测试", this.panel1, ds, ChartType.Polygram, 0, 100, 10, "瓦");
        }
        public void RestoreWindow()
        {
            //this.WindowState = ;
            //this.Show();
            //this.ShowInTaskbar = true;
            Check_FormStatus();
        }
        private void Main_From_FormClosed(object sender, EventArgs e)
        {
            //Fly_Form fly = new Fly_Form();
            //fly.Show();
        }
        public void Check_FormStatus()
        {

            Form singleForm = null;//初始化为空

            foreach (Form item in Application.OpenForms)
            {

                if (item is Main_From)
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

            if (singleForm == null) singleForm = new Main_From();

            singleForm.Show();

        }

    }

//构建类库


}