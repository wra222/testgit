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
    public class SA_CpuQuery : MarshalByRefObject, ISA_CpuQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        DataTable ISA_CpuQuery.GetCPU(string Connection, string ID)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string strSQL = @"
                    IF EXISTS(SELECT ProductID FROM Product_Part (NOLOCK) WHERE ProductID=@Product )
                               BEGIN
                                select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,d.CUSTSN,a.Status,a.Editor,a.Cdt  
                                from Material a 
                                left join  MaterialLot b on a.LotNo=b.LotNo
						        left join Product_Part c on a.MaterialCT=c.PartSn
						        left join Product d on c.ProductID=d.ProductID
						        left join MaterialBox e on a.LotNo=e.LotNo
						        where  c.PartType like 'CPU%' and d.ProductID=@Product
					           END
					
                              ELSE IF EXISTS(select top 1 ProductID  from Product  where CUSTSN=@Product ) 
                                BEGIN
                                    select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,d.CUSTSN,a.Status,a.Editor,a.Cdt  
                                        from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join Product_Part c on a.MaterialCT=c.PartSn
						                left join Product d on c.ProductID=d.ProductID
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where  c.PartType like 'CPU%' and d.CUSTSN=@Product
                                END	
                            ELSE IF EXISTS(select top 1 * from Material where MaterialCT=@Product)
                               BEGIN
                                    select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,a.Status,a.Editor,a.Cdt  
                                        into #5 from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where  a.MaterialCT=@Product
                                    select distinct a.FeedType,a.Stage,a.LotNo,a.SpecNo,a.MaterialCT,ISNULL(c.CUSTSN,'') as CUSTSN,a.Status,a.Editor,a.Cdt 
						                 from #5 a left join Product_Part b on a.MaterialCT=b.PartSn
						                           left join Product c on b.ProductID=c.ProductID
                                END	
                           ELSE IF EXISTS(select top 1 * from Material where LotNo=@Product)
                                BEGIN
                                   
                                        select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,a.Status,a.Editor,a.Cdt  
                                        into #1 from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where  a.LotNo=@Product
						                
	                                    select distinct a.FeedType,a.Stage,a.LotNo,a.SpecNo,a.MaterialCT,ISNULL(c.CUSTSN,'') as CUSTSN,a.Status,a.Editor,a.Cdt 
						                 from #1 a left join Product_Part b on a.MaterialCT=b.PartSn
						                           left join Product c on b.ProductID=c.ProductID
                                END	   
                           ELSE IF EXISTS(select top 1 * from MaterialBox where SpecNo=@Product)
                                BEGIN
                                    select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,a.Status,a.Editor,a.Cdt  
                                    into #2 from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where   e.SpecNo=@Product
						                 select distinct a.FeedType,a.Stage,a.LotNo,a.SpecNo,a.MaterialCT,ISNULL(c.CUSTSN,'') as CUSTSN,a.Status,a.Editor,a.Cdt 
						                 from #2 a left join Product_Part b on a.MaterialCT=b.PartSn
						                           left join Product c on b.ProductID=c.ProductID
                                END	  
                           ELSE IF (@Product='FA' OR @Product='SA' )
                                BEGIN
                                select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,a.Status,a.Editor,a.Cdt  
                                    into #3 from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where   a.Stage=@Product
						                 select distinct a.FeedType,a.Stage,a.LotNo,a.SpecNo,a.MaterialCT,ISNULL(c.CUSTSN,'') as CUSTSN,a.Status,a.Editor,a.Cdt 
						                 from #3 a left join Product_Part b on a.MaterialCT=b.PartSn
						                           left join Product c on b.ProductID=c.ProductID
                                    
                                END	
                           ELSE
                                BEGIN
                                select distinct e.FeedType,a.Stage,a.LotNo,b.SpecNo,a.MaterialCT,a.Status,a.Editor,a.Cdt  
                                    into #4 from Material a 
                                        left join  MaterialLot b on a.LotNo=b.LotNo
						                left join MaterialBox e on a.LotNo=e.LotNo
						                where   e.FeedType=@Product
						                 select distinct a.FeedType,a.Stage,a.LotNo,a.SpecNo,a.MaterialCT,ISNULL(c.CUSTSN,'') as CUSTSN,a.Status,a.Editor,a.Cdt 
						                 from #4 a left join Product_Part b on a.MaterialCT=b.PartSn
						                           left join Product c on b.ProductID=c.ProductID
                                   
                                END";
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

          
    }
}
