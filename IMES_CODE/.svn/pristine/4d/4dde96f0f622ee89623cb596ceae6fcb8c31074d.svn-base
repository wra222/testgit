using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;


namespace IMES.Query.Implementation
{
    public class PAK_NonPaletProductQuery : MarshalByRefObject, IPAK_NonPaletProductQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        
        public DataTable GetQueryResult(string DBConnection,  string Line)
        //public DataTable GetQueryResult(string DBConnection, string PalletType, string Line, DateTime ShipDate,string ProductType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
//                string strSQL = @"select a.Model
//                                    from Delivery a,ModelInfo b
//                                    where a.Model =b.Model 
//                                    and a.ShipDate >=GETDATE() -2
//                                    and a.Status ='00' 
//                                    and b.Name='TP' 
//                                    and b.Value ='Commparts'
//                                    order by ShipDate ";
                
//                DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
//                                                    System.Data.CommandType.Text,
//                                                    strSQL,
//                                                    SQLHelper.CreateSqlParameter("@line", 128, Line));
//                return dt;

                string SQLText = @"IMES_NonPalletProductQuery";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    SQLText,
                                                    SQLHelper.CreateSqlParameter("@Line", 128, Line)); 

            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }
    }
}
