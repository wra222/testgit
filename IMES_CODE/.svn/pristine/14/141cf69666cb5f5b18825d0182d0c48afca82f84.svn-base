select object_name(P.object_id) as TableName, 
        resource_type, resource_description,L.request_session_id,
        P.blocking_session_id
from sys.dm_tran_locks L
    join sys.partitions P on L.resource_associated_entity_id = P.hobt_id
	
	
select object_name(P.object_id) as TableName, 
        resource_type, resource_description,L.request_session_id,
        P.blocking_session_id,t2.blocking_session_id
from sys.dm_tran_locks L
join sys.partitions P on L.resource_associated_entity_id = P.hobt_id
left join sys.dm_os_waiting_tasks AS t2  ON L.lock_owner_address = t2.resource_address


--Locking table & blocking_Session
SELECT  object_name(P.object_id) as TableName,
        t1.resource_type,
        t1.resource_database_id,
        t1.resource_associated_entity_id,
        t1.request_mode,
        t1.request_session_id,
        t2.blocking_session_id
    FROM sys.dm_tran_locks as t1
	join sys.partitions P on t1.resource_associated_entity_id = P.hobt_id
    INNER JOIN sys.dm_os_waiting_tasks as t2
        ON t1.lock_owner_address = t2.resource_address;


select object_name(P.object_id) as TableName, 
        L.resource_type, 
        L.resource_description,
        L.request_session_id,
        t2.blocking_session_id
from sys.dm_tran_locks L
join sys.partitions P on L.resource_associated_entity_id = P.hobt_id
left join sys.dm_os_waiting_tasks AS t2  ON L.lock_owner_address = t2.resource_address

SELECT 
        t1.resource_type,
        t1.resource_database_id,
        t1.resource_associated_entity_id,
        t1.request_mode,
        t1.request_session_id,
        t2.blocking_session_id
FROM sys.dm_tran_locks AS t1
INNER JOIN sys.dm_os_waiting_tasks AS t2
    ON t1.lock_owner_address = t2.resource_address	
	
	
SELECT DEST.TEXT  
FROM sys.[dm_exec_connections] SDEC 
 CROSS APPLY sys.[dm_exec_sql_text](SDEC.[most_recent_sql_handle]) AS DEST 
WHERE SDEC.[most_recent_session_id] = 52 


dbcc opentran ('Northwind')

DBCC DBREINDEX('Table Name')

SELECT * 
FROM sys.dm_tran_session_transactions 

sys.dm_tran_locks, sys.dm_exec_sessions, and sys.dm_exec_requests 
select @@SPID

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
truncate table #who2
INSERT INTO #who2 EXEC sp_who2   
SELECT  *
FROM    #who2


===Displays the last statement sent from a client to an instance of Microsoft SQL Server.
DBCC INPUTBUFFER ( session_id [ , request_id ])


-----Rebuit index

SELECT 'ALTER INDEX [' + ix.name + '] ON [' + s.name + '].[' + t.name + '] ' +
   CASE
  WHEN ps.avg_fragmentation_in_percent > 15   THEN 'REBUILD'
  ELSE 'REORGANIZE'
   END +
   CASE
  WHEN pc.partition_count > 1
  THEN ' PARTITION = ' + CAST(ps.partition_number AS nvarchar(MAX))
  ELSE ''
   END,
   avg_fragmentation_in_percent
FROM   sys.indexes AS ix
   INNER JOIN sys.tables t
   ON t.object_id = ix.object_id
   INNER JOIN sys.schemas s
   ON t.schema_id = s.schema_id
   INNER JOIN
  (SELECT object_id    ,
  index_id 	 ,
  avg_fragmentation_in_percent,
  partition_number
  FROM	sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL, NULL, NULL)
  ) ps
   ON t.object_id = ps.object_id
  AND ix.index_id = ps.index_id
   INNER JOIN
  (SELECT  object_id,
   index_id ,
   COUNT(DISTINCT partition_number) AS partition_count
  FROM sys.partitions
  GROUP BY object_id,
   index_id
  ) pc
   ON t.object_id   = pc.object_id
  AND ix.index_id   = pc.index_id
WHERE  ps.avg_fragmentation_in_percent > 10
   AND ix.name IS NOT NULL
   
   
-=====================Query Plan=================

SELECT cp.objtype AS ObjectType,
OBJECT_NAME(st.objectid,st.dbid) AS ObjectName,
cp.usecounts AS ExecutionCount,
st.TEXT AS QueryText,
qp.query_plan AS QueryPlan
FROM sys.dm_exec_cached_plans AS cp
CROSS APPLY sys.dm_exec_query_plan(cp.plan_handle) AS qp
CROSS APPLY sys.dm_exec_sql_text(cp.plan_handle) AS st
--WHERE OBJECT_NAME(st.objectid,st.dbid) = 'YourObjectName'   



=======Missing Icdx =========================

SELECT t.name AS 'affected_table'
    , 'Create NonClustered Index IX_' + t.name + '_missing_' 
        + CAST(ddmid.index_handle AS VARCHAR(10))
        + ' On ' + ddmid.STATEMENT 
        + ' (' + IsNull(ddmid.equality_columns,'') 
        + CASE WHEN ddmid.equality_columns IS Not Null 
            And ddmid.inequality_columns IS Not Null THEN ',' 
                ELSE '' END 
        + IsNull(ddmid.inequality_columns, '')
        + ')' 
        + IsNull(' Include (' + ddmid.included_columns + ');', ';'
        ) AS sql_statement
    , ddmigs.user_seeks
    , ddmigs.user_scans
    , CAST((ddmigs.user_seeks + ddmigs.user_scans) 
        * ddmigs.avg_user_impact AS INT) AS 'est_impact'
    , ddmigs.last_user_seek
FROM sys.dm_db_missing_index_groups AS ddmig
INNER JOIN sys.dm_db_missing_index_group_stats AS ddmigs
    ON ddmigs.group_handle = ddmig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details AS ddmid 
    ON ddmig.index_handle = ddmid.index_handle
INNER JOIN sys.tables AS t
    ON ddmid.OBJECT_ID = t.OBJECT_ID
WHERE ddmid.database_id = DB_ID()
    --AND t.name = 'myTableName' 
ORDER BY CAST((ddmigs.user_seeks + ddmigs.user_scans) 
    * ddmigs.avg_user_impact AS INT) DESC;
