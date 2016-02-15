<%--
/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-04-06                  Create
 * Known issues:
 * TODO： 
 *
 */
--%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UnpackCartonForRCTO.aspx.cs" Inherits="PAK_UnpackPalletNoForRCTO" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
 <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
         <Services>
             <asp:ServiceReference Path= "Service/UnpackServiceForRCTO.asmx" />
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
                 <input type="hidden" runat="server" id="hidIsFUR" />
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
    var msgInputCartonOrDn = '<%=this.GetLocalResourceObject(Pre + "_msgInputCartonOrDn").ToString() %>';
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

function btnOkClick() {
    //
    var isFRU = document.getElementById("<%=hidIsFUR.ClientID%>").value;
    ShowInfo("");
    DummyPalletNo = inputControl.value.trim();

    if (DummyPalletNo != "") {
        if (isFRU == "Y") {
            if (DummyPalletNo.length != 9 && DummyPalletNo.length != 16) {
                ShowMessage(msgInputCartonOrDn);
                ShowInfo(msgInputCartonOrDn);
                callNextInput();
            }
            else {
                if (confirm(msgConfirmUnpackDPN)) {
                    beginWaitingCoverDiv(); // 保存时统一修改为不启动waiting
                    UnpackService.UnpackCartonForFRU(DummyPalletNo, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);
                }
            }
          
        }
        else {
            if (confirm(msgConfirmUnpackDPN)) {
                beginWaitingCoverDiv(); // 保存时统一修改为不启动waiting
                UnpackService.UnpackCarton(DummyPalletNo, "<%=UserId%>", station, "<%=Customer%>", onSuccess, onFail);
            }
        
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


