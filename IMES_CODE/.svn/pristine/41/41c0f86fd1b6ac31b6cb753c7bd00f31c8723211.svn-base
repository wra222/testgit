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
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_PalletType : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private IConstValueType iConstValueType = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueType>(WebConstant.ConstValueTypeObject);
    private IPalletTypeforICC iPalletTypeforICC = ServiceAgent.getInstance().GetMaintainObjectByName<IPalletTypeforICC>(WebConstant.IPalletTypeforICC);
    private string gFamily = string.Empty;
    private string gMbCode = string.Empty;
    private string gPCB = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
   
            if (!this.IsPostBack)
            {
                initLabel();
                initSelect();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "", true);
    }

    private void initLabel()
    {
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblPlletTypeforICC.Text = this.GetLocalResourceObject(Pre + "_lblPlletTypeforICC").ToString();
    }

    private void initSelect()
    {
        initShipWaySelect();
        initRegIdSelect();
        initShipWaySelect2();
        initRegId_Carrier("RegId");
        initPalletTypeSelect();
		initOceanTypeSelect();
        initPalletCodeSelect();
        //initInPltWeightSelect();
    }

    private void initRegIdSelect()
    {
        IList<string> lstRegId = iPalletTypeforICC.GetRegId();
        this.cmbRegId.Items.Clear();
        if (lstRegId.Count != 0)
        {
            this.cmbRegId.Items.Add("ALL");
            foreach (string item in lstRegId)
            {
                this.cmbRegId.Items.Add(item);
            }
            this.cmbRegId.SelectedIndex = 0;
        }
    }

    private void initShipWaySelect()
    {
        IList<string> lstShipWay = iPalletTypeforICC.GetShipWay();
        this.cmbShipWay.Items.Clear();
        if (lstShipWay.Count != 0)
        {
            this.cmbShipWay.Items.Add("ALL");
            foreach (string item in lstShipWay)
            {
                this.cmbShipWay.Items.Add(item);
            }
            this.cmbShipWay.SelectedIndex = 0;
        }
    }

    private void initShipWaySelect2()
    {
        IList<ConstValueTypeInfo> lstRegId = iConstValueType.GetConstValueTypeList("ShipWay");
        this.cmbShipWay2.Items.Clear();
        if (lstRegId.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstRegId)
            {
                string value = item.value.ToString();
                this.cmbShipWay2.Items.Add(value);
            }
            this.cmbShipWay2.SelectedIndex = 0;
            this.updatePanel4.Update();
        }
    }

    private void initRegId_Carrier(string Type)
    {
        IList<ConstValueTypeInfo> lstRegId = iConstValueType.GetConstValueTypeList(Type);
        this.cmbRegId_Carrier.Items.Clear();
        if (lstRegId.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstRegId)
            {
                string value = item.value.ToString();
                this.cmbRegId_Carrier.Items.Add(value);
            }
            this.cmbRegId_Carrier.SelectedIndex = 0;
            this.updatePanel4.Update();
        }
    }

    private void initRegId_Carrier(string Type,string Value)
    {
        IList<ConstValueTypeInfo> lstRegId = iConstValueType.GetConstValueTypeList(Type);
        this.cmbRegId_Carrier.Items.Clear();
        if (lstRegId.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstRegId)
            {
                string value = item.value.ToString();
                this.cmbRegId_Carrier.Items.Add(value);
            }
            this.cmbRegId_Carrier.SelectedValue = Value;
            this.updatePanel4.Update();
        }
    }

    private void initPalletTypeSelect()
    {
        IList<ConstValueTypeInfo> lstPalletType = iConstValueType.GetConstValueTypeList("PalletType");
        this.cmbPalletType.Items.Clear();
        if (lstPalletType.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstPalletType)
            {
                string value = item.value.ToString();
                this.cmbPalletType.Items.Add(value);
            }
            this.cmbPalletType.SelectedIndex = 0;
        }
    }
	
    private void initOceanTypeSelect()
    {
        IList<ConstValueTypeInfo> lstOceanType = iConstValueType.GetConstValueTypeList("OceanType");
        this.cmbOceanType.Items.Clear();
        if (lstOceanType.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstOceanType)
            {
                string value = item.value.ToString();
                this.cmbOceanType.Items.Add(value);
            }
            this.cmbOceanType.SelectedIndex = 0;
        }
    }
	
    private void initPalletCodeSelect()
    {
        IList<ConstValueTypeInfo> lstPalletCode = iConstValueType.GetConstValueTypeList("PalletCode");
        this.cmbPalletCode.Items.Clear();
        if (lstPalletCode.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstPalletCode)
            {
                string value = item.value.ToString();
                this.cmbPalletCode.Items.Add(value);
            }
            this.cmbPalletCode.SelectedIndex = 0;
        }
    }

    //private void initInPltWeightSelect()
    //{
    //    IList<ConstValueTypeInfo> lstInPltWeight = iConstValueType.GetConstValueTypeList("InPltWeight");
    //    this.cmbInPltWeight.Items.Clear();
    //    if (lstInPltWeight.Count != 0)
    //    {
    //        foreach (ConstValueTypeInfo item in lstInPltWeight)
    //        {
    //            string value = item.value.ToString();
    //            this.cmbInPltWeight.Items.Add(value);
    //        }
    //        this.cmbInPltWeight.SelectedIndex = 0;
    //    }
    //}

    private bool CheckRegId_Carrier(string Type,string value)
    {
        IList<ConstValueTypeInfo> lstRegId = iConstValueType.GetConstValueTypeList(Type);
        var query = (from c in lstRegId
                    where c.value == value
                    select new { c.type }).Count().ToString();
        int count = Convert.ToInt32(query);
        if (count == 0) 
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void bindTable(IList<PalletTypeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ShipWay");      //0
        dt.Columns.Add("RegId");        //1
        dt.Columns.Add("StdFullQty");   //2
        dt.Columns.Add("PalletLayer");  //3
        dt.Columns.Add("MinQty");       //4
        dt.Columns.Add("MaxQty");       //5
        dt.Columns.Add("PalletType");   //6
		dt.Columns.Add("OceanType");
       
        dt.Columns.Add("PalletCode");
        dt.Columns.Add("Weight");
        dt.Columns.Add("MinusPltWeight");
        dt.Columns.Add("CheckCode");
        dt.Columns.Add("ChepPallet");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");
        
        if (list != null && list.Count != 0)
        {
            foreach (PalletTypeInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.ShipWay;
                dr[1] = temp.RegId;


                dr[2] = temp.StdFullQty;
                dr[3] = temp.PalletLayer;
                dr[4] = temp.MinQty;
                dr[5] = temp.MaxQty;
                dr[6] = temp.PalletType;
				dr[7] = temp.OceanType;

                dr[8] = temp.PalletCode;
                dr[9] = temp.Weight;
                dr[10] = temp.InPltWeight;
                dr[11] = temp.CheckCode;
                dr[12] = temp.ChepPallet;
                dr[13] = temp.Editor;
                dr[14] = (temp.Cdt == null ? string.Empty : temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[15] = (temp.Udt == null ? string.Empty : temp.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[16] = temp.ID;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            //this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            //this.hidRecordCount.Value = "";
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
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

    protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
    {
        try
        {

            if (this.ByRegId.Checked == true)
            {
                initRegId_Carrier("RegId");
            }
            else
            {
                initRegId_Carrier("Carrier");
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }
    
    protected void btn_ServerClick(Object sender, EventArgs e)
    {
        string value = this.hidRegId_Carrier.Value.ToString();
        if (CheckRegId_Carrier("RegId", value))
        {
            this.ByRegId.Checked = true;
            initRegId_Carrier("RegId", value);
        }
        else if (CheckRegId_Carrier("Carrier", value))
        {
            this.ByCarrier.Checked = true;
            initRegId_Carrier("Carrier", value);
        }
    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string ShipWay = this.hidShipWay.Value;
            string RegId = this.hidRegid.Value;
            IList<PalletTypeInfo> lstPalletTypeList = iPalletTypeforICC.GetPalletType(ShipWay, RegId);
            bindTable(lstPalletTypeList, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }

    protected void Save_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            PalletTypeInfo PalletTypeInfo = new PalletTypeInfo();

            PalletTypeInfo.ShipWay = this.hidShipWay2.Value;
            PalletTypeInfo.RegId = this.hidRegId_Carrier.Value;
            PalletTypeInfo.PalletType = this.hidPalletType.Value;
			PalletTypeInfo.OceanType = this.hidOceanType.Value;
            PalletTypeInfo.PalletCode = this.hidPalletCode.Value;
            PalletTypeInfo.InPltWeight = this.hidInPltWeight.Value;
            PalletTypeInfo.StdFullQty = this.txtStdFullQty.Value;
            PalletTypeInfo.MinQty = Convert.ToInt32(this.txtMinQty.Value.Trim());
            PalletTypeInfo.MaxQty = Convert.ToInt32(this.txtMaxQty.Value.Trim());
            PalletTypeInfo.Weight = Convert.ToDecimal(this.txtWeight.Value.Trim());
            PalletTypeInfo.CheckCode = this.txtCheckCode.Value.Trim();
            PalletTypeInfo.ChepPallet = this.hidChepPallet.Value;
            PalletTypeInfo.PalletLayer = Convert.ToInt32(this.txtPalletLayer.Value.Trim());
            PalletTypeInfo.Editor = UserId;

            /*IList<PalletTypeInfo> lstPalletType = iPalletTypeforICC.CheckExistRangeQty(PalletTypeInfo.ShipWay, PalletTypeInfo.RegId, PalletTypeInfo.StdFullQty, PalletTypeInfo.PalletLayer, PalletTypeInfo.MaxQty, PalletTypeInfo.MinQty);
            if (lstPalletType.Count == 0)
            {
                iPalletTypeforICC.Add(PalletTypeInfo);
            }
            else
            {
                PalletTypeInfo.ID = lstPalletType[0].ID;
                iPalletTypeforICC.Update(PalletTypeInfo);
            }
            */

            string errDuplicatePalletType = this.GetLocalResourceObject(Pre + "_errDuplicatePalletType").ToString();

            IList<PalletTypeInfo> lstPalletType = iPalletTypeforICC.CheckExistRangeQty(PalletTypeInfo.ShipWay, PalletTypeInfo.RegId, PalletTypeInfo.StdFullQty, PalletTypeInfo.PalletLayer, PalletTypeInfo.OceanType, PalletTypeInfo.MaxQty, PalletTypeInfo.MinQty);
            string id = this.hidID.Value;
            if (string.IsNullOrEmpty(id))
            {
                if (lstPalletType.Count > 0)
                {
                    throw new Exception(errDuplicatePalletType);
                }

                iPalletTypeforICC.Add(PalletTypeInfo);
            }
            else
            {
                if ((lstPalletType.Count > 1) || (lstPalletType[0].ID != int.Parse(id)))
                {
                    throw new Exception(errDuplicatePalletType);
                }

                PalletTypeInfo.ID = lstPalletType[0].ID;
                iPalletTypeforICC.Update(PalletTypeInfo);
            }
			
            IList<PalletTypeInfo> lstPalletTypeList = iPalletTypeforICC.GetPalletType(this.hidShipWay.Value,this.hidRegid.Value);
            bindTable(lstPalletTypeList,DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight(); clearDetailInfo();", true);
            hideWait();
        }
    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            PalletTypeInfo PalletTypeInfo = new PalletTypeInfo();

            PalletTypeInfo.ID = Convert.ToInt32(this.hidID.Value);
            PalletTypeInfo.ShipWay = this.hidShipWay2.Value;
            PalletTypeInfo.RegId = this.hidRegId_Carrier.Value;
            if (PalletTypeInfo.ID != 0)
            {
                iPalletTypeforICC.Remove(PalletTypeInfo);
            }
            IList<PalletTypeInfo> lstPalletTypeList = iPalletTypeforICC.GetPalletType(this.hidShipWay.Value, this.hidRegid.Value);
            bindTable(lstPalletTypeList, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
        finally
        {
            hideWait();
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");HideWait();");
        //scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("getAvailableData(\"processFun\"); inputFlag = false;");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        
    }

    private void showAlertErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "showAlertErrorMessage", scriptBuilder.ToString(), false);
    }

    private void changeSelectedIndex(string index, string family, string mbcode, string ecr, string iecV, string custV)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("selectedRowIndex=" + index + ";");
        //this.CmbMaintainFamilyForECRVersion2.refresh();
        //this.CmbMaintainFamilyForECRVersion2.InnerDropDownList.SelectedValue = family;
        //scriptBuilder.AppendLine("document.getElementById('" + txtMBCode.ClientID + "').value='" + mbcode + "';");
        //scriptBuilder.AppendLine("document.getElementById('" + txtPCB.ClientID + "').value='" + ecr + "';");
        //scriptBuilder.AppendLine("document.getElementById('" + this.ttRemark.ClientID + "').value='" + custV + "';");
        if (string.Compare(index, "-1", true) == 0)
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
        }
        else
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
        }

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "changeSelectedIndex", scriptBuilder.ToString(), false);
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("setCommonFocus();");
        scriptBuilder.AppendLine("HideWait();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(7);
		gd.HeaderRow.Cells[7].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[14].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[15].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[16].Width = Unit.Percentage(2);
    }
}
