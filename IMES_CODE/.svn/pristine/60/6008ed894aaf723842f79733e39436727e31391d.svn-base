			Comdlg32 ActiveX 安装说明
-----------------------------------------------------------------------------------------------------
在本项目中打开文本属性设置对话框或DataSet配置对话框时，会调用机器上的Comdlg32 ActiveX控件，如果Browser的客户机上没有安装这一组件，或者安装了，但没有认证，均会导致异常而不能正常浏览页面和执行。此时请参照如下步骤进行控件安装：

1.查看客户端系统盘System32目录下有无comdlg32.ocx文件，如果没有就将本目录下的同名文件拷贝过去，否则执行下一步。
2.在命令行下输入regsvr32 comdlg32.ocx，对comdlg32.ocx进行注册。
3.双击comdlg32.reg文件，向注册表中写入该控件的认证信息。
4.关闭当前IE，重新打开IE再次访问网站后即可生效。




如果执行完上述操作仍然无法加载，请按以下步骤修改注册表。
1,查看注册表中是否有一下项目HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\ActiveX Compatibility\{F9043C85-F6F2-101A-A3C9-08002B2F49FB},
 如果没有请创建,并在它下面添加字符串值，名称为AlternateCLSID值为{8F0F480A-4366-4737-8265-2AD6FDAC8C31}；添加DWORD值，名称为Compatibility Flags，值为400，选择十六进制。
2,找到HKEY_USERS\S-1-5-21-1146183601-1903393612-7473742-8926\Software\Microsoft\Windows\CurrentVersion\Ext\Stats\{8F0F480A-4366-4737-8265-2AD6FDAC8C31}\iexplore  保留下面的默认，Count，Time，Type等项目，其余的删除掉。
3,查看IE的加载项是否已经有Microsoft Common Dialog Control, version6.0 (SP6)。
对于IE8需要双击该加载项打开详细信息窗口，点击所有站点上允许按钮。