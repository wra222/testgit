<%@ Page Language="C#" AutoEventWireup="true"  ContentType="text/html;Charset=UTF-8" CodeFile="ExportExcelForSQL.aspx.cs" Inherits="CommonFunction_ExportExcelForSQL"  ValidateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript">   

   
    function dealExport(DB,SQLText,Editor)
    {
        document.getElementById("<%=dDB.ClientID %>").value = DB;
        document.getElementById("<%=dSQLText.ClientID %>").value = SQLText;
        document.getElementById("<%=dEditor.ClientID %>").value = Editor;
        document.getElementById("<%=btnOK.ClientID %>").click();
    }
    
    function onComplete(errmsg)
    {
        if(errmsg!=null && errmsg!= undefined)
        {        
            window.parent.ShowMessage(errmsg);
        } 
    }

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
    <input type="hidden" id="dDB" runat="server" />
    <input type="hidden" id="dSQLText" runat="server" />
    <input type="hidden" id="dEditor" runat="server" />
    
    </form>
</body>
</html>
