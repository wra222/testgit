/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2010-05-20   207006            ITC-1155-0145
 * 2010-05-20   207006            ITC-1155-0054
 * 2010-05-21   207006            ITC-1155-0088
 * 2010-05-23   207006            ITC-1155-0052
 * 2010-05-23   207006            ITC-1155-0049
 * 2010-05-23   207006            ITC-1155-0056
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
using IMES.FisObject.PAK.FRU  ;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _055FRUCartonLabelPrint : MarshalByRefObject, IFRUCartonLabelPrint
    {
        private const Session.SessionType theType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFRUCartonLabelPrint Members

        public IList<PrintItem> Packing(IList<string> ctList, string editor, string stationId, string customerId, out string carton, IList<PrintItem> printItems)
        {
//2010-05-20   207006            ITC-1155-0145
//2010-05-20   207006            ITC-1155-0054
//2010-05-21   207006            ITC-1155-0088
//2010-05-23   207006            ITC-1155-0052
//2010-05-23   207006            ITC-1155-0049
//2010-05-23   207006            ITC-1155-0056
            logger.Debug("(_055FRUCartonLabelPrint)Packing start, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = System.Guid.NewGuid().ToString();

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "055FRUCartonLabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.GiftNoList, ctList);
                    string model = this.ValidateFruCT(ctList[0]);
                    Session.AddValue(Session.SessionKeys.ModelName , model);
                    int qty = ctList.Count();
                    Session.AddValue(Session.SessionKeys.Qty , qty);
                    
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

                carton = ((IList<string>)Session.GetValue(Session.SessionKeys.FRUCartonNoList))[0];
              
                IList<PrintItem> returnList = this.getPrintList(Session);
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
                logger.Debug("(_055FRUCartonLabelPrint)Packing end, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        public IList<PrintItem> Reprint(string cartonNo, string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(_055FRUCartonLabelPrint)Reprint start, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " cartonNo:" + cartonNo);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = System.Guid.NewGuid().ToString();

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("055FRUCartonLabelReprint.xoml", null, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo , cartonNo );
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo , cartonNo);
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
              
                IList<PrintItem> returnList = this.getPrintList(Session);
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
                logger.Debug("(_055FRUCartonLabelPrint)Reprint end, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " cartonNo:" + cartonNo);
            }
        }

        public void Unpack(string cartonNo, string editor, string stationId, string customerId)
        {
            logger.Debug("(_055FRUCartonLabelPrint)Unpack start, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " cartonNo:" + cartonNo);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = System.Guid.NewGuid().ToString();

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("055FRUCartonUnpack.xoml", null, wfArguments);
                    
                    Session.AddValue(Session.SessionKeys.Carton , cartonNo);
                    //Session.AddValue(Session.SessionKeys.PrintItems, printItems);

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
                logger.Debug("(_055FRUCartonLabelPrint)Unpack end, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " cartonNo:" + cartonNo);
            }
        }

        public string ValidateFruCT(string fruCT)
        {
            IGiftRepository giftRep = RepositoryFactory.GetInstance().GetRepository<IGiftRepository>();
            FRUGift fg = giftRep.Find(fruCT);
            if (fg == null)
            {
                throw new FisException("CHK104", new List<string>() { fruCT }); 
            }
            return fg.Model;
            
        }     

        public bool ValidateCartonNo(string cartonNo)
        {
            IFRUCartonRepository fruCartonRep = RepositoryFactory.GetInstance().GetRepository<IFRUCartonRepository>();
            
            FRUCarton fr = fruCartonRep.Find(cartonNo);
            if (fr == null)
            {
                throw new FisException("CHK105", new List<string>()); 
                //return false;
            }
            else
            {

                if (fr.Gifts == null || fr.Gifts.Count == 0)
                {
                    throw new FisException("CHK105", new List<string>());
                    //return false;
                }
            }

            return true;
        }
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
        #endregion
    }
}
