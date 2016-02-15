// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  PackingPizza bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-10   zhu lei                      create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.Common.CheckItem;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现IPackingPizza接口，Packing Pizza BLL实现类,实现Packing Pizza站检料功能
    /// </summary>
    public class PackingPizza : MarshalByRefObject, IPackingPizza
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region IPackingPizza Members

        /// <summary>
        /// 用model启动工作流
        /// </summary>
        /// <param name="model"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        void IPackingPizza.InputUUT(string model, string line, string editor, string station, string customer)
        {
            logger.Debug(" InputUUT start, model:" + model);
            try
            {

                //var currentProduct = CommonImpl.GetProductByInput(model, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = model;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PackingPizza.xoml", "PackingPizza.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ModelName, model);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(model);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
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
                logger.Debug(" InputUUT end, model:" + model);
            }
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, currentSessionType, checkValue);
        }

        /// <summary>
        /// 扫描完本站应该扫描的所有Parts后调用本方法存储扫入的所有Parts信息，结束工作流。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList IPackingPizza.Save(string model, IList<PrintItem> printItems)
        {
            logger.Debug("Save start, model:" + model);
            ArrayList retLst = new ArrayList();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                var kitId = ((Pizza)currentSession.GetValue(Session.SessionKeys.PizzaID)).PizzaID;
                var printLst =  (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retLst.Add(kitId);
                retLst.Add(printLst);
                
                return retLst;
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
                logger.Debug("Save end, model:" + model);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void IPackingPizza.Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

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

        /// <summary>
        /// 获取所有需要扫描的PizzaParts，在调用inputUUT成功开启工作流之后调用
        /// 用productID获取Session
        /// 调用ICollectionData.GetCheckItemList 
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        IList<IMES.DataModel.BomItemInfo> IPackingPizza.GetNeedCheckPartAndItem(string model)
        {
            logger.Debug("GetNeedCheckPartAndItem start, model:" + model);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model, currentSessionType);
                IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    //get bom
                    retLst = PartCollection.GeBOM(model, currentSessionType);
                    return retLst;
                    ////return CommonImpl.GetCheckItemList(currentSession);
                    ////BOM BOMItem = (BOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    ////IList<IMES.FisObject.Common.CheckItem.ICheckObject> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckObject>)currentSession.GetValue(Session.SessionKeys.ExplicityCheckItemList);
                    
                    ////IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    ////var BOMItem = (BOM)bomRepository.GetHierarchicalBOMByModel(model);

                    //IFlatBOM BOMItem = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    //if (BOMItem != null)
                    //{
                    //    foreach (IFlatBOMItem item in BOMItem.BomItems)
                    //    {
                    //        BomItemInfo ItemInfo = new BomItemInfo();
                    //        ItemInfo.qty = item.Qty;
                    //        if (item.CheckedPart != null)
                    //        {
                    //            ItemInfo.scannedQty = item.CheckedPart.Count;
                    //            ItemInfo.collectionData = new List<pUnit>();
                    //            foreach (PartUnit preItem in item.CheckedPart)
                    //            {
                    //                pUnit temp = new pUnit();
                    //                temp.sn = preItem.Sn;
                    //                temp.pn = preItem.Pn;
                    //                temp.valueType = item.CheckItemType;
                    //                ItemInfo.collectionData.Add(temp);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            ItemInfo.scannedQty = 0;
                    //            ItemInfo.collectionData = new List<pUnit>();
                    //        }

                    //        List<PartNoInfo> allPart = new List<PartNoInfo>();


                    //        foreach (IPart part in item.AlterParts)
                    //        {
                    //            PartNoInfo aPart = new PartNoInfo();
                    //            aPart.description = part.Descr;
                    //            aPart.id = part.PN;
                    //            aPart.friendlyName = aPart.id;
                    //            aPart.partTypeId = part.Type;
                    //            aPart.iecPartNo = part.PN;
                    //            aPart.valueType = item.CheckItemType;
                    //            allPart.Add(aPart);
                    //        }
                    //        allPart.Sort(delegate(PartNoInfo p1, PartNoInfo p2) { return p1.iecPartNo.CompareTo(p2.iecPartNo); });

                    //        ItemInfo.parts = allPart;
                    //        retLst.Add(ItemInfo);
                    //    }

                    //}

                    ////if (checkItems != null)
                    ////{
                    ////    foreach (ICheckObject item in checkItems)
                    ////    {
                    ////        BomItemInfo checkItemInfo = new BomItemInfo();
                    ////        checkItemInfo.qty = 1;
                    ////        checkItemInfo.scannedQty = 0;

                    ////        IList<PartNoInfo> allPart = new List<PartNoInfo>();
                    ////        PartNoInfo aPart = new PartNoInfo();
                    ////        aPart.description = string.Empty;
                    ////        aPart.id = item.ItemDisplayName;
                    ////        aPart.friendlyName = aPart.id;
                    ////        aPart.partTypeId = string.Empty;
                    ////        aPart.iecPartNo = aPart.id;
                    ////        allPart.Add(aPart);
                    ////        checkItemInfo.parts = allPart;

                    ////        retLst.Add(checkItemInfo);
                    ////    }

                    ////}
                    //return retLst;

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
                logger.Debug("GetNeedCheckPartAndItem end, model:" + model);
            }
        }

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="pizzaID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> ReprintLabel(string pizzaID, string line, string editor, string station, string customer, string reason, IList<PrintItem> printItems)
        {
            logger.Debug("(UnitLabelPrintImpl)ReprintLabel start, [pizzaID]:" + pizzaID
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = pizzaID;

            try
            {

                Session session = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, currentSessionType, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PackingPizzaLabelReprint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.PizzaNoList, pizzaID);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, pizzaID);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, pizzaID);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(UnitLabelPrintImpl)ReprintLabel end, [pizzaID]:" + pizzaID
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }

        ///<summary>
        /// 解除绑定
        /// 使用工作流UnpackPizza.xoml
        /// </summary>
        public void UnpackPizza(string kitID, string line, string editor, string station, string customer, string reason)
        {
            logger.Debug("(UnpackPizza)Unpack start, kitID:" + kitID + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "reason:" + reason);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = kitID;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PackingPizzaUnpack.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PizzaNoList, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
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
                logger.Debug("(UnpackPizza)Unpack end,  kitID:" + kitID + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "reason:" + reason);
            }
        }
        #endregion
    }
}
