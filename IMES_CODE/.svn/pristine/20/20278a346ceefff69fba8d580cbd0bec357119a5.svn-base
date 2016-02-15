using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Data;
using System.Reflection;

namespace IMES.Query.Implementation
{
    public class PAK_MrpQuery: MarshalByRefObject,IPAK_MrpQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string GetMrpLabelByDN(string Connection, string DeliveryNoOrCustsn)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string ret="" ;
            try
            {

              
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("exec sp_Query_PAKMRPLabel @DNORSN ");
                DataTable dt = new DataTable();
                dt = SQLHelper.ExecuteDataFill(Connection,
                                             System.Data.CommandType.Text,
                                             sb.ToString(),
                                             SQLHelper.CreateSqlParameter("@DNORSN", DeliveryNoOrCustsn.Trim()));
                if (dt.Rows.Count != 0)
                    {
                        ret = dt.Rows[0][0].ToString(); 
                    }
                else
                    {
                        ret = "no data";
                    }
                //if (DeliveryNoOrCustsn.Trim().StartsWith("5CG"))
                //{
                //    StringBuilder sb1 = new StringBuilder();
                //    sb1.AppendLine("SELECT DeliveryNo FROM dbo.Product WHERE CUSTSN=@CPQSNO ");
                //    DataTable dt = new DataTable();
                //    dt= SQLHelper.ExecuteDataFill(Connection,
                //                                 System.Data.CommandType.Text,
                //                                 sb1.ToString(),
                //                                 SQLHelper.CreateSqlParameter("@CPQSNO", DeliveryNoOrCustsn.Trim()));
                //    if (dt.Rows.Count == 0)
                //    {
                //        ret= "此SN 未结合船务  " + DeliveryNoOrCustsn;
                //    }
                //    else
                //    {
                //        DeliveryNoOrCustsn = dt.Rows[0][0].ToString(); 
                //    }
                //}
                //StringBuilder sb2 = new StringBuilder();
                //sb2.AppendLine("select INDIA_GENERIC_DESC , INDIA_PRICE ,INDIA_PRICE_ID from HPEDI.dbo.PAK_PAKComn (nolock) where InternalID=@DN ");
                //DataTable DT1= SQLHelper.ExecuteDataFill(Connection,
                //                           System.Data.CommandType.Text,
                //                           sb2.ToString(),
                //                           SQLHelper.CreateSqlParameter("@DN", DeliveryNoOrCustsn.Trim()));
                //if (DT1.Rows.Count == 0)
                //{
                //    ret= "此此船务无资料  " + DeliveryNoOrCustsn;
                //}
                //else
                //{
                //    string INDIA_GENERIC_DESC = DT1.Rows[0]["INDIA_GENERIC_DESC"].ToString();
                //    string INDIA_PRICE = DT1.Rows[0]["INDIA_PRICE"].ToString();
                //    string INDIA_PRICE_ID = DT1.Rows[0]["INDIA_PRICE_ID"].ToString();
                //    if (INDIA_GENERIC_DESC != "" && INDIA_PRICE != "" && INDIA_PRICE_ID != "")
                //    {
                //        ret = DeliveryNoOrCustsn+" 需要貼MPR label Price=" + INDIA_PRICE;
                //    }
                //    else if ( INDIA_GENERIC_DESC == "" && INDIA_PRICE==""&& INDIA_PRICE_ID != "")
                //    { 
                //       ret = DeliveryNoOrCustsn+" 需要貼Internal label" + INDIA_PRICE;
                //    }
                //    else if (INDIA_GENERIC_DESC == "" && INDIA_PRICE == "" && INDIA_PRICE_ID == "")
                //    {
                //        ret = DeliveryNoOrCustsn + "不需要貼MRP&Internal label";
                //    }
                //    else
                //    {
                //        ret = "检查结束";
                //    }

                //}
               
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
            return ret;
        }

       
    }
}
