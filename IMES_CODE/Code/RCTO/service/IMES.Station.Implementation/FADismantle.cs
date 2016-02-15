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
 * 2011-12-10  Kaisheng           
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
using IMES.FisObject.Common.Part;

namespace IMES.Station.Implementation
{
    public class DismantleFA : MarshalByRefObject, IDismantleFA 
    {
        private static readonly Session.SessionType theType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snorproid"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<string> InputProdId(string snorproid, string line, string pCode, string editor, string station, string customer)
        {
            logger.Debug("(DismantleFA)InputProdId start,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            IList<string> retLst = new List<string>();
            List<string> erpara = new List<string>();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                IProductRepository ip = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                ProductStatusInfo statusInfo = ip.GetProductStatusInfo(currentProduct.ProId);
                
                retLst.Add(statusInfo.pdLine);
                retLst.Add(currentProduct.Model);
                retLst.Add(currentProduct.ProId);
                retLst.Add(currentProduct.CUSTSN);
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
                logger.Debug("(DismantleFA)InputProdId End,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);

            }
        }
        /// <summary>
        /// CheckPrdIDorSNList
        /// </summary>
        /// <param name="snorproidlist"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public string CheckPrdIDorSNList(IList<string> snorproidlist, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer)
        {
            logger.Debug("(DismantleFA)CheckPrdIDorSNList start,"
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            int rtnValue = 0;
            string snorproid = "";
            string prddidstation = "";
            IList<ProductLog> prodLogLst = new List<ProductLog>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            try
            {
                for (int i = 0; i < snorproidlist.Count; i++)
                {
                    snorproid = snorproidlist[i];
                    rtnValue = rtnValue + 1;
                    var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                    ProductStatusInfo productStatus = productRepository.GetProductStatusInfo(currentProduct.ProId);
                    /*
                    if ((productStatus.station == station) && (productStatus.status == 1))
                    {
                        //已经做过Dismantle的Product，再次进入此站,报提示
                        FisException fe = new FisException("CHK150", new string[] { snorproid });
                        throw fe;
                    }
                     */
                    prddidstation = productStatus.station.Trim();
                    //Modify 2012/03/09：	Station in ('71','73''74') 提示“Please scan PIA/EPIA OutPut First!”-〉保留73
                    //if ((prddidstation == "71") || (prddidstation == "73") || (prddidstation == "74"))
                    if (prddidstation == "73")
                    {
                        //提示“Please scan PIA/EPIA OutPut First!”  
                        FisException fe = new FisException("CHK280", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                        throw fe;
                    }
                    //int numStation = Convert.ToInt32(productStatus.station);
                    //prodLogLst = productRepository.GetProductLogs(currentProduct.ProId, "69");
                    //if (prodLogLst.Count > 0)
                    //{
                    //    //提示“已经到包装，不能dismantle!”
                    //    FisException fe = new FisException("CHK281", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                    //    throw fe;
                    //}
                    //若ProductStatus.Station 不在 (ConstValue.Value( Condtion: ConstValue.Name = ‘FADismantleStation’  and Type=’FAStation’))之中，则提示：“不在Dismantle的Station中，请Check”
                    //Note：
                    //ConstValue.Value格式如下：Station1, Station2, Station3…..
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    ConstValueInfo info = new ConstValueInfo();
                    info.type = "FAStation";
                    info.name = "FADismantleStation";
                    IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
                    if (retList != null && retList.Count > 0)
                    {
                        var iloc = retList[0].value.IndexOf(prddidstation);
                        if (iloc == -1)
                        {
                            //不在Dismantle的Station中，请Check  
                            FisException fe = new FisException("CHK306", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                            throw fe;
                        }
                        else if (prddidstation.Trim() == "")
                        {
                            //不在Dismantle的Station中，请Check  
                            FisException fe = new FisException("CHK306", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                            throw fe;
                        }
                    }
                    else
                    {
                        //请联系IE，维护Dismantle的Station
                        FisException fe = new FisException("CHK307", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                        throw fe;
                    }
                    //Modify 2012/03/09：UC Station=75-〉6A Status=0 “此台为PIA/EPIA检测不良，请与ＱＣ联系”
                    //if ((prddidstation == "75") && (productStatus.status == 0))
                    if ((prddidstation == "6A") && (productStatus.status == 0))
                    {
                        //提示此台为PIA/EPIA检测不良，请与ＱＣ联系  
                        FisException fe = new FisException("CHK282", new string[] { currentProduct.ProId, currentProduct.CUSTSN });
                        throw fe;
                    }
                }
                return "OK";
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                return e.mErrmsg;
                //throw new Exception(e.Message); ;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(DismantleFA)CheckPrdIDorSNList End,"
                      + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);

            }
        }
        /// <summary>
        /// DismantleBatch
        /// </summary>
        /// <param name="snorproidlist"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="pCode"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void DismantleBatch(IList<string> snorproidlist, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer)
        {
            logger.Debug("(DismantleFA)DismantleBatch start,"
                      + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            //IList<string> retLst = null;
            List<string> erpara = new List<string>();
            string snorproid = "";
            try
            {
                for (int i = 0; i < snorproidlist.Count; i++)
                {
                    snorproid = snorproidlist[i];
                    //snorproid = snorproidlist[0];
                    var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
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
                        if (sDismantleType.ToUpper().Trim() == "KP")
                        {
                            sessionInfo.AddValue(Session.SessionKeys.ReturnStation, sReturnStation);
                            sessionInfo.AddValue(Session.SessionKeys.KPType, sKeyparts);
                        }
                        sessionInfo.AddValue("Dismantletype", sDismantleType.ToUpper());
                        sessionInfo.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                        sessionInfo.AddValue(Session.SessionKeys.PalletNo, currentProduct.PalletNo);
                        sessionInfo.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                        string getv = (string)sessionInfo.GetValue("Dismantletype");


                        string wfName, rlName;
                        RouteManagementUtils.GetWorkflow(station, "FADismantle.xoml", "fadismantle.rules", out wfName, out rlName);
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
                    /*
                    else
                    {
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    */
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
                logger.Debug("(DismantleFA)DismantleBatch End,"
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);

            }
        }
    
        /// <summary>
        /// Dismantle
        /// </summary>
        /// <param name="snorproid"></param>
        /// <param name="sDismantleType"></param>
        /// <param name="sKeyparts"></param>
        /// <param name="sReturnStation"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        public void Dismantle(string snorproid, string sDismantleType, string sKeyparts, string sReturnStation, string line, string pCode, string editor, string station, string customer)
        {

            logger.Debug("(DismantleFA)Dismantle start,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);
            FisException ex;
            //IList<string> retLst = null;
            List<string> erpara = new List<string>();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
               
                /*
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
                  */
                //ITC-1268-0019
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                ProductStatusInfo productStatus = productRepository.GetProductStatusInfo(currentProduct.ProId);
                /*
                if ((productStatus.station == station) && (productStatus.status == 1))
                {
                    //已经做过Dismantle的Product，再次进入此站,报提示
                    FisException fe = new FisException("CHK150", new string[] { snorproid });
                    throw fe;
                }
                 */
                string prddidstation = productStatus.station.Trim();
                //Modify 2012/03/09：	Station in ('71','73''74') 提示“Please scan PIA/EPIA OutPut First!”-〉保留73
                //if ((prddidstation == "71") || (prddidstation == "73") || (prddidstation == "74"))
                if (prddidstation == "73")
                {
                    //提示“Please scan PIA/EPIA OutPut First!”  
                    FisException fe = new FisException("CHK151", new string[] { snorproid });
                    throw fe;
                }
                //int numStation = Convert.ToInt32(productStatus.station);
                //IList<ProductLog> prodLogLst = new List<ProductLog>();
                //prodLogLst = productRepository.GetProductLogs(currentProduct.ProId, "69");

                //if (prodLogLst.Count > 0)
                //{
                //    //提示“已经到包装，不能dismantle!”
                //    FisException fe = new FisException("CHK139", new string[] { snorproid });
                //    throw fe;
                //}
                //----Modify 2012/08/24----------------------------------------------------------
                //若ProductStatus.Station 不在 (ConstValue.Value( Condtion: ConstValue.Name = ‘FADismantleStation’  and Type=’FAStation’))之中，则提示：“不在Dismantle的Station中，请Check”
                //Note：
                //ConstValue.Value格式如下：Station1, Station2, Station3…..
                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ConstValueInfo info = new ConstValueInfo();
                info.type = "FAStation";
                info.name = "FADismantleStation";
                IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
                if (retList != null && retList.Count > 0)
                {
                    var iloc = retList[0].value.IndexOf(prddidstation);
                    if (iloc == -1)
                    {
                        //不在Dismantle的Station中，请Check  
                        FisException fe = new FisException("CHK308", new string[] { snorproid });
                        throw fe;
                    }
                    else if (prddidstation.Trim() == "")
                    {
                        //不在Dismantle的Station中，请Check  
                        FisException fe = new FisException("CHK308", new string[] { snorproid });
                        throw fe;
                    }
                }
                else
                {
                    //请联系IE，维护Dismantle的Station
                    FisException fe = new FisException("CHK307", new string[] { snorproid });
                    throw fe;
                }
                //-------------------------------------------------------------------------------

                //if ((prddidstation == "75") || (productStatus.status == 0))
                //Modify 2012/03/09：UC Station=75-〉6A Status=0 “此台为PIA/EPIA检测不良，请与ＱＣ联系”
                //if ((prddidstation == "75") && (productStatus.status == 0))
                if ((prddidstation == "6A") && (productStatus.status == 0))
                {
                    //提示此台为PIA/EPIA检测不良，请与ＱＣ联系  
                    FisException fe = new FisException("CHK153", new string[] { snorproid });
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
                    if (sDismantleType.ToUpper().Trim() == "KP")
                    {
                        sessionInfo.AddValue(Session.SessionKeys.ReturnStation, sReturnStation);
                        sessionInfo.AddValue(Session.SessionKeys.KPType, sKeyparts);
                    }
                    sessionInfo.AddValue("Dismantletype", sDismantleType.ToUpper());
                    /*
                    if (dismantletype.toUpperCase() == "PRODUCT")
                    {
                        idismantle = 1;
                    }
                    else if (dismantletype.toUpperCase() == "KP")
                    {
                        idismantle = 2;
                    }
                    else if (dismantletype.toUpperCase() == "IMEI")
                    {
                        idismantle = 3;
                    }
                    else if (dismantletype.toUpperCase() == "AST")
                    {
                        idismantle = 4;
                    }
                    else
                    {
                        idismantle = 0;
                    }
                    */
                    sessionInfo.AddValue(Session.SessionKeys.Carton, currentProduct.CartonSN);
                    sessionInfo.AddValue(Session.SessionKeys.PalletNo, currentProduct.PalletNo);
                    sessionInfo.AddValue(Session.SessionKeys.DeliveryNo, currentProduct.DeliveryNo);
                    string getv = (string)sessionInfo.GetValue("Dismantletype");


                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "FADismantle.xoml", "fadismantle.rules", out wfName, out rlName);
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
