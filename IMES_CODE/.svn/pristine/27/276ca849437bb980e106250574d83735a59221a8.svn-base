using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Collections;


public partial class DataMaintain_VendorCode : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IVendorCode iVendorCode;

    private const int COL_NUM = 6;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iVendorCode = ServiceAgent.getInstance().GetMaintainObjectByName<IVendorCode>(WebConstant.MaintainVendorCodeObject);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();

                IList<VendorCodeDef> dataList = iVendorCode.GetAllVendorCodeList();
                bindTable(dataList, DEFAULT_ROWS);
                
            }
            this.cmbMaintainVendor.InnerDropDownList.SelectedIndexChanged += new EventHandler(CmbVendor_Selected);
            this.cmbMaintainVendor.InnerDropDownList.AutoPostBack = true;
            
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

    private Boolean ShowVendorList()
    {
        try
        {
            IList<VendorCodeDef> dataList = iVendorCode.GetAllVendorCodeList();
            if (dataList == null || dataList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }

        return true;

    }

    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {
        this.lblVendor.Text = this.GetLocalResourceObject(Pre + "_lblVendor").ToString();
        this.lblIdex.Text = this.GetLocalResourceObject(Pre + "_lblIdex").ToString();
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNO").ToString();
        this.lblVendorCode.Text = this.GetLocalResourceObject(Pre + "_lblVendorCode").ToString();

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(1);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        VendorCodeDef item = new VendorCodeDef();
        string selVender = "";
        item.Vendor = this.cmbMaintainVendor.InnerDropDownList.SelectedValue;
        selVender = item.Vendor;
        if (item.Vendor == "AST")
        {
            item.Vendor = this.dPartNo.Text.Trim().ToUpper();
        }
        var idex = this.dIdex.Text.Trim();
        if (idex.StartsWith("0"))
        {
            idex = idex.Substring(1);
        }
        item.Idex = idex;
        item.VendorCode = this.dVendorCode.Text.Trim().ToUpper();
        item.Editor = this.HiddenUserName.Value;

        try
        {
            iVendorCode.AddVendorCode(item, selVender);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        ShowVendorList();
        String itemId = replaceSpecialChart(item.VendorCode);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string id = this.dOldId.Value.Trim().ToUpper();
        try
        {
            VendorCodeDef item = new VendorCodeDef();
            item.ID = id;
            iVendorCode.DeleteVendorCode(item);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ShowVendorList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<VendorCodeDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemVendor").ToString());  //0
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemIdex").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemVendorCode").ToString());  //1

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //3

        dt.Columns.Add("Id"); //6       

        if (list != null && list.Count != 0)
        {
            foreach (VendorCodeDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Vendor;
                dr[1] = temp.Idex;
                dr[2] = temp.VendorCode;
                dr[3] = temp.Editor;
                dr[4] = temp.Cdt;
                dr[5] = temp.Udt;
                dr[6] = temp.ID;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
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

            this.hidRecordCount.Value = "";
        }

        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }

    }

    private void CmbVendor_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //if (this.cmbMaintainVendor.InnerDropDownList.SelectedItem.Text == "AST")

            //dPartNo.Attributes.Add("Enabled", "True");
            //this.dPartNo.ReadOnly = False;
            //this.dPartNo.Enabled = true;

            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("<script language='javascript'>");
            scriptBuilder.AppendLine("ChangeVendor();");
            scriptBuilder.AppendLine("</script>");
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ChangeVendor", scriptBuilder.ToString(), false);

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
            int index = this.cmbMaintainVendor.InnerDropDownList.SelectedIndex;
            this.cmbMaintainVendor.setSelected(index);
            CmbVendor_Selected(this.cmbMaintainVendor, null);
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

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
}
