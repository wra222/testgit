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
using System.Xml.Serialization;

//using NPOI;//add by qy
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;//add by qy
//using NPOI.POIFS;//add by qy
//using NPOI.SS.UserModel;
//using NPOI.Util;//add by qy
//using NPOI.XSSF;
//using NPOI.XSSF.UserModel;//add by qy


public partial class DataMaintain_PAKKittingUploadExport: System.Web.UI.Page
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private IPakKittingUpload iPakKittingUpload = (IPakKittingUpload)ServiceAgent.getInstance().GetMaintainObjectByName<IPakKittingUpload>(WebConstant.MaintainPakKittingUploadObject);
    public int ExcelUploadStationColumnCount = 5;

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
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string line = this.dLine.Value.Trim();
        string dataUpload = this.dDataExport.Value.Trim();

        DataTable result = new DataTable();
        try
        {
            XmlStringToDataTable(dataUpload, ref result);
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
        DataTable dt = new DataTable();
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        try
        {
            int totalRowCount = result.Rows.Count;
            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "Line:" + Null2String(line);
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dt.Rows.Add("");
            dr = dt.NewRow();
            dr[0] = "PartNo";
            dr[1] = "Descr";
            dr[2] = "Qty";
            dr[3] = "LightNo";
            dt.Rows.Add(dr);
            if (totalRowCount > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    dr = dt.NewRow();
                    dr[0] = item[1].ToString();
                    dr[1] = item[2].ToString();
                    dr[2] = item[3].ToString();
                    dr[3] = item[4].ToString();
                    dt.Rows.Add(dr);
                }
            }
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            string fileExport = "PAK_" + line + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            MemoryStream ms = ExcelManager.ExportDataTableToExcel(dt);

            curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileExport + ".xls"));
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
            //show error
            SendComplete(ex.Message);
            return;
        }
        //finally
        //{
        //    if (mApp != null)
        //    {
        //        try
        //        {
        //            if (m_book != null) m_book.Close(false, null, null);
        //            mApp.Quit();
        //            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
        //            if (m_book != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(m_book);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(mApp);
        //        }
        //        catch
        //        {
        //            //ignore
        //        }
        //        worksheet = null;
        //        m_book = null;
        //        mApp = null;
        //    }
        //    if (hProcess != null)
        //    {
        //        try
        //        {
        //            hProcess.Kill();
        //        }
        //        catch
        //        {
        //            // ignore
        //        }
        //    }
        //    // Collect the unreferenced objects
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //}

        //byte[] finalData = GetFileByteData(fullFileName);

        //try
        //{
        //    File.Delete(fullFileName);
        //}
        //catch (Exception ex)
        //{
        //    //ignore
        //}

        //HttpResponse response = System.Web.HttpContext.Current.Response;

        //string fileExport = "PAK_" + line +"_"+ DateTime.Now.ToString("yyyy-MM-dd-HHmmss");

        //response.Clear();
        //response.ClearHeaders();
        //response.ContentType = "application/octet-stream";
        ////Response.AddHeader("content-type", "application/x-msdownload");
        ////Response.ContentType = "application/ms-excel";
        //response.AddHeader("Content-Disposition", "attachment; filename=" + fileExport + ".xls");
        //response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

        //response.OutputStream.Write(finalData, 0, finalData.Length);
        //response.Flush();
        //response.Close();
 
    }

    private bool XmlStringToDataTable(string dataString, ref DataTable dataInfo)
    {
        XmlSerializer ser = new XmlSerializer(typeof(DataTable));
        StringReader sr = new StringReader(dataString);
        dataInfo = (DataTable)ser.Deserialize(sr);
        sr.Close();
        return true;
    }
}
