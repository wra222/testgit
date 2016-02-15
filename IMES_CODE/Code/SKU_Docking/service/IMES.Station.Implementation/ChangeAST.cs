using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;



namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class ChangeAST : MarshalByRefObject, IChangeAST
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region IChangeAST Members
        /// <summary>
        /// </summary>                
        public ArrayList CheckProduct(string prodid, string line, string editor, string station, string customer)
        {
            logger.Debug("CheckProduct Start:" + line + "," + prodid);

            ArrayList retValue = new ArrayList();
            string currentSessionKey = prodid;

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Product);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ChangeAST_Input.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);
                    

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

                IList<ASTInfo> infos = (IList<ASTInfo>)currentSession.GetValue(Session.SessionKeys.ProductPartList);
                Product currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                retValue.Add(infos);
                retValue.Add(currentProduct.Model);
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
                logger.Debug("CheckProduct End:" + line + "," + prodid);
            }
        }

        public ArrayList Change(string line, string editor, string station, string customer, 
                                    string prod1, string prod2, string model1, string model2,
                                    string ASTType, string PartNo, string PartSn, IList<PrintItem> printItemLst)
        {
            logger.Debug("ChangeAST-Change Start:" + line + "," + prod1 + "," + prod2 + ","
                        + model1 + "," + model2 + "," + ASTType + "," + PartNo + "," + PartSn);

            ArrayList retValue = new ArrayList();
            string currentSessionKey = Guid.NewGuid().ToString();

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(currentSessionKey, Session.SessionType.Common);
                if (currentSession == null)
                {
                    currentSession = new Session(currentSessionKey, Session.SessionType.Common, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Common);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("ChangeAST_Change.xoml", "ChangeAST_Change.rules", wfArguments);

                    ASTInfo info = new ASTInfo();
                    info.ASTType = ASTType;
                    info.PartNo = PartNo;
                    info.PartSn = PartSn;
                    
                    currentSession.AddValue(Session.SessionKeys.Model1, model1);
                    currentSession.AddValue(Session.SessionKeys.Model2, model2);
                    currentSession.AddValue(Session.SessionKeys.ASTInfoList, info);
                    currentSession.AddValue(Session.SessionKeys.Prod2, prod2);
                    currentSession.AddValue(Session.SessionKeys.Prod1, prod1);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, "ChangeAST");
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, prod1);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, prod2);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, PartSn);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItemLst);

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

                int isPrint = (int)currentSession.GetValue(Session.SessionKeys.InfoValue);
                IList<PrintItem> pList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retValue.Add(isPrint.ToString());
                retValue.Add(pList);
                retValue.Add(prod2);

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
                logger.Debug("ChangeAST-Change End:" + line + "," + prod1 + "," + prod2 + ","
                        + model1 + "," + model2 + "," + ASTType + "," + PartNo + "," + PartSn);

            }
        }


        public void returnException5()
        {
            FisException ex;
            List<string> erpara = new List<string>();
            ex = new FisException("CHK893", erpara);
            throw ex;
        }

        public void returnException2(string id)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            erpara.Add(id);
            ex = new FisException("CHK894", erpara);
            throw ex;
         
        }

        #endregion
    }
        
}
