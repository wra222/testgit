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
   public class PAK_CheatSheet : MarshalByRefObject, ICheatSheet
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string input, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string subSQL = "";
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"exec sp_CheatSheet @shipDate ";

                //switch (type)
                //{
                //    case "Pallet":
                //        subSQL = " and p.PalletNo=@input";
                //        break;
                //    case "DN":
                //        subSQL = " and p.DeliveryNo=@input";
                //        break;
                //    case "Model":
                //        if (input == "")
                //        { subSQL = " and b.ShipDate=@shipDate"; }
                //        else
                //        {
                //            subSQL = " and b.ShipDate=@shipDate and b.Model=@input";

                //        }
                //        break;

                //}
                if (input != "")
                { subSQL = " ,@input"; }
                else
                { subSQL = " ,''";  }
                SQLText = SQLText + subSQL;
                //b.ShipDate=@shipDate
                //                                                and b.Model=@model
                //                                                and d.InfoType='Country' 
                //                                                and d2.InfoType='ShipWay' 


                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                                SQLText,
                                                SQLHelper.CreateSqlParameter("@shipDate", shipDate),
                                                SQLHelper.CreateSqlParameter("@input",32, input));
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
