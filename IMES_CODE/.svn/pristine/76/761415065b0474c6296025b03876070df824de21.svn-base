<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="ExecSQL.aspx.cs" Inherits="CommonFunction_ExecSQL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/CommonFunction/Service/ExecSPWebService.asmx" InlineScript="true" />
        </Services>
    </asp:ScriptManager>
    <div style="width: 95%; margin: 0 auto;">
        <table width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 25%;" />
                <col style="width: 50%;" />
                <col style="width: 25%;" />
            </colgroup>
            <tr>
                <td>
                    <label id="labelDB">
                        DataBase Name:</label>
                </td>
                <td>
                    <label id="InputDB">
                    </label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <input type="button" id="BtnQuery" onclick="Query()" style="width: 120px;" value="Execute" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
        <fieldset id="FsInput" style="height: auto; width: 95%;">
            <legend id="LegendInput" style="font-weight: bold; color: Blue">Input SQL</legend>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td>
                        <textarea id="InputSQL" rows="6" cols="80" style="width: 99%; font-size: large; color: Blue;
                            overflow: scroll;"></textarea>
                    </td>
                </tr>
            </table>
        </fieldset>
        <hr />
        <fieldset id="Fieldset1" style="height: auto; width: 95%;">
            <legend id="Legend1" style="font-weight: bold; color: Blue">Query Result</legend>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="HiddenDisplayGV" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                    <table width="100%" border="0" style="table-layout: fixed;">
                        <tr>
                            <td style="width: 35%">
                                <label id="label1">
                                    Result Records:</label>
                                <asp:Label ID="LabelResultCount" runat="server" ForeColor="Green">0</asp:Label>
                            </td>
                            <td>
                                <label id="label2">
                                    Display Records:</label>
                                <asp:Label ID="LabelDisplayCount" runat="server" ForeColor="Blue">0</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true"
                                    GvExtWidth="98%" GvExtHeight="176px" Width="98%" SetTemplateValueEnable="true"
                                    GetTemplateValueEnable="true" HighLightRowPosition="3" HorizontalAlign="Left">
                                </iMES:GridViewExt>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <input id="HiddenDB" type="hidden" runat="server" />
        <input id="HiddenSQLText" type="hidden" runat="server" />
        <input type="text" id="HidderDealWithRefresh" visible="false" style="position: absolute;
            top: -6688px;" />
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <button id="HiddenDisplayGV" runat="server" type="button" onclick="" style="display: none"
                    onserverclick="DisplayResultGV">
                </button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        var ParameterNamesArray = new Array();
        var ParameterValuesArray = new Array();
        var WarnArray = new Array();
        var ErrorArray = new Array();
        var db;
        window.onload = function() {
            var AllParameters = GetAllPrarmeters();
            db = GetParameterByKey(AllParameters, "db");
            document.getElementById("InputDB").innerText = db;
            for (var i = 0; i < AllParameters.length; i++) {
                if (AllParameters[i][0].toUpperCase() == "WARN") {
                    WarnArray.push(AllParameters[i][1]);
                } else if (AllParameters[i][0].toUpperCase() == "ERROR") {
                    ErrorArray.push(AllParameters[i][1]);
                }
            }
        };

        function GetAllPrarmeters() {
            var AllParameters = new Array();
            var UrlParameters = window.location.search;
            if (UrlParameters.length > 1) {
                UrlParameters = UrlParameters.substring(1);

                var KeyValueArray = UrlParameters.split("&");

                for (i = 0; i < KeyValueArray.length; i++) {
                    AllParameters[i] = KeyValueArray[i].split("=");
                }
            }

            return AllParameters;
        }

        function GetParameterByKey(AllParameters, key) {
            var ParametersLength = AllParameters.length;
            for (i = 0; i < ParametersLength; i++) {
                if (AllParameters[i][0].toUpperCase() == key.toUpperCase()) {
                    return AllParameters[i][1];
                }
            }
            return "";
        }

        function Query() {
            document.getElementById("BtnQuery").disabled = true;
            var SQLText = document.getElementById("InputSQL").value.trim();
            if (!SQLText) {
                alert("Please input SQL!");
                document.getElementById("BtnQuery").disabled = false;
                return false;
            }
            var selectSQLText = GetSelectText();
            if (selectSQLText) {
                SQLText = selectSQLText;
            }
            try {
                for (var i = 0; i < ErrorArray.length; i++) {
                    if (ErrorArray[i].substr(0, 1) == "!") {
                        var re = new RegExp(ErrorArray[i].substr(1), "ig");
                        if (!re.test(SQLText)) {
                            alert("Error," + ErrorArray[i].substr(1) + " is need!");
                            document.getElementById("BtnQuery").disabled = false;
                            return false;
                        }
                    } else {
                        var re = new RegExp(ErrorArray[i], "ig");
                        if (re.test(SQLText)) {
                            alert("Error," + ErrorArray[i] + " is not allowed!");
                            document.getElementById("BtnQuery").disabled = false;
                            return false;

                        }
                    }
                }

                for (var i = 0; i < WarnArray.length; i++) {
                    if (WarnArray[i].substr(0, 1) == "!") {
                        var re = new RegExp(WarnArray[i].substr(1), "ig");
                        if (!re.test(SQLText)) {
                            if (!confirm(WarnArray[i].substr(1) + " is need!")) {
                                document.getElementById("BtnQuery").disabled = false;
                                return false;
                            }
                        }
                    } else {
                        var re = new RegExp(WarnArray[i].substr(1), "ig");
                        if (re.test(SQLText)) {
                            if (!confirm(WarnArray[i] + " is not allowed!")) {
                                document.getElementById("BtnQuery").disabled = false;
                                return false;
                            }
                        }
                    }
                } 
            } catch (e) { alert(e); }

            document.getElementById('<%=HiddenDB.ClientID%>').value = db;
            document.getElementById('<%=HiddenSQLText.ClientID%>').value = SQLText;
            document.getElementById('<%=HiddenDisplayGV.ClientID%>').click();
            return true;
        }

        function GetSelectText() {
            var TextObj = document.getElementById("InputSQL");
            TextObj.focus();
            if (typeof document.selection != "undefined") {
                return document.selection.createRange().text;
            } else {
                return TextObj.value.substr(TextObj.selectionStart, TextObj.selectionEnd - TextObj.selectionStart);
            }
        }

    </script>

</asp:Content>
