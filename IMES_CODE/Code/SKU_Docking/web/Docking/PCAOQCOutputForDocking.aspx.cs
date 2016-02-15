/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13   Chen Xu              Create
 * 2012-02-07   Chen Xu              Modify:ITC-1360-0222
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	记录SAOQC 结果，若有不良，则记录不良信息
 *               
 * UC Revision:  3382
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
using Microsoft.SqlServer.Dts.Runtime;
using com.inventec.imes.DBUtility;

public partial class Docking_PCAOQCOutputForDocking : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public String UserId;
    public String Customer;
    public int initRowsCount = 12;

    public string curFamily = string.Empty;
    public string curModel = string.Empty;
    public string curStation = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {

                InitLbl();

                
                //绑定空表格
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                
                initTableColumnHeader();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                this.txtDataEntry.Focus();
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();

            newRow["DefectCode"] = String.Empty;
            newRow["Description"] = String.Empty;
                       
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
        retTable.Columns.Add("DefectCode", Type.GetType("System.String"));
        retTable.Columns.Add("Description", Type.GetType("System.String"));
        
        return retTable;
    }

     /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_gridDefectCode").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_gridDescr").ToString();
        
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

    private void InitLbl()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblDefectList.InnerText = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        //this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();

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

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

 

    /// <summary>
    /// setDataEntryFocus
    /// </summary>
    /// <param name="er"></param>
    private void setDataEntryFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
               "window.setTimeout(setInputFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setInputFocus", script, false);
    }

    /// <summary>
    /// nineSelect_CheckedChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void nineSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (nineSelect.Checked == true)
        {

        }
        else
        {

        }

    }
}
