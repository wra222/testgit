<%--
/*
* INVENTEC corporation ©2012 all rights reserved. 
* Description:FA MB Return
* CI-MES12-SPEC-PAK-FA MB Return.docx –2012/1/10           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-1-10   207003                Create   
* Known issues:
* TODO：
* 
*/
 --%>
 <%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FAMBReturn.aspx.cs" Inherits="SA_FAMBReturn" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>


     <asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

         <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
     <asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">

    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >    
        var editor = "";
        var customer = "";
        var station = "";
        var txtObj;
        var msg_WrongCode = '<%=this.GetLocalResourceObject(Pre + "_msg_WrongCode").ToString() %>';
        var msg_empty = '<%=this.GetLocalResourceObject(Pre + "_msg_empty").ToString() %>';
        var msg_ConfirmSave = '<%=this.GetLocalResourceObject(Pre + "_msg_ConfirmSave").ToString() %>';
        var msg_success = '<%=this.GetLocalResourceObject(Pre + "_msg_success").ToString() %>';
        var msgConfirmSave = '<%=this.GetLocalResourceObject(Pre + "_msg_ConfirmSave").ToString() %>';
        var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';  
        window.onload = function() {
            txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
            initPage();
        };
        function initPage() {
            document.getElementById("<%=HMBSno.ClientID%>").value = "";
            ShowInfo("Please input MBSno.");
            callNextInput();
        }
        function onTextBox1KeyDown() {
            ShowInfo("");
            if (event.keyCode == 9 || event.keyCode == 13) {
                ShowInfo("");
                txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
                var inputTextBox1 = txtObj.value;
                inputTextBox1 = inputTextBox1.toUpperCase();
                inputTextBox1 = inputTextBox1.trim();
                if (inputTextBox1.length == 0) {
                    alert(msg_empty);
                    txtObj.value = "";
                    txtObj.focus();
                }
                //else if ((inputTextBox1.length == 10 || inputTextBox1.length == 11)
                //&& inputTextBox1.substr(4, 1) == "M") {
                else if ((inputTextBox1.length == 10 || inputTextBox1.length == 11)) {
                if (inputTextBox1.substr(5, 1) == "M" || inputTextBox1.substr(5, 1) == "B")
                        inputTextBox1 = inputTextBox1.substring(0, 11);
                    else
                        inputTextBox1 = inputTextBox1.substring(0, 10);
                    txtObj.value = inputTextBox1;
                    document.getElementById("<%=HMBSno.ClientID%>").value = inputTextBox1;
                    ShowInfo("MBSno:" + inputTextBox1);
                    var rng = txtObj.createTextRange();
                    rng.moveStart("character", 0);
                    rng.collapse(true);
                    rng.select();
                    txtObj.focus();
                }
                else {
                    alert(msg_WrongCode);
                    txtObj.value = "";
                    txtObj.focus();
                }
                event.cancel = true;
                event.returnValue = false;
            }
        }
        function callNextInput() {
            txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
            txtObj.value = "";
            txtObj.focus();
        }


        function ResetPage() {
            initPage();
            callNextInput();
            endWaitingCoverDiv();
        }

        function getSuccess() {
            var temp = document.getElementById("<%=HMBSno.ClientID%>").value;
            var successTemp = "";
            if (temp != "") {
                successTemp = "[" + temp + "] " + msgSuccess;
            }
            
            document.getElementById("<%=HMBSno.ClientID%>").value = "";
            txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
            txtObj.value = "";
            txtObj.focus();
            ResetPage();

            //ShowInfo(msg_success);
            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
            endWaitingCoverDiv();
        }
        function getConfirm() {
            endWaitingCoverDiv();
            beginWaitingCoverDiv();
            if (confirm(msgConfirmSave)) {
                document.getElementById("<%=btnMakeSure.ClientID%>").click();
           }
           else {
               txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
               txtObj.value = "";
               txtObj.focus();
               document.getElementById("<%=btnCancel.ClientID%>").click();
           }
        }
        function btnMBClear_onclick() {
            initPage();
        }
        function btnFAMBReturn_onclick() {
            txtObj = document.getElementById("<%=TextBox1.ClientID%>"); 
            var inputTextBox1 = document.getElementById("<%=HMBSno.ClientID%>").value;
            inputTextBox1 = inputTextBox1.toUpperCase();
            inputTextBox1 = inputTextBox1.trim();
            if (inputTextBox1.length == 0) {
                alert(msg_empty);
                txtObj.value = "";
                txtObj.focus();
            }
            //else if ((inputTextBox1.length == 10 || inputTextBox1.length == 11)
            //    && inputTextBox1.substr(4, 1) == "M") {
            else if ((inputTextBox1.length == 10 || inputTextBox1.length == 11)) {
            if (inputTextBox1.substr(5, 1) == "M" || inputTextBox1.substr(5, 1) == "B")
                        inputTextBox1 = inputTextBox1.substring(0, 11);
                    else
                        inputTextBox1 = inputTextBox1.substring(0, 10);
                    //inputTextBox1 = inputTextBox1.substring(0, 10);
                    txtObj.value = inputTextBox1;
                    ShowInfo("MBSno:" + inputTextBox1);
                    var rng = txtObj.createTextRange();
                    rng.moveStart("character", 0);
                    rng.collapse(true);
                    rng.select();
                    txtObj.focus();
                    beginWaitingCoverDiv();
                    document.getElementById("<%=btnSave.ClientID%>").click();
            }
            else {
                alert(msg_WrongCode);
                txtObj.value = "";
                txtObj.focus();
            }
        }

    </script>
     
     
     
     <div>
       <center>
       <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr >
                <td  align ="center">
                    <asp:Label ID="lblDataEntry" width="12%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" style="width:40%"    />
                </td>
            </tr>
           <tr><td>&nbsp;</td><td></td></tr>
           <tr><td>&nbsp;</td><td></td></tr>
           <tr><td>&nbsp;</td><td></td></tr>
           <tr><td>&nbsp;</td><td></td></tr>
           <tr><td>&nbsp;</td><td></td></tr>
            <tr >
                <td align ="center" > 
                    <input id="btnFAMBReturn" type="button"  runat="server" 
                        class="iMes_button"  
                        onmouseover="this.className='iMes_button_onmouseover'"  
                        onmouseout="this.className='iMes_button_onmouseout'" visible="True"  
                        onclick="btnFAMBReturn_onclick()" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="btnMBClear" type="button"  runat="server" 
                        class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'"    onclick="btnMBClear_onclick()" visible="True" />
                </td>
            </tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr><td>&nbsp;</td><td></td></tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
	                    <ContentTemplate>
                        <input type="hidden" runat="server" id="HMBSno" />  
                        <button id="btnSave" runat="server" type="button" style="display: none" />
                        <button id="btnMakeSure" runat="server" type="button" style="display: none" />
                        <button id="btnCancel" runat="server" type="button" style="display: none" />
                     </ContentTemplate>   
                    </asp:UpdatePanel> 
                </td>
            </tr>       
        </table> 
    </center>
</div>
    

</asp:Content>
