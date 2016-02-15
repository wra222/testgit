USE [HPIMES]
GO

/****** Object:  Index [IDX_ModelInfo_Name]    Script Date: 04/05/2014 08:25:53 ******/
CREATE NONCLUSTERED INDEX [IDX_ModelInfo_Name] ON [dbo].[ModelInfo] 
(
	[Name] ASC
)
INCLUDE ( [Model],
[Value],
[Descr],
[Editor],
[Cdt],
[Udt]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

