USE [GetData]
GO

/****** Object:  Trigger [dbo].[IMES_DeleteBomParts]    Script Date: 04/05/2014 13:43:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Vincent
-- Create date: 2012-05-26
-- Description:	BomParts Delete Trigger
-- Upgrade History:
-- =============================================
CREATE trigger [dbo].[IMES_DeleteBomParts]
on [dbo].[BomParts] for delete
as 
begin
	insert into dbo.BomHist(Part_Pno, Part_Tp, Part_Descr, Part_Remark, Part_Editor, Part_Cdt, Part_Udt, Part_Site, TableName, [Action], ActionCdt)
	select Pno, Tp, Descr, Remark, Editor, Cdt, Udt, [Site],'BomParts','D',GETDATE()
	  from deleted
	
end



GO

