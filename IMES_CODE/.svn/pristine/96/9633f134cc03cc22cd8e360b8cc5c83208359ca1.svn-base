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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using System;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 如果是BT 产品，需要记录PAK_PQCLog
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
    ///         1.如果是BT 产品，需要记录PAK_PQCLog
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
    public partial class WritePQCLog : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WritePQCLog()
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
            //3.	如果是BT 产品，需要记录PAK_PQCLog
            //参考方法：INSERT PAK_PQCLog VALUES (@snoid,@model,'PAK','81',GETDATE())
            IPizzaRepository ipizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (currentProduct.IsBT) 
            {
               
                PakPqclogInfo pqclogInfo = new PakPqclogInfo();
                pqclogInfo.snoId = currentProduct.ProId;
                pqclogInfo.pno = currentProduct.Model;
                // ITC-1360-1439
                //pqclogInfo.pdLine = Line;
                //pqclogInfo.wc = Station;
                pqclogInfo.pdLine = "PAK";
                pqclogInfo.wc = "81";
                pqclogInfo.cdt = DateTime.Now;

                ipizzaRepository.InsertPakPqcLogDefered(CurrentSession.UnitOfWork, pqclogInfo);
            }
	        return base.DoExecute(executionContext);
        }
	}
}
