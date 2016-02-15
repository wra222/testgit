using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using log4net;
using IMES.Query.DB;
using RemotingService;
using IMES.WS.Common;
using IMES.Station.Interface.StationIntf;
using System.Web.Configuration;

namespace IMES.WS.SAPPO
{
    /// <summary>
    /// WSPGIStatus 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WSSAPPO : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string remoteUrlName = "IMESService._PoData";

        [WebMethod]
        public NotifyCancelPoDnResponse NotifyCancelPODN(string TxnId, string PlantCode, string Type, string ID, string ITEM, string Remark)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //1.檢查必要的input parameter
                if (string.IsNullOrEmpty(TxnId))
                {
                    throw new Exception("TxnId is null or empty!");
                }
                if (string.IsNullOrEmpty(PlantCode))
                {
                    throw new Exception("PlantCode is null or empty!");
                }

                if (string.IsNullOrEmpty(Type))
                {
                    throw new Exception("Type is null or empty!");
                }

                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID is null or empty!");
                }

                if (string.IsNullOrEmpty(ITEM))
                {
                    throw new Exception("ITEM is null or empty!");
                }

                logger.InfoFormat("TxnId:{0} \r\n PlanCode:{1} \r\n Type:{2} \r\n ID:{3} \r\n ITEM:{4} \r\n Remark:{5}", TxnId, PlantCode, Type, ID, ITEM, Remark ?? "");

                //3.執行DB insert
                IPoData iCheckRoute = ServiceAgent.getInstance().GetObjectByName<IPoData>(remoteUrlName);
                ReplySAPPo replySAPPo = iCheckRoute.SendSAPCancelPO(TxnId, PlantCode, Type, ID, ITEM, Remark ?? "", "Cancel", "ASUS", "SAP");
                string holdMsg = "";
                string errorMsg = "";
                string RrunEnviroment = WebConfigurationManager.AppSettings["RunEnviroment"];

                //4.Build Response Message
                NotifyCancelPoDnResponse ret = new NotifyCancelPoDnResponse
                {
                    ErrorText = replySAPPo.ErrorMsg,
                    ID = replySAPPo.PoNo,
                    ITEM = replySAPPo.PoItem,
                    Result = replySAPPo.Result,
                    TxnId = replySAPPo.TxnID
                };

                logger.InfoFormat("Reply TxnId:{0} \r\n ID:{1} \r\n ITEM:{2} \r\n ErrorText:{3} \r\n Result:{4}", replySAPPo.TxnID, replySAPPo.PoNo, replySAPPo.PoItem, replySAPPo.ErrorMsg, replySAPPo.Result);
                if (replySAPPo.ErrorMsg.Contains("Hold"))
                {
                    holdMsg = holdMsg + "PO:" + replySAPPo.PoNo + " PO ITEM:" + replySAPPo.PoItem + " " + replySAPPo.ErrorMsg + "<br/>";
                }
                else if (replySAPPo.Result != "T")
                {
                    errorMsg = errorMsg + "PO:" + replySAPPo.PoNo + " PO ITEM:" + replySAPPo.PoItem + " " + replySAPPo.ErrorMsg + "<br/>";
                }

                if (!string.IsNullOrEmpty(holdMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");
                    //Send email
                    UTL.SendMail("SAPCancelPOHold", "Notify Cancel PO Hold Changed(" + RrunEnviroment + ")", holdMsg);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");
                    UTL.SendMail("SAPCancelPOFail", "Notify Cancel PO Receiving Error(" + RrunEnviroment + ")", errorMsg);
                }

                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);

                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                NotifyCancelPoDnResponse ret = new NotifyCancelPoDnResponse
                {
                    ErrorText = e.Message,
                    ID = ID,
                    ITEM = ITEM,
                    Result = "F",
                    TxnId = TxnId
                };
                //Send email
                UTL.SendMail("Notify Cancel PO error", "TxnId:" + TxnId + " " + e.Message);
                return ret;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
          
        }



        [WebMethod]
        public List<NotifyPoInfoResponse> NotifyPoInfo(string TxnId, string PlantCode, string PoData, string Remark)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {            
                //1.檢查必要的input parameter
               if (string.IsNullOrEmpty(TxnId))
               {
                   throw new Exception("TxnId is null or empty!");
               }
               if (string.IsNullOrEmpty(PlantCode))
               {
                   throw new Exception("PlantCode is null or empty!");
               }

               if (string.IsNullOrEmpty(PoData))
               {
                   throw new Exception("PoData is null or empty!");
               }

               logger.InfoFormat("TxnId:{0} \r\n PlanCode:{1} \r\n PoData:{2} \r\n Remark:{3}", TxnId, PlantCode, PoData, Remark ?? "");

                //3.執行DB insert
               IPoData iCheckRoute = ServiceAgent.getInstance().GetObjectByName<IPoData>(remoteUrlName);
              IList<ReplySAPPo> replySAPPoList= iCheckRoute.SendSAPPoInfo(TxnId, PlantCode, "PO", PoData, Remark ?? "", "PO", "ASUS", "SAP");
              bool hasHold = replySAPPoList.Any(x => x.ErrorMsg.Contains("[Hold]"));  
              string holdMsg="";
              string errorMsg = "";
              string fruHoldMsg = "";
              string fruErrorMsg = "";

              string RrunEnviroment = WebConfigurationManager.AppSettings["RunEnviroment"];
              //4.Build Response Message
              List<NotifyPoInfoResponse> ResponseList = new List<NotifyPoInfoResponse>();
              foreach (ReplySAPPo item in replySAPPoList)
              {
                  ResponseList.Add(new NotifyPoInfoResponse
                  {
                      CUST_PO_NO = item.PoNo,
                      CUST_PO_NO_ITEM = item.PoItem,
                      ErrorText = item.ErrorMsg,
                      TxnId = TxnId,
                      Result = item.Result
                  });
                  logger.InfoFormat("Reply PO:{0} \r\n PO_ITEM:{1} \r\n ErrorText:{2} \r\n Result:{3} \r\n IsFRU:{4} \r\n ManualOrderFlag:{5}",
                                              item.PoNo, item.PoItem, item.ErrorMsg, item.Result, item.IsFRU.ToString(), item.ManualOrderFlag);
                  if (item.IsFRU)
                  {
                      if (item.ErrorMsg.Contains("Hold"))
                      {
                          fruHoldMsg = fruHoldMsg + "PO:" + item.PoNo + " PO ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                      }
                      else if (item.Result != "T")
                      {
                          fruErrorMsg = fruErrorMsg + "PO:" + item.PoNo + " PO ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                      }
                  }
                  else
                  {
                      if (item.ErrorMsg.Contains("Hold"))
                      {
                          holdMsg = holdMsg + "PO:" + item.PoNo + " PO ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                      }
                      else if (item.Result != "T")
                      {
                          errorMsg = errorMsg + "PO:" + item.PoNo + " PO ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                      }
                  }
              }

              //if (hasHold)
              if (!string.IsNullOrEmpty(holdMsg))
              {
                  BaseLog.LoggingInfo(logger, "Start Send mail...");
                  holdMsg = holdMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                  holdMsg = holdMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                  holdMsg = holdMsg + "****************************************************************************************<br/>";
                  holdMsg = holdMsg + "<br/>JSON String :" + PoData + "<br/>";
                  //Send email
                  UTL.SendMail("SAPPOHold", "Notify SKU PO Hold Changed(" + RrunEnviroment + ")", holdMsg);                  
              }
              if (!string.IsNullOrEmpty(errorMsg))
              {
                  BaseLog.LoggingInfo(logger, "Start Send mail...");
                  errorMsg = errorMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                  errorMsg = errorMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                  errorMsg = errorMsg + "****************************************************************************************<br/>";
                  errorMsg = errorMsg + "<br/>JSON String :" + PoData + "<br/>";
                  UTL.SendMail("SAPPOFail", "Notify SKU PO Receiving Error(" + RrunEnviroment + ")", errorMsg);                 
               
              }

              if (!string.IsNullOrEmpty(fruHoldMsg))
              {
                  BaseLog.LoggingInfo(logger, "Start Send mail...");
                  //Send email
                  fruHoldMsg = fruHoldMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                  fruHoldMsg = fruHoldMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                  fruHoldMsg = fruHoldMsg + "****************************************************************************************<br/>";
                  fruHoldMsg = fruHoldMsg + "<br/>JSON String :" + PoData + "<br/>";
                  UTL.SendMail("SAPFruPOHold", "Notify FRU PO Hold Changed(" + RrunEnviroment + ")", fruHoldMsg);
              }
              if (!string.IsNullOrEmpty(fruErrorMsg))
              {
                  BaseLog.LoggingInfo(logger, "Start Send mail...");
                  fruErrorMsg = fruErrorMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                  fruErrorMsg = fruErrorMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                  fruErrorMsg = fruErrorMsg + "****************************************************************************************<br/>";
                  fruErrorMsg = fruErrorMsg + "<br/>JSON String :" + PoData + "<br/>";
                  UTL.SendMail("SAPFruPOFail", "Notify FRU PO Receiving Error(" + RrunEnviroment + ")", fruErrorMsg);
              }
              return ResponseList;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);

                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyPoInfoResponse> ResponseList = new List<NotifyPoInfoResponse>();

                ResponseList.Add(new NotifyPoInfoResponse
                {
                    CUST_PO_NO = "",
                    CUST_PO_NO_ITEM = "",
                    ErrorText = e.Message,
                    TxnId = TxnId,
                    Result = "F"
                }); 
                //Send email
                UTL.SendMail("Notify PO Info program error", "TxnId:" +TxnId + " "+  e.Message);
                return ResponseList;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        
        }

        [WebMethod]
        public List<NotifyPackageInfoResponse> NotifyPackageInfo(string TxnId, string PlantCode, string DeliveryData, string Remark)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //1.檢查必要的input parameter
                if (string.IsNullOrEmpty(TxnId))
                {
                    throw new Exception("TxnId is null or empty!");
                }
                if (string.IsNullOrEmpty(PlantCode))
                {
                    throw new Exception("PlantCode is null or empty!");
                }

                if (string.IsNullOrEmpty(DeliveryData))
                {
                    throw new Exception("DeliveryData is null or empty!");
                }

                logger.InfoFormat("TxnId:{0} \r\n PlanCode:{1} \r\n DeliveryData:{2} \r\n Remark:{3}", TxnId, PlantCode, DeliveryData, Remark ?? "");

                //3.執行DB insert
                IPoData iCheckRoute = ServiceAgent.getInstance().GetObjectByName<IPoData>(remoteUrlName);
                IList<ReplySAPPo> replySAPPoList = iCheckRoute.SendSAPPoInfo(TxnId, PlantCode, "DNPO", DeliveryData, Remark ?? "", "DN", "ASUS", "SAP");
                bool hasHold = replySAPPoList.Any(x => x.ErrorMsg.Contains("Hold"));
                string holdMsg = "";
                string errorMsg = "";
                string fruHoldMsg = "";
                string fruErrorMsg = "";

                string RrunEnviroment = WebConfigurationManager.AppSettings["RunEnviroment"];
                //4.Build Response Message
                List<NotifyPackageInfoResponse> ResponseList = new List<NotifyPackageInfoResponse>();
                foreach (ReplySAPPo item in replySAPPoList)
                {
                    ResponseList.Add(new NotifyPackageInfoResponse
                    {
                        DN = item.PoNo,
                        DN_ITEM = item.PoItem,
                        ErrorText = item.ErrorMsg,
                        TxnId = TxnId,
                        Result = item.Result
                    });

                    logger.InfoFormat("Reply DN:{0} \r\n DN_ITEM:{1} \r\n ErrorText:{2} \r\n Result:{3} \r\n IsFRU:{4} \r\n ManualOrderFlag:{5}", 
                                                item.PoNo, item.PoItem, item.ErrorMsg, item.Result,item.IsFRU.ToString(), item.ManualOrderFlag);
                    if (item.IsFRU)
                    {
                        if (item.ErrorMsg.Contains("Hold"))
                        {
                            fruHoldMsg = fruHoldMsg + "DN:" + item.PoNo + " DN ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                        }
                        else if (item.Result != "T")
                        {
                            fruErrorMsg = fruErrorMsg + "DN:" + item.PoNo + " DN ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                        }
                    }
                    else
                    {
                        if (item.ErrorMsg.Contains("Hold"))
                        {
                            holdMsg = holdMsg + "DN:" + item.PoNo + " DN ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                        }
                        else if (item.Result != "T")
                        {
                            errorMsg = errorMsg + "DN:" + item.PoNo + " DN ITEM:" + item.PoItem + " " + "ManualOrderFlag:" + item.ManualOrderFlag + " " + item.ErrorMsg + "<br/>";
                        }
                    }
                    
                }

                //if (hasHold)
                if (!string.IsNullOrEmpty(holdMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");
                    holdMsg = holdMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                    holdMsg = holdMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                    holdMsg = holdMsg + "****************************************************************************************<br/>";
                    holdMsg = holdMsg + "<br/>JSON String :" + DeliveryData + "<br/>";
                    //Send email
                    UTL.SendMail("SAPDNHold", "Notify SKU Delivery Hold Changed(" + RrunEnviroment + ")", holdMsg);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");                   

                    errorMsg = errorMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                    errorMsg = errorMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                    errorMsg = errorMsg + "****************************************************************************************<br/>";
                    errorMsg = errorMsg + "<br/>JSON String :" + DeliveryData + "<br/>";
                    UTL.SendMail("SAPDNFail", "Notify SKU DN Receiving Error(" + RrunEnviroment + ")", errorMsg);
                }

                if (!string.IsNullOrEmpty(fruHoldMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");
                    fruHoldMsg = fruHoldMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                    fruHoldMsg = fruHoldMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                    fruHoldMsg = fruHoldMsg + "****************************************************************************************<br/>";
                    fruHoldMsg = fruHoldMsg + "<br/>JSON String :" + DeliveryData + "<br/>";
                    //Send email
                    UTL.SendMail("SAPFruDNHold", "Notify FRU Delivery Hold Changed(" + RrunEnviroment + ")", fruHoldMsg);
                }

                if (!string.IsNullOrEmpty(fruErrorMsg))
                {
                    BaseLog.LoggingInfo(logger, "Start Send mail...");
                    fruErrorMsg = fruErrorMsg + "<br/><br/><br/><br/>****************************************************************************************<br/>";
                    fruErrorMsg = fruErrorMsg + "  Please Ignore the message as below ! The message only for system developers, thank you!  <br/>";
                    fruErrorMsg = fruErrorMsg + "****************************************************************************************<br/>";
                    fruErrorMsg = fruErrorMsg + "<br/>JSON String :" + DeliveryData + "<br/>";
                    UTL.SendMail("SAPFruDNFail", "Notify FRU DN Receiving Error(" + RrunEnviroment + ")", fruErrorMsg);
                }

                return ResponseList;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);

                //  UTL.SendMail("test", e.Message);
                //4.Build Response Error Message
                List<NotifyPackageInfoResponse> ResponseList = new List<NotifyPackageInfoResponse>();

                ResponseList.Add(new NotifyPackageInfoResponse
                {
                    DN = "",
                    DN_ITEM = "",
                    ErrorText = e.Message,
                    TxnId = TxnId,
                    Result = "F"
                });
                //Send email
                UTL.SendMail("Notify Delivery Info program error", "TxnId:" + TxnId + " " + e.Message);
                return ResponseList;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }


        
        

    }

    
}
