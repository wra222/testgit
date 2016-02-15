/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 2009-01-08   207013     Modify: ITC-1103-0074 、ITC-1103-0011
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;

namespace IMES.Activity
{
    /// <summary>
    /// Check LCM CT#	
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于092CombineLCMBTDL.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// Check LCM CT#	检查规则
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPDLLCMCTNO
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PartSN
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        ILCMRepository
    ///        IPartSnRepository
    /// </para> 
    /// </remarks>
    public partial class CheckTPDLLCMCT : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckTPDLLCMCT()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Check LCM CT#	检查规则：
        ///在PartSN表里根据 CT#找到对应的记录，得到Part Type=LCM，且此CT#已与Vendor SN#绑定
        ///异常情况：
        ///A. 若输入的CT#已绑定灯上（LCMBind），则提示”此CT已与其它灯上结合，不能再结合.”
        ///select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype='TPDL'
        ///B. 若CT#没在PartSN中存在，则提示”此CT不存在，请重新输入.”
        ///C. 若输入的CT#没有绑定，则提示”此CT还没有与vendor SN结合.”
        ///D. 若CT#对应的Type不等于LCM，则提示”此CT不是LCM，请重新输入.”
        ///select VendorSN,PartType from IMES_FA..PartSN  where IECSN=@CTNO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string ctno = CurrentSession.GetValue(Session.SessionKeys.TPDLLCMCTNO).ToString();

            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();

            int tpbindCount = lcmRepository.GetLCMBindCount(ctno,"TPDL");

            
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            PartSn partsnobj = currentPartSNRepository.Find(ctno);
            CurrentSession.AddValue(Session.SessionKeys.PartSN, partsnobj);
            //A. 若输入的CT#已绑定灯上（LCMBind），则提示”此CT已与其它灯上结合，不能再结合.”
            //select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype='TPDL'
            if (tpbindCount>0)
            {
                erpara.Add(ctno);
                //此CT %1 已与其它灯上结合，不能再结合.
                ex = new FisException("CHK078", erpara);
                throw ex;
            }
            else if (partsnobj == null)
            {
                erpara.Add(ctno);
                //此CT %1 不存在，请重新输入！
                ex = new FisException("CHK075", erpara);
                throw ex;
            }
            else if (partsnobj.Type != "LCM")
            {
                erpara.Add(ctno);
                ex = new FisException("CHK076", erpara);
                throw ex;
            }
            else if (partsnobj.VendorSn == "")
            {
                erpara.Add(ctno);
                ex = new FisException("CHK082", erpara);
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
