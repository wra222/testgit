using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using com.inventec.iMESWEB;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class SA_SAInputXRay : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;
    protected string station;
    //protected string AlertSelectModel = "please select Model ";
    //protected string AlertSelectPdline = "please select Pdline";
    //protected string AlertSelectLocation = "please select Location";
    //protected string AlertSelectObligation = "please select Obligation";
    //protected string AlertInputMBSno = "please input MB Sno!";
    //protected string AlertSelectstate = "please Selectstate!";
    //protected string AlertErrInput = "Wrong code,please input MB Sno!";
    //protected string AlertSuccess = "Save successfully!";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
            station = Request["Station"] ?? "";
            this.cmbPdLine.Station = station;
            this.cmbPdLine.Customer = Customer;
            this.cmbPdLine.Stage = "XRay";
            cmbConstValueType1.Type = "XRayModel";
            cmbConstValueType2.Type = "XRayLocation";
            cmbConstValueType3.Type = "XRayObligation";
            cmbConstValueType4.Type = "XRayState";
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
        }
    }
    /// <summary>
    /// 查询数据，刷新gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void QueryList(object sender, System.EventArgs e)
    {
        try
        {
            beginWaitingCoverDiv();
            this.GridViewExt1.DataSource = GetXRayList();
            this.GridViewExt1.DataBind();
            QueryFinished();
        }
        catch (FisException fe)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(fe.mErrmsg);
        }
        catch (Exception ex)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
            BtnQuery.Focus();
        }

    }

    public void ToExcel(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dtData = GetXRayList();
            System.Web.UI.WebControls.DataGrid dgExport = null;
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;

            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "UTF-8";
                curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312 >");

                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
                dgExport = new System.Web.UI.WebControls.DataGrid();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();

                for (int i = 0; i < dgExport.Items.Count; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    }
                }

                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }
        catch (FisException fe)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(fe.mErrmsg);
        }
        catch (Exception ex)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }
    private DataTable GetXRayList()
    {
        DataTable dt = InitTable();

        ISAInputXRay inputxray = ServiceAgent.getInstance().GetObjectByName<ISAInputXRay>(WebConstant.SAInputXRayObject);
        string type = "XRay";
        IList<XRay> list = inputxray.GetMaterialByTypeList(type);
           
        DataRow newRow = null;

        if (list != null && list.Count != 0)
        {
            foreach (XRay temp in list)
            {
                newRow = dt.NewRow();
                newRow["Pdline"] = temp.PdLine;
                newRow["Model"] = temp.Model;
                newRow["PCBNo"] = temp.PCBNo;
                newRow["Location"] = temp.Location;
                newRow["Obligation"] = temp.Obligation;
                newRow["IsPass"] = temp.IsPass;
                newRow["Remark"] = temp.Remark;
                newRow["Editor"] = temp.Editor;
                newRow["Cdt"] = temp.Cdt.ToString("yyyy-MM-dd HH:mm");    
                dt.Rows.Add(newRow);
            }

            for (int i = list.Count; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

        }
        else
        {
            for (int i = 0; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }
        return dt;

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
    private void QueryFinished()
    {
        string InfoQueryFinished = "QueryFinished";
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowSuccessfulInfo(true,\"" + InfoQueryFinished.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "QueryFinished", scriptBuilder.ToString(), false);

    }
    //private void InitPage()
    //{

    //    AlertNoNeedModel = GetLocalResourceObject(Pre + "AlertNoNeedModel").ToString();
    //    AlertLendSuccess = GetLocalResourceObject(Pre + "AlertLendSuccess").ToString();
    //    AlertReturnSuccess = GetLocalResourceObject(Pre + "AlertReturnSuccess").ToString();
    //    AlertSelectBorrowStatus = GetLocalResourceObject(Pre + "AlertSelectBorrowStatus").ToString();
    //    AlertWrongFormat = GetLocalResourceObject(Pre + "AlertWrongFormat").ToString();
    //    AlertModel = GetLocalResourceObject(Pre + "AlertModel").ToString();
    //    AlertInputModel = GetLocalResourceObject(Pre + "AlertInputModel").ToString();
    //    AlertInputMBProductCT = GetLocalResourceObject(Pre + "AlertInputMBProductCT").ToString();
    //    AlertInputBorrower = GetLocalResourceObject(Pre + "AlertInputBorrower").ToString();
    //    AlertInputReturner = GetLocalResourceObject(Pre + "AlertInputReturner").ToString();

    //    this.LabelBorrower.Text = GetLocalResourceObject(Pre + "LabelBorrowerReturner").ToString();
    //    this.LabelDataEntry.Text = GetLocalResourceObject(Pre + "LabelDataEntry").ToString();
    //    this.LabelLender.Text = GetLocalResourceObject(Pre + "LabelLenderAccepter").ToString();
    //    this.LabelMBPodIdCT.Text = GetLocalResourceObject(Pre + "LabelMBPodIdCT").ToString();
    //    this.LabelModel.Text = GetLocalResourceObject(Pre + "LabelModel").ToString();
    //}
    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("Pdline", Type.GetType("System.String"));
        result.Columns.Add("Model", Type.GetType("System.String"));
        result.Columns.Add("PCBNo", Type.GetType("System.String"));
        result.Columns.Add("Location", Type.GetType("System.String"));
        result.Columns.Add("Obligation", Type.GetType("System.String"));
        result.Columns.Add("IsPass", Type.GetType("System.String"));
        result.Columns.Add("Remark", Type.GetType("System.String"));
        result.Columns.Add("Editor", Type.GetType("System.String"));
        result.Columns.Add("Cdt", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();
            for (int j = 0; j < 9; j++)
            { newRow[j] = string.Empty; }
            result.Rows.Add(newRow);
        }
        return result;
    }

    private int DefaultRowsCount = 7;

}
