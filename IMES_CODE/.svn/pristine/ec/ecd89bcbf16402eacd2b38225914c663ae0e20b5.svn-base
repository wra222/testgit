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


namespace IMES.Query.Implementation
{

    public class PAK_ShipLogQuery : MarshalByRefObject, IPAK_ShipLogQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string model, DateTime fromDate, DateTime toDate, string prdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                DataTable dt = null;
                string SQLText ="";
                if(!string.IsNullOrEmpty(model) )
                {
                  SQLText = @" select b.CUSTSN,b.ProductID,b.Model,a.PoNo,CONVERT(VARCHAR(10),a.ShipDate,111) as ShipDate
                                             from Delivery a  WITH(NOLOCK) ,Product b  WITH(NOLOCK) 
                                        where a.Model in({0})
                                             and a.ShipDate between @fromDate and @toDate
                                            and a.DeliveryNo=b.DeliveryNo ";
                  SQLText = string.Format(SQLText, model);
                }
                else
                {
                    SQLText = @" select b.CUSTSN,b.ProductID,b.Model,a.PoNo,CONVERT(VARCHAR(10),a.ShipDate,111) as ShipDate
                                             from Delivery a WITH(NOLOCK) ,Product b WITH(NOLOCK) 
                                        where a.ShipDate between @fromDate and @toDate
                                            and a.DeliveryNo=b.DeliveryNo ";
                }
                if (!string.IsNullOrEmpty(prdType))
                {
                    SQLText += "  and dbo.CheckModelCategory(a.Model,'{0}')='Y' ";
                    SQLText = string.Format(SQLText, prdType);
                }
           
                
               
                dt = SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.Text,
                                                       SQLText,
                                                       SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                       SQLHelper.CreateSqlParameter("@toDate", toDate));
                                                         
                return dt;
           
              

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
