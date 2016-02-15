// INVENTEC corporation (c)2011 all rights reserved. 
// Description:BT 转 非BT
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 删除ProductBT表中符合条件的Product
    /// 符合条件的Product：
    /// Product.Model=@model且
    /// ProductStatus.Line的Stage=FA或
    /// ProductStatus.Line的Stage=PAK并且ProductStatus.Station=69
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-BT_CHANGE
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
    ///         Session.SessionKeys.ModelName
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       无
    /// </para> 
    ///<para>
    /// 数据更新:
    ///        DELETE  from FA_SnoBTDet as F inner join Product as P on F.SnoId = P.ProductID
    ///		   inner join ProductStatus as S ON P.ProductID = S.ProductID
    ///        INNER JOIN IMES2012_GetData.dbo.Line AS L ON L.Line = S.Line --COLLATE SQL_Latin1_General_CP1_CI_AS
    ///        WHERE P.Model=@Model AND ((L.Stage='FA') OR (L.Stage='PAK' AND S.Station ='69'))           
    /// </para>
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class ChangeToNonBT : BaseActivity
    {
        ///<summary>
        /// constructor
        ///</summary>
        public ChangeToNonBT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 删除ProductBT表中符合条件的Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string ModelName = CurrentSession.GetValue(Session.SessionKeys.ModelName).ToString();

            var ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (ProductRepository.PreQueryForChangeToNonBT(ModelName) == true)
            {
                ProductRepository.ChangeToNonBTDefered(CurrentSession.UnitOfWork, ModelName);
            }
            else
            {
                List<string> erpara = new List<string>();
                erpara.Add(ModelName);
                //2012-7-13, for Mantis, 修改报错信息，由“资料有误”变为“找不到匹配的Product!”     
                //throw new FisException("PAK084", erpara);
                throw new FisException("CHK202", erpara);
            }

            return base.DoExecute(executionContext);
        }
    }
}
