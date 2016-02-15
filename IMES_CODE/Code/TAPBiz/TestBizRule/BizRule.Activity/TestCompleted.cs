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

namespace BizRule.Activity
{
    class TestCompleted : IBiz
    {
        private const string msgName = "Test_Completed";
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
            string info = "";
            string msgErr = ""; 
            try
            {
                if (Tool.IsCorrectMsg(MsgType.TestCompletedIndex, msg, out msgErr, TestCompletedIndex.IsPassIndex))
                {
                    log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, msgErr);
                    throw new Exception(msgErr);
                }
                string replydata = "-1,No Data";
              
                string mbno = msg[HeaderIndex.MBSNIndex].Trim();
                string wc = msg[HeaderIndex.WCIndex].Trim();
                string toolId = msg[HeaderIndex.ToolIndex].Trim();
                string userId = msg[HeaderIndex.UserIndex].Trim();
                string stationId = msg[HeaderIndex.ToolIndex].Trim();
                string lineId = msg[HeaderIndex.LineIndex].Trim();
                string isPass = msg[TestCompletedIndex.IsPassIndex].Trim(); // 0:pass,-1: fail
              
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
                replydata = sr.ReadLine();
                sr.Close();
                TransmitMsg.sendData(replydata, msgName, socket, log);
                return true;
            }
            catch (Exception e)
            {
                log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, e.StackTrace);
                log.write(LogType.error, socket.Handle.ToInt32(), msgName, msgDirection, e.Message);
                string replydata = "-1," + e.Message;
                TransmitMsg.sendData(replydata, msgName, socket, log);
                return false;
            }



        }



        bool IBiz.execute(string msgstr, SqlConnection dbConnect, Socket socket)
        {
            return doExecute(msgstr.Split(ParseMsgRule.Delimiter), socket, log_, dbConnect);
        }

        void IBiz.init(object log, XmlNodeList parameters)
        {
            log_ = (Log)log;
            this.SAStation_ = ConfigurationManager.AppSettings["SAStation"];
            this.FAStation_ = ConfigurationManager.AppSettings["FAStation"];
            CustomerId_ = ConfigurationManager.AppSettings["CustomerId"];
            // Get Parameter Attribute
            //     TestFailLimit_ = 3;
            if (parameters != null)
            {
                TestFailLimit_ = TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount") == "" ? 3 : int.Parse(TransmitMsg.getXmAttributeValue(parameters[0], "TestFailLimitCount"));

            }
        }



        #region generate SQL Statement
        private string genDCSQL(SqlCommand cmd,
                                                  string wc,
                                                  string line,
                                                   string userId,
                                                   string fixtureId,
                                                   string pcbNo,
                                                   params string[] data)
        {

            string strSQL = "";
            string strAttrName = "";
            string strAttrValue = "";
            string[] strPara;

            // need modify this SQLstatement
            const string strOneSQL = @" insert Champ_Data_Collection(PCBNo, Station, Line, FixtureID, AttrName, AttrValue, Editor, Cdt)
                                                            values(@pcbNo,@wc,@line,@fixtureId,{0},{1},@userId,@now)";


            for (int i = 0; i < data.Length; i++)
            {
                strAttrName = "@attrName" + i.ToString();
                strAttrValue = "@attrValue" + i.ToString();
                strSQL = strSQL + string.Format(strOneSQL, strAttrName, strAttrValue) + "\r\n";
                strPara = data[i].Split('=');
                SQLHelper.createInputSqlParameter(cmd, strAttrName, 32, strPara[0].Trim());
                SQLHelper.createInputSqlParameter(cmd, strAttrValue, 255, strPara[1].Trim());

            }

            strSQL = "begin \r\n" + strSQL + "end";
            cmd.CommandText = strSQL;
            cmd.CommandType = CommandType.Text;
            SQLHelper.createInputSqlParameter(cmd, "@pcbNo", 32, pcbNo);
            SQLHelper.createInputSqlParameter(cmd, "@wc", 32, wc);
            SQLHelper.createInputSqlParameter(cmd, "@line", 32, line);
            SQLHelper.createInputSqlParameter(cmd, "@userId", 32, userId);
            SQLHelper.createInputSqlParameter(cmd, "@fixtureId", 32, fixtureId);
            SQLHelper.createInputSqlParameter(cmd, "@now", DateTime.Now);
            return strSQL;
        }


        private string genSASQL(SqlCommand cmd,
                                                string wc,
                                                string line,
                                                string userId,
                                                string fixtureId,
                                                string pcbNo,
                                                params string[] data)
        {

            string strSQL = "";
            string strAttrName = "";
            string strAttrValue = "";
            string[] strPara;

            // need modify this SQLstatement
            const string strOneSQL = @"if exists(select * from PCBAttr where AttrName={0} and PCBNo=@pcbNo)
                                                   begin
                                                     insert PCBAttrLog(PCBNo,PCBModelID,Station,AttrName,AttrOldValue,AttrNewValue,Descr,Editor,Cdt)
                                                     select a.PCBNo, b.PCBModelID, @wc, a.AttrName, a.AttrValue,{1},'',@userId,@now
                                                       from PCBAttr a, PCB b
                                                      where a.AttrName={0} and 
                                                           a.PCBNo=@pcbNo and
                                                           a.PCBNo = b.PCBNo

                                                     update	PCBAttr
                                                     set   AttrValue={1},
                                                           Udt=@now	
                                                     where AttrName={0} and 
                                                           PCBNo=@pcbNo
                                                   end
                                                else
                                                   begin
                                                     insert PCBAttr(AttrName,PCBNo, AttrValue,Editor,Cdt,Udt)
                                                     values({0},@pcbNo,{1},@userId,@now,@now)
                                                   end";

            for (int i = 0; i < data.Length; i++)
            {
                strAttrName = "@attrName" + i.ToString();
                strAttrValue = "@attrValue" + i.ToString();
                strSQL = strSQL + string.Format(strOneSQL, strAttrName, strAttrValue) + "\r\n";
                strPara = data[i].Split('=');
                SQLHelper.createInputSqlParameter(cmd, strAttrName, 32, strPara[0].Trim());
                SQLHelper.createInputSqlParameter(cmd, strAttrValue, 32, strPara[1].Trim());

            }

            strSQL = "begin \r\n" + strSQL + "end";
            cmd.CommandText = strSQL;
            cmd.CommandType = CommandType.Text;
            SQLHelper.createInputSqlParameter(cmd, "@pcbNo", 32, pcbNo);
            SQLHelper.createInputSqlParameter(cmd, "@wc", 32, wc);
            SQLHelper.createInputSqlParameter(cmd, "@line", 32, line);
            SQLHelper.createInputSqlParameter(cmd, "@userId", 32, userId);
            SQLHelper.createInputSqlParameter(cmd, "@fixtureId", 32, fixtureId);
            SQLHelper.createInputSqlParameter(cmd, "@now", DateTime.Now);
            return strSQL;






        }

        private string genFASQL(SqlCommand cmd,
                                                string wc,
                                                string line,
                                                string userId,
                                                string fixtureId,
                                                string productId,
                                                params string[] data)
        {
            string strSQL = "";
            string strAttrName = "";
            string strAttrValue = "";
            string[] strPara;

            // need modify this SQLstatement
            const string strOneSQL = @"if exists(select * from ProductAttr where AttrName={0} and ProductID=@productId)
	                                                           begin
		                                                         insert ProductAttrLog(ProductID,Model,Station,AttrName,AttrOldValue,AttrNewValue,Descr,Editor,Cdt)
                                                                 select a.ProductID, b.Model, @wc, a.AttrName, a.AttrValue,{1},'',@userId,@now
		                                                           from ProductAttr a, Product b
		                                                          where a.AttrName={0} and 
                                                                       a.ProductID=@productId and
                                                                       a.ProductID = b.ProductID

		                                                         update	ProductAttr
                                                                 set   AttrValue={1},
			                                                           Udt=@now	
		                                                         where AttrName={0} and 
                                                                       ProductID=@productId
                                                               end
	                                                        else
	                                                           begin
		                                                         insert ProductAttr(AttrName,ProductID, AttrValue,Editor,Cdt,Udt)
		                                                         values({0},@productId,{1},@userId,@now,@now)
	                                                           end";

            for (int i = 0; i < data.Length; i++)
            {
                strAttrName = "@attrName" + i.ToString();
                strAttrValue = "@attrValue" + i.ToString();
                strSQL = strSQL + string.Format(strOneSQL, strAttrName, strAttrValue) + "\r\n";
                strPara = data[i].Split('=');

                SQLHelper.createInputSqlParameter(cmd, strAttrName, 32, strPara[0].Trim());
                SQLHelper.createInputSqlParameter(cmd, strAttrValue, 32, strPara[1].Trim());

            }

            strSQL = "begin \r\n" + strSQL + "end";
            cmd.CommandText = strSQL;
            cmd.CommandType = CommandType.Text;
            SQLHelper.createInputSqlParameter(cmd, "@productId", 32, productId);
            SQLHelper.createInputSqlParameter(cmd, "@wc", 32, wc);
            SQLHelper.createInputSqlParameter(cmd, "@line", 32, line);
            SQLHelper.createInputSqlParameter(cmd, "@userId", 32, userId);
            SQLHelper.createInputSqlParameter(cmd, "@fixtureId", 32, fixtureId);
            SQLHelper.createInputSqlParameter(cmd, "@now", DateTime.Now);
            return strSQL;
        }
        private bool IsWrongCollectData(string[] data)
        {

            string[] strPara;
            for (int i = 0; i < data.Length; i++)
            {
                strPara = data[i].Split('=');
                if (strPara.Length != 2)
                { return true; }
            }
            return false;
        }

        #endregion
    }
}
