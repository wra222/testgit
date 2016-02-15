/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PAQC Output
* UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
* UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* TODO：
* 
*/
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.PrintItem;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IProductReinput接口的实现类
    /// </summary>
    public class ProductReinputImpl : MarshalByRefObject, IProductReinput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodList"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList CheckProdList(IList<string> prodList, string editor, string station, string customer)
        {
            logger.Debug("(ProductReinputImpl)CheckProdList start");

            try
            {
                ArrayList retList = new ArrayList();
                IList<DataModel.ProductInfo> failList = new List<DataModel.ProductInfo>();
                IList<DataModel.ProductInfo> okList = new List<DataModel.ProductInfo>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

                foreach (string node in prodList)
                {
                    
                    IMES.FisObject.FA.Product.IProduct curProduct = null;
                    DataModel.ProductInfo prod = new DataModel.ProductInfo();

                    curProduct = productRep.FindOneProductWithProductIDOrCustSN(node);

                    if (curProduct == null)
                    {   //[Fail List].Cause = ‘不存在’ 
                        prod.customSN = node;
                        prod.id = node;
                        prod.familyId = "不存在";
                        failList.Add(prod);
                    }
                    else
                    {
                        prod.id = curProduct.ProId;
                        prod.customSN = curProduct.CUSTSN;
                        prod.modelId = curProduct.Status.StationId;
                        prod.familyId ="";

                        switch (curProduct.Status.StationId)
                        {
                            case "73":
                                //若ProductStatus.Station = ‘73’，则在[Fail List]添加显示：
                                //[Fail List].Cause = ‘EPIA抽中，需刷出’
                                prod.familyId ="EPIA抽中，需刷出";
                                break;
                            case"71":
                                //若ProductStatus.Station = ‘71，则在[Fail List]添加显示：
                                //[Fail List].ProdId/CustSN = [ProductID]/[CustSN]
                                //[Fail List].Cause = ‘PIA抽中，需刷出’
                                prod.familyId = "PIA抽中，需刷出";
                                break;
                            default:    
                                break;
                        }

                        //获取@StationS = SysSetting.Value( Condtion: SysSetting.Name = ‘FAReturnStation’)，
                        //若ProductStatus.Station不在@StationS中，则在[Fail List]添加显示：
                        //[Fail List].Cause = ‘不在FA’
                        IList<string> valueList = new List<string>();
                        valueList = partRep.GetValueFromSysSettingByName("FAReturnStation");
                        if (!valueList[0].Contains(curProduct.Status.StationId))
                        {
                            prod.familyId = "不在FA";
                        }
                        //ProductStatus.Station<>’45’、’76’和’7P’，
                        //则获取@ForceNWC（ForceNWC.ForceNWC Condtion: ForceNWC.PreStation=ProductStatus.Station and ProductID=[ProductID]）
                        //若@ForceNWC不为空，且不为Null，则在[Fail List]添加显示：
                        //[Fail List].Cause = ‘必须去刷 ’+Station.Descr（Station.Station=@ForceNWC）
                        string tmpsta="45,76,7P";
                        if (tmpsta.IndexOf(curProduct.Status.StationId) < 0)
                        {
                            ForceNWCInfo cond = new ForceNWCInfo();
                            cond.productID = curProduct.ProId;
                            cond.preStation = curProduct.Status.StationId;
                            IList<ForceNWCInfo> flist = partRep.GetForceNWCListByCondition(cond);

                            if (flist.Count > 0)
                            {
                                IStation curstation = stationRep.Find(flist[0].forceNWC);
                                prod.familyId = "必须去刷 " + curstation.Descr;
                            }
                        }

                        // mantis 1558
                        if (curProduct.Status.Status.Equals(StationStatus.Fail))
                            prod.familyId = "存在不良";

                        if (string.IsNullOrEmpty(prod.familyId))
                        {
                            okList.Add(prod);
                        }
                        else
                        {
                            failList.Add(prod);
                        }
                    }                    
                }
                retList.Add(okList);
                retList.Add(failList);

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
                logger.Debug("(ProductReinputImpl)CheckProdList end");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passList"></param>
        /// <param name="failList"></param>
        /// <param name="reStation"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList CheckProdPassList(IList<DataModel.ProductInfo> passList,IList<DataModel.ProductInfo> failList,
                                string reStation, string editor, string station, string customer)
        {
            logger.Debug("(ProductReinputImpl)CheckProdPassList start");

            try
            {
                ArrayList retList = new ArrayList();
                IList<DataModel.ProductInfo> okList = new List<DataModel.ProductInfo>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

                foreach(DataModel.ProductInfo node in passList)
                {
                    //获取@MaxCdt (max(ProductLog.Cdt) Condtion: ProductLog.ProductID = @ProductID and Station=@ReturnStation)
                    //若@MaxCdt不存在或者为Null，则在[Fail List]添加显示：
                    //[Fail List].ProdId/CustSN = [ProductID]/[CustSN]
                    //[Fail List].Cause = ‘不能向后跳站’
                    //并在[Reinput List]删除该Product
                    IList<ProductLog> logList = new List<ProductLog>();
                    ProductLog eqCondition = new ProductLog();
                    string[] stList = { "" };
                    eqCondition.ProductID = node.id;
                    eqCondition.Station = reStation;
                    logList = productRep.GetProductLogList(eqCondition, stList);                  
                    //若@CurrentStation和@ReturnStation均在@FAStation之中，则进行如下Check：
                    //若@ReturnStation在@FAStation的位置比@CurrentStation在@FAStation的位置靠后（CHARINDEX（@ReturnStation,@FAStation,1）> CHARINDEX( @CurrentStation,@FAStation,1)），则在[Fail List]添加显示：
                    //[Fail List].ProdId/CustSN = [ProductID]/[CustSN]
                    //[Fail List].Cause = ‘不能向后跳站’
                    IList<string> valueList = new List<string>();
                    valueList = partRep.GetValueFromSysSettingByName("FAReturnStation");
                    bool backflag = false;
                    if (valueList[0].IndexOf(reStation) > valueList[0].IndexOf(node.modelId))
                    {
                        backflag = true;
                    }

                    DataModel.ProductInfo prod = node;
                    if (logList.Count == 0 || backflag)
                    {
                        prod.familyId = "不能向后跳站";
                        failList.Add(prod);
                    }
                    else
                    {
                        okList.Add(prod);
                    }
                }
                retList.Add(okList);
                retList.Add(failList);

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
                logger.Debug("(ProductReinputImpl)CheckProdPassList end");
            }
        }

        public ArrayList Save(IList<DataModel.ProductInfo> passList,string reStation, bool isPrint, 
                            string editor, string station, string customer,string line, IList<PrintItem> printItems)
        {
            logger.Debug("(ProductReinputImpl)Save start, " 
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                ILabelTypeRepository lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();

                var currentProduct = CommonImpl.GetProductByInput(passList[0].id, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                string sessionKey = currentProduct.ProId;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

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
                    RouteManagementUtils.GetWorkflow(station, "FAProductReinput.xoml", "faproductreinput.rules", out wfName, out rlName);
                    
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    currentSession.AddValue(Session.SessionKeys.Product, currentProduct);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
					currentSession.AddValue("CpuSnSessionKey", currentProduct.CVSN);
					currentSession.AddValue("ReturnStation", reStation);
                    currentSession.SetInstance(instance);

                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.LineCode, "FA");
                   
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
                
           
                string printflag = "";
                if (isPrint)
                {
                    printflag = "Print";
                }

                IUnitOfWork uof = new UnitOfWork();

                foreach(DataModel.ProductInfo prod in passList)
                {
                   productRep.CallOp_FAUnpackProductDefered(uof, prod.id, reStation, editor, printflag);
                }

                uof.Commit();               
                //===============================================================================
                //Get infomation
                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                PrintItem pitem = printList[0];
                for (int i = 1; i < passList.Count; i++)
                {
                    printList.Add(pitem);
                }
                ArrayList retList = new ArrayList();
                retList.Add(printList);
                //===============================================================================
                return retList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                logger.Debug("(ProductReinputImpl)Save end");
            }
        }
        #endregion

    }
}
