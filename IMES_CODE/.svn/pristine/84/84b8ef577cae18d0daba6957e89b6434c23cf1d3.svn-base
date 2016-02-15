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
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_authorities_edituser, App_Web_addedituser.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit User</title>
</head>
<script type="text/javascript" language="javascript">
	var diagArgs = window.dialogArguments; //undefined
    var id = "";
    if(diagArgs.operateType == "add"){
        document.title = "Add User";
    }else{//edit
        document.title = "Edit User";
    }

	function fOnBodyLoad()
	{
        if(diagArgs.operateType == "edit"){
    		fillContents();
            document.getElementById("login").disabled = true;
        }
	}

	function fillContents()
	{
		if (diagArgs.loginId != "" && diagArgs.loginId != undefined)
		{
			var dtUser = com.inventec.template.manager.TemplateManager.getUserByLoginId(diagArgs.loginId);
			if (null != dtUser.error)
			{
				alert(dtUser.error.Message);
				return;
			}
			id = dtUser.value.Rows[0]["id"];
			document.getElementById("idName").value = dtUser.value.Rows[0]["name"];
			document.getElementById("idDescription").value = dtUser.value.Rows[0]["description"];
			document.getElementById("login").value = LTrim(RTrim(diagArgs.loginId));
		}
	}

	function onClickOK()
	{
		var objLogin = document.getElementById("login");

		objLogin.value = Trim(objLogin.value); 

		if (objLogin.value == "")
		{
			objLogin.select();
			alert("<%=Constants.PLEASE_ENTER_A_VALID%>" + "login!");
			return;
		}
		if (checkTextLength("login", 20, "Login"))
		{
			return;
		}

		if (checkTextLength("idName", 20, "Name"))
		{
			return;
		}

		if (checkTextLength("idDescription", 200, "Description"))
		{
			return;
		}

		
		var result = com.inventec.template.manager.TemplateManager.saveUser(id, document.getElementById("login").value, document.getElementById("idName").value, document.getElementById("idDescription").value, document.getElementById("user").value);
		
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
    <asp:HiddenField ID="user" runat="server"></asp:HiddenField>
</form>
<table id="idMainTable" border=0 style="width:100%;height:100%;" cellpadding="10px">
	<tr>
		<td style="width:1%;" nowrap="true">
			<b>
			Login:
			</b>
		</td>
		<td>
			<input id="login" type="text" style="width:100%;" maxlength="20" />
		</td>
	</tr>
	<tr>
		<td style="width:1%;" nowrap="true">
			<b>
			Name:
			</b>
		</td>
		<td>
			<input id="idName" type="text" style="width:100%;" maxlength="20" />
		</td>
	</tr>
	<tr>
		<td colspan="2" nowrap="true">
			<b>
			Description:
			</b>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<textarea id="idDescription" rows="5" cols="6" style="width:100%;"></textarea>
		</td>
	</tr>
	<tr>
		<td colspan="2" nowrap="true" align="right">
			<button id="idBtnOK" onclick="onClickOK();">OK</button>
			&nbsp;&nbsp;&nbsp;
			<button id="idBtnCancel" onclick="onClickCancel();">Cancel</button>
			&nbsp;&nbsp;&nbsp;
		</td>
	</tr>
</table>
</body>
</html>
