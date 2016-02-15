/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: ECRVersion Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-27   Tong.Zhi-Yong     Create 
 * Known issues:Any restrictions about this file 
 * issueCode
 * ITC-1366-0013  itc210012   2011-01-11
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
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_ECRVersionMaintain : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private IECRVersion iECRVersion = null;
    private string gFamily = string.Empty;
    private string gMbCode = string.Empty;
    private string gECR = string.Empty;
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

            iECRVersion = ServiceAgent.getInstance().GetMaintainObjectByName<IECRVersion>(WebConstant.IECRVersion);
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
   //     this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
   //     this.lblCustVersion.Text = this.GetLocalResourceObject(Pre + "_lblCustVersion").ToString();
        this.lblECR.Text = this.GetLocalResourceObject(Pre + "_lblECR").ToString();
        this.lblMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCode").ToString();
  //      this.lblResult.Text = this.GetLocalResourceObject(Pre + "_titleResult").ToString();
        this.lblIECVersion.Text = this.GetLocalResourceObject(Pre + "_lblIECVersion").ToString();
        this.lblEcrVersionList.Text = this.GetLocalResourceObject(Pre + "_lblEcrVersionList").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
    }

    private void bindTable(IList<EcrVersionInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ID");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colMBCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colECR").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colIECVersion").ToString());
  //      dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCustomerVersion").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUpdateDate").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (EcrVersionInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.ID;
                dr[1] = temp.Family;
                dr[2] = temp.MBCode;
                dr[3] = temp.ECR;
                dr[4] = temp.IECVer;
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
        e.Row.Cells[0].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

            if (string.Compare(e.Row.Cells[1].Text.Trim(), gFamily.Trim(), true) == 0
                && string.Compare(e.Row.Cells[2].Text.Trim(), gMbCode.Trim(), true) == 0
                && string.Compare(e.Row.Cells[3].Text.Trim(), gECR.Trim(), true) == 0)
            {
                string cellIecV = (e.Row.Cells[4].Text.Trim().ToLower() == "&nbsp;" ? string.Empty : e.Row.Cells[4].Text.Trim());
                string cellRemark = (e.Row.Cells[5].Text.Trim().ToLower() == "&nbsp;" ? string.Empty : e.Row.Cells[5].Text.Trim());

                e.Row.CssClass = "iMes_grid_SelectedRowGvExt";
                changeSelectedIndex(e.Row.RowIndex.ToString(), gFamily.Trim(), gMbCode, gECR, cellIecV, cellRemark);
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
            IList<EcrVersionInfo> lstEcrVersionInfo = iECRVersion.GetECRVersionInfoListByFamily(family);

            bindTable(lstEcrVersionInfo, DEFAULT_ROWS);
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
            IList<EcrVersionInfo> lstEcrVersionInfo = null;
            string family = this.hidFamily2.Value;
            //issue code
            //ITC-1361-0086  itc210012  2012-2-15
            string mbCode = this.txtMBCode.Value.Trim();
            string ecr = this.txtECR.Value.Trim();
            string iecVersion = this.txtIECVersion.Value.Trim();
            string custVersion = "";
     //       string custVersion = this.txtCustVersion.Value.ToUpper().Trim();
            string remark = this.ttRemark.Value.Trim();
            string oldFamily = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue;//this.hidFamily.Value;
            string id = this.hidDeleteID.Value;
            EcrVersionInfo evi = null;

            gFamily = family;
            gMbCode = mbCode;
            gECR = ecr;

            evi = getECRVersionInfo(family, mbCode, ecr, iecVersion, remark);

            if (!string.IsNullOrEmpty(id))
            {
                evi.ID = int.Parse(id);
            }

            iECRVersion.SaveECRVersion(evi);
            this.CmbMaintainFamilyForECRVersion.refresh();
            this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue = oldFamily;
            lstEcrVersionInfo = iECRVersion.GetECRVersionInfoListByFamily(oldFamily);

            bindTable(lstEcrVersionInfo, DEFAULT_ROWS);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
            //bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
            //bindTable(null, DEFAULT_ROWS);
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
            IList<EcrVersionInfo> lstEcrVersionInfo = null;
            EcrVersionInfo deleteItem = getECRVersionInfo(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string oldFamily = this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue;//this.hidFamily.Value;
          
            deleteItem.ID = int.Parse(this.hidDeleteID.Value);
            iECRVersion.DeleteECRVersion(deleteItem);
            this.CmbMaintainFamilyForECRVersion.refresh();
            this.CmbMaintainFamilyForECRVersion.InnerDropDownList.SelectedValue = oldFamily;
            lstEcrVersionInfo = iECRVersion.GetECRVersionInfoListByFamily(oldFamily);

            bindTable(lstEcrVersionInfo, DEFAULT_ROWS);
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
        scriptBuilder.AppendLine("document.getElementById('" + txtECR.ClientID + "').value='" + ecr + "';");
        scriptBuilder.AppendLine("document.getElementById('" + txtIECVersion.ClientID + "').value='" + iecV + "';");
  //      scriptBuilder.AppendLine("document.getElementById('" + txtCustVersion.ClientID + "').value='" + custV + "';");
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

    private EcrVersionInfo getECRVersionInfo(string family, string mbcode, string ecr, string iecVersion, string custVersion)
    {
        EcrVersionInfo e = new EcrVersionInfo();

        e.Family = family;
        e.MBCode = mbcode;
        e.ECR = ecr;
        e.IECVer = iecVersion;
        e.Remark = custVersion;
        e.Editor = UserId;
       
        return e;
    }

    private void setColumnWidth()
    {
        //ITC-1361-0093 itc210012 2012-02-17
        //ITC-1361-0105  itc210012  2012-02-28
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Pixel(15);
        //gd.HeaderRow.Cells[2].Width = Unit.Pixel(10);
        //gd.HeaderRow.Cells[3].Width = Unit.Pixel(10);
        //gd.HeaderRow.Cells[4].Width = Unit.Pixel(10);
        //gd.HeaderRow.Cells[5].Width = Unit.Pixel(20);
        //gd.HeaderRow.Cells[6].Width = Unit.Pixel(10);
        //gd.HeaderRow.Cells[7].Width = Unit.Pixel(10);
        //gd.HeaderRow.Cells[8].Width = Unit.Pixel(10);

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
