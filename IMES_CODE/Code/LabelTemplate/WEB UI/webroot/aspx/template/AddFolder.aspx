
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: �������߱༭Folderҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_AddFolder, App_Web_addfolder.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>
<% if (Request.Params[0].Equals("add")){%>
     Add folder
<%} else { %> 
    Edit folder
<% }%> 
</title>
</head>

<fis:header id="header1" runat="server"/>
<body class="dialogBody" onload="initPage()">
    <form id="form1" runat="server">
    <div>
    </div>
    <asp:HiddenField ID="user" runat="server"></asp:HiddenField>
    </form>
  
    <table width="100%"  height="80%" cellpadding="0px" cellspacing="5px" border="0"  >
    <tr>
	    <td width="30%" class="inputTitle"><%=Resources.Template.folderName%>:</td>
	    <td align="right">
	    <input type="text" id="folderName" maxlength="50" class="inputStyle">
	    </td>
    </tr>
    <tr>
	    <td colspan="2" class="inputTitle"><%=Resources.Template.comment%>:</td>
    </tr>
    <tr>
	    <td colspan="2" align="right"><textarea name="comment" id="comment" rows="5" cols="1" style="width:100%"></textarea><br>(<%=Resources.Template.commentLength%>)</td>
    </tr>
    </table>
     <table width="100%"  height="20%" cellpadding="0px" cellspacing="5px" border="0"  >
    <tr valign="bottom">
	    <td align="right" colspan="2" ><button id="ok" onclick="save()"  ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;&nbsp;<button id="cancel" onclick="closePage()" ><%=Resources.Template.cancelButton%></button></td>
    </tr>
    </table>
    
</body>    
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--
var userName = document.getElementById("<%=user.ClientID%>").value; 
var dialogPar = window.dialogArguments;
var addOrEditFlag = "<%=Request.Params[0]%>"; 
var oriFolderName;
var oriComment;
    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{
  
    if(addOrEditFlag == "edit") {
        oriFolderName = htmDecodeString(dialogPar.name);
        oriComment = htmDecodeString(dialogPar.comment);
        document.getElementById("folderName").value = htmDecodeString(dialogPar.name);
        document.getElementById("comment").value = htmDecodeString(dialogPar.comment);
            
    } 
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	save
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�ύҳ�����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function save()
{
    var errorDetected = false;
    var folderName = trimString(document.getElementById("folderName").value);
    var comment = trimString(document.getElementById("comment").value);
    var ret = "";
    var beTheSame = "false";
    
    //�ж��������ݺϷ���
    //    <bug>
    //        BUG NO:ITC-992-0030
    //        REASON:Ӧ�û�ȡ����trim��folder����
    //    </bug>
    if (folderName == "") {
        errorDetected = true;
        alert("<%=Resources.Template.notInputFolderName%>");
        document.getElementById("folderName").focus();
    } else if (comment.length > 200) {
        errorDetected = true;
        alert("<%=Resources.Template.descrLong%>");
        document.getElementById("comment").select();
       
    } 
     if (errorDetected) {
        return;
    }  else {
	    //�ύҳ��
        //��ȡ�������˵�Dictionary�ṹ
        var rtnObj = com.inventec.template.manager.TemplateManager.getDictionaryObj();
        if (rtnObj.error!=null) {
            alert(rtnObj.error.Message);
        } else {
            if (addOrEditFlag == "add") {
                //ִ��addfolder
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_PARENT_ID%>", dialogPar.parentId);
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_NAME%>", folderName);
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_COMMENT%>", comment);  
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.USER_NAME%>", userName);  
                
                ret =  com.inventec.template.manager.TemplateManager.addFolder(rtnObj.value)
        
            } else if (addOrEditFlag == "edit") {
                //ִ��updatefolder
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_PARENT_ID%>", dialogPar.parentId);
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_ID%>", dialogPar.treeId);
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_NAME%>", folderName);
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_COMMENT%>", comment);  
                setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.USER_NAME%>", userName);  
                ret =  com.inventec.template.manager.TemplateManager.editFolder(rtnObj.value)
            }
            
            if (ret.error != null) {
            
                alert(ret.error.Message);
                if (ret.error.Message == "<%=ExceptionMsg.FOLDER_NOT_EXISTED%>") {
                    //���ڱ༭�����foler�Ѿ���ɾ����
                    window.returnValue  =  beTheSame;
                    window.close();
                } else {
                    document.getElementById("folderName").focus();
                }    
                
            } else {
                if (addOrEditFlag == "add") {
             
                    window.returnValue  = "add"
                    
                } else if (addOrEditFlag == "edit") {
                    
                    //�ж��Ƿ��޸���folder��Ϣ
                    if ((oriFolderName == folderName) && (oriComment == comment))
                    {
                        //δ�޸�
                        beTheSame = "true";
                    }
                    window.returnValue  =  beTheSame;
               }
               window.close();
            }
        } 
        
    }
    
            
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	closePage
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�رո�ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
function closePage()
{
    window.close();
}
  

//-->
</SCRIPT>

</html>

