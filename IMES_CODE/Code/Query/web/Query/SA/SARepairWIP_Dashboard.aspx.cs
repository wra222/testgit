﻿using System;
using System.Collections.Generic;
using System.Data;
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

public partial class Query_SA_SARepairWIP_Dashboard : System.Web.UI.Page
{
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    public static ISARepairWIP_Dashboard SARepairWIPDashboard = ServiceAgent.getInstance().GetObjectByName<ISARepairWIP_Dashboard>(WebConstant.ISARepairWIP_Dashboard);
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
        string RefreshTime = "5";
        string[] QueryColumnNames = { "Family", "Input Qty", "Output Qty", "WIP Qty", "W/H Qty" };
        List<string> returnData = new List<string>();
        try
        {
            DataTable dt = SARepairWIPDashboard.GetQueryResult(Connection);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table class='ctl' style='width:99%'><tr class=''>");
            // foreach (string columnname in QueryColumnNames)
            // {
            //     sb.AppendFormat("<td style='width:20px' font size='5px'>{0}</td>", columnname);
            // }
            sb.AppendLine("</tr>");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                sb.AppendLine("<tr class=''>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.AppendFormat("<td align=center width=22%><font size='7px'><B>{0}</B></font></td>", dt.Rows[j][i].ToString());
                }

                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            returnData.Add(sb.ToString());
            returnData.Add(RefreshTime);//刷新时间
        }
        catch (Exception e)
        {
            throw e;
        }
        return returnData;
    }
}