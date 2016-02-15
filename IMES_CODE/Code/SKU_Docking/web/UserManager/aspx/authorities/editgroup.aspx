<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: editgroup.aspx
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2008-12-19   itc98079     Create 
 * qa bug no:ITC-1103-0094
 * Known issues:Any restrictions about this file
--%>
<%@ Page Language="C#" AutoEventWireup="true" Theme="MainTheme" CodeFile="editgroup.aspx.cs" Inherits="webroot_aspx_authorities_editgroup" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Group</title>
</head>
<script type="text/javascript" language="javascript">
	var diagArgs = window.dialogArguments; //undefined
	var editorID = diagArgs.editorID;

    if(diagArgs.operateType == "add"){
        document.title = "Add Group";
    }else{//edit
        document.title = "Edit Group";
    }

	function fOnBodyLoad()
	{
		fillContents();
	}

	function fillContents()
	{
		if (diagArgs.groupID != "" && diagArgs.groupID != undefined)
		{
			var objGroupName = document.getElementById("idGroupName");
			var objComment = document.getElementById("idComment");
			var objGroupID = document.getElementById("idHidGroupID");
			var dtGroupInfo = webroot_aspx_authorities_editgroup.getGroupInfo(diagArgs.groupID);
			if (null != dtGroupInfo.error)
			{
				alert(dtGroupInfo.error.Message);
				return;
			}
			objGroupName.value = dtGroupInfo.value.Rows[0]["name"];
			objComment.value = dtGroupInfo.value.Rows[0]["comment"];
			objGroupID.value = diagArgs.groupID;
		}
	}

	function onClickOK()
	{
		var objGroupName = document.getElementById("idGroupName");
		var objComment = document.getElementById("idComment");
		var objGroupID = document.getElementById("idHidGroupID");

		objGroupName.value = Trim(objGroupName.value); 
		objComment.value = Trim(objComment.value);

		if (objGroupName.value == "")
		{
			objGroupName.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Group Name!");
			return;
		}
		if (checkTextLength("idGroupName", 50, "Group Name"))
		{
			return;
		}

		if (checkTextLength("idComment", 255, "Comment"))
		{
			return;
		}

		/*if (!isNumCharString(objGroupName.value))
		{
			objGroupName.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Group Name!");
			return;
		}

		if (!isNumCharString(objComment.value))
		{
			objComment.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "Comment!");
			return;
		}*/
		
		var result = webroot_aspx_authorities_editgroup.saveGroup(objGroupID.value, objGroupName.value, objComment.value, editorID);
		
		if (null != result.error)
		{
			alert(result.error.Message)
		}
		else
		{
			window.returnValue = result.value;
			window.close();
		}
	}

	/*
	function
	*/
	function onClickCancel()
	{
		window.returnValue = "cancel";
		window.close();
	}

	/*
	function
	*/
	function submitData()
	{
		window.returnValue = "";
		window.close();
	}

	/*
	function
	*/
	function LTrim(s)
	{
		try {
			do {
				if (null == s || s.length <= 0) break;
				var whitespace = " \t\r\n";
				if (whitespace.indexOf(s.charAt(0)) >= 0)
				{
					var len = s.length;
					var index;
					for (index = 0; index < len; ++index) {
						if (whitespace.indexOf(s.charAt(index)) < 0) break;
					}
					if (index < len) s = s.substring(index); else s = "";
				}
			} while (false);
		} catch (Cb) {
			s = "";
		}
		return s;
	}

	/*
	function
	*/
	function RTrim(s) 
	{
		try {
			do {
				if (null == s || s.length <= 0) break;
				var len = s.length;
				var whitespace = " \t\r\n";
				if (whitespace.indexOf(s.charAt(len - 1)) >= 0)
				{
					var index;
					for (index = len - 1; index >= 0; --index) {
						if (whitespace.indexOf(s.charAt(index)) == -1) break;
					}
					if (index >= 0) s = s.substring(0, index + 1); else s = "";
				}
			} while (false);
		} catch (Db) {
			s = "";
		}
		return s;
	}

	/*
	function
	*/
	function Trim(s)
	{
		return RTrim(LTrim(s));
	}

	function isNumCharString(strValue)
	{
		if (/[^a-zA-Z0-9\s]/.test(strValue))
			return false;
		return true;
	}

	function charCodeAtTest(str)
	{
		//var str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //初始化变量。
		for (var i = 0; i < str.length; ++i)
		{
			var n = str.charCodeAt(i); //获取位置n 上字符的Unicode 值。
			if (n < 33 || n > 126)
			{
				return false;
			}
		}

		return true;//返回该值。
	}

	function checkTextLength(objID, iLength, fieldName)
	{
		var obj = document.getElementById(objID);
		var inputMsg = obj.value;
		if (inputMsg.length > iLength)
		{
			alert(fieldName + " cannot exceed the maximum of " + iLength + ".");
			obj.focus();
			var r = obj.createTextRange();
			r.collapse(false);
			r.select();
			return true; //exceeded
		}
		else
		{
			return false;
		}
	}
</script>
<body onload="fOnBodyLoad();" class="dialogBody" style="background-color:rgb(210,210,210);">
<form id="idFormEditGroup" runat="server">
</form>
<table id="idMainTable" style="width:100%;height:100%;" cellpadding="10px">
	<tr>
		<td style="width:1%;" nowrap="true">
			<b>
			Group Name:
			</b>
		</td>
		<td>
			<%--<asp:TextBox ID="idGroupName" runat="server" Width="100%"></asp:TextBox>--%>
			<input id="idGroupName" type="text" style="width:100%;" maxlength="20" />
		</td>
	</tr>
	<tr>
		<td colspan="2" nowrap="true">
			<b>
			Comment:
			</b>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<%--<asp:TextBox ID="idComment" runat="server" TextMode="MultiLine" Rows="5" Columns="6" Width="100%"></asp:TextBox>--%>
			<textarea id="idComment" rows="5" cols="6" style="width:100%;"></textarea>
		</td>
	</tr>
	<tr>
		<td colspan="2" nowrap="true" align="right">
			<button id="idBtnOK" onclick="onClickOK();">OK</button>
			<%--<asp:Button ID="idBtnOK" runat="server" Text="OK" OnClick="onClickOK" Width="60px" />--%>
			&nbsp;&nbsp;&nbsp;
			<button id="idBtnCancel" onclick="onClickCancel();">Cancel</button>
			&nbsp;&nbsp;&nbsp;
		</td>
	</tr>
</table>
<input id="idHidGroupID" type="hidden" value="" />
</body>
</html>
