/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-03   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_PalletVerifyFDEOnly : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public int initRowsCount = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();

                initTableColumnHeader();
                //绑定空表格
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                this.txtDataEntry.Focus();
                
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            errorPlaySound();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            errorPlaySound();
        }
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();

            newRow["ProdID"] = String.Empty;
            newRow["CustSN"] = String.Empty;
            newRow["PAQC"] = String.Empty;
            newRow["POD"] = String.Empty;
            newRow["WC"] = String.Empty;
            newRow["CollectionData"] = String.Empty;
           
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    /// <summary>
    /// 初始化列表格
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProdID", Type.GetType("System.String"));
        retTable.Columns.Add("CustSN", Type.GetType("System.String"));
        retTable.Columns.Add("PAQC", Type.GetType("System.String"));
        retTable.Columns.Add("POD", Type.GetType("System.String"));
        retTable.Columns.Add("WC", Type.GetType("System.String"));
        retTable.Columns.Add("CollectionData", Type.GetType("System.String"));

        return retTable;
    }

    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridViewExt1.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_gridProd").ToString();
        this.GridViewExt1.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_gridCustSn").ToString();
        this.GridViewExt1.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_gridPAQC").ToString();
        this.GridViewExt1.Columns[3].HeaderText = this.GetLocalResourceObject(Pre + "_gridPOD").ToString();
        this.GridViewExt1.Columns[4].HeaderText = this.GetLocalResourceObject(Pre + "_gridWC").ToString();
        this.GridViewExt1.Columns[5].HeaderText = this.GetLocalResourceObject(Pre + "_gridCollection").ToString();

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
    /// 初始化页面的静态label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.lblTotalQty.Text = this.GetLocalResourceObject(Pre + "_lblTotalQty").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblPalletInfo.InnerText = this.GetLocalResourceObject(Pre + "_lblPalletInfo").ToString();
        this.lblPalletNo.Text = this.GetLocalResourceObject(Pre + "_lblPalletNo").ToString();
        this.lblPalletQty.Text = this.GetLocalResourceObject(Pre + "_lblPalletQty").ToString();
        this.lblProductLst.InnerText = this.GetLocalResourceObject(Pre + "_lblProductLst").ToString();
        this.lblScanQty.Text = this.GetLocalResourceObject(Pre + "_lblScanQty").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }

    

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 报错提示音
    /// <returns></returns>
    private void errorPlaySound()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (PlaySound,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.gridViewUP, typeof(System.Object), "PlaySound", script, false);
    }
}