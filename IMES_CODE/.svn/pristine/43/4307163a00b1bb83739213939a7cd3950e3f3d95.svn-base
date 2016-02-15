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

public partial class DataMaintain_QTime : System.Web.UI.Page
{
    private const int DEFAULT_ROWS = 32;
    public string UserId="";
    public String Customer;
    private IQTime iQTime = ServiceAgent.getInstance().GetMaintainObjectByName<IQTime>(WebConstant.IQTime);
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
        initLineSelect();
        initLine2Select();
        initStationSelect();
        initDefectCodeSelect();
        initHoldStationSelect();
    }

    private void initLineSelect()
    {
        IList<string> lstLine = iQTime.GetAliasLineList();
        this.cmbLine.Items.Clear();
        this.cmbLine.Items.Add(string.Empty);
        if (lstLine.Count != 0)
        {
            foreach (string item in lstLine)
            {
                this.cmbLine.Items.Add(item);
            }
            this.cmbLine.SelectedIndex = 0;
        }
    }

    private void initLine2Select()
    {
        IList<string> lstLine2 = iQTime.GetAliasLineList();
        this.cmbLine2.Items.Clear();
        if (lstLine2.Count != 0)
        {
            foreach (string item in lstLine2)
            {
                this.cmbLine2.Items.Add(item);
            }
            this.cmbLine2.SelectedIndex = 0;
        }
    }

    private void initStationSelect()
    {
        IList<ConstValueTypeInfo> lstStation = iQTime.GetStationList();
        this.cmbStation.Items.Clear();
        if (lstStation.Count != 0)
        {
            foreach (ConstValueTypeInfo item in lstStation)
            {
                this.cmbStation.Items.Add(item.value);
            }
            this.cmbStation.SelectedIndex = 0;
        }
    }

    private void initDefectCodeSelect()
    {
        IList<DefectCodeInfo> lstDefectCode = iQTime.GetDefectCodeList();
        this.cmbDefectCode.Items.Clear();
        if (lstDefectCode.Count != 0)
        {
            this.cmbDefectCode.Items.Add(string.Empty);
            foreach (DefectCodeInfo item in lstDefectCode)
            {
                ListItem items = new ListItem();
                items.Text = item.Defect.Trim() + " - " + item.Descr.Trim();
                items.Value = item.Defect.Trim();
                this.cmbDefectCode.Items.Add(items);
            }
            this.cmbDefectCode.SelectedIndex = 0;
        }
    }

    private void initHoldStationSelect()
    {
        DataTable dtHoldStation = iQTime.GetHoldStationList();
        this.cmbHoldStation.Items.Clear();
        if (dtHoldStation.Rows.Count != 0 || dtHoldStation != null)
        {
            this.cmbHoldStation.Items.Add(string.Empty);
            foreach (DataRow dr in dtHoldStation.Rows)
            {
                this.cmbHoldStation.Items.Add(dr[0].ToString().Trim());
            }
            this.cmbHoldStation.SelectedIndex = 0;
        }
    }

    private void bindTable(IList<QTimeinfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Line");
        dt.Columns.Add("Station");
        dt.Columns.Add("Family");
        dt.Columns.Add("Catagory");
        dt.Columns.Add("TimeOut");
        dt.Columns.Add("StopTime");
        dt.Columns.Add("DefectCode");
        dt.Columns.Add("HoldStation");
        dt.Columns.Add("HoldStatus");
        dt.Columns.Add("Exceptstation");
        dt.Columns.Add("Edtior");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        
        if (list != null && list.Count != 0)
        {
            foreach (QTimeinfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Line;
                dr[1] = temp.Station;
                dr[2] = temp.Family;
                dr[3] = temp.Catagory;
                dr[4] = temp.TimeOut;
                dr[5] = temp.StopTime;
                dr[6] = temp.DefectCode;
                dr[7] = temp.HoldStation;
                dr[8] = temp.HoldStatus;
                dr[9] = temp.ExceptStation;
                dr[10] = temp.Editor;
                dr[11] = (temp.Cdt == null ? string.Empty : temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[12] = (temp.Udt == null ? string.Empty : temp.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
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

    protected void cmbLine_Selected(object sender, EventArgs e)
    {
        try
        {
            string Line = this.cmbLine.SelectedItem.ToString();
            IList<QTimeinfo> QTimeinfo = iQTime.GetQTimeList(Line);
            bindTable(QTimeinfo, DEFAULT_ROWS);
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
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
            if (this.txtFamily.Value.Trim() != "" && !iQTime.CheckFamily(this.txtFamily.Value.Trim()))
            {
                throw new Exception("Can't find the matching [Family]! ");
            }
            QTimeinfo item = new QTimeinfo();
            string Line2 = this.hidLine2.Value.Trim();
            string Station = this.hidStation.Value.Trim();
            string Family = this.txtFamily.Value.Trim();
            item = iQTime.CheckExistDefectCodeList(Line2, Station, Family);
            if (item.Line == "" || item.Line == null)
            {
                item = SetValue();
                iQTime.Add(item);
            }
            else
            {
                item = SetValue();
                iQTime.Update(item);
            }
            string Line = this.hidLine.Value;
            IList<QTimeinfo> QTimeinfo = iQTime.GetQTimeList(Line);
            bindTable(QTimeinfo, DEFAULT_ROWS);    
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            return;
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "resetTableHeight", "resetTableHeight();", true);
            hideWait();
        }
    }

    public QTimeinfo SetValue()
    {
        QTimeinfo item = new QTimeinfo();
        item.Line = this.hidLine2.Value;
        item.Station = this.hidStation.Value;
        item.Family = this.txtFamily.Value.Trim();
        item.Catagory = this.hidCatagory.Value;
        item.TimeOut = Convert.ToInt32(this.txtTimeOut.Value.Trim());
        item.StopTime = Convert.ToInt32(this.txtStopTime.Value.Trim());
        item.DefectCode = this.hidDefectCode.Value;
        item.HoldStation = this.hidHoldStation.Value;
        item.HoldStatus = this.hidHoldStatus.Value;
        item.ExceptStation = this.txtExceptstation.Value.Trim();
        item.Editor = UserId;
        return item;
    }

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string Line2 = this.hidLine2.Value.Trim();
            string Station = this.hidStation.Value.Trim();
            string Family = this.txtFamily.Value.Trim();
            iQTime.Delete(Line2, Station, Family);
            string Line = this.hidLine2.Value;
            IList<QTimeinfo> QTimeinfo = iQTime.GetQTimeList(Line);
            bindTable(QTimeinfo, DEFAULT_ROWS);   
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

    private void changeSelectedIndex(string index)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("selectedRowIndex=" + index + ";");
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
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(14);
    }
}
