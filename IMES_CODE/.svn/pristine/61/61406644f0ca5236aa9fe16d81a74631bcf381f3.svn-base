using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Data;
using System.Xml;
using BizRule.Config;
using BizRule.Msg;
using TAPInterface;
using UTL;
using UTL.SQL;
using System.IO;
using System.Reflection;
using log4net;

namespace BizRule.Activity
{

    public class Get_LabelLocation : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string msgName = "Get_LabelLocation";
        private string customerId;
        private bool doExecute(string[] msg,
                                               Socket socket,                                              
                                               SqlConnection dbConnect)
        {
            string replydata = string.Format("-1{0}No Data", ParseMsgRule.Delimiter);
            string info = "";
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);
              
                if (Tool.IsCorrectMsg(MsgType.Get_LabelLocation, msg, out msgErr,0))
                {                  
                   
                    throw new Exception(msgErr);
                }

                logger.InfoFormat("Receive Data:{0}", string.Join(ParseMsgRule.Delimiter.ToString(), msg));
                string toolID = msg[CheckWCIndex.ToolIndex];
                string userID = msg[CheckWCIndex.UserIndex];
                string mbSN = msg[CheckWCIndex.MBSNIndex];
                string wc = msg[CheckWCIndex.WCIndex];
                string lineID = msg[CheckWCIndex.LineIndex];
                logger.InfoFormat("Tool ID: {0} , User ID: {1}, SN: {2}, WC : {3}, Line ID: {4}", toolID, userID, mbSN, wc, lineID);

                if (string.IsNullOrEmpty(toolID))
                {
                    throw new Exception("Tool ID is empty");
                }

                if (string.IsNullOrEmpty(userID))
                {
                    throw new Exception("userID is empty");
                }

                if (string.IsNullOrEmpty(mbSN))
                {
                    throw new Exception("SN is empty");
                }

                if (string.IsNullOrEmpty(wc))
                {
                    throw new Exception("WC is empty");
                }

                if (string.IsNullOrEmpty(lineID))
                {
                    throw new Exception("Line is empty");
                }
                string filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + msg[0].Trim();

                StreamReader sr =  new StreamReader(filePath);
                replydata= sr.ReadLine();
                sr.Close();
                TransmitMsg.sendData(replydata, msgName, socket);

                return true;
            }  
            catch (Exception e)
            {
                logger.Error(e);
                replydata = "-5" + ParseMsgRule.Delimiter + ParseMsgRule.Delimiter +ParseMsgRule.Delimiter+ e.Message;
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }       

        }

        #region IBiz Members

        bool IBiz.execute(string msgstr, SqlConnection dbConnect, Socket socket)
        {
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter), socket,dbConnect);
        }

        void IBiz.init(XmlNodeList parameters)
        {
          string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                customerId = ConfigurationManager.AppSettings["CustomerId"];
               
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
     #endregion
        


    }
}  
