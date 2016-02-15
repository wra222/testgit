using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using System.Collections;
namespace IMES.Query.Implementation
{
    public class PAK_KitPartQuery : MarshalByRefObject, IPAK_KitPartQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetData(string DBConnection, DataTable ModelLine, bool IsGroupByLine)
        {
           
            string model;
            string qty;
            string line;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"CREATE TABLE #Data  (PartNo varchar(64),Descr varchar(64), Qty int,Line varchar(16) ) ;
                                 CREATE TABLE #Rpt  (Material varchar(64),Qty int,Line varchar(64)) ");
            string strInsert = "Insert #Rpt Values('{0}',{1},'{2}')  ";
            string tmp = "";
            foreach (DataRow dr in ModelLine.Rows)
            {  model=dr["Model"].ToString();
                qty=dr["Qty"].ToString();
                line=dr["Line"].ToString();
                if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(qty) )
                {

                    tmp = string.Format(strInsert, model, qty, line);
                    sb.Append(tmp);
                }
            
            }
            sb.Append(@"  Insert  into #Data
                                      select '_'+Material,'',Qty,Line from #Rpt where 
                                    Material not in (select Material from view_PAKKittingMaterial )");
       
            if (IsGroupByLine)
            {



                sb.Append(@"  Insert into   #Data
                                        SELECT a.PartNo,a.Descr,Sum(a.Qty*b.Qty)
                                         as Qty,b.Line FROM dbo.view_PAKKittingMaterial a,#Rpt b
                                         where a.Material=b.Material
                                         group by  a.PartNo,a.Descr,b.Line  order by Line ;
                                          SELECT * from #Data  order by Line  ,PartNo desc");
            }
            else
            {
                sb.Append(@"  Insert into   #Data
                                        SELECT a.PartNo,a.Descr,Sum(a.Qty*b.Qty)
                                         as Qty,'' FROM dbo.view_PAKKittingMaterial a,#Rpt b
                                         where a.Material=b.Material
                                         group by  a.PartNo,a.Descr
                                         order by PartNo ;
                                         SELECT PartNo,Descr,SUM(Qty) as Qty from #Data
                                          group by PartNo,Descr
                                           order by PartNo desc ");
            }
           
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                    sb.ToString());

            return dt;

        }
        
        
        
        public DataTable GetData(string DBConnection, IList<string> Model, IList<string> Cnt)
        {
            
            string strSQL = 
                @"";            

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,
                                                    strSQL);

            return dt;
        }
        
        public DataTable GetFamily(string DBConnection)
        {
            string strSQL =
                @"SELECT DISTINCT Family FROM Model ORDER BY Family";

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }

        public DataTable GetModel(string DBConnection, IList<string> lFamily)
        {
            string strSQL =
                @"SELECT DISTINCT Model FROM Model
                  WHERE Family in ('{0}') AND Model Like 'PC%'
                  ORDER BY Model";
            //PC開頭為成品階

            strSQL = string.Format(strSQL, string.Join("','", lFamily.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }

        public string BuildSqlScript(string model,string qty,string line)
        { 
          
            string strSQL = @"select  p2.PartNo,p2.Descr,Convert(int,m1.Quantity)*Convert(int,m.Quantity)*{1} as Qty,'{2}' as Line
                                           from ModelBOM m,ModelBOM m1,Part p,Part p2 where
                                             m.Material='{0}'
                                             and m.Component=p.PartNo 
                                             and p.BomNodeType='VK'
                                             and m1.Material=m.Component
                                             and m1.Component=p2.PartNo
                                             and p2.BomNodeType='P1'
                                             and p.Remark='PK'

                                              Insert into #Rpt 
                                                 select p.PartNo,p.Descr,Convert(int,m.Quantity)*{1} as Qty,'{2}' as Line
                                                   from ModelBOM m,Part p where
                                                     m.Material='{0}'
                                                     and m.Component=p.PartNo 
                                                     and p.BomNodeType='C2' 

                                               Insert into #Rpt 
                                                   select p.PartNo,p.Descr,Convert(int,m.Quantity)*{1} as Qty,'{2}' as Line
                                                   from ModelBOM m,Part p,PartInfo pf where
                                                     m.Material='{0}'
                                                     and m.Component=p.PartNo 
                                                     and p.BomNodeType='P1'   
                                                     and p.PartNo=pf.PartNo
                                                     and pf.InfoType='DESC' 
                                                     and pf.InfoValue='OOA'
                                           Insert into #Rpt 
                                                
                                                select p.PartNo,p.Descr,Convert(int,m.Quantity)*{1} as Qty,'{2}' as Line
                                                 from ModelBOM m,Part p,PartInfo f where
                                                 m.Material='{0}' 
                                                 and m.Component=p.PartNo 
                                                 and p.BomNodeType='PL'
                                                 and p.PartNo=f.PartNo
                                                 and p.Descr like '%Carton%'
                                                 and f.InfoType='TYPE'
                                                 and f.InfoValue='BOX' ";

            strSQL = string.Format(strSQL, model, qty, line);
            return strSQL;
        
        
        
        }
           
    }
}

