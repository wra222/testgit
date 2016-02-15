using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.Linq;



public partial class FA_TravelCardPrintProductPlan : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMO iMO = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);
    ITravelCardPrintProductPlan iTravelCardPrint = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrintProductPlan>(WebConstant.TravelCardPrintProductPlanObject);
    ITravelCardPrintExcel iTravelCardPrint2 = ServiceAgent.getInstance().GetObjectByName<ITravelCardPrintExcel>(WebConstant.TravelCardPrintExcelObject);
    public string userId;
    public string customer;
    public string today;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Pre = this.GetLocalResourceObject("language").ToString();
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            //this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
            //this.cmbModel.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);
            //this.cmbMO.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMO_Selected);
            this.cmbexception.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbex_Selected);
            
            InitLabel();

            if (!Page.IsPostBack)
            {
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.pCode.Value = Request["PCode"];
                this.station1.Value = Request["Station"];
                //Fortest
                //this.station1.Value = "13";
                //Fortest
                this.editor1.Value = userId;
                this.customer1.Value = customer;

                this.cmbPdLine.Customer = customer;
                this.cmbPdLine.Stage = "FA";
                this.cmbFamily.Items.Add(string.Empty);
                this.cmbModel.Items.Add(string.Empty);
                this.cmbMO.Items.Add(string.Empty);
                //this.cmbFamily.Service = "013";
                //this.cmbFamily.Customer = customer;
                
                //this.cmbModel.Service = "013";
                //this.cmbMO.Service = "013";
            }
            today = DateTime.Now.ToString("yyyy-MM-dd");
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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //if (this.cmbPdLine.InnerDropDownList.SelectedValue == "")
            //{
                this.cmbFamily.Items.Clear();
                this.cmbFamily.Items.Add(string.Empty);
                this.UpdatePanel6.Update();
                this.cmbModel.Items.Clear();
                this.cmbModel.Items.Add(string.Empty);
                this.UpdatePanel7.Update();
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
                this.lbShowPlanQty.Text = "";
                this.UpdatePanel1.Update();
                this.lbShowReQty.Text = "";
                this.UpdatePanel2.Update();
                this.lbShowPrintQty.Text = "";
                this.UpdatePanel3.Update();
            //}
            //else
            //{
                string line = this.cmbPdLine.InnerDropDownList.SelectedValue;
                //line = line.Substring(0, 1);
                IList<ProductPlanFamily> ListFamily = null;
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);

                //DateTime ShipDate2 = Convert.ToDateTime(this.dCalShipdate.Value);
                ListFamily = iTravelCardPrint.GetProductPlanFamily(line, ShipDate);
                initControlFamily(ListFamily);
            //}
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

    
    private void initControlFamily(IList<ProductPlanFamily> ListFamily)
    {
        ListItem item = null;
        this.cmbFamily.Items.Clear();
        this.cmbFamily.Items.Add(string.Empty);
        if (ListFamily != null)
        {
            foreach (ProductPlanFamily temp in ListFamily)
            {
                item = new ListItem(temp.Family.Trim() ,temp.Family.Trim());
                this.cmbFamily.Items.Add(item);
            }
        }
        this.UpdatePanel6.Update();
    }

    public void btnDateChange_Click(object sender, System.EventArgs e)
    {
        try
        {
            //if (this.cmbPdLine.InnerDropDownList.SelectedValue == "")
            //{
                this.cmbFamily.Items.Clear();
                this.cmbFamily.Items.Add(string.Empty);
                this.UpdatePanel6.Update();
                this.cmbModel.Items.Clear();
                this.cmbModel.Items.Add(string.Empty);
                this.UpdatePanel7.Update();
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
                this.lbShowPlanQty.Text = "";
                this.UpdatePanel1.Update();
                this.lbShowReQty.Text = "";
                this.UpdatePanel2.Update();
                this.lbShowPrintQty.Text = "";
                this.UpdatePanel3.Update();
            //}
            //else
            //{
                string line = this.cmbPdLine.InnerDropDownList.SelectedValue;
                //line = line.Substring(0, 1);
                IList<ProductPlanFamily> ListFamily = null;
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);
                
                ListFamily = iTravelCardPrint.GetProductPlanFamily(line, ShipDate);
                initControlFamily(ListFamily);
            //}
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
    /// </summary>
    private void cmbex_Selected(object sender, System.EventArgs e)
    {
        try
        {
            string value = this.cmbexception.InnerDropDownList.SelectedValue.ToString();
            if (value == "OA" || value == "OFT" || value == "Other" || value == "PE")
            {
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

    /// <summary>
    /// </summary>

    protected void cmbFamily_Selected(object sender, EventArgs e)
    {
        try
        {
            //if (this.cmbFamily.SelectedValue == "")
            //{
                this.cmbModel.Items.Clear();
                this.cmbModel.Items.Add(string.Empty);
                this.UpdatePanel7.Update();
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
                this.lbShowPlanQty.Text = "";
                this.UpdatePanel1.Update();
                this.lbShowReQty.Text = "";
                this.UpdatePanel2.Update();
                this.lbShowPrintQty.Text = "";
                this.UpdatePanel3.Update();
            //}
            //else
            //{
                string line = this.cmbPdLine.InnerDropDownList.SelectedValue;
                line = line.Substring(0, 1);
                IList<ProductPlanInfo> ListModel = null;
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);
                string Family = this.cmbFamily.SelectedValue.ToString();

                var modelList = iTravelCardPrint.GetProductPlanModel(line, ShipDate, Family);
                var modes = (from p in modelList
                             orderby p.ID ascending
                             select p).ToList();
                ListModel = modes;
                //ListModel = iTravelCardPrint.GetProductPlanModel(line, ShipDate, Family);

                initControlModel(ListModel,sender,e);
            //}
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

    private void initControlModel(IList<ProductPlanInfo> ListFamily, object sender, System.EventArgs e)
    {
        ListItem item = null;
        this.cmbModel.Items.Clear();
        this.cmbModel.Items.Add(string.Empty);
        if (ListFamily != null)
        {
            foreach (ProductPlanInfo temp in ListFamily)
            {
                item = new ListItem(temp.Model.Trim(), temp.ID.ToString());
                this.cmbModel.Items.Add(item);
            }
        }
        this.UpdatePanel7.Update();
        if (this.cmbModel.Items.Count > 1)
        {
            this.cmbModel.SelectedIndex = 1;
            cmbModel_Selected(sender, e);
        }
    }

    /// <summary>
    /// </summary>
    protected void cmbModel_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMO.Service = "013";
            //this.cmbModel.Service = "013";
            this.cmbMO.Items.Clear();
            this.cmbMO.Items.Add(string.Empty);
            this.UpdatePanel12.Update();
            this.lbShowPlanQty.Text = "";
            this.UpdatePanel1.Update();
            this.lbShowReQty.Text = "";
            this.UpdatePanel2.Update();
            this.lbShowPrintQty.Text = "";
            this.UpdatePanel3.Update();

            this.cmbPilotMo.Items.Clear();
            this.cmbPilotMo.Items.Add(string.Empty);
            this.UpdatePanel14.Update();
            this.lblMoTypeContent.Text = "";
            this.UpdatePanel15.Update();
            this.lblMoQtyContent.Text = "";
            this.UpdatePanel16.Update();
            this.lblRemainQtyContent.Text = "";
            this.UpdatePanel17.Update();

            if (this.cmbModel.SelectedValue != "")
            {
                this.ModelSelect.Value = this.cmbModel.SelectedItem.ToString();
                CheckedBSamModel();
                refreshMOContent();
                refreshPilotMOContent();
                if (this.cmbMO.Items.Count > 1)
                {
                    this.cmbMO.SelectedIndex = 1;
                    cmbMO_Selected(sender, e);
                }
                else
                {
                    this.cmbMO.Items.Clear();
                    this.cmbMO.Items.Add(string.Empty);
                    this.UpdatePanel12.Update();
                    this.lbShowPlanQty.Text = "";
                    this.lbShowReQty.Text = "";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel2.Update();
                    this.lbShowPrintQty.Text = "";
                    this.UpdatePanel3.Update();
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

    private void CheckedBSamModel()
    {
        string Model = this.cmbModel.SelectedItem.ToString();
        if (iTravelCardPrint.CheckedBSamModel(Model))
        {
            CheckedBSamModelData();
        }
    }

    private void CheckedBSamModelData()
    {
        string Model = this.cmbModel.SelectedItem.ToString();
        string Name = "CSKU";

        try
        {
            if (!iTravelCardPrint.CheckedC_SKU(Model, Name))
            {
                this.cmbFamily.Items.Clear();
                this.cmbFamily.Items.Add(string.Empty);
                this.UpdatePanel6.Update();
                this.cmbModel.Items.Clear();
                this.cmbModel.Items.Add(string.Empty);
                this.UpdatePanel7.Update();
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
                this.lbShowPlanQty.Text = "";
                this.UpdatePanel1.Update();
                this.lbShowReQty.Text = "";
                this.UpdatePanel2.Update();
                this.lbShowPrintQty.Text = "";
                this.UpdatePanel3.Update();
                writeToAlertMessage("Bsam Model: " + Model + " 未設定CSKU，請聯繫IE Maintain...");
            }
            else if (!iTravelCardPrint.CheckedBSamModelAndC_SKU(Model, Name))
            {
                
                this.cmbFamily.Items.Clear();
                this.cmbFamily.Items.Add(string.Empty);
                this.UpdatePanel6.Update();
                this.cmbModel.Items.Clear();
                this.cmbModel.Items.Add(string.Empty);
                this.UpdatePanel7.Update();
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
                this.lbShowPlanQty.Text = "";
                this.UpdatePanel1.Update();
                this.lbShowReQty.Text = "";
                this.UpdatePanel2.Update();
                this.lbShowPrintQty.Text = "";
                this.UpdatePanel3.Update();
                writeToAlertMessage("Bsam Model之CSKU資料錯誤，請聯繫相關人員...");
            }
        }
        catch(Exception)
        {
           
        }
    }

    private void refreshMOContent()
    {
        string ID = this.cmbModel.SelectedValue;

        MOPlanInfo ListMo = null;
        ListMo = iTravelCardPrint.GetPlanMO(Convert.ToInt32(ID));
        if (ListMo.MO.ToString() != null || ListMo.MO.ToString() != "")
        {
            initControlMo(ListMo);
        }
        else
        {
            this.cmbMO.Items.Clear();
            this.cmbMO.Items.Add(string.Empty);
            this.UpdatePanel12.Update();
            this.lbShowPlanQty.Text = "";
            this.UpdatePanel1.Update();
            this.lbShowReQty.Text = "";
            this.UpdatePanel2.Update();
            this.lbShowPrintQty.Text = "";
            this.UpdatePanel3.Update();
        }
        
    }

    private void initControlMo(MOPlanInfo ID)
    {
        ListItem item = null;
        this.cmbMO.Items.Clear();
        this.cmbMO.Items.Add(string.Empty);
        if (ID != null)
        {
            item = new ListItem(ID.MO.Trim(), ID.MO.Trim());
            this.cmbMO.Items.Add(item);
        }
        this.UpdatePanel12.Update();
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbMO_Selected(object sender, System.EventArgs e)
    {
        int remainQty;
        if (this.cmbMO.SelectedValue == "")
        {
            this.lbShowPlanQty.Text = "";
            this.lbShowReQty.Text = "";
            this.lbShowPrintQty.Text = "";
            
        }
        else
        {
            try
            {
                string ID = this.cmbModel.SelectedValue;
                MOPlanInfo moInfo = iTravelCardPrint.GetPlanMO(Convert.ToInt32(ID));

                this.lbShowPlanQty.Text = moInfo.PlanQty.ToString();
                this.lbShowPrintQty.Text = moInfo.Qty.ToString();
                int Qty = Convert.ToInt32(moInfo.Qty.ToString()) - Convert.ToInt32(moInfo.Print_Qty.ToString());
                remainQty = Qty;
                this.lbShowReQty.Text = remainQty.ToString();
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
        this.UpdatePanel1.Update();
        this.UpdatePanel2.Update();
        this.UpdatePanel3.Update();
    }

    private void refreshPilotMOContent()
    {
        string model = this.cmbModel.SelectedItem.Text;

        PilotMoInfo pilotMo = new PilotMoInfo();
        IList<string> stateList = new List<string>();
        pilotMo.model = model;
        pilotMo.stage = "FA";
        stateList.Add(PilotMoCombinedStateEnum.Empty.ToString());
        stateList.Add(PilotMoCombinedStateEnum.Partial.ToString());
        IList<PilotMoInfo> pilotMoList = iTravelCardPrint.GetPilotMo(pilotMo, stateList);
        if (pilotMoList.Count > 0)
        {
            initControlPilotMo(pilotMoList);
        }
        else
        {
            //this.cmbMO.Items.Clear();
            //this.cmbMO.Items.Add(string.Empty);
            //this.UpdatePanel12.Update();
            this.lbShowPlanQty.Text = "";
            this.UpdatePanel1.Update();
            this.lbShowReQty.Text = "";
            this.UpdatePanel2.Update();
            this.lbShowPrintQty.Text = "";
            this.UpdatePanel3.Update();
        }

    }

    private void initControlPilotMo(IList<PilotMoInfo> pilotlist)
    {

        this.cmbPilotMo.Items.Clear();
        this.cmbPilotMo.Items.Add(string.Empty);
        foreach (PilotMoInfo items in pilotlist)
        {
            ListItem item = new ListItem();
            item.Text = items.mo + " - " + items.causeDescr;
            item.Value = items.mo;
            this.cmbPilotMo.Items.Add(item);
        }
        this.UpdatePanel14.Update();
    }

    protected void cmbPilotMO_Selected(object sender, System.EventArgs e)
    {
        int remainQty;
        if (this.cmbPilotMo.SelectedValue == "")
        {
            this.lbShowPlanQty.Text = "";
            this.lbShowReQty.Text = "";
            this.lbShowPrintQty.Text = "";
        }
        else
        {
            try
            {
                string mo = this.cmbPilotMo.SelectedValue;
                PilotMoInfo pilotMo = new PilotMoInfo();
                IList<string> stateList = new List<string>();
                pilotMo.mo = mo;
                stateList.Add(PilotMoCombinedStateEnum.Empty.ToString());
                stateList.Add(PilotMoCombinedStateEnum.Partial.ToString());
                IList<PilotMoInfo> pilotmoInfo = iTravelCardPrint.GetPilotMo(pilotMo, stateList);

                this.lblMoTypeContent.Text = pilotmoInfo[0].moType.ToString().Trim();
                this.lblMoQtyContent.Text = pilotmoInfo[0].qty.ToString().Trim();
                int Qty = pilotmoInfo[0].qty - pilotmoInfo[0].combinedQty;
                remainQty = Qty;
                this.lblRemainQtyContent.Text = remainQty.ToString();
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
        this.UpdatePanel15.Update();
        this.UpdatePanel16.Update();
        this.UpdatePanel17.Update();
    }

    /// <summary>
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbMO.Text = this.GetLocalResourceObject(Pre + "_lblMO").ToString();
        this.lbMoQty.Text = this.GetLocalResourceObject(Pre + "_lblMOQty").ToString();
        this.lbPrintQty.Text = this.GetLocalResourceObject(Pre + "_lblPrintQty").ToString();
        this.lbReQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        this.lbPeriod.Text = this.GetLocalResourceObject(Pre + "_lblPeriod").ToString();
        this.lbThisMonth.Text = this.GetLocalResourceObject(Pre + "_lblTMonth").ToString();
        this.lbNextMonth.Text = this.GetLocalResourceObject(Pre + "_lblNMonth").ToString();
        this.lbQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblShipdate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
        this.lbProductId.Text = this.GetLocalResourceObject(Pre + "_lblProductId").ToString();
        this.lbStart.Text = this.GetLocalResourceObject(Pre + "_lblProductStartId").ToString();
        this.lbEnd.Text = this.GetLocalResourceObject(Pre + "_lblProductEndId").ToString();
        this.lbBomRemark.Text = this.GetLocalResourceObject(Pre + "_lblBomRemark").ToString();
        this.lbRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lbException.Text = this.GetLocalResourceObject(Pre + "_lblException").ToString();
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
    }

    /// <summary>
    /// </summary>
    private void cmbModel_Selected1(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMO.Service = "013";
            //this.cmbModel.Service = "013";

            //if (this.cmbModel.SelectedValue == "")
            //{
                this.cmbMO.Items.Clear();
                this.cmbMO.Items.Add(string.Empty);
                this.UpdatePanel12.Update();
            //}
            //else
            //{
                refreshMOContent();
            //}

            this.lbShowPlanQty.Text = "";
            this.lbShowReQty.Text = "";
            this.UpdatePanel1.Update();
            this.UpdatePanel2.Update();
            this.lbShowPrintQty.Text = "";
            this.UpdatePanel3.Update();
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
    /// µ÷ÓÃweb service´òÓ¡½Ó¿Ú³É¹¦ºóÐèÒªresetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            cmbFamily_Selected(sender, e);
            showSuccessInfo1();
            //if (this.txtQty.Value == this.lbShowReQty.Text)
            //{
            //    //MB Label ÒÑ¾­È«²¿ÁÐÓ¡Íê±ÏÊ±£¬ÐèÒª½«¸ÃMO ´ÓMO ÏÂÀ­ÁÐ±íÖÐÉ¾³ý£¬µ±Ç°Ñ¡ÔñMO Îª¿Õ£¬Çå¿ÕMO ÊýÁ¿£¬Ê£ÓàÊýÁ¿£»ÆäËûÄÚÈÝ±£³Ö²»±ä
            //    cmbModel_Selected1(sender, e);
            //}
            //else
            //{
            //    cmbMO_Selected(sender, e);
            //}

            //this.cmbModel.refreshDropContentForTravelCardExcel(this.cmbPdLine.InnerDropDownList.SelectedValue, this.cmbFamily.InnerDropDownList.SelectedValue);
            //更新mo
            
            //if (this.cmbModel.Items.Count > 1)
            //{
            //    //this.cmbModel.SelectedIndex = 1;
            //    cmbModel_Selected(sender, e);
            //}
            //else
            //{
            //    this.cmbMO.Items.Clear();
            //    this.cmbMO.Items.Add(string.Empty);
            //    this.UpdatePanel12.Update();
            //    this.lbShowPlanQty.Text = "";
            //    this.lbShowReQty.Text = "";
            //    this.UpdatePanel1.Update();
            //    this.UpdatePanel2.Update();
            //    this.lbShowPrintQty.Text = "";
            //    this.UpdatePanel3.Update();
            //}
            


        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
    }

    /// <summary>
    /// resetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            showSuccessInfo2();
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
    /// Êä³ö´íÎóÐÅÏ¢
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void showSuccessInfo1()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo1", script, false);
    }

    private void showSuccessInfo2()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test1();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo2", script, false);
    }

}
