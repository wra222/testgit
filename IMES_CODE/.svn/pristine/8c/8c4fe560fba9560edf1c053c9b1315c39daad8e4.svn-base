using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.IO;


/// <summary>
///TXmlFormatManager 的摘要说明
/// </summary>

namespace com.inventec.portal.dashboard
{
    public class TXmlFormatManager<T>
    {
        /// <summary>
        /// XmlFormatManager
        /// </summary>
        public TXmlFormatManager()
        {

        }

        public static bool structureToXmlString(T structureObj, ref string _xmlString)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            System.IO.StringWriter writer = new System.IO.StringWriter();
            xmlSer.Serialize(writer, structureObj);
            _xmlString = writer.ToString();
            writer.Close();

            return true;
        }

        public static bool xmlStringToStructure(string _xmlString, ref T t)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            System.IO.StringReader reader = new System.IO.StringReader(_xmlString);
            t = (T)ser.Deserialize(reader);
            reader.Close();

            return true;
        }

    }
}
