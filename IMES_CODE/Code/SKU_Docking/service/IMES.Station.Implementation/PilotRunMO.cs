using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using System.Data;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.UnitOfWork;
using System.Linq;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.TestLog;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Extend;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PCA.MB;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class PilotRunMO : MarshalByRefObject, IPilotRunMO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private const string CreatePilotMOWF = "CreatePilotMO.xoml";
        private const string CreatePilotMORule = "";
        private IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();

        #region PilotMo

        public IList<PilotMoInfo> GetPilotMoList(PilotMoInfo condition, string beginCdt, string endCdt)
        {
            try
            {
                DateTime beginday = DateTime.Parse(beginCdt + " 00:00:00");
                DateTime endday = DateTime.Parse(endCdt + " 23:59:59");
                IList<PilotMoInfo> ret = iMORepository.SearchPilotMo(condition, beginday, endday);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IList<ConstValueInfo> GetMOTypeList(string Type)
        {
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo condition = new ConstValueInfo();
                condition.type = Type;
                return iPartRepository.GetConstValueInfoList(condition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArrayList GenPilotRunMo(PilotMoInfo item, string startmotype, string endmotype, string customer)
        {
            logger.Debug("(PilotRunMO)GenPilotRunMo start, [inputModel]:" + item.model);
            ArrayList retValue = new ArrayList();

            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);
                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, "", "", "", "");
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("Editor", item.editor);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("SessionType", theType);
                    //IList<string> productIDList = new List<string>();

                    sessionInfo.AddValue(Session.SessionKeys.VirtualMOIdentifier, startmotype);
                    sessionInfo.AddValue(Session.SessionKeys.PilotMoSuffix, endmotype);
                    sessionInfo.AddValue(Session.SessionKeys.PilotMo, item);
                    

                    //Remoting开始,调用workflow
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(CreatePilotMOWF, CreatePilotMORule, wfArguments);
                    sessionInfo.SetInstance(instance);
                    //for generate MB no
                    if (!SessionManager.GetInstance.AddSession(sessionInfo))
                    {
                        sessionInfo.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    sessionInfo.WorkflowInstance.Start();
                    sessionInfo.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }
                    throw sessionInfo.Exception;
                }
                IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                //取session值.返回
                IList<HoldInfo> retlist = (IList<HoldInfo>)sessionInfo.GetValue(ExtendSession.SessionKeys.ProdHoldInfoList);
                retValue.Add(retlist);
                retValue.Add(sessionKey);
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
                logger.Debug("(PilotRunMO)GenPilotRunMo End, [inputModel]:" + item.model);
            }
        }

        public void UpdatePilotMO(PilotMoInfo item)
        {
            try
            {
                iMORepository.UpdatePilotMo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePilotMO(string mo)
        {
            try
            {
                iMORepository.DeletePilotMo(mo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReleasePilotMO(PilotMoInfo item)
        {
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo condition = new ConstValueInfo();
                condition.type = "PilotAllowReleaseStation";
                condition.name = item.moType;
                IList<ConstValueInfo> station = iPartRepository.GetConstValueInfoList(condition);
                if (station.Count == 0)
                {
                    UpdatePilotMO(item);
                }
                else
                {
                    string[] tmpstationlst = station[0].value.Split('~');
                    string errorMsgStationList = station[0].value.Replace("~",",");
                    IList<string> stationlst = new List<string>();
                    for (int i = 0; i < tmpstationlst.Length; i++)
                    {
                        stationlst.Add(tmpstationlst[i]);
                    }
                    if (item.moType == "FA")
                    {
                        IProductRepository iProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                        if (iProductRepository.ExistsProductInfoAndLogStation("PilotMo", item.mo, stationlst, 1))
                        {
                            UpdatePilotMO(item);
                        }
                        else
                        {
                            throw new FisException("CHK1071", new string[] { errorMsgStationList });
                        }
                    }
                    else if (item.moType == "SA")
                    {
                        IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                        if (iMBRepository.ExistsPCBInfoAndLogStation("PilotMo", item.mo, stationlst, 1))
                        {
                            UpdatePilotMO(item);
                        }
                        else
                        {
                            throw new FisException("CHK1071", new string[] { errorMsgStationList });
                        }
                    }
                    else
                    {
                        UpdatePilotMO(item);
                    }

                }

                //iMORepository.UpdatePilotMo(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }
        #endregion

        #region PilotMo_Add
        public IList<string> GetAdd_Stage()
        {
            try
            {
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> siteList = ipartRepository.GetValueFromSysSettingByName("Site");
                string site = (siteList != null && siteList.Count > 0 && !string.IsNullOrEmpty(siteList[0])) ? siteList[0].Trim() : "IPC";
                string strSQL = @"select Stage from Stage";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                if (site == "IPC")
                {
                    list.Remove("PAK");
                    list.Remove("SMT");
                }
                return list;
                //string strSQL = @"select Stage from Stage";
                //DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
                //List<string> list = new List<string>(dt.Rows.Count);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    string item = dr[0].ToString().Trim();
                //    list.Add(item);
                //}
                //return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ConstValueInfo> GetAdd_MoType(string stage)
        {
            try
            {
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo condition = new ConstValueInfo();
                condition.type = "PilotMoType";
                condition.description = stage;
                return iPartRepository.GetConstValueInfoList(condition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string[]> GetAdd_Family(string stage)
        {
            try
            {
                string strSQL;
                if (stage == "SA")
                {
                    strSQL = @"SELECT DISTINCT 
                                b.InfoValue as [MB Code], c.InfoValue as [MDL], b.InfoValue + ' ' + c.InfoValue as [DisplayName] 
                                FROM Part a, PartInfo b, PartInfo c
                                WHERE a.BomNodeType = 'MB'
                                AND a.PartNo = b.PartNo
                                AND a.PartNo = c.PartNo
                                AND b.InfoType = 'MB'
                                AND c.InfoType = 'MDL'
                                AND Upper(c.InfoValue) LIKE '%B SIDE%'
                                order by b.InfoValue";
                }
                else if (stage == "FA")
                {
                    strSQL = @"select Family as [DisplayName] ,Family as [DisplayName],Family as [DisplayName]
                                from Family
                                where CustomerID='HP'";
                }
                else
                {
                    List<string[]> lists = new List<string[]>();
                    lists.Add(new string[] { string.Empty, string.Empty });
                    return lists;
                }
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL);
                List<string[]> list = new List<string[]>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string[] item = { dr[2].ToString().Trim(), dr[0].ToString().Trim() };
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public IList<string> GetAdd_Model(string mbcode)
        {
            try
            {
                string strSQL = @"select a.PartNo as [Model] 
                                    from Part a (nolock), PartInfo b(nolock) 
                                    where a.PartNo = b.PartNo
                                    and a.BomNodeType = 'MB'
                                    and b.InfoType = 'MB'
                                    and b.InfoValue = @mbcode
                                    order by a.PartNo";
                SqlParameter paraNameType = new SqlParameter("@mbcode", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = mbcode;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Print
        public ArrayList Print(string pilotrunmo, string qty, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        {
            try
            {
                logger.Debug("(PilotRunMO)Print start, [pilotrunmo]:" + pilotrunmo);
                string sessionKey = pilotrunmo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, theType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PrintPilotMo.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);  
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, pilotrunmo + "Pilot");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, pilotrunmo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, pilotrunmo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PilotRunMOLabel");
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
                ArrayList ctLst = new ArrayList();
                var printLst = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                ctLst.Add(printLst);

                return ctLst;
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
                logger.Debug(" Print end, pilotrunmo:" + pilotrunmo);
            }
        }

        #endregion
    }
}
