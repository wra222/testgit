﻿/*
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
using System.Linq;
using System.Text;
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
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.PrintLog;


namespace IMES.Station.Implementation
{
    public class _BoardInput : MarshalByRefObject, IBoardInput
    {


        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    private string OriSessionkey;
    
        #region IBoardInput Members
        public IList<BomItemInfo> InputProdIdorCustsn(string pdLine, string input, string editor, string stationId, string customerId, out string realProdID, out string model)
        {
            logger.Debug("(_BoardInput)InputProdId start, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            var currentProduct = productRepository.GetProductByIdOrSn(input);

            //var currentProduct = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

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
                //ArrayList retLst = new ArrayList();
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

                    // WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("_TEST.xoml", "_TEST.rules", wfArguments);
                //    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("021BoardInput.xoml", "021BoardInput.rules", wfArguments); 
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "021BoardInput.xoml", "021BoardInput.rules", out wfName, out rlName);
               //   RouteManagementUtils.GetWorkflow(stationId, "021BoardInput_TEST.xoml", "021BoardInput_TEST.rules", out wfName, out rlName);
                  
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
             
                    Session.AddValue(Session.SessionKeys.IsComplete, false);
                    //Session.AddValue(Session.SessionKeys.ifElseBranch, false);
                    Session.AddValue(Session.SessionKeys.CustSN, sessionKey);
                    Session.AddValue("isMB", "0");
                    Session.AddValue("IsRCTOMB", "Y");


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
                //get product data for UI
               // DataModel.ProductInfo prodInfo = currentProduct.ToProductInfo();
               // retLst.Add(prodInfo);

                //get bom
                return PartCollection.GeBOM(sessionKey, TheType);
                //retLst.Add(bomItemList);
             
                //var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);
                //model = prod.Model;
                //realProdID= prod.ProId;
                //add mb info in GetCheckItemList
              //  IList<BomItemInfo> ret = GetCheckItemList(Session, prod.PCBID, prod.PCBModel);
                //return retLst;
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
                logger.Debug("(_BoardInput)InputProdId end, pdLine:" + pdLine + " input:" + input + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        public IMES.DataModel.MatchedPartOrCheckItem InputMBSn(string prodId, string item)
        {
            logger.Debug("(_BoardInput)InputMBSn start, prodId:" + prodId + " item:" + item);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
          //  string sessionKey = OriSessionkey;

            Session Session = SessionManager.GetInstance.GetSession(prodId, TheType);
            if (Session == null)
            {

                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {

                ArrayList returnItem = new ArrayList();

                Session.Exception = null;
                Session.AddValue(Session.SessionKeys.ValueToCheck, item);
                Session.InputParameter = item;
                Session.SwitchToWorkFlow();

                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

                //2010-02-03   207006     ITC-1122-0079
                //IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo
                PartUnit matchedPart = (PartUnit)Session.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = item;
                    tempMatchedPart.ValueType = matchedPart.ValueType;
                    return tempMatchedPart;
                }

/*                IList<IBOMPart> bomPartlist = (IList<IBOMPart>)Session.GetValue(Session.SessionKeys.MatchedParts);
  
                if ((bomPartlist != null) && (bomPartlist.Count > 0))
                {
                    foreach (IBOMPart bompartitem in bomPartlist)
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        tempMatchedPart.PNOrItemName = bompartitem.PN;
                        tempMatchedPart.CollectionData = item;
                        //tempMatchedPart.CollectionData = bompartitem.MatchedSn; itc200052 , 2011.12.13
                        tempMatchedPart.ValueType = bompartitem.Type; //itc200052 , 2011.12.13
                        MatchedList.Add(tempMatchedPart);
                    }
                    return MatchedList;
                }

                else
                {
                    ICheckObject citem = (ICheckObject)Session.GetValue(Session.SessionKeys.MatchedCheckItem);
                    if (citem == null)
                    {
                        throw new FisException("MAT010", new string[] { item });
                    }
                    else
                    {
                        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                        //tempMatchedPart.PNOrItemName = citem.ItemDisplayName; itc200052, 2011.12.13
                        //tempMatchedPart.CollectionData = citem.ValueToCollect; itc200052, 2011.12.13
                        tempMatchedPart.ValueType = "";
                        MatchedList.Add(tempMatchedPart);
                        return MatchedList;
                    }
                } */
                return null;
        
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
                logger.Debug("(_BoardInput)InputMBSn end, prodId:" + prodId + " item:" + item); 
            }

        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, TheType, checkValue);
        }

        public ArrayList saveForPCAShippingLabel(string prodId, string MBno, string model, string dcode, IList<PrintItem> printItems)
        {
            logger.Debug("(_BoardInput)save start, MBno:" + MBno + "model:" + model + "dcode:" + dcode);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            ArrayList ret = new ArrayList();
            
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                //ex.logErr("", "", "", "", "83");
                //logger.Error(ex);
                throw ex;
            }
            try
            {
                Session.Exception = null;
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IMB mb = mbRepository.Find(MBno);

                Session.AddValue(Session.SessionKeys.ModelName, model);
                Session.AddValue(Session.SessionKeys.WarrantyCode, dcode);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                //Session.AddValue(Session.SessionKeys.ifElseBranch, true);
                Session.AddValue(Session.SessionKeys.MB, mb);
                Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                Session.AddValue(Session.SessionKeys.PrintLogBegNo, MBno);
                Session.AddValue(Session.SessionKeys.PrintLogEndNo, MBno);
                Session.AddValue(Session.SessionKeys.PrintLogDescr, prodId);
                //modify 1389 bug
                Session.AddValue("isMB", "1");
                Session.AddValue(Session.SessionKeys.CN, "");

                Session.SwitchToWorkFlow();


                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }

                    throw Session.Exception;
                }

               // var retDCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                var retDCode = dcode;
                var printLst = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);

                ret.Add(retDCode);
                ret.Add(printLst);

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
                logger.Debug("(_BoardInput)save end, MBno:" + MBno + "model:" + model + "dcode:" + dcode);
            }
        }

        //modify 1053 bug
        public IList<PrintItem> Save(string prodId, IList<PrintItem> printItems, bool printflag, out string custsn)
        {
            logger.Debug("(_BoardInput)Save start, prodId:" + prodId);
        //    custsn = "";
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            if (prodId.Trim().Length == 10)
            { prodId = prodId.Trim().Substring(0, 9); }  
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            if (Session == null)
            {
                erpara.Add(sessionKey);
                ex = new FisException("CHK021", erpara);
                 throw ex;
            }
            try
            {

                var mb = (IMB)Session.GetValue(Session.SessionKeys.MB);
                Session.Exception = null;
                Session.AddValue("needprintflag", printflag);

                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                if (mb != null)
                {
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, mb.Sn);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, mb.Sn);
                }
                Session.AddValue(Session.SessionKeys.PrintLogDescr, prodId);

                Session.SwitchToWorkFlow();
                var prod = (IProduct)Session.GetValue(Session.SessionKeys.Product);

                var dCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                custsn = dCode;
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
                if (e.mErrcode.ToString().Trim() == "GEN021")
                {
                    ex = new FisException("GEN047", erpara);
                    throw ex;
                }
                else
                {
                    throw e;
                }
           
            }
            catch (Exception e) 
            {
                throw new SystemException(e.Message);
            }
            finally
            { 
                logger.Debug("(_BoardInput)Save end, prodId:" + prodId);
            }
            return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
        }
 
        public IList<PrintItem> PrintCustsnLabel(IList<PrintItem> printItems, string prodId)
        {
            string sessionKey = prodId;
            Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
            Session.AddValue(Session.SessionKeys.PrintItems, printItems);
            if (Session == null)
            {

                throw new Exception("Print Error");
            }
            try
            {
                Session.AddValue(Session.SessionKeys.IsComplete, true);
                Session.SwitchToWorkFlow();
            
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
                logger.Debug("(_BoardInput)Print Custsn Label, prodId:" + prodId);
            }

            return (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
        }


        public ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            string mo = "";
            try
            {
                logger.Debug("(_BoardInput)Reprint start, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
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
                    RouteManagementUtils.GetWorkflow(station, "021BoardInputReprint.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);
                    Session.AddValue(Session.SessionKeys.Reason, reason);

                    Session.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);

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

                Product product = (Product)Session.GetValue(Session.SessionKeys.Product);
                
                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                var mb = mbRepository.Find(product.PCBID);
                if (mb == null)
                {
                    throw ex = new FisException("SFC001", new string[] { product.PCBID });
                    //throw ex;
                }
                string dcode = mb.DateCode;
                string mbsn = product.PCBID;
                string action = "0";

                retrunValue.Add(action);

                retrunValue.Add(mbsn);
                retrunValue.Add(dcode);
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
                logger.Debug("(_BoardInput)Reprint end, ProdId:" + prodid + " line:" + line + " editor:" + editor + " customerId:" + customer);
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="mbSno"></param>
        ///// <param name="reason"></param>
        ///// <param name="line"></param>
        ///// <param name="editor"></param>
        ///// <param name="station"></param>
        ///// <param name="customer"></param>
        ///// <param name="printItems"></param>
        ///// <returns></returns>
        //public ArrayList ReprintLabel(string Sno, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems)
        //{
        //    logger.Debug("(_BoardInput)ReprintLabel start, [mbSno]:" + Sno
        //        + " [reason]: " + reason
        //        + " [line]: " + line
        //        + " [editor]:" + editor
        //        + " [station]:" + station
        //        + " [customer]:" + customer);

        //    FisException ex;
        //    List<string> erpara = new List<string>();
        //    bool isProduct = false;
        //    bool bFlag = false;
        //    var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
        //    bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(printItems[0].LabelType, Sno);
        //    if (!bFlag)
        //    {
        //        ex = new FisException("CHK270", erpara);
        //        throw ex;
        //    }

        //    //if (mbSno.Length == 9)
        //    //{
        //    //    var objProduct = CommonImpl.GetProductByInput(mbSno, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

        //    //    if (String.IsNullOrEmpty(objProduct.PCBID))
        //    //    {
        //    //        erpara.Add(mbSno);
        //    //        ex = new FisException("CHK400", erpara);
        //    //        throw ex;
        //    //    }

        //    //    mbSno = objProduct.PCBID;
        //    //    isProduct = true;
        //    //}

        //    string sessionKey = Sno;

        //    try
        //    {
        //        Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
        //        if (session == null)
        //        {
        //            session = new Session(sessionKey, TheType, editor, station, string.Empty, customer);
        //            Dictionary<string, object> wfArguments = new Dictionary<string, object>();

        //            wfArguments.Add("Key", sessionKey);
        //            wfArguments.Add("Station", station);
        //            wfArguments.Add("CurrentFlowSession", session);
        //            wfArguments.Add("Editor", editor);
        //            wfArguments.Add("PdLine", string.Empty);
        //            wfArguments.Add("Customer", customer);
        //            wfArguments.Add("SessionType", TheType);
        //            WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("RePrint.xoml", string.Empty, wfArguments);

        //            session.AddValue(Session.SessionKeys.ProductIDOrCustSN, Sno);
        //            session.AddValue(Session.SessionKeys.PrintItems, printItems);
        //            session.AddValue(Session.SessionKeys.3, "BoardInput");
        //            session.AddValue(Session.SessionKeys.PrintLogBegNo, Sno);
        //            session.AddValue(Session.SessionKeys.PrintLogEndNo, Sno);
        //            session.AddValue(Session.SessionKeys.PrintLogDescr, "");
        //            session.AddValue(Session.SessionKeys.Reason, reason);
        //            session.AddValue("isProduct", isProduct);
        //            session.SetInstance(instance);

        //            if (!SessionManager.GetInstance.AddSession(session))
        //            {
        //                session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
        //                erpara.Add(sessionKey);
        //                ex = new FisException("CHK020", erpara);
        //                throw ex;
        //            }

        //            session.WorkflowInstance.Start();
        //            session.SetHostWaitOne();
        //        }
        //        else
        //        {
        //            erpara.Add(sessionKey);
        //            ex = new FisException("CHK020", erpara);
        //            throw ex;
        //        }

        //        if (session.Exception != null)
        //        {
        //            if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
        //            {
        //                session.ResumeWorkFlow();
        //            }

        //            throw session.Exception;
        //        }

        //        ArrayList ret = new ArrayList();
        //        //IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

        //        //var mb = mbRepository.Find(Sno);
        //        //string dCode = mb.DateCode;
        //        //string shipMode = mb.ShipMode;
        //        //string model = mbRepository.GetPCBInfoValue(mbSno, "FRUModel");
        //        //if (string.IsNullOrEmpty(model))
        //        //{
        //        //    model = mb.PCBModelID;
        //        //}

        //        var printLst = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);

        //        //ret.Add(dCode);
        //        ret.Add(printLst);
        //        //ret.Add(isProduct);
        //        //ret.Add(mbSno);
        //        //ret.Add(shipMode);
        //        //ret.Add(model);

        //        return ret;
        //    }
        //    catch (FisException e)
        //    {
        //        logger.Error(e.mErrmsg);
        //        throw e;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e.Message);
        //        throw e;
        //    }
        //    finally
        //    {
        //        logger.Debug("(_BoardInput)ReprintLabel end, [mbSno]:" + Sno
        //            + " [reason]: " + reason
        //            + " [line]: " + line
        //            + " [editor]:" + editor
        //            + " [station]:" + station
        //            + " [customer]:" + customer);
        //    }
        //}

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

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string SetDataCodeValue(string model, string customer)
        {
            string value = "";
            string str = "";
            string ret = "";

            try
            {
                logger.Debug("SetDataCodeValue start, model:" + model + "customer:" + customer);

                IWarrantyRepository wr = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
                IList<Warranty> warrantys = wr.GetDCodeRuleListForMB("Customer");

                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model mdl = modelRepository.Find(model);
                value = (string)mdl.GetAttribute("WRNT");
                if (value != "")
                {
                    str = value.Substring(0, 1);
                }

                if (str == "1")
                {
                    var val = (from w in warrantys
                               where (w.WarrantyCode == "4")
                               select w.Id.ToString()).ToArray();
                    ret = val[0].ToString();
                }
                else if (str == "0")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "0"
                               select w.Id.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                else if (str == "3")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "5"
                               select w.Id.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                else if (str == "9")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "3"
                               select w.Id.ToString()).ToArray();
                    ret = val[0].ToString();

                }
                //变更描述： 增加DCode与保固期之间的对应关系 modify by itc200052, 2012-5-17
                else if (str == "2")
                {
                    var val = (from w in warrantys
                               where w.WarrantyCode == "C"
                               select w.Id.ToString()).ToArray();
                    ret = val[0].ToString();

                }

                return ret;
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
                logger.Debug("SetDataCodeValue end, model:" + model + "customer:" + customer);
            }
        }


        #endregion
    }
}
