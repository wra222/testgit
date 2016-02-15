using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Material;
namespace IMES.Station.Implementation
{
    public class SeaReturn : MarshalByRefObject, ISeaReturn
    {


        private const Session.SessionType TheType = Session.SessionType.Common;

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// GetProId
        /// </summary>
        /// <returns></returns>
        public string GetProId(string ct, string returnType, string editor)
        {
            string ErrorText = "Error Executing SP";
			
			SqlParameter[] paramsArray = new SqlParameter[3];
			paramsArray[0] = new SqlParameter("@ModelType", SqlDbType.VarChar);
			paramsArray[0].Value = returnType;
            paramsArray[1] = new SqlParameter("@CT", SqlDbType.VarChar);
			paramsArray[1].Value = ct;
            paramsArray[2] = new SqlParameter("@Editor", SqlDbType.VarChar);
			paramsArray[2].Value = editor;
			
			DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, "IMES_SeaReturnRCTO", paramsArray);
			if (null != dt && dt.Rows.Count > 0)
			{
                DataRow dr = dt.Rows[0];
                if (dr.ItemArray.Length == 3)
                {
                    string status = dr[0].ToString();
                    if ("SUCCESS" == status)
                    {
                        string proId = dr[1].ToString();
                        return proId;
                    }
                    ErrorText = dr[2].ToString();
                }
			}
            throw new FisException(ErrorText);
        }

        /// <summary>
        /// InputCT
        /// </summary>
		/// <param name="ct">string</param>
        /// <param name="returnType">string</param>
        /// <param name="pdLine">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
        /// <param name="printItems">IList<PrintItem></param>
        /// <returns></returns>
        public ArrayList InputCT(string ct, string returnType, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(SeaReturn)InputProdId start, pdLine:" + pdLine + " ct:" + ct + " returnType:" + returnType +" editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret  = new ArrayList();
            try
            {
                string proId = GetProId(ct, returnType, editor);
                string sessionKey = proId;
                
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "SeaReturn.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    Session.AddValue(Session.SessionKeys.Reason, "");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, proId);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, proId);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, proId);

                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(ct);
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IList<PrintItem> resultPrintItems = Session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                ret.Add(resultPrintItems);
                ret.Add(proId);
                return ret;
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
                logger.Debug("(SeaReturn)InputProdId end, pdLine:" + pdLine + " ct:" + ct + " returnType:" + returnType +" editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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
        
    }
}
