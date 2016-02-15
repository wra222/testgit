﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2009-11-03   207006     ITC-1122-0031
 * 2010-02-03   207006     ITC-1122-0105
 * 2010-02-03   207006     ITC-1122-0108 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Implementation;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.Route;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Station.Implementation
{
    public class FATestStation: MarshalByRefObject, IFATestStation 
    {
        private const Session.SessionType TheType = Session.SessionType.Product ;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        #region IFATestStation Members
        /// <summary>
        /// Get Product Attr via CustSN
        /// </summary>
        /// <param name="custsn"></param>
        /// <returns></returns>
        private SqlDataReader GetProductAttr(string custsn)
        {
            string strSql = @" select  a.ProductID , 
                                       a.CUSTSN,
                                       b.Line,           
                                       b.Station,   
                                       b.Status,
                                       c.Descr,   
                                       isnull(e.AttrValue,'N') as AutoTestFlag
                               from Product a 
                                     inner join ProductStatus b on a.ProductID = b.ProductID 
                                     inner join Station c on   b.Station = c.Station
                                     left join StationAttr e on e.Station=c.Station and e.AttrName='AutoTest'
                               where a.CUSTSN =@custsn ";
            SqlParameter paraName = new SqlParameter("@custsn", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = custsn;
            SqlDataReader dr=  SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData,
                                                       System.Data.CommandType.Text,
                                                       strSql, paraName);
          return dr;
        }

        /// <summary>
        /// InputCustsn
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="testStation"></param>
        /// <param name="custsn"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        /// <param name="defectStation"></param>
        /// <param name="isAllowPass"></param>
        /// <param name="LastStation"></param>
        /// <returns></returns>
        public string InputCustsn(out string pdLine, string testStation, string custsn, string editor, string customerId, out string defectStation, out bool isAllowPass, out string LastStation)
        {
            defectStation = "";
            isAllowPass = true;
            LastStation = "";
            pdLine = "";
            //CommonImpl cmm = new CommonImpl();
            //IMES.DataModel.ProductInfo iProduct = cmm.GetProductInfoByCustomSn(custsn);

           SqlDataReader dr= GetProductAttr(custsn);
           string strProdId = "";
           string autoTestFlag = "";
           string status = "";
           string descr = "";
            FisException ex;
            List<string> erpara = new List<string>();

            //================================== 
            if(dr.HasRows)
            {
                dr.Read();
                {
                    pdLine = dr["Line"].ToString().Trim();
                    strProdId = dr["ProductID"].ToString().Trim();
                    autoTestFlag = dr["AutoTestFlag"].ToString().Trim();
                    status = dr["Status"].ToString().Trim();
                    descr = dr["Descr"].ToString().Trim();
                    LastStation = dr["station"].ToString().Trim();
                }
                dr.Close();
            }
            else
            {
                //List<string> errpara = new List<string>();
                erpara.Add(custsn);
                throw new FisException("SFC011", erpara);
            }
            if (testStation == "" )
            {
                if (status != "0" || autoTestFlag != "Y")
                {
                    erpara.Add(custsn);
                    erpara.Add(LastStation + "(" + descr + ")");
                    erpara.Add(status);
                    throw new FisException("CHK172", erpara);
                }
             
            
            }
            logger.Debug("(FATestStation)InputProdId start, pdLine:" + pdLine + " testStation:" + testStation + " custsn:" + custsn + " editor:" + editor + " customerId:" + customerId);
            string sessionKey = strProdId;
            try
            {
                Session _Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (_Session == null)
                {
                    _Session = new Session(sessionKey, TheType, editor, testStation, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    if (testStation == "")
                    {
                        wfArguments.Add("Station", LastStation);
                    }
                    else
                    {
                        wfArguments.Add("Station", testStation);
                    }
                 
                    wfArguments.Add("CurrentFlowSession", _Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    //2009-11-03   207006     ITC-1122-0031
                    //2010-02-03   207006     ITC-1122-0105
                    //2010-02-03   207006     ITC-1122-0108
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(testStation, "FATestStation.xoml", "FATestStation.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    _Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(_Session))
                    {
                        _Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    _Session.WorkflowInstance.Start();
                    _Session.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (_Session.Exception != null)
                {
                    if (_Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        _Session.ResumeWorkFlow();
                    }
                    throw _Session.Exception;
                }
                isAllowPass = _Session.GetValue(ExtendSession.SessionKeys.AllowPass).ToString() == "Y" ? true : false ;
                defectStation = _Session.GetValue(ExtendSession.SessionKeys.DefectStation).ToString();

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
                logger.Debug("(FATestStation)InputProdId end, pdLine:" + pdLine + " testStation:" + testStation + " custsn:" + custsn + " editor:" + editor + " customerId:" + customerId);
            }

            
           
            return strProdId;
        
        }

        private static string getCRC(string orig)
        {
            string sequence = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int sum = 0;
            foreach (char c in orig.ToUpper())
            {
                int pos = sequence.IndexOf(c);
                sum += pos >= 0 ? pos : 36;
            }
            sum %= 16;
            return sequence[sum].ToString();
        }

        private bool NeedProductCrc(IProduct currentProduct, string pdLine)
        {
            if (pdLine.Length > 1)
                pdLine = pdLine.Substring(0, 1);
            IList<ConstValueTypeInfo> lstNoCheckProductIDCRCByLineFamily = CommonImpl.GetInstance().GetConstValueTypeListByType("NoCheckProductIDCRCByLineFamily");
            foreach (ConstValueTypeInfo v in lstNoCheckProductIDCRCByLineFamily)
            {
                if (currentProduct.Family == v.value || pdLine == v.value)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// InputProdId
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="testStation"></param>
        /// <param name="prodId"></param>
        /// <param name="editor"></param>
        /// <param name="customerId"></param>
        public ArrayList InputProdId(string pdLine, string testStation, string prodId, string editor, string customerId, out bool JustKeyDefectCode, out string RunInTimes)
        {
            logger.Debug("(FATestStation)InputProdId start, pdLine:" + pdLine + " testStation:" + testStation + " prodId:" + prodId + " editor:" + editor + " customerId:" + customerId);
            JustKeyDefectCode = false;
            RunInTimes = "0";
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret = new ArrayList();

            try
            {
				// mantis 618
                bool IsUseCrc = false;
                string crc = "";
				var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
				IProduct currentProduct = productRepository.Find(prodId);
				if (null == currentProduct)
				{
                    crc = prodId.Substring(0, 1);
                    prodId = prodId.Substring(1);
                    currentProduct = productRepository.Find(prodId);
                    if (null != currentProduct)
                        IsUseCrc = true;
				}
                if (null == currentProduct)
                {
                    erpara.Add(prodId);
                    throw new FisException("SFC002", erpara);
                }
                if (NeedProductCrc(currentProduct, pdLine))
                {
                    if (!IsUseCrc)
                    {
                        throw new FisException("CQCHK1072", erpara); // 需刷入带验证码的productID
                    }
                    if (crc != getCRC(prodId))
                    {
                        throw new FisException("CQCHK1074", erpara); // 刷入productID的CRC 錯誤
                    }
                }
                else
                {
                    if (IsUseCrc)
                    {
                        throw new FisException("CQCHK1073", erpara); // 无需刷入带验证码的productID
                    }
                }
				
                string sessionKey = prodId;
				Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, testStation, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", testStation);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(testStation, "FATestStation.xoml", "FATestStation.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
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
                //if (Session.GetValue("JustKeyDefectCode") == null)
                //{
                //    JustKeyDefectCode = false;
                //}
                //else
                //{
                //    JustKeyDefectCode = (bool)Session.GetValue("JustKeyDefectCode");
                //}
                //if (JustKeyDefectCode)
                //{ 
                //    RunInTimes = (string)Session.GetValue("RunInTimes");
                //}
                string strRuntimertn = "NO!";

                IMES.DataModel.ProductInfo productInfo = productRepository.GetProductInfo(prodId);

                ret.Add(prodId);
                ret.Add(productInfo.modelId);
                ret.Add(strRuntimertn);
                ret.Add(productInfo.customSN);

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
                logger.Debug("(FATestStation)InputProdId end, pdLine:" + pdLine + " testStation:" + testStation + " prodId:" + prodId + " editor:" + editor + " customerId:" + customerId);
            }
        }
        /// <summary>
        /// InputDefectCodeList
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="defectList"></param>
        /// <param name="isNeedPrint2D"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> InputDefectCodeList(string prodId, IList<string> defectList, bool isNeedPrint2D, IList<PrintItem> printItems, out string qcMethod, out string setMsg)
        {
            logger.Debug("(FATestStation)InputDefectCodeList start, prodId:" + prodId );
            
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            string strRuntimertn;
            
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            Session.AddValue(IMES.Infrastructure.Extend.ExtendSession.SessionKeys.Print2DStation, isNeedPrint2D);
            if (printItems != null)
            {
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
            }
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                throw ex;
            }
            try
            {
                Session.Exception = null;

                strRuntimertn = "NO!";

                //Kaisheng Update 2012/05/02: UC update-当Station=57时，只接受不良品
                //--------------------BEGIN--------------------------------
                if ((defectList == null) || (defectList.Count == 0))
                {
                    //Vincent add Check AutoTest Station not allow pass
                    string allowPass = (string)Session.GetValue(ExtendSession.SessionKeys.AllowPass);
                    allowPass = string.IsNullOrEmpty(allowPass) ? "Y" : allowPass.ToUpper();
                    string curStation = Session.Station;
                    if (curStation == "57" || allowPass == "N")
                    {
                        SessionManager.GetInstance.RemoveSession(Session);
                        FisException eximpl;
                        List<string> erparaimpl = new List<string>();
                        erpara.Add(sessionKey);
                        eximpl = new FisException("CHK094", erparaimpl);//本站只能刷不良品!
                        throw eximpl;
                    }

                }
                //--------------------END----------------------------------
                if (!(defectList == null || defectList.Count == 0))
                {
                    
                    Session.AddValue(Session.SessionKeys.DefectList, defectList);
                }
                else if (strRuntimertn != "NO!")
                {
                    IList<string> defectIOUTlist = new List<string>();
                    defectIOUTlist.Add("TOUT");
                    Session.AddValue(Session.SessionKeys.DefectList, defectIOUTlist);
                }
                
                Session.SwitchToWorkFlow();


                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                IList<PrintItem> retL = null;
                if (Session.GetValue(Session.SessionKeys.PrintItems) != null)
                {  retL=(IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems); }

                try
                {
                    if (true == (bool)Session.GetValue("isPIA"))
                    {
                        qcMethod = "isPIA";
                    }
                    else
                    {
                        qcMethod = "noPIA";
                    }
					
					//if (Session.Station !="60")
					if(false == (bool)Session.GetValue("isStationPIA"))
					{
						qcMethod = "None";
					}
                }
                catch (FisException )
                {
                    qcMethod = "None";
                }
                catch (Exception)
                {
                    qcMethod = "None";
                }

                setMsg = (string)Session.GetValue(ExtendSession.SessionKeys.WarningMsg);
                return retL;
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
                logger.Debug("(FATestStation)InputDefectCodeList end, prodId:" + prodId);

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


        #endregion
    }
}
