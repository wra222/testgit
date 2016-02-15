
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ��Ԥ��ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   liu xiaoling(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_Preview, App_Web_preview.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id=Head1 runat="server">
<title>
Preview 
</title>

</head>

<fis:header id="header1" runat="server"/>
<body class="dialogBody" onload="initPage()">
    <form id="form2" runat="server">
    <div>
    </div>
    </form>
    
    <fieldset id="fieldset1" style="padding-left:0px;padding-right:0px;margin-bottom:10px">
    <legend><%=Resources.Template.inputPara%>:</legend> 
        <div style="OVERFLOW-Y:auto;HEIGHT:100px;" id="paraDiv">
	    </div>
    </fieldset> 

    <table   width="100%" cellpadding="0px" cellspacing="5px" border="0"  >
        <tr><td></td></tr>
	    <tr>
	        <td align="right"><button id="ok" onclick="save()" style="width:100px" ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;&nbsp;<button id="cancel" onclick="closePage()" style="width:100px"><%=Resources.Template.cancelButton%></button></td>
        </tr>
    </table>
  
</body>    
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--
var printTemplateInfo = "";
var templateName = "";
var paramHTML = "";

function document.onkeydown() {
    //�ж��Ƿ���Enter��
    if (event.keyCode == 13) {
        save();
    }
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
    printTemplateInfo = window.dialogArguments;
    templateName = printTemplateInfo.FileName;
    paramHTML = "<table width='100%'  cellpadding='0px' cellspacing='0px' border='0'>";
    for (i = 0; i < printTemplateInfo.InputParas.length; i++) {
        paramHTML += "<tr>"
        paramHTML += "<td width='20%'align='right'>" + printTemplateInfo.InputParas[i].ParaName + ":</td>";
        paramHTML += "<td><INPUT TYPE='text' id='" + printTemplateInfo.InputParas[i].ParaName + "' style='width:200px'>" +  printTemplateInfo.InputParas[i].ParaType + "</td>";
       // paramHTML += "<td align='left'>" + printTemplateInfo.InputParas[i].ParaType + "</td>";
        paramHTML += "</tr>"
    }
    paramHTML += "</table>"
    document.getElementById("paraDiv").innerHTML = paramHTML;
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
    var paramRtn = "";
    var errorFlag = false;
    var value = "";
    var paramName = "";
    var x;
    var y;
    var printer;
    var piece;
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
        
         paramArray.toJSON = function(){return toJSON(this);};
         printTemplateInfo.toJSON = function(){return toJSON(this);};
         var rtn = webroot_aspx_template_Preview.getImage(paramArray, printTemplateInfo);
         if (rtn.error!=null) {
            alert(rtn.error.Message);
         } else {
            var strRtn = rtn.value;
            
            var top=(screen.height-738)/3;
            var left=(screen.width-1014)/2;

            if(screen.width<1034){
                top=0;
                left=0;
            }

            strRtn = strRtn + "&" + printTemplateInfo.pixel_per_inch_y + "&" + printTemplateInfo.pixel_per_inch_x;
            window.returnValue = strRtn;
            /*
           	ShowWait();
            var wHandle = window.showModalDialog("Preview_child.aspx",strRtn,"dialogWidth:650px;dialogHeight:550px;scroll:off;center:yes;status:no;help:no");
            //var wHandle = window.open("Preview_child.aspx?uuid="+imgList,"newwindow",sFeature);
            HideWait();*/
            // printWB.print(printer, rtn.value, rtn.value.length, piece, x, y);
            window.close();;
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