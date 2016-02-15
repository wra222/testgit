/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0013 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
using System.Linq; 

namespace IMES.Station.Implementation
{

    public class SNCheck : MarshalByRefObject, ISNCheck
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members


		/// <summary>
        /// SN Check初次输入SN处理
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>ProductID</returns>
        public String InputCustSNOnProduct(string custsn, string pdLine, string editor, string stationId, string customerId)
		{
/*//test，2011-10-18，测试流程是否通，需删除========
string ret1 = custsn + " test";
return ret1;
//test，2011-10-18，测试流程是否通，需删除========*/

            logger.Debug("(SNCheck)InputCustSNOnProduct start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = custsn;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "SNCheck.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.CustSN, custsn);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    
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

                Product getProduct = (Product)Session.GetValue(Session.SessionKeys.Product);
                string ret = getProduct.ProId;
    
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
                logger.Debug("(SNCheck)InputCustSNOnProduct end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }


		/// <summary>
        /// SN Check第二次输入SN处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void InputCustSNOnPizza(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
        {
/*//test，2011-10-24，测试流程是否通，需去掉========
IProduct currentProduct = new Product("testproduct");
currentProduct.Model = "asset tag label test";
currentProduct.CUSTSN = "customerSN 111";
string prodId = currentProduct.ProId;
//test，2011-10-24，测试流程是否通，需去掉========*/

            logger.Debug("(SNCheck)InputCustSNOnPizza start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

			FisException ex;
			List<string> erpara = new List<string>();
			
            try
            {
                string sessionKey = custsnOnProduct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, custsnOnPizza);
                    
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

				return;
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
                logger.Debug("(SNCheck)InputCustSNOnPizza end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// SN Check第二次输入SN处理，返回PAQC以及ALC/NO-ALC
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SN Info</returns>
        public string InputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
        {
/*//2011-10-24，测试用，须删除
string ret1 = "";
if (custsnOnPizza.Substring(0, 1) == "P")
{
    ret1 += " NO-ALC";
}
else   
{
    ret1 += "ALC";
}
return ret1;
//2011-10-24，测试用，须删除*/

            logger.Debug("(SNCheck)InputCustSNOnPizzaReturn start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

            /*//test，2011-10-18，测试流程是否通，需去掉========
            IProduct currentProduct = new Product("testproduct");
            currentProduct.Model = "asset tag label test";
            currentProduct.CUSTSN = "customerSN 111";
            string prodId = currentProduct.ProId;
            //test，2011-10-18，测试流程是否通，需去掉========*/

            FisException ex;
            List<string> erpara = new List<string>();

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);

            try
            {
                string sessionKey = custsnOnProduct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, custsnOnPizza);

                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                string ret = "";
                if (custsnOnPizza.Substring(0, 1) == "P")
                {
                    ret = " NO-ALC";
                }
                else    
                {
                    ret = "ALC";
                }

                IList<ProductQCStatus> qcStatusList = currentProduct.QCStatus;
                var printFlag = false;
                if (qcStatusList.Count > 0)
                {
                    ProductQCStatus earlierQCStatus = qcStatusList.First<ProductQCStatus>();
                    foreach (ProductQCStatus tempQCStatus in qcStatusList)
                    {
                        if (tempQCStatus.Udt.CompareTo(earlierQCStatus.Udt) >= 0)
                        {
                            earlierQCStatus = tempQCStatus;
                            if ((tempQCStatus.Status == "8") || (tempQCStatus.Status == "9") || (tempQCStatus.Status == "A"))
                            {
                                printFlag = true;
                            }
                            else
                            {
                                printFlag = false;
                            }
                        }
                    }
                }

                if (printFlag == true)
                {
                    ret = ret + "  PAQC";
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
                logger.Debug("(SNCheck)InputCustSNOnPizzaReturn end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// SN Check第二次输入SN后判断是否一致的处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void CheckTwoSNIdentical(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(SNCheck)CheckTwoSNIdentical start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);

            try
            {
                string customerSNOnProduct = custsnOnProduct.Trim();
                string customerSNOnPizza = custsnOnPizza.Trim();

                if ((customerSNOnPizza.Length >= 10) && (customerSNOnProduct.Length >= 10))
                {
                    string SNOnPizzaLastTen = customerSNOnPizza.Substring(customerSNOnPizza.Length - 10, 10);
                    string SNOnProductLastTen = customerSNOnProduct.Substring(customerSNOnProduct.Length - 10, 10);

                    if (SNOnPizzaLastTen != SNOnProductLastTen)
                    {
                        List<string> errpara = new List<string>();

                        errpara.Add(custsnOnProduct);

                        throw new FisException("CHK207", errpara);
                    }
                }
                else
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custsnOnProduct);

                    throw new FisException("CHK207", errpara);
                }


                var hasALC = false;

                if (currentProduct.Model != "PC4941AAAAAY")
                {
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                        if ((part.Descr == "ALC") && (part.BOMNodeType == "PL"))
                        {
                            hasALC = true;

                            break;
                        }
                    }
                }

                if (hasALC == true)
                {
                    if (customerSNOnPizza.Substring(0, 1) != "A")
                    {
                        List<string> errpara = new List<string>();

                        errpara.Add(custsnOnProduct);

                        throw new FisException("CHK207", errpara);
                    }
                }
                else
                {
                    if (customerSNOnPizza.Substring(0, 1) != "P")
                    {
                        List<string> errpara = new List<string>();

                        errpara.Add(custsnOnProduct);

                        throw new FisException("CHK207", errpara);
                    }
                }

                return;
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
                logger.Debug("(SNCheck)CheckTwoSNIdentical end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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

         #endregion
    }
}
