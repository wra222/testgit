/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 2010-03-01   207006     ITC-1122-0160
 * 2010-03-01   207006     ITC-1122-0170
 * Known issues:Any restrictions about this file 
 */



using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 调整MO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///        adjustMO站
    /// </para>
    /// <para>
    /// 实现逻辑：
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
    ///         Session.SessionKeys.OldMONO
    ///         Session.SessionKeys.NewMONO
    ///         Session.SessionKeys.ProdNoList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         wu
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         ProductChangeLog
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///    
    /// </para> 
    /// </remarks>
    public sealed partial class AdjustMO : BaseActivity
    {
        public AdjustMO()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var MORepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();
            IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            var oldMONO = CurrentSession.GetValue(Session.SessionKeys.OldMONO).ToString();
            var newMONO = CurrentSession.GetValue(Session.SessionKeys.NewMONO).ToString();
            
            var prodNoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);

            var oldMO = MORepository.Find(oldMONO);
            var newMO = MORepository.Find(newMONO);

            short qty = (short)0;
            short.TryParse(CurrentSession.GetValue(Session.SessionKeys.Qty).ToString(), out qty);

            if (oldMO == null)
            {
                var ex = new FisException("CHK037", new string[] { oldMONO });
                throw ex;
            }
            if (newMO == null)
            {
                var ex = new FisException("CHK037", new string[] { newMONO });
                throw ex;
            }
            
            //数量检查
            if (newMO.Qty - newMO.PrtQty < qty)
            {
                var ex = new FisException("CHK072", new string[] { });
                throw ex;
            }

            oldMO.PrtQty = (short)(oldMO.PrtQty - qty);
            MORepository.Update(oldMO, CurrentSession.UnitOfWork);
            newMO.PrtQty = (short)(newMO.PrtQty + qty);
            MORepository.Update(newMO, CurrentSession.UnitOfWork);

            //2010-03-01   207006     ITC-1122-0160
            //2010-03-01   207006     ITC-1122-0170

            System.DateTime cdt = DateTime.Now;

            foreach (string item in prodNoList)
            {
                IProduct prod = prodRepository.Find(item);
                               
                              
                if (prod == null)
                {
                    var ex = new FisException("SFC002", new string[] { item });
                    throw ex;
                }
                prod.MO = newMONO;


                ProductChangeLog itemLog = new ProductChangeLog();
                itemLog.Editor = this.Editor;
                itemLog.Mo = oldMONO;
                itemLog.ProductID = item;
                itemLog.Station = prod.Status.StationId;
                itemLog.Cdt = cdt;

                prod.AddChangeLog(itemLog);
                prodRepository.Update(prod, CurrentSession.UnitOfWork);
            }

            
            return base.DoExecute(executionContext);
        }
    }

}
