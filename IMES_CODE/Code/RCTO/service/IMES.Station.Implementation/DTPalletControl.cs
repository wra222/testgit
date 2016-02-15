// INVENTEC corporation (c)2012 all rights reserved. 
// Description: DTPalletControl
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-21   Yuan XiaoWei                 create
// 2012-02-21   Yuan XiaoWei                 ITC-1360-1058
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class DTPalletControl : MarshalByRefObject, IDTPalletControl
    {


        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region IDTPalletControl Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IList<WhPltLogInfo> Query(string palletNo, string from, string to)
        {
            if (!string.IsNullOrEmpty(palletNo))
            {
                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<WhPltLogInfo> result = PalletRepository.GetWhPltLogInfoList(new string[] { "DT", "RD" }, palletNo);
                return result;
            }
            else
            {

                DateTime parseToDate;
                if (DateTime.TryParse(to, out parseToDate))
                {
                    //parseToDate = parseToDate.AddDays(1d);
                }

                DateTime parseFromDate;
                DateTime.TryParse(from, out parseFromDate);

                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<WhPltLogInfo> result = PalletRepository.GetWhPltLogInfoList(new string[] { "DT", "RD" }, parseFromDate, parseToDate);


                return result;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<WhPltLogInfo> DT(string palletNo, string editor, string line, string station, string customer)
        {
            logger.Debug(" DT start, PalletNo:" + palletNo);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(palletNo, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(palletNo, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", palletNo);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    if (palletNo.StartsWith("90"))
                    {
                        RouteManagementUtils.GetWorkflow(station, "DTDummyPalletControl.xoml", "DTDummyPalletControl.rules", out wfName, out rlName);
                    }
                    else {
                        RouteManagementUtils.GetWorkflow(station, "DTPalletControl.xoml", "DTPalletControl.rules", out wfName, out rlName);
                    }
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + palletNo + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(palletNo);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, (decimal)0.0);
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(palletNo);
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

                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

                IList<WhPltLogInfo> result = PalletRepository.GetWhPltLogInfoList(new string[] { "DT", "RD" }, palletNo);

                return result;

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
                logger.Debug(" DT end, PalletNo:" + palletNo);
            }
        }

        #endregion
    }
}
