/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  UnitWeight interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-03-20  Zhao Meili(EB)        Create 
 * 2011-03-16  Lucy Liu              Modify:BN需求改动
 * 2011-04-06  Lucy Liu              Modify:ITC-1268-0019
 * 2011-04-07  Lucy Liu              Modify:ITC-1268-0027 (workflow中activity顺序调整)
 * 2011-04-07  Lucy Liu              Modify:ITC-1268-0022 (Product表中为CUSTSN字段建立索引)
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
namespace IMES.Station.Implementation
{
    public class DismantleFA : MarshalByRefObject, IDismantleFA 
    {
        private static readonly Session.SessionType theType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dismantle
        /// </summary>
        /// <param name="snorproid"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void Dismantle(string snorproid, string line, string pCode, string editor, string station, string customer)
        {

            logger.Debug("(DismantleFA)Dismantle start,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
              
                if (!string.IsNullOrEmpty(currentProduct.CartonSN))
                {
                    //已经包装，不能进行Dismantle!
                    FisException fe = new FisException("CHK139", new string[] { snorproid });
                    throw fe;
                }
                
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<string> productRepairLst = productRepository.GetProductRepairByProIdAndStatus(currentProduct.ProId, 0);
                if (productRepairLst.Count > 0)
                {
                    //去ProductRepair表中去找，找到status等于0的，代表未修完(也就是维修过程中的)，就报错
                    FisException fe = new FisException("CHK140", new string[] { snorproid });
                    throw fe;
                }

                //ITC-1268-0019
                ProductStatusInfo productStatus = productRepository.GetProductStatusInfo(currentProduct.ProId);
                if ((productStatus.station == station) && (productStatus.status == 1))
                {
                    //已经做过Dismantle的Product，再次进入此站,报提示
                    FisException fe = new FisException("CHK150", new string[] { snorproid });
                    throw fe;
                }
                string sessionKey = currentProduct.ProId;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (sessionInfo == null)
                {
                    sessionInfo = new Session(sessionKey, theType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("CurrentFlowSession", sessionInfo);
                    wfArguments.Add("SessionType", theType);


                    sessionInfo.AddValue(Session.SessionKeys.Product, currentProduct);
                    //sessionInfo.AddValue(Session.SessionKeys.PCode, pCode);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "121DismantleFA.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    sessionInfo.SetInstance(instance);
             

                    if (!SessionManager.GetInstance.AddSession(sessionInfo))
                    {
                        sessionInfo.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    sessionInfo.WorkflowInstance.Start();
                    sessionInfo.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }
                    throw sessionInfo.Exception;
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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DismantleFA)Dismantle End,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);

            }
        }  
        
       

    }
}
