// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  插入Product Model 的UnitStandardWeight记录 或 更新Product Model 的UnitStandardWeight, Editor, Udt
// UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-05   Chen Xu (itc208014)          create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 如果Product Model 的UnitStandardWeight 属性不存在则记录该属性；如果该属性值的Udt 不是当天，则更新该记录的Value，Editor 和Udt；
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
    ///         1.如果Product Model 的UnitStandardWeight 属性不存在则记录该属性；如果该属性值的Udt 不是当天，则更新该记录的Value，Editor 和Udt；
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
    public partial class InsertAndUpdateModelWeight : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public InsertAndUpdateModelWeight()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Insert And Update ModelWeight
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //4.	如果Product Model 的UnitWeight (IMES_GetData..ModelWeight) 属性不存在则记录该属性；如果该属性值的Udt 不是当天，则更新该记录的UnitWeightValue，Editor 和Udt；
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
            ModelWeight currentModelWeight = currentModelWeightRepository.Find(currentProduct.Model);

            //ITC-1360-1299: 如果Product Model 的UnitWeight (IMES_GetData..ModelWeight)属性，没有新Insert或Update时，不需要更新（Insert）HP EDI 数据（PAK_SkuMasterWeight_FIS）
            CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, false);

            if (currentModelWeight == null)
            {
                ModelWeight newModelWeight = new ModelWeight();
                newModelWeight.Model = currentProduct.Model;
                newModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                newModelWeight.CartonWeight = (decimal)0.00;
                newModelWeight.SendStatus = "";
                newModelWeight.Remark = "New";
                newModelWeight.Editor = this.Editor;
                newModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Add(newModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", newModelWeight.UnitWeight);
            }
            else if (currentModelWeight.UnitWeight.Equals(0))
            {
                currentModelWeight.Model = currentProduct.Model;
                currentModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                currentModelWeight.CartonWeight = (decimal)0.00;
                currentModelWeight.SendStatus = "";
                currentModelWeight.Remark = "0";
                currentModelWeight.Editor = this.Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);

            }
            //ITC-1360-1302
            //if (currentModelWeight.Udt.ToShortDateString().ToString() != DateTime.Now.ToShortDateString().ToString())
            else if (currentModelWeight.Udt.ToShortDateString().ToString() != DateTime.Now.ToShortDateString().ToString())
            {
                currentModelWeight.Model = currentProduct.Model;
                currentModelWeight.Remark = currentModelWeight.UnitWeight.ToString();
                currentModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                currentModelWeight.SendStatus = "";
              
                currentModelWeight.Editor = this.Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);
            }
            //当 Product 为同机型机器当天的第50台Pass Unit Weight 站的机器，需要取这50台机器的Product.UnitWeight 的平均值，更新ModelWeight 对应记录的UnitWeight，Editor 和Udt
            else
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                if (49 == productRepository.GetCountOfCurrentDayByModelAndStation(currentProduct.Model, "85"))
                {
                    decimal avrWeight = productRepository.GetAverageModelWeightOfCurrentDay(currentProduct.Model, "85");
                    currentModelWeight.Remark = currentModelWeight.UnitWeight.ToString();
                    decimal thisWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                    currentModelWeight.Model = currentProduct.Model;
                    currentModelWeight.UnitWeight = (avrWeight * 49 + thisWeight) / 50;
                    currentModelWeight.SendStatus = "";
                  
                    currentModelWeight.Editor = this.Editor;
                    currentModelWeight.Udt = DateTime.Now;

                    currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                    CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                    CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);
                }
            }


	        return base.DoExecute(executionContext);
        }
	}
}
