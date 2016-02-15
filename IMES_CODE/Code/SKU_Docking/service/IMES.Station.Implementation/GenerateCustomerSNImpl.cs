﻿/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Generate Customer SN
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-01   Du.Xuan               Create   
* ITC-1360-1020 增加zzzz上限防护
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IGenerateCustomerSN接口的实现类
    /// </summary>
    public class GenerateCustomerSNImpl : MarshalByRefObject, IGenerateCustomerSN 
    {
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IGenerateCustomerSN members

        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        public ArrayList InputProdId(string pdLine, string prodId, string editor, string stationId, string customer)
        {
            logger.Debug("(GenerateCustomerSNImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "022GenerateCustomerSN.xoml", "022GenerateCustomerSN.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

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

                ArrayList retList = new ArrayList();

                Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                ProductModel curProduct = new ProductModel();

                curProduct.CustSN = tempProduct.CUSTSN;
                curProduct.ProductID = tempProduct.ProId;
                curProduct.Model = tempProduct.Model;


                //调用LabelType时，增加以下判断：
                //若该Product的Family.toUpper()中包含’ AKASHI’，则LabelType用’SN_AKASHI_Label’
                //若Product的Family.toUpper()中不包含’ AKASHI’，则LabelType用’SN Label’
                string labelType = "";
                if (tempProduct.Family.ToUpper().IndexOf("AKASHI") < 0)
                {
                    labelType = "SN Label";
                }
                else
                {
                    labelType = "SN_AKASHI_Label";
                }
                retList.Add(curProduct);
                retList.Add(labelType);

                string WhetherNoCheckDDRCT = session.GetValue("WhetherNoCheckDDRCT") as string;
                string WhetherCheckMBSno = session.GetValue("isJamestown") as string;
                WhetherCheckMBSno = "N";
                retList.Add(WhetherNoCheckDDRCT);
                retList.Add(WhetherCheckMBSno);

                return retList;
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
                logger.Debug("(GenerateCustomerSNImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 输入DDRCT和相关信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ddrct">ddrct</param>
        public ArrayList InputDDRCT(string prodId, string ddrct)
        {
            logger.Debug("(GenerateCustomerSNImpl)InputDDRCT start, [prodId]:" + prodId + ", [ddrct]:" + ddrct);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    bool existDDRCT = false;
                    IList<IProductPart> productParts = curProduct.ProductParts;
                    foreach (ProductPart part in productParts)
                    {
                        if ("DDR".Equals(part.CheckItemType) && ddrct.Equals(part.PartSn))
                        {
                            existDDRCT = true;
                            break;
                        }
                    }
                    if (!existDDRCT)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK1014", errpara); // 请核对流程卡
                        throw e;
                    }                
                 
                  }

                    ArrayList retList = new ArrayList();
                    Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                    retList.Add(tempProduct.CUSTSN);
                    return retList;
                
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
                logger.Debug("(GenerateCustomerSNImpl)InputDDRCT end, [prodId]:" + prodId + ", [ddrct]:" + ddrct);
            }
        }
        /// <summary>
        /// 输入MB和相关信息
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ddrct">mbsno</param>
        public ArrayList InputMBSno(string prodId, string mbsno)
        {
            logger.Debug("(GenerateCustomerSNImpl)InputMBSno start, [prodId]:" + prodId + ", [mbsno]:" + mbsno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    bool existmbsno = false;
                    IList<IProductPart> productParts = curProduct.ProductParts;
                    foreach (ProductPart part in productParts)
                    {
                        if ("MB".Equals(part.CheckItemType) && mbsno.Equals(part.PartSn))
                        {
                            existmbsno = true;
                            break;
                        }
                    }
                    if (!existmbsno)
                    {
                        List<string> errpara = new List<string>();
                        FisException e = new FisException("CHK1014", errpara); // 请核对流程卡
                        throw e;
                    }

                }

                ArrayList retList = new ArrayList();
                Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                retList.Add(tempProduct.CUSTSN);
                return retList;

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
                logger.Debug("(GenerateCustomerSNImpl)InputMBSno end, [prodId]:" + prodId + ", [mbsno]:" + mbsno);
            }
        }
        /// <summary>
        /// SAVE
        /// </summary>
        /// <param name="prodId">Product Id</param>
 
        public ArrayList save(string prodId)
        {
            logger.Debug("(GenerateCustomerSNImpl)save start, [prodId]:" + prodId );
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {

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

                    ArrayList retList = new ArrayList();
                    Product tempProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                    retList.Add(tempProduct.CUSTSN);
                    return retList;
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
                logger.Debug("(GenerateCustomerSNImpl)Save end, [prodId]:" + prodId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Print(string prodId, IList<PrintItem> printItems)
        {
            logger.Debug("(GenerateCustomerSNImpl)print start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);
  
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    session.AddValue(Session.SessionKeys.IsComplete, true);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    session.AddValue(Session.SessionKeys.PrintLogName, curProduct.Customer + "SNO");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, curProduct.CUSTSN);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, curProduct.CUSTSN);
                    session.AddValue(Session.SessionKeys.PrintLogDescr, curProduct.ProId);

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

                    IList<PrintItem> returnList = this.getPrintList(session);
                    return returnList;
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
                logger.Debug("(GenerateCustomerSNImpl)print end, [prodId]:" + prodId);
            }
        }

        /// <summary>
        /// 重新列印Customer SN
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        public ArrayList Reprint(string prodId, string editor, string stationId, string customer, IList<PrintItem> printItems, string reason, out string customerSN)
        {
            logger.Debug("(GenerateCustomerSNImpl)Reprint start,"
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            //string sessionKey = prodId;
            IMES.FisObject.FA.Product.IProduct product = null;

            try
            {
                //修改为输入custSN
                var currentProduct = CommonImpl.GetProductByInput(prodId, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                string sessionKey = currentProduct.ProId;

                ArrayList retList = new ArrayList();
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, string.Empty, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    //session.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    //session.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);
                    session.AddValue(Session.SessionKeys.Reason, reason);
                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("022ReprintServiceTag.xoml", null, wfArguments);

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

                product = (IMES.FisObject.FA.Product.IProduct)session.GetValue(Session.SessionKeys.Product);

                customerSN = (product == null ? string.Empty : product.CUSTSN);
                IList<PrintItem> printList = this.getPrintList(session);

                ProductModel curproc = new ProductModel();
                curproc.CustSN = product.CUSTSN;
                curproc.ProductID = product.ProId;
                curproc.Model = product.Model;

                string labelType = "";
                if (product.Family.ToUpper().IndexOf("AKASHI") < 0)
                {
                    labelType = "SN Label";
                }
                else
                {
                    labelType = "SN_AKASHI_Label";
                }

                retList.Add(printList);
                retList.Add(curproc);
                retList.Add(labelType);

                return retList;
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
                logger.Debug("(GenerateCustomerSNImpl)Reprint end,"
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
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

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(GenerateCustomerSNImpl)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

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
                logger.Debug("(GenerateCustomerSNImpl)Cancel end, [prodId]:" + prodId);                
            }
        }

        #endregion
    }
}
