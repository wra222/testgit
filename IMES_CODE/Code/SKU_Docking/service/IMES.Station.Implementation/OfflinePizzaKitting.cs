/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Pizz
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-20   Yang.Weihua               Create   
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.Common;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.PrintLog;




namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPizzaKitting接口的实现类
    /// </summary>
    public class OfflinePizzaKitting : MarshalByRefObject, IOfflinePizzaKitting
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private const string WFfile = "OfflinePizzaKitting.xoml";
        private const string Rulesfile = "OfflinePizzaKitting.rules";
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

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
        public ArrayList GetBOM(string model, string key, string lastKey, string line, string editor, string station, string customer)
        {
            logger.Debug("(PizzaKitting)InputSN start, session key:" + key
                + "Station:" + station);
            string sessionKey = key;

            try
            {

                NewWorkFlow(model, key, line, editor, station, customer);
                //Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                
                ////用ProductID启动工作流，将Product放入工作流中
                //if (currentSession == null)
                //{
                //    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                //    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                //    wfArguments.Add("Key", sessionKey);
                //    wfArguments.Add("Station", station);
                //    wfArguments.Add("CurrentFlowSession", currentSession);
                //    wfArguments.Add("Editor", editor);
                //    wfArguments.Add("PdLine", line);
                //    wfArguments.Add("Customer", customer);
                //    wfArguments.Add("SessionType", SessionType);

                //    string wfName, rlName;
                //    RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);//PDPLabel01.rules
                //    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                //    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                //    currentSession.AddValue(Session.SessionKeys.ModelName, model);
                //    currentSession.SetInstance(instance);

                //    if (!SessionManager.GetInstance.AddSession(currentSession))
                //    {
                //        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                //        FisException ex;
                //        List<string> erpara = new List<string>();
                //        erpara.Add(sessionKey);
                //        ex = new FisException("CHK020", erpara);
                //        throw ex;
                //    }

                //    currentSession.WorkflowInstance.Start();
                //    currentSession.SetHostWaitOne();
                //}
                //else
                //{
                //    FisException ex;
                //    List<string> erpara = new List<string>();
                //    erpara.Add(sessionKey);
                //    ex = new FisException("CHK020", erpara);
                //    throw ex;
                //}

                //if (currentSession.Exception != null)
                //{
                //    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                //    {
                //        currentSession.ResumeWorkFlow();
                //    }

                //    throw currentSession.Exception;
                //}
                if (lastKey != "")
                { Cancel(lastKey); }
                ArrayList retLst = new ArrayList();
                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                retLst.Add(bomItemList);
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
                logger.Debug("(OfflinePizzaKitting)InputSN end,  custSn:" + sessionKey +" model: " +model);
            }

        }
        private void NewWorkFlow(string model, string key, string line, string editor, string station, string customer)
        {
            string sessionKey = key;
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
            //用ProductID启动工作流，将Product放入工作流中
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
                RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);//PDPLabel01.rules
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                currentSession.AddValue(Session.SessionKeys.ModelName, model);
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
         }
    
       

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public ArrayList Save(IList<PrintItem> printItems, string key, string line)
        {
            logger.Debug("(OfflinePizzaKitting)save start,"
                + " [key]: " + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

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
                string editor=session.Editor;
                string customer = session.Customer;
                string station = session.Station;
                string pizzaID = ((Pizza)session.GetValue(Session.SessionKeys.PizzaID)).PizzaID.Trim();
                string model = session.GetValue(Session.SessionKeys.ModelName).ToString();
                session.AddValue(Session.SessionKeys.PrintItems, printItems);
                session.AddValue(Session.SessionKeys.IsComplete, true);
                session.AddValue(Session.SessionKeys.PrintLogName, "Offline_PizzaLabel");
                session.AddValue(Session.SessionKeys.PrintLogBegNo, pizzaID);
                session.AddValue(Session.SessionKeys.PrintLogEndNo, pizzaID);
                session.AddValue(Session.SessionKeys.PrintLogDescr, "");

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
                IList<PrintItem> printLst = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
              
                string newKey = Guid.NewGuid().ToString();
                ArrayList retLst = new ArrayList();
                retLst.Add(pizzaID);
                retLst.Add(printLst);
                retLst.Add(newKey);
                NewWorkFlow(model,newKey,line,editor,station,customer);
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(newKey, SessionType);
                retLst.Add(bomItemList);
                return retLst;
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
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }

                logger.Debug("(OfflinePizza Kitting)save end,"
                   + " [key]: " + key);
            }
        }


        public ArrayList RePrint(string sn, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(OfflinePizza Kitting)Reprint start, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                    IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    string key = "";
                if (sn.Length == 14)
                {
                    IList<PizzaPart> pizzaPartList = pizzaRepository.GetPizzaPartsByValue(sn);
                    if (pizzaPartList.Count > 0)
                    {
                        key = pizzaPartList[0].PizzaID;
                    }
                }
                else if (sn.Length == 9)
                {
                   Pizza pizza= pizzaRepository.Find(sn);
                   if (pizza != null)
                   {
                       key = pizza.PizzaID;
                   }
                }
                else
                {
                    throw new FisException("Wrong input!!");
                }
                if (key == "")
                {
                    throw new FisException("Wrong input!!");
                }
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
                    RouteManagementUtils.GetWorkflow(station, "RePrintOfflinePizzaKitting.xoml", string.Empty, out wfName, out rlName);
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
                logger.Debug("(OfflinePizza Kitting)Reprint end, ProdId:" + sn + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string key) 
        {
            logger.Debug("(OfflinePizzaKitting)Cancel start, session key:" + key);
            string sessionKey = key;

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
                logger.Debug("(OfflinePizzaKitting)Cancel end, session key:" + key);
            }
        }

      
    }
}
