/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Activity for SN Check Page
 * UI:CI-MES12-SPEC-PAK-UI SN Check.docx –2011/10/20 
 * UC:CI-MES12-SPEC-PAK-UC SN Check.docx –2011/10/20            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-20   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：判断是否需要检查Asset Tag SN Check：	Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”得到的Tp=”K”的记录时，表示需要检查Asset Tag SN Check是否做过 (目前只考虑得到一条记录的情况??)-数据接口尚未定义（in Activity：CheckAssetTagSN）
* UC 具体业务：当BOM(存在PartType=ALC and BomNodeType=PL的part) 且model<>PC4941AAAAAY时，表示有ALC，这时没有真正的Pizza盒-数据接口尚未定义（in Activity：CheckSNIdentical）
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
//2011-12-15
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
using System.Linq;  

namespace IMES.Activity
{
    /// <summary>
    /// Check [Customer S/N] on Product 与Input [Customer S/N] on Pizza 是否一致
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA站Offline print CT, FRU IECSNO 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1. 检查输入的SN后10位与第一次刷入的相同。
    ///         2. 检查第一位等于P或A
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.custsn
    ///         Session.SessionKeys.Product
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
    ///         IBOMRepository
    ///         IHierarchicalBOM
    /// </para> 
    /// </remarks>
	public partial class CheckSNIdentical: BaseActivity
	{
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSNIdentical()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check [Customer S/N] on Product 与Input [Customer S/N] on Pizza 是否一致
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            string customerSNOnProduct = this.Key.Trim();
            string customerSNOnPizza = ((string)CurrentSession.GetValue(Session.SessionKeys.CustSN)).Trim();   
            //2011-12-15
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            if ((customerSNOnPizza.Length >= 10) && (customerSNOnProduct.Length >= 10))
            {
                string SNOnPizzaLastTen = customerSNOnPizza.Substring(customerSNOnPizza.Length - 10, 10);
                string SNOnProductLastTen = customerSNOnProduct.Substring(customerSNOnProduct.Length - 10, 10);

                if (SNOnPizzaLastTen != SNOnProductLastTen)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(this.Key);

                    throw new FisException("CHK207", errpara);
                }
            }
            else
            {
                List<string> errpara = new List<string>();

                errpara.Add(this.Key);

                throw new FisException("CHK207", errpara);
            }


            var hasALC = false;

            if (currentProduct.Model != "PC4941AAAAAY")
            {
                IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                    if ((part.Descr == "ALC") && (part.BOMNodeType == "PL"))
                    {
                        hasALC = true;

                        break;
                    }
                }
            }
 

			if (hasALC == true)
			{
				if (customerSNOnPizza.Substring(0, 1) != "A")
				{
					List<string> errpara = new List<string>();

					errpara.Add(this.Key);

					throw new FisException("CHK207", errpara);  
				}
			}
			else
			{
				if (customerSNOnPizza.Substring(0, 1) != "P")
				{
					List<string> errpara = new List<string>();

					errpara.Add(this.Key);

					throw new FisException("CHK207", errpara);  
				}
			}

            return base.DoExecute(executionContext);
            
        }
	}
}
