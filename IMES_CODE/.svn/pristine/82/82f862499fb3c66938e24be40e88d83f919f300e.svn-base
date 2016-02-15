/**********************
说明：内嵌子系统到Portal的下拉菜单中。
**********************/

--use RBPC4NET

--请调整参数值后，再执行。

--注：为了report项目做特殊处理。因为report与portal不使用同一个rbpc，互相没有关系，但要求report必须集成在portal内
--正常情况：@authenticate=authenticate，必须使用iMES_InitData_AdminUser.sql做对应的初始化；
--Report：@authenticate==''，则不需要iMES_InitData_AdminUser.sql做对应的初始化


declare @RBPC_AppName nvarchar(32),@displayName nvarchar(64),@Editor nvarchar(256) 
declare @authenticate nvarchar(255),@displayPic nvarchar(255),@execPath nvarchar(255),@description nvarchar(255),@openMode nvarchar(32)
declare @SubSystem_AppName nvarchar(255)

set @RBPC_AppName='portal'													--[application] portal系统在RBPC中的Application Name
set @authenticate='authenticate'														--[type]两个值(authenticate,'') authenticate：通过权限认证，子系统才可以显示在Portal下拉项中；''：不需要权限认证，直接显示在Portal下拉项，子系统打开后有自己的登录页面
set @SubSystem_AppName='HP_iMES_Dashboard'													--[name] 子系统在RBPC中的Application Name
set @description='iMES Dashboard for HP in the portal system.'					--[descr] Portal系统下拉菜单中子系统的描述
set @openMode='1'															--[target_type] 0：子系统在Portal中打开；1：子系统在Portal外另外的弹出窗口中打开
set @execPath='http://itc-qa5-fis1/IMES2011_Dashboard/Default.aspx?customer=&Token='		--[privilege] 子系统Path
set @displayName='Dashboard'												--[privilegeDisplay] Portal系统下拉菜单中子系统的名称（不是子系统在RBPC中的Application Name），PortalUserManager也算是Portal的一个子系统。
set @displayPic='../../images/portal/sub icon5.gif'							--[targetDisplay] Portal系统下拉菜单中子系统的图标
set @Editor='itc\itc98079'

declare @EditorId int
select @EditorId=id from Account where login=@Editor and application='all'


INSERT Permission([application], [type], [name], [descr], [target_type], [target_symbol], [privilege], [constraint_def], 
	[privilegeDisplay], [targetDisplay], [cdt], [udt], [editorId])
VALUES(@RBPC_AppName, @authenticate, @SubSystem_AppName, @description, @openMode, '', @execPath, '', @displayName, @displayPic, getdate(), getdate(), @EditorId)

----这句话来自于iMES_InitData_PermissionForPAK，正式的里面要去掉
--insert Permission(application, type, name, target_type, target_symbol, privilege, targetDisplay, constraint_def, descr, cdt)
--values(@SubSystem_AppName, 'RMA|Management|Authority Manager', 'Authority Manager', 2, '1.4.1.0001', './UserManager/aspx/authorities/accountauthority.aspx', 'Authority.gif', 'Child', 'MMCM001', getdate())
