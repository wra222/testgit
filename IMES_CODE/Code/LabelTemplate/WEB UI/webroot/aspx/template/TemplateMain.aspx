
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ��ʾĳһfolder�µ�һ�������template����folder
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 

<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_TemplateMain, App_Web_templatemain.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<script type="text/javascript" src="../../commoncontrol/tableEdit/TableData.js"></script>
<script type="text/javascript" src="../../commoncontrol/tableEdit/TableEdit.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Template Main Page</title>
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
    
      <div>
      </div>
          <%--current tree id (from treeView page)--%>
       <asp:HiddenField ID="currentTreeId" runat="server"></asp:HiddenField>
       <asp:Button ID="btnExport" runat="server" Text="downloadSubmit" style="display:none" OnClick="btnExport_Click" /> 
       <asp:HiddenField ID="templateId" runat="server"></asp:HiddenField>
       <asp:HiddenField ID="templateName" runat="server"></asp:HiddenField>
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
	    <td><div style="height:100%;width:100%" id="showreportdata" ondblclick="return showreportdata_ondblclick()"></div></td>
     </tr>  
     
     <tr>
	    <td align="right"><br />
	    <div>
	        <button class="btn" id="newTemplate" onclick="newTemplate()"><%=Resources.Template.newButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="editTemplate" onclick="editTemplate();"><%=Resources.Template.editButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="del" onclick="del()"><%=Resources.Template.deleteButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="copyTemplate" onclick="copyTemplate()"><%=Resources.Template.copyButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="import" onclick="importTemplate()"><%=Resources.Template.importButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="export" onclick="exportTemplate()"><%=Resources.Template.exportButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="createFolder" onclick="createFolder()"><%=Resources.Template.createFolderButton%></button>&nbsp;&nbsp;
	        <button class="btn" id="showChangeList" onclick="showChangeList()" disabled><%=Resources.Template.BtnShowChangeList%></button>&nbsp;&nbsp;
	        <%--<button id="editFolder" style="width:100px" onclick="editFolder()"><%=Resources.Template.editFolderButton%></button>&nbsp;&nbsp;--%>
	        
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
 

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showChangeList
//| Author		:	98079
//| Create Date	:	1/4/2010
//| Description	:	��ʾchange list
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function showChangeList()
 {
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[2];
    var diaParas = new Object();
    diaParas.templateId = id;
    diaParas.templateName = rowItemArr[5];
   
    window.showModalDialog("ShowChangeList.aspx", diaParas, "dialogWidth:600px;dialogHeight:400px;center:yes;scroll:off;status:no;help:no")
 }



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	importTemplate
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	����ģ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function importTemplate()
 {
   
    var ret = window.showModalDialog("ImportTemplate.aspx", "", "dialogWidth:400px;dialogHeight:300px;center:yes;scroll:off;status:no;help:no")
    if (typeof(ret) != "undefined")
	{
		
	   var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value;
	   window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?uuid=&nodeuuid=&parentid=" + currentTreeId + "&method=import";
	   
	}
 }

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	newTemplate
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�����½�ģ���򵼶Ի���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function newTemplate()
{
   
    var diagArgs = new Object();
    diagArgs.treeId = ""; //get the parent id
    diagArgs.pixel_per_inch_x = 1;
    diagArgs.pixel_per_inch_y = 1;  
    var printTemplateXMLAndHtmlInfo;
    var rtn = com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtmlFromStructure();
    if (rtn.error != null) {
        alert(rtn.error.Message);
    } else{
        printTemplateXMLAndHtmlInfo = rtn.value;
        var a = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
        if (a.error != null) {
            alert(a.error.Message);
            return;
         } else {
            var section = a.value;
            section.Index = 0;
            section.AreaHeight = "0";  
            printTemplateXMLAndHtmlInfo.PrintTemplateInfo.DetailSections.push(section);
                           
         }
         var b = com.inventec.template.manager.TemplateManager.getSectionFromStructure();
         if (b.error != null) {
            alert(b.error.Message);
             return;
         } else {
             var section = b.value;
             section.Index = 1;
             section.AreaHeight = "0";  
             printTemplateXMLAndHtmlInfo.PrintTemplateInfo.DetailSections.push(section);
                           
         }
         
        printTemplateXMLAndHtmlInfo.toJSON = function(){return toJSON(this);};
        var ret = com.inventec.template.manager.TemplateManager.saveSession(printTemplateXMLAndHtmlInfo);
        if (ret.error != null) {
            alert(ret.error.Message); 
        }
        var a = window.showModalDialog("TemplateSetting.aspx", diagArgs, "dialogWidth:580px;dialogHeight:450px;center:yes;scroll:off;status:no;help:no");
       
        if (typeof(a) != "undefined")
	    {
//            var templateXmlandHtml = diagArgs.templateXmlandHtml;
//	        templateXmlandHtml.toJSON = function(){return toJSON(this);};
//	        var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
//	        if (ret.error != null) {
//                alert(ret.error.Message);
//            } else{
               var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value;
	           window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?uuid=&nodeuuid=&parentid=" + currentTreeId + "&method=add";

//            }
        }
    }
   
    
		
 }
 

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createFolder
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��������folder�Ի���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function createFolder()
{
    var sFeatures = "dialogWidth: 400px;dialogHeight:300px;status:no;scroll:yes;center:yes;;help:no";
    var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value
    var diagArgs = new Object();
    diagArgs.parentId = currentTreeId; //get the parent id
       
    var ret =  window.showModalDialog("AddFolder.aspx?type=add", diagArgs, sFeatures);
    
    if (typeof(ret) != "undefined")
	{
		 
		 window.parent.frames("menu").tree.freshCurrentNode(0);
	}  
               
         
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	copyTemplate
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��������ѡģ����и��ƿ����Ի���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function copyTemplate()
{
    //    <bug>
    //        BUG NO:ITC-992-0021
    //        REASON:�����ɹ�������ʾ����ˢ����
    //    </bug>  
    var copySucceed = "<%=Resources.Template.copySucceed%>";
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    var sFeatures = "dialogWidth: 480px;dialogHeight:320px;status:no;scroll:yes;center:yes;;help:no";
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];
    var parentId = rowItemArr[1];
    var targetId =  rowItemArr[2];
    var type =  rowItemArr[3];
    
    var diagArgs = new Object();
    diagArgs.id = id;
    diagArgs.targetId = targetId;
       
    var rtn =  window.showModalDialog("CopyTemplate.aspx?", diagArgs, sFeatures);
    if (typeof(rtn) != "undefined")
	{
		alert(copySucceed);
		window.parent.frames("menu").tree.freshRootNode(); 
	}   
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	editFolder
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��������folder�Ի���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function editFolder()
{
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value
    var sFeatures = "dialogWidth: 400px;dialogHeight:300px;status:no;scroll:yes;help:no";
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];
    var name =  rowItemArr[5];
    var comment =  rowItemArr[6];
    var diagArgs = new Object();
    diagArgs.treeId = id;
    diagArgs.name =  name;
    diagArgs.comment =  comment;
    diagArgs.parentId = currentTreeId; //get the parent id
       
    var ret =  window.showModalDialog("AddFolder.aspx?type=edit", diagArgs, sFeatures);
    if (typeof(ret) != "undefined")
	{
		 if (ret == "false") {
             window.parent.frames("menu").tree.freshCurrentNode(0); 
             
        
         } 
	}
       
      
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	editTemplate
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�����½�ģ���򵼶Ի���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function editTemplate()
 {
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];
    var parentId = rowItemArr[1];
    var targetId =  rowItemArr[2];
    var type =  rowItemArr[3];
    var name =  rowItemArr[5];
    var comment =  rowItemArr[6];
    var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value
    if (type == "<%=Constants.NODE_TYPE_FOLDER_REPORT%>") {
        
        var sFeatures = "dialogWidth: 400px;dialogHeight:300px;status:no;scroll:yes;help:no";
        var diagArgs = new Object();
        diagArgs.treeId = id;
        diagArgs.name =  name;
        diagArgs.comment =  comment;
        diagArgs.parentId = currentTreeId; //get the parent id
           
        var ret =  window.showModalDialog("AddFolder.aspx?type=edit", diagArgs, sFeatures);
        if (typeof(ret) != "undefined")
	    {
		     if (ret == "false") {
                 window.parent.frames("menu").tree.freshCurrentNode(0); 
                 
            
             } 
	    } 
    } else if (type == "<%=Constants.NODE_TYPE_REPORT%>") {
//          var rtn =  com.inventec.template.manager.TemplateManager.getTemplateXmlAndHtml(targetId);
//          if (rtn.error != null) {
//                alert(rtn.error.Message);
//          } else {
//                 window.parent.frames("menu").templateXmlAndHtml = rtn.value;
//                 window.parent.frames("menu").tree.searchInChildNodes("uuid", id, true);
//	             var currentTreeId = currentTreeId;
//	             window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?uuid=" + id + "&nodeuuid="+targetId + "&parentid=" + parentId + "&method=edit";
//                 
//                 
//          }
             window.parent.frames("menu").tree.searchInChildNodes("uuid", id, true);
	         var currentTreeId = currentTreeId;
	         window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?uuid=" + id + "&nodeuuid="+targetId + "&parentid=" + parentId + "&method=edit";
    }
  
   
   
    
 }
 
 //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exportTemplate
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	����ѡģ�嵼�����ͻ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function exportTemplate()
 {
    
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    
    var targetId =  rowItemArr[2];
    var name =  htmDecodeString(rowItemArr[5]);
    document.getElementById("templateId").value = targetId;
    document.getElementById("templateName").value = name;
    document.getElementById("btnExport").click();
    
 }
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	del
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	ɾ����ѡ��¼
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 function del()
 {

    var deleteFolderInfo = "<%=Resources.Template.deleteFolderInfo%>";
    var deleteFileInfo = "<%=Resources.Template.deleteFileInfo%>";
    var deleteSucceed = "<%=Resources.Template.deleteSucceed%>";
   
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount) {
        alert("<%=Resources.Template.notSelectRow%>");
        return;
    } 
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];
    var parentId = rowItemArr[1];
    var targetId =  rowItemArr[2];
    var type =  rowItemArr[3];
    var name = rowItemArr[5];
   
    if(type == "<%=Constants.NODE_TYPE_FOLDER_REPORT%>") {
   
        if(!confirm(deleteFolderInfo + "(" + htmDecodeString(name) +")"))
        {
            return;
        }
        else
        {
            //delete folder
            var rtn = com.inventec.template.manager.TemplateManager.judgeHasChild(id)
            
             if (rtn.error != null) {
                alert(rtn.error.Message);
             } else {
                deletefolder(rtn.value, id);
             }
        
        }
    } else if(type == "<%=Constants.NODE_TYPE_REPORT%>") {
   
        if(!confirm(deleteFileInfo + "(" + htmDecodeString(name) +")"))
        {
            return;
        }
        else
        {
            //delete template
            var rtn = com.inventec.template.manager.TemplateManager.deleteTemplate(id,targetId);
            
             if (rtn.error != null) {
                alert(rtn.error.Message);
             } else {
                alert(deleteSucceed);
                window.parent.frames("menu").tree.freshCurrentNode(0); 
             
                
             }
        
        }
    }
    
 }
 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	deletefolder
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	ɾ����ѡ��folder��¼
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
 function deletefolder(value, id)
{
   
     var deleteSucceed = "<%=Resources.Template.deleteSucceed%>";
   
    if(value) {
    
        alert("<%=Resources.Template.folderNotEmpty%>");
        return;
    } else { 
        
         //delete template
         var rtn = com.inventec.template.manager.TemplateManager.deleteFolder(id);
         if (rtn.error != null) {
            alert(rtn.error.Message);
         } else {
            alert(deleteSucceed);
            window.parent.frames("menu").tree.freshCurrentNode(0); 
                
          }
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
        ShowItemTable.Widths = new Array(1, 1, 1, 1,  tableWidth1*0.04, tableWidth1*0.26, tableWidth1*0.26, tableWidth1*0.1,tableWidth1*0.17,tableWidth1*0.17);
        ShowItemTable.FieldsType = new Array( 0, 0, 0, 0, 0, 0, 0, 0,0,0);
        ShowItemTable.HideColumn = new Array(false,false,false,false,true, true, true, true,true,true);
        ShowItemTable.UseSort = "TDCData";
        ShowItemTable.Divide = "<%=Constants.COL_DELIM%>";
        ShowItemTable.ScreenWidth = -1;
        //ShowItemTable.AfterNew = "MyAfterNew()";
        //ShowItemTable.UpDown = "fUpdateIssueDetail()";
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
            document.getElementById("editTemplate").disabled = true;
            document.getElementById("del").disabled = true;
            document.getElementById("copyTemplate").disabled = true;
            document.getElementById("export").disabled = true;
            document.getElementById("showChangeList").disabled = true;
//            document.getElementById("editFolder").disabled = true;
            return;
       } 
       if(ShowItemTable.RowStr != null)
       {
             rowItemArr = ShowItemTable.RowStr.split("<%=Constants.COL_DELIM%>"); 

            var itemType = rowItemArr[3];
            if(itemType == "<%=Constants.NODE_TYPE_FOLDER_REPORT %>")
            {
              // folder 
              document.getElementById("del").disabled = false;  
              document.getElementById("editTemplate").disabled = false;
              document.getElementById("copyTemplate").disabled = true;
              document.getElementById("export").disabled = true;
              document.getElementById("showChangeList").disabled = true;
              
//              document.getElementById("editFolder").disabled = false;
            } 
            else if(itemType =="<%=Constants.NODE_TYPE_REPORT %>")
            {
              // template 
//              document.getElementById("editFolder").disabled = true;
              document.getElementById("del").disabled = false;  
              document.getElementById("editTemplate").disabled = false;
              document.getElementById("copyTemplate").disabled = false;
              document.getElementById("export").disabled = false;
              document.getElementById("showChangeList").disabled = false;
              
            }
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
 
    var currentTreeId = document.getElementById("<%=currentTreeId.ClientID%>").value
    //ͨ��ajax������ȡ����¼
   
    var rtn = com.inventec.template.manager.TemplateManager.getFirLevelNodes(currentTreeId);
    if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {
        // ���ɼ�¼��
        var rs = createRecordSet(rtn.value);
        //��ʾ���
        showTable(rs);
    }
    
     //ͨ��ajax��������treeID��name
  
    var rtn = com.inventec.template.manager.TemplateManager.getTreeName(currentTreeId);
    if (rtn.error != null) {
        alert(rtn.error.Message);
    } else {
       
        document.getElementById("name").innerHTML = rtn.value;
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
    document.getElementById("editTemplate").disabled = true;
    document.getElementById("del").disabled = true;
    document.getElementById("copyTemplate").disabled = true;
    document.getElementById("export").disabled = true;
    document.getElementById("del").disabled = true;   
//    document.getElementById("editFolder").disabled = true;   
     
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
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	showreportdata_ondblclick
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	���˫���¼�
//| Input para.	:	
//| Ret value	:	
//~
 function showreportdata_ondblclick() 
 {
  
    var rownum=ShowItemTable.GetRowNumber();
    if(rownum == -1 || rownum > ShowItemTable.rs_main.recordcount)
    {
    
        return;
    } 
            
    var rowItem = ShowItemTable.RowStr;
    var rowItemArr;
    if(rowItem == null)
    {
        return;
    }
    var rowItemArr = ShowItemTable.RowStr.split('<%=Constants.COL_DELIM%>');
    var id = rowItemArr[0];
    var parentId = rowItemArr[1];
    var targetId =  rowItemArr[2];
    var type =  rowItemArr[3];
    var name = rowItemArr[5];
    
    if(type == "<%=Constants.NODE_TYPE_FOLDER_REPORT%>") {
        window.parent.frames("menu").tree.searchInChildNodes("uuid", id);
    } else {
          
        window.parent.frames("menu").tree.searchInChildNodes("uuid", id);
//        window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?type=" + type + "&uuid=" + id + "&text=&nodeuuid="+targetId + "&parentid=" + parentId + "&method=add";
        window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?uuid=" + id + "&nodeuuid="+targetId + "&parentid=" + parentId + "&method=edit";
    
               
    }
}
initPage()



//alert(window.parent.frames["tree"]);
//-->
</SCRIPT>
 </html>