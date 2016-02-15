/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: ECRVersion Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * issue:
 * itc-1366-0011  itc210012       2012-1-11
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
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;


public partial class DataMaintain_PAKitLoc : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 31;
    private IPAKitLoc iPAKitLoc;
    private const int COL_NUM = 8;
  
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
    protected void Page_Load(object sender, EventArgs e)
    {
        
 //       this.cmbPdLine.InnerDropDownList.Load += new EventHandler(cmbPdLine_Load);
        this.ddlDescr.InnerDropDownList.Load += new EventHandler(ddlDescr_Load);
        iPAKitLoc = (IPAKitLoc)ServiceAgent.getInstance().GetMaintainObjectByName<IPAKitLoc>(WebConstant.PAKITLOCMAITAIN);
        if (!IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre+"_pmtMessage10").ToString();
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            initLabel();
    //        string pdline = this.cmbPdLine.InnerDropDownList.SelectedValue.Trim();
            List<PAKitLocDef> gradeLst = null;
            
            bindTable(gradeLst, DEFAULT_ROWS);
        }
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "InitControl", "initContorls();", true);
    }
    private void ddlDescr_Load(object sender, System.EventArgs e) 
    {
        if(!this.ddlDescr.IsPostBack)
        {
            string descr = this.ddlDescr.InnerDropDownList.SelectedValue.Trim();
            this.ddlPartNo.refresh(descr);
            showListByGradeList();
        }
        
    }

   
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        PAKitLocDef pak = new PAKitLocDef();
        pak.pdLine = this.cmbPdLine.InnerDropDownList.SelectedValue.Trim();
        pak.partNo = this.ddlPartNo.InnerDropDownList.SelectedValue.Trim();
        pak.Descr = this.ddlDescr.InnerDropDownList.SelectedValue.Trim();
        pak.station = this.ddlStation.InnerDropDownList.SelectedValue.Trim();
        pak.location = this.ttLocation.Text.Trim();
        pak.editor = this.HiddenUserName.Value.Trim();

        string itemId = "";
        try
        {
            itemId = iPAKitLoc.AddPAKitLoc(pak);
            
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
        showListByGradeList();
        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);
    }
    protected void btnRefreshPartNoList_ServerClick(object sender, EventArgs e)
    {
        string descr = this.ddlDescr.InnerDropDownList.SelectedValue.Trim();
        this.ddlPartNo.refresh(descr);
        
   ////   showListByGradeList();
   ////    ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "displaySelectedRow", "showSelected();HideWait();", true);
        ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "displaySelectedRow", "HideWait();", true);
    }
    protected void btnDisplaySelectedInfo_ServerClick(object sender, EventArgs e)
    {
        string descrSeled = this.HiddenTypeDescr.Value.Trim();
        string partno = this.HiddenPartNo.Value.Trim();
        string station = this.HiddenStation.Value.Trim();
        string location = this.HiddenLocation.Value.Trim();
       
        if (partno != "")
        {
            this.ddlPartNo.refresh(descrSeled);
            showListByGradeList();
            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Object), "display", "displaySelectedItem('" + descrSeled + "','" + partno + "','" + station + "','" + location + "');HideWait();", true);
        }
    }

    private void ShowPAKitLoctionInfo() 
    {
        string pdline = this.cmbPdLine.InnerDropDownList.SelectedValue.Trim();
        List<PAKitLocDef> gradeLst = null;
        try
        {
            gradeLst = (List<PAKitLocDef>)iPAKitLoc.GetPAKitlocByPdLine(pdline);

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
        if (gradeLst != null && gradeLst.Count != 0)
        {

            string pdlineID = replaceSpecialChart(pdline);

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
            if (pdline=="")
            {
                ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "HideWait();", true);
            }
            else
            {
                 ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "NoMatchFamily();HideWait();", true);
            }
           
        }
    }
    protected void btnPdLineChange_ServerClick(object sender, EventArgs e)
    {
        ShowPAKitLoctionInfo();
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string id = this.HiddenSelectedId.Value.Trim();
        try
        {
            PAKitLocDef def = new PAKitLocDef();
            def.id = Convert.ToInt32(id);
            iPAKitLoc.DeletePAKitLoc(def);
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
        showListByGradeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DeleteComplete();HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
       
        //init newPAKitLoc
        string pdline = this.cmbPdLine.InnerDropDownList.SelectedValue.Trim();
        string partno = this.ddlPartNo.InnerDropDownList.SelectedValue.Trim();
        string descr = this.ddlDescr.InnerDropDownList.SelectedValue.Trim();
        string station = this.ddlStation.InnerDropDownList.SelectedValue.Trim();
        string location = this.ttLocation.Text.Trim();
        PAKitLocDef newDel = new PAKitLocDef();
        newDel.pdLine = pdline;
        newDel.partNo = partno;
        newDel.Descr = descr;
        newDel.station = station;
        newDel.location = location;
        newDel.editor = this.HiddenUserName.Value.Trim();
        newDel.id = Convert.ToInt32(this.HiddenSelectedId.Value.Trim());
        string itemId = this.HiddenSelectedId.Value.Trim();
        try
        {
            iPAKitLoc.UpdatePAKitLoc(newDel);
          
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
        showListByGradeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[7].Attributes.Add("style", e.Row.Cells[7].Attributes["style"] + "display:none");
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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblPAKitLoc.Text = this.GetLocalResourceObject(Pre + "_lblPAKitLoc").ToString();
        this.lblTypeDescrDdl.Text = this.GetLocalResourceObject(Pre + "_lblTypeDescr").ToString();
        this.lblStationDdl.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNo").ToString();
        this.lblLocation.Text = this.GetLocalResourceObject(Pre + "_lblLocation").ToString();
        
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

    }
    private void bindTable(IList<PAKitLocDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblTypeDescr").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblLocation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());
        dt.Columns.Add("PdLine");
        if (list != null && list.Count != 0)
        {
            foreach (PAKitLocDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.partNo;
                dr[1] = temp.Descr;
                dr[2] = temp.station;
                dr[3] = temp.location;
                dr[4] = temp.editor;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                dr[7] = temp.id;
                
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
        string pdline = this.cmbPdLine.InnerDropDownList.SelectedValue;
        IList<PAKitLocDef> gradeLst = null;
        try
        {
            if (!String.IsNullOrEmpty(pdline))
            {
                gradeLst = iPAKitLoc.GetPAKitlocByPdLine(pdline);
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
