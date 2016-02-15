
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;


public partial class PAK_UnpackAllByDN : IMESBasePage
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
                this.station.Value = Request["Station"];
            }
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
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
        this.lblDeliveryNo.Text = this.GetLocalResourceObject(Pre + "_lblDeliveryNo").ToString();
    }

    private void writeToAlertMessage(string errorMsg)
    {


        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void UnpackSuccess()
    {


        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("unpackSuccess();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "UnpackSuccess", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "endWaitingCoverDiv", script, false);
    }

    private void callNextInput()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("callNextInput();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "callNextInput", scriptBuilder.ToString(), false);
    }

    protected void hidbtn_Click(Object sender, EventArgs e)
    {
        IUnpack iUnpack = ServiceAgent.getInstance().GetObjectByName<IUnpack>(WebConstant.UnpackObject);
        string allErrors = String.Empty;
        IList<string> proList = new List<string>();
        try
        {
            proList = iUnpack.CheckDNAndGetProductListByDN(this.hidDN.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }

        if (proList != null && proList.Count > 0)
        {
            foreach (string tmp in proList)
            {
                try
                {
                    iUnpack.UnpackAllByDN(tmp, "", UserId, this.station.Value, Customer);
                }
                catch (FisException ee)
                {
                    allErrors = allErrors + ee.mErrmsg + "\r\n";
                    continue;
                }
                catch (Exception ex)
                {
                    allErrors = allErrors + ex.Message + "\r\n";
                    continue;
                }
            }

            if (allErrors != "")
            {
                writeToAlertMessage(allErrors);
            }
            else
            {
                UnpackSuccess();
            }
            endWaitingCoverDiv();
        }
        else
        {
            endWaitingCoverDiv();
        }
        callNextInput();
        
    }



}
