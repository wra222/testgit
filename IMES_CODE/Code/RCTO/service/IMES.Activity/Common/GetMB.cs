// INVENTEC corporation (c)201all rights reserved. 
// Description:  获取MBNO属性指定的MB对象放入Session.MB
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-06   YangWeihua              create
// 2010-1-12  fix bug ITC-1103-0008
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 获取MBNO属性指定的MB对象放入Session.MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为操作主线的workflow
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据MBNO属性获取IMB对象;
    ///         2.将IMB对象放入Session.MB;
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
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GetMB : BaseActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetMB()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get MB Object and Put it into Session.SessionKeys.MB
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbSn;
            if (string.IsNullOrEmpty(this.MBSnSessionKey))
            {
                mbSn = this.Key;
            }
            else
            {
                mbSn = CurrentSession.GetValue(this.MBSnSessionKey) as string;
            }

            if(mbSn ==null){
                throw new Exception("Please check your workflow whether bind correct value for GetMB activity!");
            }

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var mb = mbRepository.Find(mbSn);
            if (mb == null)
            {
                if (string.IsNullOrEmpty(NotExistException))
                {
                    NotExistException = "SFC001";
                }
                var ex = new FisException(NotExistException, new string[] { mbSn });
                throw ex;
            } else if (mb.MBStatus == null)
            {
                if (string.IsNullOrEmpty(NotExistException))
                {
                    NotExistException = "SFC001";
                }
                var ex = new FisException(NotExistException, new string[] { mbSn });
                throw ex;
            }

            CurrentSession.AddValue(Session.SessionKeys.MB, mb);

            if (this.Line == null)
            {
                this.Line = mb.MBStatus.Line;
            }
            return base.DoExecute(executionContext);
        }

        ///<summary>
        /// 在Session中存放MBSn的键值，按此键值获取mbsn， 此键值为空则使用this.key中的mbsn
        ///</summary>
        public static DependencyProperty MBSnSessionKeyProperty = DependencyProperty.Register("MBSnSessionKey", typeof(string), typeof(GetMB));

        /// <summary>
        /// 在Session中存放MBSn的键值，按此键值获取mbsn， 此键值为空则使用this.key中的mbsn
        /// </summary>
        [DescriptionAttribute("MBSnSessionKey")]
        [CategoryAttribute("InArguments Of GetMB")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string MBSnSessionKey
        {
            get
            {
                return ((string)(base.GetValue(GetMB.MBSnSessionKeyProperty)));
            }
            set
            {
                base.SetValue(GetMB.MBSnSessionKeyProperty, value);
            }
        }

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC001
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(string), typeof(GetMB));

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC001
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetMB")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NotExistException
        {
            get
            {
                return ((string)(base.GetValue(GetMB.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetMB.NotExistExceptionProperty, value);
            }
        }

    }
}
