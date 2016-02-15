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
    public class Model_ASTQuery : MarshalByRefObject, IModel_ASTQyery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetData1(string DBConnection, DataTable ModelLine)
        {

            string model;
            
            StringBuilder sb = new StringBuilder();
            sb.Append(@"CREATE TABLE #Data  (PartNo varchar(64),Descr varchar(64), Qty int,Line varchar(16) ) ;
                                 CREATE TABLE #Rpt  (Material varchar(64)) ");
            string strInsert = "Insert #Rpt Values('{0}')  ";
            string tmp = "";
            foreach (DataRow dr in ModelLine.Rows)
            {
                model = dr["Model"].ToString();
                
                if (!string.IsNullOrEmpty(model))
                {

                    tmp = string.Format(strInsert, model);
                    sb.Append(tmp);
                }

            }          
            sb.Append(@"  Insert into   #Data
                                        SELECT a.Material as PartNo,a.Component as Descr,a.Quantity
                                         as Qty,'' FROM dbo.ModelBOM a,#Rpt b
                                         where a.Material=b.Material
                                         and  Component like'%2TG%' ; 

                              SELECT PartNo as Model,convert(varchar(10),count(Descr))+' '+'Component with %2TG%' as Qty from #Data
                              group by PartNo
                                           ");
           

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                    sb.ToString());

            return dt;

        }
    }
}
