USE [HPIMES]
GO

/****** Object:  Index [Indx_Material_Component]    Script Date: 04/05/2014 08:24:46 ******/
CREATE NONCLUSTERED INDEX [Indx_Material_Component] ON [dbo].[ModelBOM] 
(
	[Material] ASC,
	[Component] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [Index]
GO

