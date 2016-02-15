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
    public class AutoWeight : IBiz
    {
      
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string customerId;
        private const string msgName = "AutoWeight";
        private const string remoteUrlName = "IMESService.AutoUnitWeight";
        private bool doExecute(string[] msg, Socket socket,SqlConnection dbConnect)
        {
            string replydata = "-1,No Data";
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            string head = "";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {

                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);

                if (Tool.IsCorrectMsg(MsgType.AutoWeightIndex, msg, out msgErr, 0))
                {

                    throw new Exception(msgErr);
                }

                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string toolID = msg[AutoWeightIndex.ToolIndex];
                string userID = msg[AutoWeightIndex.UserIndex];
                string custsn = msg[AutoWeightIndex.CUSTSNIndex];
                string wc = msg[AutoWeightIndex.WCIndex];
                string lineID = msg[AutoWeightIndex.LineIndex];
                string timeStamp = msg[AutoWeightIndex.TimeStampIndex];
                logger.InfoFormat("Tool ID: {0} , User ID: {1}, MB SN: {2}, WC : {3}, Line ID: {4}", toolID, userID, custsn, wc, lineID);
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

                if (string.IsNullOrEmpty(wc))
                {
                    throw new Exception("WC is empty");
                }

                if (string.IsNullOrEmpty(lineID))
                {
                    throw new Exception("Line is empty");
                }
                customerId = ConfigurationManager.AppSettings["CustomerId"];
               // IAutoUnitWeight Iweight = ServiceAgent.getInstance().GetObjectByName<IAutoUnitWeight>(remoteUrlName);
               // IList<BomItemInfo> ret = Iweight.InputCustsn("5CG410M2GX", "A1D", "HJY", "85", "HP");
              //  ArrayList ret = Iweight.InputCustsn(custsn, lineID, userID, wc, customerId);
                //string retmessage =ret[0].ToString();
                string retmessage = "";
                //foreach (BomItemInfo ele in ret)
                //{
                //    retmessage += ele.type + "," + ele.PartNoItem + "~";
                //}              

                replydata = "0~" + retmessage.ToString() ;
                TransmitMsg.sendData(replydata, msgName, socket);

            }
            catch (Exception e)
            {
                logger.Error(e);
                replydata =   "-1~" + e.Message;
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
