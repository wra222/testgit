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
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_MacRangeInfo : System.Web.UI.Page
{
    public String macRangeCode=null;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IMACRange iMACRange;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!this.IsPostBack)
            {
                macRangeCode = Request.QueryString["MACRangeCode"];
                //macRangeCode = "001";
                initLabel();
                iMACRange = (IMACRange)ServiceAgent.getInstance().GetMaintainObjectByName<IMACRange>(WebConstant.MaintainCommonObject);
                MACInfoDef info =iMACRange.GetMACInfo(macRangeCode);
                ShowInfo(info);

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


    private void initLabel()
    {
        this.lblMACRangeCode.Text = this.GetLocalResourceObject(Pre + "_MACRangeCode").ToString();
        this.lblMACTotal.Text = this.GetLocalResourceObject(Pre + "_MACTotal").ToString();
        this.lblMACUsed.Text = this.GetLocalResourceObject(Pre + "_MACUsed").ToString();
        this.lblMACleft.Text = this.GetLocalResourceObject(Pre + "_MACleft").ToString();
        this.lblHDCPQuery.Text = this.GetLocalResourceObject(Pre + "_HDCPQuery").ToString();
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();

    }

    private void ShowInfo(MACInfoDef info)
    {
        this.MACleft.Text = info.MACleft;
        this.MACRangeCode.Text = info.MACRangeCode;
        this.MACTotal.Text = info.MACTotal;
        this.MACUsed.Text = info.MACUsed;
        this.HDCPQuery.Text = info.HDCPQuery;
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ToShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
