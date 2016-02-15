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

    public class PAK_PakBufferQuery : MarshalByRefObject, IPAK_PakBufferQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string DN, string type, bool IsUnWeight, DateTime fromDate, DateTime toDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
          
            BaseLog.LoggingBegin(logger, methodName);
            string SQLText = "";
            try
            {
                if (type == "DN")
                {
                    SQLText = @"select a.* , b.actual as [Product Qty], c.Weight as [含棧板重],c.Weight_L as [不含棧板重]
                                            from 
                                             (select PalletNo,Sum(DeliveryQty) as Qty from Delivery_Pallet  WITH(NOLOCK) 
                                                where 
                                                 DeliveryNo like '{0}%' and PalletNo not like 'NA%'  
                                                 group by PalletNo) a
                                            left join (select PalletNo,  count(*)  as actual from Product  WITH(NOLOCK)  
                                             group by PalletNo) b on b.PalletNo = a.PalletNo
                                            left join (select PalletNo ,Weight,Weight_L from Pallet) c on c.PalletNo = a.PalletNo ";
                }
                else if (type == "Shipment")
                {
                    SQLText = @"select a.* , b.actual as [Product Qty], c.Weight as [含棧板重],c.Weight_L as [不含棧板重]
                                                    from 
                                                     (select PalletNo,Sum(DeliveryQty) as Qty from Delivery_Pallet  WITH(NOLOCK) 
                                                        where 
                                                         DeliveryNo in
                                                          (
                                                          select DeliveryNo from  DeliveryInfo    WITH(NOLOCK) 
                                                           where  InfoValue  like '{0}%'
                                                          )
                                                          and PalletNo not like 'NA%'  
                                                    group by PalletNo) a
                                                    left join (select PalletNo,  count(*)  as actual from Product  WITH(NOLOCK) 
                                                     group by PalletNo) b on b.PalletNo = a.PalletNo
                                                    left join (select PalletNo ,Weight,Weight_L from Pallet  WITH(NOLOCK) ) c on c.PalletNo = a.PalletNo ";
                }
                else if (type == "Pallet")
                {
                    SQLText = @"select  a.* , b.actual as [Product Qty], c.Weight as [含棧板重],c.Weight_L as [不含棧板重] from 
                                      (
                                        select PalletNo,Sum(DeliveryQty) as Qty from Delivery_Pallet  WITH(NOLOCK) 
                                         where PalletNo='{0}'  group by PalletNo
                                       ) a
                                       left join (select PalletNo,  count(*)  as actual from Product  WITH(NOLOCK) 
                                             group by PalletNo) b on b.PalletNo = a.PalletNo
                                            left join (select PalletNo ,Weight,Weight_L from Pallet  WITH(NOLOCK) ) c on c.PalletNo = a.PalletNo ";

                }
                else //type= Ship Date
                {
                    SQLText = @"select a.* , ISNULL(b.actual,0) as  [Product Qty],c.Weight as [含棧板重],c.Weight_L as [不含棧板重]
                                        from 
                                         (select PalletNo,Sum(DeliveryQty) as Qty from Delivery_Pallet  WITH(NOLOCK) 
                                            where 
                                             DeliveryNo
                                              in( 
                                                  select DeliveryNo from 
                                                  Delivery  WITH(NOLOCK) 
                                                  where ShipDate between @fromDate and @toDate
                                                 )
                                         
                                          and PalletNo not like 'NA%'  
                                             group by PalletNo) a
                                        left join (select PalletNo,  count(*)  as actual from Product  WITH(NOLOCK) 
                                         group by PalletNo) b on b.PalletNo = a.PalletNo
                                        left join (select PalletNo ,Weight,Weight_L from Pallet WITH(NOLOCK) ) c on c.PalletNo = a.PalletNo ";
                
                
                }
                if (type != "Ship Date")
                {
                    SQLText = string.Format(SQLText, DN);
                }
                if (IsUnWeight)
                {
                    SQLText += " where c.Weight_L=0 ";
                }

                return SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,SQLText,
                                                         SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                         SQLHelper.CreateSqlParameter("@toDate", toDate)
                                                      );

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
