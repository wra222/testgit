/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service implement for ChangeKeyParts Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Change Key Parts.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Change Key Parts.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
                //获取统计信息，删除模拟数据
                //IMiscRepository::IList<QCStatisticInfo> GetQCStatisticList(pdLine);
*/

using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for ChangeKeyParts.
    /// </summary>
    public class _ChangeKeyParts : MarshalByRefObject, IChangeKeyParts
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IChangeKeyParts Members
        /// <summary>
        /// 获取Product的信息和Part信息
        /// </summary>
        public IList<BomItemInfo> GetTableData(string input, string kpType, string line, string editor, string station, string customer, out IMES.DataModel.ProductInfo info, out string retWC)
        {
            logger.Debug("(_ChangeKeyParts)GetTableData start, pdLine:" + line + " input:" + input + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = CommonImpl.GetProductByInput(input, CommonImpl.InputTypeEnum.ProductIDOrCustSN).ProId;

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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ChangeKeyParts.xoml", "ChangeKeyParts.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.KPType, kpType);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

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
                info = p.ToProductInfo();
                retWC = currentSession.GetValue(Session.SessionKeys.ReturnStation) as string;

                IFlatBOM CurrenBom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();
                if (CurrenBom != null)
                {
                    retLst = CurrenBom.ToBOMItemInfoList();
                }
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
                logger.Debug("(_ChangeKeyParts)GetTableData end, pdLine:" + line + " input:" + input + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// 刷入替换物料后的处理
        /// </summary>
        public MatchedPartOrCheckItem TryPartMatchCheck(string sesKey, string checkValue)
        {
            logger.Debug("(_ChangeKeyParts)TryPartMatchCheck start, SessionKey:" + sesKey);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sesKey);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PartID, checkValue);
                    currentSession.AddValue(Session.SessionKeys.ValueToCheck, checkValue);
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

                PartUnit matchedPart = (PartUnit)currentSession.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = checkValue;
                    tempMatchedPart.ValueType = matchedPart.ItemType;
                    return tempMatchedPart;
                }
                else
                {
                    throw new FisException("MAT010", new string[] { checkValue });
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_ChangeKeyParts)TryPartMatchCheck end, SessionKey:" + sesKey);
            }
        }

        /// <summary>
        /// Clear CT already input
        /// </summary>
        public void ClearCT(string sesKey)
        {
            logger.Debug("(_ChangeKeyParts)ClearCT start");
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (session == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }

                IFlatBOM bom = (IFlatBOM)session.GetValue(Session.SessionKeys.SessionBom);
                bom.ClearCheckedPart();
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
                logger.Debug("(_ChangeKeyParts)ClearCT end.");
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string sesKey)
        {
            logger.Debug("(_ChangeKeyParts)save start");
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sesKey, TheType);

                if (session == null)
                {
                    erpara.Add(sesKey);
                    ex = new FisException("CHK194", erpara);
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
                logger.Debug("(_ChangeKeyParts)save end.");
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string sesKey)
        {
            logger.Debug("(_ChangeKeyParts)Cancel start.");

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
                logger.Debug("(_ChangeKeyParts)Cancel end.");
            }
        }
        #endregion
    }
}
