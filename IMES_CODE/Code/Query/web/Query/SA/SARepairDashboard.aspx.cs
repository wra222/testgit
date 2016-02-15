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
public partial class Query_SA_SARepairDashboard : IMESQueryBasePage
{
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    public static ISARepairDashboard iSA_RepairDashboard = ServiceAgent.getInstance().GetObjectByName<ISARepairDashboard>(WebConstant.ISARepairDashboard);
    public string userId;
    public string userName;
    public string customer;
    public long accountId;
    protected void Page_Load(object sender, EventArgs e)
    {
            string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
            hidDbName.Value = Request["DBName"] ?? configDefaultDB;
            hidConnection.Value = CmbDBType.ddlGetConnection();
         
            userId = Master.userInfo.UserId;
            userName = Master.userInfo.UserName;
            customer = Master.userInfo.Customer;
            accountId = Master.userInfo.AccountId;

        
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<string> RefreshTimeAndBindData(string Connection)
    {
        string[] QueryColumnNames = { "工號", "Input Qty", "Output Qty", "Repair Qty" };    
        string RefreshTime ="";
        string station="";
        List<string> returnData = new List<string>();
        try
        {
            List<DataTable> dt = iSA_RepairDashboard.GetQueryResult(Connection);
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
                
                
                if(dt[0].Rows[j][0].ToString().ToString()=="D")
                {
                    sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
                    for (int i = 1; i < dt[0].Columns.Count; i++)
                    {
                        //if (i == 3 && dt[0].Rows[j][3].ToString() != "")
                        //{
                        //    if (Convert.ToDouble(dt[0].Rows[j][3].ToString().Substring(0,dt[0].Rows[j][3].ToString().Length-1)) < 96)
                        //    {
                        //        sb.AppendFormat("<td><font color='red'><B>{0}</B></font></td>", dt[0].Rows[j][i].ToString());
                        //    }
                        //    else
                        //    {
                        //        sb.AppendFormat("<td>{0}</td>", dt[0].Rows[j][i].ToString());
                        //    }
                        //}
                        //else
                        //{
                        sb.AppendFormat("<td><font size='3px'>{0}</font></td>", dt[0].Rows[j][i].ToString());
                        //}

                    }
                }
                else
                {
                    sbNight.AppendLine("<tr class='iMes_grid_RowGvExt'>");
                    for (int i = 1; i < dt[0].Columns.Count; i++)
                    {

                        sbNight.AppendFormat("<td><font size='3px'>{0}</font></td>", dt[0].Rows[j][i].ToString());
                        
                    }
                }

                sb.AppendLine("</tr>");
                sbNight.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            sbNight.AppendLine("</table>");

            foreach (DataRow dr in dt[2].Rows)
            {
                RefreshTime = dr["MQ"].ToString().Trim();
            }



            returnData.Add(sb.ToString());      //白班数据
            returnData.Add(sbNight.ToString()); //夜班数据
            returnData.Add(RefreshTime);//刷新时间
            returnData.Add(BuildDefectHTML(dt[1]));//不良原因分析
            //returnData.Add(station);//站点
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
        string[] QueryColumnNames = { "班别", "机型", "不良现象", "不良描述", "数量"};
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
                sb.AppendFormat("<td font size='3px'>{0}</td>", dt.Rows[j][i].ToString());
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
