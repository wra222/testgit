/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PickCardImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-27   zhu lei           Create 
 * Known issues:Any restrictions about this file 
 * TODO:
 * 编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using System.Collections;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Route;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IFRULabelPrint接口的实现类
    /// </summary>
    public class FRULabelPrintImpl : MarshalByRefObject, IFRULabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType commonSessionType = Session.SessionType.Common;

        #region IFRULabelPrint members

        /// <summary>
        /// InputModel
        /// </summary>
        /// <param name="model">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customer">string</param>
        /// <param name="items">PrintItem</param>
        /// <returns></returns>
        public ArrayList InputModel(string model, string editor, string stationId, string customer, IList<PrintItem> items)
        {
            logger.Debug("(FRULabelPrintImpl)InputModel start, [editor]:" + editor + " [station]:" + stationId + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = Guid.NewGuid().ToString();
            ArrayList lstRet = new ArrayList();
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, commonSessionType, editor, stationId, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个TruckID对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", commonSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "FRULabelPrint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    session.AddValue(Session.SessionKeys.PrintItems, items);
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }
                IList<PrintItem> printList = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);
                lstRet.Add((IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems));
                return lstRet;
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
                logger.Debug("(FRULabelPrintImpl)InputModel end, [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, commonSessionType);

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

        #endregion

        #region IFRULabelPrint Members

        public ArrayList CheckModel(string model)
        {
            logger.Debug("(FRULabelPrintImpl)CheckModel starts");
            ArrayList lstRet = new ArrayList();
            string mb, bompn;
            try
            {
                string strSQL = @"select Value from ModelInfo where Model = @Model and Name='MB' and RTRIM(Value) <> '' ";
     
                SqlParameter paraNameType = new SqlParameter("@Model", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                if (dt.Rows.Count == 0)
                {
                    //throw new FisException("This Model is not set ModelInfo.Name = MB ");
                    //return false;
                    mb = "";
                }
                else
                {
                    mb = dt.Rows[0]["Value"].ToString().Trim();
                }
                strSQL = @"select Value from ModelInfo where Model = @Model and Name='BomPn' and RTRIM(Value) <> '' ";
                DataTable dt2 = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                if (dt2.Rows.Count == 0)
                {
                    //throw new FisException("This Model is not set ModelInfo.Name = BomPn ");
                    //return false;
                    bompn = "";
                }
                else
                {
                    bompn = dt2.Rows[0]["Value"].ToString().Trim();
                }
                lstRet.Add(true);
                lstRet.Add(mb);
                lstRet.Add(bompn);
                return lstRet;
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
                logger.Debug("(FRULabelPrintImpl)CheckModel end");
            }
        }

        #endregion
    }
}
