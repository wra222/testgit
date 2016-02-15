<%@ page language="C#" autoeventwireup="true" inherits="title, App_Web_title.aspx.39cd9290" theme="MainTheme" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<fis:header id="header2" runat="server"/>
<body leftMargin="0" topMargin="0" rightMargin="1" oncontextmenu="return false;" onmouseup="fOnMouseUp();">
<iframe src="test.aspx" height=0 width=0></iframe>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="0" cellspacing="0" style="border:1px; border-color:Black"  align=left>
        <tr>
            <td bgcolor="#3b85b6" align=left><img src="../../images/logo1.jpg"  /></td>
            <td id="welcome" bgcolor="#3b85b6"></td>
        </tr>
    </table>
    </div>
    <asp:HiddenField ID="user" runat="server"></asp:HiddenField>
    </form>
    
</body>
<fis:footer id="footer1" runat="server"/>
</html>
<script>
    document.getElementById("welcome").innerText = "Welcome "+document.getElementById("<%=user.ClientID%>").value; 
    function fOnMouseUp(){
        if(typeof(window.parent.frames("main").fOnMouseUp) == "function"){
            window.parent.frames("main").fOnMouseUp();
        }
    }
</script>