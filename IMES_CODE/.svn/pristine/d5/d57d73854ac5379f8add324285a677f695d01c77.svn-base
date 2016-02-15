-- =============================================
-- Author:		vincent
-- Create date: 2013-08-29
-- Description:	lookup cursor sql statement
-- =============================================
CREATE PROCEDURE [dbo].[sp_who_cursor] 
AS
BEGIN
	SELECT qs.total_worker_time as [Total CPU Time],
        SUBSTRING(qt.text,qs.statement_start_offset/2, 
			(case when qs.statement_end_offset = -1 
			then len(convert(nvarchar(max), qt.text)) * 2 
			else qs.statement_end_offset end -qs.statement_start_offset)/2 + 1) 
		as query_text,
		qt.dbid, dbname=db_name(qt.dbid),
		qt.objectid ,
		qs.sql_handle
	FROM sys.dm_exec_query_stats qs
	cross apply sys.dm_exec_sql_text(qs.sql_handle) as qt
	WHERE qt.text LIKE 'FETCH API_CURSOR%'
	ORDER BY [Total CPU Time] DESC
	
	DECLARE @handle varbinary(64)

	DECLARE cur_handle CURSOR FORWARD_ONLY READ_ONLY FOR
		SELECT  cur.sql_handle  from   sys.dm_exec_connections con 
		cross apply sys.dm_exec_cursors(con.session_id) as cur 
		WHERE cur.fetch_buffer_size = 1  AND cur.properties LIKE 'API%'
	OPEN cur_handle
	FETCH NEXT FROM cur_handle INTO @handle
	WHILE @@FETCH_STATUS = 0
	BEGIN
		select * from sys.dm_exec_sql_text(@handle)
		FETCH NEXT FROM cur_handle INTO @handle
	END
	CLOSE cur_handle
	DEALLOCATE cur_handle
END

GO


