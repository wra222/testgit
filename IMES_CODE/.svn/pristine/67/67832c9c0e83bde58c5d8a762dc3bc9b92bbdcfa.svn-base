using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using IMES.Infrastructure;
using System.Reflection;
 

namespace IMES.Query.Implementation
{
    public class SMT_TestDataReport :  MarshalByRefObject,ISMT_TestDataReport
    {
      public DataTable GetModel(string DBConnection,  IList<String> PdLine,  DateTime From, DateTime To)
     {
         DataTable Result = null;
         string selectSQL = "";
         string groupbySQL = "";

         groupbySQL += "GROUP by  b.Descr";

         string orderbySQL = "ORDER BY  b.Descr";
         StringBuilder sb = new StringBuilder();
         //sb.AppendLine("WITH [TEMP] AS (");
         sb.AppendLine(" select distinct b.Descr as Family from PCBLog a  inner join  Part b on a.PCBModel=b.PartNo ");


         //sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and  b.BomNodeType='MB'   ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }


         sb.AppendFormat("{0}  ", groupbySQL);
         sb.AppendFormat("{0}  ", orderbySQL); 
          Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));

         return Result;
     }
     public DataTable GetTestCount(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From,DateTime To)
     {
         DataTable Result = null;
         //string selectSQL = "";
         //string groupbySQL = "";
         
         //    groupbySQL += "GROUP by  a.PCBModel";

         //    string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder Model_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Model.Count.ToString()); i++)
         {
             Model_sb.Append(Model[i]);
             Model_sb.Append("~");
         }
         StringBuilder PdLine_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(PdLine.Count.ToString()); i++)
         {
             PdLine_sb.Append(PdLine[i]);
             PdLine_sb.Append("~");
         }
         StringBuilder Station_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Station.Count.ToString()); i++)
         {
             Station_sb.Append(Station[i]);
             Station_sb.Append("~");
         }
           StringBuilder sb = new StringBuilder();
           sb.AppendLine("exec SMT_TestDataQuery '" + Model_sb + "','" + PdLine_sb + "','" + Station_sb + "',@StartTime,@EndTime ,'TestCount',''");
           #region
           /*
         //sb.AppendLine("WITH [TEMP] AS (");
         //     sb.AppendLine(" select COUNT( distinct a.PCBNo)as Testcount,a.PCBModel from PCBLog a inner join Part b on a.PCBModel=b.PartNo   ");
 
         
         ////sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         // sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime   ");
         //if (PdLine.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         //}
         //if (Family.Count > 0)
         //{
         //    sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Family.ToArray()));
         //    sb.AppendLine("AND b.BomNodeType='MB'");
         //}
         //if (Station.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         //}
         //sb.AppendFormat("{0} ) ", groupbySQL);

         //sb.AppendLine(" select  a.PCBModel,c.Testcount,COUNT(distinct a.PCBNo) as Failcount from PCBLog a inner join Part b on   a.PCBModel=b.PartNo  inner join [TEMP] c on  c.PCBModel=a.PCBModel ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         //if (PdLine.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         //}
         //if (Family.Count > 0)
         //{
         //    sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Family.ToArray()));
         //    sb.AppendLine("AND b.BomNodeType='MB'");
         //}
         //if (Station.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         //}
         //groupbySQL += ",c.Testcount ";
         //sb.AppendFormat("{0} ", groupbySQL);
         */
           #endregion

           Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));

         return Result;
     }
     public DataTable Get_Defect_Rate(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To,string Type)
     {
         DataTable Result = null;
         //string selectSQL = "";
         //string groupbySQL = "";

         //groupbySQL += "GROUP by  a.PCBModel";
         StringBuilder Model_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Model.Count.ToString()); i++)
         {
             Model_sb.Append(Model[i]);
             Model_sb.Append("~");
         }
         StringBuilder PdLine_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(PdLine.Count.ToString()); i++)
         {
             PdLine_sb.Append(PdLine[i]);
             PdLine_sb.Append("~");
         }
         StringBuilder Station_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Station.Count.ToString()); i++)
         {
             Station_sb.Append(Station[i]);
             Station_sb.Append("~");
         }
         //string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("exec SMT_TestDataQuery '" + Model_sb + "','" + PdLine_sb + "','" + Station_sb + "',@StartTime,@EndTime ,'Defect_Rate','" + Type + "'");
         #region
         /*
         //sb.AppendLine("WITH [TEMP] AS (");
         //sb.AppendLine(" select COUNT(a.PCBNo)as Testcount,a.PCBModel into #temp from PCBLog a inner join Part b on a.PCBModel=b.PartNo   ");


         ////sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime ");
         //if (PdLine.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         //}
         //if (Model.Count > 0)
         //{
         //    sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Model.ToArray()));
         //    sb.AppendLine("AND b.BomNodeType='MB'");
         //}
         //if (Station.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         //}
         //sb.AppendFormat("{0}  ", groupbySQL);

         //sb.AppendLine(" select  a.PCBModel,COUNT(a.PCBNo) as FailCount,a.Line,a.Cdt into #temp_Fail from PCBLog a inner join Part b on   a.PCBModel=b.PartNo  inner join #temp c on  c.PCBModel=a.PCBModel ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         //if (PdLine.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         //}
         //if (Model.Count > 0)
         //{
         //    sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Model.ToArray()));
         //    sb.AppendLine("AND b.BomNodeType='MB'");
         //}
         //if (Station.Count > 0)
         //{
         //    sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         //}
         //groupbySQL += ",a.Line,a.Cdt";
         //sb.AppendFormat("{0} ", groupbySQL);
         //if (Type == "Day")
         //{
         //    sb.AppendLine("select distinct  round(COUNT(b.FailCount)/convert(float,a.Testcount)*100,2) as Rate ,b.PCBModel+Case when datepart(HH,b.Cdt) between '8' and '21' then rtrim(b.Line)+'白班'  else rtrim(b.Line)+'夜班' end as PCBModel  from #temp a,#temp_Fail b where a.PCBModel= b.PCBModel");
         //    sb.AppendLine("Group by b.FailCount,a.Testcount,b.PCBModel,b.Line,b.Cdt");
         //}
         //else if (Type == "Week")
         //{
         //    sb.AppendLine("select distinct  round(COUNT(b.FailCount)/convert(float,a.Testcount)*100,2) as Rate ,CONVERT(varchar(10),b.Cdt,111) as PCBModel  from #temp a,#temp_Fail b where a.PCBModel= b.PCBModel");
         //    sb.AppendLine("Group by b.FailCount,a.Testcount,b.Cdt");
         //}
         //else
         //{
         //    sb.AppendLine("select distinct  round(COUNT(b.FailCount)/convert(float,a.Testcount)*100,2) as Rate ,case DATEPART(MM,b.Cdt) when  '1' then 'Jan.' when '2' then 'Feb.' when '3' then 'Mar.' when '4' then 'Apr.' when '5' then 'May.' when '6' then 'Jun.' when '7' then 'Jul.' when '8' then 'Aug.' when '9' then 'Sep.' when '10' then 'Oct.' when '11' then'Nov.' else 'Dec.' end as PCBModel  from #temp a,#temp_Fail b where a.PCBModel= b.PCBModel");
         //    sb.AppendLine("Group by b.FailCount,a.Testcount,b.Cdt");
         //}
          */
    #endregion
         Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));

         
         return Result;
     }
     public DataTable Get_Defect_Top(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To)
     {
         DataTable Result = null;

         //string selectSQL = "";
         //string groupbySQL = "";

     
                  StringBuilder Model_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Model.Count.ToString()); i++)
         {
             Model_sb.Append(Model[i]);
             Model_sb.Append("~");
         }
         StringBuilder PdLine_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(PdLine.Count.ToString()); i++)
         {
             PdLine_sb.Append(PdLine[i]);
             PdLine_sb.Append("~");
         }
         StringBuilder Station_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Station.Count.ToString()); i++)
         {
             Station_sb.Append(Station[i]);
             Station_sb.Append("~");
         }
        // string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("exec SMT_TestDataQuery '" + Model_sb + "','" + PdLine_sb + "','" + Station_sb + "',@StartTime,@EndTime ,'Defect_Top',''");
#region
         //sb.AppendLine("exec SMT_TestDataQuery 'COMET 1.0~','AOIAA~','0B~0A~0C~0D~',@StartTime,@EndTime ,'Defect_Top',''");
        /*
         sb.AppendLine("WITH [TEMP] AS (");
         sb.AppendLine(" select PCBNo from PCBLog a inner join Part b on a.PCBModel=b.PartNo   ");


         //sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }
         if (Model.Count > 0)
         {
             sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Model.ToArray()));
             sb.AppendLine("AND b.BomNodeType='MB'");
         }
         if (Station.Count > 0)
         {
             sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         }
         sb.AppendFormat("{0} ) ", groupbySQL);

         sb.AppendLine(" select  TOP 5 a.Location,count(distinct b.PCBNo) as Count    from PCBRepair_DefectInfo a inner join PCBRepair  b on   b.ID=a.PCARepairID  inner join [TEMP] c on  c.PCBNo=b.PCBNo ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         sb.AppendLine("WHERE 1=1 ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND b.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }
 
         if (Station.Count > 0)
         {
             sb.AppendFormat("AND b.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         }
         groupbySQL += "GROUP by  a.Location";
         sb.AppendFormat("{0} ", groupbySQL);
         string orderbySQL = "ORDER BY  Count desc";
         sb.AppendFormat("{0} ", orderbySQL);
         */
#endregion
         Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));

         return Result;
     }

     public DataTable Get_Defect_Analysis(string DBConnection, IList<String> Model, IList<String> PdLine, IList<String> Station, DateTime From, DateTime To)
     {
         DataTable Result = null;
         #region
         /* string selectSQL = "";
         string groupbySQL = "";



         // string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder sb = new StringBuilder();
         //sb.AppendLine("WITH [TEMP] AS (");
         sb.AppendLine(" select PCBNo into #mb from PCBLog a inner join Part b on a.PCBModel=b.PartNo   ");


         //sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }
         if (Model.Count > 0)
         {
             sb.AppendFormat("AND b.PartType in ('{0}') ", string.Join("','", Model.ToArray()));
             sb.AppendLine("AND b.BomNodeType='MB'");
         }
         if (Station.Count > 0)
         {
             sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         }
         sb.AppendFormat("{0}  ", groupbySQL);
         sb.AppendLine(" select  count(1) as TotalCount into #Total   from PCBRepair_DefectInfo a inner join PCBRepair  b on   b.ID=a.PCARepairID  inner join #mb c on  c.PCBNo=b.PCBNo ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         sb.AppendLine("WHERE a.Cause<>'' ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND b.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }
 
         if (Station.Count > 0)
         {
             sb.AppendFormat("AND b.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         }

         sb.AppendLine(" select  TOP 5 a.Cause,count(1) as Count ,round(count(1) /convert(float,d.TotalCount)*100,2) as Rate   from PCBRepair_DefectInfo a inner join PCBRepair  b on   b.ID=a.PCARepairID  inner join #mb c on  c.PCBNo=b.PCBNo ,#Total d ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         sb.AppendLine("WHERE a.Cause<>'' ");
         if (PdLine.Count > 0)
         {
             sb.AppendFormat("AND b.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
         }

         if (Station.Count > 0)
         {
             sb.AppendFormat("AND b.Station in ('{0}') ", string.Join("','", Station.ToArray()));

         }
         groupbySQL += "GROUP by  a.Cause,d.TotalCount";
         sb.AppendFormat("{0} ", groupbySQL);
         string orderbySQL = "ORDER BY  Count desc";
         sb.AppendFormat("{0} ", orderbySQL); */
 #endregion
         StringBuilder Model_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Model.Count.ToString()); i++)
         {
             Model_sb.Append(Model[i]);
             Model_sb.Append("~");
         }
         StringBuilder PdLine_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(PdLine.Count.ToString()); i++)
         {
             PdLine_sb.Append(PdLine[i]);
             PdLine_sb.Append("~");
         }
         StringBuilder Station_sb = new StringBuilder();
         for (int i = 0; i < int.Parse(Station.Count.ToString()); i++)
         {
             Station_sb.Append(Station[i]);
             Station_sb.Append("~");
         }
        // string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("exec SMT_TestDataQuery '" + Model_sb + "','" + PdLine_sb + "','" + Station_sb + "',@StartTime,@EndTime ,'Defect_Analysis',''");

         Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));
         return Result;
     }
     public DataTable Get_Detial(string DBConnection, string Model, string PdLine, string Station, DateTime From, DateTime To, string Defect, string Tp)
     {
         DataTable Result = null;
         string groupbySQL = "";



         // string orderbySQL = "ORDER BY  a.PCBModel";
         StringBuilder sb = new StringBuilder();
         //sb.AppendLine("WITH [TEMP] AS (");
         sb.AppendLine(" select PCBNo into #mb from PCBLog a inner join Part b on a.PCBModel=b.PartNo   ");


         //sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
         sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime ");
         if (PdLine!="")
         {

             sb.AppendFormat("and a.Line='{0} '", PdLine);
         }
         if (Model != "")
         {
              
             sb.AppendFormat("and b.PartType='{0}' ", Model);
             sb.AppendLine("AND b.BomNodeType='MB'");
         }
         if (Station != "")
         {
              
             sb.AppendFormat("and a.Station ='{0}' ", Station);
         }
         sb.AppendFormat("{0}  ", groupbySQL);
         sb.AppendLine(" select   rtrim(PCBNo) as PCBNo,rtrim(PCBModelID) as PCBModelID,rtrim(Station) as Station,rtrim(DefectCode) as DefectCode,rtrim(Cause) as Cause,rtrim(Obligation) as Obligation ,rtrim(Component) as Component,rtrim(Site) as Site,rtrim(Location) as Location ,rtrim(MajorPart) as MajorPart,rtrim(Remark) as Remark from PCBRepair_DefectInfo a,PCBRepair b  ");
         //sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
         sb.AppendLine("WHERE a.PCARepairID=b.ID  and b.PCBNo in (select * from #mb) ");
         if (PdLine != "")
         {
             sb.AppendFormat("and b.Line='{0} '", PdLine);
         }

         if (Station != "")
         {
             sb.AppendFormat("and b.Station='{0}' ", Station);

         }
         if (Tp == "DefectCode")
         {
             if (Defect != "")
             {
                 sb.AppendFormat("and a.DefectCode='{0}' ", Defect);
             }
         }
         else
         {
             if (Defect != "")
             {
                 sb.AppendFormat("and a.Location='{0}' ", Defect);
             }
         }

         //groupbySQL += "GROUP by  a.Cause,d.TotalCount";
         //sb.AppendFormat("{0} ", groupbySQL);
         string orderbySQL = "ORDER BY  b.PCBNo ";
         sb.AppendFormat("{0} ", orderbySQL);
         Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", From), new SqlParameter("@EndTime", To));
         return Result;
     }
    }
}
