using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using IMES.LD;
using log4net;
using System.Text;
using System.Xml;

namespace IMESCommonWS.IMES.CheckLogin
{

    /// <summary>
    /// Summary description for CheckLogin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CheckLogin : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod]
        public XmlDataDocument CheckClient(string mac, string ip, string hostname,string UserName,string Domain)
        {
            string result = "";
            if (string.IsNullOrEmpty(mac) && string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(hostname) && string.IsNullOrEmpty(Domain))
            {
                result = "-2";
            }
            else
            {
                DataTable dt = SqlStatment.CheckLogin(hostname, mac, Domain);
                result = dt.Rows[0][0].ToString();
            }
            XmlDataDocument xd = new XmlDataDocument();
            //XmlStr.Append("<?xml version=\"1.0\" encoding=\"gb2312\"?>");
            XmlDeclaration newDec = xd.CreateXmlDeclaration("1.0", "gb2312", null);
            xd.AppendChild(newDec);
            XmlElement xmlElemFileName = xd.CreateElement("result");

            XmlText xmlTextFileName = xd.CreateTextNode(result);
                    xmlElemFileName.AppendChild(xmlTextFileName);
                    xd.AppendChild(xmlElemFileName);
           
            return xd;
        }
        [WebMethod]
        public XmlDataDocument DataCollection(string mac, string ip, string hostname, string UserName,string Domain)
        {
            string result = "";
            if (string.IsNullOrEmpty(mac) && string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(hostname) && string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Domain))
            {
                result = "-2";
            }
            else
            {
                SqlStatment.InsertData(mac, ip, hostname, UserName,Domain);
                result = "0";
            }
            XmlDataDocument xd = new XmlDataDocument();
            //XmlStr.Append("<?xml version=\"1.0\" encoding=\"gb2312\"?>");
            XmlDeclaration newDec = xd.CreateXmlDeclaration("1.0", "gb2312", null);
            xd.AppendChild(newDec);
            XmlElement xmlElemFileName = xd.CreateElement("result");

            XmlText xmlTextFileName = xd.CreateTextNode(result);
            xmlElemFileName.AppendChild(xmlTextFileName);
            xd.AppendChild(xmlElemFileName);

            return xd;
        }
        [WebMethod]
        public string GetFisSupport()
        {

            string result = "";
            DataTable dt = SqlStatment.GetFisSupport();
            result = dt.Rows[0][0].ToString();
            return result;
        }

        [WebMethod]
        public string UPSCheckUsername(string user,string password)
        {

            string result = "";
            DataTable dt = SqlStatment.UPSCheckUsername(user, password);
            result = dt.Rows[0][0].ToString();
            return result;
        }

    }
}
