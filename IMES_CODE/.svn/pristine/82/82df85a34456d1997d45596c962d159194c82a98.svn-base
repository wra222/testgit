using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class DOCKING_PCAOQCInput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 8;
    public string UserId;
    public string Customer;
    public string Station;


    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            
            if (!Page.IsPostBack)
            {
                InitLabel();
                initTableColumnHeader();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                //ViewState["ds"] = this.gridview.DataSource;

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.useridHidden.Value = UserId;
                this.customerHidden.Value = Customer;

            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
 
    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblLotNo.Text = this.GetLocalResourceObject(Pre + "_lblLotNo").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblPCS.Text = this.GetLocalResourceObject(Pre + "_lblPCS").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();

        this.lbltblmbsnList.Text = this.GetLocalResourceObject(Pre + "_lbltblmbsnList").ToString();
        this.lblDefineQty.Text = this.GetLocalResourceObject(Pre + "_lblDefineQty").ToString();
        this.lblCheckQty.Text = this.GetLocalResourceObject(Pre + "_lblCheckQty").ToString();

        setFocus();
      
    }


    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["MBSN"] = String.Empty;
            newRow["Checked"] = String.Empty;
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
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));
        retTable.Columns.Add("Checked", Type.GetType("System.String"));
        return retTable;
    }
    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_tblMBSN").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblChecked").ToString();
        //this.gridview.Columns[0].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Pixel(44);
        //this.gridview.Columns[0].HeaderStyle.Wrap = false;
        //this.gridview.Columns[1].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(24);
        
        //this.gridview.Columns[0].ItemStyle.Width = Unit.Percentage(10);
        //this.gridviewMB.Columns[1].ItemStyle.Width = Unit.Percentage(50);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
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
       // String script = "<script language='javascript'>" + "\r\n" +
       //     "window.setTimeout (setPdLineCmbFocus,100);" + "\r\n" +
       //     "</script>";
       // ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    /// <summary>
    /// reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
   
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);

        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
}
