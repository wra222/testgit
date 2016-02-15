/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for FRU Carton Label Print (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI FRU Carton Label for Docking
 * UC:CI-MES12-SPEC-PAK-UC FRU Carton Label for Docking      
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-25  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Docking.Interface.DockingIntf;
using log4net;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for FRUCartonLabel.
    /// </summary>
    public class _FRUCartonLabel : MarshalByRefObject, IFRUCartonLabel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

        #region IFRUCartonLabel Members

        public bool CheckModelExist(string customer, string model)
        {
            logger.Debug("(_FRUCartonLabel)CheckModelExist start. Model:" + model);

            try
            {
                return modelRepository.CheckExistModel(customer, model);
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
                logger.Debug("(_FRUCartonLabel)CheckModelExist end. Model:" + model);
            }
        }

        public IList<PrintItem> GetPrintTemplate(string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_FRUCartonLabel)GetPrintTemplate start");

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = Guid.NewGuid().ToString();

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, "", "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", "");
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    //Workflow仅调用print这一个Activity，其过程与获取ShipToLabel的打印模板无异，因此调用获取ShipToLabel打印模板的Workflow即可。
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PrintShipToLabelFRUForDocking.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

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

                object printObject = currentSession.GetValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                return (IList<PrintItem>)printObject;
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
                logger.Debug("(_FRUCartonLabel)GetPrintTemplate end");
            }
        }

        #endregion
    }
}
