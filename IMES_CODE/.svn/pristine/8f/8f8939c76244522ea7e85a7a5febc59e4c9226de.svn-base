using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.MORelease
{
    public class SQL
    {
        public static void CreateModelFamily( string Family, string Model,int Status)
        {
            string strSQL = @"if not exists(select * from Family where Family=@Family)
                                           begin
                                                   insert Family(Family, Descr, CustomerID, Editor, Cdt, Udt)
                                                   select top 1 @Family,'',Customer,'SAP',@Now,@Now
                                                    from Customer  
                                           end

                                           if not exists(select * from Model where Model=@Model)
                                           begin
                                                   insert Model(Model, Family, CustPN, Region, ShipType, 
                                                                Status, OSCode, OSDesc, Price, BOMApproveDate, 
                                                                Editor, Cdt, Udt)
                                                   select top 1 @Model,@Family,'',Region,'Normal',
                                                                @Status,'','','',@Now,'SAP',@Now,@Now
                                                     from Region
                                           end";
            

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                             System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Family", 32,Family),
                                                             SQLHelper.CreateSqlParameter("@Model", 32, Model),
                                                             SQLHelper.CreateSqlParameter("@Status", Status),
                                                             SQLHelper.CreateSqlParameter("@Now", DateTime.Now));


        }
        public static void CreateModelFamily(string Family, string Model, int Status, string plant)
        {
            string strSQL = @"if not exists(select * from Family where Family=@Family)
                                           begin
                                                   insert Family(Family, Descr, CustomerID, Editor, Cdt, Udt)
                                                  select  @Family,'',isnull((select top 1 Customer
                                                                                       from Customer
                                                                                       where Plant=@Plant),'')
                                                                    ,'SAP',@Now,@Now  
                                           end

                                           if not exists(select * from Model where Model=@Model)
                                           begin
                                                   insert Model(Model, Family, CustPN, Region, ShipType, 
                                                                Status, OSCode, OSDesc, Price, BOMApproveDate, 
                                                                Editor, Cdt, Udt)
                                                   select top 1 @Model,@Family,'','','Normal',
                                                                @Status,'','','',@Now,'SAP',@Now,@Now                                                    
                                           end";


            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                             System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Family", 32, Family),
                                                             SQLHelper.CreateSqlParameter("@Model", 32, Model),
                                                             SQLHelper.CreateSqlParameter("@Status", Status),
                                                             SQLHelper.CreateSqlParameter("@Plant", plant),
                                                             SQLHelper.CreateSqlParameter("@Now", DateTime.Now));


        }
        public static string CheckPart(string partno)
        {
            string strSQL = @"if exists(select PartNo from Part where PartNo=@partno)
                                             set @ret= 'Y'
                                       else                                    
                                              set @ret='F'";
            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                        System.Data.CommandType.Text,
                                                         strSQL,
                                                         SQLHelper.CreateSqlParameter("@partno", 20, partno),
                                                         ParaRet);

            return ParaRet.Value.ToString().Trim();  
        }


        public static string CheckMOBOMPart(string partno)
        {
            string strSQL = @" if exists(select Model from Model where Model=@partno)
                                                set @ret= 'Y'
                                           else if exists(select PartNo from PartData where PartNo=@partno)
                                                set @ret= 'N'
                                            else 
                                                set @ret= 'F' ";
            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

           SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                            System.Data.CommandType.Text,
                                                            strSQL,
                                                            SQLHelper.CreateSqlParameter("@partno", 20, partno),
                                                            ParaRet);

           return ParaRet.Value.ToString().Trim(); 
        }
      

        public static void InsertMO(DBMoHeader dbheader,
                                                    string isHold,
                                                    string holdCode, 
                                                    string holdComment)
        {
            string strSQL = @"IF NOT EXISTS (SELECT MO FROM MO WHERE MO=@MO)
                                        BEGIN
                                          INSERT INTO MO
                                               ([MO],[Plant],[Model],[CreateDate],[StartDate],[Qty],[SAPStatus],[SAPQty],[Print_Qty],[Transfer_Qty]
		                                        ,[Status],[Cdt],[Udt])
                                          VALUES
                                             (@MO,@Plant,@Model,@CreateDate,@StartDate,@Qty,@SAPStatus,@SAPQty,@Print_Qty,@Transfer_Qty
		                                        ,@Status,@Cdt,@Udt)
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
                                                            SAPStatus=@SAPStatus,
                                                            SAPQty=@SAPQty,
                                                            Status = (  case when Print_Qty>0 and @Status='R' then
                                                                                    'H'
                                                                              when Status='H' and @Status='R' then
                                                                                    'H'
                                                                             else 
                                                                                  @Status
                                                                            end),
                                                            Udt=@Udt,
                                                            Model=@Model                                                   
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
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
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
            //Change to default issued value =0
            salparams.Add(SQLHelper.CreateSqlParameter("@Qty", 0));
            salparams.Add(SQLHelper.CreateSqlParameter("@TotalQty", (int)totalqty));
            salparams.Add(SQLHelper.CreateSqlParameter("@Print_Qty",0));
            salparams.Add(SQLHelper.CreateSqlParameter("@Transfer_Qty",0)); 

             //R -> H -> C Status 
            string moStatus = (isHold == "Y" ? "X" : ((dbheader.Status == "TECO" || dbheader.Status == "CNF") ? "C" : "R"));
            salparams.Add(SQLHelper.CreateSqlParameter("@Status", 1, moStatus ));            
            salparams.Add(SQLHelper.CreateSqlParameter("@Cdt",DateTime.Now));
            salparams.Add(SQLHelper.CreateSqlParameter("@Udt",DateTime.Now));
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


        public static void InsertMOBOM(DBMoItemDetail[] moitems)
        {
            string strOneSQL = @"INSERT INTO MoBOM
                                                                            ([MO],[PartNo],[Qty],[Group],[Deviation],[Action],[Editor],[Cdt],[Udt])
                                                VALUES ({0},{1},{2},{3},'1','ADD','SAP',GETDATE(),GETDATE());  
                                                set @ID = SCOPE_IDENTITY();   
                                               INSERT INTO MoBOMData
                                                    ([MoBOM_ID],[TxnId],[MOItem],[Reservation],[ResvItem],[WithdrawQty],[Unit],[AltGroup]
                                                     ,[IsPhantom],[IsBulk],[SpecialStock],[MN],[ParentMaterial],[DataSource],[Udt])
                                                 VALUES  (@ID,{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'SAP',GETDATE())";
            string strSQL = "";
            SqlParameter[] sqlparamsArray = BuildBOMItemsSqlParams(moitems, ref strSQL, strOneSQL);
            //strSQL = "declare @ID int ;" + strSQL;
//            strSQL = //@"IF  EXISTS (SELECT MO FROM MO WHERE MO=@MO)
//                            @"  Delete from MoBOMData 
//                                    where  MoBOM_ID in (Select ID from MoBOM WHERE MO=@MO)
//                                    Delete from MoBOM 
//                                    WHERE MO=@MO; " + strSQL;
            // don't delete manual material 
            strSQL = @"Delete  from MoBOM 
                                 from MoBOM a inner join MoBOMData b on a.ID= b.MoBOM_ID 
                                 where MO=@MO
                                 Delete from MoBOMData 
                                 where  MoBOM_ID in (Select ID from MoBOM WHERE MO=@MO); " + strSQL;

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                              System.Data.CommandType.Text,
                                                              strSQL, 
                                                              sqlparamsArray);
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


        public static void InsertModelBOM(string Material, string Plant, DBMoItemDetail[] moitems)
        {

            SetBOMGroup(moitems);
            List<TbMoItem> TbMoItem = new List<TbMoItem>();
            foreach (DBMoItemDetail item in moitems)
            {
                TbMoItem temp = new TbMoItem();
                temp.SerialNumber = item.SerialNumber == null ? "" : item.SerialNumber;
                temp.MoNumber = item.MoNumber == null ? "" : item.MoNumber;
                temp.MoItem = item.MoItem == null ? "" : item.MoItem;
                temp.Component = item.Component == null ? "" : item.Component;
                temp.UnitReqQty = item.UnitReqQty == null ? "0" : item.UnitReqQty;
                temp.ParentMaterial = item.ParentMaterial == null ? "0" : item.ParentMaterial;
                temp.Group = item.Group;
                TbMoItem.Add(temp);
            }

            DataTable dt = TbMoItem.ToDataTable();
            
            string strSQL = @"exec IMES_ModelBOM_Insert @Material, @Plant, @dt";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                             System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Material", Material == null ? "" : Material),
                                                             SQLHelper.CreateSqlParameter("@Plant", Plant == null ? "" : Plant),
                                                             SQLHelper.CreateSqlParameter("@dt", "TbMoItem", dt));

        }

//         public static void InsertTxnDataLog(string Category,string Action,string KeyValue1,string KeyValue2,string TxnId,string ErrorCode,string ErrorDescr,string State,string Comment)
//         {
//             string strSQL = @"INSERT INTO TxnDataLog ([Category],[Action],[KeyValue1],[KeyValue2],[TxnId],[ErrorCode],[ErrorDescr]
//                                            ,[State],[Comment],[Cdt])
//                                        VALUES (@Category,@Action,@KeyValue1,@KeyValue2,@TxnId,@ErrorCode,@ErrorDescr
//                                            ,@State,@Comment,GETDATE()) ";
//             SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
//                                                             System.Data.CommandType.Text,
//                                                              strSQL,
//                                                              SQLHelper.CreateSqlParameter("@Category", 16, Category),
//                                                              SQLHelper.CreateSqlParameter("@Action", 16, Action),
//                                                              SQLHelper.CreateSqlParameter("@KeyValue1", 32, KeyValue1),
//                                                              SQLHelper.CreateSqlParameter("@KeyValue2", 32, KeyValue2),
//                                                              SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
//                                                              SQLHelper.CreateSqlParameter("@ErrorCode", 32, ErrorCode),
//                                                              SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
//                                                              SQLHelper.CreateSqlParameter("@State", 16, State),
//                                                              SQLHelper.CreateSqlParameter("@Comment", 255, Comment));
        
//         }

             
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
                        moitems[i].HasAltGroup = true;
                        moitems[k].HasAltGroup = true;
                    }

                }
                groupNo++;
            }
        }                 
    }

    public class TbMoItem
    {
        public string SerialNumber { get; set; }
        public string MoNumber { get; set; }
        public string MoItem { get; set; }
        public string Component { get; set; }
        public string UnitReqQty { get; set; }
        public string ParentMaterial { get; set; }
        public int Group { get; set; }
    }
}