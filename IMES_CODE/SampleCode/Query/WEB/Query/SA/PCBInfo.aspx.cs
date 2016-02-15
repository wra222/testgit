﻿using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using IMES.DataModel;

public partial class Query_SA_BoardHistoryQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    
    ISA_PCBInfo iPCAQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBInfo>(WebConstant.ISA_PCBInfo);
    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {
            BindNullDataTable();   
            InitGridView();
        }    
    }
  
    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }
    public void queryClick(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt1 = new DataTable();
            dt1 = iPCAQuery.GetChangePCBLog(DBConnection, hidMBSN.Value.Trim());
            if (dt1.Rows.Count > 0)
            {
                //取的最新的PCBNo
                this.gvChangePCBLog.DataSource = dt1;
                this.gvChangePCBLog.DataBind();
                hidMBSN.Value = dt1.Rows[0]["NewPCBNo"].ToString();
            }
            else
            {
                this.gvPCAHistory.DataSource = getNullDataTable_History(1);
                this.gvPCAHistory.DataBind();
            }

            
            DataSet ds = new DataSet();

            ds = getDataTable(hidMBSN.Value.Trim());
            if (ds.Tables[0].Rows.Count > 0)
            {
                //this.gvPCAStatus.DataSource = getDataTable(txtMBSN.Value.Substring(0,10).Trim());
                this.gvPCAStatus.DataSource = ds.Tables[0];
                this.gvPCAStatus.DataBind();
                InitGridView();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                this.gvPCANextStation.DataSource = ds.Tables[1];
                this.gvPCANextStation.DataBind();

                this.gvPCANextStation.DataSource = null;
                this.gvPCANextStation.DataSource = getNullDataTable_NextStation(ds.Tables[1].Rows.Count);
                this.gvPCANextStation.DataBind();

                for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                {
                    gvPCANextStation.Rows[i].Cells[0].Text = ds.Tables[1].Rows[i]["Station"].ToString();
                    gvPCANextStation.Rows[i].Cells[1].Text = ds.Tables[1].Rows[i]["Station Description"].ToString();
                }
                InitNextStationGridView();
            }
            else
            {
                this.gvPCANextStation.DataSource = getNullDataTable_NextStation(1);
                this.gvPCANextStation.DataBind();
            }

            DataTable dt = new DataTable();
            //dt=iPCAQuery.GetMBHistory(hidMBSN.Value.Trim());
            dt = iPCAQuery.GetMBHistory(DBConnection,gvPCAStatus.Rows[0].Cells[0].Text.Trim());//Dean 20110916
            if (dt.Rows.Count > 0)
            {
                this.gvPCAHistory.DataSource = null;
                this.gvPCAHistory.DataSource = getNullDataTable_History(dt.Rows.Count);
                this.gvPCAHistory.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvPCAHistory.Rows[i].Cells[0].Text = dt.Rows[i]["Station"].ToString() + " - " + dt.Rows[i]["StationName"].ToString();
                    gvPCAHistory.Rows[i].Cells[1].Text = dt.Rows[i]["Line"].ToString();
                    gvPCAHistory.Rows[i].Cells[2].Text = dt.Rows[i]["FixtureID"].ToString();
                    gvPCAHistory.Rows[i].Cells[3].Text = (dt.Rows[i]["Status"].ToString() == "1" || dt.Rows[i]["Status"].ToString().ToUpper() == "PASS") ? "OK" : "NG";
                    gvPCAHistory.Rows[i].Cells[4].Text = dt.Rows[i]["ErrorCode"].ToString();
                    gvPCAHistory.Rows[i].Cells[5].Text = dt.Rows[i]["Editor"].ToString();
                    gvPCAHistory.Rows[i].Cells[6].Text = dt.Rows[i]["Cdt"].ToString();
                }
                InitHistoryGridView();
            }
            else
            {
                this.gvPCAHistory.DataSource = getNullDataTable_History(1);
                this.gvPCAHistory.DataBind();
            }


            DataTable dtRepairInfo = new DataTable();
            dtRepairInfo = iPCAQuery.GetRepairInfo(DBConnection, hidMBSN.Value.Trim());
            if (dtRepairInfo.Rows.Count > 0)
            {
                this.gvPCARepair.DataSource = null;
                this.gvPCARepair.DataSource = getNullDataTable_Repair(dtRepairInfo.Rows.Count);
                this.gvPCARepair.DataBind();


                for (int i = 0; i <= dtRepairInfo.Rows.Count - 1; i++)
                {
                    gvPCARepair.Rows[i].Cells[0].Text = dtRepairInfo.Rows[i]["Line"].ToString();
                    gvPCARepair.Rows[i].Cells[1].Text = dtRepairInfo.Rows[i]["StationName"].ToString();
                    gvPCARepair.Rows[i].Cells[2].Text = dtRepairInfo.Rows[i]["Status"].ToString() == "1" ? "OK" : "NG";
                    gvPCARepair.Rows[i].Cells[3].Text = dtRepairInfo.Rows[i]["Location"].ToString();
                    gvPCARepair.Rows[i].Cells[4].Text = dtRepairInfo.Rows[i]["Descr"].ToString();
                    gvPCARepair.Rows[i].Cells[5].Text = dtRepairInfo.Rows[i]["Cause"].ToString();
                    gvPCARepair.Rows[i].Cells[6].Text = dtRepairInfo.Rows[i]["Obligation"].ToString();
                    gvPCARepair.Rows[i].Cells[7].Text = dtRepairInfo.Rows[i]["Remark"].ToString();
                    gvPCARepair.Rows[i].Cells[8].Text = dtRepairInfo.Rows[i]["Editor"].ToString();
                    gvPCARepair.Rows[i].Cells[9].Text = dtRepairInfo.Rows[i]["Cdt"].ToString();
                }
                InitRepairGridView();
            }
            else
            {
                this.gvPCARepair.DataSource = getNullDataTable_Repair(1);
                this.gvPCARepair.DataBind();
            }    
        
            DataTable dtTestLog = new DataTable();
            dtTestLog = iPCAQuery.GetTestLog(DBConnection, hidMBSN.Value.Trim());
            if (dtTestLog.Rows.Count > 0)
            {
                this.gvPCATestlog.DataSource = null;
                this.gvPCATestlog.DataSource = getNullDataTable_TestLog(dtTestLog.Rows.Count);
                this.gvPCATestlog.DataBind();

                for (int i = 0; i <= gvPCATestlog.Rows.Count - 1; i++)
                {
                    gvPCATestlog.Rows[i].Cells[0].Text = dtTestLog.Rows[i]["PCBNo"].ToString();
                    gvPCATestlog.Rows[i].Cells[1].Text = dtTestLog.Rows[i]["Line"].ToString();
                    gvPCATestlog.Rows[i].Cells[2].Text = dtTestLog.Rows[i]["FixtureID"].ToString();
                    gvPCATestlog.Rows[i].Cells[3].Text = dtTestLog.Rows[i]["Station"].ToString();
                    gvPCATestlog.Rows[i].Cells[4].Text = dtTestLog.Rows[i]["Type"].ToString();
                    gvPCATestlog.Rows[i].Cells[5].Text = dtTestLog.Rows[i]["Status"].ToString() == "1" ? "OK" : "NG";
                    gvPCATestlog.Rows[i].Cells[6].Text = dtTestLog.Rows[i]["DefectCodeID"].ToString();
                    gvPCATestlog.Rows[i].Cells[7].Text = dtTestLog.Rows[i]["Descr"].ToString();
                    gvPCATestlog.Rows[i].Cells[8].Text = dtTestLog.Rows[i]["Remark"].ToString();
                    gvPCATestlog.Rows[i].Cells[9].Text = dtTestLog.Rows[i]["Editor"].ToString();
                    gvPCATestlog.Rows[i].Cells[10].Text = dtTestLog.Rows[i]["Cdt"].ToString();
                }
                InitTestLodView();
            }
            else
            {
                this.gvPCATestlog.DataSource = getNullDataTable_Repair(1);
                this.gvPCATestlog.DataBind();
            }

          
        }
        catch (FisException ex)
        {
            BindNullDataTable();
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            BindNullDataTable();
            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    private DataSet getDataTable(string MBSN)
    {
        DataSet ds = new DataSet();
            
        DataTable dt = initTable(); //PCBStatus
        DataTable dt1 = initTable_NextStation();//PCB_NextStation

        DataRow newRow = null;
        List<NextStation> nextStationList = new List<NextStation>();

        IMES.Query.Interface.QueryIntf.MBInfo mb = iPCAQuery.GetMBInfo( DBConnection, MBSN, out nextStationList);

        if (mb != null)
        {
            newRow = dt.NewRow();
            newRow["MBSN"] = mb.MBSN;   
            newRow["PCBModelID"] = mb.PartNo;
            newRow["SMTMO"] = mb.SMTMO;
            newRow["StationID"] = mb.Station;
            newRow["Station"] = mb.Station + '-' +mb.StationDescr;
            newRow["Status"] = mb.Status == 0 ? "Fail" : "Pass";
            newRow["Line"] = mb.Line;
            newRow["TestFailCount"] = mb.TestFailCount;
            newRow["MAC"] = mb.MAC;
            newRow["ECR"] = mb.ECR;
            newRow["CUSTSN"] = mb.CustomSN;
            newRow["Udt"] = mb.Udt;
            dt.Rows.Add(newRow);

            foreach (NextStation ns in nextStationList)
            {
                newRow = dt1.NewRow();
                newRow["Station"] = ns.Station;
                newRow["Station Description"] = ns.Description;
                dt1.Rows.Add(newRow);
            }

            ds.Tables.Add(dt);
            ds.Tables.Add(dt1);
        }

        return ds;
    }


    #region InitGridView
    private void InitGridView() {
        InitPCAStatusGridView();
        InitNextStationGridView();
        InitHistoryGridView();
        InitChangePCBGridView();
        InitRepairGridView();
    }
    private void InitPCAStatusGridView()
    {
        int i = 100;
        int j = 70;
        gvPCAStatus.HeaderRow.Cells[0].Width = Unit.Pixel(i); //MBSN
        gvPCAStatus.HeaderRow.Cells[1].Width = Unit.Pixel(i); //PCBModelID
        gvPCAStatus.HeaderRow.Cells[2].Width = Unit.Pixel(i); //SMTMO
        gvPCAStatus.HeaderRow.Cells[3].Width = Unit.Pixel(j); //StationID                
        gvPCAStatus.HeaderRow.Cells[4].Width = Unit.Pixel(220); //Station
        gvPCAStatus.HeaderRow.Cells[5].Width = Unit.Pixel(j); //Status                
        gvPCAStatus.HeaderRow.Cells[6].Width = Unit.Pixel(j); //Line                
        gvPCAStatus.HeaderRow.Cells[7].Width = Unit.Pixel(i); //TestFailCount
        gvPCAStatus.HeaderRow.Cells[8].Width = Unit.Pixel(i); //MAC
        gvPCAStatus.HeaderRow.Cells[9].Width = Unit.Pixel(j); //ECR
        gvPCAStatus.HeaderRow.Cells[10].Width = Unit.Pixel(150); //CUSTSN
        gvPCAStatus.HeaderRow.Cells[11].Width = Unit.Pixel(180);//Udt

    }
    private void InitNextStationGridView()
    {
        gvPCANextStation.HeaderRow.Cells[0].Width = Unit.Pixel(60); //Station
        gvPCANextStation.HeaderRow.Cells[1].Width = Unit.Pixel(200); //StationName
    }
    private void InitHistoryGridView()
    {
        int i = 100;
        int j = 70;
        gvPCAHistory.HeaderRow.Cells[0].Width = Unit.Pixel(220); //Station+StationName
        gvPCAHistory.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Line
        gvPCAHistory.HeaderRow.Cells[2].Width = Unit.Pixel(i); //FixtureID
        gvPCAHistory.HeaderRow.Cells[3].Width = Unit.Pixel(j); //status                
        gvPCAHistory.HeaderRow.Cells[4].Width = Unit.Pixel(i); //ErrorCode
        gvPCAHistory.HeaderRow.Cells[5].Width = Unit.Pixel(j); //Editor                
        gvPCAHistory.HeaderRow.Cells[6].Width = Unit.Pixel(150); //Cdt                
    }
    private void InitChangePCBGridView(){
        gvChangePCBLog.HeaderRow.Cells[0].Width = Unit.Pixel(60);
        gvChangePCBLog.HeaderRow.Cells[1].Width = Unit.Pixel(60);
        gvChangePCBLog.HeaderRow.Cells[2].Width = Unit.Pixel(150);
        gvChangePCBLog.HeaderRow.Cells[3].Width = Unit.Pixel(50);
        gvChangePCBLog.HeaderRow.Cells[4].Width = Unit.Pixel(150);
    }

    private void InitRepairGridView()
    {
        int i = 100;
        int j = 150;
        int k = 70;
        int l = 400;
        gvPCARepair.HeaderRow.Cells[0].Width = Unit.Pixel(50);//Line
        gvPCARepair.HeaderRow.Cells[1].Width = Unit.Pixel(j);//StationName
        gvPCARepair.HeaderRow.Cells[2].Width = Unit.Pixel(50);//Status
        gvPCARepair.HeaderRow.Cells[3].Width = Unit.Pixel(j);//Location
        gvPCARepair.HeaderRow.Cells[4].Width = Unit.Pixel(l);//Descr
        gvPCARepair.HeaderRow.Cells[5].Width = Unit.Pixel(l);//Cause
        gvPCARepair.HeaderRow.Cells[6].Width = Unit.Pixel(j);//Obligation
        gvPCARepair.HeaderRow.Cells[7].Width = Unit.Pixel(l);//Remark
        gvPCARepair.HeaderRow.Cells[8].Width = Unit.Pixel(80);//Editor
        gvPCARepair.HeaderRow.Cells[9].Width = Unit.Pixel(j);//Cdt


    }

    private void InitTestLodView() {
        int i = 100;
        int j = 150;
        int k = 70;
        gvPCATestlog.HeaderRow.Cells[0].Width = Unit.Pixel(i);//PCBNo
        gvPCATestlog.HeaderRow.Cells[1].Width = Unit.Pixel(50);//Line
        gvPCATestlog.HeaderRow.Cells[2].Width = Unit.Pixel(50);//FixtureID
        gvPCATestlog.HeaderRow.Cells[3].Width = Unit.Pixel(50);//Station
        gvPCATestlog.HeaderRow.Cells[4].Width = Unit.Pixel(k);//Type
        gvPCATestlog.HeaderRow.Cells[5].Width = Unit.Pixel(45);//Status
        gvPCATestlog.HeaderRow.Cells[6].Width = Unit.Pixel(90);//DefectCodeID
        gvPCATestlog.HeaderRow.Cells[7].Width = Unit.Pixel(j);//Descr
        gvPCATestlog.HeaderRow.Cells[8].Width = Unit.Pixel(600);//Remark
        gvPCATestlog.HeaderRow.Cells[9].Width = Unit.Pixel(80);//Editor
        gvPCATestlog.HeaderRow.Cells[10].Width = Unit.Pixel(j);//Cdt
        
    } 

    #endregion

    #region GetNullDataTable

    public void BindNullDataTable() {
        this.gvPCAStatus.DataSource = getNullDataTable(1);
        this.gvPCAStatus.DataBind();

        this.gvPCANextStation.DataSource = getNullDataTable_NextStation(1);
        this.gvPCANextStation.DataBind();

        this.gvPCAHistory.DataSource = getNullDataTable_History(1);
        this.gvPCAHistory.DataBind();

        this.gvChangePCBLog.DataSource = getNullDataTable_ChangePCBLog(1);
        this.gvChangePCBLog.DataBind();

        this.gvPCARepair.DataSource = getNullDataTable_Repair(1);
        this.gvPCARepair.DataBind();        

        this.gvPCATestlog.DataSource = getNullDataTable_TestLog(1);
        this.gvPCATestlog.DataBind();
    }
    private DataTable getNullDataTable(int j)
    {

        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["MBSN"] = "";
            newRow["PCBModelID"] = "";
            newRow["SMTMO"] = "";
            newRow["StationID"] = "";
            newRow["Station"] = "";
            newRow["Status"] = "";
            newRow["Line"] = "";
            newRow["TestFailCount"] = "";
            newRow["MAC"] = "";
            newRow["ECR"] = "";
            newRow["CUSTSN"] = "";
            newRow["Udt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable getNullDataTable_NextStation(int j)
    {
        DataTable dt = initTable_NextStation();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Station"] = "";
            newRow["Station Description"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable getNullDataTable_ChangePCBLog(int j)
    {
        DataTable dt = initTable_ChangePCBLog();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["NewPCBNo"] = "";
            newRow["OldPCBNo"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable getNullDataTable_Repair(int j)
    {
        DataTable dt = initTable_Repair();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Line"] = "";
            newRow["Station"] = "";
            newRow["Status"] = "";
            newRow["Location"] = "";
            newRow["Defect"] = "";
            newRow["Cause"] = "";
            newRow["Obligation"] = "";
            newRow["Remark"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            
            dt.Rows.Add(newRow);
        }
        return dt;
        
    }

    private DataTable getNullDataTable_History(int j)
    {
        DataTable dt = initTable_History();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Station"] = "";
            newRow["Line"] = "";
            newRow["FixtureID"] = "";
            newRow["Status"] = "";
            newRow["ErrorCode"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable getNullDataTable_TestLog(int j) {
        DataTable dt = initTable_TestLog();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["PCBNo"] = "";
            newRow["Line"] = "";
            newRow["FixtureID"] = "";
            newRow["Station"] = "";
            newRow["Type"] = "";
            newRow["Status"] = "";
            newRow["DefectCodeID"] = "";
            newRow["Descr"] = "";
            newRow["Remark"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    #endregion

    #region GetInitDataTable    
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));
        retTable.Columns.Add("PCBModelID", Type.GetType("System.String"));
        retTable.Columns.Add("SMTMO", Type.GetType("System.String"));
        retTable.Columns.Add("StationID", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("TestFailCount", Type.GetType("System.String"));
        retTable.Columns.Add("MAC", Type.GetType("System.String"));
        retTable.Columns.Add("ECR", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));

        return retTable;
    }
    private DataTable initTable_NextStation()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Station Description", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_ChangePCBLog()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("NewPCBNo", Type.GetType("System.String"));
        retTable.Columns.Add("OldPCBNo", Type.GetType("System.String"));
        retTable.Columns.Add("Reason", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_History()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("FixtureID", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("ErrorCode", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_Repair()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));        
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Location", Type.GetType("System.String"));
        retTable.Columns.Add("Defect", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("Obligation", Type.GetType("System.String"));        
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_TestLog(){
         DataTable retTable = new DataTable();
        retTable.Columns.Add("PCBNo", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("FixtureID", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Type",Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("DefectCodeID", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    
    }
    #endregion

    private void showErrorMessage(string errorMsg)
    {
        if (Pre == "cn")
        {
            errorMsg = "没有找到 PCB 信息!!";
        }
        else
        {
            errorMsg = "Not Found PCB Information!!";
        }

        this.gvPCAStatus.DataSource = getNullDataTable(1);
        this.gvPCAStatus.DataBind();
        InitGridView();

        this.gvPCANextStation.DataSource = getNullDataTable_NextStation(1);
        this.gvPCANextStation.DataBind();
        InitNextStationGridView();

        this.gvPCAHistory.DataSource = getNullDataTable_History(1);
        this.gvPCAHistory.DataBind();
        InitHistoryGridView();

        this.gvPCARepair.DataSource = getNullDataTable_Repair(1);
        this.gvPCARepair.DataBind();

        this.gvChangePCBLog.DataSource = getNullDataTable_ChangePCBLog(1);
        this.gvChangePCBLog.DataBind();


        this.gvPCATestlog.DataSource = getNullDataTable_ChangePCBLog(1);
        this.gvPCATestlog.DataBind();
        

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }
}
