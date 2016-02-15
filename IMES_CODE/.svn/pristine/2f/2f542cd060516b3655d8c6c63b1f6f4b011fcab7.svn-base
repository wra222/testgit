// INVENTEC corporation (c)2009 all rights reserved. 
// Description: OffLine Print ComMB bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-22   Yuan XiaoWei                 create
// Known issues:
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

//[assembly: log4net.Config.DOMConfigurator(Watch = true)]
namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现IPrintComMB接口, PrintComMB站BLL实现类
    /// </summary>
    public class PrintComMB : MarshalByRefObject, IPrintComMB
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.MB;

        #region IPrintComMB Members

        IList<PrintItem> IPrintComMB.Print(string PCBNo, int multiQty, string editor, string line, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("Print start, PCBNo:" + PCBNo + " , multiQty:" + multiQty + " , editor:" + editor + " , line:" + line + " , station:" + station + " , customer:" + customer);

            try
            {
                Session currentPCBSession = SessionManager.GetInstance.GetSession(PCBNo, currentSessionType);

                if (currentPCBSession == null)
                {
                    currentPCBSession = new Session(PCBNo, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", PCBNo);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentPCBSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "004PrintComMB.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentPCBSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentPCBSession.AddValue(Session.SessionKeys.Qty, multiQty);
                    currentPCBSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentPCBSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentPCBSession))
                    {
                        currentPCBSession.WorkflowInstance.Terminate("Session:" + PCBNo + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(PCBNo);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentPCBSession.WorkflowInstance.Start();
                    currentPCBSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(PCBNo);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentPCBSession.Exception != null)
                {
                    if (currentPCBSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentPCBSession.ResumeWorkFlow();
                    }

                    throw currentPCBSession.Exception;
                }
                return (IList<PrintItem>)currentPCBSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("Print end, PCBNo:" + PCBNo + " , multiQty:" + multiQty + " , editor:" + editor + " , line:" + line + " , station:" + station + " , customer:" + customer);
            }

        }

        #endregion
    }
}
