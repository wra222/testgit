// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到ProductLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-2-2    DuXuan                       create]
// ITC-1360-0774 修改dbScope属性，写入productLog
// ITC-1360-1199 增加assetcheck判断
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
    public partial class WriteContentWarrantyLog : BaseActivity
    {
        private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteContentWarrantyLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteContentWarrantyLog));

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
                return ((StationStatus)(base.GetValue(WriteContentWarrantyLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteContentWarrantyLog.StatusProperty, value);
            }
        }

       

        /// <summary>
        /// Wrint Product Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (Editor.Trim() == "")
                logger.Error("Editor from activity is empty!");

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            Boolean confFlag = (Boolean)CurrentSession.GetValue("ConfigPrint");
            
            Boolean WarrantyPrint = (Boolean)CurrentSession.GetValue("WarrantyPrint");

            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            
            //Check Asset SN：
            //Insert ProductLog: Station=SC,Line=PAK,Status=1

            string assetcheck = (string)CurrentSession.GetValue("AssetCheck");
            var assetProductLog = new ProductLog
            {
                Model = product.Model,
                Status = Status,
                Editor = Editor,
                Line = "PAK",
                Station = "SC",
                Cdt = DateTime.Now
            };
            if (assetcheck != "")
            {
                product.AddLog(assetProductLog);
                productRepository.Update(product, CurrentSession.UnitOfWork);
            }

            //打印Warranty Label：
            //Insert ProductLog: Station=8D, Line= WarrantyCardPrint, Status=1
            var warProductLog = new ProductLog
            {
                Model = product.Model,
                Status = Status,
                Editor = Editor,
                Line = "WarrantyCardPrint",
                Station = "8D",
                Cdt = DateTime.Now
            };
            if (WarrantyPrint)
            {
                product.AddLog(warProductLog);
                productRepository.Update(product, CurrentSession.UnitOfWork);
            }
           
            //打印CFG Label：
            //Insert ProductLog: Station=8D, Line= PAK, Status=1
            var cfgProductLog = new ProductLog
            {
                Model = product.Model,
                Status = Status,
                Editor = Editor,
                Line = "PAK",
                Station = "8D",
                Cdt = DateTime.Now
            };
            if (confFlag)
            {
                product.AddLog(cfgProductLog);
                productRepository.Update(product, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}
