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
using System.Linq;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPizzaKitting接口的实现类
    /// </summary>
    public class OfflinePizzaKittingForRCTO : MarshalByRefObject,IOfflinePizzaKittingForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private const string WFfile = "OfflinePizzaKittingForRCTO.xoml";
        private const string Rulesfile = "OfflinePizzaKittingForRCTO.rules";
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
                
                if (lastKey != "")
                { Cancel(lastKey); }
                ArrayList retLst = new ArrayList();
                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                retLst.Add(bomItemList);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                var bom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                CopyBom(currentSession);
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
                logger.Debug("(OfflinePizzaKittingForRCTO)InputSN end,  custSn:" + sessionKey + " model: " + model);
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

        private void CopyBom(Session session)
        {
            IFlatBOM obom = new FlatBOM();
            var bom = (IFlatBOM)session.GetValue(Session.SessionKeys.SessionBom);
            foreach (IFlatBOMItem bi in bom.BomItems)
            {
                obom.AddBomItem(bi);
            }
        //    obom = bom;
            session.AddValue("OriginalBom", obom);
        }

        public List<string> TryPartMatchCheck(string sessionKey, string checkValue,string partNo)
        {
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
            if (currentSession == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                logger.Error(ex.Message, ex);
                throw ex;
            }

            var oriBom = (IFlatBOM)currentSession.GetValue("OriginalBom");

            IFlatBOM tmpbom = new FlatBOM();
            foreach (IFlatBOMItem bi in oriBom.BomItems)
            {
                tmpbom.AddBomItem(bi);
            }

            currentSession.AddValue(Session.SessionKeys.SessionBom, tmpbom);
            var bom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
            IList<IFlatBOMItem> lst = bom.BomItems;
            IList<IFlatBOMItem> notMatchPart = new List<IFlatBOMItem>(); 
            foreach(IFlatBOMItem flm in lst)
            {
                if (flm.PartNoItem == partNo)
                {
                    continue;
                }
                else
                {
                    notMatchPart.Add(flm);
                }
            }
            foreach (IFlatBOMItem flm2 in notMatchPart)
            {
                lst.Remove(flm2);
            }
       
          
           MatchedPartOrCheckItem mp= PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
           int q= bom.BomItems[0].AlterParts.Where(x=>x.Attributes.Where(y=>y.InfoType=="VendorCode").ToList().Count>0).ToList().Count;
           List<PartUnit> lstSessionPart = (List<PartUnit>)currentSession.GetValue("CheckPartUnitForOfflineRCTO");
           if (currentSession.GetValue("CheckPartUnitForOfflineRCTO")== null)
           {
                lstSessionPart = new List<PartUnit>();
                currentSession.AddValue("CheckPartUnitForOfflineRCTO", lstSessionPart);
           }
           lstSessionPart.Add(bom.BomItems[0].CheckedPart[0]);
           bom.BomItems[0].ClearCheckedPart();
           
            List<string> ret = new List<string>();
            ret.Add(mp.PNOrItemName);
            ret.Add(bom.BomItems[0].Tp);
            ret.Add(bom.BomItems[0].Descr);
            ret.Add(mp.CollectionData);
            ret.Add(q.ToString());
            return ret;
        }

        public IList<ConstValueTypeInfo> GetConstValueTypeListByType(string type)
        {
            IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    retLst = partRep.GetConstValueTypeList(type);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

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
            logger.Debug("(OfflinePizzaKittingForRCTO)save start,"
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
                List<PartUnit> lstSessionPart = (List<PartUnit>)session.GetValue("CheckPartUnitForOfflineRCTO");
                var bom = (IFlatBOM)session.GetValue(Session.SessionKeys.SessionBom);
                foreach(PartUnit pu in lstSessionPart)
                {
                    bom.BomItems[0].AddCheckedPart(pu);
                }

                string editor = session.Editor;
                string customer = session.Customer;
                string station = session.Station;
                string pizzaID = ((Pizza)session.GetValue(Session.SessionKeys.PizzaID)).PizzaID.Trim();
                string model = session.GetValue(Session.SessionKeys.ModelName).ToString();
                session.AddValue(Session.SessionKeys.PrintItems, printItems);
                session.AddValue(Session.SessionKeys.IsComplete, true);
                session.AddValue(Session.SessionKeys.PrintLogName, "OfflineForRCTO_PizzaLabel");
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
                NewWorkFlow(model, newKey, line, editor, station, customer);
                 Session newSession = SessionManager.GetInstance.GetSession(newKey, SessionType);
                 CopyBom(newSession);
                //IList<BomItemInfo> bomItemList = PartCollection.GeBOM(newKey, SessionType);
                //retLst.Add(bomItemList);
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
                string model="";
                if (sn.Length == 14)
                {
                    IList<PizzaPart> pizzaPartList = pizzaRepository.GetPizzaPartsByValue(sn);
                    if (pizzaPartList.Count > 0)
                    {
                        key = pizzaPartList[0].PizzaID;
                        Pizza pizza = pizzaRepository.Find(key);
                        model = pizza.Model;
                    }
                }
                else if (sn.Length == 9)
                {
                    Pizza pizza = pizzaRepository.Find(sn);
                    if (pizza != null)
                    {
                        key = pizza.PizzaID;
                        model = pizza.Model;
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
                    RouteManagementUtils.GetWorkflow(station, "RePrintOfflinePizzaKittingForRCTO.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //Get Product data

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, sn);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, sn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, sn);
                    Session.AddValue(Session.SessionKeys.ModelName, model);
                   



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
            logger.Debug("(OfflinePizzaKittingForRCTO)Cancel start, session key:" + key);
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
                logger.Debug("(OfflinePizzaKittingForRCTO)Cancel end, session key:" + key);
            }
        }


    }
}
