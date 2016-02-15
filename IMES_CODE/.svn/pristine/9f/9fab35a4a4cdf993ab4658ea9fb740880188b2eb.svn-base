/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Workflow.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using IMES.DataModel;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository.Common;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for LabelLightGuide.
    /// </summary>
    public class _LabelLightGuide : MarshalByRefObject, ILabelLightGuide
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        
        #region ILabelLightGuide Members
        /// <summary>
        /// 根据ProductID获取product
        /// </summary>
        public IMES.DataModel.ProductInfo GetProductInfo(string prodId, string line, string editor, string station, string customer, bool bQuery)
        {
            logger.Debug("(_LabelLightGuide)GetProductInfo start, productId:" + prodId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("LabelLightGuide.xoml", "LabelLightGuide.rules", wfArguments);

                    currentSession.SetInstance(instance);

                    bool bNeedSave = true;
                    if (bQuery) bNeedSave = false;
                    //4/24 UC中删除此段处理逻辑
                    /*
                    else
                    {
                        //若ProductLog中存在Station=64的Log，且最近的一次过64站之后，没有（Station=6A and Status=0）的Log记录，则不进行任何操作。
                        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                        IList<ProductLog> logList64 = productRepository.GetProductLogsOrderByCdtDesc(prodId, "64");
                        IList<ProductLog> logList6A = productRepository.GetProductLogsOrderByCdtDesc(prodId, "6A", 0);

                        if (logList64.Count > 0)
                        {
                            if (logList6A.Count <= 0) bNeedSave = false;
                            if (logList6A.Count > 0)
                            {
                                if (logList64[0].Cdt > logList6A[0].Cdt)
                                {
                                    bNeedSave = false;
                                }
                            }
                        }
                    }
                    */

                    currentSession.AddValue("bOnlyQuery", bQuery);
                    currentSession.AddValue("bNeedSave", bNeedSave);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                IProduct p = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                return p.ToProductInfo();
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
                logger.Debug("(_LabelLightGuide)GetProductInfo end, productId:" + prodId);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string sesKey,out string CheckEPIA)
        {   
            logger.Debug("(_LabelLightGuide)save start");
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                CheckEPIA = null;
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);
                
                if (session == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }

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
                CheckEPIA = (string)session.GetValue(ExtendSession.SessionKeys.FAQCStatus);
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
                logger.Debug("(_LabelLightGuide)save end.");
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string sesKey)
        {
            logger.Debug("(_LabelLightGuide)Cancel start.");

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);

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
                logger.Debug("(_LabelLightGuide)Cancel end.");
            }
        }

        /// <summary>
        /// 根据model和Light Code获取LightGuide列表
        /// </summary>
        public IList<WipBuffer> getBomData(string model, string code)
        {
            logger.Debug("(_LabelLightGuide)getBomData start, model:" + model + " KittingCode:" + code);
            List<string> erpara = new List<string>();

            try
            {
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                //IList<WipBuffer> wbList = productRepository.GetWipBufferListFromWipBuffer(code, "FA Label");
                string labelLightSetting = "LabelLightGuidType";
                string labelLight = "PAK Label";
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> values = partRep.GetValueFromSysSettingByName(labelLightSetting);
                if (values != null && values.Count > 0)
                {
                    labelLight = values[0];
                }
                else
                {
                    erpara.Add(labelLightSetting);
                    FisException ex = new FisException("CQCHK0020", erpara);
                    throw ex;
                }
                IList<WipBuffer> wbList = productRepository.GetWipBufferListFromWipBuffer(code, labelLight);

                IHierarchicalBOM bom = bomRepository.GetHierarchicalBOMByModel(model);
                IList<IBOMNode> nodes = bom.FirstLevelNodes;
            
                string syscode = "";
                IList<string> codes = bomRepository.GetOsCodeFromBomCode(model);
                if (codes.Count > 0) syscode = codes[0];
                char bc_code = 'a';
                if (model.Length >= 7) bc_code = model[6];

                IList<string> pnList = new List<string>();
                if (model.EndsWith("A") || model.EndsWith("B") || syscode == "00010" || syscode == "00011" || (bc_code >= '0' && bc_code <= '9'))
                {
                    foreach (IBOMNode tmp in nodes)
                    {
                        pnList.Add(tmp.Part.PN);
                    }
                }
                else
                {
                    foreach (IBOMNode tmp in nodes)
                    {
                        if (tmp.Part.PN != "6060B0232501" && tmp.Part.PN != "6060B0153901")
                        {
                            pnList.Add(tmp.Part.PN);
                        }
                    }
                }

                IList<WipBuffer> ret = new List<WipBuffer>();
                
                foreach (WipBuffer ele in wbList)
                {
                    if (pnList.Contains(ele.PartNo))
                    {
                        ret.Add(ele);
                    }
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
                logger.Debug("(_LabelLightGuide)GetTableData end, model:" + model + " KittingCode:" + code);
            }
        }

        /// <summary>
        /// 根据hostname获取Comm设置列表
        /// </summary>
        public IList<COMSettingInfo> getCommSetting(string hostname, string editor)
        {
            logger.Debug("(_LabelLightGuide)getCommSetting start, hostname:" + hostname);
            try
            {
                IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<COMSettingInfo> commSettingList = new List<COMSettingInfo>();
                commSettingList = iPalletRepository.FindCOMSettingByName(hostname);

                if (commSettingList == null || commSettingList.Count <= 0)
                {
                    COMSettingInfo ele = new COMSettingInfo();
                    ele.name = hostname;
                    ele.editor = editor;
                    ele.commPort = "1";
                    ele.baudRate = "9600,n,8,1";
                    ele.rthreshold = 1;
                    ele.sthreshold = 1;
                    ele.handshaking = 0;
                    iPalletRepository.AddCOMSettingItem(ele);
                    commSettingList.Add(ele);
                }
                return commSettingList;
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
                logger.Debug("(_LabelLightGuide)getCommSetting end, hostname:" + hostname);
            }
        }


        public IList<string> GetLightNumFroAOI(string tool_id, string user_id, string SN, string LabelCode, string WC, string Line, string Timestamp)
        {
          IProduct p=  productRepository.GetProductByIdOrSn(SN);
          if (p == null)
          {
              List<string> errpara = new List<string>();
              errpara.Add(SN);
              throw new FisException("SFC002", errpara);
          }
          IList<WipBuffer> wbList = getBomData(p.Model, LabelCode);
          IList<string> lnList = new List<string>();
          foreach (WipBuffer ele in wbList)
          {
              lnList.Add(ele.LightNo);
          }
           return lnList;
        }
        #endregion
    }
}
