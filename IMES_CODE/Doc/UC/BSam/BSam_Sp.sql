USE [HPIMES]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_PAK_BsamPackingRpt]    Script Date: 02/21/2013 15:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_Query_PAK_BsamPackingRpt]
               @ShipDate datetime
AS
BEGIN
	SET NOCOUNT ON;
	declare @LocationList varchar(20)
	
	CREATE TABLE #BsamPackingRpt(
		DeliveryNo varchar(20),
		Model varchar(20),
		Qty int,
		OK_Qty int,
		NG_Qty int,
		AvaliableLocation varchar(20)
	)		

	-- get Bsam Delivery data by ShipDate
	insert into #BsamPackingRpt (DeliveryNo, Model, Qty, OK_Qty, NG_Qty, AvaliableLocation)
    select a.DeliveryNo, a.Model, a.Qty, 0 as OK_Qty, a.Qty as NG_Qty, '' as AvaliableLocation
    from Delivery a
    inner join BSamModel b on a.Model = b.A_Part_Model
    where a.ShipDate = @ShipDate
    
    -- get Product count pass 8E station
	MERGE INTO #BsamPackingRpt as Target 
	USING (
		select a.DeliveryNo, count(distinct b.ProductID)
		from #BsamPackingRpt a
		inner join Product b on a.DeliveryNo = b.DeliveryNo and b.DeliveryNo !=''
		inner join ProductLog c on b.ProductID = c.ProductID and c.Station ='8E' and c.[Status]=1
		group by a.DeliveryNo
	) as Source (sDeliveryNo, sOK_Qty)
	ON Target.DeliveryNo = Source.sDeliveryNo
	WHEN MATCHED THEN
		update set OK_Qty = sOK_Qty, NG_Qty=Qty-sOK_Qty;
	   
    declare @Model varchar(20)
    declare @LocationStr varchar(256)
    set @LocationStr = ''	
            
    DECLARE Cursor_tmp CURSOR FOR
    (select distinct Model from #BsamPackingRpt where NG_Qty > 0)
	OPEN Cursor_tmp
	FETCH NEXT FROM Cursor_tmp INTO @Model
	WHILE @@FETCH_STATUS = 0
	BEGIN 
	    -- get top 5 LocationId of Model  
		select top 5 LocationId into #tempLocationStr 
		from BSamLocation where Model=@Model and HoldOutput='N' 
		order by LocationId
		
		-- compose the LocationId as string
		select @LocationStr = 
		       CASE WHEN (@LocationStr = '') THEN convert(varchar(3), LocationId)
		       ELSE @LocationStr+','+convert(varchar(3), LocationId)
		       END
		from #tempLocationStr 
		group by LocationId
		order by LocationId
		
		-- update AvaliableLocation 
		update #BsamPackingRpt
		set AvaliableLocation = @LocationStr
		where Model=@Model and NG_Qty > 0
		
		drop table #tempLocationStr
		set @LocationStr = ''	
		
		FETCH NEXT FROM Cursor_tmp INTO @Model	
	END
	CLOSE Cursor_tmp--Ãö³¬Cursor
	DEALLOCATE Cursor_tmp
	
	select DeliveryNo, Model, Qty, OK_Qty, NG_Qty, AvaliableLocation
	from #BsamPackingRpt
	order by Model, DeliveryNo
	  
	
END    
GO


USE [HPIMES]
GO

/****** Object:  StoredProcedure [dbo].[sp_Query_PAK_BsamPackingRpt_ByConsolidate]    Script Date: 02/21/2013 15:02:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_Query_PAK_BsamPackingRpt_ByConsolidate]
               @ShipDate datetime
AS
BEGIN
	SET NOCOUNT ON;
	declare @LocationList varchar(20)
	
	CREATE TABLE #BsamPackingRpt(
		ShipmentNo varchar(20),
		Model varchar(20),
		Qty int,
		OK_Qty int,
		NG_Qty int,
		AvaliableLocation varchar(20)
	)		

	-- get Bsam Delivery data by ShipDate
	insert into #BsamPackingRpt (ShipmentNo, Model, Qty, OK_Qty, NG_Qty)
    select c.ShipmentNo, a.Model, sum(a.Qty) as Qty, 0 as OK_Qty, sum(a.Qty) as NG_Qty
    from Delivery a
    inner join BSamModel b on a.Model = b.A_Part_Model
    inner join DeliveryEx c on a.DeliveryNo = c.DeliveryNo
    where a.ShipDate = @ShipDate
    group by c.ShipmentNo, a.Model	
	  
    -- get Product count pass 8E station
	MERGE INTO #BsamPackingRpt as Target 
	USING (
		select a.ShipmentNo, count(distinct b.ProductID)
		from #BsamPackingRpt a
		inner join DeliveryEx d on a.ShipmentNo = d.ShipmentNo
		inner join Product b on d.DeliveryNo = b.DeliveryNo and b.DeliveryNo !=''
		inner join ProductLog c on b.ProductID = c.ProductID and c.Station ='8E' and c.[Status]=1
		group by a.ShipmentNo
	) as Source (sShipmentNo, sOK_Qty)
	ON Target.ShipmentNo = Source.sShipmentNo
	WHEN MATCHED THEN
		update set OK_Qty = sOK_Qty, NG_Qty=Qty-sOK_Qty;
	   
    declare @Model varchar(20)
    declare @LocationStr varchar(256)
    set @LocationStr = ''	
            
    DECLARE Cursor_tmp CURSOR FOR
    (select distinct Model from #BsamPackingRpt where NG_Qty > 0)
	OPEN Cursor_tmp
	FETCH NEXT FROM Cursor_tmp INTO @Model
	WHILE @@FETCH_STATUS = 0
	BEGIN 
	    -- get top 5 LocationId of Model  
		select top 5 LocationId into #tempLocationStr 
		from BSamLocation where Model=@Model and HoldOutput='N' 
		order by LocationId
		
		-- compose the LocationId as string
		select @LocationStr = 
		       CASE WHEN (@LocationStr = '') THEN convert(varchar(3), LocationId)
		       ELSE @LocationStr+','+convert(varchar(3), LocationId)
		       END
		from #tempLocationStr 
		group by LocationId
		order by LocationId
		
		-- update AvaliableLocation 
		update #BsamPackingRpt
		set AvaliableLocation = @LocationStr
		where Model=@Model and NG_Qty > 0
		
		drop table #tempLocationStr
		set @LocationStr = ''	
		
		FETCH NEXT FROM Cursor_tmp INTO @Model	
	END
	CLOSE Cursor_tmp--Ãö³¬Cursor
	DEALLOCATE Cursor_tmp
	
	select ShipmentNo, Model, Qty, OK_Qty, NG_Qty, AvaliableLocation
	from #BsamPackingRpt
	order by Model, ShipmentNo
	  
	
END    
GO





