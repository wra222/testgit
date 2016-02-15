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
    public class FA_IMG_2PPQuery : MarshalByRefObject, IFA_IMG_2PPQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection,
                            IList<string> lstPdLine, IList<string> Model, bool IsWithoutShift, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                
                sb.AppendLine("SELECT DISTINCT a.ProductID,a.CUSTSN,b.InfoValue as 'IMGCL',c.Line ");
                sb.AppendLine("FROM Product a  (NOLOCK) ");
                sb.AppendLine("INNER JOIN ProductInfo b (NOLOCK) ON b.ProductID = a.ProductID ");
                sb.AppendLine("INNER JOIN ProductStatus c (NOLOCK) ON c.ProductID = a.ProductID ");
                sb.AppendLine("WHERE  b.InfoType='IMGCL' ");

                if (lstPdLine.Count > 0)
                {
                    if (IsWithoutShift)
                    {
                        sb.AppendFormat("AND SUBSTRING(c.Line,1,1) in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                    else
                    {
                        sb.AppendFormat("AND c.Line in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                }
                if (Model.Count > 0)
                {
                    sb.AppendFormat("AND a.ProductID in ('{0}') ", string.Join("','", Model.ToArray()));
                }
                if (ModelCategory != "")
                {
                    sb.AppendFormat(" AND dbo.CheckModelCategory(a.Model,'" + ModelCategory + "')='Y' ");
                }              
                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText
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

        public DataTable GetQueryResultByModel(string Connection, 
                            IList<string> lstPdLine,  IList<string> Model, bool IsWithoutShift, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT a.ProductID,a.CUSTSN,b.InfoValue as '2PPCL',c.Line ");
                sb.AppendLine("FROM Product a  (NOLOCK) ");
                sb.AppendLine("INNER JOIN ProductInfo b (NOLOCK) ON b.ProductID = a.ProductID ");
                sb.AppendLine("INNER JOIN ProductStatus c (NOLOCK) ON c.ProductID = a.ProductID ");
                sb.AppendLine("WHERE  b.InfoType='2PPCL'  ");

                if (lstPdLine.Count > 0)
                {
                    if (IsWithoutShift)
                    {
                        sb.AppendFormat("AND SUBSTRING(c.Line,1,1) in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                    else
                    {
                        sb.AppendFormat("AND c.Line in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                }
                if (Model.Count > 0)
                {
                    sb.AppendFormat("AND a.ProductID in ('{0}') ", string.Join("','", Model.ToArray()));
                }
                if (ModelCategory != "")
                {
                    sb.AppendFormat(" AND dbo.CheckModelCategory(a.Model,'" + ModelCategory + "')='Y' ");
                }

                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText                                              
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
