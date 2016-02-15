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

namespace IMES.WS.MOConfirmChange
{
    public class Execute
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //1.檢查必要的input parameter
        public static void ValidateParameter(MoConfirmChangeResult result)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"MoNumber",
                                                                                              "SerialNumber",                                                                                              
                                                                                               "Result"};

                logger.DebugFormat("MoConfirmChangeResult: \r\n{0}", ObjectTool.ObjectTostring(result));

                ObjectTool.CheckNullData(NotNullItemList, result);
                

                //if (result.MoNumber == "")
                //{
                //    throw new Exception("This MoNumber :" + result.MoNumber + "  is no data");
                //}

                //if (result.SerialNumber == "")
                //{
                //    throw new Exception("This SerialNumber :" + result.SerialNumber + "  is no data");
                //}

                //if (result.ItemNumber == "")
                //{
                //    throw new Exception("This ItemNumber :" + result.ItemNumber + "  is no data");
                //}

                //if (result.Result == "")
                //{
                //    throw new Exception("This Result :" + result.Result + "  is no data");
                //}


                //log.DebugFormat("Header: {0}", moheader.ToString());   
                //log.DebugFormat("Header: \r\n{0}", ObjectTool.ObjectTostring(result));                
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
        public static void CheckData(MoConfirmChangeResult CFCResult)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (Common.SQL.CheckSentData(CFCResult.SerialNumber, CFCResult.MoNumber) == "N")
                {
                    { throw new Exception("SerialNumber :" + CFCResult.SerialNumber + " or " + " MoNumber : " + CFCResult.MoNumber + "  is not exists in IMES SendData table"); }
                }
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

        //3.執行DB insert 
        public static void Process(MoConfirmChangeResult CFCResult)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                using (TransactionScope txn = UTL.CreateDbTxn())
                {
                    //1 UpdateStatus
                    string Message = SQL.UpdateStatus(CFCResult);
                    BaseLog.LoggingInfo(logger, Message);                    
                    // 最後一行
                    txn.Complete();
                }
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

        //4.Build Response message structure
        /*public static MoReleaseResponse BuildResponseMsg(MoHeader moheader, bool isOK, string errMsg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                return null;
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

            LoggingEnd(MethodBase.GetCurrentMethod());
            return null;
        }*/
    }
}
