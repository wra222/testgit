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

    public class PrintBottomCaseLabel : MarshalByRefObject,IPrintBottomCaseLabel
    {
     private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPrintBottomCaseLabel Members

        /// <summary>
        /// 打印下殼標籤
        /// </summary>
        /// <param name="inputSn">sn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="outputSn">输出mac,model</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        public IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId,string DenyStationList, IList<PrintItem> printItems,out string mac,out string model,out string sku,out string imei,out string imsi,out string iccid)
        {
            logger.Debug("(PrintBottomCaseLabel)Print start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);
            mac = "";
            model = "";
            sku = "";
            imsi = "";
            imei = "";
            iccid = "";
            var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
            string prodId = currentProduct.ProId;
            FisException ex;
            List<string> erpara = new List<string>();
            //================================== 
            if (string.IsNullOrEmpty(prodId))
            {
                 erpara.Add(custsn);
                throw new FisException("SFC011", erpara);
            }
 
            mac = currentProduct.MAC.Trim();
            model = currentProduct.Model.Trim();
            sku = GetSKU(prodId);
            string CurrStation_Status = GetCurrentStation(prodId).Trim();
            string stnBatteryInput = DenyStationList.Split(',')[DenyStationList.Split(',').Length - 1].Trim();
            GetImeiImsiIccid(prodId, out imei,out imsi,out iccid);
            if (!string.IsNullOrEmpty(CurrStation_Status))
            {
                string CurrStation = CurrStation_Status.Split(',')[0].ToString().Trim();
                string Status = CurrStation_Status.Split(',')[1].ToString().Trim();
             //   string StationName = CurrStation_Status.Split(',')[2].ToString().Trim();
                if (CurrStation == stnBatteryInput & Status == "0")
                { 
                    erpara.Add(custsn);  
                    erpara.Add(CurrStation);
                    throw new FisException("CHK173", erpara);
                }
                DenyStationList = DenyStationList.Replace(stnBatteryInput, "");// 將Battery Input站移除
                if (DenyStationList.Contains(CurrStation))
                {
                    erpara.Add(custsn);
                    erpara.Add(CurrStation);
                    throw new FisException("CHK174", erpara);
                }
            } 
            else
            {
              throw new FisException("No Product Status Data");
            }
          
        
          
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
                    RouteManagementUtils.GetWorkflow(stationId, "125PrintBottomCaseLabel.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);


                    Session.AddValue(Session.SessionKeys.Product, currentProduct);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, custsn);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, mac + "-" + model);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "BottomCase");
                   
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
                logger.Debug("(PrintBottomCaseLabel)Print end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }

        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
        /// 

        private void GetImeiImsiIccid(string prodid,out string imei,out string imsi,out string iccid)
        {
             imei = "";
             imsi = "";
             iccid = "";
             string strSQL = @" select top 1 IMEI,IMSI,ICCID from TestBoxDataLog where ProductID=@prodid order by Cdt desc  ";

            SqlParameter paraName = new SqlParameter("@prodid", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = prodid;
            SqlDataReader dr= SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            if (dr.HasRows)
            {
                dr.Read();
                imei = dr[0].ToString();
                imsi = dr[1].ToString();
                iccid = dr[2].ToString();
            }

        }
        
        private string GetSKU(string prodid)
        {
            string result = "";
            string strSQL = @" select top 1 InfoValue from ProductInfo where InfoType='SKU' and ProductID=@prodid order by Cdt desc";

            SqlParameter paraName = new SqlParameter("@prodid", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = prodid;
            object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            if (obj != null)
            {
                result = obj.ToString().Trim();
            }
            return result;
        
        }

        private string GetCurrentStation(string proid)
        {
            string result = "";
            string strSQL = @" select Station+',' +RTRIM([Status]) from ProductStatus where  ProductID=@proid";
                                         
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

        #region IPrintBottomCaseLabel 成員

       

        #endregion
    }
}
