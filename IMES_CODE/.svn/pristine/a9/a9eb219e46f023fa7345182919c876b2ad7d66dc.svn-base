/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.DataModel;
using System.Collections;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Warranty;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public class PNOPLabelPrintImpl : MarshalByRefObject, IPNOPLabelPrint 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType MBSessionType = Session.SessionType.MB;

        #region PNOPLabelPrintImpl members
        /// <summary>
        /// InputModel
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ArrayList InputModel(string Model)
        {
            logger.Debug("(PNOPLabelPrintImpl)InputModel star, [Model]:" + Model);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();
            try
            {
                IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IHierarchicalBOM sessionBOM = null;
                sessionBOM = ibomRepository.GetHierarchicalBOMByModel(Model);
                IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                bomNodeLst = sessionBOM.FirstLevelNodes;
                if (bomNodeLst == null || bomNodeLst.Count <= 0)
                {
                    erpara.Add(Model);
                    ex = new FisException("PAK039", erpara);
                    throw ex;
                }
                IList<IPart> AT_PartList = new List<IPart>();
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    IPart currentPart = ibomnode.Part;
                    if (currentPart.BOMNodeType == "MB")
                    {
                        string infoValue = iPartRepository.GetPartInfoValue(currentPart.PN,"MB");
                        retvaluelist.Add(infoValue);
                    }
                }
                return retvaluelist;

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PNOPLabelPrintImpl)InputModel End, [Model]:" + Model);
            }
            
        }
        /// <summary>
        /// InitDCode,Type = "MBDateCode",Customer = "HP"
        /// </summary>
        /// <returns></returns>
        public IList<WarrantyDef> InitDCode()
        {
            List<WarrantyDef> ret = new List<WarrantyDef>();
            IWarrantyRepository iWarrantyRepository = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository, Warranty>();
            Warranty condition = new Warranty();
            condition.Type = "MBDateCode";
            condition.Customer = "HP";
            IList<Warranty> warrantys = iWarrantyRepository.GetWarrantyListByCondition(condition);
            
            foreach (Warranty item in warrantys)
            {
                WarrantyDef iteminfo = new WarrantyDef();
                iteminfo.id = Convert.ToString(item.Id);
                iteminfo.Descr = item.Descr;
                ret.Add(iteminfo);
            }
            return ret;

        }



        /// <summary>
        /// InputMBSN
        /// </summary>
        /// <param name="MBSN"></param>
        /// <param name="PdLine"></param>
        /// <param name="DCode"></param>
        /// <param name="Model"></param>
        /// <param name="InfoValue"></param>
        /// <param name="printItems"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputMBSN(string MBSN, string PdLine, string DCode, string Model, string InfoValue, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
            logger.Debug("(PNOPLabelPrintImpl)InputMBSN start, [MBSN]:" + MBSN
                + " [Model]: " + Model
                + " [PdLinr]: " + PdLine
                + " [DCode]: " + DCode
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            //List<string> retvaluelist = new List<string>();
            ArrayList retvaluelist = new ArrayList();

            string sessionKey = MBSN;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, MBSessionType, editor, stationId, PdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", PdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", MBSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "PNOPLabelPrint.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    session.AddValue(Session.SessionKeys.WarrantyCode, DCode);
                    session.AddValue("InfoValue", InfoValue);
                    session.AddValue("Model", Model);

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
                retvaluelist.Add(Model);
                string dcode = session.GetValue(Session.SessionKeys.DCode) as string;
                retvaluelist.Add(dcode);
                retvaluelist.Add(MBSN);
                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;
                retvaluelist.Add(resultPrintItems);
                return retvaluelist; 

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
                logger.Debug("(PNOPLabelPrintImpl)InputMBSN end, [MBSN]:" + MBSN
                    + " [Model]: " + Model
                    + " [PdLinr]: " + PdLine
                    + " [DCode]: " + DCode
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(PNOPLabelPrintImpl)Cancel start, [prodId]:" + prodId);
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, MBSessionType);

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
                logger.Debug("(PNOPLabelPrintImpl)Cancel end, [prodId]:" + prodId);
            }
        }
        #endregion
    }
}
