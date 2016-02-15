using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
namespace IMES.WS.MaterialInfoNotice
{
    public class SQL
    {
        
        public static void InsertSapMaterialData(string TxnId)
        {
            string strSQL = @"Delete from PartData 
                                            where PartNo in  (SELECT Material 
                                                                                from SAPMaterialInfo 
                                                                               where TxnId=@TxnId) ;

                                            INSERT PartData (PartNo,TxnId,Plant,SAPMaterialGroup,SAPMaterialType,
                                                                           SAPMaterialStatus,SAPCustomer,SAPMaterialDescr,DataSource,Udt)
                                              SELECT Material,TxnId,Plant,SAPMaterialGroup,SAPMaterialType,
                                                                SAPMaterialStatus,SAPCustomer,SAPMaterialDescr,'SAP',GETDATE() 
                                                    from SAPMaterialInfo 
                                                  where TxnId=@TxnId 
                                                     and SAPMaterialType <> 'ZFRT';

                                            Delete  from ModelData 
                                            where Model in  (SELECT Material 
                                                                              from SAPMaterialInfo 
                                                                            where TxnId=@TxnId) ;

                                           INSERT ModelData (Model,TxnId,Plant,SAPMaterialGroup,SAPMaterialType,
                                                                            SAPMaterialStatus,SAPCustomer,SAPMaterialDescr,DataSource,Udt)
                                                 SELECT Material,TxnId,Plant,SAPMaterialGroup,SAPMaterialType,
                                                                 SAPMaterialStatus,SAPCustomer,SAPMaterialDescr,'SAP',GETDATE()
                                                     from SAPMaterialInfo 
                                                   where TxnId=@TxnId 
                                                        and SAPMaterialType = 'ZFRT'" ;
 
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                           System.Data.CommandType.Text,
                                                            strSQL, SQLHelper.CreateSqlParameter("@TxnId",32,TxnId));
        
        }
        public static void InsertUpdateSapModelToIMES(string TxnId)
        {
            string strSQL = BuildUpdateModelSQL() + BuildInsertFamilySQL() + BuildInsertModelSQL();
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                          System.Data.CommandType.Text,
                                                           strSQL, SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId));

        }
        public static void InsertUpdateSapPartToIMES(string TxnId)
        {
            string strSQL = BuildUpdatePartSQL() + BuildInsertPartSQL();
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                          System.Data.CommandType.Text,
                                                           strSQL, SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId));
       
        }
        
        
        // Insert Part table
        // The rule about column "PartType" :
        // 1. Using column "SAPMaterialGroup" link between  table SAPMaterialGroup & SAPMaterialGroupMAP
        // 2. If the relation exists, save "PartType" using the value of column MESPartType in table SAPMaterialGroupMAP
        // 3. If the relation not exists,s ave "PartType" using the value of column SAPMaterialGroup in table SAPMaterialInfo
        private static string BuildInsertPartSQL()
        {
            string strSQL = @"INSERT into Part (PartNo,Descr,PartType,CustPartNo,
                                                AutoDL, Remark,
                                                Flag,
                                                Editor,Cdt,Udt, BomNodeType)
                                            SELECT a1.Material,a1.SAPMaterialGroup,COALESCE(b1.MESPartType,c1.MESPartType, a1.SAPMaterialGroup),'',
                                                            'Y', a1.SAPMaterialDescr,
                                                           Case when a1.SAPMaterialStatus='04' or a1.SAPMaterialStatus='07' then 
                                                                         0 
                                                           else 
                                                              1   
                                                           end,
                                                            'SAP',GETDATE(),GETDATE(), a1.SAPMaterialType 
                                            from SAPMaterialInfo a1 
                                           left join SAPMaterialGroupMAP b1 on a1.SAPMaterialGroup=b1.SAPMaterialGroup and 
                                                                                                        a1.SAPMaterialType =b1.SAPMaterialType
                                           left join SAPMaterialGroupMAP c1 on substring(a1.Material,1,4) =c1.SAPMaterialGroup and 
                                                                                                        a1.SAPMaterialType =c1.SAPMaterialType    
                                          where a1.Material not in(SELECT PartNo from Part) 
                                              and a1.SAPMaterialType <> 'ZFRT'  
                                              and a1.TxnId=@TxnId; ";
            return strSQL;
          }

        // Only update Flag,Remark & Udt
        private static string BuildUpdatePartSQL()
        {
            string strSQL = @"UPDATE Part 
                                                   set Remark = (case a.PartType when 'MB' then a.Remark else b.SAPMaterialDescr end),
                                                         Flag =(Case when b.SAPMaterialStatus='04' or b.SAPMaterialStatus='07'  
                                                                    then 0
                                                                    else 1 end),
                                                          Udt=Getdate()  
                                           from Part a, 
                                                    SAPMaterialInfo b 
                                           where a.PartNo = b.Material 
                                               and TxnId=@TxnId; ";
            return strSQL;      
        
        
        }
    
        private static string BuildInsertModelSQL()
        {
            string strSQL = @"INSERT Model (Model, Family, CustPN, Region, ShipType, 
                                                                    Status, OSCode, OSDesc, Price, BOMApproveDate, 
                                                                    Editor, Cdt, Udt)
                                         SELECT Material,SAPMaterialGroup,'','','Normal',
                                                          Case when SAPMaterialStatus='04' or SAPMaterialStatus='07' 
                                                                    then 0 
                                                                    else 1 end,
                                                            '','','',GETDATE(),
                                                            'SAP',GETDATE(),GETDATE()
                                           from SAPMaterialInfo
                                           where Material not in(SELECT Model from Model) 
                                               and SAPMaterialType = 'ZFRT'
                                               and TxnId=@TxnId ; ";

          return strSQL;
        }

        //If the SAPMaterialGroup not exists in Family, then insert the string which extracts the characters from "F_" in SAPMaterialGroup
        // Example : F_Ebook -> insert Ebook to Family table.
        private static string BuildInsertFamilySQL()
        {
            string strSQL = @"INSERT Family(Family, Descr, CustomerID, Editor, Cdt, Udt)										   
                                          SELECT  top 1 a.SAPMaterialGroup, 
		                                                          a.SAPMaterialGroup, Isnull(b.Customer,'') as Customer ,'SAP' as Editor,GETDATE(),GETDATE()
                                           From SAPMaterialInfo a 
                                           LEFT OUTER JOIN Customer b on b.Plant = a.Plant
                                           where  a.SAPMaterialGroup not in (SELECT Family from Family) 
                                                          and a.SAPMaterialType ='ZFRT' 
                                                          and a.TxnId=@TxnId; ";
            return strSQL;
        }
        // Only update Status
        private static string BuildUpdateModelSQL()
        {
            string strSQL = @" UPDATE Model set 
                                           Status =(Case when s.SAPMaterialStatus='04' or s.SAPMaterialStatus='07' then 0 else 1 end),
                                           Udt=GETDATE()  from Model b, SAPMaterialInfo s
                                         where b.Model = s.Material and s.TxnId=@TxnId; ";

            return strSQL;
        
        }

        public static void InsertSAPMaterialInfo(string material,
                                                                       string txnId,
                                                                       string plant,
                                                                       string materialGroup,
                                                                       string materialType,
                                                                       string materialStatus,
                                                                       string customerMaterial,
                                                                       string customer,
                                                                       string materialDesc)
        {
            string strSQL = @"insert dbo.SAPMaterialInfo(Material, TxnId, Plant, SAPMaterialGroup, SAPMaterialType, 
                                                                                       SAPMaterialStatus, SAPCustomerMaterial, SAPCustomer, SAPMaterialDescr, IsUpdate, 
                                                                                       Cdt)
                                          values(@Material, @TxnId, @Plant, @SAPMaterialGroup, @SAPMaterialType, 
                                                       @SAPMaterialStatus, @SAPCustomerMaterial, @SAPCustomer, @SAPMaterialDescr,'T', 
                                                        GETDATE())";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                           System.Data.CommandType.Text,
                                                            strSQL, 
                                                            SQLHelper.CreateSqlParameter("@Material", 32, material),
                                                            SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                                            SQLHelper.CreateSqlParameter("@Plant", 32, plant),
                                                            SQLHelper.CreateSqlParameter("@SAPMaterialGroup", 32, materialGroup),
                                                            SQLHelper.CreateSqlParameter("@SAPMaterialType", 32, materialType),
                                                            SQLHelper.CreateSqlParameter("@SAPMaterialStatus", 32, materialStatus),
                                                            SQLHelper.CreateSqlParameter("@SAPCustomerMaterial", 32, customerMaterial),
                                                            SQLHelper.CreateSqlParameter("@SAPCustomer", 32, customer),
                                                            SQLHelper.CreateSqlParameter("@SAPMaterialDescr", 32, materialDesc));
        }
    }
}