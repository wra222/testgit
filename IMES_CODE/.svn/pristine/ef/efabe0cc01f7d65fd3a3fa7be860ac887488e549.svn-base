using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using com.inventec.iMESWEB;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class Docking_ChangeModel : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertInputModel1 = "";
    protected string AlertInputModel2 = "";
    protected string AlertSelectStation = "";
    protected string AlertInputChangeQty = "";
    protected string AlertWrongChangeQty = "";
    protected string AlertWrongCode = "";
    protected string AlertModelSame = "";
    protected string ChangeModelSuccess = "";
    protected string AlertExcel = "";

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

        this.LabelModel1.Text = GetLocalResourceObject(Pre + "LabelModel1").ToString();
        this.LabelModel2.Text = GetLocalResourceObject(Pre + "LabelModel2").ToString();
        this.LabelDataEntry.Text = GetLocalResourceObject(Pre + "LabelDataEntry").ToString();
        this.LabelCurrentStation.Text = GetLocalResourceObject(Pre + "LabelCurrentStation").ToString();
        this.LabelQty.Text = GetLocalResourceObject(Pre + "LabelQty").ToString();
        this.LabelChangeQty.Text = GetLocalResourceObject(Pre + "LabelChangeQty").ToString();

        AlertInputModel1 = GetLocalResourceObject(Pre + "AlertInputModel1").ToString();
        AlertInputModel2 = GetLocalResourceObject(Pre + "AlertInputModel2").ToString();
        AlertSelectStation = GetLocalResourceObject(Pre + "AlertSelectStation").ToString();
        AlertInputChangeQty = GetLocalResourceObject(Pre + "AlertInputChangeQty").ToString();
        AlertWrongChangeQty = GetLocalResourceObject(Pre + "AlertWrongChangeQty").ToString();
        AlertWrongCode = GetLocalResourceObject(Pre + "AlertWrongCode").ToString();
        AlertModelSame = GetLocalResourceObject(Pre + "AlertModelSame").ToString();
        ChangeModelSuccess = GetLocalResourceObject(Pre + "ChangeModelSuccess").ToString();
        AlertExcel = GetLocalResourceObject(Pre + "AlertExcel").ToString();
    }
}
