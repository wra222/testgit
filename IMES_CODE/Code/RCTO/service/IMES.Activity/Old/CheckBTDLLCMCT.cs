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
using IMES.FisObject.Common.Model;
namespace IMES.Activity
{
    /// <summary>
    /// 用于check BTDL LCM CT
    /// </summary>
    ///    <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// <para>
    /// 应用场景：
    ///      FA combine LCM BTDL/TPDL
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取NewScanedProductIDList，调用ProductRepository.BindDN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.BTDLLCMCTNO
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
    ///              IPartSnRepository
    ///              ILCMRepository
    /// </para> 
    /// </remarks>
    public partial class CheckBTDLLCMCT : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBTDLLCMCT()
        {
            InitializeComponent();
        }

        // 5. Check LCM CT#	检查规则：
            //在PartSN表里根据 CT#找到对应的记录，得到Part Type=LCM

            //以下是TSB特有业务需求：
            //若Vendor=LG并且family前7码=POTOMAC，则提示user以下信息：”需贴rubber.”
            //Family获取方法：根据PartNo得到第一个包含此part的MOBom，得到model，再找到对应的Family
                    //select Family from IMES_GetData..Model where Model=(select Model from IMES_GetData..MO where MO=(select top 1 MO from IMES_GetData..MoBOM where PartNo=(select IECPn from IMES_FA..PartSN where IECSN=@CTNO)))
            //Vendor取值参考语句：
            //Part.Vendor where PartNo=pn
                    //select Vendor from  IMES_GetData..Part where PartNo=(select IECPn from IMES_FA..PartSN where IECSN=@CTNO)

            //异常情况：
            //A.	若输入的CT#已绑定灯下（LCMBind），则提示”此CT已与其它灯下结合，不能再结合.”
                    //select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype='BTDL'
            //B.	若输入的CT#已绑定Vendor SN(PartSN)，则提示”此CT已与其它vendor SN结合，不能再结合.”
                    //select VendorSN,PartType from IMES_FA..PartSN  where IECSN=@CTNO   为空
            //C.	若CT#没在PartSN中存在，则提示”此CT不存在，重新输入.”
                    //select VendorSN,PartType from IMES_FA..PartSN  where IECSN=@CTNO  无此记录
            //D.	若CT#对应的Type不等于LCM，则提示”此CT不是LCM，请重新输入.”
                    //select VendorSN,PartType from IMES_FA..PartSN  where IECSN=@CTNO  type！=“LCM”

        //family:select 
/// <summary>
/// 根据规则检查BTDL LCM CTNO
/// </summary>
/// <param name="executionContext"></param>
/// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            string ctno = CurrentSession.GetValue(Session.SessionKeys.BTDLLCMCTNO).ToString();
            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();
            int btbindCount = lcmRepository.GetLCMBindCount(ctno, "BTDL");
       
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();

            PartSn partsnobj = currentPartSNRepository.Find(ctno);
            CurrentSession.AddValue(Session.SessionKeys.PartSN, partsnobj);
            //A.	若输入的CT#已绑定灯下（LCMBind），则提示”此CT已与其它灯下结合，不能再结合.”
            //select count(*) from IMES_FA..LCMBind where LCMSno=@CTNO and METype='BTDL'
            if (btbindCount>0)
            {
                erpara.Add(ctno);
                ex = new FisException("CHK077", erpara);
                throw ex;
            }
            else if (partsnobj == null)
            {
                erpara.Add(ctno);
                ex = new FisException("CHK075", erpara);
                throw ex;
            }
            else if (partsnobj.Type != "LCM")
            {
                erpara.Add(ctno);
                ex = new FisException("CHK076", erpara);
                throw ex;
            }
            else if (partsnobj.VendorSn != "")
            {
                erpara.Add(ctno);
                ex = new FisException("CHK028", erpara);
                throw ex;
            }

            var currentPartRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart  currentPart = currentPartRepository.Find(partsnobj.IecPn);
            //Vendor取值参考语句：AssemblyCodeInfo.InfoValue where InfoType=Vendor and AssemblyCode=ct#的前6码

            if (this.Customer == "TSB" && currentPart != null && currentPart.GetAttribute("Vendor", partsnobj.IecSn.Substring(0, 6)) == "LG")
            {
                 var currentBomRepository = IMES.Infrastructure.FisObjectRepositoryFramework.RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
                 var currentfamily = currentBomRepository.GetFirstFamilyViaMoBOM(partsnobj.IecPn);
                 //Family currentfamily = new Family("POTOMACLYFTEST001", "QATEST", "TSB");

                if (currentfamily != null && currentfamily.FamilyName.StartsWith("POTOMAC"))
                {
                    erpara.Add(ctno);
                    ex = new FisException("CHK036", erpara);
                    ex.stopWF = false;
                    throw ex;
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
