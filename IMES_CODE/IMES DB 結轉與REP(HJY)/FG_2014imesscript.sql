USE [HPIMES_2014]
GO
/****** Object:  Table [dbo].[xx_TempNum32]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[xx_TempNum32](
	[Num] [varchar](32) NOT NULL,
	[NextNum] [varchar](32) NOT NULL,
 CONSTRAINT [PK_TempNum32] PRIMARY KEY CLUSTERED 
(
	[Num] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[xx_MFG_tmpforCount]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[xx_MFG_tmpforCount](
	[SnoId] [varchar](14) NOT NULL,
	[Pno] [varchar](16) NULL,
	[Model] [varchar](50) NOT NULL,
	[WC] [varchar](20) NOT NULL,
	[PdLine] [varchar](12) NOT NULL,
	[Defect] [varchar](500) NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[xx_MFG_AlarmDefect_Dump]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[xx_MFG_AlarmDefect_Dump](
	[Pno] [varchar](12) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[PdLine] [varchar](20) NOT NULL,
	[WC] [varchar](20) NULL,
	[Defect] [varchar](1000) NOT NULL,
	[Symptom] [varchar](8000) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Mark] [varchar](1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[xx_IN1]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xx_IN1](
	[PalletNo] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[xx_IN]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xx_IN](
	[PalletNo] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmp_Trck_no]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_Trck_no](
	[DeliveryNo] [nvarchar](50) NULL,
	[ITEM] [nvarchar](50) NULL,
	[Serial_Num] [nvarchar](50) NULL,
	[Tracking_No] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmp_Iqc]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_Iqc](
	[Column 0] [nvarchar](50) NULL,
	[Column 1] [nvarchar](50) NULL,
	[Column 2] [nvarchar](50) NULL,
	[Column 3] [nvarchar](50) NULL,
	[Column 4] [nvarchar](50) NULL,
	[Column 5] [nvarchar](50) NULL,
	[Column 6] [nvarchar](50) NULL,
	[Column 7] [nvarchar](2000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[temp_dB_icc114059_PalletType_20140411]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[temp_dB_icc114059_PalletType_20140411](
	[ID] [int] NOT NULL,
	[ShipWay] [varchar](8) NOT NULL,
	[RegId] [varchar](16) NOT NULL,
	[Type] [nvarchar](32) NOT NULL,
	[StdPltFullQty] [varchar](8) NULL,
	[MinQty] [int] NOT NULL,
	[MaxQty] [int] NOT NULL,
	[Code] [varchar](8) NOT NULL,
	[PltWeight] [decimal](10, 2) NULL,
	[MinusPltWeight] [char](1) NOT NULL,
	[CheckCode] [varchar](8) NOT NULL,
	[ChepPallet] [char](1) NOT NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[PalletLayer] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tableName]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tableName](
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[Line] [char](30) NOT NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rpt_PCARep]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rpt_PCARep](
	[ID] [int] NOT NULL,
	[SnoId] [char](11) NOT NULL,
	[Tp] [char](5) NOT NULL,
	[Remark] [char](400) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Mark] [char](1) NOT NULL,
	[Username] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [rpt_PCARep_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[i_part_number_data]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[i_part_number_data](
	[part_number] [varchar](50) NOT NULL,
	[product_family] [varchar](50) NOT NULL,
	[description] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WipBuffer_bk_d]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WipBuffer_bk_d](
	[ID] [int] NOT NULL,
	[Code] [varchar](30) NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Tp] [varchar](50) NULL,
	[LightNo] [char](4) NOT NULL,
	[Picture] [varchar](30) NULL,
	[Qty] [int] NULL,
	[Sub] [varchar](20) NULL,
	[Safety_Stock] [int] NULL,
	[Max_Stock] [int] NULL,
	[Remark] [varchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[KittingType] [varchar](20) NOT NULL,
	[Station] [nvarchar](50) NOT NULL,
	[Line] [char](2) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WipBuffer_BK]    Script Date: 09/03/2014 10:09:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WipBuffer_BK](
	[ID] [varchar](48) NOT NULL,
	[Code] [varchar](30) NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Tp] [varchar](50) NULL,
	[LightNo] [char](4) NOT NULL,
	[Picture] [varchar](30) NULL,
	[Qty] [int] NULL,
	[Sub] [varchar](20) NULL,
	[Safety_Stock] [int] NULL,
	[Max_Stock] [int] NULL,
	[Remark] [varchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[KittingType] [varchar](20) NOT NULL,
	[Station] [nvarchar](50) NOT NULL,
	[Line] [char](2) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WipBuffer]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WipBuffer](
	[ID] [int] NOT NULL,
	[Code] [varchar](30) NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Tp] [varchar](50) NULL,
	[LightNo] [char](4) NOT NULL,
	[Picture] [varchar](30) NULL,
	[Qty] [int] NULL,
	[Sub] [varchar](20) NULL,
	[Safety_Stock] [int] NULL,
	[Max_Stock] [int] NULL,
	[Remark] [varchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[KittingType] [varchar](20) NOT NULL,
	[Station] [nvarchar](50) NOT NULL,
	[Line] [char](2) NULL,
 CONSTRAINT [WipBuffer_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WeightLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeightLog](
	[ID] [int] NOT NULL,
	[SN] [char](20) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [WeightLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Weight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Weight](
	[Model] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Warranty]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Warranty](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[Type] [char](10) NOT NULL,
	[DateCodeType] [char](10) NULL,
	[WarrantyFormat] [char](10) NULL,
	[ShipTypeCode] [char](2) NULL,
	[WarrantyCode] [char](1) NULL,
	[Descr] [varchar](80) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Warranty_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WPTR_ZMOD]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WPTR_ZMOD](
	[ZMOD] [char](5) NOT NULL,
	[Descr] [varchar](20) NOT NULL,
	[IECPN] [char](12) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WLBTDescr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WLBTDescr](
	[ID] [int] NOT NULL,
	[Code] [char](20) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[TpDescr] [char](30) NOT NULL,
	[Descr] [nvarchar](50) NOT NULL,
	[Site] [varchar](50) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [WLBTDescr_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WH_PltWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WH_PltWeight](
	[ID] [int] NOT NULL,
	[DN] [varchar](20) NULL,
	[Plt] [varchar](12) NULL,
	[PltQty] [int] NULL,
	[ForecasetCartonWeight] [decimal](10, 2) NULL,
	[ForecasetPltWeight] [decimal](10, 2) NULL,
	[ActualCartonWeight] [decimal](10, 2) NULL,
	[ActualPltWeight] [decimal](10, 2) NULL,
	[PltMaterialWeight] [decimal](10, 2) NULL,
	[PltWeightInaccuracy] [decimal](10, 3) NULL,
	[Remark] [varchar](1) NULL,
 CONSTRAINT [WH_PltWeight_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WH_PLTMas]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WH_PLTMas](
	[ID] [int] NOT NULL,
	[PLT] [nvarchar](14) NULL,
	[WC] [nchar](4) NULL,
	[Editor] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [WH_PLTMas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WH_PLTLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WH_PLTLog](
	[ID] [int] NOT NULL,
	[PLT] [nvarchar](14) NULL,
	[WC] [nchar](4) NULL,
	[Editor] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [WH_PLTLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WH_PLTLocLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WH_PLTLocLog](
	[ID] [int] NOT NULL,
	[PLT] [char](10) NOT NULL,
	[LOC] [char](10) NOT NULL,
	[Remark] [varchar](50) NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [WH_PLTLocLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[W8MBSPS]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[W8MBSPS](
	[MB] [nvarchar](255) NULL,
	[ZMOD] [nvarchar](255) NULL,
	[TPMONSPS] [nvarchar](255) NULL,
	[TPMOFFSPS] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VISTestDetail]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VISTestDetail](
	[SnoId] [varchar](12) NOT NULL,
	[SN] [varchar](12) NOT NULL,
	[Result] [varchar](12) NOT NULL,
	[ErrorCode] [varchar](12) NULL,
	[ErrorDescription] [varchar](12) NOT NULL,
	[PdLine] [varchar](12) NOT NULL,
	[Editor] [varchar](12) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UploadPallet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UploadPallet](
	[UploadId] [char](32) NULL,
	[Delivery_Pallet_DeliveryNo] [char](20) NULL,
	[Delivery_Pallet_PalletNo] [char](20) NULL,
	[Delivery_Pallet_DeliveryQty] [smallint] NULL,
	[PalletType] [varchar](80) NULL,
	[Pallet_UCC] [char](30) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UploadDelivery]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UploadDelivery](
	[UploadId] [char](32) NULL,
	[Delivery_DeliveryNo] [char](20) NULL,
	[Delivery_ShipDate] [char](8) NULL,
	[Delivery_PoNo] [char](20) NULL,
	[Customer] [varchar](200) NULL,
	[Name1] [varchar](200) NULL,
	[Name2] [varchar](200) NULL,
	[ShiptoId] [varchar](200) NULL,
	[Adr1] [varchar](200) NULL,
	[Adr2] [varchar](200) NULL,
	[Adr3] [varchar](200) NULL,
	[Adr4] [varchar](200) NULL,
	[City] [varchar](200) NULL,
	[State] [varchar](200) NULL,
	[Postal] [varchar](200) NULL,
	[Country] [varchar](200) NULL,
	[Carrier] [varchar](200) NULL,
	[SO] [varchar](200) NULL,
	[CustPo] [varchar](200) NULL,
	[Delivery_Model] [varchar](200) NULL,
	[BOL] [varchar](200) NULL,
	[Invoice] [varchar](200) NULL,
	[RetCode] [varchar](200) NULL,
	[ProdNo] [varchar](200) NULL,
	[IECSo] [varchar](200) NULL,
	[IECSoItem] [varchar](200) NULL,
	[PoItem] [varchar](200) NULL,
	[CustSo] [varchar](200) NULL,
	[ShipFrom] [varchar](200) NULL,
	[ShipingMark] [varchar](200) NULL,
	[Flag] [varchar](200) NULL,
	[RegId] [varchar](200) NULL,
	[BoxSort] [varchar](200) NULL,
	[Duty] [varchar](200) NULL,
	[RegCarrier] [varchar](200) NULL,
	[Destination] [varchar](200) NULL,
	[Department] [varchar](200) NULL,
	[OrdRefrence] [varchar](200) NULL,
	[DeliverTo] [varchar](200) NULL,
	[Tel] [varchar](200) NULL,
	[WH] [varchar](200) NULL,
	[Delivery_ShipmentNo] [char](20) NULL,
	[SKU] [varchar](200) NULL,
	[Deport] [varchar](200) NULL,
	[Delivery_Qty] [varchar](200) NULL,
	[CQty] [varchar](200) NULL,
	[EmeaCarrier] [varchar](200) NULL,
	[Plant] [varchar](200) NULL,
	[ShipTp] [varchar](200) NULL,
	[ConfigID] [varchar](200) NULL,
	[HYML] [varchar](200) NULL,
	[CustName] [varchar](200) NULL,
	[ShipHold] [varchar](200) NULL,
	[CTO_DS] [varchar](200) NULL,
	[PackType] [varchar](200) NULL,
	[PltType] [varchar](200) NULL,
	[ShipWay] [varchar](200) NULL,
	[Dept] [varchar](200) NULL,
	[MISC] [varchar](200) NULL,
	[ShipFromNme] [varchar](200) NULL,
	[ShipFromAr1] [varchar](200) NULL,
	[ShipFromCty] [varchar](200) NULL,
	[ShipFromTl] [varchar](200) NULL,
	[DnItem] [varchar](200) NULL,
	[Price] [varchar](200) NULL,
	[BoxReg] [varchar](200) NULL,
	[DAY850] [varchar](200) NULL,
	[TSBCUPO] [varchar](200) NULL,
	[OrderDate] [varchar](200) NULL,
	[Descr1] [varchar](200) NULL,
	[UPID] [varchar](200) NULL,
	[Descr2] [varchar](200) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackProduct_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackProduct_Part](
	[ID] [int] NOT NULL,
	[Product_PartID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL,
	[UEditor] [varchar](30) NOT NULL,
	[UPdt] [datetime] NOT NULL,
 CONSTRAINT [UnpackProduct_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackProductStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackProductStatus](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[Line] [char](30) NOT NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[UEditor] [varchar](30) NULL,
	[UPdt] [datetime] NULL,
 CONSTRAINT [UnpackProductStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackProductInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackProductInfo](
	[ID] [int] NOT NULL,
	[ProductInfoID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](225) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[UEditor] [varchar](30) NOT NULL,
	[UPdt] [datetime] NOT NULL,
 CONSTRAINT [UnpackProductInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackProduct]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackProduct](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [varchar](35) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[State] [varchar](64) NULL,
	[OOAID] [char](20) NULL,
	[PRSN] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[UEditor] [varchar](30) NULL,
	[UPdt] [datetime] NULL,
 CONSTRAINT [UnpackProduct_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackPizza_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackPizza_Part](
	[ID] [int] NOT NULL,
	[PizzaID] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[UEditor] [varchar](30) NOT NULL,
	[UPdt] [datetime] NOT NULL,
	[Pizza_PartID] [int] NOT NULL,
	[CheckItemType] [varchar](20) NOT NULL,
 CONSTRAINT [UnpackPizza_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnpackPizzaStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnpackPizzaStatus](
	[ID] [int] NOT NULL,
	[PizzaID] [char](20) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[UEditor] [varchar](30) NULL,
	[UPdt] [datetime] NULL,
 CONSTRAINT [UnpackPizzaStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnitWeightLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnitWeightLog](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_UnitWeightLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TxnDataLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[TxnDataLog](
	[ID] [int] NOT NULL,
	[Category] [varchar](16) NULL,
	[Action] [varchar](32) NULL,
	[KeyValue1] [varchar](32) NULL,
	[KeyValue2] [varchar](32) NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[TxnDataLog] ADD [TxnId] [varchar](64) NULL
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[TxnDataLog] ADD [ErrorCode] [varchar](32) NULL
ALTER TABLE [dbo].[TxnDataLog] ADD [ErrorDescr] [nvarchar](255) NULL
ALTER TABLE [dbo].[TxnDataLog] ADD [State] [varchar](16) NULL
ALTER TABLE [dbo].[TxnDataLog] ADD [Comment] [nvarchar](255) NULL
ALTER TABLE [dbo].[TxnDataLog] ADD [Cdt] [datetime] NULL
ALTER TABLE [dbo].[TxnDataLog] ADD  CONSTRAINT [PK_SendDataLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Txf_Mapping_WC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Txf_Mapping_WC](
	[FIS_WC] [varchar](2) NOT NULL,
	[IMES_WC] [varchar](16) NOT NULL,
 CONSTRAINT [PK_Txf_Mapping_WC] PRIMARY KEY CLUSTERED 
(
	[FIS_WC] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransferToIMESList_FA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransferToIMESList_FA](
	[ProductID] [varchar](9) NOT NULL,
	[Station] [varchar](5) NOT NULL,
	[CUSTSN] [varchar](30) NOT NULL,
	[UID] [char](36) NOT NULL,
	[IsTransfer] [int] NOT NULL,
	[Remark] [varchar](100) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransferToIMESList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransferToIMESList](
	[PCBNo] [varchar](20) NOT NULL,
	[Station] [varchar](5) NOT NULL,
	[UID] [char](36) NOT NULL,
	[IsTransfer] [int] NOT NULL,
	[Remark] [varchar](100) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransferToFISList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransferToFISList](
	[PCBNo] [varchar](20) NOT NULL,
	[Station] [varchar](5) NOT NULL,
	[UID] [char](36) NOT NULL,
	[IsTransfer] [int] NOT NULL,
	[Remark] [varchar](100) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TraceStd]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TraceStd](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Area] [varchar](50) NULL,
	[Type] [varchar](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [TraceStd_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TmpTable]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TmpTable](
	[ID] [int] NOT NULL,
	[BegSN] [char](15) NOT NULL,
	[EndSN] [char](15) NOT NULL,
	[PO] [char](16) NULL,
	[CustPN] [varchar](30) NULL,
	[IECPN] [char](15) NULL,
	[MSPN] [varchar](30) NULL,
	[Descr] [varchar](80) NULL,
	[ShipDate] [datetime] NULL,
	[Qty] [int] NULL,
	[Cust] [char](10) NULL,
	[Upload] [varchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[PC] [varchar](20) NULL,
 CONSTRAINT [TmpTable_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TmpKit]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TmpKit](
	[ID] [int] NOT NULL,
	[PdLine] [varchar](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Qty] [int] NOT NULL,
	[Type] [char](4) NOT NULL,
 CONSTRAINT [TmpKit_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestErrorCodeMapping]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestErrorCodeMapping](
	[Customer] [varchar](96) NOT NULL,
	[TestErrorCode] [varchar](32) NOT NULL,
	[DefectCode] [varchar](32) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [TestErrorCodeMapping_PK] PRIMARY KEY CLUSTERED 
(
	[Customer] ASC,
	[TestErrorCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestBoxDataLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestBoxDataLog](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[TestCase] [varchar](16) NULL,
	[LineNum] [varchar](16) NULL,
	[FixtureID] [varchar](20) NULL,
	[WC] [varchar](16) NULL,
	[IsPass] [int] NULL,
	[Descr] [nvarchar](1024) NULL,
	[ErrorCode] [varchar](16) NULL,
	[TestTime] [varchar](32) NULL,
	[ProductID] [varchar](32) NULL,
	[SerialNumber] [varchar](16) NULL,
	[CartonSn] [varchar](12) NULL,
	[DateManufactured] [varchar](12) NULL,
	[MACAddress] [varchar](12) NULL,
	[IMEI] [varchar](15) NULL,
	[IMSI] [varchar](15) NULL,
	[EventType] [varchar](25) NULL,
	[DeviceAttribute] [varchar](25) NULL,
	[Platform] [varchar](25) NULL,
	[ICCID] [varchar](25) NULL,
	[EAN] [varchar](13) NULL,
	[ModelNumber] [varchar](8) NULL,
	[PalletSerialNo] [varchar](20) NULL,
	[PublicKey] [varchar](512) NULL,
	[PrivateKey] [varchar](128) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [TestBoxDataLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Test]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[ShipWay] [nvarchar](255) NULL,
	[RegId] [nvarchar](255) NULL,
	[Type] [nvarchar](255) NULL,
	[StdPItFullQty] [float] NULL,
	[MinQty] [float] NULL,
	[MaxQty] [float] NULL,
	[Code] [nvarchar](255) NULL,
	[PItWeight] [nvarchar](255) NULL,
	[MinusPItWeight] [float] NULL,
	[CheckCode] [nvarchar](255) NULL,
	[ChepPallet] [nvarchar](255) NULL,
	[Editor] [nvarchar](255) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[PalletLayer] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temp_20140407COA2]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Temp_20140407COA2](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](225) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Temp_20140407COA1]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Temp_20140407COA1](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](225) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Temp_20140407COA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Temp_20140407COA](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](225) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TSModel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TSModel](
	[ID] [int] NOT NULL,
	[Model] [char](20) NULL,
	[Editor] [char](20) NULL,
	[Cdt] [datetime] NULL,
	[Mark] [char](1) NULL,
 CONSTRAINT [TSModel_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TPCB_Maintain]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TPCB_Maintain](
	[ID] [int] NOT NULL,
	[TPCB] [varchar](20) NULL,
	[Tp] [varchar](20) NULL,
	[Vcode] [varchar](20) NULL,
	[Editor] [varchar](50) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [TPCB_Maintain_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TPCBDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TPCBDet](
	[Code] [nvarchar](30) NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	[PartNo] [nvarchar](20) NOT NULL,
	[Vendor] [nvarchar](20) NOT NULL,
	[Dcode] [nvarchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [TPCBDet_PK] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TPCB]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TPCB](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NULL,
	[PdLine] [nvarchar](20) NULL,
	[Type] [nvarchar](20) NULL,
	[PartNo] [nvarchar](20) NULL,
	[Vendor] [nvarchar](20) NULL,
	[Dcode] [nvarchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [TPCB_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TEMP_PRODUCTBCK]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TEMP_PRODUCTBCK](
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [varchar](35) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[State] [varchar](64) NULL,
	[OOAID] [char](20) NULL,
	[PRSN] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysSettingLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysSettingLog](
	[ID] [int] NOT NULL,
	[Name] [varchar](64) NULL,
	[Value] [nvarchar](3000) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [nvarchar](30) NULL,
	[BackUpDate] [datetime] NOT NULL,
	[Action] [varchar](16) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SysSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SysSetting](
	[ID] [int] NOT NULL,
	[Name] [varchar](64) NULL,
	[Value] [nvarchar](3000) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [SysSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SupplierCode_FA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SupplierCode_FA](
	[ID] [int] NOT NULL,
	[Vendor] [varchar](50) NOT NULL,
	[Idex] [char](2) NOT NULL,
	[Code] [char](10) NOT NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [SupplierCode_FA_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SupplierCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SupplierCode](
	[ID] [int] NOT NULL,
	[Vendor] [varchar](50) NOT NULL,
	[Idex] [varchar](2) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [SupplierCode_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StationOrder]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StationOrder](
	[Station] [varchar](32) NOT NULL,
	[OrderId] [int] NULL,
	[Type] [varchar](64) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StationCheck]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StationCheck](
	[ID] [int] NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [varchar](30) NULL,
	[CheckItemType] [varchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Family] [varchar](50) NULL,
	[Model] [varchar](20) NULL,
	[Customer] [varchar](50) NOT NULL,
 CONSTRAINT [StationCheck_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StationAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StationAttr](
	[AttrName] [varchar](64) NOT NULL,
	[Station] [char](10) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Descr] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [StationAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[Station] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Station](
	[Station] [char](10) NOT NULL,
	[Name] [varchar](64) NULL,
	[StationType] [char](20) NOT NULL,
	[OperationObject] [int] NOT NULL,
	[Descr] [nvarchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Station_PK] PRIMARY KEY CLUSTERED 
(
	[Station] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Stage]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stage](
	[Stage] [char](10) NOT NULL,
	[Descr] [varchar](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Stage_PK] PRIMARY KEY CLUSTERED 
(
	[Stage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Special_Maintain_PAK]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Special_Maintain_PAK](
	[ID] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
	[DB] [char](10) NULL,
	[SWC] [char](10) NOT NULL,
	[L_WC] [char](10) NULL,
	[M_WC] [char](10) NOT NULL,
	[Tp] [char](2) NOT NULL,
	[Message] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[Editor] [char](18) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Special_Maintain_PAK_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Special_Maintain]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Special_Maintain](
	[Type] [char](10) NOT NULL,
	[DB] [char](10) NULL,
	[SWC] [char](10) NOT NULL,
	[L_WC] [char](10) NULL,
	[M_WC] [char](10) NOT NULL,
	[Tp] [char](2) NOT NULL,
	[Message] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[Editor] [char](18) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [Special_Maintain_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Special_Det]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Special_Det](
	[SnoId] [char](20) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Sno1] [varchar](50) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [Special_Det_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SpecialOrder]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SpecialOrder](
	[FactoryPO] [varchar](32) NOT NULL,
	[Category] [varchar](16) NOT NULL,
	[AssetTag] [varchar](32) NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [varchar](16) NOT NULL,
	[Remark] [varchar](255) NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [SpecialOrder_PK] PRIMARY KEY CLUSTERED 
(
	[FactoryPO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoLog3D]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoLog3D](
	[ID] [int] NOT NULL,
	[PCBNo] [char](10) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Line] [char](30) NULL,
	[FixtureID] [varchar](20) NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [SnoLog3D_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoDet_PoMo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoDet_PoMo](
	[SnoId] [char](14) NOT NULL,
	[Mo] [char](14) NULL,
	[PO] [varchar](50) NULL,
	[POItem] [char](10) NULL,
	[Delivery] [varchar](20) NULL,
	[PLT] [char](12) NULL,
	[BoxId] [varchar](30) NULL,
	[Remark] [varchar](20) NULL,
	[Editor] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [SnoDet_PoMo_PK] PRIMARY KEY CLUSTERED 
(
	[SnoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoDet_BTLoc]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoDet_BTLoc](
	[ID] [int] NOT NULL,
	[SnoId] [char](14) NOT NULL,
	[CPQSNO] [char](14) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Sno] [char](35) NOT NULL,
	[Status] [char](10) NOT NULL,
	[PdLine] [char](10) NOT NULL,
	[Editor] [char](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [SnoDet_BTLoc_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_Off]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_Off](
	[ID] [int] NOT NULL,
	[BegNo] [char](20) NOT NULL,
	[EndNo] [char](20) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Editor] [char](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[PartNo] [varchar](20) NULL,
 CONSTRAINT [SnoCtrl_Off_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_BoxId_SQ_mt_T]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_BoxId_SQ_mt_T](
	[Cust] [char](10) NOT NULL,
	[Num] [int] NOT NULL,
	[BegNo] [varchar](10) NOT NULL,
	[FixcodeNum] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_BoxId_SQ_mt]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_BoxId_SQ_mt](
	[Code] [char](20) NOT NULL,
	[Cust] [char](10) NOT NULL,
	[Num] [int] NOT NULL,
	[Valid] [char](1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_BoxId_SQ_Log]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_BoxId_SQ_Log](
	[Cust] [char](10) NOT NULL,
	[BegNo] [char](10) NOT NULL,
	[EndNo] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_BoxId_SQ]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_BoxId_SQ](
	[Cust] [char](10) NOT NULL,
	[BoxId] [char](10) NOT NULL,
 CONSTRAINT [SnoCtrl_BoxId_SQ_PK] PRIMARY KEY CLUSTERED 
(
	[BoxId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SnoCtrl_BoxId]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SnoCtrl_BoxId](
	[ID] [int] NOT NULL,
	[Cust] [char](10) NOT NULL,
	[BoxId] [char](10) NOT NULL,
	[valid] [char](5) NOT NULL,
 CONSTRAINT [SnoCtrl_BoxId_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmallPartsUpload]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SmallPartsUpload](
	[TSBPN] [varchar](20) NOT NULL,
	[IECPN] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
 CONSTRAINT [SmallPartsUpload_PK] PRIMARY KEY CLUSTERED 
(
	[TSBPN] ASC,
	[IECPN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShipType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShipType](
	[ShipType] [varchar](30) NOT NULL,
	[Description] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ShipType_PK] PRIMARY KEY CLUSTERED 
(
	[ShipType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShipKP]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShipKP](
	[ShipDate] [varchar](50) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Descr] [varchar](50) NOT NULL,
	[IECPN] [varchar](50) NOT NULL,
	[Vendor] [varchar](50) NOT NULL,
	[Qty] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShipBoxDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShipBoxDet](
	[Shipment] [char](10) NOT NULL,
	[Invioce] [char](10) NOT NULL,
	[DeliveryNo] [varchar](20) NOT NULL,
	[PLT] [char](12) NOT NULL,
	[BoxId] [varchar](20) NOT NULL,
	[SnoId] [char](10) NOT NULL,
	[Editor] [varchar](40) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [ShipBoxDet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SendData]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SendData](
	[ID] [int] NOT NULL,
	[Action] [varchar](32) NULL,
	[KeyValue1] [varchar](32) NULL,
	[KeyValue2] [varchar](32) NULL,
	[TxnId] [varchar](32) NULL,
	[ErrorCode] [varchar](32) NULL,
	[ErrorDescr] [varchar](255) NULL,
	[State] [varchar](16) NULL,
	[ResendCount] [int] NULL,
	[Comment] [varchar](255) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_SendData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMTTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[SMTTime](
	[ID] [int] NOT NULL,
	[Date] [smalldatetime] NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[ActTime] [decimal](5, 1) NOT NULL,
	[Cause] [varchar](10) NOT NULL,
	[ActTime1] [decimal](5, 1) NULL,
	[Cause1] [varchar](10) NULL,
	[ActTime2] [decimal](5, 1) NULL,
	[Cause2] [varchar](10) NULL,
	[ActTime3] [decimal](5, 1) NULL,
	[Cause3] [varchar](10) NULL,
	[Remark] [nvarchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL,
 CONSTRAINT [PK_SMTTime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMTMOAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SMTMOAttr](
	[AttrName] [varchar](64) NOT NULL,
	[SMTMO] [char](8) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [SMTMOAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMTMO]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SMTMO](
	[SMTMO] [char](8) NOT NULL,
	[Process] [varchar](64) NULL,
	[PCBFamily] [varchar](50) NOT NULL,
	[IECPartNo] [varchar](12) NOT NULL,
	[Qty] [int] NOT NULL,
	[PrintQty] [int] NOT NULL,
	[Remark] [varchar](255) NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [SMTMO_PK] PRIMARY KEY CLUSTERED 
(
	[SMTMO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMTLine]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[SMTLine](
	[Line] [varchar](30) NOT NULL,
	[ObTime] [decimal](4, 1) NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [SMTLine_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMTCT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[SMTCT](
	[Line] [varchar](30) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[CT] [decimal](4, 1) NOT NULL,
	[OptRate] [float] NOT NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [SMTCT_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SFGSite]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SFGSite](
	[ID] [int] NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[InfoType] [varchar](50) NOT NULL,
	[InfoValue] [varchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [SFGSite_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SA_FUNHDCP_ID]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SA_FUNHDCP_ID](
	[MAC] [char](12) NOT NULL,
	[KSV] [varchar](10) NOT NULL,
	[Cnt] [int] NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_SA_FUNHDCP_ID] PRIMARY KEY NONCLUSTERED 
(
	[KSV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SAP_WEIGHT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SAP_WEIGHT](
	[ID] [int] NOT NULL,
	[DN/Shipment] [char](30) NOT NULL,
	[Status] [char](2) NOT NULL,
	[KG] [decimal](10, 1) NOT NULL,
 CONSTRAINT [SAP_WEIGHT_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SAP_VOLUME_PLT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SAP_VOLUME_PLT](
	[ID] [int] NOT NULL,
	[Shippment&Dn] [varchar](20) NULL,
	[SnoId] [varchar](15) NOT NULL,
	[Weight] [decimal](10, 3) NOT NULL,
	[KG] [decimal](10, 1) NOT NULL,
	[a] [decimal](10, 1) NOT NULL,
	[b] [decimal](10, 1) NOT NULL,
	[h] [decimal](10, 1) NOT NULL,
	[CM] [varchar](2) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SAPWeightDef]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SAPWeightDef](
	[ID] [int] NOT NULL,
	[PlantCode] [varchar](8) NOT NULL,
	[WeightUnit] [varchar](8) NOT NULL,
	[ConnectionStr] [varchar](255) NOT NULL,
	[DBName] [varchar](32) NOT NULL,
	[MsgType] [varchar](16) NOT NULL,
	[Descr] [varchar](128) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[VolumnUnit] [varchar](8) NULL,
 CONSTRAINT [PK_SAPWeightDef] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RunInTimeControlLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RunInTimeControlLog](
	[ID] [int] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Hour] [char](4) NOT NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[TestStation] [char](10) NULL,
	[ControlType] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RunInTimeControl]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RunInTimeControl](
	[ID] [int] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Hour] [char](4) NOT NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[TestStation] [char](10) NULL,
	[ControlType] [bit] NULL,
 CONSTRAINT [RunInTimeControl_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_ReleaseType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_ReleaseType](
	[ID] [int] NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[ReleaseType] [varchar](50) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [Rework_ReleaseType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_Product_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_Product_Part](
	[ID] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Value] [varchar](35) NOT NULL,
	[ValueType] [char](10) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Rework_Product_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_ProductStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_ProductStatus](
	[ID] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Rework_ProductStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_ProductInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_ProductInfo](
	[ID] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](35) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Rework_ProductInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_Product]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_Product](
	[ID] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [char](15) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[BIOS] [char](5) NULL,
	[IMGVER] [varchar](30) NULL,
	[WMAC] [char](12) NULL,
	[IMEI] [char](20) NULL,
	[MEID] [char](20) NULL,
	[ICCID] [char](20) NULL,
	[COAID] [char](20) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[HDVD] [varchar](50) NULL,
	[BLMAC] [char](12) NULL,
	[TVTuner] [varchar](200) NULL,
 CONSTRAINT [Rework_Product_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework_Process]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework_Process](
	[ReworkCode] [char](8) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Rework_Process_PK] PRIMARY KEY CLUSTERED 
(
	[ReworkCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReworkRejectStation]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReworkRejectStation](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NULL,
 CONSTRAINT [ReworkRejectStation_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rework]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rework](
	[ReworkCode] [char](8) NOT NULL,
	[Status] [char](1) NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Rework_PK] PRIMARY KEY CLUSTERED 
(
	[ReworkCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReturnRepair]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnRepair](
	[ID] [int] NOT NULL,
	[PCBRepairID] [int] NOT NULL,
	[ProductRepairID] [int] NOT NULL,
	[ProductRepairDefectID] [int] NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ReturnRepair_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Region](
	[Region] [varchar](50) NOT NULL,
	[Description] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [Region_PK] PRIMARY KEY CLUSTERED 
(
	[Region] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RefreshModel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RefreshModel](
	[Model] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
 CONSTRAINT [RefreshModel_PK] PRIMARY KEY CLUSTERED 
(
	[Model] ASC,
	[Editor] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RePrintLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RePrintLog](
	[ID] [int] NOT NULL,
	[LabelName] [varchar](64) NULL,
	[BegNo] [varchar](50) NOT NULL,
	[EndNo] [varchar](50) NOT NULL,
	[Descr] [varchar](80) NULL,
	[Reason] [nvarchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [RePrintLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RCTOMBMaintain]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[RCTOMBMaintain](
	[Code] [varchar](20) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Type] [char](1) NOT NULL,
	[Remark] [nvarchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QueryWipLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QueryWipLog](
	[Action] [varchar](50) NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QueryWIP]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QueryWIP](
	[ID] [int] NOT NULL,
	[DN] [varchar](32) NULL,
	[Model] [varchar](32) NULL,
	[Line] [varchar](32) NULL,
	[Qty] [int] NULL,
	[DNPAK] [int] NULL,
	[MVS] [int] NULL,
	[ITCND] [int] NULL,
	[COA] [int] NULL,
	[F0] [int] NULL,
	[36] [int] NULL,
	[37] [int] NULL,
	[39] [int] NULL,
	[3A] [int] NULL,
	[3D] [int] NULL,
	[3B] [int] NULL,
	[40] [int] NULL,
	[3C] [int] NULL,
	[3K] [int] NULL,
	[59] [int] NULL,
	[58] [int] NULL,
	[50] [int] NULL,
	[55] [int] NULL,
	[60] [int] NULL,
	[57] [int] NULL,
	[45] [int] NULL,
	[71] [int] NULL,
	[79A] [int] NULL,
	[64] [int] NULL,
	[65] [int] NULL,
	[79] [int] NULL,
	[73] [int] NULL,
	[6A] [int] NULL,
	[66E] [int] NULL,
	[66] [int] NULL,
	[76] [int] NULL,
	[67] [int] NULL,
	[69] [int] NULL,
	[81] [int] NULL,
	[3E] [int] NULL,
	[68] [int] NULL,
	[PK01] [int] NULL,
	[PK02] [int] NULL,
	[PK03] [int] NULL,
	[PK04] [int] NULL,
	[PK05] [int] NULL,
	[PKOK] [int] NULL,
	[8C] [int] NULL,
	[91] [int] NULL,
	[92] [int] NULL,
	[95] [int] NULL,
	[9U] [int] NULL,
	[80] [int] NULL,
	[85] [int] NULL,
	[86] [int] NULL,
	[96] [int] NULL,
	[97] [int] NULL,
	[9B] [int] NULL,
	[PO] [int] NULL,
	[7P] [int] NULL,
	[SP] [int] NULL,
	[9A] [int] NULL,
	[99] [int] NULL,
	[GUID] [varchar](64) NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QTime](
	[Line] [varchar](8) NOT NULL,
	[Station] [varchar](32) NOT NULL,
	[Family] [varchar](32) NOT NULL,
	[Category] [varchar](16) NOT NULL,
	[TimeOut] [int] NOT NULL,
	[StopTime] [int] NOT NULL,
	[DefectCode] [varchar](16) NOT NULL,
	[HoldStation] [varchar](32) NOT NULL,
	[HoldStatus] [int] NOT NULL,
	[ExceptStation] [varchar](64) NOT NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [QTime_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Station] ASC,
	[Family] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QCStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QCStatus](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Line] [char](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Remark] [char](1) NULL,
 CONSTRAINT [QCStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QCRatioLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QCRatioLog](
	[Family] [varchar](50) NULL,
	[QCRatio] [int] NULL,
	[EOQCRatio] [int] NULL,
	[PAQCRatio] [int] NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[RPAQCRatio] [int] NOT NULL,
	[Action] [varchar](10) NULL,
	[BackUpDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QCRatio]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QCRatio](
	[Family] [varchar](50) NOT NULL,
	[QCRatio] [int] NULL,
	[EOQCRatio] [int] NULL,
	[PAQCRatio] [int] NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[RPAQCRatio] [int] NOT NULL,
 CONSTRAINT [QCRatio_PK] PRIMARY KEY CLUSTERED 
(
	[Family] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product_yxf_20140605]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product_yxf_20140605](
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [varchar](35) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[State] [varchar](64) NULL,
	[OOAID] [char](20) NULL,
	[PRSN] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product_yxf_20140525]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product_yxf_20140525](
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [varchar](35) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[State] [varchar](64) NULL,
	[OOAID] [char](20) NULL,
	[PRSN] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product_Part_yxf_20140605]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product_Part_yxf_20140605](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product_Part](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL,
 CONSTRAINT [Product_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductTestLog_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductTestLog_DefectInfo](
	[ID] [int] NOT NULL,
	[ProductTestLogID] [int] NOT NULL,
	[DefectCodeID] [char](10) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ProductTestLog_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductTestLogBack_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductTestLogBack_DefectInfo](
	[ID] [int] NOT NULL,
	[ProductTestLogBackID] [int] NOT NULL,
	[DefectCodeID] [char](10) NULL,
	[TriggerAlarm] [bit] NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ProductTestLogBack_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductTestLogBack]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductTestLogBack](
	[ID] [int] NOT NULL,
	[Type] [char](10) NULL,
	[Line] [char](30) NULL,
	[Station] [char](10) NULL,
	[Status] [int] NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[ProductID] [char](10) NOT NULL,
 CONSTRAINT [ProductTestLogBack_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductTestLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductTestLog](
	[ID] [int] NOT NULL,
	[ActionName] [varchar](64) NULL,
	[Type] [char](10) NULL,
	[Line] [char](30) NULL,
	[FixtureID] [varchar](20) NULL,
	[Station] [char](10) NULL,
	[ErrorCode] [varchar](16) NULL,
	[Descr] [nvarchar](1024) NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[ProductID] [char](10) NOT NULL,
 CONSTRAINT [ProductTestLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductStatus_yxf_20140605]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatus_yxf_20140605](
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[Line] [char](30) NOT NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductStatus_yxf_20140525]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatus_yxf_20140525](
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[Line] [char](30) NOT NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductStatusEx]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatusEx](
	[ProductID] [char](10) NOT NULL,
	[PreStation] [char](10) NOT NULL,
	[PreStatus] [int] NOT NULL,
	[PreLine] [varchar](32) NOT NULL,
	[PreTestFailCount] [int] NOT NULL,
	[PreEditor] [varchar](32) NOT NULL,
	[PreUdt] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductStatusEx] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductStatus](
	[ProductID] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[ReworkCode] [char](8) NOT NULL,
	[Line] [char](30) NOT NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProductStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductRepair_DefectInfo_BackUp]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductRepair_DefectInfo_BackUp](
	[BackUpID] [int] NOT NULL,
	[BackUpProductID] [char](10) NOT NULL,
	[BackUpEditor] [varchar](30) NOT NULL,
	[BackUpCdt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
	[ProductRepairID] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
	[DefectCode] [char](10) NULL,
	[Cause] [char](10) NULL,
	[Obligation] [char](10) NULL,
	[Component] [char](10) NULL,
	[Site] [char](10) NULL,
	[Location] [varchar](50) NULL,
	[MajorPart] [char](10) NULL,
	[Remark] [nvarchar](100) NULL,
	[VendorCT] [varchar](30) NULL,
	[PartType] [varchar](50) NULL,
	[OldPart] [varchar](30) NULL,
	[OldPartSno] [varchar](50) NULL,
	[NewPart] [varchar](30) NULL,
	[NewPartSno] [varchar](50) NULL,
	[Manufacture] [varchar](30) NULL,
	[VersionA] [char](10) NULL,
	[VersionB] [char](10) NULL,
	[ReturnSign] [char](1) NOT NULL,
	[Mark] [char](1) NOT NULL,
	[SubDefect] [char](10) NULL,
	[PIAStation] [char](10) NULL,
	[Distribution] [char](10) NULL,
	[4M] [char](10) NULL,
	[Responsibility] [char](10) NULL,
	[Action] [varchar](50) NULL,
	[Cover] [char](10) NULL,
	[Uncover] [char](10) NULL,
	[TrackingStatus] [char](10) NULL,
	[IsManual] [int] NOT NULL,
	[MTAID] [varchar](14) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ReturnStn] [varchar](50) NULL,
 CONSTRAINT [PK_ProductRepair_DefectInfo_BackUp] PRIMARY KEY CLUSTERED 
(
	[BackUpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductRepair_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductRepair_DefectInfo](
	[ID] [int] NOT NULL,
	[ProductRepairID] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
	[DefectCode] [char](10) NULL,
	[Cause] [char](10) NULL,
	[Obligation] [char](10) NULL,
	[Component] [char](10) NULL,
	[Site] [char](10) NULL,
	[Location] [varchar](50) NULL,
	[MajorPart] [char](10) NULL,
	[Remark] [nvarchar](100) NULL,
	[VendorCT] [varchar](30) NULL,
	[PartType] [varchar](50) NULL,
	[OldPart] [varchar](30) NULL,
	[OldPartSno] [varchar](50) NULL,
	[NewPart] [varchar](30) NULL,
	[NewPartSno] [varchar](50) NULL,
	[Manufacture] [varchar](30) NULL,
	[VersionA] [char](10) NULL,
	[VersionB] [char](10) NULL,
	[ReturnSign] [char](1) NOT NULL,
	[Mark] [char](1) NOT NULL,
	[SubDefect] [char](10) NULL,
	[PIAStation] [char](10) NULL,
	[Distribution] [char](10) NULL,
	[4M] [char](10) NULL,
	[Responsibility] [char](10) NULL,
	[Action] [varchar](50) NULL,
	[Cover] [char](10) NULL,
	[Uncover] [char](10) NULL,
	[TrackingStatus] [char](10) NULL,
	[IsManual] [int] NOT NULL,
	[MTAID] [varchar](14) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ReturnStn] [varchar](50) NULL,
 CONSTRAINT [ProductRepair_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductRepair]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductRepair](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Type] [char](10) NULL,
	[Line] [char](30) NULL,
	[Station] [char](10) NULL,
	[TestLogID] [int] NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[LogID] [int] NULL,
 CONSTRAINT [ProductRepair_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductProcessTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductProcessTime](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[PreStation] [char](10) NULL,
	[Station] [char](10) NOT NULL,
	[PreStatus] [int] NULL,
	[Status] [int] NULL,
	[PreLine] [varchar](32) NULL,
	[Line] [varchar](32) NULL,
	[PreEditor] [varchar](32) NULL,
	[Editor] [varchar](32) NULL,
	[PreCdt] [datetime] NULL,
	[Cdt] [datetime] NULL,
	[TimeDiff] [int] NULL,
 CONSTRAINT [PK_ProductCycleTime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductPlanLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductPlanLog](
	[ID] [int] NOT NULL,
	[Action] [varchar](16) NOT NULL,
	[PdLine] [varchar](10) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Family] [varchar](64) NOT NULL,
	[Model] [varchar](32) NOT NULL,
	[PlanQty] [int] NOT NULL,
	[AddPrintQty] [int] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductPlanLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductPlan]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductPlan](
	[ID] [int] NOT NULL,
	[PdLine] [varchar](10) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Family] [varchar](64) NOT NULL,
	[Model] [varchar](32) NOT NULL,
	[PlanQty] [int] NOT NULL,
	[AddPrintQty] [int] NOT NULL,
	[PrePrintQty] [int] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_PlanInputUpload] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductLog](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Station] [char](50) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ProductLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductInfo](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[InfoType] [varchar](20) NULL,
	[InfoValue] [varchar](225) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProductInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductBT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductBT](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[BT] [varchar](50) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProductBT_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductAttrLog](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Station] [char](10) NOT NULL,
	[AttrName] [varchar](64) NOT NULL,
	[AttrOldValue] [nvarchar](1024) NOT NULL,
	[AttrNewValue] [nvarchar](1024) NOT NULL,
	[Descr] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ProductAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductAttr](
	[AttrName] [varchar](64) NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProductAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[PCBID] [char](15) NULL,
	[PCBModel] [char](12) NULL,
	[MAC] [char](15) NULL,
	[UUID] [varchar](35) NULL,
	[MBECR] [char](5) NULL,
	[CVSN] [varchar](35) NULL,
	[CUSTSN] [varchar](30) NULL,
	[ECR] [char](5) NULL,
	[PizzaID] [char](20) NULL,
	[MO] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[CartonSN] [char](20) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[DeliveryNo] [char](20) NULL,
	[PalletNo] [char](20) NULL,
	[State] [varchar](64) NULL,
	[OOAID] [char](20) NULL,
	[PRSN] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Product_PK] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Process_Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Process_Station](
	[ID] [int] NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[PreStation] [char](10) NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Process_Station_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProcessRuleset]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessRuleset](
	[ID] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Condition1] [varchar](50) NULL,
	[Condition2] [varchar](50) NULL,
	[Condition3] [varchar](50) NULL,
	[Condition4] [varchar](50) NULL,
	[Condition5] [varchar](50) NULL,
	[Condition6] [varchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProcessRuleset_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProcessRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessRule](
	[ID] [int] NOT NULL,
	[RuleSetID] [int] NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Value1] [varchar](200) NULL,
	[Value2] [varchar](200) NULL,
	[Value3] [varchar](200) NULL,
	[Value4] [varchar](200) NULL,
	[Value5] [varchar](200) NULL,
	[Value6] [varchar](200) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProcessRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProcessFlow]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessFlow](
	[ID] [int] NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[StationFlow] [text] NOT NULL,
 CONSTRAINT [ProcessFlow_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProcessAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessAttr](
	[AttrName] [varchar](64) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[AttrValue] [nvarchar](max) NOT NULL,
	[Descr] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ProcessAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[Process] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Process]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Process](
	[Process] [varchar](20) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Descr] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Process_PK] PRIMARY KEY CLUSTERED 
(
	[Process] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintTemplate]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintTemplate](
	[TemplateName] [varchar](50) NOT NULL,
	[LabelType] [varchar](50) NOT NULL,
	[Piece] [int] NOT NULL,
	[SpName] [varchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Layout] [int] NOT NULL,
 CONSTRAINT [PrintTemplate_PK] PRIMARY KEY CLUSTERED 
(
	[TemplateName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintLog](
	[ID] [int] NOT NULL,
	[Name] [varchar](64) NULL,
	[BegNo] [varchar](50) NOT NULL,
	[EndNo] [varchar](50) NOT NULL,
	[Descr] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[LabelTemplate] [varchar](64) NULL,
	[Station] [varchar](32) NULL,
 CONSTRAINT [PrintLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintList](
	[ID] [int] NOT NULL,
	[Doc_Name] [nvarchar](100) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PrintList_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PoPlt_EDI]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PoPlt_EDI](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PLT] [varchar](20) NOT NULL,
	[UCC] [varchar](50) NOT NULL,
	[QTY] [int] NOT NULL,
	[CombineQty] [int] NULL,
	[Consolidate] [varchar](16) NOT NULL,
	[ConQTY] [int] NOT NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PoPlt_EDI_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PoPlt_DOA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PoPlt_DOA](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PLT] [varchar](20) NOT NULL,
	[UCC] [varchar](50) NOT NULL,
	[QTY] [int] NOT NULL,
	[CombineQty] [int] NOT NULL,
	[Consolidate] [varchar](16) NULL,
	[ConQTY] [int] NOT NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PoPlt_DOA_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PoPlt]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PoPlt](
	[DeliveryNo] [char](20) NOT NULL,
	[PLT] [varchar](20) NOT NULL,
	[UCC] [varchar](50) NOT NULL,
	[QTY] [int] NOT NULL,
	[CombineQty] [int] NULL,
	[Consolidate] [varchar](16) NOT NULL,
	[ConQTY] [int] NOT NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [PoPlt_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PoData_EDI]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PoData_EDI](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PoNo] [varchar](40) NOT NULL,
	[Model] [char](12) NOT NULL,
	[ShipDate] [char](10) NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Descr] [nvarchar](3900) NOT NULL,
	[Editor] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PoData_EDI_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PoData]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PoData](
	[ID] [int] NOT NULL,
	[Shipment] [char](10) NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PoNo] [varchar](40) NOT NULL,
	[Model] [char](12) NOT NULL,
	[ShipDate] [char](10) NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Descr] [nvarchar](3900) NOT NULL,
	[Editor] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PoData_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pizza_Part_yxf_20140525]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pizza_Part_yxf_20140525](
	[ID] [int] NOT NULL,
	[PizzaID] [char](20) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](30) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pizza_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pizza_Part](
	[ID] [int] NOT NULL,
	[PizzaID] [char](20) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](30) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL,
 CONSTRAINT [Pizza_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PizzaStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PizzaStatus](
	[PizzaID] [char](20) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PizzaStatus_PK] PRIMARY KEY CLUSTERED 
(
	[PizzaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PizzaLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PizzaLog](
	[ID] [int] NOT NULL,
	[PizzaID] [varchar](20) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Station] [varchar](50) NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[Descr] [varchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PizzaLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pizza]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pizza](
	[PizzaID] [char](20) NOT NULL,
	[MMIID] [char](20) NULL,
	[Model] [varchar](20) NOT NULL,
	[CartonSN] [varchar](20) NOT NULL,
	[Remark] [varchar](64) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [Pizza_PK] PRIMARY KEY CLUSTERED 
(
	[PizzaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PilotRunPrintType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PilotRunPrintType](
	[ID] [int] NOT NULL,
	[Type] [char](20) NOT NULL,
 CONSTRAINT [PilotRunPrintType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PilotRunPrintInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PilotRunPrintInfo](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Build] [nchar](10) NOT NULL,
	[SKU] [nchar](10) NOT NULL,
	[Type] [char](20) NOT NULL,
	[Descr] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PilotRunPrintInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PilotRunPrintBuild]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PilotRunPrintBuild](
	[Build] [char](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PilotRunPrintBuild_PK] PRIMARY KEY CLUSTERED 
(
	[Build] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PickIDCtrl]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PickIDCtrl](
	[ID] [int] NOT NULL,
	[PickID] [char](10) NOT NULL,
	[TruckID] [nvarchar](50) NOT NULL,
	[Driver] [nvarchar](50) NOT NULL,
	[Dt] [char](10) NOT NULL,
	[Fwd] [varchar](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[InDt] [varchar](25) NOT NULL,
	[OutDt] [varchar](25) NOT NULL,
 CONSTRAINT [PickIDCtrl_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartType_Old]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartType_Old](
	[PartType] [varchar](50) NOT NULL,
	[PartTypeGroup] [varchar](50) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PartType_Old_PK] PRIMARY KEY CLUSTERED 
(
	[PartType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartTypeMapping]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartTypeMapping](
	[ID] [int] NOT NULL,
	[SAPType] [varchar](50) NOT NULL,
	[FISType] [varchar](50) NOT NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartTypeMapping_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartTypeDescription]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartTypeDescription](
	[ID] [int] NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PartTypeDescription_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartTypeAttribute]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartTypeAttribute](
	[PartType] [varchar](50) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PartTypeAttribute_PK] PRIMARY KEY CLUSTERED 
(
	[PartType] ASC,
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartType](
	[ID] [int] NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Code] [char](6) NOT NULL,
	[Indx] [char](1) NOT NULL,
	[Description] [char](50) NOT NULL,
	[Site] [varchar](50) NULL,
	[Cust] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartSN]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartSN](
	[IECSN] [varchar](30) NOT NULL,
	[IECPn] [varchar](20) NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[VendorSN] [varchar](30) NOT NULL,
	[DateCode] [char](10) NULL,
	[VendorDCode] [char](10) NULL,
	[VCode] [char](10) NULL,
	[151PartNo] [char](12) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartSN_PK] PRIMARY KEY CLUSTERED 
(
	[IECSN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartProcess]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartProcess](
	[MBFamily] [varchar](80) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[PilotRun] [varchar](1) NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartProcess_PK] PRIMARY KEY CLUSTERED 
(
	[MBFamily] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartInfo](
	[ID] [int] NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[InfoType] [varchar](50) NOT NULL,
	[InfoValue] [varchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartForbidden]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartForbidden](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Model] [char](12) NULL,
	[Descr] [varchar](50) NULL,
	[PartNo] [varchar](20) NULL,
	[AssemblyCode] [varchar](50) NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartForbidden_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartCheckSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartCheckSetting](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[Model] [varchar](20) NULL,
	[Station] [char](10) NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[ValueType] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartCheckSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartCheckMatchRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartCheckMatchRule](
	[ID] [int] NOT NULL,
	[PartCheckID] [int] NOT NULL,
	[RegExp] [varchar](255) NULL,
	[PnExp] [varchar](255) NULL,
	[PartPropertyExp] [varchar](255) NULL,
	[ContainCheckBit] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartCheckMatchRule_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartCheck]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartCheck](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[ValueType] [char](10) NOT NULL,
	[NeedSave] [int] NOT NULL,
	[NeedCheck] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PartCheck_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Part](
	[PartNo] [varchar](20) NOT NULL,
	[Descr] [varchar](80) NULL,
	[BomNodeType] [char](3) NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[CustPartNo] [varchar](20) NULL,
	[AutoDL] [char](1) NOT NULL,
	[Remark] [nvarchar](900) NULL,
	[Flag] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Part_PK] PRIMARY KEY CLUSTERED 
(
	[PartNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pallet_RFID]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pallet_RFID](
	[ID] [int] NOT NULL,
	[PLT] [char](10) NOT NULL,
	[RFIDCode] [char](10) NOT NULL,
	[Carrier] [varchar](25) NOT NULL,
	[Editor] [varchar](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Pallet_RFID_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pallet_FIS]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pallet_FIS](
	[PalletNo] [char](20) NOT NULL,
	[UCC] [char](30) NULL,
	[Station] [char](10) NOT NULL,
	[Weight] [decimal](10, 2) NULL,
	[PalletModel] [char](10) NULL,
	[Length] [decimal](10, 2) NULL,
	[Width] [decimal](10, 2) NULL,
	[Height] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Weight_L] [decimal](10, 2) NULL,
 CONSTRAINT [Pallet_FIS1_PK] PRIMARY KEY CLUSTERED 
(
	[PalletNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletWeight](
	[ID] [int] NOT NULL,
	[PalletType] [char](10) NOT NULL,
	[PalletWeight] [decimal](18, 3) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_PalletWeight] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletType](
	[ID] [int] NOT NULL,
	[ShipWay] [varchar](8) NOT NULL,
	[RegId] [varchar](16) NOT NULL,
	[Type] [nvarchar](32) NOT NULL,
	[StdPltFullQty] [varchar](8) NULL,
	[MinQty] [int] NOT NULL,
	[MaxQty] [int] NOT NULL,
	[Code] [varchar](8) NOT NULL,
	[PltWeight] [decimal](10, 2) NULL,
	[MinusPltWeight] [char](1) NOT NULL,
	[CheckCode] [varchar](8) NOT NULL,
	[ChepPallet] [char](1) NOT NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[PalletLayer] [int] NULL,
 CONSTRAINT [PalletType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletStandard]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletStandard](
	[ID] [int] NOT NULL,
	[FullQty] [nvarchar](32) NOT NULL,
	[TierQty] [int] NOT NULL,
	[MediumQty] [int] NOT NULL,
	[LitterQty] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PalletStatndard_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletProcess]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletProcess](
	[Customer] [char](10) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PalletProcess_PK] PRIMARY KEY CLUSTERED 
(
	[Customer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletLog](
	[ID] [int] NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PalletLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletId]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletId](
	[PalletNo] [varchar](20) NOT NULL,
	[PalletId] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PalletId_PK] PRIMARY KEY CLUSTERED 
(
	[PalletNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletAttrLog](
	[ID] [int] NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[PalletModel] [char](20) NOT NULL,
	[AttrName] [varchar](64) NOT NULL,
	[AttrOldValue] [nvarchar](1024) NOT NULL,
	[AttrNewValue] [nvarchar](1024) NOT NULL,
	[Descr] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PalletAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PalletAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PalletAttr](
	[AttrName] [varchar](64) NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PalletAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[PalletNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pallet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pallet](
	[PalletNo] [char](20) NOT NULL,
	[UCC] [char](30) NULL,
	[Station] [char](10) NOT NULL,
	[Weight] [decimal](10, 2) NULL,
	[PalletModel] [char](10) NULL,
	[Length] [decimal](10, 2) NULL,
	[Width] [decimal](10, 2) NULL,
	[Height] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Weight_L] [decimal](10, 2) NULL,
	[Floor] [varchar](8) NULL,
 CONSTRAINT [Pallet_PK] PRIMARY KEY CLUSTERED 
(
	[PalletNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Packinglist_RePrint]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packinglist_RePrint](
	[ID] [int] NOT NULL,
	[DN] [nvarchar](20) NULL,
	[ShipDate] [nvarchar](10) NULL,
	[Model] [nvarchar](20) NULL,
 CONSTRAINT [Packinglist_RePrint_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PO_Label]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PO_Label](
	[LabelType] [varchar](50) NOT NULL,
	[PO] [varchar](20) NOT NULL,
	[TemplateName] [varchar](50) NULL,
 CONSTRAINT [PO_Label_PK] PRIMARY KEY CLUSTERED 
(
	[LabelType] ASC,
	[PO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PODLabelPart]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PODLabelPart](
	[ID] [int] NOT NULL,
	[Family] [char](50) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Editor] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PODLabelPart_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PLTStandard]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PLTStandard](
	[PLTNO] [char](12) NOT NULL,
	[Len] [decimal](18, 1) NOT NULL,
	[Wide] [decimal](18, 1) NOT NULL,
	[High] [decimal](18, 1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [PLTStandard_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PLTSpecification]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PLTSpecification](
	[ID] [int] NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Descr] [nvarchar](255) NOT NULL,
	[Len] [decimal](18, 1) NOT NULL,
	[Wide] [decimal](18, 1) NOT NULL,
	[High] [decimal](18, 1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_PLTSpecification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCode_LabelType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCode_LabelType](
	[PCode] [char](10) NOT NULL,
	[LabelType] [varchar](50) NOT NULL,
 CONSTRAINT [Station_LabelType_PK] PRIMARY KEY CLUSTERED 
(
	[LabelType] ASC,
	[PCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCB_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCB_Part](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[IECPn] [varchar](20) NULL,
	[CustmerPn] [varchar](20) NULL,
	[PartSn] [varchar](50) NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BomNodeType] [char](3) NOT NULL,
	[CheckItemType] [varchar](20) NULL,
 CONSTRAINT [PCB_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBVersion]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[PCBVersion](
	[Family] [varchar](50) NOT NULL,
	[MBCode] [varchar](8) NOT NULL,
	[PCBVer] [varchar](8) NOT NULL,
	[CTVer] [varchar](8) NOT NULL,
	[Supplier] [varchar](8) NOT NULL,
	[Remark] [varchar](64) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCBVersion_PK] PRIMARY KEY CLUSTERED 
(
	[Family] ASC,
	[MBCode] ASC,
	[PCBVer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBTestLog_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBTestLog_DefectInfo](
	[ID] [int] NOT NULL,
	[PCBTestLogID] [int] NOT NULL,
	[DefectCodeID] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PCBTestLog_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBTestLogBack_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBTestLogBack_DefectInfo](
	[ID] [int] NOT NULL,
	[PCBTestLogBackID] [int] NOT NULL,
	[DefectCodeID] [char](10) NOT NULL,
	[TriggerAlarm] [bit] NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PCBTestLogBack_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBTestLogBack]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBTestLogBack](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Line] [char](30) NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PCBTestLogBack_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBTestLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBTestLog](
	[ID] [int] NOT NULL,
	[ActionName] [varchar](64) NULL,
	[PCBNo] [char](11) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Line] [char](30) NULL,
	[FixtureID] [varchar](20) NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[JoinID] [varchar](36) NULL,
	[Editor] [varchar](30) NOT NULL,
	[ErrorCode] [varchar](16) NULL,
	[Descr] [nvarchar](1024) NULL,
	[Cdt] [datetime] NOT NULL,
	[Remark] [varchar](255) NULL,
 CONSTRAINT [PCBTestLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBStatusEx]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBStatusEx](
	[PCBNo] [char](10) NOT NULL,
	[PreStation] [char](10) NOT NULL,
	[PreStatus] [int] NOT NULL,
	[PreLine] [varchar](32) NOT NULL,
	[PreTestFailCount] [int] NOT NULL,
	[PreEditor] [varchar](32) NOT NULL,
	[PreUdt] [datetime] NOT NULL,
 CONSTRAINT [PK_PCBStatusEx] PRIMARY KEY CLUSTERED 
(
	[PCBNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBStatus](
	[PCBNo] [char](11) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NULL,
	[TestFailCount] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCBStatus_PK] PRIMARY KEY CLUSTERED 
(
	[PCBNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBRepair_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBRepair_DefectInfo](
	[ID] [int] NOT NULL,
	[PCARepairID] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
	[DefectCode] [char](10) NULL,
	[Cause] [char](10) NULL,
	[Obligation] [char](10) NULL,
	[Component] [char](10) NULL,
	[Site] [char](10) NULL,
	[Location] [varchar](50) NULL,
	[MajorPart] [char](10) NULL,
	[Remark] [nvarchar](100) NULL,
	[VendorCT] [varchar](30) NULL,
	[PartType] [varchar](50) NULL,
	[OldPart] [varchar](30) NULL,
	[OldPartSno] [varchar](50) NULL,
	[NewPart] [varchar](30) NULL,
	[NewPartSno] [varchar](50) NULL,
	[NewPartDateCode] [char](5) NULL,
	[Manufacture] [varchar](30) NULL,
	[VersionA] [char](10) NULL,
	[VersionB] [char](10) NULL,
	[ReturnSign] [char](1) NOT NULL,
	[Mark] [char](1) NOT NULL,
	[Side] [char](10) NULL,
	[SubDefect] [char](10) NULL,
	[PIAStation] [char](10) NULL,
	[Distribution] [char](10) NULL,
	[4M] [char](10) NULL,
	[Responsibility] [char](10) NULL,
	[Action] [varchar](50) NULL,
	[Cover] [char](10) NULL,
	[Uncover] [char](10) NULL,
	[TrackingStatus] [char](10) NULL,
	[IsManual] [int] NOT NULL,
	[MTAID] [varchar](14) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCBRepair_DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBRepair]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBRepair](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[PCBModelID] [char](12) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[TestLogID] [int] NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[LogID] [int] NULL,
 CONSTRAINT [PCBRepair_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBProcessTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBProcessTime](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[PreStation] [char](10) NULL,
	[Station] [char](10) NOT NULL,
	[PreStatus] [int] NULL,
	[Status] [int] NULL,
	[PreLine] [varchar](32) NULL,
	[Line] [varchar](32) NULL,
	[PreEditor] [varchar](32) NULL,
	[Editor] [varchar](32) NULL,
	[PreCdt] [datetime] NULL,
	[Cdt] [datetime] NULL,
	[TimeDiff] [int] NULL,
 CONSTRAINT [PK_PCBProcessTime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBOQCRepair_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBOQCRepair_DefectInfo](
	[ID] [int] NOT NULL,
	[PCBOQCRepairID] [int] NOT NULL,
	[Defect] [char](10) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_PCBOQCRepair_DefectInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBOQCRepair]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBOQCRepair](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[LotNo] [char](12) NULL,
	[Station] [char](10) NULL,
	[Remark] [nvarchar](200) NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_PCBOQCRepair] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBLotCheck]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBLotCheck](
	[ID] [int] NOT NULL,
	[LotNo] [char](12) NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PCBLotCheck_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBLot]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBLot](
	[ID] [int] NOT NULL,
	[LotNo] [char](12) NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[Status] [char](1) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_PCBLot] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBLog](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[PCBModel] [char](12) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PCBLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBInfo](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[InfoType] [char](10) NOT NULL,
	[InfoValue] [varchar](80) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCBInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBAttrLog](
	[ID] [int] NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[PCBModelID] [char](12) NULL,
	[Station] [char](10) NULL,
	[AttrName] [varchar](64) NULL,
	[AttrOldValue] [nvarchar](1024) NULL,
	[AttrNewValue] [nvarchar](1024) NULL,
	[Descr] [nvarchar](1024) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PCBAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCBAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCBAttr](
	[AttrName] [varchar](64) NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCBAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[PCBNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCB]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCB](
	[PCBNo] [char](11) NOT NULL,
	[CUSTSN] [varchar](30) NULL,
	[MAC] [char](12) NULL,
	[UUID] [char](32) NULL,
	[ECR] [char](5) NULL,
	[IECVER] [char](5) NULL,
	[CUSTVER] [char](10) NULL,
	[SMTMO] [char](8) NULL,
	[PCBModelID] [char](12) NULL,
	[DateCode] [char](10) NULL,
	[CVSN] [char](35) NULL,
	[State] [varchar](64) NULL,
	[ShipMode] [varchar](64) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[PizzaID] [varchar](20) NULL,
	[CartonSN] [varchar](20) NULL,
	[DeliveryNo] [varchar](20) NULL,
	[PalletNo] [varchar](20) NULL,
	[QCStatus] [varchar](8) NULL,
	[SkuModel] [varchar](20) NULL,
 CONSTRAINT [PCB_PK] PRIMARY KEY CLUSTERED 
(
	[PCBNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCATest_Check]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCATest_Check](
	[ID] [int] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[MAC] [char](1) NOT NULL,
	[MBCT] [char](1) NOT NULL,
	[HDDV] [varchar](50) NULL,
	[BIOS] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PCATest_Check_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PCAICTCount]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PCAICTCount](
	[ID] [int] NOT NULL,
	[PdLine] [char](10) NULL,
	[Qty] [int] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PCAICTCount_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAQCSorting_Product]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAQCSorting_Product](
	[ID] [int] NOT NULL,
	[PAQCSortingID] [int] NOT NULL,
	[CUSTSN] [varchar](30) NOT NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_PAQCSorting_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAQCSorting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAQCSorting](
	[ID] [int] NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Status] [varchar](2) NOT NULL,
	[PreviousFailTime] [datetime] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_PAQCSorting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAKitLoc]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAKitLoc](
	[ID] [int] NOT NULL,
	[PdLine] [char](4) NOT NULL,
	[PartNo] [nvarchar](20) NOT NULL,
	[Descr] [nvarchar](20) NOT NULL,
	[Station] [nvarchar](30) NOT NULL,
	[Location] [varchar](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PAKitLoc_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAK_WH_LocMas]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAK_WH_LocMas](
	[ID] [int] NOT NULL,
	[Carrier] [nvarchar](20) NULL,
	[Col] [nchar](2) NULL,
	[Loc] [int] NULL,
	[BOL] [nvarchar](30) NULL,
	[PLT1] [nvarchar](14) NULL,
	[PLT2] [nvarchar](50) NULL,
	[Editor] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PAK_WH_LocMas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PAK_WHPLT_Type]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAK_WHPLT_Type](
	[ID] [int] NOT NULL,
	[Carrier] [nchar](10) NULL,
	[BOL] [nvarchar](30) NULL,
	[PLT] [nvarchar](14) NULL,
	[Tp] [nchar](2) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PAK_WHPLT_Type_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PAK_WHLoc_TMP]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAK_WHLoc_TMP](
	[ID] [int] NOT NULL,
	[LOC] [nchar](4) NULL,
	[BOL] [nvarchar](30) NULL,
	[PLT] [nvarchar](14) NULL,
	[Tp] [char](2) NULL,
	[Editor] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PAK_WHLoc_TMP_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAK_PizzaKittingBySt_FV]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAK_PizzaKittingBySt_FV](
	[ID] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Model] [nvarchar](14) NULL,
	[St] [nvarchar](50) NULL,
	[Descr] [nvarchar](50) NULL,
	[Qty] [int] NULL,
	[Pno] [nvarchar](20) NULL,
	[SPno] [nvarchar](1500) NULL,
	[Tp] [varchar](4) NULL,
	[Sqty] [int] NULL,
	[Remark] [nvarchar](1000) NULL,
	[Sub] [nvarchar](8) NULL,
	[Vct] [nvarchar](1000) NULL,
	[Bin] [char](3) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PAK_PizzaKittingBySt_FV_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAK_PackkingData_Del]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAK_PackkingData_Del](
	[ID] [int] NOT NULL,
	[InternalID] [nvarchar](20) NULL,
	[PALLET_ID] [nvarchar](20) NULL,
	[PROD_TYPE] [nvarchar](30) NULL,
	[SERIAL_NUM] [nvarchar](30) NULL,
	[BOX_ID] [nvarchar](30) NULL,
	[ACTUAL_SHIPDATE] [nvarchar](20) NULL,
	[TRACK_NO_PARCEL] [nvarchar](20) NULL,
	[Cdt] [datetime] NULL,
	[DelDt] [datetime] NULL,
 CONSTRAINT [PAK_PackkingData_Del_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PAK_PQCLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAK_PQCLog](
	[ID] [int] NOT NULL,
	[SnoId] [char](14) NOT NULL,
	[Pno] [char](12) NOT NULL,
	[PdLine] [char](30) NOT NULL,
	[WC] [char](2) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PAK_PQCLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAK_LocMas]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAK_LocMas](
	[ID] [int] NOT NULL,
	[FL] [char](2) NULL,
	[SnoId] [char](14) NOT NULL,
	[Pno] [char](16) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[PdLine] [char](12) NOT NULL,
	[WC] [varchar](16) NOT NULL,
	[Editor] [char](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PAK_LocMas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PAK_CHN_TW_Light]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAK_CHN_TW_Light](
	[ID] [int] NOT NULL,
	[Model] [nvarchar](50) NULL,
	[PartNo] [nvarchar](20) NULL,
	[Type] [nvarchar](50) NULL,
	[Descr] [nvarchar](50) NULL,
	[LightNo] [nchar](10) NULL,
	[Editor] [nchar](10) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PAK_CHN_TW_Light_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PAK_BTLocMas]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PAK_BTLocMas](
	[SnoId] [char](10) NULL,
	[Status] [char](10) NULL,
	[FL] [char](2) NULL,
	[PdLine] [char](12) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Model] [varchar](24) NOT NULL,
	[LocQty] [int] NOT NULL,
	[CmbQty] [int] NOT NULL,
	[Editor] [char](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [PAK_BTLocMas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OlymBattery]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OlymBattery](
	[ID] [int] NOT NULL,
	[HPPN] [char](12) NOT NULL,
	[HSSN] [char](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [OlymBattery_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfflineLabelSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfflineLabelSetting](
	[ID] [int] NOT NULL,
	[FileName] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[LabelSpec] [varchar](50) NOT NULL,
	[Param1] [varchar](50) NULL,
	[Param2] [varchar](50) NULL,
	[Param3] [varchar](50) NULL,
	[Param4] [varchar](50) NULL,
	[Param5] [varchar](50) NULL,
	[Param6] [varchar](50) NULL,
	[Param7] [varchar](50) NULL,
	[Param8] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [OfflineLabelSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OLD_ModelList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OLD_ModelList](
	[Model] [varchar](20) NOT NULL,
	[PlanDate] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NumControl_bck]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NumControl_bck](
	[ID] [int] NOT NULL,
	[NoType] [char](10) NOT NULL,
	[NoName] [varchar](25) NULL,
	[Value] [varchar](50) NULL,
	[Customer] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NumControl]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NumControl](
	[ID] [int] NOT NULL,
	[NoType] [char](10) NOT NULL,
	[NoName] [varchar](25) NULL,
	[Value] [varchar](50) NULL,
	[Customer] [char](10) NULL,
 CONSTRAINT [NumControl_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NEW_ModelList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NEW_ModelList](
	[Model] [varchar](12) NOT NULL,
	[PlanDate] [varchar](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Model_Process_TMP]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Model_Process_TMP](
	[ID] [int] NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Line] [varchar](30) NULL,
	[Process] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Model_Process_TMP_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Model_Process]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Model_Process](
	[ID] [int] NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Line] [varchar](30) NOT NULL,
 CONSTRAINT [Model_Process_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelWeightTolerance]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelWeightTolerance](
	[Model] [varchar](20) NOT NULL,
	[UnitTolerance] [char](10) NULL,
	[CartonTolerance] [char](10) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ModelWeightTolerance_PK] PRIMARY KEY CLUSTERED 
(
	[Model] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelWeight](
	[Model] [varchar](20) NOT NULL,
	[UnitWeight] [decimal](10, 3) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[SendStatus] [varchar](16) NULL,
	[Remark] [varchar](32) NULL,
 CONSTRAINT [ModelWeight_PK] PRIMARY KEY CLUSTERED 
(
	[Model] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelInfoName]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelInfoName](
	[ID] [int] NOT NULL,
	[Region] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ModelInfoName_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelInfo](
	[ID] [bigint] NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](200) NOT NULL,
	[Descr] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ModelInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelDefinition]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelDefinition](
	[ID] [int] NOT NULL,
	[KW] [varchar](20) NOT NULL,
	[Descr] [varchar](250) NOT NULL,
 CONSTRAINT [ModelDefinition_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelChangeQty]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelChangeQty](
	[ID] [int] NOT NULL,
	[Line] [varchar](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Qty] [int] NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[AssignedQty] [int] NOT NULL,
	[Status] [varchar](8) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ModelChangeQty_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ModelBOM]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ModelBOM](
	[ID] [int] NOT NULL,
	[Material] [varchar](255) NULL,
	[Plant] [varchar](255) NULL,
	[Component] [varchar](255) NULL,
	[Quantity] [varchar](255) NULL,
	[Alternative_item_group] [varchar](255) NULL,
	[Priority] [varchar](255) NULL,
	[Flag] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ModelBOM_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Model]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Model](
	[Model] [varchar](20) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[CustPN] [varchar](80) NULL,
	[Region] [varchar](50) NOT NULL,
	[ShipType] [varchar](30) NOT NULL,
	[Status] [int] NOT NULL,
	[OSCode] [varchar](50) NULL,
	[OSDesc] [varchar](50) NULL,
	[Price] [varchar](20) NULL,
	[BOMApproveDate] [datetime] NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Model_PK] PRIMARY KEY CLUSTERED 
(
	[Model] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MoUpload_OpenMO]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MoUpload_OpenMO](
	[Plant] [char](50) NOT NULL,
	[PdLine] [varchar](50) NOT NULL,
	[MO] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Qty] [int] NOT NULL,
	[CreateDate] [varchar](50) NOT NULL,
	[StartDate] [varchar](50) NOT NULL,
	[SAPStatus] [char](50) NULL,
	[SAPQty] [int] NOT NULL,
	[SO] [varchar](50) NULL,
	[SoItem] [varchar](50) NULL,
	[PO] [varchar](50) NULL,
	[PoItem] [varchar](50) NULL,
	[Editor] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MoUpload]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MoUpload](
	[Plant] [char](50) NOT NULL,
	[PdLine] [varchar](50) NOT NULL,
	[MO] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Qty] [int] NOT NULL,
	[CreateDate] [varchar](50) NOT NULL,
	[StartDate] [varchar](50) NOT NULL,
	[SAPStatus] [char](50) NULL,
	[SAPQty] [int] NOT NULL,
	[SO] [varchar](50) NULL,
	[SoItem] [varchar](50) NULL,
	[PO] [varchar](50) NULL,
	[PoItem] [varchar](50) NULL,
	[Editor] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MoPartInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MoPartInfo](
	[ID] [int] NOT NULL,
	[Mo] [varchar](20) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[InfoType] [varchar](50) NOT NULL,
	[InfoValue] [varchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MoPartInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MoPart]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MoPart](
	[Mo] [char](10) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Descr] [varchar](80) NULL,
	[BomNodeType] [char](3) NOT NULL,
	[PartType] [varchar](50) NOT NULL,
	[CustPartNo] [varchar](20) NULL,
	[AutoDL] [char](1) NOT NULL,
	[Remark] [varchar](80) NULL,
	[Flag] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MoPart_PK] PRIMARY KEY CLUSTERED 
(
	[PartNo] ASC,
	[Mo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MoBOM]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MoBOM](
	[ID] [int] NOT NULL,
	[Mo] [varchar](20) NOT NULL,
	[Material] [varchar](255) NULL,
	[Component] [varchar](20) NULL,
	[Quantity] [varchar](255) NULL,
	[Alternative_item_group] [varchar](255) NULL,
	[Priority] [varchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MoBOM_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Material_Process]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Material_Process](
	[ID] [int] NOT NULL,
	[MaterialType] [varchar](20) NOT NULL,
	[Process] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Material_Process_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialLot]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MaterialLot](
	[LotNo] [varchar](16) NOT NULL,
	[MaterialType] [varchar](16) NOT NULL,
	[SpecNo] [varchar](16) NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [varchar](16) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MaterialLot_PK] PRIMARY KEY CLUSTERED 
(
	[LotNo] ASC,
	[MaterialType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MaterialLog](
	[ID] [int] NOT NULL,
	[MaterialCT] [varchar](32) NOT NULL,
	[Action] [varchar](32) NOT NULL,
	[Stage] [varchar](16) NOT NULL,
	[Line] [varchar](16) NULL,
	[PreStatus] [varchar](16) NOT NULL,
	[Status] [varchar](16) NOT NULL,
	[Comment] [varchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [MaterialLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialBoxAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaterialBoxAttrLog](
	[ID] [int] NOT NULL,
	[BoxId] [varchar](16) NOT NULL,
	[Status] [varchar](16) NULL,
	[AttrName] [varchar](64) NULL,
	[AttrOldValue] [nvarchar](1024) NULL,
	[AttrNewValue] [nvarchar](1024) NULL,
	[Descr] [nvarchar](1024) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [MaterialBoxAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialBoxAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaterialBoxAttr](
	[BoxId] [varchar](16) NOT NULL,
	[AttrName] [varchar](64) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MaterialBoxAttr_PK] PRIMARY KEY CLUSTERED 
(
	[BoxId] ASC,
	[AttrName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialBox]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaterialBox](
	[BoxId] [varchar](16) NOT NULL,
	[MaterialType] [varchar](16) NOT NULL,
	[LotNo] [varchar](16) NULL,
	[SpecNo] [varchar](16) NULL,
	[FeedType] [varchar](5) NULL,
	[Revision] [varchar](16) NULL,
	[DateCode] [varchar](16) NULL,
	[Supplier] [varchar](16) NULL,
	[PartNo] [varchar](32) NULL,
	[Qty] [int] NOT NULL,
	[Status] [varchar](16) NOT NULL,
	[Model] [varchar](20) NULL,
	[Line] [varchar](20) NULL,
	[CartonSN] [varchar](20) NULL,
	[DeliveryNo] [varchar](20) NULL,
	[PalletNo] [varchar](20) NULL,
	[QcStatus] [varchar](8) NULL,
	[BoxWeight] [decimal](10, 2) NULL,
	[Comment] [varchar](64) NULL,
	[ShipMode] [varchar](64) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MaterialBox_PK] PRIMARY KEY CLUSTERED 
(
	[BoxId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaterialAttrLog](
	[ID] [int] NOT NULL,
	[MaterialCT] [varchar](32) NOT NULL,
	[Status] [varchar](16) NULL,
	[AttrName] [varchar](64) NULL,
	[AttrOldValue] [nvarchar](1024) NULL,
	[AttrNewValue] [nvarchar](1024) NULL,
	[Descr] [nvarchar](1024) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [MaterialAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MaterialAttr](
	[MaterialCT] [varchar](32) NOT NULL,
	[AttrName] [varchar](64) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MaterialAttr_PK] PRIMARY KEY CLUSTERED 
(
	[MaterialCT] ASC,
	[AttrName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Material]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Material](
	[MaterialCT] [varchar](32) NOT NULL,
	[MaterialType] [varchar](16) NOT NULL,
	[LotNo] [varchar](16) NULL,
	[Stage] [varchar](16) NULL,
	[Line] [varchar](16) NULL,
	[PreStatus] [varchar](16) NOT NULL,
	[Status] [varchar](16) NOT NULL,
	[Model] [varchar](20) NULL,
	[PizzaID] [varchar](20) NULL,
	[CartonSN] [varchar](20) NULL,
	[DeliveryNo] [varchar](20) NULL,
	[PalletNo] [varchar](20) NULL,
	[QcStatus] [varchar](8) NULL,
	[CartonWeight] [decimal](10, 2) NULL,
	[UnitWeight] [decimal](10, 2) NULL,
	[ShipMode] [varchar](64) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Material_PK] PRIMARY KEY CLUSTERED 
(
	[MaterialCT] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MasterLabel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MasterLabel](
	[ID] [int] NOT NULL,
	[VC] [char](6) NOT NULL,
	[Family] [varchar](25) NOT NULL,
	[Code] [varchar](25) NOT NULL,
	[Editor] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MasterLabel_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Maintain_MFGAlarm]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Maintain_MFGAlarm](
	[Station] [varchar](20) NOT NULL,
	[Defect Tp] [varchar](50) NOT NULL,
	[Defect FPF] [varchar](50) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Editor] [varchar](10) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Maintain_ITCNDefect_Check]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Maintain_ITCNDefect_Check](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Type] [varchar](5) NOT NULL,
	[Editor] [varchar](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Code] [char](20) NULL,
 CONSTRAINT [Maintain_ITCNDefect_Check_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MTA_SnoRep]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MTA_SnoRep](
	[ID] [int] NOT NULL,
	[Rep_Id] [int] NOT NULL,
	[Signal] [char](50) NOT NULL,
	[Vol] [decimal](5, 2) NULL,
	[Diod] [int] NULL,
	[Freq] [decimal](6, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MTA_Mark]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MTA_Mark](
	[Rep_Id] [int] NOT NULL,
	[Defect] [char](8) NULL,
	[Version] [char](10) NULL,
	[Mark] [char](1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MTA_Location]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MTA_Location](
	[ID] [int] NOT NULL,
	[Rep_Id] [int] NOT NULL,
	[Location] [char](20) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MP_BTOrder]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MP_BTOrder](
	[ID] [int] NOT NULL,
	[REF_DATE] [char](10) NOT NULL,
	[BT] [varchar](50) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Pno] [varchar](20) NOT NULL,
	[Qty] [int] NOT NULL,
	[PrtQty] [int] NOT NULL,
	[ShipDate] [char](10) NOT NULL,
	[OutQty] [int] NULL,
	[Remark] [varchar](200) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MP_BTOrder_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MO_Label]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MO_Label](
	[LabelType] [varchar](50) NOT NULL,
	[MO] [varchar](20) NOT NULL,
	[TemplateName] [varchar](50) NULL,
 CONSTRAINT [MO_Label_PK] PRIMARY KEY CLUSTERED 
(
	[LabelType] ASC,
	[MO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MO_Excel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MO_Excel](
	[ID] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[PrintQty] [int] NOT NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_MO_Excel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MOStatusLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MOStatusLog](
	[ID] [int] NOT NULL,
	[MO] [varchar](20) NOT NULL,
	[Function] [varchar](32) NULL,
	[Action] [varchar](32) NULL,
	[Station] [varchar](10) NULL,
	[PreStatus] [varchar](8) NULL,
	[Status] [varchar](8) NULL,
	[IsHold] [char](1) NULL,
	[HoldCode] [varchar](32) NULL,
	[TxnId] [varchar](32) NULL,
	[Comment] [varchar](255) NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PK_MOStatusLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MOStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MOStatus](
	[MO] [varchar](20) NOT NULL,
	[Status] [varchar](8) NULL,
	[IsHold] [char](1) NULL,
	[HoldCode] [varchar](32) NULL,
	[Comment] [varchar](255) NULL,
	[Editor] [varchar](32) NULL,
	[LastTxnId] [varchar](32) NULL,
	[Udt] [datetime] NULL,
	[FirstPrintDate] [datetime] NULL,
	[FirstRunDate] [datetime] NULL,
	[CloseDate] [datetime] NULL,
 CONSTRAINT [PK_MOStatus] PRIMARY KEY CLUSTERED 
(
	[MO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MODismantleLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MODismantleLog](
	[ID] [int] NOT NULL,
	[PCBNo] [char](10) NOT NULL,
	[SMTMO] [char](8) NOT NULL,
	[Reason] [varchar](80) NULL,
	[Tp] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [MODismantleLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MOData]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[MOData](
	[MO] [varchar](20) NOT NULL,
	[TxnId] [varchar](32) NULL,
	[MOType] [varchar](8) NULL,
	[Unit] [varchar](8) NULL,
	[FinishDate] [datetime] NULL,
	[ProductVer] [varchar](8) NULL,
	[Priority] [varchar](8) NULL,
	[BOMCategory] [varchar](8) NULL,
	[BOMExpDate] [datetime] NULL,
	[SalesOrder] [varchar](16) NULL,
	[SOItem] [varchar](8) NULL,
	[IsProduct] [char](1) NULL,
	[DataSource] [varchar](8) NULL,
	[TotalQty] [int] NOT NULL,
	[Editor] [varchar](32) NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_MOData] PRIMARY KEY CLUSTERED 
(
	[MO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MO]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MO](
	[MO] [varchar](20) NOT NULL,
	[Plant] [char](10) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[CreateDate] [smalldatetime] NOT NULL,
	[StartDate] [smalldatetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[SAPStatus] [char](10) NULL,
	[SAPQty] [int] NOT NULL,
	[Print_Qty] [int] NOT NULL,
	[Transfer_Qty] [int] NOT NULL,
	[Status] [char](1) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[CustomerSN_Qty] [int] NULL,
 CONSTRAINT [MO_PK] PRIMARY KEY CLUSTERED 
(
	[MO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MMIDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MMIDet](
	[ID] [int] NOT NULL,
 CONSTRAINT [MMIDet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MFG_AlarmDefect]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MFG_AlarmDefect](
	[Pno] [varchar](12) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[PdLine] [varchar](20) NOT NULL,
	[WC] [varchar](20) NULL,
	[Defect] [varchar](1000) NOT NULL,
	[Symptom] [varchar](8000) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Mark] [varchar](1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MB_Test]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MB_Test](
	[ID] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Family] [char](50) NULL,
	[Remark] [nvarchar](50) NULL,
	[Editor] [char](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[Type] [bit] NULL,
 CONSTRAINT [MB_Test_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MBSN]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MBSN](
	[PCBNo] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MBCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MBCode](
	[MBCode] [varchar](3) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[MultiQty] [smallint] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Type] [char](1) NOT NULL,
 CONSTRAINT [MBCode_PK] PRIMARY KEY CLUSTERED 
(
	[MBCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MBCFG_bck2014115]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MBCFG_bck2014115](
	[ID] [int] NOT NULL,
	[MBCode] [varchar](10) NOT NULL,
	[Series] [varchar](10) NOT NULL,
	[TP] [varchar](10) NOT NULL,
	[CFG] [varchar](10) NOT NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MBCFG]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MBCFG](
	[ID] [int] NOT NULL,
	[MBCode] [varchar](10) NOT NULL,
	[Series] [varchar](10) NOT NULL,
	[TP] [varchar](10) NOT NULL,
	[CFG] [varchar](10) NOT NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [MBCFG_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MAWBUpload]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MAWBUpload](
	[MAWB] [varchar](25) NULL,
	[Sno] [char](20) NULL,
	[Delivery] [varchar](20) NOT NULL,
	[item] [varchar](10) NOT NULL,
	[Dt1] [varchar](50) NULL,
	[Dt2] [varchar](50) NULL,
	[Cloum4] [varchar](50) NULL,
	[Description] [varchar](500) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MAWB]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MAWB](
	[ID] [int] NOT NULL,
	[MAWB] [varchar](25) NOT NULL,
	[Delivery] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [MAWB_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MACRange]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MACRange](
	[ID] [int] NOT NULL,
	[Code] [varchar](30) NOT NULL,
	[BegNo] [char](12) NOT NULL,
	[EndNo] [char](12) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [MACRange_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LotSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LotSetting](
	[ID] [int] NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[PassQty] [int] NOT NULL,
	[FailQty] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[CheckQty] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
 CONSTRAINT [PK_LotSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Lot]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Lot](
	[LotNo] [char](12) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](1) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[MBCode] [char](3) NOT NULL,
 CONSTRAINT [PK_Lot] PRIMARY KEY CLUSTERED 
(
	[LotNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[List_NewModel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[List_NewModel](
	[Family] [varchar](30) NULL,
	[Model] [varchar](12) NULL,
	[PlanDate] [varchar](10) NULL,
	[DateFIS] [varchar](50) NULL,
	[EditorFIS] [varchar](20) NULL,
	[CdtFIS] [datetime] NULL,
	[DateDMI] [varchar](50) NULL,
	[EditorDMI] [varchar](20) NULL,
	[CdtDMI] [datetime] NULL,
	[DateIMG] [varchar](50) NULL,
	[EditorIMG] [varchar](20) NULL,
	[CdtIMG] [datetime] NULL,
	[Status] [varchar](10) NULL,
	[OpenStatus] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Line_Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Line_Station](
	[ID] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Line_Station_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineStationStopPeriodLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LineStationStopPeriodLog](
	[ID] [int] NOT NULL,
	[Line] [varchar](8) NOT NULL,
	[Station] [varchar](32) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NULL,
 CONSTRAINT [LineStationStopPeriodLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineStationLastProcessTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LineStationLastProcessTime](
	[Line] [varchar](8) NOT NULL,
	[Station] [varchar](32) NOT NULL,
	[ProductID] [varchar](32) NOT NULL,
	[ProcessTime] [datetime] NOT NULL,
	[Editor] [varchar](32) NULL,
 CONSTRAINT [LineStationLastProcessTime_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Station] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineSpeed]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[LineSpeed](
	[Station] [varchar](10) NOT NULL,
	[AliasLine] [varchar](30) NOT NULL,
	[LimitSpeed] [int] NOT NULL,
	[IsCheckPass] [varchar](4) NOT NULL,
	[LimitSpeedExpression] [varchar](4) NOT NULL,
	[IsHoldStation] [varchar](4) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_LineSpeed] PRIMARY KEY CLUSTERED 
(
	[Station] ASC,
	[AliasLine] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineQty]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LineQty](
	[Line] [varchar](50) NULL,
	[Qty] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineEx]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[LineEx](
	[Line] [varchar](30) NOT NULL,
	[AliasLine] [varchar](30) NOT NULL,
	[AvgSpeed] [int] NOT NULL,
	[AvgManPower] [int] NOT NULL,
	[AvgStationQty] [int] NOT NULL,
	[Owner] [varchar](64) NOT NULL,
	[IEOwner] [varchar](64) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_LineEx] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Line]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Line](
	[Line] [char](30) NOT NULL,
	[CustomerID] [char](10) NOT NULL,
	[Stage] [char](10) NULL,
	[Descr] [nvarchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Line_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LabelTypeRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LabelTypeRule](
	[LabelType] [varchar](50) NOT NULL,
	[Station] [varchar](255) NOT NULL,
	[Family] [varchar](255) NOT NULL,
	[Model] [varchar](255) NOT NULL,
	[ModelConstValue] [varchar](64) NOT NULL,
	[BomLevel] [int] NOT NULL,
	[PartNo] [varchar](255) NOT NULL,
	[BomNodeType] [varchar](16) NOT NULL,
	[PartDescr] [varchar](255) NOT NULL,
	[PartType] [varchar](255) NOT NULL,
	[PartConstValue] [varchar](64) NOT NULL,
	[Remark] [nvarchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[DeliveryConstValue] [varchar](64) NOT NULL,
 CONSTRAINT [LabelTypeRule_PK] PRIMARY KEY CLUSTERED 
(
	[LabelType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LabelType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LabelType](
	[LabelType] [varchar](50) NOT NULL,
	[PrintMode] [int] NOT NULL,
	[RuleMode] [int] NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [LabelType_PK] PRIMARY KEY CLUSTERED 
(
	[LabelType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LabelRuleSet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LabelRuleSet](
	[ID] [int] NOT NULL,
	[RuleID] [int] NULL,
	[AttributeName] [varchar](50) NULL,
	[AttributeValue] [varchar](200) NULL,
	[Mode] [char](1) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [LabelRuleSet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LabelRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LabelRule](
	[RuleID] [int] NOT NULL,
	[TemplateName] [varchar](50) NULL,
 CONSTRAINT [LabelRule_PK] PRIMARY KEY CLUSTERED 
(
	[RuleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LabelKitting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LabelKitting](
	[ID] [int] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Type] [varchar](15) NOT NULL,
	[Descr] [varchar](50) NOT NULL,
	[Remark] [nvarchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [LabelKitting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LCMBind]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LCMBind](
	[ID] [int] NOT NULL,
	[LCMSno] [varchar](20) NULL,
	[MESno] [varchar](50) NULL,
	[METype] [varchar](5) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [LCMBind_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FV]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FV](
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[MGroup] [int] NOT NULL,
 CONSTRAINT [Kitting_Location_FV_PK] PRIMARY KEY CLUSTERED 
(
	[MGroup] ASC,
	[RackID] ASC,
	[GateWayIP] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_Z]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_Z](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_Z_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_Y]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_Y](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_Y_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_X]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_X](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_X_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_W]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_W](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_W_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_V]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_V](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_V_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_U]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_U](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_U_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_T]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_T](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_T_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_S]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_S](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_S_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_R]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_R](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_R_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_Q]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_Q](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_Q_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_P]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_P](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_P_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_O]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_O](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_O_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_N]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_N](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_N_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_M]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_M](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_M_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_L]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_L](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_L_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_K]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_K](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_K_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_J]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_J](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_J_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_I]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_I](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_I_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_H]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_H](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_H_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_G]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_G](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_G_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_F]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_F](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_F_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_E]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_E](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_E_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_D]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_D](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_D_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_C]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_C](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_C_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_B]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_B](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_B_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location_FA_A]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location_FA_A](
	[ID] [int] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[MGroup] [int] NOT NULL,
	[Group] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[Editor] [varchar](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Kitting_Location_FA_A_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Location]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Kitting_Location](
	[TagID] [nchar](12) NOT NULL,
	[TagTp] [nchar](4) NOT NULL,
	[GateWayIP] [smallint] NOT NULL,
	[GateWayPort] [int] NOT NULL,
	[RackID] [smallint] NOT NULL,
	[ConfigedLEDStatus] [bit] NOT NULL,
	[ConfigedLEDBlock] [smallint] NOT NULL,
	[ConfigedDate] [datetime] NOT NULL,
	[Comm] [bit] NOT NULL,
	[RunningLEDStatus] [bit] NOT NULL,
	[RunningLEDBlock] [smallint] NOT NULL,
	[RunningDate] [datetime] NOT NULL,
	[LEDValues] [nchar](10) NULL,
	[TagDescr] [varchar](200) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_Kitting_Location_1] PRIMARY KEY NONCLUSTERED 
(
	[GateWayIP] ASC,
	[RackID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Kitting_Loc_PLMapping_St]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kitting_Loc_PLMapping_St](
	[ID] [int] NOT NULL,
	[PdLine] [nchar](10) NOT NULL,
	[LightNo] [smallint] NOT NULL,
	[Station] [nvarchar](20) NOT NULL,
	[TagID] [nchar](12) NOT NULL,
 CONSTRAINT [Kitting_Loc_PLMapping_St_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kitting_Loc_PLMapping]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kitting_Loc_PLMapping](
	[ID] [int] NOT NULL,
	[PdLine] [nchar](10) NOT NULL,
	[LightNo] [smallint] NOT NULL,
	[TagID] [nchar](12) NOT NULL,
 CONSTRAINT [Kitting_Loc_PLMapping_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KittingCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KittingCode](
	[Code] [char](30) NOT NULL,
	[Type] [char](15) NOT NULL,
	[Descr] [char](50) NOT NULL,
	[Remark] [char](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [KittingCode_PK] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[KitLoc]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KitLoc](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[PartType] [varchar](20) NOT NULL,
	[PdLine] [char](4) NOT NULL,
	[Location] [varchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [KitLoc_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[KeyParts_Defect_RPT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[KeyParts_Defect_RPT](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NULL,
	[MajorPart] [nchar](10) NULL,
	[Cdt] [datetime] NULL,
	[Line] [nchar](10) NULL,
	[CTNo] [varchar](50) NULL,
	[Voder] [varchar](30) NULL,
	[Remark] [nvarchar](100) NULL,
	[RRmark] [nvarchar](100) NULL,
	[DeLoc] [varchar](10) NULL,
	[Editor] [varchar](20) NULL,
	[PartNo] [varchar](20) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[KeyPartRepair_DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[KeyPartRepair_DefectInfo](
	[ID] [int] NOT NULL,
	[KeyPartRepairID] [int] NOT NULL,
	[Type] [char](10) NOT NULL,
	[DefectCode] [char](10) NOT NULL,
	[Cause] [char](10) NULL,
	[Obligation] [char](10) NULL,
	[Component] [char](10) NULL,
	[Site] [char](10) NULL,
	[Location] [varchar](50) NULL,
	[MajorPart] [char](10) NULL,
	[Remark] [nvarchar](100) NULL,
	[VendorCT] [varchar](30) NULL,
	[PartType] [varchar](30) NULL,
	[OldPart] [varchar](30) NULL,
	[OldPartSno] [varchar](30) NULL,
	[NewPart] [varchar](30) NULL,
	[NewPartSno] [varchar](50) NULL,
	[Manufacture] [varchar](30) NULL,
	[VersionA] [char](30) NULL,
	[VersionB] [char](30) NULL,
	[ReturnSign] [char](1) NOT NULL,
	[Mark] [char](1) NOT NULL,
	[SubDefect] [char](10) NULL,
	[PIAStation] [char](10) NULL,
	[Distribution] [char](10) NULL,
	[4M] [char](10) NULL,
	[Responsibility] [char](10) NULL,
	[Action] [varchar](50) NULL,
	[Cover] [char](10) NULL,
	[Uncover] [char](10) NULL,
	[TrackingStatus] [char](10) NULL,
	[IsManual] [int] NOT NULL,
	[MTAID] [varchar](14) NULL,
	[ReturnStn] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_KeyPartRepair_DefectInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[KeyPartRepair]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[KeyPartRepair](
	[ID] [int] NOT NULL,
	[ProductID] [char](20) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NULL,
	[TestLogID] [int] NULL,
	[LogID] [int] NULL,
	[Status] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_KeyPartRepair] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IqcUpload]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IqcUpload](
	[MpDate] [nvarchar](50) NULL,
	[SSn] [nvarchar](50) NULL,
	[MSn] [nvarchar](50) NULL,
	[Spec] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[MpFact] [nvarchar](50) NULL,
	[MpLine] [nvarchar](50) NULL,
	[MpNum] [nvarchar](2000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IqcPnoBom]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IqcPnoBom](
	[ID] [int] NOT NULL,
	[SPS] [varchar](20) NULL,
	[Pno] [char](12) NOT NULL,
	[VC] [varchar](20) NOT NULL,
	[Descr] [varchar](50) NOT NULL,
	[Vendor] [varchar](40) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [IqcPnoBom_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IqcKp]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IqcKp](
	[ID] [int] NOT NULL,
	[CtLabel] [varchar](20) NULL,
	[Model] [varchar](10) NULL,
	[Tp] [varchar](10) NULL,
	[Defect] [char](4) NULL,
	[Cause] [char](4) NULL,
	[Location] [varchar](50) NULL,
	[Obligation] [varchar](50) NULL,
	[Remark] [nvarchar](500) NULL,
	[Result] [varchar](500) NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [IqcKp_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IqcCause1]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IqcCause1](
	[ID] [int] NOT NULL,
	[CtLabel] [char](20) NULL,
	[MpDefect] [char](4) NULL,
	[IqcDefect] [char](4) NULL,
	[VeLabel] [varchar](10) NULL,
	[VeDefect] [char](10) NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[IqcCause1] ADD [Status] [char](1) NULL
ALTER TABLE [dbo].[IqcCause1] ADD  CONSTRAINT [IqcCause1_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Iqc]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Iqc](
	[Id] [int] NOT NULL,
	[SSn] [char](10) NOT NULL,
	[MSn] [char](18) NOT NULL,
	[Type] [varchar](50) NULL,
	[Vendor] [varchar](50) NULL,
	[ReturnType] [varchar](200) NULL,
	[Spec] [varchar](80) NULL,
	[Model] [varchar](50) NULL,
	[MpFact] [char](4) NULL,
	[MpLine] [char](4) NULL,
	[MpNum] [int] NULL,
	[MpEditor] [varchar](10) NULL,
	[Code] [char](3) NULL,
	[MpDate] [datetime] NULL,
	[IqcDate] [datetime] NULL,
	[IqcNum] [int] NULL,
	[IqcErrorNum] [int] NULL,
	[IqcNoDo] [int] NULL,
	[NotGood] [char](5) NULL,
	[IqcGoodNum] [int] NULL,
	[MpNoGood] [int] NULL,
	[MpEff] [char](5) NULL,
	[IqcGoNum] [int] NULL,
	[IqcOverTime] [datetime] NULL,
	[IqcEditor] [char](10) NULL,
	[IqcRemark] [varchar](8000) NULL,
	[CauseTp] [char](1) NULL,
	[MpCause] [varchar](500) NULL,
	[IqcCause] [varchar](500) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InternalCOA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InternalCOA](
	[ID] [int] NOT NULL,
	[Code] [varchar](20) NULL,
	[Type] [char](2) NULL,
	[Model] [char](16) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [InternalCOA_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IndonesiaLabel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IndonesiaLabel](
	[ID] [int] NOT NULL,
	[SKU] [varchar](32) NOT NULL,
	[Descr] [varchar](64) NOT NULL,
	[Family] [varchar](64) NOT NULL,
	[ApprovalNo] [varchar](64) NOT NULL,
	[Editor] [varchar](16) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_IndonesiaLabel] PRIMARY KEY CLUSTERED 
(
	[SKU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITCND_IP_Log]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITCND_IP_Log](
	[Sno] [varchar](20) NULL,
	[Remark] [varchar](1000) NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITCND_DCDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITCND_DCDet](
	[ID] [int] NOT NULL,
	[SnoId] [char](14) NULL,
	[DC] [nvarchar](2500) NULL,
	[Editor] [char](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [ITCND_DCDet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITCNDCheckSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITCNDCheckSetting](
	[ID] [int] NOT NULL,
	[Line] [varchar](50) NOT NULL,
	[Station] [varchar](50) NOT NULL,
	[CheckItem] [varchar](50) NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[CheckType] [varchar](10) NOT NULL,
	[CheckCondition] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
 CONSTRAINT [ITCNDCheckSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ITCNDCheckQCHold]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ITCNDCheckQCHold](
	[Code] [varchar](20) NOT NULL,
	[IsHold] [char](1) NOT NULL,
	[Descr] [nvarchar](200) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_ITCNDCheckQCHold] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IQDC_Log]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IQDC_Log](
	[FileName] [char](10) NOT NULL,
	[Count] [int] NOT NULL,
	[Dt] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IQDC_Dump]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IQDC_Dump](
	[inv_date] [varchar](10) NOT NULL,
	[sfcnumber] [varchar](12) NOT NULL,
	[PartNumber] [varchar](20) NULL,
	[buildshift] [varchar](11) NULL,
	[sft_cdate] [char](8) NULL,
	[sft_sdate] [char](8) NULL,
	[startdate] [varchar](14) NULL,
	[completedate] [varchar](14) NULL,
	[visit_number] [varchar](1) NULL,
	[opershift] [varchar](11) NULL,
	[operdate] [varchar](14) NULL,
	[operation] [varchar](8) NULL,
	[nccode] [varchar](30) NULL,
	[resrce] [varchar](20) NULL,
	[oper_sdate] [varchar](14) NULL,
	[operator] [varchar](1) NULL,
	[ncgroup] [varchar](30) NULL,
	[sympcode] [varchar](30) NULL,
	[rtime_fail] [varchar](1) NULL,
	[nccomment1] [varchar](64) NULL,
	[nccomment2] [varchar](64) NULL,
	[nccomment3] [varchar](64) NULL,
	[repaircode] [varchar](20) NULL,
	[sa_serialnumber] [varchar](14) NULL,
	[sa_partnumber] [varchar](30) NULL,
	[rccode] [varchar](1) NULL,
	[sa_quantity] [varchar](1) NULL,
	[sa_datecode] [varchar](2) NULL,
	[sa_week_mfg] [varchar](8) NULL,
	[sa_type] [varchar](1) NULL,
	[assly_code] [varchar](4) NULL,
	[revision] [varchar](2) NULL,
	[supplier] [varchar](2) NULL,
	[status] [varchar](8) NULL,
	[flag] [varchar](1) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IQDC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IQDC](
	[inv_date] [varchar](10) NOT NULL,
	[sfcnumber] [varchar](12) NOT NULL,
	[PartNumber] [varchar](20) NULL,
	[buildshift] [varchar](11) NULL,
	[sft_cdate] [char](8) NULL,
	[sft_sdate] [char](8) NULL,
	[startdate] [varchar](14) NULL,
	[completedate] [varchar](14) NULL,
	[visit_number] [varchar](1) NULL,
	[opershift] [varchar](11) NULL,
	[operdate] [varchar](14) NULL,
	[operation] [varchar](10) NULL,
	[nccode] [varchar](30) NULL,
	[resrce] [varchar](20) NULL,
	[oper_sdate] [varchar](14) NULL,
	[operator] [varchar](1) NULL,
	[ncgroup] [varchar](30) NULL,
	[sympcode] [varchar](30) NULL,
	[rtime_fail] [varchar](1) NULL,
	[nccomment1] [varchar](64) NULL,
	[nccomment2] [varchar](64) NULL,
	[nccomment3] [varchar](64) NULL,
	[repaircode] [varchar](20) NULL,
	[sa_serialnumber] [varchar](14) NULL,
	[sa_partnumber] [varchar](30) NULL,
	[rccode] [varchar](1) NULL,
	[sa_quantity] [varchar](1) NULL,
	[sa_datecode] [varchar](2) NULL,
	[sa_week_mfg] [varchar](8) NULL,
	[sa_type] [varchar](1) NULL,
	[assly_code] [varchar](4) NULL,
	[revision] [varchar](2) NULL,
	[supplier] [varchar](2) NULL,
	[status] [varchar](8) NULL,
	[flag] [varchar](1) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IMESJobSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IMESJobSetting](
	[JobName] [varchar](64) NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_IMESJobSetting] PRIMARY KEY CLUSTERED 
(
	[JobName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ICASA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ICASA](
	[ID] [int] NOT NULL,
	[VC] [char](5) NOT NULL,
	[AV] [char](12) NOT NULL,
	[Antel1] [varchar](25) NOT NULL,
	[Antel2] [varchar](25) NOT NULL,
	[ICASA] [varchar](25) NOT NULL,
	[Editor] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ICASA_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HoldRule_20140321]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HoldRule_20140321](
	[ID] [int] NOT NULL,
	[Line] [varchar](16) NULL,
	[Family] [varchar](32) NULL,
	[Model] [varchar](32) NULL,
	[CUSTSN] [varchar](32) NULL,
	[HoldStation] [varchar](32) NULL,
	[CheckInStation] [varchar](32) NULL,
	[HoldCode] [varchar](32) NOT NULL,
	[HoldDescr] [nvarchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HoldRuleLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HoldRuleLog](
	[ID] [int] NOT NULL,
	[Line] [varchar](16) NULL,
	[Family] [varchar](32) NULL,
	[Model] [varchar](32) NULL,
	[CUSTSN] [varchar](32) NULL,
	[HoldStation] [varchar](32) NULL,
	[CheckInStation] [varchar](32) NULL,
	[HoldCode] [varchar](32) NOT NULL,
	[HoldDescr] [nvarchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BackUpDate] [datetime] NOT NULL,
	[Action] [varchar](16) NOT NULL,
 CONSTRAINT [PK_HoldRuleLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HoldRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HoldRule](
	[ID] [int] NOT NULL,
	[Line] [varchar](16) NULL,
	[Family] [varchar](32) NULL,
	[Model] [varchar](32) NULL,
	[CUSTSN] [varchar](32) NULL,
	[HoldStation] [varchar](32) NULL,
	[CheckInStation] [varchar](32) NULL,
	[HoldCode] [varchar](32) NOT NULL,
	[HoldDescr] [nvarchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [HoldRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HP_WWANLabel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HP_WWANLabel](
	[ID] [int] NOT NULL,
	[ModuleNo] [varchar](20) NOT NULL,
	[Descr] [varchar](100) NOT NULL,
	[LabelMEID] [varchar](10) NULL,
	[LabelIMEI] [varchar](10) NULL,
	[LabelICCID] [varchar](10) NULL,
	[LabelESn] [varchar](10) NULL,
	[PrintType] [varchar](10) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_HP_WWANLabel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HP_WPTR_FLAG]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HP_WPTR_FLAG](
	[FLAG] [smallint] NOT NULL,
	[ShipDate] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HP_PTR_File]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HP_PTR_File](
	[SnoId] [char](10) NOT NULL,
	[Sno] [char](14) NOT NULL,
	[ShipDate] [char](10) NOT NULL,
	[Descr] [nvarchar](3900) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HP_PTR]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HP_PTR](
	[SnoId] [char](10) NOT NULL,
	[Sno] [char](14) NOT NULL,
	[Descr] [nvarchar](3900) NOT NULL,
	[Index] [int] NOT NULL,
	[Uploaded] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HP_Grade]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HP_Grade](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NULL,
	[Series] [nvarchar](30) NULL,
	[Grade] [nvarchar](20) NULL,
	[Energia] [varchar](10) NULL,
	[Espera] [varchar](10) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [HP_Grade_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HPWeekCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HPWeekCode](
	[ID] [int] NOT NULL,
	[Code] [char](20) NULL,
	[Descr] [char](50) NULL,
	[Remark] [char](255) NULL,
	[Editor] [char](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [HPWeekCode_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HPSMT_Log]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HPSMT_Log](
	[HPSMTSN] [varchar](30) NOT NULL,
	[PCBModel] [varchar](30) NOT NULL,
	[Station] [varchar](20) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [varchar](50) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HPIMES_OSWIN]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HPIMES_OSWIN](
	[ID] [int] NOT NULL,
	[Family] [varchar](64) NOT NULL,
	[Zmode] [varchar](64) NOT NULL,
	[OS] [varchar](64) NULL,
	[AV] [varchar](64) NULL,
	[Image] [varchar](64) NULL,
	[Editor] [varchar](32) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [OSWIN_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HDDCopyInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HDDCopyInfo](
	[ID] [int] NOT NULL,
	[CopyMachineID] [char](10) NOT NULL,
	[ConnectorID] [char](10) NOT NULL,
	[SourceHDD] [char](15) NOT NULL,
	[HDDNo] [char](15) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [HDDCopyInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HDCPKey]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HDCPKey](
	[KSV] [varchar](10) NOT NULL,
	[HDCPKey] [varbinary](472) NOT NULL,
	[Status] [char](1) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_HDCPKey_NEW] PRIMARY KEY NONCLUSTERED 
(
	[KSV] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GoodReadinessReport]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GoodReadinessReport](
	[DT] [varchar](10) NULL,
	[WK] [varchar](10) NULL,
	[ODM] [varchar](10) NULL,
	[DeliveryNo] [varchar](16) NULL,
	[ShipWay] [varchar](20) NULL,
	[ShipModel] [varchar](20) NULL,
	[RegId] [varchar](20) NULL,
	[Carrier] [varchar](20) NULL,
	[AWB] [varchar](30) NULL,
	[GoodTime] [varchar](20) NULL,
	[TimeofReadiness] [varchar](30) NULL,
	[GoodHour] [varchar](2) NULL,
	[Flag] [varchar](20) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FwdPlt]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FwdPlt](
	[PickID] [varchar](15) NULL,
	[Plt] [varchar](15) NULL,
	[Qty] [int] NULL,
	[Status] [varchar](5) NULL,
	[Date] [varchar](15) NULL,
	[Operator] [varchar](15) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [FwdPlt_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FwdNotice]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FwdNotice](
	[TYPE] [varchar](10) NULL,
	[Family] [varchar](25) NULL,
	[ShipDate] [char](10) NULL,
	[Week] [nvarchar](2) NULL,
	[ODM] [nvarchar](8) NULL,
	[ShipMode] [char](5) NULL,
	[RegId] [char](4) NULL,
	[FWD] [char](10) NULL,
	[BOL] [varchar](25) NULL,
	[Qty] [int] NULL,
	[Status] [char](19) NULL,
	[Time] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FruDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FruDet](
	[ID] [int] NOT NULL,
	[SnoId] [char](14) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Sno] [char](35) NOT NULL,
	[Editor] [char](25) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [FruDet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Forwarder]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Forwarder](
	[ID] [int] NOT NULL,
	[Forwarder] [varchar](50) NOT NULL,
	[Date] [varchar](10) NOT NULL,
	[MAWB] [varchar](25) NOT NULL,
	[Driver] [nvarchar](50) NOT NULL,
	[TruckID] [nvarchar](50) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[ContainerId] [varchar](64) NULL,
 CONSTRAINT [Forwarder_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ForceNWC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ForceNWC](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NULL,
	[PreStation] [char](10) NULL,
	[ForceNWC] [char](10) NULL,
	[Editor] [varchar](30) NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [ForceNWC_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ForceEOQC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ForceEOQC](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[ForceEOQCStatus] [int] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ForceEOQC_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Family_MB]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Family_MB](
	[Family] [varchar](50) NOT NULL,
	[MB] [varchar](50) NOT NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [Family_MB_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FamilyInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FamilyInfo](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](200) NOT NULL,
	[Descr] [varchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [FamilyInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Family]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Family](
	[Family] [varchar](50) NOT NULL,
	[Descr] [nvarchar](50) NOT NULL,
	[CustomerID] [varchar](80) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Family_PK] PRIMARY KEY CLUSTERED 
(
	[Family] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FailedBomParts]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FailedBomParts](
	[Pno] [char](20) NULL,
	[Tp] [char](3) NULL,
	[Descr] [char](20) NULL,
	[Remark] [char](900) NULL,
	[Editor] [char](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[Site] [varchar](100) NULL,
	[FailedType] [varchar](30) NULL,
	[ImportTime] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FailKPCollection]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FailKPCollection](
	[ID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[PdLine] [varchar](32) NOT NULL,
	[Family] [varchar](64) NOT NULL,
	[PartName] [varchar](32) NOT NULL,
	[PartNo] [varchar](32) NOT NULL,
	[Vendor] [nvarchar](32) NOT NULL,
	[Module] [varchar](32) NOT NULL,
	[FailReason] [nvarchar](64) NOT NULL,
	[Qty] [int] NOT NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[Editor] [nvarchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [FailKPCollection_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FWD_ModelType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FWD_ModelType](
	[Family] [varchar](25) NULL,
	[TYPE] [varchar](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUWeightLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUWeightLog](
	[ID] [int] NOT NULL,
	[SN] [varchar](20) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FRUWeightLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUMBVer]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUMBVer](
	[ID] [int] NOT NULL,
	[PartNo] [varchar](12) NOT NULL,
	[MBCode] [varchar](4) NOT NULL,
	[Ver] [varchar](5) NOT NULL,
	[Remark] [varchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_FRUMBVer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUGift_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUGift_Part](
	[ID] [int] NOT NULL,
	[GiftID] [varchar](20) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Value] [varchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FRUGift_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUGift]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUGift](
	[ID] [varchar](20) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Qty] [int] NOT NULL,
 CONSTRAINT [FRUGift_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUFISToSAPWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUFISToSAPWeight](
	[Shipment] [varchar](20) NOT NULL,
	[Type] [char](1) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FRUFISToSAPWeight_PK] PRIMARY KEY CLUSTERED 
(
	[Shipment] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUCarton_Part]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUCarton_Part](
	[ID] [int] NOT NULL,
	[CartonID] [varchar](20) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Value] [varchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FRUCarton_Part_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUCarton_FRUGift]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUCarton_FRUGift](
	[ID] [int] NOT NULL,
	[CartonID] [varchar](20) NOT NULL,
	[GiftID] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FRUCarton_FRUGift_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FRUCarton]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FRUCarton](
	[ID] [varchar](20) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Qty] [int] NOT NULL,
 CONSTRAINT [FRUCarton_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FISToSAPWeightLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FISToSAPWeightLog](
	[DN/Shipment] [char](20) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Value] [int] NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FISToSAPWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FISToSAPWeight](
	[Shipment] [char](20) NOT NULL,
	[Type] [char](1) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FISToSAPWeight_PK] PRIMARY KEY CLUSTERED 
(
	[Shipment] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FISToSAPPLTWeight]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FISToSAPPLTWeight](
	[Shipment] [char](20) NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [FISToSAPPLTWeight_PK] PRIMARY KEY CLUSTERED 
(
	[PalletNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FISTOSAP_WEIGHT]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FISTOSAP_WEIGHT](
	[ID] [int] NOT NULL,
	[DN/Shipment] [char](15) NOT NULL,
	[Status] [char](1) NOT NULL,
	[KG] [decimal](10, 1) NOT NULL,
	[Cdt] [datetime] NULL,
	[SendStatus] [varchar](16) NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [FISTOSAP_WEIGHT_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FA_Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FA_Station](
	[ID] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[OptCode] [char](9) NOT NULL,
	[OptName] [char](8) NOT NULL,
	[Remark] [char](50) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [FA_Station_PK] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FA_SnoBTDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FA_SnoBTDet](
	[ID] [int] NOT NULL,
	[SnoId] [char](14) NULL,
	[BT] [varchar](50) NULL,
	[Remark] [varchar](100) NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [FA_SnoBTDet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FA_PA_LightSt]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FA_PA_LightSt](
	[ID] [int] NOT NULL,
	[Pno] [nvarchar](16) NOT NULL,
	[Family] [varchar](25) NOT NULL,
	[Stn] [nchar](10) NOT NULL,
	[Editor] [nchar](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [FA_PA_LightSt_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FA_ITCNDefect_Check]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[FA_ITCNDefect_Check](
	[ID] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[MAC] [char](1) NULL,
	[MBCT] [char](1) NULL,
	[HDDV] [varchar](50) NULL,
	[BIOS] [varchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[udt] [datetime] NOT NULL,
 CONSTRAINT [PK_FA_ITCNDefect_Check] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FA_FISToIMES]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FA_FISToIMES](
	[ProductID] [varchar](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FAKittingBoxSN]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FAKittingBoxSN](
	[PdLine] [varchar](10) NOT NULL,
	[SnoId] [varchar](10) NOT NULL,
	[Tp] [char](3) NOT NULL,
	[Sno] [varchar](5) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Remark] [char](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FAI_INFO]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FAI_INFO](
	[IECPN] [varchar](20) NOT NULL,
	[HPQPN] [varchar](20) NULL,
	[SNO] [varchar](20) NULL,
	[BIOS_TYP] [varchar](20) NULL,
	[KBC_Ver] [varchar](20) NULL,
	[VDO_BIOS] [varchar](20) NULL,
	[FDD_Sup] [varchar](20) NULL,
	[HDD_Sup] [varchar](20) NULL,
	[OPT_Sup] [varchar](20) NULL,
	[RAM_TYP] [varchar](20) NULL,
	[BAT_TYP] [varchar](20) NULL,
	[FIN_Time] [datetime] NULL,
	[REC_Time] [datetime] NULL,
	[UPC_Code] [varchar](20) NULL,
	[CHK_Stat] [varchar](20) NULL,
	[NG_Record] [varchar](100) NULL,
	[IMP_Record] [varchar](100) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_FAI_INFO] PRIMARY KEY CLUSTERED 
(
	[IECPN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ErrorMessage_CQ]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorMessage_CQ](
	[Code] [char](10) NOT NULL,
	[LanguageCode] [char](3) NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ErrorMessage_CQ_PK] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[LanguageCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ErrorMessage]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[ErrorMessage](
	[Code] [char](10) NOT NULL,
	[LanguageCode] [char](3) NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ErrorMessage_PK] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[LanguageCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EnergyLabel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EnergyLabel](
	[ID] [int] NOT NULL,
	[Family] [varchar](32) NOT NULL,
	[SeriesName] [nvarchar](64) NULL,
	[Level] [char](2) NOT NULL,
	[ChinaLevel] [varchar](8) NOT NULL,
	[SaveEnergy] [varchar](16) NOT NULL,
	[Editor] [varchar](64) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [PK_EnergyLabel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EcrVersion]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EcrVersion](
	[ID] [int] NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[MBCode] [char](3) NOT NULL,
	[ECR] [char](5) NOT NULL,
	[IECVer] [char](5) NULL,
	[Remark] [nvarchar](50) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [EcrVersion_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EPIACheckStartTime]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EPIACheckStartTime](
	[ID] [int] NOT NULL,
	[Model] [char](12) NOT NULL,
	[Type] [char](10) NULL,
	[VC] [char](5) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PK_EPIACheckStartTime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ECOAReturn]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ECOAReturn](
	[GroupNo] [char](9) NOT NULL,
	[CUSTSN] [varchar](30) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[ECOANo] [varchar](255) NULL,
	[Line] [varchar](30) NULL,
	[Status] [char](1) NOT NULL,
	[Message] [varchar](50) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dump_HP_ErrSnoId]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dump_HP_ErrSnoId](
	[SnoId] [char](10) NULL,
	[Sno] [char](14) NULL,
	[ShipDate] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dummy_ShipDet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dummy_ShipDet](
	[SnoId] [char](14) NOT NULL,
	[BOL] [varchar](50) NULL,
	[PLT] [char](12) NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [Dummy_ShipDet_PK] PRIMARY KEY CLUSTERED 
(
	[SnoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dn_Cn_Volume]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dn_Cn_Volume](
	[ID] [int] NOT NULL,
	[SnoId] [varchar](50) NOT NULL,
	[Tp] [varchar](50) NOT NULL,
	[Pallet] [varchar](3) NOT NULL,
	[Dn] [varchar](50) NOT NULL,
	[Shippment] [varchar](20) NULL,
	[Qty] [int] NOT NULL,
	[Po] [varchar](50) NOT NULL,
	[Weight] [decimal](10, 3) NOT NULL,
	[a] [decimal](10, 1) NOT NULL,
	[b] [decimal](10, 1) NOT NULL,
	[h] [decimal](10, 1) NOT NULL,
	[Editor] [varchar](50) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DismantlePalletWeightLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DismantlePalletWeightLog](
	[ID] [int] NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[Weight] [decimal](10, 2) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Weight_L] [decimal](10, 2) NULL,
 CONSTRAINT [DismantlePalletWeightLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DescType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DescType](
	[ID] [int] NOT NULL,
	[Tp] [char](2) NOT NULL,
	[Code] [char](20) NOT NULL,
	[Description] [char](50) NOT NULL,
	[Editor] [char](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DescType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dept]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dept](
	[Dept] [varchar](4) NOT NULL,
	[Section] [varchar](8) NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[FISLine] [varchar](4) NOT NULL,
	[StartTime] [varchar](10) NOT NULL,
	[EndTime] [varchar](10) NOT NULL,
	[Remark] [varchar](50) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [Dept_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_TriggerBackUp]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery_TriggerBackUp](
	[DeliveryNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NULL,
	[PoNo] [nvarchar](40) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[BackUpDate] [datetime] NOT NULL,
	[ID] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_Pallet_FIS]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery_Pallet_FIS](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NOT NULL,
	[DeliveryQty] [smallint] NOT NULL,
	[Status] [char](1) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_Delivery_Pallet_FIS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_Pallet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Delivery_Pallet](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NOT NULL,
	[DeliveryQty] [smallint] NOT NULL,
	[Status] [char](1) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[PalletType] [varchar](8) NULL,
	[DeviceQty] [int] NULL,
 CONSTRAINT [Delivery_Pallet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_FIS]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery_FIS](
	[DeliveryNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NULL,
	[PoNo] [char](50) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Delivery_FIS_PK] PRIMARY KEY CLUSTERED 
(
	[DeliveryNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery_Carton]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Delivery_Carton](
	[ID] [int] NOT NULL,
	[DeliveryNo] [varchar](20) NOT NULL,
	[CartonSN] [varchar](20) NOT NULL,
	[Model] [varchar](16) NOT NULL,
	[Qty] [int] NOT NULL,
	[AssignQty] [int] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Delivery_Carton_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryLog](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[Status] [char](2) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [DeliveryLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryInfo_FIS]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryInfo_FIS](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[InfoType] [char](20) NULL,
	[InfoValue] [nvarchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DeliveryInfo_FIS_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryInfo](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[InfoType] [char](20) NULL,
	[InfoValue] [nvarchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DeliveryInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryEx]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryEx](
	[DeliveryNo] [varchar](20) NOT NULL,
	[ShipmentNo] [varchar](20) NOT NULL,
	[ShipType] [varchar](4) NOT NULL,
	[PalletType] [varchar](8) NOT NULL,
	[ConsolidateQty] [int] NOT NULL,
	[CartonQty] [int] NOT NULL,
	[QtyPerCarton] [int] NOT NULL,
	[MessageCode] [varchar](16) NOT NULL,
	[ShipToParty] [varchar](16) NOT NULL,
	[Priority] [varchar](8) NOT NULL,
	[GroupId] [varchar](16) NOT NULL,
	[OrderType] [varchar](4) NOT NULL,
	[BOL] [varchar](64) NOT NULL,
	[HAWB] [varchar](64) NOT NULL,
	[Carrier] [varchar](16) NOT NULL,
	[ShipWay] [varchar](8) NOT NULL,
	[PackID] [varchar](16) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Udt] [datetime] NOT NULL,
	[StdPltFullQty] [varchar](4) NULL,
	[StdPltStackType] [varchar](4) NULL,
 CONSTRAINT [PK_DeliveryEx] PRIMARY KEY CLUSTERED 
(
	[DeliveryNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryAttrLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryAttrLog](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NOT NULL,
	[AttrName] [varchar](64) NOT NULL,
	[AttrOldValue] [nvarchar](1024) NOT NULL,
	[AttrNewValue] [nvarchar](1024) NOT NULL,
	[Descr] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [DeliveryAttrLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeliveryAttr]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeliveryAttr](
	[AttrName] [varchar](64) NOT NULL,
	[DeliveryNo] [varchar](20) NOT NULL,
	[AttrValue] [nvarchar](1024) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DeliveryAttr_PK] PRIMARY KEY CLUSTERED 
(
	[AttrName] ASC,
	[DeliveryNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Delivery](
	[DeliveryNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NULL,
	[PoNo] [nvarchar](40) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Delivery_PK] PRIMARY KEY CLUSTERED 
(
	[DeliveryNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeletedDeliveryPallet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeletedDeliveryPallet](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[PalletNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NOT NULL,
	[DeliveryQty] [smallint] NOT NULL,
	[Status] [char](1) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NULL,
	[PalletType] [varchar](8) NULL,
	[DeviceQty] [int] NULL,
 CONSTRAINT [DeletedDeliveryPallet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeletedDelivery]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeletedDelivery](
	[ID] [int] NOT NULL,
	[DeliveryNo] [char](20) NOT NULL,
	[ShipmentNo] [char](20) NULL,
	[PoNo] [nvarchar](40) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[ShipDate] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[Status] [char](2) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DeletedDelivery_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DefectInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefectInfo](
	[ID] [int] NOT NULL,
	[Code] [char](10) NOT NULL,
	[Description] [nvarchar](80) NULL,
	[Type] [char](20) NOT NULL,
	[CustomerID] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[EngDescr] [varchar](80) NULL,
 CONSTRAINT [DefectInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DefectHoldRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefectHoldRule](
	[ID] [int] NOT NULL,
	[CheckInStation] [varchar](32) NOT NULL,
	[Line] [varchar](16) NULL,
	[Family] [varchar](32) NULL,
	[DefectCode] [varchar](8) NULL,
	[EqualSameDefectCount] [int] NOT NULL,
	[OverDefectCount] [int] NOT NULL,
	[ExceptCause] [varchar](64) NULL,
	[HoldStation] [varchar](32) NOT NULL,
	[HoldCode] [varchar](32) NOT NULL,
	[HoldDescr] [nvarchar](255) NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [DefectHoldRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DefectCode_Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefectCode_Station](
	[ID] [int] NOT NULL,
	[Defect] [char](10) NULL,
	[PRE_STN] [char](10) NOT NULL,
	[CRT_STN] [char](10) NOT NULL,
	[NXT_STN] [char](10) NULL,
	[Editor] [char](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[DefectCode_Station] ADD [Cause] [char](10) NULL
SET ANSI_PADDING ON
ALTER TABLE [dbo].[DefectCode_Station] ADD [MajorPart] [varchar](8) NOT NULL
ALTER TABLE [dbo].[DefectCode_Station] ADD  CONSTRAINT [DefectCode_Station_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DefectCode_QCSUB_BCK]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefectCode_QCSUB_BCK](
	[Defect] [char](10) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Descr] [nvarchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[EngDescr] [varchar](80) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DefectCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DefectCode](
	[Defect] [char](10) NOT NULL,
	[Type] [char](10) NOT NULL,
	[Descr] [nvarchar](80) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[EngDescr] [varchar](80) NULL,
 CONSTRAINT [DefectCode_PK] PRIMARY KEY CLUSTERED 
(
	[Defect] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Window]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Window](
	[ID] [char](32) NOT NULL,
	[WindowName] [nvarchar](128) NULL,
	[DisplayName] [nvarchar](128) NULL,
	[AlertMessage] [nvarchar](512) NULL,
	[RefreshTime] [bigint] NULL,
	[DataSourceType] [int] NULL,
	[IsStageDsp] [bit] NULL,
	[Editor] [nvarchar](100) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [Dashboard_Window_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Station_Target]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Station_Target](
	[Line] [char](30) NOT NULL,
	[ID] [char](32) NOT NULL,
	[WindowsID] [char](32) NULL,
	[Station] [char](10) NULL,
	[YieldTarget] [float] NULL,
	[DisplayFields] [varchar](100) NULL,
	[Order] [int] NULL,
	[FactorOfFPY] [bit] NULL,
 CONSTRAINT [Dashboard_Station_Target_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Station_Data_UR]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Station_Data_UR](
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Quantity] [int] NULL,
	[Defect] [int] NULL,
	[WIP] [int] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [Dashboard_Station_Data_UR_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Station] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Station_Data]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Station_Data](
	[Line] [char](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Quantity] [int] NULL,
	[Defect] [int] NULL,
	[WIP] [int] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [Dashboard_Station_Data_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Station] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Stage_Target]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Stage_Target](
	[ID] [char](32) NOT NULL,
	[Stage] [varchar](10) NULL,
	[WindowID] [char](32) NULL,
	[StartWorkTime] [datetime] NULL,
	[StopWorkTime] [datetime] NULL,
	[DisplayFields] [varchar](100) NULL,
 CONSTRAINT [Dashboard_Stage_Target_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Stage_Data_UR]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Stage_Data_UR](
	[Stage] [varchar](10) NOT NULL,
	[DN] [int] NULL,
	[FAInput] [int] NULL,
	[FAOutput] [int] NULL,
	[PAInput] [int] NULL,
	[PAOutput] [int] NULL,
	[SAInput] [int] NULL,
	[SAOutput] [int] NULL,
	[Cdt] [datetime] NULL,
	[SMTInput] [int] NULL,
	[SMTOutput] [int] NULL,
 CONSTRAINT [Dashboard_Stage_Data_UR_PK] PRIMARY KEY CLUSTERED 
(
	[Stage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Stage_Data]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Stage_Data](
	[Stage] [varchar](10) NOT NULL,
	[DN] [int] NULL,
	[FAInput] [int] NULL,
	[FAOutput] [int] NULL,
	[PAInput] [int] NULL,
	[PAOutput] [int] NULL,
	[SAInput] [int] NULL,
	[SAOutput] [int] NULL,
	[Cdt] [datetime] NULL,
	[SMTInput] [int] NULL,
	[SMTOutput] [int] NULL,
 CONSTRAINT [Dashboard_Stage_Data_PK] PRIMARY KEY CLUSTERED 
(
	[Stage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Stage_Base]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Stage_Base](
	[Stage] [varchar](10) NOT NULL,
	[Type] [int] NULL,
	[Stage_Type] [int] NULL,
 CONSTRAINT [Dash_Stage_Base_PK] PRIMARY KEY CLUSTERED 
(
	[Stage] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Line_Target]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Line_Target](
	[Line] [char](30) NOT NULL,
	[ID] [char](32) NOT NULL,
	[WindowsID] [char](32) NULL,
	[OutputTarget] [int] NULL,
	[FPYTarget] [float] NULL,
	[StartWorkTime] [datetime] NULL,
	[StopWorkTime] [datetime] NULL,
	[IsStationDsp] [bit] NULL,
	[Order] [int] NULL,
	[FPYAlert] [float] NULL,
	[Shift] [varchar](10) NULL,
	[FmlDspField] [varchar](200) NULL,
 CONSTRAINT [Dashboard_Line_Target_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Line_Data_UR]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Line_Data_UR](
	[Line] [char](30) NOT NULL,
	[Input] [int] NULL,
	[Output] [int] NULL,
	[DefQty] [int] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [Dashboard_Line_Data_UR_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Line_Data]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Line_Data](
	[Line] [char](30) NOT NULL,
	[Input] [int] NULL,
	[Output] [int] NULL,
	[DefQty] [int] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [Dashboard_Line_Data_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_IO_Station]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_IO_Station](
	[ID] [char](32) NOT NULL,
	[Family] [char](50) NOT NULL,
	[Stage] [varchar](10) NOT NULL,
	[InputStation] [char](10) NOT NULL,
	[OutputStation] [char](10) NOT NULL,
 CONSTRAINT [Dashboard_IO_Station_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Family_Target]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Family_Target](
	[ID] [char](32) NOT NULL,
	[Line] [char](30) NOT NULL,
	[WindowsID] [char](32) NOT NULL,
	[Family] [varchar](80) NOT NULL,
	[Series] [varchar](200) NOT NULL,
	[Plan] [int] NULL,
	[Order] [int] NULL,
 CONSTRAINT [Dashboard_Family_Target_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Family_Data_UR]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Family_Data_UR](
	[Line] [char](30) NOT NULL,
	[Family] [varchar](80) NOT NULL,
	[Series] [varchar](200) NOT NULL,
	[Output] [int] NULL,
	[DefectQty] [int] NULL,
	[Input] [int] NULL,
	[AOI_Output] [int] NULL,
	[AOI_Defect] [int] NULL,
	[ICT_Input] [int] NULL,
	[ICT_Defect] [int] NULL,
 CONSTRAINT [Dashboard_Family_Data_UR_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Family] ASC,
	[Series] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dashboard_Family_Data]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dashboard_Family_Data](
	[Line] [char](30) NOT NULL,
	[Family] [varchar](80) NOT NULL,
	[Series] [varchar](200) NOT NULL,
	[Output] [int] NULL,
	[DefectQty] [int] NULL,
	[Input] [int] NULL,
	[AOI_Output] [int] NULL,
	[AOI_Defect] [int] NULL,
	[ICT_Input] [int] NULL,
	[ICT_Defect] [int] NULL,
 CONSTRAINT [Dashboard_Family_Data_PK] PRIMARY KEY CLUSTERED 
(
	[Line] ASC,
	[Family] ASC,
	[Series] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DOAMBList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DOAMBList](
	[GroupNo] [char](12) NOT NULL,
	[PCBNo] [char](11) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Message] [nvarchar](100) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DOAList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DOAList](
	[ID] [int] NOT NULL,
	[Sno] [varchar](16) NOT NULL,
	[Tp] [varchar](5) NOT NULL,
	[Pno] [varchar](14) NOT NULL,
	[PoNo] [varchar](20) NOT NULL,
	[RMA] [varchar](30) NOT NULL,
	[Editor] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Mark] [int] NOT NULL,
 CONSTRAINT [DOAList_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DN_PrintList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DN_PrintList](
	[ID] [int] NOT NULL,
	[DN] [varchar](20) NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [DN_PrintList_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DKSn]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DKSn](
	[CUSTSN] [varchar](30) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Family] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Customer] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
	[Plant] [char](10) NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [Customer_PK] PRIMARY KEY CLUSTERED 
(
	[Customer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConstValueTypeLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConstValueTypeLog](
	[ID] [int] NOT NULL,
	[Type] [varchar](64) NULL,
	[Value] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NOT NULL,
	[BackUpDate] [datetime] NOT NULL,
	[Action] [varchar](16) NOT NULL,
 CONSTRAINT [PK_ConstValueTypeLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConstValueType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConstValueType](
	[ID] [int] NOT NULL,
	[Type] [varchar](64) NULL,
	[Value] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [SpecialType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConstValueLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConstValueLog](
	[ID] [int] NOT NULL,
	[Name] [varchar](64) NULL,
	[Type] [varchar](64) NULL,
	[Value] [nvarchar](3000) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
	[BackUpDate] [datetime] NOT NULL,
	[Action] [varchar](16) NOT NULL,
 CONSTRAINT [PK_ConstValueLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConstValue]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConstValue](
	[ID] [int] NOT NULL,
	[Name] [varchar](64) NULL,
	[Type] [varchar](64) NULL,
	[Value] [nvarchar](3000) NULL,
	[Description] [nvarchar](255) NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [ConstValue_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConcurrentLocks]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConcurrentLocks](
	[GUID] [uniqueidentifier] NOT NULL,
	[Type] [varchar](64) NOT NULL,
	[LockValue] [varchar](128) NOT NULL,
	[Cdt] [datetime2](7) NOT NULL,
	[ClientAddr] [varchar](512) NOT NULL,
	[Line] [varchar](512) NOT NULL,
	[Editor] [nvarchar](512) NOT NULL,
	[Station] [varchar](512) NOT NULL,
	[Customer] [varchar](512) NOT NULL,
	[TimeoutSpan4Hold] [bigint] NOT NULL,
	[TimeoutSpan4Wait] [bigint] NOT NULL,
	[Descr] [nvarchar](1024) NULL,
 CONSTRAINT [ConcurrentLocks_PK] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_ConcurrentLocks] UNIQUE NONCLUSTERED 
(
	[Type] ASC,
	[LockValue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConFig]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConFig](
	[ID] [int] NOT NULL,
	[CUSTSN] [varchar](50) NOT NULL,
	[Status] [char](2) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChinaLabel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChinaLabel](
	[ID] [int] NOT NULL,
	[Family] [nvarchar](50) NOT NULL,
	[PN] [nvarchar](20) NOT NULL,
 CONSTRAINT [ChinaLabel_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChepPallet]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChepPallet](
	[ID] [int] NOT NULL,
	[PalletNo] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [ChepPallet_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CheckItemTypeRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CheckItemTypeRule](
	[ID] [int] NOT NULL,
	[CheckItemType] [varchar](20) NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[Station] [varchar](10) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[BomNodeType] [varchar](255) NOT NULL,
	[PartDescr] [varchar](255) NOT NULL,
	[PartType] [varchar](255) NOT NULL,
	[MatchRule] [varchar](255) NOT NULL,
	[CheckRule] [varchar](255) NOT NULL,
	[SaveRule] [varchar](255) NOT NULL,
	[Descr] [nvarchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CheckItemTypeRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CheckItemType]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CheckItemType](
	[Name] [varchar](20) NOT NULL,
	[DisplayName] [varchar](50) NULL,
	[FilterModule] [varchar](128) NULL,
	[MatchModule] [varchar](128) NULL,
	[CheckModule] [varchar](128) NULL,
	[SaveModule] [varchar](128) NULL,
	[NeedUniqueCheck] [bit] NULL,
	[NeedCommonSave] [bit] NULL,
	[Editor] [varchar](30) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [CheckItemType_PK] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CheckItemSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CheckItemSetting](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[CheckCondition] [varchar](20) NULL,
	[CheckConditionType] [char](1) NULL,
	[Station] [char](10) NULL,
	[CheckItemID] [int] NOT NULL,
	[CheckType] [int] NOT NULL,
	[CheckValue] [varchar](200) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CheckItemSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CheckItem]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CheckItem](
	[ID] [int] NOT NULL,
	[Customer] [char](10) NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[Mode] [int] NOT NULL,
	[ItemType] [int] NOT NULL,
	[ItemDisplayName] [varchar](50) NOT NULL,
	[MatchRule] [varchar](255) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CheckItem_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Change_PCB]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Change_PCB](
	[OldPCBNo] [char](11) NOT NULL,
	[NewPCBNo] [char](11) NOT NULL,
	[Reason] [varchar](100) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [Change_PCB_PK] PRIMARY KEY CLUSTERED 
(
	[OldPCBNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChangeLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChangeLog](
	[ID] [int] NOT NULL,
	[ProductID] [char](10) NOT NULL,
	[Mo] [char](20) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [ChangeLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Carton_Product]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Carton_Product](
	[ID] [int] NOT NULL,
	[CartonSN] [varchar](20) NOT NULL,
	[ProductID] [varchar](16) NOT NULL,
	[DeliveryNo] [varchar](20) NOT NULL,
	[Remark] [varchar](32) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Carton_Product_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CartonStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[CartonStatus](
	[CartonNo] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_Carton] PRIMARY KEY CLUSTERED 
(
	[CartonNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CartonSSCC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CartonSSCC](
	[CartonSN] [varchar](20) NOT NULL,
	[SSCC] [varchar](20) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [CartonSSCC_PK] PRIMARY KEY CLUSTERED 
(
	[CartonSN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CartonQCLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[CartonQCLog](
	[ID] [int] NOT NULL,
	[CartonSN] [varchar](20) NOT NULL,
	[Model] [varchar](16) NOT NULL,
	[Line] [varchar](16) NOT NULL,
	[Type] [varchar](8) NOT NULL,
	[Status] [varchar](8) NOT NULL,
	[Remark] [varchar](64) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [CartonQCLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CartonLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[CartonLog](
	[ID] [int] NOT NULL,
	[CartonNo] [char](10) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Status] [int] NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_CartonLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CartonInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[CartonInfo](
	[ID] [int] NOT NULL,
	[CartonNo] [char](10) NOT NULL,
	[InfoType] [varchar](20) NOT NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[CartonInfo] ADD [InfoValue] [varchar](64) NULL
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[CartonInfo] ADD [Editor] [varchar](30) NOT NULL
ALTER TABLE [dbo].[CartonInfo] ADD [Cdt] [datetime] NOT NULL
ALTER TABLE [dbo].[CartonInfo] ADD [Udt] [datetime] NOT NULL
ALTER TABLE [dbo].[CartonInfo] ADD  CONSTRAINT [PK_CartonInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Carton]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Carton](
	[CartonSN] [varchar](20) NOT NULL,
	[PalletNo] [varchar](20) NOT NULL,
	[Model] [varchar](16) NOT NULL,
	[BoxId] [varchar](32) NOT NULL,
	[PAQCStatus] [varchar](4) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[DNQty] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[FullQty] [int] NOT NULL,
	[Status] [varchar](12) NOT NULL,
	[PreStation] [varchar](16) NOT NULL,
	[PreStationStatus] [int] NOT NULL,
	[UnPackPalletNo] [varchar](20) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Carton_PK] PRIMARY KEY CLUSTERED 
(
	[CartonSN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CacheUpdateServer]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CacheUpdateServer](
	[ServerIP] [varchar](50) NOT NULL,
	[AppName] [varchar](50) NOT NULL,
 CONSTRAINT [CacheUpdateServer_PK] PRIMARY KEY CLUSTERED 
(
	[ServerIP] ASC,
	[AppName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CacheUpdate]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CacheUpdate](
	[ID] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[Updated] [bit] NOT NULL,
	[CacheServerIP] [varchar](50) NULL,
	[AppName] [varchar](50) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CacheUpdate_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CUSTSNLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CUSTSNLog](
	[Value] [char](10) NOT NULL,
	[Cdt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CTOBom]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CTOBom](
	[ID] [int] NOT NULL,
	[MPno] [char](12) NULL,
	[Tp] [char](2) NULL,
	[SPno] [char](50) NULL,
	[Qty] [int] NULL,
	[Descr] [nvarchar](255) NULL,
	[Remark] [nvarchar](255) NULL,
	[Editor] [char](10) NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CTOBom_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CSNMas]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CSNMas](
	[ID] [int] NOT NULL,
	[CSN1] [varchar](50) NOT NULL,
	[CSN2] [varchar](50) NOT NULL,
	[Pno] [varchar](20) NOT NULL,
	[Status] [char](2) NOT NULL,
	[PdLine] [char](6) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [CSNMas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CSNLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CSNLog](
	[SnoId] [char](14) NOT NULL,
	[Tp] [char](10) NOT NULL,
	[Pno] [varchar](20) NOT NULL,
	[PdLine] [char](30) NOT NULL,
	[WC] [char](2) NOT NULL,
	[IsPass] [smallint] NOT NULL,
	[Editor] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[ID] [int] NOT NULL,
 CONSTRAINT [CSNLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COMSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COMSetting](
	[ID] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[CommPort] [char](2) NOT NULL,
	[RThreshold] [int] NOT NULL,
	[BaudRate] [varchar](30) NOT NULL,
	[Handshaking] [int] NOT NULL,
	[SThreshold] [int] NOT NULL,
	[Editor] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [COMSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COATrans_Log]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COATrans_Log](
	[ID] [int] NOT NULL,
	[BegNo] [varchar](20) NOT NULL,
	[EndNo] [varchar](20) NOT NULL,
	[Pno] [char](14) NULL,
	[PreStatus] [char](4) NULL,
	[Status] [char](4) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [COATrans_Log_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAStockQty]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAStockQty](
	[MinID] [varchar](64) NULL,
	[MaxID] [varchar](64) NULL,
	[Qty] [varchar](64) NULL,
	[Status] [varchar](32) NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAStatus]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAStatus](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Status] [char](2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [COAStatus_PK] PRIMARY KEY CLUSTERED 
(
	[COASN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAReturn]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAReturn](
	[ID] [int] NOT NULL,
	[CUSTSN] [varchar](30) NOT NULL,
	[COASN] [char](15) NOT NULL,
	[OriginalStatus] [char](2) NOT NULL,
	[Status] [char](2) NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_COAWithdraw] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAReceive]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAReceive](
	[ID] [int] NOT NULL,
	[BegSN] [char](15) NOT NULL,
	[EndSN] [char](15) NOT NULL,
	[PO] [char](16) NULL,
	[CustPN] [varchar](30) NULL,
	[IECPN] [char](15) NULL,
	[MSPN] [varchar](30) NULL,
	[Descr] [varchar](80) NULL,
	[ShipDate] [datetime] NULL,
	[Qty] [smallint] NULL,
	[Cust] [char](10) NULL,
	[Upload] [varchar](30) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [COAReceive_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COARMA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COARMA](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[RMA] [char](15) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [PK_COARMA] PRIMARY KEY CLUSTERED 
(
	[COASN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAMAS_3]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAMAS_3](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Status] [char](2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAMAS_2]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAMAS_2](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Status] [char](2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAMAS_1]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAMAS_1](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Status] [char](2) NOT NULL,
	[Line] [char](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COALog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COALog](
	[ID] [int] NOT NULL,
	[COASN] [char](15) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Tp] [char](10) NULL,
 CONSTRAINT [COALog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[COAFru]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[COAFru](
	[COASN] [char](15) NOT NULL,
	[IECPN] [varchar](20) NULL,
	[Status] [char](2) NOT NULL,
	[CUSTSN] [char](15) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[DeliveryNo] [char](20) NULL,
	[ShipDate] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CELDATA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CELDATA](
	[Platform] [varchar](100) NOT NULL,
	[ProductSeriesName] [varchar](100) NOT NULL,
	[Category] [nvarchar](1) NOT NULL,
	[Grade] [int] NULL,
	[TEC] [varchar](10) NULL,
	[ZMOD] [varchar](20) NULL,
	[Editor] [varchar](50) NOT NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CDSI_PRDList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CDSI_PRDList](
	[SnoId] [char](10) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CDSIAST]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CDSIAST](
	[ID] [int] NOT NULL,
	[SnoId] [char](10) NOT NULL,
	[Tp] [varchar](30) NOT NULL,
	[Sno] [varchar](30) NOT NULL,
 CONSTRAINT [CDSIAST_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BoxerBookData]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BoxerBookData](
	[id] [int] NOT NULL,
	[MBNo] [char](11) NULL,
	[TCaseNo] [varchar](50) NULL,
	[TLineNo] [char](3) NULL,
	[TStatNo] [int] NULL,
	[IsPass] [int] NULL,
	[Desc] [varchar](25) NULL,
	[ErrorCode] [char](4) NULL,
	[datetime] [char](14) NULL,
	[Cdt] [datetime] NULL,
	[PID] [char](10) NULL,
	[SerialNumber] [char](16) NULL,
	[CartonSN] [char](12) NULL,
	[DateManufactured] [char](10) NULL,
	[PublicKey] [varchar](500) NULL,
	[MACAddress] [char](12) NULL,
	[IMEI] [char](15) NULL,
	[IMSI] [char](15) NULL,
	[PrivateKey] [varchar](100) NULL,
	[EventType] [varchar](25) NULL,
	[DeviceAttribute] [varchar](25) NULL,
	[Platform] [varchar](25) NULL,
	[ICCID] [varchar](25) NULL,
	[EAN] [char](13) NULL,
	[ModelNumber] [char](8) NULL,
	[PalletSerialNo] [varchar](20) NULL,
 CONSTRAINT [BoxerBookData_PK] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BorrowLog2]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[BorrowLog2](
	[ID] [int] NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[BQty] [int] NOT NULL,
	[RQty] [int] NULL,
	[Borrower] [varchar](10) NOT NULL,
	[Lender] [varchar](10) NOT NULL,
	[Returner] [varchar](10) NULL,
	[Accepter] [varchar](10) NULL,
	[Status] [char](1) NOT NULL,
	[Bdate] [datetime] NOT NULL,
	[Rdate] [datetime] NOT NULL,
 CONSTRAINT [PK_BorrowLog2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BorrowLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BorrowLog](
	[ID] [int] NOT NULL,
	[Sn] [varchar](14) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Borrower] [varchar](30) NOT NULL,
	[Lender] [varchar](30) NOT NULL,
	[Returner] [varchar](30) NOT NULL,
	[Acceptor] [varchar](30) NOT NULL,
	[Status] [char](1) NOT NULL,
	[Bdate] [datetime] NOT NULL,
	[Rdate] [datetime] NOT NULL,
 CONSTRAINT [BorrowLog_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Bom_Code]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bom_Code](
	[ID] [int] NOT NULL,
	[part_number] [nvarchar](20) NULL,
	[part_number_type] [nvarchar](20) NULL,
	[product_family] [nvarchar](20) NULL,
	[description] [nvarchar](100) NULL,
	[uio_buyer_code] [varchar](10) NULL,
	[os_code] [varchar](10) NULL,
	[os_desc] [nvarchar](100) NULL,
 CONSTRAINT [Bom_Code_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BinData]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BinData](
	[SnoId] [char](9) NOT NULL,
	[Bin] [char](4) NOT NULL,
	[Qty] [int] NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Battery]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Battery](
	[ID] [int] NOT NULL,
	[HPPN] [varchar](12) NOT NULL,
	[HSSN] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [Battery_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Batt_Temp]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Batt_Temp](
	[CT] [varchar](50) NOT NULL,
	[SN] [char](10) NOT NULL,
	[Capacity] [char](10) NOT NULL,
	[Remark] [varchar](50) NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_Batt_Temp] PRIMARY KEY CLUSTERED 
(
	[CT] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BT_SeaShipmentSku]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BT_SeaShipmentSku](
	[ID] [int] NOT NULL,
	[PdLine] [char](4) NOT NULL,
	[Model] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [BT_SeaShipmentSku_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BSamModel]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BSamModel](
	[A_Part_Model] [varchar](16) NOT NULL,
	[C_Part_Model] [varchar](16) NOT NULL,
	[HP_A_Part] [varchar](16) NOT NULL,
	[HP_C_SKU] [varchar](16) NOT NULL,
	[QtyPerCarton] [int] NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [BSamModel_PK] PRIMARY KEY CLUSTERED 
(
	[A_Part_Model] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BSamLocation]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BSamLocation](
	[LocationId] [varchar](16) NOT NULL,
	[Model] [varchar](16) NOT NULL,
	[Qty] [int] NOT NULL,
	[RemainQty] [int] NOT NULL,
	[FullQty] [int] NOT NULL,
	[FullCartonQty] [int] NOT NULL,
	[HoldInput] [varchar](4) NOT NULL,
	[HoldOutput] [varchar](4) NOT NULL,
	[Editor] [varchar](32) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [BSamLocation_PK] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BSParts]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BSParts](
	[PartNo] [char](12) NOT NULL,
	[FRUNO] [char](10) NOT NULL,
	[Descr] [varchar](100) NULL,
	[PartType] [char](10) NULL,
 CONSTRAINT [BSParts_PK] PRIMARY KEY CLUSTERED 
(
	[PartNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AstRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AstRule](
	[ID] [int] NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Tp] [varchar](6) NOT NULL,
	[Station] [varchar](6) NOT NULL,
	[CheckTp] [varchar](16) NULL,
	[CustName] [varchar](30) NULL,
	[CheckItem] [varchar](20) NULL,
	[Remark] [varchar](500) NULL,
	[Editor] [varchar](20) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [AstRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetRule]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetRule](
	[ID] [int] NOT NULL,
	[AstType] [varchar](10) NOT NULL,
	[Station] [varchar](6) NOT NULL,
	[CheckingType] [varchar](4) NOT NULL,
	[CustName] [varchar](30) NOT NULL,
	[CheckItem] [varchar](20) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [AssetRule_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetRange]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetRange](
	[ID] [int] NOT NULL,
	[Code] [char](20) NOT NULL,
	[Begin] [char](20) NOT NULL,
	[End] [char](20) NOT NULL,
	[Remark] [nvarchar](255) NOT NULL,
	[Editor] [char](20) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[Status] [char](1) NOT NULL,
 CONSTRAINT [AssetRange_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssemblyVC]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssemblyVC](
	[ID] [bigint] NOT NULL,
	[Family] [varchar](64) NULL,
	[PartNo] [varchar](64) NULL,
	[VC] [varchar](16) NOT NULL,
	[CombineVC] [varchar](16) NOT NULL,
	[CombinePartNo] [varchar](64) NULL,
	[Remark] [varchar](255) NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [AssemblyVC_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssemblyCodeInfo]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssemblyCodeInfo](
	[ID] [int] NOT NULL,
	[AssemblyCode] [varchar](10) NOT NULL,
	[InfoType] [varchar](50) NOT NULL,
	[InfoValue] [varchar](200) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [AssemblyCodeInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssemblyCode]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssemblyCode](
	[ID] [int] NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Family] [varchar](50) NULL,
	[Region] [varchar](50) NULL,
	[Model] [varchar](20) NULL,
	[AssemblyCode] [varchar](10) NOT NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
 CONSTRAINT [AssemblyCode_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveTableLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveTableLog](
	[TableName] [varchar](64) NOT NULL,
	[BeforeCount] [int] NULL,
	[ArchiveCount] [int] NULL,
	[TimeStamp] [varchar](64) NOT NULL,
	[Item] [varchar](32) NULL,
	[IsCopySuccess] [varchar](8) NULL,
	[IsDeleteSuccess] [varchar](8) NULL,
	[Msg] [varchar](max) NULL,
	[CopyTime] [int] NULL,
	[DeleteTime] [int] NULL,
	[Udt] [datetime] NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PK_ArchiveTableLog] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC,
	[TimeStamp] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveTableList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveTableList](
	[TableName] [varchar](50) NOT NULL,
	[Item] [varchar](50) NULL,
	[PKName2] [varchar](50) NULL,
	[FKName] [varchar](64) NULL,
	[SQLScript] [varchar](max) NULL,
	[DeleteOrder] [int] NULL,
	[IsNeedXML] [varchar](8) NULL,
	[Enable] [varchar](8) NULL,
 CONSTRAINT [PK_ArchiveDBSetting] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveMainTable]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveMainTable](
	[Item] [varchar](50) NOT NULL,
	[TableName] [varchar](50) NOT NULL,
	[PKName] [varchar](50) NOT NULL,
	[SQLScript] [varchar](max) NULL,
	[DeleteOrder] [int] NOT NULL,
	[IsNeedXML] [varchar](8) NULL,
	[Enable] [varchar](8) NULL,
 CONSTRAINT [PK_ArchiveItem] PRIMARY KEY CLUSTERED 
(
	[Item] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveLog]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveLog](
	[ID] [int] NOT NULL,
	[LogName] [varchar](255) NULL,
	[LogValue] [varchar](1024) NULL,
	[LogType] [varchar](64) NULL,
	[TimeStamp] [nvarchar](50) NULL,
	[Cdt] [datetime] NULL,
 CONSTRAINT [PK_ArchiveMLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveIDList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveIDList](
	[ID] [int] NOT NULL,
	[PK_ID] [varchar](50) NOT NULL,
	[Item] [varchar](50) NULL,
	[Status] [varchar](32) NULL,
	[TimeStamp] [varchar](50) NULL,
	[Cdt] [datetime] NOT NULL,
 CONSTRAINT [PK_ArchiveIDList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveFKTableList]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveFKTableList](
	[TableName] [varchar](50) NOT NULL,
	[ParentTableName] [varchar](50) NULL,
	[FKName] [varchar](50) NOT NULL,
	[IsNeedXML] [varchar](8) NULL,
 CONSTRAINT [PK_ArchiveFKTableList] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveAllFKConstraint]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveAllFKConstraint](
	[ForeignKey] [varchar](255) NOT NULL,
	[TableName] [varchar](255) NULL,
	[ColumnName] [varchar](255) NULL,
	[ReferenceTableName] [varchar](255) NULL,
	[ReferenceColumnName] [varchar](255) NULL,
 CONSTRAINT [PK_ArchiveAllFKConstraint] PRIMARY KEY CLUSTERED 
(
	[ForeignKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AlarmSetting]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlarmSetting](
	[ID] [int] NOT NULL,
	[Stage] [char](3) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Type] [char](1) NOT NULL,
	[YieldRate] [float] NULL,
	[MinQty] [int] NULL,
	[DefectType] [char](1) NULL,
	[Defects] [varchar](800) NULL,
	[DefectQty] [int] NULL,
	[Period] [float] NULL,
	[Editor] [varchar](30) NOT NULL,
	[Cdt] [datetime] NOT NULL,
	[Udt] [datetime] NOT NULL,
	[LifeCycle] [bit] NOT NULL,
 CONSTRAINT [AlarmSetting_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Alarm]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Alarm](
	[ID] [int] NOT NULL,
	[Stage] [char](3) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[AlarmSettingID] [int] NOT NULL,
	[Line] [varchar](30) NOT NULL,
	[Station] [char](10) NOT NULL,
	[Family] [varchar](50) NOT NULL,
	[Defect] [char](10) NOT NULL,
	[ReasonCode] [char](4) NOT NULL,
	[Reason] [varchar](50) NOT NULL,
	[Status] [varchar](10) NOT NULL,
	[SkipHoldPIC] [varchar](30) NULL,
	[SkipHoldTime] [datetime] NULL,
	[HoldModel] [varchar](511) NULL,
	[HoldLine] [varchar](255) NULL,
	[HoldStation] [varchar](255) NULL,
	[ActionPIC] [varchar](30) NULL,
	[ActionTime] [datetime] NULL,
	[Cause] [nvarchar](255) NULL,
	[Action] [nvarchar](255) NULL,
	[ReleasePIC] [varchar](30) NULL,
	[ReleaseTime] [datetime] NULL,
	[Remark] [nvarchar](4000) NULL,
	[Cdt] [datetime] NOT NULL,
	[ConCdt] [datetime] NULL,
 CONSTRAINT [Alarm_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ACAdapMaintain]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ACAdapMaintain](
	[ID] [int] NOT NULL,
	[ASSEMB] [char](10) NULL,
	[ADPPN] [char](10) NULL,
	[AGENCY] [char](20) NULL,
	[SUPPLIER] [char](10) NULL,
	[VOLTAGE] [char](10) NULL,
	[CUR] [char](10) NULL,
	[Editor] [varchar](25) NULL,
	[Cdt] [datetime] NULL,
	[Udt] [datetime] NULL,
 CONSTRAINT [ACAdapMaintain_PAK_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AAA]    Script Date: 09/03/2014 10:09:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AAA](
	[Layer] [float] NULL,
	[ShipWay] [nvarchar](255) NULL,
	[FullPLTQty] [float] NULL,
	[RegId] [nvarchar](255) NULL,
	[MinQty] [float] NULL,
	[MaxQty] [float] NULL,
	[PLT Weight] [float] NULL,
	[PLTType] [nvarchar](255) NULL,
	[PLTCode] [nvarchar](255) NULL,
	[suttle] [float] NULL,
	[F11] [nvarchar](255) NULL,
	[F12] [nvarchar](255) NULL,
	[F13] [nvarchar](255) NULL,
	[F14] [nvarchar](255) NULL,
	[F15] [nvarchar](255) NULL,
	[F16] [nvarchar](255) NULL,
	[F17] [nvarchar](255) NULL,
	[F18] [nvarchar](255) NULL,
	[F19] [nvarchar](255) NULL,
	[F20] [nvarchar](255) NULL,
	[F21] [nvarchar](255) NULL,
	[F22] [nvarchar](255) NULL,
	[F23] [nvarchar](255) NULL,
	[F24] [nvarchar](255) NULL,
	[F25] [nvarchar](255) NULL,
	[F26] [nvarchar](255) NULL,
	[F27] [nvarchar](255) NULL,
	[F28] [nvarchar](255) NULL,
	[F29] [nvarchar](255) NULL,
	[F30] [nvarchar](255) NULL,
	[F31] [nvarchar](255) NULL,
	[F32] [nvarchar](255) NULL,
	[F33] [nvarchar](255) NULL,
	[F34] [nvarchar](255) NULL,
	[F35] [nvarchar](255) NULL,
	[F36] [nvarchar](255) NULL,
	[F37] [nvarchar](255) NULL,
	[F38] [nvarchar](255) NULL,
	[F39] [nvarchar](255) NULL,
	[F40] [nvarchar](255) NULL,
	[F41] [nvarchar](255) NULL,
	[F42] [nvarchar](255) NULL,
	[F43] [nvarchar](255) NULL,
	[F44] [nvarchar](255) NULL,
	[F45] [nvarchar](255) NULL,
	[F46] [nvarchar](255) NULL,
	[F47] [nvarchar](255) NULL,
	[F48] [nvarchar](255) NULL,
	[F49] [nvarchar](255) NULL,
	[F50] [nvarchar](255) NULL,
	[F51] [nvarchar](255) NULL,
	[F52] [nvarchar](255) NULL,
	[F53] [nvarchar](255) NULL,
	[F54] [nvarchar](255) NULL,
	[F55] [nvarchar](255) NULL,
	[F56] [nvarchar](255) NULL,
	[F57] [nvarchar](255) NULL,
	[F58] [nvarchar](255) NULL,
	[F59] [nvarchar](255) NULL,
	[F60] [nvarchar](255) NULL,
	[F61] [nvarchar](255) NULL,
	[F62] [nvarchar](255) NULL,
	[F63] [nvarchar](255) NULL,
	[F64] [nvarchar](255) NULL,
	[F65] [nvarchar](255) NULL,
	[F66] [nvarchar](255) NULL,
	[F67] [nvarchar](255) NULL,
	[F68] [nvarchar](255) NULL,
	[F69] [nvarchar](255) NULL,
	[F70] [nvarchar](255) NULL,
	[F71] [nvarchar](255) NULL,
	[F72] [nvarchar](255) NULL,
	[F73] [nvarchar](255) NULL,
	[F74] [nvarchar](255) NULL,
	[F75] [nvarchar](255) NULL,
	[F76] [nvarchar](255) NULL,
	[F77] [nvarchar](255) NULL,
	[F78] [nvarchar](255) NULL,
	[F79] [nvarchar](255) NULL,
	[F80] [nvarchar](255) NULL,
	[F81] [nvarchar](255) NULL,
	[F82] [nvarchar](255) NULL,
	[F83] [nvarchar](255) NULL,
	[F84] [nvarchar](255) NULL,
	[F85] [nvarchar](255) NULL,
	[F86] [nvarchar](255) NULL,
	[F87] [nvarchar](255) NULL,
	[F88] [nvarchar](255) NULL,
	[F89] [nvarchar](255) NULL,
	[F90] [nvarchar](255) NULL,
	[F91] [nvarchar](255) NULL,
	[F92] [nvarchar](255) NULL,
	[F93] [nvarchar](255) NULL,
	[F94] [nvarchar](255) NULL,
	[F95] [nvarchar](255) NULL,
	[F96] [nvarchar](255) NULL,
	[F97] [nvarchar](255) NULL,
	[F98] [nvarchar](255) NULL,
	[F99] [nvarchar](255) NULL,
	[F100] [nvarchar](255) NULL,
	[F101] [nvarchar](255) NULL,
	[F102] [nvarchar](255) NULL,
	[F103] [nvarchar](255) NULL,
	[F104] [nvarchar](255) NULL,
	[F105] [nvarchar](255) NULL,
	[F106] [nvarchar](255) NULL,
	[F107] [nvarchar](255) NULL,
	[F108] [nvarchar](255) NULL,
	[F109] [nvarchar](255) NULL,
	[F110] [nvarchar](255) NULL,
	[F111] [nvarchar](255) NULL,
	[F112] [nvarchar](255) NULL,
	[F113] [nvarchar](255) NULL,
	[F114] [nvarchar](255) NULL,
	[F115] [nvarchar](255) NULL,
	[F116] [nvarchar](255) NULL,
	[F117] [nvarchar](255) NULL,
	[F118] [nvarchar](255) NULL,
	[F119] [nvarchar](255) NULL,
	[F120] [nvarchar](255) NULL,
	[F121] [nvarchar](255) NULL,
	[F122] [nvarchar](255) NULL,
	[F123] [nvarchar](255) NULL,
	[F124] [nvarchar](255) NULL,
	[F125] [nvarchar](255) NULL,
	[F126] [nvarchar](255) NULL,
	[F127] [nvarchar](255) NULL,
	[F128] [nvarchar](255) NULL,
	[F129] [nvarchar](255) NULL,
	[F130] [nvarchar](255) NULL,
	[F131] [nvarchar](255) NULL,
	[F132] [nvarchar](255) NULL,
	[F133] [nvarchar](255) NULL,
	[F134] [nvarchar](255) NULL,
	[F135] [nvarchar](255) NULL,
	[F136] [nvarchar](255) NULL,
	[F137] [nvarchar](255) NULL,
	[F138] [nvarchar](255) NULL,
	[F139] [nvarchar](255) NULL,
	[F140] [nvarchar](255) NULL,
	[F141] [nvarchar](255) NULL,
	[F142] [nvarchar](255) NULL,
	[F143] [nvarchar](255) NULL,
	[F144] [nvarchar](255) NULL,
	[F145] [nvarchar](255) NULL,
	[F146] [nvarchar](255) NULL,
	[F147] [nvarchar](255) NULL,
	[F148] [nvarchar](255) NULL,
	[F149] [nvarchar](255) NULL,
	[F150] [nvarchar](255) NULL,
	[F151] [nvarchar](255) NULL,
	[F152] [nvarchar](255) NULL,
	[F153] [nvarchar](255) NULL,
	[F154] [nvarchar](255) NULL,
	[F155] [nvarchar](255) NULL,
	[F156] [nvarchar](255) NULL,
	[F157] [nvarchar](255) NULL,
	[F158] [nvarchar](255) NULL,
	[F159] [nvarchar](255) NULL,
	[F160] [nvarchar](255) NULL,
	[F161] [nvarchar](255) NULL,
	[F162] [nvarchar](255) NULL,
	[F163] [nvarchar](255) NULL,
	[F164] [nvarchar](255) NULL,
	[F165] [nvarchar](255) NULL,
	[F166] [nvarchar](255) NULL,
	[F167] [nvarchar](255) NULL,
	[F168] [nvarchar](255) NULL,
	[F169] [nvarchar](255) NULL,
	[F170] [nvarchar](255) NULL,
	[F171] [nvarchar](255) NULL,
	[F172] [nvarchar](255) NULL,
	[F173] [nvarchar](255) NULL,
	[F174] [nvarchar](255) NULL,
	[F175] [nvarchar](255) NULL,
	[F176] [nvarchar](255) NULL,
	[F177] [nvarchar](255) NULL,
	[F178] [nvarchar](255) NULL,
	[F179] [nvarchar](255) NULL,
	[F180] [nvarchar](255) NULL,
	[F181] [nvarchar](255) NULL,
	[F182] [nvarchar](255) NULL,
	[F183] [nvarchar](255) NULL,
	[F184] [nvarchar](255) NULL,
	[F185] [nvarchar](255) NULL,
	[F186] [nvarchar](255) NULL,
	[F187] [nvarchar](255) NULL,
	[F188] [nvarchar](255) NULL,
	[F189] [nvarchar](255) NULL,
	[F190] [nvarchar](255) NULL,
	[F191] [nvarchar](255) NULL,
	[F192] [nvarchar](255) NULL,
	[F193] [nvarchar](255) NULL,
	[F194] [nvarchar](255) NULL,
	[F195] [nvarchar](255) NULL,
	[F196] [nvarchar](255) NULL,
	[F197] [nvarchar](255) NULL,
	[F198] [nvarchar](255) NULL,
	[F199] [nvarchar](255) NULL,
	[F200] [nvarchar](255) NULL,
	[F201] [nvarchar](255) NULL,
	[F202] [nvarchar](255) NULL,
	[F203] [nvarchar](255) NULL,
	[F204] [nvarchar](255) NULL,
	[F205] [nvarchar](255) NULL,
	[F206] [nvarchar](255) NULL,
	[F207] [nvarchar](255) NULL,
	[F208] [nvarchar](255) NULL,
	[F209] [nvarchar](255) NULL,
	[F210] [nvarchar](255) NULL,
	[F211] [nvarchar](255) NULL,
	[F212] [nvarchar](255) NULL,
	[F213] [nvarchar](255) NULL,
	[F214] [nvarchar](255) NULL,
	[F215] [nvarchar](255) NULL,
	[F216] [nvarchar](255) NULL,
	[F217] [nvarchar](255) NULL,
	[F218] [nvarchar](255) NULL,
	[F219] [nvarchar](255) NULL,
	[F220] [nvarchar](255) NULL,
	[F221] [nvarchar](255) NULL,
	[F222] [nvarchar](255) NULL,
	[F223] [nvarchar](255) NULL,
	[F224] [nvarchar](255) NULL,
	[F225] [nvarchar](255) NULL,
	[F226] [nvarchar](255) NULL,
	[F227] [nvarchar](255) NULL,
	[F228] [nvarchar](255) NULL,
	[F229] [nvarchar](255) NULL,
	[F230] [nvarchar](255) NULL,
	[F231] [nvarchar](255) NULL,
	[F232] [nvarchar](255) NULL,
	[F233] [nvarchar](255) NULL,
	[F234] [nvarchar](255) NULL,
	[F235] [nvarchar](255) NULL,
	[F236] [nvarchar](255) NULL,
	[F237] [nvarchar](255) NULL,
	[F238] [nvarchar](255) NULL,
	[F239] [nvarchar](255) NULL,
	[F240] [nvarchar](255) NULL,
	[F241] [nvarchar](255) NULL,
	[F242] [nvarchar](255) NULL,
	[F243] [nvarchar](255) NULL,
	[F244] [nvarchar](255) NULL,
	[F245] [nvarchar](255) NULL,
	[F246] [nvarchar](255) NULL,
	[F247] [nvarchar](255) NULL,
	[F248] [nvarchar](255) NULL,
	[F249] [nvarchar](255) NULL,
	[F250] [nvarchar](255) NULL,
	[F251] [nvarchar](255) NULL,
	[F252] [nvarchar](255) NULL,
	[F253] [nvarchar](255) NULL,
	[F254] [nvarchar](255) NULL,
	[F255] [nvarchar](255) NULL
) ON [PRIMARY]
GO
