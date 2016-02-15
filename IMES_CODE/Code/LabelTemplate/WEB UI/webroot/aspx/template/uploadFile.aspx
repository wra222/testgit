<%@ page language="C#" autoeventwireup="true" inherits="webroot_aspx_template_uploadFile, App_Web_uploadfile.aspx.7a399c77" theme="MainTheme" %>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <base target="_self">  
</head>
<body class="bgBody" >
    <form id="form1" runat="server">
    <div>
    <TABLE width="100%"  border="0" cellpadding="5px" cellspacing="5px" >
        <TR>
	        <TD style="width:15%"><%=Resources.Template.fileName%>:</TD>
	        <TD align="left">
	           
	           <asp:FileUpload id="FileUpload" name="FileUpload" style="width:300px;" runat="server"/>
	          
	        </TD>
        </TR>
        <TR>
	        <TD colspan="2"><font class="tipFont1"><%=Resources.Template.uploadFileType%></font ></TD>
	        
        </TR>
         </TABLE>
    </div>
        
        <asp:Button ID="ok"  name="ok" runat="server" Text="Button"  OnClick="btnSubmit_Click"  style="display:none"/>
          <asp:HiddenField ID="picId" runat="server"   />
    <asp:HiddenField ID="picValue" runat="server" />
    <asp:HiddenField ID="picHeight" runat="server"   />
    <asp:HiddenField ID="picWidth" runat="server" />
        <asp:HiddenField ID="submitFlag" runat="server"   />
         <asp:HiddenField ID="error" runat="server"   />
    </form>
</body>
</html>

<SCRIPT LANGUAGE="JavaScript">
<!--
function callChild()
{
    
    document.getElementById("<%=picId.ClientID %>").value = window.parent.document.getElementById("picId").value;
    document.getElementById("<%=picValue.ClientID %>").value = window.parent.document.getElementById("picValue").value;
    document.getElementById("<%=picHeight.ClientID %>").value = window.parent.document.getElementById("picHeight").value;
    document.getElementById("<%=picWidth.ClientID %>").value = window.parent.document.getElementById("picWidth").value;
    document.getElementById("<%=error.ClientID %>").value = "";
   
}

function submitInitPage()
{

    window.parent.callParent(document.getElementById("<%=picId.ClientID %>").value, document.getElementById("<%=picValue.ClientID %>").value, document.getElementById("<%=picHeight.ClientID %>").value, document.getElementById("<%=picWidth.ClientID %>").value,document.getElementById("<%=error.ClientID %>").value);

   
}
//-->
</SCRIPT>