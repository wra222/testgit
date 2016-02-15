/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: HPPN Label for RCTO
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-08-14   itc000052            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// HPPNLabelforRCTO接口的实现类
    /// </summary>
    public class HPPNLabelforRCTO : MarshalByRefObject, IHPPNLabelforRCTO 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IHPPNLabelforRCTO members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        public ArrayList InputProdId(string prodId, string model, string HPPN, IList<PrintItem> printItems, string editor, string stationId, string customer)
        {
            logger.Debug("(MasterLabelPrintImpl)InputProdId start, [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            //List<string> retvaluelist = new List<string>();
            ArrayList retvaluelist = new ArrayList();

            string sessionKey = prodId;

            try
            {
////////////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////////////////////////////


                Session session = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (session == null)
                {
                    session = new Session(sessionKey, ProductSessionType, editor, stationId, "", customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "HPPNLabelforRCTO.xoml", "HPPNLabelforRCTO.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    session.AddValue(Session.SessionKeys.PrintLogName, "HP PN Label");
                    session.AddValue(Session.SessionKeys.PrintLogBegNo, prodId);
                    session.AddValue(Session.SessionKeys.PrintLogEndNo, prodId);

                    string desc = HPPN + " " + model;
                    session.AddValue(Session.SessionKeys.PrintLogDescr, desc); 
 
                    session.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(session))
                    {
                        session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    session.WorkflowInstance.Start();
                    session.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();

                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //session.SwitchToWorkFlow();

                //check workflow exception
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }

                    throw session.Exception;
                }

                retvaluelist.Add(prodId);
                retvaluelist.Add(model);
                retvaluelist.Add(HPPN);

                IList<PrintItem> resultPrintItems = session.GetValue(Session.SessionKeys.PrintItems) as IList<PrintItem>;

                retvaluelist.Add(resultPrintItems);

                return retvaluelist; 

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
                logger.Debug("(HPPNLabelforRCTO)InputProdId end, [pdLine]:" + ""
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        public ArrayList GetProductInfo(string prodId, string editor, string stationId, string customer)
        {
            logger.Debug("(IHPPNLabelforRCTO)InputProdId start, [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);

            //List<string> retvaluelist = new List<string>();
            ArrayList retvaluelist = new ArrayList();

            string sessionKey = prodId;

            try
            {
                ////////////////////////////////////////////////////////////////////////////////////
                ArrayList retList = new ArrayList();
                string productID = "";
                string lcmCT = "";
                string model = "";
                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IModelRepository CurrentModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

                if (prodId.Length == 14)
                {
                    string strLCM = prodId;
                    lcmCT = prodId;
                    //若为[LCM CT]，Product_Part表不存在@ProductID（Product_Part.PartSn=[LCM CT]）则报错：“LCM CT 不存在”
                    ProductPart conf = new ProductPart();
                    conf.PartSn = strLCM;
                    IList<ProductPart> productList = productRep.GetProductPartList(conf);

                    if (productList.Count == 0)
                    {
                        //IMES.FisObject.FA.Product.IProduct pro = productRep.Find(item.ProductID);
                        //if (!string.IsNullOrEmpty(pro.ProId))
                        //{
                        /*FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(pro.ProId);
                        ex = new FisException("CHK953", erpara);//“LCM CT不存在”
                        throw ex;*/
                        throw new FisException("CHK953", new string[] { });
                        // }
                    }

                    productID = productList[0].ProductID;
                    var currentProduct = productRep.GetProductByIdOrSn(productID);

                    model = currentProduct.Model;

                }
                else if (prodId.Length == 9 || prodId.Length == 10)
                {
                    //若ProductID在Product/ProductStatus不存在，则报错：“ProductID不存在”
                    IMES.FisObject.FA.Product.IProduct pro = productRep.Find(prodId);
                    if (pro == null)
                    {
                        /*FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(pro.ProId);
                        ex = new FisException("CHK951", erpara);//“ProductID不存在”
                        throw ex;*/
                        throw new FisException("CHK951", new string[] { });
                    }

                    productID = pro.ProId;
                    var currentProduct = productRep.GetProductByIdOrSn(productID);

                    model = currentProduct.Model;
                }
                IList<IMES.FisObject.Common.Model.ModelInfo> infos = new List<IMES.FisObject.Common.Model.ModelInfo>();
                infos = CurrentModelRepository.GetModelInfoByModelAndName(model, "FOX");
                string HPPN = "";
                if (infos == null || infos.Count <= 0)
                {
                    throw new FisException("CHK952", new string[] { }); //"HP PN不存在"
                }
                else
                {
                    HPPN = infos[0].Value;
                }

                ////////////////////////////////////////////////////////////////////////////////////

                retvaluelist.Add(productID);
                retvaluelist.Add(model);
                retvaluelist.Add(HPPN);


                return retvaluelist;

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
                logger.Debug("(HPPNLabelforRCTO)InputProdId end, [pdLine]:" + ""
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel(string prodId)
        {
            logger.Debug("(HPPNLabelforRCTO)Cancel start, [prodId]:" + prodId);
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
                logger.Debug("(HPPNLabelforRCTO)Cancel end, [prodId]:" + prodId);
            }
        }


 
        #endregion
    }
}
