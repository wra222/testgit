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
    public class PAK_ChepPallet: MarshalByRefObject, IPAK_ChepPallet
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
//                string SQLText = @"SELECT b.PalletNo,'' as REIDPalletNo ,c.InfoValue as Carrier ,a.Editor,a.Cdt,a.Udt 
//                                   FROM Delivery a (NOLOCK) INNER JOIN  Delivery_Pallet b (NOLOCK) 
//                                   ON a.DeliveryNo=b.DeliveryNo
//                                   LEFT JOIN DeliveryInfo c  (NOLOCK)
//                                   ON a.DeliveryNo=c.DeliveryNo and c.InfoType='Carrier'
//                                   AND a.Cdt BETWEEN @FromDate AND @ToDate
//                                   ";         
                string SQLText = @"select PLT as PalletNo,
                                                           RFIDCode as RFIDPalletNo,
                                                           Carrier as Carrier,
                                                           Editor as Editor,
                                                           Cdt,
                                                           Udt  
                                                    from Pallet_RFID WITH(NOLOCK)
                                                    where Cdt BETWEEN @FromDate AND @ToDate";

//                string SQLText = @"select a.PLT as PalletNo,
//                                                           a.RFIDCode as RFIDPalletNo,
//                                                           isnull(c.DeliveryNo,'') as DeliveryNo,
//                                                           c.ShipDate as ShipDate,
//                                                           isnull(c.Model,'') as Moddel,
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
