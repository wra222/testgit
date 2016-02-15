<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: Message Box page
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-03-05  Li MingJun (eB1)     Create
 Known issues:
 --%>
<%@ Page ContentType="text/html;Charset=UTF-8" Language="C#" AutoEventWireup="true" CodeFile="RedirectErrMsg.aspx.cs" Inherits="RedirectErrMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <bgsound src="" autostart="true" id="bsoundInModal" loop="infinite"></bgsound>
    <script type="text/javascript" src="../js/iMESCommonUse.js"></script>
</head>
<body bgcolor="white">
    <form id="form1" runat="server">  
        <div style="text-align:center"> 
            <br />
            <table id="tblMsg" border="0" runat="server">
                <tr>
                    <td align="center" >
                        <div id="divMsg" class="errMessage_style" runat="server"></div>
                    </td>
                </tr>
            </table>
        </div>
        <%--<iMES:WaitingCoverDiv ID="divCover" runat="server"  KeyDownFun="KeyDownEvent()"  />--%>
    </form>
    <script language="javascript" type="text/javascript">
        var sUrl = 'Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
        playSoundInModalWindow();
        
        function playSoundInModalWindow()
        {
            var paraPlay = '<%= Request["play"] %>';
            
            if (paraPlay != '' && paraPlay == 'true')
            {
                var obj = getSoundObj();
                obj.src = sUrl;    
            }
           
        }

        function getSoundObj()
        {
            return document.getElementById("bsoundInModal");
        }
    </script>
</body>
</html>
