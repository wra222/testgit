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
using IMES.Station.Interface.CommonIntf;

public partial class DataMaintain_OSWIN : System.Web.UI.Page
{
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private IOSWIN iOSWIN = ServiceAgent.getInstance().GetMaintainObjectByName<IOSWIN>(WebConstant.IOSWIN);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
   
            if (!this.IsPostBack)
            {
                initSelect();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "", true);
    }

    private void initSelect()
    {
        initFamilySelect();
        initFamily2Select();
    }

    private void initFamilySelect()
    {
        IList<string> lstFamily = iOSWIN.GetOSWINFamily();
        this.cmbFamily.Items.Clear();
        if (lstFamily.Count != 0)
        {
            this.cmbFamily.Items.Add("ALL");
            foreach (string item in lstFamily)
            {
                this.cmbFamily.Items.Add(item);
            }
            this.cmbFamily.SelectedIndex = 0;
        }
    }

    private void initFamily2Select()
    {
        IList<string> lstFamily2 = iOSWIN.GetFamilyObjList();
        this.cmbFamily2.Items.Clear();
        if (lstFamily2.Count != 0)
        {
            foreach (string item in lstFamily2)
            {
                this.cmbFamily2.Items.Add(item);
            }
            this.cmbFamily2.SelectedIndex = 0;
        }
    }

    private void bindTable(IList<OSWINInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Family");
        dt.Columns.Add("Zmod");
        dt.Columns.Add("OS");
        dt.Columns.Add("AV");
        dt.Columns.Add("Image");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");
        
        if (list != null && list.Count != 0)
        {
            foreach (OSWINInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Family;
                dr[1] = temp.Zmode;
                dr[2] = temp.OS;
                dr[3] = temp.AV;
                dr[4] = temp.Image;
                dr[5] = temp.Editor;
                dr[6] = (temp.Cdt == null ? string.Empty : temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[7] = (temp.Udt == null ? string.Empty : temp.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[8] = temp.ID;
                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < DEFAULT_ROWS; i++)
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

    protected void btnLoadFirstDataList_ServerClick(Object sender, EventArgs e)
    {

        btnQuery_ServerClick(sender, e);
    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string Family = this.cmbFamily.SelectedItem.ToString();
            IList<OSWINInfo> lstPalletTypeList = iOSWIN.GetOSWINList(Family);
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
            OSWINInfo OSWINInfo = new OSWINInfo();

            OSWINInfo.Family = this.hidFamily2.Value;
            OSWINInfo.Zmode = this.txtZmod.Value;
            OSWINInfo.OS = this.txtOS.Value;
            OSWINInfo.AV = this.txtAV.Value;
            OSWINInfo.Image = this.txtImage.Value;
            OSWINInfo.Editor = UserId;

            OSWINInfo CheckOSWIN = iOSWIN.CheckExistOSWIN(OSWINInfo.Family, OSWINInfo.Zmode);

            if (CheckOSWIN.Family == "" || CheckOSWIN.Family == null)
            {
                iOSWIN.Add(OSWINInfo);
            }
            else
            {
                OSWINInfo.ID = CheckOSWIN.ID;
                iOSWIN.Update(OSWINInfo);
            }
            IList<OSWINInfo> lstOSWINList = iOSWIN.GetOSWINList(this.cmbFamily.SelectedValue.ToString().Trim());
            initFamilySelect();
            this.updatePanel3.Update();
            bindTable(lstOSWINList, DEFAULT_ROWS);
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
            OSWINInfo OSWINInfo = new OSWINInfo();

            OSWINInfo.ID = Convert.ToInt32(this.hidID.Value);
            OSWINInfo.Family = this.hidFamily2.Value;
            if (OSWINInfo.ID != 0)
            {
                iOSWIN.Remove(OSWINInfo);
            }
            IList<OSWINInfo> lstOSWINList = iOSWIN.GetOSWINList(this.cmbFamily.SelectedValue.ToString().Trim());
            initFamilySelect();
            this.updatePanel3.Update();
            bindTable(lstOSWINList, DEFAULT_ROWS);
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
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(4);
    }
}
