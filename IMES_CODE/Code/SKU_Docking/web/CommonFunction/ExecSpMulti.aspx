<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="ExecSpMulti.aspx.cs" Inherits="CommonFunction_ExecSpMulti" %>

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
                    <input type="button" id="BtnQuery" onclick="Query()" style="width: 120px;" value="Batch Execute" />
                </td>
            </tr>
        </table>
        <br />
        <table id="SingleParametersTable" width="100%" border="0" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 25%;" />
                <col style="width: 75%;" />
            </colgroup>
        </table>
        <br />
        <fieldset id="FsInput" style="height: auto; width: 95%;">
            <legend id="LegendInput" style="font-weight: bold; color: Blue">Input Parameters</legend>
            <table id="SPParametersTable" width="100%" border="0" style="table-layout: fixed;">
            </table>
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td>
                        <textarea id="InputParametersArea" rows="6" cols="80" style="width: 99%; font-size: large;
                            color: Blue;overflow: scroll;"></textarea>
                    </td>
                </tr>
            </table>
        </fieldset>
        <hr />
        <table width="100%" border="0" style="table-layout: fixed;">
            <tr>
                <td>
                    <fieldset id="FSMessage" style="height: auto; width: 95%;">
                        <legend id="LegendMessage" style="font-weight: bold; color: Blue">Message</legend>
                        <textarea id="DisplayResut" rows="6" cols="80" style="width: 99%; font-size: large;
                            color: Green; overflow: scroll;" readonly="readonly"></textarea>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var ParameterNamesArray = new Array();
        var SingleNamesArray = new Array();
        var MultiNamesArray = new Array();
        var editor;
        var db;
        var sp;
        var NeedEditor;
        var ResultMessage = "";
        window.onload = function() {
            editor = "<%=Editor%>";
            var AllParameters = GetAllPrarmeters();
            
            db = GetParameterByKey(AllParameters, "db");
            sp = GetParameterByKey(AllParameters, "sp");
 
            document.getElementById("InputDB").innerText = db;
            document.getElementById("InputSP").innerText = sp;
            var ParametersLength = AllParameters.length;

            var SingleParametersTableObj = document.getElementById("SingleParametersTable");
            for (var i = 0; i < ParametersLength; i++) {
                var ParameterName = GetParameterByKey(AllParameters, "s" + i.toString());
                if (ParameterName) {
                    SingleNamesArray.push(ParameterName);
                    ParameterNamesArray.push(ParameterName);
                    var newRow = SingleParametersTableObj.insertRow();
                    var newCell0 = newRow.insertCell(0);
                    newCell0.innerHTML = "<label id='label" + ParameterName + "'>" + ParameterName + ":</label>";
                    var temParameterValues = new Array();
                    for (var v = 0; v < ParametersLength; v++) {
                        var tempParameterValue = GetParameterByKey(AllParameters, "s" + i.toString() + "v" + v.toString());
                        if (tempParameterValue) {
                            temParameterValues.push(tempParameterValue);
                        }
                    }
                    if (temParameterValues.length == 0) {
                        var newCell1 = newRow.insertCell(1);
                        newCell1.innerHTML = "<input id='Input" + ParameterName + "' type='text' style='width:86%;' />";
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

            var SPParametersTableObj = document.getElementById("SPParametersTable");
            var newRow = SPParametersTableObj.insertRow();
            for (var i = 0; i < ParametersLength; i++) {
                var ParameterName = GetParameterByKey(AllParameters, "p" + i.toString());
                if (ParameterName) {
                    MultiNamesArray.push(ParameterName);
                    ParameterNamesArray.push(ParameterName);
                    var newCell0 = newRow.insertCell();
                    newCell0.innerText = ParameterName;
                }
            }

            NeedEditor = GetParameterByKey(AllParameters, "NeedEditor");
            if (NeedEditor=="1") {
                ParameterNamesArray.push("Editor");
            }

            if (MultiNamesArray.length == 0) {
                document.getElementById("InputParametersArea").disabled=true;
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
            if (MultiNamesArray.length > 0) {
                var CurrentParameterValues = document.getElementById("InputParametersArea").value.trim();
                if (CurrentParameterValues=="") {
                    alert("Please input Parameters");
                    document.getElementById("BtnQuery").disabled = false;
                    return;
                }
            }
            document.getElementById("DisplayResut").value = "正在执行，请稍候..., Procedure is running, please wait...";
            QueryLoop();

        }

        function QueryLoop() {
            var ParameterValuesArray = new Array();
            for (var i = 0; i < SingleNamesArray.length; i++) {
                var tempValue = document.getElementById("Input" + SingleNamesArray[i]).value.trim();
                ParameterValuesArray.push(tempValue);
            }
            
            if (MultiNamesArray.length > 0) {
                var CurrentParameterValues = document.getElementById("InputParametersArea").value.trim();
                if (CurrentParameterValues) {
                    var AllParametersArray = CurrentParameterValues.split("\r\n");
                    var FirstLineParameters = AllParametersArray[0].split("\t");
                    if (MultiNamesArray.length != FirstLineParameters.length) {
                        alert("The count of Parameters should be " + MultiNamesArray.length);
                        document.getElementById("BtnQuery").disabled = false;
                        return;
                    }

                    for (var k = 0; k < FirstLineParameters.length; k++) {
                        ParameterValuesArray.push(FirstLineParameters[k]);
                    }
                } else {
                    alert("Please input Parameters");
                    document.getElementById("BtnQuery").disabled = false;
                    return;
                }
            }

            if (NeedEditor) {
                ParameterValuesArray.push(editor);
            }
            ExecSPWebService.GetSPResult(editor,db, sp, ParameterNamesArray, ParameterValuesArray, OnQuerySucceed);
            
        }

        function OnQuerySucceed(result) {

            if (result && result.length == 2) {
                if (result[0].toUpperCase() != "SUCCESS") {
                    document.getElementById("DisplayResut").style.color = "Red";
                    ShowMessage(result[1], true);
                    document.getElementById("DisplayResut").value = document.getElementById("DisplayResut").value + "\r\n" + result[1];
                    document.getElementById("BtnQuery").disabled = false;
                } else {
                    var CurrentParameterValues = document.getElementById("InputParametersArea").value.trim();

                    if (CurrentParameterValues) {
                        var AllParametersArray = CurrentParameterValues.split("\r\n");
                        CurrentParameterValues = CurrentParameterValues.substring(AllParametersArray[0].length + 2);
                        document.getElementById("InputParametersArea").value = CurrentParameterValues;
                    }
                    document.getElementById("DisplayResut").value = document.getElementById("DisplayResut").value + "\r\n" + result[1];
                    if (CurrentParameterValues) {
                        QueryLoop();
                    } else {
                        document.getElementById("DisplayResut").value = document.getElementById("DisplayResut").value + "\r\n" + "执行完毕， Batch Execute finished!";
                        document.getElementById("BtnQuery").disabled = false;
                    }
                    document.getElementById("DisplayResut").style.color = "Green";
                }
            }

        }


    </script>

</asp:Content>
