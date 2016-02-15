// INVENTEC corporation (c)2012 all rights reserved. 
// Description: MBBorrowControl
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.FisObject.PCA.MB;
using System.Collections;
using IMES.FisObject.FA.Product;
using IMES.DataModel;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class KBEsop : MarshalByRefObject, IKBEsop
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public ArrayList InputKey(string key, string editor, string line, string station, string customer)
        {
            logger.Debug(" InputKey start, key:" + key);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(key, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(key, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", key);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "KBEsop.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + key + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(key);
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
                    erpara.Add(key);
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


                ArrayList result = new ArrayList();
                Product currentProdct = currentSession.GetValue(Session.SessionKeys.Product) as Product;
                ProductModel resultModel = new ProductModel();
                resultModel.CustSN = currentProdct.CUSTSN;
                resultModel.ProductID = currentProdct.ProId;
                resultModel.Model = currentProdct.Model;

                result.Add(resultModel);
                result.Add(currentSession.GetValue(Session.SessionKeys.PicPositionName));
                return result;

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
                logger.Debug(" InputKey end, key:" + key);
            }
        }



        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.MB;
    }
}
