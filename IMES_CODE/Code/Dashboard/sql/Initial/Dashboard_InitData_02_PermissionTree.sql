/**********************
作者：李明军
说明：
	application：应用的名称
	type：节点的路径
	name：节点的名称
	target_type：节点的层级
	target_symbol：节点的位置，用于排序
	privilege：节点对应的url
	targetDisplay：节点的显示图标
	constraint_def：是否父节点
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



