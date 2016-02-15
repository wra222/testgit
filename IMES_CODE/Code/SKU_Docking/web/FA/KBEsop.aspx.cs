using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;

public partial class FA_KBEsop : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertWrongFormat = "Wrong Code!";
    protected string AlertSuccess = "Success!";

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
        }
    }

    private void InitPage()
    {

        AlertSuccess = GetLocalResourceObject(Pre + "AlertSuccess").ToString();
        AlertWrongFormat = GetLocalResourceObject(Pre + "AlertWrongFormat").ToString();

        
        this.LabelDataEntry.Text = GetLocalResourceObject(Pre + "LabelDataEntry").ToString();
        //this.LabelProdId.Text = GetLocalResourceObject(Pre + "LabelProdId").ToString();
        //this.LabelCustSN.Text = GetLocalResourceObject(Pre + "LabelCustSN").ToString();
        //this.LabelModel.Text = GetLocalResourceObject(Pre + "LabelModel").ToString();
    }
}
