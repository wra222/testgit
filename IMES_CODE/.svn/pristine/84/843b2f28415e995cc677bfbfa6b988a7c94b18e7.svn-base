/*    此SQL DDL 脚本由Microsoft Visual Studio (发布日期:LOCAL BUILD) 生成。                           */

/*    所用驱动程序: Microsoft Visual Studio －Microsoft SQL Server 驱动程序。                           */
/*    文档: C:\Documents and Settings\itc207024\桌面\DB Design.vsd。                              */
/*    创建时间: 2009年月日13:51。                                                                */
/*    操作: 来自Visio 生成向导。                                                                     */
/*    已连接的数据源: 没有连接。                                                                         */
/*    已连接的服务器: 没有连接。                                                                         */
/*    已连接的数据库: 不适用。                                                                          */



SET QUOTED_IDENTIFIER ON

go


use master  

go

create database "labelTemplateDB"  COLLATE Chinese_Taiwan_Stroke_BIN

go

use "labelTemplateDB"  

go

/****** Object:  Table [dbo].[T_User]    Script Date: 10/25/2011 13:49:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_User](
	[id] [char](32) NOT NULL,
	[login] [char](20) NOT NULL,
	[name] [nvarchar](20) NULL,
	[description] [nvarchar](200) NULL,
	[createTime] [datetime] NULL,
	[updateTime] [datetime] NULL,
	[author] [char](32) NULL,
 CONSTRAINT [PK_T_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

create table "T_StructureFile" ( 
	"id" char(32) not null,
	"structureXml" [nvarchar](max) null,
	"pageXml" [nvarchar](max) null,
        "structureStream" image null,
	"orignalXml" [nvarchar](max) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_StructureFile"
	add constraint "T_StructureFile_PK" primary key ("id")   


go

create table "T_Template" ( 
	"id" char(32) not null,
	"structureId" char(32) null,
	"name" nvarchar(200) null,
	"editorId" char(32) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_Template"
	add constraint "T_Template_PK" primary key ("id")   


go



create table "T_DataSet" ( 
	"id" char(32) not null,
	"templateId" char(32) null,
	"databaseId" char(32) null,
	"dateSetName" varchar(100) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_DataSet"
	add constraint "T_DataSet_PK" primary key ("id")   


go

create table "T_DataServer_Database" ( 
	"id" char(32) not null,
	"dataServerId" char(32) null,
	"databaseName" nvarchar(200) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_DataServer_Database"
	add constraint "T_DataServer_Database_PK" primary key ("id")   


go

create table "T_FunctionTree" ( 
	"id" char(32) not null,
	"parentId" char(32) null,
	"name" nvarchar(200) null,
	"targetId" char(32) null,
	"type" int null,
	"comment" nvarchar(500) null,
	"editorId" char(32) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_FunctionTree"
	add constraint "T_FunctionTree_PK" primary key ("id")   


go

create table "T_DataServer" ( 
	"id" char(32) not null,
	"name" nvarchar(200) null,
	"alias" nvarchar(200) null,
	"userName" nvarchar(50) null,
	"password" nvarchar(100) null,
	"editorId" char(32) null,
	"createTime" datetime null,
	"updateTime" datetime null,
	"lifeCycleStatus" int null)  

go

alter table "T_DataServer"
	add constraint "T_DataServer_PK" primary key ("id")   


go

alter table "T_Template"
	add constraint "T_StructureFile_T_Template_FK1" foreign key (
		"structureId")
	 references "T_StructureFile" (
		"id") on update no action on delete no action  

go

alter table "T_DataSet"
	add constraint "T_Template_T_DataSet_FK1" foreign key (
		"templateId")
	 references "T_Template" (
		"id") on update no action on delete no action  

go

alter table "T_DataSet"
	add constraint "T_DataServer_Database_T_DataSet_FK1" foreign key (
		"databaseId")
	 references "T_DataServer_Database" (
		"id") on update no action on delete no action  

go

alter table "T_DataServer_Database"
	add constraint "T_DataServer_T_DataServer_Database_FK1" foreign key (
		"dataServerId")
	 references "T_DataServer" (
		"id") on update no action on delete no action  

go

/****** Object:  Table [dbo].[T_TemplateChangeList]    Script Date: 01/18/2010 14:27:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_TemplateChangeList](
	[id] [char](32) NOT NULL,
	[templateId] [char](32) NOT NULL,
	[description] [nvarchar](255) NOT NULL,
	[editorId] [char](32) NULL,
	[createTime] [datetime] NULL,
	[updateTime] [datetime] NULL,
	[lifeCycleStatus] [int] NULL,
 CONSTRAINT [PK_T_TemplateChangeList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
/* 这是Microsoft Visual Studio 生成的SQL DDL 脚本的末尾。                                             */
