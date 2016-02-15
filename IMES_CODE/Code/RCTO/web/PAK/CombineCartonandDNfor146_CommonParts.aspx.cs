
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

public partial class PAK_CombineCartonandDNfor146_CommonParts : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private ICombineCartonandDNfor146_CommonParts iCombineCartonandDNfor146_CommonParts = ServiceAgent.getInstance().GetObjectByName<ICombineCartonandDNfor146_CommonParts>(WebConstant.CombineCartonandDNfor146_CommonPartsObject);
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

            if (!this.IsPostBack)
            {
                initLabel();
                
                Station = Request.QueryString["Station"];
                Pcode = Request.QueryString["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "81";
                }
                
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "PAK";
                this.cmbPdLine.Customer = Customer;
                initModel();
                
            }
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            this.cmbModel.SelectedIndexChanged += new EventHandler(cmbModel_Selected);
            this.cmbModel.AutoPostBack = true;
            this.cmbDelivery.SelectedIndexChanged += new EventHandler(cmbDelivery_Selected);
            this.cmbDelivery.AutoPostBack = true;
            
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
        this.lblDelivery.Text = this.GetLocalResourceObject(Pre + "_lblDelivery").ToString();
        this.TitleDelivery.Text = this.GetLocalResourceObject(Pre + "_TitleDelivery").ToString();
        this.lblTotalQty.Text = this.GetLocalResourceObject(Pre + "_lblTotalQty").ToString();
        this.lblPackedQty.Text = this.GetLocalResourceObject(Pre + "_lblPackedQty").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }

    private void initModel()
    {
        this.cmbModel.Items.Clear();
        this.cmbModel.Items.Add(string.Empty);
        IList<string> list = iCombineCartonandDNfor146_CommonParts.GetModelList();
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items.ToString();
            selectListItem.Value = items.ToString();
            this.cmbModel.Items.Add(selectListItem);

        }
    }


    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
    public void btnReflishSelect_Click(object sender, System.EventArgs e)
    {
        try
        {
            initModel();
            this.cmbDelivery.Items.Clear();
            this.cmbDelivery.Items.Add(string.Empty);

            //string model = this.modelHidden.Value.Trim();
            //this.cmbDelivery.refresh(model, "00");

            //deliveryNum = this.cmbDelivery.deliveryNum;
            //this.dnListHidden.Value = Convert.ToString(deliveryNum);

            //String script = "<script language='javascript'> setDelivery(); </script>";
            //ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "setDelivery", script, false);
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

    private void cmbModel_Selected(object sender, System.EventArgs e)
    {
        try
        {
            ArrayList retList = new ArrayList();
            string model = this.cmbModel.SelectedItem.Text;
            //string dnQty = this.cmbModel.SelectedItem.Value;
            this.cmbDelivery.Items.Clear();
            this.cmbDelivery.Items.Add(string.Empty);
            retList = iCombineCartonandDNfor146_CommonParts.GetDeliveryList(model);
            foreach (object items in retList)
            {
                string[] item = (string[])items; 
                ListItem selectListItem = new ListItem();
                selectListItem.Text = item[0].ToString();
                selectListItem.Value = item[0].ToString()+"-"+ item[1].ToString();
                this.cmbDelivery.Items.Add(selectListItem);
            }
            if (retList.Count > 0)
            {
                this.cmbDelivery.SelectedIndex = 1;
                cmbDelivery_Selected(sender,e);
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

    private void cmbDelivery_Selected(object sender, System.EventArgs e)
    {
        try
        {
            IList<string> retList = new List<string>();
            string dnno = this.cmbDelivery.SelectedItem.Text;
            string remainQty = "''";
            if (!string.IsNullOrEmpty(dnno))
            {
                int dnQty = int.Parse(this.cmbDelivery.SelectedItem.Value.Split('-')[1]);
                //      int dnQty =  int.Parse(this.cmbDelivery.SelectedItem.Value);
                int qty = int.Parse(iCombineCartonandDNfor146_CommonParts.GetRemainQty(dnno));
                int tmpremainQty = dnQty - qty;
                remainQty = tmpremainQty.ToString();
            }
            else
            { dnno = "''"; }
       

       //     String script = "<script language='javascript'>  setpagevalue(" + this.cmbDelivery.SelectedItem.Value + "," + remainQty + "); </script>";
            String script = "<script language='javascript'>  setpagevalue(" + dnno + "," + remainQty + "); </script>";
         
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "setpagevalue", script, false);
            //setpagevalue
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

