/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0013 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Station.Implementation
{

    public class SNCheck : MarshalByRefObject, ISNCheck
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IKPPrint Members

        // mantis 1573
        private bool ChkChinaEnergyLabel(string custsn)
        {
            string strSQL = @"Select a.Material, b.Descr from ModelBOM a,Part b,Product p
                where p.CUSTSN=@CustSN and 
                a.Material=p.Model and 
                b.PartNo=a.Component and b.Descr like '%China Energy Label%'";
            SqlParameter paraName = new SqlParameter("@CustSN", SqlDbType.VarChar, 30);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = custsn;
            DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_PAK, System.Data.CommandType.Text,
                    strSQL, paraName);
            if (dt == null || dt.Rows.Count == 0)
                return false;
            return true;
        }

        private List<string> GetCartonPartNoList(string model)
        {
            string sql = @" select  a.PartNo from Part a,PartInfo b,PartInfo c,ModelBOM d 
                                       where
                                            a.PartNo=b.PartNo
                                    and a.PartNo=c.PartNo and
                                          d.Material=@model
                                    and d.Component=a.PartNo
                                    and a.BomNodeType='PL'
                                    and (b.InfoType='TYPE' and b.InfoValue ='BOX'  )
                                    and (c.InfoType='Descr' and c.InfoValue like '%Carton%') ";
            SqlParameter paraName = new SqlParameter("@model", SqlDbType.VarChar, 16);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_PAK, System.Data.CommandType.Text,
                    sql, paraName);
            List<string> lst = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { lst.Add(dr[0].ToString().Trim());}
            }
                return lst;
                
        }

		/// <summary>
        /// SN Check初次输入SN处理
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>ProductID</returns>
        public String InputCustSNOnProduct(string custsn, string pdLine, string editor, string stationId, string customerId)
		{
/*//test，2011-10-18，测试流程是否通，需删除========
string ret1 = custsn + " test";
return ret1;
//test，2011-10-18，测试流程是否通，需删除========*/

            logger.Debug("(SNCheck)InputCustSNOnProduct start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                bool IsChinaEnergyLabel = false;
                string sessionKey = custsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    // mantis 1573
                    IsChinaEnergyLabel = ChkChinaEnergyLabel(custsn);
                    if (IsChinaEnergyLabel)
                        RouteManagementUtils.GetWorkflow(stationId, "SNCheckPrint.xoml", null, out wfName, out rlName);
                    else
                        RouteManagementUtils.GetWorkflow(stationId, "SNCheck.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custsn);
                    currentSession.AddValue("IsChinaEnergyLabel", IsChinaEnergyLabel);

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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }

                Product getProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                string ret = getProduct.ProId;

                if (IsChinaEnergyLabel)
                    ret += ",IsChinaEnergyLabel";

                return ret;
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
                logger.Debug("(SNCheck)InputCustSNOnProduct end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
            
        }


		/// <summary>
        /// SN Check第二次输入SN处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void InputCustSNOnPizza(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
        {
/*//test，2011-10-24，测试流程是否通，需去掉========
IProduct currentProduct = new Product("testproduct");
currentProduct.Model = "asset tag label test";
currentProduct.CUSTSN = "customerSN 111";
string prodId = currentProduct.ProId;
//test，2011-10-24，测试流程是否通，需去掉========*/

            logger.Debug("(SNCheck)InputCustSNOnPizza start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

			FisException ex;
			List<string> erpara = new List<string>();
			
            try
            {
                string sessionKey = custsnOnProduct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, custsnOnPizza);
                    
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

				return;
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
                logger.Debug("(SNCheck)InputCustSNOnPizza end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// SN Check第二次输入SN处理，返回PAQC以及ALC/NO-ALC
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>SN Info</returns>
        public string InputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId, string cartonPn)
        {
/*//2011-10-24，测试用，须删除
string ret1 = "";
if (custsnOnPizza.Substring(0, 1) == "P")
{
    ret1 += " NO-ALC";
}
else   
{
    ret1 += "ALC";
}
return ret1;
//2011-10-24，测试用，须删除*/

            logger.Debug("(SNCheck)InputCustSNOnPizzaReturn start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

            /*//test，2011-10-18，测试流程是否通，需去掉========
            IProduct currentProduct = new Product("testproduct");
            currentProduct.Model = "asset tag label test";
            currentProduct.CUSTSN = "customerSN 111";
            string prodId = currentProduct.ProId;
            //test，2011-10-18，测试流程是否通，需去掉========*/

            FisException ex;
            List<string> erpara = new List<string>();

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);
       
            try
            {
                string sessionKey = custsnOnProduct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                 if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    if (!string.IsNullOrEmpty(cartonPn))
                    {
                        BindPart(currentProduct, cartonPn, Session);
                    }
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, custsnOnPizza);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                string IsBSamModel = sessionInfo.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;

                string ret = "";
				if ("Y".Equals(IsBSamModel))
				{
					ret = " BSAM";
				}
                else if (custsnOnPizza.Substring(0, 1) == "P")
                {
                    ret = " NO-ALC";
                }
                else    
                {
                    ret = "ALC";
                }
                //   IList<ProductQCStatus> qcStatusList = currentProduct.QCStatus;
                IList<ProductQCStatus> qcStatusList = currentProduct.QCStatus.OrderByDescending(x=>x.Udt).ToList();
                bool printFlag = qcStatusList.Count > 0 && new[] { "8", "9", "A" }.Any(qcStatusList[0].Status.Contains);
                
              //  var printFlag = false;
              
                //if (qcStatusList.Count > 0)
                //{
                //    ProductQCStatus earlierQCStatus = qcStatusList.First<ProductQCStatus>();
                //    foreach (ProductQCStatus tempQCStatus in qcStatusList)
                //    {
                //        if (tempQCStatus.Udt.CompareTo(earlierQCStatus.Udt) >= 0)
                //        {
                //            earlierQCStatus = tempQCStatus;
                //            if ((tempQCStatus.Status == "8") || (tempQCStatus.Status == "9") || (tempQCStatus.Status == "A"))
                //            {
                //                printFlag = true;
                //            }
                //            else
                //            {
                //                printFlag = false;
                //            }
                //        }
                //    }
                //}

                if (printFlag == true)
                {
                    ret = ret + "  PAQC";
                }
    
                return ret;
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
                logger.Debug("(SNCheck)InputCustSNOnPizzaReturn end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        public IList<PrintItem> InputCustSNOnPizzaReturn(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId,string cartonPn, IList<PrintItem> printItems)
        {
            logger.Debug("(SNCheck)InputCustSNOnPizzaReturnPrint start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);

            try
            {
                string sessionKey = custsnOnProduct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogName, "EnergyLabel");
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.CUSTSN);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.CUSTSN);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogDescr, currentProduct.CUSTSN);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //原本InputCustSNOnPizzaReturn是回傳字串，現在這個要回傳List。 此empty是為了在List帶入原本要回傳的字串
                    string ret = InputCustSNOnPizzaReturn(custsnOnPizza, custsnOnProduct, pdLine, editor, stationId, customerId,cartonPn);
                    PrintItem empty = new PrintItem();
                    empty.LabelType = "SNCheckReturn";
                    empty.TemplateName = ret;

                    IList<PrintItem> pi = (IList<PrintItem>) sessionInfo.GetValue(Session.SessionKeys.PrintItems);
                    pi.Add(empty);
                    return pi;
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
                logger.Debug("(SNCheck)InputCustSNOnPizzaReturnPrint end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// SN Check第二次输入SN后判断是否一致的处理
        /// </summary>
        /// <param name="custsnOnPizza">Customer SN On Pizza</param>
        /// <param name="custsnOnProduct">Customer SN On Product</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void CheckTwoSNIdentical(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId)
        {
            logger.Debug("(SNCheck)CheckTwoSNIdentical start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);

            try
            {
                string customerSNOnProduct = custsnOnProduct.Trim();
                string customerSNOnPizza = custsnOnPizza.Trim();

                if ((customerSNOnPizza.Length >= 10) && (customerSNOnProduct.Length >= 10))
                {
                    string SNOnPizzaLastTen = customerSNOnPizza.Substring(customerSNOnPizza.Length - 10, 10);
                    string SNOnProductLastTen = customerSNOnProduct.Substring(customerSNOnProduct.Length - 10, 10);

                    if (SNOnPizzaLastTen != SNOnProductLastTen)
                    {
                        List<string> errpara = new List<string>();

                        errpara.Add(custsnOnProduct);

                        throw new FisException("CHK207", errpara);
                    }
                }
                else
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(custsnOnProduct);

                    throw new FisException("CHK207", errpara);
                }

                Session currentSession = SessionManager.GetInstance.GetSession(custsnOnProduct, TheType);
                if (null != currentSession)
                {
                    string IsBSamModel = currentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;
                    if ("Y".Equals(IsBSamModel))
                    {
                        if (customerSNOnPizza.Substring(0, 1) != "A" && customerSNOnPizza.Substring(0, 1) != "S")
                        {
                            List<string> errpara = new List<string>();

                            errpara.Add(custsnOnProduct);

                            throw new FisException("CHK207", errpara);
                        }

                        return;
                    }
                }
              /* Marked for [ICC IMES TEST 0000381]: SN Check 前面带A的sn无法刷入
              var hasALC = false;

              if (currentProduct.Model != "PC4941AAAAAY")
              {
                  IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                  IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                  for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                  {
                      IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                      if ((part.Descr == "ALC") && (part.BOMNodeType == "PL"))
                      {
                          hasALC = true;
                          break;
                      }
                  }
              }
              
              if (hasALC == true)
              {

                  if (customerSNOnPizza.Substring(0, 1) != "A" && customerSNOnPizza.Substring(0, 1) != "S")
                  {
                      List<string> errpara = new List<string>();
                      errpara.Add(custsnOnProduct);
                      throw new FisException("CHK207", errpara);
                  }
              }
              else
              {
                  if (customerSNOnPizza.Substring(0, 1) != "P" && customerSNOnPizza.Substring(0, 1) != "S")
                  {
                      List<string> errpara = new List<string>();
                      errpara.Add(custsnOnProduct);
                      throw new FisException("CHK207", errpara);
                  }
              }*/

                return;
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
                logger.Debug("(SNCheck)CheckTwoSNIdentical end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }
        public ArrayList InputCustSNOnProduct2(string custsn, string pdLine, string editor, string stationId, string customerId)
        {
            /*//test，2011-10-18，测试流程是否通，需删除========
            string ret1 = custsn + " test";
            return ret1;
            //test，2011-10-18，测试流程是否通，需删除========*/

            logger.Debug("(SNCheck)InputCustSNOnProduct start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                bool IsChinaEnergyLabel = false;
                string sessionKey = custsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    // mantis 1573
                    IsChinaEnergyLabel = ChkChinaEnergyLabel(custsn);
                    if (IsChinaEnergyLabel)
                        RouteManagementUtils.GetWorkflow(stationId, "SNCheckPrint.xoml", null, out wfName, out rlName);
                    else
                        RouteManagementUtils.GetWorkflow(stationId, "SNCheck.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custsn);
                    currentSession.AddValue("IsChinaEnergyLabel", IsChinaEnergyLabel);

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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }
           
                
                Product getProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                List<string> pnList = GetCartonPartNoList(getProduct.Model);
                //txtName.Text.Trim().Length == 0 ? null : txtName.Text.Trim()
            //    string chinaLabel=IsChinaEnergyLabel?"IsChinaEnergyLabel":"";
                ArrayList arr = new ArrayList();
                arr.Add(getProduct.ProId);
                arr.Add(IsChinaEnergyLabel);
                arr.Add(pnList);
                return arr;
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
                logger.Debug("(SNCheck)InputCustSNOnProduct end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
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

        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> ReprintEnergyLabel(string custSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(SNCheck EnergyLabel)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK936", erpara); //CustSn:%1 不存在
                    throw ex;
                }

                //if (!ChkChinaEnergyLabel(custSN))
                //{
                //    FisException ex;
                //    List<string> erpara = new List<string>();
                //    erpara.Add(custSN);
                //    ex = new FisException("CHK970", erpara); //CustSN %1 不可打印 Energy Label
                //    throw ex;
                //}

                string sessionKey = currentProduct.ProId;

                // For Mantis 379
                //PrintLog condition = new PrintLog();
                //condition.Name = "EnergyLabel";
                //condition.BeginNo = ""; 
                //var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                //IList<PrintLog> printLogList = repository.GetPrintLogListByCondition(condition);
                //if (printLogList.Count == 0)
                //{
                //    FisException ex;
                //    List<string> erpara = new List<string>();
                //    ex = new FisException("CHK971", erpara);//该Customer S/N 没有列印过Energy Label，不能Reprint!
                //    throw ex;
                //} 
                


                /*var repository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,station);
                
                if (logList.Count==0)
                {
                    
                } */

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("EnergyLabelReprint.xoml", "", wfArguments);
                    //currentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);

                    currentSession.AddValue(Session.SessionKeys.CustSN, custSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "EnergyLabel");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.CUSTSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.CUSTSN);
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, currentProduct.CUSTSN);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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
                if ((string)currentSession.GetValue("EnergyLabel") != "EnergyLabel")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK970", erpara); //CustSN %1 不可打印 Energy Label
                    throw ex;
                }

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                return printList;
                
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
                logger.Debug("(SNCheck EnergyLabel)ReprintLabel End,"
                                + " [custSN]:" + custSN
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        private void BindPart(IProduct prd,string cartonPn,Session session)
        {
                
            IPizzaRepository PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart p= partRepository.Find(cartonPn);
            Pizza pizza= PizzaRepository.Find(prd.PizzaID);
            IProductPart part = new ProductPart();
            part.BomNodeType = p.BOMNodeType;
            part.CheckItemType = "PizzaPart";
            part.PartType = p.Type;
            part.CheckItemType = "PizzaPart";
            part.Editor = session.Editor;
            part.Cdt = DateTime.Now;
            part.Udt = DateTime.Now;
            part.Station = session.Station;
            part.PartID = cartonPn;
            part.PartSn = "";
            pizza.AddPart(part);
            PizzaRepository.Update(pizza, session.UnitOfWork);

        }

        public String InputCustsnForCQ(string custsn, string pdLine, string editor, string stationId, string customerId,bool isNeedEnergyLabel)
        {
            logger.Debug("(SNCheck)InputCustsnForCQ start, inputCustsn:" + custsn + " editor:" + editor + " customerId:" + customerId);
         
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                 string sessionKey = custsn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    // mantis 1573
                    RouteManagementUtils.GetWorkflow(stationId, "SNCheck.xoml", "SNCheck.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custsn);
                    if (isNeedEnergyLabel)
                    { currentSession.AddValue("isNeedEnergyLabel", "Y"); }
                    else
                    { currentSession.AddValue("isNeedEnergyLabel", "N"); }
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

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }

                Product getProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                string ret = getProduct.ProId;
                return ret;
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
                logger.Debug("(SNCheck)InputCustSNOnProduct end, inputSn:" + custsn + " pdLine:" + pdLine + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }

        }

        public ArrayList InputCustsnOnPizzaForCQ(string custsnOnPizza, string custsnOnProduct, string pdLine, string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(SNCheck)InputCustSNOnPizzaReturn start, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();

            var currentProduct = CommonImpl.GetProductByInput(custsnOnProduct, CommonImpl.InputTypeEnum.CustSN);
            string paQCStatus = currentProduct.GetAttributeValue("PAQC_QCStatus");
            try
            {
                string sessionKey = custsnOnProduct;
               // Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CustSN, custsnOnPizza);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }
                ArrayList arr = new ArrayList();
                string IsBSamModel = sessionInfo.GetValue(ExtendSession.SessionKeys.IsBSamModel) as string;

                string ret = "";
                if ("Y".Equals(IsBSamModel))
                {
                    ret = " BSAM";
                }
                else if (custsnOnPizza.Substring(0, 1) == "P")
                {
                    ret = " NO-ALC";
                }
                else
                {
                    ret = "ALC";
                }
                if (paQCStatus == "8" || paQCStatus == "9" || paQCStatus == "A")
                {
                    ret = ret + "  PAQC";
                }
                IList<PrintItem> pi = (IList<PrintItem>) sessionInfo.GetValue(Session.SessionKeys.PrintItems);
              
                arr.Add(ret);
                arr.Add((string)sessionInfo.GetValue("EnergyLabel"));
                arr.Add(pi);
                return arr;
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
                logger.Debug("(SNCheck)InputCustSNOnPizzaReturn end, custsnOnPizza:" + custsnOnPizza + " custsnOnProduct:" + custsnOnProduct + " editor:" + editor + " customerId:" + customerId);
            }
        }
        #endregion
    }
}
