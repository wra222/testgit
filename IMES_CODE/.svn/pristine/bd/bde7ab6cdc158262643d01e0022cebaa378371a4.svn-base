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
using IMES.FisObject.Common.Part;

namespace IMES.Station.Implementation
{

    public partial class CNCardReceive : MarshalByRefObject, ICNCardReceive
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region ICNCardReceive Members

        public void CheckPartNo(string partNo)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();            
            try
            {
                logger.Debug("CNCardReceive start(CheckPartNo)");
                List<string> erpara = new List<string>();
                FisException ex;
                int len = partNo.Length;
                int index = partNo.IndexOf('-');
                if (len == 5 && index == 3)
                {
                    IPart iPart = partRep.Find(partNo);                                        
                    if (iPart == null)
                    {
                        ex = new FisException("CHK827", erpara);
                        throw ex;
                    }
                }
                else
                {
                    ex = new FisException("CHK814", erpara);
                    throw ex;
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
                logger.Debug("CNCardReceive end(CheckPartNo)");
            }            
        }



        public void Save(string partNo, string begNo, string endNo, string station, string editor, string pdline, string customer)
        {
            logger.Debug("CNCardReceive Save:" + partNo + "," + begNo + "," + endNo);
            IList<string> result = new List<string>();
            string currentSessionKey = Guid.NewGuid().ToString();
           
            
            try
            {
                logger.Debug("CNCardReceive save start!");
                if (partNo == "")
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("CHK812", erpara);
                    throw ex;
                }
                if (begNo == "" || endNo == "")
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("CHK813", erpara);
                    throw ex;
                }
                
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CNCardReceive.xoml", "", wfArguments);

                    currentCommonSession.SetInstance(instance);

                    currentCommonSession.AddValue(Session.SessionKeys.PartNo, partNo);
                    currentCommonSession.AddValue("BegNo", begNo);
                    currentCommonSession.AddValue("EndNo", endNo);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintLogName, "CNCard");
                    currentCommonSession.AddValue(Session.SessionKeys.PrintLogBegNo, begNo);
                    currentCommonSession.AddValue(Session.SessionKeys.PrintLogEndNo, endNo);
                    //ITC-1360-139
                    currentCommonSession.AddValue(Session.SessionKeys.PrintLogDescr, partNo);

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
                logger.Debug("CNCardReceive save end!");
            }
        }
       
        #endregion
    }
}
