USE [GetData]
GO

/****** Object:  Trigger [dbo].[IMES_UpdateBom]    Script Date: 04/05/2014 13:43:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vincent
-- Create date: 2012-05-26
-- Description:	Bom update Trigger
-- Upgrade History:
-- =============================================
CREATE trigger [dbo].[IMES_UpdateBom]
on [dbo].[Bom] for update
as 
begin
    declare @now datetime 
    set @now =getdate()
    insert into dbo.BomHist(Bom_MPno, Bom_Idex, Bom_SPno, Bom_Qty, Bom_Editor, Bom_Cdt, Bom_Udt, Bom_Site,TableName, [Action], ActionCdt)
	select MPno, Idex, SPno, Qty, Editor, Cdt, Udt, [Site],'Bom','UD',@now
	  from deleted
	  
	insert into dbo.BomHist(Bom_MPno, Bom_Idex, Bom_SPno, Bom_Qty, Bom_Editor, Bom_Cdt, Bom_Udt, Bom_Site,TableName, [Action], ActionCdt)
	select MPno, Idex, SPno, Qty, Editor, Cdt, Udt, [Site],'Bom','UC',@now
	  from inserted
end
GO

