/*
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


        public void InputProdId(string pdLine, string testStation, string prodId, string editor, string customerId)
        {
            logger.Debug("(FATestStation)InputProdId start, pdLine:" + pdLine + " testStation:" + testStation + " prodId:" + prodId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            try
            {
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

        public IList<PrintItem> InputDefectCodeList(string prodId, IList<string> defectList,bool isNeedPrint2D, IList<PrintItem> printItems)
        {
            logger.Debug("(FATestStation)InputDefectCodeList start, prodId:" + prodId );
            
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
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

                if (!(defectList == null || defectList.Count == 0))
                {
                    
                    Session.AddValue(Session.SessionKeys.DefectList, defectList);
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
