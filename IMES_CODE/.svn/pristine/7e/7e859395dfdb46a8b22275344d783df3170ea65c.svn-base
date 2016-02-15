// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Bound ProdID And BoxId
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-20   liuqingbiao                  create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// Bound ProdID And BoxId
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC FA Kitting Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         productRep.AddKittingBoxSNItem
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class BoundProdIDAndBoxId : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BoundProdIDAndBoxId()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bound ProdID And BoxId
        /// Insert KittingBoxSN, Status=O
        /// 参考方法：insert KittingBoxSN select  distinct @pid,'Box','B'+@boxid,'O', 
        ///           left(pdline,3)+'~',@dt,@dt
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
			 * Now interface shortage, so here is empty. later will added!
			 */
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            // 1. this activity is to bound prodId and boxId, so we get them first. [set in service interface.]
            string _got_prodId = (string)CurrentSession.GetValue("_prodId");
            string _got_boxId = (string)CurrentSession.GetValue("_boxId");
            string _got_pdline = (string)CurrentSession.GetValue("inter_pdline");

            /*
             * SQL:
             *      Insert KittingBoxSN, Status=O
             *      insert  FAKittingBoxSN
             *      select  distinct @pid,'Box','B'+@boxid,'O', left(pdline,3)+'~',@dt,@dt
             *      
             * interface or method:
             *      IProductRepository::
             *          void AddKittingBoxSNItem(KittingBoxSNInfo item);
             *          或void AddKittingBoxSNItemDefered(IUnitOfWork uow, KittingBoxSNInfo item);
             * 
             *    public class KittingBoxSNInfo
             *    {
             *        public DateTime cdt;
             *        public string remark;
             *        public string sno;
             *        public string snoId;
             *        public string status;
             *        public string tp;
             *        public DateTime udt;
             *
             *        public KittingBoxSNInfo();
             *    }
             */
            //@pid,   'Box','B'+@boxid,  'O',     left(pdline,3)+'~',@dt,  @dt
            //[SnoId] [Tp] , [Sno]    ,  [Status] ,[Remark]         ,[Cdt],[Udt]
            KittingBoxSNInfo _info = new KittingBoxSNInfo();

            _info.snoId = _got_prodId;
            _info.tp = "Box";
            _info.sno = "B" + _got_boxId;//_got_pdline.Substring(0,3) + "~";
            _info.status = "O";
            _info.remark = _got_pdline.Substring(0, 3) + "~";//"B" + _got_boxId;
            _info.cdt = DateTime.Now;
            _info.udt = DateTime.UtcNow;
            _info.pdLine = _got_pdline;
            //_info.tp;

            KittingBoxSNInfo _cond = new KittingBoxSNInfo();
            _cond.snoId = _got_prodId;
            productRep.DeleteKittingBoxSNItemDefered(CurrentSession.UnitOfWork, _cond);
            productRep.AddKittingBoxSNItemDefered(CurrentSession.UnitOfWork, _info);

            return base.DoExecute(executionContext);
        }

    }
}

