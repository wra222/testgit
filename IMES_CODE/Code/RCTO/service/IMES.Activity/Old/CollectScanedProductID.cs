// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 应用于045,053,054三站，用于收集扫描比检查通过的ProductID
//                  
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-28   Yuan XiaoWei                 create
// 2010-01-28   Yuan XiaoWei                 ITC-1155-0061
// 2011-03-16   Lucy Liu                     Modify:增加NewScanedProductCustSNList的收集

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
using log4net;

namespace IMES.Activity
{
    /// <summary>
    ///  应用于045,053,054三站，用于收集扫描比检查通过的ProductID
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于045,053,054三站，用于收集扫描比检查通过的ProductID
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取刚扫描的Product
    ///         2.将其ProductID保存到NewScanedProductIDList
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
    ///         Session.NewScanedProductIDList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class CollectScanedProductID : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>

        public CollectScanedProductID()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 执行添加ProductID到NewScanedProductIDList列表的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error("------------------ begin CollectScanedProductID ------------------------");
            List<string> ProductIDList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            List<string> ProductModelList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductModelList);
            List<string> ProductLineList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductLineList);
            List<string> ProductCustSNList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductCustSNList);
            if (ProductIDList == null)
            {
                ProductIDList = new List<string>();
                ProductModelList = new List<string>();
                ProductLineList = new List<string>();
                ProductCustSNList = new List<string>();
            }
            if (!ProductIDList.Contains(CurrentProduct.ProId) && BindCacheManager.Add(CurrentProduct.ProId, base.WorkflowInstanceId.ToString()))
            {
                ProductIDList.Add(CurrentProduct.ProId);
                ProductModelList.Add(CurrentProduct.Model);
                ProductLineList.Add(string.IsNullOrEmpty(this.Line) ? CurrentProduct.Status.Line : this.Line);
                ProductCustSNList.Add(CurrentProduct.CUSTSN);
                logger.Error("------------------ CurrentProduct.ProId=" + CurrentProduct.ProId + "------------------------");
                logger.Error("------------------ CurrentProduct.CUSTSN=" + CurrentProduct.CUSTSN + "------------------------");
            }
            else
            {
                FisException fe = new FisException("CHK088", new string[] { CurrentProduct.ProId });
                if (!IsStopWF)
                {
                    fe.stopWF = false;
                }
                throw fe;
            }
            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, ProductIDList);
            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductModelList, ProductModelList);
            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductLineList, ProductLineList);
            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductCustSNList, ProductCustSNList);
            logger.Error("------------------ end CollectScanedProductID ------------------------");
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CollectScanedProductID));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CollectScanedProductID.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CollectScanedProductID.IsStopWFProperty, value);
            }
        }
    }
}
