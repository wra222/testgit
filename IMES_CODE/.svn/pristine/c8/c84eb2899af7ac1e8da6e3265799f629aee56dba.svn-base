/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: 2PP Input
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
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

public partial class FA_Loc2PP : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
               
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
				selLocType.Items.Add(new ListItem("RunIn", "LCR"));
				selLocType.Items.Add(new ListItem("D/L", "LCD"));
                selLocType.Items.Add(new ListItem("AutoDownLoad", "AD"));
				
                setFocus();
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
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblReworkInput").ToString();
        this.lblProdId.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
    }

    private void setFocus()
    {
        //String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        //ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
        //DEBUG ITC-1360-0729
        String script = "<script language='javascript'>" + "\r\n" +
                        "window.setTimeout (callNextInput,100);" + "\r\n" +
                        "</script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "callNextInput", script, false);
    }
}
