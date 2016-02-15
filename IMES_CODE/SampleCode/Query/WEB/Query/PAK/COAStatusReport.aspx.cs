using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using System.Data;
using System.IO;

public partial class Query_PAK_COAStatusReport : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_COAStatusReport COAStatusReport = ServiceAgent.getInstance().GetObjectByName<IPAK_COAStatusReport>(WebConstant.COAStatusReport);
    ICOAStatus COAStatus = ServiceAgent.getInstance().GetObjectByName<ICOAStatus>(WebConstant.COAStatus);
    List<string> COAStatusList = new List<string>();
    const string DefaultType = "COASN";
    public int rowCount = 0;
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DBConnection = CmbDBType.ddlGetConnection();

            if (!this.IsPostBack) 
            {
                txtStartSN.Attributes.Add("onblur", "ClearEndTxt()");
                InitPage();
                InitCondition();
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

    private void InitPage()
    {
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblCOASN.Text = this.GetLocalResourceObject(Pre + "_lblCOASN").ToString();
        this.lblStartSN.Text = this.GetLocalResourceObject(Pre + "_lblStartSN").ToString();
        this.lblEndSN.Text = this.GetLocalResourceObject(Pre + "_lblEndSN").ToString();
        this.lblStartDate.Text = this.GetLocalResourceObject(Pre + "_lblStartDate").ToString();
        this.lblEndDate.Text = this.GetLocalResourceObject(Pre + "_lblEndDate").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblResult.Text = this.GetLocalResourceObject(Pre + "_lblResult").ToString();
        this.lblStartDateTR.Text = this.GetLocalResourceObject(Pre + "_lblStartDateTR").ToString();
        this.lblEndDateTR.Text = this.GetLocalResourceObject(Pre + "_lblEndDateTR").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");

        /*<asp:ListItem Value="1" Text="超過30天的COA Label"></asp:ListItem>
          <asp:ListItem Value="2" Text="還有5天就過期的COA Label"></asp:ListItem>
          <asp:ListItem Value="3" Text="還有10天就過期的COA Label"></asp:ListItem>
          <asp:ListItem Value="4" Text="超過5天結合未出貨"></asp:ListItem>*/

        ddlCOARemind.Items.Add(new ListItem("超過30天的COA Label", "1"));
        ddlCOARemind.Items.Add(new ListItem("還有5天就過期的COA Label", "2"));
        ddlCOARemind.Items.Add(new ListItem("還有10天就過期的COA Label", "3"));
        ddlCOARemind.Items.Add(new ListItem("超過5天結合未出貨", "4"));

    }
    private void InitCondition()
    {
        DataTable dtCOAStatus = COAStatus.GetCOAStatus(DBConnection);

        if (dtCOAStatus.Rows.Count > 0)
        {
            ddlCOAStatus.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtCOAStatus.Rows)
            {
                ddlCOAStatus.Items.Add(new ListItem(dr["Name"].ToString().Trim(), dr["Value"].ToString().Trim()));
            }
        }

        DataTable dtCOADate = COAStatus.GetCOADate(DBConnection);
        if (dtCOADate.Rows.Count > 0)
        {
            ddlStartDate.Items.Add(new ListItem("", ""));
            ddlEndDate.Items.Add(new ListItem("", ""));
            ddlStartDateTR.Items.Add(new ListItem("",""));
            ddlEndDateTR.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtCOADate.Rows)
            {
                ddlStartDate.Items.Add(new ListItem(dr["Date"].ToString().Trim(), dr["Date"].ToString().Trim()));
                ddlEndDate.Items.Add(new ListItem(dr["Date"].ToString().Trim(), dr["Date"].ToString().Trim()));
                ddlStartDateTR.Items.Add(new ListItem(dr["Date"].ToString().Trim(), dr["Date"].ToString().Trim()));
                ddlEndDateTR.Items.Add(new ListItem(dr["Date"].ToString().Trim(), dr["Date"].ToString().Trim()));
            }
        }


        this.gvResult.DataSource = getNullDataTable(1, DefaultType);
        this.gvResult.DataBind();
        InitGridView(DefaultType);
    }

    private void InitGridView(string type)
    {
        int i = 100;
        int j = 70;
        int k = 50;
        if (type == DefaultType)
        {
            gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
            gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
            gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(i);
            gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(i);
        }
        else if (type == "COADate" || type == "COARemind" || type == "COATR_T")
        {
            //COADate欄位名:IECPN/Qty/Status/Line/COAStatus
            //COARemind欄位名:COA Sno/CPQSNO/Line/Cdt/OverDue 
            gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(i);
            gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
            gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
            gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
            gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(i);
            if (ddlCOARemind.SelectedValue == "4" && type=="COARemind") //超過5天結合未出貨
            {
                gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
                gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(j);
                gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
            }
        }
        else if (type == "COAAllStatus")
        {
            for (int r = rowCount; r <= COAStatusList.Count + (rowCount - 1); r++)
            {
                gvResult.HeaderRow.Cells[r].Width = Unit.Pixel(200);
            }  
        }
        else if (type == "COATR_R")
        {
            gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(150);
            gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(150);
            gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(k);            
        }      
    }
    private DataTable getNullDataTable(int j , string type)
    {
        DataTable dt = initTable(type);
        DataRow newRow = null;
        if (type == DefaultType)
        {
            for (int i = 0; i < j; i++)
            {
                newRow = dt.NewRow();
                newRow["COA Sno"] = "";
                newRow["CPQSNO"] = "";
                newRow["Status"] = "";
                newRow["PdLine"] = "";

                dt.Rows.Add(newRow);
            }
        }
        else if (type == "COADate")
        {
            for (int i = 0; i < j; i++)
            {
                newRow = dt.NewRow();
                newRow["HP P/N"] = "";
                newRow["Qty"] = "";
                newRow["Status"] = "";
                newRow["Line"] = "";
                newRow["COAStatus"] = "";
                                
                dt.Rows.Add(newRow);
            }
        }
        else if (type=="COAAllStatus")
        {
            for (int i = 0; i < j; i++)
            {
                foreach (string STN in COAStatusList)
                {
                    newRow = dt.NewRow();
                    newRow[STN] = "";
                }
                dt.Rows.Add(newRow);
            }                                                
        }
        else if (type == "COARemind")
        {
            if (ddlCOARemind.SelectedValue != "4")
            {
                for (int i = 0; i < j; i++)
                {
                    newRow = dt.NewRow();
                    newRow["COA Sno"] = "";
                    newRow["CPQSNO"] = "";
                    newRow["Line"] = "";
                    newRow["Cdt"] = "";
                    newRow["OverDue"] = "";

                    dt.Rows.Add(newRow);
                }
            }
            else  //超過5天結合未出貨
            {
                for (int i = 0; i < j; i++)
                {
                    newRow = dt.NewRow();
                    newRow["ProdID"] = "";
                    newRow["SerialNo"] = "";
                    newRow["COA"] = "";
                    newRow["Pno"] = "";
                    newRow["Combine Date"] = "";
                    newRow["DN"] = "";
                    newRow["Model"] = "";
                    newRow["PGIFlag"] = "";

                    dt.Rows.Add(newRow);
                }
            }
        }
        else if (type == "COATR_T")
        {            
            for (int i = 0; i < j; i++)
            {
                newRow = dt.NewRow();
                newRow["BegNo"] = "";
                newRow["EndNo"] = "";
                newRow["Qty"] = "";
                newRow["Pno"] = "";
                newRow["Cdt"] = "";

                dt.Rows.Add(newRow);
            }                    
        }
        else if (type == "COATR_R")
        {
            for (int i = 0; i < j; i++)
            {
                newRow = dt.NewRow();
                newRow["HP PN / BegNo"] = "";
                newRow["TotalQty / EndNo"] = "";
                newRow["Qty"] = "";                

                dt.Rows.Add(newRow);
            }
        }
        return dt;
    }
    private DataTable initTable(string type)
    {
        DataTable retTable = new DataTable();
        if (type == DefaultType)
        {
            retTable.Columns.Add("COA Sno", Type.GetType("System.String"));
            retTable.Columns.Add("CPQSNO", Type.GetType("System.String"));
            retTable.Columns.Add("Status", Type.GetType("System.String"));
            retTable.Columns.Add("PdLine", Type.GetType("System.String"));
        }
        else if (type=="COADate")
        {
            retTable.Columns.Add("HP P/N", Type.GetType("System.String"));
            retTable.Columns.Add("Qty", Type.GetType("System.String"));
            retTable.Columns.Add("Status", Type.GetType("System.String"));
            retTable.Columns.Add("Line", Type.GetType("System.String"));
            retTable.Columns.Add("COAStatus", Type.GetType("System.String"));
        }
        else if (type == "COAAllStatus")
        {
            foreach (string STN in COAStatusList)
            {
                retTable.Columns.Add(STN, Type.GetType("System.String"));
            }
        }
        else if (type == "COARemind")
        {
            if (ddlCOARemind.SelectedValue != "4")
            {
                retTable.Columns.Add("COA Sno", Type.GetType("System.String"));
                retTable.Columns.Add("CPQSNO", Type.GetType("System.String"));
                retTable.Columns.Add("Line", Type.GetType("System.String"));
                retTable.Columns.Add("Cdt", Type.GetType("System.String"));
                retTable.Columns.Add("OverDue", Type.GetType("System.String"));
            }
            else //超過5天結合未出貨
            {
                retTable.Columns.Add("ProdID", Type.GetType("System.String"));
                retTable.Columns.Add("SerialNo", Type.GetType("System.String"));
                retTable.Columns.Add("COA", Type.GetType("System.String"));
                retTable.Columns.Add("Pno", Type.GetType("System.String"));
                retTable.Columns.Add("Combine Date", Type.GetType("System.String"));                
                retTable.Columns.Add("DN", Type.GetType("System.String"));
                retTable.Columns.Add("Model", Type.GetType("System.String"));
                retTable.Columns.Add("PGIFlag", Type.GetType("System.String"));                
            }
        }
        else if (type == "COATR_T")
        {            
            retTable.Columns.Add("BegNo", Type.GetType("System.String"));
            retTable.Columns.Add("EndNo", Type.GetType("System.String"));
            retTable.Columns.Add("Qty", Type.GetType("System.String"));
            retTable.Columns.Add("Pno", Type.GetType("System.String"));
            retTable.Columns.Add("Cdt", Type.GetType("System.String"));            
        }
        else if (type == "COATR_R")
        {
            retTable.Columns.Add("HP PN / BegNo", Type.GetType("System.String"));
            retTable.Columns.Add("TotalQty / EndNo", Type.GetType("System.String"));
            retTable.Columns.Add("Qty", Type.GetType("System.String"));            
        }

        return retTable;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //beginWaitingCoverDiv();     
   //     string Connection = CmbDBType.ddlGetConnection();
        DateTime SSS = DateTime.Parse("2012-08-26");
        string SelType = "";
        try
        {            
            DataTable dt = new DataTable();
            #region COASN
            if (rblQCondition.SelectedValue == "1")
            {
                gvResultDetail.Visible = false;
                dt = COAStatusReport.GetQueryResultByCOASN(DBConnection, txtStartSN.Text.Trim(), txtEndSN.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    this.gvResult.DataSource = null;
                    this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, DefaultType);
                    this.gvResult.DataBind();

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["COASN"].ToString();
                        gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["CUSTSN"].ToString();
                        gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Status"].ToString();
                        gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();
                    }
                    InitGridView(DefaultType);
                    EnableBtnExcel(this, true, btnExport.ClientID);
                }
                else
                {
                    BindNoData(DefaultType);
                    showErrorMessage("Not Found Any Information!!");
                }

            }
            #endregion

            #region COADate/COAStatus
            else if (rblQCondition.SelectedValue == "2" || rblQCondition.SelectedValue == "3")
            {

                SelType = "COADate";

                if (rblQCondition.SelectedValue == "2")
                {
                    DateTime ToDate = DateTime.Now;
                    string toStrDate = ddlEndDate.SelectedValue.Trim() + " 23:59:59.999";
                    ToDate = DateTime.ParseExact(toStrDate, "yyyy-MM-dd HH:mm:ss.fff", null);
                    dt = COAStatusReport.GetQueryResultByCOADate(DBConnection, DateTime.Parse(ddlStartDate.SelectedValue), ToDate);
                }
                else
                {
                    dt = COAStatusReport.GetQueryResultByCOAStatus(DBConnection, ddlCOAStatus.SelectedValue.Trim());
                }

                if (dt.Rows.Count > 0)
                {
                    //this.gvResult.DataSource = null;
                    //this.gvResult.DataSource = getNullDataTable(dt.Rows.Count,"COADate");
                    //this.gvResult.DataBind();                        
                    this.gvResult.DataSource = dt;
                    this.gvResult.DataBind();

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["IECPN"].ToString();
                        gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["Qty"].ToString();
                        gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Status"].ToString();
                        gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();
                        gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["COAStatus"].ToString();
                    }


                    InitGridView(SelType);
                    EnableBtnExcel(this, true, btnExport.ClientID);
                }
                else
                {
                    BindNoData(SelType);
                    showErrorMessage("Not Found Any Information!!");
                }
            }
            #endregion

            #region  COAAllStatus
            else if (rblQCondition.SelectedValue == "4")
            {
                SelType = "COAAllStatus";

                DataTable dtCOAStatus = COAStatus.GetCOAStatus(DBConnection);

                if (dtCOAStatus.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCOAStatus.Rows)
                    {
                        string COAName = dr["Name"].ToString().Trim();
                        COAStatusList.Add(COAName);
                    }
                }

                dt = COAStatusReport.GetQueryByCOAAllStatus(DBConnection);


                if (dt.Rows.Count > 0)
                {
                    this.gvResult.DataSource = null;
                    this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, "COAAllStatus");
                    this.gvResult.DataBind();


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        for (int j = rowCount; j <= COAStatusList.Count + (rowCount - 1); j++)
                        {
                            gvResult.Rows[i].Cells[j].Text = string.IsNullOrEmpty(dt.Rows[i][j].ToString()) ? "0" : dt.Rows[i][j].ToString();
                            if (gvResult.Rows[i].Cells[j].Text != "0")
                            {
                                gvResult.Rows[i].Cells[j].BackColor = System.Drawing.Color.Yellow;
                            }
                        }
                    }


                    InitGridView(SelType);
                    gvResultDetail.Visible = false;
                }
                else
                {
                    BindNoData(SelType);
                    showErrorMessage("Not Found Any Information!!");
                }
            }
            #endregion

            #region COARemind
            else if (rblQCondition.SelectedValue == "5")
            {
                gvResultDetail.Visible = false;
                SelType = "COARemind";
                if (ddlCOARemind.SelectedValue != "4")
                {
                    dt = COAStatusReport.GetQueryByCOARemind(DBConnection, ddlCOARemind.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        this.gvResult.DataSource = null;
                        this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, SelType);
                        this.gvResult.DataBind();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["COASN"].ToString();
                            gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["IECPN"].ToString();
                            gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Line"].ToString();
                            gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["Cdt"].ToString();
                            gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["OverDue"].ToString();
                        }
                        InitGridView(SelType);
                        EnableBtnExcel(this, true, btnExport.ClientID);
                    }
                    else
                    {
                        BindNoData(SelType);
                        showErrorMessage("Not Found Any Information!!");
                    }
                }
                else //超過5天結合未出貨
                {
                    dt = COAStatusReport.GetQueryByCOARemind(DBConnection, ddlCOARemind.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        this.gvResult.DataSource = null;
                        this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, SelType);
                        this.gvResult.DataBind();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["ProductID"].ToString();
                            gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["CUSTSN"].ToString();
                            gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["COASN"].ToString();
                            gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["IECPN"].ToString();
                            gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["CombineDate"].ToString();
                            gvResult.Rows[i].Cells[5].Text = dt.Rows[i]["DeliveryNo"].ToString();
                            gvResult.Rows[i].Cells[6].Text = dt.Rows[i]["Model"].ToString();
                            gvResult.Rows[i].Cells[7].Text = dt.Rows[i]["PGIFlag"].ToString();
                        }
                        InitGridView(SelType);
                        EnableBtnExcel(this, true, btnExport.ClientID);
                    }
                    else
                    {
                        BindNoData(SelType);
                        showErrorMessage("Not Found Any Information!!");
                    }
                }
            }
            #endregion

            #region COA Daily Check
            else if (rblQCondition.SelectedValue == "6")
            {
                gvResultDetail.Visible = false;
                if (ddlCOADailyCheck.SelectedValue == "1") //Trans領料
                {
                    SelType = "COATR_T"; // Trans/Remove
                    dt = COAStatusReport.GetQueryResultByCOADailyCheck(DBConnection, "TRANS",
                            DateTime.Parse(ddlStartDateTR.SelectedValue), DateTime.Parse(ddlEndDateTR.SelectedValue));
                    if (dt.Rows.Count > 0)
                    {
                        this.gvResult.DataSource = null;
                        this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, SelType);
                        this.gvResult.DataBind();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["BegSN"].ToString();
                            gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["EndSN"].ToString();
                            gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Qty"].ToString();
                            gvResult.Rows[i].Cells[3].Text = dt.Rows[i]["PO"].ToString();
                            gvResult.Rows[i].Cells[4].Text = dt.Rows[i]["Cdt"].ToString();
                        }
                        InitGridView(SelType);
                        EnableBtnExcel(this, true, btnExport.ClientID);
                    }
                    else
                    {
                        BindNoData(SelType);
                        showErrorMessage("Not Found Any Information!!");
                    }
                }
                else //Remove 料
                {
                    SelType = "COATR_R"; // Trans/Remove
                    dt = COAStatusReport.GetQueryResultByCOADailyCheck(DBConnection, "REMOVE",
                            DateTime.Parse(ddlStartDateTR.SelectedValue), DateTime.Parse(ddlEndDateTR.SelectedValue));
                    if (dt.Rows.Count > 0)
                    {
                        this.gvResult.DataSource = null;
                        this.gvResult.DataSource = getNullDataTable(dt.Rows.Count, SelType);
                        this.gvResult.DataBind();

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["HPPNBegNo"].ToString();
                            gvResult.Rows[i].Cells[1].Text = dt.Rows[i]["TotalEndNo"].ToString();
                            gvResult.Rows[i].Cells[2].Text = dt.Rows[i]["Qty"].ToString();                            
                        }
                        InitGridView(SelType);
                        EnableBtnExcel(this, true, btnExport.ClientID);
                    }
                    else
                    {
                        BindNoData(SelType);
                        showErrorMessage("Not Found Any Information!!");
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData(DefaultType);
        }

        endWaitingCoverDiv();
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void ShowTotal(int Count)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowTotal();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "ShowTotal", script, false);
    }
    private void BindNoData(string type)
    {
        this.gvResult.DataSource = getNullDataTable(1, type);
        this.gvResult.DataBind();
        InitGridView(type);

        if (type == DefaultType || type == "COARemind")
            return;

        this.gvResultDetail.DataSource = getNullDataTableDetail(1);
        this.gvResultDetail.DataBind();
        InitGridViewDetail();
        
        EnableBtnExcel(this, false, btnExport.ClientID);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        {
            //ToolUtility tu = new ToolUtility();
            //tu.ExportExcel(gvResult, Page.Title, Page);
            Log.logMessage("btnExport_Click Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

            GridView[] grs = new GridView[] { gvResult, gvResultDetail };
            MemoryStream ms = ExcelTool.GridViewToExcel(grs, new string[] { "总计", "具体" });
            this.Response.ContentType = "application/download";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=COAStatusReport.xls");
            this.Response.Clear();
            this.Response.BinaryWrite(ms.GetBuffer());
            Log.logMessage("btnExport_Click Start2: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

            ms.Close();
            ms.Dispose();
            Log.logMessage("btnExport_Click End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void rblQCondition_SelectedIndexChanged(object sender, EventArgs e)
    {        
        txtStartSN.Text = "";
        txtEndSN.Text = "";
        ddlCOAStatus.SelectedValue = "";
        string Condition = "";
        Condition = rblQCondition.SelectedValue;
        if (Condition == "2" || Condition == "6")
        {
            if (Condition == "2")
            {
                ddlStartDate.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");
                ddlEndDate.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                ddlStartDateTR.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");
                ddlEndDateTR.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        else
        {
            ddlStartDate.SelectedValue = "";
            ddlEndDate.SelectedValue = "";
            ddlStartDateTR.SelectedValue = "";
            ddlEndDateTR.SelectedValue = "";
        }        
    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (rblQCondition.SelectedValue == "2" || rblQCondition.SelectedValue == "3") //只有選到COADate 和COAStatus
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = 1;
                e.Row.Cells[i].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                e.Row.Cells[i].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                string IECPN = e.Row.Cells[0].Text;
                string Line = e.Row.Cells[3].Text;
                string Status = e.Row.Cells[4].Text;                
                e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('" + IECPN + "','" + Line + "' ,'" + Status + "')");
            }
        }
    }

    public void QueryDetailClick(object sender, System.EventArgs e)
    {
        try
        {            
            string IECPN = hfIECPN.Value.Trim();
            string Line = hfLine.Value.Trim();
            string Status = hfStatus.Value.Trim();
            string Connection = CmbDBType.ddlGetConnection();
            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();

            if (rblQCondition.SelectedValue=="2")
            {
                if (ddlStartDate.SelectedValue != "")
                {
                    dtFrom = DateTime.Parse(ddlStartDate.SelectedValue);
                }
                
                if (ddlEndDate.SelectedValue != "")
                {
                    dtTo = DateTime.Parse(ddlEndDate.SelectedValue);
                }
            }
            
            if (rblQCondition.SelectedValue == "3")
            {
                dtFrom = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd "));
                //dtTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));  //mark by sheng-ju
                dtTo = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd "));
            }
            
            DataTable dt = COAStatusReport.GetQueryResultDetailByCOADate(Connection,
                    dtFrom,dtTo, IECPN, Line, Status);
            
            if (dt.Rows.Count > 0)
            {
                gvResultDetail.Visible = true;
                this.gvResultDetail.DataSource = null;
                this.gvResultDetail.DataSource = getNullDataTableDetail(dt.Rows.Count);
                this.gvResultDetail.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResultDetail.Rows[i].Cells[0].Text = dt.Rows[i]["MinSnoID"].ToString();
                    gvResultDetail.Rows[i].Cells[1].Text = dt.Rows[i]["MaxSnoID"].ToString();
                    gvResultDetail.Rows[i].Cells[2].Text = dt.Rows[i]["Qty"].ToString();                    
                }
                InitGridViewDetail();
                EnableBtnExcel(this, true, btnExport.ClientID);

            }
            else
            {
                this.gvResultDetail.DataSource = getNullDataTableDetail(1);
                this.gvResultDetail.DataBind();
                InitGridViewDetail();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //BindNoData();
        }
        endWaitingCoverDiv();
    }

    private void InitGridViewDetail()
    {
        int i = 80;
        gvResultDetail.HeaderRow.Cells[0].Width = Unit.Pixel(i);
        gvResultDetail.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        gvResultDetail.HeaderRow.Cells[2].Width = Unit.Pixel(i);        
    }

    private DataTable getNullDataTableDetail(int j)
    {
        DataTable dt = initTableDetail();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["MinSnoID"] = "";
            newRow["MaxSnoID"] = "";
            newRow["Qty"] = "";            
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTableDetail()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MinSnoID", Type.GetType("System.String"));
        retTable.Columns.Add("MaxSnoID", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));        

        return retTable;
    }

}
