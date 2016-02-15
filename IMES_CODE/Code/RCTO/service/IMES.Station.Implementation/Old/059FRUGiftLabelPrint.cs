﻿
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: FRUGiftLabelPrint Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-03-09   Luyc Liu          Create 
 * 2010-05-21   Lucy Liu          Modify:   ITC-1155-0138 
 * 2010-05-21   Lucy Liu          Modify:   ITC-1155-0142 
 * 2010-05-21   Lucy Liu          Modify:   ITC-1155-0143
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{

    public class FRUGiftLabelPrint : MarshalByRefObject, IFRUGiftLabelPrint
    {
        private const Session.SessionType TheType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region IFRUGiftLabelPrint Members


        /// <summary>
        /// 验证MB并返回其111
        /// 1、MB不合法返回异常
        /// 2、MB的WC不是8G返回异常
        /// 3、取得MB的111并返回
        /// </summary>
        /// <param name="mb">MB</param>
        /// <returns>111阶</returns>
        public string Get8GMB111(string pcbNo)
        {
            logger.Debug("(FRUGiftLabelPrint)Get8GMB111 Start, "
                          + " [pcbNo]:" + pcbNo);

            try
            {
                IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //        <bug>
                //            BUG NO:ITC-1155-0138
                //            REASON:用前10位查询8G
                //        </bug>
                string temp = pcbNo.Substring(0, 10);
                string wc = mbRep.GetTheNewestStationFromPCBLog(temp);
                string ret = "";
                if (!String.IsNullOrEmpty(wc))
                {
                    if (wc != "8G")
                    {
                        throw new FisException("PAK005", new string[] { pcbNo });
                    }
                    else
                    {
                        IMB imb = mbRep.Find(temp);
                        if (imb == null)
                        {
                            throw new FisException("SFC001", new string[] { pcbNo });                            
                        }

                        ret = imb.PCBModelID;
                    }
                }
                else
                {
                    throw new FisException("PAK005", new string[] { pcbNo });
                }
                
            
                return ret;
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


                logger.Debug("(FRUGiftLabelPrint)Get8GMB111 End, "
                         + " [pcbNo]:" + pcbNo);
            }
        }



        /// <summary>
        /// 根据Model取得PartNo和Descr列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>PartNo和Descr列表</returns>
        public IList<PartNoDescrInfo> GetPartNoDescrListByModel(string model)
        {

            logger.Debug("(FRUGiftLabelPrint)GetPartNoDescrListByModel Start, "
                          + " [model]:" + model);
            try
            {
                IList<PartNoDescrInfo> ret = new List<PartNoDescrInfo>();
                
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
                DataTable dt = bomRep.GetPartsViaModelBOM(model);
                string descr;
                string partNo;
                string generatedPartNo;
                PartNoDescrInfo partNoDescrInfo;
                IList<string> assemblyCodeLst;
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i = 0;
                    generatedPartNo = "";
                    descr = dr[0].ToString();
                    partNo = dr[1].ToString();
                    if (partNo.StartsWith("FRU151")) {
                        assemblyCodeLst = partRep.GetAssemblyCodesByModel(model);
                        foreach (string assemblyCode in assemblyCodeLst)
                        {
                            i++;
                            generatedPartNo += assemblyCode;
                            if (i < assemblyCodeLst.Count) {
                                generatedPartNo += ",";
                            }                           
                        }

                    } else if (partNo.StartsWith("FRU111")) {
                        generatedPartNo = partNo.Substring(partNo.Length - 12, 12);
                    } else {
                        assemblyCodeLst = partRep.GetAssemblyCodesByPartNo(partNo);
                        foreach (string assemblyCode in assemblyCodeLst)
                        {
                            i++;
                            generatedPartNo += assemblyCode;
                            if (i < assemblyCodeLst.Count) {
                                generatedPartNo += ",";
                            }                           
                        }
                    }
                    partNoDescrInfo = new PartNoDescrInfo();
                    partNoDescrInfo.descr = descr;
                    partNoDescrInfo.partNo = partNo;
                    partNoDescrInfo.generatedPartNo = generatedPartNo;
                    ret.Add(partNoDescrInfo);

                }
                return ret;
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


                logger.Debug("(FRUGiftLabelPrint)GetPartNoDescrListByModel End, "
                              + " [model]:" + model);
            }
        }



        /// <summary>
        /// 打印Gift标签
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="pnoList">Part NO 列表</param>
        /// <param name="scanList">scan列表</param>
        /// <param name="qty">数量</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="giftId">gift no</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> Print(string model, IList<string> pnoList, IList<IList<string>> scanList, int qty,
             string editor, string stationId, string customerId, out string giftID, IList<PrintItem> printItems)
        
        {


            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = System.Guid.NewGuid().ToString("N");
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    //没有线别
                    Session = new Session(sessionKey, TheType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "059FRUGiftLabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.AddValue(Session.SessionKeys.ModelName, model);
                    Session.AddValue(Session.SessionKeys.GiftPartNoList, pnoList);
                    Session.AddValue(Session.SessionKeys.GiftScanPartList, scanList);
                    Session.AddValue(Session.SessionKeys.GiftScanPartCount, qty);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                IList<PrintItem> returnList = this.getPrintList(Session);
                giftID = (string)Session.GetValue(Session.SessionKeys.GiftID);
                return returnList;
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
                logger.Debug("(_FRUGiftLabelPrint)Print end, model:" + model + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        /// <summary>
        /// 重印Gift标签
        /// </summary>
        /// <param name="giftNo">gift NO</param>
        /// <param name="editor">操作人员</param>
        /// <param name="stationId">站ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="printItems">Print Items</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> Reprint(string giftNo,
             string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(_FRUGiftLabelPrint)Reprint start, giftNo:" + giftNo + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {

                string sessionKey = giftNo;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("059FRUGiftLabelReprint.xoml", null, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, giftNo);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, giftNo);
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

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
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
                logger.Debug("(_FRUGiftLabelPrint)Reprint end, giftNo:" + giftNo + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }
        #endregion
    }
}