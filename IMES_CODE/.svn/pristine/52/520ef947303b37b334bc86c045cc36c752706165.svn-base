/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Pallet Without Carton For FRU
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.PCA.MB;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using carton = IMES.FisObject.PAK.CartonSSCC;
using log4net;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PAK.StandardWeight;
using System.Text.RegularExpressions;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public class CombinePalletWithoutCartonForFRU : MarshalByRefObject, ICombinePalletWithoutCartonForFRU
    {
        private const Session.SessionType TheType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;
        private const string DnModelPC = "PC";

        private bool DnModelWasNotAllowed(string model)
        {
            if (string.IsNullOrEmpty(model) || model.Length <= 2)
            {
                return false;
            }
            if (DnModelPC.Equals(model.Substring(0, 2)))
            {
                return true;
            }
            return false;
        }

        private bool CheckModel146CommonParts(string model)
        {
            if (string.IsNullOrEmpty(model))
                return false;

            if (model.IndexOf("146") == 0)
            {
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                bool is146 = false;

                IList<IMES.FisObject.Common.Model.ModelInfo> info1 = modelRepository.GetModelInfoByModelAndName(model, "TP");
                if (info1 != null && info1.Count > 0)
                {
                    foreach (IMES.FisObject.Common.Model.ModelInfo v in info1)
                    {
                        if ("Commparts".Equals(v.Value))
                        {
                            is146 = true;
                            break;
                        }
                    }
                }
                if (!is146)
                {
                    // 此機型請online打印!
                    throw new FisException("CQCHK0039", new string[] { });
                }
            }
            return true;
        }

        private bool NeedCheckWeight(string model)
        {
            bool exist = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("FRUCheckModelWeightRE");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo c in lstConstValueType)
                {
                    Regex regex = new Regex(c.value);
                    if (regex.IsMatch(model))
                    {
                        exist = true;
                        break;
                    }
                }
            return exist;
        }

        private bool CheckModelWeight(string model)
        {
            if (string.IsNullOrEmpty(model))
                return false;

            bool ret = true;

            if (NeedCheckWeight(model))
            {
                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    // //该Model尚无标准重量，请先去称重。
                    throw new FisException("PAK123", new string[] { model });
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList GetDnList(string dnsn, string shipDate, string model, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePalletWithoutCartonForFRU)GetDnList start, dn:" + dnsn + " shipDate:" + shipDate + " model:" + model + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            List<string> errpara = new List<string>();
            ArrayList retList = new ArrayList();
			try
            {
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                IList<Delivery> lstDn = new List<Delivery>();

                if (!string.IsNullOrEmpty(dnsn))
                {
                    Delivery dn = deliveryRep.GetDelivery(dnsn);
                    if (null == dn || DnModelWasNotAllowed(dn.ModelName))
                    {
                        throw new FisException("CQCHK0028", new string[] { dnsn });
                    }
                    /*
                    IList<string> dnList = new List<string>();
                    deliveryRep.GetDNListByDNList();
                    */
                    lstDn.Add(dn);
                }
                else if (!string.IsNullOrEmpty(shipDate))
                {
                    DateTime begin = DateTime.Parse(shipDate);
                    DateTime end = begin.AddDays(1);
                    IList<Delivery> lstDn2 = deliveryRep.GetDeliveriesByShipDateRange(begin, end);
                    if (null != lstDn2)
                    {
                        foreach (Delivery dn in lstDn2)
                        {
                            if (!DnModelWasNotAllowed(dn.ModelName))
                            {
                                lstDn.Add(dn);
                            }
                        }
                    }
                    if (null == lstDn || lstDn.Count == 0)
                    {
                        throw new FisException("CQCHK0028", new string[] { "" });
                    }
                }
                else if (!string.IsNullOrEmpty(model))
                {
                    IList<Delivery> lstTmp = new List<Delivery>();
                    lstTmp = lstTmp.Concat(deliveryRep.GetDeliveryListByModel(model, "00")).ToList();
                    lstTmp = lstTmp.Union(deliveryRep.GetDeliveryListByModel(model, "88")).ToList();

                    foreach (Delivery dn in lstTmp)
                    {
                        if (!DnModelWasNotAllowed(dn.ModelName))
                        {
                            lstDn.Add(dn);
                        }
                    }
                    if (null == lstDn || lstDn.Count == 0)
                    {
                        throw new FisException("CQCHK0028", new string[] { "" });
                    }

                    CheckModel146CommonParts(model);
                }

                IList<string> dns = new List<string>();
                IList<string> lstModel = new List<string>();
                IList<string> lstShipDate = new List<string>();
                IList<string> lstDnQty = new List<string>();
                foreach (Delivery dn in lstDn)
                {
                    // DeliveryNo, Model, ShipDate, Qty
                    //retList.Add(dn.DeliveryNo + "," + dn.ModelName + "," + string.Format(dn.ShipDate.ToString(), "YYYY-MM-DD") + "," + dn.Qty.ToString());
                    dns.Add(dn.DeliveryNo);
                    lstModel.Add(dn.ModelName);
                    lstShipDate.Add(dn.ShipDate.ToString("yyyy-MM-dd"));
                    lstDnQty.Add(dn.Qty.ToString());
                }

                // dnsn list
                retList.Add(dns);
                retList.Add(lstModel);
                retList.Add(lstShipDate);
                retList.Add(lstDnQty);

			    return retList;
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
                logger.Debug("(CombinePalletWithoutCartonForFRU)GetDnList end, dn:" + dnsn + " shipDate:" + shipDate + " model:" + model + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }
		
		/// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList GetPalletList(string dnsn, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePalletWithoutCartonForFRU)GetPalletList start, dn:" + dnsn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            List<string> errpara = new List<string>();
            ArrayList retList = new ArrayList();
            
            IList<string> deliveryNos = new List<string>();

            try
            {
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                Delivery dn = null;
                if (!string.IsNullOrEmpty(dnsn))
                {
                    dn = deliveryRep.GetDelivery(dnsn);
                }
                if (null == dn || DnModelWasNotAllowed(dn.ModelName))
                {
                    throw new FisException("CQCHK0028", new string[] { dnsn });
                }

                CheckModel146CommonParts(dn.ModelName);

                CheckModelWeight(dn.ModelName);

                IList<string> lstPalletSn = new List<string>();
                //IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                //IList<PalletInfo> lstPallets = palletRepository.GetPalletList(dnsn);
                IList<DeliveryPalletInfo> lstPallets = deliveryRep.GetDeliveryPalletListByDN(dnsn);
                foreach (DeliveryPalletInfo p in lstPallets)
                {
                    lstPalletSn.Add(p.palletNo);
                }
                if (lstPalletSn.Count == 0)
                {
                    // Pallet %1 不存在！
                    throw new FisException("CHK106", new string[] { "" });
                }

                // ModelName
                retList.Add(dn.ModelName);

                // ShipDate
                retList.Add(dn.ShipDate.ToString("yyyy-MM-dd"));

                // DnQty
                retList.Add(dn.Qty.ToString());

                // lstPallets
                retList.Add(lstPalletSn);

                // DnStatus
                retList.Add(dn.Status);

                // DnEditor
                retList.Add(dn.Editor);

                return retList;
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
                logger.Debug("(CombinePalletWithoutCartonForFRU)GetPalletList end, dn:" + dnsn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList GetCntCartonOfPallet(string palletNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePalletWithoutCartonForFRU)GetCntCartonOfPallet start, palletNo:" + palletNo + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            ArrayList retList = new ArrayList();

            try
            {
                if (string.IsNullOrEmpty(palletNo))
                {
                    retList.Add("");
                    return retList;
                }
                

                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<DeliveryPalletInfo> lstPallets = deliveryRep.GetDeliveryPalletListByPlt(palletNo);
                int totalCartons = 0;
                foreach (DeliveryPalletInfo dp in lstPallets)
                {
                    totalCartons += dp.deliveryQty;
                }

                retList.Add(totalCartons.ToString());

                return retList;
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
                logger.Debug("(CombinePalletWithoutCartonForFRU)GetCntCartonOfPallet end, palletNo:" + palletNo + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList Save(string dnsn, string palletNo, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePalletWithoutCartonForFRU)Save start, dnsn:" + dnsn + " palletNo:" + palletNo + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);

            try
            {
                ArrayList retList = new ArrayList();
                FisException ex;
                List<string> erpara = new List<string>();

                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                if (string.IsNullOrEmpty(dnsn) || string.IsNullOrEmpty(palletNo))
                {
                    throw new FisException("Need DN, PalletNo");
                }

                string sessionKey = dnsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombinePalletWithoutCartonForFRU.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dnsn);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, dnsn);
                    currentSession.AddValue(Session.SessionKeys.Reason, "");

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

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(returnList);

                return retList;
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
                logger.Debug("(CombinePalletWithoutCartonForFRU)Save end, dnsn:" + dnsn + " palletNo:" + palletNo + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList Reprint(string dnsn, string palletNo, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinePalletWithoutCartonForFRU)Reprint Start,"
                            + " [dnsn]:" + dnsn
                            + " [palletNo]:" + palletNo
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);

            try
            {
                ArrayList retList = new ArrayList();
                FisException ex;
                List<string> erpara = new List<string>();

                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<DeliveryPalletInfo> lstPallets = deliveryRep.GetDeliveryPalletListByPlt(palletNo);
                bool isFoundDpi = false;
                foreach (DeliveryPalletInfo dpi in lstPallets)
                {
                    if (dnsn.Equals(dpi.deliveryNo))
                    {
                        if ("1".Equals(dpi.status))
                        {
                            isFoundDpi = true;
                            break;
                        }
                        else
                        {
                            // 請使用FRU Combine Pallet Without Carton打印
                            throw new FisException("CQCHK0043", new string[] {  });
                        }
                    }
                }
                if (!isFoundDpi)
                {
                    // DN:%1, PalletNo:%2 在Delivery_Pallet中不存在
                    throw new FisException("CQCHK0044", new string[] { dnsn, palletNo });
                }

                string sessionKey = palletNo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, dnsn);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);

                    //Session.AddValue(Session.SessionKeys.IsComplete, false);
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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }


                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                return arr;

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
                logger.Debug("(CombinePalletWithoutCartonForFRU)Reprint End,"
                                + " [dnsn]:" + dnsn
                                + " [palletNo]:" + palletNo
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sn"></param>
        public void cancel(string sn)
        {
            logger.Debug("(CombinePoInCarton)Cancel start, [sn]:" + sn);

            string sessionKey = sn;
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                throw e;
            }
            finally
            {
                logger.Debug("(CombinePoInCarton)Cancel end, [sn]:" + sn);
            }
        }
        
    }
}
