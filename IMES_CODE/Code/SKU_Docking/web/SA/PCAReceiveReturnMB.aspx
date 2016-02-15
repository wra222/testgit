<%@ MasterType VirtualPath="~/MasterPage.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="PCAReceiveReturnMB.aspx.cs" Inherits="SA_PCAReceiveReturnMB" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                
            </Services>
        </asp:ScriptManager>
        <center>
            <table border="0" width="95%">
                <tr>
                    <td align="left">
                        <%--<asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>--%>
                        <asp:Label ID="lblRework" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <%--<iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="100" IsPercentage="true" />--%>
                        <asp:DropDownList ID="cmbRework" runat="server" Width="98%" >
                            <asp:ListItem Text="可重工" Value="可重工"></asp:ListItem>
                            <asp:ListItem Text="不可重工" Value="不可重工" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" align="center">
                        <asp:CheckBox ID="chkMusic" Checked runat="server" CssClass="iMes_label_13pt" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblMBLookLike" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:DropDownList ID="cmbMBLookLike" runat="server" Width="98%" >
                            <asp:ListItem Text="外觀" Value="外觀"></asp:ListItem>
                            <asp:ListItem Text="可涵蓋" Value="可涵蓋" ></asp:ListItem>
                            <asp:ListItem Text="不可涵蓋" Value="不可涵蓋" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2" align="center">
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="left">
                        <asp:Label ID="lblMBSno" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:Label ID="txtMBSno" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                    
                    <asp:Label ID="txtFamily" runat="server" CssClass="iMes_label_13pt" />
                    </td>
                    <td style="width: 10%" align="left">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"  Visible="false"></asp:Label>
                    </td>
                    <td style="width: 20%" align="left">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt" Visible="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>             
                
                <tr>
                    <td colspan="7" align="left">
                        <asp:Label ID="lblTableTitle" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="7">
                        <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                            <ContentTemplate>
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                    GvExtWidth="100%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                    Width="99.9%" Height="230px" SetTemplateValueEnable="true" GetTemplateValueEnable="true"
                                    HighLightRowPosition="3" HorizontalAlign="Left">
                                    <Columns>
                                        <%--<asp:BoundField DataField="PdLine"  />
                                        <asp:BoundField DataField="TestStn"  />
                                        <asp:BoundField DataField="Defect" />
                                        <asp:BoundField DataField="Cause" />
                                        <asp:BoundField DataField="CDate" />
                                        <asp:BoundField DataField="UDate" />--%>
                                        
                                        <asp:BoundField DataField="MBSN"  />
                                        <asp:BoundField DataField="Line"  />
                                        <asp:BoundField DataField="Defcet"  />
                                        <asp:BoundField DataField="Cause" />
                                        <asp:BoundField DataField="Cover" />
                                        <asp:BoundField DataField="ICT" />
                                        <asp:BoundField DataField="ICTOpt" />
                                        <asp:BoundField DataField="Fun" />
                                        <asp:BoundField DataField="FunOpt" />
                                        <asp:BoundField DataField="Remark" />
                                        <asp:BoundField DataField="Cdt" />
                                    </Columns>
                                </iMES:GridViewExt>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
                <tr>
                    <td align="left">
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" colspan="6">
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" IsClear="true" Width="99%"
                            CanUseKeyboard="true" IsPaste="true" MaxLength="50" InputRegularExpression="^[-0-9a-zA-Z\+\s\*]*$"
                            ReplaceRegularExpression="[^-0-9a-zA-Z\+\s\*]" />
                        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <button id="btnProcess" runat="server" type="button" onclick="" style="display: none" />
                                <button id="btnSave" runat="server" type="button" onclick="" style="display: none" />
                                <button id="btnExit" runat="server" type="button" onclick="" style="display: none" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <input type="hidden" runat="server" id="hidStation" />
                        <input type="hidden" runat="server" id="hidEditor" />
                        <input type="hidden" runat="server" id="hidIsBU" />
                        <input type="hidden" runat="server" id="hidRework" />
                        <input type="hidden" runat="server" id="hidLookLike" />
                        <input type="hidden" runat="server" id="hidFlagSound" />
                        <input type="hidden" runat="server" id="hidInput" />
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">

        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';
        var mesInputError = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesInputError").ToString()%>';
        var mesNoInputMBSno = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_mesNoInputMBSno").ToString()%>';
        var SUCCESSRET = "SUCCESS";
        var flagSound = true;
        var inputFlag = false;
        var editor;
        var customer;
        var stationId;
        var line = "";
        var tbl;
        var DEFAULT_ROW_NUM = 9;
        var inputObj;
        var IsBU = "";
        
       
        document.body.onload = function() {
            try {
                editor = "<%=UserId%>";
                customer = "<%=Customer%>";
                stationId = '<%=Request["Station"] %>';
                tbl = "<%=GridViewExt1.ClientID %>";
                inputObj = getCommonInputObject();
                inputObj.focus();
                getAvailableData("input");
                
                
            } catch (e) {
                alert(e.description);
            }

        }
        
        function input(inputData) {
        //function processDataEntry(inputData) {
            ShowInfo("");
            flagSound = document.getElementById("<%=chkMusic.ClientID %>").checked;
            if (flagSound) {
                document.getElementById("<%=hidFlagSound.ClientID %>").value = "Y";
            }
            else {
                document.getElementById("<%=hidFlagSound.ClientID %>").value = "N";
            }
            if (inputFlag) {
                if (inputData == "7777") {
                    ShowInfo("");
                    Reset();
                    ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
                    setSrollByIndex(0, false);
                    getAvailableData("input");
                }
                else if (inputData == "9999") {
                    ShowInfo("");
                    //save action
                    save();
                }
                else {
                    ShowInfo("");
                    inputCode = checkInput(inputData);
                    if (inputCode == "BU") {
                        IsBU = "BU";
                        saveBU(IsBU);
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }
            }
            else {
                if (inputData != "9999" && inputData != "7777") {
                    ShowInfo("");
                    //beginWaitingCoverDiv();
                    //mbSno = SubStringSN(data, "MB");
                    inputCode = checkInput(inputData);
                    if (inputCode == "MBSno") {
                        mbSno = SubStringSN(inputData, "MB");
                        setInputOrSpanValue(document.getElementById("<%=txtMBSno.ClientID%>"), mbSno)
                        document.getElementById("<%=hidInput.ClientID %>").value = mbSno;
                        document.getElementById("<%=btnProcess.ClientID%>").click(); //btnSave
                        inputFlag = true;
                        getAvailableData("input");
                        //inputObj.focus();
                        //WebServicePCAReceiveReturnMB.test(mbSno, line, editor, stationId, customer, testSucc, testFail);
                    }
                    else if (inputCode == "BU") {
                        alert("Please Input MBSN First...");
                        getAvailableData("input");
                        inputObj.focus();
                    }
                    else {
                        alert(mesInputError);
                        getAvailableData("input");
                        inputObj.focus();
                    }
                }
                else {
                    alert(mesNoInputMBSno);
                    getAvailableData("input");
                    inputObj.focus();
                }
            }
        }

        function saveBU(IsBU) {
            setInputOrSpanValue(document.getElementById("<%=txtFamily.ClientID%>"), IsBU)
        }

        function save() {
            document.getElementById("<%=hidIsBU.ClientID%>").value = IsBU;
            document.getElementById("<%=hidRework.ClientID%>").value = document.getElementById("<%=cmbRework.ClientID%>").value; //cmbRework
            document.getElementById("<%=hidLookLike.ClientID%>").value = document.getElementById("<%=cmbMBLookLike.ClientID%>").value;

            document.getElementById("<%=btnSave.ClientID%>").click(); //btnSave
            Reset();
        }

        function checkInput(value) {
            var code = "ERROR";
            try {
                if (value.length == 2) {
                    var pattern = /[B]{1}[U]{1}/;
                    if (pattern.test(value)) {
                        code = "BU";
                    }
                }
                else if (value.length == 10) {
                    var val = value.substr(4, 1);
                    if (val == "M" || val == "B") {
                        code = "MBSno";
                    }
                }
                else if (value.length == 11) {
                    var checkCode = value.substr(5, 1);
                    if (checkCode == "M" || checkCode == "B") {
                        code = "MBSno";
                       
                    } else {
                        checkCode = value.substr(4, 1);
                        if (checkCode == "M" || checkCode == "B") {
                            code = "MBSno";
                            value = value.substr(0, 10);
                        }
                    }
                }
                return code;
            }
            catch (e) {
                alert(e.description);
            }
        }

        function playSound() {
            ShowSuccessfulInfo(flagSound, '[' + document.getElementById("<%=hidInput.ClientID%>").value + ']' + SUCCESSRET);
            document.getElementById("<%=hidInput.ClientID%>").value = "";
        }

        function playSound() {
            ShowSuccessfulInfo(flagSound, '[' + document.getElementById("<%=hidInput.ClientID%>").value + ']' + SUCCESSRET);
            document.getElementById("<%=hidInput.ClientID%>").value = "";
        }

        function callNextInput() {
            getCommonInputObject().focus();
            getCommonInputObject().select();
            getAvailableData("input");
        }

        function Reset() {
            inputFlag = false;
            setInputOrSpanValue(document.getElementById("<%=txtFamily.ClientID%>"), "")
            document.getElementById("<%=hidIsBU.ClientID%>").value = "";
            IsBU = "";
            setInputOrSpanValue(document.getElementById("<%=txtMBSno.ClientID%>"), "")
            
            
        }

        window.onbeforeunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
        
        window.onunload = function() {
            document.getElementById("<%=btnExit.ClientID%>").click();
        }
    </script>

</asp:Content>
