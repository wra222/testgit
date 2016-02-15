/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: Unpack Carton/DN/Pallet Impl
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-03-10   Lucy Liu            Create
 * 2011-04-12   Lucy Liu            Modify:ITC-1268-0058
 * 2011-09-05  Vincent Lee       add multi-unpackage function
 * 2011-10-20   itc202017           Add UnpackDNByDN implementation
 * Known issues:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.FisObject.PAK.DN;
using IMES.Route;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pallet;
using IMES.Docking.Interface.DockingIntf;


namespace IMES.Docking.Implementation
{
    /// <summary>
    /// CombinePOInCarton
    /// </summary>
    public class UnpackForDocking : MarshalByRefObject, IUnpackForDocking
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IUnpackForDocking Members



        ///<summary>
        /// 根据指定的DeliveryNo解除绑定
        /// 使用工作流070UnpackDNByDN.xoml
        /// </summary>
        public void UnpackDNByDN(string deliveryNo, bool bSuperUI, string line, string editor, string station, string customer)
        {
            if (bSuperUI)
            {
                logger.Debug("(UnpackDNByDN[Super])Unpack start, deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
            else
            {
                logger.Debug("(UnpackDNByDN)Unpack start, deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
            }

            FisException ex;
            List<string> erpara = new List<string>();


            try
            {
                var currentDelivery = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, IMES.FisObject.PAK.DN.Delivery>().Find(deliveryNo);
                
                if (null == currentDelivery)
                {
                    throw new FisException("CHK190", new string[] { deliveryNo });//DN不存在
                }

                if (!bSuperUI && "98" == currentDelivery.Status)
                {
                    throw new FisException("CHK191", new string[] { deliveryNo });//DN已上传
                }

                string sessionKey = currentDelivery.DeliveryNo;


                station = "80";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);


                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDNByDN.xoml", "", wfArguments);

                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, currentDelivery.DeliveryNo);
                    currentSession.AddValue(Session.SessionKeys.Delivery, currentDelivery);
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
                if (bSuperUI)
                {
                    logger.Debug("(UnpackDNByDN[Super])Unpack end,  deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
                }
                else
                {
                    logger.Debug("(UnpackDNByDN)Unpack end,  deliveryNo:" + deliveryNo + "line:" + line + "editor:" + editor + "station:" + station + "customer:" + customer);
                }
            }
        }

        

        ///<summary>
        /// 根据指定的sn or dn解除绑定
        /// 使用工作流070UnpackDNByall.xoml
        /// </summary>
        public void UnpackBySNCheck(string prodSn, string pdline, string editor, string station, string customer)
        {
            logger.Debug("(UnpackAllBySN)Unpack start, prodid:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);

            string currentSessionKey = prodSn;
            try
            {
                //station = "SP";
                Session currentCommonSession = SessionManager.GetInstance.GetSession(currentSessionKey, ProductSessionType);
                if (currentCommonSession == null)
                {
                    currentCommonSession = new Session(currentSessionKey, ProductSessionType, editor, station, pdline, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", currentSessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentCommonSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdline);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    WorkflowInstance instance;
                   // if (station == "SP")
                    {
                        instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackBySN.xoml", "unpackbysn.rules", wfArguments);
                    }
                   // else
                  //  {
                  //       instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UnpackDNBySN.xoml", "UnpackDNBySN.rules", wfArguments);
                  //  }
                    //currentCommonSession.AddValue(Session.SessionKeys.CN, "ALL");
                    //currentCommonSession.AddValue(Session.SessionKeys.Pallet, false);
                    currentCommonSession.AddValue(Session.SessionKeys.IsComplete, false);

                    string isSuper = "Y";
                    CommonImpl cmi = new CommonImpl();
                    IList<ConstValueInfo> lstConst = cmi.GetConstValueListByType("SAP", "Name");
                    string isExcuteDeleteSAPsn = "";
                    string allowUnpackCode = "";
                    foreach (ConstValueInfo constV in lstConst)
                    {
                        if (constV.name == "ExcuteDeleteSNonSAP")
                        {
                            isExcuteDeleteSAPsn = constV.value;
                        }
                        if (constV.name == "AllowUnpackCode")
                        {
                            allowUnpackCode = constV.value;
                        }
                    }
                    string plant = System.Configuration.ConfigurationManager.AppSettings["PlantCode"];
                    currentCommonSession.AddValue("IsSuper", isSuper);
                    currentCommonSession.AddValue("ExcuteDeleteSNonSAP", isExcuteDeleteSAPsn);
                    currentCommonSession.AddValue("AllowUnpackCode", allowUnpackCode);
                    currentCommonSession.AddValue("PlantCode", plant);
                  

                    currentCommonSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentCommonSession))
                    {
                        currentCommonSession.WorkflowInstance.Terminate("Session:" + currentSessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(currentSessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentCommonSession.WorkflowInstance.Start();
                    currentCommonSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentSessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentCommonSession.Exception != null)
                {
                    if (currentCommonSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentCommonSession.ResumeWorkFlow();
                    }

                    throw currentCommonSession.Exception;
                }

                return;
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
                logger.Debug("(UnpackAllBySN)Unpack end,  prodid:" + prodSn + "line:" + pdline + "editor:" + editor + "station:" + station + "customer:" + customer);
            }

        }

        public string UnpackbySNSave(string prodId)
        {
            logger.Debug("(UnpackAllBySN)UnpackAllbySNSave start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
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
                    return "ok";
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
                logger.Debug("(UnpackAllBySN)UnpackAllbySNSave end,"
                   + " [prodId]: " + prodId);
            }
        }


        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(Unpack)Cancel start, [prodId]:" + prodId);
            //FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
                logger.Debug("(Unpack)Cancel end, [prodId]:" + prodId);
            }
        }




        #endregion

       

    }
}
