// INVENTEC corporation (c)2012 all rights reserved. 
// Description:  根据输入的PalletNo,检查Pallet是否存在，状态是否正确
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-21   Yuan XiaoWei                 create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的PalletNo,检查Pallet是否存在，状态是否正确
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC DT Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据PalletNo，调用IPalletRepository的Find方法，获取Pallet对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Pallet
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    ///              Pallet
    /// </para> 
    /// </remarks>
    public partial class CheckPalletForDT : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public CheckPalletForDT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Pallet Object and put it into Session.SessionKeys.Pallet
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string palletNo = CurrentSession.GetValue(Session.SessionKeys.PalletNo) as string;
            IPalletRepository CurrentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            if (palletNo != null && (palletNo.StartsWith("00") || palletNo.StartsWith("01")))
            {


                Pallet CurrentPallet = CurrentPalletRepository.Find(palletNo);
                if (CurrentPallet == null)
                {
                    throw new FisException("PAK088", new string[] { palletNo });

                }

                WhPltLogInfo resultLog = CurrentPalletRepository.GetWhPltLogInfoNewestly(palletNo);
                //if (resultLog == null || (resultLog.wc != "99" && resultLog.wc != "RW" && resultLog.wc != "DT"))
                //mantis1652: DT Pallet Control 可连续2次刷入
                if (resultLog == null || (resultLog.wc != "99" && resultLog.wc != "RW"))
                {
                    throw new FisException("PAK089", new string[] { palletNo });
                }
                CurrentSession.AddValue("WhPltLogWc", resultLog.wc);
                CurrentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);
            }
            else if (palletNo != null && palletNo.StartsWith("90"))
            {
                IList<DummyShipDetInfo> dummyPallet = CurrentPalletRepository.GetDummyShipDetListByPlt(palletNo);
                if (dummyPallet == null || dummyPallet.Count == 0)
                {
                    throw new FisException("PAK088", new string[] { palletNo });
                }

                WhPltLogInfo resultLog = CurrentPalletRepository.GetWhPltLogInfoNewestly(palletNo);
                //if (resultLog == null || (resultLog.wc != "9A" && resultLog.wc != "RW" && resultLog.wc != "DT"))
                //mantis1652: DT Pallet Control 可连续2次刷入
                if (resultLog == null || (resultLog.wc != "9A" && resultLog.wc != "RW" ))
                {
                    throw new FisException("PAK089", new string[] { palletNo });
                }
                CurrentSession.AddValue("WhPltLogWc", resultLog.wc);
            }
            else
            {
                throw new FisException("PAK088", new string[] { palletNo });
            }
            return base.DoExecute(executionContext);
        }
    }
}
