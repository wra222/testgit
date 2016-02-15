// INVENTEC corporation (c)2012 all rights reserved. 
// Description: ICT Input Interface
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   Yuan XiaoWei                 create
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0416
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0429
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0413 New Request
// 2012-02-14   Yuan XiaoWei                 ITC-1360-0853 New Request
// Known issues:
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using MBInfo = IMES.DataModel.MBInfo;
using System.Collections;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class ICTInput : MarshalByRefObject, IICTInput
    {
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// scan mbsno
        /// </summary>
        /// <param name="MBSno"></param>
        /// <param name="ecr"></param>
        /// <param name="warrantyID"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public void InputMBSno(string MBSno, string ecr, int warrantyID, string editor, string line, string station, string customer)
        {
            logger.Debug(" InputMBSno start, MBSno:" + MBSno);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(MBSno, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(MBSno, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", MBSno);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ICTInput.xoml", "ICTInput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.ECR, ecr);
                    currentSession.AddValue(Session.SessionKeys.SelectedWarrantyRuleID, warrantyID);
                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + MBSno + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(MBSno);
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
                    erpara.Add(MBSno);
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
                logger.Debug(" InputMBSno end, MBSno:" + MBSno);
            }
        }

        /// <summary>
        /// save
        /// </summary>
        /// <param name="IsRCTO"></param> 
        /// <param name="MBSno"></param>
        /// <param name="aoi"></param>
        /// <param name="defectList"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList Save(bool IsRCTO, string MBSno, string aoi, IList<string> defectList, IList<PrintItem> printItems)
        {
            logger.Debug("Save start, key:" + MBSno);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(MBSno, currentSessionType);
                string CheckCode = MBSno.Substring(MBSno.Length - 5, 1);
                if (MBSno.Substring(4, 1) == "M")
                {
                    CheckCode = MBSno.Substring(5, 1);
                }
                else
                {
                    CheckCode = MBSno.Substring(6, 1);
                }

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(MBSno);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    if (!string.IsNullOrEmpty(aoi))
                    {
                        currentSession.AddValue(Session.SessionKeys.AOINo, aoi);
                    }
                    currentSession.AddValue(Session.SessionKeys.DefectList, defectList);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsRCTO, IsRCTO);
                    

                    currentSession.AddValue(Session.SessionKeys.CheckCode, CheckCode);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                ArrayList result = new ArrayList();

                int qty = (int)currentSession.GetValue(Session.SessionKeys.Qty);
                result.Add(qty);
                if (defectList != null && defectList.Count > 0)
                {
                    IList<PrintItem> noPrintItems = new List<PrintItem>();
                    result.Add(noPrintItems);
                }
                else
                {
                    IList<PrintItem> resultPrintItems = currentSession.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                    result.Add(resultPrintItems);
                }

                List<string> childMBSnList = currentSession.GetValue(Session.SessionKeys.MBNOList) as List<string>;
                string NewMBSno = MBSno;
                if (IsRCTO && CheckCode != "R")
                {
                    childMBSnList = currentSession.GetValue(Session.SessionKeys.RCTOChildMBSnList) as List<string>;
                    NewMBSno = currentSession.GetValue(Session.SessionKeys.PrintLogBegNo) as string;
                }

                
                if (childMBSnList != null)
                {
                    result.Add(childMBSnList);
                }
                else
                {
                    childMBSnList = new List<string>();
                    childMBSnList.Add(NewMBSno);
                    result.Add(childMBSnList);
                }

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
                logger.Debug("Save end, key:" + MBSno);
            }
        }


        /// <summary>
        /// 取消workflow
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
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

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.MB;

        #region ECR Reprint
        /// <summary>
        /// Reprint Input mbSno, Whether this mb can be reprinted.
        /// </summary>
        /// <returns>MBInfo</returns>
        public MBInfo EcrReprintInputMBSno(string mbSno, string editor, string line, string station, string customer)
        {
            _logger.Debug("(ICTInput)EcrReprintInputMBSno start, MB_SNo:" + mbSno);

            try
            {
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                var prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                var rptRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                var mb = mbRepository.Find(mbSno);
                if (mb == null || mb.MBStatus == null)
                {
                    var ex = new FisException("SFC001", new[] { mbSno });
                    throw ex;
                }

                //c.	如果MB 已经投入到FA 生产，则报告错误：“此MB 已经投入到FA 生产，不能Reprint!!“
                var productByMBSn = prodRepository.GetProductByMBSn(mbSno);
                if (productByMBSn != null)
                {
                    var ex = new FisException("CHK013", new string[] { });
                    throw ex;
                }

                //MBSno是否在修复select * from PCBRepair nolock where PCBNo = @MBSno and Status = '0' 若存在，则报错：“请先修复后，再打印Label”
                if (null != mb.GetCurrentRepair())
                {
                    var ex = new FisException("CHK853", new string[] { });
                    throw ex;
                }

                //检查PrintLog记录是否存在，若不存在，则报错：“没有打印记录，不能重印”
                IList<PrintLog> PrintLogList = rptRepository.GetPrintLogListByRange(mbSno, "ECR Label");
                if (PrintLogList == null || PrintLogList.Count == 0)
                {
                    throw new FisException("CHK270", new string[] { mbSno });
                }

                var ret = new MBInfo { id = mbSno, ecr = mb.ECR, dateCode = mb.DateCode };
                return ret;
            }
            catch (FisException e)
            {
                _logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
            finally
            {
                _logger.Debug("(ICTInput)EcrReprintInputMBSno end, MB_SNo:" + mbSno);
            }
        }

        /// <summary>
        /// reprint ecr label
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public IList<PrintItem> EcrReprint(string mbSno, string reason, string editor, string line, string station, string customer, IList<PrintItem> printItems)
        {
            //todo: make sure what is the key condition of this label

            _logger.Debug("(ICTInput)EcrReprint start, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " station:" + station + " customerId:" + customer);

            try
            {
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                var mb = mbRepository.Find(mbSno);
                if (mb == null)
                {
                    var ex = new FisException("SFC001", new[] { mbSno });
                    throw ex;
                }

                string sessionKey = mbSno;
                const Session.SessionType sessionType = Session.SessionType.Common;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, sessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, sessionType, editor, station, line, customer);
                    var wfArguments = new Dictionary<string, object>
                                          {
                                              {"Key", sessionKey},
                                              {"Station", ""},
                                              {"CurrentFlowSession", session},
                                              {"Editor", editor},
                                              {"PdLine", line},
                                              {"Customer", customer},
                                              {"SessionType", sessionType}
                                          };

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ECRReprint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    session.AddValue(Session.SessionKeys.MB, mb);
                    //session.AddValue(Session.SessionKeys.MBMONO, mb.SMTMO);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.PrintLogName, "ECRLabel");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, mb.Sn);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, mb.Sn);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        var ex = new FisException("CHK020", new[] { sessionKey });
                        throw ex;
                    }
                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    var ex = new FisException("CHK020", new[] { sessionKey });
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

                return (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
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
                _logger.Debug("(ICTInput)EcrReprint end, MBSno:" + mbSno + " Reason:" + reason + " editor:" + editor + " customerId:" + customer);
            }
        }

        #endregion


        #region MB Reinput
        /// <summary>
        /// input mbSno for MB reinput
        /// </summary>
        /// <returns>MBInfo</returns>
        public MBInfo MBReinputInputMBSno(string mbSno, string editor, string line, string station, string customer)
        {
            _logger.Debug("(ICTInput)MBReinputInputMBSno start, MB_SNo:" + mbSno);

            try
            {
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                var prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                var mb = mbRepository.Find(mbSno);
                if (mb == null || mb.MBStatus == null)
                {
                    var ex = new FisException("SFC001", new[] { mbSno });
                    throw ex;
                }

                //b.	检查MB SNo第六位，若为“R”，则是RCTO的板子不予通过
                if (mb.IsRCTO)
                {
                    var ex = new FisException("CHK856", new string[] { });
                    throw ex;
                }
                //e.	检查MB的当前状态(PCBStatus.Status)，若为“0”，则报错：“该MB有Fail，请先修复后再重流”
                if (mb.MBStatus.Status == MBStatusEnum.Fail)
                {
                    var ex = new FisException("ICT019", new string[] { });
                    throw ex;
                }
                //f.	检查MB的当前站（PCBStatus.Station），若为“S9、20、21、22、23”，则报错：“修复中，请修复完毕后再重流
                //      若为“CL”，则报错：“该MB的生命周期已结束”；
                //      若为“28”，则报错：“该MB已经报废，不能再使用”。
                //      若为“P0、09”，则报错：“未经过ICT测试，不能重流
                if (mb.MBStatus.Station == "S9"
                    || mb.MBStatus.Station == "20"
                    || mb.MBStatus.Station == "21"
                    || mb.MBStatus.Station == "22"
                    || mb.MBStatus.Station == "23"
                    )
                {
                    var ex = new FisException("CHK854", new string[] { });
                    throw ex;
                }
                if (mb.MBStatus.Station == "CL")
                {
                    var ex = new FisException("ICT011", new string[] { });
                    throw ex;
                }
                if (mb.MBStatus.Station == "28")
                {
                    var ex = new FisException("ICT012", new string[] { });
                    throw ex;
                }
                if (mb.MBStatus.Station == "P0"
                    || mb.MBStatus.Station == "09"
                    )
                {
                    var ex = new FisException("ICT020", new string[] { });
                    throw ex;
                }
                //g.	检查MB是否已经结合,若存在，则报错：“该MB已经结合，不能重投”
                var productByMBSn = prodRepository.GetProductByMBSn(mbSno);
                if (productByMBSn != null)
                {
                    var ex = new FisException("CHK855", new string[] { });
                    throw ex;
                }

                //c.	检查MB 是否不良
                if (null != mb.GetCurrentRepair())
                {
                    var ex = new FisException("BOR005", new string[] { });
                    throw ex;
                }

                var properties = new Dictionary<string, string>();
                properties.Add("MBCT", (string)mb.GetExtendedProperty("MBCT"));
                var ret = new MBInfo();
                ret.id = mbSno;
                ret.ecr = mb.ECR;
                ret.mac = mb.MAC;
                ret.dateCode = mb.DateCode;
                ret.properties = properties;
                return ret;
            }
            catch (FisException e)
            {
                _logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
            finally
            {
                _logger.Debug("(ICTInput)MBReinputInputMBSno end, MB_SNo:" + mbSno);
            }
        }

        /// <summary>
        /// save for MB reinput
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void MBReinputSave(string mbSno, string editor, string line, string station, string customer)
        {
            //todo: make sure what is the key condition of this label

            _logger.Debug("(ICTInput)MBReinputSave start, MBSno:" + mbSno + " editor:" + editor + " station:" + station + " customerId:" + customer);

            try
            {
                string sessionKey = mbSno;
                const Session.SessionType sessionType = Session.SessionType.MB;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, sessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, sessionType, editor, station, line, customer);
                    var wfArguments = new Dictionary<string, object>
                                          {
                                              {"Key", sessionKey},
                                              {"Station", "10"},
                                              {"CurrentFlowSession", session},
                                              {"Editor", editor},
                                              {"PdLine", line},
                                              {"Customer", customer},
                                              {"SessionType", sessionType}
                                          };

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "MBReinput.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        var ex = new FisException("CHK020", new[] { sessionKey });
                        throw ex;
                    }
                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    var ex = new FisException("CHK020", new[] { sessionKey });
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
                _logger.Debug("(ICTInput)MBReinputSave end, MBSno:" + mbSno + " editor:" + editor + " customerId:" + customer);
            }
        }

        #endregion
    }
}
