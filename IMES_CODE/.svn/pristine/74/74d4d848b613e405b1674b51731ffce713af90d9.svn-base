using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;

using System.Data;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class DataMaintain_Grade : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 31;
    private IGrade iGrade;
    private const int COL_NUM = 9;

    public  string pmtMessage1="";
    public  string pmtMessage2 = "";
    public  string pmtMessage3 = "";
    public  string pmtMessage4 = "";
    public  string pmtMessage5 = "";
    public  string pmtMessage6 = "";
    public  string pmtMessage7 = "";
    public  string pmtMessage8 = "";
    public  string pmtMessage9 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        iGrade = (IGrade)ServiceAgent.getInstance().GetMaintainObjectByName<IACAdaptor>(WebConstant.GradeMAITAIN);
        if(!IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //ITC-1361-0101  ITC210012  2012-02-21
            pmtMessage8 = this.GetLocalResourceObject(Pre+"_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre+"_pmtMessage9").ToString();

            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            List<GradeInfo> gradeLst=null;
            initLabel();
            try
            {
              gradeLst = (List<GradeInfo>)iGrade.GetAllGrades();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }
            
            bindTable(gradeLst, DEFAULT_ROWS);
        }

    }
    protected void btnAdd_ServerClick(object sender,EventArgs e)
    {
        GradeInfo grade = new GradeInfo();
        grade.family = this.ddlFamily.InnerDropDownList.SelectedValue;
        grade.grade = this.ddlGrade.InnerDropDownList.SelectedValue;
        grade.series = this.ttSeries.Text.Trim();
        grade.energia = this.ttEnergia.Text.Trim();
        grade.espera = this.ttEspera.Text.Trim();
        grade.editor = this.HiddenUserName.Value.Trim();
        string itemId="";
        try 
        {
            itemId = iGrade.AddSelectedGrade(grade);
        }
        catch(FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        showListByGradeList();
        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);
    }
    protected void btnFamilyChange_ServerClick(object sender, EventArgs e)
    {
        string family = this.cmbGradeFamilyTop.InnerDropDownList.SelectedValue.Trim();
        List<GradeInfo> gradeLst=new List<GradeInfo>();
        try 
        {
            gradeLst = (List<GradeInfo>)iGrade.GetGradesByFamily(family);
            
        }
        catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
        catch(Exception ee)
        {
            showErrorMessage(ee.Message);
            return;
        }
       if (gradeLst != null && gradeLst.Count != 0)
       {

           string familyId = replaceSpecialChart(family);

                //this.updatePanel1.Update();
                //this.updatePanel2.Update();
                //this.updatePanel3.Update();
           showListByGradeList();
           this.updatePanel2.Update();
           ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "HideWait();", true);

        }
        else
        {
            showListByGradeList();
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "NoMatchFamily();HideWait();", true);
        }
        //itc-1361-0007  itc210012 2012-1-10
       ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "clear", "clear();HideWait();", true);
    }
   
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string id = this.HiddenSelectedId.Value.Trim();
        try 
        {
            iGrade.DeleteSelectedGrade(Convert.ToInt32(id));
        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        showListByGradeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DeleteComplete();HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        //init oldGrade;
        string oldfamily = this.dOldFamilyId.Value.Trim();
        string oldseries = this.dOldSeries.Value.Trim();
        string oldgrade = this.dOldGrade.Value.Trim();
        GradeInfo oldGrade = new GradeInfo();
        oldGrade.family = oldfamily;
        oldGrade.series = oldseries;
        oldGrade.grade = oldgrade;
        //init newGrade
        string family = this.ddlFamily.InnerDropDownList.SelectedValue.Trim();
        string grade = this.ddlGrade.InnerDropDownList.SelectedValue.Trim();
        string series = this.ttSeries.Text.Trim();
        string energia = this.ttEnergia.Text.Trim();
        string espera = this.ttEspera.Text.Trim();
        GradeInfo newGrade = new GradeInfo();
        newGrade.family = family;
        newGrade.grade = grade;
        newGrade.series = series;
        newGrade.energia = energia;
        newGrade.espera = espera;
        newGrade.editor = this.HiddenUserName.Value.Trim(); 
        newGrade.id = Convert.ToInt32(this.HiddenSelectedId.Value.Trim());
        string itemId = this.HiddenSelectedId.Value.Trim();
        try
        {
             iGrade.UpdateSelectedGrade(newGrade);
        }
        catch(FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
        catch(Exception ee)
        {
            showErrorMessage(ee.Message);
            return;
        }
        showListByGradeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[8].Attributes.Add("style", e.Row.Cells[8].Attributes["style"] + "display:none");
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
   
    private void initLabel()
    {
        this.lblFamilyDdl.Text = this.GetLocalResourceObject(Pre + "_lblFamilyTop").ToString();
        this.lblGradeList.Text = this.GetLocalResourceObject(Pre + "_lblGradeList").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblGradeDdl.Text = this.GetLocalResourceObject(Pre + "_lblGrade").ToString();
        this.lblSeries.Text = this.GetLocalResourceObject(Pre + "_lblSeries").ToString();
        this.lblEnergia.Text = this.GetLocalResourceObject(Pre + "_lblEnergia").ToString();
        this.lblEspera.Text = this.GetLocalResourceObject(Pre + "_lblEspera").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();

    }
    private void bindTable(IList<GradeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblSeries").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblGrade").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEnergia").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEspera").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (GradeInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.family;
                dr[1] = temp.series;
                dr[2] = temp.grade;
                dr[3] = temp.energia;
                dr[4] = temp.espera;
                dr[5] = temp.editor;
                dr[6] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[8] = temp.id;
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
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true); 
    }
    private Boolean showListByGradeList()
    {
        string acadaptorlst = this.cmbGradeFamilyTop.InnerDropDownList.SelectedValue;
        IList<GradeInfo> gradeLst = null;
        try
        {
            if (acadaptorlst == "All")
            {
                gradeLst = iGrade.GetAllGrades();
            }
            else
            {
                gradeLst = iGrade.GetGradesByFamily(acadaptorlst);
            }

            if (gradeLst == null || gradeLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(gradeLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }
    
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(15);
 //       gd.HeaderRow.Cells[8].Width = Unit.Percentage(15);
    }
}
