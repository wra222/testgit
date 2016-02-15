CREATE PROCEDURE [dbo].[sp_who_sqltext]
@spid int	
AS
BEGIN
	
	SET NOCOUNT ON;

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
    if (@spid=0)
    begin  
		--select * 
		-- from #who2
		  
		SELECT a.*,isnull(DEST.text,'') as SQLText
		FROM    #who2 a
		left join  sys.[dm_exec_connections] SDEC on SDEC.[most_recent_session_id]= a.SPID 
		Outer APPLY sys.[dm_exec_sql_text](SDEC.[most_recent_sql_handle]) AS DEST  
		--where a.SPID>50 --and a.BlkBy like '[0-9]%'
	end
	else
	begin
	     --select * 
		 --from #who2 a
		 --where a.SPID=@spid
		 
		SELECT SPID,HostName,BlkBy,LOGIN,DBName,ProgramName,Status,isnull(DEST.text,'') as SQLText
		FROM    #who2 a
		 inner join  sys.[dm_exec_connections] SDEC on SDEC.[most_recent_session_id]= a.SPID 
		 CROSS APPLY sys.[dm_exec_sql_text](SDEC.[most_recent_sql_handle]) AS DEST 
	    where a.SPID=@spid 			 
	end
	
END



GO


