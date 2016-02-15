/*
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
        /// 不良品信息。
        /// </summary>
        /// <param name="custsn">Cust Sn</param>
        /// <param name="defectList">Defect IList</param>
        public void InputDefectCodeList(string custsn, IList<string> defectList)
        {
            logger.Debug("(PACosmeticImpl)InputDefectCodeList start,"
                + " [custsn]: " + custsn
                + " [defectList]:" + defectList);
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

                    infos = product.ProductInfoes;
                    var productInfos = (from info in infos
                                       where (info.InfoType != "PCID" && info.InfoValue == wwan && info.InfoValue != ":")
                                       select info).ToArray();
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
            FisException ex;
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

        #endregion

    }
}