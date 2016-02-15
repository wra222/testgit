using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
public partial class CommonFunction_ExecSQL : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Customer;
    protected string AlertSelectLine = "";
    protected string AlertNoData = "";
    protected string AlertSuccess = "";
    int[] columnWidth = new int[] { };
    protected string Editor = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Editor = Master.userInfo.UserId;
        this.GridViewExt1.DataSource = GetTable();
        this.GridViewExt1.DataBind();
    }

    public void DisplayResultGV(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dtData = GetDTList();

            if (dtData != null && dtData.Rows.Count > 0)
            {
                this.GridViewExt1.DataSource = dtData;
                this.GridViewExt1.DataBind();
            }
            else
            {
                writeToAlertMessage(AlertNoData);
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
        string DB = this.HiddenDB.Value;
        string SQLText = this.HiddenSQLText.Value;

        DataTable dt = InitTable();

        ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
        System.Data.DataSet SqlResult = CurrentService.GetSQLResult(Editor, DB, SQLText, null, null);
        if (SqlResult != null && SqlResult.Tables.Count > 0 && SqlResult.Tables[0].Rows.Count > 0)
        {
            dt = SqlResult.Tables[0];
        }

        DataRow newRow = null;

        if (dt != null && dt.Rows.Count != 0)
        {
            int DTRowsCount = dt.Rows.Count;
            this.LabelResultCount.Text = DTRowsCount.ToString();
            LabelDisplayCount.Text = DTRowsCount.ToString();
            if (DTRowsCount > 500)
            {
                LabelDisplayCount.Text = "500";
                for (int i = DTRowsCount-1; i > 499; i--)
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            for (int i = dt.Rows.Count; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }


            int gridviewWidth = 0;
            columnWidth = new int[] { };
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[0].ColumnName == null)
                {
                    gridviewWidth += 20;
                }
                else
                {
                    gridviewWidth += dt.Columns[i].ColumnName.Length * 3;
                }
            }
            if (gridviewWidth < 98)
            {
                gridviewWidth = 98;
            }
            this.GridViewExt1.Width = System.Web.UI.WebControls.Unit.Percentage(gridviewWidth * 1.0);


        }
        else
        {
            this.GridViewExt1.Width = System.Web.UI.WebControls.Unit.Percentage(98);
            for (int i = 0; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
        }



        return dt;

    }

    //protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    for (int i = 0; i < e.Row.Cells.Count; i++)
    //    {
    //        e.Row.Cells[i].Width = System.Web.UI.WebControls.Unit.Pixel(columnWidth[i]);
    //    }
    //}

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "document.getElementById('BtnQuery').disabled = false;endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
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
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("No Data", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();
            result.Rows.Add(newRow);
        }
        return result;
    }

    private int DefaultRowsCount = 7;
    private int MaxRowsCount = 500;
}
