<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_TreeView, App_Web_treeview.aspx.7a399c77" theme="MainTheme" %>

<%@ Import Namespace="com.inventec.system" %>
<html>
<head id=Head1 runat="server">
    <title>Untitled Page</title>
    <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>

</head>
<fis:header id="header2" runat="server"/>
<body leftMargin="0" topMargin="0" rightMargin="0" bottommargin="0" style="width:100%" onload="doLocateOnLoad()">
    <form id=Form1 runat="server">
    </form>
<table  style="width:100%; border:0px" id="tableTitle">
    <tr>
    <td class="title" style=" border:0px; word-wrap:normal;">Function Tree<button onclick="debugger;tree.freshCurrentNode(10)" >fresh</button></td>
    </tr>
    </table>
    
    <table width=100% height=100% id="tablediv" style=" margin-right:0px">
        <tr>
            <td width="100%" height=100%>
                <div id=treeviewarea style="width:100%;height:97%;overflow-y:scroll;overflow-x:scroll;background-color:#FFFFFF;" >
                  </div>
            </td>
        </tr>
    </table>
    
</body >
<fis:footer id="footer1" runat="server"/> 
</html>


         
	<SCRIPT LANGUAGE="JavaScript">
	
	
	var a ="";
		
		ShowWait();
		var isLoadingPage = true;
		var tree =null;
		var printTemplateInfo = true;
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
			tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "-1"+tree.attribute_d+"text" + tree.attribute_suffix + "DataSet"+tree.attribute_d+" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT" +tree.attribute_d+"nodeuuid" + tree.attribute_suffix + "-1" ;
			tree.customIconFun = useSOPMngIcon;
           
            tree.nodeClick = function clicknode()
			{
               
				var currentNode = tree.currentNode; 
				var uuid = currentNode.uuid;
				var pid = currentNode.parentId;
				type =  currentNode.type;
	            var text = currentNode.text;
	            var nodeuuid = currentNode.nodeuuid;
	            //alert(uuid);
	            
	            //var nodePath =tree.getNodePathByKey("UUID");
                //a = nodePath.join(",");      
	            //alert("----"+a+"-------");
	          
               
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
                case "1":
                    strIconName = "reportfolder.gif";
                    break;
                case "0":
                    strIconName = "reportfile.gif";
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
           InitChild(uuid);
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
                for (var i=0; i < 5; i++)
                {
                    //alert(treeNodeInfo.Rows[i]["nodeuuid"]);
                    var arrNode = new Array();
			        arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + rtn + i;
			        arrNode[arrNode.length] = 'text' + tree.attribute_suffix + "DS" + i;
			        arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "1";
			        arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + "1";
			        
			       	
			        arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + "true";
			        tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
                }
            
            
            tree.dataFormat();
	        tree.load(tree.expandingNode.id);
	        tree.buildNode(tree.expandingNode.id);
	        
	        //new add by lzy to expand the last visiting node.
	        
		    tree.locateNode("uuid")
		    
		    HideWait();
	       
        }
        
 
	function doLocateOnLoad()
	{
		 tree.freshPath=showDefaultPage();
		 
		 tree.locateNode("UUID");
	     
	}
	
	 function showDefaultPage()
        {
	        var index = "-141,-14";
//	        index.join
//            return index.split(",");
            return index.split(",");
        }
    	
        </script>


