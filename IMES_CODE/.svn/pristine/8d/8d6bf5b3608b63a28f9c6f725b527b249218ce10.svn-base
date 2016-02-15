-- =============================================
-- Author:		Vincent
-- Create date: 2014/09/14
-- Description:	Rebuild Index by Table
-- =============================================
CREATE PROCEDURE [dbo].[IMES_Rebuild_Index] 
   @TableNameList TbStringList readonly,
   @FragmentLowRatio decimal,
   @FragmentHighRatio decimal,
   @CPU           int
AS   
BEGIN
	SET NOCOUNT ON;
	declare @RebuildIndexCmd varchar(max)
	declare @ReorganizeIndexCmd varchar(max)
	declare @ID int
	declare @cmd varchar(max)
	declare @dbname nvarchar(128)
	declare @fragment  decimal(8,4)
	
	
	set @RebuildIndexCmd= 'USE %dbName%  ALTER INDEX %IndexName% ON %TableName% REBUILD PARTITION = ALL WITH ( PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, ONLINE = ON, SORT_IN_TEMPDB = OFF, MAXDOP=%CPU%)'
	set @ReorganizeIndexCmd ='USE %dbName% ALTER INDEX %IndexName% ON %TableName% REORGANIZE WITH ( LOB_COMPACTION = ON )'
	Create Table #RebuildIndex
	(
		[ID] int identity(1,1) not  null,
		[SchemaName] nvarchar(128) not null,
		[TableName] nvarchar(128)  not null,
		[IndexName] nvarchar(128) not null,
		[Fragment]  decimal(10,4) not null
	)
	
	SET @dbname = '['+DB_NAME(DB_ID())+']'
	set @RebuildIndexCmd = REPLACE(@RebuildIndexCmd,'%dbName%', @dbname)
	set @RebuildIndexCmd = REPLACE(@RebuildIndexCmd,'%CPU%', Cast(@CPU as varchar(2)))
	set @ReorganizeIndexCmd = REPLACE(@ReorganizeIndexCmd,'%dbName%', @dbname)
	 
	Insert #RebuildIndex([SchemaName],[TableName],[IndexName],[Fragment])
	SELECT '['+object_schema_name(IPS.object_id)+']' as SchemaName,
	       '['+ST.name+']' AS [TableName],        
           '['+SI.name+']' AS [IndexName], 
           IPS.avg_fragmentation_in_percent            
	FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL ,'LIMITED') IPS 
	INNER JOIN sys.tables ST WITH (nolock) on IPS.OBJECT_ID = ST.OBJECT_ID  
	INNER JOIN sys.indexes SI WITH (nolock) on IPS.OBJECT_ID = SI.OBJECT_ID AND IPS.index_id = SI.index_id 
	INNER JOIN @TableNameList tb on tb.data = ST.name
	WHERE	ST.is_ms_shipped = 0 AND 
			SI.Name is not null  AND 
			IPS.avg_fragmentation_in_percent >= @FragmentLowRatio
	ORDER BY ST.name 
	
	--select * from #RebuildIndex
	print  CONVERT(varchar(48),getdate(),121) +' BEGIN'+char(13) + char(10)
	WHILE (exists(select ID from #RebuildIndex))
	BEGIN
	  SELECT top 1 @ID =a.ID,@fragment=a.Fragment  from #RebuildIndex a  order by a.ID
	  
	  IF @fragment between @FragmentLowRatio and @FragmentHighRatio
	  BEGIN
			SELECT @cmd =REPLACE(REPLACE(@ReorganizeIndexCmd,'%IndexName%',a.IndexName),'%TableName%', a.SchemaName +'.' +a.TableName)
			FROM #RebuildIndex a
			WHERE a.ID=@ID	  
	  END
	  ELSE
	  BEGIN
			SELECT @cmd =REPLACE(REPLACE(@RebuildIndexCmd,'%IndexName%',a.IndexName),'%TableName%', a.SchemaName +'.' +a.TableName)
			FROM #RebuildIndex a
			WHERE a.ID=@ID   
	  END  
	   
	    
	  print  CONVERT(varchar(48),getdate(),121) +' ' +@cmd+char(13) + char(10)	  
	  execute(@cmd)
	  
	  delete from #RebuildIndex where ID =@ID 
	END
	print  CONVERT(varchar(48),getdate(),121) +' END'+char(13) + char(10)
	
END

GO
