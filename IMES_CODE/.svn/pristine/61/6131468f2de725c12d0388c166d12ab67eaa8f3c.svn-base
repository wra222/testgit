/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for WHPalletControl Page            
 * CI-MES12-SPEC-PAK W/H Pallet Control.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Linq;
using System.Text;
using IMES.Route;
using System.Workflow.Runtime;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for WHPalletControl.
    /// </summary>
    public class _WHPalletControl : MarshalByRefObject, IWHPalletControl
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        #region IWHPalletControl Members
        /// <summary>
        /// 获取plt信息
        /// </summary>
        /// <param name="input">input</param>
        public S_CheckInput_Re CheckInput(string input)
        {
            S_CheckInput_Re ret = new S_CheckInput_Re();
            string ucc = "";
            string plt = "";
            plt = input;
            if (input.Trim().Length == 20)
            {
                ucc = input.Trim();
                plt = palletRepository.GetPalletNoByUcc(ucc);
                if (plt == null)
                {
                    ret.plt = "";
                    ret.found = false;
                    return ret;
                }
                /*if (!(plt.Length == 10 && (plt.Substring(0, 2) == "00"
                    || plt.Substring(0, 2) == "01"
                    || plt.Substring(0, 2) == "90")))
                {
                    ret.plt = "";
                    ret.found = false;
                    return ret;
                }*/
            }
            IList<Delivery>deliveryList = currentRepository.GetDeliveryListByInfoValue(plt);
            int have = 0;
            foreach (Delivery tmp in deliveryList)
            {
                if (null != tmp)
                {
                    IList<DeliveryPallet> dnPalletList = tmp.DnPalletList;
                    foreach (DeliveryPallet temp in dnPalletList)
                    {
                        if (null != temp && temp.PalletID.Substring(0, 2) == "BA")
                        {
                            have = 1;
                            break;
                        }
                    }
                }
            }
            if (have == 1)
            {
                ret.plt = plt;
                ret.found = true;
                return ret;
            }
            /*int first = 0;
            IList<DeliveryPallet> reDeliveryPallet = palletRepository.GetDeliveryPallet(plt);
            if (null != reDeliveryPallet && reDeliveryPallet.Count > 0)
            {
                foreach (DeliveryPallet tmp in reDeliveryPallet)
                {
                    if (null != tmp && tmp.PalletID.Substring(0, 2) == "BA")
                    {
                        first = 1;
                    }
                    break;
                }
            }
            if (first == 1)
            { }
            else
            {
                ret.plt = "";
                ret.found = false;
                return ret;
            }*/
            IList<DummyShipDetInfo> reDummyShipDet = palletRepository.GetDummyShipDetListByPlt(plt);
            if (null != reDummyShipDet && reDummyShipDet.Count > 0)
            {
                ret.plt = plt;
                ret.found = true;
                return ret;
            }
            
            
            Pallet rePallet = palletRepository.Find(plt);
            if (null != rePallet && null != rePallet.PalletNo)
            {
                ret.plt = plt;
                ret.found = true;
                return ret;
            }
            
            ret.plt = "";
            ret.found = false;
            return ret;
        }
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        public bool CheckIn(string plt)
        {
            WhPltMasInfo resultWhPltMas = palletRepository.GetWHPltMas(plt);
            if (null == resultWhPltMas)
            {
                return false;
            }
            if (resultWhPltMas.wc == "IN")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        public bool CheckRW(string plt)
        {
            WhPltMasInfo resultWhPltMas = palletRepository.GetWHPltMas(plt);
            if (null == resultWhPltMas)
            {
                return false;
            }
            if (resultWhPltMas.wc == "RW")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        public bool CheckExist(string plt)
        {
            WhPltMasInfo resultWhPltMas = palletRepository.GetWHPltMas(plt);
            if (null == resultWhPltMas)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        /// <summary>
        /// 获取plt in信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        public bool CheckDT(string plt)
        {
            WhPltLogInfo resultWhPltLog = palletRepository.GetWhPltLogInfoNewestly(plt);
            if (null == resultWhPltLog)
            {
                return false;
            }
            else if (!(resultWhPltLog.wc == "DT" || resultWhPltLog.wc == "RW"))
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        
        /// <summary>
        /// 获取plt out信息 plt=pltno
        /// </summary>
        /// <param name="plt">plt</param>
        public bool CheckOut(string plt)
        {
            WhPltMasInfo resultWhPltMas = palletRepository.GetWHPltMas(plt);
            if (null == resultWhPltMas)
            {
                return false;
            }
            if (resultWhPltMas.wc == "OT")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取Qty信息
        /// </summary>
        /// <param name="plt">plt</param>
        public int GetQty(string plt)
        {
            int ret = 0;
            IList<DeliveryPallet> reDeliveryPallet = palletRepository.GetDeliveryPallet(plt);
            if (null != reDeliveryPallet && reDeliveryPallet.Count > 0)
            {
                foreach (DeliveryPallet tmp in reDeliveryPallet)
                {
                    ret = ret + tmp.DeliveryQty;
                }
                if (ret > 0)
                {
                    return ret;
                }
            }
            ret = 0;
            if (ret == 0)
            {
                ret = palletRepository.GetCountOfDummyShipDetByPlt(plt);
            }
            return ret;
        }
        /// <summary>
        /// 获取DN信息
        /// </summary>
        /// <param name="plt">plt</param>
        public S_DN_Extended GetDN(string plt)
        {
            S_DN_Extended ret = new S_DN_Extended();
            ret.success = 0;
            IList<DeliveryPallet> reDeliveryPallet = palletRepository.GetDeliveryPallet(plt);
            if (null == reDeliveryPallet || reDeliveryPallet.Count == 0)
            {
                IList<Delivery> deliveryList = currentRepository.GetDeliveryListByInfoValue(plt);
                foreach (Delivery tmp in deliveryList)
                {
                    if (null != tmp)
                    {
                        Delivery reDelivery = tmp;
                        ret.consolidated = (string)reDelivery.GetExtendedProperty("Consolidated");
                        if (ret.consolidated == null || ret.consolidated == "")
                        {
                            ret.consolidated = (string)reDelivery.GetExtendedProperty("RedShipment");
                        }
                        ret.bol = (string)reDelivery.GetExtendedProperty("BOL");
                        ret.emeaCarrier = (string)reDelivery.GetExtendedProperty("EmeaCarrier");
                        ret.carrier = (string)reDelivery.GetExtendedProperty("Carrier");
                        ret.modelName = reDelivery.ModelName;
                        ret.regId = (string)reDelivery.GetExtendedProperty("RegId");
                        ret.success = 1;
                        return ret;
                    }
                }
            }
            else
            {
                foreach (DeliveryPallet tmp in reDeliveryPallet)
                {
                    if (null != tmp && tmp.DeliveryID != null)
                    {
                        Delivery reDelivery = currentRepository.Find(tmp.DeliveryID);
                        if (null != reDelivery)
                        {
                            ret.consolidated = (string)reDelivery.GetExtendedProperty("Consolidated");
                            if (ret.consolidated == null || ret.consolidated == "")
                            {
                                ret.consolidated = (string)reDelivery.GetExtendedProperty("RedShipment");
                            }
                            ret.bol = (string)reDelivery.GetExtendedProperty("BOL");
                            ret.emeaCarrier = (string)reDelivery.GetExtendedProperty("EmeaCarrier");
                            ret.carrier = (string)reDelivery.GetExtendedProperty("Carrier");
                            ret.modelName = reDelivery.ModelName;
                            ret.regId = (string)reDelivery.GetExtendedProperty("RegId");
                            ret.success = 1;
                        }
                        return ret;
                    }
                }
            }
            
            return ret;
        }
        /// <summary>
        /// not AssignDelivery return > 0
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        public int NonAssignDelivery(string plt, string editor)
        {
            try
            {
                int ret = 0;
                int qty = GetQty(plt);
                if (qty < 5)
                {
                    WhPltLogInfo newLog = new WhPltLogInfo();
                    newLog.plt = plt;
                    newLog.editor = editor;
                    newLog.wc = "IN";
                    newLog.cdt = DateTime.Now;
                    palletRepository.InsertWhPltLog(newLog);
                    WhPltMasInfo haveRecord = palletRepository.GetWHPltMas(plt);
                    if (null != haveRecord)
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.UpdateWhPltMas(newMasInfo, plt);
                    }
                    else
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.cdt = DateTime.Now;
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.InsertWhPltMas(newMasInfo);
                    }
                    ret = 1;
                    return ret;
                }
                IList<DummyShipDetInfo> retDummy = palletRepository.GetDummyShipDetListByPlt(plt);
                if (retDummy != null &&
                    retDummy.Count > 0)
                {
                    ret = 2;
                    return ret;
                }
                S_DN_Extended value = GetDN(plt);
                if (value.success == 0)
                {
                    //得不到数据
                    ret = -1;
                    return ret;
                }
                string model = "";
                model = value.modelName;
                if (model == "")
                {
                    ret = -1;
                    return ret;
                }
                if (model != null
                    && model.Length > 2
                    && !model.Substring(0, 2).Equals("PC"))
                {
                    WhPltLogInfo newLog = new WhPltLogInfo();
                    newLog.plt = plt;
                    newLog.editor = editor;
                    newLog.wc = "IN";
                    newLog.cdt = DateTime.Now;
                    palletRepository.InsertWhPltLog(newLog);
                    WhPltMasInfo haveRecord = palletRepository.GetWHPltMas(plt);
                    if (null != haveRecord)
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.UpdateWhPltMas(newMasInfo, plt);
                    }
                    else
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.cdt = DateTime.Now;
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.InsertWhPltMas(newMasInfo);
                    }
                    ret = 3;
                    return ret;
                }

                return ret;
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
        }
        void DoOrderPltType(string infoValue, string infoType, string para1, string para2)
        {
            IList<Delivery> deliveryList = currentRepository.GetDeliveryByValueAndType(infoValue, infoType);
            if (infoType == "Consolidated")
            {
                deliveryList = currentRepository.GetDeliveryListByInfoValue(infoValue);
            }
            IList<DeliveryPallet> palletList = new List<DeliveryPallet> ();
            foreach (Delivery tmp in deliveryList)
            {
                foreach (DeliveryPallet aPallet in tmp.DnPalletList)
                {
                    bool nonAdd = false;
                    foreach(DeliveryPallet bPallet in palletList)
                    {
                        if (bPallet.PalletID == aPallet.PalletID)
                        {
                            nonAdd = true;
                            break;
                        }
                    }
                    if(nonAdd == false)
                    {
                        palletList.Add(aPallet);
                    }
                }
            }
            foreach (DeliveryPallet pallet in palletList)
            {
                palletRepository.DeletePakWhPltTypeByPlt(pallet.PalletID);
                PakWhPltTypeInfo newType = new PakWhPltTypeInfo();
                newType.carrier = para1;
                newType.bol = para2;
                newType.plt = pallet.PalletID;
                newType.cdt = DateTime.Now;
                newType.tp = "F";
                palletRepository.InsertPakWhPltTypeInfo(newType);
            }
        }
        /// <summary>
        /// OrderPltType
        /// </summary>
        /// <param name="plt">plt</param>
        public S_common_ret OrderPltType(string plt)
        {
            try
            {
                S_common_ret ret = new S_common_ret();
                ret.state = 0;
                IList<PakWhPltTypeInfo> reGetTypeList = palletRepository.GetPakWhPltTypeListByPlt(plt);
                foreach (PakWhPltTypeInfo tmp in reGetTypeList)
                {
                    if (null != tmp)
                    {
                        if (tmp.bol.Length > 0)
                        {
                            //有资料时,不需要重新整理
                            ret.state = 101;
                            return ret;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                S_DN_Extended value = GetDN(plt);
                if (value.success == 0)
                {
                    ret.state = 3;
                    return ret;
                }
                if (value.regId == "SAF" || value.regId == "SCN")
                {
                    if (null != value.consolidated
                        && value.consolidated.Length > 0)
                    {
                        DoOrderPltType(value.consolidated, "Consolidated", value.carrier, value.consolidated);
                    }
                    else if (null != value.emeaCarrier
                        && value.emeaCarrier.Length > 0)
                    {
                        DoOrderPltType(value.emeaCarrier, "EmeaCarrier", value.carrier, value.emeaCarrier);
                    }
                    else if (null != value.bol
                        && value.bol.Length > 0)
                    {
                        DoOrderPltType(value.bol, "BOL", value.carrier, value.bol);
                    }
                    else if (null != value.carrier
                        && value.carrier.Length > 0)
                    {
                        palletRepository.DeletePakWhPltTypeByPlt(plt);
                        PakWhPltTypeInfo newType = new PakWhPltTypeInfo();
                        newType.carrier = value.carrier;
                        newType.bol = value.carrier;
                        newType.plt = plt;
                        newType.cdt = DateTime.Now;
                        newType.tp = "F";
                        palletRepository.InsertPakWhPltTypeInfo(newType);
                    }
                }
                else if (!(value.regId == "SAF" || value.regId == "SCN"))
                {
                    if (null != value.emeaCarrier
                        && value.emeaCarrier.Length > 0)
                    {
                        DoOrderPltType(value.emeaCarrier, "EmeaCarrier", value.carrier, value.emeaCarrier);
                    }
                    else if (null != value.bol
                        && value.bol.Length > 0)
                    {
                        DoOrderPltType(value.bol, "BOL", value.carrier, value.bol);
                    }
                    else if (null != value.carrier
                        && value.carrier.Length > 0)
                    {
                        palletRepository.DeletePakWhPltTypeByPlt(plt);
                        PakWhPltTypeInfo newType = new PakWhPltTypeInfo();
                        newType.carrier = value.carrier;
                        newType.bol = value.carrier;
                        newType.plt = plt;
                        newType.cdt = DateTime.Now;
                        newType.tp = "F";
                        palletRepository.InsertPakWhPltTypeInfo(newType);
                    }
                }
                //成功结束
                ret.state = 102;
                return ret;
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
        }
        /// <summary>
        /// AssignBol
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        public S_common_ret AssignBol(string plt, string editor)
        {
            try
            {
                S_common_ret ret = new S_common_ret();
                ret.state = 0;
                IList<PakWhPltTypeInfo> reGetTypeList = palletRepository.GetPakWhPltTypeListByPlt(plt);
                string bol = "";
                string carrier = "";
                string tp = "";
                foreach (PakWhPltTypeInfo tmp in reGetTypeList)
                {
                    if (null != tmp)
                    {
                        if (tmp.bol != null && tmp.bol.Length > 0
                            || tmp.carrier != null && tmp.carrier.Length > 0
                            || tmp.tp != null && tmp.tp.Length > 0)
                        {
                            if (bol == "" || tmp.bol != null)
                            {
                                bol = tmp.bol;
                            }
                            if (carrier == "" || tmp.carrier != null)
                            {
                                carrier = tmp.carrier;
                            }
                            if (tp == "" || tmp.tp != null)
                            {
                                tp = tmp.tp;
                            }
                        }
                    }
                }
                if (bol == ""
                    && carrier == ""
                    && tp == "")
                {
                    ret.state = -4;
                    return ret;
                }
                if (bol != carrier)
                {
                    IList<PakWhLocMasInfo> getLocList = palletRepository.GetPakWhLocMasListByBolAndPlt1(bol, "");
                    if (getLocList.Count > 0)
                    {
                        //成功不需要做分配
                        ret.state = 101;
                        return ret; ;
                    }
                    IList<PakWhLocMasInfo> locList = palletRepository.GetPakWhLocMasListByBolAndPlt1AndCarrier("", "", carrier);
                    if (null == locList ||
                       locList.Count == 0)
                    {
                        ret.state = 3;
                        return ret;
                    }
                    foreach (PakWhLocMasInfo aLoc in locList)
                    {
                        palletRepository.UpdatePakWhLocBolByColAndLoc(bol, aLoc.col, aLoc.loc);
                    }
                    //成功结束
                    ret.state = 102;
                    return ret;
                }
                else
                {
                    //没有BOL号码，直接分配库位
                    WhPltLogInfo newLog = new WhPltLogInfo();
                    newLog.plt = plt;
                    newLog.editor = editor;
                    newLog.wc = "IN";
                    newLog.cdt = DateTime.Now;
                    palletRepository.InsertWhPltLog(newLog);
                    WhPltMasInfo haveRecord = palletRepository.GetWHPltMas(plt);
                    if (null != haveRecord)
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.UpdateWhPltMas(newMasInfo, plt);
                    }
                    else
                    {
                        WhPltMasInfo newMasInfo = new WhPltMasInfo();
                        newMasInfo.plt = plt;
                        newMasInfo.editor = editor;
                        newMasInfo.wc = "IN";
                        newMasInfo.cdt = DateTime.Now;
                        newMasInfo.udt = DateTime.Now;
                        palletRepository.InsertWhPltMas(newMasInfo);
                    }
                    IList<PakWhLocMasInfo> locList = palletRepository.GetPakWhLocMasListByBolAndPlt1AndCarrier("", "", carrier);
                    if (locList.Count == 0)
                    {
                        //此貨代庫位已滿,請自行處理擺放
                        ret.state = 201;
                        return ret;
                    }
                    foreach (PakWhLocMasInfo aLoc in locList)
                    {
                        WhPltLocLogInfo item = new WhPltLocLogInfo();
                        item.plt = plt;
                        item.loc = aLoc.col + aLoc.loc.ToString();
                        palletRepository.InsertWhPltLocLogInfo( item);
                        PakWhLocMasInfo newLoc = new PakWhLocMasInfo();
                        newLoc.plt1 = plt;
                        newLoc.udt = DateTime.Now;
                        newLoc.bol = bol;
                        palletRepository.UpdatePakWhLocByColAndLoc(newLoc, aLoc.col, aLoc.loc);
                        ret.describe = aLoc.col;
                        ret.describe = ret.describe + "#@#";
                        ret.describe = ret.describe + aLoc.loc.ToString();
                        //成功结束,請將此棧板放入'+@col+'區'+@loc+'庫位”
                        ret.state = 103;
                        return ret;
                    }
                    return ret;
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public void UpdatePalletWH(string palletNo, string editor, string line, string station, string customer)
        {
            Pallet rePallet = palletRepository.Find(palletNo);
            if (null == rePallet)
            {
                return;
            }
            station = "WH";
            logger.Debug(" UpdatePalletWH start, PalletNo:" + palletNo);
            try
            {
                
                Session currentSession = SessionManager.GetInstance.GetSession(palletNo, currentSessionType);

                
                if (currentSession == null)
                {
                    currentSession = new Session(palletNo, currentSessionType, editor, "WH", line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", palletNo);
                    wfArguments.Add("Station", "WH");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "WHPalletControl.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + palletNo + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(palletNo);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.Pallet, rePallet);
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(palletNo);
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

               
                return ;

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
                Session sessionDelete = SessionManager.GetInstance.GetSession(palletNo, currentSessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("UpdatePalletWH end, PalletNo:" + palletNo);
            }
        }
        /// <summary>
        /// AssignPallet
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        public S_common_ret AssignPallet(string plt, string editor)
        {
            try
            {

                Pallet curPallet = palletRepository.Find(plt);
                if (null != curPallet)
                {
                    curPallet.Station = "WH";
                    curPallet.Editor = editor;
                    curPallet.Udt = DateTime.Now;
                    PalletLog newPalletLog = new PalletLog();
                    newPalletLog.Editor = editor;
                    newPalletLog.Line = "";
                    newPalletLog.Station = "WH";
                    newPalletLog.Cdt = DateTime.Now;
                    curPallet.AddLog(newPalletLog);

                    IUnitOfWork uow = new UnitOfWork();
                    palletRepository.Update(curPallet, uow);
                    uow.Commit();
                }
                              
                
                S_common_ret ret = new S_common_ret();
                ret.state = 0;
                WhPltLogInfo newLog = new WhPltLogInfo();
                newLog.plt = plt;
                newLog.editor = editor;
                newLog.wc = "IN";
                newLog.cdt = DateTime.Now;
                palletRepository.InsertWhPltLog(newLog);
                WhPltMasInfo haveRecord = palletRepository.GetWHPltMas(plt);
                if (null != haveRecord)
                {
                    WhPltMasInfo newMasInfo = new WhPltMasInfo();
                    newMasInfo.plt = plt;
                    newMasInfo.editor = editor;
                    newMasInfo.wc = "IN";
                    newMasInfo.udt = DateTime.Now;
                    palletRepository.UpdateWhPltMas(newMasInfo, plt);
                }
                else
                {
                    WhPltMasInfo newMasInfo = new WhPltMasInfo();
                    newMasInfo.plt = plt;
                    newMasInfo.editor = editor;
                    newMasInfo.wc = "IN";
                    newMasInfo.cdt = DateTime.Now;
                    newMasInfo.udt = DateTime.Now;
                    palletRepository.InsertWhPltMas(newMasInfo);
                }

                IList<PakWhPltTypeInfo> reGetTypeList = palletRepository.GetPakWhPltTypeListByPlt(plt);
                string bol = "";
                string carrier = "";
                string tp = "";
                foreach (PakWhPltTypeInfo tmp in reGetTypeList)
                {
                    if (null != tmp)
                    {
                        if (tmp.bol != null && tmp.bol.Length > 0
                            || tmp.carrier != null && tmp.carrier.Length > 0
                            || tmp.tp != null && tmp.tp.Length > 0)
                        {
                            if (bol == "" || tmp.bol != null)
                            {
                                bol = tmp.bol;
                            }
                            if (carrier == "" || tmp.carrier != null)
                            {
                                carrier = tmp.carrier;
                            }
                            if (tp == "" || tmp.tp != null)
                            {
                                tp = tmp.tp;
                            }
                        }
                    }
                }
                if (bol == "")
                {
                    ret.state = 30;
                    return ret;
                }
                
                
                
                
                /*S_DN_Extended value = GetDN(plt);
                
                
                
                
                
                if (value.success == 0)
                {
                    ret.state = 3;
                    return ret;
                }*/
                string col = "";
                int loc = 0;
                IList<PakWhLocMasInfo> getLocList = palletRepository.GetPakWhLocMasListByBolAndPlt1(bol, "");
                if (null == getLocList ||
                    getLocList.Count == 0)
                {
                    ret.state = 3;
                    return ret;
                }
                foreach (PakWhLocMasInfo aLoc in getLocList)
                {
                    if (aLoc.col == "")
                    {
                        //此貨代庫位已滿,請自行處理擺放
                        ret.state = 2;
                        return ret;
                    }
                    else
                    {
                        col = aLoc.col;
                        loc = aLoc.loc;
                    }
                    break;
                }
                WhPltLocLogInfo item = new WhPltLocLogInfo();
                item.plt = plt;
                item.loc = col + loc.ToString();
                palletRepository.InsertWhPltLocLogInfo(item);
                PakWhLocMasInfo newLoc = new PakWhLocMasInfo();
                newLoc.plt1 = plt;
                newLoc.udt = DateTime.Now;
                palletRepository.UpdatePakWhLocByColAndLoc(newLoc, col, loc);
                ret.describe = col;
                ret.describe = ret.describe + "#@#";
                ret.describe = ret.describe + loc.ToString();
                //成功结束,請將此棧板放入'+@col+'區'+@loc+'庫位”
                ret.state = 101;
                return ret;
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
        }
        /// <summary>
        /// RemovePallet
        /// </summary>
        /// <param name="plt">plt</param>
        /// <param name="editor">editor</param>
        public void RemovePallet(string plt, string editor)
        {
            try
            {
                palletRepository.UpdatePakWhLocByPltForClearPlt1AndPlt2(plt);
                WhPltMasInfo newMasInfo = new WhPltMasInfo();
                newMasInfo.plt = plt;
                newMasInfo.editor = editor;
                newMasInfo.wc = "RW";
                newMasInfo.udt = DateTime.Now;
                palletRepository.UpdateWhPltMas(newMasInfo, plt);
                WhPltLogInfo newLog = new WhPltLogInfo();
                newLog.plt = plt;
                newLog.editor = editor;
                newLog.wc = "RW";
                newLog.cdt = DateTime.Now;
                palletRepository.InsertWhPltLog(newLog);

                Pallet curPallet = palletRepository.Find(plt);
                if (null != curPallet)
                {
                    curPallet.Station = "RW";
                    curPallet.Editor = editor;
                    curPallet.Udt = DateTime.Now;
                    PalletLog newPalletLog = new PalletLog();
                    newPalletLog.Editor = editor;
                    newPalletLog.Line = "";
                    newPalletLog.Station = "RW";
                    newPalletLog.Cdt = DateTime.Now;
                    curPallet.AddLog(newPalletLog);

                    IUnitOfWork uow = new UnitOfWork();
                    palletRepository.Update(curPallet, uow);
                    uow.Commit();
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
        }
        /// <summary>
        /// GetPalletCount
        /// </summary>
        public int GetPalletCount()
        {
            try
            {
                int ret = 0;
                ret = palletRepository.GetCountOfWhPltMas("IN");
                return ret;
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
        }
        /// <summary>
        /// Get7Days
        /// </summary>
        public IList<S_Table_Data> Get7Days()
        {
            try
            {
                IList<S_Table_Data> ret = new List<S_Table_Data>();
                IList<WhPltMasInfo> palletList = new List<WhPltMasInfo>();
                palletList = palletRepository.GetWhPltMasList("IN", 7);
                foreach (WhPltMasInfo aLoc in palletList)
                {
                    if (string.IsNullOrEmpty(aLoc.plt))
                    {
                        continue;
                    }
                    S_Table_Data newData = new S_Table_Data();
                    newData.DeliveryNO = "";
                    newData.Model = "";
                    newData.PalletNO = aLoc.plt;
                    newData.Qty = "";
                    newData.Forwarder = "";
                    newData.HAWB = "";
                    newData.LOC = "";
                    IList<DeliveryPallet> reDeliveryPallet = palletRepository.GetDeliveryPallet(aLoc.plt);
                    if (null != reDeliveryPallet && reDeliveryPallet.Count > 0)
                    {
                        //一个
                        foreach (DeliveryPallet tmp in reDeliveryPallet)
                        {
                            Delivery reDelivery = currentRepository.Find(tmp.DeliveryID);
                            if (null == reDelivery)
                            {
                                break;
                            }
                            if (!string.IsNullOrEmpty(reDelivery.DeliveryNo))
                            {
                                newData.DeliveryNO = reDelivery.DeliveryNo;
                            }
                            else
                            {
                                break;
                            }
                            if (!string.IsNullOrEmpty(reDelivery.ModelName))
                            {
                                newData.Model = reDelivery.ModelName;
                            }

                            newData.Qty = tmp.DeliveryQty.ToString();
                            
                            string reForwarder = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "Carrier");
                            if (!string.IsNullOrEmpty(reForwarder))
                            {
                                newData.Forwarder = reForwarder;
                            }
                            string reHAWB = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "BOL");
                            if (!string.IsNullOrEmpty(reHAWB))
                            {
                                newData.HAWB = reHAWB;
                            }
                            newData.Satus = "IN";
                            IList<PakWhLocMasInfo> reWhLocMas = palletRepository.GetPakWhLocMasListByPlt1OrPlt2(aLoc.plt);
                             
                            foreach (PakWhLocMasInfo Loc in reWhLocMas)
                             {
                                 newData.LOC = Loc.col + Loc.loc.ToString();
                                 break;
                             }
                             ret.Add(newData);
                             break;
                        }
                    }
                }
                return ret;
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
        }
        /// <summary>
        /// GetDateFromToNotIN
        /// </summary>
        public IList<S_Table_Data> GetDateFromToNotIN(DateTime begin, DateTime end)
        {
            try
            {
                IList<S_Table_Data> ret = new List<S_Table_Data>();
                IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
                IList<Delivery> getDeliveryList = currentRepository.GetDeliveriesByShipDateRange(begin, end);
                foreach (Delivery aDelivery in getDeliveryList)
                {
                    IList<DeliveryPalletInfo> re = currentRepository.GetDeliveryPalletListByDN(aDelivery.DeliveryNo);
                    foreach (DeliveryPalletInfo aDeliveryPalletInfo in re)
                    {
                        if (aDeliveryPalletInfo.deliveryQty == 0)
                        {
                            continue;
                        }
                        if (palletList.Contains(aDeliveryPalletInfo))
                        {
                            continue;
                        }
                        if (null != palletRepository.GetWHPltMas(aDeliveryPalletInfo.palletNo))
                        {
                            continue;
                        }
                        palletList.Add(aDeliveryPalletInfo);
                    }
                }

                foreach (DeliveryPalletInfo aLoc in palletList)
                {
                    if (string.IsNullOrEmpty(aLoc.palletNo))
                    {
                        continue;
                    }
                    S_Table_Data newData = new S_Table_Data();
                    newData.DeliveryNO = "";
                    newData.Model = "";
                    newData.PalletNO = aLoc.palletNo;
                    newData.Qty = "";
                    newData.Forwarder = "";
                    newData.HAWB = "";
                    newData.LOC = "";

                    Delivery reDelivery = currentRepository.Find(aLoc.deliveryNo);
                    if (null == reDelivery)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(reDelivery.DeliveryNo))
                    {
                        newData.DeliveryNO = reDelivery.DeliveryNo;
                    }
                    else
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(reDelivery.ModelName))
                    {
                        newData.Model = reDelivery.ModelName;
                    }

                    IList<DeliveryPallet> rePalletD = palletRepository.GetDeliveryPalletByDNAndPallet(reDelivery.DeliveryNo, aLoc.palletNo);
                    foreach (DeliveryPallet aPalletD in rePalletD)
                    {
                        newData.Qty = aPalletD.DeliveryQty.ToString();
                        break;
                    }
                    
                    string reForwarder = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "Carrier");
                    if (!string.IsNullOrEmpty(reForwarder))
                    {
                        newData.Forwarder = reForwarder;
                    }
                    string reHAWB = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "BOL");
                    if (!string.IsNullOrEmpty(reHAWB))
                    {
                        newData.HAWB = reHAWB;
                    }
                    newData.Satus = "";
                    newData.LOC = "";
                    ret.Add(newData);
                }
                return ret;
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
        }
        /// <summary>
        /// GetDateFromTo
        /// </summary>
        public IList<S_Table_Data> GetDateFromTo(DateTime begin, DateTime end)
        {
            try
            {
                IList<S_Table_Data> ret = new List<S_Table_Data>();
                IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
                IList<Delivery> getDeliveryList = currentRepository.GetDeliveriesByShipDateRange(begin, end);
                foreach (Delivery aDelivery in getDeliveryList)
                {
                    IList<DeliveryPalletInfo> re = currentRepository.GetDeliveryPalletListByDN(aDelivery.DeliveryNo);
                    foreach (DeliveryPalletInfo aDeliveryPalletInfo in re)
                    {
                        if (aDeliveryPalletInfo.deliveryQty == 0)
                        {
                            continue;
                        }
                        if (palletList.Contains(aDeliveryPalletInfo))
                        {
                            continue;
                        }
                        if (null == palletRepository.GetWHPltMas(aDeliveryPalletInfo.palletNo))
                        {
                            continue;
                        }
                        palletList.Add(aDeliveryPalletInfo);
                    }
                }

                foreach (DeliveryPalletInfo aLoc in palletList)
                {
                    if (string.IsNullOrEmpty(aLoc.palletNo))
                    {
                        continue;
                    }
                    S_Table_Data newData = new S_Table_Data();
                    newData.DeliveryNO = "";
                    newData.Model = "";
                    newData.PalletNO = aLoc.palletNo;
                    newData.Qty = "";
                    newData.Forwarder = "";
                    newData.HAWB = "";
                    newData.LOC = "";

                    Delivery reDelivery = currentRepository.Find(aLoc.deliveryNo);
                    if (null == reDelivery)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(reDelivery.DeliveryNo))
                    {
                        newData.DeliveryNO = reDelivery.DeliveryNo;
                    }
                    else
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(reDelivery.ModelName))
                    {
                        newData.Model = reDelivery.ModelName;
                    }

                    IList<DeliveryPallet> rePalletD = palletRepository.GetDeliveryPalletByDNAndPallet(reDelivery.DeliveryNo, aLoc.palletNo);
                    foreach (DeliveryPallet aPalletD in rePalletD)
                    {
                       newData.Qty = aPalletD.DeliveryQty.ToString();
                       break;
                    }
                    

                    string reForwarder = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "Carrier");
                    if (!string.IsNullOrEmpty(reForwarder))
                    {
                        newData.Forwarder = reForwarder;
                    }
                    string reHAWB = currentRepository.GetDeliveryInfoValue(newData.DeliveryNO, "BOL");
                    if (!string.IsNullOrEmpty(reHAWB))
                    {
                        newData.HAWB = reHAWB;
                    }
                    WhPltMasInfo reInfo = palletRepository.GetWHPltMas(aLoc.palletNo);
                    if (null != reInfo)
                    {
                        if (reInfo.wc == "IN")
                        {
                            newData.Satus = "IN";
                        }
                        else if (reInfo.wc == "OT")
                        {
                            newData.Satus = "OT";
                        }
                        else if (reInfo.wc == "RW")
                        {
                            newData.Satus = "RW";
                        }
                    }

                    IList<PakWhLocMasInfo> reWhLocMas = palletRepository.GetPakWhLocMasListByPlt1OrPlt2(aLoc.palletNo);
                    foreach (PakWhLocMasInfo Loc in reWhLocMas)
                    {
                        newData.LOC = Loc.col + Loc.loc.ToString();
                        break;
                    }
                    ret.Add(newData);
                }
                return ret;
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
        }
        #endregion
    }
}
