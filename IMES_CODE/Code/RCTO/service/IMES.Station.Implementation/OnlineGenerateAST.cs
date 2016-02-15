/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Implementation for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/2/6 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/2/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0494, Jessica Liu, 2012-2-17
* ITC-1360-1775, Jessica Liu, 2012-5-2
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;


namespace IMES.Station.Implementation
{

    public class OnlineGenerateAST : MarshalByRefObject, IOnlineGenerateAST
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
            logger.Debug("(OnlineGenerateAST)CheckCustomerSN start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                //ITC-1360-0090  Jessica Liu,2012-1-21
                string customerSN = custsn.Trim();
                /* 2012-2-6，for UC Change
                if (customerSN.Length == 11)
                {
                    if (customerSN.Substring(0, 3) == "SCN")
                    {
                        customerSN = customerSN.Substring(1, 10);
                    }
                }

                string strNoEmptySN = custsn.Trim();
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var info = productRepository.GetProductByCustomSn(customerSN);
                strNoEmptySN = strNoEmptySN.ToUpper();
                bool flag = false;

                if (info != null)
                {
                    if (info.ProId != null && !info.ProId.Trim().Equals(string.Empty))//customersn
                    {
                        if (strNoEmptySN.Length == 10)
                        {
                            if (strNoEmptySN.Substring(0, 2) == "CN")
                            {
                                flag = true;
                            }
                        }
                        else if (strNoEmptySN.Length == 11)
                        {
                            if (strNoEmptySN.Substring(0, 3) == "SCN")
                            {
                                flag = true;
                            }
                        }

                    }
                    else
                    {
                        //Product的id不存在（说明ProdID/Cust SN控件输入的为ProdID）？？？需要处理吗

                    }

                    if (flag == false)
                    {
                        erpara.Add(custsn);
                        throw new FisException("PAK011", erpara);
                    }
                }
                */

                /* ITC-1360-0090  Jessica Liu,2012-1-21
                string customerSN = custsn;
                if (customerSN.Length == 11)
                {
                    if (customerSN.Substring(0, 3) == "SCN")
                    {
                        customerSN = customerSN.Substring(1, 10);
                    }
                }
                */
            
                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

                string prodId = currentProduct.ProId;
                string cpqsno = currentProduct.CUSTSN; 
                string model = currentProduct.Model;

                if (string.IsNullOrEmpty(prodId))
                {
                    erpara.Add(custsn);
                    throw new FisException("SFC002", erpara);
                }

                if (String.IsNullOrEmpty((string)currentProduct.GetModelProperty("Cust")))
                {
                    erpara.Add(custsn);
                    throw new FisException("CHK205", erpara);   
                }

                bool isBinded = false;
                IList<string> ATPNList = new List<string>();
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);

                /* 2012-7-23, Jessica Liu, 新需求：去掉该部分
                //2012-7-16, Jessica Liu, 新需求：增加AT1的提示
                bool isAT1Exist = false;
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart temppart = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                    if ((temppart.BOMNodeType == "AT") && (temppart.Descr == "ATSN1"))
                    {
                        isAT1Exist = true;
                        break;
                    }
                }
                //报错："请去Combine AST 结合AT"
                if (isAT1Exist == true)
                {
                    erpara.Add(custsn);
                    throw new FisException("CHK913", erpara);
                }
                */

                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    //UC变更, Jessica Liu, 2012-4-16
                    //if ((part.BOMNodeType == "AT") && (part.Descr == "ATSN3"))
                    //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                    //if ((part.BOMNodeType == "AT") && ((part.Descr == "ATSN3") || (part.Descr == "ATSN5") || (part.Descr == "ATSN7")))
                    if ((part.BOMNodeType == "AT") && ((part.Descr == "ATSN3") || (part.Descr == "ATSN5")))
                    {
                        ATPNList.Add(part.PN);  //插入AT PN列表
                        //2012-4-9
                        //break;
                    }
                }

                string AST = "";
                foreach (IProductPart tempProductPart in currentProduct.ProductParts)
                {
                    foreach (string pn in ATPNList)
                    {
                        if (tempProductPart.PartID == pn)
                        {
                            isBinded = true;
                            AST = tempProductPart.PartSn;
                            break;
                        }
                    }
                }

                if (isBinded == true)
                {
                    /* ITC-1360-0494, Jessica Liu, 2012-2-17
                    string message = "此Product已和资产标签" + AST + "结合";
                    throw new FisException(message); 
                    */
                    throw new FisException("CHK242", new string[] { AST });
                }

                //2012-4-9
                bool canInput = false;
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part2 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                    //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                    //if ((part2.BOMNodeType == "PP") || ((part2.BOMNodeType == "AT") && ((part2.Descr == "ATSN3") || (part2.Descr == "ATSN5" || (part2.Descr == "ATSN7")))))
                    if ((part2.BOMNodeType == "PP") || ((part2.BOMNodeType == "AT") && ((part2.Descr == "ATSN3") || (part2.Descr == "ATSN5"))))
                    {
                        canInput = true;

                        break;
                    }
                }

                //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                ////报错：“非AT3\5\7和PP类，不能刷入此页面”
                //报错：“非AT3\5和PP类，不能刷入此页面”
                if (canInput == false)
                {
                    erpara.Add(custsn);
                    throw new FisException("CHK091", erpara);  
                }

                bool canCheck = false;
                bool needToCombineAST = false;
                bool needToCombineAST2 = false;
                string strToCombineAST = "";
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part3 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                    if ((part3.BOMNodeType == "AT") && (part3.Descr == "ATSN1"))
                    {
                        canCheck = true;

                        break;
                    }
                }

                if (canCheck == true)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part4 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                        //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                        //if ((part4.BOMNodeType == "PP") || ((part4.BOMNodeType == "AT") && ((part4.Descr == "ATSN3") || (part4.Descr == "ATSN5" || (part4.Descr == "ATSN7")))))
                        if ((part4.BOMNodeType == "PP") || ((part4.BOMNodeType == "AT") && ((part4.Descr == "ATSN3") || (part4.Descr == "ATSN5"))))
                        {
                            needToCombineAST = true;

                            break;
                        }
                    }

                    if (needToCombineAST == true)
                    {
                        strToCombineAST = "TRUE";
                    }
                    else
                    {
                        strToCombineAST = "FALSE";
                    }

                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part5 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                        //2012-7-13, Jessica Liu, 新需求：去除ATSN7的产生
                        //if ((part5.BOMNodeType == "PP") || ((part5.BOMNodeType == "AT") && ((part5.Descr == "ATSN3") || (part5.Descr == "ATSN5" || (part5.Descr == "ATSN7")))))
                        if ((part5.BOMNodeType == "PP") || ((part5.BOMNodeType == "AT") && ((part5.Descr == "ATSN3") || (part5.Descr == "ATSN5"))))
                        {
                            needToCombineAST2 = true;

                            break;
                        }
                    }

                    //报错：“需去Combine AST，不能刷此页面”
                    if (needToCombineAST2 == false)
                    {
                        erpara.Add(custsn);
                        throw new FisException("CHK092", erpara);
                    }
                }

                //2012-4-10, for image
                int errorFlag = 0;  //0=no error; 1=MN2错误; 2=No AST
                string imageUrl = "";
                IList<string> PNList = new List<string>();
                string strMN2 = currentProduct.GetModelProperty("MN2") as string;
                if (string.IsNullOrEmpty(strMN2))
                {
                    errorFlag = 1;
                }

                if (errorFlag == 0)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part6 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                        if ((part6.BOMNodeType == "PP") || ((part6.BOMNodeType == "AT") && ((part6.Descr == "ATSN1") || (part6.Descr == "ATSN2" || (part6.Descr == "ATSN3")))))
                        {
                            PNList.Add(part6.PN);  
                        }
                    }

                    if ((PNList != null) || (PNList.Count == 0))
                    {
                        for (int i = 1; i < PNList.Count; i++)
                        {
                            string t = PNList[i];
                            int j = i;
                            while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                            {
                                PNList[j] = PNList[j - 1];
                                --j;
                            }

                            PNList[j] = t;
                        }
                    }
                    else
                    {
                        errorFlag = 2;
                    }

                    imageUrl += strMN2;

                    foreach (string pn2 in PNList)
                    {
                        imageUrl += pn2;
                    }
                }


                List<string> ret = new List<string>();
                ret.Add(prodId);
                ret.Add(cpqsno);
                ret.Add(model);
                //2012-4-9
                ret.Add(strToCombineAST);
                //2012-4-10, for image
                ret.Add(errorFlag.ToString());
                ret.Add(imageUrl);

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
                logger.Debug("(OnlineGenerateAST)CheckCustomerSN end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
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
        /// <param name="ast">AST</param>
        public IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId, IList<PrintItem> printItems, out string ast)
        {
            logger.Debug("(OnlineGenerateAST)Print start, inputCustsn:" + custsn + " pdLine:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            
            //2011-10-23,11位处理提取到这里
            string customerSN = custsn;
            /* 2012-2-6,for UC change
            if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }
            */

            var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
            string prodId = currentProduct.ProId;
            customerSN = currentProduct.CUSTSN;

            FisException ex;
            List<string> erpara = new List<string>();

            if (string.IsNullOrEmpty(prodId))
            {                
                erpara.Add(customerSN);
                throw new FisException("SFC011", erpara);
            }

            try
            {
                string sessionKey = customerSN;
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
                    RouteManagementUtils.GetWorkflow(stationId, "OnlineGenerateASTPrint.xoml", "onlinegenerateastprint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.CustSN, customerSN);
                    Session.AddValue("IsCDSI", currentProduct.IsCDSI);
                    //Jessica Liu, 2012-4-16
                    //Session.AddValue("DESCR", "ATSN3");
                    Session.AddValue("ProdidOrCustsn", custsn);
        
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

                //2012-5-2
                //ast = (string)Session.GetValue(Session.SessionKeys.PrintLogEndNo);
                ast = (string)Session.GetValue("ASTInfo");

                IList<PrintItem> returnList = this.getPrintList(Session);
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
                logger.Debug("(OnlineGenerateAST)Print end, inputSn:" + custsn + " pdLine:" + pdLine + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// 重新打印Asset Tag Label
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        /// <param name="prodid">Product ID、ErrorFlag、imageURL</param>
        public IList<PrintItem> RePrint(string custsn, string pdLine, string stationId, string editor, string customerId, string reason, IList<PrintItem> printItems, out string prodid, out string ErrorFlag, out string imageURL)//ast)
        {
            logger.Debug("(OnlineGenerateAST)RePrint start, inputCustsn:" + custsn + " pdLine:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string customerSN = custsn.Trim();
            /* 2012-2-6,for UC change
            if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }

            string strNoEmptySN = custsn.Trim();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var info = productRepository.GetProductByCustomSn(customerSN);
            strNoEmptySN = strNoEmptySN.ToUpper();
            bool flag = false;

            if (info != null)
            {
                if (info.ProId != null && !info.ProId.Trim().Equals(string.Empty))//customersn
                {
                    if (strNoEmptySN.Length == 10)
                    {
                        if (strNoEmptySN.Substring(0, 2) == "CN")
                        {
                            flag = true;
                        }
                    }
                    else if (strNoEmptySN.Length == 11)
                    {
                        if (strNoEmptySN.Substring(0, 3) == "SCN")
                        {
                            flag = true;
                        }
                    }

                }
                else
                {
                    //Product的id不存在（说明ProdID/Cust SN控件输入的为ProdID）？？？需要处理吗
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }

            //FisException ex;
            //List<string> erpara = new List<string>();
            if (flag == false)
            {
                erpara.Add(custsn);
                throw new FisException("PAK011", erpara);   
            }
            */

            /*
            string customerSN = custsn;
            if (customerSN.Length == 11)
            {
                if (customerSN.Substring(0, 3) == "SCN")
                {
                    customerSN = customerSN.Substring(1, 10);
                }
            }
            */
            
            var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

            string prodId = currentProduct.ProId;
           
            if (string.IsNullOrEmpty(prodId))
            {
                erpara.Add(custsn);
                throw new FisException("SFC002", erpara);
            }

            try
            {
                string sessionKey = customerSN;
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
                    RouteManagementUtils.GetWorkflow(stationId, "OnlineGenerateASTReprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.CustSN, customerSN);
                    Session.AddValue("ProdidOrCustsn", customerSN);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

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

                //ast = (string)Session.GetValue(Session.SessionKeys.PrintLogEndNo);
                prodid = prodId;

                //ITC-1360-1775, Jessica Liu, 2012-5-2
                int errorFlag = 0;  //0=no error; 1=MN2错误; 2=No AST
                string imageUrl = "";
                IList<string> PNList = new List<string>();
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                string strMN2 = currentProduct.GetModelProperty("MN2") as string;
                if (string.IsNullOrEmpty(strMN2))
                {
                    errorFlag = 1;
                }

                if (errorFlag == 0)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part6 = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;

                        if ((part6.BOMNodeType == "PP") || ((part6.BOMNodeType == "AT") && ((part6.Descr == "ATSN1") || (part6.Descr == "ATSN2" || (part6.Descr == "ATSN3")))))
                        {
                            PNList.Add(part6.PN);
                        }
                    }

                    if ((PNList != null) || (PNList.Count == 0))
                    {
                        for (int i = 1; i < PNList.Count; i++)
                        {
                            string t = PNList[i];
                            int j = i;
                            while ((j > 0) && string.Compare(PNList[j - 1], t, false) > 0)
                            {
                                PNList[j] = PNList[j - 1];
                                --j;
                            }

                            PNList[j] = t;
                        }
                    }
                    else
                    {
                        errorFlag = 2;
                    }

                    imageUrl += strMN2;

                    foreach (string pn2 in PNList)
                    {
                        imageUrl += pn2;
                    }
                }
                imageURL = imageUrl;
                ErrorFlag = errorFlag.ToString();

                IList<PrintItem> returnList = this.getPrintList(Session);
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
                logger.Debug("(OnlineGenerateAST)RePrint end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
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
