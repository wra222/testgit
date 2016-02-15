/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for LCM Repair Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RCTO LCM Repair.docx
 * UC:CI-MES12-SPEC-FA-UC RCTO LCM Repair.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-16  itc202017             (Reference Ebook SourceCode) Create
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

public partial class FA_LCMRepair : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;

    ILCMRepair iLCMRepair = ServiceAgent.getInstance().GetObjectByName<ILCMRepair>(WebConstant.LCMRepairObject);

    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnProcess.ServerClick += new EventHandler(btnProcess_ServerClick);
            btnRefresh.ServerClick += new EventHandler(btnRefresh_ServerClick);
            btnReset.ServerClick += new EventHandler(btnReset_ServerClick);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            Station = Request["Station"];
            hidRowCnt.Value = "0";
            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();
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
        iLCMRepair.Cancel(hidProdID.Value.Trim());
        txtCTNO.Text = "";
        UpdatePanel1.Update();
        txtModel.Text = "";
        UpdatePanel2.Update();
        txtStation.Text = "";
        UpdatePanel3.Update();
        txtLine.Text = "";
        UpdatePanel4.Update();
        clearGrid();
        unsetGlobalCTNO();
        hidInput.Value = "";
        hidProdID.Value = "";
        hidRowCnt.Value = "0";
        upHidden.Update();
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
            string id = "";
            string line = "";
            string model = "";
            string ts = "";
            iLCMRepair.InputCTNo(hidInput.Value, UserId, Station, Customer, out id, out model, out ts, out line);

            txtCTNO.Text = hidInput.Value;
            UpdatePanel1.Update();
            txtModel.Text = model;
            UpdatePanel2.Update();
            txtStation.Text = ts;
            UpdatePanel3.Update();
            txtLine.Text = line;
            UpdatePanel4.Update();

            hidProdID.Value = id;
            upHidden.Update();

            refreshTable();
            endWaitingCoverDiv();
            setGlobalCTNO(hidInput.Value, id);
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
        }
    }

    private void refreshTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        int cnt = 0;
                
        IList<RepairInfo> repList = iLCMRepair.GetLCMRepairList(hidProdID.Value);

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
        builder.Append(seperator);
        builder.Append(temp.side);

        return builder.ToString();
    }

    private void InitLabel()
    {
        lblDataEntry.Text = GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        lblCTNO.Text = GetLocalResourceObject(Pre + "_lblCTNO").ToString();
        lblModel.Text = GetLocalResourceObject(Pre + "_lblModel").ToString();
        lblStation.Text = GetLocalResourceObject(Pre + "_lblStation").ToString();
        lblLine.Text = GetLocalResourceObject(Pre + "_lblLine").ToString();
        lblTableTitle.Text = GetLocalResourceObject(Pre + "_lblTableTitle").ToString();

        callNextInput();
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

    private void setGlobalCTNO(string info, string id)
    {
        String script = "<script language='javascript'> globalCTNO = \"" + info +"\"; globalID = \"" + id + "\";</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "setGlobalCTNO", script, false);
    }

    private void unsetGlobalCTNO()
    {
        String script = "<script language='javascript'> globalCTNO = \"\"; globalID=\"\";</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "unsetGlobalCTNO", script, false);
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
