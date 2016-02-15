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
    public class PAK_Common : MarshalByRefObject, IPAK_Common
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetDtPallet(string Connection,string palletNo, string status)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"select a.DeliveryNo,PalletNo,b.InfoValue as Forwarder,
                                                  ISNULL(c.InfoValue,'') as HAWB,ISNULL(d.WC,'') as [Status],d.Cdt as Cdt from Delivery_Pallet a WITH(NOLOCK)
                                                  left join 
                                                  (
                                                   select InfoValue,DeliveryNo from DeliveryInfo WITH(NOLOCK) where InfoType='Carrier'
                                                  ) b
                                                  on a.DeliveryNo=b.DeliveryNo
                                                 left join 
                                                  (
                                                   select InfoValue,DeliveryNo from DeliveryInfo WITH(NOLOCK)  where InfoType='BOL'
                                                  ) c
                                                  on a.DeliveryNo=c.DeliveryNo
                                                 left join
                                                 (
                                                	 
	                                                 select WC,PLT,Cdt from 
	                                                (SELECT ROW_NUMBER()over(partition by PLT order by Cdt desc ) as rum ,WC,PLT,Cdt FROM WH_PLTLog  WITH(NOLOCK) 
                                                	  
	                                                ) AS t
	                                                where t.rum=1
                                                 ) d
                                                  on a.PalletNo=d.PLT 
                                                 where
                                                 PalletNo in(select value from fn_split('{0}',',')) 
                                                 and ISNULL(d.WC,'') in({1})  order by PalletNo";
                SQLText = string.Format(SQLText,palletNo,status);
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText);

            
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
        public DataTable GetDtPallet(string Connection,string palletNo,DateTime fromDate, DateTime toDate,string status)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"sp_Query_PAK_QueryDtPallet ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText, 
                                                                   SQLHelper.CreateSqlParameter("@PalletNo", 512,palletNo),
                                                                  SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                                  SQLHelper.CreateSqlParameter("@toDate", toDate),
                                                                  SQLHelper.CreateSqlParameter("@Status", 512, status));

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
        public DataTable GetPLTLog(string Connection,string palletNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"select PLT,WC,Cdt from WH_PLTLog  WITH(NOLOCK) where PLT=@palletNo order by Cdt desc ";
                SQLText = string.Format(SQLText, palletNo);
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText, SQLHelper.CreateSqlParameter("@palletNo", 32, palletNo));

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
        public DataTable GetSnByPalletNo(string Connection, string palletNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"select ProductID,CUSTSN,PCBID,MAC,CVSN,PalletNo,CartonSN,UnitWeight, DeliveryNo from Product a  WITH(NOLOCK)  where PalletNo in
                                            (select value from fn_split('{0}',','))     order by PalletNo";
                SQLText = string.Format(SQLText, palletNo);
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);

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
        public DataTable GetDnDataByPalletNo(string Connection, string palletNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"select b.* from Delivery_Pallet a  WITH(NOLOCK) ,Delivery b WITH(NOLOCK) 
                                             where a.DeliveryNo=b.DeliveryNo and PalletNo=@palletNo";
                SQLText = string.Format(SQLText, palletNo);
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@palletNo", 32, palletNo));

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
        public DataTable GetPalletTypeBySummary(string Connection, string dnNo, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"select PalletType,COUNT(*) as Count  from 
                                                    (
                                                    select  distinct dbo.GetPalletType(DeliveryNo,PalletNo)
                                                                 as PalletType,PalletNo from Delivery_Pallet  WITH(NOLOCK) 
                                                                 where DeliveryNo in
                                                                 ({0})
                                                                 and PalletNo not like 'NA%' 
                                                     )  a           
                                                    group by a.PalletType  
                                                     having PalletType<>''  ";
                if (dnNo == "")
                {
                    SQLText = string.Format(SQLText, "select DeliveryNo from Delivery  WITH(NOLOCK)  where ShipDate=@shipDate");
                }
                else
                {
                    SQLText = string.Format(SQLText, "select DeliveryNo from Delivery WITH(NOLOCK)  where DeliveryNo=@dnNo");

                }
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText, SQLHelper.CreateSqlParameter("@dnNo", 32, dnNo),
                                                                  SQLHelper.CreateSqlParameter("@shipDate", shipDate)
                                                                  );

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

        public DataTable GetPalletTypeByDetail(string Connection, string dnNo, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @" select distinct a.PalletNo, a.PalletType,a.FWD,d.qty   from 
                                                    (
                                                       select   b.DeliveryNo, dbo.GetPalletType(b.DeliveryNo,PalletNo) as PalletType,PalletNo,
                                                                c.InfoValue as FWD
                                                                from Delivery_Pallet b  WITH(NOLOCK) 
                                                                 left join
                                                                 (select DeliveryNo,InfoValue from DeliveryInfo  WITH(NOLOCK)  where InfoType='Carrier') c
                                                                 on b.DeliveryNo=c.DeliveryNo
                                                                 where b.DeliveryNo in
                                                                 ({0})
                                                                 and PalletNo not like 'NA%' 
                                                            
                                                     )  a           
                                                     left join 
                                                     (
                                                      select PalletNo ,SUM(DeliveryQty) qty from Delivery_Pallet  WITH(NOLOCK) 
                                                      group by PalletNo
                                                     ) d
                                                     on a.PalletNo=d.PalletNo
                                                     where a.PalletType<>''    ";
                if (dnNo == "")
                {
                    SQLText = string.Format(SQLText, "select DeliveryNo from Delivery WITH(NOLOCK)  where ShipDate=@shipDate");
                }
                else
                {
                    SQLText = string.Format(SQLText, "select DeliveryNo from Delivery WITH(NOLOCK) where DeliveryNo=@dnNo");
                
                }
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText, SQLHelper.CreateSqlParameter("@dnNo", 32, dnNo),
                                                                  SQLHelper.CreateSqlParameter("@shipDate", shipDate)
                                                                  );

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

     
        public DataTable GetFamilyInfo(string Connection, string FamilyName,string ModelName,string InfoName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                if(string.IsNullOrEmpty(ModelName))
                {
                    SQLText = @"select Value from FamilyInfo WITH(NOLOCK)  where Family=@FamilyName and Name=@InfoName";
                     return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText,
                                                                  SQLHelper.CreateSqlParameter("@FamilyName", 64, FamilyName),
                                                                  SQLHelper.CreateSqlParameter("@InfoName", 64, InfoName));
                }
                else
                {
                    SQLText = @"select a.Value from FamilyInfo a WITH(NOLOCK) ,Model b WITH(NOLOCK) 
                                            where a.Family=b.Family and a.Name=@InfoName and b.Model=@ModelName";
                    return SQLHelper.ExecuteDataFill(Connection,
                                                                 System.Data.CommandType.Text,
                                                                 SQLText,
                                                                 SQLHelper.CreateSqlParameter("@InfoName", 64, InfoName),
                                                                 SQLHelper.CreateSqlParameter("@ModelName", 64, ModelName));
                
                }
                
           
                                                                  
                                                             
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
        public DataTable QueryIndiaMPRLabel(string Connection, string sn)
        {
            string SQLText = "";
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                if (sn.Length == 10)// CUSTSN
                {
                       SQLText = @"select top 1 INDIA_PRICE from HPEDI.dbo.PAK_PAKComn
                                                 where  InternalID=(select DeliveryNo from Product where CUSTSN=@sn)
                                                       and  SHIP_TO_COUNTRY_NAME ='India' 
                                                       and  INDIA_PRICE_ID<>'' ";
                }
                else
                {
                    SQLText = @"select top 1 INDIA_PRICE from HPEDI.dbo.PAK_PAKComn
                                                 where
                                                        InternalID=@sn
                                                       and  SHIP_TO_COUNTRY_NAME ='India' 
                                                       and  INDIA_PRICE_ID<>''  ";
                
                }
              
                                                     
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText, SQLHelper.CreateSqlParameter("@sn", 32, sn));

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
        public DataTable CheckWeightStation(string Connection, string sn)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"sp_Query_PAK_Docking_CheckWeight";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                   SQLHelper.CreateSqlParameter("@sn", 16, sn)
                                                                  );

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
        public int[] GetDNShipQty(string Connection, DateTime ShipDate, string DBName, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = " and Model not like 'PC%'";


                if (DBName == "HPIMES")
                {
                    subSql = " and Model like 'PC%' ";

                }
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting  WITH(NOLOCK)  where Name='PAKStation'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) where ShipDate=@ShipDate  {0} 
                                                 union ALL 
                                              
                                              select COUNT(*) as Qty from view_wip_station_NoLog
                                               where ShipDate=@ShipDate and Station in
                                               (select value from dbo.fn_split (@s,','))   {0}";

                SQLText = string.Format(SQLText, subSql);
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Model", 1024, Model));

                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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
       
        public DataTable GetProductProgress(string Connection, DateTime fromDate, DateTime toDate,string prdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"sp_Query_PAK_Docking_PrdProgress ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                   SQLHelper.CreateSqlParameter("@fromDate", fromDate),
                                                                   SQLHelper.CreateSqlParameter("@toDate", toDate),
                                                                   SQLHelper.CreateSqlParameter("@prdType",16,prdType)
                                                                  );

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
        public DataTable GetCoaStatusLine(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQLText = @"select distinct Status as Value,'Status' as Type from COAStatus  WITH(NOLOCK)  where Status <>''
                                                union ALL
                                                select distinct Line as Value,'Line'  as Type from COAStatus  WITH(NOLOCK)  where LEN(Line)=1  ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText);

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
        public DataTable ReadinessReport(string Connection, string dn, DateTime shipDate, string itemType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSQL = "";
                if (itemType == "DN")
                { subSQL = @"select value from dbo.fn_split(@DN,',') "; }
                else
                {
                    subSQL = @"select DeliveryNo from Delivery  WITH(NOLOCK) Where ShipDate=@shipDate
                                     and  DeliveryNo in  (
                                   Select DeliveryNo from  DeliveryInfo WITH(NOLOCK)  where InfoType='BOL'
                                    and InfoValue in(select value from dbo.fn_split(@DN,','))
                                    ) ";
                }
                string SQLText = "";

                SQLText = @"Select	CONVERT(VARCHAR ,GETDATE(), 1) as DT,
		                                        datepart(ww,getdate()) as WK ,'Inventec' as ODM,
		                                        a.DeliveryNo,
		                                        b.InfoValue as  ShipWay,
		                                        CASE b.InfoValue
		                                        When 'T001 ' THEN 'AIR'
		                                        When 'T002 ' THEN 'OCEAN'
		                                        When 'T003 ' THEN 'TRUCK'
		                                        ELSE 'TRUCK'
		                                         END  as ShipModel,
		                                        CASE
		                                        When  c.InfoValue in ('SNL','SCL') THEN 'LA'
		                                        When  c.InfoValue in ('SNU','SCU') THEN 'NA'
		                                        When  c.InfoValue in ('SCE','SNE') THEN 'EMEA'
		                                        When  c.InfoValue='SCN' THEN 'APJ'
		                                        When  c.InfoValue='SAF' THEN 'APJ' 
		                                        END  as RegId,
		                                        d.InfoValue as Carrier,
		                                        space(30) as HWB,
												space(30) as GoodTime,
												space(30) as TimeofReadiness,
												space (2) as GoodHour
                                        into #1		
                                        from	Delivery a   WITH(NOLOCK) 
		                                        left join (select InfoValue,DeliveryNo from DeliveryInfo  WITH(NOLOCK) where InfoType='ShipWay')b
		                                        on a.DeliveryNo=b.DeliveryNo
		                                        left join (select InfoValue,DeliveryNo from DeliveryInfo WITH(NOLOCK)  where InfoType='RegId')c
		                                        on a.DeliveryNo=c.DeliveryNo
		                                        left join (select InfoValue,DeliveryNo from DeliveryInfo  WITH(NOLOCK) where InfoType='Carrier')d
		                                        on a.DeliveryNo=d.DeliveryNo
                                        where  a.DeliveryNo in( {0} )
                                                                              
                                        update	a set a.HWB=b.InfoValue  
                                        from	#1 a,DeliveryInfo b
                                        where	a.DeliveryNo=b.DeliveryNo
	                                        and b.InfoType='BOL' and a.RegId<>'EMEA'

                                        update	a set a.HWB=b.InfoValue 
                                        from	#1 a,DeliveryInfo b
                                        where	a.DeliveryNo=b.DeliveryNo
	                                        and b.InfoType='EmeaCarrier' and a.RegId='EMEA'

                                        select distinct a.DeliveryNo,a.PalletNo,isnull(b.Cdt,'') as TT into #11 
	                                    from (select a.* from Delivery_Pallet a,#1 b where a.DeliveryNo=b.DeliveryNo)a
	                                    left join (select PLT,WC ,max(Cdt) as Cdt from WH_PLTLog where WC='IN' group by PLT,WC) b
	                                    on a.PalletNo=b.PLT
                                    	
	                                    update a set a.TimeofReadiness=CONVERT(char(30),b.tt,121),
				                                     a.GoodHour= DATEPART (HH,b.tt)				
	                                    from #1 a,
	                                         (select distinct DeliveryNo,max(TT)  as tt  from #11 where TT<>''
                                        group by DeliveryNo) b
                                        where a.DeliveryNo=b.DeliveryNo
                                              and b.tt<>''
                                        
                                        update a set a.GoodTime='Not Ready'
	                                    from #1 a,
		                                    (select distinct DeliveryNo,max(TT)  as tt  from #11 where TT=''
		                                    group by DeliveryNo) b
	                                    where a.DeliveryNo=b.DeliveryNo
	                                       and b.tt=''
                                    	
	                                    update a  
	                                     set a.TimeofReadiness='',
				                                     a.GoodHour='',
				                                     a.GoodTime='Not Ready' 		
	                                    from #1 a,
	                                        #1 b
	                                    where a.HWB=b.HWB
	                                          and b.GoodTime='Not Ready' 	
                                        		
                                        select a.HWB,SUM(b.Qty)as Qty,MAX(a.TimeofReadiness) as Udt
										into #2
										from #1 a,Delivery b (nolock)
										where a.DeliveryNo=b.DeliveryNo 
										group by a.HWB

										select distinct a.DT, a.WK, a.ODM, a.ShipWay, a.ShipModel, a.RegId,
												a.Carrier, a.HWB,b.Qty, 
												case when a.GoodTime<>'' then a.GoodTime
												when b.Udt<CONVERT (char(10),GETDATE(),121)+' 12:00:00'
												then 'Before 12:00'
												else 'After 12:00' end as GoodTime,
												max(a.TimeofReadiness) as TimeofReadiness, min(a.GoodHour) as GoodHour
										from	#1  a, #2 b
										where	a.HWB=b.HWB
										Group by a.DT, a.WK, a.ODM, a.ShipWay, a.ShipModel, a.RegId,
												a.Carrier, a.HWB,b.Qty, a.GoodTime,b.Udt";

                SQLText = string.Format(SQLText, subSQL);
                if (itemType == "DN")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                        System.Data.CommandType.Text,
                                                        SQLText,
                                                        SQLHelper.CreateSqlParameter("@DN", int.MaxValue, dn));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                          System.Data.CommandType.Text,
                                                          SQLText,
                                                          SQLHelper.CreateSqlParameter("@DN", int.MaxValue, dn),
                                                          SQLHelper.CreateSqlParameter("@shipDate",shipDate));
                }
           
              


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
        public void UpdateLineQty(string Connection, string Line,int Qty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                string SQL = @"Update LineQty set Qty=@Qty where Line=@Line";

                SQLHelper.ExecuteNonQuery(Connection,
                                                System.Data.CommandType.Text,
                                                SQL,
                                                SQLHelper.CreateSqlParameter("@Line",32, Line),
                                                SQLHelper.CreateSqlParameter("@Qty", Qty)
                                                );

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
        public DataTable GetLineQty(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @" select * from LineQty order by Line";
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText);

                return dt;
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
        private int GetDashboardPlanQty(string Connection)
        {
            string shift = "N";
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 20)
            { shift = "D"; }
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
          
            try
            {
                string SQLText = @" select SUM(Qty) from LineQty 
                                                where LEN(Line)=3 and Substring(Line,3,1)=@shift";
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.Text,
                                                                  SQLText,
                                                                  SQLHelper.CreateSqlParameter("@shift", 1024, shift));
                int i = 0;
                int.TryParse(dt.Rows[0][0].ToString(), out i);
                return i;
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
            // 

        
        }
        public DataTable GetDashBoardData_Detail(string Connection, DateTime fromDate, DateTime toDate, string line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"sp_Query_DashBoard_Detail ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                  SQLHelper.CreateSqlParameter("@D_Start", fromDate),
                                                                  SQLHelper.CreateSqlParameter("@D_End", toDate),
                                                                  SQLHelper.CreateSqlParameter("@Line", 64, line)
                                                                  );

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

        public DataTable GetDashBoardData(string Connection, out int totalPlanQty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            totalPlanQty = GetDashboardPlanQty(Connection);
            try
            {
                string SQLText = @"sp_Query_DashBoard ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText
                                                                  );

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
