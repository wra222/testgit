USE [HPIMES]
GO

/****** Object:  Index [Idx_ModelInfo_Model_Name]    Script Date: 04/05/2014 08:26:06 ******/
CREATE NONCLUSTERED INDEX [Idx_ModelInfo_Model_Name] ON [dbo].[ModelInfo] 
(
	[Model] ASC,
	[Name] ASC
)
INCLUDE ( [Value],
[Descr]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

