/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Pizz
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-20   Yang.Weihua               Create   
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.Common;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;




namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPizzaKitting接口的实现类
    /// </summary>
    public class CombinePizza : MarshalByRefObject, ICombinePizza
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private const string WFfile = "CombinePizza.xoml";
        private const string Rulesfile = "CombinePizza.rules";
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="curStation"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSN, string line, string curStation,
                                                    string editor, string station, string customer)
        {
            logger.Debug("(PizzaKitting)InputSN start, custSn:" + custSN
                + "Station:" + curStation);

            string HoldStationMsg = "";
            try
            {
                ArrayList retLst = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                //用ProductID启动工作流，将Product放入工作流中
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
                    RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);//PDPLabel01.rules
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue("pizzaline", line);
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

                //get product data for UI
                IMES.DataModel.ProductInfo prodInfo = currentProduct.ToProductInfo();
                retLst.Add(prodInfo);

                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                retLst.Add(bomItemList);

                //如果机器是海运方式出货需要弹出Message Box 提示用户：“此机器为海运方式出货,请检查是否有装乾燥剂”
                bool isOceanShipping = false;
                if (currentSession.GetValue(Session.SessionKeys.IsOceanShipping) != null)
                {
                    isOceanShipping = true;
                }
                retLst.Add(isOceanShipping);
                string weightErr =(string)currentSession.GetValue("ErrorMsgWithNoModelWeight");
                retLst.Add(weightErr);
                return retLst;
            }
            catch (FisException e)
            {

                BlockStationHoldStation(e.mErrcode, custSN, line, curStation, editor, station, customer, ref HoldStationMsg);

                logger.Error(e.mErrmsg + HoldStationMsg, e);
                throw new Exception(e.mErrmsg + HoldStationMsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message + HoldStationMsg, e);
                throw new SystemException(e.Message + HoldStationMsg);
            }
            finally
            {
                logger.Debug("(PizzaKitting)InputSN end,  custSn:" + custSN);
            }

        }

        private void BlockStationHoldStation(string e, string custSN, string line, string curStation, string editor, string station, string customer, ref string HoldStationMsg)
        {
            string sessionKey = "";
            try
            {
                logger.Debug("(CombinePizza)BlockStationHoldStation start," + " [custSN]: " + custSN);
                string ErrorCode = e;
                IList<ConstValueInfo> lstConstValue = partRepository.GetConstValueListByType("BlockWCErrorCode");
                foreach (ConstValueInfo items in lstConstValue)
                {
                    if (items.name == ErrorCode)
                    {
                        station = items.value;

                        var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                        sessionKey = currentProduct.ProId;
                        Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                        //用ProductID启动工作流，将Product放入工作流中
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
                            RouteManagementUtils.GetWorkflow(station, "CombinePizzaHoldStation.xoml", "", out wfName, out rlName);
                            WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

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
                        HoldStationMsg = "此机器已经被hold，请解掉重流...";
                    }
                }
            }
            catch (FisException ex)
            {
                logger.Error(ex.mErrmsg, ex);
                throw new Exception(ex.mErrmsg);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw new SystemException(ex.Message);
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                
                logger.Debug("(CombinePizza)BlockStationHoldStation end," + " [custSN]: " + custSN);
            }
        }

        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void Save(string prodId)
        {
            logger.Debug("(PizzaKitting)save start,"
                + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(prodId, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);

///////////////////////////////////////////////////////////////////////////
                //增加抽检

                /* Mared by Bensona at 2013/0315 for Mantis0001699 
                                string tmpline = (string)session.GetValue("pizzaline").ToString();
                                DateTime qcStartTime = new DateTime(1900, 1, 1);
                                IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                                QCRatio currentQCRatio = CurrentRepository.GetQCRatio(tmpline.Substring(0, 1));
                                if (currentQCRatio == null)
                                {
                                    List<string> errpara = new List<string>();
                                    throw new FisException("CHK040", errpara);//change Error msg
                                }
                                //---------------------------------------------------------------------

                                int iQCRadio = currentQCRatio.PAQCRatio;
                                //获取Product数量@Count：
                                int iQcCount = productRepository.GetSampleCount("PAQC", tmpline);
                                if ((iQcCount == 0))
                                {
                                    session.AddValue(Session.SessionKeys.QCStatus, true);
                                }
                                else if (iQCRadio != 0)
                                {
                                    if (iQcCount % iQCRadio == 0)
                                    {
                                        session.AddValue(Session.SessionKeys.QCStatus, true);

                                    }
                                    else
                                    {
                                        session.AddValue(Session.SessionKeys.QCStatus, false);

                                    }
                                }
                                else
                                {
                                    session.AddValue(Session.SessionKeys.QCStatus, false);
                                }

                                IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                                string[] tps = new string[1];
                                tps[0] = "PAQC";

                                IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
                                QCStatusList = repProduct.GetQCStatusOrderByCdtDesc(prodId, tps);
                                //string qcRemark = "";
                                foreach (ProductQCStatus tmp in QCStatusList)
                                {
                                    if (tmp.Remark == "P")
                                    {
                                        session.AddValue(Session.SessionKeys.QCStatus, true);
                                        break;
                                    }
                                }


                                //qcRemark = (QCStatusList != null && QCStatusList.Count != 0) ? QCStatusList[0].Remark : string.Empty;
                                //if (qcRemark == "P")
                                //{
                                //    session.AddValue(Session.SessionKeys.QCStatus, true);

                                //}
                                if (currentQCRatio.PAQCRatio == 0)
                                {
                                    session.AddValue(Session.SessionKeys.QCStatus, false);
                                }
                
                ///////////////////////////////////////////////////////////////////////////
                 Mared by Bensona at 2013/0315 for Mantis0001699 */
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
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }

                logger.Debug("(combine pizza)save end,"
                   + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            logger.Debug("(PizzaKitting)Cancel start, [prodId]:" + prodId);
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
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PizzaKitting)Cancel end, [prodId]:" + prodId);
            }
        }

        /// <summary>
        /// CheckQCStatus
        /// </summary>
        /// <param name="prodId"></param>
        public string CheckQCStatus(string prodId)
        {
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                string[] tps = new string[1];
                tps[0] = "PAQC";

                IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
                QCStatusList = repProduct.GetQCStatusOrderByCdtDesc(prodId, tps);
                string qcStatus = "";

                qcStatus = (QCStatusList != null && QCStatusList.Count != 0) ? QCStatusList[0].Status : string.Empty;
                //qcStatus == 8
                return qcStatus;
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
                logger.Debug("CheckQCStatus end! prodId:" + prodId);
            }
 
        }

        /// <summary>
        /// CheckWarrantyCard
        /// </summary>
        /// <param name="prodId"></param>
        public string CheckWarrantyCard(string prodId)
        {
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                //CheckWarrantyCard
                IList<BomItemInfo> sessionBOM = PartCollection.GeBOM(sessionKey, SessionType);

                //IList<BomItemInfo> sessionBOM = session.GetValue(Session.SessionKeys.SessionBom) as List<BomItemInfo>;
                string strReturn = "";
                bool bFound = false;
                foreach (BomItemInfo ele in sessionBOM)
                {
                    if (ele.type == "WarrantyCard")
                    {
                        bFound = true;
                        break;
                    }
                }

                if (bFound)
                {
                    session.AddValue("isWarrantyCard", true);
                    strReturn = "isWarrantyCard";

                }
                return strReturn;
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
                logger.Debug("CheckQCStatus end! prodId:" + prodId);
            }

        }

    }
}
