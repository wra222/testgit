// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Pallet Collection RCTO
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-06   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using System.Collections;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class PalletCollection : MarshalByRefObject, IPalletCollection
    {
        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region IPalletCollection Members


        /// <summary>
        /// 
        /// </summary>
        /// <param name="carton"></param>
        /// <param name="floor"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList InputCarton(string carton, string floor, string editor, string line, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug(" InputCarton start, carton:" + carton);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(carton, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(carton, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", carton);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PalletCollection.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.Floor, floor);
                    currentSession.AddValue(Session.SessionKeys.Carton, carton);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + carton + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(carton);
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
                    erpara.Add(carton);
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

                ArrayList result = new ArrayList();

                PalletCollectionUI UIObject = currentSession.GetValue(Session.SessionKeys.PalletCollectionUI) as PalletCollectionUI;
                result.Add(UIObject);

                string[] CartonArray = currentSession.GetValue(Session.SessionKeys.CartonNoList) as string[];
                result.Add(CartonArray);

                IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                result.Add(resultPrintItems);


                return result;

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
                logger.Debug(" InputCarton end, carton:" + carton);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="carton"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList Reprint(string carton, string reason, string editor, string line, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("Reprint start, [carton]:" + carton
              + " [line]: " + line
              + " [editor]:" + editor
              + " [station]:" + station
              + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = carton;
            ArrayList retLst = new ArrayList();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);
                if (session == null)
                {
                    session = new Session(sessionKey, currentSessionType, editor, station, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PalletCollectionReprint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.Carton, carton);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Reason, reason);
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

                IList<PrintItem> printLst = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                string PalletNo = session.GetValue(Session.SessionKeys.PalletNo) as string;

                retLst.Add(PalletNo);
                retLst.Add(printLst);

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
                logger.Debug("Reprint end, [carton]:" + carton
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }


        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}
