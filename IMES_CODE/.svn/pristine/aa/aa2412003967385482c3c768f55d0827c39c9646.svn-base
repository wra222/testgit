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
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Text;
using System.Collections.Generic;
using IMES.Infrastructure;

public partial class DataMaintain_RepairInfoMaintain : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 31;
    private IRepairInfoMaintain irpairInfo;
    private const int COL_NUM = 5;
    private const string CUSTOMERID = "<PUB>";
    public string pmtMessage1 = "";
    public string pmtMessage2 = "";
    public string pmtMessage3 = "";
    public string pmtMessage4 = "";
    public string pmtMessage5 = "";
    public string pmtMessage6 = "";
    public string pmtMessage7 = "";
    public string pmtMessage8 = "";
    public string pmtMessage9 = "";
    public string pmtMessage10 = "";
    public string pmtMessage11 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        irpairInfo = (IRepairInfoMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IRepairInfoMaintain>(WebConstant.REPAIRINFOMAINTAIN);
        if (!IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();

            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            List<RepairInfoMaintainDef> dataLst = null;
            initLabel();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "initConditionData", "initConditionData();", true);
        }
    //    ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "initContorls();", true);
    }
    

    protected void btnTypeFirstOption_ServerClick(object sender, EventArgs e) 
    {
        showList();
    }

    protected void btnTypeChange_ServerClick(object sender, EventArgs e)
    {
        showList();
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string type = this.cmbRepairInfoType.InnerDropDownList.SelectedValue.Trim();
        string code = this.DelCode.Value.Trim();
        try
        {
            RepairInfoMaintainDef def = new RepairInfoMaintainDef();
            def.type = type;
            def.code = code;
            irpairInfo.RemoveRepairInfoMaintainItem(def);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "delete", "DeleteComplete();HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        int itemId = 0;
        try
        {
            string type = this.cmbRepairInfoType.InnerDropDownList.SelectedValue.Trim();
            string code = this.ttCode.Text.Trim();
            string desc = this.ttDescription.Text.Trim();
            string engDes = this.ttEngDesc.Text.Trim();
            RepairInfoMaintainDef def = new RepairInfoMaintainDef();
            def.type = type;
            def.code = code;
            def.customerID = CUSTOMERID;
            def.description = desc;
            def.engDescr = engDes;
            def.editor = this.HiddenUserName.Value;
            def.cdt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            def.udt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string id = this.OldID.Value;
            if(!String.IsNullOrEmpty(id))
            {
                def.id = Convert.ToInt32(id);
            }
            itemId=irpairInfo.AddOrUpdateRepairInfoMaintain(def);
            //itemId = def.id.ToString();
        }
        catch (FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
        catch (Exception ee)
        {
            showErrorMessage(ee.Message);
            return;
        }
        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Update", "AddUpdateComplete('" + itemId + "');HideWait();", true);
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

    private void initLabel()
    {
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblRepairInfoList.Text = this.GetLocalResourceObject(Pre + "_lblRepairInfoList").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblEngDesc.Text = this.GetLocalResourceObject(Pre + "_lblEngDesc").ToString();
    }
    private void bindTable(IList<RepairInfoMaintainDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEngDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (RepairInfoMaintainDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.code;
                dr[1] = temp.description;
                dr[2] = temp.engDescr;
                dr[3] = temp.editor;
                dr[4] = temp.cdt;
                dr[5] = temp.udt;
                dr[6] = temp.id;
                
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
    private void showList()
    {
        string type = this.cmbRepairInfoType.InnerDropDownList.SelectedValue.Trim();
        List<RepairInfoMaintainDef> gradeLst = null;
        try
        {
            if (!string.IsNullOrEmpty(type))
            {
                gradeLst = (List<RepairInfoMaintainDef>)irpairInfo.GetRepairInfoByCondition(type);
            }
        }
        catch (FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
            return;
        }
        catch (Exception ee)
        {
            showErrorMessage(ee.Message);
            return;
        }
        bindTable(gradeLst, DEFAULT_ROWS);
        this.updatePanel2.Update();
        //if (gradeLst != null && gradeLst.Count != 0)
        //{
        //    string familyId = replaceSpecialChart(family);
        //    ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "HideWait();", true);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "NoMatchFamily();HideWait();", true);
        //}
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "HideWait();", true);
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
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(12);
    }
}
