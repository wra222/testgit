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

    public class PAK_PackingDataQuery : MarshalByRefObject,IPAK_PackingDataQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection,string line,string model,string station,bool excludeShipping,DateTime fromDate,DateTime toDate,string BT)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText= @"select c.Line,a.Station,b.Model,b.ProductID,b.CUSTSN,c.Cdt from ProductStatus a
                                                 left join Product b
                                                 on a.ProductID=b.ProductID
                                                 left join
                                                   ( select * from ProductLog where Station='85' ) c
                                                  on a.ProductID=c.ProductID

                                                 where
                                                   a.Station in({0}) 
                                                   and a.Cdt between @fromDate and @toDate ";
                if (line != null)
                {
                    SQLText = SQLText + " and c.Line like '" + line + "%'";
                }
                //    and  b.Model in({1})
                if(!string.IsNullOrEmpty(model))
                {
                    SQLText += " and  b.Model in(" + model + ")";
                }
                SQLText = string.Format(SQLText,station);
                if (BT == "BT")
                { SQLText = SQLText + " and a.ProductID  in(select ProductID from ProductBT)"; }
                if (BT == "No BT")
                { SQLText = SQLText + " and a.ProductID not in(select ProductID from ProductBT)"; }
//                string SQLText = @" select
//                                                    b.Line,
//                                                    b.Station as WC,
//                                                    a.Model,
//                                                    a.ProductID,
//                                                    a.CUSTSN,a.PalletNo as PLT,
//                                                    Isnull(d.Loc,'') as Loc,
//                                                    Replace(CONVERT(VARCHAR(19), b.Udt, 120),'-','/') as Udt
//                                                from Product a 
//                                                      inner join ProductStatus b on  a.ProductID = b.ProductID
//                                                      left join PAK_WH_LocMas d on a.PalletNo = d.PLT1
//                                                where a.ProductID in
//                                                    (select distinct c.ProductID   
//                                                        from  ProductLog c
//                                                      where c.Station =@station
//                                                         and c.Cdt between @fromDate and @toDate
//                                                         and c.Line=@line)
//                                                         and a.Model=@model ";
                if (excludeShipping) 
                { SQLText = SQLText + "and a.DeliveryNo in (select DeliveryNo from Delivery where Status='98')"; }
                return SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,
                                                      SQLText,
                                                      SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                      SQLHelper.CreateSqlParameter("@toDate", toDate),
                                                 
                                                      SQLHelper.CreateSqlParameter("@station", 32, station));
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
