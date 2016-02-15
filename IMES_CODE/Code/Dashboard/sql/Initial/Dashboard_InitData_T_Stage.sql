/* IMES2012_GetData 会随着数据库名修改 */
/* Stage与Descr同时改, Type与判断有关应该不动 */

USE [IMES2012_GetData]

INSERT INTO [Dashboard_Stage_Base]
           ([Stage]
           ,[Type]
           ,[Stage_Type])
     VALUES
           ('FA'
           ,1
           ,1)

INSERT INTO [Dashboard_Stage_Base]
           ([Stage]
           ,[Type]
           ,[Stage_Type])
     VALUES
           ('PAK'
           ,1
           ,2)
INSERT INTO [Dashboard_Stage_Base]
           ([Stage]
           ,[Type]
           ,[Stage_Type])
     VALUES
           ('SA'
           ,2
           ,3)
INSERT INTO [Dashboard_Stage_Base]
           ([Stage]
           ,[Type]
           ,[Stage_Type])
     VALUES
           ('SMT'
           ,3
           ,4)

----create view from Dashboard_Stage_Base------------------
GO
/****** 对象:  View [dbo].[Dashboard_Stage]    脚本日期: 12/02/2011 16:45:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Dashboard_Stage]
AS
select a.[Stage],a.[Stage] AS Descr,a.[Type],'' AS Editor FROM
(select [Type], [Stage]=REPLACE(stuff((select ','+[Stage] from [Dashboard_Stage_Base] t 
where [Type]=[Dashboard_Stage_Base].[Type] for xml 
path('') ), 1, 1, ''),',',' & ')   from [Dashboard_Stage_Base] group by [Type]) a

GO
