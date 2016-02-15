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

    public class PAK_PackingCombinePallet : MarshalByRefObject, IPAK_PackingCombinePallet
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetDetail(string Connection, string type, string selectTxt)
        {
            string strSQL = @"select a.DeliveryNo,
                                                   d.Model,
                                                   d.Qty as TotalQty,
                                                   d.Qty-e.q as [NG Qty],
                                                   d.ShipDate as [Plan GI Date],
                                                   b.InfoValue as [FWD],
                                                   c.InfoValue as [Consolidate],
                                                   a.PalletNo as PLTNO,
                                                   a.DeliveryQty as PLTQty,
                                                   h.q2 as [OK Qty],
                                                   f.Weight,
                                                   ISNULL(Convert(varchar(8),g.Loc),'') as Loc
                                                    from Delivery_Pallet a
                                                    left join  DeliveryInfo b on a.DeliveryNo=b.DeliveryNo and b.InfoType='Carrier'
                                                    left join  DeliveryInfo c on a.DeliveryNo=c.DeliveryNo and c.InfoType='Consolidated' 
                                                    left join  Delivery d on a.DeliveryNo=d.DeliveryNo
                                                    left join  (select COUNT(*) as q ,DeliveryNo from Product 
                                                      where PalletNo like '0%'
                                                      group by DeliveryNo) e 
                                                      on a.DeliveryNo =e.DeliveryNo
                                                    
                                                    left join Pallet f on a.PalletNo=f.PalletNo
                                                    left join PAK_WH_LocMas g on g.PLT1=a.PalletNo 
                                                    left join (select COUNT(*) as q2 ,DeliveryNo,PalletNo from Product 
                                                              
                                                              group by DeliveryNo,PalletNo) h 
                                                              on a.DeliveryNo =e.DeliveryNo and a.PalletNo=h.PalletNo ";
                                                   // where c.InfoValue='0100698811/41'
                                                   //--where a.PalletNo='0129001675'
                                                  // --  where a.DeliveryNo='4108425207000010' and a.PalletNo like '0%'";

            string strTmp = "";
           
            if (type == "DN")
            {
                strTmp = "where a.DeliveryNo=@selectTxt";
            
            }
            else if (type == "Consolidated")
            {
                strTmp = "where c.InfoValue=@selectTxt";
            }
            else // type="Pallet
            {
                strTmp = "where a.PalletNo=@selectTxt";
     
            }
            strSQL = strSQL + strTmp;
            return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@selectTxt",32, selectTxt));
            
           
           
         
        }

        public DataTable GetQueryResult(string Connection, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"sp_Query_PAK_PakingCombinePallet";
                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                SQLText,
                                                SQLHelper.CreateSqlParameter("@shipDate", shipDate));
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
