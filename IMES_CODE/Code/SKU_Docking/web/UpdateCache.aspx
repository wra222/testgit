<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateCache.aspx.cs" Inherits="UpdateCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SM" >
    <Services>
        <asp:ServiceReference Path="Service/UpdateCacheWebService.asmx" />
    </Services>
</asp:ScriptManager>
    <div>
     <table align ="center" >
<tr>
<td height ="100px"></td>
</tr>
<tr>
<td align ="center" >

<input type="button" value="<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(languagePre + "btnUpdateCache").ToString()%>"
             style ="width:160px; height:34px; " onclick="updateCache()" />
</td>
</tr>   
<tr>
<td height ="60px"></td>
</tr>

<tr>
<td align ="center" >

<input type="button" value="<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(languagePre + "btnClearCache").ToString()%>"
             style ="width:160px; height:34px; " onclick="btnClearCache()" />
</td>
</tr>
<tr>
<td height ="60px"></td>
</tr>

<tr>
<td align ="center" >
 
 <input type="button" value="<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(languagePre + "btnRefreshPartMatch").ToString()%>"
             style ="width:160px; height:34px; " onclick="RefreshPartMatch()" />
</button> 
</td>
</tr>
</table>
    </div>
        <iMES:WaitingCoverDiv ID="divCover" runat="server"  KeyDownFun="KeyDownEvent()"  />
    </form>
</body>
</html>
<script  language ="javascript" >

 var msg='<%=Resources.iMESGlobalMessage.ResourceManager.GetString(languagePre + "msgSuccess") %>' ;     
 var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(languagePre + "msgSystemError") %>';     
var stage;
document.body.onload = function ()
{
    try{
        stage='<%=Stage%>';
    } catch(e) {
        alert(e.description);
    }
}
function updateCache()
{
    beginWaitingCoverDiv();
    UpdateCacheWebService.UpdateCache(stage,onReturn,onReturnError);
}
function btnClearCache()
{
    beginWaitingCoverDiv();
    UpdateCacheWebService.ClearCache(stage, onReturn, onReturnError);
}
function RefreshPartMatch() {
    beginWaitingCoverDiv();
    UpdateCacheWebService.UpdatePartMatch(stage, onReturn, onReturnError);
}
function onReturn(result)
{
    endWaitingCoverDiv();
    if(result!=null)
    {
        if(result!="")
        {
             ShowMessage(result);
        }
        else
        {
            alert(msg);
        }
    }
    else
    {
        alert(msgSystemError);
    }
}
function onReturnError(result) {
    endWaitingCoverDiv();
    ShowMessage(result.get_message());
}
 // exit page
 function ExitPage()
 {
 }
 
 //refresh page
 function ResetPage()
 {

 }
  </script>