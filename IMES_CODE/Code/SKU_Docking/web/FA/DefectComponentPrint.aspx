<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DefectComponentPrint.aspx.cs" Inherits="DefectComponentPrint" Title="Defect Component Print" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <style type="text/css">
   .tdTxt {
    color: blue;
  }
  </style>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
	<script type="text/javascript" src="../CommonControl/jquery/js/jquery.exclusionInOut.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table>
                <tr>
                    <td style="width:10%">
                        <asp:Label ID="lblVendor" runat="server" CssClass="iMes_label_13pt" Text="Vendor:"></asp:Label>
                    </td>
                    <td style="width:30%">
                        <asp:DropDownList ID="cmbVendor" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td style="width:10%"></td>
                    <td style="width:30%">
                        <input id="btnQuery" type="button"  runat="server"  class="iMes_button" onserverclick="Query_ServerClick"  value="Query" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReturnLine" runat="server" CssClass="iMes_label_13pt" Text="ReturnLine:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbReturnLine" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalQty" runat="server" CssClass="iMes_label_13pt" Text="TotalQty:"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="txtTotalQty" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>Defect Component Summary</legend>
                        <div id="div4" style="height:155">
                            <iMES:GridViewExt ID="gd" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_RowDataBound"
                                OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'>
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>Defect Component Detail</legend>
                        <div id="div5" style="height:155">
                            <iMES:GridViewExt ID="gd2" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd2_RowDataBound">
                                
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                    <input type="button" id="btnQuerygd2" runat="server" onserverclick="btnQuerygd2_ServerClick" class="iMes_button" style="display: none"/>
                    <input id="hidoldguid" type="hidden" runat="server" />
                    <input id="hidoldfilename" type="hidden" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuerygd2" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="div3">
            <table width="100%">
                <tr>
                    <td align="right">&nbsp;
                        
                    </td>
                    <td align="right" colspan="2" >
                        
                    </td>
                    <td align="right">&nbsp;
                        <input id="btpPrintSet" type="button"  runat="server" 
                        class="iMes_button" onclick="showPrintSettingDialog()" 
                        onmouseover="this.className='iMes_button_onmouseover'" value="Print Setting"
                        onmouseout="this.className='iMes_button_onmouseout'" align="right"/>
                        <input id="btnPrint" type="button"  runat="server" 
                        onclick="print()" class="iMes_button" 
                        onmouseover="this.className='iMes_button_onmouseover'" value="Print"
                        onmouseout="this.className='iMes_button_onmouseout'" visible="True"/>
                        <input id="btnReprint" type="button"  runat="server" 
                        onclick="reprint()" class="iMes_button" 
                        onmouseover="this.className='iMes_button_onmouseover'" value="RePrint"
                        onmouseout="this.className='iMes_button_onmouseout'" visible="True"/>
                    </td>                      	   	   	    
                </tr>
            </table>   
        </div> 
       
        
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
       
            <ContentTemplate>
                
                <input id="hidguid" type="hidden" runat="server" />
                <input id="hidvendor" type="hidden" runat="server" />
                <input id="hidfamily" type="hidden" runat="server" />
                <input id="hidiecpn" type="hidden" runat="server" />
                <input id="hiddefect" type="hidden" runat="server" />
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
   
    <script language="javascript" type="text/javascript">

        var AccountId = '<%=Request["AccountId"] %>';
        var Login = '<%=Request["Login"] %>';
        var editor;
        var customer;
        var station;
        var pCode;
		var sn = "";
		var custsn = "";
		var mbsn = "";
		var mono = "";
		var proId = "";
		var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
		window.onload = function() {
		    $('td[id^="td"]').css("color", "blue");
		    editor = "<%=UserId%>";
		    customer = "<%=Customer%>";
		    station = '<%=Request["Station"] %>';
		    pCode = '<%=Request["PCode"] %>';
		};

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
            if (con.cells[0].innerText.trim() == "") {
                setNewItemValue();
                return;
            }
            document.getElementById("<%=hidfamily.ClientID %>").value = con.cells[2].innerText.trim();
            document.getElementById("<%=hidiecpn.ClientID %>").value = con.cells[3].innerText.trim();
            document.getElementById("<%=hidvendor.ClientID %>").value = con.cells[5].innerText.trim();
            document.getElementById("<%=hiddefect.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=btnQuerygd2.ClientID %>").click();
        }

        function setNewItemValue() {
            document.getElementById("<%=hidfamily.ClientID %>").value = "";
            document.getElementById("<%=hidiecpn.ClientID %>").value = "";
            document.getElementById("<%=hidvendor.ClientID %>").value = "";
            document.getElementById("<%=hiddefect.ClientID %>").value = "";
        }

        function query() {
            ShowInfo("");
        }

        function print() {
            var guid = document.getElementById("<%=hidguid.ClientID %>").value;
            var returnLine = document.getElementById("<%=cmbReturnLine.ClientID %>").value;
            var qty = getInputOrSpanValue(document.getElementById("<%=txtTotalQty.ClientID %>"));
            if (guid == "") {
                alert("請先查詢資料...");
                return;
            }
            
            if (returnLine == "") {
                alert("請選擇ReturnLine...");
                return;
            }
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                //OnCancel();
                return;
            }
            PageMethods.Save(guid, returnLine, qty, lstPrintItem, onSaveSucess, onSaveError);
        }

        function onSaveSucess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            setPrintItemListParam1(result[0], result[1]);
            printLabels(result[0], false);
            clearTable();
            setNewItemValue();
            document.getElementById("<%=txtTotalQty.ClientID %>").innerText = "";
            //CallNextInput();
            ShowInfo("SUCCESS!","green");
        }

        function onSaveError(result) {
            endWaitingCoverDiv();
            clearTable();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
        }

        function setPrintItemListParam1(backPrintItemList, batchID) //Modify By Benson at 2011/03/30
        {
            var keyCollection = new Array();
            var valueCollection = new Array();

            keyCollection[0] = "@BatchID";
            valueCollection[0] = generateArray(batchID);

            for (var jj = 0; jj < backPrintItemList.length; jj++) {
                backPrintItemList[jj].ParameterKeys = keyCollection;
                backPrintItemList[jj].ParameterValues = valueCollection;
            }
        }

        function clearTable() {
            try {
                ClearGvExtTable("<%=gd.ClientID%>", 6);
                ClearGvExtTable("<%=gd2.ClientID%>", 6);
            } catch (e) {
                alert(e.description);
            }
        }
        
        function initPage() {
            sn = "";
            custsn = "";
            mbsn = "";
            mbno = "";
            proId = "";
            $('td[id^="td"]').text('');
        }
        window.onbeforeunload = function()
        {
            if (sn != "") {
                OnCancel();
                initPage();
            }
        };   
       
        function OnCancel() {
             PageMethods.Cancel(sn);
         }

         function showPrintSettingDialog() {
             showPrintSetting(station, pCode);
         }

         function reprint() {
             try {
                 var url = "DefectComponentRePrint.aspx?Station=" + station + "&PCode=" + pCode + "&UserId=" + editor + "&Customer=" + customer + "&AccountId=" + AccountId + "&Login=" + Login;
                 window.showModalDialog(url, "", 'dialogWidth:950px;dialogHeight:700px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
             }
             catch (e) {
                 alert(e.description);
             }
         }

//        function CallNextInput() {
//            getAvailableData("input");
//            inputObj.focus();
//        }
    </script>
</asp:Content>

