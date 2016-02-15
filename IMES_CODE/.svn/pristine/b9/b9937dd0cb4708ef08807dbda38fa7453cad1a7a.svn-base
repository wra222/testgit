/*
 * INVENTEC corporation ?012 all rights reserved. 
 * Description:IMES service implement for UnitWeight (for Docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight for Docking
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight for Docking            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-29  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Implementation;
using log4net;
using IProduct = IMES.FisObject.FA.Product.IProduct;
using ModelInfo = IMES.FisObject.Common.Model.ModelInfo;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using System.Data;

namespace IMES.Docking.Implementation
{
    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. Print Config Label; Print POD Label
    ///                3. 上传数据至SAP
    /// </summary>
    public class PakUnitWeight : MarshalByRefObject, IPakUnitWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region PakUnitWeight

        /// <summary>
        /// 此站输入的是SN，需要在BLL中先根据SN获取Product调用CommonImpl.GetProductByInput()
        /// 如果获取不到，报CHK079！
        /// 用ProductID启动工作流
        /// 将获得的Product放到Session.Product中
        /// 获取Model和标准重量和ProductID
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="actualWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="configParams"></param>
        /// <returns>ArrayList对象</returns>
        public ArrayList InputUUT(string custSN, decimal actualWeight, string line, string editor, string station, string customer)
        {
            logger.Debug("(UnitWeight)InputUUT Start,"
                + " [custSN]:" + custSN
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                 + " [actualWeight]:" + actualWeight.ToString()
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                ArrayList retLst = new ArrayList();
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                if (currentProduct == null)
                {
                    FisException fe = new FisException("CHK079", new string[] { custSN });
                    throw fe;
                }
                string productID = currentProduct.ProId;
                string sessionKey = productID;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, Session.SessionType.Product, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PakUnitWeightForDocking.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, actualWeight);
                    currentSession.AddValue(Session.SessionKeys.Tolerance, "3%");
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

                decimal standardWeight = -1;
                if (currentSession.GetValue(Session.SessionKeys.StandardWeight) == null)
                {
                    standardWeight = -1;
                }
                else
                {
                    standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                }

                retLst.Add(productID);              //  [0] IMES_FA..ProductID
                retLst.Add(currentProduct.Model);   //  [1] IMES_FA..Product.Model
                retLst.Add(standardWeight);         //  [2] (decimal) IMES_GetData..ModelWeight.UnitWeight 
                IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                ProductQCStatus qcStatus = iDeliveryRepository.GetQCStatus(productID, "PAQC");
                bool bPAQC = false;
                if (qcStatus != null && qcStatus.Status == "8")
                {
                    bPAQC = true;
                }
                retLst.Add(bPAQC);

                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                if (e.mErrcode == "CHK020") //序號已被刷入
                {
                    throw e;
                }
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(UnitWeight)InputUUT End,"
                    + " [custSN]:" + custSN
                    + " [line]:" + line
                    + " [editor]:" + editor
                    + " [station]:" + station
                    + " [customer]:" + customer);
            }

        }

        /// <summary>
        /// 保存机器重量，更新机器状态，结束工作流。
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public void Save(string productID)
        {
            logger.Debug("(UnitWeight)Save Start,"
                + " [productID]:" + productID);
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, Session.SessionType.Product);

                if (currentSession == null)
                {
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    Product currentProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(UnitWeight)Save End,"
                    + " [productID]:" + productID);
            }
        }



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string productID)
        {
            logger.Debug("(UnitWeight)Cancel Start,"
               + " [productID]:" + productID);

            try
            {
                string sessionKey = productID;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                logger.Debug("(UnitWeight)Cancel End,"
                    + " [productID]:" + productID);
            }

        }
        #endregion


        #region ModelWeight
        /// <summary>
        /// 获取ModelWeight
        /// </summary>
        /// <param name="inputData">inputData</param>
        /// <returns></returns>
        public ModelWeightDef GetModelWeightByModelorCustSN(string inputData)
        {
            //检查合法model
            //看取得的数据是否有数据
            String result = "";
            try
            {
                string model = string.Empty;
                IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();


                //3)	如果刷入为10位，如果是CN打头的初步认定为Customer SN，如在Product．CustSN中不存在，则提示“非法的Customer SN”
                if (inputData.Length == 10 && (inputData.Substring(0, 2) == "CN" || inputData.Substring(0, 2) == "5C"))
                {
                    var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                    if (currentProduct == null)
                    {
                        FisException fe = new FisException("PAK042", new string[] { inputData });  //此Customer S/N %1 不存在！
                        throw fe;
                    }
                    else if (string.IsNullOrEmpty(currentProduct.Model))
                    {
                        FisException fe = new FisException("PAK028", new string[] { inputData });  //该Customer SN %1 还未与Model绑定！
                        throw fe;
                    }
                    else model = currentProduct.Model;
                }
                else
                {
                    Model modelItem = itemRepository.Find(inputData);
                    if (modelItem == null)
                    {
                        var currentProduct = CommonImpl.GetProductByInput(inputData, CommonImpl.InputTypeEnum.CustSN);
                        if (currentProduct == null || string.IsNullOrEmpty(currentProduct.Model))
                        {
                            FisException fe = new FisException("CHK079", new string[] { inputData });   //找不到与此序号 %1 匹配的Product! 
                            throw fe;
                        }
                        else model = currentProduct.Model;

                    }
                    else model = inputData;

                }

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();

                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //This Model, there is no standard weight, please go to the weighing.
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                result = modelWeight.Rows[0][1].ToString();

                if (result == "")
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                ModelWeightDef item = new ModelWeightDef();
                item.Model = model;
                item.UnitWeight = result;
                return item;
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
        /// 保存修改的ModelWeight
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        public void SaveModelWeightItem(ModelWeightDef item)
        {

            try
            {

                IModelWeightRepository itemRepositoryModelWeight = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository>();
                //看取得的数据是否有, 防止update的是空记录，但[PAK_SkuMasterWeight_FIS]中加入了记录
                DataTable modelWeight = itemRepositoryModelWeight.GetModelWeightItem(item.Model);
                if (modelWeight == null || modelWeight.Rows.Count == 0)
                {
                    //该Model尚无标准重量，请先去称重。
                    List<string> erpara = new List<string>();
                    erpara.Add(item.Model);
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;
                }

                IPizzaRepository itemRepositoryPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository>();

                ModelWeightInfo setValue = new ModelWeightInfo();
                setValue.unitWeight = Decimal.Parse(item.UnitWeight);
                setValue.sendStatus = "";
                setValue.remark = "";
                setValue.editor = item.Editor;
                setValue.udt = DateTime.Now;

                ModelWeightInfo condition = new ModelWeightInfo();
                condition.model = item.Model;

                PakSkuMasterWeightFisInfo pakSkuMasterWeight = new PakSkuMasterWeightFisInfo();
                pakSkuMasterWeight.model = item.Model;
                pakSkuMasterWeight.weight = setValue.unitWeight;
                pakSkuMasterWeight.cdt = setValue.udt;

                UnitOfWork uow = new UnitOfWork();
                itemRepositoryModelWeight.UpdateModelWeightDefered(uow, setValue, condition);
                itemRepositoryPizza.DeletetPakSkuMasterWeightFisByModelDefered(uow, item.Model);
                itemRepositoryPizza.InsertPakSkuMasterWeightFisDefered(uow, pakSkuMasterWeight);
                uow.Commit();

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

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }
        #endregion

    }
}
