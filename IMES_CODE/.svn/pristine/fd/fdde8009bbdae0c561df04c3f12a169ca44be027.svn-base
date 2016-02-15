// INVENTEC corporation (c)2009 all rights reserved. 
// Description:绑定Gift与Part 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Lucy Liu                 create
// 2010-06-12   Lucy Liu(EB1)             Modify:   ITC-1155-0197
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.FRU;

namespace IMES.Activity
{

    /// <summary>
    /// 绑定Gift与Part    
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FRU Gift Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.将前一步骤生成的最大生成号插入FRUGift表中，然后将scanPart插入到FRUGift_Part表中
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    public partial class BindGiftNo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindGiftNo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 将前一步骤生成的最大生成号插入FRUGift表中，然后将scanPart插入到FRUGift_Part表中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //将前一步骤生成的最大生成号插入FRUGift表中
            IGiftRepository giftRep = RepositoryFactory.GetInstance().GetRepository<IGiftRepository, FRUGift>();
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            IList<string> giftNoLst = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.GiftNoList);
            string giftID = giftNoLst[0];
            CurrentSession.AddValue(Session.SessionKeys.GiftID, giftID);
            int qty = (int)CurrentSession.GetValue(Session.SessionKeys.GiftScanPartCount);
            FRUGift item = new FRUGift(model, giftID, qty);
            
            

            //将scanPart插入到FRUGift_Part表中
            IList<string> partLst = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.GiftPartNoList);
            IList<IList<string>> scanList = (IList<IList<string>>)CurrentSession.GetValue(Session.SessionKeys.GiftScanPartList);
            for (int i = 0; i < partLst.Count; i++)
            {
                //    <bug>
                //                BUG NO:ITC-1155-0197
                //                REASON:错写成scanList.Count
                //    </bug>
                for (int j = 0; j < scanList[i].Count; j++)
                {
                    FRUPart fruPart = new FRUPart(giftID, partLst[i], scanList[i][j],this.Editor,new DateTime());
                    item.AddPart(fruPart);
                }
               
            }
            giftRep.Add(item, CurrentSession.UnitOfWork);
           

            return base.DoExecute(executionContext);
        }

       


    }
}


