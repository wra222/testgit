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
//using IMES.DataModel;

public partial class Query_SA_SABANDIT : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    ISA_SABANDIT SABANDIT = ServiceAgent.getInstance().GetObjectByName<ISA_SABANDIT>(WebConstant.ISA_SABANDIT);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    private static int IdxDefectQty = 1;
    public int[] GvDetailColumnNameWidth = { 250, 250 };

    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack)
        {
            InitPage();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
        //DropDownList ddlDB = ((DropDownList)CmbDBType.FindControl("ddlDB"));
        //ddlDB.SelectedIndexChanged += new EventHandler(this.DB_SelectChange);
        //ddlDB.AutoPostBack = true;
    }

    protected void InitPage()
    {
        InitFamily();
        //InitPdLine();
        //InitStation();
    }

    protected void InitFamily()
    {
        DataTable dtFamily = intfFamily.GetPCBFamily(DBConnection);
        ddlFamily.Items.Clear();
        //ddlFamily.Items.Add(new ListItem("All", ""));
        if (dtFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dtFamily.Rows.Count; i++)
            {
                ddlFamily.Items.Add(new ListItem(dtFamily.Rows[i]["Family"].ToString(), dtFamily.Rows[i]["Family"].ToString()));
            }
        }
    }
    //string Family = hfFamily.Value;

    //protected void InitModel()
    //{

    //    List<string> lstFamily = new List<string>();
    //    lstFamily.Add(ddlFamily.SelectedValue);

    //    DataTable Model = intfPCBStationQuery.GetModel(DBConnection, lstFamily);

    //    lboxModel.Items.Clear();
    //    for (int i = 0; i < Model.Rows.Count; i++)
    //    {
    //        lboxModel.Items.Add(new ListItem(Model.Rows[i]["InfoValue"].ToString(), Model.Rows[i]["InfoValue"].ToString()));

    //    }
    //}


    //protected void InitPdLine()
    //{
    //    //CmbPdLine.Stage = "SA"; //PCA = SA
    //    //CmbPdLine.Customer = Master.userInfo.Customer;
    //    List<string> Process = new List<string>();
    //    Process.Add("SA");

    //    DataTable dtPdLine = QueryCommon.GetLine(Process, Master.userInfo.Customer, false, DBConnection);
    //    lboxPdLine.Items.Clear();
    //    if (dtPdLine.Rows.Count > 0)
    //    {
    //        foreach (DataRow dr in dtPdLine.Rows)
    //        {
    //            lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
    //        }
    //    }

    //}

    //private void DB_SelectChange(object sender, EventArgs e)
    //{
    //    //InitFamily();
    //}
    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        //InitModel();

    }

    //protected void InitStation()
    //{

    //    List<string> lstFamily = new List<string>();
    //    lstFamily.Add(ddlFamily.SelectedValue);

    //    DataTable Station = intfPCBStationQuery.GetStation(DBConnection, lstFamily);

    //    lboxStation.Items.Clear();
    //    for (int i = 0; i < Station.Rows.Count; i++)
    //    {
    //        lboxStation.Items.Add(new ListItem(Station.Rows[i]["Station"].ToString() + " - " + Station.Rows[i]["Descr"].ToString(),
    //                                        Station.Rows[i]["Station"].ToString()));

    //    }
    //}

    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        queryClick(sender, e);
    }
    protected void btnQuery1_Click(object sender, EventArgs e)
    {
        queryClick1(sender, e);
    }
    public void queryClick1(object sender, System.EventArgs e)
    {

        try
        {
            DataTable dt = getDataTable1();
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                    {
                        string mode = gvQuery.Rows[i].Cells[0].Text;
                        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell1";
                        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail1('" + gvQuery.Rows[i].Cells[0].Text + "','" + mode + "')");
                    }
                }
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

    private DataTable getDataTable1()
    {
        DataTable dt3 = null;

        DateTime timeStart = DateTime.Parse(txtStartTime.Text);
        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
        //List<string> lstPdLine = new List<string>();
        //for (int i = 0; i < lboxPdLine.Items.Count; i++)
        //{
        //    if (lboxPdLine.Items[i].Selected)
        //    {
        //        lstPdLine.Add(lboxPdLine.Items[i].Value);
        //    }
        //}

        //List<string> lstFamily = new List<string>();
        //if ((ddlFamily.SelectedIndex > 0) && (!"".Equals(ddlFamily.SelectedValue)))
        //{
        //    lstFamily.Add(ddlFamily.SelectedValue);
        //}
        string Family = ddlFamily.SelectedValue;
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

        dt3 = SABANDIT.GetQueryResult1(DBConnection, timeStart, timeEnd, Family);
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        dt.Rows[i]["FPFRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPFRate"].ToString()));
        //        dt.Rows[i]["FPYRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPYRate"].ToString()));
        //    }
        //}
        return dt3;
    }
    public void queryClick(object sender, System.EventArgs e)
    {

        try
        {
            DataTable dt = getDataTable();
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                    {
                        string mode = gvQuery.Rows[i].Cells[0].Text;
                        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell";
                        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail('" + gvQuery.Rows[i].Cells[0].Text + "','" + mode + "')");
                    }
                }
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

        DateTime timeStart = DateTime.Parse(txtStartTime.Text);
        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
        //List<string> lstPdLine = new List<string>();
        //for (int i = 0; i < lboxPdLine.Items.Count; i++)
        //{
        //    if (lboxPdLine.Items[i].Selected)
        //    {
        //        lstPdLine.Add(lboxPdLine.Items[i].Value);
        //    }
        //}

        //List<string> lstFamily = new List<string>();
        //if ((ddlFamily.SelectedIndex > 0) && (!"".Equals(ddlFamily.SelectedValue)))
        //{
        //    lstFamily.Add(ddlFamily.SelectedValue);
        //}
        string Family = ddlFamily.SelectedValue;
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

        dt = SABANDIT.GetQueryResult(DBConnection, timeStart, timeEnd, Family);
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gvQuery.HeaderRow == null)
            return;
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvQuery, "PCBTESTQuery", Page);
    }
    protected void btnDetailExport_Click(object sender, EventArgs e)
    {
        if (gvStationDetail.HeaderRow == null)
            return;
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvStationDetail, "PCBTESTDetailQuery", Page);
    }

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

    private void InitGridView_Detail()
    {
        for (int r = 0; r <= GvDetailColumnNameWidth.Length; r++)
        {
            gvStationDetail.HeaderRow.Cells[r].Width = Unit.Pixel(GvDetailColumnNameWidth[r]);
        }
    }

    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt2 = null;
            string mode = hfmode.Value.Trim();
            DateTime timeStart = DateTime.Parse(txtStartTime.Text);
            DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
            //List<string> lstPdLine = new List<string>();
            //for (int i = 0; i < lboxPdLine.Items.Count; i++)
            //{
            //    if (lboxPdLine.Items[i].Selected)
            //    {
            //        lstPdLine.Add(lboxPdLine.Items[i].Value);
            //    }
            //}
            string Family = ddlFamily.SelectedValue;
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

            dt2 = SABANDIT.GetSelectDetail(DBConnection, timeStart, timeEnd, Family, mode);
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
    public void btnQueryDetail1_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt4 = null;
            string mode = hfmode.Value.Trim();
            DateTime timeStart = DateTime.Parse(txtStartTime.Text);
            DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
            //List<string> lstPdLine = new List<string>();
            //for (int i = 0; i < lboxPdLine.Items.Count; i++)
            //{
            //    if (lboxPdLine.Items[i].Selected)
            //    {
            //        lstPdLine.Add(lboxPdLine.Items[i].Value);
            //    }
            //}
            string Family = ddlFamily.SelectedValue;
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

            dt4 = SABANDIT.GetSelectDetail1(DBConnection, timeStart, timeEnd, Family, mode);
            gvStationDetail.DataSource = dt4;
            gvStationDetail.DataBind();
            gvStationDetail.Visible = true;
            if (dt4.Rows.Count > 0)
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

}
