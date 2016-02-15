/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 02
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx –2011/11/15 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx –2011/11/15            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-15   Du.Xuan               Create   
* ITC-1360-1733 UPPER(Descr) = 'WWAN LABEL' 修改为UPPER(Descr) LIKE 'WWAN%'
* ITC-1360-1734 取消” Notice (IMES_GetData..PartInfo.InfoValue，Condition：InfoType = 'Notice')属性为'W'”
* ITC-1360-1735 对wwan的判定增加string长度保护
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;



namespace IMES.Station.Implementation
{
    /// <summary>
    /// IPDPALable02接口的实现类
    /// </summary>
    public class PDPALabel02 : MarshalByRefObject, IPDPALabel02
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;

        #region members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="code"></param>
        /// <param name="floor"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputSN(Boolean queryflag, string custSN, string line, string code, string floor, 
                                                    string editor, string station, string customer)
        {
            logger.Debug("(PDPALabel01)InputSN start, custSn:" + custSN);

            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
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
                    
                    //wfArguments.Add("Floor", floor);
                    //wfArguments.Add("Code", code);

                    string wfName, rlName;
                    string xmlname = "";
                    if (!queryflag)
                    {
                        xmlname = "PDPALabel02.xoml";
                    }
                    else
                    {
                        xmlname = "PDPALabel02Query.xoml";
                    }
                    RouteManagementUtils.GetWorkflow(station, xmlname, "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    
                    currentSession.AddValue(Session.SessionKeys.Floor, floor);
                    currentSession.AddValue(Session.SessionKeys.MBCode,code);
                    if (!queryflag)
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, false);

                    }
                    else
                    {
                        currentSession.AddValue(Session.SessionKeys.IsComplete, true);

                    }
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

                //========================================================
                ArrayList retList = new ArrayList();
                Product curProduct = (Product)currentSession.GetValue(Session.SessionKeys.Product);
                ProductModel curModel = new ProductModel();
                
                IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                //如果Product 已经结合Delivery，如果该Delivery 的状态(Delivery.Status)为“98”，
                //则报告错误：“该Product 结合Delivery 资料已经上传SAP，请联系相关人员!”
                if (!queryflag)
                {
                    Delivery assignDelivery = null;
                    if (!string.IsNullOrEmpty(curProduct.DeliveryNo))
                    {
                        assignDelivery = deliveryRep.Find(curProduct.DeliveryNo);
                        if (assignDelivery.Status == "98")
                        {
                            SessionManager.GetInstance.RemoveSession(currentSession);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(sessionKey);
                            ex = new FisException("PAK125", erpara);//该Product 结合Delivery 资料已经上传SAP，请联系相关人员!
                            throw ex;
                        }
                    }
                }

                //如果ModelBOM 中Model 的直接下阶存在BomNodeType = 'PL', UPPER(Descr) LIKE ‘WWAN%’ 
                //则需要进行WWAN Check，
                //如果ModelBOM 中Model 的直接下阶存在PartNo = '6060B0483201'的Part，则需要进行HITACHI Check No Check
                IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
                IList<IBOMNode> bomNodeList = curBom.FirstLevelNodes;
                Boolean wwanCheck = false;
                Boolean hitachiCheck = false;

                foreach (IBOMNode bomNode in bomNodeList)
                {
                    if (bomNode.Part.BOMNodeType == "PL")
                    {
                        if (bomNode.Part.Descr.Length >= 4)
                        {
                            if (bomNode.Part.Descr.ToUpper().Substring(0, 4) == "WWAN")
                            {
                                wwanCheck = true;
                            }
                        }
                    }
                    if (bomNode.Part.PN == "6060B0483201")
                    {
                        hitachiCheck = true;
                    }
                    if (wwanCheck && hitachiCheck)
                    {
                        break;
                    }
                }


                //如果Model 的10，11码为'29' 或者'39'，
                //则需要进行Warranty Label Check，在Message 文本框中显示：“请刷Warranty Card 上的SN号码!”
                Boolean SNCheck = false;
                string tmpstr = string.Empty;
                if (curProduct.Model.Length > 11)
                {
                    tmpstr = curProduct.Model.Substring(9, 2);
                }

                if ((tmpstr == "29") || (tmpstr == "39"))
                {
                    SNCheck = true;
                }

                curModel.ProductID = curProduct.ProId;
                curModel.CustSN = curProduct.CUSTSN;
                curModel.Model = curProduct.Model;
                retList.Add(curModel);   
                retList.Add(wwanCheck);
                retList.Add(hitachiCheck);
                retList.Add(SNCheck);
                //========================================================

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
                logger.Debug("(PAQCOutputImpl)InputSN end, uutSn:" + custSN);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="WWANID"></param>
        /// <returns></returns>
        public ArrayList InputWWANID(string prodId, string WWANID)
        {
            logger.Debug("(PDPALabel02)InputWWANID start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {

                Session curSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (curSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    //========================================================
                    ArrayList retList = new ArrayList();
                    Product curProduct = (Product)curSession.GetValue(Session.SessionKeys.Product);

                    //如果用户刷入的[WWAN ID Label Item Value] 不是下列与Product 结合的数据中的任何一个，
                    //则报告错误：“WWAN ID Label 与机器不匹配!”
                    //WWAN ID Label 上与Product 结合的数据有：
                    //IMEI (IMES_FA..ProductInfo.InfoValue，Condition：InfoType = ' IMEI')
                    //MEID (IMES_FA..ProductInfo.InfoValue，Condition：InfoType = ' MEID')
                    //ESN (IMES_FA..ProductInfo.InfoValue，Condition：InfoType = 'ESN')
                    string imei = (string)curProduct.GetExtendedProperty("IMEI");
                    string meid = (string)curProduct.GetExtendedProperty("MEID");
                    string esn = (string)curProduct.GetExtendedProperty("ESN");
                    if ((WWANID != imei)&& (WWANID != meid)&& (WWANID != esn))
                    {
                        erpara.Add(sessionKey);
                        ex = new FisException("PAK074", erpara);//WWAN ID Label 与机器不匹配!
                        throw ex;
                    }
                 
                    //========================================================
                    return retList;
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
                logger.Debug("(PDPALabel02)InputWWANID end,"
                   + " [prodId]: " + prodId);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="WWANSN"></param>
        /// <returns></returns>
        public ArrayList checkCOA(string prodId, string WWANSN)
        {
            logger.Debug("(PDPALabel02)checkCOA start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {

                Session curSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (curSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {
                    //========================================================

                    Product curProduct = (Product)curSession.GetValue(Session.SessionKeys.Product);
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();


                    IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
                    IList<IBOMNode> bomNodeList = curBom.FirstLevelNodes;
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    Boolean needCheck = false;
                    Boolean errFlag = false;

                    //1.当ModelBOM 中Model 的直接下阶中有BomNodeType = 'P1'，
                    //且Descr LIKE 'COA%' 的Part 存在时，需要按照如下步骤检查COA

                    foreach (IBOMNode bomNode in bomNodeList)
                    {
                        if ((bomNode.Part.BOMNodeType == "P1") && bomNode.Part.Descr.Contains("COA"))
                        {
                            needCheck = true;
                            break;
                        }
                    }

                    //2.取于Product 结合的COA 资料，如果其Part No 与该COA 在COAStatus 表中的IECPN 不同时，
                    //需要报告错误：'COA not matches! '
                    if (needCheck)
                    {
                        foreach (IProductPart partNode in curProduct.ProductParts)
                        {
                            if (partNode.PartType == "P1")
                            {
                                IPart part = partRep.Find(partNode.PartID);
                                if (part.Descr.Contains("COA"))
                                {
                                    COAStatus status = coaRep.Find(partNode.PartID);
                                    if (partNode.PartID != status.IECPN)
                                    {
                                        errFlag = true;
                                    }
                                }
                            }
                        }
                        if (errFlag)
                        {
                            erpara.Add(sessionKey);
                            ex = new FisException("PAK075", erpara);//COA not matches!
                            throw ex;
                        }
                    }
                    //========================================================
                    curSession.Exception = null;

                    //Release The XXX: 2012.04.19 LiuDong
                    //try
                    //{
                        curSession.SwitchToWorkFlow();
                    //}
                    //finally
                    //{
                    //    IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    //    productRep.ReleaseLockByTransThread("Ucc", (Guid)curSession.GetValue<Guid>(Session.SessionKeys.lockToken_Ucc));
                    //    productRep.ReleaseLockByTransThread("Box", (Guid)curSession.GetValue<Guid>(Session.SessionKeys.lockToken_Box));
                    //    productRep.ReleaseLockByTransThread("Loc", (Guid)curSession.GetValue<Guid>(Session.SessionKeys.lockToken_Loc));
                    //    productRep.ReleaseLockByTransThread("Pallet", (Guid)curSession.GetValue<Guid>(Session.SessionKeys.lockToken_Pallet));
                    //    productRep.ReleaseLockByTransThread("Delivery", (Guid)curSession.GetValue<Guid>(Session.SessionKeys.lockToken_DN));
                    //}
                    //Release The XXX: 2012.04.19 LiuDong

                    if (curSession.Exception != null)
                    {
                        if (curSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            curSession.ResumeWorkFlow();
                        }

                        throw curSession.Exception;
                    }
                    //========================================================

                    ArrayList retList = new ArrayList();
                    ProductModel curModel = new ProductModel();
                    IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)curSession.GetValue("WipBuffer");

                    //CmdGeneratePdf(DeliveryNo,Pallet,Box,SN)
                    //'DeliveryNo – Product 结合的Delivery
                    //'Pallet – Product 结合的Pallet
                    //'Box – Product 结合的Box Id 或者UCC
                    //'SN – Product Id
                    //' CmbPL.value – UI 上选择的PdLine对应的IMES_GetData..Line.Line
                    string boxid = (string)curProduct.GetExtendedProperty("BoxId");
                    if (string.IsNullOrEmpty(boxid))
                    {
                        boxid = (string)curProduct.GetExtendedProperty("UCC");
                        if (string.IsNullOrEmpty(boxid))
                        {
                            boxid = "";
                        }
                    }

                    string pdf = (string)curSession.GetValue("CreatePDF");

                    // pdf
                    IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();

                    string templatename = "";
                    //select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位
                    if (!string.IsNullOrEmpty(pdf))
                    {
                        IList<string> docnumList = repPizza.GetDocSetNumListFromPakDashPakComnByLikeInternalID(curProduct.DeliveryNo.Substring(0, 10));
                        //SELECT @templatename = XSL_TEMPLATE_NAME 	FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
                        if (docnumList.Count > 0)
                        {
                            IList<string> tempList = repPizza.GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer("Box Ship Label", docnumList[0]);
                            if (tempList.Count > 0)
                                templatename = tempList[0];
                        }
                    }
                    retList.Add(wipBufferList);
                    retList.Add(curProduct.DeliveryNo);
                    retList.Add(curProduct.PalletNo);
                    retList.Add(boxid);
                    retList.Add(pdf);
                    retList.Add(templatename);

                    //========================================================
                    return retList;

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
                logger.Debug("(PDPALabel02)InputWWANSN end,"
                   + " [prodId]: " + prodId);
            }

        }

        public ArrayList checkCOAQuery(string prodId)
        {
            logger.Debug("(PDPALabel02)checkCOA start,"
                + " [prodId]: " + prodId);

            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodId;

            try
            {

                Session curSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (curSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                else
                {

                    //========================================================
                    curSession.Exception = null;
                    curSession.SwitchToWorkFlow();

                    if (curSession.Exception != null)
                    {
                        if (curSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            curSession.ResumeWorkFlow();
                        }

                        throw curSession.Exception;
                     }
                   
                    //========================================================

                    ArrayList retList = new ArrayList();
                    ProductModel curModel = new ProductModel();
                    Product curProduct = (Product)curSession.GetValue(Session.SessionKeys.Product);
                    IList<WipBufferDef> wipBufferList = (IList<WipBufferDef>)curSession.GetValue("WipBuffer");
                                        
                    retList.Add(wipBufferList);
                    retList.Add(curProduct.DeliveryNo);
                    retList.Add(curProduct.PalletNo);
                    
                    //========================================================
                    return retList;

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
                logger.Debug("(PDPALabel02)InputWWANSN end,"
                   + " [prodId]: " + prodId);
            }

        }
        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="prodId"></param>
        public void save(string prodId, IList<string> defectCodeList)
        {
            logger.Debug("(PDPALabel02)save start,"
                + " [prodId]: " + prodId
                + " [defectList]:" + defectCodeList);
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
                else
                {
                    session.AddValue(Session.SessionKeys.DefectList, defectCodeList);
                    session.AddValue(Session.SessionKeys.HasDefect, (defectCodeList != null && defectCodeList.Count != 0) ? true : false);
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
                logger.Debug("(PDPALabel02)save end,"
                   + " [prodId]: " + prodId
                   + " [defectList]:" + defectCodeList);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="prodId"></param>
        public void cancel(string prodId)
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

        public IList<PrintItem> Print(string productID, string line,string code, string floor, 
                                      string editor, string station, string customer,IList<PrintItem> printItems)
        {
            logger.Debug("(PDPALabel02)Print start, ProductID:" + productID + " pdLine:" + line + " stationId:" + station + " editor:" + editor);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(productID, SessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(productID);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
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
                logger.Debug("(PDPALabel02)Print end, ProductID:" + productID);
            }
        }
        #endregion
        public string GetSysSetting(string name)
        {
            try{
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> valueList= new List<string>();
                valueList =   partRepository.GetValueFromSysSettingByName(name);
                if (valueList.Count == 0)
                {
                    FisException ex;
                    ex = new FisException("CHK021", erpara);
                    //ex.logErr("", "", "", "", "83");
                    //logger.Error(ex);
                    throw ex;
                }
                return valueList[0];
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:" + name);
            }
            
        }

        public ArrayList GetSysSettingList(IList<string> nameList)
        {
            try
            {
                List<string> erpara = new List<string>();
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ArrayList retList = new ArrayList();

                foreach (string node in nameList)
                {
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName(node);
                    if (valueList.Count == 0)
                    {
                        retList.Add("");
                    }
                    else
                    {
                        retList.Add(valueList[0]);
                    }
                }
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
                logger.Debug("(PDPALabel02)GetSysSetting, name:");
            }

        }

        #region "methods do not interact with the running workflow"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        
        #endregion
    }
}
