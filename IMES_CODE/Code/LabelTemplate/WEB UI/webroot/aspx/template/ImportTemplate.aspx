<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: �ӿͻ��˵���ģ���ļ�
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   Lucy Liu(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_ImportTemplate, App_Web_importtemplate.aspx.7a399c77" theme="MainTheme" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Import Template</title>
</head>
<fis:header id="header2" runat="server"/>
<script type="text/javascript" language="javascript">
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	uploadCallBack
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function uploadCallBack(uuid)
{

//    var rtn = com.inventec.template.manager.TemplateManager.getUploadStructure(uuid);
//    if (rtn.error != null) {
//        alert(rtn.error.Message);
//    } else {
//        var templateXmlandHtml = rtn.value;;
//	    templateXmlandHtml.toJSON = function(){return toJSON(this);};
//	    var ret = com.inventec.template.manager.TemplateManager.saveSession(templateXmlandHtml);
//	    if (ret.error != null) {
//            alert(ret.error.Message);
//        } else{
            window.returnValue = "";
            window.close();

//        }

//    }
}
</script>

<body class="dialogBody">
    
    <form id="form1" runat="server"  target="frame1">
    <div>
    </div>
     <table width="100%" cellpadding="0px" cellspacing="5px" border="0" height="30%">
    <tr>
	    <td class="inputTitle"><%=Resources.Template.selectFile%>:</td>
	    
    </tr>  
    <tr>
	    <td ><asp:FileUpload id="FileUpload" name="FileUpload" style="width:100%;" runat="server"/></td>
	    
    </tr> 
     </table>
     <table width="100%" cellpadding="0px" cellspacing="5px" border="0" height="70%" >
     <tr valign="bottom">
	    <td align="right" ><button id="b3" onServerClick="btnSubmit_Click" type="button"  runat="server" ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;<button id="Button3" type="button"  onclick="Cancel()" ><%=Resources.Template.cancelButton%></button></td>
	    
    </tr> 
      </table>
    </form>
    
     
   <IFRAME   name="frame1"   height="0"   width="0"></IFRAME>   

</body>
<fis:footer id="footer1" runat="server"/> 

<script type="text/javascript" language="javascript">
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//| Name		:	Cancel
//| Author		:	Lucy Liu
//| Create Date	:	4/24/2009
//| Description	:	��ʼ��ҳ��
//| Input para.	:	
//| Ret value	:	
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function Cancel()
{
   window.close();
}
</script>
</html>



