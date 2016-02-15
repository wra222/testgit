// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据productId,station获取ProductLog对象
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-16   Chen Xu  itc208014           create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    ///   根据productId,station获取ProductLog对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于检查是否有过站log
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：
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
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class GetProductLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetProductLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前站名描述 StationDescr
        /// </summary>
        public static DependencyProperty StationDescrProperty = DependencyProperty.Register("StationDescr", typeof(String), typeof(GetProductLog));

        /// <summary>
        /// StationDescr
        /// </summary>
        [DescriptionAttribute("StationDescr")]
        [CategoryAttribute("StationDescr Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public String StationDescr
        {
            get
            {
                return ((String)(base.GetValue(GetProductLog.StationDescrProperty)));
            }
            set
            {
                base.SetValue(GetProductLog.StationDescrProperty, value);
            }
        }

        /// <summary>
        /// 根据productId,station获取ProductLog对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            IList<ProductLog> prodLogLst = new List<ProductLog>();
            IList<ProductLog> prodPassLogLst = new List<ProductLog>();

            prodLogLst = iproductRepository.GetProductLogs(currentProduct.ProId, this.Station);
            if (prodLogLst == null || prodLogLst.Count<=0)
            {
                 erpara.Add(currentProduct.ProId);
                 ex = new FisException("SFC002", erpara);    //Product 序号 不存在！
                 throw ex;
            }

            foreach (ProductLog iprodlog in prodLogLst)
            {
                if (iprodlog.Status == StationStatus.Pass)
                {
                    prodPassLogLst.Add(iprodlog);
                }
            }

            string descr = string.Empty;
            if (!String.IsNullOrEmpty(StationDescr))
            {
                descr = StationDescr;
            }
            else
            {
                descr = this.Station.ToString();
            }
            if (prodPassLogLst == null || prodPassLogLst.Count<=0)
            {
                 erpara.Add(currentProduct.ProId);
                 erpara.Add(descr);
                 ex = new FisException("SFC015", erpara);    //没有该Product 的Pass 站的记录！
                 throw ex;
            }

            
            CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, true);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogList, prodLogLst);
            
            return base.DoExecute(executionContext);
        }

        
    }
}
