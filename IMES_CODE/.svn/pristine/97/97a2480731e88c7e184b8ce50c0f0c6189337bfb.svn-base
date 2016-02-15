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

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PCAMBContact.aspx.cs" Inherits="SA_PCAMBContact" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        <Services>
            <asp:ServiceReference Path="Service/WebServicePCAMBContact.asmx" />
        </Services>
    </asp:ScriptManager>
    <div>
        <center>
            <table width="95%" style="vertical-align: middle; height: 20%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10%">
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblOldMB" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 80%" align="left">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <input type="text" id="txtOldMB" class="iMes_textbox_input_Yellow" runat="server"
                                    maxlength="200" style="width: 90%" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </tr>
                <tr style="height: 10%">
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblNewMB" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 80%" align="left">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <input type="text" id="txtNewMB" class="iMes_textbox_input_Yellow" runat="server"
                                    maxlength="200" style="width: 90%" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="height: 10%">
                    <td style="width: 10%" align="left">
                    </td>
                    <td style="width: 80%" align="left">
                        <input id="btnSave" type="button" onclick="btnOkClick()" runat="server" class="iMes_button"
                            onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <asp:UpdatePanel ID="UpdatePanelAll" runat="server" RenderMode="Inline">
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


        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var msgNewMBError = '<%=this.GetLocalResourceObject(Pre + "_msgNewMBError").ToString() %>';
        var msgOldMBError = '<%=this.GetLocalResourceObject(Pre + "_msgOldMBError").ToString() %>';
        var msgOldMBIllegeError = '<%=this.GetLocalResourceObject(Pre + "_msgOldMBIllegeError").ToString() %>';
        var msgNewMBIllegeError = '<%=this.GetLocalResourceObject(Pre + "_msgNewMBIllegeError").ToString() %>';
        
        var OldMB;
        var NewMB;

        window.onload = function() {
            ShowInfo("");
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            callNextInput();
        }
        function checkInput(data) {
            if ((data.length == 10 || data.length == 11) && (data.toString().charAt(4) == "M" || data.toString().charAt(4) == "B"))
                return SubStringSN(data, "MB");
            if (data.length == 11 && (data.toString().charAt(5) == "M" || data.toString().charAt(5) == "B")) 
                    return data
            return '0';
        }
        
        function checkMB(MB,type) {
            if (MB == "") {
                if(type =="old"){
                    alert(msgOldMBError);
                    document.getElementById("<%=txtOldMB.ClientID%>").focus();
                }
                else{
                    alert(msgNewMBError);
                    document.getElementById("<%=txtNewMB.ClientID%>").focus();
                }
                return '0';
            }
            var data = checkInput(MB);
            if (data == '0')
            {
                if (type == "old") {
                    alert(msgOldMBIllegeError);
                    document.getElementById("<%=txtOldMB.ClientID%>").value='';
                    document.getElementById("<%=txtOldMB.ClientID%>").focus();
                }
                else {
                    alert(msgNewMBIllegeError);
                    document.getElementById("<%=txtNewMB.ClientID%>").value = '';
                    document.getElementById("<%=txtNewMB.ClientID%>").focus();
                }
                return data;
            }
            return data
        }
        
        function btnOkClick() {
            ShowInfo("");
            station = document.getElementById("<%=station.ClientID%>").value;

            OldMB = document.getElementById("<%=txtOldMB.ClientID%>").value.trim();
            NewMB = document.getElementById("<%=txtNewMB.ClientID%>").value.trim();
            var ModOldMB = checkMB(OldMB, 'old');
            var ModNewMB = checkMB(NewMB, 'new');
            if (ModOldMB != '0' && ModNewMB != '0') { 
                beginWaitingCoverDiv();
                WebServicePCAMBContact.CheckMBandSave("", ModNewMB, ModOldMB, editor, stationId, customer, onSuccess, onFail);
            }
        }

        function onSuccess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                if (result == null) {
                    ShowMessage(msgSystemError);
                    ShowInfo(msgSystemError);
                }

                else if (result == SUCCESSRET) {
                    ShowSuccessfulInfo(true);
                }

                else {
                    var content = result;
                    ShowMessage(content);
                    ShowInfo(content);
                }

            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();

        }

        function onFail(error) {
            ShowInfo("");
            endWaitingCoverDiv();
            try {
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
            }
            catch (e) {
                alert(e.description);
            }
            callNextInput();

        }

        function callNextInput() {
            document.getElementById("<%=txtOldMB.ClientID %>").focus();
            document.getElementById("<%=txtOldMB.ClientID %>").value = "";
            document.getElementById("<%=txtNewMB.ClientID %>").value = "";

        }


    </script>

</asp:Content>
