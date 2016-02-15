/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:UI/WebMethod/Sevice Interface/Implementation for ConmbineTPM Page
* UI:CI-MES12-SPEC-FA-UI ConmbineTPM.docx –2011/10/11 
* UC:CI-MES12-SPEC-FA-UC ConmbineTPM.docx –2011/10/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   yang jie               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：

* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Station.Implementation;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// ICombineTPM接口的实现类
    /// </summary>
    public class CombineTPM : MarshalByRefObject, ICombineTPM
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private const string WFfile = "CombineTPM.xoml";
        private const string Rulesfile = "CombineTPM.rules";

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
            logger.Debug("(CombineTPM)InputSN start, custSn:" + custSN
                + "Station:" + curStation);

            try
            {
                ArrayList retLst = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.ProductIDOrCustSN);

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
                    RouteManagementUtils.GetWorkflow(station, WFfile, Rulesfile, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                ProductInfo prodInfo = currentProduct.ToProductInfo();
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
                return retLst;
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
                logger.Debug("(CombineTPM)InputSN end,  custSn:" + custSN);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            //MatchedPartOrCheckItem rtnMatchedPartOrCheckItem = PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
            //rtnMatchedPartOrCheckItem.CollectionData = "";
            //rtnMatchedPartOrCheckItem.PNOrItemName = "";
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
            logger.Debug("(CombineTPM)save start,"
                + " [prodId]: " + prodId);
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
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(CombineTPM)save end,"
                   + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            logger.Debug("(CombineTPM)Cancel start, [prodId]:" + prodId);
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
                logger.Debug("(CombineTPM)Cancel end, [prodId]:" + prodId);
            }
        }

    }
}
