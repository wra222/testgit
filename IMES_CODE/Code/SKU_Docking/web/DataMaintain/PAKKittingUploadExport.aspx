﻿<%@ Page Language="C#" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="PAKKittingUploadExport.aspx.cs" Inherits="DataMaintain_PAKKittingUploadExport"  ValidateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript">   
<!--
   
    function dealExport(currentLine,uploadGetData)
    {
        document.getElementById("<%=dLine.ClientID %>").value=currentLine;
        document.getElementById("<%=dDataExport.ClientID %>").value=uploadGetData;
        document.getElementById("<%=btnOK.ClientID %>").click();
    }
    
    function onComplete(errmsg)
    {
        if(errmsg!=null && errmsg!= undefined)
        {        
            window.parent.ShowMessage(errmsg);
        } 
    }
//-->
</script>

<head id="Head1" runat="server">
    <title>无标题页</title>
    <base target="_self">  
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <asp:Button ID="btnOK" runat="server" Text="Button" onclick="btnOK_Click" style="display:none" />
    <input type="hidden" id="dLine" runat="server" />
    <input type="hidden" id="dDataExport" runat="server" />
    </form>
</body>
</html>