/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PCAICTInput
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;

public partial class SA_PCAICTInput : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;
    public String Station;
    private IQty iQty = ServiceAgent.getInstance().GetObjectByName<IQty>(WebConstant.CommonObject);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            Station = Request["Station"];         
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);
                this.cmbPdLine.Customer = Customer;
                this.cmbPdLine.Station = Station;
                this.cmbDataCode.Customer = Customer;
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

    /// <summary>
    /// 选择PdLine下拉框，会获得OK Qty
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新Model下拉框内容
            if (this.cmbPdLine.InnerDropDownList.SelectedValue == "")
            {
                //清空Model下拉框内容
                this.lblQtyContent.Text = "0";

            }
            else
            {
                //方法暂缺
                var qty = (int)iQty.GetQty(this.cmbPdLine.InnerDropDownList.SelectedValue);
                this.lblQtyContent.Text = qty.ToString();
                UpdatePanel1.Update();
            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }

    }

    private void initLabel()
    {
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblDefectList.Text = this.GetLocalResourceObject(Pre + "_pnlInputDefectList").ToString();
        this.lblAoiNo.Text = this.GetLocalResourceObject(Pre + "_lblAoiNo").ToString();
        this.lblECR.Text = this.GetLocalResourceObject(Pre + "_lblECR").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblInputAoi.Text = this.GetLocalResourceObject(Pre + "_lblInputAoi").ToString();
        this.chk9999.Text = this.GetLocalResourceObject(Pre + "_chk9999").ToString();
        this.ChkRCTO.Text = this.GetLocalResourceObject(Pre + "_ChkRCTO").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblPdline.Text = this.GetLocalResourceObject(Pre + "_lblPdline").ToString();
        this.lblDataCode.Text = this.GetLocalResourceObject(Pre + "_lblDataCode").ToString();
        this.btnClearQty.Value = this.GetLocalResourceObject(Pre + "_btnClearQty").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.lblQtyContent.Text = "0";
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
