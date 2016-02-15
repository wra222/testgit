using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using System.Collections.Specialized;


namespace IMES.Query.Implementation
{
    public class PAK_ASTeSOPQuery : MarshalByRefObject, IPAK_ASTeSOP
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetASTeSOP(string DBConnection, string Model, string PartNo)
        {
            NameValueCollection path = GetASTPath(DBConnection);
            string path_FA = path["RDS_Server_FA"].ToString();
            string path_PAK = path["RDS_Server_PAK"].ToString();
            string strSQL =
                @"SELECT '{0}' AS Model , ISNULL(a.Value,'false') AS MN2 , '{1}' AS PartNo ,
                        CASE WHEN d.BomNodeType = 'PP' THEN d.BomNodeType
		                     ELSE d.PartType END AS [Type],
                 	    CASE WHEN d.BomNodeType = 'PP' OR 
		                          d.PartType IN ('ATSN1','ATSN2','ATSN3','ATSN5') THEN  '{2}' 
                             WHEN d.PartType IN ('ATSN4','ATSN7','ATSN8') THEN  '{3}'
                        END + a.Value + '{1}' + '.jpg' AS [url] 
                  FROM ModelInfo a (nolock)
	                    LEFT JOIN ModelInfo b (nolock) ON a.Model = b.Model AND b.Name = 'Cust'
                    	LEFT JOIN Part d (nolock) ON d.PartNo = '{1}' AND d.BomNodeType IN ('PP','AT') 
                  WHERE a.Model = '{0}' AND a.Name = 'MN2'";

            strSQL = string.Format(strSQL, Model, PartNo, path_FA, path_PAK);
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,
                                                    strSQL);

            return dt;
        }
        public DataTable GetASTeSOP(string DBConnection, string ProId)//測試環境A24300509  
        {
            NameValueCollection path = GetASTPath(DBConnection);
            string path_FA = path["RDS_Server_FA"].ToString();
            string path_PAK = path["RDS_Server_PAK"].ToString(); 
            
            string strSQL =
                @"SELECT DISTINCT LTRIM(RTRIM(a.Model)) AS Model , 
                         ISNULL(b.Value,'false') AS MN2 , 
                         LTRIM(RTRIM(d.PartNo)) AS PartNo , 
                        CASE WHEN d.BomNodeType = 'PP' THEN d.BomNodeType
		                     ELSE d.PartType END AS [Type],
                    	CASE WHEN d.BomNodeType = 'PP' OR 
			                      d.PartType IN ('ATSN1','ATSN2','ATSN3','ATSN5') THEN  '{1}' 
		                     WHEN d.PartType IN ('ATSN4','ATSN7','ATSN8') THEN  '{2}'
		                END  + b.Value + d.PartNo + '.jpg' AS [url] 
                  FROM Product  a (nolock)
                  RIGHT JOIN ModelInfo b (nolock) on a.Model = b.Model And Name = 'MN2'
                  RIGHT JOIN ModelBOM c (nolock) ON a.Model = c.Material 
	              RIGHT JOIN Part d (nolock)
	                	ON c.Component = d.PartNo  
	                	AND d.BomNodeType IN ('PP','AT')
	                	--AND d.PartType IN ('PP','ATSN1','ATSN2','ATSN3','ATSN4','ATSN5','ATSN6','ATSN7','ATSN8','ATSN9','ATSN10')
                  RIGHT JOIN ModelInfo e (nolock) on a.Model = e.Model And e.Name = 'Cust'
                  WHERE a.ProductID = '{0}' OR a.CUSTSN = '{0}'	 OR c.Material = '{0}'";

            strSQL = string.Format(strSQL, ProId, path_FA, path_PAK);
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                    strSQL);

            return dt;
        }


        public DataTable GetASTPart(string DBConnection)
        {
            string strSQL =
                @"SELECT PartNo FROM Part (nolock) WHERE BomNodeType like 'AT%'	 OR BomNodeType = 'PP'";
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                      System.Data.CommandType.Text,
                                                      strSQL);
            return dt;
        
        }
        public DataTable GetModel(string DBConnection, String PartNo)
        {
            string strSQL =
                @"SELECT Material AS Model  FROM ModelBOM (nolock) WHERE  RTRIM(LTRIM(Component)) IN ('{0}')";
            strSQL  = string.Format(strSQL,PartNo);
            
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }

        public NameValueCollection  GetASTPath(string DBConnection) {
            NameValueCollection nv = new NameValueCollection();
            string strSQL =
                @"SELECT DISTINCT Name,Value FROM dbo.SysSetting (nolock) WHERE Name IN ('RDS_Server_FA','RDS_Server_PAK')";            

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nv.Add(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Value"].ToString());
            }
            return nv;
        }
                
    }
}

