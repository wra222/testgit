<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:CombineCOAandDNReprint page
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-2-7     207003               Create          
 * Known issues:
 * TODO:
 */ --%>
 
 
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="unpackallbydn.aspx.cs" Inherits="DOCK_UnpackAllbyDN" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
 <div >
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Services>
    </Services>
    </asp:ScriptManager>
        <table width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width:140px;"/>
                <col />
                <col style="width:150px;"/>
            </colgroup>
             <tr>
                <td>
                    <asp:Label ID="lblPizzaID" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                </td>
                <td>
                    <iMES:Input ID="txtPizzaID" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="60%" IsClear="true" IsPaste="true"  />
                </td>
                <td>
                    <button id="btnUnpack" runat="server"    style ="width:110px; height:24px;"  onclick="DoOK()"  ></button>
                </td> 
            </tr>                            
        </table>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
              <input type="hidden" runat="server" id="HDN" />
              <button id="btnGetDN" runat="server" type="button"  style="display: none" /> 
              <button id="btnDoUnpack" runat="server" type="button" style="display: none" /> 
            </ContentTemplate>   
        </asp:UpdatePanel> 
    </div>
<script language="javascript" type="text/javascript">

    var kitID;
    var msgKitIDNull = '<%=this.GetLocalResourceObject(Pre + "_msgKitIDNull").ToString()%>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgConfirm = '<%=this.GetLocalResourceObject(Pre + "_msgConfirm").ToString()%>';
    window.onload = function() {
        initPage();
    }
    function indata(backData) {
        kitID = backData.trim();
        DoUnpack();
    }
    function DoOK() {
        ShowInfo("");
        if (null == kitID) {
            kitID = getCommonInputObject().value.trim();
        }
        if ("" == kitID) {
            kitID = getCommonInputObject().value.trim();
        }

        if (kitID.length == 16) {
        }
        else {
            alert("Wrong Code!");
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("indata");
            event.returnValue = false;
            return false;
        }
        if (kitID != "") {
            try {
                document.getElementById("<%=HDN.ClientID%>").value = kitID;
                event.returnValue = false;
                document.getElementById("<%=btnGetDN.ClientID%>").click();
                return true;
            }
            catch (e) {
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("indata");
                alert(e);
                event.returnValue = false;
                return false;
            }
        }
        else {
            alert(msgKitIDNull);
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("indata");
            event.returnValue = false;
            return false;
        }
        event.returnValue = false;
        return false;
    }
    function DoUnpack() {
        ShowInfo("");
        if (null == kitID) {
            kitID = getCommonInputObject().value.trim();
        }
        if ("" == kitID) {
            kitID = getCommonInputObject().value.trim();
        }

        if (kitID.length == 16) {
        }
        else {
            alert("Wrong Code!");
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("indata");
            return ;
        }
        if (kitID != "") {
            try {
                document.getElementById("<%=HDN.ClientID%>").value = kitID;
                document.getElementById("<%=btnGetDN.ClientID%>").click();
                return ;
            }
            catch (e) {
                getCommonInputObject().value = "";
                getCommonInputObject().focus();
                getAvailableData("indata");
                alert(e);
                return ;
            }
        }
        else {
            alert(msgKitIDNull);
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("indata");
            return ;
        }
        return ;
    }

    function getResetAll(display) {
        kitID = "";
        if (display == "success") {
            var temp = document.getElementById("<%=HDN.ClientID%>").value;
            var successTemp = "";
            if (temp != "") {
                successTemp = "[" + temp + "] " + msgSuccess;
            }
            if (successTemp != "") {
                ShowSuccessfulInfo(true, successTemp);
                //ShowInfo(successTemp, "green");
            }
            else {
                ShowSuccessfulInfo(true);
                //ShowInfo(msgSuccess, "green");
            }
        }
        document.getElementById("<%=HDN.ClientID%>").value = "";
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("indata");
        getCommonInputObject().focus();
    }     
    function initPage() {
        clearData();
        kitID = "";
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("indata");
    }
    function getConfirm() {

        if (confirm(msgConfirm)) {
            document.getElementById("<%=btnDoUnpack.ClientID%>").click();
        }
        else {
            getResetAll("");
        }
    }
    
</script>  
    
</asp:Content>

