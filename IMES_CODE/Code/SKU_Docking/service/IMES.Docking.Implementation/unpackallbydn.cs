/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineCOAandDN Page            
 * CI-MES12-SPEC-PAK Combine COA and DN.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using IMES.FisObject.PAK.COA;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
using IMES.FisObject.Common.Model;
using IMES.Docking.Interface.DockingIntf;
using IMES.Infrastructure.UnitOfWork;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for UnpackAllbyDN.
    /// </summary>
    public class _UnpackAllbyDN : MarshalByRefObject, IUnpackAllbyDN
    {
        private const Session.SessionType SessionType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
       
        /// <summary>
        /// 获取GetDN表相关信息
        /// </summary>
        /// <param name="DN">DN</param> 
        public void GetDN( string DN)
        {
            logger.Debug("(_UnpackAllbyDN)GetDN start.DN:" + DN);
            try
            {
                Delivery newDelivery = currentRepository.Find(DN);
                if (newDelivery == null)
                {
                    throw new FisException("CHK877", new string[] { });//DN不存在
                }
                return;
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
                logger.Debug("(_UnpackAllbyDN)GetDN end, DN:" + DN);
            }
        }
        /// <summary>
        /// DoUnpackByDN
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param> 
        public void DoUnpackByDN( string editor, string station, string customer, string DN)
        {
            logger.Debug("(_UnpackAllbyDN)DoUnpackByDN start.DN:" + DN);
            string keyStr = "";
            try
            {
                var currentDelivery = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, IMES.FisObject.PAK.DN.Delivery>().Find(DN);
                if (station == null)
                {
                    station = "";
                }
               
                Delivery newDelivery = currentRepository.Find(DN);
                if (newDelivery == null)
                {
                    throw new FisException("CHK877", new string[] {  });//DN不存在
                }
                IList<IProduct> productList = new List<IProduct>();
                productList = productRepository.GetProductListByDeliveryNo(DN);
                if (null == productList)
                {
                    return;
                }
                if (0 == productList.Count)
                {
                    return;
                }
                string line = "";
                string sessionKey = DN;
                keyStr = sessionKey;
                Session currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", SessionType);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "UnpackByDN.xoml", "UnpackByDN.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                currentSession.AddValue(Session.SessionKeys.DeliveryNo, DN);
                currentSession.AddValue(Session.SessionKeys.ProdNoList, productList);
                currentSession.AddValue(Session.SessionKeys.IsComplete, false);
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
                currentSession.AddValue("IsSuper", isSuper);
                currentSession.AddValue("ExcuteDeleteSNonSAP", isExcuteDeleteSAPsn);
                currentSession.AddValue("AllowUnpackCode", allowUnpackCode);
                currentSession.AddValue("PlantCode", plant);
                currentSession.AddValue("DNStatus", currentDelivery.Status);

                currentSession.SetInstance(instance);
                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");

                }
                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return;
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
                Session sessionDelete = SessionManager.GetInstance.GetSession(keyStr, SessionType); ;
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_UnpackAllbyDN)DoUnpackByDN end, DN:" + DN);
            }
        }
    }

}
