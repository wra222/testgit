USE [GetData]
GO

/****** Object:  Trigger [dbo].[IMES_UpdateBomParts]    Script Date: 04/05/2014 13:43:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Vincent
-- Create date: 2012-05-26
-- Description:	BomParts update Trigger
-- Upgrade History:
-- =============================================
CREATE trigger [dbo].[IMES_UpdateBomParts]
on [dbo].[BomParts] for update
as 
begin
    declare @now datetime 
    set @now =getdate()
    insert into dbo.BomHist(Part_Pno, Part_Tp, Part_Descr, Part_Remark, Part_Editor, Part_Cdt, Part_Udt, Part_Site, TableName, [Action], ActionCdt)
	select Pno, Tp, Descr, Remark, Editor, Cdt, Udt, [Site],'BomParts','UD',@now
	  from deleted
	  
	insert into dbo.BomHist(Part_Pno, Part_Tp, Part_Descr, Part_Remark, Part_Editor, Part_Cdt, Part_Udt, Part_Site, TableName, [Action], ActionCdt)
	select Pno, Tp, Descr, Remark, Editor, Cdt, Udt, [Site],'BomParts','UC',@now
	from inserted
	
end

GO

