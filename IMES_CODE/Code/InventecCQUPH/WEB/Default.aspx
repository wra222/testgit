﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body onload="openMainPage()">
    <form id="form1" runat="server"  >
    <div>
    
    </div>
    </form>
</body>
<script type="text/javascript" language="javascript">
    function openMainPage() {
        var openFeature = "left=0, top=0, height=" + (window.screen.height - 70) + "px, width=" + (window.screen.width - 10) + "px, status=no, resizable=yes, scrollbars=yes";
        //window.open('./webroot/aspx/index.html', '', openFeature);
        window.open('./webroot/aspx/index.html', '', '');
        window.opener = null;
        window.open('', '_self');
        window.close();


    }
</script> 
</html>
