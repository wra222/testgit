<%--
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: addeditlocaluser.aspx
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2009-11-19   itc98079        Create 
 * qa bug no:ITC-1096-0017 ITC-1096-0027
 * Known issues:Any restrictions about this file
--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addeditlocaluser.aspx.cs" Inherits="webroot_aspx_authorities_addeditlocaluser" Trace="false"%>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        input.userproperties
        {
         word-wrap:normal;
         margin:0 0 0 0;
         width:100%;
        }
    
    </style>
    
</head>
<script type="text/javascript" language="javascript">
    var gFromNormal = false;
</script>

<script for=window event=onunload language=javascript>
    if(!gFromNormal)
    {
	    window.returnValue = "cancel";
	}else{
	    window.returnValue = "ok/add";
	}
</script>
<script type="text/javascript" language="javascript">
	var diagArgs = window.dialogArguments; //undefined
	var interfaceType = diagArgs.interfaceType;
	var localUserID = diagArgs.localUserID;
	var editorID = diagArgs.editorID;

    if(diagArgs.interfaceType == "add"){
        document.title = "Add Local User";
    }else{//edit
        document.title = "Edit Local User";
    }


    function fOnBodyLoad()
    {
        if(diagArgs.interfaceType == "add"){
        
        }else{//edit
    	    fillContents();
            document.getElementById("idBtnAdd").disabled = true;
            document.getElementById("idLogin").disabled = true;
        }
    }


	function fillContents()
	{
		if (localUserID != "" && localUserID != undefined)
		{
			var dtLocalUserInfo = webroot_aspx_authorities_addeditlocaluser.getLocalUser(localUserID);
			if (null != dtLocalUserInfo.error)
			{
				alert(dtLocalUserInfo.error.Message);
				return;
			}
			
			fillLocalUserInfos(dtLocalUserInfo);
		}
	}

    //“\”不能输入
	function CheckBackslash(obj)
	{
	
        var strValue = obj.value;
        if(strValue.indexOf("\\") >= 0)
		{
            alert("Login is not allowed to contain \"\\\"!");
            return true;
        }
        return false;
	}

	function onClickSave(par)
	{
		var objIdLocalUser = document.getElementById("idLocalUser");
		var objLogin = document.getElementById("idLogin");
		var objName = document.getElementById("idName");
		var objPassword = document.getElementById("idPassword");
		var objConfirmPassword = document.getElementById("idConfirmPassword");
		var objCompany = document.getElementById("idCompany");
		var objDepartment = document.getElementById("idDepartment");
		var objEMail = document.getElementById("idEMail");


		//login不能为空
		if((Trim(objLogin.value)).length == 0){
		    alert("Login is not allowed to be empty!");
		    return;
		}
        if(CheckBackslash(objLogin)) return;
        
		//name不能为空
		if((Trim(objName.value)).length == 0){
		    alert("Name is not allowed to be empty!");
		    return;
		}

		//company不能为空
		if((Trim(objCompany.value)).length == 0){
		    alert("Company is not allowed to be empty!");
		    return;
		}

		//department不能为空
		if((Trim(objDepartment.value)).length == 0){
		    alert("Department is not allowed to be empty!");
		    return;
		}


        if(Trim(objPassword.value) != Trim(objConfirmPassword.value))
        {
            alert("Password and ConfirmPassword are not the same!");
            return;
        }

		if(objIdLocalUser.value == ""){//add

		    if((Trim(objPassword.value)).length < 6 || (Trim(objPassword.value)).length > 12)
		    {
	            alert("The length of Password should be between 6 and 12!");
	            return;
            }    		    
            
	        if(Trim(objPassword.value) != Trim(objConfirmPassword.value))
	        {
	            alert("Password and ConfirmPassword are not the same!");
	            return;
	        }

        }else{//edit
        
   		    if((Trim(objPassword.value)).length != 0){

		        if((Trim(objPassword.value)).length < 6 || (Trim(objPassword.value)).length > 12)
		        {
	                alert("The length of Password should be between 6 and 12!");
	                return;
                }    		    
                
	            if(Trim(objPassword.value) != Trim(objConfirmPassword.value))
	            {
	                alert("Password and ConfirmPassword are not the same!");
	                return;
	            }
		    
   		    }
		}
		

		var result = webroot_aspx_authorities_addeditlocaluser.saveLocalUser(objIdLocalUser.value, objLogin.value, objName.value, objPassword.value, objCompany.value, objDepartment.value, objEMail.value, editorID);
		
		if (null != result.error)
		{
		    alert(result.error.Message);
		}
		else
		{
		    if(par == "ok"){
		        gFromNormal = true;
			    window.returnValue = result.value;
    			window.close();
			}else{//"add"
		        gFromNormal = true;
			    fillLocalUserInfos("");
			}
		}
	}
	
	function fillLocalUserInfos(par){
		var objIdLocalUser = document.getElementById("idLocalUser");
		var objLogin = document.getElementById("idLogin");
		var objName = document.getElementById("idName");
		var objPassword = document.getElementById("idPassword");
		var objConfirmPassword = document.getElementById("idConfirmPassword");
		var objCompany = document.getElementById("idCompany");
		var objDepartment = document.getElementById("idDepartment");
		var objEMail = document.getElementById("idEMail");

        if(par == ""){

	        objIdLocalUser.value = "";
	        objLogin.value = "";
	        objName.value="";
	        objPassword.value = "";
	        objConfirmPassword.value = "";
	        objCompany.value = "";
	        objDepartment.value = "";
	        objEMail.value = "";

	    }else{//DataTable

	        objIdLocalUser.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_ID%>"];
	        objLogin.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_LOGIN%>"];
	        objName.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_NAME%>"];
	        objCompany.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_COMPANY%>"];
	        objDepartment.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_DEPARTMENT%>"];
	        objEMail.value = par.value.Rows[0]["<%=Constants.TABLE_COLUMN_USER_EMAIL%>"];

	    }
	}

	/*
	function
	*/
	function onClickCancel()
	{
		window.close();
	}



	function isNumCharString(strValue)
	{
		if (/[^a-zA-Z0-9\s]/.test(strValue))
			return false;
		return true;
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
<fis:header id="header1" runat="server"/>
<body onload="fOnBodyLoad();" style="background-color: rgb(210,210,210);">
<form id="idFormEditGroup" runat="server">
	    <table id="Table1" border="0" bordercolor=green cellpadding="0" cellspacing="0" style="margin-left:5%;height:100%;width:90%">
	        <tr>
	            <td nowrap width="30%">
                    <b>Login:</b>
                </td>
                <td nowrap> 
                    <input  class="userproperties" id="idLocalUser" type="hidden"/>
                    <input  class="userproperties" id="idLogin" type="text" maxlength="20"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Name:</b>
                </td>
                <td>
                    <input class="userproperties" id="idName" type="text" maxlength="20"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Password:</b>
                </td>
                <td>
                    <input class="userproperties" id="idPassword" type="password" maxlength="12"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Confirm<br>Password:</b>
                </td>
                <td>
                    <input class="userproperties" id="idConfirmPassword" type="password" maxlength=12/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Company:</b>
                </td>
                <td>
                    <input class="userproperties" id="idCompany" type="text" value="local" maxlength="20"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Department:</b>
                </td>
                <td>
                    <input class="userproperties" id="idDepartment" type="text" value="local" maxlength="20"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>eMail:</b>
                </td>
                <td>
                    <input class="userproperties" id="idEMail" type="text" maxlength="40"/>
                </td>
            </tr>
            <tr>
                <td colspan=2 align=right>
                    <button id="idBtnAdd" onclick="onClickSave('add')" style="width:60px">
	                    Add
                    </button>
                    &nbsp;&nbsp;&nbsp;
                    <button id="idBtnOK" onclick="onClickSave('ok')" style="width:60px">
	                    OK
                    </button>
                    &nbsp;&nbsp;&nbsp;
                    <button id="idBtnCancel" onclick="onClickCancel()"  style="width:60px">
	                    Cancel
                    </button>
                </td>
            </tr>
    </table>
	<input id="idHidGroupID" type="hidden" value="" />
</form>
</body>
<fis:footer id="footer1" runat="server"/>
</html>
