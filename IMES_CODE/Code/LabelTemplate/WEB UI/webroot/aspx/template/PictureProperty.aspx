<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: Picture��������ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 

<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_PictureProperty, App_Web_pictureproperty.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
    <title>Picture Property</title>
 <%--��ֹ���´����ύ--%>
<base target="_self">   
<script type="text/javascript" src="../../commoncontrol/btnTabs.js"></script>
<script language="JavaScript" src="../../commoncontrol/treeView/treeViewControl.js"></script>
<script type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>
</head>
<%--<fis:header id="header1" runat="server"/>--%>


<body class="bgBody" >
  
    <form id="form1" runat="server"  >

     <div class="propertyDialog"> 
  
    <div id="con" ></div>
    <div id="div1" style="display:none" class="propertyDialogContent">
        <%--<bug>
            BUG NO:ITC-992-0004 
            REASON:ȥ��Upload��ť
      </bug>--%>
		<TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px" >
      
        <TR>
            <TD style="width:15%"><%=Resources.Template.objectName%>:</TD>
            <TD>
	      
                <input type="text" id="objectName" class="inputStyle" maxlength="20">
            </TD>
        </TR>
	   </TABLE>
          <iframe name="action" id="action" src="uploadFile.aspx"  scrolling="no"  frameborder="0" width="100%"> 
	          
	     </iframe> 
        
    </div>    
   
    
    
    <div id="div2" style="display:;" class="propertyDialogContent" >
		    <FIELDSET >
	        <LEGEND ><%=Resources.Template.position%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.x%>:</TD>
	                <TD>
	                    <input type="text" NAME="x" id="x" class="inputStyle"></input>mm
	                </TD>
                    </TR>
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.y%>:</TD>
	                <TD>
	                    <input type="text" NAME="y" id="y" class="inputStyle"></input>mm
	                </TD>
                    </TR>
                 </TABLE>
    	       
	        </FIELDSET>
	        <div style="height:10px"></div>
	        <FIELDSET  >
	        <LEGEND><%=Resources.Template.eddy%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.angle%>:</TD>
    	           
	                <TD>
	                    <SELECT NAME="angle" id="angle" class="inputStyle" >
	                        <option value="<%=Constants.ANGLE_0%>"><%=Constants.ANGLE_0%></option>
	                        <option value="<%=Constants.ANGLE_90%>"><%=Constants.ANGLE_90%></option>
	                        <option value="<%=Constants.ANGLE_180%>"><%=Constants.ANGLE_180%></option>
	                        <option value="<%=Constants.ANGLE_270%>"><%=Constants.ANGLE_270%></option>
	                       
	                    </SELECT>
	                </TD>
                    </TR>
                 </TABLE>    	     
	        </FIELDSET>
	    
	  
	</div>
    
    <div id="div3" style="display:none;" class="propertyDialogContent">
        <div style="margin-bottom:10px">
	        <INPUT TYPE="checkbox" NAME="matchcheck" id="matchcheck" onclick="checkMatch()" unchecked>Set Dependency
	    </div>
	    <div  style="width:40%;float:left;" >
            <FIELDSET >
	        <LEGEND><%=Resources.Template.dataset%></LEGEND>  
	            <div id=treeviewarea style="width:100%;height:150px;overflow:auto;" >
                </div>
	         </FIELDSET> 
         </div>
         <div  style="width:40%;float:left;" >
            <FIELDSET >
            <%--<bug>
            BUG NO:ITC-992-0051 
            REASON:�ý���
            </bug>--%>
	        <LEGEND><%=Resources.Template.matchArea%></LEGEND>
	            <div style="height:150px;">
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5"  >
			    <TR>
	                <TD colspan="2"><INPUT TYPE="checkbox" NAME="reverse" id="reverse" disabled><%=Resources.Template.reverse%></TD>
	            </TR>
			    <TR>
	                <TD colspan="2">&nbsp;<%=Resources.Template.matchValue%></TD>
	            </TR>
                <TR>
	                <TD style="width:25%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT TYPE="radio" NAME="match" id="nullradio"  unchecked onclick="checkValue(this.id)" disabled>NULL</TD>
			    <TD></TD>
	            </TR>
                <TR>
	                <TD style="width:25%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<INPUT TYPE="radio" NAME="match" id="matchradio"  checked onclick="checkValue(this.id)" disabled>Value</TD>
	                <TD>
	                    <input type="text" NAME="matchvalue" id="matchvalue" style="width:100px" maxlength="50" disabled></input>
	                 </TD>
                </TR>
                </TABLE>  
            	 </div>
            	 
	          </FIELDSET>
         </div>
  	</div>
     <div   style="margin-top: 10px;" align="right" >
                <button id="Button1" type="button"  onclick="Save()"  ><%=Resources.Template.okButton%></button> 
      <%-- <asp:Button ID="save" runat="server" Text="Save"   style="width:60px;cursor:hand" OnClick="btnSubmit_Click"  OnClientClick="return preTest()" />--%>&nbsp;&nbsp;&nbsp;&nbsp;<button id="cancel" type="button"  onclick="Cancel()" ><%=Resources.Template.cancelButton%></button> 
        
	</div>
	
	 </div>       
    <asp:HiddenField ID="picId" runat="server"   />
    <asp:HiddenField ID="picValue" runat="server" />
    <asp:HiddenField ID="picHeight" runat="server"   />
    <asp:HiddenField ID="picWidth" runat="server" />
  
    <asp:HiddenField ID="submitFlag" runat="server"   />
    
 
   
    </form>
   
   <input type="hidden" id="selfield" />
	<input type="hidden" id="seldataset" />
	<input type="hidden" id="seltype" />
     
   

</body>


</html>
<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 
<SCRIPT LANGUAGE="JavaScript">
<!--
var templateXmlandHtml;
var printTemplateInfo = "";
var pictureObject = "";
var areaHeight = 0;
var templateWidth = 0;
var pageIndex = 1;
var parentId;
var parentObj;
var belongSecCell = false;
var isLoadingPage = true;
var tree =null;
var dataObject = "";
var div1ErrorFlag = false;
//var mm_per_inch = 25.4;//25.4mm/inch
var convertTemplateWidth = 0;
var convertAreaHeight = 0;
var pixel_per_inch_x;
var pixel_per_inch_y;




//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createBtnTabs
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Tab��ǩ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  
function createBtnTabs()
{
    tabs = new clsButtonTabs("tabs");

	tabs.preSelectChanged = preCallback;
	tabs.selectChanged = callback;
		 
	for (var i=0; i<3; i++)
	{
	     var temp = new clsButton("tabs");
	    if (i == 0) {
	        temp.normalPic = "../../images/pic" +"-1.jpg";
		    temp.selPic = "../../images/pic" +"-2.jpg";
		    temp.disablePic = "../../images/pic" +"-3.jpg";
	    } else if (i == 1){
		    temp.normalPic = "../../images/fields2" +"-1.jpg";
		    temp.selPic = "../../images/fields2" +"-2.jpg";
		    temp.disablePic = "../../images/fields2" +"-3.jpg";
		} else {
		    temp.normalPic = "../../images/fields3" +"-1.jpg";
		    temp.selPic = "../../images/fields3" +"-2.jpg";
		    temp.disablePic = "../../images/fields3" +"-3.jpg";
		}
			
		tabs.addButton(temp);
		
	}
		
	//tabs.diableTab(0,true);
	con.innerHTML = tabs.toString();
	tabs.initSelect(1);

}

createBtnTabs();
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	preCallback
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�л�Tabҳʱ��֤������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
function preCallback(index)
{  
    
    var bLeaveFlag = true;
	switch(pageIndex){
	    case 0: 
       	    bLeaveFlag = !leaveDiv1();
		    break;
	    case 1: 
            bLeaveFlag = !leaveDiv2();
		    break;
         case 2: 
            bLeaveFlag = !leaveDiv3();
		    break;
        default:
            break;	
    }
	return bLeaveFlag;
}



	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callback
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�л�Tabҳ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 	
function callback(index)
{  
    pageIndex = index;

    switch(index){
	    case 0: 
            document.getElementById("div1").style.display = "";
		 	document.getElementById("div2").style.display = "none";
		 	document.getElementById("div3").style.display = "none";
		 	
		    break;
	    case 1: 
            document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "";
		 	document.getElementById("div3").style.display = "none";
		    break;
         case 2: 
            document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "none";
		 	document.getElementById("div3").style.display = "";
		    break;
         
        default:
            break;	
     }
         
     return true;
}
	
	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	createTree
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	���ô���������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 	
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
	    document.getElementById("selfield").value = text;
	    document.getElementById("seldataset").value = nodeuuid;
	    document.getElementById("seltype").value = type;
	    if (type == "0" && document.getElementById("matchcheck").checked) {
	        document.getElementById("nullradio").disabled = false;
            document.getElementById("matchradio").disabled = false;
            document.getElementById("matchvalue").disabled = false;
            document.getElementById("reverse").disabled = false;
	    } else {
	        document.getElementById("nullradio").disabled = true;
            document.getElementById("matchradio").disabled = true;
            document.getElementById("matchvalue").disabled = true;
            document.getElementById("reverse").disabled = true;
	    }
	            
	 }	
		   
	 document.getElementById('treeviewarea').innerHTML = tree.toString();
	
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	useSOPMngIcon
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	ʹ��Icon
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	    
function useSOPMngIcon(node, treeObj)
{
    var strIconName = "";
	var strPrefix = "../../images/";
			
	switch (node.type){
        case "NODE_ROOT":
            strIconName = "root.gif";
            break;
        case "1":
            strIconName = "dataserver.gif";
            break;
        case "0":
             strIconName = "database.gif";
             break;
               
        default:
             break;
	}
    return strPrefix +strIconName;
}		

	    
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Load
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	������ʱ����ȡ����Դ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~        
function Load()
{
    var source= tree.nodes[tree.expandingNode.sourceIndex]
    var uuid = tree.getAttribute(source, "uuid")
    ShowWait();
    InitChild(uuid);
}
       
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	InitChild
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��������Դ����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~   	       
function InitChild(uuid)
{
    if (uuid == "-1") {
        for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
            var arrNode = new Array();
			arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + i;
			arrNode[arrNode.length] = 'text' + tree.attribute_suffix + printTemplateInfo.DatasettingList[i].DataSetName;
			arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "1";
			arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + "root";
    		arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + "true";
			tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
        }
                
                
     } else {
               
        for (i=0; i < printTemplateInfo.DatasettingList[uuid].Fields.length; i++) {
            var arrNode = new Array();
			arrNode[arrNode.length] = 'uuid' + tree.attribute_suffix + i;
			arrNode[arrNode.length] = 'text' + tree.attribute_suffix + printTemplateInfo.DatasettingList[uuid].Fields[i].FieldName1;
			arrNode[arrNode.length] = 'type' + tree.attribute_suffix + "0";
			arrNode[arrNode.length] = 'nodeuuid' + tree.attribute_suffix + printTemplateInfo.DatasettingList[uuid].DataSetName;
    		arrNode[arrNode.length] = 'hasChild' + tree.attribute_suffix + "false";
			tree.nodes[tree.expandingNode.sid + '_' + ++tree.total] = arrNode.join(tree.attribute_d);
         }
     }
     tree.dataFormat();
	 tree.load(tree.expandingNode.id);
	 tree.buildNode(tree.expandingNode.id);
	        
	 //new add by lzy to expand the last visiting node.
	        
	 tree.locateNode("UUID")
		    
     HideWait();
	 isLoadingPage = false;
		    
	       
}








//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{

    
    //��ȡ��������printTemplateInfo����
//    printTemplateInfo = window.dialogArguments.printTemplate;



    var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
    if (ret.error != null) {
        alert(ret.error.Message);
    } else{
        templateXmlandHtml = ret.value;
         printTemplateInfo = templateXmlandHtml.PrintTemplateInfo;
         
        
           
    }
  
    
//    templateWidth = printTemplateInfo.TemplateWidth;
    pixel_per_inch_x = window.dialogArguments.pixel_per_inch_x;
    pixel_per_inch_y = window.dialogArguments.pixel_per_inch_y;
    //��ȡҪ�༭��barcode����
    var ret = searchObject(window.dialogArguments.id);
    pictureObject = ret.object;
    areaHeight =  ret.areaHeight;
    templateWidth = ret.templateWidth;
    convertAreaHeight =  ret.convertAreaHeight;
    convertTemplateWidth =  ret.convertTemplateWidth;
    belongSecCell =  ret.belongSecCell;
    
    searchDataObject(pictureObject.ObjectName, "");
    


   //Tab3ҳ��
    createTree();


  //    if (printTemplateInfo.DatasettingList.length > 0 ) {
        
    

        if (dataObject.OutputFiledDep != "") {
            document.getElementById("matchcheck").checked = true;
            document.getElementById("reverse").disabled = false;
            
            if (dataObject.NullValue == "1") {
                document.getElementById("nullradio").checked = true;
                document.getElementById("matchvalue").disabled = true;
            } else {
                document.getElementById("matchradio").checked = true;
                document.getElementById("matchvalue").value = dataObject.DependencedValue;
                document.getElementById("matchvalue").disabled = false;
            }
            if (dataObject.Reverse != "") {
                document.getElementById("reverse").checked = true;
            } else {
                document.getElementById("reverse").checked = false;
            }
        }
    
//    }
    //Tab1ҳ��
    
    document.getElementById("objectName").value = pictureObject.RealObjectName;
    document.getElementById("<%=picId.ClientID %>").value = pictureObject.Uuid;
    document.getElementById("<%=picValue.ClientID %>").value = pictureObject.Value;
    document.getElementById("<%=picHeight.ClientID %>").value = pictureObject.HeightPixel;
    document.getElementById("<%=picWidth.ClientID %>").value = pictureObject.WidthPixel;
    
    //Tab2ҳ��
    if (pictureObject.X == "") {
        document.getElementById("x").value = "0";
    } else {
        document.getElementById("x").value = pictureObject.X;
    }
     if (pictureObject.Y == "") {
        document.getElementById("y").value = "0";
    } else {
        document.getElementById("y").value = pictureObject.Y;
    }
   
    if (pictureObject.Angle != "") {
        for (i = 0; i < document.getElementById("angle").options.length; i++) {
            if (pictureObject.Angle == document.getElementById("angle").options[i].value)
            {
                document.getElementById("angle").options[i].selected = true;
             }
        }
    }
     try {
        document.getElementById("x").focus();
    } catch(e) {
    }
    

    
}




initPage();

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	searchDataObject
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����objectName(id)��printTemplateInfo�ṹ��DataObject�������ҵ���Ӧ��dataObject����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function searchDataObject(id, setDataObject)
{
 
 
    for (i=0 ;i<printTemplateInfo.DataObjects.length; i++) {
        if (id == printTemplateInfo.DataObjects[i].ObjectName) {
            if (setDataObject == "") {
                dataObject = printTemplateInfo.DataObjects[i];
            } else {
                printTemplateInfo.DataObjects[i] = setDataObject;
            }
            break;
        }
    }
  
}       
        
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Cancel
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	ȡ���˴β������رմ��ڡ�
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Cancel()
{

    window.close();
}





//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkValue
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	ѡ��nullvalue radio����ѡ��matchvalue radio,matchvalue�ı�������Ի���Ӧ�仯
//| Input para.	:	
////| Ret value	:	
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkValue(id)
{
    if (id == "nullradio") {
      
        document.getElementById("matchvalue").disabled = true;
    } else {
        document.getElementById("matchvalue").disabled = false;
    }
     
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	checkMatch
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	ѡ��match checkBox
//| Input para.	:	
////| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function checkMatch()
{
    if (document.getElementById("matchcheck").checked &&   document.getElementById("seltype").value == "0") {
        document.getElementById("nullradio").disabled = false;
        document.getElementById("matchradio").disabled = false;
        document.getElementById("matchvalue").disabled = false;
        document.getElementById("reverse").disabled = false;
     } else {
        document.getElementById("nullradio").disabled = true;
        document.getElementById("matchradio").disabled = true;
        document.getElementById("matchvalue").disabled = true;
        document.getElementById("reverse").disabled = true;
     }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	locateTree
//| Author		:	Lucy Liu
//| Create Date	:	11/26/2008
//| Description	:	���ڵ㶨λ
//| Input para.	:	
////| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function locateTree()
{
    var index = new Array();
    if (!isLoadingPage) {
        for (i=0; i < printTemplateInfo.DatasettingList.length; i++) {
            if (dataObject.DataSetDep == printTemplateInfo.DatasettingList[i].DataSetName) {
			    index.push(i);
			    document.getElementById("seldataset").value = dataObject.DataSetDep;
			    for (j=0; j < printTemplateInfo.DatasettingList[i].Fields.length; j++) {
                    if (dataObject.OutputFiledDep == printTemplateInfo.DatasettingList[i].Fields[j].FieldName1) {
			            index.push(j);
			            document.getElementById("selfield").value = dataObject.OutputFiledDep;
			            document.getElementById("seltype").value = 0;
			            break;
                    }
			    }
            }
        }       
       
        tree.freshPath=index.reverse();
	    tree.locateNode("UUID");
	    clearInterval(interval);
	   
        
        
	} 	 
	     
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getFileName
//| Author		:	Lucy Liu
//| Create Date	:	2004-03-14
//| Description	:	get file name (not includnig extension name)
//| Input para.	:	file full name
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

function getFileExtention(fullname)
{
	
    var extention = "";
    var index = 0;
    var length = 0;
    if (fullname != null && fullname != "")
    {
        length = fullname.length;
		index = fullname.lastIndexOf(".");
		if(index != -1)
		{
		    extention = fullname.substring(index+1,length);

		}
			
			
    }

	return extention;

}

function preTest()
{
    
    
////    var ret = preCallback(pageIndex);
////    if (ret) {
////        var ret1 = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
////        if (ret1.error != null) {
////            alert(ret1.error.Message);
////        }
////    }
////    return ret;

	
 
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgeOutBound
//| Author		:	Lucy Liu
//| Create Date	:	9/23/2009
//| Description	:	�жϽ���Ԫ���Ƿ񳬳�ģ�巶Χ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//function judgeOutBound()
//{
// 

//    var errorFlag = false;
//  
//    var x = trimString(document.getElementById("x").value);
//    var y = trimString(document.getElementById("y").value);
//    var angle = document.getElementById("angle").value;
//    var widthPixel = document.getElementById("<%=picWidth.ClientID %>").value;
//    var heightPixel = document.getElementById("<%=picHeight.ClientID %>").value;
//     
//    if ((angle == "0") || (angle == "180")) { 
//     
//        if (parseFloat(x) + parseFloat(pixelToMm_x(widthPixel)) > parseFloat(convertTemplateWidth)) {
//            
//            if (belongSecCell) {
//               alert("<%=Resources.Template.picXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.picXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//           
//            errorFlag = true;
//        } else if (parseFloat(y)  + parseFloat(pixelToMm_y(heightPixel)) > parseFloat(convertAreaHeight)) {
//            
//            alert("<%=Resources.Template.picYIllegal%>" + "(Area Height:" + areaHeight + ")");
//            
//            errorFlag = true;
//        } 
//    } else {
//        if (parseFloat(x) + parseFloat(pixelToMm_x(heightPixel)) > parseFloat(convertTemplateWidth)) {
//            
//            if (belongSecCell) {
//               alert("<%=Resources.Template.picXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.picXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//            
//            errorFlag = true;
//        } else if (parseFloat(y)  + parseFloat(pixelToMm_y(widthPixel)) > parseFloat(convertAreaHeight)) {
//            
//            
//            alert("<%=Resources.Template.picYIllegal%>" + "(Area Height:" + areaHeight + ")");
//           
//            errorFlag = true;
//        } 
//        
//    }
//   
//    return errorFlag;
//}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	callParent
//| Author		:	Lucy Liu
//| Create Date	:	2004-03-14
//| Description	:	�ϴ���ͼƬ�������ؿ��Ƹ�ֵ
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function callParent(picId,picValue,picHeight,picWidth,error)
{

    if (error != "") {
        alert(error);
        div1ErrorFlag = true;
    } else {
        document.getElementById("<%=picId.ClientID %>").value = picId;
        document.getElementById("<%=picValue.ClientID %>").value = picValue;
        document.getElementById("<%=picHeight.ClientID %>").value = picHeight;
        document.getElementById("<%=picWidth.ClientID %>").value = picWidth;
        
        div1ErrorFlag = false;
   }
    if (!div1ErrorFlag) {
        //���ϴ�ͼƬҳ��û�з��ش�����
        //�ж�ͼƬλ�ã���С�Ƿ�Խ��������
        
        var x = trimString(document.getElementById("x").value);
        var y = trimString(document.getElementById("y").value);
        var angle = document.getElementById("angle").value;
        var widthPixel = document.getElementById("<%=picWidth.ClientID %>").value;
        var heightPixel = document.getElementById("<%=picHeight.ClientID %>").value;
        var judgeObject = new Object();
        judgeObject.Width = widthPixel;
        judgeObject.Height = heightPixel;
        judgeObject.X = x;
        judgeObject.Y = y;
        judgeObject.Angle = angle;
        
        
         var retObject = judgeOutBound(3,areaHeight,convertAreaHeight,templateWidth,convertTemplateWidth,belongSecCell,judgeObject,false)
       
         if (!retObject.errorFlag) {
            //����pictureObject
            saveDiv1();
            saveDiv2();
            saveDiv3();
           
            var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
            if (ret.error != null) {
                alert(ret.error.Message);
            } else {
                window.close();
                window.returnValue = "";
            }
         } else {
            alert(getPrefixMessage(retObject.prefixErr) + retObject.suffixErr);
            document.getElementById("<%=picId.ClientID %>").value = pictureObject.Uuid;
            document.getElementById("<%=picValue.ClientID %>").value = pictureObject.Value;
            document.getElementById("<%=picHeight.ClientID %>").value = pictureObject.HeightPixel;
            document.getElementById("<%=picWidth.ClientID %>").value = pictureObject.WidthPixel;
           
         }
   }
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div1����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv1()
{

    var errorFlag = false;
    var filePath = trimString(window.frames["action"].document.getElementById("FileUpload").value);
    var extention = getFileExtention(filePath).toLowerCase();
    var uuid = document.getElementById("<%=picId.ClientID %>").value;
    if (uuid == "") {
        //��һ���趨Picture
//        if (filePath == "") {
//            errorFlag = true;
//            alert("<%=Resources.Template.noChooseFile%>");  
//            window.frames["action"].document.getElementById("FileUpload").focus();
//        }
    }
    if (filePath != "") {
        if (!((extention == "jpg") || (extention == "gif") || (extention == "bmp") || (extention == "png") || (extention == "wmf"))) {
                 errorFlag = true;
            alert("<%=Resources.Template.errorFileType%>");  
            window.frames["action"].document.getElementById("FileUpload").focus();
                
        } else {
            var fso;
		    var f;
		    fso = new ActiveXObject("Scripting.FileSystemObject");
		    if (fso.FileExists(filePath)) {
		        f = fso.GetFile(filePath);  
                if (f.size > 3145728) {
                    errorFlag = true;
                    alert("<%=Resources.Template.fileLarge%>");  
                    window.frames["action"].document.getElementById("FileUpload").focus();
                        
                } else if (f.size == 0) {
                    errorFlag = true;
                    alert("<%=Resources.Template.fileEmpty%>");  
                    window.frames["action"].document.getElementById("FileUpload").focus();
                }
            } else {
                errorFlag = true;
                alert("<%=Resources.Template.fileEmpty%>");  
                window.frames["action"].document.getElementById("FileUpload").focus();
            }
        		
        }
     
      
    } 
//    if (uuid != "" && div1ErrorFlag) {
//        errorFlag = true;
//    }

  
    
    return errorFlag;
   
  
  

 
}



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	judgeFirstSave
//| Author		:	Lucy Liu
//| Create Date	:	12/28/2009
//| Description	:	�ж��ύʱ�Ƿ����ͼƬ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function judgePicturetExist()
{

    var errorFlag = false;
    var filePath = trimString(window.frames["action"].document.getElementById("FileUpload").value);
    var extention = getFileExtention(filePath).toLowerCase();
    var uuid = document.getElementById("<%=picId.ClientID %>").value;
    if (uuid == "") {
        //��һ���趨Picture
        if (filePath == "") {
            errorFlag = true;
            alert("<%=Resources.Template.noChooseFile%>");  
            
        }
    }
   

  
    
    return !errorFlag;
   
  
  

 
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv2
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	leaveDiv2����ʱ�����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv2()
{
 

    var errorFlag = false;
  
    var x = trimString(document.getElementById("x").value);
    var y = trimString(document.getElementById("y").value);
    var angle = document.getElementById("angle").value;

    //�ж�����Ϸ���
    if (!checkPosition(x)) {
        alert("<%=Resources.Template.positionFormat%>");
        document.getElementById("x").focus();
        errorFlag = true;
    } else if (!checkPosition(y)) {
        alert("<%=Resources.Template.positionFormat%>");
        document.getElementById("y").focus();
        errorFlag = true;
    } 
    
    return errorFlag;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv3
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div3����ʱ�����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv3()
{

    var errorFlag = false;
    var type = document.getElementById("seltype").value;
    var seldataset = document.getElementById("seldataset").value;
    var selfield = document.getElementById("selfield").value;
   
  
    var matchValue;
   
    if (document.getElementById("matchcheck").checked) {
        if (type == "0") {
            if (document.getElementById("matchradio").checked) {
                matchValue = trimString(document.getElementById("matchvalue").value);
                if (matchValue == "") {
                    alert("<%=Resources.Template.noInputMatchedValue%>");
                    document.getElementById("matchvalue").focus();
                    errorFlag = true;
                } 
            }
        } 
    }  
 
    return errorFlag;
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv3
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div3����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv3()
{
 

    var type = document.getElementById("seltype").value;
    var seldataset = document.getElementById("seldataset").value;
    var selfield = document.getElementById("selfield").value;
    var matchValue;
   
    if (document.getElementById("matchcheck").checked) {
        if (type == "0") {
            if (document.getElementById("matchradio").checked) {
                matchValue = trimString(document.getElementById("matchvalue").value);
            }
       
            if (document.getElementById("matchradio").checked) {
                dataObject.DependencedValue = matchValue;
                dataObject.NullValue = "";
               
            } else if (document.getElementById("nullradio").checked) {
                dataObject.NullValue = "1"; 
                dataObject.DependencedValue = "";
            } 
            dataObject.OutputFiledDep = selfield;
            dataObject.DataSetDep = seldataset;
            if (document.getElementById("reverse").checked) {
                dataObject.Reverse = "1";
            } else {
                dataObject.Reverse = "";
            }
           
           
        } else {
            dataObject.Reverse = "";
            dataObject.OutputFiledDep = "";
            dataObject.DataSetDep = "";
            dataObject.DependencedValue = "";
            dataObject.NullValue = "";

        }
    } else {
        dataObject.Reverse = "";
        dataObject.OutputFiledDep = "";
        dataObject.DataSetDep = "";
        dataObject.DependencedValue = "";
        dataObject.NullValue = "";
    }

}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div1����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv1()
{

    var picId = document.getElementById("<%=picId.ClientID %>").value;
    var picValue = document.getElementById("<%=picValue.ClientID %>").value;
    var picHeight = document.getElementById("<%=picHeight.ClientID %>").value;
    var picWidth = document.getElementById("<%=picWidth.ClientID %>").value;
    var objectName = document.getElementById("objectName").value;

   
   
    pictureObject.Uuid = picId;
    pictureObject.Value = picValue; 
    pictureObject.HeightPixel = picHeight; 
    pictureObject.WidthPixel = picWidth;
    pictureObject.RealObjectName = objectName;
  
    var ret = com.inventec.template.manager.TemplateManager.addPicSession(picId, picValue);
    if (ret.error != null) {
       alert(ret.error.Message);
   
    } 
      
 
}

 //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	saveDiv2
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Div2����
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function saveDiv2()
{

    var x = trimString(document.getElementById("x").value);
    var y = trimString(document.getElementById("y").value);
    var angle = document.getElementById("angle").value;

     pictureObject.X = x;
     pictureObject.Y = y;
     pictureObject.Angle = angle;
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	getPrefixMessage
//| Author		:	Lucy Liu
//| Create Date	:	9/23/2009
//| Description	:	����flag��ȡ������Ϣ
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function getPrefixMessage(prefixFlag)
{
    var message = "";
    switch(prefixFlag) {
        case 11:
            //text
            message = "<%=Resources.Template.textXIllegal%>";          
            break;
        case 12:
            //text
            message = "<%=Resources.Template.textYIllegal%>";          
            break;    
        case 21:
            //barcode
            message = "<%=Resources.Template.barcodeXIllegal%>"
            break;
        case 22:
            //barcode
            message = "<%=Resources.Template.barcodeYIllegal%>";         
            break;
        case 31:
            //picture
            message = "<%=Resources.Template.picXIllegal%>";          
            break;
        case 32:
            //picture
            message = "<%=Resources.Template.picYIllegal%>";         
            break;
        case 41:
            //line
            message = "<%=Resources.Template.lineXIllegal%>";
            break;
        case 42:
            //line
            message = "<%=Resources.Template.lineYIllegal%>";
            break;
        case 51:
            //area
            message = "<%=Resources.Template.areaXIllegal%>";          
            break;
        case 52:
            //area
            message = "<%=Resources.Template.areaYIllegal%>";     
            break;
        case 61:
            //rectangle
            message = "<%=Resources.Template.recXIllegal%>";
            break; 
        case 62:
            //rectangle
            message = "<%=Resources.Template.recYIllegal%>";
            break; 
          default:
            break;
     }
     return message;
    
}


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Finish
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�������ã��رմ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Save()
{

//    if (pageIndex == 0) {
//       
//        var iframe = document.getElementById("action");

//          window.frames["action"].callChild();
//          window.frames["action"].document.getElementById("ok").click();

//        
//    } else {
//        var ret = preCallback(pageIndex);
//        if (ret) {
//            var ret1 = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
//            if (ret1.error != null) {
//                alert(ret1.error.Message);
//            } else {
//                window.close();
//                window.returnValue = "";
//            }
//        }
//    }

   
    var ret = preCallback(pageIndex);
    var ret1 = judgePicturetExist();
    if (ret && ret1) {
         var filePath = trimString(window.frames["action"].document.getElementById("FileUpload").value);
         if (filePath != "") {
             var iframe = document.getElementById("action");
             window.frames["action"].callChild();
             window.frames["action"].document.getElementById("ok").click();
             
            
        } else {
              //�ж�ͼƬλ�ã���С�Ƿ�Խ��������
             var x = trimString(document.getElementById("x").value);
             var y = trimString(document.getElementById("y").value);
             var angle = document.getElementById("angle").value;
             var widthPixel = document.getElementById("<%=picWidth.ClientID %>").value;
             var heightPixel = document.getElementById("<%=picHeight.ClientID %>").value;
             var judgeObject = new Object();
             judgeObject.Width = widthPixel;
             judgeObject.Height = heightPixel;
             judgeObject.X = x;
             judgeObject.Y = y;
             judgeObject.Angle = angle;
        
        
            var retObject = judgeOutBound(3,areaHeight,convertAreaHeight,templateWidth,convertTemplateWidth,belongSecCell,judgeObject,false)
            if (!retObject.errorFlag) {
                //����pictureObject
                saveDiv1();
                saveDiv2();
                saveDiv3();
               
                var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
                if (ret.error != null) {
                    alert(ret.error.Message);
                } else {
                    window.close();
                    window.returnValue = "";
                }
            }else {
                alert(getPrefixMessage(retObject.prefixErr) + retObject.suffixErr);
                return;
            }
            
        } 
    }
           
   
}
	

//��ѯ���Ƿ�������
interval = setInterval("locateTree()", 2000);


//-->
</SCRIPT>
  






