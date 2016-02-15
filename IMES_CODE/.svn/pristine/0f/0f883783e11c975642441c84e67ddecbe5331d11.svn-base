/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/11/04 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/11/04            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-04   Du.Xuan               Create   
* Known issues:
* ITC-1414-0081 恢复重复称重报错
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
using IMES.Infrastructure.Repository._Schema;
using IMES.Docking.Interface.DockingIntf;


namespace IMES.Docking.Implementation
{

    public class PalletWeight : MarshalByRefObject, IPalletWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;


        #region PalletWeight

        /// <summary>
        /// 刷pallletNo，启动工作流，检查输入的pallletNo
        /// 将pallletNo放到Session.PalletNo中
        /// </summary>
        /// <param name="inputID"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputPallet(string inputID, decimal acturalWeight, string type,
                            string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletWeight)InputPallet Start,"
                + " [pallletNo/UCCID]:" + inputID
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);


            //当20 位长的时候,视为UCC ID,否则,视为PalletNo 
            try
            {
                string plt = "";
                ArrayList retList = new ArrayList();
                List<string> erpara = new List<string>();

                if (inputID.Length == 20)
                {
                    string strSQL = "select rtrim(PalletNo) from Pallet where UCC=@palletId";
                    SqlParameter paraName = new SqlParameter("@palletId", SqlDbType.VarChar, 32);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = inputID;

                    object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

                    if (obj == null)
                    {
                        FisException e;
                        erpara.Add(inputID);
                        e = new FisException("PAK092", erpara);
                        throw e;
                    }

                    plt = obj.ToString();
                }
                else
                {
                    plt = inputID;
                }

                string sessionKey = plt;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "PalletWeightForDocking.xoml", "", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, plt);
                    //currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);
                    currentSession.AddValue("PalletType", type);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        //List<string> erpara = new List<string>();
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
                    //List<string> erpara = new List<string>();
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
                //=============================================================
                Pallet curPallet = (Pallet)currentSession.GetValue(Session.SessionKeys.Pallet);
                IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                string strRepeat = "";
                //string strCheck = "";
                //1.	重复称重提示
                //当输入的Pallet 已经称重(ISNULL（IMES_PAK..Pallet.Weight, ''） <> '') 时，
                //使用对话框提示用户：“此栈板为重复称重.”
                if (curPallet.Weight != 0)
                {
                    /*SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK064", erpara);//此栈板为重复称重
                    throw ex;
                    */
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK064", erpara);//此栈板为重复称重
                    strRepeat = ex.mErrmsg;
                    
                }         

                //Product Info – 放在栈板上的Product Model以及数量
                //参考方
                //SELECT Model, Count(*) as PCs FROM IMES_FA..Product nolock 
                //WHERE PalletNo = @PalletNo
                //GROUP BY Model
                //ORDER BY Model
                IList<ModelStatistics> modelList = palletRep.GetByModelStatisticsForSinglePallet(curPallet.PalletNo);

                decimal standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                decimal tolerance = (decimal)currentSession.GetValue(Session.SessionKeys.Tolerance);
                
                //为了支持FRU 出货，当Pallet 结合的Delivery 的Model 是PF 开头时，
                //不需要录入Customer S/N 和Check Customer S/N
                bool fruFLag = false;
                IList<DeliveryPallet> dpList = palletRep.GetDeliveryPallet(curPallet.PalletNo);
                if (dpList.Count > 0)
                {
                    Delivery dn = deliveryRep.Find(dpList[0].DeliveryID);
                    string modelstr = dn.ModelName;
                    if (!string.IsNullOrEmpty(modelstr) && modelstr.Length >= 2)
                    {
                        if (modelstr.Substring(0, 2) == "PF")
                        {
                            fruFLag = true;
                        }
                    }

                }
                currentSession.AddValue("FRUFlag", fruFLag);

                retList.Add(curPallet.PalletNo);
                retList.Add(modelList);
                retList.Add(standardWeight);
                retList.Add(tolerance);
                retList.Add(strRepeat);
                retList.Add(fruFLag);
                //=============================================================
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
                logger.Debug("(PalletWeight)InputPallet End,"
                   + " [pallletNo]:" + inputID
                   + " [line]:" + line
                   + " [editor]:" + editor
                   + " [station]:" + station
                   + " [customer]:" + customer);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="palletNo"></param>
        public String InputCustSN(string custSN, string palletNo)
        {
            logger.Debug("(PalletWeight)Cancel Start,"
               + " [pallletNo]:" + palletNo
               + " [CustSN]:" + custSN);

            var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            
            List<string> erpara = new List<string>();
            FisException ex;
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(palletNo, currentSessionType);

                if (currentSession == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    var currentProduct = productRep.GetProductByCustomSn(custSN); 
                    //1.	Check Pallet No
                    //如果Customer S/N 绑定的Pallet No (IMES_FA..Product.PalletNo)与UI 上已经录入的Pallet No 不同
                    //则报告错误：“此栈板序号与Customer S/N 序号比对错误！”
                    if (currentProduct.PalletNo != palletNo)
                    {
                        erpara.Add(palletNo);
                        ex = new FisException("PAK065", erpara);//此栈板序号与Customer S/N 序号比对错误！
                        throw ex;
                    }

                    //3.	Unit Weight 检查
                    //如果该Customer S/N 对应的ISNULL(Product.UnitWeight, 0.0) = 0.0，
                    //则报告错误：“此Product 尚未进行Unit Weight！”
                    if (currentProduct.UnitWeight.Equals(0))
                    {
                        erpara.Add(currentProduct.ProId);
                        ex = new FisException("PAK122", erpara);//“此Product 尚未进行Unit Weight！”
                        throw ex;
                    }
                    return custSN;
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
                logger.Debug("(PalletWeight)InputCustSN End,"
                + " [pallletNo]:" + palletNo);
            }

        }

        
        /// <summary>
        /// 将ActuralWeight添加到Session.ActuralWeight中
        /// 将custSn放到Session.CustSN中
        /// 结束工作流
        /// </summary>
        /// <param name="pallletNo"></param>
        /// <param name="custSn"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="currentStandardWeight"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string pallletNo, string custSn, decimal acturalWeight, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletWeight)Save Start,"
                + " [pallletNo]:" + pallletNo
                + " [custSn]:" + custSn
                + " [acturalWeight]:" + acturalWeight.ToString());
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(pallletNo, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(pallletNo);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);

                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, pallletNo);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletWeight");

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
                logger.Debug("(PalletWeight)Save End,"
                    + " [pallletNo]:" + pallletNo
                    + " [custSn]:" + custSn
                    + " [acturalWeight]:" + acturalWeight.ToString());
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string pallletNo)
        {
            logger.Debug("(PalletWeight)Cancel Start,"
               + " [pallletNo]:" + pallletNo);
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(pallletNo, currentSessionType);

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
                logger.Debug("(PalletWeight)Cancel End,"
                + " [pallletNo]:" + pallletNo);
            }

        }
        #endregion


        #region "methods do not interact with the running workflow"


        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList ReprintPalletWeightLabel(string custSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletWeight)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            string plt = "";
            ArrayList retList = new ArrayList();
            var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            try
            {
                var currentProduct = productRep.GetProductByCustomSn(custSN); 

                /*string strSQL = "select rtrim(PalletNo) from Pallet where UCC=@palletId";
                SqlParameter paraName = new SqlParameter("@palletId", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = currentProduct.PalletNo;

                object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

                if (obj == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK106", erpara);
                    throw ex;
                 }
                */
                plt = currentProduct.PalletNo;//obj.ToString();
                string sessionKey = currentProduct.ProId;


                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = PalletRepository.Find(plt);
                if (CurrentPallet == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK106", erpara);
                    throw ex;

                }
                if (CurrentPallet.Weight.Equals(0))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                    throw ex;
                }

                /*var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                bool printFlag = false;
                printFlag = repository.CheckExistPrintLogByLabelNameAndDescr(plt, plt);
                */

                var repository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                /*IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,station);
                
                if (logList.Count==0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(plt);                  
                    ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                    throw ex;
                } */
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("044PalletWeightReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);

                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletWeight");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
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

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printList);
                retList.Add(CurrentPallet.PalletNo);
                retList.Add(CurrentPallet.Weight);

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
                logger.Debug("(PalletWeight)ReprintLabel End,"
                                + " [custSN]:" + custSN
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 获取当天已经完成称重的Pallet数量
        /// </summary>
        /// <returns></returns>
        public int getQtyOfPalletToday()
        {
            logger.Debug("(PalletWeight)getQtyOfPalletToday Start");

            try
            {
                int todayWeight;
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                todayWeight = palletRepository.GetQtyOfPackedPalletToday();
                return todayWeight;
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
                logger.Debug("(PalletWeight)getQtyOfPalletToday End");
            }
        }

        /// <summary>
        /// 获取COMSettingInfo
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns></returns>
        public IList<COMSettingDef> GetWeightSettingInfo(string hostname)
        {
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<COMSettingInfo> UnitWeighSettingInfo = new List<COMSettingInfo>();
            UnitWeighSettingInfo = iPalletRepository.FindCOMSettingByName(hostname);


            IList<COMSettingDef> UnitWeighInfoList = new List<COMSettingDef>();
            foreach (COMSettingInfo wsInfo in UnitWeighSettingInfo)
            {
                COMSettingDef wsd = new COMSettingDef();
                wsd.id = wsInfo.id;
                wsd.name = wsInfo.name;
                wsd.commport = wsInfo.commPort;
                wsd.baudRate = wsInfo.baudRate;
                wsd.rthreshold = wsInfo.rthreshold.ToString();
                wsd.sthreshold = wsInfo.sthreshold.ToString();
                wsd.handshaking = wsInfo.handshaking.ToString();
                wsd.editor = wsInfo.editor;
                wsd.cdt = wsInfo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                wsd.udt = wsInfo.udt.ToString("yyyy-MM-dd hh:mm:ss");

                UnitWeighInfoList.Add(wsd);

            }
            if (UnitWeighInfoList == null || UnitWeighInfoList.Count <= 0)
            {
                return null;
            }
            return UnitWeighInfoList;
        }
        
        #endregion

    }
}
