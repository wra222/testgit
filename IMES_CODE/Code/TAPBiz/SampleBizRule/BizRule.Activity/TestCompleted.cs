using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Data;
using System.Configuration;
using System.Xml;
using UTL;
using BizRule.Config;
using BizRule.Msg;
using TAPInterface;
using UTL.SQL;
using System.IO;
using System.Reflection;
using log4net;

namespace BizRule.Activity
{
    public class TestCompleted : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string msgName = "Test_Completed";
        private int testFailLimit;
        private string customerId;

        private bool doExecute(string[] msg,
                                                Socket socket,                                              
                                                SqlConnection dbConnect)
        {
            string info = "";
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);
                if (Tool.IsCorrectMsg(MsgType.TestCompletedIndex, msg, out msgErr, TestCompletedIndex.IsPassIndex))
                {
                    throw new Exception(msgErr);
                }
                string replydata = "-1,No Data";
                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string mbno = msg[HeaderIndex.MBSNIndex].Trim();
                string wc = msg[HeaderIndex.WCIndex].Trim();
                string toolId = msg[HeaderIndex.ToolIndex].Trim();
                string userId = msg[HeaderIndex.UserIndex].Trim();
                string stationId = msg[HeaderIndex.ToolIndex].Trim();
                string lineId = msg[HeaderIndex.LineIndex].Trim();
                string isPass = msg[TestCompletedIndex.IsPassIndex].Trim(); // 0:pass,-1: fail
                string timeStamp = msg[HeaderIndex.TimeStampIndex].Trim();
                string defectCode = msg[TestCompletedIndex.FailureCodeIndex];
                string dataCollection = msg[TestCompletedIndex.TestFileNameIndex];

                if (string.IsNullOrEmpty(toolId))
                {
                    throw new Exception("Tool ID is empty");
                }

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("userID is empty");
                }

                if (string.IsNullOrEmpty(mbno))
                {
                    throw new Exception("SN is empty");
                }

                if (string.IsNullOrEmpty(wc))
                {
                    throw new Exception("WC is empty");
                }

                if (string.IsNullOrEmpty(lineId))
                {
                    throw new Exception("Line is empty");
                }

                string filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + msg[0].Trim();

                StreamReader sr = new StreamReader(filePath);
                replydata = msgName + "," + mbno + "," + timeStamp + "," + sr.ReadLine();
                sr.Close();
                TransmitMsg.sendData(replydata, msgName, socket);
                return true;
            }            
            catch (Exception e)
            {
                logger.Error(e);
                string replydata = "-1," + e.Message;
                TransmitMsg.sendData(replydata, msgName, socket );
                return false;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }



        bool IBiz.execute(string msgstr, SqlConnection dbConnect, Socket socket)
        {
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter), socket, dbConnect);
        }

        void IBiz.init(XmlNodeList parameters)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                customerId = ConfigurationManager.AppSettings["CustomerId"];
                testFailLimit= 3;
                if (parameters != null)
                {
                    testFailLimit = TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount") == "" ? 3 : int.Parse(TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount"));

                }
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
    }
}
