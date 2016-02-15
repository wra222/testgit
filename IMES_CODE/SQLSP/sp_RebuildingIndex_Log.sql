
CREATE PROCEDURE [dbo].[sp_RebuildingIndex_Log] as 
set nocount on 
declare @tableList TbStringList

Insert @tableList(data)
Values('PCBLog'),('PCBTestLog'),('ProductTestLog'),('ProductLog'),
      ('PrintLog'),('RePrintLog'),('QCStatus'),('COALog')
      

execute IMES_Rebuild_Index @tableList,5,20,4


GO


