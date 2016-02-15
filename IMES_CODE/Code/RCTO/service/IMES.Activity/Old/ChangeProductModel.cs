/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Change Model
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-12-15   Tong.Zhi-Yong                implement DoExecute method
 * 2010-03-29   Tong.Zhi-Yong                Change Model 选用MO 的方法修改为选用StartDate 最靠前的可用MO
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.BOM;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 把Unit转为新的Model
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA站Generate customer SN
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.得到StartDate最靠前的可用的Mo, 通过Product.MO获得old MO. Old MO: Print_Qty-1, New MO：Print_Qty+1.
    ///         2.通过ProductRepository更新Product.Model, Product.MO, 添加ChangeLog.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.ModelName
    ///         
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
    ///         Product, ChangeLog, MO
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMORepository
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
	public partial class ChangeProductModel: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public ChangeProductModel()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Change Model
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IMORepository imr = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            IProductRepository ipr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository ibr = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
            string newModel = CurrentSession.GetValue(Session.SessionKeys.ModelName).ToString();
            MO newMO = null;
            MO oldMO = null;
            ProductChangeLog changeLog = null;

            //get min new MO
            //newMO = imr.GetMinUsableMO(newModel);
            //Change Model 选用MO 的方法修改为选用StartDate 最靠前的可用MO 2010-03-29
            newMO = imr.GetUsableMOOrderByStartDate(newModel);

            if (newMO == null)
            {
                //該MO已經投完,請換機型改投其他MO !!
                List<string> errpara = new List<string>();
                FisException ex = new FisException("CHK044", errpara);
                throw ex;
            }

            oldMO = imr.Find(product.MO);

            if (oldMO == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(product.MO);
                FisException ex = new FisException("CHK045", errpara);
                throw ex;
            }

            imr.IncreaseMOPrintedQtyDefered(CurrentSession.UnitOfWork, newMO, 1);
            imr.DecreaseMOPrintedQtyDefered(CurrentSession.UnitOfWork, oldMO, 1);

            //imr.Update(newMO, CurrentSession.UnitOfWork);
            //imr.Update(oldMO, CurrentSession.UnitOfWork);

            product.Model = newModel;
            product.MO = newMO.MONO;

            changeLog = new ProductChangeLog();
            changeLog.ProductID = product.ProId;
            changeLog.Mo = oldMO.MONO;
            changeLog.Station = Station;
            changeLog.Editor = Editor;
            changeLog.Cdt = DateTime.Now;

            product.AddChangeLog(changeLog);
            
            ipr.Update(product, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
	}
}
