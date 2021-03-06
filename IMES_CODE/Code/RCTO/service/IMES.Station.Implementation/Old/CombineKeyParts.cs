﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0079
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    public class _CombineKeyParts : MarshalByRefObject, ICombineKeyParts
    {


        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //    private string OriSessionkey;

        #region ICombineKeyParts Members
        public IList<BomItemInfo> InputProdIdorCustsn(string pdLine, string input, string editor, string stationId, string customerId, out string realProdID, out string model)
        {
            logger.Debug("(_CombineKeyParts)InputProdId start, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);
            FisException ex;
            List<string> erpara = new List<string>();
            if (currentProduct == null)
            {

                erpara.Add(input);
                throw new FisException("SFC002", erpara);
            }

            string sessionKey = currentProduct.ProId;
            realProdID = sessionKey;
            model = currentProduct.Model;

            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {


                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                     // WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("_TEST.xoml", "_TEST.rules", wfArguments);
                //    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("021BoardInput.xoml", "021BoardInput.rules", wfArguments); 
                    string wfName, rlName;
                  RouteManagementUtils.GetWorkflow(stationId, "CombineKeyParts.xoml", "CombineKeyParts.rules", out wfName, out rlName);
               //   RouteManagementUtils.GetWorkflow(stationId, "021BoardInput_TEST.xoml", "021BoardInput_TEST.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(input);
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                              
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }
                       
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                //model = prod.Model;
                //realProdID= prod.ProId;
                //add mb info in GetCheckItemList
                IList<BomItemInfo> ret = GetCheckItemList(Session, prod.PCBID, prod.PCBModel);
               return ret;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_CombineKeyParts)InputProdId end, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        /// 输入PPID
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ppid">PPID</param>
        /// <returns>主料的Part No</returns>
        public IList<MatchedPartOrCheckItem> InputPPID(
            string prodId,
            string ppid)
        {
            logger.Debug("(CombineKeyParts)InputPPID start,"
               + " [prodId]: " + prodId
               + " [ppid]:" + ppid);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                
                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.PartID, ppid);
                    sessionInfo.AddValue(Session.SessionKeys.ValueToCheck, ppid);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                IList<MatchedPartOrCheckItem> RetList = new List<MatchedPartOrCheckItem>(); 

                //get matchedinfo
                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)sessionInfo.GetValue(Session.SessionKeys.MatchedParts);
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
                    foreach (IBOMPart bompartitem in bomPartlist)
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = bompartitem.PN;
                        tempMatchedPart.CollectionData = bompartitem.MatchedSn;
                        tempMatchedPart.ValueType = bompartitem.ValueType;
                        RetList.Add(tempMatchedPart); 
                    }
                    return RetList;
                }
                else
                {
                    ICheckItem citem = (ICheckItem)sessionInfo.GetValue(Session.SessionKeys.MatchedCheckItem);
                    if (citem == null)
                    {
                        erpara.Add(ppid);
                        throw new FisException("MAT010", erpara);
                    }
                    else
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                        tempMatchedPart.CollectionData = citem.ValueToCollect;
                        tempMatchedPart.ValueType = "";
                        RetList.Add(tempMatchedPart);                                      
                        return RetList;
                    }
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CombineKeyParts)InputPPID  End,"
                               + " [prodId]: " + prodId
                               + " [ppid]:" + ppid);
            }
        }

        public void Save(string prodId)
        {
            logger.Debug("(CombineKeyParts)Save start,"
               + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, true);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(InitialPartsCollection)Save  End,"
                               + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }
        public int CountQty(string line )
        {
           


            DateTime dtNow = DateTime.Now;
            DateTime dt2030 = Convert.ToDateTime("20:30");
        //    string  cmdTxt = @"select count(distinct p.ProductID) from ProductLog p,Product Pr
          //               where p.ProductID=Pr.ProductID and  p.Cdt>@dtBegin and p.Cdt<@dtEnd and p.Station='40'  and Pr.PCBID <>'' and Pr.PCBID is not null";
            string cmdTxt = @"select count(*)  from PCBStatus nolock where Station='32' and Line=@Line and Udt>=@dtBegin and Udt<@dtEnd";
          
            DateTime dtBegin;
            DateTime dtEnd = DateTime.Now;
           // AM8:00算日班 , PM8:30算夜班
            if (IsDayWork())// 白班
            {
                dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
               
            }
            else
            {
                if (DateTime.Compare(dtNow, dt2030) > 0) // it means 00:00~20:30
                {
                    dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,20, 30, 0);
                }
                else
                {
                    dtBegin = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 20, 30, 0);
                }
             
            }
            SqlParameter paraName = new SqlParameter("@dtBegin", SqlDbType.DateTime);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = dtBegin;
            SqlParameter paraName2 = new SqlParameter("@dtEnd", SqlDbType.DateTime);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = dtEnd;
            SqlParameter paraLine = new SqlParameter("@Line", SqlDbType.VarChar);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            //string result = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, cmdTxt, paraName, paraName2, paraLine).ToString();
            string result = "1";
           return int.Parse(result);
        
        
        
        }
        private bool IsDayWork()
        {
            DateTime dt1 = Convert.ToDateTime("08:00");
            DateTime dt2 = Convert.ToDateTime("20:30");
            DateTime dt3 = DateTime.Now;
          
            if (DateTime.Compare(dt3, dt1) > 0 && DateTime.Compare(dt3, dt2) < 0)
            { return true; }
            return false;
        }
        private  IList<BomItemInfo> GetCheckItemList(Session Session, string MBSN, string MBModel)
        {
            try
            {
            
                BOM BOMItem = (BOM)Session.GetValue(Session.SessionKeys.SessionBom);
                IList<IMES.FisObject.Common.CheckItem.ICheckItem> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckItem>)Session.GetValue(Session.SessionKeys.ExplicityCheckItemList);
                IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();
                if (BOMItem != null)
                {
                    foreach (BOMItem item in BOMItem.Items)
                    {
                        BomItemInfo ItemInfo = new BomItemInfo();
                        ItemInfo.qty = item.Qty;
                        if (item.StationPreCheckedPart != null)
                        {
                            ItemInfo.scannedQty = item.StationPreCheckedPart.Count();
                            ItemInfo.collectionData = new List<pUnit>();
                            foreach (PartUnit preItem in item.StationPreCheckedPart)
                            {
                                pUnit temp = new pUnit();
                                temp.sn = preItem.Sn;
                                temp.pn = preItem.Pn;
                                temp.valueType = item.ValueType;
                                ItemInfo.collectionData.Add(temp);
                            }
                        }

                        else
                        {
                            ItemInfo.scannedQty = 0;
                            ItemInfo.collectionData = new List<pUnit>();
                        }

                        List<PartNoInfo> allPart = new List<PartNoInfo>();


                        foreach (BOMPart part in item.AlterParts)
                        {
                            PartNoInfo aPart = new PartNoInfo();

                            aPart.description = part.Descr2;
                            aPart.id = part.PN;
                            aPart.friendlyName = aPart.id;
                            aPart.partTypeId = part.Type;
                            aPart.iecPartNo = part.PN;
                            aPart.valueType = item.ValueType;
                            allPart.Add(aPart);
                            if (!string.IsNullOrEmpty(MBModel) &&
                                !string.IsNullOrEmpty(MBSN) &&
                                aPart.id == MBModel)
                            {
                                ItemInfo.scannedQty = 1; //item.StationPreCheckedPart.Count();
                                ItemInfo.collectionData = new List<pUnit>();

                                pUnit temp = new pUnit();
                                temp.sn = MBSN;
                                temp.pn = MBModel;
                                temp.valueType = "SN"; //item.ValueType;
                                ItemInfo.collectionData.Add(temp);

                                
                            }


                        }
                        allPart.Sort(delegate(PartNoInfo p1, PartNoInfo p2) { return p1.iecPartNo.CompareTo(p2.iecPartNo); });

                        ItemInfo.parts = allPart;
                        retLst.Add(ItemInfo);
                    }

                }

                if (checkItems != null)
                {
                    foreach (ICheckItem item in checkItems)
                    {
                        BomItemInfo checkItemInfo = new BomItemInfo();
                        checkItemInfo.qty = 1;
                        checkItemInfo.scannedQty = 0;

                        IList<PartNoInfo> allPart = new List<PartNoInfo>();
                        PartNoInfo aPart = new PartNoInfo();
                        aPart.description = string.Empty;
                        aPart.id = item.ItemDisplayName;
                        aPart.friendlyName = aPart.id;
                        aPart.partTypeId = string.Empty;
                        aPart.iecPartNo = aPart.id;
                        allPart.Add(aPart);
                        checkItemInfo.parts = allPart;

                        retLst.Add(checkItemInfo);
                    }

                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
