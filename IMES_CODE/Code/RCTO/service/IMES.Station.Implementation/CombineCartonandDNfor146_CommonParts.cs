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
    public class CombineCartonandDNfor146_CommonParts : MarshalByRefObject, ICombineCartonandDNfor146_CommonParts
    {


        private const Session.SessionType TheType = Session.SessionType.Common;

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //    private string OriSessionkey;

        #region CombineCartonandDNfor146_CommonParts Members
        
        /// <summary>
        /// GetModelList
        /// </summary>
        /// <returns></returns>
        public IList<string> GetModelList()
        {
            try
            {
                string strSQL = @"select distinct a.Model
                                    from Delivery a,ModelInfo b
                                    where a.Model =b.Model 
                                    and a.ShipDate >=GETDATE() -2
                                    and a.Status ='00' 
                                    and b.Name='TP' 
                                    and b.Value ='Commparts' ";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                IList<string> list = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetDeliveryList
        /// </summary>
        /// <param name="model">string</param>
        /// <returns></returns>
        public ArrayList GetDeliveryList(string model)
        {
            try
            {
                string strSQL = @" Select DeliveryNo ,Qty
                                      from Delivery 
                                      where Model = @model 
                                      and ShipDate > GETDATE() -2
                                      and Status ='00' ";
                SqlParameter paraNameType = new SqlParameter("@model", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = model;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                ArrayList list = new ArrayList();
                foreach (DataRow dr in dt.Rows)
                {
                    string[] items = { dr[0].ToString().Trim(), dr[1].ToString().Trim() };
                    list.Add(items);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetRemainQty
        /// </summary>
        /// <param name="dn">string</param>
        /// <returns></returns>
        public string GetRemainQty(string dn)
        {
            try
            {
                //getCombinedQtyByDnOnTrans
                var iMaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();

                int temp = iMaterialBoxRepository.GetCombinedMaterialBoxQty(null, null, dn, null);

                //string strSQL = @"select sum(Qty) from MaterialBox where DeliveryNo=@dn ";
                //SqlParameter paraNameType = new SqlParameter("@dn", SqlDbType.VarChar, 20);
                //paraNameType.Direction = ParameterDirection.Input;
                //paraNameType.Value = dn;
                //DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                //if(dt.Rows.Count == 0)
                //{
                //    return "0";
                //}
                return Convert.ToString(temp); //dt.Rows[0][0].ToString().Trim();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// InputPCSinCarton
        /// </summary>
        /// <param name="pdLine">string</param>
        /// <param name="input">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customerId">string</param>
        /// <param name="printItems">IList<PrintItem></param>
        /// <param name="dnno">out string</param>
        /// <param name="model">out string</param>
        /// <returns></returns>
        public ArrayList InputPCSinCarton(string pdLine, string input, string dnno, string model ,string editor, string stationId, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(CombineCartonandDNfor146_CommonParts)InputProdId start, pdLine:" + pdLine +" input:" + input + " dnno:" + dnno + " model:" + model +" editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList ret  = new ArrayList();
            string sessionKey = dnno;
            try
            {
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
                    RouteManagementUtils.GetWorkflow(stationId, "CombineCartonandDNfor146_CommonParts.xoml", "CombineCartonandDNfor146_CommonParts.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    int qty = int.Parse(input);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogName, "146 Carton Label");
                    Session.AddValue(Session.SessionKeys.Reason, "");
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "NoMaterialCT");

                    Session.AddValue(Session.SessionKeys.Qty, qty);
                    Session.AddValue(Session.SessionKeys.DeliveryNo, dnno);
                    Session.AddValue(Session.SessionKeys.ModelName, model);
                    Session.AddValue(Session.SessionKeys.MaterialType, "Commparts");
                    Session.AddValue(Session.SessionKeys.ShipMode, "RCTO");
                    //Session.AddValue("PrintedQty", input);

                    Session.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                        erpara.Add(input);
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

                string carton = (string)Session.GetValue(Session.SessionKeys.Carton);

                var deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                var iMaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
                //DeliveryAttrInfo condition = new DeliveryAttrInfo();
                //condition.attrName = "PrintedQty";
                //condition.deliveryNo = dnno;
                //IList<DeliveryAttrInfo> deliveryAttrList = deliveryRep.GetDeliveryAttr(condition);
                int deliveryNoQty = deliveryRep.GetDeliveryQtyOnTrans(dnno, "00");
                //if (deliveryAttrList.Count == 0)
                //{
                //    throw new FisException("CQCHK0025", new string[] { dnno });
                //}
                int printQty = iMaterialBoxRepository.GetCombinedMaterialBoxQty(null, null, dnno, null);
                string newRemainQty = "";
                if (deliveryNoQty == 0)
                {
                    newRemainQty = Convert.ToString(deliveryNoQty);
                }
                else
                {
                    newRemainQty = Convert.ToString(deliveryNoQty - printQty);
                }
                
                int temppcsQty = (int)Session.GetValue(Session.SessionKeys.Qty);
                string pcsQty = Convert.ToString(temppcsQty);
                IList<PrintItem> resultPrintItems = Session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                ret.Add("SUCCESSRET");
                ret.Add(carton);
                ret.Add(newRemainQty);
                ret.Add(pcsQty);
                ret.Add(resultPrintItems);
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
                logger.Debug("(CombineCartonandDNfor146_CommonParts)InputProdId end, pdLine:" + pdLine + " input:" + input + " dnno:" + dnno + " model:" + model + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

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
        
        private bool IsDayWork()
        {
            DateTime dt1 = Convert.ToDateTime("08:00");
            DateTime dt2 = Convert.ToDateTime("20:30");
            DateTime dt3 = DateTime.Now;
          
            if (DateTime.Compare(dt3, dt1) > 0 && DateTime.Compare(dt3, dt2) < 0)
            { return true; }
            return false;
        }
        /// <summary>
        /// RePrint
        /// </summary>
        /// <param name="CartonNo"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList RePrint(string CartonNo, string reason, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
            ArrayList retvaluelist = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = CartonNo;
          
           
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (session == null)
                {
                    session = new Session(sessionKey, TheType, editor, stationId, CartonNo, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "ReprintCartonLabelForOffCarton.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintLogName, "CartonLabelReprint");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, sessionKey);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, sessionKey);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
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

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }


                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                retvaluelist.Add(resultPrintItems);
                retvaluelist.Add(sessionKey);

                return retvaluelist;

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

            }
            
            
           
           
        }

      #endregion
    }
}
