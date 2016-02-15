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
using IMES.FisObject.FA.Product;
using System.Data;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace IMES.Activity
{
    /// <summary>
    /// MB TEST TYP[E
    /// </summary>
    public partial class MBAssignTestTypeSFCSP : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public MBAssignTestTypeSFCSP()
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
            CurrentSession.AddValue("MBAssignType", "");
            List<SqlParameter> paramsArray = new List<SqlParameter>();
            paramsArray.Add(new SqlParameter("mb", this.Key));
            paramsArray.Add(new SqlParameter("pdline", this.Line));
            paramsArray.Add(new SqlParameter("editor", this.Editor));
           
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            DataTable dt = new DataTable();
            dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_PAK, "op_GetPCBTest_Result", paramsArray.ToArray());

            if (dt.Rows.Count != 1)
            {
                throw new Exception("SP return value is invalid![row cnt:" + dt.Rows.Count + "]");
            }
            DataRow row = dt.Rows[0];
            string Result = row[0].ToString().Trim().ToUpper();
            CurrentSession.AddValue("MBAssignType", Result);
           

            return base.DoExecute(executionContext);
        }

	}
}
