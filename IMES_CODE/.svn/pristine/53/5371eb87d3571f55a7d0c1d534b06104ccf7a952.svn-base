// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的ProductID,获取Product对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-02-15   Vincenti                 create
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的ProductID,抓取HoldInfo，并放到Session中
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
    ///         1.根据ProductID，调用IProductRepository的Find方法，获取Product对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：SFC002
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class WriteMultiProductLog : BaseActivity
    {
        ///<summary>
        ///</summary>
        public WriteMultiProductLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           
            var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                       

            IList<string> keyValue = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            if (keyValue == null || keyValue.Count == 0)
            {
                throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.NewScanedProductIDList });
            }
            productRep.WriteProductLogDefered(CurrentSession.UnitOfWork, keyValue, this.Line, this.Station, (int)this.Status, this.Editor);
            
            return base.DoExecute(executionContext);
        }



        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteMultiProductLog), new PropertyMetadata(StationStatus.Pass));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteMultiProductLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteMultiProductLog.StatusProperty, value);
            }
        }
    }
}
