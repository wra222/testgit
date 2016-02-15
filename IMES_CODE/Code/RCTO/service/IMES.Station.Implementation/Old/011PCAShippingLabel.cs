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
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    public class _PCAShippingLabel : MarshalByRefObject, IPCAShippingLabel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private const string Station = "30";
        private const Session.SessionType TheType = Session.SessionType.MB;

        #region IPCAShippingLabel Members

        public void InputMBSn(string pdLine,
            string dCode,
            string MB_SNo,
            string editor, string stationId, string customerId,string no1397)
        {
            logger.Debug("(PCAShippingLabel)InputMBSn start, pdLine:" + pdLine + " dCode:" + dCode + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "011PCAShippingLabel.xoml", "011PCAShippingLabel.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    Session.AddValue(Session.SessionKeys._1397No, no1397);

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

                //Session.SwitchToWorkFlow();

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
                logger.Debug("(PCAShippingLabel)InputMBSn end, pdLine:" + pdLine + " dCode:" + dCode + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }


        public IList<PrintItem> Save(string MB_SNo,string dcode,IList<PrintItem> printItems, out string wcode)
        {
            logger.Debug("(PCAShippingLabel)Save start, MB_SNo:" + MB_SNo);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                IMB theMb = (IMB)Session.GetValue(Session.SessionKeys.MB);
                IMBMO theMBMO = mbmoRepository.Find(theMb.SMTMO);
                Session.AddValue(Session.SessionKeys.MBMONO, theMb.SMTMO);
                Session.AddValue(Session.SessionKeys.MBMO, theMBMO);
                Session.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, int.Parse(dcode));

                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                //Warranty wrty = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>().Find(int.Parse(dcode));
                wcode = (string) Session.GetValue (Session.SessionKeys.DCode);

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
                logger.Debug("(PCAShippingLabel)Save end, MB_SNo:" + MB_SNo);
            }
        }

        public IList<PrintItem> Reprint(
            string pdLine,
            string dCode,
            string MB_SNo,
            string editor, string stationId, string customerId, IList<PrintItem> printItems, string reason, out string wcode)
        {
            logger.Debug("(PCAShippingLabel)Reprint start, pdLine:" + pdLine + " dCode:" + dCode + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
            IList<PrintItem> printList;
            try
            {
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("011ReprintPCAShippingLabel.xoml", null, wfArguments);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, MB_SNo);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, MB_SNo);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "");
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, int.Parse(dCode));

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
                //Warranty wrty = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>().Find(int.Parse(dCode));
                //wcode = wrty.WarrantyCode;
                wcode = (string)Session.GetValue(Session.SessionKeys.DCode);
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
                logger.Debug("(PCAShippingLabel)Reprint end, pdLine:" + pdLine + " dCode:" + dCode + " MB_SNo:" + MB_SNo + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        public IList<MatchedPartOrCheckItem> InputCheckItemData(
            string MB_SNo,
            string item)
        {
            logger.Debug("(PCAShippingLabel)InputCheckItemData start, MB_SNo:" + MB_SNo + " item:" + item);

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string sessionKey = MB_SNo;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);


                if (Session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    ArrayList returnItem = new ArrayList();

                    Session.AddValue(Session.SessionKeys.ValueToCheck, item);
                    Session.InputParameter = item;
                    Session.Exception = null;
                    Session.SwitchToWorkFlow();

                    //check workflow exception
                    if (Session.Exception != null)
                    {
                        if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            Session.ResumeWorkFlow();
                        }
                        throw Session.Exception;
                    }

                    IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                    //get matchedinfo
                    IList<IBOMPart> bomPartlist = (IList<IBOMPart>)Session.GetValue(Session.SessionKeys.MatchedParts);
                    if ((bomPartlist != null) && (bomPartlist.Count > 0))
                    {
                        foreach (IBOMPart bompartitem in bomPartlist)
                        {
                            MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                            tempMatchedPart.PNOrItemName = bompartitem.PN;
                            tempMatchedPart.CollectionData = bompartitem.MatchedSn;
                            tempMatchedPart.ValueType = bompartitem.ValueType;
                            MatchedList.Add(tempMatchedPart);
                        }
                        return MatchedList;
                    }
                    else
                    {
                        ICheckItem citem = (ICheckItem)Session.GetValue(Session.SessionKeys.MatchedCheckItem);
                        if (citem == null)
                        {
                            throw new FisException("MAT010", new string[] { item });
                        }
                        else
                        {
                            MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                            tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                            tempMatchedPart.CollectionData = citem.ValueToCollect;
                            tempMatchedPart.ValueType = "";
                            MatchedList.Add(tempMatchedPart);
                            return MatchedList;
                        }
                    }
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
                logger.Debug("(PCAShippingLabel)InputCheckItemData end, MB_SNo:" + MB_SNo + " item:" + item);
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
                //throw new  SysException(e);
            }
        }

        #region IPCAShippingLabel Members


        public IList<string> Get1397No(string Mbsn)
        {
            IMBRepository bmRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = bmRep.Find(Mbsn);

            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<string> _1397No = bomRepository.Get1397NOList(mb.Model);
            //if ((_1397No == null) || (_1397No.Count == 0))
            //{
            //    return new List<string> { string.Empty };
            //}
            //else
            //{
            return _1397No;
            //}
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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

        public IList<BomItemInfo> GetCheckItemList(string sessionKey)
        {
            Session tempSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (tempSession == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }

            IList<IMES.DataModel.BomItemInfo> retLst = CommonImpl.GetCheckItemList(tempSession);
            return retLst;

        }

        #endregion
    }
}