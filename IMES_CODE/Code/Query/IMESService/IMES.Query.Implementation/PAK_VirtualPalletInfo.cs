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

    public class PAK_VirtualPalletInfo : MarshalByRefObject,IPAK_VirtualPalletInfo
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime fromDate, DateTime toDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select a.PLT,a.BOL,b.CUSTSN,b.DeliveryNo as DN,b.Model
                                                   from Dummy_ShipDet a,Product b ,Delivery c
                                                 where a.SnoId=b.ProductID        
                                                       and b.DeliveryNo=c.DeliveryNo
                                                       and c.ShipDate between @fromDate and @toDate
                                                       order by PLT";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                 SQLHelper.CreateSqlParameter("@toDate", toDate));
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
