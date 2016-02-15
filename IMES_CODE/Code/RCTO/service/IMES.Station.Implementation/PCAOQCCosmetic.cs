using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Defect;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class PCAOQCCosmetic : MarshalByRefObject, IPCAOQCCosmetic
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IPCAOQCCosmetic Members
        /// <summary>
        /// </summary>                
        public ArrayList CheckAndGetMBInfo(string mbsn, string line, string editor, string station, string customer)
        {
            logger.Debug("(PCAOQCCosmeticImpl)CheckAndGetMBInfo Start:" + mbsn);

            ArrayList retValue = new ArrayList();
            string currentSessionKey = mbsn;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.MB);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.MB, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.MB);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PCAOQCCosmetic.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.MB, mbsn);


                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                string stationDesc = (string)currentSession.GetValue(Session.SessionKeys.StationDescr);
                string lotNo = (string)currentSession.GetValue(Session.SessionKeys.LotNo);
                string lineDesc = (string)currentSession.GetValue(Session.SessionKeys.LineCode);

                string remark = (string)currentSession.GetValue(Session.SessionKeys.Remark);
                int id = (int)currentSession.GetValue(Session.SessionKeys.RepairDefectID);

                IList<string> defectLst = (IList<string>)currentSession.GetValue("DefectLst");
                IList<string> descLst = (IList<string>)currentSession.GetValue("DescLst");
                
                retValue.Add(stationDesc);
                retValue.Add(lotNo);
                retValue.Add(lineDesc);
                retValue.Add(remark);
                retValue.Add(id.ToString());
                retValue.Add(defectLst);
                retValue.Add(descLst);
                return retValue;
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
                logger.Debug("(PCAOQCCosmeticImpl)CheckAndGetMBInfo End:" + mbsn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Save(string key, string mbsn, string lotNo, string remark, string check, IList<string> list)
        {
            logger.Debug("(PCAOQCCosmeticImpl)Save start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.MBSN, mbsn);
                    session.AddValue(Session.SessionKeys.LotNo, lotNo);
                    session.AddValue(Session.SessionKeys.Remark, remark);
                    session.AddValue(Session.SessionKeys.VCode, check);
                    session.AddValue(Session.SessionKeys.DefectList, list);
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
                logger.Debug("(PCAOQCCosmeticImpl)Save end, [key]:" + key);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void WFCancel(string key)
        {
            logger.Debug("(PCAOQCCosmetic)Cancel start, [key]:" + key);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = key;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);
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
                logger.Debug("(PCAOQCCosmetic)Cancel end, [prodId]:" + key);
            }
        }


        public ArrayList Query()
        {
            logger.Debug("(PCAOQCCosmetic)Query start");
            ArrayList retValue = new ArrayList();
            try
            {
                var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IList<PcboqcrepairInfo> list = new List<PcboqcrepairInfo>();

                //参考方法：Select * from PCBOQCRepair where Status=0 order by Cdt
                PcboqcrepairInfo cond = new PcboqcrepairInfo();
                cond.status = "0";
                list = mbRepository.GetPcboqcrepairInfoList(cond);

                IList<string> lotNoLst = new List<string>();
                IList<string> pcbNoLst = new List<string>();
                IList<string> editorLst = new List<string>();
                IList<string> cdtLst = new List<string>();

                foreach (PcboqcrepairInfo temp in list)
                {
                    lotNoLst.Add(temp.lotNo);
                    pcbNoLst.Add(temp.pcbno);
                    editorLst.Add(temp.editor);
                    cdtLst.Add(temp.cdt.ToString());
                }

                retValue.Add(lotNoLst);
                retValue.Add(pcbNoLst);
                retValue.Add(editorLst);
                retValue.Add(cdtLst);

                return retValue;

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
                logger.Debug("(PCAOQCCosmetic)Query end");
            }
        }


        public ArrayList GetDefectInfo(string code)
        {
            logger.Debug("(PCAOQCCosmetic)GetDefectInfo start, [DefectCode]:" + code);
            ArrayList retValue = new ArrayList();
            try
            {
                IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();

                string desc = string.Empty;
                IList<DefectCodeInfo> defectcodeLst = new List<DefectCodeInfo>();
                DefectCodeInfo cond = new DefectCodeInfo();
                cond.Defect = code;
                defectcodeLst = defectRepository.GetDefectCodeInfoList(cond);
                if (defectcodeLst != null && defectcodeLst.Count > 0)
                {
                    desc = defectcodeLst[0].Descr;
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK908", erpara);
                    throw ex;
                }

                retValue.Add(desc);
                return retValue;
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
                logger.Debug("(PCAOQCCosmetic)GetDefectInfo end, [DefectCode]:" + code);
            }
        }

        #endregion
    }

}
