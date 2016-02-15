<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BTOceanOrder.aspx.cs" Inherits="DataMaintain_BTOceanOrder"
    MasterPageFile="~/MasterPageMaintain.master" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="content1" ContentPlaceHolderID="iMESContent" runat="server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js">
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" border="0">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblList" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width: 42%">
                        &nbsp;
                        <asp:TextBox ID="dSearch" runat="server" MaxLength="50" Width="56%" SkinID="textBoxSkin" onkeypress="OnKeyPress(this)"></asp:TextBox>
                    </td>
                    <td width="30%" align="right">
                        &nbsp;
                    </td>
                    <td width="10%" align="right">
                        <input type="button" runat="server" id="btnDelete" class="iMes_button" onserverclick="btnDelete_ServerClick"
                            onclick="if(clickDelete())" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 392px">
            <input type="hidden" id="hidRecordCount" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        RowStyle-Height="20" GvExtWidth="100%" GvExtHeight="392px" AutoHighlightScrollByValue="true"
                        HighLightRowPosition="3" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        EnableViewState="false" OnRowDataBound="gd_RowDataBound" Style="top: 0px; left: 23px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td width="90px">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="25%">
                        <div style="margin-left: 3px;">
                            <iMESMaintain:CmbStagePDLine ID="CmPdLine" runat="server" Width="120px" />
                        </div>
                    </td>
                    <td width="90px">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="dModel" runat="server" MaxLength="20" Width="95%" CssClass="iMes_textbox_input_Yellow"></asp:TextBox>
                    </td>
                    <td width="10%" align="center">
                        <input type="button" id="btnAdd" runat="server" class="iMes_button" onclick="if(clickAdd())"
                            onserverclick="btnAdd_serverClick" />
                    </td>
                    <td width="10%" align="center">
                        <input type="button" id="btnSave" runat="server" onclick="if(clickSave())" onserverclick="btnSave_serverClick"
                            class="iMes_button" />
                    </td>
                    <td width="10%" align="center">
                        <input type="button" runat="server" id="btnUpload" class="iMes_button" onclick="clickUpload()" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" Visible="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="hidBtnUpload" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input id="HiddenUsername" type="hidden" name="HiddenUsername" runat="server" />
        <input type="hidden" id="hidPdLine" runat="server" value="" />
        <input type="hidden" id="hidModel" runat="server" value="" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidFileName" runat="server" />
        <input type="button" id="hidBtnUpload" runat="server" onserverclick="btnUpload_serverClick"
            style="display: none" />
    </div>
    <div id="divWait" oselectid="" align="center" style="cursor: wait; filter: Chroma(Color=skyblue);
        background-color: skyblue; display: none; top: 0; width: 100%; height: 100%;
        z-index: 10000; position: absolute">
        <table style="cursor: wait; background-color: #FFFFFF; border: 1px solid #0054B9;
            font-size: 9pt; top: 30%; width: 190px; height: 25px; position: relative;">
            <tr>
                <td align="center">
                    <img alt="Please wait..." src="<%=Request.ApplicationPath%>/images/wait_animated.gif" />
                </td>
                <td align="center" style="color: #0054B9; font-size: 10pt; font-weight: bold;">
                    Please wait.....
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">

        window.onload = function() {
            msg1 = "<%=pmtMessage1 %>";
            msg2 = "<%=pmtMessage2 %>";
            msg3 = "<%=pmtMessage3 %>";
            msg4 = "<%=pmtMessage4 %>";
            msg5 = "<%=pmtMessage5 %>";
            ShowRowEditInfo(null);
            resetTableHeight();
        }
        //设置表格的高度  
        function resetTableHeight() {
            //动态调整表格的高度
            var adjustValue = 55;
            var marginValue = 12;
            var tableHeigth = 300;
            //var tableHeigth=document.body.parentElement.offsetHeight-div1.offsetHeight-div3.offsetHeight-adjustValue;
            try {
                tableHeigth = document.body.parentElement.offsetHeight - div1.offsetHeight - div3.offsetHeight - adjustValue;
            }
            catch (e) {

                //ignore
            }
            //为了使表格下面有写空隙
            var extDivHeight = tableHeigth + marginValue;
            div2.style.height = extDivHeight + "px";
            document.getElementById("div_<%=gd.ClientID %>").style.height = tableHeigth + "px";
            document.getElementById("<%=dTableHeight.ClientID %>").value = tableHeigth + "px";
        }
        function OnKeyPress(obj) {
            var key = event.keyCode;
            if (key == 13)//enter
            {
                if (event.srcElement.id == "<%=dSearch.ClientID %>") {
                    var value = document.getElementById("<%=dSearch.ClientID %>").value.trim().toUpperCase();
                    if (value != "") {
                        findBTOceanOrder(value, true);
                    }
                }
            }

        }
        function findBTOceanOrder(searchValue, isNeedPromptAlert) {
            var gdObj = document.getElementById("<%=gd.ClientID %>");
            searchValue = searchValue.toUpperCase();
            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (searchValue.trim() != "" && gdObj.rows[i].cells[1].innerText.toUpperCase().indexOf(searchValue) == 0) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                if (isNeedPromptAlert == true) {
                    alert(msg5);
                    //alert("找不到列表中与btoceanorder栏位匹配的项");     
                }
                else if (isNeedPromptAlert == null) {
                    ShowRowEditInfo(null);
                }
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }
        }
        function clickUpload() {
            var editor = document.getElementById("<%=HiddenUsername.ClientID %>").value;
            var pdLine = document.getElementById("<%=hidPdLine.ClientID %>").value;
            if (pdLine == "") {
                pdLine = getPdLineCmbObj().value;
            }
            var dlgFeature = "dialogHeight:235px;dialogWidth:424px;center:yes;status:no;help:no";

            editor = encodeURI(Encode_URL2(editor));
            var pdLine = getPdLineCmbObj().value;
            var dlgReturn = window.showModalDialog("BTOceanOrderUploadFile.aspx?pdLine=" + pdLine, window, dlgFeature);
            if (dlgReturn != null) {
                document.getElementById("<%=hidFileName.ClientID %>").value = dlgReturn;
                ShowWait();
                document.getElementById("<%=hidBtnUpload.ClientID%>").click();
            }
            return;

        }



        function NoMatchFamily() {
            alert(msg5);
            return;
        }

        function clickDelete() {
            if (iSelectedRowIndex == null) {
                alert(msg1);
                return false;
            }
            var ret = confirm(msg2);
            if (!ret) {
                return false;
            }
            ShowWait();
            return true;

        }

        function DeleteComplete() {
            DealHideWait();
            ShowRowEditInfo(null);
        }

        function NoMatchFamily() {
            alert(msg2);
            return;
        }

        function clickSave() {
            ShowWait();
            if (isNull() == false) {
                return false;
            }

            return true;
        }

        function clickAdd() {
            ShowWait();
            if (isNull() == false) {
                return false;
            }
            return true;
        }
        function isNull() {
            var descri = document.getElementById("<%=dModel.ClientID %>").value;
            if (descri.trim() == "") {
                DealHideWait();
                alert(msg4);
                return false;

            }
            return true;
        }
        function AddUpdateComplete(id, model) {
            DealHideWait();
            var gdObj = document.getElementById("<%=gd.ClientID %>");

            var selectedRowIndex = -1;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText == id && gdObj.rows[i].cells[1].innerText == model) {
                    selectedRowIndex = i;
                    break;
                }
            }

            if (selectedRowIndex < 0) {
                ShowRowEditInfo(null);
                return;
            }
            else {
                var con = gdObj.rows[selectedRowIndex];
                setGdHighLight(con);
                setSrollByIndex(iSelectedRowIndex, true, "<%=gd.ClientID%>");
                ShowRowEditInfo(con);
            }

        }

        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }
        var iSelectedRowIndex = null;
        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }
            document.getElementById("<%=dModel.ClientID %>").value = con.cells[1].innerText.trim();
            getPdLineCmbObj().value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidPdLine.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidModel.ClientID %>").value = con.cells[1].innerText.trim();
            var currentId = con.cells[0].innerText.trim();
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
                document.getElementById("<%=btnSave.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            document.getElementById("<%=dSearch.ClientID %>").value = "";
            getPdLineCmbObj().selectedIndex = 0;
            document.getElementById("<%=dModel.ClientID %>").value = ""
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
        }
        function alertMsg() {
            alert(msg6);
        }
    </script>

</asp:Content>
