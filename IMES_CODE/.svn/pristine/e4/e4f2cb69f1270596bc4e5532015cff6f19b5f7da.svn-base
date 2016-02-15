using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Docking.Interface.DockingIntf;
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

namespace IMES.Docking.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProdIdPrintForDocking : MarshalByRefObject, IProdIdPrintForDocking
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        #region IProdIdPrintForDocking Members

        public IList<PrintItem> ProductIdPrint(string pdLine, string model, string mo,
                                          int qty, bool IsNextMonth, string editor,
                                          string station, string customer, string ecr,
                                          out IList<string> prouctIdList, IList<PrintItem> printItems)
        {
            logger.Debug(" ProdIdPrint For Docking start, mo:" + mo + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString());
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
                    RouteManagementUtils.GetWorkflow(station, "ProdIdPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentCommonSession.AddValue(Session.SessionKeys.MONO, mo);
                    currentCommonSession.AddValue(Session.SessionKeys.Qty, qty);
                    currentCommonSession.AddValue(Session.SessionKeys.IsNextMonth, IsNextMonth);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentCommonSession.AddValue(Session.SessionKeys.ModelName, model);

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

                    currentCommonSession.AddValue(Session.SessionKeys.ECR, ecr);

                    IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
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
                logger.Debug("ProdIdPrint For Docking end, mo:" + mo + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString());
            }
        }


        public IList<MOInfo> GetMOList(string model)
        {
            //IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            //return moRepository.GetMOByModel(model);
            IList<string> moLst = new List<string>();
            IMBMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            moLst = moRepository.GetMOsRecentOneMonthByModelRegardlessUdt(model);
           
            IList<MOInfo> infoLst = new List<MOInfo>();
            foreach (string temp in moLst)
            {
                MOInfo currentMOInfo = new MOInfo();
                currentMOInfo.id = temp;
                currentMOInfo.friendlyName = temp;
                infoLst.Add(currentMOInfo);
            }
            return infoLst;            
        }

        public MOInfo GetMOInfo(string MOId)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            var currentMO = moRepository.Find(MOId);

            MOInfo currentMOInfo = new MOInfo();
            currentMOInfo.qty = currentMO.Qty;
            currentMOInfo.pqty = currentMO.PrtQty;
            return currentMOInfo;
        }

        /// <summary>
        /// 获取Model列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<string> GetModelList(string family)
        {
            //IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            //return moRepository.GetModelListFromMo(family);

            IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            IList<string> list = new List<string>();
            list = myRepository.GetModelListFromMo(family);

            return list;
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

        public IList<string> GetProdIdHeader(string name)
        {
            IList<string> retList = new List<string>();

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            retList = partRep.GetValueFromSysSettingByName(name);

            return retList;
        }




        #region Reprint
        public ArrayList ProdIdRePrint(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                logger.Debug("(ProdIdPrint For Docking)RePrint start, startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " customerId:" + customer);

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
                    RouteManagementUtils.GetWorkflow(station, "ProdIdRePrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

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
                logger.Debug("(ProdIdRePrint For Docking)RePrint end: " + "startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer);

            }
        }

        #endregion




        #endregion


    }
}
