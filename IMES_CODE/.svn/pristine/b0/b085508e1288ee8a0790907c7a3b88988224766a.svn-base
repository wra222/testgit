USE [GetData]
GO

/****** Object:  Trigger [dbo].[IMES_InsertBomParts]    Script Date: 04/05/2014 13:43:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Vincent
-- Create date: 2012-05-26
-- Description:	BomParts insert Trigger
-- Upgrade History:
-- =============================================
CREATE trigger [dbo].[IMES_InsertBomParts]
on [dbo].[BomParts] for insert
as 
begin
	insert into dbo.BomHist(Part_Pno, Part_Tp, Part_Descr, Part_Remark, Part_Editor, Part_Cdt, Part_Udt, Part_Site, TableName, [Action], ActionCdt)
	select Pno, Tp, Descr, Remark, Editor, Cdt, Udt, [Site],'BomParts','C',GETDATE()
	from inserted
end



GO

