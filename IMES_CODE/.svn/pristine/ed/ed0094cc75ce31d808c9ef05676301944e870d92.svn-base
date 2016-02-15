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

namespace BizRule.Activity
{

    class CheckWC : IBiz
    {
        private const string msgName = "Check_WC";
        private const string msgDirection = "TAP->Tool";
        private Log log_;
        private string SAStation_;
        private string FAStation_;
        private int TestFailLimit_;
        private string CustomerId_;
        private bool doExecute(string[] msg,
                                               Socket socket,
                                               Log log,
                                               SqlConnection dbConnect)
        {
            string replydata = "-1,No Data";
            string info = "";
            string msgErr = "";
            try
            {
                foreach (string s in msg)
                { info = info + "," + s; }
                if (Tool.IsCorrectMsg(MsgType.CheckWC, msg, out msgErr,0))
                {
                    log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, "Msg format wrong :" + info);
                    throw new Exception(msgErr);
                }

                string toolID = msg[CheckWCIndex.ToolIndex];
                string userID = msg[CheckWCIndex.UserIndex];
                string mbSN = msg[CheckWCIndex.MBSNIndex];
                string wc = msg[CheckWCIndex.WCIndex];
                string lineID = msg[CheckWCIndex.LineIndex];
                info = string.Format("\r\n   Tool ID: {0} , User ID: {1}, MB SN: {2}, WC : {3}, Line ID: {4}", toolID, userID, mbSN, wc, lineID);

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
                TransmitMsg.sendData(replydata, msgName, socket, log);
                  
            }
            catch (Exception e)
            {
                log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, e.StackTrace);
                log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, e.Message);
                replydata = "-5," + e.Message;
                TransmitMsg.sendData(replydata, msgName, socket, log );
                return false;
            }

            return true;

        }

        #region IBiz Members

        bool IBiz.execute(string msgstr, SqlConnection dbConnect, Socket socket)
        {
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter), socket, log_, dbConnect);
        }

        void IBiz.init(object log, XmlNodeList parameters)
        {
            this.log_ = (Log)log;
            this.SAStation_ = ConfigurationManager.AppSettings["SAStation"];
            this.FAStation_ = ConfigurationManager.AppSettings["FAStation"];
            CustomerId_ = ConfigurationManager.AppSettings["CustomerId"];
            TestFailLimit_ = 3;
            if (parameters != null)
            {
                TestFailLimit_ = TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount") == "" ? 3 : int.Parse(TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount"));

            }
        }
     #endregion
        StationType CheckDBStation(string wc, SqlConnection dbConnect)
        {
            lock (this)
            {
                StationType stationType = Tool.GetStation(FAStation_, SAStation_, wc);
                if (stationType != StationType.NODEFINE)
                { return stationType; }
                string sql = "select StationType from Station where Station=@wc";
                SqlCommand com = new SqlCommand();
                com.Connection = dbConnect;
                com.CommandType = CommandType.Text;
                com.CommandText = sql;
                SQLHelper.createInputSqlParameter(com, "@wc", 32, wc);
                object o= com.ExecuteScalar();
                if (o == null)
                { return StationType.NODEFINE; }
                else
                {
                    string type = o.ToString().Trim().ToUpper();
                    if (type == "SATEST")
                    { return StationType.SA; }
                    else if (type == "FATEST")
                    { return StationType.FA;}
                    else
                    { return StationType.NODEFINE; }
                }
            }
        
        }


    }
}  
