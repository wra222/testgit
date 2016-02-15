using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Xml;
using UTL;

namespace BizRule.Msg
{
    class TransmitMsg
    {
        const char STX = '\x002';
        const char ETX = '\x003';
        const string msgDirection = "TAP->Tool";

        public static void sendData(string data, string msgName, Socket socket, Log log)
        {
            data = STX + data + ETX;
            if (socket.Connected)
            {
                socket.Send(Encoding.UTF8.GetBytes(data));
            }
            log.write(LogType.Info, socket.Handle.ToInt32(), msgName, msgDirection, data);
        }

        public static string getXmAttributeValue(XmlElement element_, string name_)
        {
            try
            {
                if (element_ == null) { return ""; }
                if (element_.HasAttribute(name_))
                { return element_.Attributes[name_].Value; }
                else
                { return ""; }
            }
            catch (Exception e)
            {
                 return "";
            }
        }
        public static string getXmAttributeValue(XmlNode node_, string name_)
        {
            return getXmAttributeValue((XmlElement)node_, name_);
        }
    }
}
