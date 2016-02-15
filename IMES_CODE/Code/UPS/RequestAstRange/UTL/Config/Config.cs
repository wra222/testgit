using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Globalization;

namespace UTL.Config
{
    public class AppConfig
    {

        public readonly string SAPServerDomain;
        public readonly string SAPServerUser;
        public readonly string SAPServerPassword;

        public readonly string SAPServerFolder;
        public readonly string SAPServerBckupFolder;
        public readonly string SAPPOFile;

        public readonly string AVPartNo;

        public readonly string PONo;

        public readonly int DelayVerifyPo;
        public readonly int WaitingUPSVerifyPOExpiredTime;

        public readonly string EmailServer;
        public readonly string FromAddress;
        public readonly string[] ToAddress;
        public readonly string[] CcAddress;
        public readonly string MailSubject;

        public readonly string DBConnectStr;

        public readonly string[] OSITagName;
        public readonly string[] OSITagNameMapValue;

        public readonly string TimeFormat = "yyyyMMdd-HHmmss";
        public readonly string DateFormat = "yyyyMMdd";

        public AppConfig()
        {
            SAPServerDomain = get_config_value("SAPServerDomain");
            SAPServerUser = get_config_value("SAPServerUser");
            SAPServerPassword = get_config_value("SAPServerPassword");

            SAPServerFolder = get_config_value("SAPServerFolder");
            SAPServerBckupFolder = get_config_value("SAPServerBckupFolder");
            SAPPOFile = get_config_value("SAPPOFile");

            AVPartNo = get_config_value("AVPartNo");
            PONo = get_config_value("PONo");
            
            DelayVerifyPo = int.Parse(get_config_value("DelayVerifyPo") ?? "5");
            WaitingUPSVerifyPOExpiredTime = int.Parse(get_config_value("WaitingUPSVerifyPOExpiredTime") ?? "120");

            OSITagName = get_config_value("OSITagName").Split(new char[] { ',', '~' });
            OSITagNameMapValue = get_config_value("OSITagNameMapValue").Split(new char[] { ',', '~' });


            DBConnectStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; 
         

            EmailServer = get_config_value("MailServer");
            FromAddress = get_config_value("FromAddress");
            ToAddress = get_config_value("ToAddress").Split(new char[] { ';', ',' });
            CcAddress = get_config_value("CcAddress").Split(new char[] { ';', ',' });
            MailSubject = get_config_value("MailSubject");


        }

        private string get_config_value(string _name)
        {

            return ConfigurationManager.AppSettings[_name];
        }

        public static Dictionary<string, BUItem> readSection(string sectionName)
        {
            BUSection section = new BUSection();
            return section.get_section(sectionName);
        }

    }

    public class BUSection : IConfigurationSectionHandler
    {
        public BUSection() { }

        public object Create(object parent,
                                         object configContext,
                                         XmlNode section)
        {
            Dictionary<string, BUItem> items = new Dictionary<string, BUItem>();
            System.Xml.XmlNodeList Nodes = section.ChildNodes;
            foreach (XmlElement node in Nodes)
            {
                BUItem item = new BUItem();
                item.Name = getXmAttributeValue(node, "Name");
                item.SPName = getXmAttributeValue(node, "SPName");
                item.ErrorEmail = getXmAttributeValue(node, "ErrorEmail").Split(new char[] { ';', ',' }); ;
                item.ConnectionString = getXmAttributeValue(node, "ConnectionString");
                items.Add(item.Name, item);
            }
            return items;
        }


        public Dictionary<string, BUItem> get_section(string _sectionName)
        {
            return (Dictionary<string, BUItem>)ConfigurationManager.GetSection(_sectionName);
        }

        private string getXmAttributeValue(XmlElement element_, string name_)
        {
            try
            {
                if (element_ == null) { return ""; }
                if (element_.HasAttribute(name_))
                { return element_.Attributes[name_].Value; }
                else
                { return ""; }
            }
            catch
            {
                return "";
            }


        }
        private string getXmAttributeValue(XmlNode node_, string name_)
        {
            return getXmAttributeValue((XmlElement)node_, name_);
        }



    }




    public struct BUItem
    {
        public string Name;
        public string SPName;
        public string[] ErrorEmail;
        public string ConnectionString;
    }
}
