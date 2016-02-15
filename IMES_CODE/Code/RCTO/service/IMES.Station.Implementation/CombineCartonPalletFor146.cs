/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description: Combine Carton Pallet For 146
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
* 
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PAK.CartonSSCC;
using IMES.FisObject.PCA.MB;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using carton = IMES.FisObject.PAK.CartonSSCC;
using log4net;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// </summary>
    public class CombineCartonPalletFor146 : MarshalByRefObject, ICombineCartonPalletFor146
    {
        private const Session.SessionType TheType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.MB;
        
        public ArrayList Save(string cartonSn, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(CombineCartonPalletFor146)Save start, cartonSn:" + cartonSn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);

            try
            {
                ArrayList retList = new ArrayList();
                FisException ex;
                List<string> erpara = new List<string>();

                string sessionKey = cartonSn;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                //用ProductID启动工作流，将Product放入工作流中
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

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "CombineCartonPalletFor146.xoml", "combinecartonpalletfor146.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    
                    currentSession.AddValue(Session.SessionKeys.CartonSN, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue(Session.SessionKeys.ShipMode, "RCTO");
                    
                    currentSession.AddValue("SessionPrintLogName", printItems[0].LabelType);
                    /*currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonSN);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonSN);
                    currentSession.AddValue("SessionPrintLogDescr", "MBCT");*/
                    currentSession.AddValue(Session.SessionKeys.Reason, "");

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
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

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(returnList);

                string palletNo = currentSession.GetValue(Session.SessionKeys.PalletNo) as string;
                retList.Add(palletNo);

                string dn = currentSession.GetValue(Session.SessionKeys.DeliveryNo) as string;
                retList.Add(dn);

                string category = currentSession.GetValue(Session.SessionKeys.RCTO146Category) as string;
                retList.Add(category);

                string fullCartonsInPallet = currentSession.GetValue("FullCartonsInPallet") as string;
                retList.Add(fullCartonsInPallet);

                return retList;
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
                logger.Debug("(CombineCartonPalletFor146)Save end, cartonSn:" + cartonSn + " line:" + line + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
        }

        public ArrayList Reprint(string cartonSn, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(CombineCartonPalletFor146)Reprint Start,"
                            + " [cartonSn]:" + cartonSn
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);

            try
            {
                List<string> erpara = new List<string>();
                FisException ex;

                string sessionKey = cartonSn;
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
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "RePrintPalletForRCTO146.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.CartonSN, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    /*currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, cartonSn);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "MBCT");*/
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);

                    //Session.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }
                    throw currentSession.Exception;
                }

                ArrayList arr = new ArrayList();

                IList<PrintItem> returnList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                arr.Add(returnList);

                string palletNo = currentSession.GetValue(Session.SessionKeys.PalletNo) as string;
                arr.Add(palletNo);

                string category = currentSession.GetValue(Session.SessionKeys.RCTO146Category) as string;
                arr.Add(category);

                return arr;

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
                logger.Debug("(CombineCartonPalletFor146)Reprint End,"
                                + " [cartonSn]:" + cartonSn
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sn"></param>
        public void cancel(string sn)
        {
            logger.Debug("(CombinePoInCarton)Cancel start, [sn]:" + sn);

            string sessionKey = sn;
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
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CombinePoInCarton)Cancel end, [sn]:" + sn);
            }
        }
        
    }
}
