using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using log4net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using UTL.Reflection;

namespace UTL.Serialize
{
    public class XMLSerialize
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);      


        public static string SerializeXML<T>(Encoding encoding,
                                                        string nameSpace,
                                                        T objectToSerialize) where T:class
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
                ns.Add( "",nameSpace);                              
            }
            serializer.Serialize(xmlTextWriter, objectToSerialize, ns);
            ms = (MemoryStream)xmlTextWriter.BaseStream;
            return encoding.GetString(ms.ToArray());
        }

        public static string SerializeXMLOmitDeclaration<T>(Encoding encoding,
                                                                                string nameSpace,
                                                                                T objectToSerialize) where T:class
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
            serializer.Serialize(writer, objectToSerialize,ns);
            return encoding.GetString(ms.ToArray());
        }

        public static T DeserializeXML<T>(string xml ,string nameSpace) where T:class
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(T), nameSpace);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            return (T)serializer.Deserialize(ms);
        }

        static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {  
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.Error(ReflectObj.ObjectProperty2String(e));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
                
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.Error(ReflectObj.ObjectProperty2String(e));

            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
              
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


    }
}
