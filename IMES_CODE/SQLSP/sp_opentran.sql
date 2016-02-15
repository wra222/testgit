CREATE PROCEDURE [dbo].[sp_opentran]	
AS
BEGIN
	
	SET NOCOUNT ON;
	CREATE TABLE #opentran
    ( name VARCHAR(max),
      value varchar(max)
     )
   
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
	
    INSERT INTO #opentran EXEC ( 'dbcc opentran (''HPIMES'') WITH TABLERESULTS, NO_INFOMSGS' )
    INSERT INTO #opentran EXEC ( 'dbcc opentran (''HPDocking'') WITH TABLERESULTS, NO_INFOMSGS' )
    INSERT INTO #opentran EXEC ( 'dbcc opentran (''HPEDI'') WITH TABLERESULTS, NO_INFOMSGS' )

    SELECT SPID,HostName,BlkBy,LOGIN,DBName,ProgramName,Status,DEST.text as SQLText
	FROM    #who2 a
	inner join  sys.[dm_exec_connections] SDEC on SDEC.[most_recent_session_id]= a.SPID
	inner join 	#opentran b on a.SPID = b.value or a.BlkBy =b.value	
	CROSS APPLY sys.[dm_exec_sql_text](SDEC.[most_recent_sql_handle]) AS DEST 
	where b.name ='OLDACT_SPID' 
	      
	
	--dbcc opentran ('HPIMES')
	--dbcc opentran ('HPDocking')
	--dbcc opentran ('HPEDI')
	
     
END
GO


