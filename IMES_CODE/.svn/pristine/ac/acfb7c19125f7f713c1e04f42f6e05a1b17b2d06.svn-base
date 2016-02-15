
using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
namespace IMES.Activity
{
    /// <summary>
    ///CheckIsAOILine
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
    public partial class CheckIsAOILine : BaseActivity
	{
		///<summary>
		///</summary>
        public CheckIsAOILine()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            Line line = lineRepository.Find(this.Line);
            string lineAlias = "";
            if (line.LineEx != null && !string.IsNullOrEmpty(line.LineEx.AliasLine))
            { lineAlias = line.LineEx.AliasLine.Trim(); }
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            // 若有配置值，表示進行AOI檢測
           

            IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var resultLst = palletRepository.GetConstValueListByType("AOIServerIP");

            string isAOILie = partRep.GetConstValueTypeList("OffAOIKBTest").Any(x => x.value == lineAlias)
                                     && palletRepository.GetConstValueListByType("AOIServerIP").Any(x=>x.name==lineAlias)
                                       ? "Y" : "N";

            CurrentSession.AddValue("IsAOILine", isAOILie);
            return base.DoExecute(executionContext);
        }
	}
}
