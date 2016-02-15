// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的productID,BoxID or UCC 检查BoxID or UCC，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-05   Chen Xu (itc208014)          create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的productID,BoxID or UCC 检查BoxID or UCC，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据输入的productID,BoxID or UCC 检查BoxID or UCC，并放到Session中
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
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class CheckBoxID : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckBoxID()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string prod = currentProduct.ProId;
            IProductRepository iproductRepository =RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string boxid = (string)CurrentSession.GetValue(Session.SessionKeys.boxId);
            string infovalue = string.Empty;
            infovalue = iproductRepository.GetProductInfoValue(prod, "BoxId");
            if (string.IsNullOrEmpty(infovalue))
            {
                infovalue = iproductRepository.GetProductInfoValue(prod, "UCC");
                if (string.IsNullOrEmpty(infovalue))
                {
                    FisException fe = new FisException("PAK034", new string[] { });   //此机器无BoxId！请返回Ship To Label 确认！
                    fe.stopWF = false;
                    throw fe;
                }
            }

            if (infovalue != boxid)
            {
                FisException fe = new FisException("PAK035", new string[] { });   //Box ID / UCC 与机器不匹配！
                fe.stopWF = false;
                throw fe;
            }
	        return base.DoExecute(executionContext);
        }
	}
}
