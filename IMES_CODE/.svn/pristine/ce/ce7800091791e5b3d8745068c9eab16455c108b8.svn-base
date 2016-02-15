
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ��ʾUsers�������û�
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2011-09-27   xiaoling Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 

<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_Users, App_Web_users.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="System.Web.UI" %>


<script type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
<script type="text/javascript" src="../../commoncontrol/tableEdit/TableEdit.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Users</title>
    <style>
        button.btn {
        	font-size:8pt;
        	width:90px;
       	}
    </style>
</head>

<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body onresize="ReLoadTable();">
    <form id="form1" runat="server">
    <asp:HiddenField ID="cs" runat="server"></asp:HiddenField>
     </form>
     
     <table cellpadding="0" border="0" cellspacing="0"  width="100%">     
     <tr>
        <td class="title" style="height:20px">
        <%--show template name or folder--%>
         <label id="name"></label>
        </td>
	 </tr>  
	     
     <tr>
        <%--table area--%>
	    <td><div style="height:100%;width:100%" id="showreportdata" ondblclick="return showdata_ondblclick()"></div></td>
     </tr>  
     
     <tr>
	    <td align="right"><br />
	    <div>
	        <button class="btn" id="newUser" onclick="addEditUser('add')"><%=Resources.Template.newButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="editUser" onclick="addEditUser('edit');"><%=Resources.Template.editButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="del" onclick="del()"><%=Resources.Template.deleteButton%></button>&nbsp;&nbsp;
	     </div>
	     </td>
     </tr>
     </table>
 
</body>

<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 


<script for=ShowItemTableTableBody language=javascript event=onclick>
    //��񵥻��¼�
    return showreportdata_onclick()
</script>

<SCRIPT LANGUAGE="JavaScript">
<!--
 var ShowItemTable = null;
 





 function addEditUser(operateType)
 {
    var rowItemArr="", loginId="";
    if(operateType=="edit")
    {
        rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
        loginId = rowItemArr[1];
    }

	var sFeature_localuser = "dialogHeight:310px;dialogWidth:380px;center:yes;status:no;help:no;scroll:no;resizable:no";
	var diagArgs_user = new Object();
	diagArgs_user.loginId = loginId;
    diagArgs_user.operateType = operateType;

	var result = window.showModalDialog("addedituser.aspx", diagArgs_user, sFeature_localuser);
	if (result != undefined && result != "cancel")
	{
         window.parent.frames("menu").tree.freshCurrentNode(0); 
	}
	else
	{
	    return false;
	}


 }
 

 
 function del()
 {
	var notice = "Are you sure you want to delete the user?"; 
	if (!confirm(notice))
	{
	    return;
	}
	
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];

    //delete folder
    var rtn = com.inventec.template.manager.TemplateManager.deleteUser(id);
    
     if (rtn.error != null) {
        alert(rtn.error.Message);
     } else {
         window.parent.frames("menu").tree.freshCurrentNode(0); 
     }

    
 }
 

 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showTable
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʾ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showTable(rs){

   
    var tableWidth = document.body.clientWidth-5;
    var tableWidth1 = tableWidth - 17;
    var clientH = document.body.clientHeight;
        
    if (ShowItemTable == null){
	    ShowItemTable = new clsTable(rs, "ShowItemTable");
        if (clientH > 100){
            ShowItemTable.Height = clientH  - 88;
        } else {
            ShowItemTable.Height = clientH;
        }
        ShowItemTable.TableWidth = tableWidth;    	    
        ShowItemTable.Widths = new Array(1, tableWidth1*0.15, tableWidth1*0.16, tableWidth1*0.27, tableWidth1*0.13,tableWidth1*0.13,tableWidth1*0.16);
        ShowItemTable.FieldsType = new Array(0, 0, 0, 0, 0, 0, 0);
        ShowItemTable.HideColumn = new Array(false, true, true, true, true,true,true);
        ShowItemTable.UseSort = "TDCData";
        ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
        ShowItemTable.ScreenWidth = -1;
        ShowItemTable.UseHTML = true;
    }
      	
    ShowItemTable.rs_main = rs;
    showreportdata.innerHTML = ShowItemTable.Display();
	    
    HideWait();
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showreportdata_onclick
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�������¼�����Ķ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function showreportdata_onclick() 
{
        var rownum=ShowItemTable.GetRowNumber();
         var rowItemArr;
        if(rownum == -1 || rownum >= ShowItemTable.rs_main.recordcount)
       {
            document.getElementById("editUser").disabled = true;
            document.getElementById("del").disabled = true;
            return;
       } 
       if(ShowItemTable.RowStr != null)
       {
            document.getElementById("editUser").disabled = false;
            document.getElementById("del").disabled = false;
            rowItemArr = ShowItemTable.RowStr.split("<%=Constants.COL_DELIM%>"); 

            var itemType = rowItemArr[3];
        }
      
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getRecordSet
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ȡ�����Ϣ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function getRecordSet()
{
 
    //ͨ��ajax������ȡ����¼
    var rtn = com.inventec.template.manager.TemplateManager.getUsers();
    if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {
        // ���ɼ�¼��
        var rs = createRecordSet(rtn.value);
        //��ʾ���
        showTable(rs);
    }
    
}




//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     
function initPage()
{
    //���ð�ť��ʼ״̬    
    document.getElementById("editUser").disabled = true;
    document.getElementById("del").disabled = true;
     
    //��ȡ�������
    getRecordSet()     
        
}

function ReLoadTable()
{
    var myrownum = ShowItemTable.currentRow;
    showreportdata.innerHTML = ShowItemTable.Display();
    if(!(myrownum == -1 || myrownum > ShowItemTable.rs_main.recordcount))
    {
        ShowItemTable.HighLightRow(myrownum);
	}
	return;
}

function showdata_ondblclick() 
{
  
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount)
    {
    
        return;
    } 
            
    addEditUser("edit");
    
}
initPage()



//alert(window.parent.frames["tree"]);
//-->
</SCRIPT>
 </html>