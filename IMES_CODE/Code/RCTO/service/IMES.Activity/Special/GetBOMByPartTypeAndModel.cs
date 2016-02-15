/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:GetBOMByPartTypeAndModel
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// GetBOMByPartTypeAndModel
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
    ///     根据Model得到此unit选定的Keyparts的part BOM
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
    public partial class GetBOMByPartTypeAndModel : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetBOMByPartTypeAndModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// GetBOMByPartTypeAndModel
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));            
            string kpType = CurrentSession.GetValue(Session.SessionKeys.KPType) as string;

            IFlatBOM sessionBOM = null;
            var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            sessionBOM = bomRepository.GetFlatBOMByPartTypeModel(kpType, currentProduct.Model, this.Station, currentProduct);
            
            CurrentSession.AddValue(Session.SessionKeys.SessionBom, sessionBOM);
            
            return base.DoExecute(executionContext);
        }
    }
}
