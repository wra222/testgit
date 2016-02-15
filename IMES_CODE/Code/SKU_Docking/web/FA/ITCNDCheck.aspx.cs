/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/FA/ITCNDCheck Page
 * UI:CI-MES12-SPEC-FA-UI ITCNDCheck.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCNDCheck.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-14   zhanghe               (Reference Ebook SourceCode) Create
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

public partial class FA_ITCNDCheck : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;
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
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);

            if (!Page.IsPostBack)
            {
                string station = Request["Station"];
                string pcode = Request["PCode"];
                //station = "67";
                InitLabel();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
    
                //将pdLine combobox的初始化参数传入
                this.cmbPdLine.Station = station;
                //this.cmbPdLine.Station = "67";
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                //this.cmbPdLine.Stage = "FA";
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

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbProductId.Text = this.GetLocalResourceObject(Pre + "_lblProductId").ToString();
        this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btquery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btreprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
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
            newRow["Item"] = String.Empty;
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
        retTable.Columns.Add("Item", Type.GetType("System.String"));
        retTable.Columns.Add("Value", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_lblItem").ToString();
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
