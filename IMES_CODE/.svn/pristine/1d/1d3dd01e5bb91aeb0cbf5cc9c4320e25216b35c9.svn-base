using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

public partial class DataMaintain_SQEDefectReportMaitain : System.Web.UI.Page
{
    private string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    protected string pmtMessage1;
    protected string pmtMessage2;
    protected string pmtMessage3;
    protected string pmtMessage4;
    protected string pmtMessage5;
    protected string pmtMessage6;
    protected string pmtMessage7;
    protected string pmtMessage8;
    protected string pmtMessage9;
    protected string pmtMessage10;
    protected string pmtMessage11;
    protected string pmtMessage12;
    private const int DEFAULT_ROWS = 36;
    ISQEDefectReport SQEDefectReport = (ISQEDefectReport)ServiceAgent.getInstance().GetMaintainObjectByName<ISQEDefectReport>(WebConstant.SQEDefectReportManager);
    private const int COL_NUM = 5;
    private string username;
    protected void Page_Load(object sender, EventArgs e)
    {

        pmtMessage1 = this.GetLocalResourceObject(pre + "_pmtMessage1").ToString();
        pmtMessage2 = this.GetLocalResourceObject(pre + "_pmtMessage2").ToString();
        pmtMessage3 = this.GetLocalResourceObject(pre + "_pmtMessage3").ToString();
        pmtMessage4 = this.GetLocalResourceObject(pre + "_pmtMessage4").ToString();
        pmtMessage5 = this.GetLocalResourceObject(pre + "_pmtMessage5").ToString();
        pmtMessage6 = this.GetLocalResourceObject(pre + "_pmtMessage6").ToString();
        pmtMessage7 = this.GetLocalResourceObject(pre + "_pmtMessage7").ToString();
        pmtMessage8 = this.GetLocalResourceObject(pre + "_pmtMessage8").ToString();
        pmtMessage9 = this.GetLocalResourceObject(pre + "_pmtMessage9").ToString();
        pmtMessage10 = this.GetLocalResourceObject(pre + "_pmtMessage10").ToString();
        pmtMessage11 = this.GetLocalResourceObject(pre + "_pmtMessage11").ToString();
        pmtMessage12 = this.GetLocalResourceObject(pre + "_pmtMessage12").ToString();
        username = Master.userInfo.UserId;
        this.hiddenUserName.Value = username;
        try
        {
            if (!IsPostBack)
            {
                initDropLists();
                initLabel();
                bindTable(null, DEFAULT_ROWS);
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
    }


    #region 数据的增删改查
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string ctLabel = this.dCTNO.Text;

        if (ctLabel.Trim() != "" && ctLabel.Trim().Length == 14)
        {
            this.cmbiecpn.CtNo = ctLabel;
            this.cmbiecpn.ClearContent();
            this.cmbiecpn.refresh(ctLabel);
        }
        else
        {
            this.cmbiecpn.ClearContent();
            this.cmbiecpn.refresh();
        }
        
        string defect = this.drpDefect.InnerDropDownList.SelectedValue;
        string cause = this.drpCause.InnerDropDownList.SelectedValue;
        string model = this.dModelPartNo.Text;
        string tp = this.radioButtonGroup.Value;
        try
        {
            //添加数据
            //tp="MP->SQE"或者tp="PE"时的处理情况
            if (ridoMpToSQE.Checked == true)
            {

                CheckAndSaveIqcKp("MP", ctLabel, defect);
            }
            else if (ridoPE.Checked == true)
            {
                CheckAndSaveIqcKpSQE("PE", ctLabel, defect, cause);
            }
            else if (ridoSQE.Checked == true)
            {
                CheckAndSaveIqcKpSQE(tp, ctLabel, defect, cause);
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


    }


    /// <summary>
    /// tp="MP->SQE"的处理情况
    /// </summary>
    /// <param name="Tp"></param>
    /// <param name="CTNO"></param>
    /// <param name="Defect"></param>
    protected void CheckAndSaveIqcKp(string Tp, string CTNO, string Defect)
    {
        IList<IqcKpDef> lstIqcKp = new List<IqcKpDef>();
        lstIqcKp = SQEDefectReport.GetIqcKpByTypeCtLabelAndDefect(Tp, CTNO, Defect);
        string ctNo = this.dCTNO.Text;
        string defect = this.drpDefect.InnerDropDownList.SelectedValue;
        IqcKpDef iqckp = new IqcKpDef();
        iqckp.CtLabel = this.dCTNO.Text;
        iqckp.Model = this.dModelPartNo.Text;
        //add location
        //iqckp.Location = this.drpMajorPart.InnerDropDownList.SelectedValue + " " + this.drpCommponent.InnerDropDownList.SelectedValue;
        iqckp.Editor = this.hiddenUserName.Value;
        if (lstIqcKp == null || lstIqcKp.Count <= 0)
        {
            iqckp.Udt = DateTime.Now;
            iqckp.Defect = defect;
            iqckp.Cdt = DateTime.Now;
            iqckp.Tp = Tp;
            if (Tp == "MP")
            {
                iqckp.Obligation = "";
                iqckp.Cause = "";
                iqckp.Location = "";
                iqckp.Result = "";
                iqckp.Remark = "";
                iqckp.Location = "";
            }
            //创建一条新数据
            SQEDefectReport.AddIqcKp(iqckp);
            this.updatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();alert('" + pmtMessage3.Trim() + "');DealHideWait();", true);
        }
        else
        {
            //不存在此数据时候不做处理
            this.updatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();DealHideWait();", true);
        }

    }

    /// <summary>
    /// tp="SQE"或者tp="PE"时的处理情况
    /// </summary>
    /// <param name="Tp"></param>
    /// <param name="CTNO"></param>
    /// <param name="Defect"></param>
    /// <param name="Cause"></param>
    protected void CheckAndSaveIqcKpSQE(string Tp, string CTNO, string Defect, string Cause)
    {
        IList<IqcKpDef> lstIqcKp = new List<IqcKpDef>();
        IList<IqcKpDef> lstIqcKpPE = new List<IqcKpDef>();
        //第一次查询SQE
        lstIqcKp = SQEDefectReport.GetIqcKpByTypeCtLabelAndDefect("MP", CTNO, Defect);
        lstIqcKpPE = SQEDefectReport.GetIqcKpByTypeCtLabelAndDefect("PE", CTNO, Defect, Cause);

        IqcKpDef iqckp = new IqcKpDef();
        iqckp.CtLabel = CTNO;
        iqckp.Cause = this.drpCause.InnerDropDownList.SelectedValue;
        iqckp.Defect = Defect;
        iqckp.Model = this.dModelPartNo.Text;
        iqckp.Obligation = this.drpObligation.InnerDropDownList.SelectedValue;
        iqckp.Remark = this.dRemark.Text;
        iqckp.Result = this.dResult.Text;
        iqckp.Editor = this.hiddenUserName.Value;

        //iqckp.Location = "";
        //add location
        iqckp.Location=this.drpMajorPart.InnerDropDownList.SelectedValue + " " + this.drpCommponent.InnerDropDownList.SelectedValue;
        iqckp.Udt = DateTime.Now;

        //tp="SQE"的情况
        if (Tp == "SQE")
        {
            if (lstIqcKp != null && lstIqcKp.Count > 0)
            {
                //第二次查询SQE
                IList<IqcKpDef> lstIqcKp1 = new List<IqcKpDef>();
                lstIqcKp1 = SQEDefectReport.GetIqcKpByTypeCtLabelAndDefect("IQC", CTNO, Defect, Cause);
                iqckp.Tp = "IQC";
                if (lstIqcKp1 == null)
                {
                    //创建一条新的数据
                    iqckp.Cdt = DateTime.Now;
                    SQEDefectReport.AddIqcKp(iqckp);
                    this.updatePanel1.Update();
                    ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();DealHideWait();alert('" + pmtMessage3.Trim() + "');", true);
                    return;
                }
                else
                {
                    //修改已经存在的数据
                    SQEDefectReport.UpdateIqcKp(iqckp, "IQC", CTNO, Defect);
                    this.updatePanel1.Update();
                    ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();DealHideWait();alert('" + pmtMessage4.Trim() + "');", true);
                }
            }
            else
            {   //MP未退SQE,不能Save
                this.updatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();DealHideWait();alert('" + pmtMessage5.Trim() + "');", true);
                return;
            }
        }

        if (Tp == "PE")
        {
            //TP="PE"的情况
            if (lstIqcKpPE == null)
            {
                //添加一条TP="PE"的情况
                iqckp.Tp = "PE";
                iqckp.Cdt = DateTime.Now;
                SQEDefectReport.AddIqcKp(iqckp);
                this.updatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();DealHideWait();alert('" + pmtMessage3.Trim() + "');", true);
            }
            else
            {
                //修改一条Tp="PE"的情况
                iqckp.Tp = "PE";
                SQEDefectReport.UpdateIqcKp(iqckp, Tp, iqckp.CtLabel, iqckp.Defect);
                this.updatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();alert('" + pmtMessage4.Trim() + "');DealHideWait();", true);

            }
        }
    }


    /// <summary>
    /// 显示ctno相关的不良记录信息到gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowDetailCtno_ServerClick(object sender, EventArgs e)
    {
        GetSQEDefectCTNoInfo();
        string CtNO = this.dCTNO.Text;
        if (CtNO.Trim() != "" && CtNO.Trim().Length == 14)
        {
            this.cmbiecpn.CtNo = CtNO;
            this.cmbiecpn.ClearContent();
            this.cmbiecpn.refresh(CtNO);
        }
        else
        {
            this.cmbiecpn.ClearContent();
            this.cmbiecpn.refresh();
        }
        //获取ctno相关的model数据并绑定到指定text
        IList<string> lstPartType = SQEDefectReport.GetPartTypeSQEDefectReport(CtNO);
        if (lstPartType != null && lstPartType.Count > 0)
        {
            this.dModelPartNo.Text = lstPartType.First().ToString();
        }
        //获取ctno相关的不良数据的详细信息并绑定到相应控件
        IList<SQEDefectProductRepairReportInfo> lstDetail = SQEDefectReport.GetSQEDefectProductRepairInfo(CtNO);
        if (lstDetail != null && lstDetail.Count == 1)
        {
            SQEDefectProductRepairReportInfo temp = lstDetail.First();
            this.drpDefect.InnerDropDownList.ClearSelection();
            ListItem selectedValue = this.drpDefect.InnerDropDownList.Items.FindByValue(temp.DefectCode);
            if (selectedValue != null)
            {
                selectedValue.Selected = true;
            }
            this.drpCause.InnerDropDownList.ClearSelection();
            ListItem selectValue1 = this.drpCause.InnerDropDownList.Items.FindByValue(temp.Cause);
            if (selectValue1 != null)
            {
                selectValue1.Selected = true;
            }
            this.drpMajorPart.InnerDropDownList.ClearSelection();
            ListItem selectValue2 = this.drpMajorPart.InnerDropDownList.Items.FindByValue(temp.MajorPart);
            if (selectValue2 != null)
            {
                selectValue2.Selected = true;
            }
            this.drpObligation.InnerDropDownList.ClearSelection();
            ListItem selectValue3 = this.drpObligation.InnerDropDownList.Items.FindByValue(temp.Obligation);
            if (selectValue3 != null)
            {
                selectValue3.Selected = true;
            }
            this.dRemark.Text = temp.Remark;
        }
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();", true);

    }


    /// <summary>
    /// 取得所输CTNo相关的不良记录信息
    /// </summary>
    protected void GetSQEDefectCTNoInfo()
    {
        string ctno = this.dCTNO.Text;
        IList<SQEDefectCTNoInfo> SQEDefectCTNoLst = SQEDefectReport.GetSQEDefectCTNoInfo(ctno);
        try
        {
            if (SQEDefectCTNoLst != null && SQEDefectCTNoLst.Count > 0)
            {
                bindTable(SQEDefectCTNoLst, DEFAULT_ROWS);
                setColumnWidth();
            }
            else
            {
                //NOT DATA FOUND ERROR!
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
                showErrorMessage(pmtMessage2);
            }

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrcode);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();", true);
    }

    #endregion


    #region 下拉列表的触发事件以及初始化

    private void initDropLists()
    {
        this.drpDefect.Tp = "PRD";
        this.drpDefect.initSQEReport();
        this.drpCause.Tp = "FACause";
        this.drpCause.initSQEReport();
        this.drpMajorPart.Tp = "MajorPart";
        this.drpMajorPart.initSQEReport();
        this.drpCommponent.Tp = "Component";
        this.drpCommponent.initSQEReport();
        this.drpObligation.Tp = "Obligation";
        this.drpObligation.initSQEReport();
    }
    #endregion


    #region 控制页面初始化
    /// <summary>
    /// 初始化页面Label文本显示，added by ShhWang on 2011-10-13
    /// </summary>
    private void initLabel()
    {
        this.lblCTNO.Text = this.GetLocalResourceObject(pre + "_lblCTNO").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(pre + "_lblDefect").ToString();
        this.lblModelPartNo.Text = this.GetLocalResourceObject(pre + "_lblModelPartNo").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(pre + "_lblDefect").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(pre + "_lblCause").ToString();
        this.lblMajorPart.Text = this.GetLocalResourceObject(pre + "_lblMajorPart").ToString();
        this.lblCommponent.Text = this.GetLocalResourceObject(pre + "_lblComponent").ToString();
        this.lblObligation.Text = this.GetLocalResourceObject(pre + "_lblObligation").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(pre + "_lblRemark").ToString();
        this.lblResult.Text = this.GetLocalResourceObject(pre + "_lblResult").ToString();
        this.lblDefectList.Text = this.GetLocalResourceObject(pre + "_lblDefectList").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(pre + "_btnSave").ToString();
        this.lblMpToSQE.Text = this.GetLocalResourceObject(pre + "_lblMpToSQE").ToString();
        this.lblPE.Text = this.GetLocalResourceObject(pre + "_lblPE").ToString();
        this.lblSQE.Text = this.GetLocalResourceObject(pre + "_lblSQE").ToString();
        this.lblIECPN.Text = this.GetLocalResourceObject(pre + "_lblIECPN").ToString();
    }

    /// <summary>
    /// 设置gridview列的宽度百分比 added by ShhWang on 2011-12-22
    /// </summary>
    private void setColumnWidth()
    {

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(20);

    }


    #endregion


    #region gridview事件
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// gridview的数据绑定 added by ShhWang on 2011-10-13
    /// </summary>
    /// <param name="list"></param>
    /// <param name="defaultRow"></param>
    private void bindTable(IList<SQEDefectCTNoInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(pre + "_lstPDLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(pre + "_lstDefect").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(pre + "_lstCause").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(pre + "_lstItemUdt").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (SQEDefectCTNoInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = Null2String(temp.Line);
                dr[1] = Null2String(temp.Defect);
                dr[2] = Null2String(temp.Cause);
                dr[3] = ((System.DateTime)temp.Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[4] = ((System.DateTime)temp.Udt).ToString("yyyy-MM-dd HH:mm:ss");
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
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        //   gd.GvExtHeight = dTableHeight.Value;
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();", true);
    }


    #endregion


    #region 一些系统方法
    /// <summary>
    /// 错误信息处理
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 控制转义字符
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    /// <summary>
    /// 空值处理
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    #endregion
}
