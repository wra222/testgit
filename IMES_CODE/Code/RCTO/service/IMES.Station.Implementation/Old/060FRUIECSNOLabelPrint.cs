/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: FRU IEC SNO label print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-02-09   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using log4net;
using System.Workflow.Runtime;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.DataModel;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IFRUIECSNOLabelPrint接口的实现类
    /// </summary>
    public class FRUIECSNOLabelPrintImpl : MarshalByRefObject, IFRUIECSNOLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Common;

        #region IFRUIECSNOLabelPrint members

        /// <summary>
        /// Input Assembly Code
        /// </summary>
        /// <param name="assemblyCode">assemblyCode</param>
        /// <param name="fruNo">fruNo(Out)</param>
        /// <param name="vcode">vcode(Out)</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        public void InputAssemblyCode(string assemblyCode, out string fruNo, out string vcode, out string partNo, out string key, string editor, string stationId, string customer)
        {
            logger.Debug("(FRUIECSNOLabelPrintImpl)InputAssemblyCode start, assemblyCode:" + assemblyCode
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = Guid.NewGuid().ToString();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, SessionType, editor, stationId, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", string.Empty);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "060FRUIECSNOLabelPrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.AssemblyCode, assemblyCode);
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

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                fruNo = (string)session.GetValue(Session.SessionKeys.FRUNO);
                vcode = (string)session.GetValue(Session.SessionKeys.VCode);
                partNo = (string)session.GetValue(Session.SessionKeys.PN111);
                key = sessionKey;
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
                logger.Debug("(FRUIECSNOLabelPrintImpl)InputAssemblyCode end, assemblyCode:" + assemblyCode
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }            
        }

        /// <summary>
        /// Print
        /// </summary>
        /// <param name="_151">_151</param>
        /// <param name="vcode">vcode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="vendorDCCode">vendorDCCode</param>
        /// <param name="Qty">Qty</param>
        /// <param name="printItems">printItems</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <param name="snList">snList</param>
        /// <returns>list of PrintItem</returns>
        public IList<IMES.DataModel.PrintItem> Print(string key, string assemblyCode, string _151, string vcode, string DateCode, string vendorDCCode, int Qty, IList<IMES.DataModel.PrintItem> printItems, string editor, string stationId, string customer, out IList<string> ctList)
        {
            logger.Debug("(FRUIECSNOLabelPrintImpl)Print start,"
                + " [assemblyCode]: " + assemblyCode
                + " [_151]: " + _151
                + " [vcode]: " + vcode
                + " [DateCode]: " + DateCode
                + " [vendorDCCode]: " + vendorDCCode
                + " [Qty]: " + Qty
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.PartNo, _151);
                    session.AddValue(Session.SessionKeys.VCode, vcode);
                    session.AddValue(Session.SessionKeys.DateCode, DateCode);
                    session.AddValue(Session.SessionKeys.VendorDCode, vendorDCCode);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Qty, Qty);

                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }

                    ctList = (IList<string>)session.GetValue(Session.SessionKeys.CTList);
                    return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                }
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
                logger.Debug("(FRUIECSNOLabelPrintImpl)Print end,"
                    + " [assemblyCode]: " + assemblyCode
                    + " [_151]: " + _151
                    + " [vcode]: " + vcode
                    + " [DateCode]: " + DateCode
                    + " [vendorDCCode]: " + vendorDCCode
                    + " [Qty]: " + Qty
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string key)
        {
            logger.Debug("(FRUIECSNOLabelPrintImpl)Cancel start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
                }
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
                logger.Debug("(FRUIECSNOLabelPrintImpl)Cancel end, [key]:" + key);
            }
        }

        #endregion
    }
}
