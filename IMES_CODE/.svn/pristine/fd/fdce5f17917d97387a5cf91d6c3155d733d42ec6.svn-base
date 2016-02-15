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

namespace IMES.WS.MaterialInfoNotice
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter

        public static void ValidateParameter(MaterialInfoNoticeMsg martialNotice)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (string.IsNullOrEmpty(martialNotice.SerialNumber.Trim()))
                {
                    throw new Exception("The SerialNumber of MaterialInfoNoticeMsg is null or no data");
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

       
        }

    

        //2.檢查資料內容
     

        //3.執行DB insert 
        public static void Process(MaterialInfoNoticeMsg martialNotice)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string TxnId=martialNotice.SerialNumber.Trim();
            try

            {
               using (TransactionScope txn = UTL.CreateDbTxn())
                {

                    SQL.InsertSapMaterialData(TxnId);
                    SQL.InsertUpdateSapModelToIMES(TxnId);
                    SQL.InsertUpdateSapPartToIMES(TxnId);
                   // WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive, "MaterialInfoNotice","","",TxnId,
                   //                                                             "", "", EnumMsgState.Success,"");
                   txn.Complete();
                }

            }

            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
              //  WS.Common.SQL.InsertTxnDataLog(EnumMsgCategory.Receive, "MaterialInfoNotice","","",TxnId,"",e.Message,
              //                                                                        EnumMsgState.Fail, "Error Occurred in function Excute.Process");
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            } 
        }

        //4.Build Response message structure
   
    }
}