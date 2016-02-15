using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchiveInterface;
using System.Data;
using ArchiveDB;
using System.Data.SqlClient;
namespace DBSetting
{
    public  class HPDocking:IArchiveInterface
    {
    private const  int  _minDay=30;
    public int GetDayDiffFromMinShipDay(SqlConnection con)
    {
        string cmd = @" SELECT DATEDIFF(DAY,MIN(ShipDate),GETDATE())
                                            From Delivery ";
        DataTable dt = SQLTool.GetDataByDataTable(con, cmd);
        return int.Parse(dt.Rows[0].ToString());


    }
        public string CheckDaySetting(SqlConnection con )
        {
            string errMsg = "";
            string cmd2 = @"    select DATEDIFF(DAY,Max(ShipDate),GETDATE())
                                           from Delivery a WITH(NOLOCK),ArchiveIDList b where
                                           a.DeliveryNo=b.PK_ID
                                           and b.Item='Delivery' ";
            DataTable dt2 = SQLTool.GetDataByDataTable(con, cmd2);
            int dayDif;
            if (int.TryParse(dt2.Rows[0][0].ToString(), out dayDif))
            {
                if (dayDif < _minDay)
                {
                    errMsg = "The Max ShipDate is less than {0} with now";
                    errMsg = string.Format(errMsg, _minDay.ToString());
                }
             
            }
            return errMsg;
        }

        public string GetDeleteSql(string tableName, string deleteCount, string selepTime)
        {
            string sql = "";
            switch (tableName)   
            {
                case "Pizza":
                    sql = @"DELETE  Pizza
                                    where PizzaID in
                                    (
                                     select a.PizzaID from Product a WITH (NOLOCK)
                                       where ProductID in
                                        (
                                          select PK_ID from ArchiveIDList
                                          where Item='Product'
                                        )
                                   )";
                    break;
                case "Pizza_Part":
                    sql = @" DELETE  Pizza_Part where PizzaID in 
                                   (select a.PizzaID from Product a WITH (NOLOCK) where ProductID in
                                   (select PK_ID from ArchiveIDList where Item='Product'))";
                
                    break;
                case "COALog":
                    sql = @" DELETE  COALog where COASN in 
                                      ( 
                                        select a.PartSn from Product_Part a WITH (NOLOCK)
                                          where (a.PartType like '%COA%' or a.PartType like '%Royalty Paper%')
                                          and ProductID in (select PK_ID from ArchiveIDList where Item='Product')  
                                        union     
                                       select b.PartSn from Pizza_Part b WITH (NOLOCK)
                                         where b.PartType like '%OFFICE%' and b.PizzaID     
                                          in( 
                                              select  c.PizzaID from Product  c WITH (NOLOCK) where c.ProductID in 
                                              (select PK_ID from ArchiveIDList where Item='Product')   
                                            )      
                                       )";
                    break;
                case "COAStatus":
                    sql = @"DELETE  COAStatus where COASN in 
                                 (      
                                    select a.PartSn from Product_Part a WITH (NOLOCK)
                                      where (a.PartType like '%COA%' or  a.PartType like '%Royalty Paper%')
                                        and a.ProductID in               
                                        (select PK_ID from ArchiveIDList where Item='Product')  
                                    union     
                                      select b.PartSn from Pizza_Part b  WITH (NOLOCK) 
                                        where b.PartType like '%OFFICE%' and b.PizzaID  
                                       in  ( select  c.PizzaID from Product c  WITH (NOLOCK)
                                              where c.ProductID in   (select PK_ID from ArchiveIDList where Item='Product')
                                           )
                                 )";
                    break;
                case "CartonInfo":
                    sql = @"DELETE  CartonInfo where CartonNo 
                                     in (select a.CartonSN from Product a WITH (NOLOCK) where  a.ProductID in      
                                         (select PK_ID from ArchiveIDList where Item='Product'))";
                    break;
                case "CartonLog":
                    sql = @"DELETE CartonLog where CartonNo
                                  in (
                                        select a.CartonSN  from Product a WITH (NOLOCK) where  a.ProductID in                  
                                          (select PK_ID from ArchiveIDList where Item='Product')
                                     ) ";
                    break;
                case "CartonStatus":
                    sql = @" DELETE CartonStatus where CartonNo in 
                                 (select a.CartonSN from Product a WITH (NOLOCK) where  a.ProductID in                  
                                    (select PK_ID from ArchiveIDList where Item='Product')) ";
                    break;
                case "PizzaStatus":
                    sql = @" DELETE PizzaStatus where  PizzaID in ( select b.PizzaID  from  Pizza b (nolock)
                                      where b.PizzaID in
                                      (
                                        select a.PizzaID from Product a  (nolock)  where  a.ProductID in
                                        (
                                          select PK_ID from ArchiveIDList where Item='Product'
                                        )
                                      
                                     )) ";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("DELETE", " DELETE  Top({0}) ");
                sql = "While 1=1 Begin  WAITFOR DELAY '{1}'  " + sql + "   If @@rowcount <{0}  break END ";
                sql = string.Format(sql, deleteCount, selepTime);
            }
            return sql;
        }


    }
}
