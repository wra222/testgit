using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
namespace ArchiveDB
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
        //{  string hh="";
        //    XmlNode nodeSection = (XmlNode)ConfigurationSettings.GetConfig("Archive");
        //    foreach (XmlNode node in nodeSection.ChildNodes)
        //    {
        //        hh = node.Attributes["ArchiveName"].Value;
        //        foreach (XmlNode node2 in node.ChildNodes)
        //        {
        //            hh = node2.Attributes["FKTableList"].Value;
        //            hh = node2.ChildNodes[0].Attributes["ID"].Value;
        //            hh = node2.ChildNodes[0].Attributes["Conditon"].Value;
        //        }
                
        //        //hh = node.Attributes["ArchiveName"].Value;
        //        // foreach(XmlAttribute x in node.ChildNodes[0].Attributes["FKTableList"])
        //        // {
        //        //     hh = x.Value;
        //        // }
        //     //  hh=     node.ChildNodes[0].Attributes["FKTableList"][Value;
        //    }
        ////    string h = node.Attributes["PKTable"].Value;
        //    return ConfigurationManager.AppSettings[_name];
        //}
    }
}
