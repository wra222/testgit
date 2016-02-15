/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine COA and DN
 * CI-MES12-SPEC-PAK-Combine COA and DN.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
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
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;
public partial class DOCK_BTCooLabel : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICooLabel iCooLabel = ServiceAgent.getInstance().GetObjectByName<ICooLabel>(WebConstant.CooLabel);
    public String userId;
    public String customer;
    public String station;
    public String code;
    public String Login;
    public String AccountId;
    public String UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnGetProduct.ServerClick += new EventHandler(btnGetProduct_ServerClick);
        btnGetQty.ServerClick += new EventHandler(btnGetQty_ServerClick);
        btnGetQtyRefresh.ServerClick += new EventHandler(btnGetQtyRefresh_ServerClick);
        btnSave.ServerClick += new EventHandler(btnSave_ServerClick);
        btnRefresh.ServerClick += new EventHandler(btnRefresh_ServerClick);
        btnRefresh2.ServerClick += new EventHandler(btnRefresh2_ServerClick);
        userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;
        if (null == station || "" == station)
        {
            station = Request["Station"];
        }
        if (null == AccountId || "" == AccountId)
        {
            AccountId = Request["AccountId"];
        }
        if (null == Login || "" == Login)
        {
            Login = Request["Login"];
        }
        if (null == code || "" == code)
        {
            code = Request["PCode"];
        }
        if (null == UserName || "" == UserName)
        {
            UserName = Request["UserName"];
        }
        if (!this.IsPostBack)
        {
            customer = Master.userInfo.Customer;
            //writeToInfoMessage("Please check radio, and then input customer S/N !");
            this.drpDN.Attributes.Add("onchange", "drpOnChange()");
            InitLabel();
        }
    }
    private void InitLabel()
    {
        this.lbDN.Text = this.GetLocalResourceObject(Pre + "_lbDN").ToString();
        this.lbCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lbCustomerSN").ToString();
        this.lbProductID.Text = this.GetLocalResourceObject(Pre + "_lbProductID").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lbModel").ToString();
        this.chkNotDN.Text = this.GetLocalResourceObject(Pre + "_chkNotDN").ToString();
        this.lbPackQty.Text = this.GetLocalResourceObject(Pre + "_lbPackQty").ToString();
        this.lbProductInfo.Text = this.GetLocalResourceObject(Pre + "_lbProductInfo").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.lbJapan.Text = this.GetLocalResourceObject(Pre + "_lbJapan").ToString();
    }
    
    
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToInfoMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToInfoMessage", scriptBuilder.ToString(), false);
    }

    
    private void ReDisplay(string display)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDisplay(\"" + display + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDisplay", scriptBuilder.ToString(), false);
    }
   
    private void ReReset(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getReset(\"" + msg + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReReset", scriptBuilder.ToString(), false);
    }

    private void ReGetProductFail(string productWrong)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getProductFail(\"" + productWrong + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetProductFail", scriptBuilder.ToString(), false);
    }


   

    
   
    private void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        string sn = HSN.Value;
        string prod = HPROD.Value;
        string QCIS = "";
        try
        {
            QCIS = iCooLabel.CombineDN(userId, station, customer, sn, prod, HDN.Value, HISCHECK.Value);
            if (QCIS.IndexOf("true") != -1)
            {
                HQCView.Value = "true";
            }
            ResetAndRefresh("true");
        }
        catch (FisException ee)
        {
            string temp = ee.mErrmsg;
            if (ee.mErrmsg.IndexOf("[]") != -1)
            { 
                temp = ee.mErrmsg.Substring(0,ee.mErrmsg.IndexOf("[]"));
                if (sn != "")
                {
                    temp = "[" + sn + "]";
                }
                else if (prod != "")
                {
                    temp = "[" + prod + "]";
                }
                temp = ee.mErrmsg.Replace("[]", temp);
            }
            writeToAlertMessage(temp);
            ResetAndRefresh("false");
            return;
        }
        catch (Exception ex)
        {
            string temp = ex.Message;
            if (ex.Message.IndexOf("[]") != -1)
            {
                temp = ex.Message.Substring(0, ex.Message.IndexOf("[]"));
                if (sn != "")
                {
                    temp = "[" + sn + "]";
                }
                else if (prod != "")
                {
                    temp = "[" + prod + "]";
                }
                temp = ex.Message.Replace("[]", temp);
            }
            writeToAlertMessage(temp);
            ResetAndRefresh("false");
            return;
        }
    }
    private void Refresh2()
    {
        try
        {
            this.drpDN.Items.Clear();
            this.drpDN.Items.Add(string.Empty);
            IList<DNForUI> relist = iCooLabel.GetDeliveryListByModel(HMODE.Value, "00");
            int first = 1;
            foreach (DNForUI temp in relist)
            {
                if (first == 1)
                {
                    HDN.Value = temp.DeliveryNo;
                    ReGetQTY(iCooLabel.GetQTY(HDN.Value).ToString());
                    first = 0;
                }
                ListItem item = null;
                item = new ListItem(temp.DeliveryNo + "_" + temp.ShipDate.ToString("yyyy/MM/dd") + "_" + temp.Qty, temp.DeliveryNo);
                this.drpDN.Items.Add(item);
            }
            if (relist.Count > 0)
            {
                drpDN.Items[1].Selected = true;
            }
            this.UpdatePanel1.Update();
        }
         catch (FisException ee)
         {
             writeToAlertMessage(ee.mErrmsg);
             return;
         }
         catch (Exception ex)
         {
             writeToAlertMessage(ex.Message);
             return;
         }
    }

    private void btnGetQtyRefresh_ServerClick(object sender, System.EventArgs e)
    { 
        try
        {
            int ret = 0;
            if (HDN.Value != "")
            {
                ret = iCooLabel.GetQTY(HDN.Value);
                if (ret == -99999999)
                {
                    ReGetQTY("");
                }
                else if (ret < 0)
                {
                    //Refresh2();
                }
                else
                {
                    ReGetQTY(ret.ToString());
                }
            }
            S_CooLabel retProduct = iCooLabel.GetProductOnly(userId, station, customer, HSN.Value, HPROD.Value);
            if (HSN.Value == "")
            {
                HSN.Value = retProduct.CustomerSN;
            }
            if (HPROD.Value == "")
            {
                HPROD.Value = retProduct.ProductID;
            }
            HJAPAN.Value = retProduct.IsJapan;
            HMO.Value = retProduct.Mo;
            HTOTAL.Value = retProduct.Total;
            HPASS.Value = retProduct.Pass;
            if (HISCHECK.Value == "true")
            {
                HMODE.Value = retProduct.Model;
                ReGetProductFresh("");
                return;
            }
            if (retProduct.Model != HMODE.Value)
            {
                HMODE.Value = retProduct.Model;
            }
            if (ret < 0)
            {
                ReGetProductFresh("true");
            }
            else
            {
                ReGetProductFresh("");
            }
           
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            HSN.Value = "";
            HPROD.Value = "";
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            HSN.Value = "";
            HPROD.Value = "";
            return;
        }
       
    } 
    private void btnGetQty_ServerClick(object sender, System.EventArgs e)
    { 
        try
        {
            int ret = iCooLabel.GetQTY(HDN.Value);
            if (ret == -99999999)
            {
                ReGetQTY("");
            }
            else if (ret < 0)
            {
                ReGetQTY((0 - ret).ToString());
            }
            else
            {
                ReGetQTY(ret.ToString());
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    private void btnRefresh_ServerClick(object sender, System.EventArgs e)
    {
        Refresh();
    }
    private void btnRefresh2_ServerClick(object sender, System.EventArgs e)
    {
        Refresh2();
    }
    
    private void Refresh()
    {
        try
        {
            bool ret = iCooLabel.ISDNChange(HDN.Value);
            if (ret == false)
            {
                return;
            }
            
            this.drpDN.Items.Clear();
            this.drpDN.Items.Add(string.Empty);
            if (HMODE.Value == "")
            {
                this.UpdatePanel1.Update();
                return;
            }
            IList<DNForUI> relist = iCooLabel.GetDeliveryListByModel(HMODE.Value, "00");
            int first = 1;
            foreach (DNForUI temp in relist)
            {
                if (first == 1)
                {
                    HDN.Value = temp.DeliveryNo;
                    ReGetQTY(iCooLabel.GetQTY(HDN.Value).ToString());
                    first = 0;
                }
                ListItem item = null;
                item = new ListItem(temp.DeliveryNo + "_" + temp.ShipDate.ToString("yyyy/MM/dd") + "_" + temp.Qty, temp.DeliveryNo);
                this.drpDN.Items.Add(item);
            }
            if (relist.Count > 0)
            {
                drpDN.Items[1].Selected = true;
            }
            this.UpdatePanel1.Update();
        }
         catch (FisException ee)
         {
             writeToAlertMessage(ee.mErrmsg);
             return;
         }
         catch (Exception ex)
         {
             writeToAlertMessage(ex.Message);
             return;
         }
    }

   
    private void btnGetProduct_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            S_CooLabel ret = iCooLabel.GetProduct(userId, station, customer, HSN.Value, HPROD.Value);
            if (ret.IsCombineDN != "")
            {
                ReDNBind();
                return;
            }
            if (HSN.Value == "")
            {
                HSN.Value = ret.CustomerSN;
            }
            if (HPROD.Value == "")
            {
                HPROD.Value = ret.ProductID;
            }
            HJAPAN.Value = ret.IsJapan;
            HMO.Value = ret.Mo;
            HTOTAL.Value = ret.Total;
            HPASS.Value = ret.Pass;
            if (HISCHECK.Value == "true")
            {
                HMODE.Value = ret.Model;
                ReGetProduct();
                return;
            }
            if (ret.Model != HMODE.Value)
            {
                HMODE.Value = ret.Model;
                HDN.Value = "";
                Refresh();
            }
            ReGetProduct();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ResetAll();
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ResetAll();
        }
    }

    private void ReGetProduct()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getProduct();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetProduct", scriptBuilder.ToString(), false);
    }
    private void ReGetProductFresh(string clear)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getProductFresh(\"" + clear + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetProductFresh", scriptBuilder.ToString(), false);
    }
    private void ReGetQTY(string qty)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQTY(\"" + qty + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetQTY", scriptBuilder.ToString(), false);
    }
    private void ResetAll()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAll();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAll", scriptBuilder.ToString(), false);
    }
    private void ResetValue()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetValue();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetValue", scriptBuilder.ToString(), false);
    }
    private void ResetAndRefresh(string display)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAndRefresh(\"" + display + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAndRefresh", scriptBuilder.ToString(), false);
    }
    private void ReDNBind()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDNBind();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDNBind", scriptBuilder.ToString(), false);
    }
}
