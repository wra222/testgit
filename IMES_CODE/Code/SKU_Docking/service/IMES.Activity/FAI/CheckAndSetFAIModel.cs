using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 檢查CheckAndSetFAIModel .
    ///CheckAndSetFAIModel:假如有檢查到以上條件符合要報錯，預設:True, False:不符合設置條件以上報錯
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
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
    ///         
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
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckAndSetFAIModel : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAndSetFAIModel()
		{
			InitializeComponent();
		}

        private static IList<string> needCheckInQtyFAIFAState = new List<string> { "Approval", "Pilot" };
        private static IList<string> allowFAState = new List<string> { "Approval", "Pilot","Release" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            Session session =CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            string needFAI = utl.GetSysSetting("IsFAI", "N");   //只針對FA FAI Model 管控是否要檢查FAIModel
            if (needFAI == "Y")
            {
                var mo = (MO)session.GetValue(Session.SessionKeys.ProdMO);
                string model = mo.Model;

                FAIModelInfo faimodel = iModelRepository.GetFAIModelByModelWithTrans(model);
                int currenQty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
                if (faimodel != null)
                {
                    if (allowFAState.Contains(faimodel.FAState))
                    {
                        if (CheckAndSetInFAQtyWithTrans(faimodel, currenQty))
                        {
                            CurrentSession.AddValue("inFAI", "Y");
                            iModelRepository.UpdateFAIModelDefered(session.UnitOfWork, faimodel);
                        }
                    }
                    else
                    {
                        throw new FisException("CQCHK50013", new List<string> { faimodel.FAState });
                    }
                }
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faiModel"></param>
        /// <param name="inQty"></param>
        /// <returns></returns>
        public bool CheckAndSetInFAQtyWithTrans(FAIModelInfo faiModel, int inQty)
        {
            if (faiModel != null && needCheckInQtyFAIFAState.Contains(faiModel.FAState))
            {
                int remainingQty = faiModel.FAQty - faiModel.inFAQty;
                if ((remainingQty - inQty) < 0)
                {
                    throw new FisException("CQCHK50005", new List<string>{faiModel.Model, 
                                                                          "FA Travel Card Station", 
                                                                          remainingQty.ToString(), 
                                                                          inQty.ToString()});
                }

                faiModel.inFAQty = faiModel.inFAQty + inQty;
                if (faiModel.FAState == "Approval")
                {
                    faiModel.FAState = "Pilot";
                }
                faiModel.Editor = "FAI" + faiModel.Editor;
                return true;
            }

            return false;
        }

        
	}
}
