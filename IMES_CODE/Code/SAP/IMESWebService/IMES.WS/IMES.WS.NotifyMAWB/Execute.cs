﻿using System;
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

namespace IMES.WS.NotifyMAWB
{
   
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(string connectionDB_BK, NotifyMAWB[] MAWBItems, string BatchId)
        {
            string connectionDB = "SD_DBServer";
            //string connectionDB_BK = "SD_DBServer_BK";
            //string connectionDB_BK = "SD_DBServer_BK_CQ";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                List<string> NotNullItemList = new List<string> {"SerialNumber",
                                                                 "MAWB",
                                                                 "Plant",
                                                                 "DN",
                                                                 "Items",
                                                                 "Declaration"};
                foreach (NotifyMAWB item in MAWBItems)
                {
                    logger.DebugFormat("NotifyMAWB: \r\n{0}", ObjectTool.ObjectTostring(item));
                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Receive,
                                            "NotifyMAWB",
                                            string.IsNullOrEmpty(item.MAWB) ? "" : item.MAWB,
                                            "",
                                            string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                            "",
                                            "",
                                            EnumMsgState.Received,
                                            item.Declaration+","+item.Containerid);
                    SQL.InsertTempMAWB(connectionDB_BK, dbIndex, item, "Receive", "", BatchId);
                }

                foreach (NotifyMAWB item in MAWBItems)
                {
                    string SerialNumber = item.SerialNumber;
                    string MAWB = item.MAWB;
                    //Check null data
                    string className = item.GetType().BaseType.Name;
                    if (className == "Object")
                    { className = item.GetType().Name; }
                    string title = "These columns of " + className + " are null or no data : ";
                    string error = "";
                    foreach (string itemcolumn in NotNullItemList)
                    {
                        if (string.IsNullOrEmpty(GetValueByType(itemcolumn, item).Trim()))
                        { error = error + itemcolumn + ","; }
                    }

                    if (error != "")
                    {
                        error = title + error;
                        //Update MAWB Status.
                        string State = "Fail";
                        SQL.UpdateTempMAWB(connectionDB_BK, dbIndex, SerialNumber, MAWB, State, error);
                    } 
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
        public static void Process(string connectionDB_BK, NotifyMAWB[] MAWBItems, string BatchId)
        {
            string connectionDB = "SD_DBServer";
            //string connectionDB_BK = "SD_DBServer_BK";
            //string connectionDB_BK = "SD_DBServer_BK_CQ";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                //Log success message to PGILog.
                string dataType = "MAWB";
                string State = "Success";
                string ErrorDescr = "";
                //Get DB Connection string
                List<SAPMAWBDef> DefList = SQL.GetSAPMawbDef(connectionDB, dbIndex, dataType);

                //Get MAWB items
                List<MAWBMaster> MAWBMaster = SQL.GetMawbMaster(connectionDB_BK, dbIndex, BatchId);
                foreach (MAWBMaster itemM in MAWBMaster)
                {
                    if (itemM.State != "F") {
                        //Get MAWB detail items
                        List<MAWBDetail> MAWBDetail = SQL.GetMawbDetail(connectionDB_BK, dbIndex, itemM.SerialNumber);
                        foreach (SAPMAWBDef DefItem in DefList)
                        {
                            int itemcnt = 0;
                            int itemcnt_udDN = 0;
                            int itemcnt_udAll = 0;
                                        
                            foreach (MAWBDetail itemD in MAWBDetail)
                            {
                                if (itemD.Plant == DefItem.PlantCode)
                                {
                                    //MAWB Case 1 : (TSB, FJ)
                                    //刪除時，依據@MAWB & DN & Items為條件，將上次insert的先刪除
                                    //新增時，取SAP.MAWB下滑線前的值=@MAWB，insert @MAWB 至MAWB表
                                    if (DefItem.VolumnUnit == "DbyDN")
                                    {
                                        BaseLog.LoggingInfo(logger, "MAWB Type:[DbyDN] for TSB,FJ");
                                        BaseLog.LoggingInfo(logger, "SAPWeightDef: \r\n{0}", DefItem.DBName+", Descr:"+DefItem.Descr);
                                        //if (itemcnt_udDN == 0)
                                        //{
                                        //    string LastMAWBSerialNumber = SQL.GetLastMAWB(connectionDB_BK, dbIndex, itemM.MAWB, itemM.Cdt);
                                        //    //取得此MAWB最後更新的時間, 以及DN+Item
                                        //    //若有資料，則先刪掉MAWB中，這些DN的資料
                                        //    if (LastMAWBSerialNumber != "")
                                        //    {
                                        //        List<MAWBDetail> LastMAWBDetail = SQL.GetMawbDetail(connectionDB_BK, dbIndex, LastMAWBSerialNumber);
                                        //        foreach (MAWBDetail itemL in LastMAWBDetail)
                                        //        {
                                        //            SQL.DeleteMAWBbyDelivery(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB, itemL.DN, itemL.Items, itemcnt_udDN);
                                        //        }
                                        //    }
                                        //}  
                                        //刪除:若@MAWB+DN不存在於MAWB表中，則刪除MAWB中，DN對應的所有MAWB數據
                                        SQL.DeleteMAWBbyDelivery(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB, itemD.DN, itemD.Items, itemcnt_udDN);
                                        //新增:若@MAWB+DN不存在於MAWB表中，新增@MAWB & DN數據
                                        SQL.UpdateMAWB_UnderLineByDN(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB,
                                                       itemD.DN, itemD.Items, itemcnt_udDN, itemM.Cdt);

                                        itemcnt_udDN++;
                                    }                                    
                                    else {
                                        //MAWB Case 2: (DELL)
                                        //刪除時，同時刪掉MAWB表中 MAWB=@MAWB or MAWB=SAP.MAWB之資料
                                        //新增時，取SAP.MAWB下滑線前的值=@MAWB，insert @MAWB 至MAWB表
                                        if (DefItem.VolumnUnit == "DbyAll")
                                        {
                                            BaseLog.LoggingInfo(logger, "MAWB Type:[DbyAll] for DELL");
                                            BaseLog.LoggingInfo(logger, "SAPWeightDef: \r\n{0}", DefItem.DBName + ", Descr:" + DefItem.Descr);
                                            SQL.DeleteMAWB(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB, itemcnt_udAll);
                                            SQL.UpdateMAWB_UnderLine(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB,
                                                           itemD.DN, itemD.Items, itemcnt_udAll, itemM.Cdt);

                                            itemcnt_udAll++;
                                        }
                                        else if (DefItem.VolumnUnit == "Declaration") {
                                        //MAWB Case 4: (HP, ASUS) 
                                        //新增時，取SAP.MAWB=@MAWB，insert @MAWB 至MAWB表
                                        //刪除時，同時刪掉MAWB表中 MAWB=@MAWB or MAWB=SAP.MAWB之資料
                                        //新增報關單號、海運櫃號
                                            BaseLog.LoggingInfo(logger, "MAWB Type:[DbyDN] for HP, ASUS");
                                            BaseLog.LoggingInfo(logger, "SAPWeightDef: \r\n{0}", DefItem.DBName + ", Descr:" + DefItem.Descr);
                                            //新增MAWB至各Plant的MAWB Table
                                            SQL.UpdateMAWB_Declaration(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB,
                                                           itemD.DN, itemD.Items, itemcnt, itemM.Cdt, itemD.DeclarationId, itemD.OceanContainer, itemD.HAWB);
                                            itemcnt++;
                                        
                                        }
                                        //MAWB Case 3: (HP_FRU, WYSE) 
                                        //新增時，取SAP.MAWB=@MAWB，insert @MAWB 至MAWB表
                                        //刪除時，同時刪掉MAWB表中 MAWB=@MAWB or MAWB=SAP.MAWB之資料
                                        else
                                        {
                                            BaseLog.LoggingInfo(logger, "MAWB Type:[DbyDN] for HP, HP_FRU, WYSE");
                                            BaseLog.LoggingInfo(logger, "SAPWeightDef: \r\n{0}", DefItem.DBName + ", Descr:" + DefItem.Descr);
                                            //新增MAWB至各Plant的MAWB Table
                                            SQL.UpdateMAWB(DefItem.ConnectionStr, DefItem.DBName, itemM.MAWB,
                                                           itemD.DN, itemD.Items, itemcnt, itemM.Cdt);

                                            itemcnt++;
                                        }
                                    }
                                }
                            }
                        }
                        //更新MAWB狀態
                        SQL.UpdateTempMAWB(connectionDB_BK, dbIndex, itemM.SerialNumber, itemM.MAWB, State, ErrorDescr);
                    }
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
        public static List<NotifyMAWBResponse> BuildResponseMsg(string connectionDB_BK, string BatchId)
        {
            string connectionDB = "SD_DBServer";
            //string connectionDB_BK = "SD_DBServer_BK";
            //string connectionDB_BK = "SD_DBServer_BK_CQ";
            int dbIndex = 0;
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            List<NotifyMAWBResponse> ResponseList = new List<NotifyMAWBResponse>();
            List<MAWBMaster> MAWBStatus = SQL.GetMawbMaster(connectionDB_BK, dbIndex, BatchId);
           
            try
            {
                foreach (MAWBMaster item in MAWBStatus)
                {
                    BaseLog.LoggingInfo(logger, "SerialNumber: \r\n{0}", item.SerialNumber);

                    NotifyMAWBResponse response = new NotifyMAWBResponse();
                    string SerialNumber = item.SerialNumber;
                    string MAWB = item.MAWB;
                    string Result = item.State;
                    string ErrorText = item.ErrorDescr;
                    response.SerialNumber = item.SerialNumber;
                    response.MAWB = MAWB;
                    response.Result = Result;
                    response.ErrorText = ErrorText;
                    ResponseList.Add(response);

                    SQL.InsertTxnDataLog_DB(connectionDB, dbIndex,
                                            EnumMsgCategory.Response,
                                            "NotifyMAWBResponse",
                                            string.IsNullOrEmpty(item.MAWB) ? "" : item.MAWB,
                                            "",
                                            string.IsNullOrEmpty(item.SerialNumber) ? "" : item.SerialNumber,
                                            "",
                                            "",
                                            EnumMsgState.Success,
                                            "");
                }
                return ResponseList;
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
        private static string GetValueByType(string type, object dataObj)
        {

            FieldInfo fi = dataObj.GetType().GetField(type);
            if (fi == null)
                return "";
            else
                if (fi.GetValue(dataObj) == null)
                { return ""; }
                else
                { return fi.GetValue(dataObj).ToString(); }

        }

    }
}