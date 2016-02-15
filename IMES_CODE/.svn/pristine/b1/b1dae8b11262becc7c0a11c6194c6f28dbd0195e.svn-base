USE [HPIMES]
GO

/****** Object:  Index [IDX_PartInfoType]    Script Date: 04/05/2014 08:28:28 ******/
CREATE NONCLUSTERED INDEX [IDX_PartInfoType] ON [dbo].[PartInfo] 
(
	[InfoType] ASC
)
INCLUDE ( [PartNo],
[InfoValue],
[ID],
[Editor],
[Cdt],
[Udt]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

