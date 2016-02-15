using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Xml;
using UTL;
using log4net;
using System.Reflection;

namespace BizRule.Msg
{
    class TransmitMsg
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const char STX = '\x002';
        const char ETX = '\x003';
        const string msgDirection = "TAP->Tool";

        public static void sendData(string data, string msgName, Socket socket)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                data = STX + data + ETX;
                if (socket.Connected)
                {
                    socket.Send(Encoding.UTF8.GetBytes(data));
                }
                logger.InfoFormat("Reply MsgName:{0} Data:{1}", msgName, data);
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static string getXmAttributeValue(XmlElement element_, string name_)
        {
            try
            {
                if (element_ == null) { return ""; }
                if (element_.HasAttribute(name_))
                { return element_.Attributes[name_].Value; }
                else
                {
                    return "";
                }
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

