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

public partial class DataMaintain_FAITCNDefectCheck : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //interface
    private IFAITCNDefectCheck iDefectMaintain;

    private const int DEFAULT_ROWS = 36;
    private const int COL_NUM = 6;

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
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();

            pmtMessage7 = this.GetLocalResourceObject(Pre+"_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre+"_pmtMessage8").ToString();

            iDefectMaintain = (IFAITCNDefectCheck)ServiceAgent.getInstance().GetMaintainObjectByName<IFAITCNDefectCheck>(WebConstant.FAITCNDefectCheckObject);

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
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
        this.lblDefectCheckList.Text = this.GetLocalResourceObject(Pre + "_lblDefectCheckList").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblHDDV.Text = this.GetLocalResourceObject(Pre + "_lblHDDV").ToString();
        this.lblBIOS.Text = this.GetLocalResourceObject(Pre + "_lblBIOS").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

        this.CheckMAC.Text = this.GetLocalResourceObject(Pre + "_chkMAC").ToString();
        this.CheckMBCT.Text = this.GetLocalResourceObject(Pre + "_chkMBCT").ToString(); 
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(17);

    }

    private void bindTable(IList<FaItCnDefectCheckInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //set columns
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemMAC").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemMBCT").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemHDDV").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemBIOS").ToString());
        dt.Columns.Add("ID");

        //need to modify these codes after get the interface 
        if (list != null && list.Count != 0)
        {

            foreach (FaItCnDefectCheckInfo info in list)
            {
                dr = dt.NewRow();
                dr[0] = info.code;
                dr[1] = info.mac;
                dr[2] = info.mbct;
                dr[3] = info.hddv;
                dr[4] = info.bios;
                dr[5] = info.id;
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

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {

        FaItCnDefectCheckInfo defect = new FaItCnDefectCheckInfo();
        defect.code = this.txtCode.Text.ToString().ToUpper().Trim();
        defect.hddv = this.txtHDDV.Text.ToString().ToUpper().Trim();
        defect.bios = this.txtBIOS.Text.ToString().ToUpper().Trim();
        defect.editor = this.HiddenUserName.Value.Trim();

        if (this.CheckMAC.Checked)
        {
            defect.mac = "Y";
        }
        else
        {
            defect.mac = "N";
        }

        if (this.CheckMBCT.Checked)
        {
            defect.mbct = "Y";
        }
        else
        {
            defect.mbct = "N";
        }

        string itemId = "";
        try
        {
            itemId = iDefectMaintain.InsertDefectCheck(defect);
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
        itemId = replaceSpecialChart(itemId);
        
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "save complete", "DealHideWait();AddUpdateComplete('" + itemId + "');resetTableHeight()", true);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {

        string itemId = this.HiddenSelectedId.Value.Trim();
        
        FaItCnDefectCheckInfo defect = new FaItCnDefectCheckInfo();

        defect.id = Convert.ToInt32(itemId);
        defect.code = this.txtCode.Text.ToString().ToUpper().Trim();
        defect.hddv = this.txtHDDV.Text.ToString().ToUpper().Trim();
        defect.bios = this.txtBIOS.Text.ToString().ToUpper().Trim();
        defect.editor = this.HiddenUserName.Value.Trim();

        if (this.CheckMAC.Checked)
        {
            defect.mac = "Y";
        }
        else
        {
            defect.mac = "N";
        }

        if (this.CheckMBCT.Checked)
        {
            defect.mbct = "Y";
        }
        else
        {
            defect.mbct = "N";
        }

        try
        {
            iDefectMaintain.UpdateDefectCheck(defect);

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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "save complete", "DealHideWait();AddUpdateComplete('" + itemId + "');resetTableHeight()", true);
        
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string ID = this.HiddenSelectedId.Value.Trim();
        try
        {
            iDefectMaintain.DeleteDefectCheck(Convert.ToInt32(ID));
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
            IList<FaItCnDefectCheckInfo> iList;
            iList = iDefectMaintain.GetDefectCheckList();

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

        e.Row.Cells[COL_NUM - 1].Attributes.Add("style", e.Row.Cells[COL_NUM - 1].Attributes["style"] + "display:none");
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
