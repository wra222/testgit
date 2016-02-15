﻿/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:Update/Insert ForceNWC.
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc202017             Create
 * Known issues:
*/
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// Update/Insert ForceNWC.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Key Parts.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///    详见UC
    /// </para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class WriteForceNWCForMVunpack : BaseActivity
    {
        ///<summary>
        ///</summary>
        public WriteForceNWCForMVunpack()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update/Insert ForceNWC.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IProduct currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            string retWC = (string)CurrentSession.GetValue(Session.SessionKeys.ReturnStation);
            
            ForceNWCInfo item = new ForceNWCInfo();
            item.editor = Editor;
            item.forceNWC = retWC;
            item.preStation =currentProduct.Status.StationId;


            ForceNWCInfo cond = new ForceNWCInfo();
            cond.productID = currentProduct.ProId;
            if (partRepository.CheckExistForceNWC(cond))
            {
                partRepository.UpdateForceNWCDefered(CurrentSession.UnitOfWork, item, cond);
            }
            else
            {
                item.productID = currentProduct.ProId;
                
                partRepository.InsertForceNWCDefered(CurrentSession.UnitOfWork, item);
            }

            return base.DoExecute(executionContext);
        }
    }
}