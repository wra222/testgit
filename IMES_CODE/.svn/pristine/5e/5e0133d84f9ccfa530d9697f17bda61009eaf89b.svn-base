/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Activity/UpdateMO
 * UI:CI-MES12-SPEC-FA-UI Travel Card Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC Travel Card Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-15   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Update MO by TravelCardPrint
    /// </summary>
    /// <remarks>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-FA-UC Travel Card Print.docx
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    /// </para> 
    /// <para>    
    /// 输入：
    /// </para> 
    /// <para>    
    /// 中间变量：
    /// </para> 
    ///<para> 
    /// 输出：
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         MO
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMORepository    
    /// </para> 
    /// </remarks>
    public partial class UpdateMO: BaseActivity
	{
        private static Object _syncObj = new Object();

        /// <summary>
        /// </summary>
        public UpdateMO()
		{
			InitializeComponent();
		}

        /// <summary>
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {        
            var mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            object Qty = CurrentSession.GetValue(Session.SessionKeys.Qty);

            short IncreasePrintedQty = (short)0;
            if (Qty != null)
            {
                short.TryParse(Qty.ToString(),out IncreasePrintedQty);
            }
            else
            {
                IncreasePrintedQty = (short)0;
            }

            lock (_syncObj)
            {
                if (IncreasePrintedQty != (short)0)
                {
                    mo.PrtQty = (short)(mo.PrtQty + IncreasePrintedQty);
                    IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
                    moRepository.Update(mo, CurrentSession.UnitOfWork);
                }
            }

            return base.DoExecute(executionContext);
        }
	}
}
