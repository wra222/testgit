// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Change Model
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Docking.Interface.DockingIntf;
using log4net;
using IMES.DataModel;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Part;

namespace IMES.Docking.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class ChangeModel : MarshalByRefObject, IChangeModel
    {

        #region IChangeModel Members

        public List<StationDescrQty> InputModel1(string model1, string editor, string line, string station, string customer)
        {
            return InputModel1(model1, editor, line, station, customer, "");
        }

        public List<StationDescrQty> InputModel1(string model1, string editor, string line, string station, string customer, string typeChangeModel)
        {
            logger.Debug(" InputModel1 start, model1:" + model1);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                // mantis 1557, Change Model(FIC-True)
                if (TypeChangeModelFIC.Equals(typeChangeModel))
                {
                    ChkChangeModelStation(model1);
                }

                if (currentSession == null)
                {
                    currentSession = new Session(model1, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", model1);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ChangeModelDocking.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + model1 + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(model1);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentSession.AddValue("TypeChangeModel", typeChangeModel);
                    currentSession.AddValue(Session.SessionKeys.Model1, model1);
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
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

                return currentSession.GetValue(Session.SessionKeys.StationTable) as List<StationDescrQty>;

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
                logger.Debug(" InputModel1 end, model1:" + model1);
            }
        }

        public void InputModel2(string model1, string model2)
        {
            logger.Debug("InputModel2 start, model1:" + model1 + " model2:" + model2);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    // mantis 1557, Change Model(FIC-True)
                    string typeChangeModel = currentSession.GetValue("TypeChangeModel") as string;
                    if (TypeChangeModelFIC.Equals(typeChangeModel))
                    {
                        if (!ChkFamilyEqual(model1, model2))
                        {
                            Cancel(model1);

                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(model1);
                            erpara.Add(model2);
                            ex = new FisException("CHM009", erpara); // @Model1和@Model2的Family不一致，不可以转换
                            throw ex;
                        }
                    }

                    currentSession.AddValue(Session.SessionKeys.Model2, model2);
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
                logger.Debug("InputModel2 end, model1:" + model1 + " model2:" + model2);

            }
        }

        public List<ProductModel> Change(string model1, string selectStation, int changeQty)
        {
            logger.Debug("Change start, model1:" + model1 + " SelectStation:" + selectStation);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(model1, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(model1);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.SelectStation, selectStation);
                    currentSession.AddValue(Session.SessionKeys.Qty, changeQty);
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

                return currentSession.GetValue(Session.SessionKeys.ProductIDListStr) as List<ProductModel>;
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
                logger.Debug("Change END, model1:" + model1 + " SelectStation:" + selectStation);

            }
        }

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

        private const Session.SessionType currentSessionType = Session.SessionType.Common;

        // mantis 1557, Change Model(FIC-True)
        public const string TypeChangeModelFIC = "FIC";
        private void ChkChangeModelStation(string model1)
        {
            string ChangeModelStation = ",";
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> cvInfo = partRepository.GetConstValueListByType("ChangeModel");
            if (cvInfo != null && cvInfo.Count > 0)
            {
                foreach (var v in cvInfo)
                    if (!(string.IsNullOrEmpty(v.name)) && v.name.Contains("ChangeModelStation"))
                        ChangeModelStation += v.value.Trim() + ",";
            }
            else
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHM007", erpara); // 请联系IE维护允许转换机型的站号
                throw ex;
            }

            string strSQL = "select a.Station from ProductStatus a inner join Product b on a.ProductID=b.ProductID where b.Model=@Model";
            SqlParameter paraName = new SqlParameter("@Model", SqlDbType.VarChar, 20);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model1;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                strSQL, paraName);
            bool bEqu = false;
            foreach (DataRow r in tb.Rows)
            {
                if ((r[0] != null) && ChangeModelStation.Contains("," + r[0].ToString().Trim() + ","))
                {
                    bEqu = true;
                    break;
                }
            }
            if (!bEqu)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(model1);
                ex = new FisException("CHM008", erpara); // Model：XXX不存在可以转换的Product
                throw ex;
            }
        }

        // mantis 1557, Change Model(FIC-True)
        private bool ChkFamilyEqual(string model1, string model2)
        {
            string strSQL = "select Family,Model from Model where Model=@Model1 or Model=@Model2";
            SqlParameter paraName1 = new SqlParameter("@Model1", SqlDbType.VarChar, 20);
            paraName1.Direction = ParameterDirection.Input;
            paraName1.Value = model1;
            SqlParameter paraName2 = new SqlParameter("@Model2", SqlDbType.VarChar, 20);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = model2;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text,
                strSQL, paraName1, paraName2);
            if (tb != null && tb.Rows.Count == 2)
            {
                if (tb.Rows[0][0].ToString().Equals(tb.Rows[1][0].ToString()))
                    return true;
            }
            return false;
        }

        #endregion
    }
}
