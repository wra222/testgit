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
using IMES.FisObject.PAK.FRU ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 将页面输入的gift与产生的carton号绑定
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于PAK 055站
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
    ///         SSession.SessionKeys.FRUCartonNoList 
    ///         Session.SessionKeys.ModelName
    ///         Session.SessionKeys.GiftNoList
    ///         Session.SessionKeys.Qty 
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
    /// insert FRU Carton和FRUCarton_FRUGift        
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        
    /// </para> 
    /// </remarks>
    public partial class BindGiftNoFor055 : BaseActivity
    {

        public BindGiftNoFor055()
        {
            InitializeComponent();
        }


        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IFRUCartonRepository fruCartonRep = RepositoryFactory.GetInstance().GetRepository<IFRUCartonRepository>();
            string cartonNo = ((IList<string>)this.CurrentSession.GetValue(Session.SessionKeys.FRUCartonNoList))[0];
            string model = (string)this.CurrentSession.GetValue(Session.SessionKeys.ModelName);
            int qty = (int)this.CurrentSession.GetValue(Session.SessionKeys.Qty);
            IList<string> giftList = (IList<string>)this.CurrentSession.GetValue(Session.SessionKeys.GiftNoList);


            FRUCarton fr = new FRUCarton(cartonNo, model, qty);

            foreach (string gift in giftList)
            {
                if (fruCartonRep.CheckExistForFRUCarton_FRUGift(gift))
                {
                    throw new FisException("CHK132", new string[] { gift });
                }
                IFRUPart item = new FRUPart(cartonNo, gift, "", this.Editor, System.DateTime.Now);
                fr.AddGift(item);
            }

            fruCartonRep.Add(fr, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}
