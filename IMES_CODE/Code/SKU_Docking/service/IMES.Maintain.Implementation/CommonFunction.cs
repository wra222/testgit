using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using log4net;
using System.Data;

namespace IMES.Maintain.Implementation
{
    public class CommonFunction : MarshalByRefObject, ICommonFunction       
    {
        #region IExecSP Members
        ILog ExecSQLLogger = LogManager.GetLogger("ExecSQLLogger");

        public DataTable GetSPResult(string editor, string dbName, string spName, string[] ParameterNameArray, string[] ParameterValueArray)
        {
            if (ParameterNameArray == null || ParameterValueArray == null)
            {
                return null;
            }

            string ParameterValuesStr = "";
            System.Data.SqlClient.SqlParameter[] paramsArray = new System.Data.SqlClient.SqlParameter[ParameterNameArray.Length];
            for (int i = 0; i < ParameterNameArray.Length; i++)
            {
                paramsArray[i] = new System.Data.SqlClient.SqlParameter(ParameterNameArray[i].StartsWith("@") ? ParameterNameArray[i] : "@" + ParameterNameArray[i], ParameterValueArray[i]);
                ParameterValuesStr = ParameterValuesStr + paramsArray[i].ParameterName + "=" + ParameterValueArray[i] + ",";
            }
            ExecSQLLogger.Info("editor is: " + editor + ", Database is: " + dbName + ", SPName is: " + spName + ", Parameters is :" + ParameterValuesStr.TrimEnd(new char[] { ',' }));


            IProductRepository MyRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            string ConnectString = string.Format(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString, dbName);
            try
            {
                return IMES.Infrastructure.Repository._Schema.SqlHelper.ExecuteDataFillConsiderOutParams(ConnectString, CommandType.StoredProcedure, spName, paramsArray);
            }
            catch (Exception e)
            {
                ExecSQLLogger.Error(e.Message, e);
                throw e;
            }
        }

        public DataSet GetSQLResult(string editor, string dbName, string sqlText, string[] ParameterNameArray, string[] ParameterValueArray)
        {
            ExecSQLLogger.Info("editor is: " + editor + ", Database is: " + dbName + ", ExecSQL is: " + "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + sqlText);
            System.Data.SqlClient.SqlParameter[] paramsArray = null;
            if (ParameterNameArray != null && ParameterValueArray != null && ParameterNameArray.Length == ParameterValueArray.Length)
            {
                paramsArray = new System.Data.SqlClient.SqlParameter[ParameterNameArray.Length];
                for (int i = 0; i < ParameterNameArray.Length; i++)
                {
                    paramsArray[i] = new System.Data.SqlClient.SqlParameter(ParameterNameArray[i].StartsWith("@") ? ParameterNameArray[i] : "@" + ParameterNameArray[i], ParameterValueArray[i]);
                }
            }

            sqlText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; \n" + sqlText + " select @@ROWCOUNT as AffectRowCount";
            string ConnectString = string.Format(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString, dbName);
            if (string.IsNullOrEmpty(dbName))
            {
                ConnectString = IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_FA;
            }

            IProductRepository MyRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            try
            {
                return IMES.Infrastructure.Repository._Schema.SqlHelper.ExecSPorSql(ConnectString, CommandType.Text, sqlText, paramsArray);
            }
            catch (Exception e)
            {
                ExecSQLLogger.Error(e.Message, e);
                throw e;
            }
        }

        #endregion
    }

}
