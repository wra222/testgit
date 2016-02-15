USE [HPIMES]
GO

/****** Object:  Index [Idx_BomNode_PartType]    Script Date: 04/05/2014 08:27:56 ******/
CREATE NONCLUSTERED INDEX [Idx_BomNode_PartType] ON [dbo].[Part] 
(
	[PartNo] ASC,
	[BomNodeType] ASC,
	[PartType] ASC
)
INCLUDE ( [Descr],
[Remark],
[Flag]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

