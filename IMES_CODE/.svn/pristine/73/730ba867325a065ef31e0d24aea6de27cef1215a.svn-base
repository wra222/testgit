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