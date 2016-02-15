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

public partial class DataMaintain_PCBVersionMaintain : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private IPCBVersion iPCBVersion = null;
    private string gFamily = string.Empty;
    private string gMbCode = string.Empty;
    private string gPCB = string.Empty;
    private bool gHasHightRow = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;   //
           //UserId = "";
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
                //ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "init", "showFirstFamilyDataList();", true);
      //          Customer = UserInfo.Customer;
            }

            iPCBVersion = ServiceAgent.getInstance().GetMaintainObjectByName<IPCBVersion>(WebConstant.IPCBVersion);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "initContorls();", true);
    }

    private void initLabel()
    {
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblFamily2.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblPCB.Text = this.GetLocalResourceObject(Pre + "_lblPCB").ToString();
        this.lblMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCode").ToString();
        this.lblCTVer.Text = this.GetLocalResourceObject(Pre + "_lblCTVer").ToString();
        this.lblSupplier.Text = this.GetLocalResourceObject(Pre + "_lblSupplier").ToString();
        this.lblPCBVersionList.Text = this.GetLocalResourceObject(Pre + "_lblPCBVersionList").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
    }

    private void bindTable(IList<PCBVersionInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colMBCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPCB").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCTVer").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSupplier").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUpdateDate").ToString());
        
        if (list != null && list.Count != 0)
        {
            foreach (PCBVersionInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Family;
                dr[1] = temp.MBCode;
                dr[2] = temp.PCBVer;
                dr[3] = temp.CTVer;
                dr[4] = temp.Supplier;
                dr[5] = temp.Remark;
                dr[6] = temp.Editor;
                dr[7] = (temp.Cdt == null ? string.Empty : temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[8] = (temp.Udt == null ? string.Empty : temp.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));

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

    protected void gd_DataBound(object sender, EventArgs e)
    {
        if (!gHasHightRow)
        {
            changeSelectedIndex("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
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

            if (string.Compare(e.Row.Cells[0].Text.Trim(), gFamily.Trim(), true) == 0
                && string.Compare(e.Row.Cells[1].Text.Trim(), gMbCode.Trim(), true) == 0
                && string.Compare(e.Row.Cells[2].Text.Trim(), gPCB.Trim(), true) == 0)
            {
                string cellIecV = (e.Row.Cells[4].Text.Trim().ToLower() == "&nbsp;" ? string.Empty : e.Row.Cells[4].Text.Trim());
                string cellRemark = (e.Row.Cells[5].Text.Trim().ToLower() == "&nbsp;" ? string.Empty : e.Row.Cells[5].Text.Trim());

                e.Row.CssClass = "iMes_grid_SelectedRowGvExt";
                changeSelectedIndex(e.Row.RowIndex.ToString(), gFamily.Trim(), gMbCode, gPCB, cellIecV, cellRemark);
                gHasHightRow = true;
            }
        }
    }

    protected void btnLoadFirstDataList_ServerClick(Object sender,EventArgs e) 
    {
        btnFamilyChange_ServerClick(sender,e);
    }

    protected void btnFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string family = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue.Trim();
            IList<PCBVersionInfo> lstPCBVersionInfo = iPCBVersion.GetPCBVersionInfoListByFamily(family);

            bindTable(lstPCBVersionInfo, DEFAULT_ROWS);
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
        finally
        {
            hideWait();
        }
    }

    protected void Save_ServerClick(Object sender, EventArgs e)
    {
        string msgHasExistRecord = GetLocalResourceObject(Pre + "_msgHasExistRecord").ToString();

        try
        {
            PCBVersionInfo PCBVersionInfo = new PCBVersionInfo();
            PCBVersionInfo.Family = this.hidFamily2.Value;
            PCBVersionInfo.MBCode = this.txtMBCode.Value.Trim();
            PCBVersionInfo.PCBVer = this.txtPCB.Value.Trim();
            PCBVersionInfo.CTVer  = this.txtCTVer.Value.Trim();
            PCBVersionInfo.Supplier = this.txtSupplier.Value.Trim();
            PCBVersionInfo.Remark = this.ttRemark.Value.Trim();
            PCBVersionInfo.Editor = UserId.ToString();
            string oldFamily = this.CmbMaintainFamilyForECRVersion2.InnerDropDownList.SelectedValue;//this.hidFamily.Value;
      

            gFamily = this.hidFamily2.Value;
            gMbCode = this.txtMBCode.Value.Trim();
            gPCB = this.txtPCB.Value.Trim();

            iPCBVersion.SavePCBVersion(PCBVersionInfo);

            this.CmbMaintainFamilyForECRVersion.refresh();
            this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue = oldFamily;
            IList<PCBVersionInfo> lstPCBVersionInfo = iPCBVersion.GetPCBVersionInfoListByFamily(oldFamily);
            bindTable(lstPCBVersionInfo, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
            bindTable(null, DEFAULT_ROWS);
            //this.hidRecordCount.Value = "0";
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);
            hideWait();
        }
    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<PCBVersionInfo> lstPCBVersionInfo = null;
            PCBVersionInfo deleteItem = new PCBVersionInfo();
            deleteItem.Family=this.hidFamily2.Value;
            deleteItem.MBCode = this.hidDeleteMBCode.Value.Trim();
            deleteItem.PCBVer = this.hidDeletePCB.Value.Trim();
            string oldFamily = this.hidFamily2.Value;

            iPCBVersion.DeletePCBVersion(deleteItem);
            
            this.CmbMaintainFamilyForECRVersion.refresh();
            this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue = oldFamily;
            lstPCBVersionInfo = iPCBVersion.GetPCBVersionInfoListByFamily(oldFamily);
            bindTable(lstPCBVersionInfo, DEFAULT_ROWS);
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
        this.CmbMaintainFamilyForECRVersion2.refresh();
        this.CmbMaintainFamilyForECRVersion2.InnerDropDownList.SelectedValue = family;
        scriptBuilder.AppendLine("document.getElementById('" + txtMBCode.ClientID + "').value='" + mbcode + "';");
        scriptBuilder.AppendLine("document.getElementById('" + txtPCB.ClientID + "').value='" + ecr + "';");
        scriptBuilder.AppendLine("document.getElementById('" + this.ttRemark.ClientID + "').value='" + custV + "';");
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
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(10);
    }
}
