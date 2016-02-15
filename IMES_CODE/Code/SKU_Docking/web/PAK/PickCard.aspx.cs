/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PickCard
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-27   zhu lei          Create 
 * 
 * Known issues:Any restrictions about this file 
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
using com.inventec.iMESWEB;
using System.Text;
using IMES.Infrastructure;

public partial class PAK_PickCard : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
           
            }
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }


    private void initLabel()
    {
        this.btnPrintSetting.InnerText = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblDriver.Text = this.GetLocalResourceObject(Pre + "_lblDriver").ToString();
        this.lblForwarder.Text = this.GetLocalResourceObject(Pre + "_lblForwarder").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblTruckID").ToString();
    }

 
}
