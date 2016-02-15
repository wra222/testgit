/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
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
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using dm=IMES.DataModel;
using System.Collections;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;


namespace IMES.Station.Implementation
{
    /// <summary>
    /// IOQCOutput接口的实现类
    /// </summary>
    public class ReturnUsedKeysImpl : MarshalByRefObject, ReturnUsedKeys 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region ReturnUsedKeys members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        public ArrayList Check( string custSn,string editor, string stationId, string customer)
        {
            logger.Debug("(ReturnUsedKeysImpl)Check start " 
                + " [custSn]: " + custSn
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();    
            try
            {
                //*
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct CurrentProduct = null;
                CurrentProduct = productRepository.GetProductByCustomSn(custSn);

                if (CurrentProduct == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(custSn);
                    throw new FisException("SFC002", errpara);
                }
                if (string.IsNullOrEmpty(CurrentProduct.ProId))
                {
                    //erpara.Add("CustSN：XXX不存在");
                    erpara.Add(CurrentProduct.CUSTSN);
                    ex = new FisException("CHK936", erpara);
                    throw ex;
                }

                if (CurrentProduct.Status.Status == 0)
                {
                    //erpara.Add("CustSN：XXX存在不良，请先去修护");
                    erpara.Add(CurrentProduct.CUSTSN);
                    ex = new FisException("CHK938", erpara);
                    throw ex;
                }
                IList<string> itemTypes = new List<string>();
                itemTypes.Add("P/N");
                IList<IMES.FisObject.FA.Product.ProductInfo> productInfo ;
                productInfo = productRepository.GetProductInfoListUpperCaseItemType(CurrentProduct.ProId ,  itemTypes);
                if(productInfo.Count == 0){
                    //erpara.Add("CustSN：XXX没有找到上传的信息");
                    erpara.Add(CurrentProduct.CUSTSN);
                    ex = new FisException("CHK958", erpara);
                    throw ex;
                }
                retvaluelist.Add(productInfo[0].InfoValue);
                //*/
               // retvaluelist.Add("PN" + custSn);
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
                logger.Debug("(ReturnUsedKeysImpl)Check end:" 
                    + " [custSn]: " + custSn
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
            logger.Debug("(PrintInatelICASAImpl)Cancel start, [prodId]:" + prodId);
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
                logger.Debug("(PrintInatelICASAImpl)Cancel end, [prodId]:" + prodId);
            }
        }

        public ArrayList Save(List<string> SNList,
                              List<string> PNList,
                              List<string> errList, string Editor, string Station, string customer)
        {
            logger.Debug("(ReturnUsedKeysImpl)Save start" 
                
                + " [editor]:" + Editor
                + " [station]:" + Station
                + " [customer]:" + customer);
            //FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retvaluelist = new ArrayList();
            string ECOAReturnStatus = "0";
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            try
            {
                ///生成group id
                SqlTransactionManager.Begin();
                IUnitOfWork Number_uof = new UnitOfWork();//使用自己的UnitOfWork
                string prestr = "";
                string maxnum = "";
                bool addflag = false;
               
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                maxnum = numControl.GetMaxNumber("GroupID", prestr + "{0}");
                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "001";
                    addflag = true;
                }
                else
                {
                    if (maxnum.Substring(6, 3).ToUpper() == "999")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    else
                    {
                        int count = Int32.Parse(maxnum.Substring(6, 3));
                        count = count + 1;
                        maxnum = (count).ToString().PadLeft(3, '0');
                    }
                }
                DateTime dd = DateTime.Now;
                string Groupid = dd.Year.ToString().Substring(2,2) + dd.Month.ToString().PadLeft(2, '0') + dd.Day.ToString().PadLeft(2, '0') + maxnum;
                NumControl item = new NumControl();
                item.NOType = "GroupID";
                item.Value = Groupid;
                item.NOName = "";
                item.Customer = "HP";

                numControl.SaveMaxNumber(item, addflag, prestr + "{0}");
               
                Number_uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值
                SqlTransactionManager.Commit();//提交事物，释放行级更新锁

                
                 
                /////////////////////////////////////////////////
                IUnitOfWork work = new UnitOfWork();
                for (int i = 0; i < SNList.Count; i++)
                {
                    IProduct CurrentProduct = null;
                    if (errList[i] == "Pass")
                    {
                        string custSn = SNList[i];
                                           
                        CurrentProduct = productRepository.GetProductByCustomSn(custSn);
                        if (CurrentProduct == null)
                        {
                            continue;
                        }
                        string line = CurrentProduct.Status.Line;
                        StationStatus Status = StationStatus.Pass;
                      
                        var productLog = new ProductLog
                        {
                            Model = CurrentProduct.Model,
                            Status = Status,
                            Editor = Editor,
                            Line = line,
                            Station = Station,
                            Cdt = DateTime.Now
                        };
                        CurrentProduct.AddLog(productLog);
                      
                        /////////update product status
                        var newStatus = new ProductStatus();
                        newStatus.Status = Status;
                        newStatus.StationId = Station;
                        newStatus.Editor = Editor;
                        newStatus.Line = line;
                        newStatus.TestFailCount = 0;

                        if (!string.IsNullOrEmpty(CurrentProduct.Status.ReworkCode) && productRepository.IsLastReworkStation(CurrentProduct.Status.ReworkCode, Station, (int)Status))
                        {
                            newStatus.ReworkCode = "";
                            IMES.DataModel.Rework r = new IMES.DataModel.Rework();
                            r.ReworkCode = CurrentProduct.Status.ReworkCode;
                            r.Editor = Editor;
                            r.Status = "3";
                            r.Udt = DateTime.Now;
                            productRepository.UpdateReworkConsideredProductStatusDefered(work, r, CurrentProduct.ProId);
                        }
                        else
                        {
                            newStatus.ReworkCode = CurrentProduct.Status.ReworkCode;
                        }
                        newStatus.ProId = CurrentProduct.ProId;
                        CurrentProduct.UpdateStatus(newStatus);

                        productRepository.Update(CurrentProduct, work);

                        ECOAReturnStatus = "1";
                    }
                    dm.EcoareturnInfo item1 = new dm.EcoareturnInfo();
                    item1.custsn = SNList[i];
                    item1.partNo = PNList[i];
                    item1.message = errList[i];
                    item1.status = ECOAReturnStatus;
                    item1.line = CurrentProduct.Status.Line;
                    item1.groupNo = Groupid;
                    item1.editor = Editor;
                    
                    currentRepository.InsertEcoareturn(item1);

                }
                work.Commit();

                dm.SysSettingInfo condition = new dm.SysSettingInfo();
                condition.name = "ReturnECOAURL";
                IList<dm.SysSettingInfo> tmp = ipartRepository.GetSysSettingInfoes(condition);
                if (tmp.Count == 0 || tmp[0].value == "")
                {
                    FisException fe = new FisException("CHK962", new string[] { });   //未设置ReturnECOAURL
                    throw fe;
                }


                retvaluelist.Add(Groupid);
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
                logger.Debug("(ReturnUsedKeysImpl)Save end" 
                    
                    + " [editor]:" + Editor
                    + " [station]:" + Station
                    + " [customer]:" + customer);
            }
        }
        #endregion
    }
}
