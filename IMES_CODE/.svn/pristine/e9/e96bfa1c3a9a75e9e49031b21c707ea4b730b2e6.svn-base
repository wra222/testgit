USE [GetData]
GO

/****** Object:  Trigger [dbo].[IMES_DeleteBom]    Script Date: 04/05/2014 13:42:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vincent
-- Create date: 2012-05-26
-- Description:	Bom Delete Trigger
-- Upgrade History:
-- =============================================
CREATE trigger [dbo].[IMES_DeleteBom]
on [dbo].[Bom] for delete
as 
begin
	insert into dbo.BomHist(Bom_MPno, Bom_Idex, Bom_SPno, Bom_Qty, Bom_Editor, Bom_Cdt, Bom_Udt, Bom_Site,TableName, [Action], ActionCdt)
	select MPno, Idex, SPno, Qty, Editor, Cdt, Udt, [Site],'Bom','D',GETDATE()
	  from deleted
end





GO

