/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006            Create 
 * 2010-05-23   207006            ITC-1155-0043
 * 2010-05-23   207006            ITC-1155-0080
 * 2010-05-23   207006            ITC-1155-0098
 * 2010-05-23   207006            ITC-1155-0099
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
using IMES.FisObject.PAK.DN;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _062FRUShiptoLabelPrint : MarshalByRefObject, IFRUShiptoLabelPrint
    {
        private const Session.SessionType theType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

        #region IFRUShiptoLabelPrint Members

        public IList<PrintItem> Print(string dn, string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
//2010-05-23   207006            ITC-1155-0043
//2010-05-23   207006            ITC-1155-0080
//2010-05-23   207006            ITC-1155-0098

            logger.Debug("(_062FRUShiptoLabelPrint)Print start, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " dn:" + dn);

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
                    RouteManagementUtils.GetWorkflow(stationId, "062FRUShiptoLabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.DeliveryNo, dn);

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
                logger.Debug("(_062FRUShiptoLabelPrint)Print end, editor:" + editor + " stationId:" + stationId + " customerId:" + customerId + " dn:" + dn);

            }
        }

        public IList<DNForUI> getDNList(DNQueryCondition MyCondition)
        {
//2010-05-23   207006            ITC-1155-0099
            logger.Debug("getDNList start, DeliveryNo:" + MyCondition.DeliveryNo + " , Model:" + MyCondition.Model + " , PONo:" + MyCondition.PONo + " , ShipDateFrom:" + MyCondition.ShipDateFrom + " , ShipDateTo:" + MyCondition.ShipDateTo);

            try
            {
                IList<DNForUI> result = currentDNRepository.GetDNListByCondition(MyCondition);
                if (result != null && result.Count > 600)
                {
                    FisException fe = new FisException("CHK110", new string[] { });
                    throw fe;
                }
                return result;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("getDNList end, DeliveryNo:" + MyCondition.DeliveryNo + " , Model:" + MyCondition.Model + " , PONo:" + MyCondition.PONo + " , ShipDateFrom:" + MyCondition.ShipDateFrom + " , ShipDateTo:" + MyCondition.ShipDateTo);
            }
        }

        public IList<DNPalletQty> getPalletList(string dn)
        {
            logger.Debug("QueryPallet start, DeliveryNo:" + dn);

            try
            {
                return currentDNRepository.GetPalletList(dn);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("QueryPallet end, DeliveryNo:" + dn);
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
    }
}
