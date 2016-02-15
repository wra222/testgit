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
    public class PAK_ASTReport : MarshalByRefObject, IPAK_ASTReport
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetASTReport(string DBConnection, DateTime StartTime, DateTime EndTime,IList<string> Family, IList<string> Model)
        {
            if (Model.Count == 0) {
               DataTable dtModel = GetASTModel(DBConnection, Family);
               if (dtModel.Rows.Count > 0)
               {
                   for (int i = 0; i < dtModel.Rows.Count; i++)
                   {
                       Model.Add(dtModel.Rows[i]["Model"].ToString());
                   }               
               }
            }

            string strSQL =
                @"WITH temp AS (
                   SELECT a.DeliveryNo,a.Model,a.Qty FROM Delivery a
                   WHERE ShipDate Between @StartTime AND @EndTime AND
                   a.Model IN ('{0}')
                 )
                 SELECT  a.Model,SUM(a.Qty) AS Qty_SUM ,b.AST_QTY , c.Value AS Cust,
					 CASE WHEN ISNULL(d.[Begin],'NULL')= 'NULL' THEN '' ELSE LTRIM(RTRIM(d.[Begin])) + '~' + LTRIM(RTRIM(d.[End])) END AS Range ,
					 e.Value AS MaxNo
                 FROM temp a	
	                LEFT JOIN 
	                (
		                SELECT a.Model,COUNT(c.PartSn) AS AST_QTY FROM temp a 
			                LEFT JOIN 	Product b ON a.DeliveryNo = b.DeliveryNo AND a.Model = b.Model
			                LEFT JOIN Product_Part c ON b.ProductID = c.ProductID AND c.BomNodeType IN ('AT','PP')		 
		                 GROUP BY a.Model
	                ) b ON a.Model = b.Model
                    LEFT JOIN ModelInfo c ON a.Model = c.Model AND Name = 'Cust'
            	    LEFT JOIN AssetRange d ON d.Code = c.Value
            	    LEFT JOIN NumControl e ON e.NoName = c.Value   
                GROUP BY a.Model, b.AST_QTY, c.Value, 
                     CASE WHEN ISNULL(d.[Begin],'NULL')= 'NULL' THEN '' ELSE LTRIM(RTRIM(d.[Begin])) + '~' + LTRIM(RTRIM(d.[End])) END ,e.Value 
                ORDER BY Model
                 ";

            strSQL = string.Format(strSQL, string.Join("','", Model.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,
                                                    strSQL,new SqlParameter("@StartTime",StartTime),new SqlParameter("@EndTime",EndTime));

            return dt;
        }
        public DataTable GetSelectDetail(string DBConnection, DateTime StartTime, DateTime EndTime, string Model)
        {
            string strSQL =
                 @"
                    SELECT a.DeliveryNo,a.Model,b.ProductID,b.CUSTSN,c.PartSn 
                    FROM Delivery a
	                    LEFT JOIN Product b ON b.DeliveryNo = a.DeliveryNo and b.Model = a.Model
	                    LEFT JOIN Product_Part c ON c.ProductID = b.ProductID AND c.BomNodeType IN ('AT','PP')
                    WHERE --ShipDate Between @StartTime AND @EndTime AND
                    a.Model IN ('{0}')
                    ORDER bY c.ProductID ";

            strSQL = string.Format(strSQL, Model);
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                        strSQL, new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));

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

        public DataTable GetASTModel(string DBConnection, IList<string> lFamily)
        {
            string strSQL =
                @"SELECT DISTINCT Model 
                  FROM Model (nolock)a
                    INNER JOIN ModelBOM b ON a.Model = b.Material 
                    INNER JOIN Part c ON b.Component = c.PartNo AND c.BomNodeType IN ('PP','AT')
                  WHERE Family in ('{0}') AND Model Like 'PC%'";
            //PC開頭為成品階

            strSQL = string.Format(strSQL, string.Join("','", lFamily.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }


    }
}

