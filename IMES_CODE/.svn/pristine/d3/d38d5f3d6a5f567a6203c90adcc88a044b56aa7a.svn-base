CREATE PROCEDURE [dbo].[sp_who_block]	
AS
BEGIN
	
	SET NOCOUNT ON;
		
	--declare @spId TbIntList
	declare @blkId TbIntList
	
	
    CREATE TABLE #who2(
        SPID INT,
        Status VARCHAR(MAX),
        LOGIN VARCHAR(MAX),
        HostName VARCHAR(MAX),
        BlkBy VARCHAR(MAX),
        DBName VARCHAR(MAX),
        Command VARCHAR(MAX),
        CPUTime INT,
        DiskIO INT,
        LastBatch VARCHAR(MAX),
        ProgramName VARCHAR(MAX),
        SPID_1 INT,
        REQUESTID INT
		)

	INSERT INTO #who2 EXEC sp_who2
	
	--insert @spId
	--select SPID 
	-- from #who2
	 
	 insert @blkId
	 select cast( BlkBy as int)
	  from #who2
	 where  BlkBy like '[0-9]%'
	
	--select *
	--from   #who2 a
	--where  SPID in (select distinct data from @blkId)  and ltrim(BlkBy)='.'
	
	SELECT  SPID,HostName,BlkBy,LOGIN,DBName,ProgramName,Status,DEST.text as SQLText
	FROM    #who2 a
	 inner join  sys.[dm_exec_connections] SDEC on SDEC.[most_recent_session_id]= a.SPID 
	 outer APPLY sys.[dm_exec_sql_text](SDEC.[most_recent_sql_handle]) AS DEST
	 --where   a.SPID in (select data from @blkId except select data from @blkId)
	where a.SPID in (select data from @blkId) or  --(select b.BlkBy from #who2 b where  b.BlkBy like '[0-9]%') 
	      a.BlkBy in (select cast(data as varchar(max)) from @blkId)
	order by BlkBy, SPID       
END
GO


