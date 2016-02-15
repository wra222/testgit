/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
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
using IMES.FisObject.PAK.FRU;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 插入FRUCarton FRUCart_Part数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于PAK 061站
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
    ///         Session.SessionKeys.GiftScanPartCount
    ///         Session.SessionKeys.GiftPartNoList 
    ///         Session.SessionKeys.GiftScanPartList
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
    /// insert(FRUCarton) and (FRUCart_Part)        
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        
    /// </para> 
    /// </remarks>
    public partial class BindCartonToCT: BaseActivity
	{
		
        public BindCartonToCT()
		{
			InitializeComponent();
		}
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IFRUCartonRepository fruCartonRep = RepositoryFactory.GetInstance().GetRepository<IFRUCartonRepository>();
            string cartonNo = ((IList<string>)this.CurrentSession.GetValue(Session.SessionKeys.FRUCartonNoList))[0];
            string model = (string)this.CurrentSession.GetValue(Session.SessionKeys.ModelName);
            int qty = (int)this.CurrentSession.GetValue(Session.SessionKeys.GiftScanPartCount);
            
            FRUCarton fr = new FRUCarton(cartonNo, model, qty);

          
            IList<string> partLst = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.GiftPartNoList);
            IList<IList<string>> scanList = (IList<IList<string>>)CurrentSession.GetValue(Session.SessionKeys.GiftScanPartList);
           
            for (int i = 0; i < partLst.Count; i++)
            {
                for (int j = 0; j < scanList[i].Count; j++)
                {
                    FRUPart fruPart = new FRUPart(cartonNo, partLst[i], scanList[i][j], this.Editor, new DateTime());
                    fr.AddPart(fruPart);
                }
               
            }
           
            fruCartonRep.Add(fr, CurrentSession.UnitOfWork);
       
            return base.DoExecute(executionContext);
     
        }
	}
}
