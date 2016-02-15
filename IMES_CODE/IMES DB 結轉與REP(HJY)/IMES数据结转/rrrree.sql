USE [HPIMES_TR]
GO
/****** Object:  Table [dbo].[ArchiveTableLog]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveTableLog](
	[TableName] [varchar](64) NULL,
	[BeforeCount] [int] NULL,
	[ArchiveCount] [int] NULL,
	[TimeStamp] [varchar](64) NULL,
	[Item] [varchar](32) NULL,
	[IsCopySuccess] [varchar](8) NULL,
	[IsDeleteSuccess] [varchar](8) NULL,
	[Msg] [varchar](max) NULL,
	[CopyTime] [int] NULL,
	[DeleteTime] [int] NULL,
	[DeleteSQL] [varchar](max) NULL,
	[Udt] [datetime] NULL,
	[Cdt] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveTableList]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveTableList](
	[TableName] [varchar](50) NULL,
	[Item] [varchar](50) NULL,
	[PKName2] [varchar](50) NULL,
	[FKName] [varchar](64) NULL,
	[SQLScript] [varchar](max) NULL,
	[DeleteOrder] [int] NULL,
	[IsNeedXML] [varchar](8) NULL,
	[Enable] [varchar](8) NULL,
	[ViewName] [varchar](64) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveMainTable]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveMainTable](
	[Item] [varchar](50) NULL,
	[TableName] [varchar](50) NULL,
	[PKName] [varchar](50) NULL,
	[SQLScript] [varchar](max) NULL,
	[DeleteOrder] [int] NULL,
	[IsNeedXML] [varchar](8) NULL,
	[Enable] [varchar](8) NULL,
	[ViewName] [varchar](64) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveLog]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveLog](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
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
/****** Object:  Table [dbo].[ArchiveIDList]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveIDList](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
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
/****** Object:  Table [dbo].[ArchiveFKTableList]    Script Date: 09/02/2014 09:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArchiveFKTableList](
	[TableName] [varchar](50) NULL,
	[ParentTableName] [varchar](50) NULL,
	[FKName] [varchar](50) NULL,
	[IsNeedXML] [varchar](8) NULL,
	[ViewName] [varchar](64) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArchiveAllFKConstraint]    Script Date: 09/02/2014 09:52:20 ******/
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
