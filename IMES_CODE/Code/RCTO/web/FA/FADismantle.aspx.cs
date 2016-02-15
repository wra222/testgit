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

public partial class FA_Dismantle : System.Web.UI.Page
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
            this.DropDismantletype.InnerDropDownList.SelectedIndexChanged += new EventHandler(dismantletypeChanged);
            this.DropDismantletype.InnerDropDownList.AutoPostBack = true;
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
                this.DropDismantletype.DismantleType = "DismantleType";
                this.DropDismantletype.Enabled = true;
                this.CmbKeyparts.DismantleType = "DismantleKP";
                this.CmbReturnstation.DismantleType = "DismantleReturnWC";
                //this.lblKeyparts.Enabled = false;
                
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
        this.lblDismantletype.Text = this.GetLocalResourceObject(languagePre + "lblDismantletype").ToString();
        this.lblPdline.Text = this.GetLocalResourceObject(languagePre + "lblPdline").ToString();
        this.lblProdId.Text = this.GetLocalResourceObject(languagePre + "lblProdId").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(languagePre + "lblModel").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(languagePre + "lblDataEntry").ToString();
        this.batchdismantle.Text = this.GetLocalResourceObject(languagePre + "batchdismantle").ToString();
        this.lblKeyparts.Text = this.GetLocalResourceObject(languagePre + "lblKeyparts").ToString();
        this.lblReturnStation.Text = this.GetLocalResourceObject(languagePre + "lblReturnStation").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(languagePre + "lblCustSN").ToString();
        
        //get user,Customer Info
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
     }

   /* 
    protected void batchdismantle_CheckedChanged(object sender, EventArgs e)
    {

        //oncheckedchanged="batchdismantle_CheckedChanged"
        if (batchdismantle.Checked == true)
        {
            String script = "<script language='javascript'>" + "\r\n" +
                         "window.setTimeout (setCmbKPenabled,100);" + "\r\n" +
                         "</script>";
            ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setCmbKPenabled", script, false);
      
        }
        else
        {
            String script = "<script language='javascript'>" + "\r\n" +
                "window.setTimeout (setCmbKPdisabled,100);" + "\r\n" +
                "</script>";
            ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setCmbKPdisabled", script, false);

        }

    }
    */

     private void dismantletypeChanged(object sender, EventArgs e)
    {
        if (this.DropDismantletype.InnerDropDownList.SelectedValue.Trim() == "KP")//KP
        {

            //this.lblKeyparts.ForeColor = System.Drawing.Color.Black;//"Black"; 
            this.CmbKeyparts.InnerDropDownList.Enabled = true;
            this.CmbKeyparts.refresh();
            this.CmbReturnstation.InnerDropDownList.Enabled = true;
            this.CmbReturnstation.refresh();
            
        }
        else
        {
            //this.lblKeyparts.ForeColor = System.Drawing.Color.Gray;// "Gray"; 
            
            //this.lblKeyparts.Enabled = false;
            this.CmbKeyparts.InnerDropDownList.Enabled = false;
            this.CmbKeyparts.refresh();
            this.CmbReturnstation.InnerDropDownList.Enabled =false;
            this.CmbReturnstation.refresh();
        }
         /*
        if (this.DropDismantletype.InnerDropDownList.SelectedValue.Trim() != "PC")//PC
        {
            //this.batchdismantle.Visible = false;
            String script = "<script language='javascript'>" + "\r\n" +
                "window.setTimeout (setbachfileenbabled,100);" + "\r\n" +
                "</script>";
            ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setbachfileenbabled", script, false);
        }
        else
        {
            //this.batchdismantle.Visible = true;
            
            String script = "<script language='javascript'>" + "\r\n" +
                "window.setTimeout (setbachfiledisabled,100);" + "\r\n" +
                "</script>";
            ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setbachfiledisabled", script, false);

        }
        */
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
