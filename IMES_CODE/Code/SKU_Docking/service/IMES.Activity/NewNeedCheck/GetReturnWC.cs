/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Get return wc of product.
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Activity
{
    /// <summary>
    /// Get return wc of product.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Key Parts.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///    详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class GetReturnWC : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetReturnWC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get return wc of product.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            string retWC = "";
            IList<ProductLog> logList1 = productRepository.GetProductLogs(currentProduct.ProId, "59");
            IList<ProductLog> logList2 = productRepository.GetProductLogs(currentProduct.ProId, "50");
            
            /*
             * Answer to: ITC-1360-0559
             * Description: Get ReturnWC according to new UC.
            */
            if (logList1 != null && logList1.Count != 0)
            {
                retWC = "59";
            }
            else if (logList2 == null || logList2.Count == 0)
            {
                retWC = currentProduct.Status.StationId;
            }
            else
            {
                IModelRepository modelRepository =  RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                string propSTAG = modelRepository.Find(currentProduct.Model).GetAttribute("STAG");
                if (propSTAG == "S" || propSTAG == "T")   //需要打印Master Label
                {
                    retWC = "59";
                }
                else
                {
                    retWC = "50";
                }
            }

            CurrentSession.AddValue(Session.SessionKeys.ReturnStation, retWC);

            return base.DoExecute(executionContext);
        }
    }
}
