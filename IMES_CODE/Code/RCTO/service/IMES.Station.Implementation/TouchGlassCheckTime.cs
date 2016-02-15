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
using IMES.FisObject.Common.Defect;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;


namespace IMES.Station.Implementation
{
    public class TouchGlassCheckTime : MarshalByRefObject, ITouchGlassCheckTime 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOLabelReprint Members

        public ArrayList Save(string ct, string line, string editor,
                               string station, string customer, string materialType, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            string cartonSN = "";
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(TouchGlassCheckTime) start, start ct:" + ct + " editor:" + editor + " customerId:" + customer);
                string sessionKey = ct;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "TouchGlassCheckTime.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.MaterialType, materialType);
                    currentSession.AddValue(Session.SessionKeys.MaterialCT, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.MaterialCTList, new string[] { sessionKey });

                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "TouchTime Label");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "TouchTime");
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.ShipMode, "TouchTime");
                    currentSession.AddValue("MaterialStage", "CleanRoom");
                    //string shipMode = (string)CurrentSession.GetValue(Session.SessionKeys.ShipMode) ?? "";
                    // string materialStage = (string)CurrentSession.GetValue("MaterialStage") ?? "";

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
                IList<PrintItem> printItem = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retrunValue.Add(ct);
                retrunValue.Add(printItem);
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
                logger.Debug("(TouchGlassCheckTime)Reprint end, ct:" + ct + " editor:" + editor + " station:" + station + " customerId:" + customer);

            }
        }
      
        public void CheckMaSn(string sn, string station)
        {
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Material.IMaterialRepository>();
            Material material = materialRep.Find(sn);
            if (material != null && !material.CheckMaterailStatus(station))
            {
                throw new FisException("CQCHK1068", new string[] { material.MaterialCT });
            }
        }

       

        public ArrayList RePrint(string ct, string reason, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();
            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            Material material = materialRep.Find(ct);
            if (material == null )
            {
                throw new FisException("The CT is null");
            }
            string sessionKey = ct;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (session == null)
                {
                    session = new Session(sessionKey, Session.SessionType.Product, editor, stationId, ct, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "ReprintCartonLabel.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintLogName, "TouchTime Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, ct);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, ct);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }


                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                retvaluelist.Add(resultPrintItems);

                string printLabel = (string)session.GetValue(Session.SessionKeys.PrintLogName);

                retvaluelist.Add(ct);

                return retvaluelist;

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);

                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {

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
                logger.Debug("TouchGlassCheckTime(Cancel) start, sessionKey:" + sessionKey);
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
                logger.Debug("TouchGlassCheckTime(Cancel) end, sessionKey:" + sessionKey);
            }

        }



        #endregion
    }
}
