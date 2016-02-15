/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-02-02   207006            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _110FRUTravelCard : MarshalByRefObject, IFRUTravelCard
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Session.SessionType theType = Session.SessionType.Common;

        #region IFRUTravelCard Members

        public IList<PrintItem> Print(string model, int qty, bool GiftOrCarton, int pcs, int gqty, int cqty, int ys, string editor, string pdLine, string stationId, string customerId, IList<PrintItem> printItems)
        {

            logger.Debug("(FRUTravelCard)Print start, model:" + model + " editor:" + editor + " pdLine:" + pdLine + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = model;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "110FRUTravelCard.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    var modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                    var _modelObj = modelRepository.Find(model);
                    if (_modelObj == null)
                    {
                        erpara.Add(model);
                        ex = new FisException("CHK038", erpara);
                        throw ex;
                    }

                    Session.SetInstance(instance);
                    Session.AddValue(Session.SessionKeys.ModelName, model);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(FRUTravelCard)Print end, model:" + model + " editor:" + editor + " pdLine:" + pdLine + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        #endregion
    }
}
