using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using log4net;
using System.Reflection;
using System.Transactions;
using IMES.WS.Common;
using System.Web.Configuration;
using IMES.Query.DB;

namespace IMES.WS.HPMORelease
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(DBMoHeader header, DBMoItemDetail[] itemList)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                logger.DebugFormat("Header: \r\n{0}", ObjectTool.ObjectTostring(header));


                ObjectTool.CheckNullData(header.NotNullItemList, header);           

            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }                       
        }     

        //2.檢查資料內容
        public static void CheckData(DBMoHeader moheader,DBMoItemDetail[] lstItemdetail)
        {
            string HPPlantCode = WebConfigurationManager.AppSettings["HPPlantCode"];
            if (moheader.Plant != HPPlantCode)
            {
                string errMsg = "This Plant :[" + moheader.Plant + "]  is not HP Plant Code";
                throw new Exception(errMsg);
            }
        }

        //3.執行DB insert 
        public static void Process(DBMoHeader dbheader, DBMoItemDetail[] moitems, string connectionDB)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
               using (TransactionScope txn = UTL.CreateDbTxn())
               {

                    dbheader.IsProduct = "Y";
                    //檢查 MO 的 Model是否存在於 DockMOMaterialPreFix 設定中。
                    string DockMoMtlPreFixList = WebConfigurationManager.AppSettings["DockMOMaterialPreFix"];
                    string[] DockMOMaterialPreFix = DockMoMtlPreFixList.Split(new char[] { ',', ';' });
                    int stringCnt = 0;
                    for (int i = 0; i < DockMOMaterialPreFix.Length; i++)
                    {
                        if (DockMOMaterialPreFix[i].Trim() != "")
                        {
                            bool b = dbheader.BuildOutMtl.StartsWith(DockMOMaterialPreFix[i]);
                            //not match
                            if (!b)
                            {
                                 stringCnt++;
                            }
                        }
                    }
                    if (stringCnt == DockMOMaterialPreFix.Length)
                    {
                        //Model不符合 Docking的Model，則將MO insert 到HP DB
                        //insert MO to HP
                        int dbIndex = 1;
                        SQL.InsertMO(connectionDB, dbIndex, dbheader, "N", "", "");
                    }
                    else
                    {
                        //Model有符合 Dock的Model，則將MO insert 到Docking DB
                        //insert MO to Docking
                        int dbIndex = 0;
                        SQL.InsertMO(connectionDB, dbIndex, dbheader, "N", "", "");
                    }
                    txn.Complete();
               }
            }

            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                //SQL.InsertTxnDataLog("Receive", "MoRelease", dbheader.MoNumber, dbheader.BuildOutMtl, dbheader.TxnId,"",e.Message, "Fail","");
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            } 
        }

        //4.Build Response message structure
        public static MoReleaseResponse BuildResponseMsg(MoHeader moheader, bool isOK, string errMsg)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);            
            try
            {
                MoReleaseResponse response = new MoReleaseResponse();
                      
                if (isOK)
                {
                    response.FailDescr = "";
                    response.MoNumber = moheader.MoNumber;
                    response.Result = "T";
                }
                else
                {
                    response.FailDescr = errMsg;
                    response.MoNumber = moheader.MoNumber;
                    response.Result = "F";
                }
                BaseLog.LoggingInfo(logger,"ResponseMsg: \r\n{0}", ObjectTool.ObjectTostring(response));

               
                //SQL.InsertTxnDataLog("Send", "MoReleaseResponse", moheader.MoNumber, moheader.BuildOutMtl, moheader.TxnId, "", errMsg, "Sending","");
                return response;


            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                //SQL.InsertTxnDataLog("Send", "MoReleaseResponse", moheader.MoNumber, moheader.BuildOutMtl, moheader.TxnId, "",e.Message, "Fail","");
                throw e;

            }

            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }        

        }
    }
}