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
using log4net;
using System.Text;

public partial class Docking_Dismantle : System.Web.UI.Page
{
    protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";    
    public String UserId;
    public String Customer;
    public String Station;
    public String pCode;

    private const int DEFAULT_ROWS = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //this.batchdismantle.AutoPostBack = true;
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            Station = Request["Station"];
            pCode = Request["PCode"];
            if (!IsPostBack)
            {
                
                initPage();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Station = Request["Station"];
                pCode = Request["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "";
                }
                if (string.IsNullOrEmpty(pCode))
                {
                    pCode = "";
                }
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.Master.NeedPrint = false ;
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

    ///-----------------------------------
    ///Init UI page
    ///-----------------------------------
    private void initPage()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(languagePre + "lblDataEntry").ToString();
        //get user,Customer Info
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
     }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}
