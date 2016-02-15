<%@ Page Language="C#" AutoEventWireup="true" CodeFile="treeView.aspx.cs" Inherits="webroot_aspx_dashboard_treeView" %>
<%@ Import Namespace="com.inventec.system" %>

<html>
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
</head>
<fis:header id="header2" runat="server"/>
<body  leftMargin="0" topMargin="0" rightMargin="0" bottommargin="0" style="width:100%;" >
    <form id="Form2" runat="server">
    </form>
<table style="width:100%; border:0px" id="tableTitle">
    <tr>
    <td class="title" style=" border:0px; word-wrap:normal; height:20px">&nbsp;&nbsp;Function Tree</td>
    </tr>
    </table>
    <div style= "position:absolute;right:25px;top:2px;" id="freshimg">
        <INPUT type="image" id="dUserManager" src="../../images/authority-h.gif" alt="Authority Manager" onclick="userManagerClick();"/>
    </div> 
<%--  <div style= "position:absolute;right:0px;top:0px;">
        <INPUT style="" type="image" src="../../images/nav2.gif"value="button" id="shleft" name="shleft" onclick="OpenClose()">
    </div> --%>
    <table width=96% style="height:98%;_height:expression(document.body.clientHeight-22);" id="tablediv" style=" margin-right:0px" border=1px >
        <tr>
            <td width="100%" height=100%>
                <%--<div id=treeviewarea style="width:100%;height:97%;overflow-y:scroll;overflow-x:scroll;background-color:#FFFFFF;" >--%>
                <div id=treeviewarea style="width:100%;height:100%;overflow-y:auto;overflow-x:auto;background-color:#FFFFFF;" >
                </div>
            </td>
        </tr>
    </table>
</body>
<fis:footer id="footer1" runat="server"/> 
</html>
<%--    <script language="javascript">


debugger;
            function OpenClose()
			{
			    var expendPic = "images/nav2.gif";
			    var srcValue = document.getElementById("shleft").src;
			    var from = srcValue.lastIndexOf(expendPic);
			    var to = srcValue.length;
			    var tmp = srcValue.substring(from, to);
	
			    if (tmp == expendPic) 
			    {
			        parent.menu.cols="20,*";
					document.getElementById("shleft").src="../../images/nav.gif";
					document.getElementById("tablediv").style.display = "none";
					document.getElementById("tableTitle").style.display="none";					
					parent.document.getElementById("menu").noResize=false ;		
	            }
			    else
				{
					parent.menu.cols="230,*";
					document.getElementById("shleft").src="../../images/nav2.gif";
					document.getElementById("tablediv").style.display = "";
					document.getElementById("tableTitle").style.display="";					
					parent.document.getElementById("menu").noResize=false;
				}
			}
	</script>--%>
<%--	<script for=window language=javascript event=onresize>

    return ResizeElement()
    </script>--%>
	<SCRIPT LANGUAGE="JavaScript">
	
	    getEditUserInfo();
	    
	    window.parent.frames("main").location.href ="dashboardList.aspx";
	    	    
	    if(window.parent.isAuthorityUsermanager=="True")
	    {
	        document.getElementById("dUserManager").style.display="";
	    }
	    else
	    {
	        document.getElementById("dUserManager").style.display="none";
	    }
	    
		ShowWait();
		var isLoadingPage = true;
		var tree =null;
		createTree();
		
		function userManagerClick()
		{
		    //window.parent.frames("main").location.href ="../../../UserManager/aspx/authorities/accountauthority.aspx";
		    var editor=	window.parent.editor;	
		    editor=encodeURI(Encode_URL2(editor));        
            var dlgFeature = "dialogHeight:600px;dialogWidth:1010px;center:yes;status:no;help:no";      
            var dlgReturn=window.showModalDialog("../../../UserManager/aspx/authorities/accountauthority.aspx?logonUser="+editor, window, dlgFeature);
		
		}
		
		//document.body.style.backgroundColor ="rgb(156,192,248)"
		//ResizeElement();


		
//		function ResizeElement()
//	    {
//		    document.getElementById('tablediv').style.height=window.frameElement.clientHeight-document.getElementById('tableTitle').offsetHeight+4;
//		    document.body.style.backgroundColor ="rgb(156,192,248)"
//		}
//		
        function getEditUserInfo()
        {
            if(window.parent.editor=="")
            {
                //从令牌中得到
                //window.parent.editor="qqq";
                var rtn=com.inventec.portal.dashboard.AccountUserInfo.GetLogonUser(window.parent.tokenString);
                if (rtn.error != null) 
                {
                    //alert(rtn.error.Message);
                    //window.parent.location.href ="errorpage.aspx?errormsg=Invalid logon user";
                    return;
                }
                else 
                { 
                    if(rtn.value==null||rtn.value.LogonUser=="")
                    {
                        //alert("Invalid logon user.");
                        return;
                    }
                    
                    window.parent.editor=rtn.value.LogonUser;
                    window.parent.isAuthorityUsermanager=rtn.value.IsAuthorityUsermanager;
                    window.parent.isAuthorityDashboard = rtn.value.IsAuthorityDashboard;
                    window.parent.isAuthorityUsermanager = true;
                    window.parent.isAuthorityDashboard = true;

                }             
                //alert(window.parent.editor);
            }
        }
        
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
			tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "0"+tree.attribute_d+"text" + tree.attribute_suffix + "Dashboard List"+tree.attribute_d+" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT" +tree.attribute_d+"nodeuuid" + tree.attribute_suffix + "-1" ;
			tree.customIconFun = useSOPMngIcon;

            tree.nodeClick = function clicknode()
			{
				var currentNode = tree.currentNode; 
				var uuid = currentNode.uuid;
				var pid = currentNode.parentId;
				type =  currentNode.type;
	            var text = currentNode.text;
	            var nodeuuid = currentNode.nodeuuid;
	         
                switch (type){
                    case "NODE_ROOT":
                        if(window.parent.isAuthorityDashboard=="True")
                        {
                            window.parent.frames("main").location.href ="dashboardList.aspx";
                        }
                        break;
                    case "DASHBOARD_FILE":                    
                    
                        var stageType=getStageTypeByWinId(uuid);                    
                        if(stageType=="<%=Constants.FA_STAGE%>")
                        {
                            window.parent.frames("main").location.href="dashboardSetting.aspx?uuid=" + uuid;
                        }
                        else
                        {
                            window.parent.frames("main").location.href="dashboardSmtSetting.aspx?uuid=" + uuid+"&stageType="+String(stageType);
                        }                    
                        break;
                    default:
                        break;
                }
		    }	
		    document.getElementById('treeviewarea').innerHTML = tree.toString();
		    HideWait();
		}

		function useSOPMngIcon(node, treeObj)
		{
		    var strIconName = "";
			var strPrefix = "../../images/";
			
			switch (node.type){
                case "NODE_ROOT":
                    strIconName = "reportfolder.gif";
                    break;
                case "DASHBOARD_FILE":
                    strIconName = "reportfile.gif";
                    break;
                default:
                break;
			}
			return strPrefix +strIconName;
		}		

		function Load()
        {
	        var source= tree.nodes[tree.expandingNode.sourceIndex]
	        var uuid = tree.getAttribute(source, "uuid")
	        type = tree.getAttribute(source, "type");
	
	        getNodeData(type,uuid);
        }
     
        function getNodeData(type,uuid)
        {
            ShowWait();
            //var rtn = TreeView.getNodeData(type, uuid, InitChild);
            var rtn;    
            try
            {
               if(window.parent.isAuthorityDashboard=="True")
               {
                   rtn = webroot_aspx_dashboard_treeView.getNodeData(type, uuid);
               }
               else
               {
                   rtn = webroot_aspx_dashboard_treeView.getEmptyNodeData();
               }
            }
            catch(e)
            {
               alert("Can't get data from server.");
               return;
            }            
            
            InitChild(rtn);
            
        }
	       
        function InitChild(rtn)
        {
            if (rtn.error != null) 
            {
                HideWait();
                
                var errorInfo = rtn.error.Message;
                
                if (errorInfo == "Unknown")
                {
                    alert("The connection is time out. Please refresh!");
                }
                else
                {
                    alert(errorInfo);
                }
                
                tree.emptyNode(tree.expandingNode);
                return;
            }
            
            if(rtn.value==null)
            {
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
		    
		    tree.locateNode("UUID")
		    HideWait();
	        return;
        }
	
//        MzTreeView.prototype.freshNodeRoot = function( focusNodeId)
//        {	  
//            var node = this.node[1];
//          
//	        this.delChildrenNodeInfo(node);
//	        this.delChildrenHTML(node.id);
//        	
//	        node.isLoad = false;
//	        node.isExpand = false;
//	        node.hasChild = true;
//	        node.childNodes = new Array();	
//	        this.switchIcon(node.id);
//	        this.expand(node.id, true);
//	        //this.focusClientNode(node.id);
//        	
//	        return true;
//        }

    MzTreeView.prototype.insertNode1  = function(newUUID)
    {	
        this.freshPath = new Array(); 
        this.freshPath[this.freshPath.length] = newUUID;
    	
        var node = this.node[1];

        this.focusClientNode(node.id,true);  
        this.delChildrenNodeInfo(node);
        this.delChildrenHTML(node.id);
    	
        node.isLoad = false;
        node.isExpand = false;
        node.hasChild = true;
        node.childNodes = new Array();
        		
	    this.freshCurrentNode(0)

    };
    
    function getStageTypeByWinId(winId)
    {
         var rtn;    
         try
         {
            rtn = com.inventec.portal.dashboard.common.DashboardCommon.GetStageType(winId);
         }
         catch(e)
         {
            alert("Can't get data from server.");
            return;
         }
        
        if (rtn.error != null) 
        {
            alert(rtn.error.Message);
            return;
        }
        else 
        { 
            return rtn.value;
        }

    }
    	
    </script>


