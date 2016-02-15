/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-02-02   207006            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _094AdjustMO : MarshalByRefObject, IAdjustMO, IModel
    {
        //private static readonly string station;
        private static readonly Session.SessionType theType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IAdjustMO Members

        public void Adjust(string oldMo, string newMo, int adjustQty, IList<string> productList, string pCode, string editor, string pdLine, string stationId, string customerId)
        {
            logger.Debug("(AdjustMO)Adjust start, oldMo:" + oldMo + "newMo:" + newMo + "adjustQty:" + adjustQty + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "094AdjustMO.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Qty, adjustQty);
                    Session.AddValue(Session.SessionKeys.OldMONO , oldMo);
                    Session.AddValue(Session.SessionKeys.NewMONO , newMo);
                    Session.AddValue(Session.SessionKeys.ProdNoList, productList);
                    Session.AddValue(Session.SessionKeys.PCode , pCode );

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
                logger.Debug("(AdjustMO)Adjust end, oldMo:" + oldMo + "newMo:" + newMo + "adjustQty:" + adjustQty + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        public IList<IMES.DataModel.ProductStatus> GetProductFromProductStatus(string mo)
        {
            IProductRepository proRepository = RepositoryFactory.GetInstance().GetRepository <IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            return (IList<IMES.DataModel.ProductStatus>)proRepository.GetProductFromProductStatus(mo); 
           
        }


        public IList<MOInfo> GetOldMOList(string model)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            return (IList<IMES.DataModel.MOInfo>)moRepository.GetOldMOListFor094(model);
      
        }

        public IList<MOInfo> GetnewMOList(string model)
        {
            IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            return (IList<IMES.DataModel.MOInfo>)moRepository.GetNewMOListFor094(model);
      
        }

        #endregion

        #region IModel Members

       
        public IList<IMES.DataModel.ModelInfo> GetModelList(string familyId)
        {
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            return (List<IMES.DataModel.ModelInfo>)modelRepository.GetModelListFor094(familyId);
        }
        #endregion  
    }
}
