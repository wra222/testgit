using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using IMES.DataModel;
using System.Text;

namespace com.inventec.iMESWEB
{

    [Serializable]
    [XmlRootAttribute("XMLScript", IsNullable = false)]
    public class BtwXMLScript
    {
        [XmlAttribute]
        public string Version = "2.0";

        [XmlElement("Command")]
        public Command commad = new Command();
    }


    public class Command
    {
        [XmlAttribute]
        public string Name = new Guid().ToString();
        [XmlElement]
        public Print print = new Print();
    }

    public class Print
    {
        [XmlElement]
        public Format Format = new Format();
        [XmlElement("PrintSetup")]
        public PrintSetup PrintSetup = new PrintSetup();
        [XmlElement("NamedSubString")]
        public List<NamedSubString> NameValueList = new List<NamedSubString>();

    }

    public class Format
    {
        [XmlAttribute]
        public string SaveAtEndOfJob = "false";
        [XmlText]
        public string name;
    }
    public class NamedSubString
    {
        [XmlAttribute]
        public string Name;
        [XmlElement]
        public string Value;
    }

    public class PrintSetup
    {
        [XmlElement]
        public string IdenticalCopiesOfLabel;
        [XmlElement]
        public string Printer;
    }
    /// <summary>
    /// Summary description for BtwXMLScript
    /// </summary>
    public class SerializeXMLUtility
    {
        public static string SerializeXML<T>(Encoding encoding,
                                                      string nameSpace,
                                                      T objectToSerialize) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(ms, encoding);
            xmlTextWriter.Formatting = Formatting.Indented;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (string.IsNullOrEmpty(nameSpace))
            {
                ns.Add(string.Empty, string.Empty);
            }
            else
            {
                ns.Add("", nameSpace);
            }
            serializer.Serialize(xmlTextWriter, objectToSerialize, ns);
            ms = (MemoryStream)xmlTextWriter.BaseStream;
            return encoding.GetString(ms.ToArray());
        }

        public static string SerializeXMLOmitDeclaration<T>(Encoding encoding,
                                                                                string nameSpace,
                                                                                T objectToSerialize) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.Encoding = encoding;
            XmlWriter writer = XmlWriter.Create(ms, settings);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (string.IsNullOrEmpty(nameSpace))
            {
                ns.Add(string.Empty, string.Empty);
            }
            else
            {
                ns.Add("", nameSpace);
            }
            serializer.Serialize(writer, objectToSerialize, ns);
            return encoding.GetString(ms.ToArray());
        }


        public static string GenBtwXMLScript(string btwPath, PrintItem printItem, IList<NameValueDataTypeInfo> nameValueList)
        {
          
            BtwXMLScript btwXML = new BtwXMLScript();
            btwXML.Version = "2.0";
            btwXML.commad.Name = printItem.TemplateName + DateTime.Now.ToString("yyMMddHHmmss");
            btwXML.commad.print.Format.name = btwPath +"\\"+ printItem.TemplateName;
            foreach (NameValueDataTypeInfo item in nameValueList)
            {
                if (item.DataType == "1")
                {
                    btwXML.commad.print.NameValueList.Add(new NamedSubString { Name = item.Name, Value = item.Value });
                }
            }

            btwXML.commad.print.PrintSetup.IdenticalCopiesOfLabel = printItem.Piece.ToString();
            btwXML.commad.print.PrintSetup.Printer = printItem.PrinterPort;
            return SerializeXML<BtwXMLScript>(Encoding.UTF8, "", btwXML);
        }
    }
}
