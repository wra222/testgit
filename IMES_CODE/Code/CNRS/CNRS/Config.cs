using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace UTL
{
    class AppConfig
    {

       
        public readonly int OffsetDay;

        public readonly int AssignedPOCmdInterval;

        public readonly int MOStartDateOffSetDay=0;
        public readonly int   MOUdtOffSetDay=0;

        public readonly int AutoAssignPO = 0;
        public readonly int ShipDateOffSetDay;

        public readonly string logPath;
        public readonly string logPrefixName;

        public readonly bool bCheckQty;
        public readonly string DBConnectStr;
        public readonly string CDSIDBConnectStr;
     
        public readonly string EmailServer;
        public readonly string FromAddress;
        public readonly string[] ToAddress;
        public readonly string[] CcAddress;
        public readonly string MailSubject;
        public readonly string ASTStation;

        public  AppConfig()
        {

            
            OffsetDay = int.Parse(get_config_value("OffSetDay") == null ? "-3" : get_config_value("OffSetDay"));

            AssignedPOCmdInterval = (int.Parse(get_config_value("AssignedPOCmdInterval") == null ? "10" : get_config_value("AssignedPOCmdInterval"))) * 1000;

            //MOStartDateOffSetDay = int.Parse(get_config_value("MOStartDateOffSetDay") == null ? "-3" : get_config_value("MOStartDateOffSetDay"));
            //MOUdtOffSetDay = int.Parse(get_config_value("MOUdtOffSetDay") == null ? "-10" : get_config_value("MOUdtOffSetDay"));
            ShipDateOffSetDay= int.Parse(get_config_value("ShipDateOffSetDay") == null ? "-1" : get_config_value("ShipDateOffSetDay"));
            AutoAssignPO = int.Parse(get_config_value("AutoAssignPO") == null ? "0" : get_config_value("AutoAssignPO"));
            bCheckQty = get_config_value("CheckQty") == null ? false : (get_config_value("CheckQty").ToUpper() == "Y" ? true : false);

            logPath = get_config_value("LogPath");
            logPrefixName = get_config_value("LogPreFixName");
          
            
            
            EmailServer = get_config_value("MailServer");
            FromAddress = get_config_value("FromAddress");
            ToAddress = get_config_value("ToAddress").Split(new char[]{';',','});
            CcAddress = get_config_value("CcAddress").Split(new char[] { ';', ',' });
            MailSubject = get_config_value("MailSubject");
            ASTStation = get_config_value("ASTStation");

            
            DBConnectStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            //CDSIDBConnectStr = ConfigurationManager.ConnectionStrings["CDSIDBConnection"].ConnectionString;
           
        }

        private string get_config_value(string _name)
        {
                 
            return ConfigurationManager.AppSettings[_name];
        }

        
    }
}
