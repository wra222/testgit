<%@ WebHandler Language="C#" Class="AOI" %>

using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;
using UTL;
using UTL.SQL;
using System.Data;
using System.Data.SqlClient;


public class AOI : IHttpHandler {


    private string logPath;
    private string DBConnect;
    private string QuerySPName;
    private int timeOut = 30;
    private string sn="";
    private string kbPartNo="";
    private string lbPartNo="";
    private string model="";

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    
    public void ProcessRequest (HttpContext context)
    {
        try
        {      
            getConfig();
            writeLog("Begin", "ProcessRequest");
            string responseText = getRequest(context.Request);
            responseText = getRecordSet(responseText,sn);
            writeLog("Response", responseText);
            sendResponse(responseText, context.Response);
            writeLog("Reply", responseText);
            //context.Response.End();
            
        }
        catch (Exception e)
        {
            sendResponse(genReplyXML(sn, "","","", "Fail", e.Message), context.Response);
            writeLog("Error", e.Message);           
        }
        finally
        {
            writeLog("End", "ProcessRequest");
        }
    }
     
    private string getRequest(HttpRequest request)
    {
        writeLog("URL", request.RawUrl);
        NameValueCollection query = request.QueryString;
        if (query.HasKeys())
        {
            sn = query["SN"].Trim();
            if (string.IsNullOrEmpty(sn))
            {
                writeLog("Error", "SN is empty!!");
                return genReplyXML("","","", "", "Fail", "SN is empty!!");
            }
            writeLog("Receive", "SN=" + sn);
            return sn;
        }
        else
        {
            return genReplyXML("","","", "", "Fail", " Missing URL Query Parameter!!");
        }    
    }

    private string getRecordSet(string responseText,string sn)
    {
        if (responseText.Contains("Fail"))
        {
            return responseText;
        }
        
        SqlParameter parakey = new SqlParameter("@Sno", SqlDbType.VarChar);
        parakey.Direction = ParameterDirection.Input;
        parakey.Value = sn;

        DataTable dt = SQLHelper.ExecuteDataFill(DBConnect, CommandType.StoredProcedure, QuerySPName, parakey);
        //DataTable dt = new DataTable();
        //dt.Columns.Add("SN", System.Type.GetType("System.String"));
        //dt.Columns.Add("Model", System.Type.GetType("System.String"));
        //dt.Columns.Add("KBPart", System.Type.GetType("System.String"));
        //dt.Columns.Add("LBPart", System.Type.GetType("System.String"));
        //DataRow dr = dt.NewRow();
        //dr["SN"] = "CUN1234567";
        //dr["Model"] = "PCB1234567";
        //dr["KBPart"] = "KBPartNo12345";
        //dr["LBPart"] = "LabelPart12345";
        //dt.Rows.Add(dr);
        
        if (dt == null || dt.Rows.Count == 0)
        {
            writeLog("Error", "Part Information is empty!!");
            return genReplyXML(sn, "", "", "", "Fail", "Part Information is empty!!");
        }

        model = dt.Rows[0]["Model"].ToString().Trim();
        if (string.IsNullOrEmpty(model))
        {
            writeLog("Error", "model is empty!!");
            return genReplyXML(sn, "", "", "", "Fail", "model is empty!!");
        }
        
        kbPartNo = dt.Rows[0]["KBPartNo"].ToString().Trim();
        if (string.IsNullOrEmpty(kbPartNo))
        {
            writeLog("Error", "KBPartNo is empty!!");
            return genReplyXML(sn, "", "", "", "Fail", "KBPartNo is empty!!");
        }

        lbPartNo = dt.Rows[0]["LabelPartNo"].ToString().Trim();
        if (string.IsNullOrEmpty(lbPartNo))
        {
            writeLog("Error", "LabelPartNo is empty!!");
            return genReplyXML(sn, "", "", "", "Fail", "LabelPartNo is empty!!");
        }
        writeLog("Receive", "SN:"+sn+" Model:" + model + " KBPartNo:"+kbPartNo+" LabelPartNo:"+lbPartNo);
        return genReplyXML(sn, model, kbPartNo, lbPartNo, "Pass", "");
    }

    private void sendResponse(string responseString, HttpResponse response)
    {
        response.Clear();
        response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        response.StatusDescription = "ok";
        response.ContentType = "text/xml";
        response.AddHeader("Access-Control-Allow-Origin", "*");
        response.Write(responseString);
    }

    private void getConfig()
    {
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeOut"]))
        {
            timeOut = int.Parse(ConfigurationManager.AppSettings["TimeOut"]);
        }

        logPath = ConfigurationManager.AppSettings["logPath"];
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }

        DBConnect = ConfigurationManager.ConnectionStrings["DBConnection"].ToString().Trim();
        if (string.IsNullOrEmpty(DBConnect))
        {
            Exception e = new Exception("DBConnection is not exists");
            writeLog("Error", e.Message);
            throw e;
        }

        QuerySPName = ConfigurationManager.AppSettings["QuerySPName"].ToString().Trim();
        if (string.IsNullOrEmpty(QuerySPName))
        {
            Exception e = new Exception("QuerySPName is not exists");
            writeLog("Error", e.Message);
            throw e;
        }
    }

    private string genReplyXML(string sn, string model, string kbPartNo, string labelPartNo, string result, string errorDescr)
    {
        string ret = "<Device><SN>%sn%</SN><Model>%Model%</Model><KBPartNo>%KBPartNo%</KBPartNo><LabelPartNo>%LabelPartNo%</LabelPartNo><Result>%Result%</Result><ErrorDescr>%ErrorDescr%</ErrorDescr></Device>";
        ret = ret.Replace("%sn%", sn)
                 .Replace("%Model%", model)
                 .Replace("%KBPartNo%", kbPartNo)
                 .Replace("%LabelPartNo%", labelPartNo)
                 .Replace("%Result%", result)
                 .Replace("%ErrorDescr%", errorDescr);
        return ret;
    }


    private void writeLog(string logType,string msgText)
    {
        string logFormat = @"{0} [{1}] [{2}]   {3}" + "\r\n";
        string fileName = logPath + "\\KBCCD_" + DateTime.Now.ToString("yyMMddHH") + ".txt";
        FileStream logFile = null;
        try
        {
            if (File.Exists(fileName))
            {
                logFile = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            }
            else
            {
                logFile = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            }
            string logStr = string.Format(logFormat,
                                                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                                 Thread.CurrentThread.ManagedThreadId.ToString(),
                                                                      logType,
                                                                      msgText);
            byte[] info = new System.Text.UTF8Encoding(true).GetBytes(logStr);
            logFile.Write(info, 0, info.Length);
            logFile.Close();
        }
        catch
        {
            
        }      
    }
}
