using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Docking.Interface.DockingIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Part;
using System.Collections;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.Generates.impl;


namespace IMES.Docking.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ShipToLabelPrintForDocking : MarshalByRefObject, IShipToLabelPrintForDocking
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IShipToLabelPrintForDocking Members

        public ArrayList Print(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems)
        {
            logger.Debug("ShipToLabelPrint For Docking start, id:" + id + " ,pdLine:" + pdline + " ,station:" + station);
            ArrayList retValue = new ArrayList();

            string true_id = String.Empty;
            if (id.Length == 11)
            {
                true_id = id.Substring(1, 10);
            }
            else
            {
                true_id = id;
            }
            string currentSessionKey = true_id;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ShipToLabelPrint.xoml", "ShipToLabelPrint.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue("isRePrint", 0);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                string cartonNo = string.Empty;
                cartonNo = currentProduct.CartonSN;

                int action = (int)currentSession.GetValue(Session.SessionKeys.InfoValue);
                IList<PrintItem> printParams = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                string pdfname = String.Empty;
                pdfname = (string)currentSession.GetValue("PDFFileName");

                retValue.Add(cartonNo);
                retValue.Add(id);
                retValue.Add(printParams);
                retValue.Add(action.ToString());
                retValue.Add(pdfname);
                
                return retValue;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("ShipToLabelPrint For Docking end, id:" + id + " ,pdLine:" + pdline + " ,station:" + station);
            }
        }

        public IList<string> GetValueByName(string name)
        {
            IList<string> retList = new List<string>();

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            retList = partRep.GetValueFromSysSettingByName(name);

            return retList;
        }


        public ArrayList RePrint(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems)
        {
            logger.Debug("ShipToLabelRePrint For Docking start, id:" + id + " ,pdLine:" + pdline + " ,station:" + station);
            ArrayList retValue = new ArrayList();

            string true_id = String.Empty;
            if (id.Length == 11)
            {
                true_id = id.Substring(1, 10);
            }
            else
            {
                true_id = id;
            }
            string currentSessionKey = true_id;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ShipToLabelRePrint.xoml", "ShipToLabelPrint.rules", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    //Mantis 1037
                    currentSession.AddValue("isRePrint", 1);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "Shipto");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, true_id);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, true_id);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, true_id);
                    currentSession.AddValue(Session.SessionKeys.Reason, "Reprint");

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
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
                    erpara.Add(currentSessionKey);
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

                var currentProduct = (IProduct)currentSession.GetValue(Session.SessionKeys.Product);
                string cartonNo = string.Empty;
                cartonNo = currentProduct.CartonSN;

                int action = (int)currentSession.GetValue(Session.SessionKeys.InfoValue);
                IList<PrintItem> printParams = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                string pdfname = String.Empty;
                pdfname = (string)currentSession.GetValue("PDFFileName");

                //mantis 1037
                string strDN = (string)currentSession.GetValue(Session.SessionKeys.DeliveryNo);
                string template = (string)currentSession.GetValue("TemplateName");

                retValue.Add(cartonNo);
                retValue.Add(id);
                retValue.Add(printParams);
                retValue.Add(action.ToString());
                retValue.Add(pdfname);

                //mantis 1037
                retValue.Add(strDN);
                retValue.Add(template);

                return retValue;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("ShipToLabelRePrint For Docking end, id:" + id + " ,pdLine:" + pdline + " ,station:" + station);
            }
        }

        #endregion


    }
}
