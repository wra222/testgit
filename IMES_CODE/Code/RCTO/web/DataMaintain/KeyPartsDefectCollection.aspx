<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" ContentType="text/html;Charset=UTF-8"
    AutoEventWireup="true" CodeFile="KeyPartsDefectCollection.aspx.cs" Inherits="DataMaintain_KeyPartsDefectCollection"
    ValidateRequest="false" %>

<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">

    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container" class="iMes_div_MainTainContainer">
        <div id="div1" class="iMes_div_MainTainDiv1">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width:15%"  align="right">
                        <asp:Label ID="lblShipdate" runat="server" CssClass="iMes_label_13pt" Text="生產日期:"></asp:Label>
                    </td>
                    <td style="width:35%"  align="left">
                        <input type="text" id="dCalShipdate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow" onpropertychange="checkselectdatetime()""/>
                        <input id="btnCal" type="button" value=".." 
                          onclick="showCalendar('dCalShipdate')"  style="width: 17px" class="iMes_button"  />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblPdLine" runat="server" CssClass="iMes_label_13pt" Text="線別:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbPdLine" runat="server" Width="100" AutoPostBack="true" OnSelectedIndexChanged="cmbPdLine_Selected"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td class="iMes_div_MainTainListLable">
                        <asp:Label ID="lblLst" runat="server" CssClass="iMes_label_13pt" Text="不良材料列表:"></asp:Label>
                    </td>
                    <td align="right">
                        <input type="button" id="btnDelete" runat="server" class="iMes_button" onclick="if(clkDelete())" value="刪除"
                            onmouseout="this.className='iMes_button_onmouseout'" onmouseover="this.className='iMes_button_onmouseover'"
                            onserverclick="btnDelete_ServerClick" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" style="height: 390px">
            <input id="hidRecordCount" type="hidden" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline"
                Visible="true">
                <ContentTemplate>
                    <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" Width="100%"
                        GvExtWidth="100%" GvExtHeight="356px" AutoHighlightScrollByValue="true" HighLightRowPosition="3"
                        RowStyle-Height="20" OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'
                        OnRowDataBound="gd_RowDataBound" EnableViewState="false" Style="top: 0px; left: 23px">
                    </iMES:GridViewExt>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%" class="iMes_div_MainTainEdit">
                <tr>
                    <td style="width: 5%;">
                        <asp:Label ID="lblFamily" runat="server" CssClass="iMes_label_13pt" Text="機型:"></asp:Label>
                    </td>
                    <td style="width: 20%;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbFamily" runat="server" Width="80%" AutoPostBack="true"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 5%;">
                        <asp:Label ID="lblPartName" runat="server" CssClass="iMes_label_13pt" Text="材料:"></asp:Label>
                    </td>
                    <td style="width: 20%;">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbPartName" runat="server" Width="80%" AutoPostBack="true"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 5%;">
                        <asp:Label ID="lblPartNO" runat="server" CssClass="iMes_label_13pt" Text="材料P/N:"></asp:Label>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="txtPartNO" runat="server" MaxLength="30" Width="77%"></asp:TextBox>
                    </td>
                    <td style="width: 25%;" colspan="2"></td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="廠商:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbVendor" runat="server" Width="80%" AutoPostBack="true"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblModuleNO" runat="server" CssClass="iMes_label_13pt" Text="模號:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbModuleNo" runat="server" Width="80%" AutoPostBack="true"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblFailReason" runat="server" CssClass="iMes_label_13pt" Text="不良現象:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:DropDownList ID="cmbFailReason" runat="server" Width="80%" AutoPostBack="true"></asp:DropDownList>    
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt" Text="數量:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQty" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" runat="server" Width="77%"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblRemark" runat="server" CssClass="iMes_label_13pt" Text="備註:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemark" runat="server" Width="90%"></asp:TextBox>
                    </td>
                    <td colspan="2" align="center">
                        <input id="btnSave" runat="server" class="iMes_button" onclick="if(clkSave())" onmouseout="this.className='iMes_button_onmouseout'"
                        onmouseover="this.className='iMes_button_onmouseover'" onserverclick="btnSave_ServerClick" value="保存"
                        type="button" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="updatePanelAll" runat="server" UpdateMode="Always" RenderMode="Inline" Visible="true">
            <ContentTemplate>
                <button id="btnDateChange" runat="server" onserverclick="btnDateChange_Click" style="display: none" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>
        <input type="hidden" id="HiddenUserName" runat="server" />
        <input type="hidden" id="dTableHeight" runat="server" />
        <input type="hidden" id="hidId" runat="server" />
        <input type="hidden" id="hidselecttimeforsave" runat="server" />
        <asp:UpdatePanel ID="updatePanel8" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <input type="hidden" id="hidshipdate" runat="server" />
            </ContentTemplate>   
        </asp:UpdatePanel>
        
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

    <script language="javascript" type="text/javascript">
        var toDate = document.getElementById('dCalShipdate');
        window.onload = function() {
            toDate.value = "<%=today%>";
            document.getElementById("<%=HiddenUserName.ClientID %>").value = "<%=userName%>";
            //ShowRowEditInfo(null);
            resetTableHeight();
        };
        var iSelectedRowIndex = null;
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

        function checkselectdatetime() {
            document.getElementById("<%=hidshipdate.ClientID %>").value = document.getElementById('dCalShipdate').value;
            document.getElementById("<%=btnDateChange.ClientID %>").click();

        }

        function clkSave() {
            if (document.getElementById('dCalShipdate').value == "") {
                alert("請選擇日期...");
                return false;
            };
            if (document.getElementById("<%=cmbPdLine.ClientID %>").value == "") {
                alert("請選擇線別...");
                return false;
            };
            if (document.getElementById("<%=cmbFamily.ClientID %>").value == "") {
                alert("請選擇機型...");
                return false;
            };
            if (document.getElementById("<%=cmbPartName.ClientID %>").value == "") {
                alert("請選擇材料...");
                return false;
            };
            if (document.getElementById("<%=txtPartNO.ClientID %>").value == "") {
                alert("請輸入材料P/N...");
                return false;
            };
            if (document.getElementById("<%=cmbVendor.ClientID %>").value == "") {
                alert("請選擇廠商...");
                return false;
            };
            if (document.getElementById("<%=cmbModuleNo.ClientID %>").value == "") {
                alert("請選擇模號...");
                return false;
            };
            if (document.getElementById("<%=cmbFailReason.ClientID %>").value == "") {
                alert("請選擇不良現象...");
                return false;
            };
            if (document.getElementById("<%=txtQty.ClientID %>").value == "") {
                alert("請輸入數量...");
                return false;
            };
            
            return true;
        }
        function clickTable(con) {
            setGdHighLight(con);
            ShowRowEditInfo(con);

        }

        function setGdHighLight(con) {
            if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
            iSelectedRowIndex = parseInt(con.index, 10);
        }
        function clkDelete() {
            var ret = confirm("是否確定要刪除?");
            if (!ret) {
                return false;
            }

            return true;

        }
        
//        function clkSave() {
//            return check();
//        }
//        function check() {
//            var numFloat = parseFloat(txtTime.value);
//            if (numFloat >= 100 || numFloat < 0) {
//                alert(msg5);
//                return false;
//            }

//            
//            return true;
        //        }

        function ShowRowEditInfo(con) {
            if (con == null) {
                setNewItemValue();
                return;
            }

            document.getElementById("<%=hidId.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=hidselecttimeforsave.ClientID %>").value = con.cells[1].innerText.trim();
            document.getElementById("<%=cmbFamily.ClientID %>").value = con.cells[3].innerText.trim()
            document.getElementById("<%=cmbPartName.ClientID %>").value = con.cells[4].innerText.trim()
            document.getElementById("<%=txtPartNO.ClientID %>").value = con.cells[5].innerText.trim()
            document.getElementById("<%=cmbVendor.ClientID %>").value = con.cells[6].innerText.trim()
            document.getElementById("<%=cmbModuleNo.ClientID %>").value = con.cells[7].innerText.trim()
            document.getElementById("<%=cmbFailReason.ClientID %>").value = con.cells[8].innerText.trim()
            document.getElementById("<%=txtQty.ClientID %>").value = con.cells[9].innerText.trim()
            document.getElementById("<%=txtRemark.ClientID %>").value = con.cells[10].innerText.trim()

            var currentId = con.cells[0].innerText.trim();
            
            if (currentId == "") {
                setNewItemValue();
                return;
            }
            else {
                document.getElementById("<%=btnDelete.ClientID %>").disabled = false;
            }
        }

        function setNewItemValue() {
            document.getElementById("<%=btnDelete.ClientID %>").disabled = true;
        }
        function DealHideWait() {
            HideWait();
        }
    </script>

</asp:Content>
