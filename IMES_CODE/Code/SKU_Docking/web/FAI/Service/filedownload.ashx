<%@ WebHandler Language="C#" Class="filedownload" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using IMES.Station.Interface.StationIntf;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using System.Collections.Specialized;


public class filedownload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string guid = context.Request.QueryString["guid"] as string;
        string department = context.Request.QueryString["Department"] as string;
        string model = context.Request.QueryString["Model"] as string;
        String rootPath = context.Request.QueryString["FileFolder"] as string; ;
        //讀取DB
        
        IFAIFARelease iFAIFARelease = com.inventec.iMESWEB.ServiceAgent.getInstance().GetObjectByName<IFAIFARelease>(com.inventec.iMESWEB.WebConstant.FAIFAReleaseObject);

        string filename = iFAIFARelease.UploadFile(guid);
        context.Response.Buffer = true;
        context.Response.Clear();
        context.Response.ContentType = "application/download";
        context.Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8) + ";");
        context.Response.BinaryWrite(File.ReadAllBytes(string.Format(@"{4}{2}\\{3}\\{0}~{1}", guid, filename, department, model,rootPath)));
        context.Response.Flush();
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}