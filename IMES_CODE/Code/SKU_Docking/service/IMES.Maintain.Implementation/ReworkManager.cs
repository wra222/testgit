/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-4-21  itc207024          Create 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Process;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Maintain.Implementation
{
    class ReworkManager : MarshalByRefObject, IRework
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        public IProcessRepository processRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();


        /// <summary>
        ///根据productIDList取得product的详细信息，先将全部ProductID存入一个临时表中，在
        ///拿临时表与Product表结合，取出详细信息。
        /// </summary>
        /// <returns></returns>
        public IList<ProductInfoMaintain> GetProductListByIDList(IList<string> productIDList)
        {
            //string userKey = Guid.NewGuid().ToString();
            IList<IProduct> productList;
            IList<ProductInfoMaintain> productInfoMaintainList = new List<ProductInfoMaintain>();
            try
            {
                //insert ProductIDListForRework(UserKey,ProductID) values(?,?)
                //productRepository.CreateTempProductIDList(productIDList, userKey);

                //select b.ProductID,b.CUSTSN,b.Model,b.MO,b.DeliveryNo,c.Station,c.Status 
                //from IMES_GetData.dbo.TempProductID a 
                //     join IMES_FA.dbo.Product b on a.ProductID = b.ProductID or a.ProductID = b.CUSTSN
                //     join IMES_FA.dbo.ProductStatus c on c.ProductID=b.ProductID 
                //where a.UserKey=？ 
                //order by b.ProductID 
                //productList = productRepository.GetProductListByByUserKey(userKey);
                productList=productRepository.GetProductListByIdList(productIDList);
                
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                //delete ProductIDListForRework where UserKey=?
                //productRepository.DeleteProductIDListByUserKey(userKey);
            }

            foreach(Product product in productList)
            {
                ProductInfoMaintain productInfoMaintain = new ProductInfoMaintain();
                productInfoMaintain.ProductID = product.ProId;
                productInfoMaintain.Sn = product.CUSTSN;
                if (product.Status == null)
                {
                    productInfoMaintain.Station = "";
                    productInfoMaintain.Status = "";
                }
                else
                {
                    productInfoMaintain.Station = product.Status.StationId;
                    productInfoMaintain.Status = product.Status.Status.ToString();
                }
                productInfoMaintain.Model = product.Model;
                productInfoMaintain.Mo = product.MO;
                productInfoMaintain.Dn = product.DeliveryNo;
                productInfoMaintainList.Add(productInfoMaintain);
            }

            return productInfoMaintainList;
        }

        /// <summary>
        ///返回不合法的productID，逗号分割，先将全部ProductID存入一个临时表中，在
        ///拿临时表与Product表结合，结合不上的数据认为是不合法的
        /// </summary>
        /// <returns></returns>
        public string ReturnInvalidProductID(IList<string> productIDList)
        {
            string userKey = Guid.NewGuid().ToString();
            IList<string> productIDListReturn;
            string returnValues="";
            try
            {
                //insert ProductIDListForRework(UserKey,ProductID) values(?,?)
                productRepository.CreateTempProductIDList(productIDList, userKey);

                //select a.ProductID  
                //from IMES_GetData.dbo.TempProductID a 
                //     left outer join IMES_FA.dbo.Product b on a.ProductID = b.ProductID  
                //     left outer join IMES_FA.dbo.Product c on a.ProductID = c.CUSTSN  
                //where a.UserKey=？and b.ProductID is null and c.ProductID is null  
                productIDListReturn = productRepository.GetAllInvalidProductIdByUserKey(userKey);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
            finally
            {
                //delete ProductIDListForRework where UserKey=?
                productRepository.DeleteProductIDListByUserKey(userKey);
            }

            if (productIDListReturn != null && productIDListReturn.Count > 0)
            {
                int i = 0;
                while (i < productIDListReturn.Count)
                {
                    if (i == 0)
                    {
                        returnValues = productIDListReturn[i];
                    }
                    else
                    {
                        returnValues += "," + productIDListReturn[i];
                    }
                    i++;
                }
                return returnValues;
            }
            else
            {
                return null;
            }
        }

        //检查输入的ProductID是否是已经存在的ProductID
        public bool CheckExistedProdId(string strProdId)
        {
            //select a.ProductID,a.CUSTSN,a.Model,a.MO,a.DeliveryNo,b.Station,b.Status 
            //from Product a
            //     join ProductStatus b on a.ProductID=b.ProductID 
            //where a.ProductID = 'productIdOrSN' or a.CUSTSN = 'productIdOrSN'  
            IProduct product = productRepository.GetProductByIdOrSn(strProdId);
            //ITC-1136-0143   添加|| product.Status == null itc207024 2010-05-11
            if (product == null || product.Status == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //取得Product的详细信息
        public ProductInfoMaintain GetProductInfo(string strProdId)
        {
            //select a.ProductID,a.CUSTSN,a.Model,a.MO,a.DeliveryNo,b.Station,b.Status 
            //from Product a
            //     join ProductStatus b on a.ProductID=b.ProductID 
            //where a.ProductID = 'productIdOrSN' or a.CUSTSN = 'productIdOrSN'  
            IProduct product = productRepository.GetProductByIdOrSn(strProdId);
            ProductInfoMaintain productMaintainInfo = new ProductInfoMaintain();
            if (product == null)
            {
                return productMaintainInfo;
            }
            productMaintainInfo.ProductID = product.ProId;
            productMaintainInfo.Sn = product.CUSTSN;
            if (product.Status == null)
            {
                productMaintainInfo.Station = "";
                productMaintainInfo.Status = "";
            }
            else
            {
                productMaintainInfo.Station = product.Status.StationId;
                productMaintainInfo.Status = product.Status.Status.ToString();
            }
            productMaintainInfo.Model = product.Model;
            productMaintainInfo.Mo = product.MO;
            productMaintainInfo.Dn = product.DeliveryNo;
            return productMaintainInfo;
        }

        /// <summary>
        /// 创建Rework，检查Product List中所有Product是否可以做Rework，若不可以，则提示用户，放弃后续操作。
        /// </summary>
        /// <returns></returns>
        public string CreateRework(IList<string> productIDList,string editor)
        {
            string userKey = Guid.NewGuid().ToString();
            //string reworkCode;
            Rework rework = new Rework();
            try
            {
                //insert ProductIDListForRework(UserKey,ProductID) values(?,?)
                productRepository.CreateTempProductIDList(productIDList, userKey);

                //select count(1) 
                //from IMES_GetData.dbo.TempProductID a 
                //     join IMES_FA.dbo.Rework_Product b on a.ProductID = b.ProductID  
                //     join IMES_FA.dbo.Rework c on b.ReworkCode=c.ReworkCode 
                //where Rework.Status<>3 and a.UserKey=?
                int count1 = productRepository.GetUnitExistsCountByUserKey(userKey);

                //select count(1) 
                //from IMES_GetData.dbo.TempProductID a 
                //     join IMES_FA.dbo.ProductStatus b on a.ProductID = b.ProductID  
                //     join IMES_GetData.dbo.ReworkRejectStation c on b.Station =c.Station and b.Status=c.Status  
                //where a.UserKey=? 
                int count2 = productRepository.GetInvalidUnitCountByUserKey(userKey);

                List<string> erpara = new List<string>();
                if (count1 > 0)
                {
                    //存在Unit存在Rework_Product表且ReworkCode对应的Rework.Status<>3
                    FisException ex1 = new FisException("DMT023", erpara);
                    throw ex1;
                }
                if (count2 > 0)
                {
                    //存在unit的Product.Station and Status存在于ReworkRejectStation表
                    FisException ex2 = new FisException("DMT024", erpara);
                    throw ex2;
                }

                rework.Status = "0";
                rework.Editor = editor;
                rework.Cdt = DateTime.Now;
                rework.Udt = DateTime.Now;

                UnitOfWork uow = new UnitOfWork();
                //创建Rework记录，Rework.Status=0(Create)
                productRepository.CreateReworkDefered(uow, rework);
                //将对应ProductStatus表中的ReworkCode栏位设置为新产生的Rework Code。
                ReworkObj rwkObj = new ReworkObj();
                SetterBetween stbt = new SetterBetween(rework, "ReworkCodeProperty", rwkObj, "ReworkCodeProperty");
                uow.RegisterSetterBetween(stbt);
                productRepository.UpdateProductStatusReworkCodeByUserKeyDefered(uow, userKey, rwkObj);
                uow.Commit();
            }
            catch (Exception e)
            {
                logger.Debug(e.Message);
                throw e;
            }
            finally
            {
                //delete ProductIDListForRework where UserKey=? 
                productRepository.DeleteProductIDListByUserKey(userKey);
            }

            return rework.ReworkCode;
        }

        /// <summary>
        /// 选项包括所有Process,按字符序排列 
        /// </summary>
        /// <returns></returns>
        public IList<ProcessMaintainInfo> GetProcessList()
        {
            IList<Process> rempList = processRepository.FindAll();
            List<ProcessMaintainInfo> processMaintainInfoList = new List<ProcessMaintainInfo>();
            foreach(Process process in rempList)
            {
                ProcessMaintainInfo processMaintainInfo = new ProcessMaintainInfo();
                processMaintainInfo.Process = process.process;
                processMaintainInfo.Editor = process.Editor;
                processMaintainInfo.Cdt = process.Cdt;
                processMaintainInfo.Udt = process.Udt;
                processMaintainInfoList.Add(processMaintainInfo);
            }
            return processMaintainInfoList;
        }

        /// <summary>
        /// 取得指定时间段内修改过的Rework记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetReworkList(string strDateFrom, string strDateTo)
        {
            DateTime from = DateTime.Parse(strDateFrom);
            DateTime to = DateTime.Parse(strDateTo);
            to=to.AddDays(1).AddMilliseconds(-1);
            DataTable reworkList = productRepository.GetReworkList(from, to);
            if (reworkList == null)
            {
                return new DataTable();
            }
            return reworkList; 
        }

        /// <summary>
        ///设置strReworkCode与strProcess相关联 
        /// </summary>
        /// <returns></returns>
        public void SetProcess(string strProcess, string reworkCode, string editor)
        {
            string reworkStatus = productRepository.GetReworkStatus(reworkCode);
            if (reworkStatus == null)
            {
                return;
            }
            //判断rework的当前状态
            if (!reworkStatus.Equals("0"))
            {
                List<string> erpara = new List<string>();
                FisException ex1 = new FisException("DMT025", erpara);
                throw ex1;
            }
            //保存rework 和 process的关系
            ReworkProcess reworkProcess = new ReworkProcess();
            reworkProcess.ReworkCode = reworkCode;
            reworkProcess.Process = strProcess;
            reworkProcess.Editor = editor;
            reworkProcess.Cdt = DateTime.Now; 
            reworkProcess.Udt = DateTime.Now; 
            productRepository.SetReworkProcess(reworkProcess);
        }

        /// <summary>
        /// 删除Rework记录 
        /// </summary>
        /// <returns></returns>
        public void DeleteRework(string reworkCode)
        {
            string reworkStatus = productRepository.GetReworkStatus(reworkCode);
            if (reworkStatus == null)
            {
                return;
            }
            //判断rework的当前状态
            if (reworkStatus.Equals("2") || reworkStatus.Equals("3"))
            {
                List<string> erpara = new List<string>();
                FisException ex1 = new FisException("DMT026", erpara);
                throw ex1;
            }

            //删除Rework记录,Rework_Process记录,清空ProductStatus记录的ReworkCode栏位
            //a)update ProductStatus set ReworkCode='' where ReworkCode=? 
            //b)delete Rework_Process where ReworkCode=? 
            //c)delete Rework where ReworkCode=? 
            productRepository.RemoveARework(reworkCode);

        }

        /// <summary>
        /// 判断Rework是否可以Submit 
        /// </summary>
        /// <returns></returns>
        public void CheckReworkSubmit(string reworkCode)
        {
            string reworkStatus = productRepository.GetReworkStatus(reworkCode);
            if (reworkStatus == null)
            {
                return;
            }
            //判断rework的当前状态
            if (!reworkStatus.Equals("0"))
            {
                List<string> erpara = new List<string>();
                FisException ex1 = new FisException("DMT027", erpara);
                throw ex1;
            }

            //ITC-1136-0142 added itc207024
            if (!productRepository.IFReworkHasProcess(reworkCode))
            {
                List<string> erpara2 = new List<string>();
                FisException ex2 = new FisException("DMT033", erpara2);
                throw ex2;
            }
        }

        /// <summary>
        /// 判断Rework是否有ReleaseType   
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllReleaseTypeByRework(string reworkCode)
        {
            IList<string> releaseTypeList = productRepository.GetProcessReleaseType(reworkCode);
            if (releaseTypeList == null)
            {
                return new List<string>();
            }
            else
            {
                return releaseTypeList;
            }
        }

        /// <summary>
        /// 将被选Rework的状态改为1(Submit)  
        /// </summary>
        /// <returns></returns>
        public void SubmitRework(string reworkCode,string editor)
        {
            Rework rework = new Rework();
            rework.ReworkCode = reworkCode;
            rework.Status = "1";
            rework.Editor = editor;
            rework.Udt = DateTime.Now;

            //更新Rework记录，Rework.Status=1(Create)
            productRepository.UpdateRework(rework);
        }

        /// <summary>
        /// 将被选Rework的状态改为2(Confirm)  ,复制数据, 解绑
        /// </summary>
        /// <returns></returns>
        public void ConfirmRework(string reworkCode,string editor)
        {
            string reworkStatus = productRepository.GetReworkStatus(reworkCode);
            if (reworkStatus == null)
            {
                return;
            }
            //判断rework的当前状态
            if (reworkStatus.Equals("0"))
            {
                List<string> erpara = new List<string>();
                FisException ex1 = new FisException("DMT028", erpara);
                throw ex1;
            }
            if (!reworkStatus.Equals("1"))
            {
                List<string> erpara = new List<string>();
                FisException ex2 = new FisException("DMT029", erpara);
                throw ex2;
            }

            Rework rework = new Rework();
            rework.ReworkCode = reworkCode;
            rework.Status = "2";
            rework.Editor = editor;
            rework.Udt = DateTime.Now;

            //复制数据，Update ProductStatus, Station=‘RW’, Status=1, Insert ProductLog。 
            //解绑数据
            //1，取得ReleaseType 
            //2，根据ReleaseType解绑数据
            //设置对应Rework Process的首站的PreStation（即PreStation=‘‘）为‘RW’
            productRepository.ClearData(reworkCode, editor);

            //更新Rework记录，Rework.Status=2(Create)
            productRepository.UpdateRework(rework);

        }

        //取得ReworkCode下的unit
        public DataTable GetProductListByReworkCode(string strReworkCode)
        {
            DataTable result = productRepository.GetProductListByReworkCode(strReworkCode);
           
            if (result == null)
            {
                return new DataTable();
            }

            foreach(DataRow row in result.Rows)
            {
                row[6]=Enum.GetName(typeof(StationStatus), Enum.Parse(typeof(StationStatus), row[6].ToString()));
            }
            return result;
        }

        //取得Rework下的Product，按Model分组
        public IList<ModelAndCount> GetProductListByReworkGroupByModel(string reworkCode)
        {
            IList<ModelAndCount> result = productRepository.GetProductModelStatisticByReworkCode(reworkCode);
            if (result == null)
            {
                return new List<ModelAndCount>();
            }
            else
            {
                return result;
            }
        }

        //取得Rework下的Product，按Station分组
        public IList<StationAndCount> GetProductListByReworkGroupByStation(string reworkCode)
        {
            IList<StationAndCount> result = productRepository.GetProductStationStatisticByReworkCode(reworkCode);
            if (result == null)
            {
                return new List<StationAndCount>();
            }
            else
            {
                return result;
            }
        }

        //取得Rework下的unit的数量
        public int GetUnitCountByRework(string reworkCode)
        {
            return productRepository.GetUnitCountByRework(reworkCode);
        }

        //取得Rework的ProcessStation
        public IList<ProcessStationInfo> GetProcessStationList(string reworkCode)
        {
            IList<ProcessStationInfo> result = productRepository.GetProcessStationList(reworkCode);
            if (result == null)
            {
                return new List<ProcessStationInfo>();
            }
            else
            {
                return result;
            }
        }
    }
}
