using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.HPMORelease
{
    public class SQL
    {

        public static void InsertMO(string connectionDB, int dbIndex, 
                                                    DBMoHeader dbheader,
                                                    string isHold,
                                                    string holdCode, 
                                                    string holdComment)
        {
            string strSQL = @"IF NOT EXISTS (SELECT MO FROM MO WHERE MO=@MO)
                                        BEGIN
                                          INSERT INTO MO
                                               ([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],[Print_Qty],[Transfer_Qty]
		                                        ,[Status],[Cdt],[Udt], [CustomerSN_Qty])
                                          VALUES
                                             (@MO,@Plant,@Model,@CreateDate,@StartDate,@Qty,
                                              case when (@SAPStatus = 'REL' or @SAPStatus='PCNF') then ' ' else @SAPStatus end,
                                                 @SAPQty,@Print_Qty,@Transfer_Qty
		                                        ,@Status,@Cdt,@Udt,@CustomerSN_Qty)
                                          INSERT INTO MOData 
                                               ([MO],[TxnId],[MOType],[Unit] ,[FinishDate] ,[ProductVer],[Priority],[BOMCategory],[BOMExpDate] ,[SalesOrder]
                                               ,[SOItem],[IsProduct] ,[DataSource],[TotalQty],[Editor],[Udt])
                                          VALUES
                                              (@MO,@TxnId,@MOType,@Unit ,@FinishDate ,@ProductVer,@Priority,@BOMCategory,@BOMExpDate ,@SalesOrder
                                               ,@SOItem,@IsProduct ,@DataSource,@TotalQty,@Editor,@Udt) 
                                           
                                          INSERT INTO MOStatus(MO, Status, IsHold, HoldCode, 
                                                                         Comment, Editor, LastTxnId, Udt)
                                          VALUES(@MO, 'Release', @IsHold, @HoldCode,
                                                          @HoldComment, @Editor, @TxnId, @Udt)

                                         INSERT INTO MOStatusLog(MO, [Function], [Action], Station, PreStatus, 
                                                                               Status, IsHold, HoldCode, TxnId, Comment, 
                                                                               Editor, Cdt)
                                           VALUES(@MO, 'WSMORelease','CreateMO', '','', 
                                                           'Release',@IsHold, @HoldCode,@TxnId ,@HoldComment, 
                                                           @Editor, @Udt)   

                                         END
                                     ELSE 
                                           BEGIN
                                              UPDATE MO 
                                                    SET StartDate=@StartDate,
                                                            Qty=(case when Qty>@TotalQty  then 
                                                                                        @TotalQty
                                                                       else 
                                                                                         Qty
                                                                       end),
                                                            SAPStatus=(case when (@SAPStatus = 'REL' or @SAPStatus='PCNF') then ' ' else @SAPStatus end),
                                                            SAPQty=@SAPQty,
                                                            Udt=@Udt,
                                                            Model=@Model,
                                                            Status=@Status                                                   
                                                     WHERE MO=@MO
 
                                               UPDATE MOData
                                                     SET TxnId=@TxnId,MOType=@MOType,Unit=@Unit,FinishDate=@FinishDate,ProductVer=@ProductVer,Priority=@Priority,
                                                            BOMCategory=@BOMCategory,BOMExpDate=@BOMExpDate,SalesOrder=@SalesOrder,SOItem=@SOItem,
                                                            IsProduct=@IsProduct,TotalQty=@TotalQty, Udt=@Udt
                                                     WHERE MO=@MO
                                                
                                                IF  @IsHold='N'  
                                                BEGIN 
                                                  update MOStatus
                                                   set  IsHold=@IsHold,
                                                          HoldCode=@HoldCode, 
                                                          Comment=@HoldComment
                                                   where  MO=@MO and
                                                              ( HoldCode='SYS100' OR
                                                                 HoldCode='SYS101' )
                                                END
                                                ELSE
                                                BEGIN
                                                    update MOStatus
                                                   set  IsHold=@IsHold,
                                                          HoldCode=@HoldCode, 
                                                          Comment=@HoldComment
                                                   where  MO=@MO and
                                                               IsHold='N'
                                                END
                                                  INSERT INTO MOStatusLog(MO, [Function], [Action], Station, PreStatus, 
                                                                               Status, IsHold, HoldCode, TxnId, Comment, 
                                                                               Editor, Cdt)
                                                  SELECT @MO, 'WSMORelease','UpdateMO', '',Status, Status, 
                                                             IsHold, HoldCode,@TxnId ,Comment, @Editor, @Udt
                                                   FROM  MOStatus 
                                                   WHERE MO =@MO

                                            END";

            SqlParameter[] sqlparamsArray = BuildHeaderSqlParams(dbheader,
                                                                 isHold,
                                                                 holdCode,
                                                                 holdComment );
            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                           System.Data.CommandType.Text,
                                                            strSQL, sqlparamsArray);

        }

 
        private static SqlParameter[] BuildHeaderSqlParams(DBMoHeader dbheader,
                                                                      string isHold,
                                                                      string holdCode,
                                                                      string holdComment)
        {
            List<SqlParameter> salparams = new List<SqlParameter>();
            salparams.Add(SQLHelper.CreateSqlParameter("@MO", 20, dbheader.MoNumber.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@Plant", 20, dbheader.Plant.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@Model", 20, dbheader.BuildOutMtl.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@CreateDate", ObjectTool.String2Date(dbheader.CreateDate)));
            salparams.Add(SQLHelper.CreateSqlParameter("@StartDate", ObjectTool.String2Date(dbheader.BasicStartDate)));
            salparams.Add(SQLHelper.CreateSqlParameter("@SAPStatus",10, dbheader.Status.Trim()));
            double qty = Math.Ceiling(float.Parse( dbheader.DeliveredQty));
            salparams.Add(SQLHelper.CreateSqlParameter("@SAPQty",(int)qty));
            double totalqty = Math.Ceiling(float.Parse(dbheader.TotalQty));
            salparams.Add(SQLHelper.CreateSqlParameter("@Qty", (int)totalqty));
            salparams.Add(SQLHelper.CreateSqlParameter("@TotalQty", (int)totalqty));
            salparams.Add(SQLHelper.CreateSqlParameter("@Print_Qty",0));
            salparams.Add(SQLHelper.CreateSqlParameter("@Transfer_Qty",0));
            if (dbheader.Status.Trim() == "TECO" || dbheader.Status.Trim() == "CNF" || dbheader.Status.Trim() == "CLOSE")
                salparams.Add(SQLHelper.CreateSqlParameter("@Status",1,"C"));// 表示工單關閉
            else
                salparams.Add(SQLHelper.CreateSqlParameter("@Status", 1, "H"));
            salparams.Add(SQLHelper.CreateSqlParameter("@Cdt", DateTime.Now));
            salparams.Add(SQLHelper.CreateSqlParameter("@Udt",DateTime.Now));
            salparams.Add(SQLHelper.CreateSqlParameter("@CustomerSN_Qty",0));            
            salparams.Add(SQLHelper.CreateSqlParameter("@FinishDate", ObjectTool.String2Date(dbheader.BasicFinishDate)));
            salparams.Add(SQLHelper.CreateSqlParameter("@MOType", 4, dbheader.MoType.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@Unit", 8, dbheader.Unit.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@ProductVer", 8, dbheader.ProductionVer.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@Priority", 8, dbheader.Priority==null ? "": dbheader.Priority.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@BOMCategory", 8, dbheader.BOMStatus.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@BOMExpDate", ObjectTool.String2Date(dbheader.BOMExplDate)));
            salparams.Add(SQLHelper.CreateSqlParameter("@SalesOrder", 16, string.IsNullOrEmpty( dbheader.SalesOrder)? "" :dbheader.SalesOrder.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@SOItem",8, string.IsNullOrEmpty(dbheader.SOItem)?"":dbheader.SOItem.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@TxnId",8, dbheader.SerialNumber.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@IsProduct", 1, dbheader.IsProduct.Trim()));
            salparams.Add(SQLHelper.CreateSqlParameter("@DataSource",8,  "SAP"));
            salparams.Add(SQLHelper.CreateSqlParameter("@Editor",8,  "SAP"));

            salparams.Add(SQLHelper.CreateSqlParameter("@IsHold", 1, isHold));
            salparams.Add(SQLHelper.CreateSqlParameter("@HoldCode", 32, holdCode));
            salparams.Add(SQLHelper.CreateSqlParameter("@HoldComment", 255, holdComment));
            return salparams.ToArray();
          }


        private static SqlParameter[] BuildBOMItemsSqlParams(DBMoItemDetail[] moitems, ref string strSQL,string strOneSQL)
        {
             SetBOMGroup(moitems);
             List<SqlParameter> sqlparams = new List<SqlParameter>();
             int i = 0; //items count
             sqlparams.Add(SQLHelper.CreateSqlParameter("@MO", 20, moitems[0].MoNumber.Trim()));
             sqlparams.Add(SQLHelper.CreateSqlParameter("@ID", 0));

             foreach (DBMoItemDetail item in moitems)
            {

                 // SAP delete material flag don't insert MoBOM 
                 if (string.IsNullOrEmpty( item.Delete))
                 {
                    string[] col = new string[16];
                    col[0] = "@MO" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[0], 20, item.MoNumber.Trim()));
                    col[1] = "@PartNo" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[1], 20, item.Component.Trim()));
                    col[2] = "@Qty" + i.ToString();
                    //double qty = Math.Ceiling(float.Parse(item.ReqQty));
                    double qty = Math.Ceiling(float.Parse(item.UnitReqQty));
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[2], (int)qty));
                    col[3] = "@Group" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[3], item.Group));//group
                    col[4] = "@TxnId" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[4], 32, item.SerialNumber.Trim()));
                    col[5] = "@MOItem" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[5], 8, item.MoItem.Trim()));
                    col[6] = "@Reservation" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[6], 16, item.Reservation.Trim()));
                    col[7] = "@ResvItem" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[7], 8, item.ResvItem.Trim()));
                    col[8] = "@WithdrawQty" + i.ToString();
                    double wqty = Math.Ceiling(float.Parse(item.WithdrawQty));
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[8], (int)wqty));
                    col[9] = "@Unit" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[9], 8, item.Unit));
                    col[10] = "@AltGroup" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[10], 8, item.AltGroup == null ? "" : item.AltGroup.Trim()));
                    col[11] = "@IsPhantom" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[11], 1, item.Pantom == null ? "" : item.Pantom.Trim()));
                    col[12] = "@IsBulk" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[12], 1, item.Bulk == null ? "" : item.Bulk.Trim()));
                    col[13] = "@SpecialStock" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[13], 1, item.SpecialStock == null ? "" : item.SpecialStock.Trim()));
                    col[14] = "@MN" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[14], 16, item.MN == null ? "" : item.MN.Trim()));
                    col[15] = "@ParentMaterial" + i.ToString();
                    sqlparams.Add(SQLHelper.CreateSqlParameter(col[15], 16, item.ParentMaterial == null ? "" : item.ParentMaterial.Trim()));
                    strSQL = strSQL + string.Format
                                      (strOneSQL, col[0], col[1], col[2], col[3], col[4], col[5], col[6], col[7], col[8], col[9], col[10],
                                                                         col[11], col[12], col[13], col[14], col[15]) + "\r\n";

                    i++;
                 }
            }
             return sqlparams.ToArray();
        }

        private static void SetBOMGroup(DBMoItemDetail[] moitems)
        {
          
            int groupNo = 1;
            for (int i = 0; i < moitems.Length; i++)
            {
                if (moitems[i].Group == 0)
                { 
                    moitems[i].Group = groupNo; 
                }
                else
                { 
                    continue; 
                }

                 for (int k = i + 1; k < moitems.Length; k++)
                {
                    if (moitems[i].AltGroup == moitems[k].AltGroup && 
                        moitems[i].ParentMaterial == moitems[k].ParentMaterial &&
                        !string.IsNullOrEmpty(moitems[i].AltGroup ) &&
                        !string.IsNullOrEmpty(moitems[k].AltGroup))
                    {
                        moitems[i].Group = groupNo;
                        moitems[k].Group = groupNo;
                    }

                }
                groupNo++;
            }
        }
         
        
    }
}