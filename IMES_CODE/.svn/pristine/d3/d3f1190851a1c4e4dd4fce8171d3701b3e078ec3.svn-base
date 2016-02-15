// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-26   Du Xuan (itc98066)          create
// Known issues:
// ITC-1360-0892 修改Delivery_Pallet查询接口
// ITC-1360-0719 增加未满判定

using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Pallet 的分配原则（当Product 尚未结合Pallet 时，才需要分配Pallet / Box Id / UCC）
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
    ///         Pallet 分配
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class AssignPallet : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignPallet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pallet 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            IList<ProductBTInfo> btList = productRep.GetProductBT(curProduct.ProId);
            if (btList.Count > 0 && Station.Trim() == "92")
            {
                return base.DoExecute(executionContext);
            }

            string assignPalletNo = curProduct.PalletNo;
            if (string.IsNullOrEmpty(assignPalletNo))
            {
                assignPalletNo = palletRep.GetAutoAssignPallet(curProduct.DeliveryNo);
            }


            ////Lock The XXX: 2012.04.20 LiuDong
            //if (!string.IsNullOrEmpty(assignPalletNo))
            //{
            //    Guid gUiD = Guid.Empty;
            //    var identity = new ConcurrentLocksInfo();
            //    identity.clientAddr = "N/A";
            //    identity.customer = CurrentSession.Customer;
            //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //    identity.editor = CurrentSession.Editor;
            //    identity.line = CurrentSession.Line;
            //    identity.station = CurrentSession.Station;
            //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
            //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
            //    gUiD = productRep.GrabLockByTransThread("Pallet", assignPalletNo, identity);
            //    CurrentSession.AddValue(Session.SessionKeys.lockToken_Pallet, gUiD);
            //}
            ////Lock The XXX: 2012.04.20 LiuDong

            //2012.05.11 LD
            ////2012.05.09 LD
            //Pallet assignPallet = palletRep.Find_OnTrans(assignPalletNo); 
            Pallet assignPallet = palletRep.Find(assignPalletNo);
            ////2012.05.09 LD
            //2012.05.11 LD

            if (assignPallet == null)
            {
                FisException fe = new FisException("PAK093", new string[] { });   //此船务栈板已满!
                throw fe;
            }

            curProduct.PalletNo = assignPallet.PalletNo;

            CurrentSession.AddValue(Session.SessionKeys.Pallet, assignPallet);

            return base.DoExecute(executionContext);

        }
    }

}
