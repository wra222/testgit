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
//using IMES.Station.Interface.CommonIntf;
using System.Data;

public partial class Query_FA_PoWIPTracking : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    IFA_PoWIPTracking PoWIPTracking = ServiceAgent.getInstance().GetObjectByName<IFA_PoWIPTracking>(WebConstant.PoWIPTracking);
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    List<string> StationList = new List<string>();
    public int rowCount =3;
    public string PAKStation = WebCommonMethod.getConfiguration("PAKOnlineStation");
    public string FAOnlineStation = WebCommonMethod.getConfiguration("FAOnlineStation");
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    DataTable dtStation;
    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DBConnection = CmbDBType.ddlGetConnection();
            if (!this.IsPostBack)
            {
             

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
        this.lblShipDate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
        this.lblPoNo.Text = this.GetLocalResourceObject(Pre + "_lblPoNo").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblTotalQty.Text = this.GetLocalResourceObject(Pre + "_lblTotalQty").ToString();
        this.lblActualQty.Text = this.GetLocalResourceObject(Pre + "_lblActualQty").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }
    private void InitCondition()
    {
        GetLine();
        GetFamily();
         dtStation = Station.GetStation(DBConnection);
        string customer = Master.userInfo.Customer;       
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtDNDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        hidDNDate.Value = DateTime.Now.ToString("yyyy-MM-dd"); 
        /*DataTable dtModel = Model.GetModel();

        if (dtModel.Rows.Count > 0)
        {
            ddlModel.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtModel.Rows)
            {
                ddlModel.Items.Add(new ListItem(dr["Model"].ToString().Trim(), dr["Model"].ToString().Trim()));
            }
        }*/
        SetStationList();
       this.gvResult.DataSource = getNullDataTable(1);
       this.gvResult.DataBind();
           InitGridView();
    }
    private void InitGridView()
    {
        int i = 100;
        int j = 80;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(140);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(70);
      //  gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        
       // for (int r = rowCount; r <= StationList.Count + (rowCount - 1); r++)
        for (int r = rowCount; r < gvResult.HeaderRow.Cells.Count ; r++)
        
        {
            gvResult.HeaderRow.Cells[r].Width = Unit.Pixel(70);
        }
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
          

            foreach (string STN in StationList)
            {
                newRow[STN] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));

        foreach (string STN in StationList)
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));
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
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
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
            //foreach (DataRow dr in dtPdLine.Rows)
            //{
            //    lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            //}
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() , dr["Line"].ToString().Trim()));
            }
        }
    }
    private string GetTipString(string descr)
    {
        string s = @"Tip('{0}',SHADOW, true, SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300)";
        s = string.Format(s, descr);
        return s;
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        gvResult.GvExtHeight = "400px";
        CallReset();
        //beginWaitingCoverDiv();     
        //Keep Value fro Select Detail using
        hidOriDate.Value = txtShipDate.Text.Trim();
        hidOriFamily.Value = ddlFamily.SelectedValue;
        hidOriModelType.Value = hidModelType.Value;

        //Keep Value fro Select Detail using

        string shipdate = "";
        string modelList = "";
        if (hidModelType.Value == "1")
        { shipdate = txtShipDate.Text.Trim(); }
        else
        {
            hidOriDate.Value = "";
            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtModel.Text = txtModel.Text.Replace("'", "");
            string[] modelArr = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (modelArr.Length == 0)
            {
                modelList = "";
                //modelList = string.Join("','", modelArr);
            }
            else
            { modelList = hidModelList.Value; }
           
            if (!string.IsNullOrEmpty(txtModel.Text))
            {
                modelList = txtModel.Text.Trim();
              //  modelList = "'" + modelList + "'";
            }
            hidOriModelList.Value = modelList;
        
        }
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
            string lineList = "";
            foreach (ListItem item in lboxPdLine.Items)
            {
                if (item.Selected)
                {
                    lineList = lineList + item.Value.Trim() + ",";
                }
            }
            hidOriLine.Value = lineList;


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

            DataSet ds = PoWIPTracking.GetDNShipQty(Connection, DateTime.Parse(txtShipDate.Text), txtPoNo.Text.Trim(),
                            txtModel.Text.Trim(), "2");

            lblTotalQtyCount.Text = ds.Tables[0].Rows[0][0].ToString();
            lblActualQtyCount.Text= ds.Tables[1].Rows[0][0].ToString();

            //DataTable dt = PoWIPTracking.GetQueryResult(Connection, DateTime.Parse(txtShipDate.Text),txtPoNo.Text.Trim(),
            //                txtModel.Text.Trim(), "2");
            DataTable dt = PoWIPTracking.GetQueryResult2(Connection, shipdate, modelList, lineList, stationList, droGroupType.SelectedValue, ddlFamily.SelectedValue,hidDNDate.Value);
           // EnableBtnExcel(this, true, btnExport.ClientID);
            if (dt.Rows.Count > 0)
            {
                EnableBtnExcel(this, true, btnExport.ClientID);
         

               
               this.gvResult.DataSource = dt;
               this.gvResult.DataBind();
             
               if (dtStation == null) { dtStation = Station.GetStation(DBConnection); }
               string descr = "";
               string tipCmd = "";
               for (int i = 3; i < gvResult.HeaderRow.Cells.Count; i++)
               {
                   descr = GetStationDescr(dtStation, gvResult.HeaderRow.Cells[i].Text.Trim());
                   tipCmd = GetTipString(descr);
                   gvResult.HeaderRow.Cells[i].Attributes.Add("onmouseover", tipCmd);
                   gvResult.HeaderRow.Cells[i].Attributes.Add("onmouseout", "UnTip()");
               }
                //Calc Total
               if (droGroupType.SelectedValue!="ALL")
               {
                   int[] sum = new int[dt.Columns.Count];
                   gvResult.FooterStyle.Font.Bold = true;
                   gvResult.FooterRow.Cells[0].Text = "";
                   gvResult.FooterRow.Cells[1].Text = "";
                   gvResult.FooterRow.Cells[2].Text = "TOTAL";
                   gvResult.FooterRow.Cells[2].Font.Size = FontUnit.Parse("16px");
                 for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                  
                    for (int j = rowCount; j <= dt.Columns.Count -1; j++)
                    {
                      
                  
                            sum[j] = sum[j] + int.Parse(gvResult.Rows[i].Cells[j].Text);
                     } 
                   
                }



                   for (int j = rowCount; j <= dt.Columns.Count - 1; j++)
                   {
                       gvResult.FooterRow.Cells[j].Text = sum[j].ToString();
                       gvResult.FooterRow.Cells[j].Font.Bold = true;
                       gvResult.FooterRow.Cells[j].Font.Size = FontUnit.Parse("14px");
                   }
                   gvResult.FooterRow.Visible = true;
                   gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;

               }

                //Calc Total

               
              
                           
                InitGridView();
                gvStationDetail.Visible = false;
            
            }
            else
            {
                EnableBtnExcel(this, false, btnExport.ClientID);
      
               
                BindNoData();

                showErrorMessage("Not Found Any Information!!");
                
            }
           // CallTip();
           
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }

        endWaitingCoverDiv();
        ShowTotal(lblTotalQtyCount.Text, lblActualQtyCount.Text);
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv(); " + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void CallTip()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "calltip();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void BindNoData()
    {
        //EnableBtnExcel(this, false, btnExportDetail.ClientID);
        //EnableBtnExcel(this, false, btnExport.ClientID);
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();

        gvStationDetail.Visible = false;
        /*if (gvStationDetail.Rows.Count > 0)
        {
            this.gvStationDetail.DataSource = getNullDataTableDetail(1);
            this.gvStationDetail.DataBind();
            InitGridViewDetail();
        }*/

  
        
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);                        
    }

    protected void btnExportDetail_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();     
        tu.ExportExcel(gvStationDetail, Page.Title, Page);
    }
    private void ShowTotal(string TotalQtyCount,string ActualQtyCount)
    {        
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowTotal('"+TotalQtyCount+"','"+ActualQtyCount+"');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "ShowTotal", script, false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "ShowTotal", script, false);        
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    string modeltemp = "";
    int count = 0; //#64E986   C3FDB8
    System.Drawing.Color[] bgc = { System.Drawing.ColorTranslator.FromHtml("#C3FDB8"), System.Drawing.ColorTranslator.FromHtml("#64E986") };
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
      string StationWC ="";
      string Line = "";
      string Model = "";
  
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
          int idx = 1;
          if (droGroupType.SelectedValue == "Line") { idx = 2; }

          if (modeltemp == "") {
              modeltemp = e.Row.Cells[idx].Text;
          }

          if (e.Row.Cells[idx].Text == modeltemp) { }
          else {
              modeltemp = e.Row.Cells[idx].Text;
              count += 1;
          }
          e.Row.BackColor = bgc[count %2];

          
          Model = e.Row.Cells[1].Text.Trim();
          Line = e.Row.Cells[2].Text.Trim();
          if (Model == "ALL") { Model = ""; }
          if (Line == "ALL") { Line = ""; }
          for (int i = 3; i < e.Row.Cells.Count; i++)
          {
              StationWC = gvResult.HeaderRow.Cells[i].Text.Trim();
             
              if (e.Row.Cells[i].Text == "&nbsp;")
              { e.Row.Cells[i].Text = "0"; }
              else
              {
                  e.Row.Cells[i].BackColor=System.Drawing.ColorTranslator.FromHtml("#F78181");
                  e.Row.Cells[i].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                  e.Row.Cells[i].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                  e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('" + StationWC + "','" + Line + "','" + Model + "')");
              }
          }
      }
    }
    private void GetFamily()
    {
        DataTable dtPno = Family.GetFamily(DBConnection);

        if (dtPno.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtPno.Rows)
            {
                ddlFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
        }
    }
    public void ChangeLine_S(object sender, System.EventArgs e)
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine =null ;
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
        CallReset();
        endWaitingCoverDiv();
    }
    public void QueryDetailClick(object sender, System.EventArgs e)
    {
        try
        {
            gvResult.GvExtHeight = "200px";
            CallReset();
            string Station = hfStation.Value;
            string Line = hfLine.Value;
            string Model = hfModel.Value;
         
            if (Model == "")
            { Model = hidOriModelList.Value; }
            if (Line == "" &&  hidOriLine.Value!="")
            { Line = hidOriLine.Value; }
            string Connection = CmbDBType.ddlGetConnection();
            DataTable dt = PoWIPTracking.GetSelectDetail2(Connection, hidOriDate.Value, Model, Line,
                                                                                     hfStation.Value,hidOriFamily.Value);
                                
            if (dt.Rows.Count > 0)
            {
                gvStationDetail.Visible = true;
              
                this.gvStationDetail.DataSource = dt;
           //     this.gvStationDetail.DataSource = getNullDataTableDetail(dt.Rows.Count);
                this.gvStationDetail.DataBind();

                //for (int i = 0; i <= dt.Rows.Count - 1; i++)
                //{
                //    gvStationDetail.Rows[i].Cells[0].Text = dt.Rows[i]["ProductID"].ToString();
                //    gvStationDetail.Rows[i].Cells[1].Text = dt.Rows[i]["CUSTSN"].ToString();
                //    gvStationDetail.Rows[i].Cells[2].Text = dt.Rows[i]["Descr"].ToString();
                //    gvStationDetail.Rows[i].Cells[3].Text = dt.Rows[i]["Line"].ToString();
                //    gvStationDetail.Rows[i].Cells[4].Text = dt.Rows[i]["Editor"].ToString();                    
                //}
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
        endWaitingCoverDiv();
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
    public void CallReset()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "Reset();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "CallReset", script, false);
    }

    protected void droGroupType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tmp = "SetChxDN(0);";
        if (droGroupType.SelectedValue.IndexOf("Model") >= 0)
        {
            tmp = "SetChxDN(1);";

        }
   
            String script = "<script language='javascript'>" +tmp+  "</script>";
            ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "CallResetDNChx", script, false);
            CallReset();
       
    }
}
