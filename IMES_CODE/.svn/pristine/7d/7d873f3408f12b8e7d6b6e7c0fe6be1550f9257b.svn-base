/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-02-03   207006     ITC-1122-0078 
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
using IMES.Station.Interface.CommonIntf;
using System.Collections;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.FisObject.Common.PrintLog;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public partial class TravelCardPrint : MarshalByRefObject, ITravelCardPrint, IMO
    {

        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        public ArrayList ReprintTravelCard(string prodid, string editor, string station, string customer, string reason, string pCode, IList<PrintItem> printItems)
        {        
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {  logger.Debug("(TravelCardPrint)ReprintLabel start, startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " customerId:" + customer + " reason:" + reason);
                // Check Prodid....
                      IProductRepository ip = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                        IMES.FisObject.FA.Product.IProduct product = ip.Find(prodid);

                         if (product == null)
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(prodid);
                            throw new FisException("SFC002", errpara);
                        }
                         
                         ProductStatusInfo statusInfo = ip.GetProductStatusInfo(prodid);
                         string sLine = "";
                         if (statusInfo.pdLine != null)
                         {
                             sLine = statusInfo.pdLine;
                         }
                         retrunValue.Add(statusInfo.pdLine);
                         Object o = product.GetExtendedProperty("ShipDate");
                         string sShipDate="";
                         if (o != null)
                         {
                             sShipDate = o.ToString();
                         }
                         else
                         {
                             erpara.Add(prodid);
                             ex = new FisException("CHK166", erpara);
                             throw ex;
                         
                         }
                     
                         // // *********************** Get Ship Date*******************************
                         //IList<IMES.FisObject.FA.Product.ProductInfo> productinfos = product.ProductInfoes;

                         //foreach (IMES.FisObject.FA.Product.ProductInfo info in productinfos)
                         //{
                         //    if (info.InfoType == "ShipDate")
                         //    {
                         //        sShipDate = info.InfoValue;
                         //        break;
                         //    }
                         //}
                         // *********************** Get Ship Date*******************************
                  
                         retrunValue.Add(sShipDate);
                         retrunValue.Add("1");
                        retrunValue.Add(product.Model);
                        mo=product.MO;

                        retrunValue.Add(product.MO);
                        retrunValue.Add(prodid);
                
          
                    // Check Prodid....
                
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "013Reprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, product.MO);
                    Session.AddValue(Session.SessionKeys.MONO, product.MO);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PCode, pCode);
                    //2010-02-03   207006     ITC-1122-0078 

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
                  

                IList<PrintItem> returnList =(IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
     
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
                logger.Debug("(TravelCardPrint)ReprintLabel end, mo:" +mo + " startProdId:" + prodid + " endProdId:" + prodid + " editor:" + editor + " station:" + station + " customerId:" + customer + " reason:" + reason);

            }
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
                //throw new  SysException(e);
            }


        
        }
        public ArrayList ReprintLabel(
            string mo,
            string startProdId,
            string endProdId,
            string pCode,
            string editor, string station, string customer,
            string reason, IList<PrintItem> printItems)
        {
            logger.Debug("(TravelCardPrint)ReprintLabel start, mo:" + mo + " startProdId:" + startProdId + " endProdId:" + endProdId + " editor:" + editor + " station:" + station + " customerId:" + customer + " reason:" + reason);

            FisException ex;
            List<string> erpara = new List<string>();
            IList<PrintItem> printList;
            try
            {
                string sessionKey = mo;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "013Reprint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, startProdId);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, endProdId);
                    Session.AddValue(Session.SessionKeys.Reason, reason);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, mo);
                    Session.AddValue(Session.SessionKeys.MONO, mo);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.PCode, pCode);
                    //2010-02-03   207006     ITC-1122-0078 

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

                ArrayList retrunValue = new ArrayList();

                IProductRepository pRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<string> range = pRepository.GetProductIdsByRange(startProdId, endProdId);
                IList<PrintItem> returnList = this.getPrintList(Session);
                retrunValue.Add(range);
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
                logger.Debug("(TravelCardPrint)ReprintLabel end, mo:" + mo + " startProdId:" + startProdId + " endProdId:" + endProdId + " editor:" + editor + " station:" + station + " customerId:" + customer + " reason:" + reason);

            }
        }


     

        #region IMO Members

        public IList<MOInfo> GetMOList(string modelId)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            return (List<MOInfo>)moRepository.GetMOListFor013();
        }

        MOInfo IMO.GetMOInfo(string MOId)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            var currentMO = moRepository.Find(MOId);

            MOInfo currentMOInfo = new MOInfo();
            currentMOInfo.qty = currentMO.Qty;
            currentMOInfo.pqty = currentMO.PrtQty;
            return currentMOInfo;
        }

        #endregion

        public DataTable GetPrintLogProIdListByDataTable(string mo)
        {



            string strSQL = "select * from PrintLog where Descr=@MO order by BegNo ";

            SqlParameter paraName = new SqlParameter("@MO", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = mo;

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName);

            return tb;
        }
        private IList<string> GetPrintLogProIdListByList(string mo)
        {
          
            string strSQL = @" select distinct BegNo from PrintLog where Descr=@MO union 
                                            select distinct EndNo from PrintLog where Descr=@MO order by BegNo  ";
           
            SqlParameter paraName = new SqlParameter("@MO", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = mo;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName);
            IList<string> ret = new List<string>();
            foreach (DataRow dr in tb.Rows)
            {
                ret.Add(dr["BegNo"].ToString());
              //  ret.Add(dr["EndNo"].ToString());
              
            }
            return ret;
        }

     



        private string CutOutSeq(string orig, out string preStr, int length)
        {
            string ret = string.Empty;
            preStr = orig.Substring(0, orig.Length - length);
            ret = orig.Substring(orig.Length - length);
            return ret;
        }
        public IList<IMES.DataModel.ProdIdRangeInfo> GetPrintLogProdIdRangeList(string MOId)
        {
            try
            {
                IList<IMES.DataModel.ProdIdRangeInfo> ret = new List<IMES.DataModel.ProdIdRangeInfo>();

                IList<string> lst = GetPrintLogProIdListByList(MOId);

                //规则冗余,应该先执行Rule然后取得.
                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789", 6, "999999", "000000", '0');

                //if (seqCvt == null || seqCvt.NumberRule == null)
                //    throw new Exception("Cannot find the ISequenceConverter for Product ID!"); 

                if (lst != null && lst.Count > 0)
                {
                    string preStr_last = null;
                    string seq_last = null;
                    foreach (string proId in lst)
                    {
                        bool isContinue = false;

                        string preStr = string.Empty;
                        string seq = CutOutSeq(proId, out preStr, seqCvt.NumberRule.iBits);

                        isContinue = (preStr_last != null && preStr_last == preStr && seq_last != null && seqCvt.NumberRule.IncreaseToNumber(seq_last, 1) == seq);
                        if (isContinue)
                        {
                            IMES.DataModel.ProdIdRangeInfo piri = ret[ret.Count - 1];
                            piri.endId = proId;
                            ret.RemoveAt(ret.Count - 1);
                            ret.Add(piri);
                        }
                        else
                        {
                            IMES.DataModel.ProdIdRangeInfo piri = new IMES.DataModel.ProdIdRangeInfo();
                            piri.endId = piri.startId = proId;
                            ret.Add(piri);
                        }
                        preStr_last = preStr;
                        seq_last = seq;
                    }
                }

                #region . OLD .
                //_Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
                //if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                //{
                //    sqlCtx = new _Schema.SQLContext();
                //    sqlCtx.Sentence =   "SELECT MAX({0}) as productID" +
                //                        "  FROM {1} WHERE {2}=%{2} " + 
                //                        "UNION " +
                //                        "SELECT MIN({0}) as productID " +
                //                        "  FROM {1} WHERE {2}=%{2} " + 
                //                        "ORDER BY productID DESC";

                //    sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Product.fn_ProductID,
                //                                                    typeof(_Schema.Product).Name,
                //                                                    _Schema.Product.fn_MO
                //                                                    );

                //    sqlCtx.Params.Add(_Schema.Product.fn_MO, new SqlParameter("@" + _Schema.Product.fn_MO, SqlDbType.VarChar));
                //    _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                //}
                //}
                //sqlCtx.Params[_Schema.Product.fn_MO].Value = MOId;
                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_FA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                //{
                //    if (sqlR != null)
                //    {
                //        ProdIdRangeInfo pri = null;
                //        while (sqlR.Read())
                //        {
                //            if (null == pri)
                //                pri = new ProdIdRangeInfo();

                //            ret.Add
                //        }
                //    }
                //}
                #endregion

                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
