using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Transactions;
using System.Xml;
using System.Data;
using System.IO;

namespace ArchiveDB
{
     public static class  Tool
    {
         public static string GetIDListSQL(string item)
         {
             string cmd = "";
             switch (item)
             {
                 case "Product":
                     cmd = @" select ProductID PK_ID,'{0}' Item,'noarchive' [Status],GETDATE() Cdt from Product  where DeliveryNo in
                                 ( select DeliveryNo from  dbo.Delivery where ShipDate < DateAdd(""d"", @day, GETDATE()))";
                     break;
                 case "PCB":
                     cmd = @" select PCBID PK_ID,'{0}' Item,'noarchive' [Status],GETDATE() Cdt from Product  where DeliveryNo in
                                 ( select DeliveryNo from  dbo.Delivery where ShipDate < DateAdd(""d"", @day, GETDATE()))
                                      and PCBID is not null and PCBID<>'' ";
                     break;
                 //                case pkType.CartonSN:
                 //                    cmd = @"select distinct CartonSN PK_ID,'CartonSN' [Type],'noarchive' [Status],GETDATE() Cdt from Product  where DeliveryNo in
                 //                                 ( select DeliveryNo from  dbo.Delivery where ShipDate <DateAdd(""d"", @day, GETDATE()))";
                 //                    break;
                 case "Delivery":
                     cmd = @"select distinct DeliveryNo PK_ID,'{0}' Item,'noarchive' [Status],GETDATE() Cdt from Product  where DeliveryNo in
                                 ( select DeliveryNo from  dbo.Delivery where ShipDate <DateAdd(""d"", @day, GETDATE()))";
                     break;
                 case "Pallet" :
                     cmd = @"select distinct PalletNo PK_ID,'{0}' Item,'noarchive' [Status],GETDATE() Cdt from Product  where DeliveryNo in
                                 ( select DeliveryNo from  dbo.Delivery where ShipDate <DateAdd(""d"", @day, GETDATE()))
                                 and PalletNo is not null and PalletNo<>'' ";
                     break;

             }
             cmd = string.Format(cmd, item);
             return cmd;
         }
         public static string SetCopyFKTableSQL(string mainScript,string parentfkName,string fkName,string fkTbName)
         {
             string r = "Select * from " + fkTbName + " where  " + fkName + " in ({0}) ;";
             mainScript = mainScript.Replace("*", parentfkName);
             r = string.Format(r, mainScript);
             return r;
            
         }
         public static string SetCopySQL(int layer)
         {
             string r = "";
             if (layer == 0)
             {
                 r = @"select * from  {0} WITH (NOLOCK) where {1} in (select PK_ID from ArchiveIDList where Item='{2}')  ";
             }
             else if (layer == 1)
             {
                 r = @" select * from  {0}  WITH (NOLOCK) 
                                      where {1} in
                                      (
                                        select a.{1} from {2} a WITH (NOLOCK)  where {3} in
                                        (
                                          select PK_ID from ArchiveIDList where Item='{4}'
                                        )
                                      
                                     )";


             }
             else
             {

                 r = @"  select * from {0} WITH (NOLOCK)  where {1} in
                                     (
	                                     select {1} from  {2}  WITH (NOLOCK) 
	                                      where {1} in
	                                      (
		                                    select {1} from {3} WITH (NOLOCK) where {4} in
		                                    (
		                                      select PK_ID from ArchiveIDList where Item='{5}'
		                                    )
	                                      )
                                      )";


             }
             return r;
         
         }
         public static string SetDeleteSQLfromCopy(string sql,string rows,string selepTime)
         {
             sql = sql.Replace("WITH (NOLOCK)", " ");
             int y = sql.Trim().ToUpper().IndexOf("FROM");
             string sub1 = sql.Trim().Substring(0, y);//get the string "Select ProductID from  Product -> return "Select ProductID"
             string t = string.Format("DELETE  Top({0}) ", rows);
             string tmp = sql.Replace(sub1, t);
          
             //   string tmp = sql.Replace(sub1, "DELETE  Top(500) "); 
             //   tmp = "While 1=1 Begin " + tmp + " WAITFOR DELAY '00:00:00.5' If @@rowcount <500 break END ";
             //   tmp = "While 1=1 Begin " + tmp + " WAITFOR DELAY '{0}'  If @@rowcount <{1}  break END ";
             //   tmp = string.Format(tmp, selepTime, rows);

             tmp = "While 1=1 Begin   WAITFOR DELAY '{1}'   " + tmp + "  If @@rowcount <{0}  break END ";
             tmp = string.Format(tmp, rows, selepTime); 
          
             return tmp;
         //    return sql.Replace(sub1, "DELETE ");
         }
         public static string SetCopyCdtSQL(string tableName,int cdtDay)
         {
             string r = @"select * from {0} where Cdt < DateAdd(d,-{1}, GETDATE())    ";
             r = string.Format(r, tableName, cdtDay.ToString());
             return r;
         }


         public static string CovertInsertMainItemSQL(string sql,string item,string timeStamp)
         {  //{0} PK_ID,'{1}' Item,'noarchive' [Status],GETDATE() Cdt
           
             int y = sql.Trim().ToUpper().IndexOf("FROM");
             string pkName = sql.Trim().Substring(7, y - 8).Trim();
             string sub1 = sql.Substring(0, y);//get the string "Select ProductID from  Product -> return "Select ProductID"
             string sub2 = sql.Trim().Substring(y, sql.Length - y).Trim(); //get the string "Select ProductID from  Product -> return " from  Product"
             string tmp = pkName +" PK_ID,'" +item.Trim()+"' Item, '' Status,'" +timeStamp +"' [TimeStamp],GETDATE() Cdt ";
             sub1 = sub1.Replace(pkName, tmp);
             return sub1 + " " + sub2;
         }




         //Calc Time
         private static DateTime _startTime;
         public static void BeginCountdown()
         { _startTime = DateTime.Now; }
         public static int EndCountdown()
         {
             if (_startTime.Year == 1) { return 0; }
             int cost;
             TimeSpan copyTime;
             copyTime = DateTime.Now - _startTime;
             cost = copyTime.Minutes * 60 + copyTime.Seconds;
             return cost;
         }

    }
}
