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
    public class PAK_COAStoreQuery : MarshalByRefObject, IPAK_COAStoreQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetCOAStoreQueryResult(string Connection,bool flag_COA)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                if (flag_COA)     //COA Win7
                {
                    sb.AppendLine(@"select IECPN,count(DISTINCT COASN) as Qty from COAStatus (nolock)
                                    where Status NOT IN('01','03','05','16','A1') and COASN not in 
                                        (select a.COASN from COAStatus a,COATrans_Log b (nolock)
                                        where a.Status NOT IN('01','03','05','16','A1') 
                                        and a.COASN between b.BegNo and b.EndNo 
                                        and b.PreStatus ='RE' and b.Status ='01') group by IECPN");
                }
                else           //COA Win8
                {
                    sb.AppendLine(@"select b.Component,COUNT(Distinct(a.InfoValue))
                                    from ProductInfo a,ModelBOM b,Part c (nolock)
                                    where a.InfoType='Key' and a.ProductID in(
                                        select ProductID
                                        from Product (nolock)
                                        where DeliveryNo in(
                                            select DeliveryNo 
                                            from Delivery (nolock)
                                            where Status<>'98'))
                                            and b.Material in 
                                                (select c.Model 
                                                from Product c (nolock)
                                                where a.ProductID=c.ProductID) 
                                                and c.PartNo=b.Component and c.Descr LIKE 'ECOA%' group by b.Component");
                }


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
