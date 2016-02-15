using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;

namespace IMES.Query.Implementation
{
    public class PAK_KitFloatLocQuery : MarshalByRefObject, IPAK_KitFloatLocQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetData(string DBConnection, string Model, string Line)
        {

            string strSQL = @"select a.Station,a.PartNo,b.Descr,a.Qty,a.Station from WipBuffer a  WITH(NOLOCK)  join
                                          Part b  WITH(NOLOCK) on a.PartNo=b.PartNo 
                                           where a.PartNo in
                                                ( 
                                                  select PartNo from view_PAKKittingMaterial 
                                                  where Material=@Model
                                                 ) and Code=@Line ";


            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                                            System.Data.CommandType.Text,
                                                                            strSQL,
                                                                             SQLHelper.CreateSqlParameter("@Model", 32, Model),
                                                                              SQLHelper.CreateSqlParameter("@Line", 32, Line));

            return dt;
        }
             
        public DataTable GetModel(string DBConnection, IList<string> lPdLine)
        {
            string strSQL =
                @"SELECT DISTINCT Model FROM Model  WITH(NOLOCK) 
                  WHERE Family in ('{0}') AND Model Like 'PC%'
                  ORDER BY Model";
            //PC開頭為成品階

            strSQL = string.Format(strSQL, string.Join("','", lPdLine.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }                        

                
    }
}

