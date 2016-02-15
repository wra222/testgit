
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ����ѡģ����и��ƿ���ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_CopyTemplate, App_Web_copytemplate.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Copy Template</title>
    <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
</head>
<fis:header id="header2" runat="server"/>
<body class="dialogBody" >
    <form id="form1" runat="server">
    <div>
    
    </div>
     <asp:HiddenField ID="user" runat="server"></asp:HiddenField>
    </form>
    
    <table width="100%"  height="80%" cellpadding="0px" cellspacing="5px" border="0"  >
    <tr>
    <td>
        <div >
	        <div  style="width:32%;float:left;"  >
                <FIELDSET style="padding:0;height:200px">
	            <LEGEND><%=Resources.Template.folder%></LEGEND>  
	                <div id="treeviewarea" style="width:100%;height:190px;overflow:auto;" >
                    </div>
	             </FIELDSET> 
             </div>
             <div  style="float:left;;" >
               <FIELDSET style="padding:0;height:200px">
	            <LEGEND>Tempalte Information:</LEGEND> 
	            <TABLE width="100%"  border="0" cellpadding="0px" cellspacing="5px"  >
                    <tr>
	                    <td style="width:100px" class="inputTitle"><%=Resources.Template.copyName%>:</td>
	                    <td ><input type="text" id="templateName" maxlength="50" style="width:200px" /></td>
                    </tr>
                    
                    <tr>
	                    <td colspan="2" class="inputTitle"><%=Resources.Template.description%>:</td>
	                </tr>
	                <tr>
	                    <td colspan="2"><textarea name="comment" id="comment" rows="5"  cols="1" style="width:100%"></textarea><br>(<%=Resources.Template.commentLength%>)</td>
                    </tr>
                           
                           
                 </TABLE>  
                </FIELDSET> 	 
              </div>
  	    </div>
  	</td>
    </tr>
    </table>
  	<table width="100%"  height="20%" cellpadding="0px" cellspacing="5px" border="0"  >
    <tr valign="bottom">
	    <td align="right" colspan="2" ><button id="ok" onclick="save()"  ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;&nbsp;<button id="cancel" onclick="closePage()" ><%=Resources.Template.cancelButton%></button></td>
    </tr>
    </table>
    <input type="hidden" id="selNodeId" />
    
  
</body>
<fis:footer id="footer1" runat="server"/> 


<SCRIPT LANGUAGE="JavaScript">
<!--	
var userName = document.getElementById("<%=user.ClientID%>").value; 
ShowWait();
var isLoadingPage = true;
var tree =null;
var dialogPar = window.dialogArguments;	

createTree();
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createTree
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function createTree()
{	
	
	tree = new MzTreeView("tree");
	tree.useArrow = false;
	tree.LoadData = "Load()";
	tree.icons["property"] = "property.gif";
	tree.icons["css"] = "collection.gif";
	tree.icons["book"]  = "book.gif";
	tree.iconsExpand["book"] = "bookopen.gif"; 
	tree.setIconPath("./"); 
	tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "f0000000000000000000000000000002"+tree.attribute_d+"text" + tree.attribute_suffix + "Template"+tree.attribute_d+" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT" +tree.attribute_d+"nodeuuid" + tree.attribute_suffix + "-1" ;
	tree.customIconFun = useSOPMngIcon;

	tree.nodeClick = function clicknode()
	{
		var currentNode = tree.currentNode; 
		var uuid = currentNode.uuid;
		var pid = currentNode.parentId;
		type =  currentNode.type;
        var text = currentNode.text;
        var nodeuuid = currentNode.nodeuuid;
        //�������ڵ�ʱ,����ѡ�ڵ�id���浽hidden�ؼ�
       
        document.getElementById("selNodeId").value = uuid;
    
    }	
 
    document.getElementById('treeviewarea').innerHTML = tree.toString();
    HideWait();
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	useSOPMngIcon
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	���ݽڵ���������ͼ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function useSOPMngIcon(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
	
	switch (node.type){
		case "NODE_ROOT":
			strIconName = "root.gif";
			break;
		case "<%=Constants.NODE_TYPE_FOLDER_REPORT%>":
			strIconName = "reportfolder.gif";
			break;
		case "<%=Constants.NODE_TYPE_REPORT%>":
			strIconName = "reportfile.gif";
			break;
	   
		default:
		break;
	}
	return strPrefix +strIconName;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Load
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�������ڵ�����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   
function Load()
{
    var source= tree.nodes[tree.expandingNode.sourceIndex]
    var uuid = tree.getAttribute(source, "uuid")
    type = tree.getAttribute(source, "type");
    getNodeData(type,uuid);
}
        
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getNodeData
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ȡ��ѡ�ڵ��µĵ�һ��ڵ�(folder����)
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~       
function getNodeData(type,uuid)
{
	ShowWait();
	var rtn = com.inventec.template.manager.TemplateManager.getFirLevelFolders(uuid, InitChild);
	
}
   
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	InitChild
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	�������ڵ�
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
function InitChild(rtn)
{
       
    if (rtn.error != null) 
    {
        HideWait();
        
        var errorInfo = rtn.error.Message;
        
        if (errorInfo == "Unknown")
        {
            alert("error");
        }
        else
        {
            alert(errorInfo);
        }
        
        tree.emptyNode(tree.expandingNode);
        return;
    }


    var treeNodeInfo = rtn.value;
    if (treeNodeInfo.Rows.length < 1)
    {
        HideWait();
        tree.emptyNode(tree.expandingNode);
        return;
    }

    if(treeNodeInfo != null && typeof(treeNodeInfo) == "object")
    {
        var arrName = new Array();
        for (var i=0; i < treeNodeInfo.Columns.length; i++)
        {
            arrName[i] = treeNodeInfo.Columns[i].Name;
            
        }
        
        for (var i=0; i < treeNodeInfo.Rows.length; i++)
        {
            //alert(treeNodeInfo.Rows[i]["nodeuuid"]);
            var arrNode = new Array();
	        arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[0]];
		       // alert(treeNodeInfo.Rows[i][arrName[1]]);
	        //arrNode[arrNode.length] = 'parentId' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[1]];
	        arrNode[arrNode.length] = 'text' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[2]];
	        arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "<%=Constants.NODE_TYPE_FOLDER_REPORT%>";
    	
    	
	        var hasChild;
	        if(treeNodeInfo.Rows[i][arrName[3]] == "0")
	        {
		        hasChild = "false"; 
	        }
	        else
	        {
		        hasChild = "true";
	        }				
	        arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + hasChild;
	        tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
        }
     }

      tree.dataFormat();
      tree.load(tree.expandingNode.id);
      tree.buildNode(tree.expandingNode.id);
            
        //new add by lzy to expand the last visiting node.
            
      tree.locateNode("UUID")
    	
      HideWait();
   
   
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
    var templateName = trimString(document.getElementById("templateName").value);
    var comment = trimString(document.getElementById("comment").value);
    var selFolderId = document.getElementById("selNodeId").value;
    var ret = "";
    
    
    //�ж��������ݺϷ���
    if (selFolderId == "") {
        errorDetected = true;
        alert("<%=Resources.Template.notSelectFolder%>");
       
    }else if (templateName == "") {
        errorDetected = true;
        alert("<%=Resources.Template.notInputTemplateName%>");
        document.getElementById("templateName").focus();
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
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_ID%>", dialogPar.id);
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TEMPLATE_ID%>", dialogPar.targetId);
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_NAME%>", templateName);
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_COMMENT%>", comment);  
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.TREENODE_PARENT_ID%>", selFolderId); 
            setKeyValueOfDic(rtnObj.value, "<%=AttributeNames.USER_NAME%>", userName); 
            ret =  com.inventec.template.manager.TemplateManager.copyTemplate(rtnObj.value, userName)
        
            if (ret.error != null) {
                alert(ret.error.Message);
             
                
            } else {
                window.returnValue  =  "success";
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


