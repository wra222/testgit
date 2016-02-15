USE [HPIMES]
GO

drop table [Carton]
go

CREATE TABLE [dbo].[Carton](
	[CartonSN]   [varchar](20)   NOT NULL ,	
	[PalletNo]   [varchar](20)   NOT NULL default(''),
	[Model]      [varchar](16)   Not NULL default(''),
	[BoxId]      [varchar](32)   Not Null default(''),
	[PAQCStatus] [varchar](4)    Not Null default(''), 
	[Weight]     [decimal](10,2) Not Null default(0.00),
	[DNQty]        [int]           Not null default(0), 
	[Qty]          [int]           NOT NULL default(0),
	[FullQty]      [int]           NOT NULL default(0),
	[Status]       [varchar](12)    not null default('Create'),
	[PreStation]    [varchar](16)   Not NULL default(''),
	[PreStationStatus]     int      NOT NULL default(1),
    [UnPackPalletNo] [varchar](20)  NOT NULL default(''),	
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,	
 CONSTRAINT [Carton_PK] PRIMARY KEY CLUSTERED 
(	[CartonSN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [WIP_PAK]

GO

drop table [Delivery_Carton]

go

CREATE TABLE [dbo].[Delivery_Carton](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[DeliveryNo] [varchar](20) NOT NULL ,
	[CartonSN]   [varchar](20)   NOT NULL,
	[Model] [varchar](16) NOT NULL default(''),     
	[Qty]        [int]    Not Null default(0),
	[AssignQty]  [int]     NOT NULL default(0),
	--[Status]      [varchar](16) not null default('Reserve'), 
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,	
 CONSTRAINT [Delivery_Carton_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [WIP_PAK]

go
drop table [Carton_Product]

go

CREATE TABLE [dbo].[Carton_Product](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CartonSN]    [varchar](20) NOT NULL ,
	[ProductID]   [varchar](16)   NOT NULL,
	[DeliveryNo]  [varchar](20) NOT NULL default(''),     
	[Remark]      [varchar](32) not null default(''), 
	[Editor]      [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,	
 CONSTRAINT [Carton_Product_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [WIP_PAK]

GO
drop table CartonQCLog
CREATE TABLE [dbo].[CartonQCLog](
    [ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CartonSN]   [varchar](20)   NOT NULL default(''),
	[Model]      [varchar](16)   Not NULL default(''),
	[Line]       [varchar](16)   Not Null default(''),
	[Type]       [varchar](8)    Not Null default(''), 
    [Status]     [varchar](8)    NOT NULL default(''),
	[Remark]     [varchar](64)   NOT NULL default(''),
	[Editor] [varchar](32)       NOT NULL,
	[Cdt] [datetime]             NOT NULL,	
 CONSTRAINT [CartonQCLog_PK] PRIMARY KEY CLUSTERED 
(	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [HISTORY_PAK]

GO

/*
drop table BSamLocation

GO

CREATE TABLE [dbo].[BSamLocation](
	[LocationId] [int] NOT NULL,
	[Model] [varchar](16) NOT NULL default(''),
	[Qty] [int] NOT NULL default(0),
	[RemainQty] [int] NOT NULL default(0),
	[FullQty] [int] NOT NULL default(0),
	[FullCartonQty] [int] not null default(1),
	[HoldInput] [varchar](4) not null default('N'),
	[HoldOutput] [varchar](4) not null default('N'),
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,	
 CONSTRAINT [BSamLocation_PK] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [DEFINE]

GO



CREATE TABLE [dbo].[BSamModel](
	[A_Part_Model] [varchar](16) NOT NULL,
	[C_Part_Model] [varchar](16) NOT NULL default(''),
	[HP_A_Part] [varchar](16)    NOT NULL default(''),
	[HP_C_SKU]  [varchar](16)    Not Null,
	[QtyPerCarton] [int] NOT NULL default(0),
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,	
 CONSTRAINT [BSamModel_PK] PRIMARY KEY CLUSTERED 
(
	[A_Part_Model] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [DEFINE]

GO

CREATE TABLE [dbo].[DeliveryEx](
	[DeliveryNo] [varchar](20) NOT NULL default(''),
	[ShipmentNo] [varchar](20) NOT NULL default(''),
	[ShipType] [varchar](4) NOT NULL default(''),
	[PalletType] [varchar](8) NOT NULL default(''),
	[ConsolidateQty] [int] NOT NULL default(0),
	[CartonQty] [int] NOT NULL default(0),
	[QtyPerCarton] [int] NOT NULL default(0),
	[MessageCode] [varchar](16) NOT NULL default(''),
	[ShipToParty] [varchar](16) NOT NULL default(''),
	[Priority] [varchar](8) NOT NULL default(''),
	[GroupId] [varchar](16) NOT NULL default(''),
	[OrderType] [varchar](4) NOT NULL default(''),
	[BOL] [varchar](16) NOT NULL default(''),
	[HAWB] [varchar](16) NOT NULL default(''),
	[Carrier] [varchar](8) NOT NULL default(''),
	[ShipWay] [varchar](8) NOT NULL default(''),
	[PackID] [varchar](16) NOT NULL default(''),
	[Editor] [varchar](32) NOT NULL default(''),
	[Udt] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_DeliveryEx] PRIMARY KEY CLUSTERED 
(
	[DeliveryNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [DEFINE]

GO
*/


CREATE TABLE [dbo].[LineEx](
	[Line] [varchar](30) NOT NULL default(''),
	[AliasLine] [varchar](30) NOT NULL default(''),
	[AvgSpeed] [int] NOT NULL default(0),
	[AvgManPower] [int] NOT NULL default(0),
	[AvgStationQty] [int] NOT NULL default(0),
	[Owner] [varchar](64) NOT NULL default(''),
	[IEOwner] [varchar](64) NOT NULL default(''),
	[Editor] [varchar](32) NOT NULL default(''),
	[Udt] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_LineEx] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [DEFINE]

GO

CREATE TABLE [dbo].[LineSpeed](
	[Station] [varchar](10) NOT NULL default(''),
	[AliasLine] [varchar](30) NOT NULL default(''),
	[LimitSpeed] [int] NOT NULL default(0),       -- Speed is between two device 
	[IsCheckPass] [varchar](4) NOT NULL default('N'),   -- 檢查Pass時間Flag
	[LimitSpeedExpression] [varchar](4) NOT NULL default('>'), -- = equal, > 大於, < 小於, <=, >= , !=   
	[IsHoldStation] [varchar](4) NOT NULL default('N'),
	[Editor] [varchar](32) NOT NULL default(''),
	[Udt] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_LineSpeed] PRIMARY KEY CLUSTERED 
(
	[Station] ASC,
	[AliasLine] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [DEFINE]
) ON [DEFINE]
