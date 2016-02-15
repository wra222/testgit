// INVENTEC corporation (c)2010 all rights reserved. 
// Description:  IECLabelPrint 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-28   zhu lei                      create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Warranty;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现IIECLabelPrint接口，IECLabelPrint实现类,实现IECLabelPrint打印和重印功能
    /// </summary>
    public class IECLabelPrint : MarshalByRefObject, IIECLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Product;

       // #region IIECLabelPrint Members

        /// <summary>
        /// 用model启动工作流
        /// </summary>
        /// <param name="dataCode"></param>
        /// <param name="config"></param>
        /// <param name="rev"></param>
        /// <param name="qty"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        ArrayList IIECLabelPrint.Print(string dataCode, string config, string rev, string ctqty, string qty, string line, string editor, string station, string customer, List<PrintItem> printItems)
        {
            logger.Debug(" Print start, dataCode:" + dataCode);
            ArrayList retLst = new ArrayList();
            try
            {
                //var currentProduct = CommonImpl.GetProductByInput(model, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = dataCode;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "IECLabelPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    int num = 0;
                    if (qty.Length > 3)
                    {
                        num = Convert.ToInt32(ctqty.ToString());
                    }
                    else
                    {
                        num = Convert.ToInt32(qty.ToString())*Convert.ToInt32(ctqty.ToString());
                    }
                    //currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.WarrantyCode, dataCode);
                    currentSession.AddValue(Session.SessionKeys.AssemblyCode, config);
                    currentSession.AddValue(Session.SessionKeys.IECVersion, rev);
                    currentSession.AddValue(Session.SessionKeys.Qty, num);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(dataCode);
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
                    erpara.Add(sessionKey);
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
                IList ctLst = new ArrayList();
                ctLst = (IList)currentSession.GetValue(Session.SessionKeys.VCodeInfoLst);
                var dCode = (string)currentSession.GetValue(Session.SessionKeys.DCode);
                var printLst = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retLst.Add(ctLst);
                retLst.Add(dCode);
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
                logger.Debug(" Print end, dataCode:" + dataCode);
            }
        }

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns>string</returns>
        public string SetDataCodeValue(string model, string customer)
        {
            logger.Debug(" SetDataCodeValue start, model:" + model);
            string wrnt = "";
            string ret = "";

            try
            {
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model mdl = modelRepository.Find(model);
                wrnt = (string)mdl.GetAttribute("WRNT");
                if (wrnt != "" && wrnt != null)
                {
                    var wrntLeft = wrnt.Substring(0, 1);
                    IWarrantyRepository wr = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
                    IList<Warranty> warrantys = wr.GetDCodeRuleListForKP(customer);
                    // 通过model获得datecode
                    if (wrntLeft == "1")
                    {
                        var value = (from w in warrantys
                                    where w.WarrantyCode == "4"
                                    select w.Descr.ToString()).ToArray();
                        if (value.Count() > 0)
                        {
                            ret = value[0].ToString();
                        }
                    }
                    else
                    {
                        var value = (from w in warrantys
                                    where w.WarrantyCode == "5"
                                    select w.Descr.ToString()).ToArray();
                        if (value.Count() > 0)
                        {
                            ret = value[0].ToString();
                        }
                    }
                }

                return ret;
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
                logger.Debug(" SetDataCodeValue end, model:" + model);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void IIECLabelPrint.Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

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

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="vendorCT"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        public ArrayList ReprintLabel(string vendorCT, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(UnitLabelPrintImpl)ReprintLabel start, [vendorCT]:" + vendorCT
                + " [line]: " + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = vendorCT;
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("IECLabelReprint.xoml", string.Empty, wfArguments);

                    session.AddValue(Session.SessionKeys.VCode, vendorCT);
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

                IList<PrintItem> printLst = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                string dCode = (string)session.GetValue(Session.SessionKeys.DCode);
                string rev = (string)session.GetValue(Session.SessionKeys.IECVersion);

                retLst.Add(dCode);
                retLst.Add(rev);
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
                logger.Debug("(UnitLabelPrintImpl)ReprintLabel end, [vendorCT]:" + vendorCT
                    + " [line]: " + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }
        }
    }
}