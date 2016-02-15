// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 保存Pallet重量到FisToSAPPLTWeight,FisToSAPWeight
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
// ITC-1360-1014 根据UC定义修改存储过程名称
// Known issues:
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{

    /// <summary>
    /// 保存Pallet重量到FisToSAPPLTWeight,FisToSAPWeight
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PalletWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从session中获取Pallet
    ///         2.查看该Pallet的任一个DN是对应的Shipment栏位是否为空
    ///         3.1不为空表示按Shipment出货，
    ///         检查该Shipment下的Pallet是否都已经称重过
    ///         如果都已经称重
    ///         将各个Pallet的重量写到FISToSAPPLTWeight
    ///         将该Shipment所有Pallet的重量写到FISToSAPWeight
    ///
    ///         3.2为空表示按DN出货，
    ///         检查该DN前十码下的Pallet是否都已经称重过
    ///         如果都已经称重
    ///         将各个Pallet的重量写到FISToSAPPLTWeight
    ///         将该DN前十码所有Pallet的重量写到FISToSAPWeight
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
    ///         Session.Pallet
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
    ///         FISToSAPPLTWeight,FISToSAPWeight
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///            Pallet
    ///            IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class UpdatePalletWeightToSAP : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdatePalletWeightToSAP()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行更新重量到SAP的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);

            IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            PalletRepository.UpdatePltWeightToSAPDefered( CurrentSession.UnitOfWork,CurrentPallet.PalletNo);

            /*
            CurrentSession.UnitOfWork.Commit();
            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("@pallet", SqlDbType.Char);
            paramsArray[0].Value = CurrentPallet.PalletNo;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "op_Plt_upload_to_SAP", paramsArray);
            */
            return base.DoExecute(executionContext);
        }
    }
}
