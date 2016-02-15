
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���ӡҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' 2009-09-01   Lucy Liu(EB2)  modify:ITC-1200-0009;ITC-1200-0010
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_Print, App_Web_print.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
<title>
Print 
</title>

</head>

<object classid="clsid:24BB7CDB-562E-4D60-8A56-4DD1DD77BE48" codebase="../../activex/print.cab#version=<%= Constants.version%>"
        id="printWB" style="display:none"></object>
        
<fis:header id="header1" runat="server"/>
<body class="dialogBody" onload="initPage()">
    <form id="form2" runat="server">
    <div>
    </div>
    </form>
    
    <fieldset id="fieldset1" style="padding-left:0px;padding-right:0px;margin-bottom:10px">
    <legend><%=Resources.Template.inputPara%>:</legend> 
        <div style="OVERFLOW-Y:auto;HEIGHT:55px;" id="paraDiv">
	    </div>
    </fieldset> 
    <br>
    
    <fieldset id="fieldset2" style="padding-left:0px;padding-right:0px;margin-bottom:10px">
    <legend><%=Resources.Template.inputOffset%>:</legend> 
        <div style="OVERFLOW-Y:auto;HEIGHT:55px;">
		    <table cellpadding="0px" cellspacing="0px"   border="0" width="100%">
			    <tr>
				    <td width="35%" align="right"><%=Resources.Template.x%>:&nbsp;</td>
					<td><INPUT TYPE="text" NAME="x" id="Text1" style="width:150px">mm</td>
				    <%--	<td align="left" >mm</td>--%>
				</tr>
				<tr>
				    <td width="35%" align="right"><%=Resources.Template.y%>:&nbsp;</td>
					<td ><INPUT TYPE="text" NAME="y" id="Text2"  style="width:150px">mm</td>
					    <%--<td align="left">mm</td>--%>
				</tr>
    				
			</table>
	    </div>
    </fieldset>
    <br>
    
    <fieldset id="fieldset3" style="padding-left:0px;padding-right:0px;margin-bottom:1px">
	<legend><%=Resources.Template.printSetting%>:</legend> 
        <div style="OVERFLOW-Y:auto;HEIGHT:110px;">
        <table   width="100%"  cellpadding="0px" cellspacing="0px" >
        <tr>
	        <td width="28%" align="right"><%=Resources.Template.printer%>:&nbsp;</td>
		    <td ><SELECT NAME="printer" id="printer"  style="width:230px"></SELECT></td>
		
        </tr>
	    <tr>
	        <td align="right"><%=Resources.Template.piece%>:&nbsp;</td>
		    <td ><INPUT TYPE="text" NAME="piece" id="piece" style="width:230px" value="1"></td>
		
        </tr>
        <tr>
            <td align="right"><%=Resources.Template.printFormat%>:&nbsp;</td>
            <td ><INPUT TYPE="radio" NAME="printFormat" id="frontalPrint" checked><%=Resources.Template.frontalFormat%>&nbsp;&nbsp;&nbsp;<INPUT TYPE="radio" NAME="printFormat" id="reversedPrint" ><%=Resources.Template.reversedFormat%></td>
	     </tr>
	     
	     <tr>
            <td align="right"><%=Resources.Template.printPaperLayout%>:&nbsp;</td>
            <td > 
                <SELECT NAME="printPaperLayout" id="printPaperLayout" class="inputStyle" style="width:230px">
                    <option value="1">Portrait</option>
	                <option value="2">Landscape</option>	                
	            </SELECT></td>
	     </tr>
               
        </table>
    </div>
    </fieldset>
    <br>
    
    <table   width="100%"  cellpadding="0px" cellspacing="0px" border="0"  >
	<tr >
	    <td align="right" colspan="2" ><button id="ok" onclick="save()"  ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;&nbsp;<button id="cancel" onclick="closePage()" ><%=Resources.Template.cancelButton%></button></td>
    </tr>
    </table>
   

  
</body>    
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--
var printTemplateInfo = "";
var templateName = "";
var currentOcxVersion;
var supportVersion;
var paramHTML = "";


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	fillField
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ȡ���й����ӡ���б����䵽��������
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function fillField(){

    var objSel = document.getElementById("printer");
    objSel.innerHTML = "";
    
    var prints = printWB.getPrintList();
    //alert(prints);
    var fieldInfos = prints.split(";");
    
    var defaultPrinter = printWB.GetDefaultPrinter();
    
    var index = 0;
    for (var i = 0; i < fieldInfos.length; i++){
        var fieldInfo = fieldInfos[i];
        var option = document.createElement("option");
        option.value = fieldInfo;
        option.text =  fieldInfo;              
        objSel.add(option); 
        if (fieldInfo == defaultPrinter) {
            index = i;
        }
   }
   objSel.selectedIndex = index;
}
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
    
    fillField();
    
    printTemplateInfo = window.dialogArguments;
    templateName = printTemplateInfo.FileName;
    paramHTML = "<table width='100%'  cellpadding='0px' cellspacing='0px' border='0'>";
    for (i = 0; i < printTemplateInfo.InputParas.length; i++) {
        paramHTML += "<tr>"
        paramHTML += "<td width='35%'align='right'>" + printTemplateInfo.InputParas[i].ParaName + ":&nbsp;</td>";
        paramHTML += "<td><INPUT TYPE='text' id='" + printTemplateInfo.InputParas[i].ParaName + "' style='width:150px'>" +  printTemplateInfo.InputParas[i].ParaType + "</td>";
       // paramHTML += "<td align='left'>" + printTemplateInfo.InputParas[i].ParaType + "</td>";
        paramHTML += "</tr>"
    }
    paramHTML += "</table>"
   document.getElementById("paraDiv").innerHTML = paramHTML;
   
   
    if (printTemplateInfo.OutputImageType != "<%=Constants.BITMAP_TYPE%>") {
        document.getElementById("reversedPrint").disabled = true;
    }
    
    //bug no:ITC-992-0064
    //reason:���ܽ���
    document.getElementById(printTemplateInfo.InputParas[0].ParaName).focus();
    
   
    
   
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
   
    try {
    
        var paramRtn = "";
        var errorFlag = false;
        var value = "";
        var paramName = "";
        var x;
        var y;
        var printer;
        var piece;
        //             <bug>
        //                BUG NO:ITC-992-0091
        //                REASON:��ղ���
        //            </bug>
        
          
          var ret = webroot_aspx_template_Print.getSupportVersion();
          HideWait();     
          if (ret.error!=null) {
            alert(ret.error.Message);
          } else {
            supportVersion = ret.value;        
         }
        //             <bug>
        //                BUG NO:ITC-1200-0009
        //                REASON:�Ƚϰ汾
        //            </bug>
         currentOcxVersion = printWB.getVersion()
         if (supportVersion.indexOf(currentOcxVersion) == -1) {
            alert("<%=Resources.Template.ocxVersionError%>");
            return;
         }
         
        var paramArray = new Array();
        for (i = 0; i < printTemplateInfo.InputParas.length; i++) {
            paramRtn = com.inventec.template.manager.TemplateManager.getParamInfoFromStructure();
            if (paramRtn.error != null) {
                errorFlag = true;
                alert(paramRtn.error.Message);
                break;
            } else {
                paramName = printTemplateInfo.InputParas[i].ParaName;
                value = trimString(document.getElementById(paramName).value);
                if (value == "") {
                    errorFlag = true;
                    document.getElementById(paramName).focus();
                    alert("<%=Resources.Template.paramValue%>" + printTemplateInfo.InputParas[i].ParaName + "!");
                    break;
                } else {
                    paramRtn.value.ParamName = printTemplateInfo.InputParas[i].ParaName;
                    paramRtn.value.Values.push(value);
                    paramArray.push(paramRtn.value);
                }
            }
        } 
        if (!errorFlag) {
             x = trimString(document.getElementById("x").value);
             if (!checkPosition(x)) {
                 alert("<%=Resources.Template.positionFormat%>");
                 document.getElementById("x").focus();
                 errorFlag = true;
             }
         } 
        if (!errorFlag) {
             y = trimString(document.getElementById("y").value);
             if (!checkPosition(x)) {
                 alert("<%=Resources.Template.positionFormat%>");
                 document.getElementById("y").focus();
                 errorFlag = true;
             }
         } 
         if (!errorFlag) {
             printer = trimString(document.getElementById("printer").value);
             if (printer == "") {
                 alert("<%=Resources.Template.noInputPrinter%>");
                 document.getElementById("printer").focus();
                 errorFlag = true;
             }
         } 
          if (!errorFlag) {
             piece = trimString(document.getElementById("piece").value);
             if (piece == "") {
                 alert("<%=Resources.Template.noInputPiece%>");
                 document.getElementById("piece").focus();
                 errorFlag = true;
             } else if (!checkDigits(piece)) {
                 alert("<%=Resources.Template.digitFormat%>");
                 document.getElementById("piece").focus();
                 errorFlag = true;
             }
             
         } 
          var printPaperLayout = document.getElementById("printPaperLayout").value;
          
        if (!errorFlag) {
            

             paramArray.toJSON = function(){return toJSON(this);};
              ShowWait();
             var rtn = webroot_aspx_template_Print.getImage(paramArray, templateName);
              HideWait();
             var obj = rtn.value;
             if (rtn.error!=null) {
                alert(rtn.error.Message);
             } else {
                  var list = new Array(); 
                  for(var i = 0; i < obj.length; i++)
                  {
                     list[i] = obj[i].ImageString;
                  }
    //             <bug>
    //                BUG NO:ITC-992-0063
    //                REASON:offset*10
    //            </bug>
               
                var format;
                 if (document.getElementById("reversedPrint").disabled) {
                    format = false;
                 } else {
                    if (document.getElementById("frontalPrint").checked) {
                        format = false;
                    } else {
                        format = true;
                    }
                 }
               
                 printWB.printWithRotateAndPaperLayout(printer, list , list.length, piece, x*10, y*10,obj[0].ImageWidthPX,obj[0].ImageHeightPX,obj[0].ImagePixPerM, format, printPaperLayout);
              
                 window.close();;
             }
         }    
    } catch(e) {
        //             <bug>
        //                BUG NO:ITC-1200-0010
        //                REASON:�Ƚϰ汾
        //            </bug>
      
          alert("<%=Resources.Template.ocxVersionError%>");
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