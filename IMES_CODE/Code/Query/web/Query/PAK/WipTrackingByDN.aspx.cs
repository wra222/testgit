using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
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


public partial class WipTrackingByDN : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    IFA_PoWIPTracking PoWIPTracking = ServiceAgent.getInstance().GetObjectByName<IFA_PoWIPTracking>(WebConstant.PoWIPTracking);
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    List<string> StationList = new List<string>();
    public int rowCount = 8;
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    DataTable dtStation;
    String DBConnection = "";
    string dbName;
    protected void Page_Load(object sender, EventArgs e)
    {
        //
        string Connection = CmbDBType.ddlGetConnection();
        DBConnection = CmbDBType.ddlGetConnection();
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        dbName = Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
     
        if (!this.IsPostBack)
        { InitCondition();
      
        }
        
    }
    
    public void QueryDetailClick(object sender, System.EventArgs e)
    {
        try
        {
    
            string Line = hfLine.Value;
            string Model = hfModel.Value;
            if (Model == "")
            { Model = hidOriModelList.Value; }
            if (Line == "&nbsp;") { Line = ""; }
            if (Line == "" && hidOriLine.Value != "")
            { Line = hidOriLine.Value; }
            string Connection = CmbDBType.ddlGetConnection();
            if (FAOnlineStation.IndexOf(hfStation.Value) > -1)
            {
                hidSelectDN.Value = "";
            }

            DataTable dt = PoWIPTracking.GetSelectDetail2(Connection, hidOriDate.Value, Model, Line,
                                                                                     hfStation.Value, hidSelectDN.Value);

            if (dt.Rows.Count > 0)
            {
                gvStationDetail.Visible = true;
                this.gvStationDetail.DataSource = dt;
                this.gvStationDetail.DataBind();
                 InitGridViewDetail();
            
            }
            else
            {
                this.gvStationDetail.DataSource = getNullDataTableDetail(1);
                this.gvStationDetail.DataBind();
                InitGridViewDetail();

                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }
        finally
        {
            endWaitingCoverDiv();
        }
        
    }
    private void InitGridViewDetail()
    {
        int i = 50;
        int j = 200;
        gvStationDetail.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        gvStationDetail.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        gvStationDetail.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        gvStationDetail.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        gvStationDetail.HeaderRow.Cells[4].Width = Unit.Pixel(i);
        gvStationDetail.HeaderRow.Cells[5].Width = Unit.Pixel(i);
        gvStationDetail.HeaderRow.Cells[6].Width = Unit.Pixel(j);
    }
    private DataTable getNullDataTableDetail(int j)
    {
        DataTable dt = initTableDetail();
        DataRow newRow = null;
        for (int i = 0; i < j; i++) //ProductID,CUSTSN,Model,DeliveryNo,Line,Station,StationDescr
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["CUSTSN"] = "";
            newRow["Model"] = "";
            newRow["DeliveryNo"] = "";
            newRow["Line"] = "";
            newRow["Station"] = "";
            newRow["StationDescr"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTableDetail()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("DeliveryNo", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("StationDescr", Type.GetType("System.String"));

        return retTable;
    }
    private void BindNoData()
    {
   
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        gvStationDetail.Visible = false;
    }
    private void InitGridView()
    {
        int i = 100;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(140);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(120);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(70);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(70);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(70);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(70);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(70);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(70);
       
        for (int r = rowCount; r < gvResult.HeaderRow.Cells.Count; r++)
        {
            gvResult.HeaderRow.Cells[r].Width = Unit.Pixel(70);
        }
    }
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
      errorMsg=  errorMsg.Replace("'", "");
       errorMsg= errorMsg.Replace(@"""","");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
       
        string Connection = CmbDBType.ddlGetConnection();
        hidOriDate.Value = txtShipDate.Text.Trim();
        hidOriModelType.Value = hidModelType.Value;
        //Get select Model List by ShipDate or Input Model List 
        DateTime dTimeShipe=DateTime.Parse(txtShipDate.Text.Trim());
   
           string modelList = "";
     
            hidOriDate.Value = "";
            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtModel.Text = txtModel.Text.Replace("'", "");
            string[] modelArr = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
           
            if (modelArr.Length == 0)
            {
                modelList = "";
            }
            else
            { modelList = hidModelList.Value; }

            if (!string.IsNullOrEmpty(txtModel.Text))
            {
                modelList = txtModel.Text.Trim();
            }
            hidOriModelList.Value = modelList;

        
        //Get select Model List by ShipDate or Input Model List 

        //Get Line List
        string lineList = "";
        string IsShiftLine = "0";
        foreach (ListItem item in lboxPdLine.Items)
        {
            if (item.Selected)
            {
   
                lineList = lineList + item.Value.Trim() + ",";
            }
        }
        if (lboxPdLine.Items[0].Value.Trim().Length == 1) { IsShiftLine = "1"; }
        hidOriLine.Value = lineList;
        //Get Line List

        string stationList = "";
        if (radProcess.SelectedValue == "ALL")
        { stationList = FAOnlineStation + "," + PAKOnlineStation; }
        else if (radProcess.SelectedValue == "FA")
        { stationList = FAOnlineStation; PAKOnlineStation = ""; }
        else //PAK
        { stationList = PAKOnlineStation; FAOnlineStation = ""; }
             
           string[] arrStation = stationList.Split(',');
           foreach (string s in arrStation)
           {
               StationList.Add(s.Trim());
           }
           



        
         
      //     DataTable dt2 = PoWIPTracking.GetDNShipQty(Connection, dTimeShipe, PAKStation);
         
           DataTable dt = null;
           int[] dnQty;
           try
           { 
              dt = PoWIPTracking.GetQueryResultBySP(Connection, txtShipDate.Text, FAOnlineStation, PAKOnlineStation, modelList, lineList, 
                    IsShiftLine, droGroupType.SelectedValue,dbName,out dnQty);
           
           }
           catch(Exception exp)
           { showErrorMessage(exp.ToString()); return; }
           gvResult.DataSource = dt;
           if (dt.Rows.Count > 0)
           {
               lblTotalQtyCount.Text = dnQty[0].ToString();
               lblActualQtyCount.Text = dnQty[1].ToString();
              
               
               gvResult.Visible = true;
               EnableBtnExcel(this, true, btnExport.ClientID);
        
               this.gvResult.DataSource = dt;
               this.gvResult.DataBind();
         
               
               if (dtStation == null) { dtStation = Station.GetStation(DBConnection); }
               string descr = "";
               string tipCmd = "";
               for (int i = 7; i < gvResult.HeaderRow.Cells.Count; i++)
               {
                   descr = GetStationDescr(dtStation, gvResult.HeaderRow.Cells[i].Text.Trim());
                   tipCmd = GetTipString(descr);
                   gvResult.HeaderRow.Cells[i].Attributes.Add("onmouseover", tipCmd);
                   gvResult.HeaderRow.Cells[i].Attributes.Add("onmouseout", "UnTip()");
             //      gvResult.HeaderRow.Cells[3].CssClass = "FreezingCol ";

               }
               //Calc Total
               if (radProcess.SelectedValue=="PAK")
               {
                 
                   rowCount--;
                   int[] sum = new int[dt.Columns.Count];
                   gvResult.FooterStyle.Font.Bold = true;
                   gvResult.FooterRow.Cells[0].Text = "";
                   gvResult.FooterRow.Cells[1].Text = "";
                   gvResult.FooterRow.Cells[2].Text = "TOTAL";
                   gvResult.FooterRow.Cells[2].Font.Size = FontUnit.Parse("16px");
                   for (int i = 0; i <= dt.Rows.Count - 1; i++)
                   {

                       for (int j = 3; j <= dt.Columns.Count - 1; j++)
                       {

                           if (gvResult.Rows[i].Cells[j].Text != "&nbsp;" && gvResult.Rows[i].Cells[j].Text != "")
                           {
                              sum[j] = sum[j] + int.Parse(gvResult.Rows[i].Cells[j].Text);
                           }
                        
                       }

                   }



                   for (int j = 3; j <= dt.Columns.Count - 1; j++)
                   {
                       gvResult.FooterRow.Cells[j].Text = sum[j].ToString();
                       gvResult.FooterRow.Cells[j].Font.Bold = true;
                       gvResult.FooterRow.Cells[j].Font.Size = FontUnit.Parse("14px");
                   }
                   gvResult.FooterRow.Visible = true;
                   gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;

               }
               gvResult.HeaderRow.Cells[4].BorderColor = System.Drawing.Color.Red;
               gvResult.HeaderRow.Cells[5].BorderColor = System.Drawing.Color.Red;
               gvResult.HeaderRow.Cells[6].BorderColor = System.Drawing.Color.Red;
               gvResult.HeaderRow.Cells[7].BorderColor = System.Drawing.Color.Red;
             
            
               InitGridView();
               gvStationDetail.Visible = false;
           
           }
           else
           {
               EnableBtnExcel(this, false, btnExport.ClientID);
               lblTotalQtyCount.Text = "0";
               lblActualQtyCount.Text = "0";
               gvResult.Visible = false;
           //    BindNoData();
               gvStationDetail.Visible = false;
               showErrorMessage("Not Found Any Information!!");

           }
      //     ResetGrv();
       endWaitingCoverDiv();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, "Data", Page);
    }
    private string GetStationDescr(DataTable dt, string station)
    {
        string descr = "";
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Station"].ToString().Trim() == station)
            {
                descr = dr["Descr"].ToString().Trim();
                return descr;
            }
        }
        return descr;
    }
    private string GetTipString(string descr)
    {
        string s = @"Tip('{0}',SHADOW, true, SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300)";
        s = string.Format(s, descr);
        return s;
    }
    public void ChangeLine_S(object sender, System.EventArgs e)
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = null;
        bool IsOnlyShift = false;
        if (lboxPdLine.Items[0].Text.Length == 1)
        { dtPdLine = QueryCommon.GetLine(Process, customer, false, DBConnection); IsOnlyShift = true; }
        else
        { dtPdLine = QueryCommon.GetLine(Process, customer, true, DBConnection); }

        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            if (!IsOnlyShift)
            {
                foreach (DataRow dr in dtPdLine.Rows)
                {
                    lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
                }
            }
            else
            {
                foreach (DataRow dr in dtPdLine.Rows)
                {
                    lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
                }
            }


        }
        HideWait();


    }
    private void InitCondition()
    {
        GetLine();
        string customer = Master.userInfo.Customer;
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        SetStationList();
       
    }
    private void SetStationList()
    {

        string stationList = "";
        if (radProcess.SelectedValue == "ALL")
        { stationList = FAOnlineStation + "," + PAKStation; }
        else if (radProcess.SelectedValue == "FA")
        { stationList = FAOnlineStation; }
        else //PAK
        { stationList = PAKStation; }

        string[] arrStation = stationList.Split(',');
        foreach (string s in arrStation)
        {
            StationList.Add(s.Trim());
        }
    }
    private void GetLine()
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, true, DBConnection);
        if (dtPdLine.Rows.Count > 0)
        {
           
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
   
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "EndRequestHandler();endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
     //   ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    // HideWait();
    private void HideWait()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "HideWait();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "HideWait", script, false);
        //   ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    
    private DataTable initTable()
    {
       
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        foreach (string STN in StationList)
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));
        }

        return retTable;
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["DN"] = "";
            newRow["Model"] = "";
            newRow["Line"] = "";
            newRow["Qty"] = "";

            foreach (string STN in StationList)
            {
                newRow[STN] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    string modeltemp = "";
    int count = 0; //#64E986   C3FDB8
    System.Drawing.Color[] bgc = { System.Drawing.ColorTranslator.FromHtml("#F1F7F9"), System.Drawing.ColorTranslator.FromHtml("#EEF3F7") };
    string[] bgclass = { "row1", "row2" };
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string StationWC = "";
        string Line = "";
        string Model = "";
       // e.Row.Cells[0].CssClass = "FreezingCol";
        string DN = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 1;

          
            if (e.Row.Cells[idx].Text == modeltemp)
            {
               // e.Row.CssClass = "row1";
               // e.Row.Cells[4].Text = "";
                e.Row.Cells[5].Text = "";
                e.Row.Cells[6].Text = "";
                e.Row.Cells[7].Text = "";

            }
            else
            {
                modeltemp = e.Row.Cells[idx].Text;
                count += 1;
               // e.Row.CssClass = "row2";
            }
            //e.Row.BackColor = bgc[count % 2];

            e.Row.CssClass = bgclass[count % 2];
            DN = e.Row.Cells[0].Text.Trim();
            Model = e.Row.Cells[1].Text.Trim();
            Line = e.Row.Cells[2].Text.Trim();
            if (Model == "ALL") { Model = ""; }
            if (Line == "ALL") { Line = ""; }
            for (int i =8; i < e.Row.Cells.Count; i++)
            { 
                StationWC = gvResult.HeaderRow.Cells[i].Text.Trim();

                if (e.Row.Cells[i].Text == "&nbsp;" || e.Row.Cells[i].Text == "0")
                {
                 //   e.Row.Cells[i].Text = "0";
                  //  e.Row.Attributes.Add("onclick", "ChangeRowColor()");
                }
                else
                {
                    e.Row.Cells[i].CssClass = "querycell";
                //    e.Row.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#F78181");
                //    e.Row.Cells[i].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                  //  e.Row.Cells[i].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                    e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('" + StationWC + "','" + Line + "','" + Model +  "','"+DN+ "')");
                }
          


            }
        }
    }
  
}
