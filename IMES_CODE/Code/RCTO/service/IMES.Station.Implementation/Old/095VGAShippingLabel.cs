/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: VGAShippingLabel print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-02-01   Tong.Zhi-Yong     Create 
 * 2010-03-01   Tong.Zhi-Yong     Modify ITC-1103-0234
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IVGAShippingLabel接口的实现类
    /// </summary>
    public class VGAShippingLabelImpl : MarshalByRefObject, IVGAShippingLabel 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;

        #region IVGAShippingLabel

        /// <summary>
        /// 列印VGA Shipping Label。
        /// </summary>
        /// <param name="DCodeType">DCodeType</param>
        /// <param name="Version">Version</param>
        /// <param name="Qty">要打印的数量</param>
        /// <param name="VGASno">VGASno</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <returns>IList<PrintItem></returns>
        public IList<PrintItem> PrintVGAShippingLabel(string DCodeType, string Version, int Qty, string VGASno, IList<PrintItem> printItems, string editor, string stationId, string customer, out string FRUNo, out IList<string> snList, out string dcode)
        {
            logger.Debug("(VGAShippingLabelImpl)PrintVGAShippingLabel start, [DCodeType]:" + DCodeType
                + " [Version]: " + Version
                + " [Qty]: " + Qty
                + " [VGASno]: " + VGASno
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = VGASno;

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
                    RouteManagementUtils.GetWorkflow(stationId, "095PrintVGALabel.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //ITC-1103-0234 Tong.Zhi-Yong 2010-03-01
                    session.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, int.Parse(DCodeType)); 
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Qty, Qty);
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

                FRUNo = (string)session.GetValue(Session.SessionKeys.FRUNO);
                snList = (IList<string>)session.GetValue(Session.SessionKeys.SVBSnList);
                dcode = (string)session.GetValue(Session.SessionKeys.DCode);
                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(VGAShippingLabelImpl)PrintVGAShippingLabel end, [DCodeType]:" + DCodeType
                    + " [Version]: " + Version
                    + " [Qty]: " + Qty
                    + " [VGASno]: " + VGASno
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        #endregion
    }
}
