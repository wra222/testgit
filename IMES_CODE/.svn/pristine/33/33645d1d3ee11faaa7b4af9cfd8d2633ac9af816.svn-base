// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  HDDTest bll
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-04   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.HDDCopyInfo;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// HDDTest站的BLL实现类，实现IHDDTest接口
    /// </summary>
    public class HDDTest : MarshalByRefObject, IHDDTest
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int ConnectorMAXUseTimes = 600;
        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        #region IHDDTest members

        /// <summary>
        /// 保存硬盘拷贝测试信息
        /// </summary>
        /// <param name="machineNo"></param>
        /// <param name="originalHDD"></param>
        /// <param name="connectNo"></param>
        /// <param name="testHDDSn"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        void IHDDTest.TestHDD(string machineNo, string originalHDD, string connectNo, string testHDDSn, string editor, string line, string station, string customer,string pcode)
        {
            logger.Debug(" TestHDD start, testHDDSn:" + testHDDSn);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(testHDDSn, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(testHDDSn, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", testHDDSn);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "091HDDTest.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MachineNo, machineNo);
                    currentSession.AddValue(Session.SessionKeys.OriginalHDD, originalHDD);
                    currentSession.AddValue(Session.SessionKeys.ConnectNo, connectNo);
                    currentSession.AddValue(Session.SessionKeys.PCode, pcode);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + testHDDSn + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(testHDDSn);
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
                    erpara.Add(testHDDSn);
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
                logger.Debug(" TestHDD end, testHDDSn:" + testHDDSn);
            }
        }

        /// <summary>
        /// 检查Connector使用次数是否超过600次
        /// </summary>
        /// <param name="currentConnectNo"></param>
        void IHDDTest.CheckConnector(string currentConnectNo)
        {
            IHDDCopyInfoRepository currentHDDRepository = RepositoryFactory.GetInstance().GetRepository<IHDDCopyInfoRepository, HDDCopyInfo>();

            int ConnectorUseTimes = currentHDDRepository.GetCountByConnectorNo(currentConnectNo);
            if (ConnectorUseTimes >= ConnectorMAXUseTimes)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentConnectNo);
                ex = new FisException("CHK018", erpara);
                throw new Exception(ex.mErrmsg);
            }
        }
        #endregion
    }
}
