using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace UTL
{
    public class AppConfig
    {

        public readonly string LocalSN2POFolder;
        public readonly string LocalMAILBOXFolder;
        public readonly string LocalResultFolder;

        public readonly string CDSISN2POFolder;
        public readonly string CDSIMAILBOXFolder;
        public readonly string CDSIResultFolder;

        public readonly string CDSIDomain;
        public readonly string CDSIServerUser;
        public readonly string CDSIServerPassword;


        public readonly string IMGFolder;
        public readonly string IMGDomain;
       public readonly string IMGServerUser;
       public readonly string IMGServerPassword;
        public readonly int OffsetDay;

        public readonly int CDSICmdInterval;

        public readonly int MOStartDateOffSetDay=0;
        public readonly int   MOUdtOffSetDay=0;

        public readonly int AutoAssignPO = 0;
        public readonly int ShipDateOffSetDay;

        public readonly string logPath;
        public readonly string logPrefixName;
        
        public readonly string DBConnectStr;
        public readonly string CDSIDBConnectStr;
     
        public readonly string EmailServer;
        public readonly string FromAddress;
        public readonly string[] ToAddress;
        public readonly string[] CcAddress;
        public readonly string MailSubject;

        public  AppConfig()
        {

            LocalSN2POFolder = get_config_value("LocalSN2POFolder");
            LocalMAILBOXFolder = get_config_value("LocalMAILBOXFolder");
            LocalResultFolder = get_config_value("LocalResultFolder");

            CDSISN2POFolder = get_config_value("CDSISN2POFolder");
            CDSIMAILBOXFolder = get_config_value("CDSIMAILBOXFolder");
            CDSIResultFolder = get_config_value("CDSIResultFolder");

            CDSIDomain = get_config_value("CDSIDomain");
            CDSIServerUser = get_config_value("CDSIServerUser");
            CDSIServerPassword = get_config_value("CDSIServerPassword");


            IMGFolder = get_config_value("IMGServerFolder");
            IMGDomain = get_config_value("IMGDomain");
            IMGServerUser = get_config_value("IMGServerUser");
            IMGServerPassword = get_config_value("IMGServerPassword");

            OffsetDay = int.Parse(get_config_value("OffSetDay") == null ? "-3" : get_config_value("OffSetDay"));

            CDSICmdInterval = (int.Parse(get_config_value("CDSICmdInterval") == null ? "10" : get_config_value("CDSICmdInterval")))*1000;

            //MOStartDateOffSetDay = int.Parse(get_config_value("MOStartDateOffSetDay") == null ? "-3" : get_config_value("MOStartDateOffSetDay"));
            //MOUdtOffSetDay = int.Parse(get_config_value("MOUdtOffSetDay") == null ? "-10" : get_config_value("MOUdtOffSetDay"));
            ShipDateOffSetDay= int.Parse(get_config_value("ShipDateOffSetDay") == null ? "-1" : get_config_value("ShipDateOffSetDay"));
            AutoAssignPO = int.Parse(get_config_value("AutoAssignPO") == null ? "0" : get_config_value("AutoAssignPO"));

            logPath = get_config_value("LogPath");
            logPrefixName = get_config_value("LogPreFixName");
          
            
            
            EmailServer = get_config_value("MailServer");
            FromAddress = get_config_value("FromAddress");
            ToAddress = get_config_value("ToAddress").Split(new char[]{';',','});
            CcAddress = get_config_value("CcAddress").Split(new char[] { ';', ',' });
            MailSubject = get_config_value("MailSubject");

            
            DBConnectStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            CDSIDBConnectStr = ConfigurationManager.ConnectionStrings["CDSIDBConnection"].ConnectionString;
           
        }

        private string get_config_value(string _name)
        {
                 
            return ConfigurationManager.AppSettings[_name];
        }

        
    }
}
