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

public partial class SA_PCAOQCOutput : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IVirtualMo iVirtualMo = ServiceAgent.getInstance().GetObjectByName<IVirtualMo>(WebConstant.VirtualMoObject);

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
            ////注册下拉框的选择事件
            //this.CmbPdLine1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            //this.CmbPdLine1.InnerDropDownList.AutoPostBack = true;

            if (!IsPostBack)
            {

                InitLbl();

                
                //绑定空表格
                this.GridViewExt1.DataSource = getNullDataTable();
                this.GridViewExt1.DataBind();
                
                initTableColumnHeader();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                //this.CmbPdLine1.Station = Request["station"];
                //this.CmbPdLine1.Customer = Master.userInfo.Customer;
              //  this.CmbPdLine1.Stage = "SA";

                //setPdLineFocus();

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
        //this.GridViewExt1.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_gridDefectCode").ToString();
        //this.GridViewExt1.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_gridDescr").ToString();
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_gridDefectCode").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_gridDescr").ToString();

        //// 改变表格宽度: ITC-1360-0222
        //this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        //this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(200);
        
        
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
        this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();

    }


    //private void cmbPdLine_Selected(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        this.CmbPdLine1.Station = Request["station"];
    //        this.CmbPdLine1.Customer = Master.userInfo.Customer;
    //       // this.CmbPdLine1.Stage = "SA";
            
    //        if (this.CmbPdLine1.InnerDropDownList.SelectedValue == "")
    //        {
    //            alertNoPdLine();
    //            setPdLineFocus();
    //        }
    //        else
    //        {
    //            setDataEntryFocus();
    //        }

    //    }
    //    catch (FisException exp)
    //    {
    //        writeToAlertMessage(exp.mErrmsg);
    //    }
    //    catch (Exception exp)
    //    {
    //        writeToAlertMessage(exp.Message.ToString());
    //    }
    //}

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
    /// setPdLineCmbFocus
    /// </summary>
    /// <param name="er"></param>
    //private void setPdLineFocus()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //           "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    //}

    /// <summary>
    /// AlertErrorMessage for Family
    /// </summary>  
    //private void alertNoPdLine()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (alertNoPdLine,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectFamily", script, false);
    //}


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
