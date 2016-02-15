/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for KP Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI KeyParts Defect Input.docx
 * UC:CI-MES12-SPEC-SA-UC KeyParts Defect Input.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-20  itc202017             (Reference Ebook SourceCode) Create
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

public partial class FA_KPDefectInput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IKPDefectInput iKPDefectInput = ServiceAgent.getInstance().GetObjectByName<IKPDefectInput>(WebConstant.KPDefectInputObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try          
        {
            cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            btnInputCTNo.ServerClick += new EventHandler(btnInputCTNo_ServerClick);
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
                this.cmbPdLine.Stage = "FA";

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
        string ctno = txtCTNo.Text.Trim();
        if (ctno != "")
        {
            iKPDefectInput.Cancel(ctno);
        }
    }

    private void btnInputCTNo_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string ctno = hidInput.Value;
            iKPDefectInput.CheckCTNO(ctno, cmbPdLine.InnerDropDownList.SelectedValue, UserId, hidStation.Value, Customer);
            setCTNOInfo(ctno);
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
            if (this.txtCTNo.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnInputDefect_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string ctno = txtCTNo.Text.Trim();
            IList<DefectCodeDescr> defectList = iKPDefectInput.InputDefect(ctno, hidInput.Value);

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
            if (this.txtCTNo.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnClearDefect_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string ctno = txtCTNo.Text.Trim();
            iKPDefectInput.ClearDefect(ctno);
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
            if (this.txtCTNo.Text != "") disableCombox();
            hidInput.Value = "";
            upHidden.Update();
            callNextInput();
        }
    }

    private void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string ctno = txtCTNo.Text.Trim();
            iKPDefectInput.Save(ctno);
            showSuccessInfo(ctno);
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
            setCTNOInfo("");
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
        this.lblCTNo.Text = this.GetLocalResourceObject(Pre + "_lblCTNo").ToString();
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

    private void setCTNOInfo(string ctno)
    {
        try
        {
            this.txtCTNo.Text = ctno;
            this.UpdatePanel1.Update();
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

    private void showSuccessInfo(string ctno)
    {
        String script = "<script language='javascript'> ShowSuccessfulInfoFormat(true, \"CT No\", \"" + ctno + "\"); </script>";
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
