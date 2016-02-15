/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
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
using System.Text;

public partial class PAK_PACosmeticForDocking : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.CmbPdLine.Station = Request["station"];
                this.CmbPdLine.Customer = Customer;
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
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.Panel3.GroupingText = this.GetLocalResourceObject(Pre + "_pnlInputDefectList").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.lblProductId.Text = this.GetLocalResourceObject(Pre + "_lblProductid").ToString();
        this.lblCustomerSn.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSn").ToString();
        this.Panel1.GroupingText = this.GetLocalResourceObject(Pre + "_pnlProductInformation").ToString();
        this.chk9999.Text = this.GetLocalResourceObject(Pre + "_chk9999").ToString();
    }

    private void showErrorMessage(string errorMsg)
    {
        bindTable(DEFAULT_ROWS);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefectCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }
}
