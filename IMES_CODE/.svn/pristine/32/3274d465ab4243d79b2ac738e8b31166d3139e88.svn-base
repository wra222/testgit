/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CompleteRepair。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-13   Tong.Zhi-Yong                implement DoExecute method
 * 2012-01-11   Yang.Wei-Hua                 For IMES2012 SA Repair
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 检查是否满足complete条件, 更新Repair状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///        
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB, Product为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查MBRepair or ProductRepair是否满足complete条件, ;
    ///         2.更新MBReair or ProductRepair状态;
    ///         3.保存MBRepair or ProductRepair;
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
    ///         update PCARepair
    ///         update ProductRepair
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IProduct
    ///         IMBRepository
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CompleteRepair : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CompleteRepair()
		{
			InitializeComponent();
		}

        /// <summary>
        /// NeedPartSN
        ///</summary>
        public static DependencyProperty NeedPartSNProperty = DependencyProperty.Register("NeedPartSN", typeof(string), typeof(CompleteRepair));

        ///<summary>
        /// 处理那个类型的维修(SA, FA, PAK)
        ///</summary>
        [DescriptionAttribute("NeedPartSN")]
        [CategoryAttribute("NeedPartSN Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string NeedPartSN
        {
            get
            {
                return ((string)(base.GetValue(CompleteRepair.NeedPartSNProperty)));
            }
            set
            {
                base.SetValue(CompleteRepair.NeedPartSNProperty, value);
            }
        }
		
		/// <summary>
        /// 检查是否满足complete条件, 更新Repair状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IRepairTarget repairTarget = GetRepairTarget();

            //check session
            if (CurrentSession == null)
            {
                List<String> erpara = new List<String>();

                throw new FisException("CHK021", erpara);
            }
            
            if (repairTarget == null)
            {
                throw new NullReferenceException();
            }
			
			string stage = CurrentSession.GetValue("Stage") as string;

            if ("Y" == this.NeedPartSN)
			{
				Repair r = repairTarget.GetCurrentRepair();
                var lstNeedSN = r.Defects.Where(x => ((x.MajorPart == "CRLCM" && stage != "CleanRoom") || (!string.IsNullOrEmpty(x.PartType))) &&
					(string.IsNullOrEmpty(x.OldPartSno) || string.IsNullOrEmpty(x.NewPartSno))
					).Select(y => y.DefectCodeID).ToList();
				if (null != lstNeedSN && lstNeedSN.Count > 0)
				{
					// 序號不得為空
					FisException ex = new FisException("CQCHK50063", new List<string>() { string.Join(",", lstNeedSN.ToArray()) });
					throw ex;
				}
			}

            if (typeof(MB).IsInstanceOfType(repairTarget))
            {
                //update [MTA_Mark] Mark = ‘1’
                IMBRepository repMB = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IList<Repair> repairList = repairTarget.GetRepair().Where(x=>x.Status == Repair.RepairStatus.NotFinished).ToList();
                foreach (Repair item in repairList)
                {
                    repMB.UpdateMtaMarkByRepairIdDefered(CurrentSession.UnitOfWork, item.ID, "1");
                    repairTarget.CompleteRepair(Line, Station, Editor);   
                    //repMB.UpdateMtaMarkByRepairId(item.ID, "1");    
                }
                //repMB.UpdateMtaMarkByRepairIdDefered(CurrentSession.UnitOfWork, repairTarget.GetCurrentRepair().ID, "1");
            }
            else{
                //update Repair
                repairTarget.CompleteRepair(Line, Station, Editor);
            }
            UpdateRepairTarget(repairTarget);                
            return base.DoExecute(executionContext);
        }
	}
}
