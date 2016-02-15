/*
* INVENTEC corporation ©2011 all rights reserved. 
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using System.Linq;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// ICollectTabletFaPart接口的实现类
    /// </summary>
    public class CollectTabletFaPart : MarshalByRefObject, ICollectTabletFaPart
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private const string WFfile = "CollectTabletFaPart.xoml";
        private const string Rulesfile = "CollectTabletFaPart.rules";

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="curStation"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSN, string line, string curStation,
                                                    string editor, string station, string customer,bool allnullbomitem)
        {
            logger.Debug("(CollectTabletFaPart)InputSN start, custSn:" + custSN
                + "Station:" + curStation);

            try
            {
                ArrayList retLst = new ArrayList();
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                if (currentProduct == null)
                {
                   throw new FisException("PAK084", new string[] { });
                }
                string sessionKey = currentProduct.ProId;
                Dictionary<string, object> sessionKeyValue = new Dictionary<string, object>();
                sessionKeyValue.Add(Session.SessionKeys.IsComplete, false);
                Session currentSession=  WorkflowUtility.InvokeWF(sessionKey, station, line, customer, editor, SessionType, WFfile, Rulesfile, sessionKeyValue);

                /*
                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                    WorkflowUtility.InvokeWF(sessionKey,station,line,customer,editor,SessionType,
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);
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
                }*/

                //get product data for UI
                IMES.DataModel.ProductInfo prodInfo = currentProduct.ToProductInfo();
                retLst.Add(prodInfo);

                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                if (!allnullbomitem)
                {
                    if (bomItemList == null || bomItemList.Count == 0)
                    {
                        Cancel(sessionKey);
                        throw new FisException("PKIT03", new string[] { });
                    }
                }
                retLst.Add(bomItemList);
                bool needPODLabel = false;
                string color = "";
                if (station.Equals("T2") || station.Equals("T3"))
                {
                    needPODLabel = IsNeedPodLabel(currentProduct.Model);
                    if(station.Equals("T2"))
                    {  color = GetCqPodLabelColor(currentProduct.Model);}
                }
                retLst.Add(needPODLabel);
                retLst.Add(color);
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
                logger.Debug("(CollectTabletFaPart)InputSN end,  custSn:" + custSN);
            }

        }
        public ArrayList RePrintPOD(string sn, string reason, string editor, string station, string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                ArrayList retLst = new ArrayList();
                var currentProduct = CommonImpl.GetProductByInput(sn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                if (currentProduct == null)
                {
                    throw new FisException("PAK084", new string[] { });
                }
                bool needPODLabel = IsNeedPodLabel(currentProduct.Model);
                if (!needPODLabel)
                {
                    throw new FisException("This Model need not print POD label!");
                }
                string color = GetCqPodLabelColor(currentProduct.Model);
                retLst.Add(currentProduct.Model);
                retLst.Add(color);

                var log = new ReprintLog
                {
                    LabelName = "Tablet POD",
                    BegNo = sn,
                    EndNo = sn,
                    Descr = "Reprint "+color +" POD",
                    Reason =reason,
                    Editor = editor
                };
                IMES.Infrastructure.UnitOfWork.IUnitOfWork uof = new IMES.Infrastructure.UnitOfWork.UnitOfWork();
                var rep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();
                rep.Add(log, uof);
                uof.Commit();
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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        
        
        }
        private bool IsNeedPodLabel(string model)
        {
            IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            podLabelPartLst = PartRepository.GetPODLabelPartListByPartNo(model);
            return podLabelPartLst.Count > 0;
        }
        private string GetCqPodLabelColor(string model)
        {

            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            CommonImpl2 cm2 = new CommonImpl2();
            string color = "Black";
            IList<string> lstValue
                     = cm2.GetConstValueTypeByType("POD_White_Lable_PN").Where(x => x.value != "").Select(x => x.value).ToList();
            if (lstValue != null && lstValue.Count > 0)
            {
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    if (lstValue.Contains(ibomnode.Part.PN))
                    { color = "White"; ; break; }

                }
            }
            return color;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            //MatchedPartOrCheckItem rtnMatchedPartOrCheckItem = PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
            //rtnMatchedPartOrCheckItem.CollectionData = "";
            //rtnMatchedPartOrCheckItem.PNOrItemName = "";
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public ArrayList Save(string prodId, IList<PrintItem> printItems)
        {
            logger.Debug("(CollectTabletFaPart)save start,"
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
                    throw ex;
                }
                session.AddValue(Session.SessionKeys.PrintItems, printItems);
                //session.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                //session.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                //session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                //session.AddValue(Session.SessionKeys.PrintLogDescr, prodId);
                //session.AddValue(Session.SessionKeys.Reason, "");
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
           
                IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                string paqcStatus = product.GetAttributeValue("PAQC_QCStatus") ?? "";
           
                IList<PrintItem> lst = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(lst);
                arr.Add(paqcStatus);
                bool[] b = CheckLabel(product.Model);
                arr.Add(b[0]);
                arr.Add(b[1]);
                arr.Add(b[2]);
                return arr;
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
                logger.Debug("(CollectTabletFaPart)save end,"
                   + " [prodId]: " + prodId);
            }
        }
        private bool[] CheckLabel(string model)
        {

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> PLBomNodeList = curBom.GetFirstLevelNodesByNodeType("PL");
            bool[] r = new bool[3];
           r[0] = PLBomNodeList.Any(x => x.Part.PN == "6060B1577401");  // E-waste label: ModelBOM下带6060B1577401,需要在T4站提示：礼盒Sleeve上需贴附巴西E-waste label 
           r[1] = PLBomNodeList.Any(x => x.Part.PN == "6060B1144701"); //..BAHASA label：ModelBOM下带6060B1144701,需要在T4站提示：礼盒Sleeve上需贴附BAHASA label
           r[2] = PLBomNodeList.Any(x => x.Part.PN == "60LANOM00001"); //NOM Label: ModelBOM下带60LANOM00001料
            return r;

        }
        public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(CollectTableFaPart)Reprint start, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                string custsn = "";
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IMES.FisObject.FA.Product.IProduct currentProduct = productRepository.GetProductByIdOrSn(sn);

                if (currentProduct == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(sn);
                    throw new FisException("SFC002", errpara);
                }
                custsn = currentProduct.CUSTSN;
                string sessionKey = currentProduct.ProId;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrintEx.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //Get Product data

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, sessionKey);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, sessionKey);



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


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                arr.Add(custsn);
                return arr;
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
                logger.Debug("(CombineKeyParts)Reprint end, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            logger.Debug("(CollectTabletFaPart)Cancel start, [prodId]:" + prodId);
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
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(CollectTabletFaPart)Cancel end, [prodId]:" + prodId);
            }
        }

    }
}
