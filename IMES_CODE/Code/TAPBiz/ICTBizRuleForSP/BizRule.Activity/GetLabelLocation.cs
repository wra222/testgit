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

using System.Collections;
using UTL.SQL;
using IMES.Infrastructure;
namespace BizRule.Activity
{

    public class GetLabelLocation : IBiz
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string msgName = "Get_LabelLocation";
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
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
             
                logger.InfoFormat("socket.Handle:{0} msgName:{1}", socket.Handle.ToInt32(), msgName);

                if (Tool.IsCorrectMsg(MsgType.CheckWC, msg, out msgErr, 0))
                {

                    throw new Exception(msgErr);
                }

                logger.InfoFormat("Receive Data:{0}", string.Join("~", msg));
                string toolID = msg[Get_LabelLocationIndex.ToolIndex];
                string userID = msg[Get_LabelLocationIndex.UserIndex];
                string custsn = msg[Get_LabelLocationIndex.CUSTSNIndex];
                string wc = msg[Get_LabelLocationIndex.WCIndex];
                string lineID = msg[Get_LabelLocationIndex.LineIndex];
                logger.InfoFormat("Tool ID: {0} , User ID: {1}, CUSTSN: {2}, WC : {3}, Line ID: {4}", toolID, userID, custsn, wc, lineID);

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
                //call iMES Service CheckWC
                //ServiceAgent.getInstance().GetObjectByName<
                string remoteAddr= ConfigurationManager.AppSettings["SAAddress"];
                string remotePort= ConfigurationManager.AppSettings["SAPort"];
                string uri= ConfigurationManager.AppSettings["SAUri"];
                customerId = ConfigurationManager.AppSettings["CustomerId"];

                string DBConnect = ConfigurationManager.AppSettings["DbConnectionString"];
                string Timstamp = DateTime.Now.ToString("YYYYMMDDhhmmssfff");

                SqlParameter parakey_toolID = new SqlParameter("@tool_id", SqlDbType.VarChar);
                parakey_toolID.Direction = ParameterDirection.Input;
                parakey_toolID.Value = toolID;

                SqlParameter parakey_userID = new SqlParameter("@user_id", SqlDbType.VarChar);
                parakey_userID.Direction = ParameterDirection.Input;
                parakey_userID.Value = userID;

                SqlParameter parakey_custsn = new SqlParameter("@SN", SqlDbType.VarChar);
                parakey_custsn.Direction = ParameterDirection.Input;
                parakey_custsn.Value = custsn;

                SqlParameter parakey_wc = new SqlParameter("@WC", SqlDbType.VarChar);
                parakey_wc.Direction = ParameterDirection.Input;
                parakey_wc.Value = wc;

                SqlParameter parakey_lineID = new SqlParameter("@Line", SqlDbType.VarChar);
                parakey_lineID.Direction = ParameterDirection.Input;
                parakey_lineID.Value = lineID;

                SqlParameter parakey_Timstamp = new SqlParameter("@Timstamp", SqlDbType.VarChar);
                parakey_Timstamp.Direction = ParameterDirection.Input;
                parakey_Timstamp.Value = Timstamp;

                //DataTable dt = SQLHelper.ExecuteDataFill(DBConnect, CommandType.StoredProcedure, "Get_LabelLocation", parakey_toolID,
                //                                                                                               parakey_userID,
                //                                                                                               parakey_custsn,
                //                                                                                               parakey_wc,
                //                                                                                               parakey_lineID,
                //                                                                                               parakey_Timstamp);


                DataTable dt = SQLHelper.ExecuteDataFill(dbConnect, CommandType.StoredProcedure, "Get_LabelLocation", parakey_toolID,
                                                                                                               parakey_userID,
                                                                                                               parakey_custsn,
                                                                                                               parakey_wc,
                                                                                                               parakey_lineID,
                                                                                                               parakey_Timstamp);


                string Result = dt.Rows[0]["Result"].ToString();
                string Location = dt.Rows[0]["Location"].ToString();
                string Coordinate = dt.Rows[0]["Coordinate"].ToString();
                string Error_Text = dt.Rows[0]["Error_Text"].ToString();




                //List<string> lstLoc = (List<string>)arr[0];
                //List<string> lstCoord = (List<string>)arr[1];
                //string loc = "";
                //string coord = "";
                ////通訊格式如下
                ////STX0~F1,F3,F0,F0,F0~(10,10),(10,20),(x3,y3) ,(x4,y4),(x5,y5)~ETX

                //List<string> lstLocReply = new List<string>();
                //List<string> lstCoordReply = new List<string>();
                //for (int i = 1; i < 6; i++)
                //{
                //    lstLocReply.Add("F0");
                //    lstCoordReply.Add("(X" + i.ToString() + "," + "Y" + i.ToString()+")");
                //}
                //if (lstLoc.Count > 0 && lstCoord.Count > 0)
                //{
                //    for (int i = 0; i < lstLoc.Count; i++)
                //    {
                //        lstLocReply[i] ="F"+ lstLoc[i];
                //        lstCoordReply[i] ="("+ lstCoord[i]+")";
                //        if (i == 4)
                //        { break; }
                //    }
                //    loc = string.Join(",", lstLocReply.ToArray());
                //    coord = string.Join(",", lstCoordReply.ToArray());
             
                //}

                if (Location == "" || Coordinate == "")
                {
                    replydata = "-4~~~No location or Coordinate data";
                }
                else
                {
                    replydata = Result + "~" + Location + "~" + Coordinate + "~";
                }
            
                TransmitMsg.sendData(replydata, msgName, socket);
                  
            }
            catch (FisException ex)
            {
                replydata = "-1~~~" + ex.mErrmsg;
                logger.Error(ex);
                TransmitMsg.sendData(replydata, msgName, socket);
                return false;

            }
            catch (Exception e)
            {
                logger.Error(e);
                replydata = "-5~~~" + e.Message;
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
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter2), socket, dbConnect);
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
