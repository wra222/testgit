/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Output
 * UI:CI-MES12-SPEC-SA-UI Combine PCB in Lot.docx 
 * UC:CI-MES12-SPEC-SA-UC Combine PCB in Lot.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-11   Kaisheng,Zhang       Create
 * Known issues:
 * TODO：
 * UC 具体业务： 
 *               
 * UC Revision:  3382
 */
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
using IMES.Docking.Interface.DockingIntf;
using log4net;
using MBInfo = IMES.DataModel.MBInfo;
using System.Collections;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.Line;

namespace IMES.Docking.Implementation
{
    using System.Linq;
    using IMES.FisObject.Common.Part;

    /// <summary>
    /// Combine Pcb In Lot
    /// </summary> 
    /// 


    public class CombinePcbInLot : MarshalByRefObject, ICombinePcbInLot
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        #region ICombinePcbInLot Members

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <returns>model</returns>
        public ArrayList inputMBSno(string mbsno, string editor, string station, string customer)
        {
            logger.Debug("(CombinePcbInLot)inputMBSno start, mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            List<string> strstatus = new List<string>();
            ArrayList retLst = new ArrayList();

            try
            {
                string pdline = null;

                string sessionKey = mbsno;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.MB, editor, station, pdline, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombinePCBinLot.xoml", "combinepcbinlot.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MBSN, mbsno);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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
                MB currentMB = (MB)currentSession.GetValue(Session.SessionKeys.MB);
                IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

                retLst.Add(currentMB.MBStatus.Pcbid);                //mbsn
                ILineRepository iline = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                Line line = iline.Find(currentMB.MBStatus.Line);
                if (line !=null)
                {
                    retLst.Add(line.Id.Trim()+" " +line.Descr);      //Line + Descr
                }
                else
                {
                    retLst.Add(currentMB.MBStatus.Line);
                }
                int intLotqty = (int) currentSession.GetValue("PassQtyinlotSetting");
                retLst.Add(intLotqty);  //Pass Qty for LotSetting
                var getLotlist =  (IList<LotInfo>)currentSession.GetValue("LotListforCombinePcb");
                
                retLst.Add(getLotlist);
                for (int i = 0; i <= getLotlist.Count - 1;i++ )
                {
                    switch (getLotlist[i].status)
                    {
                        case ("0"):
                            strstatus.Add("Not full");
                            break;
                        case ("1"):
                            strstatus.Add("Combine finished");
                            break;
                        case ("2"):
                            strstatus.Add("OQC In");
                            break;
                        case ("3"):
                            strstatus.Add("Lock");
                            break;
                        case ("4"):
                            strstatus.Add("Undo");
                            break;
                        case ("9"):
                            strstatus.Add("PASS");
                            break;
                        default:
                            strstatus.Add("Unkown");
                            break;
                    }
                }
                retLst.Add(strstatus);
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
                logger.Debug("(CombinePcbInLot)inputMBSno end, mbsno:" + mbsno + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        public ArrayList save(string mbsno, string LotNo)
        {
            logger.Debug("(CombinePcbInLot)save start,"
                + " mbsno: " + mbsno
                + " LotNo:" + LotNo);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbsno;
            ArrayList retLst = new ArrayList();

            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            try
            {
                
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);
                MB currentMB = session.GetValue(Session.SessionKeys.MB) as MB;
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.LotNo, LotNo);

                    //	Check Lot Qty
                    //重新获取被选择Lot的Lot.Qty，若Lot.Qty大于UI的[Lot Qty]，
                    //则提示用户：“LotNo：XXX已满，请重新选择Lot”，并重新执行‘3.4 Get [Lot List]’
                    var lotQty = (int)session.GetValue("PassQtyinlotSetting");
                    LotInfo conLotInfock = new LotInfo();
                    conLotInfock.lotNo = LotNo;
                    IList<LotInfo> getLotInfo = iMBRepository.GetlotInfoList(conLotInfock);
                    if ((getLotInfo == null) || (getLotInfo.Count == 0))
                    {
                        erpara.Add(LotNo);
                        Cancel(mbsno);
                        ex = new FisException("CHK313", erpara); //该Lot不存在
                        throw ex;
                    }
                    try
                    {
                        //UC Update 2012/07/16  Save时增加对Lot.Status的判断
                        var strStatus = getLotInfo[0].status;
                        session.AddValue("CurrentLotStatus", strStatus);
                        if (getLotInfo[0].qty >= lotQty)
                        {
                            erpara.Add(LotNo);
                            ex = new FisException("CHK279", erpara);
                            throw ex;
                        }
                    }
                    catch (FisException ex1)
                    {
                        
                       //get list again!
                        String mbType = (string)session.GetValue("GetMBType");
                        int intDay = (int)session.GetValue("OQCTimeSpan");
                        int intLotqty = (int)session.GetValue("PassQtyinlotSetting");
                        string[] statusparam = { "1", "2" };
                        IList<LotInfo> currentLotlst = itemRepository.GetLotList(statusparam, intDay, currentMB.MBStatus.Line, currentMB.MBCode, intLotqty);
                        IList<LotInfo> retorderbylot = new List<LotInfo>();
                        if (mbType.ToUpper() == "PC")
                        {
                            retorderbylot = (from item in currentLotlst where item.type == "PC" || item.type == "FRU" select item).ToList();//orderby item.cdt
                        }
                        else
                        {
                            retorderbylot = (from item in currentLotlst where item.type == mbType select item).ToList();//orderby item.cdt
                        }
                        for (int i = 0; i <= retorderbylot.Count - 1; i++)
                        {
                            retorderbylot[i].editor = retorderbylot[i].cdt.ToString();
                        }
                        List<string> strstatus = new List<string>();
                        for (int i = 0; i <= retorderbylot.Count - 1; i++)
                        {
                            switch (retorderbylot[i].status)
                            {
                                case ("0"):
                                    strstatus.Add("Not full");
                                    break;
                                case ("1"):
                                    strstatus.Add("Combine finished");
                                    break;
                                case ("2"):
                                    strstatus.Add("OQC In");
                                    break;
                                case ("3"):
                                    strstatus.Add("Lock");
                                    break;
                                case ("4"):
                                    strstatus.Add("Undo");
                                    break;
                                case ("9"):
                                    strstatus.Add("PASS");
                                    break;
                                default:
                                    strstatus.Add("Unkown");
                                    break;
                            }
                        }
                        session.AddValue("LotListforCombinePcb", retorderbylot);
                        retLst.Add("ReloadLotList");
                        retLst.Add(ex1.mErrmsg);
                        retLst.Add(retorderbylot);
                        retLst.Add(strstatus);
                        return retLst;
                    }
                   

                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }

                }
                retLst.Add("OK");
                return retLst;
            }
            catch (FisException ex1)
            {
                logger.Error(ex1.mErrmsg);
                throw ex1;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CombinePcbInLot)save end,"
                   + " mbsno: " + mbsno
                  + " LotNo:" + LotNo);
            }
        }


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        public void Cancel(string mbsno)
        {
            logger.Debug("(CombinePcbInLot)Cancel Start," + "mbsno:" + mbsno);
            try
            {
                string sessionKey = mbsno;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException ex)
            {
                logger.Error(ex.mErrmsg);
                throw ex;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(CombinePcbInLot)Cancel End," + "mbsno:" + mbsno);
            }

        }


        #endregion


    }

    
}
