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

    public class PAK_PLTDimension : MarshalByRefObject, IPAK_PLTDimension
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string input, string type, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string subSQL = "";
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select    p.DeliveryNo,
                           b.Model,
                           d.InfoValue as Country,
                           d2.InfoValue as ShipWay,
                           p.PalletNo as PLTNO,
                          e.Weight,
                           a.Len as [Len],
                           a.High as [Height],
                           a.Wide as [Width],
                           Replace(CONVERT(VARCHAR(19), a.Cdt, 120),'-','/') as Cdt
              from Delivery_Pallet p  WITH(NOLOCK) 
                     left join Pallet e on p.PalletNo=e.PalletNo
                     left join PLTStandard a on p.PalletNo=a.PLTNO
                     left join DeliveryInfo d on  d.DeliveryNo=p.DeliveryNo
                     left join DeliveryInfo d2 on  d2.DeliveryNo=p.DeliveryNo
                     left join Delivery b on  p.DeliveryNo=b.DeliveryNo
                where 
                       d.InfoType='Country' 
                and  d2.InfoType='ShipWay'   ";
                switch (type)
                { 
                    case "Pallet" :
                        subSQL = " and p.PalletNo=@input";
                        break;
                    case "DN":
                        subSQL = " and p.DeliveryNo=@input";
                        break;
                    case "Model":
                        if (input == "")
                        { subSQL = " and b.ShipDate=@shipDate";}
                        else
                        {
                            subSQL = " and b.ShipDate=@shipDate and b.Model=@input"; 
                          
                        }
                        break;
                       
                }
                SQLText = SQLText + subSQL;
                //b.ShipDate=@shipDate
//                                                and b.Model=@model
//                                                and d.InfoType='Country' 
//                                                and d2.InfoType='ShipWay' 
                                               
                                               
                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                                SQLText,
                                                SQLHelper.CreateSqlParameter("@shipDate", shipDate),
                                                SQLHelper.CreateSqlParameter("@input", 32, input));
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
