<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UpdateShipDate
 * CI-MES12-SPEC-PAK-UpdateShipDate.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateShipDate.aspx.cs" Inherits="UpdateShipDate" Title="Update Ship Date" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 
 <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
        </Services>
    </asp:ScriptManager>
<div>
   <center >
        <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
                  
        <tr style="height:10%">
            <td style="width:15%" align="left"  >
                <asp:Label ID="LabelDN" runat="server"  CssClass="iMes_DataEntryLabel"   ></asp:Label>
            </td>
            <td style="width:85%" align="left"  >
               <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="65%" IsClear="true" IsPaste="true" />                  
            </td>
        </tr>
         
        
        <tr>
        <td><asp:Label ID="lblStatus" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left">
           
                 <asp:Label ID="lblStatusValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
        </td>
        </tr>
        <tr>
        <td><asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left">
           
                 <asp:Label ID="lblQtyValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </td>
        </tr>
        <tr>
        <td><asp:Label ID="lblShipdate" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
        <td align="left">
            
                 <asp:Label ID="lblShipdateValue" runat="server" CssClass="iMes_label_11pt"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                
        </td>
        </tr>
        
        <tr>
            <td><asp:Label ID="lblUpdateTo" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
            <td align="left"><input type="text" id="dCalShipdate" style="width:30%;" readonly="readonly" />

                <input id="btnCal" type="button" value=".." 
                  onclick="showCalendar('dCalShipdate')" style="width: 17px" 
                  class="iMes_button"  />
             </td>
        </tr>
        <tr>
        <td align ="right" colspan="2">
        <button  id ="btnSave"  style ="width:110px; height:24px;" onclick="btnSave_onclick()">
         Save
        </button> 
        </td>  
        </tr>
        <tr>
            <asp:UpdatePanel ID="UpdatePanelAll" runat="server"  RenderMode="Inline">
            <ContentTemplate>
                 <input type="hidden" runat="server" id="hidDN" />
                 <input type="hidden" runat="server" id="station" /> 
                 <input type="hidden" runat="server" id="pCode" /> 
                 <input type="hidden" runat="server" id="HIfUpdate" />
                 <input type="hidden" runat="server" id="hidDate" />
                  <button id="btnButton1" runat="server" type="button" style="display: none" />
                  <button id="btnButton2" runat="server" type="button" style="display: none" />
            </ContentTemplate>   
             </asp:UpdatePanel> 
        </tr>
       </table>
  </center>
</div>
   
<script type="text/javascript">
 
var station="";
var msgConfirmSave = '<%=this.GetLocalResourceObject(Pre + "_msgConfirmSave").ToString()%>';
var msgCartonProdSNNull = '<%=this.GetLocalResourceObject(Pre + "_msgCartonProdSNNull").ToString()%>';
var msgInvalidPattern = '<%=this.GetLocalResourceObject(Pre + "_msgInvalidPattern").ToString()%>';
var todayValue = "<%=today%>";
var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>'; 
//var todayValue = "2012 - 03 - 15";
window.onload = function() {
    var todayValue = "<%=today%>";
}
function DisplsyMsg(src) {
    ShowMessage(src);
    ShowInfo(src);
}
window.onbeforeunload = function() {
    ResetPage();
}
window.onunload = function() {
    ResetPage();
}
function input(data) {
    ShowInfo("");
    
    var dn = data.trim();
    if (dn != "") {
        document.getElementById("<%=hidDN.ClientID%>").value = dn;
        document.getElementById("<%=HIfUpdate.ClientID%>").value = "";
        document.getElementById("<%=hidDate.ClientID%>").value = "";
        document.getElementById("<%=lblStatusValue.ClientID%>").innerText = "";
        document.getElementById("<%=lblShipdateValue.ClientID%>").innerText = "";
        document.getElementById("<%=lblQtyValue.ClientID%>").innerText = "";
        document.getElementById("<%=btnButton1.ClientID%>").click();

    } else {
        ResetPage();
    }
}
/*function onTextBox1KeyDown() {
    ShowInfo("");
    if (event.keyCode == 9 || event.keyCode == 13) {
        event.cancel = true;
        event.returnValue = false;
        ShowInfo("");
        var dn = document.getElementById("<%=TextBox1.ClientID%>").value.trim();
        if (dn != "") {
            document.getElementById("<%=hidDN.ClientID%>").value = dn;
            document.getElementById("<%=HIfUpdate.ClientID%>").value = "";
            document.getElementById("<%=hidDate.ClientID%>").value = "";
            document.getElementById("<%=lblStatusValue.ClientID%>").innerText = "";
            document.getElementById("<%=lblShipdateValue.ClientID%>").innerText = "";
            document.getElementById("<%=lblQtyValue.ClientID%>").innerText = "";
            document.getElementById("<%=btnButton1.ClientID%>").click();
      
        } else {
            ResetPage();
        }
    }
}*/
function callNextInput() {
    
    getCommonInputObject().value = "";
    getCommonInputObject().focus();
    getAvailableData("input");
}
function getSuccess(display) {
    ResetPage();
    ShowInfo(display);
    endWaitingCoverDiv();
}
function getSuccessFull() {
    var temp = document.getElementById("<%=hidDN.ClientID%>").value;
    var successTemp = "";
    if (temp != "") {
        successTemp = "[" + temp + "] " + msgSuccess;
    }
    ResetPage();
    if (successTemp != "") {
        //ShowSuccessfulInfo(true);
        ShowSuccessfulInfo(true, successTemp);
    }
    else {
        ShowSuccessfulInfo(true);
    }
    //ShowInfo(successTemp);
    endWaitingCoverDiv();
}
function ResetPage()
{
    document.getElementById("<%=HIfUpdate.ClientID%>").value = "";
    document.getElementById("<%=hidDN.ClientID%>").innerText = "";
    document.getElementById("<%=hidDN.ClientID%>").value = "";
    document.getElementById("dCalShipdate").value = "";
    document.getElementById("<%=hidDate.ClientID%>").innerText = "";
    document.getElementById("<%=lblStatusValue.ClientID%>").innerText = "";
    document.getElementById("<%=lblShipdateValue.ClientID%>").innerText = "";
    document.getElementById("<%=lblQtyValue.ClientID%>").innerText = "";
    callNextInput();
    ShowInfo("");
}

function btnSave_onclick() {
    ShowInfo("");

    var dn = document.getElementById("<%=hidDN.ClientID%>").value;
    if (document.getElementById("<%=HIfUpdate.ClientID%>").value == "doNotSave") {
        DisplsyMsg('<%=this.GetLocalResourceObject(Pre + "_msgInvalidStatus").ToString()%>');
        ResetPage();
        return;
    }
    var selectDateValue = document.getElementById("dCalShipdate").value;
    var temp = selectDateValue.toString();
    if (temp == "") {
        DisplsyMsg('<%=this.GetLocalResourceObject(Pre + "_msgEmptyDate").ToString()%>');
        return;
    }
    if (temp < todayValue) {
        DisplsyMsg('<%=this.GetLocalResourceObject(Pre + "_msgErrorDate").ToString()%>');
        document.getElementById("dCalShipdate").value = "";
        return;
    }
    if (dn != "") {
        if (confirm(msgConfirmSave)) {
            document.getElementById("<%=hidDate.ClientID%>").innerText = temp;
            beginWaitingCoverDiv();
            document.getElementById("<%=btnButton2.ClientID%>").click();
        } else {
            ResetPage();
        }

    } else {
        ResetPage();
        DisplsyMsg(msgCartonProdSNNull);
    }
}
function SetPage() {
    ShowMessage(msgInvalidPattern);
    ResetPage();
    ShowInfo(msgInvalidPattern);
}
function getStatus(Status) {

    document.getElementById("<%=lblStatusValue.ClientID%>").innerText = Status;
}
function getShipDate(ShipDate) {

    document.getElementById("<%=lblShipdateValue.ClientID%>").innerText = ShipDate;
}
function getQty(Qty) {

    document.getElementById("<%=lblQtyValue.ClientID%>").innerText = Qty;
}
</script>
    



</asp:Content>


