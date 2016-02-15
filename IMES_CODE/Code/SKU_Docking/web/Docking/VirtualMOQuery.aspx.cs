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
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using com.inventec.imes.DBUtility;


public partial class FA_VirtualMOQuery : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IVirtualMoForDocking iVirtualMo = ServiceAgent.getInstance().GetObjectByName<IVirtualMoForDocking>(WebConstant.VirtualMoForDocking);

    public String UserId;
    public String Customer;
    public int initRowsCount = 6;

    public string curFamily = string.Empty;
    public string curModel = string.Empty;
    public string curStation = string.Empty;
    private int maxLine = 1000;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////注册下拉框的选择事件
            this.CmbFamily1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
            this.CmbFamily1.InnerDropDownList.AutoPostBack = true;

            if (!IsPostBack)
            {
                InitLbl();
                bindTable(null);
                setColumnWidth();
                
                //UserId = Master.userInfo.UserId;
                //Customer = Master.userInfo.Customer;
                UserId = Request["UserId"];
                Customer = Request["Customer"];
                curStation = Request["Station"];

                this.CmbFamily1.Station = curStation;
                this.CmbFamily1.Customer = Customer;
                this.CmbFamily1.Service = "VirtualMoForDocking";

                Master.DisplayInfoArea = false;
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
            //禁用查询按键
            this.btnQuery.Disabled = true;

            ShowInfoClear();
            curFamily = string.Empty;
            curModel = string.Empty;

            this.CmbFamily1.Station = Request["Station"];
            this.CmbFamily1.Customer = Request["Customer"];
            this.CmbFamily1.Service = "VirtualMoForDocking";

            //清空MO输入框
            //this.CmbMo1.InnerDropDownList.SelectedValue = "";
            
            //清空Model输入框
            //this.CmbModel1.InnerDropDownList.SelectedValue = "";
            
            //清空表格
            clearTabel();
           

            if (this.CmbFamily1.InnerDropDownList.SelectedValue == "")
            {
                //this.CmbModel1.clearContent();
                //this.CmbMo1.clearContent();
            }
            else
            {
                // 刷新Model下拉框
                curFamily = this.CmbFamily1.InnerDropDownList.SelectedValue;
                //this.CmbModel1.Service = "102";
                //this.CmbModel1.Station = Request["Station"];
                //this.CmbModel1.Customer = Request["Customer"];
                //this.CmbModel1.refreshDropContent(curFamily);


                // 如果Model List仅有1条记录，省选中此model
                //if (this.CmbModel1.InnerDropDownList.Items.Count == 2)
                //{
                //    this.CmbModel1.setSelected(1);
                //    curModel = this.CmbModel1.InnerDropDownList.SelectedValue;
                //    this.txtMO.Focus();
                //}
                //else
                //{
                //    this.CmbModel1.setSelected(-1);
                    //setModelFocus();
                //} // 取消Model一条记录的自动带出，因为可以对Family单独查询

                //this.CmbModel1.setSelected(-1);
                //setModelFocus();
                //ModelDropDownListCSS();
               // this.txtModel.Focus();
            }
            //启用查询按键
            this.btnQuery.Disabled = false;
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

    private void cmbModel_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //禁用查询按键
            this.btnQuery.Disabled = true;

            ShowInfoClear();
            curModel = string.Empty;
            //this.CmbModel1.Service = "102";
            //this.CmbModel1.Station = Request["station"];
            //this.CmbModel1.Customer = Master.userInfo.Customer;

            //清空MO输入框
            //this.CmbMo1.InnerDropDownList.SelectedValue = "";
            
            //清空表格
            clearTabel();
            
            //if (this.CmbModel1.InnerDropDownList.SelectedValue == "")
            {
            //    this.CmbMo1.clearContent();
            }
            //else
            {
            //    curModel = this.CmbModel1.InnerDropDownList.SelectedValue;
            //    this.CmbMo1.Service = "VirtualMoForDocking";
            //    this.CmbMo1.refreshDropContent(curModel);
            }

            //启用查询按键
            this.btnQuery.Disabled = false;

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


    private void GetMOInfo()
    {
        try
        {
            if (this.txtMO.Value == "" && this.txtModel.Value == "" && this.CmbFamily1.InnerDropDownList.SelectedValue == "")
            {
                alertNoQueryCondAndFocus();
            }
            else
            {
                //if (!string.IsNullOrEmpty(this.txtMO.Value))
                //{
                //    this.CmbFamily1.setSelected(-1);
                //    this.CmbModel1.setSelected(-1);
                    //this.txtModel.Value = "";
                //}
               // else if (!string.IsNullOrEmpty(this.txtModel.Value))
                //{
               //     this.CmbFamily1.setSelected(-1);
               // }

                DateTime startTime = Convert.ToDateTime(this.hidStart.Value.ToString());
                DateTime endTime = Convert.ToDateTime(this.hidEnd.Value.ToString());

                string mo = this.txtMO.Value.ToUpper();
                //string model = this.CmbModel1.InnerDropDownList.SelectedValue;
                string model = this.txtModel.Value.ToUpper();
                string family = this.CmbFamily1.InnerDropDownList.SelectedValue;
                IList<MOInfo> list = iVirtualMo.GetVirtualForQuery2(mo, model, family, startTime, endTime);               
                bindTable(list);
                if (list != null && list.Count > maxLine)
                {
                    ShowInfo1000();
                }
              //  ModelDropDownList();

              //  this.CmbFamily1.setSelected(-1);
                this.txtModel.Value = "";
                this.txtMO.Value = "";
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

   

    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            GetMOInfo();
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



    private void InitLbl()
    {
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblMO.Text = this.GetLocalResourceObject(Pre + "_lblMO").ToString();
        this.lblStart.Text = this.GetLocalResourceObject(Pre + "_lblStart").ToString();
        this.lblEnd.Text = this.GetLocalResourceObject(Pre + "_lblEnd").ToString();
        this.btnQuery.Value=this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        this.btnExecl.Value = this.GetLocalResourceObject(Pre + "_btnExcel").ToString();
        this.lblCreatedMOList.InnerText = this.GetLocalResourceObject(Pre + "_lblCreatedMOList").ToString();

    }

    private DataTable initBomTable()
    {
       
        DataTable retTable = new DataTable();

        //retTable.Columns.Add(" ");
        retTable.Columns.Add("MO", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("CreateDate", Type.GetType("System.DateTime"));
        retTable.Columns.Add("StartDate", Type.GetType("System.DateTime"));
        retTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        retTable.Columns.Add("PQty", Type.GetType("System.Int32"));
        retTable.Columns.Add("CustomerSN_Qty", Type.GetType("System.Int32"));
        return retTable;
        
    }

    private DataTable bindTable(IList<MOInfo> list)
    {
        DataTable dt = initBomTable();
        DataRow newRow = null;
        
        if (list != null && list.Count != 0)
        {
            //foreach (MOInfo temp in list)
            for (int j = 0; j < list.Count && j < maxLine; j++)
            {
                newRow = dt.NewRow();
                newRow["MO"] = list[j].id;
                newRow["Model"] = list[j].model;
                newRow["CreateDate"] = list[j].createDate;
                newRow["StartDate"] = list[j].startDate;
                newRow["Qty"] = list[j].qty;
                newRow["PQty"] = list[j].pqty;
                newRow["CustomerSN_Qty"] = list[j].customerSN_Qty;
                dt.Rows.Add(newRow);
            }

            for (int i = list.Count; i < initRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
           
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

        return dt;
    }

    private void setColumnWidth()
    {
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colMO").ToString();
        this.GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colModel").ToString();
        this.GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_colCreateDate").ToString();
        this.GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_colStartDate").ToString();
        this.GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_colQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_colPQty").ToString();
        this.GridViewExt1.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_colCustomerSN_Qty").ToString();
        
        

        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(90);
        //this.GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(120);
        //this.GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(120);
        //this.GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(50);
        //this.GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(60);
    }

    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) || (e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                CheckBox check = (CheckBox)e.Row.FindControl("RowChk");
                check.Style.Add("display", "none");
            }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace(Environment.NewLine, "<br>");
                }
            }
        }
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

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ShowInfoClear", scriptBuilder.ToString(), false);
    }

    private void ShowInfo1000()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"只能显示MO的前1000行数据，请修改查询条件，减少MO获取数量\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ShowInfoClear", scriptBuilder.ToString(), false);
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
    //private void alertNoFamily()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (alertSelectFamily,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectFamily", script, false);
    //}

    
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
    /// AlertErrorMessage for No Query Conidtion and Set Focus
    /// </summary>  
    private void alertNoQueryCondAndFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertNoQueryCondAndFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertNoQueryCondAndFocus", script, false);
    }

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

    ///// <summary>
    /////ModelDropDownList (No focus)
    ///// </summary>  
    //private void ModelDropDownList()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "$('#" + CmbModel1.ClientID + "_DropDownList1').combobox();" + "\r\n" +
    //        "window.setTimeout (MoFocus,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ModelDropDownList", script, false);
    //}

    /// <summary>
    ///置焦点:Model
    /// </summary>  
    //private void setModelFocus()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        //"window.setTimeout (setModelCmbFocus,100);" + "\r\n" +
    //         "window.setTimeout(function(){$('input.ui-combobox').focus();},100);" + "\r\n" +
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
    //private void FreshButton()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (getMO,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "getMObyModel", script, false);
    //}

       
    private void clearTabel()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (clearTabel,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearTabel", script, false);
    }

    private void getDateTime()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDateTime();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "getDateTime", scriptBuilder.ToString(), false);
    }

    protected void excelClick(object sender, System.EventArgs e)
    {
        DataTable2Excel(GetMOInfobyModel_excel());
    }
    
    private DataTable GetMOInfobyModel_excel()
    {
        DateTime startTime = Convert.ToDateTime(this.hidStart.Value.ToString());
        DateTime endTime = Convert.ToDateTime(this.hidEnd.Value.ToString());
        DataTable dt = null;

        string mo = this.txtMO.Value.ToUpper();
        string model = this.txtModel.Value.ToUpper();
        string family = this.CmbFamily1.InnerDropDownList.SelectedValue;
        IList<MOInfo> list = iVirtualMo.GetVirtualForQuery2(mo, model, family, startTime, endTime);

        dt = bindTable(list);

        return dt;
    }

    public static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            //            <bug>
            //            BUG NO:ITC-1103-0261 
            //            REASON:中文编码
            //            </bug>
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "UTF-8";
            curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312 >");
            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            for (int i = 0; i < dgExport.Items.Count; i++)
            {
                //excel.Columns[3].ItemStyle.CssClass = "xlsText";
                //excel.Items[i].Cells[3].Style.Add("mso-number-format", "\"@\"");
                for (int j = 0; j < 7; j++)
                {
                    //if (j == 7)
                    //{
                    //    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:yyy-mm-dd ##:##:##");
                    //}
                    //else
                    //{
                    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    //}
                }
            }

            // 返回客户端
            dgExport.RenderControl(htmlWriter);

            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    
}
