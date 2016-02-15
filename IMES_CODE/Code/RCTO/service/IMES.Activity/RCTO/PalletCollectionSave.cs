/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: Pallect Collection Save
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
    ///         1.	Assign Pallet
    ///         2.  Update Product
    ///         3.	如果Delivery 完成Pallet Collection，需要将状态修改为’88’
    ///         4.	Assign Ware House Location
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
    public partial class PalletCollectionSave : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public PalletCollectionSave()
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
            string Floor = CurrentSession.GetValue(Session.SessionKeys.Floor) as string;

            SqlParameter[] paramsArray = new SqlParameter[5];
            paramsArray[0] = new SqlParameter("CartonNo", CurrentCarton);
            paramsArray[1] = new SqlParameter("Editor", Editor);
            paramsArray[2] = new SqlParameter("Line", Line);
            paramsArray[3] = new SqlParameter("Floor", Floor);
            paramsArray[4] = new SqlParameter("Station", Station);

            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            System.Data.DataTable result = CurrentProductRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "IMES_PalletCollection_AssignPallet", paramsArray);

            if (result != null && result.Rows.Count > 0)
            {
                PalletCollectionUI UIObject = new PalletCollectionUI();
                UIObject.CartonNo = CurrentCarton;
                UIObject.DeliveryNo = result.Rows[0][0] as string;
                UIObject.PalletNo = result.Rows[0][1] as string;
                UIObject.TotalQty = (int)result.Rows[0][2];
                UIObject.PackedQty = (int)result.Rows[0][3];
                UIObject.Loc = result.Rows[0][4] as string;

                CurrentSession.AddValue(Session.SessionKeys.PalletCollectionUI, UIObject);
                string CartonStr = result.Rows[0][5] as string;
                string[] CartonArray = CartonStr.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                CurrentSession.AddValue(Session.SessionKeys.CartonNoList, CartonArray);
            }


            return base.DoExecute(executionContext);
        }

    }
}
