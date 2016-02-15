/*
 *Issue 
 *
 * ITC-1361-0036  itc210012  2012-01-17
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
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_MasterLabel : System.Web.UI.Page
{
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    public String UserId;
    public String Customer;
    private IMasterLabel imasterLabel = null;
    private string gFamily = string.Empty;
    private string gVc = string.Empty;
    
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
            imasterLabel = ServiceAgent.getInstance().GetMaintainObjectByName<IMasterLabel>(WebConstant.MASTERMAITAIN);
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_msgConfirmDelete").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_msgInput").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();

                initLabel();
                IList<MasterLabelDef> lstMasterLabel = imasterLabel.GetMasterLabelByVCAndCode(string.Empty, string.Empty);
                bindTable(lstMasterLabel, DEFAULT_ROWS);
                setColumnWidth();
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
        this.lblVc.Text=this.GetLocalResourceObject(Pre+"_lblvc").ToString();
        this.lblFamily.Text=this.GetLocalResourceObject(Pre+"_lblFamily").ToString();
        this.btnQuery.Value=this.GetLocalResourceObject(Pre+"_btnQuery").ToString();
        this.lblMasterLabelList.Text=this.GetLocalResourceObject(Pre+"_lblMasterLabelList").ToString();
        this.btnDelete.Value=this.GetLocalResourceObject(Pre+"_btnDelete").ToString();
        this.lblVcBottom.Text=this.GetLocalResourceObject(Pre+"_lblVCBottom").ToString();
        this.lblFamilyBottom.Text=this.GetLocalResourceObject(Pre+"_lblFamilyBottom").ToString();
        this.lblCode.Text=this.GetLocalResourceObject(Pre+"_lblCode").ToString();
        this.btnSave.Value=this.GetLocalResourceObject(Pre+"_btnSave").ToString();
    }

    private void bindTable(IList<MasterLabelDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("ID");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colVC").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        

        if (list != null && list.Count != 0)
        {
            foreach (MasterLabelDef temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.id;
                dr[1] = temp.vc;
                dr[2] = temp.family;
                dr[3] = temp.code;
                dr[4] = temp.editor;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                
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
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }


    protected void gd_DataBound(object sender, EventArgs e)
    {
        if (!gHasHightRow)
        {
            changeSelectedIndex("-1", string.Empty, string.Empty,string.Empty);
        }
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

            //if (string.Compare(e.Row.Cells[1].Text.Trim(), gFamily.Trim(), true) == 0
            //    && string.Compare(e.Row.Cells[2].Text.Trim(), gVc.Trim(), true) == 0)
            //{
            //    string cellCode = (e.Row.Cells[4].Text.Trim().ToLower() == "&nbsp;" ? string.Empty : e.Row.Cells[4].Text.Trim());
    
            //    e.Row.CssClass = "iMes_grid_SelectedRowGvExt";
            //    changeSelectedIndex(e.Row.RowIndex.ToString(), gFamily.Trim(), gVc,cellCode);
            //    gHasHightRow = true;
            //}
        }
    }

    protected void Query_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string family = this.hidFamily.Value;
            //ITC-1361-0119 ITC210012 2012-3-7
            string vc = this.ttVc.Value.ToUpper().Trim();
            IList<MasterLabelDef> lstMasterLabel = imasterLabel.GetMasterLabelByVCAndCode(vc,family);

            bindTable(lstMasterLabel, DEFAULT_ROWS);
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
    //    string msgHasExistRecord = GetLocalResourceObject(Pre + "_msgHasExistRecord").ToString();

        try
        {
            IList<MasterLabelDef> lstMasterLabel = null;
            string family = this.cmbFamily2.InnerDropDownList.SelectedValue.Trim();
            //itc-1361-0087 itc210012
            string vc = this.ttVcBottom.Value.Trim().ToUpper();
            string code = this.ttCode.Value.Trim().ToUpper();
           
            string id = this.hidDeleteID.Value;
            MasterLabelDef ml = new MasterLabelDef();
            ml.vc = vc;
            ml.code = code;
            ml.family = family;
            ml.editor = this.HiddenUserName.Value;
            gFamily = family;
            gVc = vc;
  
            if (!string.IsNullOrEmpty(id))
            {
                ml.id = int.Parse(id);
            }
            imasterLabel.SaveMasterLabelItem(ml);

            //this.cmbFamily1.refresh();
            //this.cmbFamily1.InnerDropDownList.SelectedValue = family;

            lstMasterLabel = imasterLabel.GetMasterLabelByVCAndCode(this.ttVc.Value.Trim(), this.cmbFamily1.InnerDropDownList.SelectedValue.Trim());

            bindTable(lstMasterLabel, DEFAULT_ROWS);
            //this.ttVc.Value = vc;
            //this.updatePanel6.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + vc + "','" + family + "');", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
  //          showAlertErrorMessage(msgHasExistRecord);
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

    protected void Delete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string family = this.cmbFamily1.InnerDropDownList.SelectedValue.Trim();
            string vc = this.ttVc.Value.Trim();
            IList<MasterLabelDef> lstMasterLabel = null;
            MasterLabelDef deleteItem = new MasterLabelDef();
            
            deleteItem.id = int.Parse(this.hidDeleteID.Value);
            imasterLabel.RemoveMasterLabelItem(deleteItem.id);
            this.cmbFamily1.refresh();
            this.cmbFamily1.InnerDropDownList.SelectedValue = family;
            
            lstMasterLabel = imasterLabel.GetMasterLabelByVCAndCode(vc, family);
            bindTable(lstMasterLabel, DEFAULT_ROWS);
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
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("selectedRowIndex=" + index + ";");
        this.cmbFamily2.refresh();
        this.cmbFamily2.InnerDropDownList.SelectedValue = family;
        scriptBuilder.AppendLine("document.getElementById('" + this.ttVcBottom.ClientID + "').value='" + vc + "';");
        scriptBuilder.AppendLine("document.getElementById('" + this.ttCode.ClientID + "').value='" + code + "';");
        
        if (string.Compare(index, "-1", true) == 0)
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
        }
        else
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
        }

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "changeSelectedIndex", scriptBuilder.ToString(), false);
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
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(50);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(70);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(65);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(40);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(60);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(60);
    }
    
}
