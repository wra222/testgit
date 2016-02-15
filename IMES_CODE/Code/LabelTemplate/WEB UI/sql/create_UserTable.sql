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