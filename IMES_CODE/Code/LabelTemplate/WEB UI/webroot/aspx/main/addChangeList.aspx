<%--
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description: addChangeList.aspx
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-11-19   itc98079     Create 
 * Known issues:Any restrictions about this file
--%>
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_main_addChangeList, App_Web_addchangelist.aspx.39cd9290" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Save</title>
</head>
<script type="text/javascript" language="javascript">
    var gFromNormal = false;
</script>

<script for=window event=onunload language=javascript>
    if(!gFromNormal)
    {
	    window.returnValue = "cancel";
	}
</script>

<script type="text/javascript" language="javascript">
	var diagArgs = window.dialogArguments; //undefined
    //diagArgs.operateType


	function onClickOK()
	{
	
		var objDescription = document.getElementById("idDescription");

		objDescription.value = Trim(objDescription.value);


		if (checkTextLength("idDescription", 100, "Description"))
		{
			return;
		}

		
        gFromNormal = true;
		window.returnValue = objDescription.value;
		window.close();
	}

	/*
	function
	*/
	function onClickCancel()
	{
        gFromNormal = true;
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
	
    function CheckLength()
    {   
        if(document.getElementById("idDescription").value.length>100)
        {   
            document.getElementById("idDescription").value = document.getElementById("idDescription").value.substring(0,100);
        }
    } 	
</script>
<body class="dialogBody">
<form id="idFormEditTemplate" runat="server">
	<table id="idMainTable" style="width:100%;height:100%;" border=0 cellpadding="0px" cellspacing=0>
		<tr>
			<td nowrap="true">
				Description:
			</td>
		</tr>
		<tr>
			<td>
				<%--<asp:TextBox ID="idDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="6" Width="100%" onpropertychange="CheckLength();"></asp:TextBox>--%>
				<textarea id="idDescription" rows="5" cols="6" style="height:110;width:100%;"></textarea><br>(<%=Resources.VisualTemplate.commentLength%>)
			</td>
		</tr>
		<tr><td height=5></td></tr>
		<tr>
			<td nowrap="true" align="right">
				<button id="idBtnOK" onclick="onClickOK();" accesskey="O" title="Alt+O">OK</button>
				&nbsp;&nbsp;&nbsp;
				<button id="idBtnCancel" onclick="onClickCancel();" accesskey="C" title="Alt+C">Cancel</button>
			</td>
		</tr>
	</table>
</form>
</body>
</html>
