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


namespace IMES.Query.Implementation
{
    public class SA_SABANDIT : MarshalByRefObject, ISA_SABANDIT
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
             string Family)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //string colStr = "SELECT ";
                //string groupStr = " GROUP BY ";

                //                string SQLText = @"SELECT c.Family,b.Model,a.Line,a.Station+d.Descr as Station,
                //                                   COUNT(*) AS InputQty ,  
                //                                   SUM( CASE WHEN a.Status=0 THEN 1 ELSE 0 END ) AS DefectQty                          
                //                                   FROM ProductLog a (NOLOCK)
                //                                   INNER JOIN Product b (NOLOCK) on a.ProductID=b.ProductID
                //                                   INNER JOIN Model c (NOLOCK) on b.Model=c.Model
                //                                   INNER JOIN Station d (NOLOCK) ON a.Station=d.Station 
                //                                   WHERE 1=1 AND a.Cdt BETWEEN @FromDate AND @ToDate
                //                                   ";

                string SQLText = @"create table #PCBTESTFamily (PCBNo nvarchar(20),ID INT)
                                   create table #PCBTESTMODE (Mode varchar(10),PCBNo nvarchar(20))
                                   create table #PCBTESTQty (Mode varchar(10),Qty int)
                                   insert #PCBTESTFamily 
                                   select PCBNo,MAX(ID)  from PCBTestLog where  Station='15'AND 
                                   LEFT(PCBNo,2) in (SELECT SUBSTRING(Remark,CHARINDEX('MB=',Remark)+3,2) FROM Part WHERE Descr=@Descr and BomNodeType='MB' and PartNo like '131%') and Remark<>''
                                   GROUP BY PCBNo
                                   INSERT #PCBTESTMODE
                                   select 'BS',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and a.ID=b.ID  and  b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND b.Remark LIKE'%V:%_BS%'
                                   INSERT #PCBTESTMODE
                                   select 'B/S',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND b.Remark LIKE'%V:%_B/S%'
                                   INSERT #PCBTESTMODE
                                   select 'MB',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND  substring(b.Remark,charindex('~BIOS',b.Remark)-2,2)<>'BS' AND substring(b.Remark,charindex('~BIOS',b.Remark)-3,3)<>'B/S' and b.Type<>'M/B'
                                   INSERT #PCBTESTMODE
                                   select 'M/B',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND  substring(b.Remark,charindex('~BIOS',b.Remark)-2,2)<>'BS' AND substring(b.Remark,charindex('~BIOS',b.Remark)-3,3)<>'B/S'and b.Type<>'MB'
                                   insert #PCBTESTQty
                                   select Mode,COUNT(distinct PCBNo) from #PCBTESTMODE group by Mode
                                   select * from #PCBTESTQty
                                   drop table #PCBTESTFamily
                                   drop table #PCBTESTMODE
                                   drop table #PCBTESTQty 
                                   ";
                //if (Station != "")
                //{
                //    SQLText += "AND ba.Station = '" + Station + "'";
                //}

                //if (Family != "")
                //{
                //    SQLText += "AND c.Family = '" + Family + "'";
                //    colStr = colStr + " c.Family,";
                //    groupStr = groupStr + " c.Family,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Family,";
                //}


                //if (Model != "")
                //{
                //    SQLText += "AND b.Model = '" + Model + "'";
                //    colStr = colStr + "  b.Model,";
                //    groupStr = groupStr + " b.Model,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Model,";
                //}


                //if (lstPdLine.Count != 0)
                //{
                //    //SQLText += "AND a.Line = '" + Line + "'";
                //    SQLText += string.Format("AND ba.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                //    colStr = colStr + "  ba.Line,";
                //    groupStr = groupStr + " ba.Line,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Line,";
                //}

                //if (ModelCategory != "")
                //{
                //    SQLText += " AND dbo.CheckModelCategory(b.Model,'" + ModelCategory + "')='Y' ";
                //}

                //groupStr = groupStr + " ba.Station,ba.Descr";
                //SQLText = colStr + SQLText + groupStr;

                ////SQLText += "GROUP BY c.Family,b.Model,a.Station,a.Line,d.Descr";

                //SQLText += "  ORDER BY ba.Station ";

                //SQLText = String.Format("WITH #base AS (SELECT a.Station, b.Line,a.Descr FROM Station a,Line b WHERE Station IN ('{0}')) " , string.Join("','",lstStation.ToArray())).ToString() + SQLText;
                //SQLText += "  ORDER BY b.Model "; 
                //string SQL = string.Format(SQLText, FromDate.ToString("yyyy/MM/dd HH:mm:ss"), ToDate.ToString("yyyy/MM/dd HH:mm:ss"));


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                 SQLHelper.CreateSqlParameter("@Descr", 30, Family));

                //SQLHelper.CreateSqlParameter("@Descr", Family));
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
        public DataTable GetQueryResult1(string Connection, DateTime FromDate, DateTime ToDate,
          string Family)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //string colStr = "SELECT ";
                //string groupStr = " GROUP BY ";

                //                string SQLText = @"SELECT c.Family,b.Model,a.Line,a.Station+d.Descr as Station,
                //                                   COUNT(*) AS InputQty ,  
                //                                   SUM( CASE WHEN a.Status=0 THEN 1 ELSE 0 END ) AS DefectQty                          
                //                                   FROM ProductLog a (NOLOCK)
                //                                   INNER JOIN Product b (NOLOCK) on a.ProductID=b.ProductID
                //                                   INNER JOIN Model c (NOLOCK) on b.Model=c.Model
                //                                   INNER JOIN Station d (NOLOCK) ON a.Station=d.Station 
                //                                   WHERE 1=1 AND a.Cdt BETWEEN @FromDate AND @ToDate
                //                                   ";

                string SQLText = @"   create table #ProductFamily(ID int,ProductID varchar(20))
                                  create table #PCBTESTFamily (PCBNo nvarchar(20),ID INT)
                                  create table #PCBTESTMODE (Mode varchar(10),PCBNo nvarchar(20))
                                  create table #PCBTESTQty (Mode varchar(10),Qty int)
                                  insert  #ProductFamily
                                 select MAX(b.ID),b.ProductID from Product a,ProductLog b where a.Model in (  
                                   select distinct Model from Model where Family=@Descr) and a.ProductID=b.ProductID
                                   and b.Station='40' group by b.ProductID
                                   select a.PCBID into #11 from Product a,#ProductFamily b,ProductLog c where a.ProductID=b.ProductID and b.ProductID=c.ProductID
                                   and c.Station='40' and b.ID=c.ID and c.Cdt between @FromDate and @ToDate 
                                   INSERT #PCBTESTFamily
                                   select PCBNo,MAX(ID) from PCBTestLog where PCBNo in (select PCBID FROM #11) and Remark<>'' GROUP BY PCBNo
                                   insert #PCBTESTMODE
                                   select 'BS',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID and a.Remark LIKE'%V:%_BS%' and a.Station='15'
                                    insert #PCBTESTMODE
                                   select 'B/S',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID and a.Remark LIKE'%V:%_B/S%' and a.Station='15'
                                     insert #PCBTESTMODE
                                   select 'MB',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID  and a.Station='15'
                                   AND a.Type='MB' AND substring(a.Remark,charindex('~BIOS',a.Remark)-2,2)<>'BS' AND substring(a.Remark,charindex('~BIOS',a.Remark)-3,3)<>'B/S' and a.Type<>'M/B'
                                     insert #PCBTESTMODE
                                   select 'M/B',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID  and a.Station='15'
                                   AND a.Type='M/B' AND substring(a.Remark,charindex('~BIOS',a.Remark)-2,2)<>'BS' AND substring(a.Remark,charindex('~BIOS',a.Remark)-3,3)<>'B/S' and a.Type<>'MB'
                                   insert #PCBTESTQty
                                   select Mode,COUNT(distinct PCBNo) from #PCBTESTMODE group by Mode
                                   select * from #PCBTESTQty
                                   drop table #PCBTESTFamily
                                   drop table #PCBTESTMODE
                                   drop table #PCBTESTQty
                                   drop table #ProductFamily
                                    drop table #11   
                                   ";
                //if (Station != "")
                //{
                //    SQLText += "AND ba.Station = '" + Station + "'";
                //}

                //if (Family != "")
                //{
                //    SQLText += "AND c.Family = '" + Family + "'";
                //    colStr = colStr + " c.Family,";
                //    groupStr = groupStr + " c.Family,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Family,";
                //}


                //if (Model != "")
                //{
                //    SQLText += "AND b.Model = '" + Model + "'";
                //    colStr = colStr + "  b.Model,";
                //    groupStr = groupStr + " b.Model,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Model,";
                //}


                //if (lstPdLine.Count != 0)
                //{
                //    //SQLText += "AND a.Line = '" + Line + "'";
                //    SQLText += string.Format("AND ba.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                //    colStr = colStr + "  ba.Line,";
                //    groupStr = groupStr + " ba.Line,";
                //}
                //else
                //{
                //    colStr = colStr + " 'ALL' as Line,";
                //}

                //if (ModelCategory != "")
                //{
                //    SQLText += " AND dbo.CheckModelCategory(b.Model,'" + ModelCategory + "')='Y' ";
                //}

                //groupStr = groupStr + " ba.Station,ba.Descr";
                //SQLText = colStr + SQLText + groupStr;

                ////SQLText += "GROUP BY c.Family,b.Model,a.Station,a.Line,d.Descr";

                //SQLText += "  ORDER BY ba.Station ";

                //SQLText = String.Format("WITH #base AS (SELECT a.Station, b.Line,a.Descr FROM Station a,Line b WHERE Station IN ('{0}')) " , string.Join("','",lstStation.ToArray())).ToString() + SQLText;
                //SQLText += "  ORDER BY b.Model "; 
                //string SQL = string.Format(SQLText, FromDate.ToString("yyyy/MM/dd HH:mm:ss"), ToDate.ToString("yyyy/MM/dd HH:mm:ss"));


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                 SQLHelper.CreateSqlParameter("@Descr", 30, Family));

                //SQLHelper.CreateSqlParameter("@Descr", Family));
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
        public DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate,
                 string Family, string mode)
        {
            //string selectSQL = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"  create table #PCBTESTFamily (PCBNo nvarchar(20),ID int)
                              create table #PCBTESTMODE (Mode varchar(10),PCBNo nvarchar(20))
                              insert #PCBTESTFamily 
                              select PCBNo,MAX(ID) AS Cdt from PCBTestLog where  Station='15'AND 
                              LEFT(PCBNo,2) in (SELECT SUBSTRING(Remark,CHARINDEX('MB=',Remark)+3,2) FROM Part WHERE Descr=@Descr and BomNodeType='MB' and PartNo like '131%') and Remark<>''
                              GROUP BY PCBNo
                              INSERT #PCBTESTMODE
                              select 'BS',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo  and  b.Station='15' and a.ID=b.ID and b.Cdt BETWEEN @FromDate AND @ToDate AND b.Remark LIKE'%V:%_BS%'
                              INSERT #PCBTESTMODE
                              select 'B/S',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and  a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND b.Remark LIKE'%V:%_B/S%'
                              INSERT #PCBTESTMODE
                              select 'MB',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and  a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND  substring(b.Remark,charindex('~BIOS',b.Remark)-2,2)<>'BS' AND substring(b.Remark,charindex('~BIOS',b.Remark)-3,3)<>'B/S' and b.Type<>'M/B'
                              INSERT #PCBTESTMODE
                              select 'M/B',a.PCBNo from #PCBTESTFamily a,PCBTestLog b where a.PCBNo=b.PCBNo and a.ID=b.ID and b.Station='15' and b.Cdt BETWEEN @FromDate AND @ToDate AND  substring(b.Remark,charindex('~BIOS',b.Remark)-2,2)<>'BS' AND substring(b.Remark,charindex('~BIOS',b.Remark)-3,3)<>'B/S'and b.Type<>'MB' 
                              SELECT distinct b.PCBNo, b.Type, b.Line, b.FixtureID, b.Station, b.Status, b.JoinID, b.Editor, b.ErrorCode, b.Descr, b.Cdt, b.Remark
                              FROM #PCBTESTMODE a ,PCBTestLog b ,#PCBTESTFamily c where  a.Mode=@mode and a.PCBNo=b.PCBNo and c.ID=b.ID and a.PCBNo=c.PCBNo   
                              drop table #PCBTESTFamily
                              drop table #PCBTESTMODE
                               ");

            //if (Model.Count > 0)
            //{
            //    sb.AppendFormat("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' AND InfoValue IN ('{0}') ", string.Join("','", Model.ToArray()));
            //}
            //else
            //{
            //    sb.AppendLine("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' ");
            //}

            //sb.AppendLine("INNER JOIN PCB b (NOLOCK) ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
            //if (Family.Length > 0)
            //{
            //    sb.AppendLine(string.Format("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel AND UPPER(c.Descr) = '{0}' ", Family));
            //}
            //else
            //{
            //    sb.AppendLine("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel ");
            //}

            //sb.AppendLine("INNER JOIN Station d ON a.Station=d.Station ");

            //sb.AppendLine("Left Join PCBRepair r (NOLOCK) On r.LogID=a.ID ");
            //sb.AppendLine("LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r_info.ID=( select MAX(ID) from PCBRepair_DefectInfo where r.ID=PCARepairID ) ");

            //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
            //if (PdLine.Count > 0)
            //{
            //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
            //}

            //if (Station.Count > 0)
            //{
            //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

            //}

            //sb.AppendLine("order by c.Descr, a.PCBNo, a.Cdt ");

            return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString(),
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                 SQLHelper.CreateSqlParameter("@Descr", 30, Family),
                                                 SQLHelper.CreateSqlParameter("@mode", 30, mode));


        }
        public DataTable GetSelectDetail1(string Connection, DateTime FromDate, DateTime ToDate,
                 string Family, string mode)
        {
            //string selectSQL = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"  create table #ProductFamily(ID int,ProductID varchar(20))
                                  create table #PCBTESTFamily (PCBNo nvarchar(20),ID INT)
                                  create table #PCBTESTMODE (Mode varchar(10),PCBNo nvarchar(20))
                                  insert  #ProductFamily
                                 select MAX(b.ID),b.ProductID from Product a,ProductLog b where a.Model in (  
                                   select distinct Model from Model where Family=@Descr) and a.ProductID=b.ProductID
                                   and b.Station='40' group by b.ProductID
                                   select a.PCBID into #22 from Product a,#ProductFamily b,ProductLog c where a.ProductID=b.ProductID and b.ProductID=c.ProductID
                                   and c.Station='40' and b.ID=c.ID and c.Cdt between @FromDate and @ToDate 
                                   INSERT #PCBTESTFamily
                                   select PCBNo,MAX(ID) from PCBTestLog where PCBNo in (select PCBID FROM #22) and Remark<>'' GROUP BY PCBNo
                                   insert #PCBTESTMODE
                                   select 'BS',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID and a.Remark LIKE'%V:%_BS%' and a.Station='15'
                                    insert #PCBTESTMODE
                                   select 'B/S',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID and a.Remark LIKE'%V:%_B/S%' and a.Station='15'
                                     insert #PCBTESTMODE
                                   select 'MB',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID  and a.Station='15'
                                   AND a.Type='MB' AND substring(a.Remark,charindex('~BIOS',a.Remark)-2,2)<>'BS' AND substring(a.Remark,charindex('~BIOS',a.Remark)-3,3)<>'B/S' and a.Type<>'M/B'
                                     insert #PCBTESTMODE
                                   select 'M/B',a.PCBNo from PCBTestLog a,#PCBTESTFamily b where a.PCBNo=b.PCBNo and a.ID=b.ID  and a.Station='15'
                                   AND a.Type='M/B' AND substring(a.Remark,charindex('~BIOS',a.Remark)-2,2)<>'BS' AND substring(a.Remark,charindex('~BIOS',a.Remark)-3,3)<>'B/S' and a.Type<>'MB'
                                  SELECT distinct b.PCBNo, b.Type, b.Line, b.FixtureID, b.Station, b.Status, b.JoinID, b.Editor, b.ErrorCode, b.Descr, b.Cdt, b.Remark
                                  FROM #PCBTESTMODE a ,PCBTestLog b ,#PCBTESTFamily c where  a.Mode=@mode and a.PCBNo=b.PCBNo and c.ID=b.ID and a.PCBNo=c.PCBNo  
                                   drop table #PCBTESTFamily
                                   drop table #PCBTESTMODE
                                   drop table #ProductFamily
                                   drop table #22 
                               ");

            //if (Model.Count > 0)
            //{
            //    sb.AppendFormat("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' AND InfoValue IN ('{0}') ", string.Join("','", Model.ToArray()));
            //}
            //else
            //{
            //    sb.AppendLine("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' ");
            //}

            //sb.AppendLine("INNER JOIN PCB b (NOLOCK) ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
            //if (Family.Length > 0)
            //{
            //    sb.AppendLine(string.Format("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel AND UPPER(c.Descr) = '{0}' ", Family));
            //}
            //else
            //{
            //    sb.AppendLine("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel ");
            //}

            //sb.AppendLine("INNER JOIN Station d ON a.Station=d.Station ");

            //sb.AppendLine("Left Join PCBRepair r (NOLOCK) On r.LogID=a.ID ");
            //sb.AppendLine("LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r_info.ID=( select MAX(ID) from PCBRepair_DefectInfo where r.ID=PCARepairID ) ");

            //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
            //if (PdLine.Count > 0)
            //{
            //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
            //}

            //if (Station.Count > 0)
            //{
            //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

            //}

            //sb.AppendLine("order by c.Descr, a.PCBNo, a.Cdt ");

            return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 sb.ToString(),
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                 SQLHelper.CreateSqlParameter("@Descr", 30, Family),
                                                 SQLHelper.CreateSqlParameter("@mode", 30, mode));


        }
    }
}
