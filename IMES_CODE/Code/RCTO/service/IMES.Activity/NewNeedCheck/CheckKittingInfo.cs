// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Check if has maintain Kitting Information. -- FA Kitting Input.
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-20   liuqingbiao                  create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// Check if has maintain Kitting Information
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC FA Kitting Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         A.	若没有维护ProdId对应的family相应的kitting信息，则提示” 沒有maintain 資料”
    ///             If exist(SELECT [Code],[PartNo]  FROM [FA].[dbo].[WipBuffer] where [Code] = @family)
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckKittingInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckKittingInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if has maintain Kitting Information
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
			 * 在WipBuffer表中不存在Code = @family 的纪录
			 * @family =Model.Family 
			 * If Not Exist(SELECT Top 1 [ID]
  			 * FROM [IMES2012_FA].[dbo].[WipBuffer] where [KittingType]='FA Kitting' and [Code]= @family)

			 */
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            string prodId = currentProduct.ProId;
            string model = currentProduct.Model;
            string family = currentProduct.Family;
            string kittingType = "FA Kitting";
            bool exist_record = false;

            IList<WipBuffer> wipbufferList = productRep.GetWipBufferByKittingTypeAndCode(kittingType, family);

            foreach (WipBuffer node in wipbufferList)
            {
                exist_record = true; break;
            }

            if (exist_record == false)
            {
                FisException ex = new FisException("CHK010", new string[] {  }); // has no maintain!
                throw ex;
            }

            // check extra check, for -- Phase II 2012-4-18 [IN UC].
            // 宋海燕 需要 修改 这儿

            return base.DoExecute(executionContext);
        }

    }
}

