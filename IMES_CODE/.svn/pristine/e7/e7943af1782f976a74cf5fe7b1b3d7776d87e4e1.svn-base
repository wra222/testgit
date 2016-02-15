using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;


namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class RULabelPrint : MarshalByRefObject, IRULabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region RULabelPrint Members

        public ArrayList CheckPalletNo(string PalletNo)
        {
            logger.Debug("(RULabelPrint)CheckPalletNo start, PalletNo:" + PalletNo);
            ArrayList retrunValue = new ArrayList();
            try
            {

                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                IList<DeliveryPalletInfo> dnPalletList = iDeliveryRepository.GetDeliveryPalletListByPlt(PalletNo);
                if (dnPalletList.Count == 0)
                {
                    throw new FisException("Delivery_Pallet is not Exist");
                }
                Pallet palletInfo = iPalletRepository.Find(PalletNo);
                if (palletInfo == null)
                {
                    throw new FisException("Pallet is not Exist");
                }

                PalletAttr palletattr = iPalletRepository.GetPalletAttr(PalletNo, "RUNoList");
                if (palletattr != null)
                {
                    throw new FisException("Please use RePrint");
                }
                int qty=0;
                foreach (DeliveryPalletInfo item in dnPalletList)
                {
                    qty += item.deliveryQty;
                }
                retrunValue.Add(PalletNo);
                retrunValue.Add(qty);
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
                logger.Debug("(RULabelPrint)CheckPalletNo end, PalletNo:" + PalletNo);
            }
        }

        public ArrayList Print(string PalletNo, string RUNoQty, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(RULabelPrint)Print start, PalletNo:" + PalletNo + " editor:" + editor + " customerId:" + customer);

                string sessionKey = PalletNo;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Common, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RULabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    //Session.SessionKeys.PalletNo
                    Session.AddValue(Session.SessionKeys.PalletNo, PalletNo);
                    Session.AddValue("RUNoQty", RUNoQty);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                retrunValue.Add(returnList);
                retrunValue.Add(PalletNo);
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
                logger.Debug("(RULabelPrint)Print end, PalletNo:" + PalletNo + " editor:" + editor + " customerId:" + customer);

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
                logger.Debug("RULabelPrint(Cancel) start, sessionKey:" + sessionKey);
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
                logger.Debug("RULabelPrint(Cancel) end, sessionKey:" + sessionKey);
            }

        }
        #endregion

        #region RULabelRePrint Members

        public ArrayList CheckRePrintPalletNo(string PalletNo)
        {
            logger.Debug("(RULabelPrint)CheckRePrintPalletNo start, PalletNo:" + PalletNo);
            ArrayList retrunValue = new ArrayList();
            try
            {

                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                IList<DeliveryPalletInfo> dnPalletList = iDeliveryRepository.GetDeliveryPalletListByPlt(PalletNo);
                if (dnPalletList.Count == 0)
                {
                    throw new FisException("Delivery_Pallet is not Exist");
                }
                Pallet palletInfo = iPalletRepository.Find(PalletNo);
                if (palletInfo == null)
                {
                    throw new FisException("Pallet is not Exist");
                }

                PalletAttr palletattr = iPalletRepository.GetPalletAttr(PalletNo, "RUNoList");
                if (palletattr == null)
                {
                    throw new FisException("Please use Print");
                }
                int qty = 0;
                foreach (DeliveryPalletInfo item in dnPalletList)
                {
                    qty += item.deliveryQty;
                }
                retrunValue.Add(PalletNo);
                retrunValue.Add(qty);
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
                logger.Debug("(RULabelPrint)CheckRePrintPalletNo end, PalletNo:" + PalletNo);
            }
        }

        public ArrayList RePrint(string palletNo, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(RULabelPrint)Reprint start, palletNo:" + palletNo + " line:" + line + " editor:" + editor + " customerId:" + customer);
                List<string> erpara = new List<string>();
                FisException ex;
                string key = palletNo;
                
                //Check Print Log
                var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


                //bool bFlag = false;

                //bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(printItems[0].LabelType, key);
                //if (!bFlag)
                //{

                //    ex = new FisException("CHK270", erpara);
                //    throw ex;
                //}

                //Check Print Log
                string sessionKey = key;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Common, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, palletNo);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, palletNo);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, palletNo);



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
                ArrayList arr = new ArrayList();
                arr.Add(returnList);
                return arr;
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
                logger.Debug("(RULabelPrint)Reprint end, palletNo:" + palletNo + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }


        #endregion
    }
}
