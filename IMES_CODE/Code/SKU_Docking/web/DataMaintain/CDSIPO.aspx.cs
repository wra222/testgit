/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for CDSI PO Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
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
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;

public partial class DataMaintain_CDSIPO : System.Web.UI.Page
{
    public String UserId;

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ICDSIPO iCDSIPO = (ICDSIPO)ServiceAgent.getInstance().GetMaintainObjectByName<ICDSIPO>(WebConstant.MaintainCDSIPOObject);

    protected void Page_Load(object sender, EventArgs e)
    {
        UserId = Master.userInfo.UserId;
        if (!this.IsPostBack)
        {
            initLabel();
            showList();
            this.updatePanel2.Update();      
        }
    }

    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string id = this.hidSelectedId.Value.Trim();
        try
        {
            iCDSIPO.Delete(id);
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

        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "delete", "resetTableHeight();initPage();HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        string id = this.txtProd.Text.Trim();
        try
        {
            if (iCDSIPO.CheckIfExist(id))
            {
                jsAlert(this.GetLocalResourceObject(Pre + "_msgAlreadyExists").ToString());
                hideWait();
                return;
            }

            if (!iCDSIPO.CheckIfExistProduct(id))
            {
                jsAlert(this.GetLocalResourceObject(Pre + "_msgNoSuchProduct").ToString());
                hideWait();
                return;
            }

            SnoDetPoMoInfo item = new SnoDetPoMoInfo();
            item.snoId = id;
            item.mo = iCDSIPO.GetMOByProductID(id);
            item.po = this.txtPO.Text.Trim();
            item.poitem = this.txtPOItem.Text.Trim();
            item.delivery = this.txtDelivery.Text.Trim();
            item.plt = this.txtPallet.Text.Trim();
            item.boxId = this.txtBoxID.Text.Trim();
            item.remark = this.txtRemark.Text.Trim();
            item.editor = UserId;

            iCDSIPO.Add(item);
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

        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);        
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string oldID = this.hidSelectedId.Value.Trim();
        string newID = this.txtProd.Text.Trim();
        try
        {
            if (oldID != newID && iCDSIPO.CheckIfExist(newID))
            {
                jsAlert(this.GetLocalResourceObject(Pre + "_msgAlreadyExists").ToString());
                hideWait();
                return;
            }

            if (!iCDSIPO.CheckIfExistProduct(newID))
            {
                jsAlert(this.GetLocalResourceObject(Pre + "_msgNoSuchProduct").ToString());
                hideWait();
                return;
            }

            SnoDetPoMoInfo item = new SnoDetPoMoInfo();
            item.snoId = newID;
            item.mo = iCDSIPO.GetMOByProductID(newID);
            item.po = this.txtPO.Text.Trim();
            item.poitem = this.txtPOItem.Text.Trim();
            item.delivery = this.txtDelivery.Text.Trim();
            item.plt = this.txtPallet.Text.Trim();
            item.boxId = this.txtBoxID.Text.Trim();
            item.remark = this.txtRemark.Text.Trim();
            item.editor = UserId;

            iCDSIPO.Update(oldID, item);
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

        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + newID + "');HideWait();", true);
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
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblProd.Text = this.GetLocalResourceObject(Pre + "_lblProd").ToString();
        this.lblPO.Text = this.GetLocalResourceObject(Pre + "_lblPO").ToString();
        this.lblPOItem.Text = this.GetLocalResourceObject(Pre + "_lblPOItem").ToString();
        this.lblDelivery.Text = this.GetLocalResourceObject(Pre + "_lblDelivery").ToString();
        this.lblPallet.Text = this.GetLocalResourceObject(Pre + "_lblPallet").ToString();
        this.lblBoxID.Text = this.GetLocalResourceObject(Pre + "_lblBoxID").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    private void bindTable(IList<SnoDetPoMoInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        
        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colProductID").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colMO").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPO").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPOItem").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDelivery").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPallet").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colBoxID").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        if (list != null && list.Count != 0)
        {
            foreach (SnoDetPoMoInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.snoId;
                dr[1] = temp.mo;
                dr[2] = temp.po;
                dr[3] = temp.poitem;
                dr[4] = temp.delivery;
                dr[5] = temp.plt;
                dr[6] = temp.boxId;
                dr[7] = temp.remark;
                dr[8] = temp.editor;
                /*
                * Answer to: ITC-1361-0149
                * Description: Updated format of CDT and UDT.
                */
                dr[9] = temp.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                dr[10] = temp.udt.ToString("yyyy-MM-dd hh:mm:ss");
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

      //  ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "selectedRowIndex = -1;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(12);
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

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void hideWait()
    {
        String script = "<script language='javascript'> HideWait(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "hideWait", script, false);
    }

    private Boolean showList()
    {       
        IList<SnoDetPoMoInfo> dataLst = null;
        try
        {
            dataLst = iCDSIPO.GetList();

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
}
