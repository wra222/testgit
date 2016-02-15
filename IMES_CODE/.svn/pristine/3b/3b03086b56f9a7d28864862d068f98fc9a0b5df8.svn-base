using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Part;
using System.Collections;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
using System.Text.RegularExpressions;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TravelCardPrint2012 : MarshalByRefObject, ITravelCardPrint2012, IMO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly Session.SessionType theType = Session.SessionType.Common;
        private IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        private static IList<string> needCheckInQtyFAIFAState = new List<string> { "Approval", "Pilot" };
        

        #region ITravelCardPrint Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="mo"></param>
        /// <param name="qty"></param>
        /// <param name="IsNextMonth"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="prouctIdList"></param>
        /// <param name="printItems"></param>
        /// <param name="battery"></param>
        /// <param name="lcm"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="bomremark"></param>
        /// <param name="remark"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public IList<PrintItem> PrintTCWithProductIDForBN(string pdLine, string model, string mo,
                                          int qty, bool IsNextMonth, string editor,
                                          string station, string customer,
                                          out IList<string> prouctIdList, IList<PrintItem> printItems,
                                          out string battery, out string lcm,
                                          string deliveryDate, string bomremark, string remark, string exception)
        {
            logger.Debug(" PrintTCWithProductID start, mo:" + mo + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString());
            string currentSessionKey = mo;           
            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);

                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdLine, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "TravelCardPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentCommonSession.AddValue(Session.SessionKeys.MONO, mo);
                    currentCommonSession.AddValue(Session.SessionKeys.Qty, qty);
                    currentCommonSession.AddValue(Session.SessionKeys.IsNextMonth, IsNextMonth);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentCommonSession.AddValue(ExtendSession.SessionKeys.DeliveryDate, deliveryDate);
                    currentCommonSession.AddValue(Session.SessionKeys.ModelName, model);
                    string baseModel = "";
                    bool isPrintComboTravelCard = CheckIsPrintComboTravelCard(mo, out baseModel);
                    currentCommonSession.AddValue("IsPrintComboTravelCard", "N");
                    currentCommonSession.AddValue("TravelCardBaseModel", "");
                    if (isPrintComboTravelCard)
                    {
                        currentCommonSession.AddValue("IsPrintComboTravelCard", "Y");
                        currentCommonSession.AddValue("TravelCardBaseModel", baseModel);
                        qty = qty * 2;
                        currentCommonSession.AddValue(Session.SessionKeys.Qty, qty);
                    }
                    currentCommonSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentCommonSession.AddValue("BomRemark", bomremark);
                    currentCommonSession.AddValue("Remark", remark);
                    currentCommonSession.AddValue("Exception", exception);
                    IList<string> valueList  = PartRepository.GetValueFromSysSettingByName("Site");
                    if (valueList.Count == 0)
                    {
                        throw new Exception("Error:尚未設定Site...");
                    }
                    else 
                    {
                        if (valueList[0] == "ICC")
                        {
                            currentCommonSession.AddValue("CityType", "cq");
                        }
                        else 
                        {
                            currentCommonSession.AddValue("CityType", "sh");
                        }
                    }
                    //currentCommonSession.AddValue("CityType", "sh");

                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }
                afterCommitFAIModel(model, qty, editor);
                string[] param = (string[])currentCommonSession.GetValue("CardParam");
                //battery = param[0];
                //lcm = param[1];
                //ForTest
                battery = "battery";
                lcm = "lcm";

                prouctIdList = (IList<string>)currentCommonSession.GetValue(Session.SessionKeys.ProdNoList);
                return (IList<PrintItem>)currentCommonSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug(" PrintTCWithProductID end, mo:" + mo + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString());
            }
        }

        private void afterCommitFAIModel(string model, int inQty, string editor)
        {
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            FAIModelInfo faiModel = iModelRepository.GetFAIModelByModel(model);

            if (faiModel != null &&
                 needCheckInQtyFAIFAState.Contains(faiModel.FAState) &&
                faiModel.Editor.StartsWith("FAI"))
            {
                faiModel.Editor = editor;
                iModelRepository.UpdateFAIModel(faiModel);
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
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
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


        public IList<ConstValueInfo> GetExList()
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ConstValueInfo info = new ConstValueInfo();
            info.type = "FaException";
            IList<ConstValueInfo> list = new List<ConstValueInfo>();
            list = partRepository.GetConstValueInfoList(info);
            return list;
        }

        public bool CheckBTByModel(string model)
        {
            bool isBT = false;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            if (!String.IsNullOrEmpty(model))
            {
                isBT = repPizza.CheckExistMpBtOrder(model);                
            }

            return isBT;
        }


        public bool CheckModel(string family, string model)
        {
            bool ret = false;

            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model modelObj = modelRepository.Find(model);

            if (modelObj == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(model);
                FisException ex = new FisException("CHK928", errpara);
                throw ex;
            }

            if (modelObj.Family.FamilyName != family || modelObj.Status != "1")
            {
                List<string> errpara = new List<string>();
                errpara.Add(model);
                FisException ex = new FisException("CHK928", errpara);
                throw ex;
            }

            return ret;
        }



        #region IMO Members

        public IList<MOInfo> GetMOList(string modelId)
        {
            //IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            IList<string> moLst = new List<string>();
            IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            moLst = myRepository.GetMOsRecentOneMonthByModelRegardlessUdt(modelId);

            IList<MOInfo> infoLst = new List<MOInfo>();
            foreach (string temp in moLst)
            {
                MOInfo currentMOInfo = new MOInfo();
                currentMOInfo.id = temp;
                currentMOInfo.friendlyName = temp;
                infoLst.Add(currentMOInfo);
            }
            return infoLst;
            //return (List<MOInfo>)moRepository.GetMOListFor014ConsiderStartDateOrderByMo(modelId);
        }

        
        MOInfo IMO.GetMOInfo(string MOId)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            var currentMO = moRepository.Find(MOId);

            MOInfo currentMOInfo = new MOInfo();
            currentMOInfo.qty = currentMO.Qty;
            currentMOInfo.pqty = currentMO.PrtQty;
            return currentMOInfo;
        }

        #endregion

        #region Reprint
        public ArrayList ReprintTravelCard(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(TravelCardPrint)ReprintLabel start, startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " customerId:" + customer);
                // Check Prodid....
                // Check Prodid....

                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "TravelCardRePrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    //Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);                    
                    //Session.AddValue(Session.SessionKeys.PrintLogDescr, product.MO);
                    //Session.AddValue(Session.SessionKeys.MONO, product.MO);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //ITC-1360-1265
                    Session.AddValue(Session.SessionKeys.PrintLogName, "PrdId");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                                        
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

                string sLine = "";
                sLine = (string)Session.GetValue(Session.SessionKeys.LineCode);
                retrunValue.Add(sLine);
                retrunValue.Add(prodid);
                retrunValue.Add(returnList);

                return retrunValue;
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
                logger.Debug("(TravelCardPrint)ReprintLabel end, mo:" + mo + " startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer);

            }
        }

        #endregion

        /// <summary>
        /// 获取Model列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.ModelInfo> GetModelList(string family)
        {
            //IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //return myRepository.GetModelListByFamilyAndStatus(family, 1);
            //ITC-1360-1256
            //IMBMORepository::
            //IList<string> GetModelListFromMo(string family);
            IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            IList<string> list = new List<string>();
            list = myRepository.GetModelListFromMo(family);

            IList<IMES.DataModel.ModelInfo> ret = new List<IMES.DataModel.ModelInfo>();

            if (list != null && list.Count > 0)
            {
                foreach (string temp in list)
                {
                    IMES.DataModel.ModelInfo mdli = new IMES.DataModel.ModelInfo();
                    mdli.id = mdli.friendlyName = temp;
                    ret.Add(mdli);
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.FamilyInfo> GetFamilyList(string customer)
        {
            //ITC-1360-0353
            IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
            //IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            return familyRepository.FindFamiliesByCustomerOrderByFamily(customer);
        }

        public List<SerialNumber2012> GetPrintLogProidList(string mo)
        {
            List<SerialNumber2012> retLst = new List<SerialNumber2012>();
            IList<PrintLog> prtLst = null;
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            prtLst = repository.GetPrintLogListByDescr(mo);
            if (prtLst != null)
            {
                foreach (PrintLog s in prtLst)
                {
                    //SerialNumber sn=new SerialNumber(

                    //retLst.Add(s.BeginNo + "-" + s.EndNo);
                    retLst.Add(new SerialNumber2012 { beginNumber = s.BeginNo, endNumber = s.EndNo });
                }
            }
            return retLst;
        }
        #endregion 

        public bool CheckIsPrintComboTravelCard(string moNo, out string baseModel)
        {
            baseModel="";
            var currentMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            var currentMO = currentMORepository.Find(moNo);
            if (currentMO == null)
            {
                var ex = new FisException("CHK037", new string[] { moNo });
                throw ex;
            }
            string model = currentMO.Model;
            
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
             Model modelObj=modelRep.Find(model);
            if (modelObj == null)
            {
                throw new FisException("CHK928", new string[] {model });
            }
            string family=modelObj.Family.FamilyName;

            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> valueList = partRepository.GetConstValueListByType("PrintComboTravelCard");
            IList<ConstValueInfo> valueModel= valueList.Where(x=>x.name=="Model").ToList();
            IList<ConstValueInfo> valueFamily= valueList.Where(x=>x.name=="Family").ToList();
            bool b=false;
            if(valueModel!=null && valueModel.Count>0)
            {
               b=Regex.IsMatch(model,valueModel[0].value);
           }
            if(!b && valueFamily!=null && valueFamily.Count>0 )
            {
                 b=Regex.IsMatch(family,valueFamily[0].value);
            }
            if(!b)
            {return b;}
            //
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;
            IList<IBOMNode> lst=bomNodeList.Where(x=>x.Part.Descr=="ZMODE" && x.Part.PN.StartsWith("ZM2TK")).ToList();
            if (lst.Count > 0)
            {
                baseModel = lst[0].Part.PN.Replace("ZM2TK", "2TK");
                return true;
            }
            else
            {
                return false;
            }
           
        }
    }
}
