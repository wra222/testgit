/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify Data for docking                   
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================

 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Collections;
using IMES.FisObject.Common;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using System.Data;
using IMES.FisObject.PAK.Pizza;
using carton = IMES.FisObject.PAK.CartonSSCC;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// Pallet Verify
    /// station : 9A
    /// 本站实现的功能：
    ///     检查Pallet 上的所有PRODUCT；
    ///     列印Ship to Pallet Label；
    ///     内销要额外列印一张Pallet CPMO Label
    /// </summary> 

    public class PalletVerifyForRCTO : MarshalByRefObject, IPalletVerifyForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPalletVerifyForRCTO Members
        carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

        public ArrayList InputFirstCartonNo(string firstCN, string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletVerifyForRCTO)InputFirstCartonNo start, custsn:" + firstCN + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            try
            {                
                // check carton info
                CartonStatusInfo condition = new CartonStatusInfo();
                condition.cartonNo = firstCN;
                IList<CartonStatusInfo> CartonStatus = cartRep.GetCartonStatusInfo(condition);
                if (CartonStatus.Count == 0)
                {
                    //erpara.Add("Invalid Carton No!");
                    erpara.Add(firstCN);
                    ex = new FisException("PAC001", erpara);
                    throw ex;
                }
                
                IList<IProduct>productList =  productRepository.GetProductListByCartonNo(firstCN);
                if (productList.Count == 0)
                {
                    //erpara.Add("This Carton does not be combined with Product!");
                    erpara.Add(firstCN);
                    ex = new FisException("PAK161", erpara);
                    throw ex;
                }
                string palletNo = productList[0].PalletNo;
                string DeliveryNo = productList[0].DeliveryNo;

                IProduct isNullCondition = new Product();
                isNullCondition.PalletNo ="";

                IProduct eqCondition = new Product();
                eqCondition.CartonSN =firstCN;

                IList<IProduct>productPalletLst =  productRepository.GetProductInfoListByConditions( eqCondition,  isNullCondition);
                if (productPalletLst.Count != 0)
                {
                    //erpara.Add("This Carton does not be combined with Pallet!");
                    erpara.Add(firstCN);
                    ex = new FisException("PAK162", erpara);
                    throw ex;
                }
                
                ////check The Pallet have verified!
                PalletLogInfo log_condition = new PalletLogInfo();
                log_condition.palletNo = palletNo;
                log_condition.station = "9A";
                IList<PalletLogInfo> lstPallet = iPalletRepository.GetPalletLogInfoList(log_condition);
                if (lstPallet.Count > 0)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("PAK163", erpara);    //Pallet have verified
                    throw ex;
                }

                ////check shipment
                int palletQty = DeliveryRepository.GetSumofDeliveryQtyFromDeliveryPallet(palletNo);

                IList<DeliveryPalletInfo> lstShipment = DeliveryRepository.GetDeliveryPalletListByPlt(palletNo);
                if (lstShipment.Count > 0)
                {
                    string shipment = lstShipment[0].shipmentNo;

                    string consolidate = DeliveryRepository.GetDeliveryInfoValue(DeliveryNo, "Consolidated");

                    if (consolidate != null && consolidate !="")
                    {
                        int count = DeliveryRepository.GetCountOfDeliveryNoPrefixForDoubleDeliveryInfoPairs("Consolidated", consolidate, "RedShipment", consolidate);
                        if (palletNo.Substring(0, 2) != "BA" && palletNo.Substring(0, 2) != "NA")
                        {
                            string[] pattern = consolidate.Split('/');
                            int dnQty = 0;
                            if (pattern.Length.ToString() != "2" || string.IsNullOrEmpty(pattern[0]) || string.IsNullOrEmpty(pattern[1]))
                            {
                                erpara.Add(firstCN);
                                ex = new FisException("PAK024", erpara);  //找不到该Delivery No 的Consolidated 属性
                                throw ex;
                            }

                            dnQty = Int32.Parse(pattern[1]);

                            if (dnQty != count)
                            {
                                erpara.Add(firstCN);
                                ex = new FisException("PAK165", erpara);  //Delivery 尚未完全Download
                                throw ex;
                            }
                        }
                        ////////////////

                        int SumCartonQty = 0;
                        int SumDnPallletQty = 0;
                        IList<Delivery> dnList = DeliveryRepository.GetDeliveryListByInfoTypeAndValue("Consolidated", consolidate);
                        foreach (Delivery dn in dnList)
                        {
                            string DeliveryNo1 = dn.DeliveryNo;
                            string cqty = DeliveryRepository.GetDeliveryInfoValue(DeliveryNo1, "CQty");
                            Decimal qty = Convert.ToDecimal(cqty);
                            int cartonQty = (int)(dn.Qty / qty);
                            if (dn.Qty % qty != 0)
                                cartonQty++;
                            SumCartonQty += cartonQty;
                            int DnPallletQty = DeliveryRepository.GetSumDeliveryQtyOfACertainDN(DeliveryNo1);
                            SumDnPallletQty += DnPallletQty;
                        }
                        if (SumCartonQty != SumDnPallletQty)
                        {
                            //从整机库get
                            {
                                erpara.Add(palletNo);
                                ex = new FisException("CHK903", erpara);    //PALLET  未完全Download!
                                throw ex;
                            }
                        }
                    }
                }
                retLst.Add(palletNo);
                retLst.Add(firstCN);
                retLst.Add(DeliveryNo);
                retLst.Add(palletQty);

                return retLst;

            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletVerifyImpl)InputCustSNOnCooLabel end, custsn:" + firstCN + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="custSn">custSn</param>
        /// <returns></returns>
        public ArrayList InputCartonNo(string firstCN, string CartonNo,string firstPalletNo)
        {
            logger.Debug("(PalletVerifyImpl)ScanSN start, firstSn:" + firstCN + " custSn:" + CartonNo);
            ArrayList retLst = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {   
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
               

                CartonStatusInfo condition = new CartonStatusInfo();
                condition.cartonNo = CartonNo;
                IList<CartonStatusInfo> CartonStatus = cartRep.GetCartonStatusInfo(condition);
                if (CartonStatus.Count == 0)
                {
                    //erpara.Add("Invalid Carton No!");
                    erpara.Add(CartonNo);
                    ex = new FisException("PAC001", erpara);
                    throw ex;
                }

                IList<IProduct> productList = productRepository.GetProductListByCartonNo(CartonNo);
                if (productList.Count == 0)
                {
                    //erpara.Add("This Carton does not be combined with Product!");
                    erpara.Add(CartonNo);
                    ex = new FisException("PAK161", erpara);
                    throw ex;
                }
                string palletNo = productList[0].PalletNo;
                string DeliveryNo = productList[0].DeliveryNo;

                IProduct isNullCondition = new Product();
                isNullCondition.PalletNo = "";

                IProduct eqCondition = new Product();
                eqCondition.CartonSN = CartonNo;

                IList<IProduct> productPalletLst = productRepository.GetProductInfoListByConditions(eqCondition, isNullCondition);
                if (productPalletLst.Count != 0)
                {
                    //erpara.Add("This Carton does not be combined with Pallet!");
                    erpara.Add(firstCN);
                    ex = new FisException("PAK162", erpara);
                    throw ex;
                }

                if (palletNo != firstPalletNo)
                {
                    //erpara.Add("This palletNo is not equal!");
                    erpara.Add(palletNo);
                    erpara.Add(firstPalletNo);
                    ex = new FisException("PAK164", erpara);
                    throw ex;
                }
                retLst.Add(palletNo);
                retLst.Add(CartonNo);
                retLst.Add(DeliveryNo);
                       
                return retLst;

                
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
                logger.Debug("(PalletVerifyImpl)ScanSN end, firstSn:" + firstCN + " custSn:" + CartonNo);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="printItems">printItems</param> 
 
        /// <returns></returns>
        public ArrayList Save(string firstCN, IList<PrintItem> printItems, string PalletNo,string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletVerifyImpl)Save start, firstSN:" + firstCN + "printItems" + printItems);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();


            string sessionKey = firstCN;

            try
            {             
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PalletVerifyForRCTO.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, firstCN);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.Pallet, PalletNo);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);

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
               
                IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                retLst.Add(resultPrintItems);
                string printLabel = currentSession.GetValue(Session.SessionKeys.QCStatus) as string;
                retLst.Add(printLabel);
                
                return retLst;
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
                logger.Debug("(PalletVerifyImpl)Save end, firstSn:" + firstCN + "printItems" + printItems);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string uutSn)
        {
            logger.Debug("(PalletVerifyImpl)Cancel Start," + "uutSn:" + uutSn);
            try
            {
                string sessionKey = uutSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                logger.Debug("(PalletVerifyImpl)Cancel End," + "uutSn:" + uutSn);
            }

        }

        /// <summary>
        /// 调存储过程： HP_EDI.dbo.op_TemplateCheck_LANEW 
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="docType">docType</param>
        /// <returns>DataTable</returns>
        public DataTable call_Op_TemplateCheckLaNew(string dn, string docType)
        {
            try
            {
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                DataTable TempDB = new DataTable();
                TempDB = repPizza.CallTemplateCheckLaNew(dn, docType);
                foreach (DataRow row in TempDB.Rows)
                {
                    if (row[0].ToString() == "ERROR")
                    {
                        FisException fe = new FisException("PAK105", new string[] { });  //存储过程HP_EDI.dbo.op_TemplateCheck_LANEW 报错，请检查！
                        throw fe;
                    }
                }

                return TempDB;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }


        /// <summary>
        /// 与Pallet绑定的所有Delivery
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <returns>list</returns>
        public IList<string> getDeliveryPalletList(string palletNo)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<DeliveryPalletInfo> DeliveryPalletInfoList = ideliveryRepository.GetDeliveryPalletListByPlt(palletNo);
            if (DeliveryPalletInfoList == null || DeliveryPalletInfoList.Count <= 0)
            {
                erpara.Add(palletNo);    //没有获得当前Pallet的DeliveryPallet！
                ex = new FisException("PAK045", erpara);
                throw ex;
            }
            IList<string> DeliveryList = new List<string>();
            foreach (DeliveryPalletInfo iDPI in DeliveryPalletInfoList)
            {
                DeliveryList.Add(iDPI.deliveryNo);
            }
            return DeliveryList;

        }

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>

        public ArrayList rePrint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletVerifyOnlyImpl)rePrint Start," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            string sessionKey = Guid.NewGuid().ToString();
            try
            {
                //获取Pallet Info
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = null;
                CurrentPallet = iPalletRepository.Find(palletNo);
                if (CurrentPallet == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK881", erpara);    // Pallet不存在！
                    throw ex;
                }
                PalletLogInfo condition = new PalletLogInfo();
                condition.palletNo = palletNo;
                condition.station = "9A";
                IList<PalletLogInfo>lstPallet  =  iPalletRepository.GetPalletLogInfoList(condition);
                if (lstPallet.Count <= 0)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("PAK120", erpara);    
                    throw ex;
                }

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PalletVerifyReprintForRCTO.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.Pallet, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "PalletReprintForRCTO");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletDataVerifyReprintForRCTO");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                retLst.Add(resultPrintItems);
                string printLabel = currentSession.GetValue(Session.SessionKeys.QCStatus) as string;
                retLst.Add(printLabel);
                string flag = (string)currentSession.GetValue(Session.SessionKeys.EEP);
                retLst.Add(flag);

                return retLst;

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
                logger.Debug("(PalletVerifyOnlyImpl)rePrint End," + "palletNo:" + palletNo + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer + "printItems" + printItems);
            }

        }


        #endregion
    }


}
