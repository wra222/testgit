insert into T_FunctionTree([id],[parentId],[name],[targetId],[type],[comment],[editorId],[createTime],[updateTime],[lifeCycleStatus])
select 'f0000000000000000000000000000001',null, 'Data Source Setting',null, 0,'', null, getdate(), getdate(),1
go		

insert into T_FunctionTree([id],[parentId],[name],[targetId],[type],[comment],[editorId],[createTime],[updateTime],[lifeCycleStatus])
select 'f0000000000000000000000000000002', null,'Template',null, 1,'', null, getdate(), getdate(),1
go
