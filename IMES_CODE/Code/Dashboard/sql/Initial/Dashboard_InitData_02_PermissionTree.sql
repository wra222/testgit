/**********************
���ߣ�������
˵����
	application��Ӧ�õ�����
	type���ڵ��·��
	name���ڵ������
	target_type���ڵ�Ĳ㼶
	target_symbol���ڵ��λ�ã���������
	privilege���ڵ��Ӧ��url
	targetDisplay���ڵ����ʾͼ��
	constraint_def���Ƿ񸸽ڵ�
	descr: P Code
**********************/
declare @Customer varchar(20), @RBPC_AppName varchar(32)
set @Customer = 'HP'
set @RBPC_AppName = @Customer + '_iMES_Dashboard'

--use RBPC4NET

--Root node
insert Permission(application, type, name, target_type, target_symbol, privilege, targetDisplay, constraint_def, descr, cdt)
values(@RBPC_AppName, 'Dashboard', 'Dashboard', 0, '1', '', '', 'Parent', '', getdate())

--First parent node
insert Permission(application, type, name, target_type, target_symbol, privilege, targetDisplay, constraint_def, descr, cdt)
values(@RBPC_AppName, 'Dashboard|Maintain', 'Maintain', 1, '1.1', '', '', 'Parent', '', getdate())

insert Permission(application, type, name, target_type, target_symbol, privilege, targetDisplay, constraint_def, descr, cdt)
values(@RBPC_AppName, 'Dashboard|Authority Manager', 'Authority Manager', 1, '1.4', '', '', 'Parent', '', getdate())



