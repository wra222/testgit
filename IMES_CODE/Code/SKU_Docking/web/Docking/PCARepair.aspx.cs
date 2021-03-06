/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repair Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair For Docking.docx
 * UC:CI-MES12-SPEC-SA-UC PCA Repair For Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-12  itc202017             (Reference Ebook SourceCode) Create
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
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class SA_PCARepair : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IPCARepair iPCARepair = ServiceAgent.getInstance().GetObjectByName<IPCARepair>(WebConstant.PCARepairForDockingObject);
    IMB iMB = ServiceAgent.getInstance().GetObjectByName<IMB>(WebConstant.CommonObject);
    ITestStation iTS = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);
    IRepair iRepair = ServiceAgent.getInstance().GetObjectByName<IRepair>(WebConstant.CommonObject);
    IPdLine iPdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.CommonObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
          
        {
            cmbRepStn.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbRepStn_Selected);
            btnProcess.ServerClick += new EventHandler(btnProcess_ServerClick);
            btnRefresh.ServerClick += new EventHandler(btnRefresh_ServerClick);
            btnReset.ServerClick += new EventHandler(btnReset_ServerClick);
            cmbRepStn.InnerDropDownList.AutoPostBack = true;
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            hidRowCnt.Value = "0";
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();

                cmbRepStn.Station = Request["Station"];
                cmbRepStn.Customer = Customer;
                cmbRepStn.StationType = "SARepair";
                cmbRepStn.BNeedStationInText = true;
                cmbRepStn.SortField = "Station";
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

    private void clearPage()
    {
        /*
         * Answer to: ITC-1360-0400
         * Description: Remain op result after save.
         */
        clearMBInfo();
        hidInput.Value = "";
        hidRowCnt.Value = "0";
        upHidden.Update();
        clearGrid();
        cancelWorkFlow();
        unsetGlobalMBSno();
        enableCmbRepStn();
    }

    private void cmbRepStn_Selected(object sender, System.EventArgs e)
    {
        /*
         * Answer to: ITC-1360-0210
         * Description: Refresh pdLine combox according to selected repair station.
         */
        clearPage();
        showInfo("");
        callNextInput();
    }

    protected void gd_DataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Font.Bold = true;
        
        if (e.Row.Cells[0].Text == "X")
        {
            e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            e.Row.Cells[0].ForeColor = System.Drawing.Color.Green;
        }

        /*
         * Answer to: ITC-1360-0390
         * Description: Show tip of fields defect and cause.
         */
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i <= 2; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }

        e.Row.Cells[5].Style.Add("display", "none");        
    }

    private void btnReset_ServerClick(object sender, System.EventArgs e)
    {
        clearPage();
    }

    private void btnProcess_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iPCARepair.InputMBSn(hidInput.Value, null, UserId, cmbRepStn.InnerDropDownList.SelectedValue, Customer);
                        
            MBInfo info = iMB.GetMBInfo(hidInput.Value);
            setMBInfo(info);

            refreshTable();
            endWaitingCoverDiv();
            disableCmbRepStn();
            setGlobalMBSno(hidInput.Value);
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
            callNextInput();
        }
    }

    private void btnRefresh_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            refreshTable();
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
            disableCmbRepStn();
        }
    }

    private void refreshTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        int cnt = 0;

        IList<RepairInfo> repList = iRepair.GetMBRepairList(hidInput.Value);

        if (repList != null && repList.Count != 0)
        {
            foreach (RepairInfo ele in repList)
            {
                newRow = dt.NewRow();
                if (ele.isManual == "1")
                {
                    newRow["Icon"] = "√";
                }
                else
                {
                    newRow["Icon"] = "X";
                }
                newRow["Defect"] = ele.defectCodeID + " " + ele.defectCodeDesc;
                newRow["Cause"] = ele.cause + " " + ele.causeDesc;
                newRow["CDate"] = ele.cdt.ToShortDateString();
                newRow["UDate"] = ele.udt.ToShortDateString();
                newRow["hidCol"] = getHideColumn(ele);
                dt.Rows.Add(newRow);
                cnt++;
            }
        }

        hidRowCnt.Value = cnt.ToString();
        upHidden.Update();

        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        GridViewExt1.DataSource = dt;
        GridViewExt1.DataBind();
        initTableColumnHeader();
        gridViewUP.Update();
    }

    private string getHideColumn(RepairInfo temp)
    {
        StringBuilder builder = new StringBuilder();
        string seperator = "\u0003";

        builder.Append(temp.id);
        builder.Append(seperator);
        builder.Append(temp.repairID);
        builder.Append(seperator);
        builder.Append(temp.type);
        builder.Append(seperator);
        builder.Append(temp.obligation);
        builder.Append(seperator);
        builder.Append(temp.component);
        builder.Append(seperator);
        builder.Append(temp.site);
        builder.Append(seperator);
        builder.Append(temp.majorPart);
        builder.Append(seperator);
        /*
         * Answer to: ITC-1360-0240
         * Description: Change and recover character "<NL>" in remark.
         */
        builder.Append(temp.remark.Replace("\r\n", "___M_NL___"));
        builder.Append(seperator);
        builder.Append(temp.vendorCT);
        builder.Append(seperator);
        builder.Append(temp.partType);
        builder.Append(seperator);
        builder.Append(temp.oldPart);
        builder.Append(seperator);
        builder.Append(temp.oldPartSno);
        builder.Append(seperator);
        builder.Append(temp.newPart);
        builder.Append(seperator);
        builder.Append(temp.newPartSno);
        builder.Append(seperator);
        builder.Append(temp.manufacture);
        builder.Append(seperator);
        builder.Append(temp.versionA);
        builder.Append(seperator);
        builder.Append(temp.versionB);
        builder.Append(seperator);
        builder.Append(temp.returnSign);
        builder.Append(seperator);
        builder.Append(temp.mark);
        builder.Append(seperator);
        builder.Append(temp.subDefect);
        builder.Append(seperator);
        builder.Append(temp.piaStation);
        builder.Append(seperator);
        builder.Append(temp.distribution);
        builder.Append(seperator);
        builder.Append(temp._4M);
        builder.Append(seperator);
        builder.Append(temp.responsibility);
        builder.Append(seperator);
        builder.Append(temp.action);
        builder.Append(seperator);
        builder.Append(temp.cover);
        builder.Append(seperator);
        builder.Append(temp.uncover);
        builder.Append(seperator);
        builder.Append(temp.trackingStatus);
        builder.Append(seperator);
        builder.Append(temp.isManual);
        builder.Append(seperator);
        builder.Append(temp.editor);
        builder.Append(seperator);
        builder.Append(temp.newPartDateCode);
        builder.Append(seperator);
        builder.Append(temp.defectCodeID);
        builder.Append(seperator);
        builder.Append(temp.cause);

        //2010-03-20 add side
        builder.Append(seperator);
        builder.Append(temp.side);

        return builder.ToString();
    }

    private void InitLabel()
    {
        lblRepStn.Text = GetLocalResourceObject(Pre + "_lblRepStn").ToString();
        lblPdLine.Text = GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        lblDataEntry.Text = GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        lblTestStn.Text = GetLocalResourceObject(Pre + "_lblTestStn").ToString();
        lblMBSno.Text = GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        lblFamily.Text = GetLocalResourceObject(Pre + "_lblFamily").ToString();
        lblModel.Text = GetLocalResourceObject(Pre + "_lblModel").ToString();
        lblTableTitle.Text = GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
    
        setRepairStationComboFocus();
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
        retTable.Columns.Add("Icon", Type.GetType("System.String"));
        retTable.Columns.Add("Defect", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("CDate", Type.GetType("System.String"));
        retTable.Columns.Add("UDate", Type.GetType("System.String"));
        retTable.Columns.Add("hidCol", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader()
    {
        GridViewExt1.HeaderRow.Cells[0].Text = "";
        GridViewExt1.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleDefect").ToString();
        GridViewExt1.HeaderRow.Cells[2].Text = GetLocalResourceObject(Pre + "_titleCause").ToString();
        GridViewExt1.HeaderRow.Cells[3].Text = GetLocalResourceObject(Pre + "_titleCDate").ToString();
        GridViewExt1.HeaderRow.Cells[4].Text = GetLocalResourceObject(Pre + "_titleUDate").ToString();
        GridViewExt1.HeaderRow.Cells[5].Text = "";
        GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(10);
        GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(145);
        GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(145);
        GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(100);
    }

    private void clearMBInfo()
    {
        txtMBSno.Text = "";
        UpdatePanel1.Update();
        txtFamily.Text = "";
        UpdatePanel2.Update();
        txtModel.Text = "";
        UpdatePanel3.Update();
        txtTestStn.Text = "";
        UpdatePanel4.Update();
        txtPdLine.Text = "";
        UpdatePanel5.Update();
    }

    private void setMBInfo(MBInfo info)
    {
        txtMBSno.Text = info.id;
        UpdatePanel1.Update();
        txtFamily.Text = info.family;
        UpdatePanel2.Update();
        txtModel.Text = info._111LevelId;
        UpdatePanel3.Update();
        /*
         * Answer to: ITC-1360-0264
         * Description: display description of test station on UI.
         */
        txtTestStn.Text = info.testStation + " " + iTS.GeStationDescr(info.testStation);
        UpdatePanel4.Update();
        txtPdLine.Text = iPdLine.GetPdLine(info.line).friendlyName;
        UpdatePanel5.Update();
    }

    private void clearGrid()
    {
        GridViewExt1.DataSource = getNullDataTable();
        GridViewExt1.DataBind();
        initTableColumnHeader();
        gridViewUP.Update();
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void setGlobalMBSno(string info)
    {
        String script = "<script language='javascript'> globalMBSno = \"" + info +"\"; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "setGlobalMBSno", script, false);
    }

    private void unsetGlobalMBSno()
    {
        String script = "<script language='javascript'> globalMBSno = \"\"; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "unsetGlobalMBSno", script, false);
    }

    /*
     * Answer to: ITC-1360-0270
     * Description: Disable repair station combox after input MBSno,
     *              and enable it after repair complete or page reset.
     */
    private void enableCmbRepStn()
    {
        String script = "<script language='javascript'> getStationByTypeCmbObj().disabled = false; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "enableCmbRepStn", script, false);
    }

    private void disableCmbRepStn()
    {
        //String script = "<script language='javascript'> document.getElementById(\"" + cmbRepStn.InnerDropDownList.ClientID + "\").disabled = true; </script>";
        String script = "<script language='javascript'> getStationByTypeCmbObj().disabled = true; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "disableCmbRepStn", script, false);
    }

    private void cancelWorkFlow()
    {
        String script = "<script language='javascript'> clearSession(); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "cancelWorkFlow", script, false);
    }

    private void setRepairStationComboFocus()
    {
        String script = "<script language='javascript'>  window.setTimeout (setStationByTypeFocus,100); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "setRepairStationComboFocus", script, false);
    }

    private void setPdLineComboFocus()
    {
        String script = "<script language='javascript'>  window.setTimeout (setPdLineCmbFocus,100); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "setPdLineComboFocus", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    private void callNextInput()
    {
        String script = "<script language='javascript'>callNextInput();</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
}
