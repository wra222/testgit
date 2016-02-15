using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository._Schema;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;

namespace IMES.Activity
{
    /// <summary>
    /// 檢查CheckFAIPAKModel .
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckFAIPAKModel : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckFAIPAKModel()
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
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            string FAIinPAK = (string)product.GetExtendedProperty("FAIinPAK");
            if ("Y" != FAIinPAK)
            {
                // 非FAI機器，不允許做FAI Input
                throw new FisException("CQCHK50012", new string[] {  });
            }

            if (CheckFaiIn(product.Model))
            {
                // 此Model：%1 已有機器進入FAI In，不可再刷入!
                throw new FisException("CQCHK50064", new string[] { product.Model });
            }

            if (CheckFaiOut(product.Model))
            {
                // 此Model：%1 已有機器完成FAI Out，不可再刷入!
                throw new FisException("CQCHK50065", new string[] { product.Model });
            }

            return base.DoExecute(executionContext);
        }

        private bool CheckFaiIn(string model)
        {
            string strSQL = @"select a.AttrValue from ProductAttr a
inner join Product b on a.ProductID = b.ProductID and b.Model=@Model
where a.AttrName='FAIOutputReturnStation' and a.AttrValue <> ''";

            SqlParameter paraName = new SqlParameter("@Model", SqlDbType.VarChar, 20);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);
            
            if (tb != null && tb.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckFaiOut(string model)
        {
            string strSQL = @"select top 1 a.ProductID
from ProductLog a
inner join Product b on a.ProductID = b.ProductID and b.Model=@Model
where a.Station='FAIOut' and a.Status='1' ";

            SqlParameter paraName = new SqlParameter("@Model", SqlDbType.VarChar, 20);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

            if (tb != null && tb.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

	}
}
