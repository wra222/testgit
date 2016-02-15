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

    public class PAK_StatusByDeliveryNo : MarshalByRefObject, IPAK_StatusByDeliveryNo
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, List<string> deliveryNo,DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select ProductID,CUSTSN,Model,PCBID,CartonSN,DeliveryNo,PalletNo,Station as WC,
                                                 Line,UnitWeight
                                                from view_wip_station  ";
                 DataTable dt;
                if(deliveryNo.Count==0)
                {
                  SQLText+="  where ShipDate=@shipDate  ";
                  dt= SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,SQLText,
                                                       SQLHelper.CreateSqlParameter("@shipDate", shipDate));

                }
                else
                {
                     string stemp = "";
                     string[] arr = deliveryNo.ToArray();
                    if (arr.Length > 0)
                    { stemp = "DeliveryNo like '" + string.Join("%' OR DeliveryNo like '", arr) + "%'"; }
                   SQLText+=" where " + stemp;
                  dt= SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,SQLText);
                
                }
        
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
