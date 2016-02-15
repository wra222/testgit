
<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���趨�򵼿��ҳ��
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_TemplateSetting, App_Web_templatesetting.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Template Setting</title>
</head>


<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body class="bgBody"  onload="initPage()" >
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
  
    <TABLE width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" >
    <tr height="90%">
        <td>
             <iframe id="iframe" src="" height="100%" width="100%"  scrolling="no"  frameborder="0"    >
             </iframe>
        </td>
    </tr>
     <tr height="10%">
        <td valign="top" align="right">
            <hr/>
            <button id="previousBtn" type="button" style="width:100px" onclick="Previous()" disabled><%=Resources.Template.previousButton%> </button>&nbsp;&nbsp;
            <button id="nextBtn" type="button" style="width:100px" onclick="Next()"   disabled><%=Resources.Template.nextButton%></button>&nbsp;&nbsp;&nbsp;
            <button id="finishBtn" type="button" style="width:100px" onclick="Finish()" disabled ><%=Resources.Template.finishButton%></button>&nbsp;&nbsp;
            <button id="cancelBtn" type="button" style="width:100px" onclick="Cancel()" disabled><%=Resources.Template.cancelButton%></button>
            
        </td>
     </tr>
    </TABLE>
</body>

<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 

<SCRIPT LANGUAGE="JavaScript">
<!--
var frameSrcArr = new Array();
frameSrcArr[0] = "TemplateSetting1.aspx";
frameSrcArr[1] = "TemplateSetting2.aspx";
frameSrcArr[2] = "TemplateSetting3.aspx";
frameSrcArr[3] = "TemplateSetting4.aspx";

var printTemplateInfo;
var templateXmlAndHtml;
var treeId = "";
var pixel_per_inch_x;
var pixel_per_inch_y;
//��ҳ�Ƿ񴥷���Previous��ť
var framePreFlag = new Array();
framePreFlag[0] = false;
framePreFlag[1] = false;
framePreFlag[2] = false;
framePreFlag[3] = false;

//frame3ҳ�汣��ͻ�����������
var width;
var height;
var headerheight;
var footerheight;
var detail1height;
var detail2height;
var printpagefooter;
var infoheight;

//frame4ҳ�汣��ͻ�����������
var detail1headerheight;
var detail1rowheight;
var detail1reset;
var detail1datasetnum;
var detail1columnnum;
var detail1fixedheight;
var detail1initdataset;
var detail1initcolumn;

var detail2headerheight;
var detail2rowheight;
var detail2reset;
var detail2datasetnum;
var detail2columnnum;
var detail2fixedheight;
var detail2initdataset;
var detail2initcolumn;


//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	setButtonStatus
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Ŀǰ���ĸ���ҳ��ȷ����ǰ��ť��״̬��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function setButtonStatus(currentFrameIndex)
{
  
   switch(currentFrameIndex ) {
    case 0:
        document.getElementById("finishBtn").disabled = true;
        document.getElementById("previousBtn").disabled = true;
        document.getElementById("nextBtn").disabled = false;
        break;
    case 1:
    case 2:
        
        document.getElementById("finishBtn").disabled = true;
        document.getElementById("previousBtn").disabled = false;
        document.getElementById("nextBtn").disabled = false;
        break;
    case 3:
        document.getElementById("finishBtn").disabled = false;
        document.getElementById("previousBtn").disabled = false;
        document.getElementById("nextBtn").disabled = true;
        break;
    default:
        break;
   }
   document.getElementById("cancelBtn").disabled = false;
 
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	disableButton
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	����Ŀǰ���ĸ���ҳ��ȷ����ǰ��ť��״̬��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function disableButton()
{
  
  
        document.getElementById("finishBtn").disabled = true;
        document.getElementById("previousBtn").disabled = true;
        document.getElementById("nextBtn").disabled = true;
        document.getElementById("cancelBtn").disabled = true;
    
   
 
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Previous
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	������˰�ť
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Previous()
{
    var i = 0;
    var ret = iframe.exitPrePage();
    
    {
        
        for (i = 0; i < frameSrcArr.length; i++) {
            if (document.getElementById("iframe").src == frameSrcArr[i]) {
                framePreFlag[i] = true;
                break;
            }
        }
        
      
        if (i > 0) {
            document.getElementById("iframe").src = frameSrcArr[i-1];
        }
        //setButtonStatus(i-1);
        disableButton();
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Cancel
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	������˰�ť
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
//| Description	:	������˰�ť
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Finish()
{

    var ret = iframe.exitPage();
    if (!ret) {
       templateXmlAndHtml.toJSON = function(){return toJSON(this);};
	    var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlAndHtml);
	    if (ret.error != null) {
            alert(ret.error.Message);
        } else{
            
        }

        window.returnValue = "";
        window.close();
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Next
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	������˰�ť
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Next()
{
    var i = 0;
    var ret = iframe.exitPage();
    if (!ret) {
        for (i = 0; i < frameSrcArr.length; i++) {
            if (document.getElementById("iframe").src == frameSrcArr[i]) {
                
                break;
            }
        }

        if  (i < frameSrcArr.length) {
            document.getElementById("iframe").src = frameSrcArr[i+1];
            
        }
        //setButtonStatus(i+1);
        disableButton();
    }
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


    var ret = window.dialogArguments;
    if (ret.treeId == "") {
        //�����������½���ť�����������Ǵӱ༭������Template Setting��ť����
//        templateXmlAndHtml =window.dialogArguments.templateXmlandHtml;
     
    } else {
   
//        templateXmlAndHtml = eval('(' + ret.templateXmlandHtml   + ')');
        treeId = ret.treeId;
      
    } 
    pixel_per_inch_x = ret.pixel_per_inch_x;
    pixel_per_inch_y = ret.pixel_per_inch_y;  
    var ret = com.inventec.template.manager.TemplateManager.getImportStructure("<%=Constants.IMPORT_SESSION%>");
    if (ret.error != null) {
         
        alert(ret.error.Message);
    } else{
        templateXmlAndHtml = ret.value;
           
           
    }     
   
    document.getElementById("iframe").src = frameSrcArr[0];
    
}
//-->
</SCRIPT>
</html>