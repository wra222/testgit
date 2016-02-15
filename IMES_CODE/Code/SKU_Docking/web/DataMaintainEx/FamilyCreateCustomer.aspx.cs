using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;


public partial class FamilyCreateCustomer : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    ICustomer iCustomer = ServiceAgent.getInstance().GetMaintainObjectByName<ICustomer>(WebConstant.MaintainCommonObject);
    private string editor;

    private string strProcessName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            editor = Request.QueryString["editor"];//"itc98079";//
            if (!Page.IsPostBack)
            {
                InitLabel();
                
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

    }




    protected void btnOK_Click(Object sender, EventArgs e)
    {
        string strCustomer = txtCustomer.Text;
        string strPlant = txtPlant.Text;
        string strCode = txtCode.Text;
        string strDescr = txtDescr.Text;
        CustomerInfo customerInfo = new CustomerInfo();
        try
        {
            customerInfo.customer = strCustomer;
            customerInfo.Code = strCode;
            customerInfo.Plant = strPlant;
            customerInfo.Description = strDescr;


            iCustomer.AddCustomer(customerInfo);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "close", "onOk();", true);

    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomer").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblPlant.Text = this.GetLocalResourceObject(Pre + "_lblPlant").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();

        //setFocus();


    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
