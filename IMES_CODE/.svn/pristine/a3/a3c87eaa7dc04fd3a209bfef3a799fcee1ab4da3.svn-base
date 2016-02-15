// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Assign WH Location
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-01   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System;

namespace IMES.Activity
{
    /// <summary>
    /// Assign WH Location
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Assign WH Location for BT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.优先按照PAK_BTLocMas.Model = @Model (用户刷入的Customer S/N 对应的Model) 分配状态为'Open' (PAK_BTLocMas.Status = 'Open')的库位中进行分配
    ///         如果该Model 已经没有剩余库位的话，在PAK_BTLocMas.Model = 'Other' 的状态为'Open'  (PAK_BTLocMas.Status = 'Open')的库位中进行分配
    ///         无论分配的是Model 专用库位还是Other 库位，都是按照库位号正序排列取第一个库位(PAK_BTLocMas.SnoId)
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                   PAK009
    ///                     
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
    ///         Session.WHLocationObj
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class GetWHLocation : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public GetWHLocation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Assign WH Location
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();


            //Comment out the ConcurrentLocks Mechanism: 2012.04.24 LiuDong
            ////Lock The XXX: 2012.04.24 LiuDong
            //var identity = new ConcurrentLocksInfo();
            //identity.clientAddr = "N/A";
            //identity.customer = CurrentSession.Customer;
            //identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //identity.editor = CurrentSession.Editor;
            //identity.line = CurrentSession.Line;
            //identity.station = CurrentSession.Station;
            //identity.timeoutSpan4Hold = new TimeSpan(0, 0, 8).Ticks;
            //identity.timeoutSpan4Wait = new TimeSpan(0, 0, 10).Ticks;
            //Guid gUiD = productRep.GrabLockByTransThread("BTLoc", "ALL", identity);
            //CurrentSession.AddValue(Session.SessionKeys.lockToken_Loc, gUiD);
            ////Lock The XXX: 2012.04.24 LiuDong
            //Comment out the ConcurrentLocks Mechanism: 2012.04.24 LiuDong


            PakBtLocMasInfo result = null;
            // cut off the following line for UC modified.
            //result = currentRepository.GetPakBtLocMasInfo(CurrentProduct.Model, "Open");
            if (result == null)
            {
                result = currentRepository.GetPakBtLocMasInfo("Other", "Open");
            }

            if (result == null)
            {
                FisException ex;
                ex = new FisException("PAK009", new string[] { });
                throw ex;
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.WHLocationObj, result);
            }

            return base.DoExecute(executionContext);
        }
    }
}
