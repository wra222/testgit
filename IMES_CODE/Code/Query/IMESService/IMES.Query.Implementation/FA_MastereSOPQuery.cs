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
    public class FA_MastereSOPQuery : MarshalByRefObject, IFA_MastereSOPQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        public DataTable GetASTeSOP(string DBConnection, string Model, string PartNo)
//        {
//            NameValueCollection path = GetASTPath(DBConnection);
//            string path_FA = path["RDS_Server_FA"].ToString();
//            string path_PAK = path["RDS_Server_PAK"].ToString();
//            string strSQL =
//                @"SELECT '{0}' AS Model , ISNULL(a.Value,'false') AS MN2 , '{1}' AS PartNo ,
//                        CASE WHEN d.BomNodeType = 'PP' THEN d.BomNodeType
//		                     ELSE d.PartType END AS [Type],
//                 	    CASE WHEN d.BomNodeType = 'PP' OR 
//		                          d.PartType IN ('ATSN1','ATSN2','ATSN3','ATSN5') THEN  '{2}' 
//                             WHEN d.PartType IN ('ATSN4','ATSN7','ATSN8') THEN  '{3}'
//                        END + a.Value + '{1}' + '.jpg' AS [url] 
//                  FROM ModelInfo a (nolock)
//	                    LEFT JOIN ModelInfo b (nolock) ON a.Model = b.Model AND b.Name = 'Cust'
//                    	LEFT JOIN Part d (nolock) ON d.PartNo = '{1}' AND d.BomNodeType IN ('PP','AT') 
//                  WHERE a.Model = '{0}' AND a.Name = 'MN2'";

//            strSQL = string.Format(strSQL, Model, PartNo, path_FA, path_PAK);
//            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,
//                                                    strSQL);

//            return dt;
//        }
        public DataTable GetMastereSOP(string DBConnection, string ProId)//測試環境A24300509  
        {
            //NameValueCollection path = GetASTPath(DBConnection);
            //string path_FA = path["RDS_Server_FA"].ToString();
            //string path_PAK = path["RDS_Server_PAK"].ToString();

            string strSQL =
                @"EXEC Master_Label_Esop '{0}'
                   ";

            strSQL = string.Format(strSQL, ProId);
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                    strSQL);

            return dt;
            //string methodName = MethodBase.GetCurrentMethod().Name;
            //BaseLog.LoggingBegin(logger, methodName);
            //try
            //{
            //    string SQLText = @"Master_Label_Esop ";
            //    return SQLHelper.ExecuteDataFill(DBConnection,
            //                                                      System.Data.CommandType.StoredProcedure,
            //                                                      SQLText,
            //                                                       SQLHelper.CreateSqlParameter("@ProId", 32, ProId));

            //}
            //catch (Exception e)
            //{

            //    BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
            //    throw;
            //}
            //finally
            //{
            //    BaseLog.LoggingEnd(logger, methodName);
            //}
        }


        //public DataTable GetASTPart(string DBConnection)
        //{
        //    string strSQL =
        //        @"SELECT PartNo FROM Part (nolock) WHERE BomNodeType like 'AT%'	 OR BomNodeType = 'PP'";
        //    DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
        //                                              System.Data.CommandType.Text,
        //                                              strSQL);
        //    return dt;
        
        //}
        //public DataTable GetModel(string DBConnection, String PartNo)
        //{
        //    string strSQL =
        //        @"SELECT Material AS Model  FROM ModelBOM (nolock) WHERE  RTRIM(LTRIM(Component)) IN ('{0}')";
        //    strSQL  = string.Format(strSQL,PartNo);
            
        //    DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
        //                                             System.Data.CommandType.Text,
        //                                             strSQL);
        //    return dt;
        //}

        //public NameValueCollection  GetASTPath(string DBConnection) {
        //    NameValueCollection nv = new NameValueCollection();
        //    string strSQL =
        //        @"SELECT DISTINCT Name,Value FROM dbo.SysSetting (nolock) WHERE Name IN ('RDS_Server_FA','RDS_Server_PAK')";            

        //    DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
        //                                             System.Data.CommandType.Text,
        //                                             strSQL);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        nv.Add(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Value"].ToString());
        //    }
        //    return nv;
        //}
                
    }
}

