/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for Asset Tag Label Print Page
 * UI:CI-MES12-SPEC-PAK-UI Asset Tag Label Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC Asset Tag Label Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0725, Jessica Liu, 2012-3-7
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
//2012-7-16, Jessica Liu, 新需求：增加ESOP显示
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
//using System.Collections;
//using System.Collections.Generic;
using System.Linq;


namespace IMES.Station.Implementation
{

    public class AssetTagLabelPrint : MarshalByRefObject, IAssetTagLabelPrint
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members

        /// <summary>
        /// 检查CustomSN合法性并获得对应ProductID
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <returns>CustomSN和ProductID值</returns>
        public List<string> CheckCustomerSN(string custsn, string pdLine, string stationId, string editor, string customerId)
        {
/*//test，2011-10-18，测试流程是否通，需去掉========
List<string> ret1 = new List<string>();
ret1.Add(custsn);
ret1.Add(custsn);
return ret1;
//test，2011-10-18，测试流程是否通，需去掉========*/

            logger.Debug("(AssetTagLabelPrint)CheckCustomerSN start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            //ITC-1360-0725, Jessica Liu, 2012-3-7
            string customerSN = custsn;
            if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }

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

                    RouteManagementUtils.GetWorkflow(stationId, "AssetTagLabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //Session.AddValue(Session.SessionKeys.CustSN, custsn);
                    Session.AddValue(Session.SessionKeys.CustSN, customerSN);

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
                string productID = getProduct.ProId;

                string imgeFileFlag = "1";
                string imgeFile = "";
                string hasMN2 = (string)Session.GetValue(Session.SessionKeys.HasMN2);
                IList<string> imageFileList = (IList<string>)Session.GetValue(Session.SessionKeys.ImageFileList);
                if (hasMN2 == "Y")
                {
                    if (imageFileList != null && imageFileList.Count > 0)
                    {
                        imgeFileFlag = "0";
                        imgeFile = imageFileList[0];
                    }
                    else
                    {
                        imgeFileFlag = "2";
                    }
                }
               

                #region disable code
                //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
                //IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                //IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(getProduct.Model);
                //int errorFlag = 0;  //0=no error; 1=MN2错误; 2=No AST
                //string imageUrl = "";
                //IList<string> PNList = new List<string>();
                //string strMN2 = getProduct.GetModelProperty("MN2") as string;
                //if (string.IsNullOrEmpty(strMN2))
                //{
                //    errorFlag = 1;
                //}

                //if (errorFlag == 0)
                //{
                //    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                //    {
                //        IPart part6 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                //        if ((part6.BOMNodeType == "AT") && ((part6.Descr == "ATSN4") || (part6.Descr == "ATSN7") || (part6.Descr == "ATSN8")))
                //        {
                //            PNList.Add(part6.PN);
                //        }
                //    }

                //    if ((PNList != null) || (PNList.Count == 0))
                //    {
                //        for (int i = 1; i < PNList.Count; i++)
                //        {
                //            string t = PNList[i];
                //            int j = i;
                //            while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                //            {
                //                PNList[j] = PNList[j - 1];
                //                --j;
                //            }

                //            PNList[j] = t;
                //        }
                //    }
                //    else
                //    {
                //        errorFlag = 2;
                //    }

                //    imageUrl += strMN2;

                //    foreach (string pn2 in PNList)
                //    {
                //        imageUrl += pn2;
                //    }
                //}
                #endregion
                List<string> ret = new List<string>();
                ret.Add(custsn);
                ret.Add(productID);
                //2012-7-16, Jessica Liu, 新需求：增加ESOP显示
                //ret.Add(errorFlag.ToString());
                //ret.Add(imageUrl);
                ret.Add(imgeFileFlag);
                ret.Add(imgeFile);
                return ret;
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
                logger.Debug("(AssetTagLabelPrint)CheckCustomerSN end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }

        /// <summary>
        /// 打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        public IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string astCodeList)
        {
            logger.Debug("(AssetTagLabelPrint)Print start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

/*//test，2011-10-18，测试流程是否通，需去掉========
IProduct currentProduct = new Product("testproduct");
currentProduct.Model = "asset tag label test";
currentProduct.CUSTSN = "customerSN 111";
string prodId = currentProduct.ProId;

FisException ex;
List<string> erpara = new List<string>();
//test，2011-10-18，测试流程是否通，需去掉========*/
// * test，2011-10-18，测试流程是否通，需放开========    
			string customerSN = custsn;
			if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }
            
            var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
            string prodId = currentProduct.ProId;

            FisException ex;
            List<string> erpara = new List<string>();
            //==================================
            if (string.IsNullOrEmpty(prodId))
            {
                Cancel(custsn);

                //List<string> errpara = new List<string>();
                erpara.Add(custsn);
                throw new FisException("SFC011", erpara);
            }
//test，2011-10-18，测试流程是否通，需放开======== * /               

            // string temp = string.Empty;
            try
            {
                string sessionKey = custsn;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.Product, currentProduct);
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, customerSN);

                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);

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

                IList<PrintItem> returnList = this.getPrintList(sessionInfo);

                IList<AstDefineInfo> astDefineInfoList = (IList<AstDefineInfo>)sessionInfo.GetValue(Session.SessionKeys.NeedCombineAstDefineList);
                IList<string> codeList = astDefineInfoList.Where(y => y.NeedPrint == "Y")
                                                    .Select(x => x.AstCode).Distinct().ToList();

                //Check ATSN5 不打印 
                //codeList = codeList.Where(x => x != "ATSN5").ToList();
                if (codeList.Count == 0)
                {
                    throw new FisException("CQCHK1091", new List<string> { custsn, string.Join(",", astDefineInfoList.Select(x => x.AstCode).Distinct().ToArray()) });
                }

                astCodeList = string.Join(",", codeList.ToArray());
                return returnList;
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
                logger.Debug("(AssetTagLabelPrint)Print end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// 打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="prodid">ErrorFlag、imageURL</param>
        public IList<PrintItem> RePrint(string custsn, string pdLine, string stationId, string editor, string customerId, string reason, IList<PrintItem> printItems, out string ErrorFlag, out string imageURL,out string astCodeList)
        {
            logger.Debug("(AssetTagLabelPrint)RePrint start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            //根据customer SN获得product信息
/*//test，2011-10-18，测试流程是否通，需去掉========
IProduct currentProduct = new Product("testproduct");
currentProduct.Model = "asset tag label test";
currentProduct.CUSTSN = "customerSN 111";
string prodId = currentProduct.ProId;

FisException ex;
List<string> erpara = new List<string>();
//test，2011-10-18，测试流程是否通，需去掉========*/
// * test，2011-10-18，测试流程是否通，需放开========
			string customerSN = custsn;
			if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }
            
			var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
			string prodId = currentProduct.ProId;

			FisException ex;
			List<string> erpara = new List<string>();
			//==================================
			if (string.IsNullOrEmpty(prodId))
			{
				//List<string> errpara = new List<string>();
				erpara.Add(customerSN);
				throw new FisException("SFC011", erpara);
			}
//test，2011-10-18，测试流程是否通，需放开======== * /               
            
			// string temp = string.Empty;
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
                    RouteManagementUtils.GetWorkflow(stationId, "AssetTagLabelReprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.Reason,reason);
                    Session.AddValue(Session.SessionKeys.CustSN, customerSN);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    
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

                #region disable code
                ////2012-7-16, Jessica Liu, 新需求：增加ESOP显示
                //int errorFlag = 0;  //0=no error; 1=MN2错误; 2=No AST
                //string imageUrl = "";
                //IList<string> PNList = new List<string>();
                //IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                //IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                //string strMN2 = currentProduct.GetModelProperty("MN2") as string;
                //if (string.IsNullOrEmpty(strMN2))
                //{
                //    errorFlag = 1;
                //}

                //if (errorFlag == 0)
                //{
                //    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                //    {
                //        IPart part6 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                //        if ((part6.BOMNodeType == "AT") && ((part6.Descr == "ATSN4") || (part6.Descr == "ATSN7") || (part6.Descr == "ATSN8")))
                //        {
                //            PNList.Add(part6.PN);
                //        }
                //    }

                //    if ((PNList != null) || (PNList.Count == 0))
                //    {
                //        for (int i = 1; i < PNList.Count; i++)
                //        {
                //            string t = PNList[i];
                //            int j = i;
                //            while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                //            {
                //                PNList[j] = PNList[j - 1];
                //                --j;
                //            }

                //            PNList[j] = t;
                //        }
                //    }
                //    else
                //    {
                //        errorFlag = 2;
                //    }

                //    imageUrl += strMN2;

                //    foreach (string pn2 in PNList)
                //    {
                //        imageUrl += pn2;
                //    }
                //}
                //imageURL = imageUrl;
                //ErrorFlag = errorFlag.ToString();
                #endregion

                ErrorFlag = "1";
                 imageURL = "";
                string hasMN2 = (string)Session.GetValue(Session.SessionKeys.HasMN2);
                IList<string> imageFileList = (IList<string>)Session.GetValue(Session.SessionKeys.ImageFileList);
                if (hasMN2 == "Y")
                {
                    if (imageFileList != null && imageFileList.Count > 0)
                    {
                        ErrorFlag = "0";
                        imageURL = imageFileList[0];
                    }
                    else
                    {
                        ErrorFlag = "2";
                    }
                }
                
                IList<PrintItem> returnList = this.getPrintList(Session);

                IList<AstDefineInfo> astDefineInfoList = (IList<AstDefineInfo>)Session.GetValue(Session.SessionKeys.NeedCombineAstDefineList);
                IList<string> codeList = astDefineInfoList.Where(y => y.NeedPrint == "Y")
                                                    .Select(x => x.AstCode).Distinct().ToList();

                //Check ATSN5 不打印 
                //codeList = codeList.Where(x => x != "ATSN5").ToList();
                if (codeList.Count == 0)
                {
                    throw new FisException("CQCHK1091", new List<string> { custsn, string.Join(",", astDefineInfoList.Select(x => x.AstCode).Distinct().ToArray()) });
                }

                astCodeList = string.Join(",", codeList.ToArray());

                return returnList;
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
                logger.Debug("(AssetTagLabelPrint)RePrint end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

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