
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" AsyncTimeout="3000" CodeFile="UpdateConsolidate.aspx.cs" Inherits="PAK_UpdateConsolidate" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>


<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>

<div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
            <asp:ServiceReference Path= "Service/WebServiceUpdateConsolidate.asmx" />
        </Services>
    </asp:ScriptManager>
   <center>
   
    <table border="0" width="95%">
    <tr>
        <td style="width:40%"><asp:Label ID="lblInput" runat="server" 
                CssClass="iMes_label_13pt" Font-Bold="True"></asp:Label></td>
        <td style="width:40%">
            <input type="text" id="txtInput" style="width:100%" class="iMes_input_White"
                            onkeydown="inputConsolidate()"  runat="server"  />
        </td>
        <td align="right">
            <input id="btnQuery" type="button" style="width:60%" runat="server" class="iMes_button" 
               onclick="clkQuery()" onmouseover="this.className='iMes_button_onmouseover'" 
               onmouseout="this.className='iMes_button_onmouseout'"/>
        </td>
    </tr>
    <tr><td colspan="5"><hr></td></tr>
    </table>
    <table border="0" width="95%">
    <tr>
        <td style="width:40%">
            <table border="0" width="100%">
                <tr>
                    <td style="width:50%"><asp:Label ID="lblDelivery" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtDelivery" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><asp:Label ID="lblPallet" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtPallet" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtModel" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><asp:Label ID="lblShipment" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtShipment" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
            </table>
        </td>
        <td style="width:40%">
            <table border="0" width="100%">
                <tr>
                    <td style="width:50%"><asp:Label ID="lblConQty" runat="server" 
                            CssClass="iMes_label_13pt" Font-Bold="True"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtConQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><asp:Label ID="lblActConQty" runat="server" 
                            CssClass="iMes_label_13pt" Font-Bold="True"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtActConQty" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width:50%"><asp:Label ID="lblConsolidate" runat="server" 
                            CssClass="iMes_label_13pt" Font-Bold="True"></asp:Label></td>
                    <td style="width:50%"><asp:Label ID="txtConsolidate" runat="server" CssClass="iMes_label_13pt"></asp:Label></td>
                </tr>
                
            </table>
        </td>
        <td style="width:20%">
            <table border="0" width="100%">
                <tr>
                    <td align="right">
                    <input id="btnUpdate" type="button" style="width:60%" runat="server" class="iMes_button" 
                        onclick="clkUpdate()" onmouseover="this.className='iMes_button_onmouseover'" 
                        onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                    <input id="btnClear" type="button" style="width:60%" runat="server" class="iMes_button" 
                        onclick="clkClear()" onmouseover="this.className='iMes_button_onmouseover'" 
                        onmouseout="this.className='iMes_button_onmouseout'"/>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
            </table>
        </td>
     </tr>
     
     
     <tr><td colspan="5"><hr></td></tr>
     </table>
     <table border="0" width="95%">
     <tr>
        <td colspan="3" style="width:90%">
        <fieldset id="Fieldset1" >
            <legend align ="left"  ><asp:Label ID="lblField" runat="server" 
                    CssClass="iMes_label_13pt" Font-Bold="True" ForeColor="Blue" /></legend>
            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGridClear" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="btnhidQuery" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
	            <iMES:GridViewExt ID="gd" runat="server" AutoGenerateColumns="true" AutoHighlightScrollByValue="true" 
                        GetTemplateValueEnable="False" GvExtHeight="180px" Height="180px" 
                        GvExtWidth="100%" OnGvExtRowClick="clickTable(this)"
                        OnGvExtRowDblClick="" SetTemplateValueEnable="False" 
                        HighLightRowPosition="1" HorizontalAlign="Left"
                        onrowdatabound="gd_RowDataBound">                                     
                </iMES:GridViewExt>                                                                      
            </ContentTemplate>   
        </asp:UpdatePanel>
		</fieldset>
        </td>
     </tr>	
     <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" UpdateMode="Always">
                <ContentTemplate>
                    <button id="btnGridClear" runat="server" type="button" onclick="" style="display:none" onserverclick="clearGrid"></button>
                    <button id="btnhidQuery" runat="server" type="button" onclick="" style="display:none" onserverclick="ConsolidateQuery"></button>
                </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
      </tr>
     </table>
    </center>
</div>
<script type="text/javascript">

    
    var station = "";
    var editor = "";
    var customer = "";
    var pdline = "";
    var msgSystemError = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSystemError") %>';
    var mesNoConsolidate = '<%=this.GetLocalResourceObject(Pre + "_mesNoConsolidate").ToString()%>';
    var mesUpdateConfirm = '<%=this.GetLocalResourceObject(Pre + "_mesUpdateConfirm").ToString()%>';
    var mesAllConsolidate = '<%=this.GetLocalResourceObject(Pre + "_mesAllConsolidate").ToString()%>';

    document.body.onload = function() {
        station = "<%=Station%>";
        editor = "<%=UserId%>";
        customer = "<%=Customer%>";

        document.getElementById("<%=txtInput.ClientID %>").focus();
    }

    function clickTable(con) {
        ShowInfo("");
        setGdHighLight(con);
        ShowRowEditInfo(con);
    }
    var iSelectedRowIndex = null;
    function setGdHighLight(con) {
        if ((iSelectedRowIndex != null) && (iSelectedRowIndex != parseInt(con.index, 10))) {
            //去掉过去高亮行
            setRowSelectedOrNotSelectedByIndex(iSelectedRowIndex, false, "<%=gd.ClientID %>");
        }
        //设置当前高亮行   
        setRowSelectedOrNotSelectedByIndex(con.index, true, "<%=gd.ClientID %>");
        //记住当前高亮行
        iSelectedRowIndex = parseInt(con.index, 10);
    }

    function ShowRowEditInfo(con) {
        if (con == null) {
            document.getElementById("<%=txtShipment.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtPallet.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtModel.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtConQty.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = "";
            return;
        }

        if (con.cells[0].innerText.trim() == "" || con.cells[1].innerText.trim() == "" || con.cells[2].innerText.trim() == "") {
            document.getElementById("<%=txtShipment.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtPallet.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtModel.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtConQty.ClientID %>").innerHTML = "";
            document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = "";
        }
        else {
            document.getElementById("<%=txtShipment.ClientID %>").innerHTML = con.cells[0].innerText.trim();
            document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = con.cells[1].innerText.trim();
            document.getElementById("<%=txtPallet.ClientID %>").innerHTML = con.cells[2].innerText.trim();
            document.getElementById("<%=txtModel.ClientID %>").innerHTML = con.cells[3].innerText.trim();
            document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = con.cells[4].innerText.trim();
            document.getElementById("<%=txtConQty.ClientID %>").innerHTML = con.cells[5].innerText.trim();
            document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = con.cells[6].innerText.trim();
        }
    }

    function clkClear() {
        ShowInfo("");
        document.getElementById("<%=txtShipment.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtPallet.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtModel.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtConQty.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = "";

        beginWaitingCoverDiv();
        document.getElementById("<%=btnGridClear.ClientID%>").click();
        document.getElementById("<%=txtInput.ClientID %>").value = "";
        document.getElementById("<%=txtInput.ClientID %>").focus();

    }

    function clkUpdate() {
        var consolidate = "";
        var actQty = "";
        consolidate = document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML;
        if (consolidate == "") {
            alert(mesNoConsolidate);
            return;
        }
        actQty = document.getElementById("<%=txtActConQty.ClientID %>").innerHTML;
        
        if (confirm(mesUpdateConfirm)) {
            beginWaitingCoverDiv();
            WebServiceUpdateConsolidate.Update(pdline, station, editor, customer, consolidate, actQty, onUpdateSucceed, onUpdateFail);
        }
    }
    function onUpdateSucceed(result) {
        try {
            if (result == null) {
                endWaitingCoverDiv();
                var content = msgSystemError;
                ShowMessage(content);
                ShowInfo(content);
            }
            else if (result[0] == SUCCESSRET) {
                var mes = document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML;            
                document.getElementById("<%=txtShipment.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtPallet.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtModel.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtConQty.ClientID %>").innerHTML = "";
                document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = "";
                
                ShowInfo("The Consolidate [" + mes + "] has just been Updated!", "green");
                document.getElementById("<%=btnGridClear.ClientID%>").click();
                document.getElementById("<%=txtInput.ClientID %>").value = "";
                document.getElementById("<%=txtInput.ClientID %>").focus();
            }
            else {
                endWaitingCoverDiv();
                var content = result[0];
                ShowMessage(content);
                ShowInfo(content);
            }
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }

    function onUpdateFail(error) {
        try {
            endWaitingCoverDiv();
            ShowMessage(error.get_message());
        } catch (e) {
            alertAndCallNext(e.description);
        }
    }
    function alertAndCallNext(message) {
        endWaitingCoverDiv();
        alert(message);
    }


    function clkQuery() {
        ShowInfo("");
        var inputConsolidate = document.getElementById("<%=txtInput.ClientID %>").value;
        if (inputConsolidate != "" && inputConsolidate.length != 10) {
            alert(mesAllConsolidate);
            document.getElementById("<%=txtInput.ClientID %>").focus();
            return;
        }
        beginWaitingCoverDiv();
        document.getElementById("<%=txtShipment.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtDelivery.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtPallet.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtModel.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtConsolidate.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtConQty.ClientID %>").innerHTML = "";
        document.getElementById("<%=txtActConQty.ClientID %>").innerHTML = "";
        document.getElementById("<%=btnhidQuery.ClientID%>").click();
    }

    function inputConsolidate() {
        if (event.keyCode == 13) {
            clkQuery();
        }
    }
    
    
</script>
</asp:Content>