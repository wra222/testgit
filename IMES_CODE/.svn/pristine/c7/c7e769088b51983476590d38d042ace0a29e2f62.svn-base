/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: FinalScan Implementation
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-10  Zhang.Kai-sheng       (Reference Ebook SourceCode) Create
 * TODO:
 * UI/UC Update (2010/10/20),ADD "Chep Pallet" 
 * UC --"5. Check Pallet"/"7. Check Chep Pallet"/"6. Save Data" -add "Chep Pallet Check" process (P7-P10)
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using System.Collections;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using IMES.Route;
using IMES.FisObject.PAK.StandardWeight;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IFinalScan接口的实现类
    /// </summary>
    public class FinalScan : MarshalByRefObject, IFinalScan   
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType commonSessionType = Session.SessionType.Common;
        
        #region IFinalScan members

        /// <summary>
        /// 输入pickID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">pickID</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <returns>List</returns>
        public IList InputPickID(
            string pdLine,
            string pickID,
            string editor, string stationId, string customer)
        {
            logger.Debug("(FinalScanImpl)InputPickID start, [pdLine]:" + pdLine
                + " [pickID]: " + pickID
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = pickID;
            IList lstRet = new ArrayList();
            
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, commonSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个TruckID对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", commonSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "FinalScan.xoml", "FinalScan.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
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

                //add items to list
                var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
               // var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now);
                var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now,"Out");
                //UC Update:	Duplicate，刷入的栈板是该Pick Card ID 结合的栈板，但是在FwdPlt 表中Status 已经是Out 时，发出的声音
                var outlst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "Out", DateTime.Now);

                //Add 2012/06/14 UC Update :Get Chep Pallet Qty
                //SELECT COUNT(DISTINCT b.PalletNo)
                //FROM FwdPlt a (NOLOCK) LEFT JOIN ChepPallet b (NOLOCK)
                //    ON a.Plt = b.PalletNo
                //WHERE a.PickID = @PickId
                //    AND a.Status = 'Out'
                //    AND CONVERT(char(10), a.Udt, 111) = CONVERT(char(10), GETDATE(), 111) :111-yy/mm/dd
                int iChepPalletQty = palletRepository.GetChepPalletQty(pickID, "Out");

                string showDriver = session.GetValue("ShowDriver") as string;
                string showForwarder = session.GetValue("ShowForwarder") as string;

                lstRet.Add((PickIDCtrlInfo)session.GetValue(Session.SessionKeys.PickIDCtrl)); //PickIDCtrl from GetPickIDCtrlByPickUD Activity
                lstRet.Add(lst);
                ForwarderInfo fwderInfo = (ForwarderInfo)session.GetValue(Session.SessionKeys.Forwarder);
                
                //lstRet.Add(fwderInfo.Forwarder);
                //lstRet.Add(fwderInfo.Driver);
                lstRet.Add(showForwarder);
                lstRet.Add(showDriver);
				
                //Add
                lstRet.Add(outlst);
                lstRet.Add(iChepPalletQty);
                if (lst == null || lst.Count == 0)
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);

                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                    //Add Kaisheng2012/03/23 UC Update:异常：若未能取得符合调剂的Pallet，则报告错误“未找到当天出货的Pallet”。
                    //2012/10/11 若未能取得符合条件的Pallet--
                    //"In" "Out"均为NULL--未找到当天出货的Pallet
                    if (outlst == null || outlst.Count == 0)
                    {
                        //FwdPlt--当天该PickID的"In" "Out"均为NULL--[PickID：%1]未找到当天出货的Pallet
                        List<string> errpara = new List<string>();
                        errpara.Add(pickID);
                        throw new FisException("CHK185", errpara);
                    }
                    else
                    {
                        //FwdPlt--当天该PickID的"In"为NULL，当天该PickID的"Out"不为NULL --[PickID：%1]已完成当天出货的Pallet
                        var ChepPalletCount = iChepPalletQty.ToString();
                        List<string> errpara = new List<string>();
                        errpara.Add(pickID);
                        errpara.Add(ChepPalletCount);
                        throw new FisException("CHK288", errpara);
                    }
                    
                }
                return lstRet;
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
                logger.Debug("(FinalScanImpl)InputTruckID end, [pdLine]:" + pdLine
                    + " [pickID]: " + pickID
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        ///----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 输入UCCID相关Pallet信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">pickID</param>
        /// <param name="UCCID">pltNo</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        public IList InputUCCID(
            string pdLine,
            string pickID,
            string UCCID,
            string editor, string stationId, string customer)
        {
            logger.Debug("(FinalScanImpl)InputUCCID start, [pdLine]:" + pdLine
                + " [pickID]: " + pickID
                + " [UCCID]: " + UCCID
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            //FisException ex;
            List<string> erpara = new List<string>();
            IList lstRet = new ArrayList();
            IList lstPlt = new ArrayList();
            try
            {
                var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                var lst = (string)palletRepository.GetPalletNoByUcc(UCCID);

                if (string.IsNullOrEmpty(lst))
                {
                    throw new FisException("CHK106", new string[] { UCCID });
                }
            
                //   lstPlt = InputPalletNo(pdLine, pickID, lst, editor, stationId, customer);
                lstRet.Add(lst);
                return lstRet;
            
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
                logger.Debug("(FinalScanImpl)InputUCCID end, [pdLine]:" + pdLine
                    + " [pickID]: " + pickID
                    + " [pltNo]: " + UCCID
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }
        ///----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 输入pallet no相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">pickID</param>
        /// <param name="pltNo">pltNo</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        public IList InputPalletNo(
            string pdLine,
            string pickID,
            string pltNo,
            string editor, string stationId, string customer)
        {
            logger.Debug("(FinalScanImpl)InputPalletNo start, [pdLine]:" + pdLine
                + " [pickID]: " + pickID
                + " [pltNo]: " + pltNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = pickID;
            IList lstRet = new ArrayList();

            string RegId="";
            string PalletQty="";
            string ShipWay = "";
            int PQty = 0;

            bool bNeedChepPalletCheck = false;

            try
            {
                //Need Chep Pallet Check？
                //get pallet ->Delivery->Model
                IPalletRepository PltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

                //UC Add :如果当前刷入的Pallet 在Pallet 表不存在，则不需要进行Chep Pallet Check
                //DEBUG ITC-1360-0590 
                Pallet ExistPallet = PltRepository.Find(pltNo);
                if (ExistPallet != null)
                {

                    IList<DeliveryPallet> dpList = PltRepository.GetDeliveryPallet(pltNo);
                    if ((dpList == null) || (dpList.Count == 0))
                    {
                        List<string> errpara1 = new List<string>();
                        errpara1.Add(pltNo);
                        throw new FisException("CHK241", errpara1);
                    }

                    //mantis1695: Final Scan需增加check前一站是否有刷入WC=IN 
                    WhPltMasInfo info = PltRepository.GetWHPltMas(pltNo);
                    if (info == null || info.wc.Trim() != "IN")
                    {
                        string wc = info == null ? "" : info.wc;

                        throw new FisException("CHK999", new string[] { wc });
                    }

                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    Delivery CurrentDelivery = DeliveryRepository.Find(dpList[0].DeliveryID);
                    //get RegId / PalletQty / ShipWay / PQty
                    IDeliveryRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                    if (CurrentDelivery.ModelName.Substring(0, 2) == "PC")
                    {

                        RegId = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnDeliveryNo(pltNo, "RegId");
                        PalletQty = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnDeliveryNo(pltNo, "PalletQty");
                        ShipWay = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnDeliveryNo(pltNo, "ShipWay");

                        PQty = itemRepository.GetSumofDeliveryQtyFromDeliveryPallet(pltNo);
                    }
                    else
                    {
                        RegId = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnShipmentNo(pltNo, "RegId");
                        PalletQty = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnShipmentNo(pltNo, "PalletQty");
                        ShipWay = itemRepository.GetCertainInfoValueForDeliveryByPalletNoOnShipmentNo(pltNo, "ShipWay");

                        PQty = itemRepository.GetSumofDeliveryQtyFromDeliveryPallet(pltNo);
                    }
                    if ((PalletQty == null) || (PalletQty.Trim() == ""))
                        PalletQty = "0";

                    if (RegId != null && RegId.Length == 3)
                    { RegId = RegId.Substring(1, 2); }
                    else
                    { RegId = ""; }

                    //Double numfPalletQty = Convert.ToDouble(PalletQty);
                    //long numlPalletQty = Convert.ToInt64(numfPalletQty);
                    //int numlPalletQty = Convert.ToInt32(numfPalletQty);
                  //  if (((RegId == "SNE") || (RegId == "SCE")) && (ShipWay == "T001"))
                    string site = IMES.Infrastructure.Utility.Common.CommonUti.GetSite();

                   
                   
                    if (site == "ICC")
                    {
                        IList<PalletType> lstPalletType = IMES.Infrastructure.Utility.Common.CommonUti.GetPalletType(pltNo, CurrentDelivery.DeliveryNo);
                        //mantis 0001400
                        IList<PickIDCtrlInfo> lstPickIDCtrlInfo = PltRepository.GetPickIDCtrlInfoByPickIDAndDate(pickID, DateTime.Now);
                        PickIDCtrlInfo pickIDCtrlInfo;
                        bool bneedcheppalletcheckbyfow = false;
                        if (lstPickIDCtrlInfo != null && lstPickIDCtrlInfo.Count != 0)
                        {
                            pickIDCtrlInfo = lstPickIDCtrlInfo[0];
                            IList<ConstValueTypeInfo> lstconstvaluetype = IMES.Infrastructure.Utility.Common.CommonUti.GetConstValueTypeByType("NeedCheckCheppalletFwd");
                            if (lstconstvaluetype != null && lstconstvaluetype.Count > 0)
                            {
                                bneedcheppalletcheckbyfow=lstconstvaluetype.Where(x => x.value == pickIDCtrlInfo.Fwd && !string.IsNullOrEmpty(x.value)).Any();
                            }
                        }

                       if ((lstPalletType.Count > 0 && lstPalletType[0].ChepPallet.Trim() == "1")||bneedcheppalletcheckbyfow)
                        {
                            bNeedChepPalletCheck = true;
                            lstRet.Add(pltNo);
                            lstRet.Add(bNeedChepPalletCheck);
                            return lstRet;
                        }
                        
                        else
                        { bNeedChepPalletCheck = false; }
                    }
                    else //  if (site == "ICC")
                    {
                        if (((RegId == "NE") || (RegId == "CE")) && (ShipWay == "T001"))
                        {
                            //int pqty2 = PltRepository.GetTierQtyFromPalletQtyInfo(numlPalletQty.ToString());
                            int pqty2 = PltRepository.GetTierQtyFromPalletQtyInfo(PalletQty);
                            //UC Update:修改栈板类型的判定方法
                            //B.	如果@pqty >= @pqty2时，需要进行Chep Pallet Check；否则，不需要进行Chep Pallet Check
                            //->    如果@pqty > @pqty2时，需要进行Chep Pallet Check；否则，不需要进行Chep Pallet Check
                            //if (PQty >= pqty2)
                            if (PQty > pqty2)
                            {
                                //Need Chep Pallet Check 
                                bNeedChepPalletCheck = true;
                                lstRet.Add(pltNo);
                                lstRet.Add(bNeedChepPalletCheck);
                                return lstRet;
                            }
                            else
                            {
                                bNeedChepPalletCheck = false;
                            }

                        }
                    }


                
                }
                else
                {                   
                    bNeedChepPalletCheck = false;
                }
                
                //don't need Chep Pallet Check 
                Session session = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.PalletNo, pltNo);
                    session.AddValue("IsCheckChepPallet", false);
                    
                    IPalletRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                    Pallet CurrentPallet = currentRepository.Find(pltNo);
                    session.AddValue(Session.SessionKeys.Pallet, CurrentPallet);

                    WhPltMasInfo resultWhPltMas = currentRepository.GetWHPltMas(pltNo);
                    if (resultWhPltMas == null || resultWhPltMas.wc.Trim() == "IN")
                    {
                        session.AddValue("IsNotInPalletButInWH_PLTMas", true);
                    }
                    else
                    {
                        session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    }

                    //if (CurrentPallet == null)
                    //{
                    //    WhPltMasInfo resultWhPltMas = currentRepository.GetWHPltMas(pltNo);
                    //    if (resultWhPltMas == null || resultWhPltMas.wc.Trim() == "IN")
                    //    {
                    //        session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    //    }
                    //    else
                    //    {
                    //        session.AddValue("IsNotInPalletButInWH_PLTMas", true);
                    //    }

                    //}
                    //else
                    //{
                    //    session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    //}

                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                }

                
                var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                //var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now);
                var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now,"Out");

                if (lst == null || lst.Count == 0)
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);

                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                }

                lstRet.Add(lst);
                lstRet.Add(bNeedChepPalletCheck);
                return lstRet;
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
                logger.Debug("(FinalScanImpl)InputPalletNo end, [pdLine]:" + pdLine
                    + " [pickID]: " + pickID
                    + " [pltNo]: " + pltNo
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        ///----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// InputChepPalletNo
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="pickID"></param>
        /// <param name="pltNo"></param>
        /// <param name="cheppltNo"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList InputChepPalletNo(
            string pdLine,
            string pickID,
            string pltNo,
            string chepPltNo,
            string editor, string stationId, string customer)
        {
            logger.Debug("(FinalScanImpl)InputChepPalletNo start, [pdLine]:" + pdLine
                + " [pickID]: " + pickID
                + " [pltNo]: " + pltNo
                + " [cheppltNo]: " + chepPltNo
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = pickID;
            IList lstRet = new ArrayList();

            try
            {
         
                //Chep Pallet Check 
                IPalletRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                ChepPalletInfo resultChepPallet = currentRepository.GetChepPalletInfo(chepPltNo);
                //if (resultChepPallet == null)
                //{
                //    List<string> errpara = new List<string>();
                //    errpara.Add(chepPltNo);
                //    ex = new FisException("PAK010", errpara);
                //    throw ex;
                //    //throw new FisException("PAK010", new string[] { chepPltNo });

                //}
                //Check Chep Pallet OK
                Session session = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.PalletNo, pltNo);
                    session.AddValue(Session.SessionKeys.ChepPallet, chepPltNo);
                    session.AddValue("IsCheckChepPallet", true);
                    
                    Pallet CurrentPallet = currentRepository.Find(pltNo);
                    session.AddValue(Session.SessionKeys.Pallet, CurrentPallet);

                    WhPltMasInfo resultWhPltMas = currentRepository.GetWHPltMas(pltNo);
                    if (resultWhPltMas == null || resultWhPltMas.wc.Trim() == "IN")
                    {
                        session.AddValue("IsNotInPalletButInWH_PLTMas", true);
                    }
                    else
                    {
                        session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    }

                    //if (CurrentPallet == null)
                    //{
                    //    WhPltMasInfo resultWhPltMas = currentRepository.GetWHPltMas(pltNo);
                    //    if (resultWhPltMas == null || resultWhPltMas.wc.Trim()=="IN")
                    //    {
                    //        session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    //    }
                    //    //else
                    //    //{
                    //    //    session.AddValue("IsNotInPalletButInWH_PLTMas", true);
                    //    //}

                    //}
                    //else
                    //{
                    //    session.AddValue("IsNotInPalletButInWH_PLTMas", false);
                    //}

                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                }


                var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                //var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now);
                var lst = palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now,"Out");

                if (lst == null || lst.Count == 0)
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);

                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                }

                lstRet.Add(lst);
                return lstRet;
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
                logger.Debug("(FinalScanImpl)InputChepPalletNo end, [pdLine]:" + pdLine
                    + " [pickID]: " + pickID
                    + " [pltNo]: " + pltNo
                    + " [cheppltNo]: " + chepPltNo
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }



        ///----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey); 

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

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
        ///----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 从FwdPlt表中获取相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">pickID</param>
        /// <param name="pltNo">pltNo</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        public IList<IMES.DataModel.FwdPltInfo> GetFwdPltList(string pdLine, string pickID, string pltNo, string editor, string stationId, string customer)
        {
            try
            {
                var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

                //return palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now);
                return palletRepository.GetFwdPltInfosByPickIDAndStatusAndDate(pickID, "In", DateTime.Now,"Out");
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
                //logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        #endregion
    }
}
