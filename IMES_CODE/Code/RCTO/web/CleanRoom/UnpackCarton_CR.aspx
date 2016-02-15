﻿

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UnpackCarton_CR.aspx.cs" Inherits="UnpackCarton_CR" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
 <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "Service/WebServiceUnpackCarton_CR.asmx" />
        </Services>
    </asp:ScriptManager>
<div>
   <center >
        <table width="95%" style="vertical-align:middle; height:40%" cellpadding="0" cellspacing="0" >
        <tr style="height:10%"><td colspan="3"> &nbsp;</td></tr>
         <tr style="height:20%" valign="middle">
            <td style="width:30%" align="left"  >
                <asp:Label ID="lblDummyPalletNo" runat="server"  CssClass="iMes_DataEntryLabel"   ></asp:Label>
            </td>
            <td style="width:60%" align="left"  >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <iMES:Input ID="txtDummyPalletNo" runat="server"  CanUseKeyboard="true" IsPaste="true" ProcessQuickInput="true" IsClear="false"
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
        <tr style="height:10%"><td colspan="3"> &nbsp;</td></tr>
        <tr>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
                <ContentTemplate>
                </ContentTemplate>   
            </asp:UpdatePanel> 
        </tr>
       </table>
  </center>
</div>
<script type="text/javascript">

    var msgConfirmUnpackDPN = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmUnpackDPN").ToString() %>';
    var msgInvalidDPN = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidDPN").ToString() %>';
    var msgNullDPN = '<%=this.GetLocalResourceObject(Pre + "_msgNullDPN").ToString() %>';
 
    var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';

    var inputControl = getCommonInputObject();
    var station = '<%=station%>';
    var pCode = '<%=pCode%>';

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
    DummyPalletNo = inputControl.value.trim();

    if (DummyPalletNo != "") {
        if (confirm(msgConfirmUnpackDPN)) {
            beginWaitingCoverDiv(); // 保存时统一修改为不启动waiting
            WebServiceUnpackCarton_CR.UnpackCarton(DummyPalletNo, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);
        }
    } else {
        alert(msgNullDPN);
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
            ShowSuccessfulInfo(true, "[" + DummyPalletNo + "]" + " " + msgSuccess);
        }
        
        else 
        {
            var content =result;
            ShowMessage(content);
            ShowInfo(content);
        } 
      
    }
    catch (e) 
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
    catch (e) 
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


