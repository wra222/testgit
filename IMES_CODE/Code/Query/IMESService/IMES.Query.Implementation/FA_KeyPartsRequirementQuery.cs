using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using System.Collections;
namespace IMES.Query.Implementation
{
    public class FA_KeyPartsRequirementQuery : MarshalByRefObject, IFA_KeyPartsRequirementQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetData(string DBConnection, string models, out string outputmodels)
        {

            DataTable dt = new DataTable();


            SqlParameter[] paramsArray = new SqlParameter[2];
            paramsArray[0] = new SqlParameter("Models", models);
            paramsArray[1] = new SqlParameter("OutputModels", SqlDbType.VarChar, 5000);
            paramsArray[1].Direction = System.Data.ParameterDirection.Output;
            dt = ExecSpForQuery(DBConnection, "op_FAKitting_KPRequirement", paramsArray);
            outputmodels = paramsArray[1].Value.ToString();

            return dt;

        }


        public DataTable ExecSpForQuery(string DBConnection, string spName, params SqlParameter[] paramsArray)
        {
            try
            {
                DataTable ret = null;
                ret = SQLHelper.ExecuteDataFill(DBConnection, CommandType.StoredProcedure, spName, paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}