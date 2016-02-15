<%--
/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * 
 --%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DefectComponentRePrint.aspx.cs" Inherits="DefectComponentRePrint" Title="Defect Component RePrint" %>
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
	<script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
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
                    <td style="width:10%">
                        <asp:Label ID="lblPrintDate" runat="server" CssClass="iMes_label_13pt" Text="PrintDate:"></asp:Label>
                    </td>
                    <td style="width:35%" align="left">
                        <input type="text" id="StartDate" style="width:100px;" readonly="readonly" class="iMes_textbox_input_Yellow"/>
                        <input id="btnCal" type="button" value=".." onclick="showCalendar('StartDate')"  style="width: 17px" class="iMes_button"  />
                    </td>
                    <td style="width:10%"></td>
                    <td style="width:30%">
                        <input id="btnQuery" type="button"  runat="server"  class="iMes_button" onclick="if(query())" onserverclick="Query_ServerClick" value="Query" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>Batch ID List</legend>
                        <div id="div6" style="height:155">
                            <iMES:GridViewExt ID="gd_Batch" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_Batch_RowDataBound"
                                OnGvExtRowClick='if(typeof(clickTable_Batch)=="function") clickTable_Batch(this)'>
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
        
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>Defect Component Summary</legend>
                        <div id="div4" style="height:155">
                            <iMES:GridViewExt ID="gd_Defect" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_RowDataBound"
                                OnGvExtRowClick='if(typeof(clickTable_Defect)=="function") clickTable_Defect(this)'>
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                    <input type="button" id="btnQuerygd_Defect" runat="server" onserverclick="btnQuerygd_Defect_ServerClick" class="iMes_button" style="display: none"/>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuerygd_Defect" EventName="ServerClick" />
                </Triggers>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                <ContentTemplate>
                    <fieldset>
                        <legend>Defect Component Detail</legend>
                        <div id="div5" style="height:155">
                            <iMES:GridViewExt ID="gd_Detail" runat="server" 
                                AutoGenerateColumns="true" 
                                GvExtHeight="150"
                                GvExtWidth="100%" 
                                style="top: 0px; left: 0px" 
                                Height="140" 
                                SetTemplateValueEnable="False" 
                                HighLightRowPosition="3" 
                                AutoHighlightScrollByValue="True"
                                onrowdatabound="gd_Detail_RowDataBound">
                            </iMES:GridViewExt> 
                        </div>
                    </fieldset>
                    <input type="button" id="btnQuerygd_Detail" runat="server" onserverclick="btnQuerygd_Detail_ServerClick" class="iMes_button" style="display: none"/>
                    <input id="hidoldguid" type="hidden" runat="server" />
                    <input id="hidoldfilename" type="hidden" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnQuerygd_Detail" EventName="ServerClick" />
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
                        onclick="RePrint()" class="iMes_button" 
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
                <input id="hidBatchID" type="hidden" runat="server" />
                <input id="hidPrintDate" type="hidden" runat="server" />
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
   
    <script language="javascript" type="text/javascript">
       
        var editor;
        var customer;
        var station;
        var pCode;
		var sn = "";
		var custsn = "";
		var mbsn = "";
		var mono = "";
		var proId = "";
		var todayDate = "<%=today%>";
		window.onload = function() {
		$('td[id^="td"]').css("color", "blue");
		document.getElementById("StartDate").value = "<%=today%>";
		    editor = "<%=UserId%>";
		    customer = "<%=Customer%>";
		    station = '<%=Request["Station"] %>';
		    pCode = '<%=Request["PCode"] %>';
		};

		function clickTable_Defect(con) {
		    setGdHighLight_Defect(con);
		    ShowRowEditInfo_Defect(con);
        }

        var iSelectedRowIndex_Defect = null;
        function setGdHighLight_Defect(con) {
            if ((iSelectedRowIndex_Defect != null) && (iSelectedRowIndex_Defect != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex_Defect, false, "<%=gd_Defect.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd_Defect.ClientID %>");
            iSelectedRowIndex_Defect = parseInt(con.index, 10);
        }

        function ShowRowEditInfo_Defect(con) {
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
            document.getElementById("<%=btnQuerygd_Detail.ClientID %>").click();
        }

        function setNewItemValue() {
            document.getElementById("<%=hidfamily.ClientID %>").value = "";
            document.getElementById("<%=hidiecpn.ClientID %>").value = "";
            document.getElementById("<%=hidvendor.ClientID %>").value = "";
            document.getElementById("<%=hiddefect.ClientID %>").value = "";
        }

        function query() {
            document.getElementById("<%=hidPrintDate.ClientID %>").value = document.getElementById("StartDate").value;
            return true;
        }

        
        function clickTable_Batch(con) {
            setGdHighLight_Batch(con);
            ShowRowEditInfo_Batch(con);
        }

        var iSelectedRowIndex_Batch = null;
        function setGdHighLight_Batch(con) {
            if ((iSelectedRowIndex_Batch != null) && (iSelectedRowIndex_Batch != parseInt(con.index, 10))) {
                setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex_Batch, false, "<%=gd_Batch.ClientID %>");
            }
            setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd_Batch.ClientID %>");
            iSelectedRowIndex_Batch = parseInt(con.index, 10);
        }

        function ShowRowEditInfo_Batch(con) {
            if (con == null) {
                setNewItemValue_Batch();
                return;
            }
            if (con.cells[0].innerText.trim() == "") {
                setNewItemValue_Batch();
                return;
            }
            document.getElementById("<%=hidBatchID.ClientID %>").value = con.cells[0].innerText.trim();
            document.getElementById("<%=btnQuerygd_Defect.ClientID %>").click();
        }

        function setNewItemValue_Batch() {
            document.getElementById("<%=hidfamily.ClientID %>").value = "";
            document.getElementById("<%=hidiecpn.ClientID %>").value = "";
            document.getElementById("<%=hidvendor.ClientID %>").value = "";
            document.getElementById("<%=hiddefect.ClientID %>").value = "";
        }

        function RePrint() {
            var batchID = document.getElementById("<%=hidBatchID.ClientID %>").value;
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara);
                //OnCancel();
                return;
            }
            PageMethods.RePrint(batchID, customer, "", editor, lstPrintItem, onSaveSucess, onSaveError);
        }

        function onSaveSucess(result) {
            ShowInfo("");
            endWaitingCoverDiv();
            setPrintItemListParam1(result[0], result[1]);
            printLabels(result[0], false);
            clearTable();
            //clearTable_Detail();
            setNewItemValue();

            //CallNextInput();
            ShowInfo("SUCCESS!","green");
        }

        function onSaveError(result) {
            endWaitingCoverDiv();
            ShowMessage(result.get_message());
            ShowInfo(result.get_message());
            //CallNextInput();
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
                ClearGvExtTable("<%=gd_Batch.ClientID%>", 6);
                ClearGvExtTable("<%=gd_Defect.ClientID%>", 6);
                ClearGvExtTable("<%=gd_Detail.ClientID%>", 6);
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
//        function CallNextInput() {
//            getAvailableData("input");
//            inputObj.focus();
//        }
    </script>
</asp:Content>

