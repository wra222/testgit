// INVENTEC corporation (c)2012 all rights reserved. 
// Description: 根据IEC CTSN获取PartSN对象并放到Session中
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-11   Yuan XiaoWei                 create
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

namespace IMES.Activity
{
    /// <summary>
    /// 根据IEC CT SN获取PartSN对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-SA-UI MB Borrow Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK027
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         IEC CT SN是Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PartSN
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         无
    /// </para> 
    /// </remarks>
    public partial class GetPartSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetPartSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据IEC CTSN获取PartSN对象并放到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            PartSn currentPartSN = currentPartSNRepository.Find(Key);
            if (currentPartSN == null)
            {
                if (Key.Length >= 13)
                {
                    currentPartSN = currentPartSNRepository.Find(Key.Substring(0, 13));
                }
            }

            if (currentPartSN == null)
            {
                if (string.IsNullOrEmpty(NotExistException))
                {
                    NotExistException = "CHK027";
                }
                var ex = new FisException(NotExistException, new string[] { Key });
                throw ex;
            }

            CurrentSession.AddValue(Session.SessionKeys.PartSN, currentPartSN);

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入序号找不到Part报错信息的ErroCode,不填则默认报错CHK027
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(string), typeof(GetPartSN));

        /// <summary>
        /// 输入序号找不到Part报错信息的ErroCode,不填则默认报错CHK027
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetPartSN")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NotExistException
        {
            get
            {
                return ((string)(base.GetValue(GetPartSN.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetPartSN.NotExistExceptionProperty, value);
            }
        }
    }
}
