/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: CombineCOAandDNReprint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-2-7     207003           Create 
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
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;
public partial class DOCK_UnpackAllbyDN : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IUnpackAllbyDN iUnpackAllbyDN = ServiceAgent.getInstance().GetObjectByName<IUnpackAllbyDN>(WebConstant.UnpackAllbyDN);
    public String UserId;
    public String Customer;
    public String station;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnGetDN.ServerClick += new EventHandler(btnGetDN_ServerClick);
        btnDoUnpack.ServerClick += new EventHandler(btnDoUnpack_ServerClick);
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
        if (null == station || "" == station)
        {
            station = Request["Station"];
        }
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                //ResetAll("");
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
    public void btnGetDN_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iUnpackAllbyDN.GetDN(HDN.Value);
            ReConfirm();
            return;
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ResetAll("");
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ResetAll("");
            return;
        }
    }
    private void btnDoUnpack_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iUnpackAllbyDN.DoUnpackByDN(UserId, station, Customer, HDN.Value);
            ResetAll("success");
            return;
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ResetAll("");
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ResetAll("");
            return;
        }
    }
   
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void initLabel()
    {
        this.lblPizzaID.Text = this.GetLocalResourceObject(Pre + "_lblPizzaID").ToString();
        this.btnUnpack.InnerText = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }
    private void ResetAll(string display)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAll(\"" + display + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAll", scriptBuilder.ToString(), false);
    }
    private void ReConfirm()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getConfirm();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReConfirm", scriptBuilder.ToString(), false);
    }
}
