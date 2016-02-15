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
    public class LabelLightGuid : IBiz
    {

        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string msgName = "LabelLightGuid";
        private const string remoteUrlName = "IMESService._LabelLightGuide";
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

                if (Tool.IsCorrectMsg(MsgType.LabelLightGuidIndex, msg, out msgErr, 0))
                {

                    throw new Exception(msgErr);
                }

                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string toolID = msg[LabelLightGuidIndex.ToolIndex];
                string userID = msg[LabelLightGuidIndex.UserIndex];
                string custsn = msg[LabelLightGuidIndex.CUSTSNIndex];
                string labelcode = msg[LabelLightGuidIndex.LabelCode];
                string wc = msg[LabelLightGuidIndex.WCIndex];
                string lineID = msg[LabelLightGuidIndex.LineIndex];
                string timeStamp = msg[LabelLightGuidIndex.TimeStampIndex];
                logger.InfoFormat("Tool ID: {0} , User ID: {1}, CUST SN: {2}, WC : {3}, Line ID: {4}", toolID, userID, custsn, wc, lineID);
                head = msgName + "," + custsn + "," + timeStamp + ",";
                if (string.IsNullOrEmpty(toolID))
                {
                    throw new Exception("Tool ID is empty");
                }

                if (string.IsNullOrEmpty(userID))
                {
                    throw new Exception("userID is empty");
                }

                if (string.IsNullOrEmpty(custsn))
                {
                    throw new Exception("SN is empty");
                }
                if (string.IsNullOrEmpty(labelcode))
                {
                    throw new Exception("labelcode is empty");
                }
                if (string.IsNullOrEmpty(wc))
                {
                    throw new Exception("WC is empty");
                }

                if (string.IsNullOrEmpty(lineID))
                {
                    throw new Exception("Line is empty");
                }
                customerId = ConfigurationManager.AppSettings["CustomerId"];
                ILabelLightGuide labellightguide = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(remoteUrlName);
                IList<string> lightlist = labellightguide.GetLightNumFroAOI(toolID, userID, custsn.Trim(), labelcode, wc, lineID, timeStamp);
                string result = "";
                for (int j = 0; j < lightlist.Count; j++)
                {
                    result = result + lightlist[j].ToString() + ",";
                    
                }
                replydata = "0~"+result ;
                TransmitMsg.sendData(replydata, msgName, socket);

            }
            catch (FisException ex)
            {
                logger.Error(ex);
                replydata =  "-1~" + ex.mErrmsg;
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e);
                replydata = "-1~" + e.Message;
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
