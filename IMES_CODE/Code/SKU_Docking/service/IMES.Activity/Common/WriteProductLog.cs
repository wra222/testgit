// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到ProductLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;

namespace IMES.Activity
{
    /// <summary>
    /// 用于记录到Product Log
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
    ///         1.从Session中获取Product，调用Product的AddLog方法
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
    ///         更新ProductLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    ///              ProductLog
    /// </para> 
    /// </remarks>
    public partial class WriteProductLog : BaseActivity
    {
        //private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteProductLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteProductLog));

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
                return ((StationStatus)(base.GetValue(WriteProductLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteProductLog.StatusProperty, value);
            }
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(WriteProductLog));

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
                return ((bool)(base.GetValue(WriteProductLog.IsSingleProperty)));
            }
            set
            {
                base.SetValue(WriteProductLog.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// Wrint Product Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //if (Editor.Trim() == "")
            //    logger.Error("Editor from activity is empty!");

            string line = default(string);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (IsSingle)
            {
                var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (product == null)
                {
                    throw new NullReferenceException("Product in session is null");
                }

                line = string.IsNullOrEmpty(this.Line) ? product.Status.Line : this.Line;

                var productLog = new ProductLog
                {
                    Model = product.Model,
                    Status = Status,
                    Editor = Editor,
                    Line = line,
                    Station = Station,
                    Cdt = DateTime.Now
                };

                product.AddLog(productLog);
                productRepository.Update(product, CurrentSession.UnitOfWork);
            }
            else
            {
                var productList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as List<IProduct>;
                if (productList == null)
                {
                    throw new NullReferenceException("ProdList in session is null");
                }
                foreach (var product in productList)
                {
                    line = string.IsNullOrEmpty(this.Line) ? product.Status.Line : this.Line;
                    var productLog = new ProductLog
                    {
                        Model = product.Model,
                        Status = Status,
                        Editor = Editor,
                        Line = line,
                        Station = Station,
                        Cdt = DateTime.Now
                    };

                    product.AddLog(productLog);
                    productRepository.Update(product, CurrentSession.UnitOfWork);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
