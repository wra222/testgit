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
using UTL.Agent;
using log4net;
using System.Reflection;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;
using System.Collections;
namespace BizRule.Activity
{

    public class CheckWC : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string msgName = "Check_WC";
        private const string remoteUrlName = "IMESService.AutoICTTest";
        //private const string msgDirection = "TAP->Tool";
        private string customerId;
        private bool doExecute(string[] msg,
                                               Socket socket,                                              
                                               SqlConnection dbConnect)
        {
            string replydata = "-1,No Data";            
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string head = "";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
             
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);

                if (Tool.IsCorrectMsg(MsgType.CheckWC, msg, out msgErr, 0))
                {

                    throw new Exception(msgErr);
                }

                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string toolID = msg[CheckWCIndex.ToolIndex];
                string userID = msg[CheckWCIndex.UserIndex];
                string mbSN = msg[CheckWCIndex.MBSNIndex];
                string wc = msg[CheckWCIndex.WCIndex];
                string lineID = msg[CheckWCIndex.LineIndex];
                string timeStamp = msg[CheckWCIndex.TimeStampIndex];
                logger.InfoFormat("Tool ID: {0} , User ID: {1}, MB SN: {2}, WC : {3}, Line ID: {4}", toolID, userID, mbSN, wc, lineID);
                head = msgName + "," + mbSN + "," + timeStamp + ",";
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
                //call iMES Service CheckWC
                //ServiceAgent.getInstance().GetObjectByName<
                string remoteAddr= ConfigurationManager.AppSettings["SAAddress"];
                string remotePort= ConfigurationManager.AppSettings["SAPort"];
                string uri= ConfigurationManager.AppSettings["SAUri"];
                customerId = ConfigurationManager.AppSettings["CustomerId"];
                //IAutoICTTest iAutoICTTest = ServiceAgent.getInstance().GetObjectByName<IAutoICTTest>(remoteUrlName);
               // ArrayList arr= iAutoICTTest.CheckWC(toolID, userID, mbSN, wc, lineID, customerId);
                //=> STX Check_WC, 1234M67890,2010090809037700,-2,Not Match tool idETX
               
                //replydata = head + arr[0].ToString() + ",";
                replydata = "";
                TransmitMsg.sendData(replydata, msgName, socket);
                  
            }
            catch (FisException ex)
            {
                switch(ex.mErrcode)
                {
                    case "CHK1099":
                        replydata =head+ "-1," + ex.mErrmsg;
                        break;
                    case "CHK1098":
                        replydata = head + "-2," + ex.mErrmsg;
                        break;
                    case "CHK002":
                        replydata = head + "-3," + ex.mErrmsg;
                        break;
                    default:
                        replydata = head + "-4," + ex.mErrmsg;
                        break;
                }
                logger.Error(ex);
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;

            }
            catch (Exception e)
            {
                logger.Error(e);
                replydata = head + "-5," + e.Message;
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }       

            return true;

        }

        #region IBiz Members

        bool IBiz.execute(string msgstr, SqlConnection dbConnect, Socket socket)
        {
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter), socket, dbConnect);
        }

        void IBiz.init( XmlNodeList parameters)
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
