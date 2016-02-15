/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: 获取可显示的PICPosition, PICName
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-09-10   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using System.Data;

namespace IMES.Activity
{
    /// <summary>
    /// 获取可显示的PICPosition, PICName
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-FA-UC KB ESOP Projection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         select b.PartNo, c.InfoValue as PICPosition, d.InfoValue as PICName from ModelBOM a, Part b, PartInfo c,PartInfo d
    ///         where a.Material = '@Model'
    ///         and a.Component = b.PartNo 
    ///         and b.BomNodeType = 'PL'
    ///         and b.PartNo = c.PartNo 
    ///         and c.InfoType = 'Screenpic'
    ///         and b.PartNo = d.PartNo 
    ///         and d.InfoType = 'Screenpicname'
    ///         order by c.InfoValue
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GetPicPositionName : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GetPicPositionName()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;



            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("Model", currentProduct.Model);
            DataTable result = CurrentProductRepository.ExecSpForQuery(SqlHelper.ConnectionString_FA, "IMES_GetPicPositionName", paramsArray);
            if (result != null && result.Rows.Count >0){
                string[,] PicPositionName = new string[result.Rows.Count,2];
                //string[][] PicPositionName = new string[result.Rows.Count][];
                for (int i = 0; i < result.Rows.Count; i++) {
                    PicPositionName[i,0] = result.Rows[i][0] as string;
                    PicPositionName[i,1] = result.Rows[i][1] as string; 
                }
                CurrentSession.AddValue(Session.SessionKeys.PicPositionName, PicPositionName);
            }


            
            return base.DoExecute(executionContext);
        }
    }
}
