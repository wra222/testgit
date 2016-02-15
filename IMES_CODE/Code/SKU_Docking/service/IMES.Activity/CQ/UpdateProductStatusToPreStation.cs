// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新ProductStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincent                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;

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
    public partial class UpdateProductStatusToPreStation : BaseActivity
    {
        
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateProductStatusToPreStation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", 
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(UpdateProductStatusToPreStation),
                                                                                                                                    new PropertyMetadata(false));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductStatusToPreStation.IsSingleProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusToPreStation.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// Write Relase Code
        /// </summary>
        public static DependencyProperty IsReleaseHoldProperty = DependencyProperty.Register("IsReleaseHold",
                                                                                                                                    typeof(bool),
                                                                                                                                    typeof(UpdateProductStatusToPreStation),
                                                                                                                                    new PropertyMetadata(true));

        /// <summary>
        /// Release Hold Code
        /// </summary>
        [DescriptionAttribute("IsReleaseHold")]
        [CategoryAttribute("IsReleaseHold Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsReleaseHold
        {
            get
            {
                return ((bool)(base.GetValue(UpdateProductStatusToPreStation.IsReleaseHoldProperty)));
            }
            set
            {
                base.SetValue(UpdateProductStatusToPreStation.IsReleaseHoldProperty, value);
            }
        }


        /// <summary>
        /// Update Product Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> prodIDList = new List<string>();
            if (this.IsSingle)
            {
                IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct == null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.Product });
                }
                prodIDList.Add(currentProduct.ProId);
            }
            else
            {
               prodIDList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
                if (prodIDList == null || prodIDList.Count == 0)
                {
                    IList<IProduct> productList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                    if (productList == null || productList.Count == 0)
                    {
                        throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.ProdList + " and " + Session.SessionKeys.NewScanedProductIDList });
                    }
                    prodIDList = (from p in productList
                                  select p.ProId).ToList();                   
                }
                
            }


            IList<TbProductStatus> productStatusList = prodRep.GetProductStatus(prodIDList);
            prodRep.UpdateStationToPreStationDefered(CurrentSession.UnitOfWork, prodIDList, this.Editor);
            prodRep.WriteProdLogByPreStationDefered(CurrentSession.UnitOfWork, prodIDList, this.Editor);
            prodRep.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, productStatusList);

            if (this.IsReleaseHold)
            {
                IList<HoldInfo> holdInfoList = ( IList<HoldInfo> )CurrentSession.GetValue(ExtendSession.SessionKeys.ProdHoldInfoList);
                if (holdInfoList == null || holdInfoList.Count==0)
                {
                    throw new FisException("CQCHK0006", new string[] { ExtendSession.SessionKeys.ProdHoldInfoList });
                }

                string  releaseCode = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.ReleaseCode);
                if (string.IsNullOrEmpty(releaseCode))
                {
                    throw new FisException("CQCHK0006", new string[] { ExtendSession.SessionKeys.ReleaseCode });
                }

                prodRep.ReleaseHoldProductIDDefered(CurrentSession.UnitOfWork, holdInfoList, releaseCode, this.Editor);
            }

            return base.DoExecute(executionContext);
        }

       
    }
}
