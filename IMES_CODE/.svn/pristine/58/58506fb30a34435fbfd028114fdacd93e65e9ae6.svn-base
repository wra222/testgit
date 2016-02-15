using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
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
    ///         Session.DeliveryNo
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnpackOSCOA : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnpackOSCOA()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product productPartOwner = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ProId = productPartOwner.ProId;

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            IList<ProductPart> part_P1 = currentProductRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, "P1", "COA");
            if (part_P1 != null && part_P1.Count > 0)
            {
                currentProductRepository.BackUpProductPartByBomNodeTypeAndDescrLike(ProId, this.Editor, "P1", "COA%");
                currentProductRepository.RemoveProductPartByBomNodeTypeAndDescrLike(ProId, "P1", "COA%");
                currentProductRepository.Update(productPartOwner, CurrentSession.UnitOfWork);
            }



            return base.DoExecute(executionContext);
        }
    }
}
