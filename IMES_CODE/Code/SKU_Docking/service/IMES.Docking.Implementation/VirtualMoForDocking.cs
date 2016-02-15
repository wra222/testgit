using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Route;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MBMO;



namespace IMES.Docking.Implementation
{
    /// <summary>
    /// Virtual MO
    /// 本站实现的功能：
    ///     1.	Add Qty of Virtual MO
    ///     2.	Add New Virtual MO
    ///     3.  Query MO
    ///     4.  Auto Download MO (调产线接口)
    /// </summary> 
    public class VirtualMoForDocking : MarshalByRefObject, IVirtualMoForDocking, IModel
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Session.SessionType theType = Session.SessionType.Common;

        #region IVirtualMo Members

        /// <summary>
        /// Get MO by Model（含Virtual Mo和真实Mo）
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>MOInfoList查询结果</returns>
        public IList<MOInfo> GetVirtualMOByModel(string model, string editor, string stationId, string customerId)
        {
            // SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM IMES_GetData..MO
	        // WHERE Model = @Model
		    // AND Cdt BETWEEN CONVERT(varchar, GETDATE(),111) AND CONVERT(varchar, DATEADD(day, 1, GETDATE()), 111)
	        // ORDER BY MO
            try
            {
                var currentMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                IList<MO> currentMOList = (IList<MO>)currentMORepository.GetVirtualMOAndRealMObyModel(model);
                IList<MOInfo> returnMOList = new List<MOInfo>();

                if (currentMOList != null)
                {
                    foreach (MO moItem in currentMOList)
                    {
                        MOInfo tempMO = new MOInfo();
                        tempMO.createDate = moItem.CreateDate;
                        tempMO.startDate = moItem.StartDate;
                        tempMO.qty = moItem.Qty;
                        tempMO.pqty = moItem.PrtQty;
                        tempMO.model = moItem.Model;
                        tempMO.id = moItem.MONO;
                        returnMOList.Add(tempMO);
                    }
                }

                return returnMOList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        // <summary>
        /// Get MO （For Query）
        /// </summary>
        /// <param name="mo">mo</param>
        /// <param name="model">Model</param>
        /// <param name="family">family</param>
        /// <param name="editor">editor</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>MOInfoList查询结果</returns>
        public IList<MOInfo> GetVirtualForQuery(string mo, string model, string family, string editor, string stationId, string customerId)
        {
            //UC Revision:6789: 
            //SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM  MO
            //    WHERE MO = @MO 
            //或者

            //SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM  MO
            //    WHERE Model = @Model and  Qty> Print_Qty order by MO
            //或者

            //SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM  MO a，Model b
            //    WHERE a.Model=b.Model and b.Family= @Family and  Qty> Print_Qty order by MO

            //查询顺序具有优先级： MO　-> Model -> Family

            //Revision: 8815: 查询窗口的查询结果增加CustomerSN_Qty的显示


            try
            {
                var currentMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();

                IList<MOInfo> returnMOList = new List<MOInfo>();

                if (!string.IsNullOrEmpty(mo))
                {
                    MO moItem = new IMES.FisObject.Common.MO.MO();
                    moItem = currentMORepository.Find(mo);
                    if (moItem != null)
                    {
                        MOInfo tempMO = new MOInfo();
                        tempMO.createDate = moItem.CreateDate;
                        tempMO.startDate = moItem.StartDate;
                        tempMO.qty = moItem.Qty;
                        tempMO.pqty = moItem.PrtQty;
                        tempMO.model = moItem.Model;
                        tempMO.id = moItem.MONO;
                        tempMO.customerSN_Qty = moItem.CustomerSN_Qty;
                        returnMOList.Add(tempMO);                        
                    }
                    
                }
                else if (!string.IsNullOrEmpty(model))
                {
                    IList<MOInfo> MOInfoList = new List<MOInfo>();
                    MOInfoList = currentMORepository.GetMOList(model);
                    if (MOInfoList != null)
                    {
                        foreach (MOInfo moItem in MOInfoList)
                        {
                            if (moItem.qty > moItem.pqty)
                            {
                                returnMOList.Add(moItem);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(family))
                {
                    returnMOList = currentMORepository.GetMOListByFamily(family);
                }

                return returnMOList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }



        public IList<MOInfo> GetVirtualForQuery2(string mo, string model, string family, DateTime startTime, DateTime endTime)
        {
            //SELECT MO, a.Model, CreateDate, StartDate, Qty, Print_Qty ,CustomerSN_Qty FROM  MO a, Model b
        	//WHERE a.Model=b.Model and b.Family like '%@Family%' 
        	//and a.Model like '%@Model%'
        	//and a.MO like '%@MO%'
        	//and CONVERT(Varchar,a.CreateDate,111)>='@StartDate'
        	//and CONVERT(Varchar,a.CreateDate,111)<='@EndDate'
        	//and  Qty> Print_Qty 
    	    //and a.Status = 'H'
	        //order by MO

            try
            {
                IMBMORepository mbMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
                IList<MO> moLst = new List<MO>();
                moLst = mbMORepository.GetMoModelInfoListFromMoModel(family, model, mo, startTime, endTime, "H");

                IList<MOInfo> returnMOList = new List<MOInfo>();

                foreach (MO temp in moLst)
                {
                    MOInfo tempMO = new MOInfo();
                    tempMO.createDate = temp.CreateDate;
                    tempMO.startDate = temp.StartDate;
                    tempMO.qty = temp.Qty;
                    tempMO.pqty = temp.PrtQty;
                    tempMO.model = temp.Model;
                    tempMO.id = temp.MONO;
                    tempMO.customerSN_Qty = temp.CustomerSN_Qty;

                    returnMOList.Add(tempMO);
                }

                return returnMOList;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }



        public IList<MOInfo> GetMOList(string model)
        {
            var currentMORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();

            IList<MOInfo> returnMOList = new List<MOInfo>();

            IList<MOInfo> MOInfoList = new List<MOInfo>();
            MOInfoList = currentMORepository.GetMOList(model);
            if (MOInfoList != null)
            {
                foreach (MOInfo moItem in MOInfoList)
                {
                    if (moItem.qty > moItem.pqty)
                    {
                        returnMOList.Add(moItem);
                    }
                }
            }

            return returnMOList;
        }

        /// <summary>
        /// 为指定的Model 创建Virtual Mo
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="qty">Qty</param>
        /// <param name="pCode">pCode</param>
        /// <param name="startDate">startDate</param>
        /// <param name="editor">editor</param>
        /// <param name="pdLine">pdLine</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void CreateNewVirtualMo(string model, int qty, string pCode, string startDate, string editor, string pdLine, string stationId, string customerId)
        {
            logger.Debug("(VirtualMoForDocking)CreateNewVirtualMo start, model:" + model + "qty:" + qty + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {

                checkModel(model);
                string sessionKey = model;
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
                    RouteManagementUtils.GetWorkflow(stationId, "VirtualMoForDocking.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.Qty, qty);
                    Session.AddValue(Session.SessionKeys.ModelName, model);
                    Session.AddValue(Session.SessionKeys.PCode , pCode );
                    Session.AddValue(Session.SessionKeys.StartDate, startDate);

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
                logger.Debug("(VirtualMoForDocking)CreateNewVirtualMo end, model:" + model + "qty:" + qty + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);
            }
        }

        public void DeleteMo(string mo, string editor, string station, string customer)
        {
            logger.Debug("(VirtualMoForDocking)DeleteMo start, mo:" + mo + " editor:" + editor + " station:" + station + " customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = Guid.NewGuid().ToString();
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);

                if (Session == null)
                {
                    Session = new Session(sessionKey, theType, editor, station, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", theType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "VirtualMoDelForDocking.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.MONO, mo);

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
                logger.Debug("(VirtualMoForDocking)DeleteMo end, mo:" + mo + " editor:" + editor + " station:" + station + " customer:" + customer);
            }
            
        }

        #endregion

        #region IModel Members

        /// <summary>
        ///获得ModelList
        /// </summary>
        /// <param name="familyId">familyId</param>
        public IList<IMES.DataModel.ModelInfo> GetModelList(string familyId)
        {
            try
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                return currentModelRepository.GetModelListByFamilyAndStatus(familyId, 1);
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        /// <summary>
        /// 检查用户输入的Model在Model表中是否存在
        /// </summary>
        /// <param name="model">Model</param>
        public void checkModel(string model)
        {
            try 
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                var currentModel = currentModelRepository.Find(model);
                if (currentModel == null || currentModel.Status != "1")
                {
                    throw new FisException("CHK038", new List<string>() { model });
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
        }

        /// <summary>
        /// 检查用户输入的Model在是否属于该Family
        /// </summary>
        /// <param name="model">Model</param>
       public void checkModelinFamily(string family, string model)
        {
            try
            {
                var currentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                var currentModel = currentModelRepository.Find(model);
                if (currentModel == null || currentModel.Status != "1")
                {
                    throw new FisException("CHK038", new List<string>() { model });
                }

                IList<Model> modelLst = new List<Model>();
                modelLst = currentModelRepository.GetModelListByModel(family, model);       //支持like功能
                if (modelLst == null || modelLst.Count <= 0)
                {
                    throw new FisException("PAK096", new List<string>() { model,family });
                }
                else
                {
                    Boolean modelFlag =false;
                    foreach (Model imodel in modelLst)
                    {
                        if (imodel.ModelName == model)
                        {
                            modelFlag = true;
                            break;
                        }
                    }

                    if (!modelFlag)
                    {
                        throw new FisException("PAK096", new List<string>() { model, family });
                    }
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
        }




        // <summary>
        /// DownloadMO
        /// </summary>
        /// <returns>_Schema.SqlHelper.ConnectionString_GetData</returns>
        public string GetDataConnectionString()
        {
            IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            return currentDNRepository.GetDataConnectionString();
        }

        // <summary>
        /// Auto Download MO(调用客户提供的SP)
        /// SP: op_ExecuteMoUploadJOB(string jobname)
        /// </summary>
        /// <returns></returns>
        public void AutoDownloadMO_SP(string jobname)
        {
            try
            {
                IMORepository imoRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                imoRepository.ExecuteMoUploadJOB(jobname);
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        /// <summary>
        /// 取得Product的Family信息列表
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>Family信息列表</returns>

        public IList<FamilyInfo> FindFamiliesByCustomer(string customer)
        {
            //UC Revision:2363: SELECT [Family] ,[Descr] ,[CustomerID] FROM [IMES2012_GetData].[dbo].[Family] Order by [Family]

            //UC Revision:6789: SELECT [Family] FROM [IMES2012_GetData].[dbo].[Family] where CustomerID= (select Value from SysSetting where Name = ‘Customer’) Order by [Family]

            try
            {
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> customerLst = new List<string>();
                customerLst = partRepository.GetValueFromSysSettingByName("Customer");
                if (customerLst != null && customerLst.Count > 0)
                {
                    customer = customerLst[0];
                }
                else
                {
                   // throw new FisException("PAK087", new List<string>() { "Customer" });
                    return null;
                }

                IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                IList<FamilyInfo> retLst = null;
                retLst = familyRepository.FindFamiliesByCustomerOrderByFamily(customer);
                return retLst;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
        }


        /// <summary>
        /// 取得Server当前时间
        /// </summary>
        /// <returns>DateTime</returns>
        public DateTime GetCurDate()
        {
            return DateTime.Now;
        }



        #endregion
    }
}
