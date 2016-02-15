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
    public class FA_QueryByPartSN : MarshalByRefObject,IFA_QueryByPartSN
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetProduct(string Connection,string id)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select ProductID,Model,PCBID,MAC,CUSTSN,PizzaID,DeliveryNo,PalletNo from Product where ProductID=@id";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText, SQLHelper.CreateSqlParameter("@id", 32, id));
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
        public System.Data.DataTable GetInfo(string Connection,string PartSn)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select ProductID,PartNo,PartSn,Station,Editor,Cdt from Product_Part where PartSn=@PartSn
                                                union all
                                               select b.ProductID,PartNo,PartSn,Station,Editor,a.Cdt 
                                                from Pizza_Part a, Product b
                                                where a.PartSn=@PartSn  and a.PizzaID=b.PizzaID  ";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText, SQLHelper.CreateSqlParameter("@PartSn", 32, PartSn));
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
