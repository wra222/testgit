// INVENTEC corporation (c)2009 all rights reserved. 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// Assign Model
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      mantis 1923 Assign Model
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class AssignModel : BaseActivity
	{
		///<summary>
		///</summary>
		public AssignModel()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
            if (currentProduct == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }

            string theLine = CurrentSession.GetValue("theLine") as string;
            string theFamily = CurrentSession.GetValue("theFamily") as string;
            string theModel = CurrentSession.GetValue("theModel") as string;
          

            if (!theFamily.Equals(currentProduct.Family)) // 输入的机型不属于所选的Family，不能Assign
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK1023", errpara);
            }

            ModelChangeQtyDef v = myRepository.GetActiveModelChangeQty(theLine, theModel);
            if (v == null) // 所选机型在Line %1 没有可分配的数量
            {
                List<string> errpara = new List<string>();
                errpara.Add(theLine);
                throw new FisException("CHK1022", errpara);
            }

            // mantis 2634
            string NeedCheckMB = "Y";
            IList<ConstValueTypeInfo> lstNoCheckMB = ActivityCommonImpl.Instance.GetConstValueTypeByType("AssignModelNoCheckMBFamily");
            if (null != lstNoCheckMB && lstNoCheckMB.Where(x => x.value == currentProduct.Family).Count() > 0)
                NeedCheckMB = "N";
            CurrentSession.AddValue("NeedCheckMB", NeedCheckMB);
            IList<ProductLog> pp = prodRep.GetProductLogs(currentProduct.ProId, "AM");
            if (pp == null || pp.Count==0)
            {
             myRepository.AssignedModelChangeQty(v.ID);
            }
            currentProduct.Model = theModel;
            
            return base.DoExecute(executionContext);
        }
	}
}
