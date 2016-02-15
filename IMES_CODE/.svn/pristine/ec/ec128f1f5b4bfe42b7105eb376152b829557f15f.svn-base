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
    public partial class CheckCOAForUnpackAllByDN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckCOAForUnpackAllByDN()
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
            Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<ProductPart> partLst = new List<ProductPart>();
            partLst = productRepository.GetProductPartByBomNodeTypeAndDescrLike(newProduct.ProId, "P1", "COA");
            if (partLst != null && partLst.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.COASN, partLst[0].PartSn);
                CurrentSession.AddValue(Session.SessionKeys.ifElseBranch, 1);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.ifElseBranch, 0);
            }


            
            return base.DoExecute(executionContext);
        }
    }
}
