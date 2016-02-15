/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description: Virtual MO
 * UI:CI-MES12-SPEC-PAK-UI Virtual MO.docx
 * UC:CI-MES12-SPEC-PAK-UC Virtual MO.docx        
 * Update: 
 * Date        Name                 Reason 
 * ==========  ==================== =====================================
 * 2011-11-30   Chen Xu             Create
 * 2012-02-06   Chen Xu             Modify:ITC-1360-0171 , ITC-1360-0280
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	Add Qty of Virtual MO
 *                2.	Add New Virtual MO
 * UC Revision: 6789              
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

public partial class FA_VirtualMO : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IVirtualMo iVirtualMo = ServiceAgent.getInstance().GetObjectByName<IVirtualMo>(WebConstant.VirtualMoObject);

    public String UserId;
    public String Customer;
    public int initRowsCount = 6;

    public string curFamily = string.Empty;
    public string curModel = string.Empty;
    public string curStation = string.Empty;

    public string today;
    public string MOPrefix;
    public string JobName = ConfigurationManager.AppSettings["VirtualMO_JOBNAME"].ToString(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////注册下拉框的选择事件
            this.CmbFamily1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
            //this.CmbModel1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);

            this.CmbFamily1.InnerDropDownList.AutoPostBack = true;
            //this.CmbModel1.InnerDropDownList.AutoPostBack = true;

            if (!IsPostBack)
            {

                InitLbl();
                bindTable(null);
                setColumnWidth();

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                curStation = Request["station"];
                MOPrefix = Request["MOPrefix"];
                this.CmbFamily1.Station = curStation;
                this.CmbFamily1.Customer = Customer;
                this.CmbFamily1.Service = "000";

                //this.CmbModel1.Station = curStation;
                //this.CmbModel1.Customer = Customer;

                today = iVirtualMo.GetCurDate().ToString("yyyy-MM-dd");

                setFamilyFocus();
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


    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.CmbFamily1.Station = Request["station"];
            this.CmbFamily1.Customer = Master.userInfo.Customer;
            this.CmbFamily1.Service = "000"; 

            ShowInfoClear();
            this.txtModel.Value = "";
            this.txtQty.Value = "";
            clearTabel();

            if (this.CmbFamily1.InnerDropDownList.SelectedValue == "")
            {
                alertNoFamily();
                setFamilyFocus();
            }
            else
            {
                this.txtModel.Focus();
            }

        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    //private void cmbFamily_Selected(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        ShowInfoClear();
    //        curFamily = string.Empty;
    //        curModel = string.Empty;

    //        this.CmbFamily1.Station = Request["station"];
    //        this.CmbFamily1.Customer = Master.userInfo.Customer;
    //        this.CmbFamily1.Service = "000"; //ITC-1360-0171: order by Family

    //        if (this.CmbFamily1.InnerDropDownList.SelectedValue == "")
    //        {
    //            alertNoFamily();

    //            //清空Model下拉框内容
    //            this.CmbModel1.clearContent();
    //            setFamilyFocus();
                
    //        }
    //        else
    //        {
    //            curFamily = this.CmbFamily1.InnerDropDownList.SelectedValue;
    //            this.CmbModel1.Service = "102";
    //            this.CmbModel1.Station = Request["station"];
    //            this.CmbModel1.Customer = Master.userInfo.Customer;
    //            this.CmbModel1.refreshDropContent(curFamily);


    //            // ITC-1360-0280:如果Model List仅有1条记录，建议缺省选中此model
    //            // this.CmbModel1.setSelected(-1);
    //            // setModelFocus();
    //            if (this.CmbModel1.InnerDropDownList.Items.Count == 2)
    //            {
    //                this.CmbModel1.setSelected(1);
    //                curModel = this.CmbModel1.InnerDropDownList.SelectedValue;
    //                FreshButton();
    //                //setQtyFocus();
    //                this.txtQty.Focus();
    //            }
    //            //else if (this.CmbModel1.InnerDropDownList.Items.Count == 1)
    //            //{ 
    //            //    errorMsgModelNull();
    //            //    setFamilyFocus();
    //            //}
    //            else
    //            {
    //                this.CmbModel1.setSelected(-1);
    //                setModelFocus();
    //                ModelDropDownListCSS();
    //            }
                
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

    //private void cmbModel_Selected(object sender, System.EventArgs e)
    //{
    //    try
    //    {
           
    //        ShowInfoClear();
    //        curModel = string.Empty;
    //        this.CmbModel1.Service = "102";
    //        this.CmbModel1.Station = Request["station"];
    //        this.CmbModel1.Customer = Master.userInfo.Customer;

    //        if (this.CmbModel1.InnerDropDownList.SelectedValue == "")
    //        {
    //            alertNoModel();
    //            this.CmbModel1.setSelected(-1);
    //            setModelFocus();
    //        }
    //        else
    //        {
    //            curModel = this.CmbModel1.InnerDropDownList.SelectedValue;
    //          //  GetMOInfobyModel();
                
    //            FreshButton();
    //            //setQtyFocus();
    //            //ModelDropDownList();
    //            this.txtQty.Focus();
               
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

    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //no need tip
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //if ((i == 0) || (i == 1) || (i == 2) || (i == 9) || (i == 10))
                //{
                //    continue;
                //}
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void GetMOInfobyModel()
    {
       // curModel = this.CmbModel1.InnerDropDownList.SelectedValue;
        curModel = this.txtModel.Value.ToUpper();
        IList<MOInfo> list = iVirtualMo.GetVirtualMOByModel(curModel, Master.userInfo.UserId, curStation, Master.userInfo.Customer);
        bindTable(list);
    //    ModelDropDownList();
    //    this.txtQty.Focus();
        
        this.txtModel.Value = "";
        this.txtModel.Focus();
    }
    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            GetMOInfobyModel();
           // setQtyFocus();
           // this.txtQty.Focus();
            
            
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

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            curFamily = string.Empty;
            curModel = string.Empty;
            this.CmbFamily1.setSelected(-1);
          //  this.CmbModel1.clearContent();
            bindTable(null);

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


    protected void clearGridView(object sender, System.EventArgs e)
    {
        try
        {
            bindTable(null);

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

    ////<summary>
    ////Summary description for Auto Download MO
    ////</summary>
    //protected void DownloadMOGrid(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        if (CallDownLoadMoDTS())
    //        {
    //            DownloadMOSucceed();
    //            GetMOInfobyModel();
    //          //  setQtyFocus();
    //            this.txtQty.Focus();
    //        }
            
    //    }
    //    catch (FisException ex)
    //    {
    //        writeToAlertMessage(ex.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        writeToAlertMessage(ex.Message);
    //    }
    //}

    //private static bool CallDownLoadMoDTS()
    //{
        
    //    string dtsFtpPath = ConfigurationManager.AppSettings["DTSFtpPath"].ToString(); ;
    //    string dnFileName = ConfigurationManager.AppSettings["DTSFtpFileName"].ToString(); ;  
        
    //    if (!dtsFtpPath.EndsWith("\\"))
    //    {
    //        dtsFtpPath = dtsFtpPath + "\\";
    //    }
    //    string dnDtsPath = dtsFtpPath + dnFileName;
    //    Application uploadDTSApp = new Application();
    //    Package dnPackage = uploadDTSApp.LoadPackage(dnDtsPath, null, true);
    //    //set Variables


    //    IPOData currentPODataService = ServiceAgent.getInstance().GetObjectByName<IPOData>(WebConstant.PODataObject);
    //    string faConnectStr = currentPODataService.GetPAKConnectionString() + ";Provider=SQLNCLI10.1;Auto Translate=False;";

    //    //set datasource Connections
    //    dnPackage.Connections["IMES2012_GetData"].ConnectionString = faConnectStr;
    //    dnPackage.Connections["DNFile"].ConnectionString = dtsFtpPath ;

    //    //Execute
    //    DTSExecResult dnResult = dnPackage.Execute();
       
    //    if (dnResult.Equals(DTSExecResult.Success))
    //    {
    //        // System.IO.File.Delete(dtsFtpPath);
    //        return true;
    //    }
    //    else
    //    {
    //        throw new Exception(dnPackage.Errors[0].Description);
    //    }    
    //}

    private void InitLbl()
    {
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblShipdate.Text = this.GetLocalResourceObject(Pre + "_lblStartDate").ToString();

        this.btnCreate.Value = this.GetLocalResourceObject(Pre + "_btnCreate").ToString();
        this.btnDownloadMO.Value = this.GetLocalResourceObject(Pre + "_btnDownload").ToString();
        this.btnQuery.Value=this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
       
        this.lblCreatedMOList.InnerText = this.GetLocalResourceObject(Pre + "_lblCreatedMOList").ToString();

    }

    private DataTable initBomTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MO", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("CreateDate", Type.GetType("System.DateTime"));
        retTable.Columns.Add("StartDate", Type.GetType("System.DateTime"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));

        return retTable;
    }

    private void bindTable(IList<MOInfo> list1)
    {
        DataTable dt = initBomTable();
        DataRow newRow = null;
        List<MOInfo> list = null;
        if (list1 != null)
        {
            list = new List<MOInfo>(list1);
        }


        if (list != null && list.Count != 0)
        {
            foreach (MOInfo temp in list)
            {
                newRow = dt.NewRow();
                newRow["MO"] = temp.id;
                newRow["Model"] = temp.model;
                newRow["CreateDate"] = temp.createDate;
                newRow["StartDate"] = temp.startDate;
                newRow["Qty"] = temp.qty;
                newRow["PQty"] = temp.pqty;
                dt.Rows.Add(newRow);
            }

            for (int i = list.Count; i < initRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            /////===========Test Begin============
            //for (int i = 0; i < initRowsCount; i++)
            //{
            //    newRow = dt.NewRow();
            //    newRow["MO"] = "1";
            //    newRow["Model"] = "1730B0107701";
            //    newRow["CreateDate"] = "1/4/2012 2:52:28 PM";
            //    newRow["StartDate"] = "1/4/2012 2:52:28 PM";
            //    newRow["Qty"] = 1;
            //    newRow["PQty"] = 1;
            //    dt.Rows.Add(newRow);

            //}
            /////===========Test End============

        }
        else
        {
            for (int i = 0; i < initRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }


        GridViewExt1.DataSource = dt;
        GridViewExt1.DataBind();
        setColumnWidth();
    }

    private void setColumnWidth()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colMO").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colModel").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colCreateDate").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_colStartDate").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_colQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_colPQty").ToString();

        //this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(200);
        //this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(200);
        ////this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(0);
        ////this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(200);
        //this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(80);
        //this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(80);
    }


    /// <summary>
    /// 清空错误信息
    /// </summary>
    /// <param name="er"></param>
    private void ShowInfoClear()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" +  "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
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
    /// AlertErrorMessage for Family
    /// </summary>  
    private void alertNoFamily()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertSelectFamily,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectFamily", script, false);
    }

    
    /// <summary>
    /// AlertErrorMessage for Model
    /// </summary>  
    //private void alertNoModel()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (alertSelectModel,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectModel", script, false);
    //}
    
    /// <summary>
    /// alert Model Null
    /// </summary>  
    //private void errorMsgModelNull()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (alertModelNull,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertModelNull", script, false);
    //}

    /// <summary>
    ///置焦点:QTY
    /// </summary>  
    //private void setQtyFocus()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (qtyFocus,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "qtyFocus", script, false);
    //}

    /// <summary>
    ///ModelDropDownListCSS
    /// </summary>  
    //private void ModelDropDownListCSS()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "$('#" + CmbModel1.ClientID + "_DropDownList1').combobox();" + "\r\n" +
    //        "window.setTimeout(function(){$('input.ui-combobox').focus();},100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ModelDropDownListCSS", script, false);
    //}

    /// <summary>
    ///ModelDropDownList (No focus)
    /// </summary>  
    //private void ModelDropDownList()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "$('#" + CmbModel1.ClientID + "_DropDownList1').combobox();" + "\r\n" +
    //        "window.setTimeout (qtyFocus,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ModelDropDownList", script, false);
    //}

    /// <summary>
    ///置焦点:Model
    /// </summary>  
    //private void setModelFocus()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //       // "window.setTimeout (setModelCmbFocus,100);" + "\r\n" +
    //       "window.setTimeout(function(){$('input.ui-combobox').focus();},100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "modelFocus", script, false);
    //}

    /// <summary>
    ///置焦点:Family
    /// </summary>  
    private void setFamilyFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setFamilyCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "familyFocus", script, false);
    }

    /// <summary>
    /// Fresh Button
    /// </summary>  
    private void FreshButton()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (getMObyModel,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "getMObyModel", script, false);
    }

    
    /// <summary>
    /// Download 成功信息
    /// </summary>  
    //private void DownloadMOSucceed()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (DownloadSucceed,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "DownloadSucceed", script, false);
    //}
    private void clearTabel()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (clearTabel,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearTabel", script, false);
    }
}
