/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ProdID Dismantle implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-06-06  Kaisheng           
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Docking.Interface.DockingIntf;
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
namespace IMES.Docking.Implementation
{
    public class DismantleForDocking : MarshalByRefObject, IDismantleForDocking
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

            logger.Debug("(DismantleDocking)Dismantle start,"
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
                var inputProdID = false;
                //var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IProduct currentProduct;
                string inputString = snorproid.Trim();
                if ((inputString.Length == 10) || (inputString.Substring(0, 3) == "CNU" || inputString.Substring(0, 3) == "5CG"))
                {
                    inputProdID = false; //CustSN
                    currentProduct = productRepository.GetProductByCustomSn(inputString);
                }
                else 
                {
                    inputProdID = true;
                    if (inputString.Length == 10)
                        inputString = inputString.Substring(0, 9);
                    currentProduct = productRepository.Find(inputString);

                }
                //var currentProduct = CommonImpl.GetProductByInput(snorproid, CommonImpl.InputTypeEnum.ProductIDOrCustSN);
                //	若Product/PruductStatus不存在该Product信息，则报错：“Product：XXX不存在”
                if (currentProduct == null)
                {
                    if (inputProdID)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(snorproid);
                        throw new FisException("SFC002", errpara);
                    }
                    else
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(snorproid);
                        //throw new FisException("PAK042", errpara);
                        throw new FisException("CHK144", errpara);
                    }
                }
                ProductStatusInfo productStatus = productRepository.GetProductStatusInfo(currentProduct.ProId);
                if (productStatus.station == null)
                {
                    erpara.Add(currentProduct.ProId);
                    ex = new FisException("PAK026", erpara);    //没有Product Status 站数据！
                    throw ex;

                }
                //UC Update 2012/08/27 去除固定站点的检查；
                ////	若ProductLog存在’69’或者’80’或者’81’的记录，则报错：“Product：XXX已流入包装，不能Dismantle”
                //if (currentProduct.ProductLogs != null)
                //{
                //    foreach (var log in currentProduct.ProductLogs)
                //    {
                //        if ((log.Station == "69") || (log.Station == "80") || (log.Station == "81"))
                //        {
                //            //FisException fe = new FisException("CHK139", new string[] { snorproid });
                //            FisException fe = new FisException("CHK141", new string[] { snorproid });
                //            throw fe;
                //        }
                //    }
                //}
                //	若ProductStatus.Station=’38’，则报错：“Product：XXX已Dismantle，不需再次执行”
                string prddidstation = productStatus.station.Trim();
                //UC update 2012/08/27--若ProductStatus.Station 不在 (ConstValue.Value( Condtion: ConstValue.Name = ‘FADismantleStation’  
                //and Type=’FAStation’))之中，则提示：“不在Dismantle的Station中，请Check”
                //------------------------------------------------------------------------------------
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
                //------------------------------------------------------------------------------------
                //if ((prddidstation == "38") ||(productStatus.status==1))
                if (prddidstation == "38")
                {
                    //已经做过Dismantle的Product，再次进入此站,报提示
                    //FisException fe = new FisException("CHK150", new string[] { snorproid });
                    FisException fe = new FisException("CHK142", new string[] { snorproid });
                    throw fe;
                }
                //	若ProductStatus.Station等于’71’或者’73’，则报错：“Product：XXX为抽检抽中机器，请先刷PIA Output”
                if ((prddidstation == "71") || (prddidstation == "73"))
                {
                    //提示“Please scan PIA/EPIA OutPut First!”  
                    //FisException fe = new FisException("CHK151", new string[] { snorproid });//--error message要修改
                     FisException fe = new FisException("CHK143", new string[] { snorproid });
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
                    RouteManagementUtils.GetWorkflow(station, "ProdIDDismantleDocking.xoml", null, out wfName, out rlName);
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
                logger.Debug("(DismantleDocking)Dismantle End,"
                      + " [CustSN or ProductId]:" + snorproid
                       + " [line]:" + line
                      + " [editor]:" + editor
                      + " [station]:" + station
                      + " [customer]:" + customer);

            }
        }  
        
       

    }
}
