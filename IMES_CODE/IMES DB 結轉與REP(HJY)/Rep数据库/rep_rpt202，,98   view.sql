USE [HPIMES_Rep]
GO
/****** Object:  View [dbo].[W8MBSPS]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 创建新视图。                                                                                     */
/* 创建视图 v_MBCFG。                                                                              */
CREATE VIEW [dbo].[W8MBSPS]
AS SELECT * FROM [192.168.0.28].HPIMES.dbo.W8MBSPS
GO
/****** Object:  View [dbo].[v_SupplierCode]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 创建视图 v_SupplierCode。                                                                       */
CREATE VIEW [dbo].[v_SupplierCode]
AS SELECT * FROM dbo.SupplierCode
GO
/****** Object:  View [dbo].[Production_Schedule_Line]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Production_Schedule_Line]
AS
SELECT * FROM [10.99.2.36].DATAWH.dbo.t_pc_production_schedule_line_runtime WHERE  part_number like 'PC%'
GO
/****** Object:  View [dbo].[view_PAKKittingMaterial]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_PAKKittingMaterial]
AS
 select  m.Material
         ,p2.PartNo,
         p2.Descr,
         Convert(int,m1.Quantity)*Convert(int,m.Quantity) as Qty
   from ModelBOM m,ModelBOM m1,Part p,Part p2 
   where --m.Material='PCAN030QJBBY'
    --and 
    m.Component=p.PartNo 
    and p.BomNodeType='VK'
    and m1.Material=m.Component
    and m1.Component=p2.PartNo
    and p2.BomNodeType='P1'
    and p.Remark='PK'
             

             UNION ALL
                 select m.Material,p.PartNo,p.Descr,Convert(int,m.Quantity) as Qty
                   from ModelBOM m,Part p 
                   where --m.Material='PCAN030QJBBY'
                     --and 
                     m.Component=p.PartNo 
                     and p.BomNodeType='C2' 

              UNION ALL 
                   select m.Material,p.PartNo,p.Descr,Convert(int,m.Quantity)as Qty
                   from ModelBOM m,Part p,PartInfo pf 
                   where --m.Material='PCAN030QJBBY'
                     --and 
                     m.Component=p.PartNo 
                     and p.BomNodeType='P1'   
                     and p.PartNo=pf.PartNo
                     and pf.InfoType='DESC' 
                     and pf.InfoValue='OOA'
              UNION ALL
                
                select m.Material,p.PartNo,p.Descr,Convert(int,m.Quantity) as Qty
                 from ModelBOM m,Part p,PartInfo f 
                 where --m.Material='PCAN030QJBBY' 
                 --and 
                 m.Component=p.PartNo 
                 and p.BomNodeType='PL'
                 and p.PartNo=f.PartNo
                 and p.Descr like '%Carton%'
                 and f.InfoType='TYPE'
                 and f.InfoValue='BOX'

            UNION ALL
            select m.Material,p.PartNo,p.Descr,Convert(int,m.Quantity)as Qty
                from ModelBOM m,Part p,PartInfo pf 
                   where --m.Material='PCAN030QJBBY'
                     --and 
                     m.Component=p.PartNo 
                     and p.BomNodeType='P1'   
                     and p.PartNo=pf.PartNo
                     and pf.InfoType='DESC' 
                     and UPPER(pf.InfoValue) like '%OFFICE%'

UNION ALL
SELECT DISTINCT
                  a.Material, RTRIM(c.PartNo) ,
                    b.Descr,c.Qty
            FROM dbo.ModelBOM a ,
                    dbo.Part b ( NOLOCK ) ,
                    WipBuffer c 
                   
            WHERE  ( a.Component LIKE '6%'
                          OR a.Component LIKE '2TG%'
                        )
                    AND a.Component = b.PartNo
                    AND b.PartType NOT IN ( 'BM', 'P1' )
                    AND b.Descr NOT IN ( 'TPM', 'ALC', 'Carton AMD LABEL',
                                         'special label' )
                    AND b.Descr NOT IN ( 'JGS' )
                    AND c.Code IN ('ESOP Label','PD PA label-1','PD PA label-2')
                    AND b.PartNo = c.PartNo
GO
/****** Object:  View [dbo].[Dashboard_Stage]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Dashboard_Stage]
AS
select a.[Stage],a.[Stage] AS Descr,a.[Type],'' AS Editor FROM
(select [Type], [Stage]=REPLACE(stuff((select ','+[Stage] from [Dashboard_Stage_Base] t 
where [Type]=[Dashboard_Stage_Base].[Type] for xml 
path('') ), 1, 1, ''),',',' & ')   from [Dashboard_Stage_Base] group by [Type]) a
GO
/****** Object:  View [dbo].[v_ProductDefectList]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_ProductDefectList]
AS
SELECT     TOP (100) PERCENT a.ID, b.ProductID, d.Model, b.Line, b.Station, f.Descr, a.DefectCode, a.Cause, RTRIM(a.MajorPart) + ' ' + a.Site AS Location, 
                      a.Obligation, a.Remark, a.Editor, b.Cdt, a.Udt
FROM         dbo.ProductRepair_DefectInfo AS a WITH (NOLOCK) LEFT OUTER JOIN
                      dbo.ProductRepair AS b WITH (NOLOCK) ON a.ProductRepairID = b.ID LEFT OUTER JOIN
                      dbo.ProductTestLog AS c WITH (NOLOCK) ON b.TestLogID = c.ID LEFT OUTER JOIN
                      dbo.Product AS d WITH (NOLOCK) ON d.ProductID = b.ProductID LEFT OUTER JOIN
                      dbo.Station AS f WITH (NOLOCK) ON b.Station = f.Station
GO
/****** Object:  View [dbo].[v_PCADefectList]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_PCADefectList]
AS
SELECT     ROW_NUMBER() OVER (ORDER BY a.Cdt) AS ID, a.PCBNo, pc.PCBModelID AS Pno, pa.Descr AS Family, a.Line, sta.Station, pr_info.DefectCode, pr_info.Cause, 
RTRIM(pr_info.Type) + ' ' + pr_info.Location AS [Location], pr_info.Obligation, pr_info.Remark, pr_info.Editor, a.Cdt, pr_info.Udt
FROM         PCBLog a INNER JOIN
                      Station sta ON a.Station = sta.Station AND sta.StationType = 'SATest' LEFT JOIN
                      dbo.PCBRepair pr ON a.ID = pr.LogID LEFT JOIN
                      dbo.PCBRepair_DefectInfo pr_info ON pr.ID = pr_info.PCARepairID LEFT JOIN
                      dbo.PCB pc ON a.PCBNo = pc.PCBNo LEFT JOIN
                      dbo.Part pa ON pc.PCBModelID = pa.PartNo
WHERE     a.Status = 0
GO
/****** Object:  View [dbo].[v_MBCFG]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 创建新视图。                                                                                     */
/* 创建视图 v_MBCFG。                                                                              */
CREATE VIEW [dbo].[v_MBCFG]
AS SELECT * FROM dbo.MBCFG
GO
/****** Object:  View [dbo].[v_Bom_Code]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 创建视图 v_Bom_Code。                                                                           */
CREATE VIEW [dbo].[v_Bom_Code]
AS SELECT * FROM dbo.Bom_Code
GO
/****** Object:  View [dbo].[V_Dummy_ShipDet]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 创建新视图。                                                                                     */
/* 创建视图 v_MBCFG。                                                                              */
CREATE VIEW [dbo].[V_Dummy_ShipDet]
AS SELECT * FROM Dummy_ShipDet
GO
/****** Object:  View [dbo].[view_wip_station_NoLog]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_NoLog]
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
           a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt,  f.ShipDate, b.Line, 
           (CASE 
                       WHEN (b.Station ='85'OR b.Station ='86')  AND  Q.Status='8'
                             THEN '85Q' 
                         WHEN b.Station ='PO' AND  Q.Status='9'
                             THEN 'POP' 
                         WHEN b.Station ='PO' AND  Q.Status='A'
                             THEN 'POF'
                        ELSE RTRIM(b.Station) 
            END)AS Station,
            b.Editor, b.Udt AS StatusUdt, g.Line as Start_Line
FROM                --select  Dateadd(day,-30,GETDATE())
 --(select * from Product WITH (NOLOCK) where  Cdt>Dateadd(day,-15,GETDATE())) a INNER JOIN
                 dbo.Product AS a WITH (NOLOCK)   
INNER JOIN dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT OUTER JOIN (
                   select ProductID,AttrValue as [Status] 
                    from ProductAttr WITH (NOLock) where AttrName='PAQC_QCStatus' 
			    ) Q  on a.ProductID=Q.ProductID
-- (select ProductID,[Status] 
--                   from QCStatus WITH (NOLock)
--				   where ID in ((select MAX(ID) 
--							      from QCStatus WITH (NOLock) 
--							      where Tp='PAQC'	 
--							      group by ProductID)
--					             ) 
--					   ) Q  on a.ProductID=Q.ProductID
--LEFT OUTER JOIN (select a1.ProductID,a1.[Status] 
--                   from QCStatus a1 WITH (NOLock)
--                    cross apply fn_Last_QC_Status(a1.ProductID, a1.ID)
--				 ) Q  on a.ProductID=Q.ProductID    
INNER JOIN (  select ProductID,Line 
              from  ProductLog WITH (NOLock) 
              where Station='40'
              group by ProductID,Line
		   ) g on a.ProductID=g.ProductID
GO
/****** Object:  View [dbo].[view_wip_station_PAK_TEST]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_PAK_TEST]
 WITH SCHEMABINDING
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, 
           a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, 
           a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.DeliveryNo, 
           a.PalletNo, a.OOAID,a.Cdt,f.ShipDate, b.Line, 
           c.Descr,
           (CASE  WHEN (b.Station ='85'OR b.Station ='86')  AND  Q.Status='8'
                  THEN '85Q' 
                  WHEN b.Station ='PO' AND  Q.Status='9'
                  THEN 'POP' 
                  WHEN b.Station ='PO' AND  Q.Status='A'
                  THEN 'POF'
                  WHEN b.Station IN ('79', '6A') AND EXISTS
                           (SELECT    top 1 ProductID
                            FROM          dbo.ProductLog(nolock)
                            WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt) 
                  THEN '66' 
                  WHEN b.Station = '73' AND EXISTS
                          (SELECT    top 1 ProductID
                            FROM     dbo.ProductLog   
                            WHERE    ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt)
                  THEN '66E'                  
                  ELSE RTRIM(b.Station) 
            END)AS Station, b.Editor, b.Udt AS StatusUdt,
            (CASE WHEN b.Station IN ('79', '6A', '73') AND EXISTS
                          (SELECT   top 1  ProductID
                            FROM          dbo.ProductLog WITH(nolock)
                            WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt)
                             THEN 'Image DL Pass' 
                   ELSE RTRIM(c.Descr) 
              END) AS StationDescr,
              g.Line as Start_Line
     FROM dbo.Product AS a                  
  -- (select * from Product    where  Cdt>Dateadd(day,-15,GETDATE())) AS a  INNER JOIN                 
     INNER JOIN  dbo.ProductStatus AS b    ON a.ProductID = b.ProductID 
     INNER JOIN  dbo.Station AS c    ON b.Station= c.Station 
     INNER JOIN dbo.Delivery AS f    ON a.DeliveryNo = f.DeliveryNo 
     INNER JOIN dbo.QCStatus AS Q on a.ProductID=Q.ProductID
                   and ID in
                   (
                   select MAX(ID) 
					                from dbo.QCStatus    
					                where Tp='PAQC'	 
							        group by ProductID
                   )
     
     
       --(select ProductID,[Status] 
       --                from dbo.QCStatus   
					  -- where ID in((select MAX(ID) 
					  --              from dbo.QCStatus    
					  --              where Tp='PAQC'	 
							--        group by ProductID)) 
					  -- ) Q  on a.ProductID=Q.ProductID
     INNER JOIN ( select distinct ProductID,Line 
                        from dbo.ProductLog    
                        where Station='40'
                        group by ProductID,Line
				      ) g on a.ProductID=g.ProductID
GO
/****** Object:  View [dbo].[view_FA_wip_station_Test]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_FA_wip_station_Test]
AS


--select @x= Value from SysSetting where Name='PAKCosmetic_SUM'
--select * from fn_split(@x,',')
 

SELECT     a.ProductID,Isnull(f.Model,a.Model) as Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
                      a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt, m.StartDate, f.ShipDate, b.Line as StationLine,b.Status,
                 
                    case  
                        when isnull(a1.Station, b.Station)='68' and isnull(a.DeliveryNo,'') !='' Then 'NoBT68'
                        when isnull(a1.Station, b.Station)='85' and isnull(a.DeliveryNo,'') !='' Then 'NoBT85'      
                        else
                            isnull(a1.Station, b.Station)
                        end
                  AS Station, 
                        b.Editor, 
                        isnull(a1.Cdt,b.Udt) AS StatusUdt, 
                        e.Family, 
                      (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS Descr,
                       f.ShipDate as P_ShipDate,
                       case 
   when 
     
          a1.Station in
				(select fn.value from SysSetting as S 
				 CROSS APPLY fn_split(S.Value,',') as fn
				where Name in('PAKCosmetic_SUM','MVS_SUM')
         )
       --gt station  
         
 
   then b.Line
   else g.AttrValue
  end as Line 
                       
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.Model AS e WITH (NOLock) ON a.Model = e.Model
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
--LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
--LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP'  
LEFT OUTER JOIN dbo.MO AS m WITH (NOLOCK) ON a.MO = m.MO
 --LEFT JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
 LEFT JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT JOIN  dbo.ProductAttr AS g WITH (NOLock) ON a.ProductID = g.ProductID and g.AttrName='StartLine'
--LEFT OUTER JOIN  dbo.ProductInfo as g WITH (NOLock) ON a.ProductID=g.ProductID and g.InfoType='ShipDate'
GO
/****** Object:  View [dbo].[view_FA_wip_station]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_FA_wip_station]
AS
SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
                      a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt, m.StartDate, f.ShipDate, b.Line as StationLine,b.Status,
                        (case  
                        when isnull(a1.Station, b.Station)='68' and isnull(a.DeliveryNo,'') !='' Then 'NoBT68'
                        when isnull(a1.Station, b.Station)='85' and isnull(a.DeliveryNo,'') !='' Then 'NoBT85'      
                        else
                            isnull(a1.Station, b.Station)
                        end) AS Station, 
                        b.Editor, 
                        isnull(a1.Cdt,b.Udt) AS StatusUdt, 
                        e.Family, 
                      (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS Descr,
                       f.ShipDate as P_ShipDate,
                       CASE 
					   WHEN 
					     
							  b.Station in('39','3A','3B','3M' ) 
									--(select fn.value from SysSetting as S 
									-- CROSS APPLY fn_split(S.Value,',') as fn
									--where Name in('39','3A','3B','3M' ) --39 3A 3B 3M 
							 --)
					   then b.Line  
					   else g.AttrValue
					  end as Line 
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.Model AS e WITH (NOLock) ON a.Model = e.Model
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
--LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
--LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP'  
LEFT OUTER JOIN dbo.MO AS m WITH (NOLOCK) ON a.MO = m.MO
 LEFT JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT JOIN  dbo.ProductAttr AS g WITH (NOLock) ON a.ProductID = g.ProductID and g.AttrName='StartLine'

--StartLine 
 --FULL JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 

--LEFT OUTER JOIN  dbo.ProductInfo as g WITH (NOLock) ON a.ProductID=g.ProductID and g.InfoType='ShipDate'
GO
/****** Object:  View [dbo].[view_wip_status_InputLine]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_status_InputLine]
AS
SELECT     a.ProductID, a.CUSTSN, a.Model, a.DeliveryNo, a.PalletNo, d.ShipDate, 
           isnull(p.AttrValue, '') as InputLine, b.Line, 
           isnull(a1.Station, b.Station)  AS Station,
           isnull(a1.Status, b.Status) as Status, 
           b.Editor, 
          ISNULL(a1.Cdt, b.Udt) as Udt, 
           e.Family, 
        (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS Descr, 
	     a.PCBID, 
	     a.PCBModel, 
         a.Cdt
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
LEFT OUTER JOIN  ProductAttr as p with (nolock) ON a.ProductID = p.ProductID and p.AttrName='StartLine' 
LEFT OUTER JOIN dbo.Delivery AS d WITH (NOLOCK) ON a.DeliveryNo = d.DeliveryNo 
LEFT OUTER JOIN dbo.Model AS e WITH (NOLOCK) ON a.Model = e.Model
GO
/****** Object:  View [dbo].[view_wip_status]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_status]
AS
SELECT     a.ProductID, a.CUSTSN, a.Model, a.DeliveryNo, a.PalletNo, d.ShipDate, b.Line, 
           isnull(a1.Station, b.Station)  AS Station,
           isnull(a1.Status, b.Status) as Status, 
           b.Editor, 
          ISNULL(a1.Cdt, b.Udt) as Udt, 
           e.Family, 
        (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS Descr, 
	     a.PCBID, 
	     a.PCBModel, 
         a.Cdt
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
--LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
--LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP' 
LEFT OUTER JOIN dbo.Delivery AS d WITH (NOLOCK) ON a.DeliveryNo = d.DeliveryNo 
LEFT OUTER JOIN dbo.Model AS e WITH (NOLOCK) ON a.Model = e.Model
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 114
               Left = 236
               Bottom = 222
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 204
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_wip_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_wip_status'
GO
/****** Object:  View [dbo].[view_wip_station_group]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_group]
AS
SELECT  substring(b.Line,1,1) as PdLine, b.Line, 
           a.ProductID, a.Model, a.CUSTSN,            
           isnull(a1.Station, b.Station) as Station,
           (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr,
           (CASE WHEN a1.Station is not null  THEN a1.Station
            ELSE RTRIM(d.Type)
            END) AS WCGroup,            
           isnull(a1.Cdt, b.Udt) AS StatusUdt
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID and b.Station <> 'F0'
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.StationOrder AS d WITH (NOLOCK) ON b.Station = d.Station
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1  
--outer apply  dbo.fn_GetIMGDownLoadstation(b.ProductID, b.Station, b.Udt) as a1
GO
/****** Object:  View [dbo].[view_wip_station_PAK_Vincent]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_PAK_Vincent]
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, 
           a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, 
           a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.DeliveryNo, 
           a.PalletNo, a.OOAID,a.Cdt,f.ShipDate, b.Line, 
           c.Descr,
           (CASE  WHEN (b.Station ='85'OR b.Station ='86')  AND  Q.Status='8'
                  THEN '85Q' 
                  WHEN b.Station ='PO' AND  Q.Status='9'
                  THEN 'POP' 
                  WHEN b.Station ='PO' AND  Q.Status='A'
                  THEN 'POF'
                  WHEN b.Station IN ('79', '6A') AND
						   ((a1.AttrValue is not null and a1.Udt > b.Udt)  OR
                            (a2.AttrValue is not null and a2.Udt > b.Udt))	 
                           --EXISTS
                           --(SELECT    top 1 ProductID
                           -- FROM          dbo.ProductLog(nolock)
                           -- WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt) 
                  THEN '66' 
                  WHEN b.Station = '73' AND
							((a1.AttrValue is not null and a1.Udt > b.Udt)  OR
                            (a2.AttrValue is not null and a2.Udt > b.Udt)) 
                          -- EXISTS
                          --(SELECT    top 1 ProductID
                          --  FROM     dbo.ProductLog WITH (nolock)
                          --  WHERE    ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt)
                  THEN '66E'                  
                  ELSE RTRIM(b.Station) 
            END)AS Station, b.Editor, b.Udt AS StatusUdt,
            (CASE WHEN b.Station IN ('79', '6A', '73') AND
							((a1.AttrValue is not null and a1.Udt > b.Udt)  OR
                            (a2.AttrValue is not null and a2.Udt > b.Udt)) 
                          --EXISTS
                          --(SELECT   top 1  ProductID
                          --  FROM          dbo.ProductLog WITH(nolock)
                          --  WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt)
                   THEN 'Image DL Pass' 
                   ELSE RTRIM(c.Descr) 
              END) AS StationDescr,
              g.Line as Start_Line
     FROM dbo.Product AS a WITH (NOLOCK)               
  -- (select * from Product WITH (NOLOCK) where  Cdt>Dateadd(day,-15,GETDATE())) AS a  INNER JOIN                 
     INNER JOIN  dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
     INNER JOIN  dbo.Station AS c WITH (NOLOCK) ON b.Station= c.Station
     LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
     LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP' 
     LEFT OUTER JOIN dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
     LEFT OUTER JOIN (select a1.ProductID,a1.[Status] 
                   from QCStatus a1 WITH (NOLock)
                    cross apply fn_Last_QC_Status(a1.ProductID, a1.ID) 
					   ) Q  on a.ProductID=Q.ProductID
     LEFT OUTER JOIN ( select ProductID,Line 
                        from dbo.ProductLog WITH (NOLock) 
                        where Station='40'
                        group by ProductID,Line
				      ) g on a.ProductID=g.ProductID
GO
/****** Object:  View [dbo].[view_wip_station_PAK3]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_PAK3]
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, 
           a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, 
           a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.DeliveryNo, 
           a.PalletNo, a.OOAID,a.Cdt,f.ShipDate, b.Line, 
           c.Descr,
           (CASE  WHEN (b.Station ='85'OR b.Station ='86')  AND  (Q.AttrValue='8' or Q.AttrValue='C' or Q.AttrValue='B') 
                  THEN '85Q' 
                  WHEN b.Station ='PO' AND  Q.AttrValue='9'
                  THEN 'POP' 
                  WHEN b.Station ='PO' AND  Q.AttrValue='A'
                  THEN 'POF'                 
                  ELSE RTRIM(isnull(a1.Station,b.Station)) 
            END)AS Station, b.Editor, isnull(a1.Cdt, b.Udt) AS StatusUdt,
            (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr, g.AttrValue as Start_Line
     FROM dbo.Product AS a WITH (NOLOCK)                  
     INNER JOIN  dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
     INNER JOIN  dbo.Station AS c WITH (NOLOCK) ON b.Station= c.Station
     OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1
     --LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
     --LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP' 
     LEFT OUTER JOIN dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
     left outer join  ProductAttr as Q WITH (NOLock) on a.ProductID=Q.ProductID and Q.AttrName='PAQC_QCStatus' 
 
     LEFT OUTER JOIN ProductAttr as g WITH (NOLock) 
                 on a.ProductID=g.ProductID and g.AttrName='StartLine'
GO
/****** Object:  View [dbo].[view_wip_station_PAK]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_PAK]
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, 
           a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, 
           a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.DeliveryNo, 
           a.PalletNo, a.OOAID,a.Cdt,f.ShipDate, b.Line, 
           c.Descr,
           (CASE  WHEN (b.Station ='85'OR b.Station ='86')  AND  Q.Status='8'
                  THEN '85Q' 
                  WHEN b.Station ='PO' AND  Q.Status='9'
                  THEN 'POP' 
                  WHEN b.Station ='PO' AND  Q.Status='A'
                  THEN 'POF'                 
                  ELSE RTRIM(isnull(a1.Station,b.Station)) 
            END)AS Station, b.Editor, 
            isnull(a1.Cdt, b.Udt) AS StatusUdt,
            (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr,
              g.Line as Start_Line
     FROM dbo.Product AS a WITH (NOLOCK)              
 
     INNER JOIN  dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
     INNER JOIN  dbo.Station AS c WITH (NOLOCK) ON b.Station= c.Station
     OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
     --LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
     --LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP' 
     LEFT OUTER JOIN dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
   
	 LEFT OUTER JOIN (select a1.ProductID,a1.[Status] 
                   from QCStatus a1 WITH (NOLock)
                    cross apply fn_Last_QC_Status(a1.ProductID, a1.ID)				   
					   ) Q  on a.ProductID=Q.ProductID    				
     LEFT OUTER JOIN ( select ProductID,Line 
                        from dbo.ProductLog WITH (NOLock) 
                        where Station='40'
                        group by ProductID,Line
				      ) g on a.ProductID=g.ProductID
GO
/****** Object:  View [dbo].[view_wip_station_NoLog_test]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_NoLog_test]
AS

SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
           a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt,  f.ShipDate, b.Line, 
           (CASE 
                       WHEN (b.Station ='85'OR b.Station ='86')  AND  Q.Status='8'
                             THEN '85Q' 
                         WHEN b.Station ='PO' AND  Q.Status='9'
                             THEN 'POP' 
                         WHEN b.Station ='PO' AND  Q.Status='A'
                             THEN 'POF'
                        ELSE RTRIM(b.Station) 
            END)AS Station,
            b.Editor, b.Udt AS StatusUdt, g.Line as Start_Line
FROM                --select  Dateadd(day,-30,GETDATE())
 --(select * from Product WITH (NOLOCK) where  Cdt>Dateadd(day,-15,GETDATE())) a INNER JOIN
                 dbo.Product AS a WITH (NOLOCK)   
INNER JOIN dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT OUTER JOIN (select a1.ProductID,a1.[Status] 
                   from QCStatus a1 WITH (NOLock)
                    cross apply fn_Last_QC_Status(a1.ProductID, a1.ID)
				 ) Q  on a.ProductID=Q.ProductID   
INNER JOIN (  select ProductID,Line 
              from  ProductLog WITH (NOLock) 
              where Station='40'
              group by ProductID,Line
		   ) g on a.ProductID=g.ProductID
GO
/****** Object:  View [dbo].[view_wip_station_InputLine]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_InputLine]
AS
SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
                      a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt, m.StartDate, f.ShipDate, case when(isnull(p.AttrValue, '') ='') then b.Line else p.AttrValue end as Line, 
                        isnull(a1.Station, b.Station)  AS Station, 
                        b.Editor, 
                        isnull(a1.Cdt,b.Udt) AS StatusUdt, 
                        e.Family, 
                      (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr,
                       CONVERT(datetime,g.InfoValue) as P_ShipDate
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.Model AS e WITH (NOLock) ON a.Model = e.Model
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
LEFT OUTER JOIN  ProductAttr as p with (nolock) ON a.ProductID = p.ProductID and p.AttrName='StartLine' 
LEFT OUTER JOIN dbo.MO AS m WITH (NOLOCK) ON a.MO = m.MO
LEFT OUTER JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT OUTER JOIN  dbo.ProductInfo as g WITH (NOLock) ON a.ProductID=g.ProductID and g.InfoType='ShipDate'
GO
/****** Object:  View [dbo].[view_wip_station_0]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station_0]
AS
SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, 
           a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, 
           a.UnitWeight, a.CartonSN, a.CartonWeight, a.DeliveryNo, a.PalletNo, a.State, 
           a.OOAID, a.PRSN, a.Cdt, a.Udt, m.StartDate,f.ShipDate, 
           b.Line, 
           isnull(a1.Station, b.Station) as Station,
           /*
                      (CASE WHEN b.Station IN ('79', '6A') AND
                          ((a1.AttrValue is not null and a1.Udt > b.Udt)  or
                           (a2.AttrValue is not null and a2.Udt > b.Udt)) 
                          --EXISTS
                          --(SELECT    top 1 ProductID
                          --  FROM          ProductLog(nolock)
                          --  WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt) 
                        THEN '66' 
                        WHEN b.Station = '73' AND
                          ((a1.AttrValue is not null and a1.Udt > b.Udt)  or
                           (a2.AttrValue is not null and a2.Udt > b.Udt))  
                          --  EXISTS
                          --(SELECT    top 1 ProductID
                          --  FROM          ProductLog
                          --  WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt) 
                        THEN '66E' 
                        ELSE RTRIM(b.Station) 
                        END) AS Station,
                        */ 
                        b.Editor, 
                        isnull(a1.PassTime, b.Udt) AS StatusUdt, 
                        e.Family, 
                        /*
                      (CASE WHEN b.Station IN ('79', '6A', '73') AND
                            ((a1.AttrValue is not null and a1.Udt > b.Udt)  or
                           (a2.AttrValue is not null and a2.Udt > b.Udt))    
                             --EXISTS
                             --(SELECT   top 1  ProductID
                             -- FROM          ProductLog(nolock)
                             --  WHERE      ProductID = a.ProductID AND Station = '66' AND Cdt > b.Udt) 
                       THEN 'Image DL Pass' 
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr,
                       */
                       (CASE WHEN a1.Station in ('2PP', 'D/L') THEN a1.Station
                        ELSE RTRIM(c.Descr)
                        END) AS StationDescr ,
                       CONVERT(datetime,g.InfoValue) as P_ShipDate
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.Model AS e WITH (NOLock) ON a.Model = e.Model
outer apply  dbo.fn_GetIMGDownLoadstation(b.ProductID, b.Station, b.Udt) as a1 
--LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
--LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP'  
LEFT OUTER JOIN dbo.MO AS m WITH (NOLOCK) ON a.MO = m.MO
LEFT OUTER JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT OUTER JOIN  dbo.ProductInfo as g WITH (NOLock) ON a.ProductID=g.ProductID and g.InfoType='ShipDate'
GO
/****** Object:  View [dbo].[view_wip_station2]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station2]
AS
SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN,
           a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
           a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt, 
           f.ShipDate, b.Line, isnull(a1.Station, b.Station) as Station
FROM    dbo.Product AS a WITH (NOLOCK) 
INNER JOIN  dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
LEFT OUTER JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1
GO
/****** Object:  View [dbo].[view_wip_station]    Script Date: 04/05/2014 11:07:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_wip_station]
AS
SELECT     a.ProductID, a.Model, a.PCBID, a.PCBModel, a.MAC, a.UUID, a.MBECR, a.CVSN, a.CUSTSN, a.ECR, a.PizzaID, a.MO, a.UnitWeight, a.CartonSN, a.CartonWeight, 
                      a.DeliveryNo, a.PalletNo, a.State, a.OOAID, a.PRSN, a.Cdt, a.Udt, m.StartDate, f.ShipDate, b.Line, 
                        isnull(a1.Station, b.Station)  AS Station, 
                        b.Editor, 
                        isnull(a1.Cdt,b.Udt) AS StatusUdt, 
                        e.Family, 
                      (CASE isnull(a1.Station, b.Station) 
                       WHEN  '6P' THEN  'Image Download'
                       WHEN  '6R' THEN  'Run-In Test'  
                       WHEN  '6I' THEN  'Inject WIN8 Key'
                       WHEN  '66' THEN  'POST Test'
                       WHEN  '6U' THEN  'WIN8 Unpack Key'
                       ELSE RTRIM(c.Descr) 
                       END) AS StationDescr,
                       CONVERT(datetime,g.InfoValue) as P_ShipDate
FROM         dbo.Product AS a WITH (NOLOCK) 
INNER JOIN   dbo.ProductStatus AS b WITH (NOLOCK) ON a.ProductID = b.ProductID 
INNER JOIN   dbo.Station AS c WITH (NOLOCK) ON b.Station = c.Station 
INNER JOIN   dbo.Model AS e WITH (NOLock) ON a.Model = e.Model
OUTER APPLY  dbo.fn_GetLastProdutLog(b.ProductID,b.Station) a1 
--LEFT OUTER JOIN  ProductAttr as a1 with (nolock) ON a.ProductID = a1.ProductID and a1.AttrName='IMG_IMAGEDOWN_PASS' 
--LEFT OUTER JOIN  ProductAttr as a2 with (nolock) ON a.ProductID = a2.ProductID and a2.AttrName='IMG_IMAGEDOWN_2PP'  
LEFT OUTER JOIN dbo.MO AS m WITH (NOLOCK) ON a.MO = m.MO
LEFT OUTER JOIN  dbo.Delivery AS f WITH (NOLock) ON a.DeliveryNo = f.DeliveryNo 
LEFT OUTER JOIN  dbo.ProductInfo as g WITH (NOLock) ON a.ProductID=g.ProductID and g.InfoType='ShipDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 114
               Left = 236
               Bottom = 222
               Right = 399
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 204
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f"
            Begin Extent = 
               Top = 222
               Left = 242
               Bottom = 330
               Right = 393
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
      ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_wip_station'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'   Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_wip_station'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_wip_station'
GO
