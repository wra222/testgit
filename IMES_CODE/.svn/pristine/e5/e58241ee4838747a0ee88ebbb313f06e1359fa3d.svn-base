<%--
 INVENTEC corporation (c)2008 all rights reserved. 
 Description: model info maintain
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2010-11-10  98079                Create 
 Known issues: ITC-1361-0132

 --%>

<%@ Page Language="C#" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true"
    CodeFile="RuleSetting.aspx.cs" Inherits="RuleSetting" Title="" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <style>
        table.edit
        {
            border: thin solid Black;
            background-color: #99CDFF;
        }
        .bStyle
        {
            font-size: 12pt;
            font-family: Verdana;
            font-weight: bold;
        }
    </style>
</head>
<body style="position: relative; width: 100%">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container">
        <table border="0">
            <tr>
                <td height="5px">
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" width="100%">
                        <tr>
                            <td align="left" class="bStyle" style="width: 120px;">
                                <asp:Label ID="lblLabelType" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="left" class="bStyle">
                                <asp:Label ID="valLabelType" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="right">
                                <input id="btnExit" type="button" runat="server" class="iMes_button" onclick="onExit()"
                                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="bStyle">
                                <asp:Label ID="lblTemplate" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="left" colspan="2" class="bStyle">
                                <asp:Label ID="valTemplate" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10px">
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0">
                        <tr>
                            <td align="left" valign="top" class="bStyle" style="width: 120px;">
                                <asp:Label ID="lblRules" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                            </td>
                            <td align="left" class="bStyle">
                                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd1" EventName="ServerClick" />
                                        <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="ServerClick" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:ListBox ID="lstRules" Style="width: 150px; height: 100px;" size="4" runat="server"
                                            CssClass="iMes_label_13pt" Font-Size="12pt"></asp:ListBox>
                                        <input id="hidRuleId" type="hidden" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 20px">
                            </td>
                            <td align="center" class="bStyle">
                                <input id="btnAdd1" type="button" runat="server" class="iMes_button" onclick="" onserverclick="btnAdd1_Click"
                                    onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                                <br>
                                <input id="btnDelete1" disabled type="button" runat="server" class="iMes_button"
                                    onclick="if(clikDeleteRule())" onserverclick="btnDelete1_Click" onmouseover="this.className='iMes_button_onmouseover'"
                                    onmouseout="this.className='iMes_button_onmouseout'" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10px">
                </td>
            </tr>
            <tr>
                <td align="left" class="bStyle">
                    <asp:Label ID="lblRuleSettingItems" runat="server" CssClass="iMes_label_13pt" Font-Size="12pt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave2" EventName="ServerClick" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete2" EventName="ServerClick" />
                            <asp:AsyncPostBackTrigger ControlID="btnRefreshRuleSettingList" EventName="ServerClick" />
                        </Triggers>
                        <ContentTemplate>
                            <iMES:GridViewExt ID="gdRuleSettingList" runat="server" AutoGenerateColumns="true"
                                Width="100%" GvExtWidth="100%" GvExtHeight="120px" Height="110px" OnRowDataBound="gd_RowDataBound"
                                OnGvExtRowClick="clickTable(this)" SetTemplateValueEnable="False" HighLightRowPosition="3"
                                AutoHighlightScrollByValue="True">
                            </iMES:GridViewExt>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <button id="btnRefreshRuleSettingList" runat="server" type="button" style="display: none"
                        onserverclick="btnRefreshRuleSettingList_Click">
                    </button>
                    <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Always" Visible="false">
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td height="8px">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <fieldset class="myFieldsetStyle">
                        <legend class="myLegend">Rule Setting Item</legend>
                        <table width="100%" border="0">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="lblMode" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td width="35%">
                                    <asp:DropDownList ID="selMode" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="lblAttribute" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td colspan="2" width="40%">
                                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnReloadCmbAttribute" EventName="ServerClick" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <iMESMaintain:CmbAttributeForMaintain ID="cmbAttribute" runat="server" Width="150"
                                                IsPercentage="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="lblValue" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                                </td>
                                <td colspan="2" width="55%">
                                    <asp:TextBox ID="txtValue" MaxLength="50" runat="server" SkinID="textBoxSkin" Width="95%"></asp:TextBox>
                                </td>
                                <td width="15%">
                                    <input id="btnSave2" type="button" runat="server" class="iMes_button" onclick="if(clkSave2())"
                                        onserverclick="btnSave2_Click" onmouseover="this.className='iMes_button_onmouseover'"
                                        onmouseout="this.className='iMes_button_onmouseout'" align="right" />
                                </td>
                                <td width="15%" style="">
                                    <input id="btnDelete2" type="button" runat="server" class="iMes_button" onclick="if(clkDelete())"
                                        onserverclick="btnDelete2_Click" onmouseover="this.className='iMes_button_onmouseover'"
                                        onmouseout="this.className='iMes_button_onmouseout'" align="right" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
    <input id="hidMode" type="hidden" runat="server" />
    <input id="hidAttribute" type="hidden" runat="server" />
    <input id="hidRuleSettingItemId" type="hidden" runat="server" />
    <button id="btnReloadCmbAttribute" runat="server" type="button" style="display: none"
        onserverclick="btnReloadCmbAttribute_Click">
    </button>
    <iMES:WaitingCoverDiv ID="divCover" runat="server" KeyDownFun="KeyDownEvent()" />
    </form>
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

    <script type="text/javascript">
        var msg1;
        var msg2;
        window.onload = function() {
            msg1 = "<%=pmtMessage1 %>";
            msg2 = "<%=pmtMessage2 %>";
            setNewItemValue();
            document.getElementById("<%=btnSave2.ClientID%>").disabled = true;
            initControl();
        }
        function initControl() {
            document.getElementById("<%=lstRules.ClientID%>").onchange = lstRules_OnSelectedIndexChanged;
            document.getElementById("<%=selMode.ClientID%>").onchange = selMode_OnSelectedIndexChanged;
        }
        var msgDelete = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgDelete").ToString() %>';
        var msgSave2Value = '<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_msgSave2Value").ToString() %>';
        var pmtMessage3 = "<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_pmtMessage3").ToString() %>";
        var pmtMessage4 = "<%=this.GetLocalResourceObject(WebCommonMethod.getConfiguration(WebConstant.LANGUAGE) + "_pmtMessage4").ToString() %>";
        var selectedRowIndex_Item = null;

        function onExit() {
            window.close();
        }

        function lstRules_OnSelectedIndexChanged() {
            var objLstRules = document.getElementById("<%=lstRules.ClientID%>");

            document.getElementById("<%=hidRuleId.ClientID%>").value = objLstRules.options[objLstRules.selectedIndex].value;
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = false;


            document.getElementById("<%=hidMode.ClientID%>").value = "";
            document.getElementById("<%=hidAttribute.ClientID%>").value = "";
            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidRuleSettingItemId.ClientID%>").value = "";
            document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;
            document.getElementById("<%=btnSave2.ClientID%>").disabled = false;

            document.getElementById("<%=btnRefreshRuleSettingList.ClientID%>").click();

        }

        function selMode_OnSelectedIndexChanged() {
            var objSelMode = document.getElementById("<%=selMode.ClientID%>");

            document.getElementById("<%=hidMode.ClientID%>").value = objSelMode.value;

            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidAttribute.ClientID%>").value = "";

            document.getElementById("<%=btnReloadCmbAttribute.ClientID%>").click();

        }


        function clkDelete() {
            ShowWait();
            var objSelMode = document.getElementById("<%=selMode.ClientID%>");

            if (objSelMode.value == "") {
                alert(msg2);
            }
            if (confirm(msgDelete)) {
                DealHideWait();
                return true;
            }
            DealHideWait();
            return false;

        }

        function clikDeleteRule() {
            if (confirm(msg1)) {
                DealHideWait();
                return true;
            }
            DealHideWait();
            return false;
        }


        function clkSave2() {
            ShowWait();
            var value1 = document.getElementById("<%=selMode.ClientID %>").value;
            if (value1.trim() == "") {
                alert(pmtMessage3);
                DealHideWait();
                return false;
            }

            var value2 = getMyAttributeCmbObj().value;
            if (value2.trim() == "") {
                alert(pmtMessage4);
                DealHideWait();
                return false;
            }
            
            var value = document.getElementById("<%=txtValue.ClientID %>").value;
            if (value.trim() == "") {
                alert(msgSave2Value);
                DealHideWait();
                return false;
            }
            return true;

        }

        function Add1Complete(id) {
            DealHideWait();
            var objLstRules = document.getElementById("<%=lstRules.ClientID%>");
            objLstRules.value = id;
            lstRules_OnSelectedIndexChanged();
            objLstRules.onchange = lstRules_OnSelectedIndexChanged;
            document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;
        }

        function Delete1Complete() {
            DealHideWait();
            document.getElementById("<%=btnDelete1.ClientID%>").disabled = true;
            document.getElementById("<%=hidRuleId.ClientID%>").value = "";
            document.getElementById("<%=btnRefreshRuleSettingList.ClientID%>").click();
            var objLstRules = document.getElementById("<%=lstRules.ClientID%>");
            objLstRules.onchange = lstRules_OnSelectedIndexChanged;
            document.getElementById("<%=btnSave2.ClientID%>").disabled = true;
            document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;
            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidRuleSettingItemId.ClientID%>").value = "";
            document.getElementById("<%=hidMode.ClientID%>").value = "";
            document.getElementById("<%=hidAttribute.ClientID%>").value = "";

        }

        function clickTable(row) {
            //selectedRowIndex = parseInt(con.index, 10);
            if ((selectedRowIndex_Item != null) && (selectedRowIndex_Item != parseInt(row.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(selectedRowIndex_Item, false, "<%=gdRuleSettingList.ClientID %>");
            }

            setRowSelectedOrNotSelectedByIndex(row.index, true, "<%=gdRuleSettingList.ClientID %>");
            selectedRowIndex_Item = parseInt(row.index, 10);
            //labeltype
            if (row.cells[0].innerText.trim() == "") {
                document.getElementById("<%=hidRuleSettingItemId.ClientID%>").value = "";
                document.getElementById("<%=selMode.ClientID%>").value = "";
                document.getElementById("<%=hidMode.ClientID%>").value = "";
                document.getElementById("<%=hidAttribute.ClientID%>").value = "";
                document.getElementById("<%=txtValue.ClientID%>").value = "";
                document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;
                document.getElementById("<%=btnReloadCmbAttribute.ClientID%>").click();

            } else {
                document.getElementById("<%=hidRuleSettingItemId.ClientID%>").value = row.cells[0].innerText.trim();
                document.getElementById("<%=selMode.ClientID%>").value = row.cells[1].innerText.trim();
                document.getElementById("<%=hidMode.ClientID%>").value = row.cells[1].innerText.trim();
                document.getElementById("<%=hidAttribute.ClientID%>").value = row.cells[2].innerText.trim();
                document.getElementById("<%=btnReloadCmbAttribute.ClientID%>").click();
                document.getElementById("<%=txtValue.ClientID%>").value = row.cells[3].innerText.trim();
                document.getElementById("<%=btnDelete2.ClientID%>").disabled = false;


            }

        }

        function Save2Complete(ruleSetId) {
            DealHideWait();
            if (ruleSetId.length == 0) return;
            var gdRuleSettingListClientID = "<%=gdRuleSettingList.ClientID%>";
            var row = eval("setScrollTopForGvExt_" + gdRuleSettingListClientID + "('" + ruleSetId + "',0)");
            clickTable(row);
        }

        function Delete2Complete() {
            DealHideWait();
            document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;

            document.getElementById("<%=txtValue.ClientID%>").value = "";
            document.getElementById("<%=hidRuleSettingItemId.ClientID%>").value = "";
            document.getElementById("<%=hidMode.ClientID%>").value = "";
            document.getElementById("<%=hidAttribute.ClientID%>").value = "";

        }
        function DealHideWait() {
            HideWait();
        }
        function setNewItemValue() {
            document.getElementById("<%=btnSave2.ClientID%>")
            document.getElementById("<%=btnSave2.ClientID%>").disabled = true;
            document.getElementById("<%=btnDelete1.ClientID %>").disabled = true;
            document.getElementById("<%=btnDelete2.ClientID%>").disabled = true;

        }

    </script>

</body>
</html>
