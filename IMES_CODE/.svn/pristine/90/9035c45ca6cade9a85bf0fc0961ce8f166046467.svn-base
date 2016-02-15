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
    public class PAK_BsamLocationQuery : MarshalByRefObject, IPAK_BsamLocationQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetResultForSummary(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select LocationId, Model, Qty ");
                sb.AppendLine("from BSamLocation ");
                sb.AppendLine("where Qty > 0 ");
                sb.AppendLine("order by LocationId, Model ");

                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);

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
        public DataTable GetResultForDetail(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select distinct b.LocationId, b.Model, d.CUSTSN, e.Line, e.Station, a.Udt ");
                sb.AppendLine("from ProductAttr a ");
                sb.AppendLine("inner join BSamLocation b on b.LocationId = a.AttrValue ");
                sb.AppendLine("inner join ProductLog c on c.Station='81' and a.ProductID=c.ProductID ");
                sb.AppendLine("inner join Product d on a.ProductID=d.ProductID ");
                sb.AppendLine("inner join ProductStatus e on a.ProductID=e.ProductID");
                sb.AppendLine("where a.AttrName='CartonLocation' and a.AttrValue !='' ");
                sb.AppendLine("order by b.LocationId, b.Model ");

                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);
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
