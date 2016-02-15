/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
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
using IMES.Station.Interface.CommonIntf;
using System.Collections;

using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _ProIdPrint : MarshalByRefObject, IProIdPrint, IModel, IMO
    {
        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IProIdPrint Members

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

        public ArrayList PrintProId(string pdLine, string mo, int qty, string ecr, int month, string pCode, string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(_ProIdPrint)PrintProId start, pdLine:" + pdLine + " mo:" + mo + " qty:" + qty + " ecr:" + ecr + " month:" + month.ToString() + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = mo;
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
                    RouteManagementUtils.GetWorkflow(stationId, "014ProIdPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    if (month == 0)
                    {
                        Session.AddValue(Session.SessionKeys.IsNextMonth, false);
                    }
                    else
                    {
                        Session.AddValue(Session.SessionKeys.IsNextMonth, true);
                    }

                    Session.AddValue(Session.SessionKeys.Qty, qty);
                    Session.AddValue(Session.SessionKeys.ECR, ecr);
                    Session.AddValue(Session.SessionKeys.MONO, mo);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PCode, pCode);

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


                IList<PrintItem> printLists = this.getPrintList(Session);
                ArrayList returnList = new ArrayList();
                returnList.Add((IList<string>)Session.GetValue(Session.SessionKeys.ProdNoList));
                returnList.Add(printLists);
                return returnList;
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
                logger.Debug("(_ProIdPrint)PrintProId end, pdLine:" + pdLine + " mo:" + mo + " qty:" + qty + " ecr:" + ecr + " month:" + month.ToString() + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }


        #endregion
        private IList<PrintItem> getPrintList(Session session)
        {
            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }
                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region IModel Members

        public IList<IMES.DataModel.ModelInfo> GetModelList(string familyId)
        {
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            return (List<IMES.DataModel.ModelInfo>)modelRepository.GetModelListFor014_RecentOneMonth(familyId);
        }

        #endregion

        #region IMO Members

        public IList<IMES.DataModel.MOInfo> GetMOList(string modelId)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            return (IList<IMES.DataModel.MOInfo>)moRepository.GetMOListFor014(modelId);
        }

        #endregion

        #region IMO Members


        public MOInfo GetMOInfo(string MOId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

