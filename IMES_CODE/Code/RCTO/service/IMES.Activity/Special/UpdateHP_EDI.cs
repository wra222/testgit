// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  如果是BT 产品，需要记录PAK_PQCLog
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
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System;

namespace IMES.Activity
{
    /// <summary>
    /// 更新HP EDI 数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unit Weight.docx 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.更新HP EDI 数据
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
    ///              ProId,model(机型12码)
    /// </para> 
    /// </remarks>
    public partial class UpdateHP_EDI : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateHP_EDI()
		{
			InitializeComponent();
		}

        /// <summary>
        /// WritePQCLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            // 更新HP EDI 数据:
            // DELETE HP_EDI.dbo.PAK_SkuMasterWeight_FIS WHERE Model = RTRIM(@model)
            // INSERT INTO HP_EDI.dbo.PAK_SkuMasterWeight_FIS 
            // VALUES(RTRIM(@model),CONVERT(decimal(10,2),@weight),GETDATE())
            IPizzaRepository ipizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            //ITC-1360-1299: 如果Product Model 的UnitWeight (IMES_GetData..ModelWeight)属性，没有新Insert或Update时，不需要更新（Insert）HP EDI 数据（PAK_SkuMasterWeight_FIS）
            Boolean Updateflg=(Boolean)CurrentSession.GetValue(Session.SessionKeys.IsCheckPass);
           
            if (Updateflg)
            {
                ipizzaRepository.DeletetPakSkuMasterWeightFisByModelDefered(CurrentSession.UnitOfWork, currentProduct.Model);

                PakSkuMasterWeightFisInfo pakSMWFInfo = new PakSkuMasterWeightFisInfo();
                pakSMWFInfo.model = currentProduct.Model;
                pakSMWFInfo.weight = (decimal)CurrentSession.GetValue("SetWeight");
                pakSMWFInfo.cdt = DateTime.Now;
                ipizzaRepository.InsertPakSkuMasterWeightFisDefered(CurrentSession.UnitOfWork, pakSMWFInfo);
            }
           
            return base.DoExecute(executionContext);
         }
        
	}
}
