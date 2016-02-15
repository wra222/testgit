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
using System.Data.Sql;
using System.Data.SqlClient;

namespace IMES.Query.Implementation
{
    public class FA_SQEReport : MarshalByRefObject, IFA_SQEReport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string qType, string fromDate, string toDate, string materialType, string kp)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                DateTime dateToday = DateTime.Now;
                DateTime dateFrom = new DateTime(Convert.ToInt16(fromDate.Substring(0, 4)), Convert.ToInt16(fromDate.Substring(5, 2)), Convert.ToInt16(fromDate.Substring(8, 2)));
                DateTime dateTo = new DateTime(Convert.ToInt16(toDate.Substring(0, 4)), Convert.ToInt16(toDate.Substring(5, 2)), Convert.ToInt16(toDate.Substring(8, 2)));

                if (dateFrom > dateTo)
                    throw new Exception("DateTo 需大於 DateFrom");

                if (dateTo > dateToday)
                    throw new Exception("Date 需小於或等於系統日期");

                if (materialType != "All")
                {
                    materialType = " and MaterialType='" + materialType + "' ";
                }
                else
                    materialType = "";

                if (kp != "")
                {
                    kp = " and KP='" + kp + "' ";
                }

                string SQLText = @"
select Site, Plant, MaterialType, dbo.GetQueryDateType(@qType, Period_Date) Period_Date, KP,
VDCT, Family, IEC_PN, Vendor, InputQty from View_MaterialUse_SQE
where Period=@qType and (Period_Date between dbo.GetStartPeriodDate(@qType, 'D', @fromDate) and dbo.GetEndPeriodDate(@qType, 'D', @toDate) ) "
+ materialType + kp +
" order by Site, Plant, MaterialType, Period_Date";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 new SqlParameter("@qType", qType),
                                                 new SqlParameter("@fromDate", fromDate),
                                                 new SqlParameter("@toDate", toDate));
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
		
		public DataTable GetKpByMaterialType(string Connection, string  materialType)
		{
			string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (!(materialType == "EE" || materialType == "ME"))
				{
					return null;
				}

                string SQLText = @"Select SQEType from SQEType Where MaterialType=@MaterialType order by SQEType";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 new SqlParameter("@MaterialType", materialType));
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
