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
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Part;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class DefectComponentRejudge : MarshalByRefObject, IDefectComponentRejudge
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region DefectComponentRejudge Members
        /// <summary>
        /// </summary>                
        public ArrayList GetDefectComponentInfo(string sn, string customer, string status, string user)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            ArrayList retValue = new ArrayList();
            string currentSessionKey = sn;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Common, user, status, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", status);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", user);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);

                    IList<string> customerList = iPartRepository.GetValueFromSysSettingByName("Customer");
                    IList<string> custcodeList = iPartRepository.GetValueFromSysSettingByName("CustomerCode");
      
                    if (customerList != null && customerList.Count > 0)
                    {
                        customer = customerList[0];
                    }

                    string custcode = custcodeList[0];
                    string firstcode = sn.Substring(0, 1);
                   // if (firstcode != custcode)
                   // {
                    //    throw new FisException("CQCHK50108", new string[] { sn, customer });
                   // }
                    currentSession.AddValue("PartSerialNo", sn);
                    //currentSession.AddValue("custsn", custsn);
                    currentSession.AddValue("custcode", custcode);
                    currentSession.AddValue("Status", status);
                    
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("DefectComponentRejudge.xoml", "", wfArguments);


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
                IList<DefectComponentInfo> retList = (IList<DefectComponentInfo>)currentSession.GetValue("DefectComponentInfo");
                IList<string> recList = (IList<string>)currentSession.GetValue("RecycleList");
                string statusDesc = (string)currentSession.GetValue("StatusDesc");
                DefectComponentInfo defectInfo = retList[0];
                string recycle = recList[0];

                retValue.Add(defectInfo);
                retValue.Add(recycle ?? "");
                retValue.Add(statusDesc ?? "");
                return retValue;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        public void Save(string sn, string status, string comment)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = sn;
                Session session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.Exception = null;
                    session.AddValue("Status", status);
                    session.AddValue("Comment", comment);
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
                
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public DataTable GetQuery(string partSN, string customer)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
			logger.DebugFormat("BEGIN: {0}()", methodName);
            IList<DefectComponentLogInfo> ret = new List<DefectComponentLogInfo>();
			try	 
            {
			    string strSQL = @"select a.PartSn, c.PartType, a.ActionName, a.Customer, 
                                            a.Model, a.Family, 
                                            d.ProductID as SourcePrdId, 
                                            a.DefectCode+' - '+ a.DefectDescr as DefectDescr, 
                                            a.ReturnLine, a.[Status]+' - '+b.Value as [Status],
                                            a.Editor, a.Cdt
                                    from DefectComponentLog a
                                    inner join ConstValue b on b.[Type]='DCStatus'and 
                                    b.Name = a.[Status]
                                    inner join DefectComponent c on a.ComponentID = c.ID
                                    inner join ProductRepair d on a.RepairID = d.ID
                                    where a.PartSn = @InputData and a.Customer =@Customer
                                    order by a.Cdt ";

                SqlParameter paraNamePartSN = new SqlParameter("@InputData", SqlDbType.VarChar, 20);
                paraNamePartSN.Direction = ParameterDirection.Input;
                paraNamePartSN.Value = partSN;

                SqlParameter paraNameCustomer = new SqlParameter("@Customer", SqlDbType.VarChar, 10);
                paraNameCustomer.Direction = ParameterDirection.Input;
                paraNameCustomer.Value = customer;

                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                                                            strSQL, paraNamePartSN, paraNameCustomer);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    DefectComponentLogInfo item = new DefectComponentLogInfo();
                //    item.PartSn = dr["PartSn"].ToString().Trim();
                //    item.BatchID = dr["PartType"].ToString().Trim();
                //    item.ActionName = dr["ActionName"].ToString().Trim();
                //    item.Customer = dr["Customer"].ToString().Trim();
                //    item.Model = dr["Model"].ToString().Trim();
                //    item.Family = dr["Family"].ToString().Trim();
                //    item.Comment = dr["SourcePrdId"].ToString().Trim();
                //    item.DefectDescr = dr["DefectDescr"].ToString().Trim();
                //    item.ReturnLine = dr["ReturnLine"].ToString().Trim();
                //    item.Status = dr["Status"].ToString().Trim();
                //    item.Editor = dr["Editor"].ToString().Trim();
                //    DateTime time = DateTime.Parse(dr["Editor"].ToString());
                //    item.Cdt = time;
                //}
                return dt;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
				throw new Exception(e.mErrmsg); 
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
				throw;					
            }
			finally
			{
				logger.DebugFormat("END: {0}()", methodName);
			}
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">uutSn</param>
        public void Cancel(string sn)
        {
            logger.Debug("(DismantlePilotRun)Cancel Start," + "SN:" + sn);
            try
            {
                string sessionKey =sn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Common);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DismantlePilotRun)Cancel End," + "SN:" + sn);
            }

        }


        #endregion
    }

}
