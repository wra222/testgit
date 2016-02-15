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
public partial class RePrintShipToCartonLabel : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IShipToCartonLabel iShipToCartonLabel =
                    ServiceAgent.getInstance().GetObjectByName<IShipToCartonLabel>(WebConstant.ShipToCartonLabelObject);

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
            IList<string> path3 = iShipToCartonLabel.GetEditAddr("PLEditsImage");
            if (path3 != null && path3.Count > 0)
            {
                this.addr.Value = path3[0].ToString();
            }
            else
            {
                this.addr.Value = "";
            }
            //exe path
            IList<string> path2 = iShipToCartonLabel.GetEditAddr("PDFPrintPath");
            if (path2 != null && path2.Count > 0)
            {
                this.exepath.Value = path2[0].ToString();
            }
            else
            {
                this.exepath.Value = "";
            }
            //pdf path
            IList<string> path1 = iShipToCartonLabel.GetEditAddr("PLEditsPDF");
            if (path1 != null && path1.Count > 0)
            {
                this.pdfpath.Value = path1[0].ToString();
            }
            else
            {
                this.pdfpath.Value = "";
            }

            //edits url
            IList<string> path4 = iShipToCartonLabel.GetEditAddr("PLEditsURL");
            if(path4 != null && path4.Count > 0)
            {
                this.editsURL.Value = path4[0].ToString();
            }
            else
            {
                this.editsURL.Value = "";
            }
            //edits xml
            IList<string> path5 = iShipToCartonLabel.GetEditAddr("PLEditsXML");
            if (path5 != null && path5.Count > 0)
            {
                this.editsXML.Value = path5[0].ToString();
            }
            else
            {
                this.editsXML.Value = "";
            }
            //edits temp
            IList<string> path6 = iShipToCartonLabel.GetEditAddr("PLEditsTemplate");
            if (path6 != null && path6.Count > 0)
            {
                this.editsTEMP.Value = path6[0].ToString();
            }
            else
            {
                this.editsTEMP.Value = "";
            }
            //edits FOP
            IList<string> path7 = iShipToCartonLabel.GetEditAddr("FOPFullFileName");
            if (path7 != null && path7.Count > 0)
            {
                this.editsFOP.Value = path7[0].ToString();
            }
            else
            {
                this.editsFOP.Value = "";
            }
            //edits PDF
            IList<string> path8 = iShipToCartonLabel.GetEditAddr("PLEditsPDF");
            if (path8 != null && path8.Count > 0)
            {
                this.editsPDF.Value = path8[0].ToString();
            }
            else
            {
                this.editsPDF.Value = "";
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
        this.lblCustomSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lbReason.Text = this.GetLocalResourceObject(Pre + "_lblReason").ToString();               
        this.btnRePrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.btnSetting.Value = this.GetLocalResourceObject(Pre + "_btnSetting").ToString();
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
