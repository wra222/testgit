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
using System.Text;
using IMES.Infrastructure;

public partial class RCTOLabelPrint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);

            if (!Page.IsPostBack)
            {
                string station = Request["Station"];
                string pcode = Request["PCode"];
                InitLabel();
                
                //将pdLine combobox的初始化参数传入
                this.cmbPdLine.Stage = "PAK";
                this.cmbPdLine.Customer = Master.userInfo.Customer;

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.station1.Value = station;
                this.editor1.Value = UserId;
                this.customer1.Value = Customer;
                this.pCode1.Value = pcode;    
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message);
        }
    }

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblProdInfo.Text = this.GetLocalResourceObject(Pre + "_lblProdInfo").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSet").ToString();
		this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
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
