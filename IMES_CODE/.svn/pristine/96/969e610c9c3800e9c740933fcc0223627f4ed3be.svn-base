<%--
/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description:clear page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2008-12-10  Zhao Meili(DD)        Create      
 * 2009-01-19  Zhao Meili(DD)        Create:  ITC-932-0179问题解决            
 * Known issues:
 */
 --%>
<%@ Page  ContentType="text/html;Charset=UTF-8" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Clear.aspx.cs" Inherits="Clear" %>
  <%@ MasterType VirtualPath ="~/MasterPage.master" %>
  <asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="CommonControl/JS/iMESCommonUse.js"></script>

<asp:ScriptManager runat="server" ID="SM" EnablePartialRendering="true">
    <Services>
        <asp:ServiceReference Path="Service/SessionService.asmx" />
    </Services>
</asp:ScriptManager>

    <div>
<table align ="center" >
<tr>
<td height ="10px"></td>
</tr>
<%--<tr>
<td align="center" >
<asp:Label runat="server" ID="lblTitle" Text ="" CssClass="label_title"></asp:Label>
</td>
</tr>--%>
<tr>
<td height ="30px"></td>
</tr>
<tr>
<td >
<asp:Label runat="server" ID="lblSK" Text ="" ></asp:Label>&nbsp;
<select runat ="server"  id="dlType" style =" width :130px"    onchange =" FreshGrid()"  >
</select>
</td>
</tr>
<tr>
<td height ="5px"></td>
</tr>
<tr>
<td width ="100%">
      <iframe id='GridviewFram' width='900px' frameborder='0'
        scrolling='no' height='276px' style="margin-bottom: 0px; margin-left: 0px; margin-right: 0px;
        margin-top: 0px; border: 0px; padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
        clip: rect(0px 0px 0px 0px); padding-top: 0px; background-color:Transparent;"></iframe>
</td>
</tr>
<tr>
<td height ="10px"></td>
</tr>
<%--<tr>
<td >
<asp:Label runat="server" ID="lblReason" Text ="" ></asp:Label>&nbsp;&nbsp;
<input type="text" id="txtInput" style = "width :830px; height:20px " />
</td>
</tr>--%>
<tr>
<td height ="30px"></td>
</tr>
<tr>
<td align ="center" >
<button  id ="btnClear"  style ="width:110px; height:24px; " onclick="clearSession()" >
 <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_btnClearSession").ToString()%>
</button> 
</td>
</tr>
</table>
   <input type ="text" id="hfvalue" style ="display :none"  />
      <input type ="text" id="hfstationId" style ="display :none"  />
    </div>
    
 <script  language ="javascript" >
//   var keySN = "SN";
//   var keyPO  = "PO";
//   var keyDN  = "DN";
//   var keyCT  = "CARTON";
//  var keyCL  = "COBOL";
   var dlType=document.getElementById("<%=dlType.ClientID%>"); 
   
     window.onload = function()
    {
        dlType.focus();
        ShowGrid("clear","");
    }
     function ShowGrid(flag,key)
     {
      document.getElementById("GridviewFram").src = "GridForClear.aspx?key="+key.toString();
     }
 
 function clearSession()
 {

    var key=document.getElementById("hfvalue").value;
    var serviceName=document.getElementById("hfstationId").value;
    if((selectKey!="")&&(key!=""))
    {
          var message='<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_ConfirmCS").ToString()%>';
          if(confirm(message))
          {
                SessionService.ClearSession(key,selectKey,serviceName,onClearReturn);
          }
    }
    else
    {
        var message= '<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_NoSesToClear").ToString()%>';
        alert(message);
    }
 }
 
 function onClearReturn(result)
 {
     if(result!="")
     {
        ShowMessage(result);
     }
     else
     {
        var mes='<%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_ClearSuc").ToString()%>';
        alert(mes);
        document.getElementById("GridviewFram").src = "GridForClear.aspx?key="+selectKey.toString();
     }

 }
     var selectKey;
 function FreshGrid()
 {
    selectKey=dlType.options[dlType.selectedIndex].value.trim();  
    document.getElementById("GridviewFram").src = "GridForClear.aspx?key="+selectKey.toString();
 }
 
 // exit page
 function ExitPage()
 {
 }
 
 //refresh page
 function ResetPage()
 {
    dlType.selectedIndex=0;
    FreshGrid();
 }
 
 </script>
</asp:Content>