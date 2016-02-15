
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
    public class PAK_MaterialBoxQuery : MarshalByRefObject, IPAK_MaterialBoxQuery 
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
//                     
                string SQLText = @"select BoxId , LotNo,SpecNo,FeedType,Qty,[Status],Editor,Cdt 
                                      from  MaterialBox 
                                            where Cdt between @FromDate and @ToDate";

//                string SQLText = @"select a.PLT as PalletNo,
//                                                           a.RFIDCode as RFIDPalletNo,
//                                                           isnull(c.DeliveryNo,'') as DeliveryNo,
//                                                           c.ShipDate as ShipDate,
//                                                           (c.Model,'') as Moddel,
//                                                           isnull(c.PoNo,'') as PoNo,
//                                                           a.Carrier as Carrier,
//                                                           a.Editor as Editor,
//                                                           a.Cdt,
//                                                           a.Udt  
//                                                    from Pallet_RFID a
//                                                    left join Delivery_Pallet b on b.PalletNo = a.PLT
//                                                    left join Delivery c on b.DeliveryNo= c.DeliveryNo 
//                                                    where a.Cdt BETWEEN @FromDate AND @ToDate";
                
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
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

