/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: POD Label Check
 * UI:CI-MES12-SPEC-PAK-UI POD Label Check.docx 
 * UC:CI-MES12-SPEC-PAK-UC POD Label Check.docx                        
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-11-01   Chen Xu (eB1-4)     create
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Station.Interface.CommonIntf;
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
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// POD Label Check
    /// </summary>
    /// 
    
    public class PodLabelCheck: MarshalByRefObject,IPodLabelCheck
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
       
        #region IPODLabelCheck Members

        /// <summary>
        /// 根据刷入的Customer S/N,SFC,获得 Product ID,Customer P/N 信息
        /// Product ID: Product.ProductID;
        /// Customer P/N: Product.Model对应的Model.CustPN
        /// </summary>
        ProductModel IPodLabelCheck.InputCustSnOnCooLabel(string custsn, string pdLine, string editor, string station, string customer, out string custpn)
        {
            logger.Debug("(PodLabelCheckImpl)InputCustSNOnCooLabel start, custsn:" + custsn + "pdLine:" + pdLine + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
               
                var currentProduct = CommonImpl.GetProductByInput (custsn, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PodLabelCheck.xoml", "PodLabelCheck.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custsn);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
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


                ProductModel currentProductModel = new ProductModel();

                currentProductModel.CustSN = currentProduct.CUSTSN;
                currentProductModel.Model = currentProduct.Model;
                currentProductModel.ProductID = currentProduct.ProId;

                custpn = string.Empty;
                //custpn = currentProduct.ModelObj.CustPN;
                //if (String.IsNullOrEmpty(custpn))
                //{
                //    erpara.Add(custsn);
                //    ex = new FisException("CHK043", erpara);
                //    throw ex;
                //} //UC Revision: 7294: CustPN 修改为Model
                //Revision: 9810:	修改列印Config Label 的条件为非BT
                currentSession.AddValue(Session.SessionKeys.ifElseBranch, ""); 
                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                
                string printlabeltype1 = string.Empty; // Config Label
                string printlabeltype2 = string.Empty; // POD Label
                string delievery = string.Empty;    // currentDeliveryNo
                string pno = string.Empty;      // currentModel
                delievery = currentProduct.DeliveryNo;
                if (!currentProduct.IsBT)
                {
                    string BTRegId = iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");
                    string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                    string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                    if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                    {
                        //ITC-1360-1527:RegId 为'SNE'时，未能打印CONFIG LABEL
                        if ((BTRegId == "SCN" || BTRegId == "SAF" || BTRegId == "SNE" || BTRegId == "SCE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                        {
                            printlabeltype1 = "ConfigLabel";
                            currentSession.AddValue(Session.SessionKeys.ifElseBranch, printlabeltype1);
                            currentProductModel.CustSN = printlabeltype1;
                        }
                    }
                }
               
               
                //  Print =====如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====
                pno = currentProduct.Model;
                IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
                podLabelPartLst = ipartRepository.GetPODLabelPartListByPartNo(pno);
                if (podLabelPartLst.Count > 0)
                {
                    string number = "0123456789";
                    string modelbit = currentProduct.Model.Substring(6, 1);
                    if (!number.Contains(modelbit)) //Model的第7位不是数字
                    {
                        printlabeltype2 = "PODLabel";
                        currentSession.AddValue(Session.SessionKeys.ifElseBranch, printlabeltype2);  //如果列印PodLabel，则需要再记录一次ProductLog (Station = 'PD'，Line = 'POD Label Print')
                        currentProductModel.CustSN = printlabeltype2;
                    }
                }
                return currentProductModel;
            }
            catch (FisException e)
            {
                //logger.Error(e.mErrmsg, e);
                //throw new Exception(e.mErrmsg);
                logger.Error(e.mErrmsg, e);
                if (e.mErrcode == "CHK020") //序號已被刷入
                {
                    throw e;
                }
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PodLabelCheckImpl)InputCustSNOnCooLabel end, custsn:" + custsn + "pdLine:" + pdLine + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        string IPodLabelCheck.InputCustPnOnPodLabel(string productIdValue, string custSnOnCooValue, string custPnOnPodValue, string pdLine, string editor, string station, string customer)
        {
            logger.Debug("(PodLabelCheckImpl)Save start, productIdValue:" + productIdValue + "custSnOnCooValue:" + custSnOnCooValue + "custPnOnPodValue" + custPnOnPodValue + "pdLine" + pdLine + "editor:" + editor + "station:" + station + "customer" + customer);
            string ret = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = "";
            try
            {

                sessionKey = productIdValue;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                
                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;

                }

               else
                {
                    if (custSnOnCooValue == "")
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                    }
                    else
                    {
                        bool check = false;
                        var currentProduct = CommonImpl.GetProductByInput(productIdValue, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                        
                        if (custSnOnCooValue == currentProduct.MAC)
                        {
                            ret = "mac";
                            check = true;
                        }
                        if (check == false)
                        {
                            // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                            IList<IProductPart> productParts = new List<IProductPart>();
                            productParts = currentProduct.ProductParts;
                            if (productParts == null || productParts.Count <= 0)
                            {
                                //List<string> errpara = new List<string>();
                                //errpara.Add(productIdValue);
                                //throw new FisException("PAK038", errpara);  //该Product尚未绑定Part！
                            }
                            else
                            {
                                foreach (ProductPart iprodpart in productParts)
                                {
                                    if (iprodpart.BomNodeType == "AT")
                                    {
                                        IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                                        if (curPart.BOMNodeType == "AT")
                                        {
                                            if (custSnOnCooValue == iprodpart.PartSn)
                                            {
                                                ret = "tag";
                                                check = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (check == false)
                        {
                            //throw new FisException("CHK888", new string[] { });
                            return "checkwrong";
                        }
                        currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
            finally
            {
                logger.Debug("(PodLabelCheckImpl)Save end,productIdValue:" + productIdValue + "custSnOnCooValue:" + custSnOnCooValue + "custPnOnPodValue" + custPnOnPodValue + "pdLine" + pdLine + "editor:" + editor + "station:" + station + "customer" + customer);
                /*Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }*/
            }
        }

        void IPodLabelCheck.Cancel(string custsn)
        {
            try
            {
                logger.Debug("(PodLabelCheckImpl)Cancel start, custsn:" + custsn);

                var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
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
                logger.Debug("(PodLabelCheckImpl)Cancel end, custsn:" + custsn);
            }
        }

        #endregion
    }
}
