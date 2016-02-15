using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;

public partial class CheckItemTypeRuleforTestREDialog : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Boolean a = Regex.IsMatch("abe", "[a-z]");
    }

    protected void betTestReg_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            if(Regex.IsMatch(this.txtInput.Value.Trim(), this.txtRegExp.Value.Trim()))
            {
                this.lblResultAnws.InnerHtml ="True!!";
            }
            else
            {
                this.lblResultAnws.InnerHtml ="False!!";
            }
        }
        catch (FisException ex)
        {
            //showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //showErrorMessage(ex.Message);
            return;
        }
        
        ScriptManager.RegisterStartupScript(this.up, typeof(System.Object), "checkReg", "", true);
    }

}
