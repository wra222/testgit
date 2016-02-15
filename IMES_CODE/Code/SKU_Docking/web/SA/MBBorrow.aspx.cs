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

public partial class SA_MBBorrow : System.Web.UI.Page
{

    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertModel = "Product and Model does not match!";
    protected string AlertInputModel = "please input Model!";
    protected string AlertInputBorrower = "please input Borrower!";
    protected string AlertInputReturner = "please input Returner!";
    protected string AlertInputMBProductCT = "please input MB Sno/Product ID/CT No!";
    protected string AlertWrongFormat = "Input data error!";
    protected string AlertSelectBorrowStatus = "please select query type";
    protected string AlertLendSuccess = "Lend successfully!";
    protected string AlertReturnSuccess = "Return successfully!";
    protected string AlertNoNeedModel = "Do not need input Model!";

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

    /// <summary>
    /// 查询数据，刷新gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void QueryBorrowList(object sender, System.EventArgs e)
    {
        try
        {
            beginWaitingCoverDiv();
            this.GridViewExt1.DataSource = GetBorrowList();
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
            DataTable dtData = GetBorrowList();
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

    private DataTable GetBorrowList()
    {
        DataTable dt = InitTable();

        IQueryBorrow QueryService = ServiceAgent.getInstance().GetObjectByName<IQueryBorrow>(WebConstant.QueryBorrowObject);
        string status = this.HiddenSelectType.Value;
        IList<BorrowLog> list = QueryService.GetBorrowList(status);
        DataRow newRow = null;

        if (list != null && list.Count != 0)
        {
            foreach (BorrowLog temp in list)
            {
                newRow = dt.NewRow();
                newRow["Sno"] = temp.Sn;
                newRow["Model"] = temp.Model;
                newRow["Borrower"] = temp.Borrower;
                newRow["Lender"] = temp.Lender;
                newRow["Returner"] = temp.Returner;
                newRow["Accepter"] = temp.Acceptor;
                newRow["Status"] = temp.Status;
                newRow["BorrowDate"] = temp.Bdate.ToString("yyyy-MM-dd HH:mm");
                if (temp.Status == "R")
                {
                    newRow["ReturnDate"] = temp.Rdate.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    newRow["ReturnDate"] = "";
                }

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

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void QueryFinished()
    {
        string InfoQueryFinished = GetLocalResourceObject(Pre + "InfoQueryFinished").ToString();
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowSuccessfulInfo(true,\"" + InfoQueryFinished.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "QueryFinished", scriptBuilder.ToString(), false);

    }

    private void InitPage()
    {

        AlertNoNeedModel = GetLocalResourceObject(Pre + "AlertNoNeedModel").ToString();
        AlertLendSuccess = GetLocalResourceObject(Pre + "AlertLendSuccess").ToString();
        AlertReturnSuccess = GetLocalResourceObject(Pre + "AlertReturnSuccess").ToString();
        AlertSelectBorrowStatus = GetLocalResourceObject(Pre + "AlertSelectBorrowStatus").ToString();
        AlertWrongFormat = GetLocalResourceObject(Pre + "AlertWrongFormat").ToString();
        AlertModel = GetLocalResourceObject(Pre + "AlertModel").ToString();
        AlertInputModel = GetLocalResourceObject(Pre + "AlertInputModel").ToString();
        AlertInputMBProductCT = GetLocalResourceObject(Pre + "AlertInputMBProductCT").ToString();
        AlertInputBorrower = GetLocalResourceObject(Pre + "AlertInputBorrower").ToString();
        AlertInputReturner = GetLocalResourceObject(Pre + "AlertInputReturner").ToString();

        this.LabelBorrower.Text = GetLocalResourceObject(Pre + "LabelBorrowerReturner").ToString();
        this.LabelDataEntry.Text = GetLocalResourceObject(Pre + "LabelDataEntry").ToString();
        this.LabelLender.Text = GetLocalResourceObject(Pre + "LabelLenderAccepter").ToString();
        this.LabelMBPodIdCT.Text = GetLocalResourceObject(Pre + "LabelMBPodIdCT").ToString();
        this.LabelModel.Text = GetLocalResourceObject(Pre + "LabelModel").ToString();
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("Sno", Type.GetType("System.String"));
        result.Columns.Add("Model", Type.GetType("System.String"));
        result.Columns.Add("Borrower", Type.GetType("System.String"));
        result.Columns.Add("Lender", Type.GetType("System.String"));
        result.Columns.Add("Returner", Type.GetType("System.String"));
        result.Columns.Add("Accepter", Type.GetType("System.String"));
        result.Columns.Add("Status", Type.GetType("System.String"));
        result.Columns.Add("BorrowDate", Type.GetType("System.String"));
        result.Columns.Add("ReturnDate", Type.GetType("System.String"));
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
