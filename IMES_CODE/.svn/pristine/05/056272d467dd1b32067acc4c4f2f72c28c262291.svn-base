/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI ROMEO Battery.docx
* UC:CI-MES12-SPEC-PAK-UC ROMEO Battery.docx              
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
 * 
* Known issues:
* TODO：
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure.Extend;
using log4net;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IROMEOBattery接口的实现类
    /// </summary>
    public class ROMEOBattery : MarshalByRefObject, IROMEOBattery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 刷custSn，启动工作流，检查输入的custSn，卡站，获取ProductModel
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns>ProductModel</returns>
        public ArrayList InputSN(string custSN, string line, string editor, string station, string customer)
        {
            logger.Debug("(PizzaKitting)InputSN start, custSn:" + custSN
                + "Station:" + station);

            try
            {
                ArrayList retLst = new ArrayList();

                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);

                string sessionKey = currentProduct.ProId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                //用ProductID启动工作流，将Product放入工作流中
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", SessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "ROMEOBattery.xoml", "ROMEOBattery.rules", out wfName, out rlName);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
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

                //============================================================================
                
                //get product data for UI
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                DataModel.ProductInfo prodInfo = curProduct.ToProductInfo();
                retLst.Add(prodInfo);

                //get bom
                IList<BomItemInfo> bomItemList = PartCollection.GeBOM(sessionKey, SessionType);
                //IList<BomItemInfo> bomItemList = new List<BomItemInfo>() ;

                retLst.Add(bomItemList);
                //============================================================================
                return retLst;

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
                logger.Debug("(PizzaKitting)InputSN end,  custSn:" + custSN);
            }

        }


        public MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue)
        {
            //MatchedPartOrCheckItem item = new MatchedPartOrCheckItem();
            //item.PNOrItemName = "01C2PNPK110001";
            //return item;
            return PartCollection.TryPartMatchCheck(sessionKey, SessionType, checkValue);
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void Save(string prodId)
        {
            logger.Debug("(PDPALabel02)save start,"
                + " [prodId]: " + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;
            
            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.IsComplete, true);
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
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                logger.Debug("(PDPALabel02)save end,"
                   + " [prodId]: " + prodId);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void Cancel(string prodId)
        {
            logger.Debug("(PDPALabel02)Cancel start, [prodId]:" + prodId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

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
                logger.Debug("(PDPALabel02)Cancel end, [prodId]:" + prodId);
            }
        }
        
        #endregion

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByCode(string code)
        {
            logger.Debug("(PizzaKitting)getBomByCode Start[code]:" + code);
            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<LightBomInfo> retLst = productRepository.GetWipBufferInfoListByKittingCode(code);
                return retLst;
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
                logger.Debug("(PizzaKitting)getBomByCode End, "
                   + " [code]:" + code);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList<LightBomInfo> getBomByModel(string model, out string code)
        {
            logger.Debug("(PizzaKitting)getBomByModel Start [model]:" + model);

            try
            {
                IList<LightBomInfo> retLst = new List<LightBomInfo>();

                //判断系统中是否存在该Model
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model modelObject = modelRepository.Find(model);
                if (modelObject == null)
                {
                    throw new FisException("CHK038", new string[] { model });
                }
                else
                {
                    //判断该Model是否维护Kitting Code
                    string ret = modelObject.GetAttribute("DM2");

                    logger.Debug("(PizzaKitting)Maintain Kitting Code[ret]:" + ret);
                    if (ret == null || ret.Trim() == string.Empty)
                    {
                        throw new FisException("CHK113", new string[] { model });
                    }
                    else
                    {
                        //根据model获取Kitting Code
                        code = modelRepository.GetKittingCodeByModel(model);
                        if (code == null || code.Trim() == string.Empty)
                        {
                            throw new FisException("CHK113", new string[] { model });
                        }
                        else
                        {
                            //判断查询得到的Kitting Code 是否在Kitting 表中存在
                            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                            int count = productRepository.GetCountOfKittingCodeByCode(code);
                            if (count == 0)
                            {
                                throw new FisException("CHK114", new string[] { model });
                            }
                            else
                            {
                                //获取bom
                                retLst = productRepository.GetWipBufferInfoListByKittingCodeAndModel(code, model);
                            }
                        }
                    }
                }

                return retLst;
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
                logger.Debug("(PizzaKitting)getBomByModel End[model]:" + model);

            }
        }

        #endregion
    }
}
