-- =============================================
-- Author:		vincent Lee
-- Create date: 2013-12-20
-- Description:	Get dead lock xml
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDeadLock] 
AS
BEGIN
	select c.query('.') xmlData, 
			c.value('(./@name)[1]','varchar(50)') name, 
			c.value('(./@id)[1]','varchar(50)') id, 
			c.value('(./@timestamp)[1]','datetime') timestamp, 
			c.value('(./data/value)[1]','varchar(50)') value 
	from (select CAST(target_data as XML) errEvent 
			from sys.dm_xe_session_targets dxst where target_data is not null 
		) t 
	cross apply errEvent.nodes('//event[@name="xml_deadlock_report"]') n( c )

END



GO
