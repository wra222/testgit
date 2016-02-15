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
using System.Web.Services;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_OfflinePizzaKitting : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 6;
    private const int DEFAULT_ROWS =6;
    public string userId;
    public string customer;
    public string station;
    public string pCode;
    public String AccountId;
    public String Login;
    public String UserName;
 //   public string pcode;
    IOfflinePizzaKitting offlinePizzaKitting = ServiceAgent.getInstance().GetObjectByName<IOfflinePizzaKitting>(WebConstant.IOfflinePizzaKitting);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
         
        {
         
            this.cmbConstValueType1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbConstValueType1_Selected);
            this.cmbModel1.InnerDropDownList.SelectedIndexChanged +=new EventHandler(cmbModel1_Selected);
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbLine_Selected);
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            cmbModel1.Service = "002";
            userId = Master.userInfo.UserId;
            customer = Master.userInfo.Customer;
            AccountId = Master.userInfo.AccountId.ToString();
            Login = Master.userInfo.Login;
            UserName = Master.userInfo.UserName;
            if (!Page.IsPostBack)
            {
        
                InitLabel();
                bindTable(DEFAULT_ROWS);
                initTableColumnHeader();
                cmbConstValueType1.Type = "OfflinePizzaFamily";
                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Stage = "PAK";
                this.cmbPdLine.Customer = customer;
                station = Request["Station"];
                pCode = Request["PCode"];
               
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
    private void cmbModel1_Selected(object sender, System.EventArgs e)
    {
        CallClientFun("onModelChange");
    }
    private void CallClientFun(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName + "();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "funcName", scriptBuilder.ToString(), false);
    }
    private void cmbLine_Selected(object sender, System.EventArgs e)
    {
       
        CallClientFun("onModelChange");
    }
    private void cmbConstValueType1_Selected(object sender, System.EventArgs e)
    {
        try
        {
         
            if (this.cmbConstValueType1.InnerDropDownList.SelectedValue == "")
            {
                this.cmbModel1.clearContent();

            }
            else
            {
                this.cmbModel1.refreshDropContent(this.cmbConstValueType1.InnerDropDownList.SelectedValue);
                 if (this.cmbModel1.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbModel1.InnerDropDownList.SelectedIndex =0;
                  
                }

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

    private void InitLabel()
    {
        
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();

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

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }


   
   

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("PartNo");
        dt.Columns.Add("PartType");
        dt.Columns.Add("Description");
        dt.Columns.Add("Qty");
        dt.Columns.Add("PQty");
        dt.Columns.Add("Collection");

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }
         gd.DataSource = dt;
         gd.DataBind();
    }
   
    
    private void initTableColumnHeader()
    {
      
        this.gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        this.gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);
   
    }

   

    /// <summary>
    /// 清空表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            bindTable(DEFAULT_ROWS);
   
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
    /// 扫入ProdId后，获取part Bom信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FreshGrid(object sender, System.EventArgs e)
    {
        try
        {
            cmbModel1.InnerDropDownList.SelectedIndex = 0;
            return;
   
        }
     
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
     
    }

    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "callInputRun", script, false);
    }



    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
   
 
}
