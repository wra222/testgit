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

public partial class UpdateCache : System.Web.UI.Page
{
    protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";   
    public String UserId;
    public String Customer;
    public String Station;
    protected string Pcode;
    protected string Stage;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Station = Request.QueryString["Station"];
            Pcode = Request.QueryString["PCode"];
            Stage = Request.QueryString["Stage"];
            if (!IsPostBack)
            {
        
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "";
                }
//                UserId = UserInfo.UserId;
//                Customer = UserInfo.Customer;
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

   

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
}
