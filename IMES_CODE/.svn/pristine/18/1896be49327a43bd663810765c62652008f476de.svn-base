// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  CombineKPCT bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-06   Yuan XiaoWei                 create
// 2010-01-30   Yuan XiaoWei                 ITC-1122-0045 
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 实现ICombineKPCT接口, CombineKPCT站BLL实现类
    /// </summary>
    public class CombineKPCT : MarshalByRefObject, ICombineKPCT
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;


        #region ICombineKPCT Members

        /// <summary>
        /// 根据选择的KP type和线别得到已结合总数，进行显示
        /// (只有Customer是TSB时才需要调用本方法)
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="KPType"></param>
        /// <returns>已结合总数</returns>
        int ICombineKPCT.GetCombinedQty(string pdLine, string KPType)
        {
            DateTime currentTime = DateTime.Now;

            string currentDayStr = currentTime.ToString("yyyy-MM-dd");
            string lastDayStr = currentTime.AddDays(-1).ToString("yyyy-MM-dd");

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;

            if (string.IsNullOrEmpty(pdLine))
            {
                return 0;
            }

            if (pdLine.StartsWith("PS"))
            {
                if (currentTime.Hour >= 8 && (currentTime.Hour < 20 || (currentTime.Hour == 20 && currentTime.Minute < 30)))
                {
                    startTime = DateTime.Parse(currentDayStr + " 08:00");
                    endTime = currentTime;
                }
                else if ((currentTime.Hour > 20 || (currentTime.Hour == 20 && currentTime.Minute >= 30)))
                {
                    startTime = DateTime.Parse(currentDayStr + " 20:30");
                    endTime = currentTime;
                }
                else
                {
                    startTime = DateTime.Parse(currentDayStr + " 00:00");
                    endTime = currentTime;
                }
            }
            else if (pdLine.Length > 2)
            {
                if (pdLine.Substring(2, 1) == "D")
                {
                    if (currentTime.Hour >= 8)
                    {
                        startTime = DateTime.Parse(currentDayStr + " 08:00");
                        endTime = DateTime.Parse(currentDayStr + " 20:30");
                    }
                    else
                    {
                        startTime = DateTime.Parse(lastDayStr + " 08:00");
                        endTime = DateTime.Parse(lastDayStr + " 20:30");
                    }

                }
                else if (pdLine.Substring(2, 1) == "N")
                {
                    if (currentTime.Hour < 20 || (currentTime.Hour == 20 && currentTime.Minute < 30))
                    {
                        startTime = DateTime.Parse(lastDayStr + " 20:30");
                        endTime = DateTime.Parse(currentDayStr + " 07:30");
                    }
                    else
                    {
                        startTime = DateTime.Parse(currentDayStr + " 20:30");
                        endTime = DateTime.Parse(currentTime.AddDays(1).ToString("yyyy-MM-dd") + " 07:30");
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

            var currentPartSNRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.PartSn.IPartSnRepository, IMES.FisObject.Common.PartSn.PartSn>();
            return currentPartSNRepository.GetCombineCountByPartType(KPType, startTime, endTime);
        }

        /// <summary>
        /// 启动工作流，检查CT SN的合法性,检查通过后KPType不可更改
        /// 如果当前选择的KPType是Battery，通过后输入BatteryPN,调用InputBatteryPN
        /// 如果当前选择的KPType不是Battery，通过后输入Vendor CT SN ,调用InputVendorCTSN
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="KPType"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="pdLine"></param>
        /// <param name="customer"></param>
        void ICombineKPCT.InputIECCTSN(string IECCTSN, string KPType, string editor, string station, string pdLine, string customer, string pcode)
        {
            logger.Debug("InputIECCTSN start, IECCTSN:" + IECCTSN + " KPType" + KPType + " editor" + editor);

            try
            {
                Session currentCTSession = SessionManager.GetInstance.GetSession(IECCTSN, currentSessionType);

                if (currentCTSession == null)
                {
                    currentCTSession = new Session(IECCTSN, currentSessionType, editor, station, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个IECCTSN对应一个workflow
                    wfArguments.Add("Key", IECCTSN);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCTSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "090CombineKPCT.xoml", "090CombineKPCT.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentCTSession.AddValue(Session.SessionKeys.KPType, KPType);
                    currentCTSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentCTSession.AddValue(Session.SessionKeys.PCode, pcode);
                    currentCTSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentCTSession))
                    {
                        currentCTSession.WorkflowInstance.Terminate("Session:" + IECCTSN + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(IECCTSN);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentCTSession.WorkflowInstance.Start();
                    currentCTSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(IECCTSN);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentCTSession.Exception != null)
                {
                    if (currentCTSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCTSession.ResumeWorkFlow();
                    }

                    throw currentCTSession.Exception;
                }

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
                logger.Debug("InputIECCTSN end, IECCTSN:" + IECCTSN + " KPType" + KPType + " editor" + editor);
            }
        }

        /// <summary>
        /// 如果选择的KPType是Battery，调用该方法检查输入的BatteryPN是否和CTSN的PN相同
        /// 通过后调用InputVendorCTSN
        /// 未通过可再刷BatteryPN，再调用本方法
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="batteryPN"></param>
        void ICombineKPCT.InputBatteryPN(string IECCTSN, string batteryPN)
        {
            logger.Debug("InputBatteryPN start, IECCTSN:" + IECCTSN + " batteryPN:" + batteryPN);

            try
            {
                Session currentCTSession = SessionManager.GetInstance.GetSession(IECCTSN, currentSessionType);

                if (currentCTSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(IECCTSN);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentCTSession.AddValue(Session.SessionKeys.BatteryPN, batteryPN);
                    currentCTSession.AddValue(Session.SessionKeys.CheckBattery, true);
                    currentCTSession.Exception = null;
                    currentCTSession.SwitchToWorkFlow();

                    if (currentCTSession.Exception != null)
                    {
                        if (currentCTSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentCTSession.ResumeWorkFlow();
                        }
                        throw currentCTSession.Exception;
                    }
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
                logger.Debug("InputBatteryPN end, IECCTSN:" + IECCTSN + " batteryPN:" + batteryPN);
            }
        }

        /// <summary>
        /// 输入VendorSN，检查合法性，检查通过后更新PartSN表
        /// 未通过可再刷vendorCTSN，再调用本方法
        /// </summary>
        /// <param name="IECCTSN"></param>
        /// <param name="vendorCTSN"></param>
        void ICombineKPCT.InputVendorCTSN(string IECCTSN, string vendorCTSN)
        {
            logger.Debug("InputVendorCTSN start, IECCTSN:" + IECCTSN + " vendorCTSN:" + vendorCTSN);

            try
            {
                Session currentCTSession = SessionManager.GetInstance.GetSession(IECCTSN, currentSessionType);

                if (currentCTSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(IECCTSN);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentCTSession.AddValue(Session.SessionKeys.VendorSN, vendorCTSN);
                    currentCTSession.AddValue(Session.SessionKeys.CheckBattery, false);
                    currentCTSession.Exception = null;
                    currentCTSession.SwitchToWorkFlow();

                    if (currentCTSession.Exception != null)
                    {
                        if (currentCTSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            currentCTSession.ResumeWorkFlow();
                        }
                        throw currentCTSession.Exception;
                    }
                }

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
                logger.Debug("InputVendorCTSN end, IECCTSN:" + IECCTSN + " vendorCTSN:" + vendorCTSN);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="IECCTSN"></param>
        void ICombineKPCT.Cancel(string IECCTSN)
        {
            try
            {
                logger.Debug("Cancel start, IECCTSN:" + IECCTSN);

                Session currentCTSession = SessionManager.GetInstance.GetSession(IECCTSN, currentSessionType);

                if (currentCTSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentCTSession);
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
                logger.Debug("Cancel end, IECCTSN:" + IECCTSN);
            }
        }

        #endregion

        private Session.SessionType GetStationType(string station)
        {

            var currentStationRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
            var currentStation = currentStationRepository.Find(station);
            if (currentStation == null)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(station);
                ex = new FisException("CHK019", erpara);
                throw ex;
            }
            return (Session.SessionType)currentStation.OperationObject;

        }
    }
}
