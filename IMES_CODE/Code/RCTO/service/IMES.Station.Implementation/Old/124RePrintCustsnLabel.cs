/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: KP Print Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-22   LuycLiu     Create 

 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{

    public class RePrintCustsnLabel : MarshalByRefObject, IRePrintCustsnLabel
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members

        private string IsCorrectInputByPCB(string input)
        {
            string result = "";           
            string strSQL = "select top 1 CUSTSN from PCB where CUSTSN=@input or PCBNo=@input";
            SqlParameter paraName = new SqlParameter("@input", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = input;
            Object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName);
            if (obj != null)
            { result = obj.ToString(); }
            return result;
        }


        public IList<PrintItem> PrintForPCB(string input, string stationId, string editor, string customerId,string reason,out string custsn, IList<PrintItem> printItems)
        {
            logger.Debug("(RePrintCustsnLabel)Print start, inputCustsn:" + input + " editor:" + editor + " customerId:" + customerId);
            custsn = "";
            //============= Wrong Input  ============
            FisException ex;
            List<string> erpara = new List<string>();
            //Dean 20110627 舊資料的MB SN 為11碼
            if (input.Trim().Length == 11)
            {
                input = input.Substring(0, 10);
            }
            custsn = IsCorrectInputByPCB(input);
            if (custsn=="")
            {
                    erpara.Add(input);
                    ex = new FisException("SFC011", erpara);
                    throw ex;
            }
           //============= Wrong Input  ============

            try
            {
                string sessionKey = input;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, "", customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "124RePrintCustsnLabel.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                   // Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, input);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, input);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "ReprintByInputPCBNo");
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.WorkflowInstance.Start();
                    Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
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
                logger.Debug("(RePrintCustomerSNLabel)Print end, inputSn:" + input + " pdLine:" + "" + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }




      
        
        }

        /// <summary>
        /// 打印KP标签
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        public IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId,string reason, IList<PrintItem> printItems)
        {
            logger.Debug("(RePrintCustsnLabel)Print start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);


       //     CommonImpl cmm = new CommonImpl();
        //    IMES.DataModel.ProductInfo iProduct = cmm.GetProductInfoByCustomSn(custsn);
            var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
            string prodId = currentProduct.ProId;
            FisException ex;
            List<string> erpara = new List<string>();
            //==================================
            if (string.IsNullOrEmpty(prodId))
            {
                //List<string> errpara = new List<string>();
                erpara.Add(custsn);
                throw new FisException("SFC011", erpara);
            }
               
            
           // string temp = string.Empty;
            try
            {

                string sessionKey = custsn;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "124RePrintCustsnLabel.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");
                    Session.AddValue(Session.SessionKeys.Reason,reason);
        

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
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
                logger.Debug("(RePrintCustomerSNLabel)Print end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        private IList<PrintItem> getPrintList(Session session)
        {

            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        #endregion
    }
}
