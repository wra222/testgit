/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx –2011/11/15 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx –2011/11/15            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
* ITC-1413-0006 增加Consumer Family / Commercial Family判断
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
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure.Extend;
using log4net;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPizzaKitting接口的实现类
    /// </summary>
    public class PizzaKitting : MarshalByRefObject, IPizzaKitting
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSN, string line, string curStation,
                                                    string editor, string station, string customer)
        {
            logger.Debug("(PizzaKitting)InputSN start, custSn:" + custSN
                + "Station:" + curStation);

            try
            {
                ArrayList retLst = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", curStation);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    string wfName, rlName;
                    if (curStation != "PKCK")
                    {
                        RouteManagementUtils.GetWorkflow(station, "PizzaKitting.xoml", "PizzaKitting.rules", out wfName, out rlName);
                    }
                    else
                    {
                        RouteManagementUtils.GetWorkflow(station, "PizzaKittingCheck.xoml", "PizzaKittingCheck.rules", out wfName, out rlName);
                    }
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

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

                //============================================================================
                //get product data for UI
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
                retLst.Add(prodInfo);

                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;
                /*for (int i = 1; i < 14; i++)
                {
                    BomItemInfo bom =new BomItemInfo();
                    pUnit unit = new pUnit();
                    PartNoInfo info = new PartNoInfo();
                    unit.pn = "unitpn"+Convert.ToString(i);
                    info.id = "partid" + Convert.ToString(i);
                    bom.parts = new List<PartNoInfo>();
                    bom.parts.Add(info);
                    bom.type = "type"+Convert.ToString(i);
                    bom.description="description"+Convert.ToString(i);
                    bom.qty = i;
                    bom.scannedQty = i;
                    bom.collectionData = new List<pUnit>();
                    bom.collectionData.Add(unit);
                    bomItemList.Add(bom);
                    
                }*/
                retLst.Add(bomItemList);
                //============================================================================
                return retLst;

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
                logger.Debug("(PizzaKitting)InputSN end,  custSn:" + custSN);
            }

        }

        public void InputPizzaID(string productID, string pizzaID, string line, string curStation, string model,
                                        string editor, string station, string customer)
        {
            logger.Debug("(PizzaKitting)InputPizzaID start, PizzaID:" + pizzaID
                + "Station:" + station);

            //return;
            //a.	如果1st Pizza ID 在数据库(IMES_PAK..Pizza.PizzaID)中不存在，
            //      则报告错误：“非法的Pizza ID!”
            IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            Pizza pizza = pizzaRep.Find(pizzaID);
            if (pizza == null)
            {
                //todo: throw FisException
                throw new FisException("CHK852", new List<string>());
            }
            
            //b.	如果1st Pizza ID 与和Product 结合的1st Pizza ID(IMES_FA..Product.PizzaID) 不同，
            //      则报告错误：“错误的Pizza ID!”
            Session session = SessionManager.GetInstance.GetSession(productID, SessionType);
            if (session == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(productID);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }

            IProduct product = (Product) session.GetValue(Session.SessionKeys.Product);
            if (pizzaID.CompareTo(product.PizzaID) != 0)
            {
                //todo: throw FisException
                throw new FisException("CHK851", new List<string>());
            }

            return;
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            //MatchedPartOrCheckItem item = new MatchedPartOrCheckItem();
            //item.PNOrItemName = "01C2PNPK110001";
            //return item;
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void Save(string prodId)
        {
            logger.Debug("(PDPALabel02)save start,"
                + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);
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
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PDPALabel02)save end,"
                   + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            logger.Debug("(PDPALabel02)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

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
                logger.Debug("(PDPALabel02)Cancel end, [prodId]:" + prodId);
            }
        }

        public ArrayList Print(string productID, string line, string code, string floor,
                                      string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PizzaKitting)Print start, ProductID:" + productID + " pdLine:" + line + " stationId:" + station + " editor:" + editor);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
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

                //Consumer Family / Commercial Family 使用不同的Template
                //Consumer Family 使用COO Label-2模板
                //Commercial Family 使用COO Label模板

                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model curModel = modelRep.Find(curProduct.Model);
                string family = curModel.FamilyName;
                string labelTemp = "COO Label";
                //下列Family 为Consumer Family:
                if (family == "HARBOUR 1.0" || family == "HARBOUR 1.1" || family == "ST133I 1.0" || family == "ST133I 1.1"
                    || family == "ST133I 1.2" || family == "ST145A 1.0" || family == "ST145A 1.1" || family == "ST145A 1.2"
                    || family == "ST145I 1.0" || family == "ST145I 1.1" || family == "ST145I 1.2" || family == "ROMEO 1.0"
                    || family == "ROMEO 1.1" || family == "ROMEO 1.2" || family == "ROMEO 2.0" || family == "ZIDANE 1.0"
                    || family == "ZIDANE 1.1" || family == "ZIDANE 1.2" || family == "ZIDANE 2.0" || family == "ZIDANE 2.1"
                    || family == "MURRAY 1.1" || family == "MURRAY 1.2" || family == "MURRAY 1.2" || family == "JIXI 1.0"
                    || family == "JIXI 2.0" || family == "JIXI 2.1" || family == "JIXI 2.2" || family == "JACKMAND 1.0"
                    || family == "JACKMANU 1.0" || family == "JACKMAND 1.1" || family == "JACKMANU 1.1" || family == "JACKMAND 1.2"
                    || family == "JACKMANU 1.2" || family == "PARKERU 1.0" || family == "PARKERD 1.0" || family == "PARKERU 1.1"
                    || family == "PARKERD 1.1" || family == "PARKERU 1.2" || family == "PARKERD 1.2" || family == "KITTY 1.0"
                    || family == "HELLO 1.0" || family == "VUITTON 1.0" || family == "LAUREN 1.0" || family == "VUITTON 1.1"
                    || family == "LAUREN 1.1")
                {
                    labelTemp = "COO Label-2";
                }

                ArrayList retList = new ArrayList();
                IList<PrintItem> printlist = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printlist);
                retList.Add(labelTemp);

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
                logger.Debug("(PizzaKitting)Print end, ProductID:" + productID);
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
        public ArrayList ReprintPizzaKitting(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(ReprintPizzaKitting)ReprintConfigurationLabel start, customerSN:" + customerSN
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,"PKOK");
                if (logList.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK860", erpara);
                    throw ex;
                }

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
                    RouteManagementUtils.GetWorkflow(station, "ReprintPizzaKitting.xoml", null, out wfName, out rlName);
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
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "PizzaKitting");
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

                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model curModel = modelRep.Find(curProduct.Model);
                string family = curModel.FamilyName;
                string labelTemp = "COO Label";
                //下列Family 为Consumer Family:
                if (family == "HARBOUR 1.0" || family == "HARBOUR 1.1" || family == "ST133I 1.0" || family == "ST133I 1.1"
                    || family == "ST133I 1.2" || family == "ST145A 1.0" || family == "ST145A 1.1" || family == "ST145A 1.2"
                    || family == "ST145I 1.0" || family == "ST145I 1.1" || family == "ST145I 1.2" || family == "ROMEO 1.0"
                    || family == "ROMEO 1.1" || family == "ROMEO 1.2" || family == "ROMEO 2.0" || family == "ZIDANE 1.0"
                    || family == "ZIDANE 1.1" || family == "ZIDANE 1.2" || family == "ZIDANE 2.0" || family == "ZIDANE 2.1"
                    || family == "MURRAY 1.1" || family == "MURRAY 1.2" || family == "MURRAY 1.2" || family == "JIXI 1.0"
                    || family == "JIXI 2.0" || family == "JIXI 2.1" || family == "JIXI 2.2" || family == "JACKMAND 1.0"
                    || family == "JACKMANU 1.0" || family == "JACKMAND 1.1" || family == "JACKMANU 1.1" || family == "JACKMAND 1.2"
                    || family == "JACKMANU 1.2" || family == "PARKERU 1.0" || family == "PARKERD 1.0" || family == "PARKERU 1.1"
                    || family == "PARKERD 1.1" || family == "PARKERU 1.2" || family == "PARKERD 1.2" || family == "KITTY 1.0"
                    || family == "HELLO 1.0" || family == "VUITTON 1.0" || family == "LAUREN 1.0" || family == "VUITTON 1.1"
                    || family == "LAUREN 1.1")
                {
                    labelTemp = "COO Label-2";
                }

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printList);
                retList.Add(labelTemp);

                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Debug("(IPrintContentWarranty)ReprintPizzaKitting end, customerSN:" + customerSN);
            }

        }

        #endregion

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByCode(string code)
        {
            logger.Debug("(PizzaKitting)getBomByCode Start[code]:" + code);
            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<LightBomInfo> retLst = productRepository.GetWipBufferInfoListByKittingCode(code);
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
                logger.Debug("(PizzaKitting)getBomByCode End, "
                   + " [code]:" + code);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByModel(string model, out string code)
        {
            logger.Debug("(PizzaKitting)getBomByModel Start [model]:" + model);

            try
            {
                IList<LightBomInfo> retLst = new List<LightBomInfo>();

                //判断系统中是否存在该Model
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model modelObject = modelRepository.Find(model);
                if (modelObject == null)
                {
                    throw new FisException("CHK038", new string[] { model });
                }
                else
                {
                    //判断该Model是否维护Kitting Code
                    string ret = modelObject.GetAttribute("DM2");

                    logger.Debug("(PizzaKitting)Maintain Kitting Code[ret]:" + ret);
                    if (ret == null || ret.Trim() == string.Empty)
                    {
                        throw new FisException("CHK113", new string[] { model });
                    }
                    else
                    {
                        //根据model获取Kitting Code
                        code = modelRepository.GetKittingCodeByModel(model);
                        if (code == null || code.Trim() == string.Empty)
                        {
                            throw new FisException("CHK113", new string[] { model });
                        }
                        else
                        {
                            //判断查询得到的Kitting Code 是否在Kitting 表中存在
                            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                            int count = productRepository.GetCountOfKittingCodeByCode(code);
                            if (count == 0)
                            {
                                throw new FisException("CHK114", new string[] { model });
                            }
                            else
                            {
                                //获取bom
                                retLst = productRepository.GetWipBufferInfoListByKittingCodeAndModel(code, model);
                            }
                        }
                    }
                }

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
                logger.Debug("(PizzaKitting)getBomByModel End[model]:" + model);

            }
        }

        #endregion
    }
}
