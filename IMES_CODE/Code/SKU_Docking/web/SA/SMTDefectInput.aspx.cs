/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for SMT Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class SA_SMTDefectInput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    ISMTDefectInput iSMTDefectInput = ServiceAgent.getInstance().GetObjectByName<ISMTDefectInput>(WebConstant.SMTDefectInputObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try          
        {
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            btnInputMBSno.ServerClick += new EventHandler(btnInputMBSno_ServerClick);
            btnInputDefect.ServerClick += new EventHandler(btnInputDefect_ServerClick);
            btnClearDefect.ServerClick += new EventHandler(btnClearDefect_ServerClick);
            btnSave.ServerClick += new EventHandler(btnSave_ServerClick);
            btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();

                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "SA";

                this.hidStation.Value = Request["Station"];
                this.upHidden.Update();
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        showInfo("");
        callNextInput();
    }

    private void btnExit_ServerClick(object sender, System.EventArgs e)
    {
        string mbsno = txtMBSno.Text.Trim();
        if (mbsno != "")
        {
            iSMTDefectInput.Cancel(mbsno);
        }
    }

    private void btnInputMBSno_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string mbsno = hidInput.Value;
            iSMTDefectInput.CheckMBSno(mbsno, cmbPdLine.InnerDropDownList.SelectedValue, UserId, hidStation.Value, Customer);
            setMBInfo(mbsno);
            hidCurrentDefectList.Value = "";
            upHidden.Update();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            if (this.txtMBSno.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnInputDefect_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string mbsno = txtMBSno.Text.Trim();
            IList<DefectCodeDescr> defectList = iSMTDefectInput.InputDefect(mbsno, hidInput.Value);

            DataTable dt = initTable();
            DataRow newRow;
            int cnt = 0;

            string curDefects = ":";

            foreach (DefectCodeDescr ele in defectList)
            {
                curDefects += ele.DefectCode + ":";
                newRow = dt.NewRow();
                newRow["Defect"] += ele.DefectCode;
                newRow["Descr"] += ele.Description;
                dt.Rows.Add(newRow);
                cnt++;
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.GridViewExt1.DataSource = dt;
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();
            if (curDefects == ":") curDefects = "";
            hidCurrentDefectList.Value = curDefects;
            upHidden.Update();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            if (this.txtMBSno.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnClearDefect_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string mbsno = txtMBSno.Text.Trim();
            iSMTDefectInput.ClearDefect(mbsno);
            hidCurrentDefectList.Value = "";
            upHidden.Update();
            clearGrid();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            if (this.txtMBSno.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string mbsno = txtMBSno.Text.Trim();
            iSMTDefectInput.Save(mbsno);
            showSuccessInfo(mbsno);
            updateQty();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            setMBInfo("");
            clearGrid();
            enableCombox();
            hidInput.Value = "";
            hidCurrentDefectList.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void InitLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();

        hidCurrentDefectList.Value = "";
        upHidden.Update();
        setPdLineCombFocus();
    }

    private DataTable getNullDataTable()
    {      
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Defect", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        this.GridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleDefect").ToString();
        this.GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDescr").ToString();
        this.GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(150);
        this.GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(350);
    }

    private void setMBInfo(string mbsno)
    {
        try
        {
            this.txtMBSno.Text = mbsno;
            this.UpdatePanel3.Update();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
    
    private void updateQty()
    {
        try
        {
            int fQty = int.Parse(txtFailQty.Text.Trim());
            fQty++;
            txtFailQty.Text = fQty.ToString();
            this.UpdatePanel2.Update();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void clearGrid()
    {
        try
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            initTableColumnHeader();
            gridViewUP.Update();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void showSuccessInfo(string mbsno)
    {
        /*
         * Answer to: ITC-1360-1830
         * Description: Show success info in format.
         */
        String script = "<script language='javascript'> ShowSuccessfulInfoFormat(true, \"MBSNo\", \"" + mbsno + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void setPdLineCombFocus()
    {
        String script = "<script language='javascript'>  window.setTimeout (setPdLineCmbFocus,100); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void enableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = false;</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "enableCombox", script, false);
    }

    private void disableCombox()
    {
        String script = "<script language='javascript'> getPdLineCmbObj().disabled = true;</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "disableCombox", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void callNextInput()
    {
        String script = "<script language='javascript'>callNextInput();</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
}
