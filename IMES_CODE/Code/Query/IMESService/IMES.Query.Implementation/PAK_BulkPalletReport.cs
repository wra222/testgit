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

    public class PAK_BulkPalletReport : MarshalByRefObject, IPAK_BulkPalletReport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime shipDate, string type, string PAKStation,string kind)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                string stemp = "";
                string stemp2="";
                if (kind == "ALL")
                {
                    
                  stemp = @"   left join ProductStatus g on p.ProductID=g.ProductID
                                                           and 
                                                          g.Station in('9A','99')
                                           left join ProductStatus f on p.ProductID=f.ProductID
                                                           and 
                                                          f.Station in(select value from fn_split('{0}',','))";
                stemp2 = @"            
                                           case when g.ProductID is null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as Verify,
                                           case when f.ProductID is null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as 'Pass 85',
                                                                                ";
                }
                if (kind == "OK")
                {
                    stemp = @"   inner join ProductStatus g on p.ProductID=g.ProductID
                                                           and 
                                                          g.Station in('9A','99')
                                           left join ProductStatus f on p.ProductID=f.ProductID
                                                           and 
                                                          f.Station in(select value from fn_split('{0}',','))";
                    stemp2 = @"            
                                           case when g.ProductID is null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as Verify,
                                           case when f.ProductID is null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as 'Pass 85',
                                                                                ";
                
                }
                if (kind == "NG")
                {
                    stemp = @"   right join ProductStatus g on p.ProductID=g.ProductID
                                                           and 
                                                          g.Station not in('9A','99')
                                           left join ProductStatus f on p.ProductID=f.ProductID
                                                           and 
                                                          f.Station in(select value from fn_split('{0}',','))";
                    stemp2 = @"            
                                           case when g.ProductID is not null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as Verify,
                                           case when f.ProductID is null
                                                        then (select Station from ProductStatus where ProductID=p.ProductID)
                                                           else '1'
                                                        end as 'Pass 85',
                                                                                ";
                
                
                }
                if (type == "Detail")
                {
                    stemp = string.Format(stemp, PAKStation);

                    SQLText = @" select 
                                                      CONVERT(VARCHAR(19),b.ShipDate, 111) as ShipDate,
                                                       p.DeliveryNo as DN, 
                                                       d2.InfoValue as FWD,
                                                       p.Model,CUSTSN,
                                                       p.PalletNo as PLT,"+
                                                      stemp2+
                                                      @"d.InfoValue as BOL,
                                                       y.PLT as 虛擬站板號
                                                          from Product p " +
                                                          stemp +
                                                              @" left join DeliveryInfo d on p.DeliveryNo=d.DeliveryNo and d.InfoType='BOL'
                                                          left join DeliveryInfo d2 on p.DeliveryNo=d2.DeliveryNo and d2.InfoType='Carrier'
                                                          left join Delivery b on p.DeliveryNo=b.DeliveryNo
                                                          left join Dummy_ShipDet y on y.SnoId=p.ProductID                          
                                                 where ShipDate=@shipDate
                                                        and b.DeliveryNo in
                                                        (select DeliveryNo From Delivery_Pallet where PalletNo like 'NA%' or PalletNo like 'BA%')";
                  

                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.Text,
                                                    SQLText,
                                                    SQLHelper.CreateSqlParameter("@shipDate", shipDate));
                
                }
                else
                {
                    SQLText = @"sp_Query_PAK_RealTimePackingRpt ";

                    DataTable dt= SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    SQLText,
                                                    SQLHelper.CreateSqlParameter("@shipDate", shipDate),
                                                    SQLHelper.CreateSqlParameter("@PAKStation", 512, PAKStation));

                 
                    return dt;

                
                }
           
                
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
