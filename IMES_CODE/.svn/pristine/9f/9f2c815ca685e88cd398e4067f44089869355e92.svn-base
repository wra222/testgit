using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;


namespace IMES.Query.Implementation
{
    public class FA_ProductInfo : MarshalByRefObject,IFA_ProductInfo
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
              
        ProdutData IFA_ProductInfo.ProductInfo(string Connection, string ID, out List<NextStation> NextStationList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {       

            NextStationList = new List<NextStation>();
            ProdutData product = null;            

            string strSQL = @"
                IF EXISTS(SELECT ProductID FROM Product_Part (NOLOCK) WHERE PartSn=@Product)
                BEGIN
                   SELECT  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO, a.UnitWeight, 
                           a.PCBID, a.PCBModel, a.Model,d.Family,
                           a.CartonSN, a.PalletNo, a.DeliveryNo,
                           a.MO , b.Station, c.Descr,
                           b.Line, b.Status, b.TestFailCount, CONVERT (nvarchar(20),b.Udt,120) AS Udt,
                            e.SnoId as [WHLocation],
                            f.ShipDate
                    FROM    Product a (NOLOCK)
                           LEFT JOIN ProductStatus b (NOLOCK)  ON a.ProductID = b.ProductID 
                           LEFT JOIN Station c (NOLOCK)	ON b.Station = c.Station
                           LEFT JOIN Model d (NOLOCK)  ON a.Model = d.Model
                           LEFT JOIN (Select * from PAK_LocMas  (NOLOCK)
                             where Pno<>'' and Pno is not null) e on a.PalletNo=e.Pno
                           LEFT JOIN Delivery f ON a.DeliveryNo=f.DeliveryNo 
                   WHERE   a.ProductID IN
                            (SELECT TOP 1 ProductID FROM Product_Part (NOLOCK) WHERE  PartSn=@Product)                           
                END

                ELSE IF EXISTS(
                    SELECT TOP 1 ProductID FROM Product
                    WHERE ProductID = @Product OR CUSTSN = @Product OR PCBID = @Product OR MAC=@Product
                ) 
                BEGIN
                   SELECT  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO, a.UnitWeight, 
                           a.PCBID, a.PCBModel, a.Model,d.Family,
                           a.CartonSN, a.PalletNo, a.DeliveryNo,  
                           a.MO , b.Station, c.Descr, 
                           b.Line, b.Status, b.TestFailCount,  CONVERT (nvarchar(20),b.Udt,120) AS Udt    ,
                           e.SnoId as [WHLocation],
                            f.ShipDate
                   FROM    Product a (NOLOCK)
                           LEFT JOIN ProductStatus b (NOLOCK)  ON a.ProductID = b.ProductID 
                           LEFT JOIN Station c (NOLOCK)	ON b.Station = c.Station
                           LEFT JOIN Model d (NOLOCK)  ON a.Model = d.Model
                           LEFT JOIN  (Select * from PAK_LocMas  (NOLOCK)
                             where Pno<>'' and Pno is not null) e on a.PalletNo=e.Pno
                            LEFT JOIN Delivery f ON a.DeliveryNo=f.DeliveryNo 
                   WHERE  a.ProductID = @Product OR a.CUSTSN = @Product OR a.PCBID = @Product  OR MAC=@Product
                END




                ELSE IF EXISTS(
                 SELECT PCBNo FROM  PCBInfo WHERE InfoValue=@Product
                ) 
                
                BEGIN
                   SELECT  a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO, a.UnitWeight, 
                           a.PCBID, a.PCBModel, a.Model,d.Family,
                           a.CartonSN, a.PalletNo, a.DeliveryNo,  
                           a.MO , b.Station, c.Descr, 
                           b.Line, b.Status, b.TestFailCount,  CONVERT (nvarchar(20),b.Udt,120) AS Udt    ,
                           e.SnoId as [WHLocation],
                           f.ShipDate                                                                  
                   FROM    Product a (NOLOCK)
                           LEFT JOIN ProductStatus b (NOLOCK)  ON a.ProductID = b.ProductID 
                           LEFT JOIN Station c (NOLOCK)	ON b.Station = c.Station
                           LEFT JOIN Model d (NOLOCK)  ON a.Model = d.Model
                           LEFT JOIN  (Select * from PAK_LocMas  (NOLOCK)
                             where Pno<>'' and Pno is not null) e on a.PalletNo=e.Pno
                           LEFT JOIN Delivery f ON a.DeliveryNo=f.DeliveryNo                         
                   WHERE    a.ProductID = (
                                           select ProductID from Product where PCBID
                                                     = (select PCBNo from PCBInfo where InfoValue=@Product)
                                          )
                 END
                ELSE
                BEGIN
                  SELECT TOP 1 a.ProductID , a.CUSTSN, a.MAC, a.MBECR as ECR, a.MO, a.UnitWeight, 
                           a.PCBID, a.PCBModel, a.Model,d.Family,
                           a.CartonSN, a.PalletNo, a.DeliveryNo,  
                           a.MO , b.Station, c.Descr, 
                           b.Line, b.Status, b.TestFailCount,  CONVERT (nvarchar(20),b.Udt,120) AS Udt    ,
                           e.SnoId as [WHLocation],
                           f.ShipDate
                   FROM    UnpackProduct up (NOLOCK) 
		                   LEFT JOIN Product a (NOLOCK) ON a.ProductID = up.ProductID
		                   LEFT JOIN ProductStatus b (NOLOCK)  ON a.ProductID = b.ProductID 
		                   LEFT JOIN Station c (NOLOCK)	ON b.Station = c.Station
		                   LEFT JOIN Model d (NOLOCK)  ON a.Model = d.Model
                           LEFT JOIN (Select * from PAK_LocMas  (NOLOCK)
                             where Pno<>'' and Pno is not null) e on a.PalletNo=e.Pno
                           LEFT JOIN Delivery f ON a.DeliveryNo=f.DeliveryNo 

                   WHERE   up.ProductID = @Product OR up.CUSTSN = @Product
                   ORDER BY up.Cdt DESC
                END";

            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SQLHelper.ExecuteDataFill(Connection,
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
                product.WHLocation = tb.Rows[0]["WHLocation"].ToString().Trim();
                product.UnitWeight = Double.Parse(tb.Rows[0]["UnitWeight"].ToString().Trim()) == 0 ? "" : tb.Rows[0]["UnitWeight"].ToString().Trim();

                product.Station = tb.Rows[0]["Station"].ToString().Trim();
                product.StationDescr = tb.Rows[0]["Descr"].ToString().Trim();
                product.Status = (int)tb.Rows[0]["Status"];
                product.TestFailCount = (int)tb.Rows[0]["TestFailCount"];
                product.Udt = DateTime.Parse(tb.Rows[0]["Udt"].ToString());

                product.ShipDate = tb.Rows[0]["ShipDate"].ToString();          //shipdate

                strSQL = @"IF  ((SELECT COUNT('X') FROM ForceNWC WHERE ProductID = @ProductID )= 0 ) 
		                       OR
                                ((SELECT COUNT('X') FROM ForceNWC a (NOLOCK),
							                            ProductLog b (NOLOCK)
	                            WHERE a.ProductID  = @ProductID 
		                            AND a.ProductID = b.ProductID 
	                            AND a.Udt < b.Cdt) > 0)
                               BEGIN                                    
                                    If EXISTS (SELECT * FROM Model_Process (NOLOCK) WHERE Model = @Model )
		                                BEGIN		
		                                  SELECT b.Station,c.Descr
                                           FROM Model_Process a (NOLOCK), Process_Station b (NOLOCK) , Station c (NOLOCK)
                                           WHERE a.Model = @Model 
                                           AND a.Process = b.Process  
                                           AND b.PreStation =@Station
                                           AND b.Status=@Status
                                           AND b.Station =c.Station	                                    
                                            RETURN
                                        END
                                        ELSE IF EXISTS(SELECT * FROM ProcessRule (NOLOCK) WHERE Value1 = @Family)
		                                BEGIN
			                                SELECT b.Station,c.Descr
                                           FROM ProcessRule a (NOLOCK), Process_Station b (NOLOCK) , Station c (NOLOCK)
                                           WHERE a.Value1 = @Family
                                           AND a.Process = b.Process  
                                           AND b.PreStation =@Station
                                           AND b.Status=@Status
                                           AND b.Station =c.Station	                                    
                                           RETURN
		                                END                                             
                                           SELECT '' AS Station,'' AS Descr                                        
                               END
                               ELSE 
                               BEGIN
	                                SELECT b.Station ,b.Descr FROM ForceNWC a (NOLOCK),Station b (NOLOCK)
	                                WHERE a.ProductID = @ProductID AND a.ForceNWC = b.Station
                               END"
                            ;

                SqlParameter para1 = new SqlParameter("@Model", SqlDbType.VarChar, 32);
                para1.Direction = ParameterDirection.Input;
                para1.Value = product.Model;

                SqlParameter para2 = new SqlParameter("@Station ", SqlDbType.VarChar, 32);
                para2.Direction = ParameterDirection.Input;
                para2.Value = product.Station;
                SqlParameter para3 = new SqlParameter("@Status ", SqlDbType.Int);
                para3.Direction = ParameterDirection.Input;
                para3.Value = product.Status;

                SqlParameter para4 = new SqlParameter("@ProductID ", SqlDbType.VarChar, 32);
                para4.Direction = ParameterDirection.Input;
                para4.Value = product.ProductID;
                SqlParameter para5 = new SqlParameter("@Family", SqlDbType.VarChar, 32);
                para5.Direction = ParameterDirection.Input;
                para5.Value = product.Family;

                DataTable tb1 = SQLHelper.ExecuteDataFill(Connection,
                                                          System.Data.CommandType.Text,
                                                          strSQL,
                                                          para1,
                                                          para2,
                                                          para3,
                                                          para4,
                                                          para5);
                foreach (DataRow dr in tb1.Rows)
                {
                    NextStation nextStation = new NextStation();
                    nextStation.Station = dr["Station"].ToString().Trim();
                    nextStation.Description = dr["Descr"].ToString().Trim();
                    NextStationList.Add(nextStation);
                }
            }
            else if (tb.Rows.Count == 0)
            {
                product = new ProdutData();
                product.CartonSN = "";
                product.CustomSN = "";
                product.DeliveryNo = "";
                product.ECR = "";
                product.Family = "";
                product.Line = "";
                product.MAC = "";
                product.MBPartNo = "";
                product.MBSN = "";
                product.MO = "";
                product.Model = "";
                product.PalletNo = "";
                product.ProductID = "";
                product.WHLocation = "";
                product.UnitWeight = "";
                product.Station = "";
                product.StationDescr = "";
                product.Status = 0;
                product.TestFailCount = 0;
                product.Udt = DateTime.Parse("1900-01-01");

                product.ShipDate = "";
            }

                return product;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductHistory(string Connection, string ID)
        {
            return ((IFA_ProductInfo)this).GetProductHistory(Connection, ID, "");
        }

        DataTable IFA_ProductInfo.GetProductHistory(string Connection, string ID, string CUSTSN)
        {       
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {     

//            string strSQL = @"SELECT  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') AS FixtureID, 
//                                (CASE a.Status WHEN 1 THEN 'Pass' ELSE 'Fail' END) AS Status , 
//                            ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt
//                            FROM ProductLog a (NOLOCK) INNER JOIN Station c (NOLOCK) ON a.Station = c.Station                
//                            INNER JOIN Product d (NOLOCK) ON a.ProductID=d.ProductID
//                            LEFT  JOIN ProductTestLog b (NOLOCK)       
//                            ON a.ProductID= b.ProductID AND a.Station = b.Station AND a.Cdt =b.Cdt                                  
//                            WHERE a.ProductID =@Product
//                            ORDER BY a.Cdt";
            // 120725 union RePrintLog
            // 120730 union PrintLog
            // 120817 becasuse PrintLog and RePrintLog have trash data,
            //        add  " AND BegNo LIKE SUBSTRING(@Product,1,1)+'%' AND EndNo LIKE SUBSTRING(@Product,1,1)+'%'  "
            string strSQL = @"SELECT  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') AS FixtureID, 
                            (CASE a.Status WHEN 1 THEN 'Pass' ELSE 'Fail' END) AS Status , 
                        ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt
                        FROM ProductLog a (NOLOCK) INNER JOIN Station c (NOLOCK) ON a.Station = c.Station                
                        INNER JOIN Product d (NOLOCK) ON a.ProductID=d.ProductID
                        LEFT  JOIN ProductTestLog b (NOLOCK)       
                        ON a.ProductID= b.ProductID AND a.Station = b.Station AND a.Cdt =b.Cdt                                  
                        WHERE a.ProductID =@Product
                        union all
                        select 'Reprint' as Station, LabelName as StationName, '' as Line, '' as FixtureID, 'Pass' AS Status,
                        '', Editor, CONVERT (nvarchar(20),Cdt,120) AS Cdt
						from RePrintLog (NOLOCK) where 
                        (@Product between BegNo and EndNo
                        AND BegNo LIKE SUBSTRING(@Product,1,1)+'%'
                        AND EndNo LIKE SUBSTRING(@Product,1,1)+'%')
                        or BegNo=@Product or Descr=@Product or
                        (@CUSTSN between BegNo and EndNo) or BegNo=@CUSTSN or Descr=@CUSTSN
                        union all
                        select 'Print' as Station, Name as StationName, '' as Line, '' as FixtureID, 'Pass' AS Status,
                        '', Editor, CONVERT (nvarchar(20),Cdt,120) AS Cdt
						from PrintLog (NOLOCK) where 
                        (@Product between BegNo and EndNo
                        AND BegNo LIKE SUBSTRING(@Product,1,1)+'%'
                        AND EndNo LIKE SUBSTRING(@Product,1,1)+'%')
                        or BegNo=@Product or Descr=@Product or
                        (@CUSTSN between BegNo and EndNo) or BegNo=@CUSTSN or Descr=@CUSTSN
                        ORDER BY Cdt,a.Station";
            // RePrintLog 的 BegNo,EndNo 可能為 CUSTSN

            //where (d.ProductID =@Product or d.CUSTSN=@Product)
            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            SqlParameter para2 = new SqlParameter("@CUSTSN ", SqlDbType.VarChar, 32);
            para2.Direction = ParameterDirection.Input;
            para2.Value = CUSTSN != ""? CUSTSN : "--------------" ;
            DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName,
                                                     para2);

            return tb;
                            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }

        DataTable IFA_ProductInfo.GetProductRepair(string Connection, string ID)
        {
                        string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {  
            string strSQL = @"SELECT r.Line,RTRIM(r.Station)+' - '+s.Descr as StationName,Status,
                                r_info.Location ,RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,    
                                RTRIM(r_info.Cause)+' - '+di.Description as Cause,
                                RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
                                r_info.Remark, r.Editor, 
                                CONVERT (nvarchar(20),r.Cdt,120) AS Cdt
                             FROM Product a (NOLOCK) 
                             LEFT JOIN ProductRepair r (NOLOCK) ON a.ProductID=r.ProductID
                             LEFT JOIN Station s (NOLOCK) on r.Station=s.Station
                             INNER JOIN ProductRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.ProductRepairID
                             LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
                             LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='FACause'
                             LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
                             WHERE a.ProductID=@Product
                             ORDER BY a.Cdt";                             
            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName);

            return tb;
                            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

        }

        DataTable IFA_ProductInfo.GetProductInfo(string Connection, string ID)
        {

                        string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {  
            string strSQL = @"SELECT InfoType,InfoValue,Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt
                             FROM ProductInfo a (NOLOCK) 
                             WHERE a.ProductID=@Product
                             ORDER BY a.Cdt";
            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName);

            return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductPart(string Connection, string ID)
        {

                        string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {  
            string strSQL = @"
                        SELECT a.PartNo,a.PartType,c.Descr,PartSn,a.BomNodeType,CheckItemType,RTRIM(a.Station)+' - '+b.Descr as Station,a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt 
                        FROM Product_Part a (NOLOCK) 
                        LEFT JOIN Station b (NOLOCK) ON a.Station=b.Station 
                        LEFT JOIN Part c (NOLOCK) ON a.PartNo = c.PartNo 
                        WHERE a.ProductID= @Product
                        UNION 
                        SELECT a.PartNo,a.PartType,c.Descr,PartSn,a.BomNodeType,CheckItemType,RTRIM(a.Station)+' - '+b.Descr as Station,a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt 
                        FROM Pizza_Part a (NOLOCK) 
                        LEFT JOIN Station b (NOLOCK) ON a.Station=b.Station 
                        LEFT JOIN Part c (NOLOCK) ON a.PartNo = c.PartNo 
                        WHERE  a.PizzaID = (SELECT TOP 1 PizzaID FROM Product WHERE ProductID = @Product )
                        ORDER BY CONVERT (nvarchar(20),a.Cdt,120) ";
            SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = ID;
            DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName);

            return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductUnpack(string Connection, string ID)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string strSQL = @"
                    IF EXISTS(SELECT ProductID FROM UnpackProduct_Part (NOLOCK) WHERE PartSn=@Product)
                                    BEGIN
                                       select ProductID,Model,PCBID,PCBModel,MAC,CUSTSN,UEditor,UPdt
                                       from UnpackProduct
                                       WHERE  ProductID IN
                                                (SELECT distinct ProductID FROM UnpackProduct_Part (NOLOCK) WHERE  PartSn=@Product)                           
                                    END

                    ELSE IF EXISTS(SELECT ProductID FROM UnpackProduct (NOLOCK) 
                                    WHERE ProductID = @Product OR CUSTSN = @Product OR PCBID = @Product OR MAC=@Product)
                                    BEGIN
                                       select ProductID,Model,PCBID,PCBModel,MAC,CUSTSN,UEditor,UPdt
                                       from UnpackProduct
                                       WHERE  ProductID IN
                                                (SELECT distinct ProductID FROM UnpackProduct (NOLOCK) WHERE 
                                                ProductID = @Product OR CUSTSN = @Product OR PCBID = @Product  OR MAC=@Product)                           
                                    END

                    ELSE IF EXISTS(SELECT ProductID FROM UnpackProductInfo (NOLOCK) WHERE InfoValue = @Product)
                                    BEGIN
                                       select ProductID,Model,PCBID,PCBModel,MAC,CUSTSN,UEditor,UPdt
                                       from UnpackProduct
                                       WHERE  ProductID IN
                                                (SELECT distinct ProductID FROM UnpackProductInfo (NOLOCK) WHERE InfoValue = @Product)                           
                                    END ";
                SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ID;
                DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                         System.Data.CommandType.Text,
                                                         strSQL,
                                                         paraName);

                return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductChange(string Connection, string ID)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string strSQL = @"
                    IF EXISTS(SELECT NewPartSno FROM ProductRepair_DefectInfo (NOLOCK) WHERE OldPartSno=@Product and PartType='MB')
                            BEGIN
                                SELECT ProductID,Model,PCBID,PCBModel,MAC,CUSTSN,Cdt FROM dbo.Product WHERE PCBID IN 
                                (SELECT NewPartSno FROM ProductRepair_DefectInfo WHERE OldPartSno =@Product and PartType='MB')
                            END
                    IF EXISTS(SELECT NewPartSno FROM ProductRepair_DefectInfo (NOLOCK) WHERE OldPartSno=@Product and PartType='KP/ME')
                            BEGIN
                                SELECT ProductID,Model,PCBID,PCBModel,MAC,CUSTSN,Cdt From dbo.Product WHERE ProductID IN
                                (SELECT distinct ProductID FROM dbo.Product_Part WHERE PartSn IN 
									(SELECT NewPartSno FROM ProductRepair_DefectInfo WHERE OldPartSno =@Product and PartType='KP/ME'))
                            END ";
                SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ID;
                DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                         System.Data.CommandType.Text,
                                                         strSQL,
                                                         paraName);

                return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductCRPart(string Connection, string ID)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string strSQL = @"
                                SELECT a.PartNo,a.PartType,c.Descr,PartSn,a.BomNodeType,CheckItemType,RTRIM(a.Station)+' - '+b.Descr as Station,a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt 
                                FROM Product_Part a (NOLOCK) 
                                LEFT JOIN Station b (NOLOCK) ON a.Station=b.Station 
                                LEFT JOIN Part c (NOLOCK) ON a.PartNo = c.PartNo
                                left JOIN Product d (NOLOCK) on a.ProductID=d.ProductID
                                WHERE d.Model like 'CR%' and a.ProductID= @Product" ;
                SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ID;
                DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                         System.Data.CommandType.Text,
                                                         strSQL,
                                                         paraName);

                return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductCRLog(string Connection, string ID)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string strSQL = @"
                                SELECT  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') AS FixtureID, 
                                (CASE a.Status WHEN 1 THEN 'Pass' ELSE 'Fail' END) AS Status , 
                                ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,CONVERT (nvarchar(20),a.Cdt,120) AS Cdt
                                FROM ProductLog a (NOLOCK) INNER JOIN Station c (NOLOCK) ON a.Station = c.Station                
                                INNER JOIN Product d (NOLOCK) ON a.ProductID=d.ProductID
                                LEFT  JOIN ProductTestLog b (NOLOCK)       
                                ON a.ProductID= b.ProductID AND a.Station = b.Station AND a.Cdt =b.Cdt                                  
                                WHERE d.Model like 'CR%' and a.ProductID =@Product ";
                SqlParameter paraName = new SqlParameter("@Product", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ID;
                DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                         System.Data.CommandType.Text,
                                                         strSQL,
                                                         paraName);

                return tb;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        DataTable IFA_ProductInfo.GetProductITCND(string Connection, string ID)
        {
            string methodname = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodname);
            try
            {
                string strSQL = @"
                            if exists( select 1 from sys.tables where name='ProductAttrLog')
                                begin
                                    if  exists (select 1 from ProductAttrLog (nolock)
                                                where  Descr in (select MAC from Product(nolock)) 
                                                and  LEN(Descr)=12 
                                                and AttrName='TEST_IPADDRESS' and Descr=@ProductID)
	                                    begin 
		                                    select Descr as Sno ,AttrNewValue as Remark ,Cdt 
                                            from ProductAttrLog(nolock) where Descr=@ProductID order by Cdt 
	                                    end
                                    else if exists(select MAC from Product(nolock) where CUSTSN=@ProductID)
                                        begin
                                            Select a.Descr as Sno,a.AttrNewValue as Remark,a.Cdt  
                                            from ProductAttrLog a(nolock),Product b(nolock)  
                                            where b.CUSTSN =@ProductID  
                                            and a.Descr = b.MAC
                                            and a.AttrName='TEST_IPADDRESS'  
                                            order by a.Cdt  
                                        end
                                end
                            else 
                                begin
                                    select ''  as Sno,'' as  Remark  ,'' as Cdt
                                end
                            ";

                SqlParameter paraName = new SqlParameter("@ProductID", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = ID;
                DataTable tb = SQLHelper.ExecuteDataFill(Connection,
                                                         System.Data.CommandType.Text,
                                                         strSQL,
                                                         paraName);

                return tb;
            }

            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodname);
            }
        }
    }
}
