// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Insert PCBInfo
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-28   Kerwin                       create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Insert PCBInfo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPECFA-UC Online Generate AST 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.InfoValue
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
    ///         PCBInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         MB
    /// </para> 
    /// </remarks>
    public partial class WriteMBInfo : BaseActivity
    {

        /// <summary>
        ///  InfoType of PCBInfo
        /// </summary>
        public static DependencyProperty InfoTypeProperty = DependencyProperty.Register("InfoType", typeof(string), typeof(WriteMBInfo));

        /// <summary>
        /// InfoType of PCBInfo
        /// </summary>
        [DescriptionAttribute("InfoTypeProperty")]
        [CategoryAttribute("InArguments Of WriteMBInfo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string InfoType
        {
            get
            {
                return ((string)(base.GetValue(InfoTypeProperty)));
            }
            set
            {
                base.SetValue(InfoTypeProperty, value);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public WriteMBInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get MB,InfoValue from Session,Insert into PCBInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentMb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            string infoValue = CurrentSession.GetValue(Session.SessionKeys.InfoValue) as string;
            if (!string.IsNullOrEmpty(InfoValueSessionKey))
            {
                infoValue = CurrentSession.GetValue(InfoValueSessionKey) as string;
            }
            currentMb.SetExtendedProperty(InfoType, infoValue, CurrentSession.Editor);
            var repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            repository.Update(currentMb, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  用那个sesionkey来获取InfoValue的值,不填默认从Session.SessionKeys.InfoValue里获取
        /// </summary>
        public static DependencyProperty InfoValueSessionKeyProperty = DependencyProperty.Register("InfoValueSessionKey", typeof(string), typeof(WriteMBInfo));

        /// <summary>
        /// 用那个sesionkey来获取InfoValue的值,不填默认从Session.SessionKeys.InfoValue里获取
        /// </summary>
        [DescriptionAttribute("InfoValueSessionKeyProperty")]
        [CategoryAttribute("InArguments Of WriteMBInfo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string InfoValueSessionKey
        {
            get
            {
                return ((string)(base.GetValue(InfoValueSessionKeyProperty)));
            }
            set
            {
                base.SetValue(InfoValueSessionKeyProperty, value);
            }
        }
    }
}
