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
using IMES.Entity.Repository.Meta.IMESSKU;
using IMES.Entity.Infrastructure.Framework;
using IMES.Entity.Infrastructure.Interface;

namespace IMES.Query.Implementation
{
   public class SARepairWIP_Dashboard : MarshalByRefObject, ISARepairWIP_Dashboard
    {
       private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       public DataTable GetQueryResult(string DBConntion)
       {
           string methodName = MethodBase.GetCurrentMethod().Name;
           BaseLog.LoggingBegin(logger, methodName);
           try
           {
               DataTable Result = null;
               StringBuilder sb = new StringBuilder();
               sb.AppendLine("exec op_SARepairWIP ");
               Result = SQLHelper.ExecuteDataFill(DBConntion, System.Data.CommandType.Text,
                                                    sb.ToString());
               return Result;
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
