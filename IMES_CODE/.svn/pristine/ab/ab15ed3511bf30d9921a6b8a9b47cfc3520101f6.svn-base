/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PackingPizzaLabelReprint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-11-14   zhu lei           Create 
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
using IMES.Infrastructure;
using System.Text;

public partial class PAK_PackingPizzaLabelReprint : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.txtReason.Focus();
            }
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
        this.lblPizzaID.Text = this.GetLocalResourceObject(Pre + "_lblPizzaID").ToString();
        this.btnPrintSetting.InnerText = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.lblReason.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblReason");
        this.btnReprint.InnerText = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }
}
