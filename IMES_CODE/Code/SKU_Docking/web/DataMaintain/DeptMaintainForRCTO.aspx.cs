using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;


public partial class DataMaintain_DeptMaintainForRCTO : IMESBasePage
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IDept iDept;

    private const int COL_NUM = 9;

    public string strmsgSelectOne;
    public string strmsgDelConfirm;
    public string strmsgSelDept;
    public string strmsgSelSection;
    public string strmsgSelLine;
    public string strmsgSelFISLine;
    public string strmsgSelStart;
    public string strmsgSelEnd;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            userName = Master.userInfo.UserId;

            iDept = ServiceAgent.getInstance().GetMaintainObjectByName<IDept>(WebConstant.MaintainDeptObject);
            
            //注册Customer下拉框的选择事件
            this.cmbMaintainDept.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMaintainDept_Selected);
            this.cmbMaintainSection.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMaintainSection_Selected);


            if (!this.IsPostBack)
            {
                strmsgSelectOne = this.GetLocalResourceObject(Pre + "_MsgSelectOne").ToString();
                strmsgDelConfirm = this.GetLocalResourceObject(Pre + "_MsgDelConfirm").ToString();
                strmsgSelDept = this.GetLocalResourceObject(Pre + "_MsgSelDept").ToString();
                strmsgSelSection = this.GetLocalResourceObject(Pre + "_MsgSelSection").ToString();
                strmsgSelLine = this.GetLocalResourceObject(Pre + "_MsgSelLine").ToString();
                strmsgSelFISLine = this.GetLocalResourceObject(Pre + "_MsgSelFISLine").ToString();
                strmsgSelStart = this.GetLocalResourceObject(Pre + "_MsgSelStart").ToString();
                strmsgSelEnd = this.GetLocalResourceObject(Pre + "_MsgSelEnd").ToString();

                userName = Master.userInfo.UserName;
                //this.HiddenUserName.Value = userName;
                initLabel();
                ShowAllLineList();
               // bindTable(null, DEFAULT_ROWS);
            }
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

    protected void btnSectionChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowLineList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);

    }

    protected void btnDeptChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowLineList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);

    }

    /// <summary>
    /// 选择Dept下拉框，刷新Section
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbMaintainDept_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbMaintainSection.Dept = this.cmbMaintainDept.InnerDropDownList.SelectedValue;
            this.cmbMaintainSection.refresh();

            ShowLineList();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    /// <summary>
    /// 选择Section下拉框，会刷新Line下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbMaintainSection_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbMaintainSection.Dept = this.cmbMaintainDept.InnerDropDownList.SelectedValue;
            this.cmbMaintainSection.Section = this.cmbMaintainSection.InnerDropDownList.SelectedValue;

            ShowLineList();

        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }


    private Boolean ShowLineList()
    {
        string dept = this.cmbMaintainDept.InnerDropDownList.SelectedValue;
        String section = this.cmbMaintainSection.InnerDropDownList.SelectedValue;
        try
        {
            DeptInfo deptCondition = new DeptInfo();
            deptCondition.dept = dept;
            DeptInfo deptlikeCondition = new DeptInfo();
            deptlikeCondition.section = section + "%";

            List<DeptInfo> dataList = null;


            dataList = (List<DeptInfo>)iDept.GetLineList(deptCondition, deptlikeCondition);

            if (dataList == null)
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

        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();ShowRowEditInfo(null);", true);
        return true;

    }

    private Boolean ShowAllLineList()
    {
        try
        {
            IList<string> lstMaintainDept = null;

            lstMaintainDept = iDept.GetDeptList();

            DeptInfo dept = new DeptInfo();

            dept.dept = lstMaintainDept[0];
            List<DeptInfo> dataList = null;

            dataList = (List<DeptInfo>)iDept.GetDeptInfoList(dept);

            if (dataList == null)
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

        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();ShowRowEditInfo(null);", true);
        return true;
    }

    private void initLabel()
    {
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

        this.lblStart.Text = this.GetLocalResourceObject(Pre + "_lblStart").ToString();
        this.lblEnd.Text = this.GetLocalResourceObject(Pre + "_lblEnd").ToString();
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblFISLine.Text = this.GetLocalResourceObject(Pre + "_lblFISLine").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblDept.Text = this.GetLocalResourceObject(Pre + "_lblDept").ToString();
        this.lblSection.Text = this.GetLocalResourceObject(Pre + "_lblSection").ToString();
        this.lblLineList.Text = this.GetLocalResourceObject(Pre + "_lblLineList").ToString();
        /*
        this.txtStart.Text = "08:00:00";
        this.UpdatePanel1.Update();

        this.txtEnd.Text = "20:00:00";
        this.UpdatePanel2.Update();
        */

   }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(0);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(0);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        DeptInfo item = new DeptInfo();

        item.dept = this.cmbMaintainDept.InnerDropDownList.SelectedValue.Trim();
        item.section = this.cmbMaintainSection.InnerDropDownList.SelectedValue.Trim();

        item.line = this.txtLine.Text.Trim().ToUpper();
        item.fisline = this.txtFISLine.Text.Trim().ToUpper();
        item.startTime = this.txtStart.Text.Trim().ToUpper();
        item.endTime = this.txtEnd.Text.Trim();
        item.remark = this.txtRemark.Text.Trim();

        item.editor = userName;

        string oldItemDept = this.dOldDept.Value.Trim();
        string oldItemSection = this.dOldSection.Value.Trim();
        string oldItemLine = this.dOldLine.Value.Trim();

        DeptInfo olditem = new DeptInfo();
        olditem.dept = oldItemDept;
        olditem.section = oldItemSection;
        olditem.line = oldItemLine.ToUpper();
        string itemId = "";
        try
        {
            if ((oldItemDept == item.dept) && (oldItemSection == item.section) && (oldItemLine == item.line))
            {
                //UpdateDeptInfo(DeptInfo setValue, DeptInfo condition)
                itemId =this.dOldID.Value.Trim();
                iDept.UpdateDeptInfo(item, olditem);
            }
            else
            {
                //AddDeptInfo(DeptInfo item)
                itemId = iDept.AddDeptInfo(item);
            }
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
        ShowLineList();

        this.updatePanel.Update();

        String script = "<script language='javascript'> AddUpdateComplete('" + itemId + "'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "saveUpdate", script, false);

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');", true);
    }



    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string olddept = this.dOldDept.Value.Trim();
        string oldsection = this.dOldSection.Value.Trim();
        string oldline = this.dOldLine.Value.Trim();

        DeptInfo depttemp = new DeptInfo();
        depttemp.dept = olddept.ToUpper();
        depttemp.section = oldsection.ToUpper();
        depttemp.line = oldline.ToUpper();

        try
        {
            iDept.DeleteDeptInfo(depttemp);
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
        ShowLineList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(IList<DeptInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;


        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Dept").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Section").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Line").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_FISLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Start").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_End").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Remark").ToString());
        dt.Columns.Add("");
        dt.Columns.Add("");


        if (list != null && list.Count != 0)
        {
            foreach (DeptInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.dept;
                dr[1] = temp.section;
                dr[2] = temp.line;
                dr[3] = temp.fisline;
                dr[4] = temp.startTime;
                dr[5] = temp.endTime;
                if (temp.remark.Length > 15)
                {
                    dr[6] = temp.remark.Substring(0, 15) + "...";
                }
                else
                {
                    dr[6] = temp.remark;
                }
                dr[7] = temp.remark;
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
        e.Row.Cells[7].Attributes.Add("style", "display:none;");

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
                if (i == 6)
                {
                    e.Row.Cells[6].ToolTip = e.Row.Cells[7].Text;

                }
            }
        }
    }

}
