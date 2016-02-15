using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
namespace AlarmMail
{
    class Config : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }

  
        public string GetConnectionString(string connName)
        {
          return  ConfigurationManager.ConnectionStrings[connName].ToString();
        }
        public string GetValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
   
    }
}
