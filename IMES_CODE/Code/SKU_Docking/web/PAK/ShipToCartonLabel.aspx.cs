
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class PAK_ShipToCartonLabel : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 11;
    private const int DEF_DN_ROWS = 12;
    IShipToCartonLabel iShipToCartonLabel =
                    ServiceAgent.getInstance().GetObjectByName<IShipToCartonLabel>(WebConstant.ShipToCartonLabelObject);
    public String UserId;
    public String Customer;
    public string Station;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!this.IsPostBack)
            {                
                IList<DNForUI> dnInfo = new List<DNForUI>();
                //IList<string> result = new List<string>();
               

                //dnInfo = iShipToCartonLabel.GetDNListByUI("");
                 bindDNTable(dnInfo, DEF_DN_ROWS);
                initLabel();                
                bindProTable(null, DEFAULT_ROWS);

                this.stationHF.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];                

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Station = Request["Station"];

                this.station1.Value = Station;
                this.editor1.Value = UserId;
                this.customer1.Value = Customer;

                //result = iShipToCartonLabel.WFStart("", Station, UserId, Customer);
                //this.hidSessionKey.Value = result[0].ToString();                
            }
            //ITC-1360-0595
            IList<string> path3 = iShipToCartonLabel.GetEditAddr("PLEditsImage");
            //IList<string> addr = iShipToCartonLabel.GetEditAddr("EditsFISAddr_zh");
            if (path3 != null && path3.Count > 0)
            {
                this.addr.Value = path3[0].ToString();
            }
            else
            {
                this.addr.Value = "";
            }
            //exe path
            IList<string> path2 = iShipToCartonLabel.GetEditAddr("PDFPrintPath");
            if (path2 != null && path2.Count > 0)
            {
                this.exepath.Value = path2[0].ToString();
            }
            else
            {
                this.exepath.Value = "";
            }
            //pdf path
            IList<string> path1 = iShipToCartonLabel.GetEditAddr("PLEditsPDF");
            if (path1 != null && path1.Count > 0)
            {
                this.pdfpath.Value = path1[0].ToString();
            }
            else
            {
                this.pdfpath.Value = "";
            }

            this.CmbPallet.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPallet_Selected);
            this.CmbPallet.InnerDropDownList.AutoPostBack = true;
        }
        catch (FisException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    protected void hidbtn_Clear(Object sender, EventArgs e)
    {
        IList<DNForUI> dnInfo = new List<DNForUI>();
        dnInfo = iShipToCartonLabel.GetDNListByUI("");
        bindDNTable(dnInfo, DEF_DN_ROWS);

        lblDNContent.Text = "";
        lblModelContent.Text = "";
        lblQtyContent.Text = "";
        lblPoNoContent.Text = "";
        lblPartNoContent.Text = "";
        //this.updatePanel2.Update();
        bindProTable(null, DEFAULT_ROWS);
        lblQty2Content.Text = "";
        lblScanQtyContent.Text = "";
        //updatePanel1.Update();
        //updatePanel3.Update();
        this.CmbPallet.InnerDropDownList.Items.Clear();
        this.CmbPallet.InnerDropDownList.Items.Add(string.Empty);
        this.CmbPallet.setSelected(0);
        endWaitingCoverDiv();
    }

    private void cmbPallet_Selected(object sender, System.EventArgs e)
    {
        if (this.CmbPallet.InnerDropDownList.SelectedValue != "")
        {
            IList<ProductModel> proList = new List<ProductModel>();
            int palletQty = 0;
            int scanQty = 0;
            string selectedPallet = this.CmbPallet.InnerDropDownList.SelectedValue;

            //this.CmbPallet.InnerDropDownList.SelectedIndex = 2;

            proList = iShipToCartonLabel.GetProductByPallet(selectedPallet);                        
            palletQty = iShipToCartonLabel.GetQtyByPallet(selectedPallet);
            scanQty = iShipToCartonLabel.GetScanQtyByPallet(selectedPallet);

            bindProTable(proList, DEFAULT_ROWS);
            lblQty2Content.Text = palletQty.ToString();
            lblScanQtyContent.Text = scanQty.ToString();
            updatePanel1.Update();
            updatePanel3.Update();            
        }
        else
        {
            bindProTable(null, DEFAULT_ROWS);
            lblQty2Content.Text = "";
            lblScanQtyContent.Text = "";

            updatePanel1.Update();
            updatePanel3.Update();
        }
    }

    protected void hidbtn_Close(Object sender, EventArgs e)
    {
        //string key = this.hidSessionKey.Value.Trim();

        //iShipToCartonLabel.WFCancel(key);
    }

    protected void hidbtn_DataEntry(Object sender, EventArgs e)
    {
        string inputData = this.hidData.Value.Trim();
        string key = this.hidSessionKey.Value.Trim();
        DNForUI info = new DNForUI();
        IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
        IList<ProductModel> proList = new List<ProductModel>();
        int i = 1;
        
        try
        {            
            string pallet = this.hidPallet.Value.ToString();
            string dnNo = this.hidDeliveryNo.Value.ToString();
            

            palletList = iShipToCartonLabel.GetPalletInfoByDN(dnNo);
            int PalletQty = 0;
            int scanQty = 0;
            this.CmbPallet.InnerDropDownList.Items.Clear();
            this.CmbPallet.InnerDropDownList.Items.Add(string.Empty);

            if (palletList != null && palletList.Count != 0)
            {                
                foreach (DeliveryPalletInfo temp in palletList)
                {
                    this.CmbPallet.InnerDropDownList.Items.Add(new ListItem(temp.palletNo + " " + temp.deliveryQty, temp.palletNo));
                    if (pallet != "")
                    {
                        if (pallet == temp.palletNo)
                        {
                            this.CmbPallet.setSelected(i);
                            proList = iShipToCartonLabel.GetProductByPallet(temp.palletNo);
                            bindProTable(proList, DEFAULT_ROWS);
                            PalletQty = iShipToCartonLabel.GetQtyByPallet(temp.palletNo);
                            scanQty = iShipToCartonLabel.GetScanQtyByPallet(temp.palletNo);
                            lblQty2Content.Text = PalletQty.ToString();
                            lblScanQtyContent.Text = scanQty.ToString();
                            
                            i++;
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {                            
                            proList = iShipToCartonLabel.GetProductByPallet(temp.palletNo);
                            bindProTable(proList, DEFAULT_ROWS);
                            PalletQty = iShipToCartonLabel.GetQtyByPallet(temp.palletNo);
                            scanQty = iShipToCartonLabel.GetScanQtyByPallet(temp.palletNo);
                            lblQty2Content.Text = PalletQty.ToString();
                            lblScanQtyContent.Text = scanQty.ToString();
                            
                            this.CmbPallet.setSelected(1);
                            i++;
                        }
                    }
                }                
            }
            //inputFinish();
            endWaitingCoverDiv();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            inputFinish();            
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            inputFinish();            
        }        
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("showError1(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void showMyMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowError(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");       
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showMyMessage", scriptBuilder.ToString(), false);
    }


    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void inputFinish()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("inputFinish();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "inputFinish", scriptBuilder.ToString(), false);
    }
    

    protected void hidbtn_gdDoubleClick(Object sender, EventArgs e)
    {
        string selectedDN = this.hidDeliveryNo.Value.Trim();
        string qty = this.hidQty.Value.Trim();
        IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
        IList<ProductModel> proList = new List<ProductModel>();
        int i = 1;        

        lblDNContent.Text = selectedDN;
        lblModelContent.Text = this.hidModel.Value.Trim();
        lblQtyContent.Text = qty;
        lblPoNoContent.Text = this.hidPoNo.Value.Trim();
        lblPartNoContent.Text = this.hidPartNo.Value.Trim();

        //this.updatePanel2.Update();

        if (!String.IsNullOrEmpty(selectedDN))
        {
            palletList = iShipToCartonLabel.GetPalletInfoByDN(selectedDN);
            this.CmbPallet.InnerDropDownList.Items.Clear();
            this.CmbPallet.InnerDropDownList.Items.Add(string.Empty);

            foreach (DeliveryPalletInfo temp in palletList)
            {
                this.CmbPallet.InnerDropDownList.Items.Add(new ListItem(temp.palletNo + " " + temp.deliveryQty, temp.palletNo));
                if (i == 1)
                {
                    i++;
                    int PalletQty = 0;
                    int scanQty = 0;

                    proList = iShipToCartonLabel.GetProductByPallet(temp.palletNo);
                    bindProTable(proList, DEFAULT_ROWS);
                    PalletQty = iShipToCartonLabel.GetQtyByPallet(temp.palletNo);
                    scanQty = iShipToCartonLabel.GetScanQtyByPallet(temp.palletNo);
                    lblQty2Content.Text = PalletQty.ToString();
                    lblScanQtyContent.Text = scanQty.ToString();
                    //this.updatePanel3.Update();
                    this.CmbPallet.setSelected(1);
                }
            }
        }

        

        //endWaitingCoverDiv();

    }



    private void initLabel()
    {
        this.lblDN.Text = GetLocalResourceObject(Pre + "_lblDN").ToString();
        this.lblModel.Text = GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblPartNo.Text = GetLocalResourceObject(Pre + "_lblPartNo").ToString();
        this.lblPoNo.Text = GetLocalResourceObject(Pre + "_lblPoNo").ToString();
        this.lblQty.Text = GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblQty2.Text = GetLocalResourceObject(Pre + "_lblQty2").ToString();
        this.lblScanQty.Text = GetLocalResourceObject(Pre + "_lblScanQty").ToString();
        this.lblPalletNo.Text = GetLocalResourceObject(Pre + "_lblPalletNo").ToString();
        this.lblEntry.Text = GetLocalResourceObject(Pre + "_lblEntry").ToString();
      //  this.lblDNList.Text = GetLocalResourceObject(Pre + "_lblDNList").ToString();
        this.lblDNList.Text = "";
        
        this.lblPalletList.Text = GetLocalResourceObject(Pre + "_lblPalletList").ToString();
        //this.btnClear.InnerText = GetLocalResourceObject(Pre + "_btnClear").ToString();
        this.btnClear.Value = GetLocalResourceObject(Pre + "_btnClear").ToString();
        this.btnPrintSetting.Value = GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();        
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }
    protected void gd_RowDataBound_dn(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void bindProTable(IList<ProductModel> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colProdId").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCartonNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCPQ").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (ProductModel temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ProductID;
                //ITC-1360-1350
                dr[1] = temp.Model;
                dr[2] = temp.CustSN;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < defaultRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

        }
        gd.DataSource = dt;
        gd.DataBind();
    }

    private void bindDNTable(IList<DNForUI> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCustomer").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPoNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colShipDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDeliveryNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (DNForUI temp in list)
            {

                dr = dt.NewRow();

                string partNo = string.Empty;
                temp.DeliveryInfo.TryGetValue("PartNo", out partNo);
                dr[0] = temp.ModelName;
                dr[1] = partNo;
                dr[2] = temp.PoNo;
                dr[3] = temp.ShipDate.ToString();
                dr[4] = temp.DeliveryNo;
                dr[5] = temp.Qty;          
                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < defaultRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

        }
        gd_dn.DataSource = dt;
        gd_dn.DataBind();
    }   
}
