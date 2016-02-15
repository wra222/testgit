/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for APT maintain Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;

public partial class DataMaintain_APT : System.Web.UI.Page
{
    public String UserId;

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string SUCCESSRET = WebConstant.SUCCESSRET;
    private const int DEFAULT_ROWS = 16;
    private static IActualProductionTime iActualProductionTime = (IActualProductionTime)ServiceAgent.getInstance().GetMaintainObjectByName<IActualProductionTime>(WebConstant.MaintainActualProductionTimeObject);
    private IList<DeptInfo> deptInfoList = new List<DeptInfo>();
    private static IList<ConstValueInfo> causeList = iActualProductionTime.GetProductionCauseList();

    [WebMethod]
    public static ArrayList GetPageInfo()
    {
        ArrayList ret = new ArrayList();

        try
        {
            IList<DeptInfo> retLst1 = iActualProductionTime.GetDeptInfoList();
            if (retLst1.Count <= 0)
            {
                throw new Exception("Please check the data in table [Dept]!");
            }

            IList<string> lineList = new List<string>();
            foreach (DeptInfo ele in retLst1)
            {
                if (!lineList.Contains(ele.line))
                {
                    lineList.Add(ele.line);
                }
            }
            IList<SMTLineDef> retLst2 = iActualProductionTime.GetSMTLineInfoListByLineList(lineList);
            if (retLst2.Count <= 0)
            {
                throw new Exception("Please check the data in table [SMTLine]!");
            }

            IList<ConstValueInfo> retLst3 = iActualProductionTime.GetProductionCauseList();
            if (retLst3.Count <= 0)
            {
                throw new Exception("Please check the data with field value [Type='SMTCauseTable'] in table [ConstValue]!");
            }
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(retLst1);
            ret.Add(retLst2);
            ret.Add(retLst3);
            return ret;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UserId = Master.userInfo.UserId;
        btnProcess.ServerClick += new EventHandler(btnProcess_ServerClick);
        if (!this.IsPostBack)
        {
            initLabel();
            showList(false);
            this.updatePanel2.Update();      
        }
        try
        {
            deptInfoList = iActualProductionTime.GetDeptInfoList();
            IList<string> lineList = new List<string>();
            foreach (DeptInfo ele in deptInfoList)
            {
                if (!lineList.Contains(ele.line))
                {
                    lineList.Add(ele.line);
                }
            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void btnProcess_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            showList(true);
            this.updatePanel2.Update();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
        }
    }

    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        DateTime date = Convert.ToDateTime(hidDate.Value);
        string line = this.hidSelectedId.Value.Trim();
        try
        {
            SmttimeInfo cond = new SmttimeInfo();
            cond.date = date;
            cond.line = line;
            iActualProductionTime.DeleteSMTTimeInfo(cond);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        showList(true);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "delete", "resetTableHeight();showEmptyContent();HideWait();", true);
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        DateTime date = Convert.ToDateTime(hidDate.Value);
        string content = hidContent.Value;
        IList<string> dataList = content.Split('\u0003');

        SmttimeInfo cond = new SmttimeInfo();
        cond.date = date;
        cond.line = dataList[0];

        SmttimeInfo item = new SmttimeInfo();
        item.date = date;
        item.line = dataList[0];
        item.actTime = decimal.Parse(dataList[1]);
        item.cause = dataList[2];
        item.actTime1 = decimal.Parse(dataList[3]);
        item.cause1 = dataList[4];
        item.actTime2 = decimal.Parse(dataList[5]);
        item.cause2 = dataList[6];
        item.actTime3 = decimal.Parse(dataList[7]);
        item.cause3 = dataList[8];
        item.remark = dataList[9];
        item.editor = UserId;

        try
        {
            if (iActualProductionTime.CheckExistSMTTimeInfo(cond))
            {
                iActualProductionTime.UpdateSMTTimeInfo(cond, item);
            }
            else
            {
                iActualProductionTime.AddSMTTimeInfo(item);
            }
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        showList(true);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + cond.line + "');HideWait();", true);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Add Tip
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < 11; i++)
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
        this.lblPageTitle.Text = this.GetLocalResourceObject(Pre + "_lblPageTitle").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblLine1.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.lblLine2.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblAPT.Text = this.GetLocalResourceObject(Pre + "_lblAPT").ToString();
        this.lblHr.Text = this.GetLocalResourceObject(Pre + "_lblHour").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblOLT1.Text = this.GetLocalResourceObject(Pre + "_lblOLT1").ToString();
        this.lblHr1.Text = this.GetLocalResourceObject(Pre + "_lblHour").ToString();
        this.lblOLT2.Text = this.GetLocalResourceObject(Pre + "_lblOLT2").ToString();
        this.lblHr2.Text = this.GetLocalResourceObject(Pre + "_lblHour").ToString();
        this.lblOLT3.Text = this.GetLocalResourceObject(Pre + "_lblOLT3").ToString();
        this.lblHr3.Text = this.GetLocalResourceObject(Pre + "_lblHour").ToString();
        this.lblCause1.Text = this.GetLocalResourceObject(Pre + "_lblCause1").ToString();
        this.lblCause2.Text = this.GetLocalResourceObject(Pre + "_lblCause2").ToString();
        this.lblCause3.Text = this.GetLocalResourceObject(Pre + "_lblCause3").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private string getLineDescr(string line)
    {
        foreach (DeptInfo ele in deptInfoList)
        {
            if (ele.line == line)
            {
                return ele.remark;
            }
        }
        return "";
    }

    private string getCauseStrInTable(string cause)
    {
        if (cause == "")
        {
            return "----";
        }

        foreach (ConstValueInfo ele in causeList)
        {
            if (ele.name == cause)
            {
                return ele.name + ":" + ele.value;
            }
        }

        return cause;
    }

    private void bindTable(IList<SmttimeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLineName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colAPT").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colOLT1").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause1").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colOLT2").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause2").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colOLT3").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause3").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        if (list != null && list.Count != 0)
        {
            foreach (SmttimeInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.line;
                dr[1] = getLineDescr(temp.line);
                dr[2] = float.Parse(temp.actTime.ToString());
                dr[3] = getCauseStrInTable(temp.cause);
                dr[4] = float.Parse(temp.actTime1.ToString());
                dr[5] = getCauseStrInTable(temp.cause1);
                dr[6] = float.Parse(temp.actTime2.ToString());
                dr[7] = getCauseStrInTable(temp.cause2);
                dr[8] = float.Parse(temp.actTime3.ToString());
                dr[9] = getCauseStrInTable(temp.cause3);
                dr[10] = temp.remark;
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

            this.hidRecordCount.Value = "0";
        }
        gd.GvExtHeight = hidTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "selectedRowIndex = -1;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(15);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }

    private void hideWait()
    {
        String script = "<script language='javascript'> HideWait(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "hideWait", script, false);
    }

    private void showList(bool bNeedData)
    {
        IList<SmttimeInfo> dataLst = null;
        try
        {
            if (bNeedData)
            {
                DateTime date = Convert.ToDateTime(hidDate.Value);
                dataLst = iActualProductionTime.GetSMTTimeInfoList(date);
            }

            if (dataLst == null || dataLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
        }
        catch (System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
        }
    }
}
