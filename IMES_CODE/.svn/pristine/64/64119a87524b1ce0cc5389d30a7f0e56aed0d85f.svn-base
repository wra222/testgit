<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardmain.aspx.cs" Inherits="webroot_aspx_dashboard_dashboardmain" %>
<%@ Import Namespace="com.inventec.system" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>Dashboard</title>
</head>


<script language=javascript>

var domain="<%=DocumentDomain%>";
if(domain!="")
{
    document.domain=domain;
}
  
//令牌
var tokenString = changeNull('<%=Request.Params["Token"]%>');
//var userNameString = changeNull('<%=Request.Params["UserName"]%>');
var editor = changeNull('<%=Request.Params["editor"]%>');
var isAuthorityUsermanager="False";
var isAuthorityDashboard="False";
//var logon; //记录登陆人， 可以用登陆人和application取此人的AccountInfo
//need delete!!!
//tokenString="AAEAAAD/////AQAAAAAAAAAMAgAAAF5jb20uaW52ZW50ZWMuUkJQQy5OZXQuZGF0YW1vZGVsLlNlc3Npb24sIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBQEAAAAtY29tLmludmVudGVjLlJCUEMuTmV0LmRhdGFtb2RlbC5TZXNzaW9uLlRva2VuAgAAAAF0CElUb2tlbit0AwMLU3lzdGVtLkd1aWQLU3lzdGVtLkd1aWQCAAAABP3///8LU3lzdGVtLkd1aWQLAAAAAl9hAl9iAl9jAl9kAl9lAl9mAl9nAl9oAl9pAl9qAl9rAAAAAAAAAAAAAAAIBwcCAgICAgICAuNUg5DAbJZBsz+eUjPKGF0B/P////3////jVIOQwGyWQbM/nlIzyhhdCw=="
function changeNull(val)
{
    if ("undefined" == val || null == val)
    {
        val = "";
    }
    return val;
}

//var oNewWindow=null; 
function showDisplay(uuid)
{
    //目的是使窗口能够打开并成为topmost
//    if(oNewWindow!=null)
//    {
//        try
//        {
//            oNewWindow.close();
//        }
//        catch(e)
//        {    
//            //忽略，用户可能手动已经关掉了window
//        }
//    }

//    oNewWindow=window.open("dashboardShow.aspx?uuid="+uuid,"_blank",
//      "fullscreen=yes,status=no,titlebar=no,toolbar=no,menubar=no,location=no,directories =no,scrollbars =no");      

//    oNewWindow=window.open("dashboardShow.aspx?uuid="+uuid,"_blank",
//      "fullscreen=no,status=no,titlebar=no,toolbar=no,menubar=no,location=no,directories =no,scrollbars =no");   
      
    try
    {
        var stageType=frames("menu").getStageTypeByWinId(uuid);
                   
        var WshShell =new ActiveXObject("WScript.Shell");
        var ref=window.location.href  
        //var re = /dashboardmain.aspx/gi;
        var re=new RegExp("dashboardmain.aspx(.*)","gi")
        var newref
        if(stageType=="<%=Constants.FA_STAGE %>")
        {
            newref = ref.replace(re, "dashboardShow.aspx?uuid="+uuid+"&stageType="+String(stageType));
        }
        else
        {
            newref = ref.replace(re, "dashboardSmtShow.aspx?uuid="+uuid+"&stageType="+String(stageType));
        }
        //var newref = ref.replace(re, "dashboardShow.aspx?uuid="+uuid+"&fromwhere=main" );
        var execname="IEXPLORE.exe "+newref;
        WshShell.run(execname); 
    }
    catch(e)
    {
        alert("Running activex control was forbidden in browser setting.");
        return;
    }
      
}



</script>
<frameset cols="20%,*" id = "mainfrm" name="_top;mainfrm" style=" border:0px; margin:0" frameborder=no>
    <frame name="menu" src="treeView.aspx" marginheight="0" marginwidth="0" scrolling="no" noresize >
	<frame name="main"  marginwidth="0px"  marginheight="0px" src="dashboardList.aspx" style="border:0" scrolling="no" noresize >
</frameset>

</html>

