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
using IMES.DataModel;

public partial class PCAOQCCosmetic : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int DEFAULT_ROWS = 6;

    public String UserId;
    public String Customer;
    public String AccountId;
    public String Login;
    public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                string station = Request["Station"];
                string pcode = Request["PCode"];

                InitLabel();
                //绑定空表格
                //bindTable(null, DEFAULT_ROWS);
                //setColumnWidth();
                this.gd.DataSource = getNullDataTable(DEFAULT_ROWS);
                this.gd.DataBind();
                initTableColumnHeader();

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

    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Defect Code"] = String.Empty;
            newRow["Description"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Defect Code", Type.GetType("System.String"));
        retTable.Columns.Add("Description", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {

        this.gd.HeaderRow.Cells[0].Text = "Defect Code";
        this.gd.HeaderRow.Cells[1].Text = "Description";
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(200);
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblLotNo.Text = this.GetLocalResourceObject(Pre + "_lblLotNo").ToString();
        this.lblPdline.Text = this.GetLocalResourceObject(Pre + "_lblPdline").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lbDefectList").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblChecked.Text = this.GetLocalResourceObject(Pre + "_lblChecked").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnQuery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
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
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
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

    private int bindTable(IList<RepairInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        int ret = 0;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefect").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDesc").ToString());
        
        if (list != null && list.Count != 0)
        {
            foreach (RepairInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.isManual;
                dr[1] = temp.pdLine;
                
                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        return ret;
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(100);
    }
}
