<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: Line��������ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 

<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_LineProperty, App_Web_lineproperty.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
    <title>Line Property</title>
  
<script type="text/javascript" src="../../commoncontrol/btnTabs.js"></script>
<script type="text/javascript" src="../../commoncontrol/function/commonFunction.js"></script>


</head>
<%--<fis:header id="header1" runat="server"/>--%>


<body class="bgBody" >
  
   <form id="form1" runat="server"  >
     <div>
     </div>
   </form>
    
    <div class="propertyDialog"> 
    <div id="con" ></div>
   <div id="div1" style="display:none" class="propertyDialogContent">
		<TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px" >
		<TR>
	        <TD style="width:15%"><%=Resources.Template.objectName%>:</TD>
	        <TD>
	      
	            <input type="text" id="objectName" class="inputStyle" maxlength="20">
	        </TD>
        </TR>
        <TR>
            
	        <TD style="width:20%" ><%=Resources.Template.thickness%>:</TD>
	        <TD align="left">
	           <input type="text" NAME="thickness" id="thickness" class="inputStyle">mm</input>
	        </TD>
        </TR>
        <TR>
	        <TD style="width:20%"><%=Resources.Template.length%>:</TD>
	        <TD align="left">
	           <input type="text" NAME="length" id="length" class="inputStyle" >mm</input>
	        </TD>
        </TR>
        </TABLE>
      
    </div>    
   
    
    
    <div id="div2" style="display:;"   class="propertyDialogContent" >
         <FIELDSET >
	        <LEGEND ><%=Resources.Template.position%></LEGEND>
	            <TABLE width="100%"  border="0" cellpadding="5" cellspacing="5">
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.x%>:</TD>
	                <TD>
	                    <input type="text" NAME="x" id="Text1" class="inputStyle"></input>mm
	                </TD>
                    </TR>
                    <TR>
	                <TD style="width:20%"><%=Resources.Template.y%>:</TD>
	                <TD>
	                    <input type="text" NAME="y" id="Text2" class="inputStyle"></input>mm
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
	    
	   
	
    
     <div   class="propertyDialogButton">
		<button id="save" type="button"  onclick="Save()"  ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;
        <button id="cancel" type="button"  onclick="Cancel()"  ><%=Resources.Template.cancelButton%></button> 
	 </div>
	 </div>

</body>


</html>

<SCRIPT LANGUAGE="JavaScript">
<!--

var templateXmlandHtml;
var printTemplateInfo = "";
var lineObject = "";
var areaHeight = 0;
var templateWidth = 0;
var pageIndex = 1;
var belongSecCell = false;
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
		 
	for (var i=0; i<2; i++)
	{
	    var temp = new clsButton("tabs");
	    if (i == 0) {
	        temp.normalPic = "../../images/line" +"-1.jpg";
		    temp.selPic = "../../images/line" +"-2.jpg";
		    temp.disablePic = "../../images/line" +"-3.jpg";
	    } else {
		    temp.normalPic = "../../images/fields2" +"-1.jpg";
		    temp.selPic = "../../images/fields2" +"-2.jpg";
		    temp.disablePic = "../../images/fields2" +"-3.jpg";
		}	
		tabs.addButton(temp);
		
	}
		
	//tabs.diableTab(0,true);
	con.innerHTML = tabs.toString();
	tabs.initSelect(1);

}

//��������
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
		 	
		    break;
	    case 1: 
            
		    document.getElementById("div1").style.display = "none";
		 	document.getElementById("div2").style.display = "";
		    break;
       
         
        default:
            break;	
     }
         
     return true;
}
	
	



//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv1
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div1����ʱ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv1()
{
//  <bug>
//    BUG NO:ITC-992-0033  
//    REASON:�ж��Ƿ񳬳���������ʱ��ȥ��=���жϣ�ֻ�ж�>
//  </bug>
  
   var errorFlag = false;
   
    var thickness = trimString(document.getElementById("thickness").value);
    var length = trimString(document.getElementById("length").value);
  
    //�ж�����Ϸ���
    if (!check1DecimalNotZero(thickness)) {
        
        alert("<%=Resources.Template.oneDecimalFormat%>");
        document.getElementById("thickness").focus();
        errorFlag = true;
    }  else if (parseFloat(thickness) < parseFloat("<%=Constants.LINE_MINIMAL_THICKNESS%>")){
        
        alert("<%=Resources.Template.lineThicknessMinimalError%>" + "(Line Minimal Thickness:" + "<%=Constants.LINE_THICKNESS%>" + ")");
        document.getElementById("thickness").focus();
        errorFlag = true;
    } else if (!check1DecimalNotZero(length)) {
        alert("<%=Resources.Template.oneDecimalFormat%>");
        document.getElementById("length").focus();
        errorFlag = true;
    } 
    
   
    return errorFlag; 
 
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
//    var x = trimString(document.getElementById("x").value);
//    var y = trimString(document.getElementById("y").value);
//    var angle = document.getElementById("angle").value;
//    var thickness = trimString(document.getElementById("thickness").value);
//    var length = trimString(document.getElementById("length").value);
//    
//   if ((angle == "0") || (angle == "180")) { 
//        if (parseFloat(x) + parseFloat(length) > parseFloat(convertTemplateWidth)) {
//            
//            if (belongSecCell) {
//               alert("<%=Resources.Template.lineXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.lineXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//            errorFlag = true;
//        } else if (parseFloat(y) + parseFloat(thickness) > parseFloat(convertAreaHeight)) {
//            
//            alert("<%=Resources.Template.lineYIllegal%>" + "(Area Height:" + areaHeight + ")");
//            errorFlag = true;
//        } 
//   } else {
//        if (parseFloat(x) + parseFloat(thickness) > parseFloat(convertTemplateWidth)) {
//            
//            if (belongSecCell) {
//               alert("<%=Resources.Template.lineXIllegal%>" + "(Section Cell Width:" + convertTemplateWidth + ")");
//            } else {
//                alert("<%=Resources.Template.lineXIllegal%>" + "(Template Width:" + templateWidth + ")")
//            }
//          
//            errorFlag = true;
//        } else if (parseFloat(y) + parseFloat(length) > parseFloat(convertAreaHeight)) {
//            
//            alert("<%=Resources.Template.lineYIllegal%>" + "(Area Height:" + areaHeight + ")");
//            errorFlag = true;
//        } 
//        
//   } 
//   return  errorFlag;
//}
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
//  <bug>
//    BUG NO:ITC-992-0033  
//    REASON:�ж��Ƿ񳬳���������ʱ��ȥ��=���жϣ�ֻ�ж�>
//  </bug>
 
   
    var thickness = trimString(document.getElementById("thickness").value);
    var length = trimString(document.getElementById("length").value);
    var objectName = document.getElementById("objectName").value;
    lineObject.Thickness = thickness;
    lineObject.Length = length;
    lineObject.RealObjectName = objectName;
        
   
 
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	leaveDiv2
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�뿪Div2����ʱ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function leaveDiv2()
{
 
    
    var errorFlag = false;
    var x = trimString(document.getElementById("x").value);
    var y = trimString(document.getElementById("y").value);
  
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
    //����
//    if (!errorFlag) {
//        lineObject.X = x;
//        lineObject.Y = y;
//        lineObject.Angle = angle;
//    }
    return errorFlag;
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

    lineObject.X = x;
    lineObject.Y = y;
    lineObject.Angle = angle;
    
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
//    templateWidth = printTemplateInfo.TemplateWidth
    pixel_per_inch_x = window.dialogArguments.pixel_per_inch_x;
    pixel_per_inch_y = window.dialogArguments.pixel_per_inch_y;
    //��ȡҪ�༭��barcode����
    var ret = searchObject(window.dialogArguments.id);
    lineObject = ret.object;
    areaHeight =  ret.areaHeight;
    templateWidth = ret.templateWidth;
    convertAreaHeight =  ret.convertAreaHeight;
    convertTemplateWidth =  ret.convertTemplateWidth;
    belongSecCell =  ret.belongSecCell;
 
    
    
    //Tab1ҳ��
    document.getElementById("objectName").value = lineObject.RealObjectName;
    document.getElementById("thickness").value = lineObject.Thickness;
    document.getElementById("length").value = lineObject.Length;
    
    //Tab2ҳ��
    if (lineObject.X == "") {
        document.getElementById("x").value = "0";
    } else {
        document.getElementById("x").value = lineObject.X;
    }
     if (lineObject.Y == "") {
        document.getElementById("y").value = "0";
    } else {
        document.getElementById("y").value = lineObject.Y;
    }
   
    if (lineObject.Angle != "") {
        for (i = 0; i < document.getElementById("angle").options.length; i++) {
            if (lineObject.Angle == document.getElementById("angle").options[i].value)
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
//����
initPage();





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
//| Name		:	Finish
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�������ã��رմ���
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Save()
{
    
    var ret = preCallback(pageIndex);
    if (ret) {
        var x = trimString(document.getElementById("x").value);
        var y = trimString(document.getElementById("y").value);
        var angle = document.getElementById("angle").value;
        var thickness = trimString(document.getElementById("thickness").value);
        var length = trimString(document.getElementById("length").value);
        var judgeObject = new Object();
        judgeObject.Thickness = thickness;
        judgeObject.Length = length;
        judgeObject.X = x;
        judgeObject.Y = y;
        judgeObject.Angle = angle;
        
        
        var retObject = judgeOutBound(4,areaHeight,convertAreaHeight,templateWidth,convertTemplateWidth,belongSecCell,judgeObject,false)
        if (!retObject.errorFlag) {
            saveDiv1();
            saveDiv2();
           
        } else {
             alert(getPrefixMessage(retObject.prefixErr) + retObject.suffixErr);
            return;
        }
        templateXmlandHtml.toJSON = function(){return toJSON(this);};
	    var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
	    if (ret.error != null) {
            alert(ret.error.Message);
        } else{
               
            window.returnValue = "";
            window.close();  
        }
        
   } 
}
	




//-->
</SCRIPT>
  







