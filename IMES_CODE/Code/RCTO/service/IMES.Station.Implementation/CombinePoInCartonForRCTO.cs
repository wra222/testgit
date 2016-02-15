﻿/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21     
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0074  修正reprint入参错误
* ITC-1414-0082  补充syssetting描述
* ITC-1414-0087  修改DN显示格式
* ITC-1414-0125  DN完成后记录log
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
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using IMES.Infrastructure.Repository._Schema;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// CombinePoInCarton接口的实现类
    /// </summary>
    public class CombinePoInCartonForRCTO : MarshalByRefObject, ICombinePoInCartonForRCTO
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
        public ArrayList InputSN(string inputSN, string model, string firstProID, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombinePoInCarton)InputSN start, inputSN:" + inputSN);

            try
            {
                bool newflag = false;
                if (firstProID == "")
                {
                    newflag = true;
                }

                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();


                //[Product Id] – 如果用户录入的数据长度为9，且以字母’C’开头，则可以认定用户录入的数据是[Product Id]
                //[Vendor CT] – 如果用户录入的数据长度为14，则可以认定用户录入的数据是[Vendor CT]
                //SELECT ProductID FROM Product_Part NOLOCK WHERE PartSN = @VendorCT

                IProduct curProduct = null;
                string vendorCT = "";
                if (inputSN.Length == 9 || inputSN.Length == 10)
                {
                    //curProduct = productRep.Find(inputSN);
                    curProduct = productRep.GetProductByIdOrSn(inputSN);

                }
                if (curProduct==null)
                {
                    vendorCT = inputSN;
                    ProductPart conf = new ProductPart();
                    conf.PartSn = inputSN;

                    IList<ProductPart> productList = productRep.GetProductPartList(conf);
                    if (productList.Count > 0)
                    {
                        curProduct = productRep.Find(productList[0].ProductID);
                    }
                    else
                    {
                        //b.InfoType = 'MBCT2' AND b.InfoValue = @VendorCT	
                        var mcond = new IMES.FisObject.PCA.MB.MBInfo();
                        mcond.InfoType = "MBCT2";
                        mcond.InfoValue = vendorCT;
                        IList<IMES.FisObject.PCA.MB.MBInfo> mbList = mbRep.GetPcbInfoByCondition(mcond);
                        if (mbList.Count > 0)
                        {
                            IList<IProduct> pList = productRep.GetProductListByPCBID(mbList[0].PCBID);
                            if (pList.Count > 0)
                            {
                                curProduct = pList[0];
                            }
                        }
                        else
                        {
                            IList<IMES.FisObject.FA.Product.ProductInfo> productinfolist = productRep.GetProductInfoListByKeyAndValue("ModelCT", vendorCT);
                            {
                                if (productinfolist.Count > 0)
                                {

                                    curProduct = productRep.Find(productinfolist[0].ProductID);

                                }
                                else
                                {
                                    productinfolist = productRep.GetProductInfoListByKeyAndValue("SleeveCT", vendorCT);
                                    if (productinfolist.Count > 0)
                                    {

                                        curProduct = productRep.Find(productinfolist[0].ProductID);

                                    }

                                
                                }
                            }
                        }
                    }
                }

                if (null == curProduct)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(inputSN);
                    throw new FisException("SFC002", errpara);

                }

                if (model == "")
                {
                    model = curProduct.Model;
                }

                if (firstProID == "")
                {
                    firstProID = curProduct.ProId;
                }

                //检查Product Model (Product.Model) 是否与页面上的[Model] 相同，如果不同，则报告错误：“Model is not match!”
                if (curProduct.Model != model)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.Model);
                    ex = new FisException("PAK131", erpara);//Model is not match!
                    throw ex;
                }

                //h.	如果用户刷入的Product Id (可能是直接刷入的，也可能是基于Customer S/N查询到的)已经刷过，则报告错误：“Duplicate data!”
                if (!string.IsNullOrEmpty(curProduct.CartonSN))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(curProduct.Model);
                    ex = new FisException("PAK134", erpara);//Duplicate data!
                    throw ex;
                }
                CheckPAQC(curProduct);//检查QC抽中未刷出
                CheckFRUMBOA3(curProduct.PCBID, curProduct.Model);
                ArrayList retList = new ArrayList();
                string sessionKey = firstProID;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    if (!newflag)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        ex = new FisException("PAK157", erpara);
                        retList.Add("Error");
                        retList.Add(ex);
                        return retList;
                    }
                    
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

                    RouteManagementUtils.GetWorkflow(station, "CombinePoInCartonForRCTO.xoml", "CombinePoInCartonForRCTO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.Product, curProduct);
                    IList<IProduct> productList = new List<IProduct>();
                    currentSession.AddValue(Session.SessionKeys.ProdList, productList);
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
                    if (newflag)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
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

                ProductInfoMaintain prodInfo = new ProductInfoMaintain();
                if (string.IsNullOrEmpty(curProduct.CUSTSN))
                {
                    curProduct.CUSTSN = "";
                }
                prodInfo.ProductID = curProduct.ProId;
                prodInfo.Sn = curProduct.CUSTSN;
                prodInfo.Model = curProduct.Model;
                prodInfo.Station = vendorCT;

                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model curModel = modelRep.Find(curProduct.Model);
                string zmode = "";
                zmode = curModel.GetAttribute("ZMODE");

                IList<IProduct> proList = (List<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                IList<string> idList = (List<string>)currentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
                proList.Add(curProduct);
                idList.Add(curProduct.ProId);

                currentSession.AddValue(Session.SessionKeys.ProdList, proList);
                currentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, idList);

                retList.Add("Success");
                retList.Add(prodInfo);
                retList.Add(zmode);
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
                logger.Debug("(CombinePoInCarton)InputSN end, uutSn:" + inputSN);
            }
        }



        public ArrayList Save(string firstProID, string deliveryNo, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinePoInCarton)Save start, firstProID:" + firstProID);

            try
            {
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
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

                    string pn = "";
                    string modelstr = product.Model;
                    Model curModel = modelRep.Find(product.Model);
                    pn = curModel.GetAttribute("PN");

                    bool labelFlag = false;
                    if (!string.IsNullOrEmpty(pn) && pn.Length >= 6)
                    {
                        if (pn.Substring(5, 1) == "U"
                            || pn.Substring(5, 1) == "E")
                        {
                            labelFlag = true;
                        }
                    }
                    if (product.Model.Substring(0, 3) == "156"
                            || product.Model.Substring(0, 3) == "173"
                            || product.Model.Substring(0, 3) == "146"
                            || product.Model.Substring(0, 3) == "157"
                            || product.Model.Substring(0, 3) == "158"
                            || product.Model.Substring(0, 2) == "PO"
                            || product.Model.Substring(0, 2) == "2P"
                            || product.Model.Substring(0, 3) == "172"
                            || product.Model.Substring(0, 2) == "BC"
                            || product.Model.Substring(0, 2) == "JO"
                            || product.Model.Substring(0, 2) == "SF")
                    {
                        labelFlag = true;
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


                //a)	如果Remain Qty = 0，则提示用户：’Po:’ + @Delivery + ‘ is finished!’；
                //提示用户后，执行Reset（Reset 说明见下文）
                //b)	如果PCs in Carton > Remain Qty，则令[PCs in Carton] = Remain Qty
                string strEnd = "";
                int remainQty = totalQty - packedQty;
                if (remainQty == 0)
                {
                    //5.当页面选择的Delivery 已经结合满Product时，需要将Delivery.Status 更新为'87'
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(delivery.DeliveryNo);
                    ex = new FisException("PAK136", erpara);//’Po:’ + @Delivery + ‘ is finished!’
                    strEnd = ex.mErrmsg;

                    IUnitOfWork uof = new UnitOfWork();
                    delivery.Status = "87";
                    delivery.Udt = DateTime.Now;
                    delivery.Editor = currentSession.Editor;
                    deliveryRep.Update(delivery, uof);
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

        public ArrayList ReprintCartonLabel(string inputSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(CombinPoInCarton)ReprintLabel Start,"
                            + " [custSN]:" + inputSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);

            ArrayList retList = new ArrayList();
            try
            {
                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

                IProduct curProduct;

                curProduct = productRep.FindOneProductWithProductIDOrCustSNOrCarton(inputSN);
                if (curProduct == null)
                {
                    string vendorCT = inputSN;
                    ProductPart conf = new ProductPart();
                    conf.PartSn = inputSN;
                    IList<ProductPart> productList = productRep.GetProductPartList(conf);
                    if (productList.Count > 0)
                    {
                        curProduct = productRep.Find(productList[0].ProductID);
                    }
                    else
                    {
                        //b.InfoType = 'MBCT2' AND b.InfoValue = @VendorCT	
                        var mcond = new IMES.FisObject.PCA.MB.MBInfo();
                        mcond.InfoType = "MBCT2";
                        mcond.InfoValue = vendorCT;
                        IList<IMES.FisObject.PCA.MB.MBInfo> mbList = mbRep.GetPcbInfoByCondition(mcond);
                        if (mbList.Count > 0)
                        {
                            IList<IProduct> pList = productRep.GetProductListByPCBID(mbList[0].PCBID);
                            if (pList.Count > 0)
                            {
                                curProduct = pList[0];
                            }
                        }
                    }
                    if (curProduct == null)
                    {
                        if (inputSN.Length == 9)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(inputSN);
                            throw new FisException("CHK801", errpara);
                        }
                        else
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(inputSN);
                            throw new FisException("SFC002", errpara);
                        }
                    }
                }

                //d.	如果Product 尚未结合的Carton，则报告错误：“该Product 尚未Combine Carton，不能Reprint Carton Label!”
                if (string.IsNullOrEmpty(curProduct.CartonSN))
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(curProduct.ProId);
                    throw new FisException("PAK138", errpara);
                }

                //e.	如果输入的[Carton No] 在数据库(CartonStatus.CartonNo) 中不存在，则报告错误：“此Carton 不存在!”
                carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();
                CartonStatusInfo carConf = new CartonStatusInfo();
                carConf.cartonNo = curProduct.CartonSN;
                IList<CartonStatusInfo> carList = cartRep.GetCartonStatusInfo(carConf);
                if (carList.Count == 0)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(curProduct.ProId);
                    throw new FisException("PAK139", errpara);//此Carton 不存在!
                }

                //如果Product 非Frame Or TRO Or BaseModel Or SLICE 的话，需要报告错误：“Product is not Frame Or TRO Or BaseModel Or SLICE” 
                //SELECT @PN = Value FROM ModelInfo NOLOCk WHERE Model = @Model AND Name = 'PN'
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string pn = "";
                string noCarton = "";
                string modelstr = curProduct.Model;
                Model curModel = modelRep.Find(curProduct.Model);
                pn = curModel.GetAttribute("PN");
                noCarton = curModel.GetAttribute("NoCarton");

                bool labelFlag = false;
                if (!string.IsNullOrEmpty(pn) && pn.Length >= 6)
                {
                    if (pn.Substring(5, 1) == "U"
                        || pn.Substring(5, 1) == "E")
                    {
                        labelFlag = true;
                    }
                }
                if (curProduct.Model.Substring(0, 3) == "156"
                        || curProduct.Model.Substring(0, 3) == "173"
                        || curProduct.Model.Substring(0, 3) == "146"
                        || curProduct.Model.Substring(0, 3) == "157"
                        || curProduct.Model.Substring(0, 3) == "158"
                        || curProduct.Model.Substring(0, 2) == "PO"
                        || curProduct.Model.Substring(0, 2) == "2P"
                        || curProduct.Model.Substring(0, 3) == "172"
                        || curProduct.Model.Substring(0, 2) == "BC"
                        || curProduct.Model.Substring(0, 2) == "JO"
                        || curProduct.Model.Substring(0, 2) == "SF")
                {
                    labelFlag = true;
                }
                if (!labelFlag)
                {
                    FisException fe = new FisException("PAK133", new string[] { });  //Product is not Frame Or TRO Or BaseModel Or SLICE
                    throw fe;
                }

                string sessionKey = curProduct.ProId;


                var repository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();

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

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ReprintCombinPoInCarton.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, curProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "CombinInCarton");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                Delivery curDev = deliveryRep.Find(curProduct.DeliveryNo);

                string cQtyStr = (string)curDev.GetExtendedProperty("CQty");
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

                var log = new ReprintLog
                {
                    LabelName = currentSession.GetValue(Session.SessionKeys.PrintLogName).ToString(),
                    BegNo = currentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString(),
                    EndNo = currentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString(),
                    Descr = (string)currentSession.GetValue(Session.SessionKeys.PrintLogDescr),
                    Reason = (string)currentSession.GetValue(Session.SessionKeys.Reason),
                    Editor = editor
                };

                //当Delivery 的CQty 属性=1，但Delivery.Model 的NoCarton 属性(Model.InfoType = ‘NoCarton’)不存在，或者存在但<>’Y’ 时，也需要列印Carton Label
                string printflag = "N";
                if (cQty > 1 || (cQty == 1 && noCarton != "Y"))
                {
                    printflag = "Y";
                    IUnitOfWork uof = new UnitOfWork();
                    var rep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();
                    rep.Add(log, uof);
                    uof.Commit();
                }

                retList.Add(printList);
                retList.Add(0);
                retList.Add(curProduct.CartonSN);
                retList.Add(curProduct.DeliveryNo);
                retList.Add(curProduct.CUSTSN);
                retList.Add(cQty);
                retList.Add(printflag);

                return retList;

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
                logger.Debug("(CombinPoInCarton)ReprintLabel End,"
                                + " [custSN]:" + inputSN
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
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

        public string GetSysSetting(string name)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName(name);
                if (valueList.Count == 0)
                {
                    FisException ex;
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                return valueList[0];
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:" + name);
            }

        }

        public ArrayList GetSysSettingList(IList<string> nameList)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ArrayList retList = new ArrayList();

                foreach (string node in nameList)
                {
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName(node);
                    if (valueList.Count == 0)
                    {
                        retList.Add("");
                    }
                    else
                    {
                        retList.Add(valueList[0]);
                    }
                }
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:");
            }

        }

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 获取setting相关值，未得到则置default值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <param name="hostname"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public string GetSysSettingSafe(string name, string defValue, string hostname, string editor)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName(name);
                if (valueList.Count == 0)
                {
                    SysSettingInfo info = new SysSettingInfo();
                    info.name = name;
                    info.value = defValue;
                    info.description = "PCs in Carton";

                    partRepository.AddSysSettingInfo(info);

                    valueList.Add(defValue);
                }
                return valueList[0];
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
                logger.Debug("(CombinPoInCarton)GetSysSetting, name:" + name);
            }
        }

        public void SetSysSetting(string name, string value, string hostname, string editor)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                SysSettingInfo conf = new SysSettingInfo();
                SysSettingInfo info = new SysSettingInfo();

                info.value = value;
                info.description = "PCs in Carton";

                conf.name = name;

                partRepository.UpdateSysSettingInfo(info, conf);
                return;
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
                logger.Debug("(CombinPoInCarton)GetSysSetting, name:" + name);
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

                retList.Add(totalQty);
                retList.Add(cQty);
                retList.Add(packedQty);

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


        public void CheckFRUMBOA3(string mbsn, string usedmodel)
        {
            CommonImpl cmi = new CommonImpl();
            IList<ConstValueTypeInfo> lstConst = cmi.GetConstValueTypeListByType("RMAOA3CheckModel");
            bool needcheckoa3 = lstConst.Where(x => x.value == usedmodel).Any();
            if (needcheckoa3)
            {
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(mbsn);
                if (mb == null)
                {
                    throw new FisException("SFC001", new string[] { mbsn });
                }
                string mbct = (string)mb.GetExtendedProperty("MBCT");
                if (string.IsNullOrEmpty(mbct))
                {
                    throw new FisException("ICT024");
                }

                string strSQL = @"IF EXISTS(SELECT 1 from RMA_OA3Data where CT=@CT)
                                                         SELECT 1
                                                        ELSE
                                                         SELECT 0 ";
                SqlParameter sqlPara = new SqlParameter("@CT", SqlDbType.VarChar, 32);
                sqlPara.Direction = ParameterDirection.Input;
                sqlPara.Value = mbct;
                object data = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, sqlPara);
                if ((int)data == 0)
                {
                    throw new FisException("CQCHK50118", new string[] { mbct });
                }




            }

        }

        public void CheckPAQC(IProduct p)
        {
            CommonImpl cmi = new CommonImpl();
            bool noneedcheck = false;
            IList<ConstValueTypeInfo> lstConst = cmi.GetConstValueTypeListByType("RCTOCombinePOInCartonNoCheckQC");
            if (lstConst != null && lstConst.Count > 0)
            {
                noneedcheck = lstConst.Where(x => !string.IsNullOrEmpty(x.value) && (x.value == p.Model || x.value == p.Family)).Any(); 
            }
            if (noneedcheck)//维护机型或者Family不检查qcstatus
            {
                return;
            }
            IList<ProductQCStatus> qcsStatus = p.QCStatus;
            if (qcsStatus != null && qcsStatus.Count>0)
            {
                ProductQCStatus PQ = qcsStatus.Where(x => x.Type == "PAQC").OrderByDescending(y => y.Udt).First();
                if (PQ != null)
                {
                    switch (PQ.Status)
                    {
                        case "":
                            throw new FisException("PAK014", new string[] { });
                        case "8":
                            throw new FisException("PAK014", new string[] { });
                        case "B":
                            throw new FisException("PAK014", new string[] { });
                        case "C":
                            throw new FisException("PAK014", new string[] { });
                        case "A":
                            throw new FisException("PAK015", new string[] { });
                        case "9":
                            
                        default:
                            break;
                    }

                }
                else
                {
                    List<string> erpara = new List<string>();
                    erpara.Add(p.ProductID);
                    erpara.Add("PAQC");
                   throw new FisException("PAK051", erpara);    //QCStatus 中Product%1 的参数 %2 不存在！
                   
                }
            }
          
        }

        #endregion
    }
}
