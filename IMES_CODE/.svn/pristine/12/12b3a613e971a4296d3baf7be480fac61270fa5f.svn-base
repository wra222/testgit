/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
  * Known issues:
 * TODO：
 * 
 * * XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;

public partial class FA_CommonLabelPrint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 8;
    //private const string STATION = "66";

    public String UserId;
    public String Customer;
    public String AccountId;
    public String Login;
    public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            initTableColumnHeader();

            if (!Page.IsPostBack)
            {
                string station = Request["Station"];
                string pcode = Request["PCode"];
                //station = "67";
                InitLabel();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
    
                setFocus();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;
                //ForTest
                //station = "ITCND";
                this.station1.Value = station;
                this.editor1.Value = UserId;
                this.customer1.Value = Customer;
                this.pCode1.Value = pcode;               
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message);
        }
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnSetting").ToString();
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Parameter"] = String.Empty;
            newRow["Value"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Parameter", Type.GetType("System.String"));
        retTable.Columns.Add("Value", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = "Parameter";
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(160);
        //this.gridview.Columns[1].ItemStyle.Width = Unit.Pixel(160);
    }



    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }



    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }
}
