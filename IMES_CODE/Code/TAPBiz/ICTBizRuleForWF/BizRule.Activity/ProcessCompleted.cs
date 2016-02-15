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
using System.Collections;
using UTL.SQL;
using IMES.Infrastructure;
namespace BizRule.Activity
{
    public class ProcessCompleted : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string msgName = "Process_Completed";
        private string customerId;
        private const string remoteUrlName = "IMESService._LabelLightGuide";
        private bool doExecute(string[] msg,
                                                Socket socket,                                             
                                                SqlConnection dbConnect)
        {
            string msgErr = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);
                if (Tool.IsCorrectMsg(MsgType.ProcessCompletedIndex, msg, out msgErr, TestCompletedIndex.IsPassIndex))
                {
                    throw new Exception(msgErr);
                }
                //    string replydata = "-1,No Data";
                logger.InfoFormat("Receive Data:{0}", string.Join(",", msg));
                string custsn = msg[HeaderIndex.MBSNIndex].Trim();
                string wc = msg[HeaderIndex.WCIndex].Trim();
                string toolId = msg[HeaderIndex.ToolIndex].Trim();
                string userId = msg[HeaderIndex.UserIndex].Trim();
                string stationId = msg[HeaderIndex.ToolIndex].Trim();
                string lineId = msg[HeaderIndex.LineIndex].Trim();
                string isPass = msg[TestCompletedIndex.IsPassIndex].Trim(); // 0:pass,-1: fail

                string failureCode = msg[TestCompletedIndex.FailureCodeIndex];
           


                if (string.IsNullOrEmpty(toolId))
                {
                    throw new Exception("Tool ID is empty");
                }

                if (string.IsNullOrEmpty(userId))
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

                if (string.IsNullOrEmpty(lineId))
                {
                    throw new Exception("Line is empty");
                }

                //Call IMES Service
                //   string remoteAddr = ConfigurationManager.AppSettings["SAAddress"];
           
                //ILabelLightGuide iLabelLightGuide = ServiceAgent.getInstance().GetObjectByName<ILabelLightGuide>(remoteUrlName);
                //ArrayList arr = iLabelLightGuide.ProcessCompleted(custsn, wc, lineId, userId,isPass,failureCode, customerId);

                string DBConnect = ConfigurationManager.AppSettings["DbConnectionString"];
                string Timstamp = DateTime.Now.ToString("YYYYMMDDhhmmssfff");

                SqlParameter parakey_toolID = new SqlParameter("@tool_id", SqlDbType.VarChar);
                parakey_toolID.Direction = ParameterDirection.Input;
                parakey_toolID.Value = toolId;

                SqlParameter parakey_userID = new SqlParameter("@user_id", SqlDbType.VarChar);
                parakey_userID.Direction = ParameterDirection.Input;
                parakey_userID.Value = userId;

                SqlParameter parakey_custsn = new SqlParameter("@SN", SqlDbType.VarChar);
                parakey_custsn.Direction = ParameterDirection.Input;
                parakey_custsn.Value = custsn;

                SqlParameter parakey_wc = new SqlParameter("@WC", SqlDbType.VarChar);
                parakey_wc.Direction = ParameterDirection.Input;
                parakey_wc.Value = wc;

                SqlParameter parakey_lineID = new SqlParameter("@Line", SqlDbType.VarChar);
                parakey_lineID.Direction = ParameterDirection.Input;
                parakey_lineID.Value = lineId;

                SqlParameter parakey_Timstamp = new SqlParameter("@Timstamp", SqlDbType.VarChar);
                parakey_Timstamp.Direction = ParameterDirection.Input;
                parakey_Timstamp.Value = Timstamp;

                SqlParameter parakey_Is_pass = new SqlParameter("@Is_pass", SqlDbType.VarChar);
                parakey_Is_pass.Direction = ParameterDirection.Input;
                parakey_Is_pass.Value = isPass;//parakey_Timstamp

                SqlParameter parakey_Failure_Code = new SqlParameter("@Failure_Code", SqlDbType.VarChar);
                parakey_Failure_Code.Direction = ParameterDirection.Input;
                parakey_Failure_Code.Value = failureCode;//parakey_Timstamp

                //DataTable dt = SQLHelper.ExecuteDataFill(DBConnect, CommandType.StoredProcedure, "Process_Completed", parakey_toolID,
                //                                                                                               parakey_userID,
                //                                                                                               parakey_custsn,
                //                                                                                               parakey_wc,
                //                                                                                               parakey_lineID,
                //                                                                                               parakey_Timstamp,
                //                                                                                               parakey_Is_pass,
                //                                                                                               parakey_Failure_Code);

                DataTable dt = SQLHelper.ExecuteDataFill(dbConnect, 
                                                                            CommandType.StoredProcedure, "Process_Completed", parakey_toolID,
                                                                                                               parakey_userID,
                                                                                                               parakey_custsn,
                                                                                                               parakey_wc,
                                                                                                               parakey_lineID,
                                                                                                               parakey_Timstamp,
                                                                                                               parakey_Is_pass,
                                                                                                               parakey_Failure_Code);



                TransmitMsg.sendData(dt.Rows[0]["Result"].ToString() + "~", msgName, socket);
                return true;
            }
            catch (FisException ex)
            {
                logger.Error(ex);
                string replydata = "-1," + ex.mErrmsg;
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e);
                string replydata = "-1," + e.Message;
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
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter2), socket, dbConnect);
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
