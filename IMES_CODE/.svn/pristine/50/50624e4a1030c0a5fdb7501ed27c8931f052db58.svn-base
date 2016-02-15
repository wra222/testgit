using System;
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
using com.inventec.imes.DBUtility;
using System.Timers;
//using IMES.DataModel;

public partial class Query_PAK_Warehouse1 : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);


    IPAK_Warehouse1 PAK_Warehouse1 = ServiceAgent.getInstance().GetObjectByName<IPAK_Warehouse1>(WebConstant.IPAK_Warehouse1);
    //DataTable dt = PAK_Common.GetWarehouseDashBoardData(Connection);
    //IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    //ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    //IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    private static int IdxDefectQty = 5;
    private static int FixedColCount=3;
    //public int[] GvDetailColumnNameWidth = { 110, 110, 80, 60, 90, 70, 80, 80, 80, 140, 150, 150 };

    String DBConnection = "";
    String ShipDate;
    public int[] GvDetailColumnNameWidth = { 250, 250 };
    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        string name= Request["DBName"] ?? configDefaultDB;
        DBConnection = CmbDBType.ddlGetConnection();
        Label3.Text =DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
      
        //DBConnection = Request["DBName"] ?? configDefaultDB;
      if (!IsPostBack)
        {         
            txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ShipDate = txtShipDate.Text;
        }
      ShipDate = txtShipDate.Text;
      InitPage();     
    }

    protected void InitPage() {
        WarehouseDB();
        WarehouseDB2();
    }

    protected void btnShipDate_Click()
    {
        InitPage();
        Label3.Text = "11";
    }

    public void WarehouseDB()
    {                

        try
        {
            ShipDate = txtShipDate.Text;
            DataTable dt = PAK_Warehouse1.GetWarehouseDashBoardData(DBConnection, DateTime.Parse(ShipDate));
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                //    {
                //        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell";
                //        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail('" + gvQuery.Rows[i].Cells[0].Text + "')");
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    public void WarehouseDB2()
    {

        try
        {
            ShipDate = txtShipDate.Text;
            DataTable dt3 = PAK_Warehouse1.GetWarehouseDashBoardData2(DBConnection, DateTime.Parse(ShipDate));
            gvQuery2.DataSource = dt3;
            gvQuery2.DataBind();
            if (dt3.Rows.Count > 0)
            {
                gvQuery2.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery2.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                //    {
                //        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell";
                //        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail('" + gvQuery.Rows[i].Cells[0].Text + "')");
                //    }
                //}
                for (int j = IdxDefectQty; j <= dt3.Columns.Count - 1; j++)
                {

                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        string r_Status = "";
                        r_Status = dt3.Columns[j].ColumnName.ToString();
                        if (gvQuery2.Rows[i].Cells[j].Text != "0")
                        {

                            string MAWB = gvQuery2.Rows[i].Cells[3].Text;
                            string ShipDate1 = gvQuery2.Rows[i].Cells[0].Text;
                            gvQuery2.Rows[i].Cells[j].CssClass = "querycell";
                            gvQuery2.Rows[i].Cells[j].Attributes.Add("onclick", "SelectDetail('" + gvQuery2.Rows[i].Cells[3].Text + "','" + r_Status + "','" + ShipDate1 + "')");
                        }
                    }
                }
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                //    {
                //        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell";
                //        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail('" + gvQuery.Rows[i].Cells[0].Text + "')");
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private DataTable getDataTable()
    {
        DataTable dt = null;

        //DateTime shipDate = DateTime.Parse(txtShipDate.Text);

        dt = PAK_Warehouse1.GetWarehouseDashBoardData2(DBConnection, DateTime.Parse(ShipDate));        
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        dt.Rows[i]["FPFRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPFRate"].ToString()));
        //        dt.Rows[i]["FPYRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPYRate"].ToString()));
        //    }     
        //}
        return dt;
    }
    //protected void btnExport_Click(object sender, EventArgs e)
    //{
    //    if (gvQuery.HeaderRow == null)
    //        return;
    //    ToolUtility tu = new ToolUtility();
    //    tu.ExportExcel(gvQuery, "PCBStationQuery", Page);
    //}
    //protected void btnDetailExport_Click(object sender, EventArgs e)
    //{
    //    if (gvStationDetail.HeaderRow == null)
    //        return;
    //    ToolUtility tu = new ToolUtility();
    //    tu.ExportExcel(gvStationDetail, "PCBStationDetailQuery", Page);
    //}

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    //public void btnQueryDetail_Click(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        DateTime timeStart = DateTime.Parse(txtStartTime.Text);
    //        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
    //        List<string> lstPdLine = new List<string>();
    //        for (int i = 0; i < lboxPdLine.Items.Count; i++)
    //        {
    //            if (lboxPdLine.Items[i].Selected)
    //            {
    //                lstPdLine.Add(lboxPdLine.Items[i].Value);
    //            }
    //        }

    //        string Family = hfFamily.Value;
    //        List<string> lstModel = new List<string>();
    //        for (int i = 0; i < lboxModel.Items.Count; i++)
    //        {
    //            if (lboxModel.Items[i].Selected)
    //            {
    //                lstModel.Add(lboxModel.Items[i].Value);
    //            }
    //        }
    //        List<string> lstStation = new List<string>();
    //        for (int i = 0; i < lboxStation.Items.Count; i++)
    //        {
    //            if (lboxStation.Items[i].Selected)
    //            {
    //                lstStation.Add(lboxStation.Items[i].Value);
    //            }
    //        }

    //        DataTable dt = intfPCBStationQuery.GetSelectDetail(DBConnection, timeStart, timeEnd, lstPdLine, Family, lstModel, lstStation);
    //        gvStationDetail.DataSource = dt;
    //        gvStationDetail.DataBind();
    //        gvStationDetail.Visible = true;
    //        if (dt.Rows.Count > 0)
    //        {
    //            InitGridView_Detail();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        writeToAlertMessage(ex.ToString());
    //    }
    //    finally
    //    {
    //        endWaitingCoverDiv();
    //    }
    //}
    protected void Button2_Click(object sender, EventArgs e)
    {
      
        InitPage();
    }

    private void InitGridView_Detail()
    {
        for (int r = 0; r <= GvDetailColumnNameWidth.Length; r++)
        {
            gvStationDetail.HeaderRow.Cells[r].Width = Unit.Pixel(150);
        }
    }


  

    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt2 = null;
            string MAWB = hfMAWB.Value.Trim();
            string Status = hfStatus.Value.Trim();
            string ShipDate1 = hfShipDate1.Value.Trim();
            
            //string Status = hfStatus.Value.Trim();
            //string ShipDate = hfShipDate.Value.Trim();
            //DateTime timeStart = DateTime.Parse(txtStartTime.Text);
            //DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
            //List<string> lstPdLine = new List<string>();
            //for (int i = 0; i < lboxPdLine.Items.Count; i++)
            //{
            //    if (lboxPdLine.Items[i].Selected)
            //    {
            //        lstPdLine.Add(lboxPdLine.Items[i].Value);
            //    }
            //}
            //string Family = ddlFamily.SelectedValue;
            //string Family = hfFamily.Value;
            //List<string> lstModel = new List<string>();
            //for (int i = 0; i < lboxModel.Items.Count; i++)
            //{
            //    if (lboxModel.Items[i].Selected)
            //    {
            //        lstModel.Add(lboxModel.Items[i].Value);
            //    }
            //}
            //List<string> lstStation = new List<string>();
            //for (int i = 0; i < lboxStation.Items.Count; i++)
            //{
            //    if (lboxStation.Items[i].Selected)
            //    {
            //        lstStation.Add(lboxStation.Items[i].Value);
            //    }
            //}

            //dt2 = PAK_Warehouse1.GetWarehouseDashBoardData_Detail(DBConnection, ShipDate,MAWB,Status);
            dt2 = PAK_Warehouse1.GetWarehouseDashBoardData_Detail(DBConnection, MAWB,Status,ShipDate1);
            gvStationDetail.DataSource = dt2;
            gvStationDetail.DataBind();
            gvStationDetail.Visible = true;
            if (dt2.Rows.Count > 0)
            {
                InitGridView_Detail();
            }
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    protected void hfMAWB_ValueChanged(object sender, EventArgs e)
    {

    }
    protected void gvQuery_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}



 
