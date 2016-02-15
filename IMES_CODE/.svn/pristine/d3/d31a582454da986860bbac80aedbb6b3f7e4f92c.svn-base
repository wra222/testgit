/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Docking.Interface.DockingIntf;
using log4net;

namespace IMES.Docking.Implementation
{

    public class REVLabelPrintForDocking : MarshalByRefObject, IREVLabelPrintForDocking
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IREVLabelPrintForDocking Members

        /// <summary>
        /// 打印REV Label For Docking
        /// </summary>
        /// <param name="family">Family</param>
        /// <param name="dcode">DataCode</param>
        /// <param name="qty">PrintQty</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>DCode、Print Item列表</returns>
        public ArrayList Print(string family, string dcode, int qty, string stationId, string editor, string customerId, IList<PrintItem> printItems)
        {
            logger.Debug("(REVLabelPrintForDocking)Print start, inputFamily:" + family + " DCode:" + dcode + " Qty:" + qty + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = dcode;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, "", customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "REVLabelPrintForDocking.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.FamilyName, family);
                    Session.AddValue(Session.SessionKeys.WarrantyCode, dcode);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);
                    Session.AddValue(Session.SessionKeys.Qty, qty);

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

                /*
                retDCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                IList<PrintItem> returnList = this.getPrintList(Session);
                return returnList;
                */
                ArrayList retLst = new ArrayList();
                var retDCode = (string)Session.GetValue(Session.SessionKeys.DCode);
                var printLst = this.getPrintList(Session);

                retLst.Add(retDCode);
                retLst.Add(qty);
                retLst.Add(printLst);

                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(REVLabelPrintForDocking)Print end, inputFamily:" + family + " DCode:" + dcode + " Qty:" + qty + " stationId:" + stationId + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// 从Sessin里获取打印列表
        /// </summary>
        /// <param name="session">session</param>
        /// <returns></returns>
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

            }
        }

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

        #endregion
    }
}