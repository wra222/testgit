
/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/11/04 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/11/04            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-04   Du.Xuan               Create   
* Known issues:
* TODO：
* [ITC-1360-0007][PalletType下拉框不能显示]:type传入错误
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
using log4net;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;


public partial class PAK_PalletWeight : System.Web.UI.Page
{
    protected string languagePre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_";
    private const int DEFAULT_ROWS = 3;
    public String UserId;
    public String Customer;
    public String Station;
    protected string Pcode;
    protected int isTestWeight = WebCommonMethod.isTestWeight();
    protected IPalletWeight palletweightManager = (IPalletWeight)ServiceAgent.getInstance().GetObjectByName<IPalletWeight>(WebConstant.PalletWeight);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int todayQty = 0;
            if (!IsPostBack)
            {
                initPage();
                bindTable();
                Station = Request.QueryString["Station"];
                Pcode = Request.QueryString["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "";
                }
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                todayQty= palletweightManager.getQtyOfPalletToday();
                this.lblTotalQtyValue.Text = Convert.ToString(todayQty);
                //ITC-1360-0007
                this.cmbPalletKind.PalletType = "PT";

                //IList<ConstValueInfo> GetConstValueListByType(string type)
                //IPartRepository

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

    private void initPage()
    {
        this.lblTotalQty.Text = GetLocalResourceObject(languagePre + "TotalQty").ToString();
        this.lblPalletKind.Text = GetLocalResourceObject(languagePre + "lblPalletType").ToString();
        this.lblCustSN.Text = GetLocalResourceObject(languagePre + "CustSN").ToString();
        this.lblUnitWeight.Text = GetLocalResourceObject(languagePre + "UnitWeight").ToString();
        this.lblPalletNo.Text = GetLocalResourceObject(languagePre + "PalletNo").ToString();
        this.lblProductInfo.Text = this.GetLocalResourceObject(languagePre + "lblProductInfo").ToString();
        this.lblManual.Text = this.GetLocalResourceObject(languagePre + "WeightManual").ToString();
        this.lblKG.Text = this.GetLocalResourceObject(languagePre + "KG").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "lblDataEntry").ToString();
        this.btpPrintSet.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "btnPrtSet").ToString();
        this.btnReprint.InnerText = GetLocalResourceObject(languagePre + "btnReprint").ToString();
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void bindTable()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(languagePre + "Model").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(languagePre + "PCs").ToString());

        for (int i = 0; i < DEFAULT_ROWS; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }
}
