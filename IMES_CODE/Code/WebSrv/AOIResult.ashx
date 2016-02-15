<%@ WebHandler Language="C#" Class="AOIResult" %>

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

public class AOIResult : IHttpHandler
{
    private string DBConnect;
    private string InsertSPName;
    private string logPath;
    private int timeOut = 30;
    private string sn="";
    private string result = "";
    private string errorCode = "";
    private string errorDescr = "";
    private string pdLine = "";
    private string oper = "";
    private string[] inputParameter;

    
    public void ProcessRequest (HttpContext context)
    {
        try
        {      
            getConfig();
            writeLog("Begin", "ProcessRequest");
            string responseText = null;
            bool isPass = getRequest(context.Request, out responseText);
            if (isPass)
            {
                responseText = getRecordSet(responseText, sn);
                writeLog("Response", responseText);
            }
            sendResponse(responseText, context.Response);
            writeLog("Reply", responseText);
            //context.Response.End();
        }
        catch (Exception e)
        {
            sendResponse(genReplyXML(sn, "Fail", "0005", e.Message,"",""), context.Response);
            writeLog("Error", e.Message);           
        }
        finally
        {
            writeLog("End", "ProcessRequest");
        }
      
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


    private bool getRequest(HttpRequest request, out string replyMsg)
    {
        writeLog("URL", request.RawUrl);
        NameValueCollection query = request.Form;
        replyMsg = "";
        if (query.HasKeys())
        {
            sn = query["SN"].Trim();
            if (string.IsNullOrEmpty(sn))
            {
                writeLog("Error", "SN is empty!!");
                replyMsg = genReplyXML("", "Fail", "0001", "SN is empty!!", "", "");
                return false;
            }
            result = query["Result"].Trim();
            if (string.IsNullOrEmpty(result))
            {
                writeLog("Error", "Result is empty!!");
                replyMsg = genReplyXML(sn, "Fail", "0002", "Result is empty!!", "", "");
                return false;
            }
            errorDescr = query["ErrorDescr"].Trim();
            if (result == "Fail" && string.IsNullOrEmpty(errorDescr))
            {
                writeLog("Error", "Parameter Result is Fail!!");
                replyMsg = genReplyXML(sn, "Fail", "0003", "Parameter Result is Fail!!", "", "");
                return false;
            }
            errorCode = query["ErrorCode"].Trim();
            
            pdLine = query["PdLine"].Trim();
            oper = query["Operator"].Trim();
            writeLog("Receive", "SN=" + sn + " Result=" + result + " ErrorCode=" + errorCode + " ErrorDescr=" + errorDescr + " PdLine=" + pdLine + " Operator=" + oper);
            inputParameter = new string[] { sn,result,errorCode,errorDescr,pdLine,oper};
            foreach (string item in inputParameter)
            {
                replyMsg += item + ",";
            }
            replyMsg = replyMsg.Substring(0, replyMsg.Length - 1);
            return true;
        }
        else
        {
            replyMsg = genReplyXML("", "Fail", "0003", " Missing URL Query Parameter!!","","");
            return false;
        }    

    }

    private string getRecordSet(string responseText, string sn)
    {
        writeLog("start getRecordSet", responseText);
        try
        {
            //if (responseText.Contains("Fail"))
            //{
            //    writeLog("getRecordSet", "ResponseText:" + responseText);
            //    return responseText;
            //}
            string[] values = responseText.Split(',');


            SqlParameter parasSN = new SqlParameter("@SN", SqlDbType.VarChar);
            parasSN.Direction = ParameterDirection.Input;
            parasSN.Value = values[0];
            SqlParameter paraResult = new SqlParameter("@Result", SqlDbType.VarChar);
            paraResult.Direction = ParameterDirection.Input;
            paraResult.Value = values[1];
            SqlParameter paraErrorCode = new SqlParameter("@ErrorCode", SqlDbType.VarChar);
            paraErrorCode.Direction = ParameterDirection.Input;
            paraErrorCode.Value = values[2];
            SqlParameter paraErrorDescr = new SqlParameter("@ErrorDescr", SqlDbType.VarChar);
            paraErrorDescr.Direction = ParameterDirection.Input;
            paraErrorDescr.Value = values[3];
            SqlParameter paraPdLine = new SqlParameter("@PdLine", SqlDbType.VarChar);
            paraPdLine.Direction = ParameterDirection.Input;
            paraPdLine.Value = values[4];
            SqlParameter paraOperator = new SqlParameter("@Operator", SqlDbType.VarChar);
            paraOperator.Direction = ParameterDirection.Input;
            paraOperator.Value = values[5];

            //SqlDataReader obj = SQLHelper.ExecuteReader(DBConnect, CommandType.StoredProcedure, InsertSPName, parasSN,
            //                                                                                                paraResult,
            //                                                                                                paraErrorCode,
            //                                                                                                paraErrorDescr,
            //                                                                                                paraPdLine,
            //                                                                                                paraOperator);
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnect, CommandType.StoredProcedure, InsertSPName, parasSN,
                                                                                                            paraResult,
                                                                                                            paraErrorCode,
                                                                                                            paraErrorDescr,
                                                                                                            paraPdLine,
                                                                                                            paraOperator);


            int ret = int.Parse(dt.Rows[0][0].ToString());
            //int ret = 0;
            if (ret != 0)
            {
                string errorMsg = "";
                if (ret == 1)
                {
                    errorMsg = "SN is empty!!";
                }
                else if (ret == 2)
                {
                    errorMsg = "Result is empty!!";
                }
                else if (ret == 3)
                {
                    errorMsg = "Fail but ErrorDescr is empty!!";
                }
                else if (ret == 4)
                {
                    errorMsg = "SN is not exist!!";
                }
                else if (ret == 5)
                {
                    errorMsg = "Result is Fail!!";
                    return genReplyXML(sn, "Fail", "0008", errorMsg, "", "");
                }
                else
                {
                    errorMsg = "SQL error!!!";
                }
                writeLog("Error", errorMsg);
                return genReplyXML(sn, "Fail", "0007", errorMsg, "", "");
            }
            
            writeLog("Receive", "SN:" + sn +" Pass!!");
            return genReplyXML(sn, "Pass", "", "","","");
        }
        catch (Exception e)
        {
            writeLog("Error", e.Message);
            return genReplyXML(sn, "Fail", "0006", e.Message,"","");
        }
        finally
        {
            writeLog("End getRecordSet", responseText);
        }   
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

        InsertSPName = ConfigurationManager.AppSettings["InsertSPName"].ToString().Trim();
        if (string.IsNullOrEmpty(InsertSPName))
        {
            Exception e = new Exception("InsertSPName is not exists");
            writeLog("Error", e.Message);
            throw e;
        }
    }
    
    private string genReplyXML(string sn, string result, string errorCode, string errorDescr,string pdLine,string oper)
    {
        string ret = "<Device><SN>%sn%</SN><Result>%Result%</Result><ErrorCode>%ErrorCode%</ErrorCode><ErrorDescr>%ErrorDescr%</ErrorDescr><PdLine>%PdLine%</PdLine><Operator>%Operator%</Operator></Device>";
        ret = ret.Replace("%sn%", sn)
                  .Replace("%Result%", result)
                  .Replace("%ErrorCode%", errorCode)
                  .Replace("%ErrorDescr%", errorDescr)
                  .Replace("%PdLine%", pdLine)
                  .Replace("%Operator%", oper);
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