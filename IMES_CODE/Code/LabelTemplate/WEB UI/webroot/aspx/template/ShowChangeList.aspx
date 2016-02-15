<%--
 * INVENTEC corporation (c)2008 all rights reserved.
 * Description: selectuser.aspx
 * Update:
 * Date         Name            Reason
 * ========== ================= =====================================
 * 2008-12-19   itc98079     Create
 * Known issues:Any restrictions about this file
--%>

<%@ page language="C#" autoeventwireup="true" theme="MainTheme" inherits="webroot_aspx_template_showchangelist, App_Web_showchangelist.aspx.7a399c77" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="log4net" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Show Change List</title>
</head>
<fis:header id="header1" runat="server"/>
<script language="javascript" type="text/javascript">
	var ShowItemTable = null;
	var diagArgs = window.dialogArguments; //undefined
	var templateId = diagArgs.templateId;
</script>

<script language="javascript" type="text/javascript">
	function fOnBodyLoad()
	{
		getRecordSet();
	    document.getElementById("templateName").value = diagArgs.templateName;
	}




	function onClickCancel()
	{
		window.close();
	}



	/*
	' ======== ============ =============================
	' Description: 利用commonFunction.js中的createRecordSet
	'              方法获得表格的记录集
	' Author: LZ
	' Side Effects:
	' Date:2008-11
	' ======== ============ =============================
	*/
	function getRecordSet()
	{
		var rtn = webroot_aspx_template_showchangelist.getTableData(templateId);

		//后台提供的DataTable
		if (rtn.error != null)
		{
			alert(rtn.error.Message);
		}
		else
		{
			//生成记录集
			var rs = createRecordSet(rtn.value);
			//显示表格
			showTable(rs);
		}
	}

	/*
	' ======== ============ =============================
	' Description: display table edit
	' Author: LZ
	' Side Effects:
	' Date:2008-11
	' ======== ============ =============================
	*/
	function showTable(rs)
	{
		var tableWidth = 560;
		var tableHeight = document.body.clientHeight - 140;
		//alert(document.body.clientWidth);
		//alert(document.body.clientHeight);
		var tableWidth1 = tableWidth - 18;
		if (ShowItemTable == null)
		{
			ShowItemTable = new clsTable(rs, "ShowItemTable");

//			ShowItemTable.HeadStyle("background-color:rgb(133,133,133);font-size:9pt;color:white");//表头背景
//			ShowItemTable.arrColor = new Array("rgb(226,235,204)","rgb(212,218,195)");//表体间隔行的颜色
//			ShowItemTable.TableLineColor = "rgb(166,166,166)";//表头线
//			ShowItemTable.LightBKColor = "rgb(169,217,86)";//高亮行的背景色
//			ShowItemTable.LightColor = "black";//高亮行的前景色



			ShowItemTable.Height = tableHeight;
			ShowItemTable.TableWidth = tableWidth;
			ShowItemTable.adjustRateW = 1.006;
			ShowItemTable.UseSort = "TDCData";
			ShowItemTable.ScreenWidth = -1;
			ShowItemTable.ControlPath = "../../commoncontrol/tableEdit/";
			ShowItemTable.Widths = new Array(tableWidth1 * 0.5, tableWidth1 * 0.20, tableWidth1 * 0.3);
			ShowItemTable.FieldsType = new Array(0, 0, 0);
			ShowItemTable.HideColumn = new Array(true, true, true);
			ShowItemTable.Title = true;
			ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
		}
		ShowItemTable.rs_main = rs;
		idDivTableEdit.innerHTML = ShowItemTable.Display();
		//HideWait();
	}






</script>
<body onload="fOnBodyLoad();" class="dialogBody">

	<table id="idMainTable" cellpadding="10px" border=0 bordercolor=red>
		<tr>
			<td nowrap="true">
				<b>
				Template Name:
				</b>
				<%--<asp:TextBox ID="idGroupName" runat="server" Width="100%"></asp:TextBox>--%>
				<input id="templateName" type="text" style="width:450px" readonly/>
			</td>
		</tr>
		<tr>
			<td nowrap="true">
	            <div id="idDivTableEdit"></div>
			</td>
		</tr>
		<tr>
		    <td nowrap="true" align="right">
			    <button id="idBtnCancel" onclick="onClickCancel();" style="width:90px">
				    Close
			    </button>
		    </td>
	    </tr>
    </table>
<fis:footer id="footer1" runat="server"/>

<form id="form1" runat="server">
</form>
</body>
</html>