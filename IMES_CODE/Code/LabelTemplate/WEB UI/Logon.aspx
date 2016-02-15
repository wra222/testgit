<%@ page language="C#" autoeventwireup="true" inherits="Logon, App_Web_logon.aspx.cdcab7d2" theme="MainTheme" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="com.inventec.system" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body onload="load()">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
<script language="JavaScript">
<!--
function load()
{
    var openFeature = "left=0, top=0, height=" + (screen.height - 70) + "px, width=" + (screen.width - 10) + "px, status=no, resizable=no, scrollbars=yes";

    var ret = com.inventec.template.manager.TemplateManager.checkIfValidUser();
	if (ret.error != null) {
        alert(ret.error.Message);
    } else{
        if(!ret.value)
        {
            alert("Invalid user!");
            return;
        }
        
    }
    ret = com.inventec.template.manager.TemplateManager.getUuid();
	if (ret.error != null) {
        alert(ret.error.Message);
    } else{
      window.open("webroot/aspx/main/Default.aspx", ret.value, openFeature);
      window.opener = null;
      window.close();      
        
    }
  
}      
//-->
</script>
