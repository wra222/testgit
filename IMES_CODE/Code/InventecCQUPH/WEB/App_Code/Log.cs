using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log
{
    public static void logError(string Error)
    {
        log("Error :" + Error);
        StringBuilder sb = new StringBuilder();
        string path = string.Format("{0}\\Error-{1}.log", HttpContext.Current.Server.MapPath("~\\log"),
                                                  DateTime.Now.ToString("yyyy-MM-dd"));
        sb.AppendLine(string.Format("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "Error :" + Error));
        File.WriteAllText(path, sb.ToString());
    }
    public static void logMessage(string Message)
    {
        log("Message :" + Message);
    }
    private static void log(string text)
    {
        StringBuilder sb = new StringBuilder();
        string path = string.Format("{0}\\{1}.log", HttpContext.Current.Server.MapPath("~\\log"),
                                                  DateTime.Now.ToString("yyyy-MM-dd"));
        CheckForder(path);
        using (StreamWriter w = File.AppendText(path))
        {
            w.WriteLine("{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), text);
            w.Close();
        }
    }
    private static void CheckForder(string file)
    {
        string path = System.IO.Directory.GetParent(file).ToString();
        if (!(System.IO.Directory.Exists(path)))
        {
            System.IO.Directory.CreateDirectory(path);
        };

    }

}
