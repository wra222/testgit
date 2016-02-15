using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class ITCNDCheck : MarshalByRefObject, IITCNDCheck
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IITCNDCheck Members
        /// <summary>
        /// </summary>                
        public ArrayList CheckImageDL(IList<PrintItem> printItems, string pdline, string prodid, string station, string editor, string customer)
        {
            logger.Debug("ITCND Check:" + pdline + "," + prodid);            
            ArrayList retValue = new ArrayList();
            string currentSessionKey = prodid;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<string> valueList = partRep.GetValueFromSysSettingByName("Site");
                    string site = "";
                    if (valueList.Count > 0)
                    { site = valueList[0]; }


                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);                    

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ITCNDCheck.xoml", "ITCNDCheck.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue("Site", site);
                    if (!"ICC".Equals(site))
                    {
                        //win8 oa3
                        string fkiPath = System.Configuration.ConfigurationManager.AppSettings["FKIServicePath"].Trim();
                        string fkiUser = System.Configuration.ConfigurationManager.AppSettings["FKIAuthUser"].Trim();
                        string fkiPwd = System.Configuration.ConfigurationManager.AppSettings["FKIAuthPassword"].Trim();

                        currentSession.AddValue("FKIPATH", fkiPath);
                        currentSession.AddValue("FKIUSER", fkiUser);
                        currentSession.AddValue("FKIPWD", fkiPwd);
                        //win8 oa3
                    }

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                string customerSN = string.Empty;
                string model = string.Empty;
                customerSN = currentProduct.CUSTSN;
                model = currentProduct.Model;
                var items = (IList<string>)currentSession.GetValue("ITCNDCheckItems");
                var values = (IList<string>)currentSession.GetValue("ITCNDCheckValues");
                string result = (string)currentSession.GetValue(Session.SessionKeys.ValueToCheck);
                IList<PrintItem> printParams = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                if (result == "PASS")
                {
                    string setMsg = (string)currentSession.GetValue(ExtendSession.SessionKeys.WarningMsg);
                    if ((string)currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) == "Y")
                    {
                        setMsg = setMsg + "  BSaM機器，請收電池!";
                    }
                    int action = (int)currentSession.GetValue(Session.SessionKeys.Action);
                    retValue.Add(customerSN);
                    retValue.Add(model);
                    retValue.Add(items);
                    retValue.Add(items.Count);
                    retValue.Add(values);
                    retValue.Add(values.Count);
                    retValue.Add(result);
                    retValue.Add(printParams);
                    retValue.Add(action.ToString());
                    retValue.Add(currentProduct.ProId);
                    retValue.Add(setMsg);
                }
                else
                {
                    if (values[0] == "EXIST")
                    {
                        List<string> errpara = new List<string>();
                        if (items != null && items.Count > 0)
                            errpara.Add(items[0]);
                        else
                            errpara.Add("");
                        FisException e = new FisException("CHK832", errpara);
                        throw e;
                    }
                    else if (values[0] == "UNIQUE")
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(items[0]);
                        FisException e = new FisException("CHK833", errpara);
                        throw e;
                    }
                }

                return retValue;
            }
            catch (FisException e)
            {
                throw e;
            }

            catch (System.Net.WebException )
            {
                throw new SystemException("無法連接HP OA3 Server,請聯繫測試PE處理");
            }

            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("ITCNDCheck Success!");
            }            
        }


        #region Reprint
        public ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            //string mo = "";
            try
            {
                logger.Debug("(ITCNDCheck)Reprint start, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
                // Check Prodid....
                // Check Prodid....

                string sessionKey = prodid;
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
                    RouteManagementUtils.GetWorkflow(station, "ITCNDCheckRePrint.xoml", "ITCNDCheckRePrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid); 
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
                string linecode = (string)Session.GetValue(Session.SessionKeys.LineCode);
                string id = (string)Session.GetValue(Session.SessionKeys.ProductIDOrCustSN);
                string action = (string)Session.GetValue(Session.SessionKeys.MaintainAction);
                var currentProduct = (IProduct)Session.GetValue(Session.SessionKeys.Product);

                retrunValue.Add(action);
                retrunValue.Add(currentProduct.ProId);
                retrunValue.Add(linecode);
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
                logger.Debug("(ITCNDCheck)Reprint end, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }

        #endregion

        /// <summary>
        /// </summary>        
        public IList<string> GetPdLinePass(string pdline, string station, string editor, string customer)
        {
            IList<string> result = new List<string>();

            try
            {
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                DateTime today = DateTime.Now;
                DateTime yestoday = DateTime.Now.AddDays(-1);
                DateTime begin;
                DateTime end;

                DateTime fixTodayTime = new DateTime(today.Year, today.Month, today.Day, 7, 50, 0);
                DateTime fixYestodayTime = new DateTime(yestoday.Year, yestoday.Month, yestoday.Day, 7, 50, 0);

                if (today.Ticks > fixTodayTime.Ticks)
                {
                    begin = fixTodayTime;
                    end = today;
                }
                else
                {
                    begin = fixYestodayTime;
                    end = fixTodayTime;
                }

                //result.Add(begin.ToString());
                //result.Add(end.ToString());

                //TODO:
                //IList<ProductLog> infos = new List<ProductLog>();
                IList<ModelStatistics> infos = new List<ModelStatistics>();

                infos = productRepository.GetByModelStatisticsFromProductLog(1, "67", begin, end, pdline);

                foreach (ModelStatistics temp in infos)
                {
                    Model model = new Model() ;
                    model = (Model)modelRepository.Find(temp.Model);
                    if (model != null)
                    {
                        result.Add(temp.Model);
                        result.Add(temp.Qty.ToString());
                    }
                }

                return result;
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
                logger.Debug("Call ITCNDCheck->GetPdLinePass Success!");
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
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
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
    }
         #endregion
}
