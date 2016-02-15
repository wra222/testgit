/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/18
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/18           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
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

public partial class PAK_CombinePoInCartonForRCTO : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;
    public String Pcode;
    public int deliveryNum = 0;

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

                Station = Request.QueryString["Station"];
                Pcode = Request.QueryString["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "82";
                }

                this.CmbPdLine1.Station = Station;
                this.CmbPdLine1.Customer = Master.userInfo.Customer;

                this.cmbDelivery.Model = "";
            }

            this.cmbDelivery.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbDelivery_Selected);
            this.cmbDelivery.InnerDropDownList.AutoPostBack = true;
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
        this.lblPCs.Text = this.GetLocalResourceObject(Pre + "_lblPCs").ToString();
        this.lblDelivery.Text = this.GetLocalResourceObject(Pre + "_lblDelivery").ToString();
        this.TitleDelivery.Text = this.GetLocalResourceObject(Pre + "_TitleDelivery").ToString();
        this.lblTotalQty.Text = this.GetLocalResourceObject(Pre + "_lblTotalQty").ToString();
        this.lblPackedQty.Text = this.GetLocalResourceObject(Pre + "_lblPackedQty").ToString();
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblScannedQty.Text = this.GetLocalResourceObject(Pre + "_lblScannedQty").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
    }
    

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colProductID").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colVendorCT").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCustSN").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }
   
    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================


        //================================= Add Code End ==================================
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
    public void btnGetDn_Click(object sender, System.EventArgs e)
    {
        try
        {
            string model = this.modelHidden.Value.Trim(); 
            if (string.IsNullOrEmpty(model))
            {
                this.cmbDelivery.clearContent();
                setFocus();
                return;
            }    
            
            
            this.cmbDelivery.refresh(model, "00");
            deliveryNum = this.cmbDelivery.deliveryNum;
            this.dnListHidden.Value = Convert.ToString(deliveryNum);

            String script = "<script language='javascript'> setDelivery(); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "setDelivery", script, false);
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

    public void btnResetDnInfo_Click(object sender, System.EventArgs e)
    {
        try
        {
            cmbDelivery_Selected(sender, e);
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
    private void cmbDelivery_Selected(object sender, System.EventArgs e)
    {
        try
        {
            ICombinePoInCartonForRCTO CombinePoInCarton = ServiceAgent.getInstance().GetObjectByName<ICombinePoInCartonForRCTO>(WebConstant.CombinePoInCartonObjectForRCTO);
            ArrayList retList = new ArrayList();
            string dnno = this.cmbDelivery.InnerDropDownList.SelectedValue;

            if (!string.IsNullOrEmpty(dnno.Trim()))
            {
                retList = CombinePoInCarton.getDeliveryInfo(dnno);

                this.dnQtyHidden.Value = Convert.ToString(retList[0]);
                this.dnPcsHidden.Value = Convert.ToString(retList[1]);
                this.dnPackedHidden.Value = Convert.ToString(retList[2]);
            }
            else
            {
                this.dnQtyHidden.Value = "0";
                this.dnPcsHidden.Value = "0";
                this.dnPackedHidden.Value = "0";
            }
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("<script language='javascript'>");
            scriptBuilder.AppendLine("ChangeDn();");
            scriptBuilder.AppendLine("</script>");
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "ChangeDn", scriptBuilder.ToString(), false);
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

}
