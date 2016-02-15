/*
* INVENTEC corporation ?011 all rights reserved. 
* Description:UI for ConmbineTPM Page
* UI:CI-MES12-SPEC-FA-UI ConmbineTPM.docx ?011/10/11 
* UC:CI-MES12-SPEC-FA-UC ConmbineTPM.docx ?011/10/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   yang jie               (Reference Ebook SourceCode) Create
* Known issues:
*/
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
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_CollectTabletPakPart : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private const int DEFAULT_ROWS = 9;
    public String UserId;
    public String Customer;
    public String Station;
    public String AccountId;
    public String Login;
    public String UserName;
    public string pcode;
    public string PDFClinetPath = ConfigurationManager.AppSettings["UnitWeightPDFPath"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //this.CmbCode.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCode_Selected);
            if (string.IsNullOrEmpty(Customer))
            { 
				Customer = Master.userInfo.Customer; 
			}
            if (string.IsNullOrEmpty(UserId))
            { 
				UserId = Master.userInfo.UserId; 
			}
             Station = Request["Station"];
             if (Station == "T4")
             {
                 this.txt.IsKeepWhitespace = true;
             }
         

			pcode = Request["PCode"];

            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);
                CheckPrintItem();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
               
             
                //this.cmbPdLine.Station = Station;
                if ((Station ==null) ||(Station ==""))
                {
                    this.cmbPdLine.Stage = "FA";
                }
                else
                {
                    this.cmbPdLine.Station = Station;
                }
                this.cmbPdLine.Customer = Customer;
                AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                UserName = Master.userInfo.UserName;



                //setColumnWidth();
                //setFocus();
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.Message);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
    private void CheckPrintItem()
    {
        IPrintTemplate iPrintTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);
      IList<string> labelTypeList = iPrintTemplate.GetPrintLabelTypeList(pcode);
      if (labelTypeList != null && labelTypeList.Count != 0)
      {
          btnPrintSet.Visible = true;
          btnReprint.Visible = true;
      }
      else
      {
          btnPrintSet.Visible = false;
          btnReprint.Visible = false;
      }
    }
    private void initLabel()
    {
        this.lbPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbCollection.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();

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
        GridViewExt1.DataSource = dt;
        GridViewExt1.DataBind();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
    }

    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('input'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "callInputRun", script, false);
    }

}
