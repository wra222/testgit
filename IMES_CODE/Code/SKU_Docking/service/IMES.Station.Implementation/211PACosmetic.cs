﻿/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: PACosmeticImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-10-20   zhu lei           Create 
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
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Collections;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IFACosmetic接口的实现类
    /// </summary>
    public class PACosmeticImpl : MarshalByRefObject, IPACosmetic
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IPACosmetic members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custsn">Cust sn</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        /// <returns>prestation</returns>
        public string InputCustSn(string pdLine, string custsn, string editor, string stationId, string customer)
        {
            logger.Debug("(PACosmeticImpl)InputCustSn start, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
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
                    RouteManagementUtils.GetWorkflow(stationId, "123PACosmetic.xoml", "123PACosmetic.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.CustSN, sessionKey);
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

                var checkItem = (string)session.GetValue(Session.SessionKeys.C);
                var isEpia = (string)session.GetValue(ExtendSession.SessionKeys.FAQCStatus);
                if (isEpia == "EPIA")
                {
                    erpara.Add(sessionKey);
                    FisException e = new FisException("CHK402",erpara);
                    throw e;
                }

                return checkItem;
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
                logger.Debug("(PACosmeticImpl)InputCustSn end, [pdLine]:" + pdLine
                    + " [custsn]: " + custsn
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 输入Product Id相关信息并处理For CQ
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custsn">Cust sn</param>
        /// <param name="editor">Editor</param>
        /// <param name="stationId">Station</param>
        /// <param name="customer">Customer</param>
        /// <returns>prestation</returns>
        public ArrayList InputCustSnForCQ(string pdLine, string custsn, string editor, string stationId, string customer)
        {
            logger.Debug("(PACosmeticImpl)InputCustSnForCQ start, [pdLine]:" + pdLine
                + " [custsn]: " + custsn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
            //For CQ
            var currentProduct = CommonImpl.GetProductByInput(custsn, CommonImpl.InputTypeEnum.CustSN);
            if (currentProduct == null)
            {
                FisException fe = new FisException("CHK079", new string[] { custsn });
                throw fe;
            }
            string modelType = GetModelType(currentProduct.Model);
        
         
            //For CQ
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
                    RouteManagementUtils.GetWorkflow(stationId, "123PACosmetic.xoml", "123PACosmetic.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.IsComplete, false);
                    session.AddValue(Session.SessionKeys.CustSN, sessionKey);
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
                string color = (string)session.GetValue("CoverColorMsg");
                var checkItem = (string)session.GetValue(Session.SessionKeys.C);

                if ((string)session.GetValue("IsCheckWWAN") == "N")
                 {checkItem= checkItem.Replace("WWAN", ""); }
                if ((string)session.GetValue("IsCheckPCID") == "N")
                { checkItem = checkItem.Replace("PCID", ""); }

                var isEpia = (string)session.GetValue(ExtendSession.SessionKeys.FAQCStatus);
                if (isEpia == "EPIA")
                {
                    erpara.Add(sessionKey);
                    FisException e = new FisException("CHK402", erpara);
                    throw e;
                }
                string SIMCardAlertMessage =  (string)session.GetValue("SIMCardAlertMessage");
              

                ArrayList arr = new ArrayList();
                arr.Add(checkItem);
                arr.Add(modelType + "," + color);
                arr.Add(SIMCardAlertMessage);
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
                throw e;
            }
            finally
            {
                logger.Debug("(PACosmeticImpl)InputCustSnForCQ end, [pdLine]:" + pdLine
                    + " [custsn]: " + custsn
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 不良品信息。
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="defectList">Defect IList</param>
        public string InputDefectCodeList(string custsn, IList<string> defectList, out string setMsg)
        {
            logger.Debug("(PACosmeticImpl)InputDefectCodeList start,"
                + " [custsn]: " + custsn
                + " [defectList]:" + defectList
                );
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
            string qcMethod = "None";
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                    if (defectList.Count == 0)
                        defectList = null;
                    session.AddValue(Session.SessionKeys.DefectList, defectList);

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
                  
                    try
                    {
                        if (true == (bool)session.GetValue("isPIA"))
                        {
                            qcMethod = "isPIA";
                        }
                        else
                        {
                            qcMethod = "noPIA";
                        }

                        //if (Session.Station !="60")
                        if (false == (bool)session.GetValue("isStationPIA"))
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
                }
                setMsg = (string)session.GetValue(ExtendSession.SessionKeys.WarningMsg);
                return qcMethod;
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
                logger.Debug("(PACosmeticImpl)InputDefectCodeList end,"
                   + " [custsn]: " + custsn
                   + " [defectList]:" + defectList);
            }
        }

        /// <summary>
        /// check pcid
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="pcid">PCID</param>
        /// <returns></returns>
        public bool checkpcid(string custsn, string pcid)
        {
            logger.Debug("(PACosmeticImpl)checkpcid start,"
                + " [custsn]: " + custsn
                + " [pcid]: " + pcid);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
            bool errorFlg = false;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                IProduct product = null;
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    IList<ProductInfo> infos = null;

                    infos = product.ProductInfoes;
                    var productInfos = (from info in infos
                                       where (info.InfoType == "PCID" && info.InfoValue == pcid)
                                       select info).ToArray();
                    if (productInfos.Count() == 0)
                    {
                        errorFlg = true;
                        //throw new FisException("CHK167", erpara);
                        //throw ex;
                    }
                }
                return errorFlg;
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
                logger.Debug("(PACosmeticImpl)checkpcid end,"
                    + " [custsn]: " + custsn
                    + " [pcid]: " + pcid);
            }
        }

        /// <summary>
        /// check wwan
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="wwan">WWAN</param>
        /// <returns></returns>
        public bool checkwwan(string custsn, string wwan)
        {
            logger.Debug("(PACosmeticImpl)checkwwan start,"
                + " [custsn]: " + custsn
                + " [wwan]: " + wwan);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = custsn;
            bool errorFlg = false;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                IProduct product = null;
                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    product = (IProduct)session.GetValue(Session.SessionKeys.Product);
                    IList<ProductInfo> infos = null;
                    IList<ProductInfo> productInfos;
                    infos = product.ProductInfoes;
                    var productInfos_AICCID = (from info in infos
                                               where ((info.InfoType == "AICCID" || info.InfoType == "SICCID") && info.InfoValue != ":")
                                               select info).ToArray();
                    if (productInfos_AICCID.Count() > 0)
                    {
                         productInfos = (from info in infos
                                            where ((info.InfoType == "AICCID" || info.InfoType == "SICCID") && info.InfoValue != ":" && info.InfoValue == wwan)
                                            select info).ToArray();
                    }
                    else
                    {
                         productInfos = (from info in infos
                                            where (info.InfoType != "PCID" && info.InfoValue == wwan && info.InfoValue != ":")
                                            select info).ToArray();
                    }
                    if (productInfos.Count() == 0)
                    {
                        errorFlg = true;
                        //throw new FisException("CHK168", erpara);
                        //throw ex;
                    }
                }
                return errorFlg;
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
                logger.Debug("(PACosmeticImpl)checkwwan end,"
                    + " [custsn]: " + custsn
                    + " [wwan]: " + wwan);
            }
        }

        /// <summary>
        /// get Other Tips
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="swc">SWC</param>
        /// <returns>string</returns>
        public string getOtherTips(string productId, string swc)
        {
            logger.Debug("(PACosmeticImpl)getOtherTips start,"
                + " [productId]: " + productId
                + " [swc]: " + swc);
            List<string> erpara = new List<string>();
            string ret = string.Empty;
            IList<string> tips;

            try
            {
                //Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                tips = productRepository.GetMessageListFromSpecialDetAndSpecialMaintain(productId,swc);
                if (tips.Count() > 0)
                {
                    ret = tips[0].ToString();
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
                throw e;
            }
            finally
            {
                logger.Debug("(PACosmeticImpl)getOtherTips end,"
                    + " [productId]: " + productId
                    + " [swc]: " + swc);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            logger.Debug("(PACosmeticImpl)Cancel start, [custsn]:" + sessionKey);
            //FisException ex;
            List<string> erpara = new List<string>();

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
                logger.Debug("(PACosmeticImpl)Cancel end, [custSn]:" + sessionKey);
            }
        }

        //public string GetLineSpeed(string Line, string station)
        //{
            
        //    logger.Debug("(PACosmeticImpl)Cancel start");

        //    try
        //    {
        //        ILineRepository iLineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
        //        Line line = iLineRepository.Find(Line);
        //        IList<LineSpeed> ListSpeed = line.LineSpeed;
        //        var Query = (from s in ListSpeed
        //                     where s.Station == station
        //                     select s).ToList<LineSpeed>();
        //        if (Query.Count > 0)
        //        {
        //            if (Query[0].LimitSpeed != 0)
        //            { return Query[0].LimitSpeed.ToString() + "L"; }
        //            else if (Query[0].PassLimitSpeed != 0)
        //            { return Query[0].PassLimitSpeed.ToString() + "P"; }
        //            else { return "-1"; }
        //        }
        //        else
        //        {
        //            return "-1";
        //        }
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
        //        logger.Debug("(PACosmeticImpl)Cancel end");
        //    }
        //}

        public IList<string> GetLineSpeed(string Line, string station)
        {

            logger.Debug("(PACosmeticImpl)Cancel start");

            try
            {
                ILineRepository iLineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                Line line = iLineRepository.Find(Line);
                IList<LineSpeed> ListSpeed = line.LineSpeed;
                IList<string> ret = new List<string>();
                var Query = (from s in ListSpeed
                             where s.Station == station
                             select s).ToList<LineSpeed>();
                if (Query.Count > 0)
                {
                    ret.Add(Query[0].Station.ToString());
                    ret.Add(Query[0].AliasLine.ToString());
                    ret.Add(Query[0].LimitSpeed.ToString());
                    ret.Add(Query[0].IsCheckPass.ToString());
                    ret.Add(Query[0].LimitSpeedExpression.ToString());
                    ret.Add(Query[0].IsHoldStation.ToString());
                    return ret;
                }
                else
                {
                    return ret;
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
                logger.Debug("(PACosmeticImpl)Cancel end");
            }
        }

        private string GetModelType(string model)
        {
            string r ="";
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model mdl = modelRepository.Find(model);
            string value = (string)mdl.GetAttribute("Infor");
            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf("HP")>0)
                { r = "HP"; }
                else if (value.IndexOf("CP") > 0)
                { r = "CQ"; }
            }
            return r;
        }
      
        #endregion

    }
}