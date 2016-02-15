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
    public partial class QC_Reports : Form
    {
        public QC_Reports()
        {
            InitializeComponent();
            AppHelper AH = new AppHelper();
           
                        IsInOANet IsOANet = new IsInOANet();
            string Url = AH.ReadConfig("appSettings", "NetCheck");
            if (IsOANet.IsCanConnect(Url))
            {
                string CamText = AH.ReadConfig("appSettings", "Data_for_QCReports");
                DataSet ds = new DataSet();
                ds = Func.DBbind(CamText);
                // ds = Func.DBbind("select '甲' as name,50 as value union all select '乙',60 union all select '丙',70 union all select '丁',80");
                Func.DrawingChart("品質不良報表", this.panel1, ds, ChartType.Histogram, 0, 100, 10, "");
            }
            else
            {
                MessageBox.Show("無法連接到OA 網絡請檢查網絡連接", "系統提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
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

                if (item is QC_Reports)
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

            if (singleForm == null) singleForm = new QC_Reports();

            singleForm.Show();

        }
    }
}
