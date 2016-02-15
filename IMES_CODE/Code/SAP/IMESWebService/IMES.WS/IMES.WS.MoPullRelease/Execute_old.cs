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
using IMES.WS.MoQuery;
using RemotingService;
using IMES.SAP.Interface;

namespace IMES.WS.MOPullRelease
{
    public class Execute
    {


        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> NotNullItemList = new List<string>
               { 
                   "SerialNumber",
                   "Plant",
                   "MoNumber",
                   "ProductionVer", 
                   "IssuedQty",
                   "CurrentyIssueQty"
                   };

        //1.檢查必要的input parameter
        public static void ValidateParameter(MoPullHeader header)
        {
             
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                logger.DebugFormat("Header: \r\n{0}", ObjectTool.ObjectTostring(header));

                ObjectTool.CheckNullData(NotNullItemList, header);
                float data=0;

                if (!float.TryParse(header.IssuedQty, out data))
                     throw new Exception(" IssuedQty : " + header.IssuedQty + " is not number");

                if (!float.TryParse(header.CurrentyIssueQty, out data))
                    throw new Exception(" CurrentyIssueQty : " + header.CurrentyIssueQty + " is not number");

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
        public static void CheckData(MoPullHeader moheader)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                
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
        public static string Process(MoPullHeader header)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                 string retvalue = "";
                using (TransactionScope txn = UTL.CreateDbTxn())
                {
                   
                    //1. update MO.Qty
                    retvalue= SQL.UpdateMO(header);
                    txn.Complete();
                   
                }

                if (retvalue == "F")
                {
                    ISAPService SapService = ServiceAgent.getInstance().GetObjectByName<ISAPService>(WebConstant.ISAPService);
                    string errorMsg="";
                    if (!SapService.UpdateMO(header.MoNumber, out errorMsg))
                    {
                        BaseLog.LoggingError(logger, "call UpdateMO error Messag : {0}", errorMsg);
                    }

                    using (TransactionScope txn = UTL.CreateDbTxn())
                    {

                        retvalue = SQL.UpdateMO(header);
                        txn.Complete();

                    }

                }
                return retvalue;

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
        public static MoPullReleaseResponse BuildResponseMsg(MoPullHeader moheader, string ret, ref string errMsg)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                MoPullReleaseResponse response = new MoPullReleaseResponse();
                response.MoNumber = moheader.MoNumber;
                response.SerialNumber = moheader.SerialNumber;

                response.Result = ret;
                if (ret != "T" && string.IsNullOrEmpty(errMsg) )
                {
                    errMsg = "Can not find MO [" + moheader.MoNumber + "]";
                }
                

                response.ErrorMessage = errMsg; 
              
                BaseLog.LoggingInfo(logger, "ResponseMsg: \r\n{0}", ObjectTool.ObjectTostring(response));

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