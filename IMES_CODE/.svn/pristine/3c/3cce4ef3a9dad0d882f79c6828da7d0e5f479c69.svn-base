// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  SwitchBoardLabelPrint bll
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
using IMES.DataModel;
using IMES.Route;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// SwitchBoardLabelPrint站的BLL实现类，实现ISwitchBoardLabelPrint接口
    /// </summary>
    public class SwitchBoardLabelPrint : MarshalByRefObject, ISwitchBoardLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;


        #region ISwitchBoardLabelPrint Members
        /// <summary>
        /// 取得Switch Board的Family信息列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFamilyList()
        {
            try
            {
                logger.Debug("GetFamilyList start");
                var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return currentPartRepository.GetFamilyList();
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
            { logger.Debug("GetFamilyList end"); }
        }

        /// <summary>
        /// 取得Family的PCB信息列表 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<string> GetPCBListByFamily(string family)
        {
            try
            {
                logger.Debug("GetPCBListByFamily start, family:" + family);
                var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return currentPartRepository.GetPCBListByFamily(family);
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
            { logger.Debug("GetPCBListByFamily end, family:" + family); }
        }

        /// <summary>
        /// 取得PCB的111信息列表
        /// </summary>
        /// <param name="pcb"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<string> Get111ListByPCB(string pcb, string family)
        {
            try
            {
                logger.Debug("Get111ListByPCB start, pcb:" + pcb + " family:" + family);
                var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return currentPartRepository.Get111ListByPCB(pcb, family);
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
            { logger.Debug("Get111ListByPCB end, pcb:" + pcb + " family:" + family); }
        }

        /// <summary>
        /// 取得111的FruNo信息列表
        /// </summary>
        /// <param name="pn111"></param>
        /// <returns></returns>
        public string GetFruNoBy111(string pn111)
        {
            try
            {
                logger.Debug("GetFruNoBy111 start, pn111:" + pn111);
                var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                string fruNo = currentPartRepository.GetFruNoBy111(pn111);
                if (fruNo == null)
                {
                    fruNo = "";
                }
                return fruNo;
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
            { logger.Debug("GetFruNoBy111 end, pn111:" + pn111); }
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="pn111"></param>
        /// <param name="fruNo"></param>
        /// <param name="qty"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Print(string pn111, string fruNo, int qty, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug(" Print start, pn111:" + pn111 + " fruNo:" + fruNo + " qty:" + qty.ToString() + " line:" + line + " editor:" + editor);
            try
            {
                string sessionKey = Guid.NewGuid().ToString();
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
                    RouteManagementUtils.GetWorkflow(station, "106SwitchBoardLabelPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
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

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug(" Print end, pn111:" + pn111 + " fruNo:" + fruNo + " qty:" + qty.ToString() + " line:" + line + " editor:" + editor);
            }
        }

        #endregion

    }
}
