/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0079
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
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.Extend;
namespace IMES.Station.Implementation
{
    public class CreateProductandCombineLCM : MarshalByRefObject, ICreateProductandCombineLCM
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region ICreateProductandCombineLCM Members
        /// <summary>
        /// CreateProductID
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="model"></param>
        /// <param name="mo"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<string> CreateProductID(string pdLine, string model, string mo, string editor, string station, string customer, string family, string moPrefix)
        {
            logger.Debug(" CreateProductID start, mo:" + mo + " ,pdLine:" + pdLine);
            string currentSessionKey = mo;
            bool IsNextMonth = false;
            try
            {
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);

                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, pdLine, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CreateProductandCombineLCMforCreateProduct.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    string deliveryDate = DateTime.Now.ToString("yyyy/MM/dd");
                    currentCommonSession.AddValue(Session.SessionKeys.MONO, mo);
                    currentCommonSession.AddValue(Session.SessionKeys.Qty, 1);
                    currentCommonSession.AddValue(Session.SessionKeys.IsNextMonth, IsNextMonth);
                    currentCommonSession.AddValue(ExtendSession.SessionKeys.DeliveryDate, deliveryDate);
                    currentCommonSession.AddValue(Session.SessionKeys.ModelName, model);
                    currentCommonSession.SetInstance(instance);
                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentCommonSession.AddValue("Family", family);
                    currentCommonSession.AddValue("BomRemark", "");
                    currentCommonSession.AddValue("Remark", "");
                    currentCommonSession.AddValue("Exception", "");
                    currentCommonSession.AddValue("MOPrefix", moPrefix);
                    
                    currentCommonSession.AddValue("CityType", "cq");
                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }

                IList<string> ret = new List<string>();
                IList<string> temp = new List<string>();
                temp = (IList<string>)currentCommonSession.GetValue(Session.SessionKeys.ProdNoList);
                int tempqty = (int)currentCommonSession.GetValue("Qty");
                string qty = Convert.ToString(tempqty);
                ret.Add(temp[0]);
                ret.Add(qty);
                return ret; //(IList<PrintItem>)currentCommonSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug(" CreateProductID end, mo:" + mo + " ,pdLine:" + pdLine);
            }
        }
        /// <summary>
        /// InputProdIdorCustsn
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="input"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        /// <param name="realProdID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<BomItemInfo> InputProdIdorCustsn(string pdLine, string input, string editor, string stationId, string customerId, out string realProdID, out string model)
        {
            logger.Debug("(CreateProductandCombineLCM)InputProdId start, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);
            FisException ex;
            List<string> erpara = new List<string>();
            if (currentProduct == null)
            {

                erpara.Add(input);
                throw new FisException("SFC002", erpara);
            }

            string sessionKey = currentProduct.ProId;
            realProdID = sessionKey;
            model = currentProduct.Model;

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
                    RouteManagementUtils.GetWorkflow(stationId, "CreateProductandCombineLCMforCombineKeyParts.xoml", "CreateProductandCombineLCMforCombineKeyParts.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
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
                       
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                //model = prod.Model;
                //realProdID= prod.ProId;
                //add mb info in GetCheckItemList
                IList<IMES.DataModel.BomItemInfo> retLst = PartCollection.GeBOM(sessionKey, TheType);
                if (retLst.Count == 0)
                {
                    Session.AddValue("getbomnull", "1");
                }
                else
                {
                    Session.AddValue("getbomnull", "0");

                }

                
                return PartCollection.GeBOM(sessionKey, TheType);
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
                logger.Debug("(CreateProductandCombineLCM)InputProdId end, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }
        /// <summary>
        /// InputPPID
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="ppid"></param>
        /// <returns></returns>
        public IMES.DataModel.MatchedPartOrCheckItem InputPPID(string prodId, string ppid)
        {
            logger.Debug("(CreateProductandCombineLCM)InputPPID start," + " [prodId]: " + prodId + " [ppid]:" + ppid);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                string sessionKey = prodId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.PartID, ppid);
                    sessionInfo.AddValue(Session.SessionKeys.ValueToCheck, ppid);
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

                PartUnit matchedPart = (PartUnit)sessionInfo.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = ppid;
                    tempMatchedPart.ValueType = matchedPart.ValueType;
                    return tempMatchedPart;
                }
                return null;
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
                logger.Debug("(CreateProductandCombineLCM)InputPPID  End," + " [prodId]: " + prodId + " [ppid]:" + ppid);
            }
        }
        /// <summary>
        /// TryPartMatchCheck
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="flag"></param>
        /// <param name="flag_39"></param>
        /// <param name="printItems"></param>
        /// <param name="custsn"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string prodId, bool flag, bool flag_39, IList<PrintItem> printItems, out string custsn)
        {
            logger.Debug("(CreateProductandCombineLCM)Save start,"
               + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            try
            {


                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue("flag", flag);
                    sessionInfo.AddValue("flag39", flag_39);
                    sessionInfo.AddValue(Session.SessionKeys.IsComplete, true);
                    sessionInfo.AddValue(Session.SessionKeys.PrintItems, printItems);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogName, "CTLabel");
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);
                    sessionInfo.AddValue(Session.SessionKeys.PrintLogDescr, prodId);


                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                    var prod = (IProduct)sessionInfo.GetValue(Session.SessionKeys.Product);
                    custsn = prod.ProId;


                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
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
                logger.Debug("(CreateProductandCombineLCM)Save  End,"
                               + " [prodId]: " + prodId);
            }
            return (IList<PrintItem>)sessionInfo.GetValue(Session.SessionKeys.PrintItems);
        }
        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="pCode"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(CreateProductandCombineLCM)Reprint start, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
                // Check Prodid....
                // Check Prodid....

                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombineKeyPartsReprint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, "CTLabel");

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, prodid);



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


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
               // string linecode = (string)Session.GetValue(Session.SessionKeys.LineCode);
                string id = (string)Session.GetValue(Session.SessionKeys.ProductIDOrCustSN);
                string action = "0";

                retrunValue.Add(action);
                retrunValue.Add(id);
                //retrunValue.Add(linecode);
                retrunValue.Add(returnList);

                return retrunValue;
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
                logger.Debug("(CreateProductandCombineLCM)Reprint end, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }
        /// <summary>
        /// ClearPart
        /// </summary>
        /// <param name="sessionKey"></param>
        public void ClearPart(string sessionKey)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);

                    IFlatBOM bom = null;
                    bom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    bom.ClearCheckedPart();
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
        /// Cancel
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
        /// GetMOListByFamily
        /// </summary>
        /// <param name="Family"></param>
        /// <returns></returns>
        public IList<MOInfo> GetMOListByFamily(string Family)
        {
            IMORepository iMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
            return iMORepository.GetMOListByFamily(Family);
        }

        




        public int CountQty(string line )
        {
           


            DateTime dtNow = DateTime.Now;
            DateTime dt2030 = Convert.ToDateTime("20:30");
        //    string  cmdTxt = @"select count(distinct p.ProductID) from ProductLog p,Product Pr
          //               where p.ProductID=Pr.ProductID and  p.Cdt>@dtBegin and p.Cdt<@dtEnd and p.Station='40'  and Pr.PCBID <>'' and Pr.PCBID is not null";
            string cmdTxt = @"select count(*)  from PCBStatus nolock where Station='32' and Line=@Line and Udt>=@dtBegin and Udt<@dtEnd";
          
            DateTime dtBegin;
            DateTime dtEnd = DateTime.Now;
           // AM8:00算日班 , PM8:30算夜班
            if (IsDayWork())// 白班
            {
                dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
               
            }
            else
            {
                if (DateTime.Compare(dtNow, dt2030) > 0) // it means 00:00~20:30
                {
                    dtBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,20, 30, 0);
                }
                else
                {
                    dtBegin = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 20, 30, 0);
                }
             
            }
            SqlParameter paraName = new SqlParameter("@dtBegin", SqlDbType.DateTime);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = dtBegin;
            SqlParameter paraName2 = new SqlParameter("@dtEnd", SqlDbType.DateTime);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = dtEnd;
            SqlParameter paraLine = new SqlParameter("@Line", SqlDbType.VarChar);
            paraLine.Direction = ParameterDirection.Input;
            paraLine.Value = line;

            //string result = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, cmdTxt, paraName, paraName2, paraLine).ToString();
            string result = "1";
           return int.Parse(result);
        
        
        
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

      

        #endregion
    }
}
