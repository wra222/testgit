/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN                
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create  
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
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
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using carton = IMES.FisObject.PAK.CartonSSCC;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// CombineCartonInDN接口的实现类
    /// </summary>
    public class CombineCartonInDNForRCTO : MarshalByRefObject, ICombineCartonInDNForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="model"></param>
        /// <param name="firstProID"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSN(string inputCartonNo, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombineCartonInDN)inputCartonNo start, inputCartonNo:" + inputCartonNo);

            try
            {
                ArrayList retLst = new ArrayList();

                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                
                //a.	如果用户录入的[Carton No] 在数据库（CartonStatus.CartonNo）中不存在，则报告错误：“Invalid Carton No!”
                carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                CartonStatusInfo carConf = new CartonStatusInfo();
                carConf.cartonNo = inputCartonNo;
                IList<CartonStatusInfo> carList = cartRep.GetCartonStatusInfo(carConf);
                if (carList.Count == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(inputCartonNo);
                    throw new FisException("PAK139", errpara);//此Carton 不存在!
                }
                //b.如果用户录入的[Carton No] 已经结合了Pallet，则报告错误：“This Carton has combined Pallet!”
                IProduct eqCondition1 = new Product();
                IProduct notNullCondition1 = new Product();

                eqCondition1.CartonSN = inputCartonNo;
                notNullCondition1.PalletNo = "";
                IList<IProduct> prodcutlist1 = productRep.GetProductInfoListByConditionsNotNull(eqCondition1, notNullCondition1);
                if (prodcutlist1.Count != 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(inputCartonNo);
                    throw new FisException("PAK159", errpara);//“This Carton has combined Pallet!”

                }
                //c.如果用户录入的[Carton No] 已经结合了Delivery，则报告错误：“This Carton has combined Delivery!”
                IProduct eqCondition2 = new Product();
                IProduct notNullCondition2 = new Product();

                eqCondition2.CartonSN = inputCartonNo;
                notNullCondition2.DeliveryNo = "";
                IList<IProduct> prodcutlist2 = productRep.GetProductInfoListByConditionsNotNull(eqCondition2, notNullCondition2);
                if (prodcutlist2.Count != 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(inputCartonNo);
                    throw new FisException("PAK160", errpara);//“This Carton has combined Delivery!”

                }

                IList<IProduct> prodList = new List<IProduct>();
                prodList = productRep.GetProductListByCartonNo(inputCartonNo);
                if (prodList.Count == 0)
                {
                    throw new FisException("CHK109", new string[] { });                
                }

                IProduct curProduct;
                curProduct = productRep.Find(prodList[0].ProId);

                retLst.Add(prodList[0].Model);

                string sessionKey = prodList[0].ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(station, "CombineCartonInDNForRCTO.xoml", "CombineCartonInDNForRCTO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.Product, curProduct);
                    currentSession.AddValue(Session.SessionKeys.ProdList, prodList);
                    IList<string> productIDList = new List<string>();
                    currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.Product, curProduct);

                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                    /*FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;*/
                }
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                //========================================================
                ArrayList retList = new ArrayList();
            
                ProductInfoMaintain prodInfo = new ProductInfoMaintain();
                if (string.IsNullOrEmpty(curProduct.CUSTSN))
                {
                    curProduct.CUSTSN ="";
                }
                prodInfo.ProductID = curProduct.ProId;
                prodInfo.Sn = curProduct.CUSTSN;
                prodInfo.Model = curProduct.Model;
                //prodInfo.Station = vendorCT;

                IList<IProduct> proList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                IList<string>  idList = (List<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
                proList = productRep.GetProductListByCartonNo(inputCartonNo);
                foreach (IProduct item in proList)
                {
                    idList.Add(item.ProId);
                }

                currentSession.AddValue(Session.SessionKeys.ProdList, proList);
                currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, idList);

                retList.Add(prodInfo);
                retList.Add(prodList.Count);
                //========================================================
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
                logger.Debug("(CombineCartonInDN)inputCartonNo end, uutSn:" + inputCartonNo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        public ArrayList getDeliveryInfo(string deliveryNo)
        {
            try
            {
                //Check Qty by Delivery
                ArrayList retList = new ArrayList();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

                Delivery curDev = deliveryRep.Find(deliveryNo);
                //Get Total Qty，PCs in Carton and Packed Qty by Delivery
                //SELECT Qty as [Total Qty] FROM Delivery NOLOCK WHERE DeliveryNo = @Delivery
                int totalQty = curDev.Qty;
                //SELECT InfoValue as [PCs in Carton] FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @Delivery AND InfoType = 'CQty'
                string cQtyStr = (string)curDev.GetExtendedProperty("CQty");
                int cQty = 0;
                if (string.IsNullOrEmpty(cQtyStr))
                {
                    cQty = 0;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(cQtyStr);
                    cQty = Convert.ToInt32(tmp);
                }

                //SELECT COUNT(ProductID) as [Packed Qty] FROM Product NOLOCK WHERE DeliveryNo = @Delivery
                int packedQty = 0;
                var tmpList = productRep.GetProductListByDeliveryNo(curDev.DeliveryNo);
                packedQty = tmpList.Count;
                string model = "";
                model = curDev.ModelName;
                retList.Add(totalQty);
                retList.Add(cQty);
                retList.Add(packedQty);
                retList.Add(model);

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
                logger.Debug("(CombinPoInCarton)getDeliveryInfo, name:" + deliveryNo);
            }

        }

        public ArrayList Save(string firstProID, string deliveryNo, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinePoInCarton)Save start, firstProID:" + firstProID);

            try
            {
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Session currentSession = SessionManager.GetInstance.GetSession(firstProID, SessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(firstProID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    Delivery curDn = deliveryRep.Find(deliveryNo);
                    currentSession.AddValue(Session.SessionKeys.Delivery, curDn);

                    //如果Product 非Frame Or TRO Or BaseModel Or SLICE 的话，需要报告错误：“Product is not Frame Or TRO Or BaseModel Or SLICE” 
                    //SELECT @PN = Value FROM ModelInfo NOLOCk WHERE Model = @Model AND Name = 'PN'
                    IProduct product = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                    product.DeliveryNo = deliveryNo;

                    IList<IProduct> proList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                    foreach (IProduct item in proList)
                    {
                        item.DeliveryNo = deliveryNo;
                    }

                    string pn = "";
                    string modelstr = product.Model;
                    Model curModel = modelRep.Find(product.Model);
                    pn = curModel.GetAttribute("PN");

                    bool labelFlag = false;
                    if (!string.IsNullOrEmpty(pn) && pn.Length >= 6)
                    {
                        if (pn.Substring(5, 1) == "U"
                            || pn.Substring(5, 1) == "E"
                            || product.Model.Substring(0, 3) == "156"
                            || product.Model.Substring(0, 3) == "173"
                            || product.Model.Substring(0, 3) == "146"
                            || product.Model.Substring(0, 3) == "157"
                            || product.Model.Substring(0, 3) == "158"
                            || product.Model.Substring(0, 2) == "PO"
                            || product.Model.Substring(0, 2) == "2P"
                            || product.Model.Substring(0, 3) == "172"
                            || product.Model.Substring(0, 2) == "BC")
                        {
                            labelFlag = true;
                        }
                    }
                    if (!labelFlag)
                    {
                        SessionManager.GetInstance.RemoveSession(currentSession);
                        FisException fe = new FisException("PAK133", new string[] { });  //Product is not Frame Or TRO Or BaseModel Or SLICE
                        throw fe;
                    }

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
                ArrayList retList = new ArrayList();
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                Delivery delivery = (Delivery)currentSession.GetValue(Session.SessionKeys.Delivery);
                Product curProd = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                int totalQty = delivery.Qty;
                int packedQty = 0;

                var tmpList = productRep.GetProductListByDeliveryNo(delivery.DeliveryNo);
                foreach (var prod in tmpList)
                {
                    if (!string.IsNullOrEmpty(prod.CartonSN))
                    {
                        packedQty++;
                    }
                }

                string locate = (string)currentSession.GetValue("Location");                
                //a)	如果Remain Qty = 0，则提示用户：’Po:’ + @Delivery + ‘ is finished!’；
                //提示用户后，执行Reset（Reset 说明见下文）
                //b)	如果PCs in Carton > Remain Qty，则令[PCs in Carton] = Remain Qty
                string strEnd="";
                int remainQty = totalQty - packedQty;
                if (remainQty == 0)
                {
                    //5.当页面选择的Delivery 已经完成Combine PO in Carton_RCTO 时，需要将Delivery.Status 更新为'82'
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(delivery.DeliveryNo);
                    ex = new FisException("PAK136", erpara);//’Po:’ + @Delivery + ‘ is finished!’
                    strEnd = ex.mErrmsg;

                    IUnitOfWork uof = new UnitOfWork();
                    delivery.Status = "87";
                    delivery.Udt = DateTime.Now;
                    delivery.Editor = currentSession.Editor;
                    deliveryRep.Update(delivery,uof);
                    uof.Commit(); 
                }


                //当Delivery 的CQty 属性=1，但Delivery.Model 的NoCarton 属性(Model.InfoType = ‘NoCarton’)不存在，或者存在但<>’Y’ 时，也需要列印Carton Label
                //Model curModel = modelRep.Find(curProduct.Model);
                Model model = modelRep.Find(curProd.Model);
                string noCarton = model.GetAttribute("NoCarton");

                string cQtyStr = (string)delivery.GetExtendedProperty("CQty");
                int cQty = 0;
                if (string.IsNullOrEmpty(cQtyStr))
                {
                    cQty = 5;
                }
                else
                {
                    decimal tmp = Convert.ToDecimal(cQtyStr);
                    cQty = Convert.ToInt32(tmp);
                }

                string printflag = "N";
                if (cQty > 1 || (cQty == 1 && noCarton != "Y"))
                {
                    printflag = "Y";
                }
                printflag = "Y";
                retList.Add(printList);
                retList.Add(packedQty);
                retList.Add(curProd.CartonSN);
                retList.Add(curProd.DeliveryNo);
                retList.Add(curProd.CUSTSN);
                retList.Add(strEnd);
                retList.Add(printflag);

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
                logger.Debug("(CombinePoInCarton)Print end, firstProID:" + firstProID);
            }
        }

         /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string deliveryNO)
        {
            logger.Debug("(CombinePoInCarton)Cancel start, [deliveryNO]:" + deliveryNO);

            string sessionKey = deliveryNO;
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
                logger.Debug("(CombinePoInCarton)Cancel end, [deliveryNO]:" + deliveryNO);
            }
        }

        #endregion
    }
}
