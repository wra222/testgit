/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
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
using IMES.DataModel;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MB;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.Repository;
using IMES.Infrastructure.Repository.PCA;
using log4net;
using System.Collections;
using IMES.Route;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;
namespace IMES.Station.Implementation
{
    public class _MBLabelPrint : MarshalByRefObject, IMBLabelPrint, ISMTMO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        #region IMBLabelPrint Members

        public IList<PrintItem> Print(
            string pdLine,
            bool isNextMonth,
            string mo,
            int qty,
            string dateCode,
            string editor,
            string stationId, string customerId,
            out IList<string> startProdIdAndEndProdId, string _111, IList<PrintItem> printItems)
        {
            logger.Debug("(MBLabelPrint)Print start, pdLine:" + pdLine + " isNextMonth:" + isNextMonth.ToString() + " mo:" + mo + " qty:" +qty.ToString()+" dateCode:"+dateCode+ " editor:" + editor +" stationId:"+stationId+ " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = mo;

                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType,editor,stationId,pdLine,customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelPrint.xoml",  "002MBLabelPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.IsNextMonth, isNextMonth);
                    Session.AddValue(Session.SessionKeys.Qty, qty);
                    Session.AddValue(Session.SessionKeys.DateCode, dateCode);
                    Session.AddValue(Session.SessionKeys.motherOrChild, "0");
                    Session.AddValue(Session.SessionKeys.MBMONO, mo);
                    Session.AddValue(Session.SessionKeys.ModelName,_111);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //MB mb = new MB();
                    //mb.Model = _111;

                    var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository,IMBModel>();
                    MBModel model = (MBModel)mbModelRepository.Find(_111);

                    Session.AddValue(Session.SessionKeys.MBCode, model.Mbcode);
                    Session.AddValue(Session.SessionKeys.MBType, model.Type);
                     
                    Session.SetInstance(instance);
                    //for generate MB no

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

                var MBNOList = (IList<string>)Session.GetValue(Session.SessionKeys.MBNOList);

                startProdIdAndEndProdId = MBNOList;
                //startProdIdAndEndProdId.add(MBNOList[0]);
                //startProdIdAndEndProdId.Add(MBNOList[MBNOList.Count - 1]);

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
                logger.Debug("(MBLabelPrint)Print end, pdLine:" + pdLine + " isNextMonth:" + isNextMonth.ToString() + " mo:" + mo + " qty:" + qty.ToString() + " dateCode:" + dateCode + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
                
            }
        }

        public string Find(string mo,
            string isNextMonth,
            string editor)
        {
            throw new NotImplementedException();
        }

        #endregion

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


        #region IMBLabelPrint Members


        public string Find(string mo, string isNextMonth, string editor, string stationId, string customerId)
        {
            throw new NotImplementedException();
        }

        public void Dismantle(string startMBSNo, string endMBSNo, string reason, string editor, string stationId, string customerId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISMTMO Members

        public IList<SMTMOInfo> GetSMTMOList(string _111LevelId)
        {
            IMBMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            return (List<SMTMOInfo>)moRepository.GetSMTMOListFor002(_111LevelId);
        }

        public SMTMOInfo GetSMTMOInfo(string SMTMOId)
        {
            throw new NotImplementedException();
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, theType);

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
        #region Add Reprint 106137
      
        public void Reprint(
       string startMBSNo,
       string endMBSNo,
       string reason,
       string editor, string stationId, string customerId)
        {
            logger.Debug("(MBLabelReprint)Reprint start, startMBSno:" + startMBSNo + "endMBSno:" + endMBSNo + " Reason:" + reason + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = startMBSNo;
                int Qty = Convert.ToInt32(endMBSNo) - Convert.ToInt32(startMBSNo) + 1;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelReprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);


                    //WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("002MBLabelReprint.xoml", null, wfArguments);
                    MBRepository mbRepository = new MBRepository();
                    IMES.DataModel.MBInfo mbInfo = new IMES.DataModel.MBInfo();
                    mbInfo = mbRepository.GetMBInfo(startMBSNo);


                    Session.AddValue(Session.SessionKeys.Qty, Qty);
                    Session.AddValue(Session.SessionKeys.DateCode, "");
                    Session.AddValue(Session.SessionKeys.motherOrChild, "0");
                    Session.AddValue(Session.SessionKeys.MBMONO, mbInfo.SMTMOId);
                    Session.AddValue(Session.SessionKeys.ModelName, mbInfo._111LevelId);
                    //  Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    //MB mb = new MB();
                    //mb.Model = _111;

                    var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    MBModel model = (MBModel)mbModelRepository.Find(mbInfo._111LevelId);

                    Session.AddValue(Session.SessionKeys.MBCode, model.Mbcode);
                    Session.AddValue(Session.SessionKeys.MBType, model.Type);

                    Session.SetInstance(instance);
                    //for generate MB no

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

                var MBNOList = (IList<string>)Session.GetValue(Session.SessionKeys.MBNOList);

                // startProdIdAndEndProdId = MBNOList;
                //startProdIdAndEndProdId.add(MBNOList[0]);
                //startProdIdAndEndProdId.Add(MBNOList[MBNOList.Count - 1]);

                IList<PrintItem> returnList = this.getPrintList(Session);
                // return returnList;
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
                logger.Debug("(MBLabelReprint)Reprint start, startMBSno:" + startMBSNo + "endMBSno:" + endMBSNo + " Reason:" + reason + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }


        }

        #endregion

        public bool CheckIsProduct(string beginNo, string endNo, string SA1StationName,out string ExistMB)
        {
            bool chkResult=false;
            string strSQL = " select count(*) from PCBLog where PCBNo between @beginNo and @endNo and Station='"+SA1StationName+"'";
            SqlParameter paraName = new SqlParameter("@beginNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = beginNo;
            SqlParameter paraName2 = new SqlParameter("@endNo", SqlDbType.VarChar, 32);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = endNo;

            string re= SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName, paraName2).ToString();

            if (int.Parse(re) > 0)
            {
                chkResult = true;
            }
            //check MB is exist
            ArrayList arr2 = new ArrayList();
            ArrayList arr3 = new ArrayList();
            arr2 = GetMBNoList(beginNo, beginNo);
            arr3 = GetMBNoList(endNo, endNo);
            List<string> lstMBNo2 = (List<string>)arr2[0];
            List<string> lstMBNo3 = (List<string>)arr3[0];

            if (lstMBNo2.Count == 0 || lstMBNo3.Count == 0)
            {
                ExistMB = "N";
            }
            else
                ExistMB = "Y";

            return chkResult;
            
            //select count(*) from PCBLog
             //where PCBNo between 'QI15M1001L' and 'QI15M1001P' and Station='1A'
        
        }

        private ArrayList GetMBNoList(string beginNo, string endNo)
        {
            FisException ex;
            string strSQL = "select PCBNo, SMTMO from PCB where PCBNo between @beginNo and @endNo";
            List<string> relLst = new List<string>();
            List<string> _111Lst = new List<string>();
            SqlParameter paraName = new SqlParameter("@beginNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = beginNo;
            SqlParameter paraName2 = new SqlParameter("@endNo", SqlDbType.VarChar, 32);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = endNo;
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName, paraName2);
            var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
            MBRepository mbRepository = new MBRepository();
            IMES.DataModel.MBInfo mbInfo = new IMES.DataModel.MBInfo();
            bool bFirst = true;
            bool bCheckPass = true;
            string STMMO = "";
            List<string> erpara = new List<string>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (bFirst)
                    {
                        STMMO = dr["SMTMO"].ToString();
                        bFirst = false;
                        erpara.Add(STMMO);
                    }
                    else
                    {
                        if (STMMO != dr["SMTMO"].ToString())
                        {
                            bCheckPass = false;
                            erpara.Add(dr["SMTMO"].ToString());
                            break;
                        }
                    }
                  relLst.Add(dr[0].ToString());
                  mbInfo = mbRepository.GetMBInfo(dr[0].ToString());
                  MBModel model = (MBModel)mbModelRepository.Find(mbInfo._111LevelId);
                  _111Lst.Add(mbInfo._111LevelId);
                }
            
            }

            if (!bCheckPass)
            {
                ex = new FisException("SFC012",erpara);
                throw ex;
            }
            ArrayList retrunValue = new ArrayList();
            retrunValue.Add(relLst);
            retrunValue.Add(_111Lst);
            return retrunValue;
        
        }

        public IList<PrintItem> RePrintMbLabel(string beginNo, string endNo, string customerId, string reason, string editor,  string stationId, IList<PrintItem> printItems, out List<string> lstMBNo, out List<string> lstParttNo)
        {
            logger.Debug("(MBLabelReprint)Reprint start, startMBSno:" + beginNo + "endMBSno:" + endNo + " Reason:" + reason + " editor:" + editor + " station:" + stationId + " customerId:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList arr1 = new ArrayList();
            //ArrayList arr2 = new ArrayList();
            //ArrayList arr3= new ArrayList();
            //arr2 = GetMBNoList(beginNo, beginNo);
            //arr3 = GetMBNoList(endNo, endNo);
            //List<string> lstMBNo2 = (List<string>)arr2[0];
            //List<string> lstMBNo3 = (List<string>)arr3[0];

            IList<PrintItem> printList = null;
            //if (lstMBNo2.Count != 0 && lstMBNo3.Count != 0)
            //{
                arr1 = GetMBNoList(beginNo, endNo);
                lstMBNo = (List<string>)arr1[0];
                lstParttNo = (List<string>)arr1[1];
                if (lstMBNo.Count == 0)
                {
                    return printList;
                }
            //}
            //else
            //{
            //    lstMBNo = (List<string>)arr2[0];
            //    lstParttNo = (List<string>)arr2[1];
            //    return printList;
            //}
           
            #region mark macthMO
            // for Mo Macth
            //IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
            //var mb1 = mbRepository.Find(beginNo);
            //var mb2 = mbRepository.Find(endNo);
            //string beginMO ;
            //if(mb1 !=null && mb2!=null)
            //{
            //    string beginMO = mb1.SMTMO;
            //     string endMO = mb2.SMTMO;
            //     string begMO = mb1.SMTMO;
            // string beginMO = mb1.SMTMO;
            
            //string endMO = mb2.SMTMO;
            //string begMO = mb1.SMTMO;
            //} 
          
            //if (endMO != begMO)
            //{
            //    lstMBNo.Add("NOMATCH");
            //}


            // =================
            #endregion
            try
            {
                string sessionKey = beginNo;
         //       int Qty = Convert.ToInt32(endMBSNo) - Convert.ToInt32(startMBSNo) + 1;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelReprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    //WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("002MBLabelReprint.xoml", null, wfArguments);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo,beginNo);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo,endNo);
                    Session.AddValue(Session.SessionKeys.Reason,reason);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");
                    Session.SetInstance(instance);
                    //for generate MB no

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

                return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(MBLabelReprint)Reprint end, startMBSno:" + beginNo + "endMBSno:" + endNo + " Reason:" + reason + " editor:" + editor + " customerId:" + customerId);
           }
     }

     }
   
 }

