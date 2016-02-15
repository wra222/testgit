USE [HPIMES]
GO

/****** Object:  Index [Indx_PartNo_InfoType]    Script Date: 04/05/2014 08:29:13 ******/
CREATE NONCLUSTERED INDEX [Indx_PartNo_InfoType] ON [dbo].[PartInfo] 
(
	[PartNo] ASC,
	[InfoType] ASC
)
INCLUDE ( [InfoValue]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

