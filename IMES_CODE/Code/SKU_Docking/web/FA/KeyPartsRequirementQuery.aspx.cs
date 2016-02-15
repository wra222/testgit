using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.IO;


public partial class PAK_KeyPartsRequirementQuery : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string filename = "";
    public int fullRowCount = 10;
    public String UserId;
    public String Customer;
    public String Station;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Station = Request["Station"];
                bindTable(null, null);
                setLeftColumnWidth();
                setRightColumnWidth();
                InitLabel();
            }
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    
    public void uploadOver(object sender, System.EventArgs e)
    {
        try
        {
            string uploadFile = this.hidFileName.Value;
            string fullName = Server.MapPath("../") + uploadFile;
            DataTable dt = ExcelManager.getExcelSheetData(fullName);
            string models = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                string tmp = dr[0].ToString().Trim() + ":" + dr[1].ToString().Trim() + ";";
                if(tmp != ":;")
                    models = models + tmp;
            }
            IKeyPartsRequirementQuery iKeyParts = ServiceAgent.getInstance().GetObjectByName<IKeyPartsRequirementQuery>(WebConstant.KeyPartsRequirementQueryObject);

            string outmodels = string.Empty;
            DataTable retTable = new DataTable();
            retTable = iKeyParts.KeyPartQuery(models, out outmodels);
            bindTable(retTable, outmodels);
            File.Delete(fullName);
            endWaitingCoverDiv();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
    }

    private void bindTable(DataTable dt, string models)
    {
        DataTable leftdt = new DataTable();
        DataRow leftdr = null;
        leftdt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPart").ToString());
        leftdt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSPS").ToString());
        leftdt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCount").ToString());
        if(dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                leftdr = leftdt.NewRow();
                leftdr[0] = dt.Rows[i][0];
                leftdr[1] = dt.Rows[i][1];
                leftdr[2] = dt.Rows[i][2];

                leftdt.Rows.Add(leftdr);
            }
            for (int j = dt.Rows.Count; j < fullRowCount; j++)
            {
                leftdt.Rows.Add(leftdt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < fullRowCount; i++)
            {
                leftdr = leftdt.NewRow();
                leftdt.Rows.Add(leftdr);
            }
        }
        gdleft.DataSource = leftdt;
        gdleft.DataBind();
        setLeftColumnWidth();


        DataTable rightdt = new DataTable();
        DataRow rightdr = null;
        rightdt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        rightdt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        
        if (!String.IsNullOrEmpty(models))
        {
            string[] tmp = models.Split(';');
            int count = 0;
            foreach (string item in tmp)
            {
                if (item.Contains(":"))
                {
                    string[] a = item.Split(':');

                    rightdr = rightdt.NewRow();
                    rightdr[0] = a[0];
                    rightdr[1] = a[1];

                    rightdt.Rows.Add(rightdr);
                    count++;
                }
            }
            for (int i = count; i < fullRowCount; i++)
            {
                rightdt.Rows.Add(rightdt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < fullRowCount; i++)
            {
                rightdr = rightdt.NewRow();
                rightdt.Rows.Add(rightdr);
            }
        }
        gdright.DataSource = rightdt;
        gdright.DataBind();
        setRightColumnWidth();
    }

    private void setLeftColumnWidth()
    {
        gdleft.HeaderRow.Cells[0].Width = Unit.Pixel(50);
        gdleft.HeaderRow.Cells[1].Width = Unit.Pixel(200);
        gdleft.HeaderRow.Cells[2].Width = Unit.Pixel(20);
    }

    private void setRightColumnWidth()
    {
        gdright.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gdright.HeaderRow.Cells[1].Width = Unit.Pixel(30);
    }


    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private DataTable getNullLeftTable(int rowCount)
    {
        DataTable dt = initLeftTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Part"] = String.Empty;
            newRow["SPS"] = String.Empty;
            newRow["Count"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullRightTable(int rowCount)
    {
        DataTable dt = initRightTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Model"] = String.Empty;
            newRow["Qty"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initLeftTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Part", Type.GetType("System.String"));
        retTable.Columns.Add("SPS", Type.GetType("System.String"));
        retTable.Columns.Add("Count", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initRightTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        return retTable;
    }

    protected void gdleft_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    protected void gdright_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }

    private void InitLabel()
    {
        this.lblFile.Text = this.GetLocalResourceObject(Pre + "_lblFile").ToString();
        this.lblKeyPartList.Text = this.GetLocalResourceObject(Pre + "_lblKeyPartList").ToString();
        this.lblModelList.Text = this.GetLocalResourceObject(Pre + "_lblModelList").ToString();
        this.toExcel.Value = this.GetLocalResourceObject(Pre + "_btnToExcel").ToString();
        this.refresh.Value = this.GetLocalResourceObject(Pre + "_btnRefresh").ToString();
    }

    protected void excelClick(object sender, System.EventArgs e)
    {
        DataTable2Excel(GetKeyPartsTable());
    }

    private DataTable GetKeyPartsTable()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPart").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSPS").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCount").ToString());
        for (int i = 0; i < this.gdleft.Rows.Count; i++)
        {
            if (!String.IsNullOrEmpty(this.gdleft.Rows[i].Cells[0].Text.Trim())
                && !(this.gdleft.Rows[i].Cells[0].Text.Trim().ToLower() == "&nbsp;"))
            {
                dr = dt.NewRow();
                dr[0] = this.gdleft.Rows[i].Cells[0].Text.Trim();
                dr[1] = this.gdleft.Rows[i].Cells[1].Text.Trim();
                dr[2] = this.gdleft.Rows[i].Cells[2].Text.Trim();
                dt.Rows.Add(dr);
            }
        }

        return dt;
    }

    public static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            //            <bug>
            //            BUG NO:ITC-1103-0261 
            //            REASON:中文编码
            //            </bug>
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "UTF-8";
            curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312 >");
            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            for (int i = 0; i < dgExport.Items.Count; i++)
            {
                //excel.Columns[3].ItemStyle.CssClass = "xlsText";
                //excel.Items[i].Cells[3].Style.Add("mso-number-format", "\"@\"");
                for (int j = 0; j < 3; j++)
                {
                    //if (j == 7)
                    //{
                    //    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:yyy-mm-dd ##:##:##");
                    //}
                    //else
                    //{
                    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    //}
                }
            }

            // 返回客户端
            dgExport.RenderControl(htmlWriter);

            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            bindTable(null, null);
            setLeftColumnWidth();
            setRightColumnWidth();
            endWaitingCoverDiv();
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
}