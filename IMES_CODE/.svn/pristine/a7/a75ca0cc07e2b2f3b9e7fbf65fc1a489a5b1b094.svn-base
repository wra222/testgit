using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using log4net;

using System.Data;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
public partial class Query_SA_SMT_Dashboard : IMESQueryBasePage
{
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    public static ISA_SMTDashboard iSA_SMTDashboard = ServiceAgent.getInstance().GetObjectByName<ISA_SMTDashboard>(WebConstant.ISA_SMTDashboard);
    public string userId;
    public string userName;
    public string customer;
    public long accountId;
    protected void Page_Load(object sender, EventArgs e)
    {
            string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
            hidDbName.Value = Request["DBName"] ?? configDefaultDB;
            hidConnection.Value = CmbDBType.ddlGetConnection();
            InitCondition();
            userId = Master.userInfo.UserId;
            userName = Master.userInfo.UserName;
            customer = Master.userInfo.Customer;
            accountId = Master.userInfo.AccountId;

        
    }
    public void InitCondition()
    {
        ISA_SMTDashboard SA_SMTDashboard = ServiceAgent.getInstance().GetObjectByName<ISA_SMTDashboard>(WebConstant.ISA_SMTDashboard);
        List<SMT_DashBoard_Line> dtLines = SA_SMTDashboard.GetQueryLine();
        List<String> data = new List<string>();
        if (dtLines.Count > 0)
        {
            foreach (SMT_DashBoard_Line dr in dtLines)
            {
                if (!data.Contains(dr.line))
                {
                    data.Add(dr.line);
                }

            }
        }
        if (data.Count > 0)
        {
            foreach (String temp in data)
            {
                dropLine.Items.Add(temp);
            }
        }
        
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<string> RefreshTimeAndBindData(string Connection, string line)
    {
        string[] QueryColumnNames = { "时间", "机型", "产出", "标准产能", "产出率", "不良", "良率", "Top3不良位置" };    
        string RefreshTime ="";
        string station="";
        List<string> returnData = new List<string>();
        try
        {
            List<DataTable> dt = iSA_SMTDashboard.GetQueryResult(Connection, line);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbNight = new StringBuilder();//夜班       
            sb.AppendLine("<table class='ctl' style='border-width:0px;height:1px;width:100%;'><thead><tr class='iMes_grid_HeaderRowGvExt'>");
            sbNight.AppendLine("<table class='ctl' style='border-width:0px;height:1px;width:100%;'><thead><tr class='iMes_grid_HeaderRowGvExt'>");
            foreach (string columnname in QueryColumnNames)
            {
                sb.AppendFormat("<th>{0}</th>", columnname);
                sbNight.AppendFormat("<th>{0}</th>", columnname);
            }
            sb.AppendLine("</tr></thead><tbody>");
            sbNight.AppendLine("</tr></thead><tbody>");
            for (int j = 0; j < dt[0].Rows.Count; j++)
            {
                // string classname = "";
                // classname = "lesson" + dt2.Rows[j][1].ToString();
                
                
                if(dt[0].Rows[j][0].ToString().ToString()=="08:00--10:00"||dt[0].Rows[j][0].ToString().ToString()=="10:00--12:00"||dt[0].Rows[j][0].ToString().ToString()=="12:00--14:00"||dt[0].Rows[j][0].ToString().ToString()=="14:00--16:00"||dt[0].Rows[j][0].ToString().ToString()=="16:00--18:00"||dt[0].Rows[j][0].ToString().ToString()=="18:00--20:30")
                {
                    sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
                    for (int i = 0; i < dt[0].Columns.Count; i++)
                    {
                        if (i == 6 && dt[0].Rows[j][6].ToString() != "")
                        {
                            if (Convert.ToDouble(dt[0].Rows[j][6].ToString().Substring(0,dt[0].Rows[j][6].ToString().Length-1)) < 96)
                            {
                                sb.AppendFormat("<td><font color='red'><B>{0}</B></font></td>", dt[0].Rows[j][i].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("<td>{0}</td>", dt[0].Rows[j][i].ToString());
                            }
                        }
                        else
                        {
                            sb.AppendFormat("<td>{0}</td>", dt[0].Rows[j][i].ToString());
                        }

                    }
                }
                else
                {
                    sbNight.AppendLine("<tr class='iMes_grid_RowGvExt'>");
                    for (int i = 0; i < dt[0].Columns.Count; i++)
                    {
                        if (i == 6 && dt[0].Rows[j][6].ToString() != "")
                        {
                            if (Convert.ToDouble(dt[0].Rows[j][6].ToString().Substring(0, dt[0].Rows[j][6].ToString().Length - 1)) < 96)
                            {
                                sbNight.AppendFormat("<td><font color='red'><B>{0}</B></font></td>", dt[0].Rows[j][i].ToString());
                            }
                            else
                            {
                                sbNight.AppendFormat("<td>{0}</td>", dt[0].Rows[j][i].ToString());
                            }
                        }
                        else
                        {
                            sbNight.AppendFormat("<td>{0}</td>", dt[0].Rows[j][i].ToString());
                        }
                    }
                }

                sb.AppendLine("</tr>");
                sbNight.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            sbNight.AppendLine("</table>");

            foreach (DataRow dr in dt[2].Rows)
            {
                RefreshTime = dr["RefreshTime"].ToString().Trim();
                station = dr["Station"].ToString().Trim();
            }



            returnData.Add(sb.ToString());      //白班数据
            returnData.Add(sbNight.ToString()); //夜班数据
            returnData.Add(RefreshTime);//刷新时间
            returnData.Add(BuildDefectHTML(dt[1]));//不良原因分析
            returnData.Add(station);//站点
            //this.gvResult.DataSource = dt;
            //this.gvResult.DataBind();
        }
        catch (Exception e)
        {
            throw e;
        }
        return returnData;
    }


    public static string BuildDefectHTML(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        string[] QueryColumnNames = { "时间", "机型", "位置", "不良现象", "改善对策"};
        sb.AppendLine("<table  style='border-width:0px;height:1px;width:98%;font-size:12px'><thead><tr class='iMes_grid_HeaderRowGvExt'>");
        foreach (string columnname in QueryColumnNames)
        {
            sb.AppendFormat("<th>{0}</th>", columnname);
        }
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            // string classname = "";
            // classname = "lesson" + dt2.Rows[j][1].ToString();
            sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.AppendFormat("<td>{0}</td>", dt.Rows[j][i].ToString());
            }
            sb.AppendLine("</tr>");
        }
        for (int i = 0; i < 2; i++)
        {
            sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
            foreach (string columnname in QueryColumnNames)
            {
                sb.AppendFormat("<td>{0}</td>", " ");
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }



}
