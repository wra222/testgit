using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.QueryInf;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using log4net;


namespace IMES.Station.Implementation
{
    class FAQueryImpl : MarshalByRefObject, IFAQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IFAQuery Members

        ProdutData IFAQuery.ProductInfo(string ID, out List<NextStation> NextStationList)
        {
            NextStationList = new List<NextStation>();
            ProdutData product = null;
            /*string strSQL = @" select  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO,
                                                     a.PCBID, a.PCBModel, a.Model,d.Family,
                                                     a.CartonSN, a.PalletNo, a.DeliveryNo,  
                                                      a.MO , b.Station, c.Descr, 
                                                      b.Line, b.Status, b.TestFailCount,  b.Udt    
                                             from Product a ,ProductStatus b, Station c, Model d
                                            where (a.ProductID = @Product or
                                                         a.CUSTSN = @Product) and
                                                   a.ProductID = b.ProductID and
                                                   a.Model = d.Model   and
                                                   b.Station = c.Station  ";*/

            string strSQL = @"
                IF EXISTS(SELECT ProductID FROM Product_Part WHERE Value=@Product)
                BEGIN
                   SELECT  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO,
                           a.PCBID, a.PCBModel, a.Model,d.Family,
                           a.CartonSN, a.PalletNo, a.DeliveryNo,
                           a.MO , b.Station, c.Descr,
                           b.Line, b.Status, b.TestFailCount, b.Udt
                   FROM    Product a ,ProductStatus b, Station c, Model d
                   WHERE   a.ProductID IN
			                (SELECT ProductID FROM Product_Part WHERE  Value=@Product)
                           AND a.ProductID = b.ProductID
                           AND a.Model = d.Model
                           AND b.Station = c.Station
                END
                ELSE 
                BEGIN
                   SELECT  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO,
		                   a.PCBID, a.PCBModel, a.Model,d.Family,
		                   a.CartonSN, a.PalletNo, a.DeliveryNo,  
		                   a.MO , b.Station, c.Descr, 
		                   b.Line, b.Status, b.TestFailCount,  b.Udt    
                   FROM    Product a ,ProductStatus b, Station c, Model d
                   WHERE   (a.ProductID = @Product 
   			                OR a.CUSTSN = @Product 
			                OR a.PCBID = @Product ) 
		                  AND a.ProductID = b.ProductID 
		                  AND a.Model = d.Model
		                  AND b.Station = c.Station
                END  ";

            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL,
                                                                                paraName);
            if (tb.Rows.Count == 1)
            {
                product = new ProdutData();
                product.CartonSN = tb.Rows[0]["CartonSN"].ToString().Trim();
                product.CustomSN = tb.Rows[0]["CUSTSN"].ToString().Trim();
                product.DeliveryNo = tb.Rows[0]["DeliveryNo"].ToString().Trim();
                product.ECR = tb.Rows[0]["ECR"].ToString().Trim();
                product.Family = tb.Rows[0]["Family"].ToString().Trim();
                product.Line = tb.Rows[0]["Line"].ToString().Trim();
                product.MAC = tb.Rows[0]["MAC"].ToString().Trim();
                product.MBPartNo = tb.Rows[0]["PCBModel"].ToString().Trim();
                product.MBSN = tb.Rows[0]["PCBID"].ToString().Trim();
                product.MO = tb.Rows[0]["MO"].ToString().Trim();
                product.Model = tb.Rows[0]["Model"].ToString().Trim();
                product.PalletNo = tb.Rows[0]["PalletNo"].ToString().Trim();
                product.ProductID = tb.Rows[0]["ProductID"].ToString().Trim();     
                    
               
                product.Station = tb.Rows[0]["Station"].ToString().Trim();
                product.StationDescr = tb.Rows[0]["Descr"].ToString().Trim();
                product.Status = (int)tb.Rows[0]["Status"];
                product.TestFailCount = (int)tb.Rows[0]["TestFailCount"];
                product.Udt = (DateTime)tb.Rows[0]["Udt"];
                strSQL = @"select b.Station,c.Descr
                                     from Model_Process a , Process_Station b, Station c
                                     where a.Model = @Model and
                                           a.Process = b.Process  and
                                           b.PreStation =@Station and
                                           b.Status=@Status  and 
                                           b.Station =c.Station";

                SqlParameter para1 = new SqlParameter("@Model", SqlDbType.VarChar, 32);
                para1.Direction = ParameterDirection.Input;
                para1.Value = product.Model;
                SqlParameter para2 = new SqlParameter("@Station ", SqlDbType.VarChar, 32);
                para2.Direction = ParameterDirection.Input;
                para2.Value = product.Station;
                SqlParameter para3 = new SqlParameter("@Status ", SqlDbType.Int);
                para3.Direction = ParameterDirection.Input;
                para3.Value = product.Status;
                DataTable tb1 = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                     System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    para1,
                                                                                    para2,
                                                                                    para3);
                foreach (DataRow dr in tb1.Rows)
                {
                    NextStation nextStation = new NextStation();
                    nextStation.Station = dr["Station"].ToString().Trim();
                    nextStation.Description = dr["Descr"].ToString().Trim();
                    NextStationList.Add(nextStation);
                }
            }
            return product;
        }

        DataTable IFAQuery.GetProductHistory(string ID)
        {
            string strSQL = @"select  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') as FixtureID, 
                    (case a.Status when 1 then 'Pass' else 'False' end) as Status , 
                ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,a.Cdt 
                from ProductLog a 
                inner join Station c
                on a.Station = c.Station                
                inner join Product d on a.ProductID=d.ProductID
                left  join ProductTestLog b                 
                on a.ProductID= b.ProductID and a.Station = b.Station and a.Cdt =b.Cdt                                  
                where a.ProductID =@Product
                order by a.Cdt";
            //where (d.ProductID =@Product or d.CUSTSN=@Product)
            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL,
                                                                                paraName);

            return tb;
        }

        DataTable IFAQuery.GetStation()
        {
            string strSQL = @"select Station, Descr from Station where OperationObject=2 and StationType != 'FARepair' and Station NOT IN ('68','82') order by Station";
            
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);

            return tb;
        }

        DataTable IFAQuery.GetMultiProductInfo(IList<string> CustSNList)
        {
            string strSQL = @"select  a.ProductID,case when b.Station in ('82','83','85','87','89') then 'NO' else 'YES' end as AllowJump, 
                                      a.Model, a.CUSTSN, a.PCBID, b.Station, RTrim(b.Station) +' - '+ c.Descr as StationDesc,
                                      CASE b.Status WHEN 1 THEN 'PASS' ELSE 'Fail' END AS Status
                              from Product a, ProductStatus b, Station c 
                              where a.ProductID= b.ProductID and b.Station = c.Station and
                                    a.CUSTSN in ('{0}')  ";


            string inSection = string.Join("','", CustSNList.ToArray());
                        
            strSQL = string.Format(strSQL, inSection);

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);

            return tb;
        }

        void IFAQuery.UpdateProdStatus(IList<string> CustSNList,
                          string station,
                          int status,
                          string editor)
        {
            {
                try
                {

                    SqlTransactionManager.Begin();
                    DateTime now = DateTime.Now;
                    string strSQL = @"update ProductStatus 
                                            set Station= @station ,
                                                   Status= @status,
                                                   TestFailCount=0,
                                                   Editor =@editor,
                                                   Udt=@now 
                                            where ProductID in (SELECT ProductID FROM Product WHERE CUSTSN in ('{0}')) ";
                    string inSection = string.Join("','", CustSNList.ToArray());
                    strSQL = string.Format(strSQL, inSection);

                    logger.Error("UpdateProdStatus SQL:" + strSQL);

                    SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
                    paraStation.Direction = ParameterDirection.Input;
                    paraStation.Value = station;
                    SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
                    paraStatus.Direction = ParameterDirection.Input;
                    paraStatus.Value = status;
                    SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
                    paraEditor.Direction = ParameterDirection.Input;
                    paraEditor.Value = editor;
                    SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
                    paraNow.Direction = ParameterDirection.Input;
                    paraNow.Value = now;
                    SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                                   CommandType.Text,
                                                                   strSQL,
                                                                   paraStation, paraStatus, paraEditor, paraNow);

                    // write product Log
                    strSQL = @"insert ProductLog (ProductID, Model, Station, Status, Line, Editor, Cdt)
                                    select ProductID, Model, @station,@status,@line,@editor,@now 
                                      from Product 
                                      where CUSTSN in ('{0}') ";

                    strSQL = string.Format(strSQL, inSection);

                    SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
                    paraLine.Direction = ParameterDirection.Input;
                    paraLine.Value = "NA";

                    SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                                   CommandType.Text,
                                                                   strSQL,
                                                                   paraStation,
                                                                   paraStatus,
                                                                   paraLine,
                                                                   paraEditor,
                                                                   paraNow);


                    SqlTransactionManager.Commit();
                }
                catch (Exception e)
                {
                    SqlTransactionManager.Rollback();
                    throw e;
                }
            }

        }

        #endregion


        #region

        private DataTable CreateLogTable()
        {
            DataTable newLog = new DataTable("ProductLog");

            // Add PCBLog column objects to the table.
            DataColumn ID = new DataColumn();
            ID.DataType = System.Type.GetType("System.Int32");
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            newLog.Columns.Add(ID);

            DataColumn pcbNo = new DataColumn();
            pcbNo.DataType = System.Type.GetType("System.String");
            pcbNo.ColumnName = "ProductID";
            newLog.Columns.Add(pcbNo);

            DataColumn modelID = new DataColumn();
            modelID.DataType = System.Type.GetType("System.String");
            modelID.ColumnName = "Model";
            newLog.Columns.Add(modelID);

            DataColumn station = new DataColumn();
            station.DataType = System.Type.GetType("System.String");
            station.ColumnName = "Station";
            newLog.Columns.Add(station);

            DataColumn line = new DataColumn();
            line.DataType = System.Type.GetType("System.String");
            line.ColumnName = "Line";
            newLog.Columns.Add(line);

            DataColumn status = new DataColumn();
            status.DataType = System.Type.GetType("System.Int32");
            status.ColumnName = "Status";
            newLog.Columns.Add(status);

            DataColumn editor = new DataColumn();
            editor.DataType = System.Type.GetType("System.String");
            editor.ColumnName = "Editor";
            newLog.Columns.Add(editor);

            DataColumn cdt = new DataColumn();
            cdt.DataType = System.Type.GetType("System.DateTime");
            cdt.ColumnName = "Cdt";
            newLog.Columns.Add(cdt);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            newLog.PrimaryKey = keys;

            // Return the new DataTable. 
            return newLog;

        }

        private void addLog(DataTable dt,
                                          string productID,
                                        string model,
                                        string station,
                                        int status,
                                        string editor,
                                        DateTime now)
        {

            // Add some new rows to the collection. 
            DataRow row = dt.NewRow();
            row["ProductID"] = productID;
            row["Model"] = model;
            row["Station"] = station;
            row["Line"] = "NA";
            row["Status"] = status;
            row["Editor"] = editor;
            row["Cdt"] = now;
            dt.Rows.Add(row);
            dt.AcceptChanges();
        }
        #endregion
       
    }
}
