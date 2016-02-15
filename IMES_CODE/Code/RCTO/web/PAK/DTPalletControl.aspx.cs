using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class PAK_DTPalletControl : System.Web.UI.Page
{

    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertInputFrom = "";
    protected string AlertInputTo = "";
    protected string AlertInputCorrectFrom = "";
    protected string AlertInputCorrectTo = "";
    protected string AlertFromLargeTo = "";
    protected string AlertInputPalletNo = "";
    protected string AlertNoData = "";
    protected string AlertSuccess = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
        }
    }

    public void ToExcel(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dtData = GetDTList();
            System.Web.UI.WebControls.DataGrid dgExport = null;
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;

            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            if (dtData != null && dtData.Rows.Count > 0)
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
                    for (int j = 0; j < 3; j++)
                    {
                        dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    }
                }

                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
                WriteAlertInfo(AlertSuccess);
            }
            else
            {
                WriteAlertInfo(AlertNoData);
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

    private DataTable GetDTList()
    {
        string palletNo = this.HiddenPalletNo.Value;
        string from = this.HiddenFrom.Value;
        string to = this.HiddenTo.Value;

        DataTable dt = InitTable();

        IDTPalletControl currentService = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IDTPalletControl>(com.inventec.iMESWEB.WebConstant.DTPalletControlObject);
        IList<WhPltLogInfo> dtList = currentService.Query(palletNo, from, to);


        DataRow newRow = null;

        if (dtList != null && dtList.Count != 0)
        {
            foreach (WhPltLogInfo temp in dtList)
            {
                newRow = dt.NewRow();
                newRow["PalletNo"] = temp.plt;
                newRow["Editor"] = temp.editor;
                newRow["Cdt"] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");

                dt.Rows.Add(newRow);
            }

            for (int i = dtList.Count; i < DefaultRowsCount; i++)
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

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void WriteAlertInfo(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "WriteAlertInfoAgent", scriptBuilder.ToString(), false);

    }

    private void InitPage()
    {

        AlertInputFrom = GetLocalResourceObject(Pre + "AlertInputFrom").ToString();
        AlertInputTo = GetLocalResourceObject(Pre + "AlertInputTo").ToString();
        AlertInputCorrectFrom = GetLocalResourceObject(Pre + "AlertInputCorrectFrom").ToString();
        AlertInputCorrectTo = GetLocalResourceObject(Pre + "AlertInputCorrectTo").ToString();
        AlertFromLargeTo = GetLocalResourceObject(Pre + "AlertFromLargeTo").ToString();
        AlertInputPalletNo = GetLocalResourceObject(Pre + "AlertInputPalletNo").ToString();
        AlertNoData = GetLocalResourceObject(Pre + "AlertNoData").ToString();
        AlertSuccess = GetLocalResourceObject(Pre + "AlertSuccess").ToString();
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("PalletNo", Type.GetType("System.String"));
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
            for (int j = 0; j < 3; j++)
            { newRow[j] = string.Empty; }
            result.Rows.Add(newRow);
        }
        return result;
    }

    private int DefaultRowsCount = 7;

}
