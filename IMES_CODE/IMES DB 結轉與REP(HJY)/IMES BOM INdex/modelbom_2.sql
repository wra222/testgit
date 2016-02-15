USE [HPIMES]
GO

/****** Object:  Index [IDX_Material]    Script Date: 04/05/2014 08:24:23 ******/
CREATE NONCLUSTERED INDEX [IDX_Material] ON [dbo].[ModelBOM] 
(
	[Material] ASC
)
INCLUDE ( [Component],
[Quantity],
[Alternative_item_group],
[Cdt],
[Udt]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

