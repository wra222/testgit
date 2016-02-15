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
using System.Data;

namespace IMES.WS.MORelease
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
           

                //string h = ObjectTool.ObjectTostring(obj);

                //ObjectTool.CheckNullData(header.NotNullItemList, header);

                int i = 1;
                foreach (DBMoItemDetail item in itemList)
                {
                    logger.DebugFormat("Item{0}: \r\n{1}", i.ToString(), ObjectTool.ObjectTostring(item));

                    if (!string.IsNullOrEmpty(item.Delete))
                        logger.DebugFormat("Component {0} has delete flag", item.Component);

                    ObjectTool.CheckNullData(item.NotNullItemList, item);
                    i++;
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


      

        //2.檢查資料內容
        public static void CheckData(DBMoHeader moheader,DBMoItemDetail[] lstItemdetail)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                //檢查 MO 的 Model是否存在於 FAMOMaterialPreFix 設定中。
                string FAMoMtlPreFixList = WebConfigurationManager.AppSettings["FAMOMaterialPreFix"];
                string[] FAMOMaterialPreFix = FAMoMtlPreFixList.Split(new char[] { ',', ';' });
                int stringCnt = 0;
                for (int i = 0; i < FAMOMaterialPreFix.Length; i++)
                {
                    if (FAMOMaterialPreFix[i].Trim() != "")
                    {
                        bool b = moheader.BuildOutMtl.StartsWith(FAMOMaterialPreFix[i]);
                        if (!b)
                        {
                            stringCnt++;
                        }
                    }
                }
                if (stringCnt == FAMOMaterialPreFix.Length)
                {
                    string errMsg = "This Model :[" + moheader.BuildOutMtl + "]  is not include in FA MO material list";
                    throw new Exception(errMsg);
                }

                string isproduct = SQL.CheckMOBOMPart(moheader.BuildOutMtl);
                if (isproduct == "N")  // semi-product need create Model
                {
                    if (moheader.MaterialType.Trim() == WSConstant.SAPFinishedGood)
                        SQL.CreateModelFamily(moheader.MaterialGroup, moheader.BuildOutMtl, 1, moheader.Plant);
                    else
                        SQL.CreateModelFamily(moheader.MaterialGroup, moheader.BuildOutMtl, 2,moheader.Plant);
                }
                else if (isproduct == "F")
                {
                    if (moheader.MaterialType.Trim() == WSConstant.SAPSemiProduct)
                    {
                        SQL.CreateModelFamily(moheader.MaterialGroup, moheader.BuildOutMtl,2);
                        moheader.IsProduct = "N";
                    }
                    else
                    {
                        SQL.CreateModelFamily("UNKNOW", "UNKNOW",3);
                        string model = moheader.BuildOutMtl;
                        string errMsg = "This Model :[" + moheader.BuildOutMtl + "]  is no data in IMES Model table";

                        moheader.BuildOutMtl = "UNKNOW";

                        moheader.IsProduct = "N";
                        if (moheader.MaterialType.Trim() == WSConstant.SAPFinishedGood)
                            moheader.IsProduct = "Y";

                        SQL.InsertMO(moheader, "Y",
                                             SysHoldCode.MOCheckModelFail,
                                             errMsg);

                        moheader.BuildOutMtl = model;

                        throw new Exception(errMsg);
                    }
                }

                moheader.IsProduct = "N";
                if (moheader.MaterialType.Trim() == WSConstant.SAPFinishedGood)
                    moheader.IsProduct = "Y";

                

                string ispart="";
                List<String> missParts = new List<string>();
                foreach (DBMoItemDetail item in lstItemdetail)
                {
                    ispart = SQL.CheckPart(item.Component);
                    if (ispart == "F")
                    {
                        missParts.Add(item.Component);
                    }
                }

                if (missParts.Count > 0)
                {
                    string errMsg = "This PartNO :[" + string.Join(",", missParts.ToArray()) + "]  is no data in IMES Part table";
                    SQL.InsertMO(moheader, "Y",
                                           SysHoldCode.MOCheckMaterialFail,
                                            errMsg);
                    throw new Exception(errMsg);
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
        public static void Process(DBMoHeader dbheader, DBMoItemDetail[] moitems)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
               using (TransactionScope txn = UTL.CreateDbTxn())
               {
                 
                   //1. insert MO & ModelBOM for SKU level
                    SQL.InsertMO(dbheader,"N","","");

                    //2 insert MOBOM
                    if (moitems.Length > 0 && dbheader.Status != "TECO")
                    {
                        SQL.InsertMOBOM(moitems);
                    }
                    //if (dbheader.Status != "TECO") {
                    //    // if dbheader is finish good then create ModelBOM
                    //    if (dbheader.MaterialType == "ZFRT" || dbheader.MaterialType == "HALB")
                    //    {
                    //        SQL.InsertModelBOM(dbheader.BuildOutMtl, dbheader.Plant, moitems);
                    //    }                    
                    //}
                     

                    //SQL.InsertMOBOM(moitems);
                    //SQL.InsertTxnDataLog("Receive", "MoRelease", dbheader.MoNumber, dbheader.BuildOutMtl, dbheader.TxnId, "", "", "Received","");
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