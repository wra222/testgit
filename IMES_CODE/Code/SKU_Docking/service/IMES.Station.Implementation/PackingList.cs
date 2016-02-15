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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public partial class PackingList : MarshalByRefObject, IPackingList, IPackingListForControl
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();        


        #region IPackingList Members

        public IList<string> WFStart(string pdLine, string station, string editor, string customer)
        {
            logger.Debug("PackingList WF start, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PackingList.xoml", "PackingList.rules", wfArguments);

                    currentCommonSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                IList<string> resList = new List<string>();

                resList.Add(currentSessionKey);

                return resList;
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
                logger.Debug("PackingList end, pdLine:" + pdLine + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);

            }
        }

        public void WFCancel(string sessionKey)
        {
            try
            {
                logger.Debug("PackingList Cancel start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (currentSession != null)
                {
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("PackingList Cancel end, sessionKey:" + sessionKey);
            }
        }


        public IList<string> PackingListForOBQuery(string pdline, string station, string editor, string customer,
                                        string region, string carrier, string begintime, string endtime)
        {
            IList<string> dnList = new List<string>();
            DateTime dtBegin = new DateTime();
            DateTime dtEnd = new DateTime();

            string[] begArray = begintime.Split('-');
            string[] endArray = endtime.Split('-');
            
            dtBegin = Convert.ToDateTime(begintime);
            dtEnd = Convert.ToDateTime(endtime);

            
            try
            {
                logger.Debug("(PackingList)PackingListForOBQuery start, Region:" + region + " Carrier:" + carrier);
                if (carrier == "ALL")
                {
                    // select distinct left(a.InternalID,10) from v_PAKComn a,[PAK.PAKEdi850raw] b 
                    // where a.REGION = @region and a.ACTUAL_SHIPDATE between convert(nvarchar(10),@Cdt,120) 
                    // and convert(nvarchar(10),@Edt,120)  and a.PO_NUM=b.PO_NUM
                    dnList = repPizza.GetInternalIdsFromVPakComn(region, dtBegin, dtEnd);
                }
                else
                {
                    // select distinct left(a.InternalID,10) from v_PAKComn a,[PAK.PAKEdi850raw] b
                    // where a.REGION = @region and a.ACTUAL_SHIPDATE between convert(nvarchar(10),@Cdt,120) 
                    // and convert(nvarchar(10),@Edt,120)  and a.PO_NUM=b.PO_NUM and a.INTL_CARRIER = @carrier
                    dnList = repPizza.GetInternalIdsFromVPakComn(region, dtBegin, dtEnd, carrier);
                    //ForTest
                    //dnList.Add(region);
                    //dnList.Add(carrier);
                    //dnList.Add(begintime);
                    //dnList.Add(endtime);
                }
                return dnList;
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
                logger.Debug("(PackingList)PackingListForOBQuery end, Region:" + region + " Carrier:" + carrier);
            }
        }


        public ArrayList PackingListForOBCheck(string pdline, string station, string editor, string customer, 
                                string data, string doc_type, string region, string carrier, string sessionKey)
        {

            logger.Debug("PackingList WF start, pdLine:" + pdline + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);
            string currentSessionKey = Guid.NewGuid().ToString();
            ArrayList retValue = new ArrayList();
            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PackingList.xoml", "PackingList.rules", wfArguments);
                    
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

                    currentCommonSession.AddValue("Data", data);
                    currentCommonSession.AddValue("Doc_type", doc_type);
                    currentCommonSession.AddValue("Region", region);
                    currentCommonSession.AddValue("Carrier", carrier);

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
                //6.5 10or16
                string typeInfo = (string)currentCommonSession.GetValue(Session.SessionKeys.VCode);

                data = (string)currentCommonSession.GetValue("Data");

                //6.18 BOL
                if (typeInfo != "BOL")
                {
                    ArrayList fileInfo = new ArrayList();
                    fileInfo = (ArrayList)currentCommonSession.GetValue("PDFAndTemp");

                    retValue.Add(data);
                    retValue.Add(fileInfo);
                    //6.5
                    retValue.Add(typeInfo);
                }
                else
                {
                    //6.18 BOL
                    ArrayList fileInfo = new ArrayList();
                    fileInfo = (ArrayList)currentCommonSession.GetValue("BOLPdfList");

                    retValue.Add(data);
                    retValue.Add(fileInfo);
                    retValue.Add(typeInfo);
                }

                return retValue;
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
                logger.Debug("PackingList end, pdLine:" + pdline + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer);

            }
        }        

        #endregion



        #region IPackingListControl Members
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDocTypeList(string service)
        {
            IList<string> res = new List<string>();
            IPizzaRepository repPizza =
                    RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            if (service == "PackingList")
            {
                res = repPizza.GetDocCatsFromPakPakRT("Pack List");
            }
            else if (service == "PackingListForPL")
            {
                //ITC-1360-1036  "Pack List-Transportation"->"Pack List- Transportation"
                res = repPizza.GetDocCatFromPakDotParRt("Pack List", "Pack List- Transportation");
            }

            return res;
        }

        /// <summary>
        /// </summary>
        public IList<string> GetRegionList()
        {
            IList<string> res = new List<string>();
            IPizzaRepository repPizza =
                    RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            res = repPizza.GetRegionsFromVPakComn();

            return res;
        }

        /// <summary>
        /// </summary>
        public IList<string> GetCarrierList()
        {
            IList<string> res = new List<string>();
            IPizzaRepository repPizza =
                    RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

            res = repPizza.GetIntlCarrierListFromVPakComn();
            return res;
        }
        #endregion


        public ArrayList CheckSN(string sn, string flag, int count, string doctype, string pdline, string station, string editor, string customer)
        {            
            try
            {
                logger.Debug("PackingListForPL WF start, pdLine:" + pdline + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer + " ,sn:" + sn);
                ArrayList retValue = new ArrayList();
                string currentSessionKey = sn;
                try
                {
                    Session currentProductSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                    if (currentProductSession == null)
                    {
                        currentProductSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                        Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                        wfArguments.Add("Key", currentSessionKey);
                        wfArguments.Add("Station", station);
                        wfArguments.Add("CurrentFlowSession", currentProductSession);
                        wfArguments.Add("Editor", editor);
                        wfArguments.Add("PdLine", pdline);
                        wfArguments.Add("Customer", customer);
                        wfArguments.Add("SessionType", Session.SessionType.Product);
                        WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PackingListForPL.xoml", "", wfArguments);
                        
                        currentProductSession.AddValue("CurrentStationCombineQty", count);
                        currentProductSession.AddValue("Doc_type", doctype);
                        currentProductSession.AddValue("Print_Flag", flag);
                        currentProductSession.AddValue(Session.SessionKeys.CustSN, sn);
                            
                        currentProductSession.SetInstance(instance);

                        if (!SessionManager.GetInstance.AddSession(currentProductSession))
                        {
                            currentProductSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(currentSessionKey);
                            ex = new FisException("CHK020", erpara);
                            throw ex;
                        }

                        currentProductSession.WorkflowInstance.Start();
                        currentProductSession.SetHostWaitOne();
                    }
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }


                    if (currentProductSession.Exception != null)
                    {
                        if (currentProductSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentProductSession.ResumeWorkFlow();
                        }

                        throw currentProductSession.Exception;
                    }

                    ArrayList fileInfo = new ArrayList();
                    fileInfo = (ArrayList)currentProductSession.GetValue("PDFAndTemp");

                    retValue.Add(sn);
                    retValue.Add(fileInfo);


                    return retValue;
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
                    logger.Debug("PackingListForPL WF end, pdLine:" + pdline + " ,editor:" + editor + " ,station:" + station + " ,customer:" + customer + " ,sn:" + sn);
                }
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
                logger.Debug("Get Product ID & SN By PrSN");
            }
        }


        public void insertPrintListTable(string list, int count)
        {
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

            if (!String.IsNullOrEmpty(list))
            {
                string[] pdfList = list.Split(';');

                foreach (string temp in pdfList)
                {
                    if (!String.IsNullOrEmpty(temp))
                    {
                        PrintListInfo item = new PrintListInfo();
                        item.doc_Name = temp;
                        item.cdt = DateTime.Now;
                        if (count == 1)
                        {
                            repository.InsertPrintListInfo(item);
                        }
                        else if (count == 2)
                        {
                            repository.InsertPrintListInfo(item);
                            repository.InsertPrintListInfo(item);
                        }
                    }
                }
            }

            return;
        }
    }
}
