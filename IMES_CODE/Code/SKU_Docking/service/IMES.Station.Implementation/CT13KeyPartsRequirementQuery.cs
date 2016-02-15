using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.FA.Product;
using System.Data.SqlClient;
using System.Data;

namespace IMES.Station.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CT13KeyPartsRequirementQuery : MarshalByRefObject, ICT13KeyPartsRequirementQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly Session.SessionType theType = Session.SessionType.Common;


        #region CT13IKeyPartsRequirementQuery Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="outputmodels"></param>
        /// <returns></returns>
        public DataTable KeyPartQuery(string models, out string outputmodels)
        {
            DataTable dt = new DataTable();

            
            SqlParameter[] paramsArray = new SqlParameter[2];
            paramsArray[0] = new SqlParameter("Models", models);
            paramsArray[1] = new SqlParameter("OutputModels",  SqlDbType.VarChar, 5000);
            paramsArray[1].Direction = System.Data.ParameterDirection.Output;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_FA, "dbo.op_CT13Offline_KPRequirement", paramsArray);
            outputmodels = paramsArray[1].Value.ToString();
            
            /*
            //For Test
            outputmodels = "";
            dt.Columns.Add("Part");
            dt.Columns.Add("SPS");
            dt.Columns.Add("Count");
            for (int i = 0; i < 1; i++)
            {
                DataRow newRow;
                newRow = dt.NewRow();
                newRow[0] = "Part" + i.ToString();
                newRow[1] = "SPS" + i.ToString();
                newRow[2] = "Count" + i.ToString();
                dt.Rows.Add(newRow);

                outputmodels = outputmodels + "model" + i.ToString() + ":" + i.ToString() + ";";
            }
            //outputmodels = "model1:1;model2:2;model3:3;";
            //For Test
            */
            return dt;
        }
        #endregion


    }
}
