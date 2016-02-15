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

    public class Print2dDoorLabel : MarshalByRefObject, IPrint2dDoorLabel
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPrint2dDoorLabel Members

        /// <summary>
        /// 打印2D Door标签
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        public IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId,string DenyStationList, IList<PrintItem> printItems)
        {
            logger.Debug("(Print2dDoorLabel)Print start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);


       //     CommonImpl cmm = new CommonImpl();
        //    IMES.DataModel.ProductInfo iProduct = cmm.GetProductInfoByCustomSn(custsn);
            var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
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

            string CurrStation_Status = GetCurrentStation(prodId).Trim();
            string stnPreTest = DenyStationList.Split(',')[DenyStationList.Split(',').Length - 1].Trim();

            if (!string.IsNullOrEmpty(CurrStation_Status))
            {
                string CurrStation = CurrStation_Status.Split(',')[0].ToString().Trim();
                string Status = CurrStation_Status.Split(',')[1].ToString().Trim();
                string descr = CurrStation_Status.Split(',')[2].ToString().Trim();
                if (descr == stnPreTest & Status == "0")
                {
                    erpara.Add(custsn);
                    erpara.Add(CurrStation);
                    throw new FisException("CHK171", erpara);
                }

                if (DenyStationList.Contains(CurrStation))
                {
                    erpara.Add(custsn);
                    erpara.Add(CurrStation);
                    throw new FisException("CHK169", erpara);
                }
            }
            else
            {
              throw new FisException("No Product Status Data");
            }
          
          
            
            // if (CurrStation=="87" | CurrStation=="89")
            //{
            //    erpara.Add(custsn);
            //    erpara.Add(CurrStation);
            //    throw new FisException("CHK169", erpara);
            
            //}
          
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
                    RouteManagementUtils.GetWorkflow(stationId, "122Print2dDoorLabel.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);


                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");
                    Session.AddValue(Session.SessionKeys.Reason, "");
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
                logger.Debug("(Print2dDoorLabel)Print end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        /// 
        private string GetCurrentStation(string proid)
        {
            string result = "";
            string strSQL = @" select  p.Station+',' +RTRIM(p.[Status]) +','+s.Descr from ProductStatus p,Station s
                                            where  p.Station=s.Station and  ProductID=@proid";
                                         
            SqlParameter paraName = new SqlParameter("@proid", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = proid;
            object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            if (obj!=null)
            {
                result = obj.ToString().Trim();
            }
            return result;
        
        
        }

        private bool IsPassPrTestStation(string proid)
        { //in 40 , 41 , 87 can not print
           bool result = true;
           string strSQL = @" select count(*) from ProductLog where ProductID=@proid
                                         and Station='50'  ";
           SqlParameter paraName = new SqlParameter("@proid", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = proid;
            object _ob = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            if (int.Parse(_ob.ToString()) ==0)
            {
                result = false;
            }
            return result;
        }

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
