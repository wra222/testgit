// INVENTEC corporation (c)2009 all rights reserved. 
// Description: TravelCard Print bll
//                         
// Update: 
// Date         Name                         Reason  
// ==========   =======================      ============
// 2009-12-22   Yuan XiaoWei                 create
// Known issues:
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
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
namespace IMES.Station.Implementation
{
   
    public partial class TravelCardPrint : MarshalByRefObject, ITravelCardPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region ITravelCardPrint Members

        /// <summary>
        /// 根据产线生产安排，打印travel card
        /// A.	不做管控，按照选定的Model 打印；将打印出来的ProdId label 贴在travel card上，开始online作业
        /// 目的：打印travel card，作流线管控使用
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="model">Product Model</param>
        /// <param name="qty">打印数量</param>
        /// <param name="editor">operator</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>Print Items</returns> 
        public IList<PrintItem> PrintTCNoProductID(string pdLine, 
                                                                                string model, 
                                                                                int qty, string editor, 
                                                                                string station, string customer, 
                                                                                IList<PrintItem> printItems)
        {
            logger.Debug(" PrintTCNoProductID start, model:" + model + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString() + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);
            string currentSessionKey = Guid.NewGuid().ToString();
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("013TravelCardPrintNoProductID.xoml", "", wfArguments);
                 

                    currentCommonSession.AddValue(Session.SessionKeys.ModelName, model);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                logger.Debug(" PrintTCNoProductID end, model:" + model + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString() + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);

            }

        }

        /// <summary>
        /// 根据产线生产安排，打印travel card
        /// B.	在输入时需要根据MO进行管控，Travel card上会同时打印ProdId，开始online作业
        /// 目的：打印travel card，作流线管控使用
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="mo">Product MO</param>
        /// <param name="qty">打印数量</param>
        /// <param name="ecr">ECR</param>
        /// <param name="IsNextMonth">是否是下一月</param>
        /// <param name="editor">operator</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="ProuctIdList">返回所有ProuctIdList</param>
        /// <returns>Print Items</returns>
        public IList<PrintItem> PrintTCWithProductID(string pdLine, string mo, int qty, string ecr, bool IsNextMonth, string editor, string station, string customer, out IList<string> prouctIdList, IList<PrintItem> printItems)
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
                    RouteManagementUtils.GetWorkflow(station, "013travelcardprint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentCommonSession.AddValue(Session.SessionKeys.MONO, mo);
                    currentCommonSession.AddValue(Session.SessionKeys.ECR, ecr);
                    currentCommonSession.AddValue(Session.SessionKeys.Qty, qty);
                    currentCommonSession.AddValue(Session.SessionKeys.IsNextMonth, IsNextMonth);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                logger.Debug(" PrintTCWithProductID end, mo:" + mo + " ,pdLine:" + pdLine + " ,qty:" + qty.ToString());
            }
        }

        public IList<PrintItem> PrintTCWithProductIDForBN(string pdLine, string mo, 
                                                                                            int qty, bool IsNextMonth, string editor, 
                                                                                            string station, string customer, 
                                                                                            out IList<string> prouctIdList, IList<PrintItem> printItems, 
                                                                                            out string battery, out string lcm, 
                                                                                            string deliveryDate,string sku)
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
                    RouteManagementUtils.GetWorkflow(station, "013TravelCardprint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    //WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("_TEST.xoml", "", wfArguments);
                    currentCommonSession.AddValue(Session.SessionKeys.MONO, mo);
                    currentCommonSession.AddValue(Session.SessionKeys.ECR, "");
                    currentCommonSession.AddValue(Session.SessionKeys.Qty, qty);
                    currentCommonSession.AddValue(Session.SessionKeys.IsNextMonth, IsNextMonth);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentCommonSession.AddValue(ExtendSession.SessionKeys.DeliveryDate,deliveryDate);
                    currentCommonSession.AddValue(ExtendSession.SessionKeys.SKU, sku);
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

                string[] param = (string[])currentCommonSession.GetValue("CardParam");
                battery = param[0];
                lcm = param[1];
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
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);
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

        /// <summary>
        /// 获取Model列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.ModelInfo> GetModelList(string family)
        {
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            return myRepository.GetModelListFor014(family);
        }

        /// <summary>
        /// 获取Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.FamilyInfo> GetFamilyList(string customer)
        {
            IMBMORepository myRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            return myRepository.GetFamilysByCustomerWithMORecentOneMonth(customer);
        }
        public string GetSKU(string model)
        {  
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model _Mdoel = myRepository.Find(model);
            string result = _Mdoel.GetAttribute("SKU");
            if (string.IsNullOrEmpty(result))
            { return ""; }
            else
            { return result; }
        }
        public List<SerialNumber> GetPrintLogProidList(string mo)
        {
            List<SerialNumber> retLst = new List<SerialNumber>();
            IList<PrintLog> prtLst = null;
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            prtLst = repository.GetPrintLogListByDescr(mo);
            if (prtLst != null)
            {
                foreach (PrintLog s in prtLst)
                {
                   //SerialNumber sn=new SerialNumber(
                    
                    //retLst.Add(s.BeginNo + "-" + s.EndNo);
                    retLst.Add(new SerialNumber{beginNumber=s.BeginNo,endNumber=s.EndNo});
                }
            }
            return retLst;
        }
        #endregion
    }
}
