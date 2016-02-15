// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的UpdateProductList中的ProductID列表，
//              插入列表中所有Product的Log      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using log4net;
namespace IMES.Activity
{
    /// <summary>
    /// 用于更新一组Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PAK Pallet Data Collection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取NewScanedProductIDList，调用ProductRepository.UpdateProductListStatus
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.NewScanedProductIDList
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
    ///         记录ProductLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              ProductLog
    /// </para> 
    /// </remarks>
    public partial class WriteProductListLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteProductListLog()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WriteProductListLog));

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WriteProductListLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteProductListLog.StatusProperty, value);
            }
        }

        /// <summary>
        /// 用于更新一组Product的状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error("------------------ begin WriteProductListLog ------------------------");
            IList<ProductLog> logList = new List<ProductLog>();

            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            IList<string> ProductModelList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductModelList);
            IList<string> ProductLineList = new List<string>();
            if (string.IsNullOrEmpty(Line))
            {
                ProductLineList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductLineList);
            }
            
            if (ProductIDList != null && ProductModelList != null)
            {
                for (int i = 0; i < ProductIDList.Count; i++)
                {
                    ProductLog newLog = new ProductLog();
                    newLog.Editor = Editor;
                    if (string.IsNullOrEmpty(Line) && ProductLineList != null)
                    {
                        newLog.Line = ProductLineList[i];
                    }
                    else
                    {
                        newLog.Line = Line;
                    }
                    newLog.Station = Station;
                    newLog.Status = Status;
                    newLog.ProductID = ProductIDList[i];
                    newLog.Model = ProductModelList[i];
                    logList.Add(newLog);
                }
                string[] strs = ProductIDList.ToArray();
                string ret = string.Format("'{0}'", string.Join("','", strs));
                logger.Error("------------------ ProductIDList= " + ret + "------------------------");
            }

            ProductRepository.WriteProductListLogDefered(CurrentSession.UnitOfWork, logList);
            logger.Error("------------------ end WriteProductListLog ------------------------");
            return base.DoExecute(executionContext);
        }
    }
}
