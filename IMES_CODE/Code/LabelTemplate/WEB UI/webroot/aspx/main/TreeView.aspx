<%@ page language="C#" autoeventwireup="true" inherits="TreeView, App_Web_treeview.aspx.39cd9290" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<html>
<head runat="server">
    <title>Untitled Page</title>
    <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>

</head>
<fis:header id="header2" runat="server"/>
<body leftMargin="0" topMargin="0" rightMargin="0" bottommargin="0" style="width:100%"  oncontextmenu="return false;" onmouseup="fOnMouseUp();">
    <form runat="server">
    </form>
<table  style="width:100%; border:0px" id="tableTitle">
    <tr>
    <td class="title" style=" border:0px; word-wrap:normal;">Function Tree</td>
    </tr>
    </table>
    <div style= "position:absolute;right:25px;top:4px;" id="freshimg">
        <INPUT type="image"  src="../../images/fresh.gif"  onclick="tree.freshRootNode();"/>
    </div> 
        <div style= "position:absolute;right:3px;top:1px;">
        <INPUT style="" type="image" src="../../images/nav2.gif" value="button" id="shleft" name="shleft" onclick="OpenClose()">
    </div>    
    <table width=100% height=100% id="tablediv" style=" margin-right:0px">
        <tr>
            <td width="100%" height=100%>
                <div id=treeviewarea style="width:100%;height:97%;overflow-y:scroll;overflow-x:scroll;background-color:#FFFFFF;" >
                  </div>
            </td>
        </tr>
    </table>
</body>
<fis:footer id="footer1" runat="server"/> 
</html>
    <script language="javascript">
    var bTemplateChanged = false;

            function fOnMouseUp(){
                if(typeof(window.parent.frames("main").fOnMouseUp) == "function"){
                    window.parent.frames("main").fOnMouseUp();
                }
            }
    
    
            function OpenClose()
			{
			    var expendPic = "images/nav2.gif";
			    var srcValue = document.getElementById("shleft").src;
			    var from = srcValue.lastIndexOf(expendPic);
			    var to = srcValue.length;
			    var tmp = srcValue.substring(from, to);
			    
			    <%--
                ' ======== ============ =============================
                ' Description: ITC-934-0002
                ' Author: zhao, qing-rong
                ' Date:2009-02-10
                ' ======== ============ =============================
                --%>
			    if (tmp == expendPic) 
			    {
			        parent.leftmenu.cols="20,*";
					document.getElementById("shleft").src="../../images/nav.gif";
					document.getElementById("tablediv").style.display = "none";
					document.getElementById("tableTitle").style.display="none";
					document.getElementById("freshimg").style.display="none";
					//parent.document.getElementById("leftmenu").noResize=true;
					parent.leftmenu.childNodes[0].noResize=true;
					
			    }
			    else
				{
					parent.leftmenu.cols="230,*";
					document.getElementById("shleft").src="../../images/nav2.gif";
					document.getElementById("tablediv").style.display = "";
					document.getElementById("tableTitle").style.display="";
					document.getElementById("freshimg").style.display="";
					parent.leftmenu.childNodes[0].noResize=false;
				}
			}
		</script>
		
<script language=javascript>

//��/��
function window.confirm(str)
{
    execScript('n = msgbox("'+str+'","4132")', "vbscript");
    return(n == 6);
}
</script>
		
	<SCRIPT LANGUAGE="JavaScript">
		
		ShowWait();
		var isLoadingPage = true;
		var tree =null;
		var templateXmlAndHtml = "";
		
	    <%--
            ' ======== ============ =============================
            ' Description: ���ô���������
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%> 	
		createTree();
		
	    <%--
            ' ======== ============ =============================
            ' Description: ������
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>		
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
			tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "0"+tree.attribute_d+"text" + tree.attribute_suffix + "Root"+tree.attribute_d+" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT" +tree.attribute_d+"nodeuuid" + tree.attribute_suffix + "-1" ;
			tree.customIconFun = useSOPMngIcon;

            tree.nodeClick = function clicknode()
			{
			    // && confirm("Current template has been modified, do you want to save it?")
			    if(bTemplateChanged && window.confirm("Current template has been modified, do you want to save it?")){
                    //ShowWait();
			        window.parent.frames("main").saveForTree();
			        //HideWait();
			    }
		        bTemplateChanged = false;
		        
		        
				var currentNode = tree.currentNode; 
				var uuid = currentNode.uuid;
				var pid = currentNode.parentId;
				type =  currentNode.type;
	            var text = currentNode.text;
	            var nodeuuid = currentNode.nodeuuid;
	         //alert(type+":::"+nodeuuid);
                switch (type){
                    case "NODE_ROOT":
                        window.parent.frames("main").location.href ="main.aspx";
                        break;
                    case "<%=Constants.NODE_TYPE_FOLDER_DATA%>":

                        window.parent.frames("main").location.href ="../datasource/dataSourceSetting.aspx?type=" + type + "&uuid=" + uuid + "&text="+text + "&nodeuuid="+nodeuuid;
                        break;
                    case "<%=Constants.NODE_TYPE_FOLDER_REPORT%>":
                        window.parent.frames("main").location.href ="../template/TemplateMain.aspx?type=" + type + "&uuid=" + uuid + "&text=" +text + "&nodeuuid="+nodeuuid;
                        break;

                    case "<%=Constants.NODE_TYPE_DATASERVER%>":
                        var parentId = upperSearchPropertyByKey(tree, "<%=Constants.NODE_TYPE_FOLDER_DATA%>","uuid");
                        window.parent.frames("main").location.href ="../datasource/showDataSource.aspx?type=" + type + "&uuid=" + uuid + "&text=&nodeuuid="+nodeuuid +"&parentTreeId="+parentId;
                        break;
                    case "<%=Constants.NODE_TYPE_DATABASE%>":
                        var parentId = upperSearchPropertyByKey(tree, "<%=Constants.NODE_TYPE_DATASERVER%>","nodeuuid");
                        //alert(parentId);
                        window.parent.frames("main").location.href ="../datasource/showTableTree.aspx?type=" + type + "&uuid=" + uuid + "&text=&nodeuuid="+nodeuuid+"&parentNodeuuid="+ parentId;
                        break;
                    case "<%=Constants.NODE_TYPE_REPORT%>":
                        
                        window.parent.frames("main").location.href ="../main/visualEditPanel.aspx?type=tree&uuid=" + uuid + "&text=&nodeuuid="+nodeuuid + "&parentid=" + pid;
                        break;

                    case "<%=Constants.NODE_TYPE_FOLDER_ACCOUNT%>":
                        
                        window.parent.frames("main").location.href ="../template/Users.aspx?type=&uuid=" + uuid + "&text=&nodeuuid="+nodeuuid;
                        break;

                    default:
                        break;
                }
		    }	
		    document.getElementById('treeviewarea').innerHTML = tree.toString();
		    
		    HideWait();
		}

	    <%--
            ' ======== ============ =============================
            ' Description: ʹ��Icon
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
		function useSOPMngIcon(node, treeObj)
		{
		    var strIconName = "";
			var strPrefix = "../../images/";
			
			switch (node.type){
                case "NODE_ROOT":
                    strIconName = "root.gif";
                    break;
                case "<%=Constants.NODE_TYPE_FOLDER_DATA%>":
                    strIconName = "datasource.gif";
                    break;
                case "<%=Constants.NODE_TYPE_FOLDER_REPORT%>":
                    strIconName = "reportfolder.gif";
                    break;

                case "<%=Constants.NODE_TYPE_DATASERVER%>":
                    strIconName = "dataserver.gif";
                    break;
                case "<%=Constants.NODE_TYPE_DATABASE%>":
                    strIconName = "database.gif";
                    break;
                case "<%=Constants.NODE_TYPE_REPORT%>":
                    strIconName = "reportfile.gif";
                    break;
                case "<%=Constants.NODE_TYPE_FOLDER_ACCOUNT%>":
                    strIconName = "user.gif";
                    break;

                default:
                break;
			}
			return strPrefix +strIconName;
		}		

	    <%--
            ' ======== ============ =============================
            ' Description: ������ʱ����ȡ����Դ����
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>
		function Load()
        {
	        var source= tree.nodes[tree.expandingNode.sourceIndex]
	        var uuid = tree.getAttribute(source, "uuid")
	        type = tree.getAttribute(source, "type");

            //setTimeout("getNodeData('"+type+"','"+uuid+"')",0);


	        getNodeData(type,uuid);
        }
        
	    <%--
            ' ======== ============ =============================
            ' Description: ��ȡ����Դ����
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%>        
        function getNodeData(type,uuid)
        {
            ShowWait();
            var rtn = TreeView.getNodeData(type, uuid, InitChild);
        }
	       
	    <%--
            ' ======== ============ =============================
            ' Description: ��������Դ����
            ' Author: zhao, qing-rong
            ' Side Effects:
            ' Date:2008-11-11
            ' ======== ============ =============================
        --%> 	        
        function InitChild(rtn)
        {
        //debugger;
            if (rtn.error != null) 
            {
                HideWait();
                
                var errorInfo = rtn.error.Message;
                
                if (errorInfo == "Unknown")
                {
                    alert("<%=Resources.VisualTemplate.Tree_Time_Out %>");
                }
                else
                {
                    alert(errorInfo);
                }
                
                tree.emptyNode(tree.expandingNode);
                return;
            }
            /*
            if(rtn.value == null){
                HideWait();
                alert("<%=Resources.VisualTemplate.Tree_Time_Out %>");
                tree.emptyNode(tree.expandingNode);
                return;
            }*/
            
            var treeNodeInfo = rtn.value;
            if (treeNodeInfo.Rows.length < 1)
            {
                HideWait();
                //alert("<%=Resources.VisualTemplate.Tree_Time_Out %>");
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
			        arrNode[arrNode.length] = 'text' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[1]];
			        arrNode[arrNode.length] = 'type' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[2]];
			        arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + treeNodeInfo.Rows[i][arrName[4]];
			        
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
	        return;
        }

    <%--
        ' ======== ============ =============================
        ' Description: ˢ�½ڵ�
        ' Author: zhao, qing-rong
        ' Side Effects:
        ' Date:2008-11-11
        ' ======== ============ =============================
    --%> 		
	function Refresh()
	{		 	 	  
		 tree.focusClientNode(1); 	 
		 tree.freshCurrentNode();
	}
	
	//add by lzy,locate treenode on entering from mail. 
	function doLocateOnLoad()
	{
		var freshPath = "<%=(String)Session[Constants.TREE_EXPAND_PATH]%>";
		
	    isLoadingPage = false;
	    if (freshPath != "")
	    {
		    tree.freshPath = freshPath.split(",");
		    tree.freshPath = tree.freshPath.reverse();
		    isLoadingPage = false;
		    //alert(tree.freshPath.length)
		    //tree.focusClientNode(1); 
		    tree.locateNode("UUID");
	    }	
	}	
	
	
    //treeObject: tree object pointer

    //nodeType sample:"NODE_SOP"

    //key: which value will be returned.

    //return value: null--can't find.

    function upperSearchPropertyByKey(treeObject, nodeType, key)
    {
      var reValue = null;
      var node = treeObject.currentNode;
      if (key.length == 0 || node == null)
      {
         alert("Can't find the current node.");
         return reValue; 
      }
      var source = treeObject.nodes[node.sourceIndex];
      while (treeObject.getAttribute(source, "type") != "NODE_ROOT")
      {   
          if (treeObject.getAttribute(source, "type") == nodeType)
         {   
             reValue = treeObject.getAttribute(source, key);         
             break; 
          }
          node = node.parentNode;  
          source = treeObject.nodes[node.sourceIndex];
      }   

      return reValue;
    }
    	
        </script>


