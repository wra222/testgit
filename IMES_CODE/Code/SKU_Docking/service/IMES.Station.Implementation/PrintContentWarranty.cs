/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print 业务流程
* UI:CI-MES12-SPEC-PAK-UI Content & Warranty Print.docx –2011/10/13 
* UC:CI-MES12-SPEC-PAK-UC Content & Warranty Print.docx –2011/10/13            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-08   Du.Xuan               Create  
* ITC-1360-0769 Part.PartType判断变更为Part.Descr
* ITC-1360-0770 非workflow异常，remove session
* ITC-1360-0777 BOM 中有资产标签，并且内销返回Message信息显示
* ITC-1360-0778 返回正确Model.CustPN
* ITC-1360-1770 对Name=”WRNT”的记录为null增加保护
* Known issues:
* TODO：
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Collections;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Route;
using IMES.Infrastructure.Extend;
using IMES.Common;


namespace IMES.Station.Implementation
{
    public class PrintContentWarranty : MarshalByRefObject, IPrintContentWarranty
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        ArrayList IPrintContentWarranty.InputCustomerSN(string customerSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(PrintContentWarrantyImpl)InputCustomerSN start, customerSN:" + customerSN
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {
                string assetCheck = "";
                string assetMessage = "";
                ArrayList retList = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PrintContentWarranty.xoml", "PrintContentWarranty.rules", out wfName, out rlName);
                    //RouteManagementUtils.GetWorkflow(station, "104KPPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                //===============================================================================
                //Get infomation
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                
                Delivery curDelivery = new Delivery();
                if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                {
                    curDelivery = DeliveryRepository.Find(curProduct.DeliveryNo);
                }
                //------Modify Kaisheng 2012/07/21
                //5A.不需要打印Warranty Card
                //Model<>'PC47011QM03Y'且BOM中不存在BomNodeType='PR' and Descr ='Warranty Card'的part
                //FlatBOM curBOM = (FlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                IHierarchicalBOM curBOM = null;
                Boolean printFlag = false;
                curBOM = bomRepository.GetHierarchicalBOMByModel(curProduct.Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();

                if (curProduct.Model != "PC47011QM03Y")
                {
                    //Boolean printFlag = false;
                    bomNodeLst = curBOM.FirstLevelNodes;
                    if (bomNodeLst != null && bomNodeLst.Count > 0)
                    {
                        foreach (IBOMNode ibomnode in bomNodeLst)
                        {
                            IPart currentPart = ibomnode.Part;
                            if (currentPart.BOMNodeType == "PR" && currentPart.Descr == "Warranty Card")
                            {
                                printFlag = true;
                                break;
                            }
                        }
                    }

                    //currentSession.AddValue("WarrantyPrint", printFlag);
                }
                currentSession.AddValue("WarrantyPrint", printFlag);
                //------end 2012/07/21------------

                //6. BOM 中有资产标签，并且内销，才需要Check
                //select b.Tp,Type,Message,Sno1 from Special_Det a,Special_Maintain b where a.SnoId=@Productid 
                //and b.Type=a.Tp and b.SWC=”8D” order by Type,b.Tp
                //得到的Tp=”C”，表示需要check Ast SN，且Sno1为Asset Tag SN(目前只考虑得到一条记录的情况)
                //弹出的对话框信息是记录中的Message
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<SpecialCombinationInfo> infoList = productRepository.GetSpecialDetSpecialMaintainInfoList(curProduct.ProId, "8D");

                foreach (SpecialCombinationInfo infoNode in infoList)
                {
                    if (infoNode.maintainInfo.tp.Trim() == "C")
                    {
                        assetCheck = infoNode.detInfo.sno1;
                        assetMessage = infoNode.maintainInfo.message;
                    } 
                }

                //10. 判断是否需要打印Configuration Label   
                //Unit绑定的DN对应的RegId='SCN' 且  ShipTp='CTO'时，需要打印label
                Boolean conPrintFlag = false;
                //打印配置标签优先检查SIE维护的料号，如果没匹配到再检查DN.
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("PrintContentPartNoList");
                IList<IBOMNode> NodeLst = curBOM.FirstLevelNodes;
                bool bCompareBios = false;
                if (NodeLst != null && NodeLst.Count > 0 && lstConst != null && lstConst.Count>0)
                {
                    foreach (IBOMNode ibomnode in NodeLst)
                    {
                        var partLabelList = (from p in lstConst
                                             where p.value == ibomnode.Part.PN
                                             select p.value).ToList();
                        if (partLabelList != null && partLabelList.Count > 0)
                        {
                            bCompareBios = true;
                            conPrintFlag = true;
                            break;
                        }
                    }
                }
                if (!bCompareBios)
                {
                    if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                    {
                        string reg = (string)curDelivery.GetExtendedProperty("RegId");
                        if (reg != null && reg.Length == 3)
                        { reg = reg.Substring(1, 2); }
                        else
                        { reg = ""; }
                        string tp = (string)curDelivery.GetExtendedProperty("ShipTp");
                        // if ((reg == "CN") && (tp == "CTO"))
                        if ((ActivityCommonImpl.Instance.CheckDomesticDN(reg)) && (tp == "CTO"))
                        {
                            conPrintFlag = true;
                        }
                        //currentSession.AddValue("ConfigPrint", conPrintFlag);
                    }
                }
                currentSession.AddValue("ConfigPrint", conPrintFlag);
                //Model.CustPN
                string custPN = "";
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                custPN = modelRepository.Find(curProduct.Model.Trim()).CustPN;

                currentSession.AddValue("AssetCheck", assetCheck);

                retList.Add(curProduct.ProId);   //productID
                retList.Add(custPN);   //customerPN
                retList.Add((string)curProduct.GetModelProperty("WRNT"));    //Warranty
                retList.Add((string)curProduct.GetModelProperty("TYP"));     //configuration
                retList.Add(assetCheck);
                retList.Add(conPrintFlag);
                retList.Add(assetMessage);
                //Add kaisheng,2012/07/21
                retList.Add(printFlag);
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        IList<PrintItem> IPrintContentWarranty.WarrantyPrint(string productID, Boolean conPrintFlag, IList<PrintItem> printItems)
        {
            logger.Debug("[DX](PrintContentWarrantyImpl)WarrantyPrint start, prodId:" + productID);
            //FisException ex;
            //List<string> erpara = new List<string>();
            string sessionKey = productID;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                    IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                    Delivery curDelivery = new Delivery();
                    if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                    {
                        curDelivery = DeliveryRepository.Find(curProduct.DeliveryNo);
                    }

                    //5A.不需要打印Warranty Card
                    //A. 当不需要打印Warranty Card时，则提示错误信息” No need to print Warranty Card!”
                    //移动至web端提示

                    Boolean WarrantyPrint = (Boolean)currentSession.GetValue("WarrantyPrint");
                    if (WarrantyPrint)
                    {
                        //5B. 没有维护Warranty Card
                        //Product.Model在ModelInfo中找不到对应的Name=” WRNT”的记录
                        string wrnt = (string)curProduct.GetModelProperty("WRNT");
                        if (string.IsNullOrEmpty(wrnt))
                        {
                            SessionManager.GetInstance.RemoveSession(currentSession);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(sessionKey);
                            ex = new FisException("PAK060", erpara);//没有Maintain,请联系PE
                            throw ex;
                        }

                        //5C. Warranty维护错误
                        //[5B]中得到的值，应该是用”,”分隔的，按照”,”解析到数组中，得到数组Ubound值小于2时，报错
                        string[] sArray = wrnt.Split(',');
                        if (sArray.Length < 2)
                        {
                            SessionManager.GetInstance.RemoveSession(currentSession);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(sessionKey);
                            ex = new FisException("PAK084", erpara);//资料有误
                            throw ex;
                        }

                        //5D. unit还没有结合DN
                        //Product.DeliveryNo=””
                        //if ((curProduct.DeliveryNo == null) || (curProduct.DeliveryNo == ""))
                        //{
                        //    SessionManager.GetInstance.RemoveSession(currentSession);
                        //    FisException ex;
                        //    List<string> erpara = new List<string>();
                        //    erpara.Add(sessionKey);
                        //    ex = new FisException("PAK062", erpara);//请先结合DN
                        //    throw ex;
                        //}

                        ////5E. unit结合DN没有ShipDate
                        ////当DN的RegId属性值等于SCN and DN的ShipTp属性值等于CTO时，若Delivery.ShipDate=””

                        //String shipData = curDelivery.ShipDate.ToString();
                        //if (shipData == "")
                        //{
                        //    SessionManager.GetInstance.RemoveSession(currentSession);
                        //    FisException ex;
                        //    List<string> erpara = new List<string>();
                        //    erpara.Add(sessionKey);
                        //    ex = new FisException("PAK063", erpara);//無ShipDate,不能打印
                        //    throw ex;
                        //}
                    }
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.AddValue(Session.SessionKeys.ValueToCheck, conPrintFlag);

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


                    IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                    return returnList;

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
                logger.Debug("(PrintContentWarrantyImpl)WarrantyPrint end, prodId:" + productID);
            }
        }

        IList<PrintItem> IPrintContentWarranty.ConfigurationPrint(string productID, IList<PrintItem> printItems)
        {
            logger.Debug("(PrintContentWarrantyImpl)ConfigurationPrint start, prodId:" + productID);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = productID;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    Product tempProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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


                    IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                    return returnList;

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
                logger.Debug("(PrintContentWarrantyImpl)ConfigurationPrint end, prodId:" + productID);
            }
        }

        void IPrintContentWarranty.Cancel(string productID)
        {
            string sessionKey = productID;
            try
            {
                logger.Debug("(PrintContentWarranty)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(PrintContentWarranty)Cancel end, sessionKey:" + sessionKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList IPrintContentWarranty.ReprintConfigurationLabel(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(IPrintContentWarranty)ReprintConfigurationLabel start, customerSN:" + customerSN
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ReprintConfigurationLabel.xoml", null, out wfName, out rlName);
                    //RouteManagementUtils.GetWorkflow(station, "104KPPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    //==============================================
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.LineCode, "PAK");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "ConfigurationLabel");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    //==============================================

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
                //===============================================================================
                //Get infomation
                ArrayList retList = new ArrayList();
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(returnList);
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Debug("(IPrintContentWarranty)ReprintConfigurationLabel end, customerSN:" + customerSN);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList IPrintContentWarranty.ReprintWarrantyLabel(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(IPrintContentWarranty)ReprintWarrantyLabel start, customerSN:" + customerSN
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ReprintWarrantyLabel.xoml", null, out wfName, out rlName);
                    //RouteManagementUtils.GetWorkflow(station, "104KPPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    //==============================================
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.LineCode, "WarrantyCardPrint");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "WarrantyLabel");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);

                    //==============================================

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
                //===============================================================================
                ArrayList retList = new ArrayList();
                //Get infomation
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);


                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(returnList);
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Debug("(IPrintContentWarranty)ReprintWarrantyLabel end, customerSN:" + customerSN);
            }

        }
    }
}
