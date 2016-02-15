// INVENTEC corporation (c)2011 all rights reserved. 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-27   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.Common.Line;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 请参考.\Common\Common Rule.docx 文档中的相关描述.没找到相应的文档。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class GetLine : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GetLine()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.DNInfoValue
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //////logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            IList<string> lines = lineRepository.GetLineByCustAndStage(this.CustomerID,this.Stage);
            if (null == lines || lines.Count == 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }
            CurrentSession.AddValue(Session.SessionKeys.Lines, lines);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty StageProperty = DependencyProperty.Register("Stage", typeof(string), typeof(GetLine));
        /// <summary>
        /// Stage
        /// </summary>
        [DescriptionAttribute("Stage")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Stage
        {
            get
            {
                return ((string)(base.GetValue(GetLine.StageProperty)));
            }
            set
            {
                base.SetValue(GetLine.StageProperty, value);
            }
        }
        /// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty CustomerIDProperty = DependencyProperty.Register("CustomerID", typeof(string), typeof(GetLine));
        /// <summary>
        /// InfoType
        /// </summary>
        [DescriptionAttribute("CustomerID")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CustomerID
        {
            get
            {
                return ((string)(base.GetValue(GetLine.CustomerIDProperty)));
            }
            set
            {
                base.SetValue(GetLine.CustomerIDProperty, value);
            }
        }
	}
}
