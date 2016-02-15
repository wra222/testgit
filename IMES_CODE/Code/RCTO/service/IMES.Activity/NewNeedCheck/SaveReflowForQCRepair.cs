/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for QC Repair Page
* UI:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/2/16 
* UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2012/7/18            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-7-18   Jessica Liu           Create
* Known issues:
* TODO：
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;
using System.Data;
using IMES.FisObject.Common.Station;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// Save Reflow处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      QC Repair
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取上一站@NextStation
    ///         2.获取QCStatusID
    ///         3.Update QCStatus
    ///         4.跳站
    ///         5.Message
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Station
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///             
    /// </para> 
    /// </remarks>
    public partial class SaveReflowForQCRepair : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveReflowForQCRepair()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save Reflow处理
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string returnStation = (string)CurrentSession.GetValue("ReturnStation");
            string returnStationText = (string)CurrentSession.GetValue("ReturnStationText");
            CurrentSession.AddValue("StationStation", returnStation);
            CurrentSession.AddValue("StationDescr", returnStationText);

            if (returnStation == "PO" || returnStation == "6A")
            {

                //获取上一站@NextStation
                /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IStation iStation = null;
                string[] stationLst = {"76", "7P", "45"};
                ProductLog proLog = new ProductLog();
                proLog.ProductID = currenProduct.ProId;
                IList<ProductLog> proLogLst = productRepository.GetProductLogList(proLog, stationLst);
                if ((proLogLst != null) && (proLogLst.Count != 0))
                {
                    string stionid = proLogLst[0].Station;
                    IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                    iStation = stationRepository.Find(stionid);                 
                }

                if (iStation != null)
                {
                    CurrentSession.AddValue("StationStation", iStation.StationId);
                    CurrentSession.AddValue("StationDescr", iStation.Descr);
                }
                else
                {
                    CurrentSession.AddValue("StationStation", Station);
                    CurrentSession.AddValue("StationDescr", "");
                }
                */
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                //获取QCStatusID
                //select top 1 @QCStatusID=ID, @PrdQCStatus = Status from QCStatus where ProductID = @ProductID order by Udt desc
                IList<ProductQCStatus> proQCStatusLst = productRepository.GetQCStatusOrderByUdtDesc(currenProduct.ProId);
                //int QCStatusId = -1;
                string PrdQCStatus = null;
                if ((proQCStatusLst != null) && (proQCStatusLst.Count > 0))
                {
                    ProductQCStatus currentStatus = proQCStatusLst[0];
                    //QCStatusId = proQCStatusLst[0].ID;
                    PrdQCStatus = proQCStatusLst[0].Status;

                    //Update QCStatus
                    //Update QCStatus Status = (@PrdQCStatus=’A’,则为’8’; =’7’,则为’5’;=’4’,则为’2’) Udt = Getdate() Where ID=@QCStatusID
                    if (PrdQCStatus == "A")
                    {
                        currentStatus.Status = "8";
                    }
                    else if (PrdQCStatus == "7")
                    {
                        currentStatus.Status = "5";
                    }
                    else if (PrdQCStatus == "4")
                    {
                        currentStatus.Status = "2";
                    }

                    currenProduct.UpdateQCStatus(currentStatus);
                    productRepository.Update(currenProduct, CurrentSession.UnitOfWork);
                }
                else
                {
                    List<String> erpara = new List<String>();
                    throw new FisException("CHK932", erpara);
                }
            }


            //跳站
	        //若ForceNWC存在（ForceNWC.ProductID=@ProductID）则Update ForceNWC 否则Insert  ForceNWC
	        //PreStation=@CurrentStation
	        //ForceNWC=@NextStation
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ForceNWCInfo cond = new ForceNWCInfo();
            cond.productID = currenProduct.ProId;

            ForceNWCInfo forceNWCInfo = new ForceNWCInfo();
            forceNWCInfo.preStation = Station;
            /* 2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            if (iStation != null)
            {
                forceNWCInfo.forceNWC = iStation.StationId;
                forceNWCInfo.productID = currenProduct.ProId;
                forceNWCInfo.editor = Editor;
            }
            else
            {
                List<string> errpara = new List<string>();
                errpara.Add(currenProduct.ProId);
                throw new FisException("CHK925", errpara);
            }
            */
            forceNWCInfo.forceNWC = returnStation;
            forceNWCInfo.productID = currenProduct.ProId;
            forceNWCInfo.editor = Editor;

            bool bExist = ipartRepository.CheckExistForceNWC(cond);
            if (bExist == true)
            {
                ipartRepository.UpdateForceNWC(forceNWCInfo ,cond);
            }
            else
            {
                ipartRepository.InsertForceNWC(forceNWCInfo);
            }

            return base.DoExecute(executionContext);

        }
    }
}
