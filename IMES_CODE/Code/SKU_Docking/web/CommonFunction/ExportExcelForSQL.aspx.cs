using System;
using System.Web;
using IMES.Infrastructure;
using IMES.DataModel;
using System.IO;
using com.inventec.system.util;
using IMES.Maintain.Interface.MaintainIntf;
using System.Runtime.InteropServices;
using System.Text;
using com.inventec.iMESWEB;
using System.Web.UI;
using System.Data;

//using NPOI;//add by qy
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;//add by qy
//using NPOI.POIFS;//add by qy
//using NPOI.SS.UserModel;
//using NPOI.Util;//add by qy
//using NPOI.XSSF;
//using NPOI.XSSF.UserModel;//add by qy


public partial class CommonFunction_ExportExcelForSQL : System.Web.UI.Page
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
    public int ExcelUploadStationColumnCount = 4;

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    private void SendComplete(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);

        scriptBuilder.AppendLine("<script language='javascript'>");
        if (errorMsg != "")
        {
            scriptBuilder.AppendLine("onComplete('" + errorMsg+ "');");
        }
        else
        {
            scriptBuilder.AppendLine("onComplete();");
        }
        scriptBuilder.AppendLine("</script>");

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onComplete", scriptBuilder.ToString(), false);

   }

    public static byte[] GetFileByteData(String fileName)
    {

        FileStream fs = new FileStream(fileName, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        byte[] testArray = new byte[fs.Length];
        int count = br.Read(testArray, 0, testArray.Length);

        br.Close();

        return testArray;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    private void Excel(DataTable result, string SQLText)
    {
        try
        {
            DataTable dt = new DataTable();
            foreach (DataColumn dc in result.Columns)
            {
                dt.Columns.Add(dc.ToString());
            }
            DataRow dr;
            dr = dt.NewRow();
            foreach (DataColumn dc in result.Columns)
            {
                dr[dc.Ordinal] = dc.ToString();
            }
            dt.Rows.Add(dr);

            foreach (DataRow item in result.Rows)
            {
                dr = dt.NewRow();

                foreach (DataColumn column in result.Columns)
                {
                    dr[column.Ordinal] = item[column].ToString();
                }
                dt.Rows.Add(dr);
            }

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            string fileExport = SQLText + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            MemoryStream ms = ExcelManager.ExportDataTableToExcel(dt);
            if (curContext.Request.Browser.Browser.ToUpper() == "IE")
            {
                curContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileExport, System.Text.Encoding.UTF8) + ".xls");
            }
            else
            {
                curContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileExport + ".xls");
            } 
            curContext.Response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
        }
        catch (FisException ex)
        {
            SendComplete(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            SendComplete(ex.Message);
            return;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string DB = this.dDB.Value.Trim();
        string SQLText = this.dSQLText.Value.Trim();
        string Editor = this.dEditor.Value.Trim();
        DataTable result = new DataTable();
        try
        {
            DataTable dtData = InitTable();
            if (SQLText != "")
            {
                ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
                System.Data.DataSet SqlResult = CurrentService.GetSQLResult(Editor, DB, SQLText, null, null);
                if (SqlResult != null && SqlResult.Tables.Count > 0 && SqlResult.Tables[0].Rows.Count > 0)
                {
                    dtData = SqlResult.Tables[0];
                }
            }
            if (dtData != null && dtData.Rows.Count > 0)
            {
                Excel(dtData, SQLText);
            }
        }
        catch (FisException ex)
        {
            SendComplete(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            SendComplete(ex.Message);
            return;
        }
        
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("No Data", Type.GetType("System.String"));
        return result;
    }
}
