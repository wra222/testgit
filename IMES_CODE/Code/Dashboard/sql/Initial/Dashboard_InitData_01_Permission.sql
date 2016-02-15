/**********************
˵������Ƕ��ϵͳ��Portal�������˵��С�
**********************/

--use RBPC4NET

--���������ֵ����ִ�С�

--ע��Ϊ��report��Ŀ�����⴦����Ϊreport��portal��ʹ��ͬһ��rbpc������û�й�ϵ����Ҫ��report���뼯����portal��
--���������@authenticate=authenticate������ʹ��iMES_InitData_AdminUser.sql����Ӧ�ĳ�ʼ����
--Report��@authenticate==''������ҪiMES_InitData_AdminUser.sql����Ӧ�ĳ�ʼ��


declare @RBPC_AppName nvarchar(32),@displayName nvarchar(64),@Editor nvarchar(256) 
declare @authenticate nvarchar(255),@displayPic nvarchar(255),@execPath nvarchar(255),@description nvarchar(255),@openMode nvarchar(32)
declare @SubSystem_AppName nvarchar(255)

set @RBPC_AppName='portal'													--[application] portalϵͳ��RBPC�е�Application Name
set @authenticate='authenticate'														--[type]����ֵ(authenticate,'') authenticate��ͨ��Ȩ����֤����ϵͳ�ſ�����ʾ��Portal�������У�''������ҪȨ����֤��ֱ����ʾ��Portal�������ϵͳ�򿪺����Լ��ĵ�¼ҳ��
set @SubSystem_AppName='HP_iMES_Dashboard'													--[name] ��ϵͳ��RBPC�е�Application Name
set @description='iMES Dashboard for HP in the portal system.'					--[descr] Portalϵͳ�����˵�����ϵͳ������
set @openMode='1'															--[target_type] 0����ϵͳ��Portal�д򿪣�1����ϵͳ��Portal������ĵ��������д�
set @execPath='http://itc-qa5-fis1/IMES2011_Dashboard/Default.aspx?customer=&Token='		--[privilege] ��ϵͳPath
set @displayName='Dashboard'												--[privilegeDisplay] Portalϵͳ�����˵�����ϵͳ�����ƣ�������ϵͳ��RBPC�е�Application Name����PortalUserManagerҲ����Portal��һ����ϵͳ��
set @displayPic='../../images/portal/sub icon5.gif'							--[targetDisplay] Portalϵͳ�����˵�����ϵͳ��ͼ��
set @Editor='itc\itc98079'

declare @EditorId int
select @EditorId=id from Account where login=@Editor and application='all'


INSERT Permission([application], [type], [name], [descr], [target_type], [target_symbol], [privilege], [constraint_def], 
	[privilegeDisplay], [targetDisplay], [cdt], [udt], [editorId])
VALUES(@RBPC_AppName, @authenticate, @SubSystem_AppName, @description, @openMode, '', @execPath, '', @displayName, @displayPic, getdate(), getdate(), @EditorId)

----��仰������iMES_InitData_PermissionForPAK����ʽ������Ҫȥ��
--insert Permission(application, type, name, target_type, target_symbol, privilege, targetDisplay, constraint_def, descr, cdt)
--values(@SubSystem_AppName, 'RMA|Management|Authority Manager', 'Authority Manager', 2, '1.4.1.0001', './UserManager/aspx/authorities/accountauthority.aspx', 'Authority.gif', 'Child', 'MMCM001', getdate())
