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
//using IMES.Station.Interface.CommonIntf;
//using IMES.Station.Interface.StationIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class DOCKING_CombinePCBinLot : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 8;
    public int initRowsCountForLot = 8;
    public string userId;
    public string customer;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            if (!Page.IsPostBack)
            {
                Master.NeedPrint = false;
                InitLabel();
                initTableColumnHeader();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                //ViewState["ds"] = this.gridview.DataSource;
                
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.useridHidden.Value = userId;
                this.customerHidden.Value = customer;
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
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblotlist.Text = this.GetLocalResourceObject(Pre + "_lblotlist").ToString();
        this.lblMBSN.Text = this.GetLocalResourceObject(Pre + "_lblMBSN").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblLotQty.Text = this.GetLocalResourceObject(Pre + "_lblLotQty").ToString();
        //setFocus();
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCountForLot; i++)
        {
            newRow = dt.NewRow();
            newRow["LotNo"] = String.Empty;
            newRow["Type"] = String.Empty;
            newRow["Qty"] = String.Empty;
            newRow["Status"] = String.Empty;
            newRow["CreateDate"] = String.Empty;
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
        //retTable.Columns.Add("ChkAll", Type.GetType("System.String"));
        retTable.Columns.Add("LotNo", Type.GetType("System.String"));
        retTable.Columns.Add("Type", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("CreateDate", Type.GetType("System.String"));
        return retTable;
    }
    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = "";
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblLotNo").ToString();
        this.gridview.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_tblType").ToString();
        this.gridview.Columns[3].HeaderText = this.GetLocalResourceObject(Pre + "_tblQty").ToString();
        this.gridview.Columns[4].HeaderText = this.GetLocalResourceObject(Pre + "_tblStatus").ToString();
        this.gridview.Columns[5].HeaderText = this.GetLocalResourceObject(Pre + "_tblCreateDate").ToString();
        this.gridview.Columns[0].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Pixel(44);
        this.gridview.Columns[0].HeaderStyle.Wrap = false;
        this.gridview.Columns[1].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(19);
        this.gridview.Columns[2].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(15);
        this.gridview.Columns[3].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(15);
        this.gridview.Columns[4].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(19);
        this.gridview.Columns[5].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(26);

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

    private void writeTouccessfulInfo(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowSuccessfulInfo(true);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeTouccessfulInfo", scriptBuilder.ToString(), false);

    }
    private void writeToNullMessage(string errorMsg)
    {


        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToNullMessage", scriptBuilder.ToString(), false);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewLot_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) || (e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                RadioButton check = (RadioButton)e.Row.FindControl("RowRadioChk");
                //check.Style.Add("display", "none");
                check.Style.Add("visibility", "hidden");
            }
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
        setFocus();
    }

    
      /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        //String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        //ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
        //DEBUG ITC-1360-0729
        String script = "<script language='javascript'>" + "\r\n" +
                        "window.setTimeout (callNextInput,100);" + "\r\n" +
                        "</script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "callNextInput", script, false);
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
    /// <summary>
    /// btnFru_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnFru_Click(object sender, System.EventArgs e)
    {
        //DEBUG ITC-1360-0358 ADD Server method refresh Pdline
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
