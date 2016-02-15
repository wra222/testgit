/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for MB CT Check Page
* Known issues:
* TODO：
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
//using IMES.Station.Interface.StationIntf;
//using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_MBCTCheck : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
           UserId = Master.userInfo.UserId;
           this.hideditor.Value = UserId;
           Customer = Master.userInfo.Customer;
           this.hidCustomer.Value = Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                this.hiddenStation.Value = Request["Station"];
                
                this.pCode.Value = Request["PCode"];
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */
                this.txtDataEntry.Focus();
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

    private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        //this.lblPcbNo.Text = "PcbNo";
        //this.lblMBCT.Text = "first KBCT";
    }

    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            //ÖØÖÃÎÄ±¾¿ò
            this.lblDataEntry.Text = "";
            this.lblLCM.Text = "";
            this.lblMBCT.Text = "";
            this.lblSN.Text = "";
            this.txtLCM.Text = "";
            this.txtMBCT.Text = "";
            this.txtSN.Text = "";
            this.txtDataEntry.Focus();
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
}


