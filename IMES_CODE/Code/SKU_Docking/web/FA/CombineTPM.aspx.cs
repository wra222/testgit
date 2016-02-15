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

public partial class FA_CombineTPM : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private const int DEFAULT_ROWS = 9;
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //this.CmbCode.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCode_Selected);

            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Station = Request["Station"];

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

    private void initLabel()
    {
        this.lbPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbCollection.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblColPartType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblColPartDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblColPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblColQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblColPQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString());

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
