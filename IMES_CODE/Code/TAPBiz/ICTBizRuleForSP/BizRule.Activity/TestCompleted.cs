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
using System.IO;
using System.Reflection;
using log4net;
using UTL.Agent;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;
using System.Collections;
namespace BizRule.Activity
{
    public class TestCompleted : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string msgName = "Test_Completed";
        private string customerId;
        private const string remoteUrlName = "IMESService.AutoICTTest";
        private bool doExecute(string[] msg,
                                                Socket socket,                                             
                                                SqlConnection dbConnect)
        {
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string head = "";
            string replydata = "";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);
                if (Tool.IsCorrectMsg(MsgType.TestCompletedIndex, msg, out msgErr, TestCompletedIndex.IsPassIndex))
                {
                    throw new Exception(msgErr);
                }
                //    string replydata = "-1,No Data";
                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string mbno = msg[HeaderIndex.MBSNIndex].Trim();
                string wc = msg[HeaderIndex.WCIndex].Trim();
                string toolId = msg[HeaderIndex.ToolIndex].Trim();
                string userId = msg[HeaderIndex.UserIndex].Trim();
                string stationId = msg[HeaderIndex.ToolIndex].Trim();
                string lineId = msg[HeaderIndex.LineIndex].Trim();
                string timeStamp = msg[HeaderIndex.TimeStampIndex];
                string isPass = msg[TestCompletedIndex.IsPassIndex].Trim(); // 0:pass,-1: fail

                string defectCode = msg[TestCompletedIndex.FailureCodeIndex];
                string cutDefectCode = defectCode.Length > 1024 ? defectCode.Substring(0, 1024) : defectCode;
                if (defectCode.Length > 1024)
                { logger.Info("The length of failure code>1024 :" + defectCode); }

                string dataCollection = msg[TestCompletedIndex.TestFileNameIndex];
                head = msgName + "," + mbno + "," + timeStamp + ",";
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

                //Call IMES Service
                //IAutoICTTest iAutoICTTest = ServiceAgent.getInstance().GetObjectByName<IAutoICTTest>(remoteUrlName);
                string actionName = "AutoICTTest";
                string testLogErrorCode = "OT05";
                IList<string> defectList = new List<string>();
                if (isPass != "0")
                { defectList.Add("OT05"); }
             //        ArrayList ICTTestCompleted(string mbSN, string station, string toolId, string userId,string line,string customer,
             //                                          string isPass, string failureCode, string testLogFilename, string actionName, IList<string> defectList, string testLogErrorCode);
             //   ArrayList arr = iAutoICTTest.ICTTestCompleted(mbno, wc, toolId, userId, lineId, customerId, isPass, cutDefectCode, dataCollection);
               // ArrayList arr = iAutoICTTest.ICTTestCompleted(mbno, wc, toolId, userId, lineId, customerId,
                //                                                                           isPass, cutDefectCode, dataCollection, actionName, defectList, testLogErrorCode);
           
                replydata ="";//head + arr[0].ToString() + ",";
                TransmitMsg.sendData(replydata, msgName, socket);
                return true;
            }
            catch (FisException ex)
            {
                logger.Error(ex);
                 replydata = head+"-1," + ex.mErrmsg;
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e);
                replydata = head + "-1," + e.Message;
                TransmitMsg.sendData(replydata, msgName, socket);
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
