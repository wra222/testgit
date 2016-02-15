using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.MOPullRelease
{
    public class SQL
    {
        public static string UpdateMO(MoPullHeader header)
        {
            string strSQL = @" IF Exists(select * from   MO where MO=@MO)
                                            Begin 
                                                UPDATE MO 
                                               SET    Qty=(case when @Qty>b.TotalQty  then 
                                                                              b.TotalQty
                                                                       else 
                                                                                @Qty
                                                                       end),
                                                            Status= (case  when Status='R' then
                                                                                            'H'       
                                                                                  when Status='C' And Qty > Print_Qty then
                                                                                            'H'
                                                                                   else 
                                                                                         Status
                                                                          end ),                                                    
                                                            Udt=@Udt
                                                From MO a, MOData b                                                   
                                                WHERE  a.MO = b.MO and 
                                                               a.MO=@MO
                                                
                                                IF Exists(select * from MOStatus where MO=@MO and Status='Close')
                                                Begin
                                                    Update   MOStatus
                                                    Set Status='Release'
                                                    where  MO=@MO  
                                                   INSERT INTO MOStatusLog(MO, [Function], [Action], Station, PreStatus, 
                                                                                   Status, IsHold, HoldCode, TxnId, Comment, 
                                                                                   Editor, Cdt)
                                                   Select @MO, 'WSMOPullRelease','UpdateMO', '','Close', 
                                                              Status,IsHold, HoldCode,@TxnId ,@Comment, @Editor, @Udt
                                                   FROM  MOStatus 
                                                   WHERE MO =@MO
                                                
                                               End
                                               Else
                                               Begin
                                                    INSERT INTO MOStatusLog(MO, [Function], [Action], Station, PreStatus, 
                                                                                   Status, IsHold, HoldCode, TxnId, Comment, 
                                                                                   Editor, Cdt)
                                                   Select @MO, 'WSMOPullRelease','UpdateMO', '',Status, 
                                                              Status,IsHold, HoldCode,@TxnId ,@Comment, @Editor, @Udt
                                                   FROM  MOStatus 
                                                   WHERE MO =@MO
                                               End
                                               set @Ret='T' 
                                        End
                                        Else
                                        Begin
                                             set @Ret='F'
                                        End       
                                                      
                                            ";

             double totalqty = Math.Ceiling(float.Parse(header.IssuedQty)) + Math.Ceiling(float.Parse(header.CurrentyIssueQty));
             string Comment = "IssuedQty:" + header.IssuedQty + "  CurrentyIssueQty:" + header.CurrentyIssueQty;
             SqlParameter retPara = SQLHelper.CreateSqlParameter("@Ret", 8,"", ParameterDirection.Output);
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                           System.Data.CommandType.Text,
                                                            strSQL, 
                                                            SQLHelper.CreateSqlParameter("@MO",20, header.MoNumber),
                                                            SQLHelper.CreateSqlParameter("@Qty", (int) totalqty),
                                                            SQLHelper.CreateSqlParameter("@TxnId", 16,header.SerialNumber),
                                                            SQLHelper.CreateSqlParameter("@Comment", 255, Comment),
                                                            SQLHelper.CreateSqlParameter("@Editor", 16,"SAP"),
                                                            SQLHelper.CreateSqlParameter("@Udt", DateTime.Now),
                                                             retPara);

            return retPara.Value.ToString();
        }

    }
}