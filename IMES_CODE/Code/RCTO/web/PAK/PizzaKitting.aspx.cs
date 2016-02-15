/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* ITC-1360-0932 添加筛选pdLine
* ITC-1360-1643 恢复_colPartNo列
* Known issues:
* TODO：
* 
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
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;

public partial class PAK_PizzaKitting : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);

                
                Station = Request["Station"];
                this.customerHidden.Value = Customer;
                //this.CmbPdLine1.Customer = Customer;
                //this.CmbPdLine1.Station = Station;
                //this.CmbPdLine1.Stage = "PAK";

                //this.CmbStation.Type = "PAKKitting";
                this.CmbStation.Station = Station;
                this.CmbStation.Customer = Customer;
                this.CmbStation.StationType = "PAKKitting";
                //setColumnWidth();
                //setFocus();
            }
            this.CmbStation.InnerDropDownList.SelectedIndexChanged += new EventHandler(CmbStation_Selected);
            this.CmbStation.InnerDropDownList.AutoPostBack = true;
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.Message);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPizza.Text = this.GetLocalResourceObject(Pre + "_lblPizza").ToString();
        this.lblLabelList.Text = this.GetLocalResourceObject(Pre + "_lblLabelList").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet"); 
        //this.CmbPdLine1.InnerDropDownList.SelectedIndex = 1;
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCollection").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
    }

    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "callInputRun", script, false);
    }

    private void CmbStation_Selected(object sender, System.EventArgs e)
    {
        try
        {
            if (this.CmbStation.InnerDropDownList.SelectedValue == "")
            { 
                this.CmbPdLine1.clearContent(); 
            }
            else
            {
                var selecttstation = CmbStation.InnerDropDownList.SelectedValue;
                var selectLine = CmbPdLine1.InnerDropDownList.SelectedItem.Text;

                this.CmbPdLine1.refresh(this.CmbStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
                int i = 0;
                foreach (ListItem lst in this.CmbPdLine1.InnerDropDownList.Items)
                {
                    if (selectLine == lst.Text)
                    {
                        CmbPdLine1.InnerDropDownList.SelectedIndex = i;
                        return;
                    }
                    i++;
                }
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
    
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;

            this.CmbStation.setSelected(0);
            CmbStation_Selected(sender, e);
            setFocus();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);

        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);

        }
    }
    /// <summary>
    /// btnFru_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnFru_Click(object sender, System.EventArgs e)
    {
        //DEBUG ITC-1360-0358 ADD Server method refresh Pdline
        try
        {
            var selectLine = CmbPdLine1.InnerDropDownList.SelectedItem.Text;
            this.CmbPdLine1.refresh(this.CmbStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
            int i = 0;
            foreach (ListItem lst in this.CmbPdLine1.InnerDropDownList.Items)
            {
                if (selectLine == lst.Text)
                {
                    CmbPdLine1.InnerDropDownList.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
}
