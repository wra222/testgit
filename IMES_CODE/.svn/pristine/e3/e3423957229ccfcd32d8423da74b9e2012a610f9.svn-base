/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:
* UI:CI-MES12-SPEC-FA-UI RCTO TPDL Check.docx 
* UC:CI-MES12-SPEC-FA-UC RCTO TPDL Check.docx            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-08-06   Du.Xuan               Create  
* Known issues:
* TODO：
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Collections;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Route;
using IMES.Infrastructure.Extend;


namespace IMES.Station.Implementation
{
    public class TPDLCheckForRCTOImpl : MarshalByRefObject, ITPDLCheckForRCTO
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        ArrayList ITPDLCheckForRCTO.InputProductID(string ID, string line, string editor, string station, string customer)
        {
            logger.Debug("(TPDLCheckForRCTOImpl)InputCustomerSN start, customerSN:" + ID
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {
                ArrayList retList = new ArrayList();
                string productID = "";
                string lcmCT = "";
                var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();

                if (ID.Length == 14)
                {
                    string ctno = ID;
                    lcmCT = ID;
                    //若刷入的为[LCM CT]，则获取Product_Part.ProductID 
                    //( Condtion: Product_Part.PartSn=[LCM CT] and CheckItemType=’LCM’)
                    //若ProductID不存在，则报错：“错误的LCM CT”；若存在，则显示LCM CT
                    ProductPart conf = new ProductPart();
                    conf.PartSn = ctno;
                    conf.CheckItemType = "LCM";
                    IList<ProductPart> productList = productRep.GetProductPartList(conf);

                    foreach (ProductPart item in productList)
                    {
                        IMES.FisObject.FA.Product.IProduct pro = productRep.Find(item.ProductID);
                        if (!string.IsNullOrEmpty(pro.CartonSN))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(pro.ProId);
                            ex = new FisException("CHK940", erpara);//“错误的LCM CT”
                            throw ex;
                        }
                    }
                    productID = productList[0].ProductID;
                }
                else
                {
                    //若刷入的为[ProductID]，取Product_Part.PartSn 
                    //(Condition: Product_Part.ProductID=[ProductID] and CheckItemType = ‘LCM’)，
                    //若PartSn不存在，则报错：“未结合LCM CT”；否则，显示LCM CT
                    ProductPart conf = new ProductPart();
                    conf.ProductID = ID;
                    conf.CheckItemType = "LCM";
                    IList<ProductPart> productList = productRep.GetProductPartList(conf);

                    if (productList.Count == 0)
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(ID);
                        ex = new FisException("CHK941", erpara);//“未结合LCM CT”
                        throw ex;
                    }
                    productID = ID;
                    lcmCT = productList[0].PartSn;
                }

                //获取[ProductID]已绑定TPDL的PartSn（Top 1 Product_Part.PartSn Condition: Product_Part.ProductID=[ProductID] and CheckItemType=’TPDL’）
                //若PartSn为空或者Null，则报错：“LCM未结合TPDL”
                //若PartSn不为空，则显示在UI，显示格式：*****
                string tpdl = "";
                ProductPart tconf = new ProductPart();
                tconf.ProductID = productID;
                tconf.CheckItemType = "TPDL";
                IList<ProductPart> proList = productRep.GetProductPartList(tconf);

                if (proList.Count == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(ID);
                    ex = new FisException("CHK945", erpara);//“LCM未结合TPDL”
                    throw ex;
                }
                tpdl = proList[0].PartSn;

                string sessionKey = productID; 
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

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "TPDLCheckForRCTO.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, sessionKey);
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
                //===============================================================================
                //Get infomation
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);

                currentSession.AddValue("TPDL", tpdl);

                retList.Add(curProduct.ProId);
                retList.Add(lcmCT);
                retList.Add(tpdl);
                
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }

        }


        void ITPDLCheckForRCTO.Cancel(string productID)
        {
            string sessionKey = productID;
            try
            {
                logger.Debug("(PrintContentWarranty)Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
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
                logger.Debug("(PrintContentWarranty)Cancel end, sessionKey:" + sessionKey);
            }
        }
        ArrayList ITPDLCheckForRCTO.Save(string prodID,string TpdlCT)
        {
            logger.Debug("(TPDLCheckForRCTOImpl)Save Start,[ProductID] " + prodID
                +"[TpdlCT]:" + TpdlCT);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(prodID, ProductSessionType);
                ArrayList retList = new ArrayList();
                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(prodID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {

                    //若PartSn的前5码与[TPDL CT]的前5码不同，则报错：“LCM结合的TPDL与 刷入的[TPDL CT] 匹配错误”
                    string tpdl = (string)currentSession.GetValue("TPDL");
                    if (tpdl.Substring(0, 5) != TpdlCT.Substring(0, 5))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(TpdlCT);
                        ex = new FisException("CHK946", erpara);
                        logger.Error(ex.Message, ex);
                        SessionManager.GetInstance.RemoveSession(currentSession);
                        throw ex;
                    }

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

                return retList;

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
                logger.Debug("(TPDLCheckForRCTOImpl)Save end,[ProductID] " + prodID
                + "[TpdlCT]:" + TpdlCT);
            }
        }
    }


}
