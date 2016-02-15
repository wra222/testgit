/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* ITC-1360-1398: 完善label打印检查流程
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPDPALable01接口的实现类
    /// </summary>
    public class PDPALabel01 : MarshalByRefObject, IPDPALabel01
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="queryflag"></param>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="code"></param>
        /// <param name="floor"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSN(Boolean queryflag, string custSN, string line, string code, string floor,
                                                    string editor, string station, string customer)
        {
            logger.Debug("(PDPALabel01)InputSN start, custSn:" + custSN);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    //wfArguments.Add("Floor", floor);
                    //wfArguments.Add("Code", code);

                    string wfName, rlName;

                    string xmlname = "";
                    if (!queryflag)
                    {
                        xmlname = "PDPALabel01.xoml";
                    }
                    else
                    {
                        xmlname = "PDPALabel01Query.xoml";
                    }
                    RouteManagementUtils.GetWorkflow(station, xmlname, "", out wfName, out rlName);//PDPLabel01.rules
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Floor, floor);
                    currentSession.AddValue(Session.SessionKeys.MBCode, code);
                    currentSession.AddValue(Session.SessionKeys.IsPalletChange, queryflag);

                    if (!queryflag)
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    }
                    else
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, true);

                    }
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                //========================================================
                ArrayList retList = new ArrayList();
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                ProductModel curModel = new ProductModel();
                IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)currentSession.GetValue("WipBuffer");

                /*IList<WipBuffer> wipBufferList = new List<WipBuffer>();
                for (int i = 1; i < 14; i++)
                {
                    WipBuffer item = new WipBuffer();
                    item.PartNo = "PartNo"+Convert.ToString(i);
                    item.Tp = "TP"+Convert.ToString(i);
                    item.Qty = i;
                    wipBufferList.Add(item);
                }
                */

                string wlabel = (string)currentSession.GetValue("WLabel");
                string clabel = (string)currentSession.GetValue("LanguageLabel");
                string cmessage = (string)currentSession.GetValue("LanguageMessage");
                string llabel = (string)currentSession.GetValue("LANOMLabel");

                curModel.ProductID = curProduct.ProId;
                curModel.CustSN = curProduct.CUSTSN;
                curModel.Model = curProduct.Model;
                retList.Add(curModel);
                retList.Add(wipBufferList);

                retList.Add(wlabel);
                retList.Add(clabel);
                retList.Add(cmessage);
                retList.Add(llabel);
                //========================================================

                return retList;

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
                logger.Debug("(PDPALabel01)InputSN end, uutSn:" + custSN);
            }
        }

        public IList<PrintItem> Print(string productID, string line, string code, string floor,
                              string editor, string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PDPALabel1)Print start, ProductID:" + productID + " pdLine:" + line + " stationId:" + station + " editor:" + editor);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(PDPALabel1)Print end, ProductID:" + productID);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void save(string prodId, IList<string> defectCodeList)
        {
            logger.Debug("(PDPALabel01)save start,"
                + " [prodId]: " + prodId
                + " [defectList]:" + defectCodeList);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                    session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? true : false);
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
                logger.Debug("(PDPALabel01)save end,"
                   + " [prodId]: " + prodId
                   + " [defectList]:" + defectCodeList);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string prodId)
        {
            logger.Debug("(PDPALabel01)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                logger.Debug("(PDPALabel01)Cancel end, [prodId]:" + prodId);
            }
        }

        public ArrayList ReprintLabel(string customerSN, string reason, IList<PrintItem> printItems, 
            string line, string editor, string station, string customer)
        {
            logger.Debug("(PDPALabel01)ReprintLabel start, customerSN:" + customerSN
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {

                var currentProduct = CommonImpl.GetProductByInput(customerSN, CommonImpl.InputTypeEnum.CustSN);
                string sessionKey = currentProduct.ProId;

                var repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //IList<ProductLog> logList = repository.GetProductLogs("91", 1, line);

                IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,"91"); 

                if (logList.Count == 0) 
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentProduct.ProId);
                    ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                    throw ex;
                }

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ReprintPDPALabel01.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                //===============================================================================
                //Get infomation
                ArrayList retList = new ArrayList();
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(returnList);

                string wlabel = (string)currentSession.GetValue("WLabel");
                string clabel = (string)currentSession.GetValue("LanguageLabel");
                string cmessage = (string)currentSession.GetValue("LanguageMessage");
                string llabel = (string)currentSession.GetValue("LANOMLabel");

                string[] tmpList = {"China label", "Taiwan Label", "ICASA Label L", "GOST Lable",
                    "KC Label", "WWAN ID Label", "Wimax Label", "LA NOM Label"};

                IReprintLogRepository printLogRep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();

                IUnitOfWork uof = new UnitOfWork();
                for (var i = 0; i < tmpList.Length; i++)
                {
                    if (i <= 4)
                    {
                        if (tmpList[i] != clabel)
                        {
                            continue;
                        }
                    }
                    else if (i <= 6)
                    {
                        if (tmpList[i] != wlabel)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (tmpList[i] != llabel)
                        {
                            continue;
                        }
                    }

                    var log = new ReprintLog
                    {
                        LabelName = tmpList[i],
                        BegNo = curProduct.CUSTSN,
                        EndNo = curProduct.CUSTSN,
                        Descr = tmpList[i],
                        Reason = reason,
                        Editor = editor
                    };

                    printLogRep.Add(log, uof);
                }
                uof.Commit();

                retList.Add(wlabel);
                retList.Add(clabel);
                retList.Add(cmessage);
                retList.Add(llabel);
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Debug("(PDPALabel01)ReprintLabel end, customerSN:" + customerSN);
            }

        }
        #endregion

    }
}
