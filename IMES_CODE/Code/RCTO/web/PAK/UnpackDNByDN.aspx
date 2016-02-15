<%--
 INVENTEC corporation ©2011 all rights reserved. 
 Description:UI for UnpackDNByDN Page
 UI:CI-MES12-SPEC-PAK-UI Unpack.docx –2011/10/17 
 UC:CI-MES12-SPEC-PAK-UC Unpack.docx –2011/10/17            
 Update: 
 Date        Name                  Reason 
 ==========  ===================== =====================================
 2011-10-20  itc202017             (Reference Ebook SourceCode) Create
 Known issues:
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UnpackDNByDN.aspx.cs" Inherits="PAK_UnpackDNByDN" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "Service/UnpackService.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
        <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
                  
        <tr style="height:10%">
            <td style="width:20%" align="left"  >
                <asp:Label ID="lblDeliveryNo" runat="server"  CssClass="iMes_DataEntryLabel"   ></asp:Label>
            </td>
            <td style="width:70%" align="left"  >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <iMES:Input ID="txtDeliveryNo" runat="server"  CanUseKeyboard="true" IsPaste="true" ProcessQuickInput="true" IsClear="false"
                         MaxLength="30" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"  ReplaceRegularExpression ="[^-0-9a-zA-Z\+\s\*]" 
                         Width="90%"  TabIndex="0" />
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </td>
            <td align ="right">
                <button  id ="btnOK"  style ="width:110px; height:24px;" onclick="btnOkClick()" type='button' >
                    <%=Resources.iMESGlobalDisplay.ResourceManager.GetObject(Pre + "_btnOK").ToString()%>
                </button>
            </td>  
        </tr>

        <tr>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
                <ContentTemplate>
                </ContentTemplate>   
            </asp:UpdatePanel> 
            <td>
                <input type="hidden" runat="server" id="station" /> 
                <input type="hidden" runat="server" id="hidSuper" />
            </td>
        </tr>
       </table>
  </center>
</div>
<script type="text/javascript">

    var msgConfirmUnpackDN = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpackDN").ToString() %>';
    var msgDNUploaded = '<%=this.GetLocalResourceObject(Pre + "_msgDNUploaded").ToString() %>';
    var msgInvalidDN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidDN").ToString() %>';
    var msgNullDN = '<%=this.GetLocalResourceObject(Pre + "_msgNullDN").ToString() %>';
 
var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var inputControl = getCommonInputObject();

window.onload = function() {
    ShowInfo("");
    callNextInput();
}

function processDataEntry(inputData)
{
   btnOkClick();
}  

function btnOkClick()
{
    ShowInfo("");
    dn = inputControl.value.trim();
    station = document.getElementById("<%=station.ClientID%>").value;

    if (dn != "") {
        if (confirm(msgConfirmUnpackDN)) {
            beginWaitingCoverDiv();
            if (document.getElementById('<%=hidSuper.ClientID%>').value == "") {
                UnpackService.UnpackDNByDN(dn, false, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);
            }
            else {
                UnpackService.UnpackDNByDN(dn, true, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);
            }
            
        }
    } else {
        alert(msgNullDN);
        callNextInput();
    }
}

function onSuccess(result)
{
    ShowInfo("");
    endWaitingCoverDiv();
    try 
    {
        if(result==null)
        {
            ShowMessage(msgSystemError);
            ShowInfo(msgSystemError);
        }

        else if (result==SUCCESSRET)
        {
              ShowSuccessfulInfo(true);
        }
        
        else 
        {
            var content =result;
            ShowMessage(content);
            ShowInfo(content);
        } 
      
    } 
    catch(e)
    {
        alert(e.description);
    }
    callNextInput();
     
}
    
function onFail(error)
{
    ShowInfo("");
    endWaitingCoverDiv();
    try
    {
       ShowMessage(error.get_message());
       ShowInfo(error.get_message());
    }
    catch(e) 
    {
    alert(e.description);
    }
    callNextInput();
   
}
    
function callNextInput()
{
    getCommonInputObject().value="";
    getCommonInputObject().focus();
    getAvailableData("processDataEntry");    
}


 </script>
    



</asp:Content>


