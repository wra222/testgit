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

    public class PAK_BTLocQuery : MarshalByRefObject,IPAK_BTLocQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string line, string input, string inputType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                DataTable dt=null;
                if (string.IsNullOrEmpty(line) && string.IsNullOrEmpty(input))
                {
                    sb.Append("Select SnoId as LocID,Model,PdLine,Status,CmbQty from PAK_BTLocMas WITH(NOLOCK) order by LocID");
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                              System.Data.CommandType.Text,sb.ToString());
                }
                else if (!string.IsNullOrEmpty(line))
                { 
#if(DEBUG)
                    sb.Append(@"select top 50 a.PdLine,a.Sno as LocID,CPQSNO,b.Model,a.Cdt from SnoDet_BTLoc a WITH(NOLOCK),
                                        PAK_BTLocMas b WITH(NOLOCK) where a.Sno=b.SnoId and b.Tp='BTLoc' and a.PdLine=@PdLine");
                   dt = SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                               sb.ToString(),
                                               SQLHelper.CreateSqlParameter("@PdLine", 32, line));
#else
                    sb.Append(@"select a.PdLine,a.Sno as LocID,CPQSNO,Model,a.Cdt from SnoDet_BTLoc a,
                                        PAK_BTLocMas b where a.Sno=b.SnoId and b.Tp='BTLoc' and a.PdLine=@PdLine ");

                    if (!string.IsNullOrEmpty(input.Trim()))
                    {
                        string tmp;
                        tmp = inputType == "CPQSNO" ? "and CPQSNO=@input " : "and Model=@input ";
                        sb.Append(tmp);
                        dt = SQLHelper.ExecuteDataFill(Connection,
                                                   System.Data.CommandType.Text,
                                                  sb.ToString(),
                                                  SQLHelper.CreateSqlParameter("@PdLine", 32, line),
                                                  SQLHelper.CreateSqlParameter("@input", 32, input));

                    }
                    else
                    {
                        dt = SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.Text,
                                                    sb.ToString(),
                                                    SQLHelper.CreateSqlParameter("@PdLine", 32, line));
                    }
                    

#endif
                }
                else if (inputType == "CPQSNO")
                {
                    sb.Append(@"select a.CPQSNO,a.Sno as LocID,a.Status,a.PdLine,b.Model
                                                 from SnoDet_BTLoc a WITH(NOLOCK),PAK_BTLocMas b WITH(NOLOCK) where a.Sno=b.SnoId and
                                                    a.CPQSNO=@input and b.Tp='BTLoc'");
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                                   System.Data.CommandType.Text,
                                                  sb.ToString(),
                                                  SQLHelper.CreateSqlParameter("@input", 32, input));


                }
                else
                {
                    sb.Append(@"Select SnoId as LocID,Model,PdLine,Status,CmbQty from PAK_BTLocMas WITH(NOLOCK) where Model=@input
                                        order by LocID");
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                               sb.ToString(),
                                               SQLHelper.CreateSqlParameter("@input", 32, input));

                
                
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

        public DataTable GetDetailResult(string Connection, string LocID)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;
            try
            {
                sb.Append(@"select a.Sno as LocID,CPQSNO,Model,a.PdLine,a.Cdt
                                     from SnoDet_BTLoc a join  PAK_BTLocMas b on a.Sno=b.SnoId
                                    where a.Sno=@LocID");
                dt = SQLHelper.ExecuteDataFill(Connection,
                                          System.Data.CommandType.Text,
                                         sb.ToString(),
                                         SQLHelper.CreateSqlParameter("@LocID", 32, LocID));
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
