/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-6-2   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using System.IO;

public partial class DataMaintain_DefectMaintain : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //interface
    private IDefectMaintain iDefectMaintain;

    private const int DEFAULT_ROWS = 36;
    private const int COL_NUM = 7;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    //ITC-1361-0112 ITC210012 2012-02-28
    public string pmtMessage7;
    public string pmtMessage8;  

    //lstItems in cmbType
    public string lstItemCmbType1;
    public string lstItemCmbType2;
    public string lstItemCmbType3;
    public string lstItemCmbType4;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.cmdDefectCodeTypeTop.Load += new EventHandler(cmdDefectCodeTypeTop_Load);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();

            pmtMessage7 = this.GetLocalResourceObject(Pre+"_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre+"_pmtMessage8").ToString();

            iDefectMaintain = (IDefectMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IDefectMaintain>(WebConstant.MaintainDefectMaintainObject);

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                InitcmdDefectCodeType();
                showList();
                setColumnWidth();
                

            }
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

    private void initLabel()
    {
        //init labels
        this.lblDefectCodeList.Text = this.GetLocalResourceObject(Pre + "_lblDefectCodeList").ToString();
        this.lblDefectCode.Text = this.GetLocalResourceObject(Pre + "_lblDefectCode").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.lblEngDescr.Text = this.GetLocalResourceObject(Pre + "_lblEngDescr").ToString();
        //init cmbType items
        lstItemCmbType1 = this.GetLocalResourceObject(Pre + "_lstItemCmbType1").ToString();
        lstItemCmbType2 = this.GetLocalResourceObject(Pre + "_lstItemCmbType2").ToString();
        lstItemCmbType3 = this.GetLocalResourceObject(Pre + "_lstItemCmbType3").ToString();
        lstItemCmbType4 = this.GetLocalResourceObject(Pre + "_lstItemCmbType4").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(15);

    }

    private void InitcmdDefectCodeType()
    {
        IConstValueType iConstValueType = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueType>(WebConstant.ConstValueTypeObject);
        IList<ConstValueTypeInfo> valueList = iConstValueType.GetConstValueTypeList("DefectCodeType");
        this.cmdDefectCodeTypeTop.Items.Clear();
        this.cmdDefectCodeType.Items.Clear();
        this.cmdDefectCodeTypeTop.Items.Add(string.Empty);

        foreach (ConstValueTypeInfo items in valueList)
        {
            ListItem item = new ListItem();
            item.Text = items.value;
            item.Value = items.value;
            this.cmdDefectCodeTypeTop.Items.Add(item);
            this.cmdDefectCodeType.Items.Add(item);
        }
    }

    private void cmdDefectCodeTypeTop_Load(object sender, System.EventArgs e)
    {
        showList();
    }

    private void bindTable(IList<DefectCodeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //set columns
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDefect").ToString());
        dt.Columns.Add("Type");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstEngDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        //need to modify these codes after get the interface 
        if (list != null && list.Count != 0)
        {

            foreach (DefectCodeInfo info in list)
            {
                dr = dt.NewRow();
                dr[0] = info.Defect;
                dr[1] = info.Type;
                dr[2] = info.Descr;
                dr[3] = info.engDescr;
                dr[4] = info.Editor;
                dr[5] = info.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = info.Udt.ToString("yyyy-MM-dd HH:mm:ss");
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

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex=null;", true);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string defect = this.txtDefect.Text;
            string oldCode = this.hidCode.Value;
            string type = this.cmdDefectCodeType.SelectedValue;
            DefectCodeInfo dfc = new DefectCodeInfo();
            dfc.Defect = defect;
            dfc.Type = type;
            dfc.Descr = this.txtDescription.Text.ToString().Trim();
            dfc.engDescr = this.txtEngDescr.Text;
            dfc.Editor = this.HiddenUserName.Value.Trim();
            dfc.Udt = DateTime.Now;
            if (defect == oldCode)
            {
                iDefectMaintain.UpdateDefectCode(dfc);
            }
            else
            {
                
                dfc.Cdt = DateTime.Now;
                iDefectMaintain.InsertDefectCode(dfc);
            }
            showList();
            updatePanel.Update();
            defect = replaceSpecialChart(dfc.Defect);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "save complete", "DealHideWait();AddUpdateComplete('" + defect + "');resetTableHeight()", true);
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

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string strDefect = this.hidCode.Value;
        try
        {
            iDefectMaintain.DeleteDefectCode(strDefect);
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
        showList();
        updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "deleteUpdate", "DeleteComplete();DealHideWait();resetTableHeight();", true);
    }

    private void showList()
    {
        try
        {
            IList<DefectCodeInfo> iList;
            if (this.cmdDefectCodeTypeTop.SelectedValue == "")
            {
                iList = iDefectMaintain.GetDefectCodeList();
            }
            else
            {
                string type = this.cmdDefectCodeTypeTop.SelectedValue;
                iList = iDefectMaintain.GetDefectCodeList(type);
            }
            if (iList != null && iList.Count > 0)
            {
                bindTable(iList, DEFAULT_ROWS);
            }
            else
            {
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
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
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

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private string replaceString(string strGd)
    {
        strGd = strGd.Replace("&lt;", "<");
        strGd = strGd.Replace("&gt;", ">");
        strGd = strGd.Replace("&amp;", "&");
        strGd = strGd.Replace("&quot;", "\"");
        return strGd;
    }


}
