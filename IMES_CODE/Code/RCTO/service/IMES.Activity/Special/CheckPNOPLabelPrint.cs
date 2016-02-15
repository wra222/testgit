
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;
using System;

namespace IMES.Activity
{

    /// <summary>
    /// FRU 新需求 PNOP Label Print
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PNOP Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.	刷入 "PF" 12碼機型 
    ///     2.  依據刷入的@Model, 展BOM 至PC 階的下一階至BomNodeType='MB'，抓取@MB_PartNo
    ///         再抓取@MB_PartNo的 PartInfo 
    ///         select @MBPrefix =InfoValue 
    ///         from PartInfo 
    ///         where PartNo=@MB_PartNo and InfoType='MB'
    ///     3.  依據刷入的@MBSN, 抓取@MBSN前2碼與@MBPrefix 比對，若不一致報錯。
    ///     4.  檢查BOM是否有配置SPS, 若未配置則報錯。
    ///         select Value from ModelInfo where Model=@Model and Name = 'MB'
    ///     5.  產生DCode
    ///     6.  打印標籤(Win8 PNOP標籤)
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckPNOPLabelPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPNOPLabelPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否符合PNOP Print邏輯
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            string mbCode = currenMB.Sn.Substring(0,2);
            string infoValue = CurrentSession.GetValue("InfoValue") as string;
            if (infoValue == null || infoValue == "")
            {
                throw new FisException("CQCHK1053",new string[] {});
            }
            IList<string> infoValueItems = new List<string>();
            string[] list = infoValue.Split(',');
            foreach (string item in list)
            {
                infoValueItems.Add(item);
            }
            var query = (from q in infoValueItems
                         where q == mbCode
                            select q).ToArray();
            if (query.Count() == 0)
            {
                throw new FisException("CQCHK1054", new string[] { currenMB.Sn });
            }
            IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            string model = CurrentSession.GetValue("Model") as string;
            if (model == null || model == "")
            {
                throw new FisException("CQCHK1055", new string[] { });
            }
            IList<ModelInfo> modelInfoList = iModelRepository.GetModelInfoByModelAndName(model, "MB");
            if (modelInfoList == null || modelInfoList.Count == 0)
            {
                throw new FisException("CQCHK1056", new string[] { });
            }
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"PNOP");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenMB.Sn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenMB.Sn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr,model);
            return base.DoExecute(executionContext);
        }

    }
}

