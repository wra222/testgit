// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新ProductStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Product为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Product，调用Product的UpdateStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新ProductStatus  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    ///              ProductStatus
    /// </para> 
    /// </remarks>
    public partial class UpdateProductStatus : BaseActivity
    {
        /// <summary>
        /// Is Return Station Property
        /// </summary>
        public static DependencyProperty IsReturnStatoinProperty = DependencyProperty.Register("IsReturnStatoin", typeof(bool), typeof(UpdateProductStatus));

        /// <summary>
        /// Is Return Station Property
        /// </summary>
        [DescriptionAttribute("IsReturnStatoin")]
        [CategoryAttribute("IsReturnStatoin Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsReturnStatoin
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductStatus.IsReturnStatoinProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatus.IsReturnStatoinProperty, value);
            }
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateProductStatus));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateProductStatus.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatus.StatusProperty, value);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public UpdateProductStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update Product Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentStation = default(string);

            if (IsReturnStatoin)
            {
                currentStation = CurrentSession.GetValue(Session.SessionKeys.ReturnStation).ToString();
            }
            else
            {
                currentStation = Station;
            }
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var newStatus = new ProductStatus();

            string line = default(string);
            if (string.IsNullOrEmpty(this.Line))
            {
                line = currentProduct.Status.Line;
            }
            else
            {
                line = this.Line;
            }

            newStatus.Status = Status;
            newStatus.StationId = currentStation;
            newStatus.Editor = Editor;
            newStatus.Line = line;

            string AllowPass = "";

            if (CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass) != null)
            {
                AllowPass = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass);
            }

            if (AllowPass == "N" && Status == StationStatus.Fail)
            {
                newStatus.TestFailCount = 999;
            }
            else
            {
                newStatus.TestFailCount = 0;
            }

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if ( !string.IsNullOrEmpty(currentProduct.Status.ReworkCode) && productRepository.IsLastReworkStation(currentProduct.Status.ReworkCode,Station,(int)Status))
            {
                newStatus.ReworkCode = "";
                IMES.DataModel.Rework r = new IMES.DataModel.Rework();
                r.ReworkCode = currentProduct.Status.ReworkCode;
                r.Editor = Editor;
                r.Status = "3";
                r.Udt = DateTime.Now;
                productRepository.UpdateReworkConsideredProductStatusDefered(CurrentSession.UnitOfWork,r,currentProduct.ProId);
            }
            else {
                newStatus.ReworkCode = currentProduct.Status.ReworkCode;
            }
            newStatus.ProId = currentProduct.ProId;

            #region record previous product Status
            //System.Data.DataTable preStatus = CreateDataTable.CreateProductStatusTb();
            //preStatus.Rows.Add(currentProduct.ProId,
            //                                   currentProduct.Status.StationId,
            //                                   currentProduct.Status.Status == StationStatus.Pass ? 1 : 0,
            //                                   currentProduct.Status.ReworkCode,
            //                                   currentProduct.Status.Line,
            //                                   currentProduct.Status.TestFailCount,
            //                                   currentProduct.Status.Editor,
            //                                   //currentProduct.Status.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
            //                                   currentProduct.Status.Udt
            //                                   );



            //System.Data.DataTable curStatus = CreateDataTable.CreateProductStatusTb();
            //newStatus.Udt = DateTime.Now;
            //curStatus.Rows.Add(currentProduct.ProId,
            //                                   newStatus.StationId,
            //                                   newStatus.Status == StationStatus.Pass ? 1 : 0,
            //                                   currentProduct.Status.ReworkCode,
            //                                   newStatus.Line,
            //                                   newStatus.TestFailCount,
            //                                   newStatus.Editor,
            //                                   //newStatus.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
            //                                   newStatus.Udt
            //                                   );

            //SqlParameter para1 =new SqlParameter("PreStatus", System.Data.SqlDbType.Structured);
            //para1.Direction = System.Data.ParameterDirection.Input;
            //para1.Value = preStatus;

            //SqlParameter para2 =new SqlParameter("Status", System.Data.SqlDbType.Structured);
            //para2.Direction = System.Data.ParameterDirection.Input;
            //para2.Value = curStatus;

            
            //productRepository.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,
            //                                                                                 IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA,
            //                                                                                 "IMES_UpdateProductStatus",
            //                                                                                para1,
            //                                                                                para2);

            IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(new List<string>{currentProduct.ProId});
            productRepository.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, stationList);
                                                                                             
            #endregion

            currentProduct.UpdateStatus(newStatus);
            
            //070031 2012.09.18
            //Vincent disable 檢查重複BoxId功能，在分配時BoxId 已做檢查
            //0001649: [Activity]修改UpdateProductStatus activity 刪除檢查重複Boxd 問題
            //var ivkBdy = productRepository.CheckTheBoxIdDefered(CurrentSession.UnitOfWork, currentProduct);
            //070031 2012.09.18

            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            //070031 2012.09.18
            //Vincent disable 檢查重複BoxId功能，在分配時BoxId 已做檢查
            //0001649: [Activity]修改UpdateProductStatus activity 刪除檢查重複Boxd 問題
            //var ivkBdy2 = productRepository.CheckTheBoxIdDefered(CurrentSession.UnitOfWork, currentProduct);
            //ivkBdy2.DependencyIvkbdy = ivkBdy;
            //ivkBdy2.ExpectRetVal = true;
            //070031 2012.09.18

            return base.DoExecute(executionContext);
        }

       
    }
}
