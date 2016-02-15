
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
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;

public partial class DataMaintain_DefectHoldRule : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public String UserId;
    public String Customer;
    private IDefectHoldRule iDefectHoldRule = null;
    
    private bool gHasHightRow = false;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            this.HiddenUserName.Value = UserId;
            iDefectHoldRule = ServiceAgent.getInstance().GetMaintainObjectByName<IDefectHoldRule>(WebConstant.DefectHoldRuleMaintainObject);
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_msgInput").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                initLabel();
                initlineTop();
                initline();
                initHoldStation();
                initHoldCode(); 
                initCheckInStation();
                bindTable(null, DEFAULT_ROWS);
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
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }

    private void initLabel()
    {
        this.btnQuery.Value=this.GetLocalResourceObject(Pre+"_btnQuery").ToString();
        this.lblMasterLabelList.Text=this.GetLocalResourceObject(Pre+"_lblMasterLabelList").ToString();
        this.btnDelete.Value=this.GetLocalResourceObject(Pre+"_btnDelete").ToString();
        this.btnSave.Value=this.GetLocalResourceObject(Pre+"_btnSave").ToString();
    }

    private void initlineTop()
    {
        IList<string> list = iDefectHoldRule.GetLineTop();
        this.cmbLineTop.Items.Clear();
        this.cmbLineTop.Items.Add(new ListItem("ALL",""));
        foreach (string items in list)
        {
            if (items == "")
            {
                continue;
            }
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbLineTop.Items.Add(selectListItem);
        }
    }

    private void initline()
    {
        IList<string> list = iDefectHoldRule.GetLine();
        this.cmbLine.Items.Clear();
        this.cmbLine.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbLine.Items.Add(selectListItem);
        }
    }

    private void initCheckInStation()
    {
        IList<string> list = iDefectHoldRule.GetCheckInStation();
        this.cmbCheckINStation.Items.Clear();
        this.cmbCheckINStation.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbCheckINStation.Items.Add(selectListItem);
        }
    }

    private void initHoldStation()
    {
        IList<string> list = iDefectHoldRule.GetHoldStation();
        this.cmbHoldStation.Items.Clear();
        this.cmbHoldStation.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbHoldStation.Items.Add(selectListItem);
        }
    }

    private void initHoldCode()
    {
        IList<string> list = iDefectHoldRule.GetHoldCode();
        this.cmbHoldCode.Items.Clear();
        this.cmbHoldCode.Items.Add(new ListItem("", ""));
        foreach (string items in list)
        {
            ListItem selectListItem = new ListItem();
            selectListItem.Text = items;
            selectListItem.Value = items;
            this.cmbHoldCode.Items.Add(selectListItem);
        }
    }

    private void bindTable(IList<DefectHoldRuleInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ID");//0
        dt.Columns.Add("Line");
        dt.Columns.Add("Family/Model");
        dt.Columns.Add("Defect");
        dt.Columns.Add("ExceptCause");
        dt.Columns.Add("EqualSameDefectCount");
        dt.Columns.Add("OverDefectCount");
        dt.Columns.Add("CheckINStation");
        dt.Columns.Add("HoldStation");
        dt.Columns.Add("HoldCode");
        dt.Columns.Add("HoldDescr");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");//13

        if (list != null && list.Count != 0)
        {
            foreach (DefectHoldRuleInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.Line;
                dr[2] = temp.Family;
                dr[3] = temp.DefectCode;
                dr[4] = temp.ExceptCause;
                dr[5] = temp.EqualSameDefectCount;
                dr[6] = temp.OverDefectCount;
                dr[7] = temp.CheckInStation;
                dr[8] = temp.HoldStation;
                dr[9] = temp.HoldCode;
                dr[10] = temp.HoldDescr;
                dr[11] = temp.Editor;
                dr[12] = temp.Cdt;
                dr[13] = temp.Udt;
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
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
        }
    }

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string line = this.cmbLineTop.SelectedItem.Value.ToString();
            string familyandmodel = this.txtFamilyAndModelTop.Text.Trim();
            string defect = this.txtDefectTop.Text.Trim();
            string exceptcause = this.txtExceptCause.Text.Trim();
            DefectHoldRuleInfo condition = new DefectHoldRuleInfo();
            if (line != "")
            {
                condition.Line = line; 
            }
            if (familyandmodel != "")
            {
                condition.Family = familyandmodel;
            }
            if (defect != "")
            {
                condition.DefectCode = defect;
            }
            if (exceptcause != "")
            {
                condition.ExceptCause = exceptcause;
            }
            IList<DefectHoldRuleInfo> defectHoldRuleList = iDefectHoldRule.GetDefectHoldRule(condition);
            bindTable(defectHoldRuleList, DEFAULT_ROWS);
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
        try
        {
            IList<DefectHoldRuleInfo> defectHoldRuleList = null;
            string line = this.cmbLine.SelectedItem.Value.ToString();
            string familyandmodel = this.txtFamilyAndModel.Text.ToString().Trim().ToUpper();
            string defect = this.txtDefect.Text.ToString().Trim().ToUpper();
            string exceptcause = this.txtExceptCause.Text.ToString().Trim();
            string strequalsameDefectcount = this.txtEqualSameDefectCount.Text.ToString().Trim();
            string stroverdefectcount = this.txtOverDefectCount.Text.ToString().Trim();
            string checkinstation = this.cmbCheckINStation.SelectedItem.Value.ToString();
            string holdstation = this.cmbHoldStation.SelectedItem.Value.ToString();
            string holdcode = this.cmbHoldCode.SelectedItem.Value.ToString();
            string holdDescr = this.txtHoldDescr.Text.ToString().Trim().ToUpper();
            string id = this.hidDeleteID.Value;
            if (!string.IsNullOrEmpty(familyandmodel))
            {
                if (!iDefectHoldRule.CheckFamilyAndModel(familyandmodel))
                {
                    throw new FisException("Input Fmaily/Model is not found");
                }
            }
            int equalsameDefectcount = 0;
            int overdefectcount = 0;
            if(strequalsameDefectcount != "")
            {
                equalsameDefectcount = int.Parse(strequalsameDefectcount);
            }
            if(stroverdefectcount != "")
            {
                overdefectcount = int.Parse(stroverdefectcount);
            }
            DefectHoldRuleInfo defectHoldInfo = new DefectHoldRuleInfo();
            defectHoldInfo.Line = line;
            defectHoldInfo.Family = familyandmodel;
            defectHoldInfo.DefectCode = defect;
            defectHoldInfo.ExceptCause = exceptcause;
            defectHoldInfo.EqualSameDefectCount = equalsameDefectcount;
            defectHoldInfo.OverDefectCount = overdefectcount;
            defectHoldInfo.CheckInStation = checkinstation;
            defectHoldInfo.HoldStation = holdstation;
            defectHoldInfo.HoldCode = holdcode;
            defectHoldInfo.HoldDescr = holdDescr;
            defectHoldInfo.Editor = UserId;
            defectHoldInfo.Cdt = DateTime.Now;
            defectHoldInfo.Udt = DateTime.Now;

            if (!string.IsNullOrEmpty(id))
            {
                defectHoldInfo.ID = int.Parse(id);
                iDefectHoldRule.UpdateDefectHoldRule(defectHoldInfo);
            }
            else
            {
                iDefectHoldRule.InsertDefectHoldRule(defectHoldInfo);
            }
            defectHoldRuleList = iDefectHoldRule.GetDefectHoldRule(new DefectHoldRuleInfo{ Line = line,
                                                                                           Family = familyandmodel,
                                                                                           DefectCode = defect,
                                                                                           ExceptCause = exceptcause,
                                                                                           EqualSameDefectCount = equalsameDefectcount,
                                                                                           OverDefectCount = overdefectcount,
                                                                                           CheckInStation = checkinstation});
            int ID = defectHoldRuleList[0].ID;
            bindTable(defectHoldRuleList, DEFAULT_ROWS);
            initlineTop();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + ID + "');", true);//
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
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

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            IList<DefectHoldRuleInfo> defectHoldRuleList = null;
            string id = this.hidDeleteID.Value.ToString();
            if (string.IsNullOrEmpty(id))
            {
                throw new FisException("This ID is not Exists");
            }
            int delID = int.Parse(id);
            iDefectHoldRule.DeleteDefectHoldRule(delID);

            defectHoldRuleList = iDefectHoldRule.GetDefectHoldRule(new DefectHoldRuleInfo { Line = this.cmbLineTop.SelectedItem.Value.ToString(),
                                                                                            Family = this.txtFamilyAndModelTop.Text.Trim(),
                                                                                            DefectCode = this.txtDefectTop.Text.Trim() });



            bindTable(defectHoldRuleList, DEFAULT_ROWS);
            initlineTop();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Delete", "DeleteComplete();", true);
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
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
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

    private void changeSelectedIndex(string index, string family, string vc,string code)
    {
        
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        
        scriptBuilder.AppendLine("HideWait();");
        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(2);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(4);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(13);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[7].Width = Unit.Pixel(9);
        gd.HeaderRow.Cells[8].Width = Unit.Pixel(7);
        gd.HeaderRow.Cells[9].Width = Unit.Pixel(6);
        gd.HeaderRow.Cells[10].Width = Unit.Pixel(6);
        gd.HeaderRow.Cells[11].Width = Unit.Pixel(3);
        gd.HeaderRow.Cells[12].Width = Unit.Pixel(15);
        gd.HeaderRow.Cells[13].Width = Unit.Pixel(15);
    }
    
}
