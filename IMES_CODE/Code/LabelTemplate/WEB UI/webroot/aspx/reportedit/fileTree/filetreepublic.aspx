
<script type="text/javascript">
        var type;
		var goods_rs_main;
		var treeId = 2;  
		var isLoadingPage = true;
		var rtnStatus;
		var col_uuid = 0;
		var col_name = 1;
		var col_type = 2;
	    var col_hasChild = 3;
		var col_status = 4;
		var tree =null;
		var projectId;
		var nodeName;
		var  fieldType ;
		var  serverId = "";
		var databaseName = "";
		var fieldName;
		var tableNameVsFieldname;
//		var  serverId = window.parent.serverIdDeposit;
//		var databaseName = window.parent.databaseNameDeposit;
		
//		var dbServerName;
//        var userName;
//        var password;
//        var dicObjDatabase;
      // generateTreeCondtion();
		
	//	createTree(serverId, databaseName);
		
        function generateTreeCondtion()
        {
             alert(serverId);
             var rtnDb = com.inventec.fisreport.manager.ReportManager.getDataSourceDatabaseListForSelect(serverId);
              if(rtnDb.error != null)
	            {
	                alert(rtnDb.error);
	            }
	            else
	            {
	                alert(rtnDb.Columns.length);
	                 for(var i=0; i <  rtnDb.Columns.length; i++)
                    {
                        for(var j = 0; j <  rtnDb.Rows.length; j++)
                       {
                              databaseName =  rtnDb.Rows[i]["name"];
                       }
                    }
	            }
	}       
//             get structure from parent
//            dbServerName = window.parent.objReportSturcture.ReportDescriptionInfo.DatabaseServer;
//            databaseName = window.parent.objReportSturcture.ReportDescriptionInfo.Database;
//            userName = window.parent.objReportSturcture.ReportDescriptionInfo.DatabaseUser; 
//            serverId = window.parent.serverIdDeposit;
//            dicObjDatabase =  com.inventec.fisreport.manager.ReportManager.getDictionaryObj();
//             if(dicObjDatabase.error!=null)
//             {
//                    alert(dicObjDatabase.error.Message);
//             }
//            else
//            {
//                    setKeyValueOfDic(dicObjDatabase.value, "data_server_name", dbServerName);
//                    setKeyValueOfDic(dicObjDatabase.value, "data_base_name", databaseName);
//                    setKeyValueOfDic(dicObjDatabase.value, "data_server_user_name", userName);
//                    setKeyValueOfDic(dicObjDatabase.value, "data_server_pwd", password);  
//            } 

		function createTree(serverIdPar, databaseNamePar)
		{	 
		    serverId = serverIdPar;
		    databaseName = databaseNamePar;
			tree = new MzTreeView("tree");
			tree.LoadData = "Load()";
			tree.icons["property"] = "property.gif";
			tree.icons["css"] = "collection.gif";
			tree.icons["book"]  = "book.gif";
			tree.iconsExpand["book"] = "bookopen.gif"; //?1?a¨º¡À??¨®|¦Ì?¨ª???
			
			tree.setIconPath("./"); //?¨¦¨®??¨¤???¡¤??
            tree.useArrow = false;
            
			tree.nodes["-1_" + tree.total++] = "uuid" + tree.attribute_suffix + "0"+tree.attribute_d+"text" + tree.attribute_suffix + htmEncodeString(databaseName) + tree.attribute_d +" hasChild" + tree.attribute_suffix + "true"+tree.attribute_d+" type" + tree.attribute_suffix + "NODE_ROOT";
	
			
			tree.customIconFun = useSOPMngIcon;
		
			tree.nodeClick = function clicknode()
			{
				var currentNode = tree.currentNode; 
				var uuid = currentNode.uuid;
				var pid = currentNode.parentId;
				type =  currentNode.type;
				nodeName = currentNode.text;
				fieldType = currentNode.fieldType;
				fieldName = currentNode.fieldName;
			
				var sourceId = pid + "_" + currentNode.id;
	            var EDIT_FLAG = "0";
	            var projectId;
	            var text = currentNode.text;
	            var procedureName = "";
	             var pcbRevisionId ;
                //getFieldType();  // for others who use the attributename of the field node
                 var outerCaller= treeClickCallBack;
                //type="table"is table node type; type="field"is field node type
                if(type == "field")
                {
                        var parentNodeName = upperSearchPropertyByKey(tree, "table", "text") ;
                        tableNameVsFieldname = "["+parentNodeName+"]." + "[" + fieldName + "]";
                }
                if (typeof(outerCaller) == "function")	         
                eval(outerCaller)(); 
            
	         }
		    document.getElementById('_treeviewarea').innerHTML = tree.toString();
		}
	
function getNodeData(type,uuid)
{
   //get structure from parent
//    dbServerName = window.parent.objReportSturcture.ReportDescriptionInfo.DatabaseServer; 
//    databaseName = window.parent.objReportSturcture.ReportDescriptionInfo.Database;
//    userName = window.parent.objReportSturcture.ReportDescriptionInfo.DatabaseUser; 
//    password = window.parent.passwordDeposit;
  ShowWait();
    var rtn = com.inventec.template.reportedit.GetFileTreeData.getNodeData(serverId, databaseName, type, nodeName);

    if (rtn.error != null) 
    {
        var errorInfo = rtn.error.Message;
                
        if (errorInfo == "Unknown")
        {
//            alert("<%=Resources.alertMsg.Tree_Time_Out %>");
        }
        else
        {
            alert(errorInfo);
        }
        tree.emptyNode(tree.expandingNode);
       HideWait();
    }
    else 
    {
       
        InitChild(rtn.value);
         HideWait();
    }
}
function Load()
{
	var source= tree.nodes[tree.expandingNode.sourceIndex];
	var uuid = tree.getAttribute(source, "uuid");
	type = tree.getAttribute(source, "type");
	nodeName = tree.getAttribute(source, "text");
    fieldType = tree.getAttribute(source, "fieldType");
    fieldName  =  tree.getAttribute(source, "fieldName");
 
	getNodeData(type,uuid);
}

function getFieldType()
{
        alert(fieldType+"fieldType");
        return fieldType;
}
//function getNodeAttr(tree, Attr)
//{	
//	tree.getNodeProperty(tree.currentNode, Attr)
//}

function InitChild(treeNodeInfo)
{
		if(treeNodeInfo != null && typeof(treeNodeInfo)=="object")
		{
		    var i=0;
		    var nameArr = new Array() ;
	        for( i = 0;i < treeNodeInfo.Columns.length;i++)
	        {
	            nameArr[i] = treeNodeInfo.Columns[i].Name;
	        }
		
		    for(var i=0; i<treeNodeInfo.Rows.length;i++)
		    {	
			    var arrNode = new Array();
			    
			   
                if(type=="NODE_ROOT")
                {
                    arrNode[arrNode.length] = 'text' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[0]];//treePropertyArr[0];//'uuid:' + rs.Fields(col_uuid).value;
                    arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[1]];//treePropertyArr[1];//'text:' + rs.Fields(col_name).value;
                    arrNode[arrNode.length] = 'type' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[2]];//treePropertyArr[1];//'text:' + rs.Fields(col_name).value;
              
                }
                else if(type == "table")
                {
                    arrNode[arrNode.length] = 'text' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[0]]+
                                                                "("+ treeNodeInfo.Rows[i][nameArr[1]] +"("+ treeNodeInfo.Rows[i][nameArr[2]]+")"+")";
                    arrNode[arrNode.length] = 'fieldType' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[1]] +
                                                                 "("+ treeNodeInfo.Rows[i][nameArr[2]]+")"
                    arrNode[arrNode.length] = 'type' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[4]];     
                      
                    arrNode[arrNode.length] = 'fieldName' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[0]];   
                }

//			    arrNode[arrNode.length] = 'type' + tree.attribute_suffix + treeNodeInfo.Rows[i][nameArr[2]];//treePropertyArr[2];//'type:' + rs.Fields(col_type).value;
//			    var hasChild;
//			    if(treeNodeInfo.Rows[i][nameArr[3]] == "0")
//			    {
//				    hasChild = "false"; 
//			    }
//			    else
//			    {
//				    hasChild = "true";
//			    }				
//			    arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + hasChild;//'hasChild:' + hasChild;
//		        if(type == "NODE_ROOT")
//		        { 
//			        arrNode[arrNode.length] = 'status' + tree.attribute_suffix +  treePropertyArr[4];
//		        }

			    tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
    			
		    }
		}
	

	tree.dataFormat();
	tree.load(tree.expandingNode.id);
	tree.buildNode(tree.expandingNode.id);
	//new add by lzy to expand the last visiting node.
	if (!isLoadingPage){
		tree.locateNode("UUID")
	} else {		
		doLocateOnLoad();
	}
	return;
}

	function Refresh(){		 	 	  
		 tree.focusClientNode(1); 	 
		 tree.freshCurrentNode();
	 
	}
	//add by lzy,locate treenode on entering from mail. 
	function doLocateOnLoad(){
	}	
//}
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
function useSOPMngIcon(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
	//SOP_file_link.gif
	 if (node.type == "NODE_ROOT")
	 {	 
	 	strIconName = 'database.gif';	
	 }
	 else if(node.type == "table" )
	 {
	    strIconName = 'Tables.jpg';
	 }
	 else if(node.type == "field")
	 {
	    strIconName = 'dbfield.png';
	 }
	return strPrefix +strIconName;
}	
	
</script>

