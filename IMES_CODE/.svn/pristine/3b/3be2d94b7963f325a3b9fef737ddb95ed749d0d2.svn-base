<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: ģ���趨��ҳ��1
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 

<%@ page language="C#" autoeventwireup="true" inherits="webroot_axpx_template_TemplateSetting1, App_Web_templatesetting1.aspx.7a399c77" theme="MainTheme" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Configuration" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Template Setting-Step 1</title>
</head>
<%--load all js files--%>
<fis:header id="header1" runat="server"/>

<body onload="initPage()"  class="bgBody" >
    <form id="form1" runat="server">
    <div>
       
    </div>
    </form>
    
    <DIV class="wizardTip" >
        <span style="font-weight: bold"><%=Resources.Template.templateSetting%></span><br>
        <span style="margin-left:50px"><%=Resources.Template.templateSettingTitle1%></span><br><br>
    </DIV>
    <DIV class="wizardContent" >
    <TABLE  width="100%" border="0" cellpadding="5" cellspacing="5">
        <TR>
	        <TD style="width:30%" class="inputTitle"><%=Resources.Template.templateName%>:</TD>
	        <TD><INPUT TYPE="text" NAME="name" id="name" style="width:300px" maxlength="50"><font class="tipFont">*</font></TD>
        </TR>
        <TR>
            <TD style="width:30%" class="inputTitle"><%=Resources.Template.description%>:</TD>
	        <TD ><TEXTAREA NAME="descr" id="descr" ROWS="6" COLS="10" style="width:300px"></TEXTAREA><br>(<%=Resources.Template.commentLength%>)</TD>
        </TR>
         <TR>
            <TD style="width:30%" class="inputTitle"><%=Resources.Template.dpi%>:</TD>
	        <TD >
	             <SELECT NAME="dpi" id="dpi" style="width:300px"  >
	             <%--   <option value="<%=Constants.PRINTER_200_DPI%>"><%=Constants.PRINTER_200_DPI%></option>
	                <option value="<%=Constants.PRINTER_300_DPI%>"><%=Constants.PRINTER_300_DPI%></option>
	                <option value="<%=Constants.PRINTER_600_DPI%>"><%=Constants.PRINTER_600_DPI%></option>
	                	  --%>          
	            </SELECT>
	        </TD>
        </TR>
         <TR>
            <TD style="width:30%" class="inputTitle"><%=Resources.Template.templateType%>:</TD>
	        <TD >
	             <SELECT NAME="templateType" id="templateType" style="width:300px"  >
	                <option value="<%=Constants.BITMAP_TYPE%>"><%=Constants.BITMAP_TYPE%></option>
	                <option value="<%=Constants.METAFILE_TYPE%>"><%=Constants.METAFILE_TYPE%></option>
	               
	                	            
	            </SELECT>
	        </TD>
        </TR>
        
    </TABLE>
    </div>
     <input type="hidden" runat="server" id="hidDpi" />
</body>
<%--set style for readonly input controller--%>
<fis:footer id="footer1" runat="server"/> 


<SCRIPT LANGUAGE="JavaScript">
<!--
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	initPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	��ʼ����ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function initPage()
{

    var temp = document.getElementById("<%=hidDpi.ClientID%>").value;
    var array = temp.split(",");
    for(var j = 0; j < array.length; j++)
    {
       var objOption = document.createElement("OPTION");
       objOption.value = array[j];
       objOption.text = array[j];  
       dpi.add(objOption);
    }
    document.getElementById("name").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.FileName;
    document.getElementById("descr").value = window.parent.templateXmlAndHtml.PrintTemplateInfo.Comment;
    if (window.parent.templateXmlAndHtml.PrintTemplateInfo.PrinterDpi != "") {
        for (i = 0; i < document.getElementById("dpi").options.length; i++) {
            if (window.parent.templateXmlAndHtml.PrintTemplateInfo.PrinterDpi == document.getElementById("dpi").options[i].value)
            {
                document.getElementById("dpi").options[i].selected = true;
                break;
             }
        }
      
    }
    if (window.parent.templateXmlAndHtml.PrintTemplateInfo.OutputImageType != "") {
        for (i = 0; i < document.getElementById("templateType").options.length; i++) {
            if (window.parent.templateXmlAndHtml.PrintTemplateInfo.OutputImageType == document.getElementById("templateType").options[i].value)
            {
                document.getElementById("templateType").options[i].selected = true;
                break;
             }
        }
      
    }
    window.parent.setButtonStatus(0);
    
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	exitPage
//| Author		:	Lucy Liu
//| Create Date	:	4/30/2009
//| Description	:	�˳���ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function exitPage()
{
    var errorFlag = false;
    var name = trimString(document.getElementById("name").value);
    var comment = trimString(document.getElementById("descr").value);
    var dpi = document.getElementById("dpi").value
    var templateType = document.getElementById("templateType").value
   
        if (name == "") {
            errorFlag = true;
            alert("<%=Resources.Template.notInputTemplateName%>");
            document.getElementById("name").focus();
        } else if (comment.length > 200) {
            errorFlag = true;
            alert("<%=Resources.Template.descrLong%>");
            document.getElementById("descr").select();
           
        }
        if (!errorFlag) {
            var ret; 
            if (window.parent.treeId == "") {
                ret =  com.inventec.template.manager.TemplateManager.judgeDublicateTempalteExist(name);
            } else {
                ret =  com.inventec.template.manager.TemplateManager.judgeDublicateTempalteExist1(name, window.parent.treeId);
            }
            
            if (ret.error != null) {
                errorFlag = true;
                alert(ret.error.Message);
                document.getElementById("name").focus();
            }
        }
    
    if (!errorFlag) {
        window.parent.templateXmlAndHtml.PrintTemplateInfo.FileName = name;
        window.parent.templateXmlAndHtml.PrintTemplateInfo.Comment = comment;
        window.parent.templateXmlAndHtml.PrintTemplateInfo.PrinterDpi = dpi;
        window.parent.templateXmlAndHtml.PrintTemplateInfo.OutputImageType = templateType;
//        if (dpi == "<%=Constants.PRINTER_200_DPI%>") {
//            window.parent.templateXmlAndHtml.PrintTemplateInfo.BaseWidth = '<%=System.Configuration.ConfigurationManager.AppSettings.Get("200DpiPrinterBaseWidth").ToString()%>';
//        } else if (dpi == "<%=Constants.PRINTER_300_DPI%>") {
//           window.parent.templateXmlAndHtml.PrintTemplateInfo.BaseWidth = '<%=System.Configuration.ConfigurationManager.AppSettings.Get("300DpiPrinterBaseWidth").ToString()%>';
//        } else if (dpi == "<%=Constants.PRINTER_600_DPI%>") {
//           window.parent.templateXmlAndHtml.PrintTemplateInfo.BaseWidth = '<%=System.Configuration.ConfigurationManager.AppSettings.Get("600DpiPrinterBaseWidth").ToString()%>';
//        } 
        
        
    }
    return errorFlag;
}

//-->
</SCRIPT>
</html>