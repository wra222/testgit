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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;

namespace IMES.Activity
{
	public partial class GetMBModel: BaseActivity
	{
		public GetMBModel()
		{
			InitializeComponent();

		}
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
            mbRepository.FillModelObj(mb);
            string mbCode = mb.ModelObj.Mbcode;

            #region find Model from Modelinfo table by MBCode SQL statement 
            //string strSQL="select top 1 Model from IMES_GetData..ModelInfo where Name=@Name and Value=@MBCode order by Cdt desc";
            //Dean 20110804 ModelInfo 有多個MBCode
           // string strSQL = "select top 1 Model from ModelInfo where Name=@Name and Value like @MBCode order by Cdt desc";
            //jiajia 20110907 增加 FRU condition IsFRU
            string strSQL = "select top 1 a.Model from ModelInfo a,ModelInfo b where a.Model=b.Model and a.Name=@Name and b.Name='IsFRU' and a.Value like @MBCode and b.Value='Y' order by a.Cdt desc";
           
            SqlParameter paraName= new SqlParameter("@Name", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = "MB";

            SqlParameter paraMBCode= new SqlParameter("@MBCode", SqlDbType.VarChar, 32);
            paraMBCode.Direction = ParameterDirection.Input;
            paraMBCode.Value = "%" + mbCode + "%"; //Dean 20110804 ModelInfo 有多個MBCode 

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName, paraMBCode);
            string ModelName = "";
            if (tb.Rows.Count > 0)
            {
                ModelName = tb.Rows[0][0].ToString();
            }
            else // not found Model name from modelInfo by MBCode
            {
                var ex = new FisException("CHK160", new string[] { mb.Model });
                throw ex;
            }
            #endregion

            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
            Model ModelObject = modelRepository.Find(ModelName);
            if (ModelObject == null) // not found Model name
            {
                var ex = new FisException("CHK039", new string[] { ModelObject.ModelName });
                throw ex;
            }
            //modelRepository.
            //modelRepository.GetModelInfoNameAndModelInfoValueListByModel(
            /*1.	基于MB SNo 找到其Part No
            2.	基于该Part No 找到其MB Code 属性
            3.	基于MB Code 属性查询Model 属性，找到对应的Model
            4.	再查询该Model 的属性得到Configuration Code 和UUID Range*/

            mb.SetAttributeValue("Model", ModelName, this.Editor, this.Station, "");
            CurrentSession.AddValue(Session.SessionKeys.ModelObj, ModelObject);

            //ProductAttribute item=new ProductAttribute();
            //item.AttributeName = "Model";
            //item.AttributeValue = ModelName;
            //item.ProductId = mb.Sn;
           // mb.PCBAttributes.Add(item);        
            
            return base.DoExecute(executionContext);
        }

	}
}
