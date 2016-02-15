/**********************
说明：创建iMES系统的初始化管理员权限
**********************/

--use RBPC4NET

--请调整参数值后，再执行。

--for permission of Authority Manager
declare @SubSystem_AppName varchar(32),@AdminRole_2 nvarchar(32),@AdminUserId_2 int, @AdminRoleId_2 int, @AuthorityManager_PermissionId int
set @SubSystem_AppName='HP_iMES_Dashboard'					--子系统在RBPC中的Application Name
set @AdminRole_2='Role_'+@SubSystem_AppName						--要为子系统初始化的Role,赋予其访问子系统的权限(Authority Manager节点的权限)。
  

--===============================赋予AdminUser访问子系统的权限============================

declare @type nvarchar(255), @name nvarchar(100), @login nvarchar(256), @email nvarchar(255), @password nvarchar(255)
declare @domain nvarchar(255), @company nvarchar(255), @department nvarchar(255), @manager nvarchar(255)
--以下如果管理员是域用户for domain adminuser，初始化如下
--set @type='domain'
--set @name='Liu, Xiao-Ling (劉曉玲 ITC)'
--set @login='itc\itc98079'
--set @password=''
--set @email='Liu.Xiao-Ling@itc.inventec'
--set @domain='itc'
--set @company='IEC'
--set @department='eB1-2'
--set @manager='Gao, Rui (高瑞 ITC)'
--or 如果管理员是本地用户for local adminuser，初始化如下
set @type='local'
set @name='admin'
set @login='admin'
set @password='G72IZGCCcBXl1gXtRCUiUQ=='		--11111111
set @email=''																--任意
set @domain='local'
set @company='local'															--必输
set @department='local'													--必输
set @manager=''															--任意

INSERT INTO [Account]([application],[type],[name],[descr],[login],[password],[email],[contact],[extraInformation],[cdt],[udt],[editorId],[domain],[company],[department],[manager],[accountType])
     VALUES(@SubSystem_AppName, @type, @name, '', @login, @password, @email, '', '', getdate(), getdate(), '', @domain, @company, @department, @manager, 'accountuser')



--==============================赋予AdminUser访问子系统Authority Manager的权限==============================
select @AdminUserId_2 = id from Account where login=@login and application=@SubSystem_AppName

--select @AdminUserId_2 = id from Account where login=@AdminUser_2
select @AuthorityManager_PermissionId = id from Permission where [application] = @SubSystem_AppName and [name] = 'Authority Manager'

--初始化子系统的AdminRole
INSERT INTO [Role]([application], [type], [name], [descr], [cdt], [udt], [editorId])
VALUES(@SubSystem_AppName, '1', @AdminRole_2, '', getdate(), getdate(), @AdminUserId_2)

--关联AdminRole和AdminUser
select @AdminRoleId_2 = id from Role where [application] = @SubSystem_AppName and [type] = '1' and [name] = @AdminRole_2

INSERT AccountRole([acct_id], [role_id], [cdt], [udt], [editorId])
VALUES(@AdminUserId_2, @AdminRoleId_2, getdate(), getdate(), @AdminUserId_2)

--赋予AdminRole访问子系统Authority Manager的权限
INSERT RolePermission([role_id], [perm_id], [cdt], [udt], [editorId])
VALUES(@AdminRoleId_2, @AuthorityManager_PermissionId, getdate(), getdate(), @AdminUserId_2)



