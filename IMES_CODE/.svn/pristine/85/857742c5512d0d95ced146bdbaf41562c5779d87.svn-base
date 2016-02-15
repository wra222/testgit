
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" AsyncTimeout="9000" CodeFile="UnpackAllByDN.aspx.cs" Inherits="PAK_UnpackAllByDN" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 
<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="9000">
         <Services>
             <asp:ServiceReference Path= "Service/UnpackService.asmx" />
        </Services>
    </asp:ScriptManager>
    
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
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="hidbtnClick" EventName="ServerClick" />
                </Triggers>  
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
                <input type="hidden" runat="server" id="hidDN" />
                <button id="hidbtnClick" style="width: 0; display: none;" runat="server" onserverclick="hidbtn_Click"></button>
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
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
 
var SUCCESSRET ="<%=WebConstant.SUCCESSRET%>";
var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
var inputControl = getCommonInputObject();

window.onload = function() {
    ShowInfo("");
    callNextInput();
}

function processDataEntry(inputData) {

    btnOkClick();
}  

function btnOkClick()
{
    ShowInfo("");

    station = document.getElementById("<%=station.ClientID%>").value;
    document.getElementById("<%=hidDN.ClientID%>").value = getCommonInputObject().value;
    var dn = document.getElementById("<%=hidDN.ClientID%>").value;
    if (dn != "") {
        if (confirm(msgConfirmUnpackDN)) {
            beginWaitingCoverDiv();
            
            document.getElementById("<%=hidbtnClick.ClientID %>").click();

        }
    } else {
        alert(msgNullDN);
        callNextInput();
    }
}

function unpackSuccess() {
    ShowSuccessfulInfo(true, "[" + document.getElementById("<%=hidDN.ClientID%>").value + "] " + msgSuccess);
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
    getCommonInputObject().value = "";

    getCommonInputObject().focus();

    getCommonInputObject().select();
    
    getAvailableData("processDataEntry");    
}


 </script>
    



</asp:Content>


