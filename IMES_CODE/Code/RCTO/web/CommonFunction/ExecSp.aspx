<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="ExecSp.aspx.cs" Inherits="CommonFunction_ExecSp" %>

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
                </td>
            </tr>
            <tr>
                <td>
                    <label id="labelSPName">
                        SP Name:</label>
                </td>
                <td>
                    <label id="InputSP">
                    </label>
                </td>
                <td>
                    <input type="button" id="BtnQuery" onclick="Query()" style="width: 120px;" value="Execute" />
                </td>
            </tr>
        </table>
        <br />
        <table id="SPParametersTable" width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 25%;" />
                <col style="width: 75%;" />
            </colgroup>
        </table>
        <br />
        <table width="100%" border="0" style="table-layout: fixed;">
            <tr>
                <td>
                    <fieldset id="FSMessage" style="height: auto; width: 95%;">
                        <legend id="LegendMessage" style="font-weight: bold; color: Blue">Message</legend>
                        <textarea id="DisplayResut" rows="6" cols="80" style="width: 99%; font-size: large;
                            color: Green;" readonly="readonly"></textarea>
                    </fieldset>
                </td>
            </tr>
        </table>
        <input type="text" id="HidderDealWithRefresh" visible="false" style="position: absolute;
            top: -6688px;" />
    </div>

    <script language="javascript" type="text/javascript">
        var ParameterNamesArray = new Array();
        var ParameterValuesArray = new Array();
        var editor;
        var db;
        var sp;
        var NeedEditor;
        window.onload = function() {
            editor = "<%=Editor%>";
            var AllParameters = GetAllPrarmeters();
            db = GetParameterByKey(AllParameters, "db");
            sp = GetParameterByKey(AllParameters, "sp");
            document.getElementById("InputDB").innerText = db;
            document.getElementById("InputSP").innerText = sp;
            var SPParametersTableObj = document.getElementById("SPParametersTable");
            var ParametersLength = AllParameters.length;
            var LastInputId;
            for (var i = 0; i < ParametersLength; i++) {
                var ParameterName = GetParameterByKey(AllParameters, "p" + i.toString());
                if (ParameterName) {
                    ParameterNamesArray.push(ParameterName);
                    var newRow = SPParametersTableObj.insertRow();
                    var newCell0 = newRow.insertCell(0);
                    newCell0.innerHTML = "<label id='label" + ParameterName + "'>" + ParameterName + ":</label>";
                    var temParameterValues = new Array();
                    for (var v = 0; v < ParametersLength; v++) {
                        var tempParameterValue = GetParameterByKey(AllParameters, "p" + i.toString() + "v" + v.toString());
                        if (tempParameterValue) {
                            temParameterValues.push(tempParameterValue);
                        }
                    }
                    if (temParameterValues.length == 0) {
                        var newCell1 = newRow.insertCell(1);
                        newCell1.innerHTML = "<input id='Input" + ParameterName + "' type='text' style='width:86%;' />";
                        LastInputId = "Input" + ParameterName;
                    } else {
                        var newCell1 = newRow.insertCell(1);
                        var selectHtml = "<select id='Input" + ParameterName + "' style='width:86%;'>";
                        for (var j = 0; j < temParameterValues.length; j++) {
                            selectHtml = selectHtml + "<option value='" + temParameterValues[j] + "'>" + temParameterValues[j] + "</option>";
                        }
                        selectHtml = selectHtml + "</select>";
                        newCell1.innerHTML = selectHtml;

                    }
                }
            }
            if (LastInputId) {
                document.getElementById(LastInputId).attachEvent("onkeydown", InputKeyDown);
            }

            NeedEditor = GetParameterByKey(AllParameters, "NeedEditor");
            if (NeedEditor == "1") {
                ParameterNamesArray.push("Editor");
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

        function InputKeyDown() {
            if (event.keyCode == 9 || event.keyCode == 13) {
                Query();
            }
        }

        function Query() {
            document.getElementById("BtnQuery").disabled = true;
            ParameterValuesArray = new Array();

            var NoEditorLength = ParameterNamesArray.length;
            if (NeedEditor == "1") {
                NoEditorLength = NoEditorLength - 1;
            }
            for (var i = 0; i < NoEditorLength; i++) {
                var tempValue = document.getElementById("Input" + ParameterNamesArray[i]).value.trim();
                ParameterValuesArray.push(tempValue);
            }

            if (NeedEditor == "1") {
                ParameterValuesArray.push(editor);
            } 
            ExecSPWebService.GetSPResult(editor, db, sp, ParameterNamesArray, ParameterValuesArray, OnQuerySucceed);
            document.getElementById("DisplayResut").value = "正在执行，请稍候..., Procedure is running, please wait...";
        }

        function OnQuerySucceed(result) {
            document.getElementById("BtnQuery").disabled = false;

            var NoEditorLength = ParameterNamesArray.length;
            if (NeedEditor == "1") {
                NoEditorLength = NoEditorLength - 1;
            }
            for (var i = 0; i < NoEditorLength; i++) {
                document.getElementById("Input" + ParameterNamesArray[i]).value = "";
            }
            
            if (result && result.length == 2) {
                if (result[0].toUpperCase() != "SUCCESS") {
                    document.getElementById("DisplayResut").style.color = "Red";
                    ShowMessage(result[1], true);
                } else {
                    document.getElementById("DisplayResut").style.color = "Green";
                }
                document.getElementById("DisplayResut").value = result[1];
            }

        }


    </script>

</asp:Content>
