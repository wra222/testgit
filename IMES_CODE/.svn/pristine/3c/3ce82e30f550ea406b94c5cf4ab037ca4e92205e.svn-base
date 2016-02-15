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

public partial class DataMaintain_SMTLineSpeedMaintain : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private SMTLineSpeed iSMTLineSpeed = null;
    private string gFamily = string.Empty;
    private string gMbCode = string.Empty;
    private string gECR = string.Empty;
    private bool gHasHightRow = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;   //
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
            }

            iSMTLineSpeed = ServiceAgent.getInstance().GetMaintainObjectByName<SMTLineSpeed>(WebConstant.SMTLineSpeed);
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
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblMode1.Text = this.GetLocalResourceObject(Pre + "_lblMode").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblSpeed.Text = this.GetLocalResourceObject(Pre + "_lblSpeed").ToString();
        this.lblRate.Text = this.GetLocalResourceObject(Pre + "_lblRate").ToString();
        this.lblOther.Text = this.GetLocalResourceObject(Pre + "_lblOther").ToString();
        this.lblSMTLineSpeed.Text = this.GetLocalResourceObject(Pre + "_lblSMTLineSpeed").ToString();
        this.lblMode.Text = this.GetLocalResourceObject(Pre + "_lblMode").ToString();
    }

    private void bindTable(IList<SmtctInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

     
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colMode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSpeed").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (SmtctInfo temp in list)
            {
                dr = dt.NewRow();

               
                dr[0] = temp.line;
                dr[1] = temp.family;
                dr[2] = temp.optRate;
                dr[3] = temp.ct;
                dr[4] = temp.remark;
                dr[5] = temp.editor;
                dr[6] = (temp.cdt == null ? string.Empty : temp.cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[7] = temp.id;
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "resetTableHeight();", true); 
    }

    protected void gd_DataBound(object sender, EventArgs e)
    {
        //e.Row.Cells[0].Style.Add("display", "none");

        if (!gHasHightRow)
        {
            changeSelectedIndex("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[7].Style.Add("display", "none");

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

    protected void btnFamilyTextChange_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string text = this.CmbMaintainFamilyForSMT.InnerDropDownList.SelectedValue.Trim();

            // this.CmbMaintainFamilyForSMT.change(text);
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
    protected void btnFamilyChange_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string line = this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue.Trim();
            IList<SmtctInfo> lstSmtctInfo = iSMTLineSpeed.GetAllSMTLineItems(line); //GetECRVersionInfoListByFamily(family);

            bindTable(lstSmtctInfo, DEFAULT_ROWS);
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
            IList<SmtctInfo> lstSmtctInfo = null;
            string family = this.hidFamily2.Value;
            decimal ct = decimal.Parse( this.txtSpeed.Value.Trim());
            float oprate = float.Parse(this.txtRate.Value.Trim());
            string remark = this.txtOther.Value.Trim();

            string oldLine = this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue;//this.hidFamily.Value;
            string id = this.hidDeleteID.Value;
            SmtctInfo SmtctNew = null;
            SmtctInfo SmtctCond = new SmtctInfo();
            
            gFamily = family;
            SmtctCond.id = int.Parse(this.hidDeleteID.Value);
            SmtctNew = getSmtctInfo(oldLine, family, ct, oprate, remark);

            iSMTLineSpeed.UpdateLotSMTLine(SmtctNew,SmtctCond);
            this.CmbMaintainLineForSMT.refresh();
            this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue = oldLine;
            lstSmtctInfo = iSMTLineSpeed.GetAllSMTLineItems(oldLine);

            bindTable(lstSmtctInfo, DEFAULT_ROWS);
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + id + "');", true);
            hideWait();
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
           // ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);

           // hideWait();
        }
    }
    protected void Add_ServerClick(Object sender, EventArgs e)
    {
        string msgHasExistRecord = GetLocalResourceObject(Pre + "_msgHasExistRecord").ToString();

        try
        {
            IList<SmtctInfo> lstSmtctInfo = null;
            string family = this.hidFamily2.Value;
            decimal ct = decimal.Parse(this.txtSpeed.Value.Trim());
            float oprate = float.Parse(this.txtRate.Value.Trim());
            string remark = this.txtOther.Value.Trim();

            string oldLine = this.hidFamily.Value;
            string id = this.hidDeleteID.Value;
            SmtctInfo Smtct = null;

            gFamily = family;
            //gMbCode = mbCode;
            //gECR = ecr;

            Smtct = getSmtctInfo(oldLine, family, ct, oprate, remark);

            iSMTLineSpeed.AddSMTLine(Smtct);
            this.CmbMaintainLineForSMT.refresh();
            this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue = oldLine;
            lstSmtctInfo = iSMTLineSpeed.GetAllSMTLineItems(oldLine);

            bindTable(lstSmtctInfo, DEFAULT_ROWS);
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + id + "');", true);
            hideWait();
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
            //ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);
            //hideWait();
        }
    }
    

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<SmtctInfo> lstSmtctInfo = null;
            SmtctInfo deleteItem = new SmtctInfo(); // getSmtctInfo(string.Empty, string.Empty, 0, 0, string.Empty);
            string oldFamily = this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue;//this.hidFamily.Value;
            //deleteItem.line = oldFamily;
            //deleteItem.family = this.hidMode.Value;

            deleteItem.id = int.Parse(this.hidDeleteID.Value);
            iSMTLineSpeed.RemoveSMTLine(deleteItem);
            this.CmbMaintainLineForSMT.refresh();
            this.CmbMaintainLineForSMT.InnerDropDownList.SelectedValue = oldFamily;
            lstSmtctInfo = iSMTLineSpeed.GetAllSMTLineItems(oldFamily);

            bindTable(lstSmtctInfo, DEFAULT_ROWS);
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
       // this.CmbMaintainFamilyForSMT.refresh();
       // this.CmbMaintainFamilyForSMT.InnerDropDownList.SelectedValue = family;
        scriptBuilder.AppendLine("document.getElementById('" + txtSpeed.ClientID + "').value='" + mbcode + "';");
        scriptBuilder.AppendLine("document.getElementById('" + txtRate.ClientID + "').value='" + ecr + "';");
        scriptBuilder.AppendLine("document.getElementById('" + txtOther.ClientID + "').value='" + iecV + "';");
  //      scriptBuilder.AppendLine("document.getElementById('" + txtCustVersion.ClientID + "').value='" + custV + "';");
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

    private SmtctInfo getSmtctInfo(string line, string mode, decimal rate, double speed, string remark)
    {
        SmtctInfo e = new SmtctInfo();

        e.line = line;
        e.family = mode;
        e.ct =  rate;
        e.optRate =  speed;
        e.remark = remark;
        e.editor = UserId;
       
        return e;
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
    }
}
