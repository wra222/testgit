using System;
using System.Collections.Generic;
// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  检查在IMES_FA..Product_Part 表中没有Office COA绑定记录
// UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-05   Chen Xu (itc208014)          create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// 如果ModelBOM 中存在BomNodeType 为'P1'，属性Descr like 'OFFIC%' 的Part 时，如果在IMES_FA..Product_Part 表中没有绑定记录的时候，需要报告错误：“尚未结合Office COA!”
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
    ///         1.如果ModelBOM 中存在BomNodeType 为'P1'，属性Descr like 'OFFIC%' 的Part 时，如果在IMES_FA..Product_Part 表中没有绑定记录的时候，需要报告错误：“尚未结合Office COA!”
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：如果在IMES_FA..Product_Part 表中没有绑定记录的时候，需要报告错误：“尚未结合Office COA!”
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
    public partial class CheckCOA : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckCOA()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check COA
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //2.	如果ModelBOM 中存在BomNodeType 为'P1'，属性Descr like 'OFFIC%' 的Part 时，如果在IMES_FA..Product_Part 表中没有绑定记录的时候，需要报告错误：“尚未结合Office COA!”
           
            // Revision 9817: 修改检查Office COA 是否结合是检查Pizza_Part:
            //      如果ModelBOM 中存在BomNodeType 为'P1'，属性Part.Descr like 'OFFIC%' 的Part 时，如果在IMES_FA..Pizza_Part 表中没有绑定记录的时候
            //      (基于Product 找到与Product 绑定的1st Pizza ID（Product.PizzaID），然后再使用PizzaID=@1stPizzaID查询Pizza_Part 表，其中BomNodeType = 'P1', 
            //      属性Part.Descr like 'OFFIC%' 的记录，为Office COA 绑定记录)，需要报告错误：“尚未结合Office COA!”

            FisException ex;
            List<string> erpara = new List<string>();

            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string partno = string.Empty;
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            if (bomNodeLst == null || bomNodeLst.Count <= 0)
            {
                erpara.Add(currentProduct.Model);
                ex = new FisException("PAK039", erpara);
                throw ex;
            }
            Boolean flag = false;
            foreach (IBOMNode ibomnode in bomNodeLst)
            {
                IPart bomPart = ibomnode.Part;
                if (bomPart.BOMNodeType == "P1" && bomPart.Descr.Contains("OFFIC"))
                {
                    partno = bomPart.PN;
                    flag = true;  // 存在
                    break;
                }
            }

            // ITC-1360-1291: 如果ModelBOM 中存在BomNodeType 为'P1'，属性Part.Descr like 'OFFIC%' 的Part 时，
            //                如果在IMES_FA..Product_Part 表中没有绑定记录的时候，需要报告错误：“尚未结合Office COA!”
            
            if (flag)
            {
                //IList<IProductPart> productParts = new List<IProductPart>();
                //productParts = currentProduct.ProductParts;
                //if (productParts != null && productParts.Count > 0)
                //{
                //    foreach (ProductPart iProPart in productParts)
                //    {
                //        if (iProPart.PartID == partno)
                //        {
                //            flag = false;   // 有绑定记录
                //            break;
                //        }
                //    }
                //    if (flag) //没有绑定记录
                //    {
                //        erpara.Add(currentProduct.ProId);
                //        ex = new FisException("PAK037", erpara);  //该Product %1 尚未结合Office COA！
                //        throw ex;
                //    }
                //}

                // Revision 9817: 修改检查Office COA 是否结合是检查Pizza_Part:
                //      如果ModelBOM 中存在BomNodeType 为'P1'，属性Part.Descr like 'OFFIC%' 的Part 时，如果在IMES_FA..Pizza_Part 表中没有绑定记录的时候
                //      (基于Product 找到与Product 绑定的1st Pizza ID（Product.PizzaID），然后再使用PizzaID=@1stPizzaID查询Pizza_Part 表，其中BomNodeType = 'P1', 
                //      属性Part.Descr like 'OFFIC%' 的记录，为Office COA 绑定记录)，需要报告错误：“尚未结合Office COA!”
               
                Pizza pizzaobj = currentProduct.PizzaObj;
                if (pizzaobj==null || pizzaobj.PizzaParts==null )
                {
                    FisException fe = new FisException("CHK851", new string[] {});  //错误的Pizza ID!
                    throw fe;
                }

                IList<IProductPart> productParts = currentProduct.PizzaObj.PizzaParts;

                
                if (productParts != null && productParts.Count > 0)
                {
                    foreach (ProductPart iProPart in productParts)
                    {
                        if (iProPart.PartID == partno && iProPart.BomNodeType=="P1")
                        {
                            
                            flag = false;   // 有绑定记录
                            break;
                        }
                    }
                    
                }
                if (flag) //没有绑定记录
                {
                    erpara.Add(currentProduct.ProId);
                    ex = new FisException("PAK037", erpara);  //该Product %1 尚未结合Office COA！
                    throw ex;
                }
            }
                
	        return base.DoExecute(executionContext);
        }
	}
}
