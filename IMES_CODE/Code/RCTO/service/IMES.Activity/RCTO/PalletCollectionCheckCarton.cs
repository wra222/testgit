﻿/*
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
using IMES.FisObject.Common.Process;
using IMES.Common;
using IMES.Resolve.Common;

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
    ///         a.	如果用户录入的[Carton No] 在数据库（CartonStatus.CartonNo）中不存在，则报告错误：“Invalid Carton No!”
    ///         b.	如果用户录入的[Carton No] 已经结合了Pallet，则报告错误：“This Carton has combined Pallet!”
    ///         c.	如果[Carton] 结合的Product的Product.Model 尚未称重，则报告错误：“This Model has no weight!
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
    public partial class PalletCollectionCheckCarton : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public PalletCollectionCheckCarton()
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
            System.Data.DataTable result = CurrentProductRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "IMES_PalletCollection_CheckCarton", paramsArray);

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
            }
            // Do BlockStation 
            try
            {
                IProductRepository iprr = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();

                List<string> productIDList = iprr.GetProductIDListByCarton(CurrentCarton);
                if(productIDList.Count==0)
                {
                    throw new FisException("CHK1045" ,new string[]{});
                }
                string keyOfSFC;
                string notEmpytLine = Line;
                foreach (String prdId in productIDList)
                {
                    IProduct product = iprr.Find(prdId);
                    keyOfSFC = product.ProId;
                    if (product.Status == null)
                    {
                        throw new FisException("SFC002", new string[] { keyOfSFC });
                    }

                    if (string.IsNullOrEmpty(notEmpytLine) && product.Status != null)
                    {
                        notEmpytLine = product.Status.Line;
                    }

                    if (product.Status != null && string.IsNullOrEmpty(product.Status.ReworkCode))
                    {
                        string firstLine = "";
                        if (!string.IsNullOrEmpty(notEmpytLine))
                        {
                            firstLine = notEmpytLine.Substring(0, 1);
                        }
                        IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(product.Model, firstLine);
                        if (currentModelProcess == null || currentModelProcess.Count == 0)
                        {
                            //CurrentProcessRepository.CreateModelProcess(product.Model, Editor, firstLine);
                            ResolveProcess.CreateModelProcess(product.ModelObj, Editor, firstLine);
                        }
                    }
                    CurrentProcessRepository.SFC(notEmpytLine, Customer, Station, keyOfSFC, "Product");

                }
            }
            catch (FisException fe)
            {
              
                fe.stopWF = true;
                throw fe;
            }
            // Do BlockStation 
            return base.DoExecute(executionContext);
        }

    }
}