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
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

public partial class ShipToLabelPrint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.stationHF.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                InitLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }

            IShipToLabelPrintForDocking iShipToLabelPrint =
                    ServiceAgent.getInstance().GetObjectByName<IShipToLabelPrintForDocking>(WebConstant.ShipToLabelPrintForDocking);
            //exe path
            IList<string> path2 = iShipToLabelPrint.GetValueByName("PDFPrintPath");
            if (path2 != null && path2.Count > 0)
            {
                this.exepath.Value = path2[0].ToString();
            }
            else
            {
                this.exepath.Value = "C:\\FIS\\";
            }
            //pdf path
            IList<string> path1 = iShipToLabelPrint.GetValueByName("PLEditsPDF");
            if (path1 != null && path1.Count > 0)
            {
                this.pdfpath.Value = path1[0].ToString();
            }
            else
            {
                this.pdfpath.Value = "\\\\hp-iis\\OUT\\DOCKPDF\\";
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void InitLabel()
    {
        this.btnSetting.Value = this.GetLocalResourceObject(Pre + "_btnSetting").ToString();
        this.lblCarton.Text = this.GetLocalResourceObject(Pre + "_lblCarton").ToString();
        this.lblCartonNo.Text = this.GetLocalResourceObject(Pre + "_lblCartonNo").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

}
