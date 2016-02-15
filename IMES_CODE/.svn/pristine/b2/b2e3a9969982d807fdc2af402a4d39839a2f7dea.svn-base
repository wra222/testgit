// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.call sp_Txf_IMES_To_FIS_PCA_ByPCBNO
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-02                               create
// Known issues:

using System.Data.SqlClient;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
//using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using System.Collections.Generic;
using System.Data;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using System.Text.RegularExpressions;
using System; 

namespace IMES.Activity
{

    /// <summary>
    /// call sp_Txf_IMES_To_FIS_PCA_ByPCBNO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PCA TestStation
    /// </para>
    /// <para>
    /// 实现逻辑：调用存储过程结转数据到旧FIS
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
    ///         Session.MB
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
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class Callsp_Txf_IMES_To_FIS_PCA_ByPCBNO : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Callsp_Txf_IMES_To_FIS_PCA_ByPCBNO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change Model Save
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (this.Station != "15")
            {
                return base.DoExecute(executionContext);
            }
            
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string mbsn = currentMB.Sn;
            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("PCBNo", mbsn);

            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            DataTable returnTable = CurrentProductRepository.ExecSpForQuery(SqlHelper.ConnectionString_PCA, "Txf_IMES_To_FIS_PCA_ByPCBNo", paramsArray);
            if (returnTable != null && returnTable.Rows.Count > 0)
            {
                string GeterrorString = returnTable.Rows[0][0].ToString();
                Regex reg = new Regex("^[0-9]*$");//^[0-9]*$ //^[0-9]+$
                Match ma = reg.Match(GeterrorString);
                if (ma.Success)
                {
                    //是数字  
                }
                else
                {
                    //不是数字  
                    throw new Exception(GeterrorString); 
                }
            }
            else
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentMB.Sn);
                throw new FisException("IEC005", errpara);

            }
            return base.DoExecute(executionContext);
        }

    }
}

