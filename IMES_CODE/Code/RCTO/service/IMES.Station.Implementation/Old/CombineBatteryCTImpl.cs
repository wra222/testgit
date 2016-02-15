/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  UnitWeight interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2011-05-21 Shao.Rong-hua         Create 
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    public class CombineBatteryCTImpl : MarshalByRefObject, ICombineBatteryCT
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region ICombineBatteryCT members

        /// <summary>
        /// 输入CustSN和相关信息
        /// </summary>
        /// <param name="pdLine">PdLine</param>
        /// <param name="custsn">CustSN</param>
        /// <param name="editor">operator</param>、
        public string InputCustSN(string pdLine, string custsn, string stationId, string editor, string customer, out string model,out string ActualLine)
        {
            logger.Debug("(CombineBatteryCTImpl)InputCustSN start, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [stationId]:" + stationId
                + " [editor]:" + editor
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ActualLine = "";
            //var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //var currentProduct = productRepository.GetProductByIdOrSn(custsn);


            //if (currentProduct == null)
            //{
            //    erpara.Add(custsn);
            //    throw new FisException("SFC011", erpara);
            //}
            //else
            //{
            //    prodid = currentProduct.ProId;
            //}

            string sessionKey = custsn;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    ////Get Line at BoardInput Station
                    //string strGetLineSQL = "select  rtrim(c.Line)+ ' ' + Descr from Product a,ProductStatus b,Line c where Station= '40' and a.ProductID = b.ProductID  and b.Line = c.Line and  a.CUSTSN = @CUSTSN";
                    //SqlParameter GetLinePara = new SqlParameter("@CUSTSN", SqlDbType.VarChar, 32);
                    //GetLinePara.Direction = ParameterDirection.Input;
                    //GetLinePara.Value = custsn;
                    //SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strGetLineSQL, GetLinePara);
                    //if (dr.HasRows)
                    //{
                    //    while (dr.Read())
                    //    {
                    //        ActualLine = dr[0].ToString();

                    //    }
                    //}
                    //else
                    //{
                    //    FisException e;
                    //    //erpara.Add(sessionKey);
                    //    erpara.Add("Line");
                    //    e = new FisException("CHK049", erpara);
                    //    throw e;
                    //}

                      

                    string strSQL = "SELECT count(b.PartNo) FROM Product a, Product_Part b, Part c where a.CUSTSN=@custsn and a.ProductID=b.ProductID and b.Station=@stationId and b.PartNo=c.PartNo";
                    SqlParameter paraName = new SqlParameter("@custsn", SqlDbType.VarChar, 32);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = custsn;
                    SqlParameter paraName2 = new SqlParameter("@stationId", SqlDbType.VarChar, 32);
                    paraName2.Direction = ParameterDirection.Input;
                    paraName2.Value = stationId;

                    string re = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName, paraName2).ToString();

                    if (int.Parse(re) > 0)
                    {
                        FisException e;
                        //erpara.Add(sessionKey);
                        e = new FisException("CHK165", erpara);        
                        throw e;
                    }


                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    
                    //WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("CombineBatteryCT.xoml", "", wfArguments);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "CombineBatteryCT.xoml", "CombineBatteryCT.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    //session.AddValue(Session.SessionKeys.Product, currentProduct);
                    session.AddValue(Session.SessionKeys.CustSN, custsn);
                    //session.AddValue(Session.SessionKeys.IsComplete, false);
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

                var prodid = (IProduct)session.GetValue(Session.SessionKeys.Product);

                model = prodid.Model;
                ActualLine = prodid.Status.Line;

                return prodid.ProId;
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
                logger.Debug("(CombineBatteryCTImpl)InputCustSN end, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [stationId]:" + stationId
                + " [editor]:" + editor
                + " [customer]:" + customer);

            }
        }



        /// <summary>
        /// 输入Battery CT
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="custsn">Cust SN</param>
        /// <param name="batteryct">Battery CT</param>
        public void InputBatteryCT(string prodId, string custsn, string batteryct)
        {
            logger.Debug("(CombineBatteryCTImpl)InputBatteryCT start, [prodId]:" + prodId
                + " [custsn]:" + custsn
                + " [batteryct]:" + batteryct);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;


            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    session.AddValue(ExtendSession.SessionKeys.BatteryCT, batteryct);
                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.ValueToCheck, batteryct);

                    session.Exception = null;

                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
                   
                    //get matchedinfo
                    IList<IBOMPart> bomPartlist = (IList<IBOMPart>)session.GetValue(Session.SessionKeys.MatchedParts);
                    if ((bomPartlist != null) && (bomPartlist.Count > 0))
                    {                       
                    }
                    else
                    {
                        ICheckItem citem = (ICheckItem)session.GetValue(Session.SessionKeys.MatchedCheckItem);
                        if (citem == null)
                        {
                            //Cancel(custsn);
                            throw new FisException("MAT010", new string[] { batteryct });
                        }
                      
                    }

                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.Exception = null;

                    session.SwitchToWorkFlow();

                    //check workflow exception
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
                logger.Debug("(CombineBatteryCTImpl)InputBatteryCT end, [prodId]:" + prodId);
            }
        }



        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string custsn)
        {
            logger.Debug("(CombineBatteryCTImpl)Cancel start, [custsn]:" + custsn);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                throw e;
            }
            finally
            {
                logger.Debug("(CombineBatteryCTImpl)Cancel end, [custsn]:" + custsn);
            }
        }



        #endregion
    }
}
