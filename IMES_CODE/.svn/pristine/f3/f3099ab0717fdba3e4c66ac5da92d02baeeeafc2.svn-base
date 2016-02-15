<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PilotRunMO.aspx.cs" Inherits="FA_PilotRunMO" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="1"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/FA/Service/WebServicePilotRunMO.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 99%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr style="width:120px"> 
                    <td align="left">
                        <asp:Label ID="lblShipdate" runat="server" CssClass="iMes_label_13pt" Text="Create Date:"></asp:Label>
                    </td>
                    <td colspan="4" align="left">
                        <asp:label ID="lblfrom" Text="from" runat="server"></asp:label>&nbsp;
                        <input type="text" id="dCalShipdate_from" style="width:90px;" readonly="readonly" />&nbsp;
                        <input id="btnCalfrom" type="button" value=".." onclick="showCalendar('dCalShipdate_from')" style="width: 17px" class="iMes_button"  />
                        &nbsp;&nbsp;&nbsp;
                        <asp:label ID="lblto" Text="to" runat="server"></asp:label>&nbsp;
                        <input type="text" id="dCalShipdate_to" style="width:90px;" readonly="readonly" />&nbsp;
                        <input id="btnCalto" type="button" value=".." onclick="showCalendar('dCalShipdate_to')" style="width: 17px" class="iMes_button"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMOType" runat="server" CssClass="iMes_label_13pt">MOType:</asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="cmbMOType" runat="server" Width="98%"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </td>
                    <td></td>
                    <td>
                        <input type="button" id="btnQuery" runat="server" value="Query" onclick="if(query())" onserverclick="btnQuery_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                    <td></td>
                </tr>
            </table>
            <asp:Panel ID="Panel3" runat="server" Width="100%"> 
                <table width="100%" border="0" style="table-layout: fixed;">
                    <tr>
                        <td align="center">
                            <input type="button" id="btnRelease" runat="server" value="Release" onserverclick="btnRelease_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;
                            <input type="button" id="btnHold" runat="server" value="Hold" onserverclick="btnHole_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
                                <ContentTemplate>
                                    <div id="div2" style="height:225px">
                                        <iMES:GridViewExt ID="gd" runat="server" 
                                            AutoGenerateColumns="true" 
                                            GvExtHeight="220px"
                                            GvExtWidth="100%" 
                                            style="top: 0px; left: 0px" 
                                            Height="210px" 
                                            SetTemplateValueEnable="False" 
                                            HighLightRowPosition="3" 
                                            AutoHighlightScrollByValue="True"
                                            onrowdatabound="gd_RowDataBound"
                                            OnGvExtRowClick='if(typeof(clickTable)=="function") clickTable(this)'>
                                        </iMES:GridViewExt> 
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                    </tr> 
                </table>
             </asp:Panel>
             <table width="100%" border="0" style="table-layout: fixed;">
                <tr style="width:120px"> 
                    <td>
                        <asp:Label ID="lblPilotMO" runat="server" CssClass="iMes_label_13pt" Text="PilotMO:"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblPilotMOContent" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblQty" runat="server" CssClass="iMes_label_13pt" Text="Qty:"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtQty" runat="server" MaxLength="10" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"></asp:TextBox>
                    </td>
                    <td align="center">
                        <input type="button" id="btnSave" runat="server" value="Save" onclick="if(save())" onserverclick="btnSave_ServerClick" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>&nbsp;&nbsp;
                        <input type="button" id="btnNew" runat="server" value="New" onclick="add()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"/>    
                        <input id="btprint" type="button"  runat="server" value="Print" onclick="print()" class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'" />
                        <input id="btprintsetting" type="button"  runat="server" value="PrintSetting" class="iMes_button" onclick="showPrintSettingDialog()" onmouseover="this.className='iMes_button_onmouseover'"  onmouseout="this.className='iMes_button_onmouseout'" />
                    </td>
                    
                </tr>
               <TR>
    	                    	   	   	    
    </TR>
            </table>              
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional"  RenderMode="Inline" Visible="true" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnQuery" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updHidden" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <input id="TypeValue" type="hidden" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="hidQueryValue" type="hidden" runat="server" />
        <input id="hideditor" type="hidden" runat="server" />
        <input id="hidcustomer" type="hidden" runat="server" />
        <input id="hidstationId" type="hidden" runat="server" />
        <input id="hidfromday" type="hidden" runat="server" />
        <input id="hidtoday" type="hidden" runat="server" />
        <input id="hidcombineqty" type="hidden" runat="server" />
        <input id="hidpilotmo" type="hidden" runat="server" />
    </div>   
    <script language="javascript" type="text/javascript">
        var editor;
        var tbl;
        var DEFAULT_ROW_NUM = 7;
        var customer;
        var stationId;
        var showMsgList = "";
        var fromDate = document.getElementById('dCalShipdate_from');
        var toDate = document.getElementById('dCalShipdate_to');
        var accountid = '<%=AccountId%>';
        var login = '<%=Login%>';
        var pcode = "";
        var msgPrintSettingPara = "請選擇打印設定";
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        window.onload = function() {
            tbl = "<%=gd.ClientID %>";
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            stationId = '<%=Request["Station"] %>';
            fromDate.value = "<%=fromday%>";
            toDate.value = "<%=today%>";
            pcode = '<%=Request["PCode"] %>';
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
        
        function getcmbMoTypeObj() {
            return document.getElementById("<%=cmbMOType.ClientID %>");
        }

        function query() {
            var motype = getcmbMoTypeObj().options[getcmbMoTypeObj().selectedIndex].value;
            if (motype == "") {
                alert("請選擇MoType...");
                return false;
            }
            document.getElementById("<%=hidfromday.ClientID %>").value = document.getElementById('dCalShipdate_from').value
            document.getElementById("<%=hidtoday.ClientID %>").value = document.getElementById('dCalShipdate_to').value
            return true;
        }

        function save() {
            var qty = parseInt(document.getElementById("<%=txtQty.ClientID %>").value);
            var pilotMO = document.getElementById("<%=lblPilotMOContent.ClientID %>").innerText;
            document.getElementById("<%=hidpilotmo.ClientID %>").value = pilotMO;
            var combineqqty = parseInt(document.getElementById("<%=hidcombineqty.ClientID %>").value);
            if (combineqqty > qty) {
                alert("Pilot MO Qty can not small Combine Qty");
                return false;
            }
            return true;
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
            document.getElementById("<%=lblPilotMOContent.ClientID %>").innerText = con.cells[0].innerText.trim();
            document.getElementById("<%=hidpilotmo.ClientID %>").value = con.cells[0].innerText.trim();
//            document.getElementById("<%=txtQty.ClientID %>").value = con.cells[6].innerText.trim();
            //            document.getElementById("<%=hidcombineqty.ClientID %>").value = con.cells[10].innerText.trim();
            document.getElementById("<%=txtQty.ClientID %>").value = con.cells[7].innerText.trim();
            document.getElementById("<%=hidcombineqty.ClientID %>").value = con.cells[9].innerText.trim();
            document.getElementById("<%=btnRelease.ClientID %>").disabled = false;
            document.getElementById("<%=btnHold.ClientID %>").disabled = false;
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
        }

        function setNewItemValue() {
            document.getElementById("<%=txtQty.ClientID %>").value = "";
            document.getElementById("<%=lblPilotMOContent.ClientID %>").innerText = "";
            document.getElementById("<%=hidpilotmo.ClientID %>").value = "";
            document.getElementById("<%=hidcombineqty.ClientID %>").value = "";
            document.getElementById("<%=btnRelease.ClientID %>").disabled = true;
            document.getElementById("<%=btnHold.ClientID %>").disabled = true;
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }

        function initPage()
        {
            tbl = "<%=gd.ClientID %>";
            ClearGvExtTable(tbl, DEFAULT_ROW_NUM);
            setSrollByIndex(0 ,false);
        }

        window.onbeforeunload = function()
        {
            OnCancel();
        };   
        
        function OnCancel()
        {
            //WebServicePilotRunMO.cancel("");
        }
        
        function ExitPage(){
            OnCancel();
        }
        
        function ResetPage(){
            ExitPage();
            initPage();
            ShowInfo("");
        }

        function add() {
            var fistSelStation = "";
            var url = "PilotRunMO_Add.aspx?UserId=" + editor + "&Customer=" + customer + "&UserName=" + editor + "&AccountId=" + accountid + "&Login=" + login;
            var paramArray = new Array();
            var model = "A";
            if (model != "") {
                paramArray[0] = model;
                paramArray[1] = editor;
                paramArray[2] = customer;
                paramArray[3] = fistSelStation;
                window.showModalDialog(url, paramArray, 'dialogWidth:900px;dialogHeight:450px;status:no;help:no;menubar:no;toolbar:no;resize:no;scrollbars:vertical');
            }
            else {
                alert("Pless Select Model");
            }
            document.getElementById("<%=btnQuery.ClientID %>").click();
        }
        function showPrintSettingDialog() {
            showPrintSetting(stationId, pcode);
        }
        function print() {
            var lstPrintItem = getPrintItemCollection();
            if (lstPrintItem == null) {
                alert(msgPrintSettingPara); //請先檢查設置列印頁面參數
                return;
            } 
            var qty = parseInt(document.getElementById("<%=txtQty.ClientID %>").value);
            var pilotMO = document.getElementById("<%=lblPilotMOContent.ClientID %>").innerText;
            if (pilotMO==null ||pilotMO=="")
            {
              alert("請先選中需要打印的PilotMO");
            }
            else {
                beginWaitingCoverDiv();
                WebServicePilotRunMO.Print(pilotMO, qty, "A1D", editor, "PMO", customer, lstPrintItem, onPrintSucc, onPrintFail);
            
            }
        }
        function onPrintSucc(result) {
            try {
                endWaitingCoverDiv();
                if (result == null) {
                    ShowInfo("");
                    var content = "System error";
                    ShowMessage(content);
                    ShowInfo(content);
                }
                else if (result[0] == SUCCESSRET) {
                setPrintItemListParam(result[1][0], result[2], result[3]);
                printLabels(result[1][0], false);
                ShowInfo(result[2]+" print success!", "green");         
                }
                else {
                    ShowInfo("");
                    var content = result[0];
                    ShowMessage(content);
                    ShowInfo(content);
                }
            }
            catch (e) {
                alert(e.description);
            }
        }

        function onPrintFail(error) {
            try {
                endWaitingCoverDiv();
                ShowInfo("");
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());

            } catch (e) {
                alert(e.description);
                
            }
        }
        function setPrintItemListParam(backPrintItemList, mo,qty) {
            var lstPrtItem = backPrintItemList;
            var keyCollection = new Array();
            var valueCollection = new Array();
            keyCollection[0] = "@MO";
            valueCollection[0] = generateArray(mo);
            keyCollection[1] = "@QTY";
            valueCollection[1] = generateArray(qty);
            setAllPrintParam(lstPrtItem, keyCollection, valueCollection);

        }
    </script>
</asp:Content>

