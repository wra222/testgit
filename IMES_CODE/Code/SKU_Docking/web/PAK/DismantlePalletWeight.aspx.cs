using System;
using System.Data;
using com.inventec.iMESWEB;

public partial class PAK_DismantlePalletWeight : System.Web.UI.Page
{

    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertInputPalletOrDn = "Please input Pallet No or Delivery No!";
    protected string AlertInputCorrectPalletOrDn = "Please input correct Pallet No or Delivery No!";
    protected string MsgSuccess = "Success!";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
        }
    }

    private void InitPage()
    {
        MsgSuccess = GetLocalResourceObject(Pre + "MsgSuccess").ToString();
        AlertInputPalletOrDn = GetLocalResourceObject(Pre + "AlertInputPalletOrDn").ToString();
        AlertInputCorrectPalletOrDn = GetLocalResourceObject(Pre + "AlertInputCorrectPalletOrDn").ToString();
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("DeliveryNo", Type.GetType("System.String"));
        result.Columns.Add("PalletNo", Type.GetType("System.String"));
        result.Columns.Add("Weight", Type.GetType("System.String"));
        result.Columns.Add("WeightL", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();
            for (int j = 0; j < 4; j++)
            { newRow[j] = string.Empty; }
            result.Rows.Add(newRow);
        }
        return result;
    }

    private int DefaultRowsCount = 7;

}
