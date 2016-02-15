<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true" CodeFile="NoneShipProductQuery.aspx.cs" Inherits="Query_NoneShipProductQuery" %>
<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../../js/jquery-1.7.1.js "></script>    
    <script type="text/javascript" src="../../CommonControl/JS/Browser.js"></script>
    <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../../js/jquery-ui-1.8.13.custom.min.js "></script>     
    <script src="../../js/jscal2.js "></script>
    <script src="../../js/lang/cn.js "></script>    

    <script type="text/javascript" src="../../js/jquery.multiselect.js "></script>     
    <script type="text/javascript"  src="../../js/jquery.multiselect.filter.js "></script>     
    <script type="text/javascript" src="../../js/wz_tooltip.js "></script>
        
    
    <link rel="stylesheet" type="text/css" href="../../css/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="../../css/jscal2.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.multiselect.filter.css" />                        
    
    <script type="text/javascript"></script>
                     
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <center>
            <fieldset style="border: solid #000000">
                <legend>None Ship Product Query</legend>
                <table border="0" width="100%" style="border-width:thin;">
                    <tr>
                        <td width ="10%">
                            <asp:Label ID="lblDB" runat="server" Text="DBName:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td width="30%">
                            <iMESQuery:CmbDBType ID="CmbDBType" runat="server" />
                        </td>
                        <td width="10%"></td>
                        <td width="30%"></td>
                        <td width="10%"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblModel" runat="server" Text="Model:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="txtModel" style="width: 300px" maxlength="14" class="iMes_textbox_input_Yellow" />
                            <input id="BtnBrowse" type="button" value="Browse"  onclick="UploadModelList()" style="display:none" />               
                            <asp:HiddenField ID="hfModel" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="PdLine:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:ListBox ID="lboxPdLine" runat="server" SelectionMode="Multiple" Height="95%" Width="200px" CssClass="CheckBoxList"></asp:ListBox>
                            <asp:HiddenField ID="hfLineShife" value="true" runat="server" />
                        </td>
                        <td>
                            <button id="btnQuery"  runat="server" onserverclick="btnQuery_Click" onclick="if(Query())" style="width: 100px">Query</button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Station:" CssClass="iMes_label_13pt"></asp:Label>
                        </td>
                        <td>
                            <asp:ListBox ID="lboxStation" runat="server" SelectionMode="Multiple" Height="95%" Width="200px" CssClass="CheckBoxList"></asp:ListBox>
                            <asp:HiddenField ID="hfStationShife" value="true" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td>
                            <button id="btnExport"  runat="server" onserverclick="btnExport_Click" style="width: 100px; display: none;">Export</button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>        
                    <iMES:GridViewExt ID="gvQuery" runat="server" AutoGenerateColumns="true" GvExtHeight="420px"
                                        Width="98%" GvExtWidth="98%" Height="1px" style="top: 0px; left: 0px" onrowdatabound="gvQuery_RowDataBound" >
                    </iMES:GridViewExt>
                    <asp:LinkButton ID="lbtFreshPage" runat="server" OnClientClick="return ReloadImesPage();" onclick="lbtFreshPage_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <asp:HiddenField ID="hidUser" runat="server" />       
        <asp:HiddenField ID="hidprocess" runat="server" />              
        <asp:HiddenField ID="hidsource" runat="server" />   
        <asp:HiddenField ID="hidModelList" runat="server" />   
        <asp:HiddenField ID="hidPdLineList" runat="server" />   
        <asp:HiddenField ID="hidpdLine" runat="server" />
        <asp:HiddenField ID="hidmodel" runat="server" />
        <asp:HiddenField ID="hidstationList" runat="server" />
    </div>


    <script language="javascript" type="text/javascript">

        var inputObj;        
        function processFun(backData) {
            ShowInfo("");
            beginWaitingCoverDiv();            
            document.getElementById("<%=btnQuery.ClientID%>").click();
        }

        function initPage() {
            clearData();
            inputObj.value = "";
            getAvailableData("processFun");
            inputObj.focus();
        }

        function setCommonFocus() {
            endWaitingCoverDiv();
            inputObj.focus();
            inputObj.select();
            window.onload();
        }

        function EndRequestHandler(sender, args) {
            $('.CheckBoxList').multiselect({ selectedList: 1, position: { my: 'left bottom', at: 'left top' }, noneSelectedText: 'Please Select ' }).multiselectfilter();

            $("#<%=gvQuery.ClientID%> tr:nth-child(n+2) ").click(function() { rowclick(); });
        };
        
        window.onload = function() {
            EndRequestHandler();
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        };
        
        function UploadModelList() {
            var dlgFeature = "dialogHeight:600px;dialogWidth:250px;center:yes;status:no;help:no;scroll:no";
            var saveasUrl = "UploadModelList.aspx?ModelList=" + document.getElementById("<%=hidModelList.ClientID %>").value;
            var dlgReturn = window.showModalDialog(saveasUrl, window, dlgFeature);
            if (dlgReturn) {
                dlgReturn = dlgReturn.replace(/\r\n/g, ",");
                document.getElementById("<%=hfModel.ClientID %>").value = RemoveBlank(dlgReturn);
            }
            else {
                if (dlgReturn == "")
                { document.getElementById("<%=hfModel.ClientID %>").value = ""; }
                return;
            }
        }

        function RemoveBlank(modelList) {
            var arr = modelList.split(",");
            var model = "";
            if (modelList != "") {
                for (var m in arr) {
                    if (arr[m] != "") {
                        model = model + arr[m] + ",";
                    }
                }
                model = model.substring(0, model.length - 1)
            }
            return model;
        }

        function Query() {
            var pdline = '';
            $("#ctl00_iMESContent_lboxPdLine option:selected").each(function() {
                pdline = pdline + $(this).val() + ',';
            });
            document.getElementById("<%=hidPdLineList.ClientID %>").value = pdline;
            var station = '';
            $("#ctl00_iMESContent_lboxStation option:selected").each(function() {
                station = station + $(this).val() + ',';
            });
            document.getElementById("<%=hidstationList.ClientID %>").value = station;
            var model = '';
            if (document.getElementById('txtModel').value != "") {
                document.getElementById("<%=hfModel.ClientID %>").value = document.getElementById('txtModel').value;
            }
            model = document.getElementById("<%=hfModel.ClientID %>").value;

            if (pdline == "" && station == "" && model == "") {
                alert("Line,Station,Model is not Emptry.");
                return false;
            }
           
            var inputList = [pdline, station, model];
            var checkFlag = 0;
            for (var i = 0; i < 3; i++) {
                if (inputList[i] != "") {
                    checkFlag += 1;
                }
            }
            if (checkFlag > 1) {
                alert("Model Station Line just Input One");
                return false;
            }
            beginWaitingCoverDiv();
            return true;
        }

        function cleanhfModel() {
            document.getElementById("<%=hfModel.ClientID %>").value = "";
            document.getElementById("<%=hidPdLineList.ClientID %>").value = "";
            document.getElementById("<%=hidstationList.ClientID %>").value = "";
        }
    </script>
    <script type="text/javascript">
    </script>
</asp:Content>