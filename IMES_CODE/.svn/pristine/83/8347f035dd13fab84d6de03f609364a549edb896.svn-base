// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判定PartNo是否存在于Part数据表的PartNo栏位中
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-11   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{

    /// <summary>
    /// 判定PartNo是否存在于Part数据表的PartNo栏位中 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-CN Card Receive
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.该PartNo不存在于Part数据表的PartNo栏位中，提示PN not exist！
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK012
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PartNo
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPart
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckPNValid : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPNValid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所输PartNo不存在于Part数据表的PartNo栏位中，提示PN not exist！
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得PartNo
            string currentPN = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart currentPart = currentRepository.Find(currentPN);
            if (currentPart == null)
            {
                throw new FisException("PAK012", new string[] { currentPN });
            }                
            return base.DoExecute(executionContext);
        }

    }
}

