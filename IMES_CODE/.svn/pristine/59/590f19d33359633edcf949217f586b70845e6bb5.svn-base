<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="RunningSQLStatement.aspx.cs" Inherits="CommonFunction_RunningSQLStatement" %>

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
                            <input type="button" id="BtnQuery" onclick="Query()" style="width: 120px;" value="sp_who3" />
                            <input type="button" id="btnOpenTran" onclick="OpenTran()" style="width: 120px;" value="sp_opentran" />
                            <input type="button" id="btnBlockSpid" onclick="BlockSpid()" style="width: 120px;" value="sp_who_block" />
                            <input type="button" id="btnExcel" onclick="ExportExcel()" disabled="disabled"  style="width: 120px;" value="Export Excel" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
        <%--<fieldset id="FsInput" style="height: auto; width: 95%;">
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
        <hr />--%>
        <fieldset id="Fieldset1" style="height: 500px; width: 95%;">
            
            
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="HiddenDisplayGV" EventName="ServerClick" />
                </Triggers>
                <ContentTemplate>
                <legend id="Legend1" style="font-weight: bold; color: Blue" runat="server" >Query Result</legend>
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
                                <iMES:GridViewExt ID="GridViewExt1" runat="server" 
                                    AutoGenerateColumns="true" 
                                    AutoHighlightScrollByValue="true"
                                    GvExtWidth="98%" GvExtHeight="450px" 
                                    Width="98%" SetTemplateValueEnable="true" 
                                    RowStyle-Height="20"
                                    GetTemplateValueEnable="true" 
                                    HighLightRowPosition="3" 
                                    HorizontalAlign="Left"
                                    OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'  
                                    OnGvExtRowDblClick="dblClickTable(this)"
                                    onrowdatabound="gd_RowDataBound">
                                </iMES:GridViewExt>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <input id="HiddenDB" type="hidden" runat="server" />
        <input id="HiddenSQLText" type="hidden" runat="server" />
        <input id="hidSQLMsg" type="hidden" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="text" id="HidderDealWithRefresh" visible="false" style="position: absolute;
            top: -6688px;" />
        
        <asp:UpdatePanel ID="updatePanel3" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <button id="hidbtnsession" runat="server" type="button" onclick="" style="display: none"
                    onserverclick="SetSession">
                </button>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <button id="HiddenDisplayGV" runat="server" type="button" onclick="" style="display: none"
                    onserverclick="DisplayResultGV">
                </button>
                
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <iframe name="exportframe" id="exportframe" src="ExportExcelForSQL.aspx" style="display:none;" ></iframe>
    <script language="javascript" type="text/javascript">
        var ParameterNamesArray = new Array();
        var ParameterValuesArray = new Array();
        var WarnArray = new Array();
        var ErrorArray = new Array();
        var db = "HPIMES";
        var SQLText;
        var Editor;
        window.onload = function() {
            var AllParameters = GetAllPrarmeters();
            db = GetParameterByKey(AllParameters, "db");
            Editor = GetParameterByKey(AllParameters, "editor");
            document.getElementById("InputDB").innerText = db;
            
            resetTableHeight();
        };

        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;

            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {
                //ignore
            }

            //为了使表格下面有些空隙
            var extDivHeight = tableHeigth + marginValue;
            //div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=GridViewExt1.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }
        

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

        function OpenTran() {
            document.getElementById("BtnQuery").disabled = true;
            document.getElementById("btnOpenTran").disabled = true;
            document.getElementById("btnBlockSpid").disabled = true;
            document.getElementById("btnExcel").disabled = false;
            SQLText = "Execute sp_opentran";
            document.getElementById('<%=HiddenDB.ClientID%>').value = db;
            document.getElementById('<%=HiddenSQLText.ClientID%>').value = SQLText;
            document.getElementById('<%=HiddenDisplayGV.ClientID%>').click();

            return true;
        }

        function BlockSpid() {
            document.getElementById("BtnQuery").disabled = true;
            document.getElementById("btnOpenTran").disabled = true;
            document.getElementById("btnBlockSpid").disabled = true;
            document.getElementById("btnExcel").disabled = false;
            SQLText = "Execute sp_who_block";
            document.getElementById('<%=HiddenDB.ClientID%>').value = db;
            document.getElementById('<%=HiddenSQLText.ClientID%>').value = SQLText;
            document.getElementById('<%=HiddenDisplayGV.ClientID%>').click();

            return true;
        }
        
        function Query() {
            document.getElementById("BtnQuery").disabled = true;
            document.getElementById("btnOpenTran").disabled = true;
            document.getElementById("btnBlockSpid").disabled = true;
            document.getElementById("btnExcel").disabled = false;
            SQLText = "Execute sp_who3";
            document.getElementById('<%=HiddenDB.ClientID%>').value = db;
            document.getElementById('<%=HiddenSQLText.ClientID%>').value = SQLText;
            document.getElementById('<%=HiddenDisplayGV.ClientID%>').click();
            return true;
        }

        function ExportExcel() {
            if (window.frames["exportframe"].dealExport != null) {
                var gv = document.getElementById("<%=GridViewExt1.ClientID %>")
                if (gv.rows[1].cells[0].innerText.trim() != "") {
                    window.frames["exportframe"].dealExport(db, SQLText, Editor);
                }
                else {
                    alert("沒有可以匯出的資料...");
                }
            }
            return;
        }

        function clickTable(con) {
            setGdHighLight(con);
            //ShowRowEditInfo(con);
        }

        var iSelectedRowIndex = null; 
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=GridViewExt1.ClientID %>");
            }

            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=GridViewExt1.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function dblClickTable(row) {
            var gridview = document.getElementById('<%=GridViewExt1.ClientID%>');
            var MsgInfoTODialog = "";
            if (row.cells[0].innerText.trim() != '') {
                for (var i = 0; i < row.cells.length; i++) {
                    var col = gridview.cells[i].firstChild.nodeValue;
                    var cell = row.cells[i].innerText.trim();
                    MsgInfoTODialog += col + ":" + cell + "^";
                }
                document.getElementById("<%=hidSQLMsg.ClientID %>").value = MsgInfoTODialog;
                document.getElementById('<%=hidbtnsession.ClientID%>').click();
            }
        }

        function ShowDialog() {
            var ret = window.showModalDialog("SQLInfoDialog.aspx?AccountId=111", 0, "dialogwidth:1000px; dialogheight:560px;status:no;help:no; ");
        }
    </script>

</asp:Content>
