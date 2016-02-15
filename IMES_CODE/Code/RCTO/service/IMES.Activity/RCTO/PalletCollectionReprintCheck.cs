/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: Pallect Collection Check Carton
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-07-30   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{
    /// <summary>
    /// Pallect Collection Check Carton
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-PAK-UC Pallet Collection _RCTO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.Carton
    ///         a.	如果输入的[Carton No] 在数据库(Product.CartonSN)中不存在，则报告错误：“此Carton No不存在!”
    ///         b.	如果此Carton 结合的Pallet No尚未完成Pallet Collection，则报告错误：“此Carton尚不能Reprint Pallet Number Label！”
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Carton
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
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class PalletCollectionReprintCheck : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public PalletCollectionReprintCheck()
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

            string CurrentCarton = CurrentSession.GetValue(Session.SessionKeys.Carton) as string;

            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("CartonNo", CurrentCarton);
            System.Data.DataTable result = CurrentProductRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "IMES_PalletCollection_ReprintCheck", paramsArray);

            if (result != null && result.Rows.Count > 0)
            {
                string ResutlSigal = result.Rows[0][0] as string;
                if (ResutlSigal != "SUCCESS")
                {
                    List<string> errpara = new List<string>();
                    for (int i = 1; i < result.Columns.Count; i++)
                    {
                        errpara.Add(result.Rows[0][i] as string);
                    }
                    throw new FisException(ResutlSigal, errpara);

                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.PalletNo, result.Rows[0][1] as string);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "RCTOPLTNumLabel");
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, result.Rows[0][1] as string);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, result.Rows[0][1] as string);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");
                }
            }

            return base.DoExecute(executionContext);
        }

    }
}
