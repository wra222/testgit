using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;



public partial class DataMaintain_AssemblyVC : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IAssemblyVC iAssemblyVC; 

    private const int COL_NUM = 9;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iAssemblyVC = ServiceAgent.getInstance().GetMaintainObjectByName<IAssemblyVC>(WebConstant.MaintainAssemblyVCObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            this.cmbVC.SelectedIndexChanged += new EventHandler(btnVCChange_ServerClick);
            this.cmbCombineVC.SelectedIndexChanged += new EventHandler(btnCombineVCChange_ServerClick);
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initcmbVC();
                initcmbCombineVC();
                initcmbFamily();
            }
            ////bindTable(null, DEFAULT_ROWS);
            ShowListByCustomAndStage();
            //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
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

    protected void btnVCChange_ServerClick(object sender, System.EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "VCUpdate", "ShowWait();", true);
        initcmbCombineVC();
    }

    protected void btnCombineVCChange_ServerClick(object sender, System.EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "VCUpdate", "ShowWait();", true);
        btnQuery_ServerClick(sender, e);
    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        ShowListByCustomAndStage();
    }

    private void initcmbVC()
    {
        ListItem item = null;
        IList<AssemblyVCInfo> list = new List<AssemblyVCInfo>();
        IList<string> stringList = new List<string>();
        AssemblyVCInfo Condition = new AssemblyVCInfo();
        list = iAssemblyVC.GetAssemblyVCbyCondition(Condition);
        stringList = (from q in list
                      orderby q.vc
                      select q.vc).Distinct().ToList<string>();
        this.cmbVC.Items.Clear();
        this.cmbVC.Items.Add(new ListItem("ALL", "ALL"));
        foreach (string temp in stringList)
        {
            item = new ListItem(temp, temp);
            this.cmbVC.Items.Add(item);
        }
        this.cmbVC.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "VCUpdate", "DealHideWait();", true);
    }

    private void initcmbCombineVC()
    {
        ListItem item = null;
        IList<AssemblyVCInfo> list = new List<AssemblyVCInfo>();
        IList<string> stringList = new List<string>();
        AssemblyVCInfo Condition = new AssemblyVCInfo();
        if (this.cmbVC.SelectedValue != "ALL")
        {
            Condition.vc = this.cmbVC.SelectedValue.Trim();
        }
        list = iAssemblyVC.GetAssemblyVCbyCondition(Condition);

        stringList = (from q in list
                      orderby q.combineVC descending
                      select q.combineVC).Distinct().ToList<String>();



        this.cmbCombineVC.Items.Clear();
        this.cmbCombineVC.Items.Add(new ListItem("ALL", "ALL"));
        foreach (string temp in stringList)
        {
            item = new ListItem(temp, temp);
            this.cmbCombineVC.Items.Add(item);
        }
        this.cmbCombineVC.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "VCUpdate", "DealHideWait();", true);
        //ShowListByCustomAndStage();
    }

    private void initcmbFamily()
    {
        ListItem item = null;
        IList<string> list = new List<string>();
        list = iAssemblyVC.GetFamilyList();
        this.cmbFamily.Items.Clear();
        this.cmbFamily.Items.Add(string.Empty);
        foreach (string temp in list)
        {
            item = new ListItem(temp, temp);
            this.cmbFamily.Items.Add(item);
        }
        this.cmbCombineVC.SelectedIndex = 0;
    }

    private Boolean ShowListByCustomAndStage()
    {
        String vc = this.cmbVC.SelectedValue;
        String combineVC = this.cmbCombineVC.SelectedValue;
        AssemblyVCInfo Condition = new AssemblyVCInfo();
        IList<AssemblyVCInfo> list = new List<AssemblyVCInfo>();
        try
        {
            if (vc != "ALL")
            {
                Condition.vc = vc;
            }
            if (combineVC != "ALL")
            {
                Condition.combineVC = combineVC;
            }
            list = iAssemblyVC.GetAssemblyVCbyCondition(Condition);

            DataTable dataList = ConvertToDataTable(list);

            if (dataList == null || dataList.Rows.Count == 0)
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
        finally
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "VCUpdate", "DealHideWait();", true);
        }
        return true;
    }

    private DataTable ConvertToDataTable(IList<AssemblyVCInfo> list)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("VC");
        dt.Columns.Add("PartNO");
        dt.Columns.Add("CombineVC");
        dt.Columns.Add("CombinePartNO");
        dt.Columns.Add("Family");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");

        if (list != null && list.Count != 0)
        {
            foreach (AssemblyVCInfo item in list)
            {
                dr = dt.NewRow();
                dr[0] = item.vc;
                dr[1] = item.partNo;
                dr[2] = item.combineVC;
                dr[3] = item.combinePartNo;
                dr[4] = item.family;
                dr[5] = item.remark;
                dr[6] = item.editor;
                dr[7] = item.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[8] = item.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[9] = item.id;
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private void initLabel()
    {
        this.lblVCTop.Text = this.GetLocalResourceObject(Pre + "_lblVCTop").ToString();
        this.lblCombineVCTop.Text = this.GetLocalResourceObject(Pre + "_lblCombineVCTop").ToString();
        this.lblVC.Text = this.GetLocalResourceObject(Pre + "_lblVC").ToString();
        this.lblPartNO.Text = this.GetLocalResourceObject(Pre + "_lblPartNO").ToString();
        this.lblCombineVC.Text = this.GetLocalResourceObject(Pre + "_lblCombineVC").ToString();
        this.lblCombinePartNO.Text = this.GetLocalResourceObject(Pre + "_lblCombinePartNO").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(1);
       
    }


   


    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        AssemblyVCInfo item = new AssemblyVCInfo();
        item.vc = this.txtVC.Value.Trim();
        item.partNo = this.hidAddInsertPartNO.Value.Trim();
        item.combineVC = this.txtCombineVC.Value.Trim();
        item.combinePartNo = this.hidAddInsertCombinePartNO.Value.Trim();
        item.family = this.cmbFamily.SelectedValue.Trim();
        item.remark = this.txtRemark.Value.Trim();
        item.editor = this.HiddenUserName.Value;
        item.udt = DateTime.Now;

        string oldItemId = this.dOldId.Value.Trim();
        try
        {
            if (oldItemId != "")
            {
                item.id = Convert.ToInt64(oldItemId);
                iAssemblyVC.UpdateAssemblyVC(item);
            }
            else
            {
                item.cdt = DateTime.Now;
                iAssemblyVC.InsertAssemblyVC(item);
            }
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
        ShowListByCustomAndStage();

        
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
    }

  

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string oldId = this.dOldId.Value.Trim();
        try
        {
            iAssemblyVC.DeleteAssemblyVC(oldId);
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
        ShowListByCustomAndStage();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(DataTable  list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("VC");
        dt.Columns.Add("PartNO");
        dt.Columns.Add("CombineVC");
        dt.Columns.Add("CombinePartNO");
        dt.Columns.Add("Family");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                 for (int j = 0; j < list.Rows[i].ItemArray.Count(); j++)
                {
                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }
                }
                dt.Rows.Add(dr);
            }
            
            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
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

}
