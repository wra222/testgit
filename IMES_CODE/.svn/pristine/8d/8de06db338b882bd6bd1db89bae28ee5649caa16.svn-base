/*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;
using System.Linq; 
using IMES.DataModel;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Station.Implementation
{

    public class CombineOfflinePizzaForRCTO : MarshalByRefObject, ICombineOfflinePizzaForRCTO
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        #region ICombineOfflinePizzaForRCTO Members

        /// <summary>
        /// Save
        /// </summary>
        public ArrayList InputCartonId(string cartonSN, IList<PrintItem> printItems, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(CombineOfflinePizzaForRCTO)InputCartonId start, cartonSN:" + cartonSN + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);

            try
            {
                ArrayList retLst = new ArrayList();

                string sessionKey = cartonSN;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "CombineOfflinePizzaForRCTO.xoml", "CombineOfflinePizzaForRCTO.rules", out wfName, out rlName);//PDPLabel01.rules
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, cartonSN);
                    currentSession.AddValue(Session.SessionKeys.Reason, "");
					
					currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue("CartonSN", cartonSN);
					
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

                Product currentProduct = currentSession.GetValue(Session.SessionKeys.Product) as Product;

                //IList<IProduct> prodList = (IList<IProduct>)currentSession.GetValue(Session.SessionKeys.ProdList);
                IList<string> prodNoList = (IList<string>)currentSession.GetValue(Session.SessionKeys.ProdNoList);
                retLst.Add(prodNoList.Count); // 1

                //get bom
                ArrayList lstBoms = new ArrayList();
                IList<IBOMNode> lstBomsC2 = new List<IBOMNode>();

                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM curBOM = bomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
                IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;
                foreach (IBOMNode n in bomNodeLst)
                {
                    if (!"C2".Equals(n.Part.BOMNodeType))
                        continue;

                    string showPartNo = n.Part.PN;
                    string sub = ""; // 替代料
                    foreach (PartInfo pi in n.Part.Attributes)
                    {
                        if ("VendorCode".Equals(pi.InfoType))
                        {
                            showPartNo = pi.InfoValue;
                        }
                        if ("SUB".Equals(pi.InfoType))
                        {
                            sub = sub + "/" + pi.InfoValue;
                        }
                    }
                    showPartNo = showPartNo + sub;

                    ArrayList lstBom = new ArrayList();
                    lstBom.Add(showPartNo); // PartNo/Item Name
                    lstBom.Add(n.Part.Type);
                    lstBom.Add(n.Part.Descr);
                    lstBom.Add(prodNoList.Count.ToString()); // Qty
                    lstBom.Add(""); // PQty
                    lstBom.Add(""); // Collection Data

                    lstBoms.Add(lstBom);
                    lstBomsC2.Add(n);
                }
                if (lstBoms.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK165", erpara);
                    ex.stopWF = true;
                    throw ex;
                }
                currentSession.AddValue("BomC2", lstBomsC2);
                currentSession.AddValue("CheckedBomPartNo", new List<string>());

                currentSession.AddValue(Session.SessionKeys.PizzaNoList, null);

                //IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, Session.SessionType.Product);
                retLst.Add(lstBoms); // 2

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retLst.Add(returnList); // 3
				
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
                logger.Debug("(CombineOfflinePizzaForRCTO)InputCartonId end, cartonSN:" + cartonSN + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            }
        }

        /// <summary>
        /// InputPizzaId
        /// </summary>
        public ArrayList InputPizzaId(string cartonSN, string pizzaId, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(CombineOfflinePizzaForRCTO)InputPizzaId start, cartonSN:" + cartonSN + " , pizzaId:" + pizzaId + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            Session Session = SessionManager.GetInstance.GetSession(cartonSN, TheType);
            if (Session == null)
            {
                erpara.Add(cartonSN);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {
                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.PizzaID, pizzaId);
                
                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                ArrayList retLst = new ArrayList();
                bool isComplete = (bool) Session.GetValue(Session.SessionKeys.IsComplete);
                if (isComplete)
                    retLst.Add("Finish"); // 1
                else
                    retLst.Add("");

                int idxPizzaMatch = (int)Session.GetValue("IdxPizzaMatch");
                retLst.Add(idxPizzaMatch); // 2

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
                logger.Debug("(CombineOfflinePizzaForRCTO)InputPizzaId end, cartonSN:" + cartonSN + " , pizzaId:" + pizzaId + " , pdLine: " + pdLine + " , editor: " + editor + " , stationId: " + stationId + " , customerId: " + customerId);
            }
            return null;
        }

        public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(CombineOfflinePizzaForRCTO)Reprint start, cartonSN:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                string key = sn;
                
                //Check Print Log
                var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


                bool bFlag = false;

                bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(printItems[0].LabelType, key);
                if (!bFlag)
                {

                    ex = new FisException("CHK270", erpara);
                    throw ex;
                }

                //Check Print Log
                string sessionKey = key;
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
                    RouteManagementUtils.GetWorkflow(station, "RePrintCombineOfflinePizzaForRCTO.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //Get Product data

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, sn);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, sn);



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
                logger.Debug("(CombineOfflinePizzaForRCTO)Reprint end, cartonSN:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
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
                logger.Debug("(CombineOfflinePizzaForRCTO)Cancel start, sessionKey:" + sessionKey);

                Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                logger.Debug("(CombineOfflinePizzaForRCTO)Cancel end, sessionKey:" + sessionKey);
            }
        }

         #endregion
    }
}
