using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.BSamIntf;
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
using IMES.FisObject.PAK.Carton;
using System.Data;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pizza;
using IFAPrd = IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{   /// <summary>
    /// Combine Carton In DN
    /// station :
    /// 本站实现的功能：
    ///    
    ///     
    ///    
    /// </summary> 

    public class CombineCartonInDN_BIRCH:MarshalByRefObject,ICombineCartonInDN_BIRCH
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ArrayList InputCustSn(string firstSn, string custSn)
        {
            ArrayList retLst = new ArrayList();
            logger.Debug("(CombineCartonInDN_BIRCH)InputCustSn start, first custsn:" + firstSn + " custsn:" + custSn);
            try
            {
                // IMES.FisObject.FA.Product.IProduct
                CheckQCStatus(custSn);
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = firstSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);


                if (currentSession == null)
                { 
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                 
                    IMES.FisObject.FA.Product.IProduct product = CommonImpl.GetProductByInput(custSn, CommonImpl.InputTypeEnum.CustSN);
                    if (product==null)
                    {
                        erpara.Add(custSn);
                        ex = new FisException("CHK152", erpara);    //this Customer SN %1 is invalid , please rescan！
                        throw ex;
                    }
                    string firstModel = (string)currentSession.GetValue("FirstModelInCombineCarton");
                    if(!product.Model.Trim().Equals(firstModel.Trim()) )
                    {
                      throw new FisException("The input Model must be :" + firstModel);
                    }
                        
                    currentSession.AddValue(Session.SessionKeys.Product, product);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();

                    if (currentSession.Exception != null)
                    {
                        if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentSession.ResumeWorkFlow();
                        }
                        throw currentSession.Exception;
                    }
                  
                    S_BSam_Product sProduct = new S_BSam_Product();
                    sProduct.CustomerSN = product.CUSTSN;
                    sProduct.ProductID = product.ProId;
                    sProduct.Model = product.Model;
                    sProduct.Location = (string)product.GetAttributeValue("CartonLocation") ?? "";
                    sProduct.DeliveryNo = product.DeliveryNo;
                    retLst.Add(sProduct);
                   // retLst.Add(product.DeliveryNo);
                    return retLst;
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
                //logger.Debug("(CombineCartonInDN)InputFirstSN, firstSn:" + firstSn );
            }

        }
        private bool CheckIsMRP(string dn)
        {
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery dnObj = dnRep.GetDelivery(dn);
            string flag = (string)dnObj.GetExtendedProperty("Flag");
            MRPLabelDef mrpDef=  dnRep.GetMRPLabel(dn);
            return string.Compare(flag, "N") == 0 && mrpDef != null && !string.IsNullOrEmpty(mrpDef.IndiaPriceID);
           
        }
        private void CheckQCStatus(string sn)
        {
            IMES.FisObject.FA.Product.IProduct product = CommonImpl.GetProductByInput(sn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
            if (!string.IsNullOrEmpty(product.CartonSN))
            {
                throw new FisException("CQCHK1082", new string[] { product.CartonSN });
            }
            string paqcStatus = product.GetAttributeValue("PAQC_QCStatus") ?? "";
            switch (paqcStatus)
            {
                case "" :
                    throw new FisException("PAK014", new string[] { });
                case "8":
                case "B":
                case "C":
                    throw new FisException("PAK014", new string[] { });
                case "A":
                    throw new FisException("PAK015", new string[] { });
                default:
                    break;
            }
        }
        public ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer,string floor)
        {
            logger.Debug("(CombineCartonInDN_BIRCH)InputFirstCustSn start, custsn:" + firstSn + "pdLine:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            try
            {
                CheckQCStatus(firstSn);
                string sessionKey = firstSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(ExtendSession.SessionKeys.IsSameStation, false);     
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombineCartonInDN_BIRCH.xoml", "CombineCartonInDN_BIRCH.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, firstSn);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.Floor,floor);
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
                Product product = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                currentSession.AddValue("FirstModelInCombineCarton", product.Model);
                S_BSam_Product sProduct = new S_BSam_Product();

                sProduct.CustomerSN = product.CUSTSN;
                sProduct.ProductID = product.ProId;
                sProduct.Model = product.Model;
                sProduct.Location = (string)product.GetAttributeValue("CartonLocation") ?? "";
                sProduct.DeliveryNo = product.DeliveryNo;
                //(string)CurrentSession.GetValue("AllowUnpackCode") ?? "";
                Carton carton=(Carton)  currentSession.GetValue(Session.SessionKeys.Carton);
                S_BSam_Carton sCarton=new S_BSam_Carton();
                sCarton.CartonSN=carton.CartonSN;
                sCarton.ActualQty=carton.Qty.ToString();
                sCarton.FullQty=carton.FullQty.ToString();
                sCarton.PalletNo=carton.PalletNo;
                // IList<AvailableDelivery> selectedDNList = new List<AvailableDelivery>();
              //  CurrentSession.AddValue(ExtendSession.SessionKeys.AvailableDNList, selectedDNList);
                IList<AvailableDelivery> avaiDNLst = (IList<AvailableDelivery>)currentSession.GetValue(ExtendSession.SessionKeys.AvailableDNList);
                List<S_BSam_DN> sLstDN = new List<S_BSam_DN>();
                foreach (AvailableDelivery aDN in avaiDNLst)
                {
                    S_BSam_DN sDN = new S_BSam_DN();
                    sDN.DeliveryNo = aDN.DeliveryNo;
                    sDN.Model = aDN.Model;
                    sDN.ShipDate = aDN.ShipDate.ToString("yyyy-MM-dd") ;
                    sDN.Qty = aDN.Qty.ToString();
                    sDN.RemainQty = aDN.RemainQty.ToString();
                    sLstDN.Add(sDN);
                }
                bool isMRP = CheckIsMRP(product.DeliveryNo);
                string templatename = "";
                if (isMRP)
                {
                    IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    IList<string> docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(product.DeliveryNo.Substring(0, 10));
                    //SELECT @templatename = XSL_TEMPLATE_NAME 	FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                    if (docnumList.Count > 0)
                    {
                        IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label_Over Pack_Gift Box", docnumList[0]);
                        if (tempList.Count > 0)
                            templatename = tempList[0];
                    }
                }
                int qtyPerCarton=0;
                if (avaiDNLst.Count > 0)
                {
                    qtyPerCarton = avaiDNLst[0].QtyPerCarton;
                }
                retLst.Add(sProduct);
                retLst.Add(sCarton);
                retLst.Add(sLstDN);
                retLst.Add(qtyPerCarton);
                retLst.Add(isMRP);
                retLst.Add(templatename);
            //    retLst.Add(product.DeliveryNo);
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
                //logger.Debug("(CombineCartonInDN)InputFirstSN, firstSn:" + firstSn );
            }


        }
        public ArrayList Save(string firstSn, string cartonSn,string editor, IList<PrintItem> printItems)
        {
            logger.Debug("(CombineCartonInDN_BIRCH)Save Start,Key:" + firstSn);
           ArrayList retLst = new ArrayList();
            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = firstSn;
                string line = "";
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                line = currentSession.Line.Substring(0,1);
                currentSession.Exception = null;
                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                string createPDF=currentSession.GetValue("CreatePDF")!=null?currentSession.GetValue("CreatePDF").ToString():""; //if need print pdf ->"pdf"
                string locMsg = currentSession.GetValue("LocationMess") != null ? currentSession.GetValue("LocationMess").ToString() : "";
          //      IList<PrintItem> newPrintItem = new List<PrintItem>();
                
                // Get Edits Param
           //     IMES.FisObject.FA.Product.IProduct product = CommonImpl.GetProductByInput(firstSn, CommonImpl.InputTypeEnum.CustSN);
                string dn =(string)currentSession.GetValue(Session.SessionKeys.DeliveryNo);
                string templatename = "";
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                if (!string.IsNullOrEmpty(createPDF))
                {
                    IList<string> docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(dn.Substring(0, 10));
                    //SELECT @templatename = XSL_TEMPLATE_NAME 	FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                    if (docnumList.Count > 0)
                    {
                        //Box Ship Label_Tablet_Wholesale 
                         IList<AvailableDelivery> avaiDNLst = (IList<AvailableDelivery>)currentSession.GetValue(ExtendSession.SessionKeys.AvailableDNList);
                        int qtyPerCarton=0;
                         if (avaiDNLst.Count > 0)
                          {
                                qtyPerCarton = avaiDNLst[0].QtyPerCarton;
                          }
                         if (CheckIsMRP(dn) && qtyPerCarton > 1)
                         {
                             IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label_Over Pack_Wholesale", docnumList[0]);
                             if (tempList.Count > 0)
                                 templatename = tempList[0];
                         }
                         else
                         {
                             IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", docnumList[0]);
                             if (tempList.Count > 0)
                                 templatename = tempList[0];
                         }
                    }
                   
                    //foreach (PrintItem pi in printItems)
                    //{
                    //    if (pi.LabelType.ToUpper().IndexOf("SHIP TO") >= 0 || pi.LabelType.ToUpper().IndexOf("SHIPTO") >= 0)
                    //    { continue; }
                    //    else
                   //  { newPrintItem.Add(pi); }
                    //}

                }
             

                retLst.Add(dn);
                retLst.Add(locMsg);
                retLst.Add(createPDF);
                retLst.Add(templatename);
                retLst.Add((IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems));
                return retLst;
            }
          
            catch (FisException e)
            {
                ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                cartonRep.RollBackAssignCarton(cartonSn,editor);
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                deliveryRep.RollBackAssignBoxId(cartonSn);

                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                cartonRep.RollBackAssignCarton(cartonSn,editor);
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                deliveryRep.RollBackAssignBoxId(cartonSn);

                logger.Error(e.Message);
                throw new SystemException(e.Message);

            }
            finally
            {
                //logger.Debug("(CombineCartonInDN)InputFirstSN, firstSn:" + firstSn );
            }
        }

        public  ArrayList RePrintCartonLabel(string sn, string editor, string station,string customer, string reason,IList<PrintItem> printItems)
        {
            logger.Debug("(CombineCartonInDN)RePrintCartonLabel Start,Key:" + sn);
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
             FisException ex;
            try
            {
                CommonImpl cmi = new CommonImpl();
                IList<ConstValueInfo> lstConst = cmi.GetConstValueListByType("CTO Check", "Name").Where(x => x.value.Trim() != "").ToList(); 
                string line = "";
                string custsn = "";
                string dn = "";
                string flag = "";
                string descr = "";
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IFAPrd.IProduct>();
                IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                Carton objCarton=cartonRep.Find(sn);
                 if (objCarton == null)
                {
                    objCarton = cartonRep.GetCartonByBoxId(sn);
                    if (objCarton == null) // input = CUSTSN
                    {
                        IFAPrd.IProduct objPrd = productRep.GetProductByCustomSn(sn);
                        custsn = sn;
                        if (objPrd != null)
                        {
                            objCarton = cartonRep.Find(objPrd.CartonSN);
                        }

                    }

                }
           
                if (objCarton == null)
                {
                    throw new FisException("PAK084", new string[] { });
                }
                if (custsn == "")
                {
                    IList<CartonProduct> lstCartonPrd = objCarton.CartonProducts;
                    if (lstCartonPrd != null && lstCartonPrd.Count > 0)
                    {
                        string prdId = lstCartonPrd[0].ProductID;
                        IFAPrd.IProduct objPrd = productRep.Find(prdId);
                        line = objPrd.Status.Line.Substring(0, 1);
                        custsn = objPrd.CUSTSN;
                        dn = objPrd.DeliveryNo;
                     }
                }
                else
                {
                    IFAPrd.IProduct objPrd = productRep.GetProductByCustomSn(custsn);
                    line = objPrd.Status.Line.Substring(0, 1);
                    dn = objPrd.DeliveryNo;
                }
                Delivery dnObj = deliveryRepository.Find(dn);
                if (dnObj == null)
                { throw new FisException("CHK190", new string[] { dn }); }
                flag = (string)dnObj.GetExtendedProperty("Flag");
            
                //IList<CartonInfo> lstCartonInfo=objCarton.CartonInfos;
                //if (lstCartonInfo != null && lstCartonInfo.Count > 0)
                //{
                //    var infoList = (from p in lstCartonInfo
                //                    where p.InfoType == "PdfFileName"
                //                    select p.InfoValue).ToList();
                //    if (infoList .Count>0)
                //    {
                //        pdfFileName = infoList[0].ToString();
                //    }
                //}
                
             
         
                //Start Workflow
                string sessionKey = custsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo,dn);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "Tablet Carton Label");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, sn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, sn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, descr);        
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ReprintCartonLabel.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
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
                string templatename = "";
                if (flag == "N")
                {
                    IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    IList<string> docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(dn.Substring(0, 10));
                    //SELECT @templatename = XSL_TEMPLATE_NAME 	FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                    int qtyPerCarton = dnObj.DeliveryEx.QtyPerCarton;
                    if (docnumList.Count > 0)
                    {
                        if (CheckIsMRP(dn) && qtyPerCarton > 1)
                        {
                            IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label_Over Pack_Wholesale", docnumList[0]);
                            if (tempList.Count > 0)
                                templatename = tempList[0];
                        }
                        else
                        {
                            IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", docnumList[0]);
                            if (tempList.Count > 0)
                                templatename = tempList[0];
                        }
                    }
                
                }
             
                retLst.Add(custsn);
                retLst.Add(dn);
                retLst.Add(objCarton.CartonSN);
                retLst.Add(templatename);
                retLst.Add(line);
                retLst.Add(flag);
                retLst.Add((IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems));
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
                throw e;
            }
            finally
            {
                logger.Debug("(CombineCartonInDN)RePrintCartonLabel end, key:" + sn);
            }
        }
        public string GetInputMode(string name)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList = new List<string>();
                valueList = partRepository.GetValueFromSysSettingByName(name);
                string mode = "1";
                if (valueList.Count > 0 && (valueList[0] == "1" || valueList[0] == "2"))
                {
                   mode= valueList[0];
                }
                return mode;
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
                logger.Debug("(CombineCartonInDN)GetInputMode, name:" + name);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey, string cartonSn,string editor)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

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
                if (!string.IsNullOrEmpty(cartonSn))
                {
                    ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                    cartonRep.RollBackAssignCarton(cartonSn,editor);
                    IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    deliveryRep.RollBackAssignBoxId(cartonSn);
                }

                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

    }
}
