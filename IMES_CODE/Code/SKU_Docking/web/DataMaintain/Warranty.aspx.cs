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
//using IMES.Station.Interface.StationIntf;
//using IMES.Station.Interface.CommonIntf;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

//ITC-1136-0022
//ShipTypeCode原来是个下拉框，被换成了输入

public partial class DataMaintain_Warranty : IMESBasePage 
{

    //!!! need change
    public String userName;

    public const String DATACODE_TYPE_SHIPTYPE = "ShipType";
    public const String DATACODE_TYPE_WARRANTYTYPE = "Warranty";
    
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IWarranty iWarranty;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    //public string pmtMessage6;
    //public string pmtMessage7;
    //public string pmtMessage8;
    //public string pmtMessage9;
    //public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            //pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            //pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();

            this.cmbCustomer.InnerDropDownList.Load += new EventHandler(cmbCustomer_Load);
            //this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_SelectedChange); 
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId; 
                initLabel();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
            }

            iWarranty = (IWarranty)ServiceAgent.getInstance().GetMaintainObjectByName<IWarranty>(WebConstant.MaintainCommonObject);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
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


    protected void btnCustomerChange_ServerClick(object sender, System.EventArgs e)
    //private void cmbCustomer_SelectedChange(object sender, System.EventArgs e)
    {
        ShowWarrantyListByCustom();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "setNewItemValue();DealHideWait();", true);

    }

    private void cmbCustomer_Load(object sender, System.EventArgs e)
    {
        ShowWarrantyListByCustom();
    }

    private Boolean ShowWarrantyListByCustom()
    {
        String customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        try
        {
            IList<WarrantyDef> dataList;
            dataList = iWarranty.GetWarrantyList(customer);
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
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomerText").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDesc").ToString();

        this.lblShipTypeCode.Text = this.GetLocalResourceObject(Pre + "_lblShipTypeCode").ToString();
        this.lblWarrantyCode.Text = this.GetLocalResourceObject(Pre + "_lblWarrantyCode").ToString();
        //this.dShipType.Text = this.GetLocalResourceObject(Pre + "_lblShipType").ToString();
        //this.dWarranty.Text = this.GetLocalResourceObject(Pre + "_lblWarranty").ToString();
        this.lblFormat.Text = this.GetLocalResourceObject(Pre + "_lblFormat").ToString();

        this.btnAdd.Value  = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value  = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(22);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        WarrantyDef item = new WarrantyDef();
        item.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        //if(this.dShipType.Checked==true)
        //{
        //    item .DateCodeType=DATACODE_TYPE_SHIPTYPE;
        //}
        //else
        //{
        //    item .DateCodeType=DATACODE_TYPE_WARRANTYTYPE;
        //}
        
        item.Descr =this.dDesc.Text.Trim();
        item.Editor = this.HiddenUserName.Value; 
        item.id=this.dOldId.Value.Trim();
        item.ShipTypeCode = this.dShipTypeCode.Text.Trim().ToUpper();
        item.ShipTypeCodeText=item.ShipTypeCode;
        item.Type=this.cmbWarrantyType.InnerDropDownList.SelectedValue;
        item.TypeText=this.cmbWarrantyType.InnerDropDownList.SelectedItem.Text;
        item.WarrantyCode=this.dWarrantyCode.Text.Trim().ToUpper();
        item.WarrantyFormat=this.cmbWarrantyFormat.InnerDropDownList.SelectedValue;
        item.WarrantyFormatText=this.cmbWarrantyFormat.InnerDropDownList.SelectedItem.Text;

         try
        {
            iWarranty.UpdateWarranty(item, item.id);
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
        ShowWarrantyListByCustom();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + item.id + "');DealHideWait();", true);

    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        WarrantyDef item = new WarrantyDef();
        item.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        //if (this.dShipType.Checked == true)
        //{
        //    item.DateCodeType = DATACODE_TYPE_SHIPTYPE;
        //}
        //else
        //{
        //    item.DateCodeType = DATACODE_TYPE_WARRANTYTYPE;
        //}

        item.Descr = this.dDesc.Text.Trim();
        item.Editor = this.HiddenUserName.Value; 
        item.id = "0";
        item.ShipTypeCode = this.dShipTypeCode.Text.Trim().ToUpper();
        item.ShipTypeCodeText = item.ShipTypeCode;
        item.Type = this.cmbWarrantyType.InnerDropDownList.SelectedValue;
        item.TypeText = this.cmbWarrantyType.InnerDropDownList.SelectedItem.Text;
        item.WarrantyCode = this.dWarrantyCode.Text.Trim().ToUpper();
        item.WarrantyFormat = this.cmbWarrantyFormat.InnerDropDownList.SelectedValue;
        item.WarrantyFormatText = this.cmbWarrantyFormat.InnerDropDownList.SelectedItem.Text;

        string ret = "0";
        try
        {
            ret=iWarranty.AddWarranty(item);
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

        ShowWarrantyListByCustom();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + ret + "');DealHideWait();", true);
    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        //string familyId = this.dFamily.Text.Trim();
        string oldId = this.dOldId.Value.Trim();
        try
        {
            iWarranty.DeleteWarranty(oldId);
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
        ShowWarrantyListByCustom();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
        //dOldFamilyId.Value = "aaaa";

    }

    private void bindTable(IList<WarrantyDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Description").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Type").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_ShipTypeCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_WarrantyFormat").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_WarrantyCode").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        dt.Columns.Add("Id");
        dt.Columns.Add("Type");
        dt.Columns.Add("ShipTypeCodeValue");
        dt.Columns.Add("WarrantyFormatValue");
        dt.Columns.Add("DateCodeTypeValue");

        if (list != null && list.Count != 0)
        {
            foreach (WarrantyDef temp in list)
            {
                dr = dt.NewRow();

                //List<string[]> selectValues = new List<string[]>();
                //string[] item1 = new String[2];
                //item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText1");
                //item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue1");
                //selectValues.Add(item1);

                //string[] item2 = new String[2];
                //item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText2");
                //item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue2");
                //selectValues.Add(item2);

                //string[] item3 = new String[2];
                //item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText3");
                //item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue3");
                //selectValues.Add(item3);

                //string[] item4 = new String[2];
                //item4[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemText4");
                //item4[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbWarrantyCodeItemValue4");
                //selectValues.Add(item4);

                //for (int k = 0; k < selectValues.Count; k++)
                //{
                //    if (temp.ShipTypeCode.Trim() == selectValues[k][1])
                //    {
                //        temp.ShipTypeCodeText = selectValues[k][0];
                //        break;
                //    }
                //}

                //因为原来ShipTypeCode是个下拉列表
                temp.ShipTypeCodeText = temp.ShipTypeCode;
                dr[0] = temp.Descr;
                dr[1] = temp.TypeText;
                dr[2] = temp.ShipTypeCodeText;
                dr[3] = temp.WarrantyFormatText;
                dr[4] = temp.WarrantyCode;

                dr[5] = temp.Editor;
                dr[6] = temp.cdt;
                dr[7] = temp.udt;

                dr[8] = temp.id;
                dr[9] = temp.Type;
                dr[10] = temp.ShipTypeCode;
                dr[11] = temp.WarrantyFormat;
                dr[12] = temp.DateCodeType;
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;DealHideWait();", true); 
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

    //private void hideWait()
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("setCommonFocus();");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    //}

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
}
