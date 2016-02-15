
<%@ MasterType VirtualPath="~/MasterPage.master" %>  
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FRULabelPrint.aspx.cs" Inherits="PAK_FRULabelPrint" Title="无标题页" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/PAK/Service/WebServiceFRULabelPrint.asmx" />
        </Services>
    </asp:ScriptManager>
    
    <div id="container" style="width: 95%; border: solid 0px red; margin: 0 auto;">
        <div id="div1">
        
            
            <table width="100%" border="0" style="table-layout: fixed;">
                <tr>
                    <td style="width:9%">
                        <asp:Label ID="lblModel" runat="server" CssClass="iMes_label_13pt">Model:</asp:Label>
                    </td>
                    <td  align="left" style="width:10%" >
                         <asp:Label ID="lblModelContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td style="width:5%" align="right">
                        <asp:Label ID="lblMB" runat="server" CssClass="iMes_label_13pt">MB:</asp:Label>
                    </td>
                    <td align="left" style="width:10%">
                         <asp:Label ID="lblMBContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td style="width:5%" align="right">
                        <asp:Label ID="lblDescr" runat="server" CssClass="iMes_label_13pt">Descr:</asp:Label>
                    </td>
                    <td  align="left" style="width:50%">
                         <asp:Label ID="lblDescrContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDeliveryQty" runat="server" CssClass="iMes_label_13pt">Delivery Qty:</asp:Label>
                    </td>
                    <td colspan="5">
                         <asp:Label ID="lblDeliveryQtyContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPCSInCarton" runat="server" CssClass="iMes_label_13pt">PCS In Carton:</asp:Label>
                    </td>
                    <td colspan="4">
                         <asp:Label ID="lblPCSInCartonContent" runat="server" CssClass="iMes_label_11pt"></asp:Label>
                    </td>
                    <td align="right">
                        <button id="btnPrintSetting" runat="server" onclick="clkSetting()" class="iMes_button" onmouseover="this.className='iMes_button_onmouseover'" onmouseout="this.className='iMes_button_onmouseout'"></button>
                    </td>
                </tr>                                
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDataEntry" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td colspan="5">
                        <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true" Width="99%" IsClear="true" IsPaste="true" />
                    </td>
                </tr>                                
            </table>
            <asp:UpdatePanel ID="updatePanel" runat="server"></asp:UpdatePanel>  
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var g_truckid;
        var station;
        var pCode;
        var editor;
        var customer;
        var emptyPattern = /^\s*$/;
        var inputObj;
        var emptyString = "";
        var msgPrintSettingPara = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgPrintSettingPara") %>';
        var firstInputFlage = false;
        var model;
        var dnQty = 0;
        var pcsInQty = 0;
        
        window.onload = function() {
            editor = "<%=UserId%>";
            customer = "<%=Customer%>";
            inputObj = getCommonInputObject();
            station = '<%=Request["Station"] %>'; //"12";
            pCode = '<%=Request["PCode"] %>'; // "OPPA052";
            getAvailableData("processFun");
        };
        
        function processFun(backData)
        {
            ShowInfo("");
            try {
                if (backData == "") {
                    alert("Input is null");
                    getAvailableData("processFun");
                    inputObj.focus();
                    return;
                }
                var lstPrintItem = getPrintItemCollection();
                if (lstPrintItem == null)                 
                {
                    alert(msgPrintSettingPara);
                    getAvailableData("processFun");
                    inputObj.focus();
                    return;
                }

                if (!firstInputFlage) {
                    if (backData.length != 12) {
                        alert("Model length need 12");
                        getAvailableData("processFun");
                        inputObj.focus();
                        return;
                    }
                    model = backData;
                    WebServiceFRULabelPrint.CheckModel(model, onCheckSucc, onCheckFail);
                    getAvailableData("processFun");
                    inputObj.focus();
                    return;
                }
                
                if (isNaN(backData)) {
                    alert("Please Input Int");
                    getAvailableData("processFun");
                    inputObj.focus();
                    return;
                }
                var inputInt = parseInt(backData);
                if (document.getElementById("<%=lblDeliveryQtyContent.ClientID %>").innerText == "") {
                    if (inputInt <= 0) {
                        alert("Please Input Positive integers");
                        getAvailableData("processFun");
                        inputObj.focus();
                        return;
                    }
                    document.getElementById("<%=lblDeliveryQtyContent.ClientID %>").innerText = backData;
                    dnQty = inputInt;
                    return;
                }
                
                if (document.getElementById("<%=lblPCSInCartonContent.ClientID %>").innerText == "") {
                    if (inputInt <= 0) {
                        alert("Please Input Positive integers");
                        getAvailableData("processFun");
                        inputObj.focus();
                        return;
                    } 
                    document.getElementById("<%=lblPCSInCartonContent.ClientID %>").innerText = backData;
                    pcsInQty = inputInt;
                }

                model = document.getElementById("<%=lblModelContent.ClientID %>").innerText;
                WebServiceFRULabelPrint.InputModel(model, editor, station, customer, lstPrintItem, onSucc, onFail);
            }
            catch(e) {
                CleanInput();
                alert(e.description);
                getAvailableData("processFun");
                inputObj.focus(); 
            }              
        }

        function onCheckSucc(result) {
            if(!result[1])
            {
                firstInputFlage = false;
                ShowInfo("This Model is not set ModelInfo.Name = MB or ModelInfo.Name = BomPn");
                getAvailableData("processFun");
                endWaitingCoverDiv();
                inputObj.focus();
                CleanInput();
                return;
            }
            firstInputFlage = true;
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = result[0];
            document.getElementById("<%=lblMBContent.ClientID %>").innerText = result[2];
            document.getElementById("<%=lblDescrContent.ClientID %>").innerText = result[3];
            getAvailableData("processFun");
            inputObj.focus();
        }

        function onCheckFail(result) {
            firstInputFlage = false;
            ShowInfo("This Model is not set ModelInfo.Name = MB or ModelInfo.Name = BomPn");
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblMBContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblDescrContent.ClientID %>").innerText = "";
            getAvailableData("processFun");
            endWaitingCoverDiv();
            inputObj.focus();
            CleanInput();
        }
        
        function onSucc(result) {

            firstInputFlage = false;
            var printQty = 0
            if(dnQty % pcsInQty == 0)
            {
                printQty = dnQty/pcsInQty;
            }
            else
            {
                printQty = parseInt(dnQty / pcsInQty) + 1;
            }
            
            result[0][0].BatPiece = 0;
            
            var msgSuccess = 'Success';
            var message = "[" + model + "] " + msgSuccess;
            ShowSuccessfulInfo(true, message);


            var lstPrtItem = result[0];
            var templateName = lstPrtItem[0].TemplateName;
            var keyCollection = new Array();
            var valueCollection = new Array();
            //@Model, @CartonQty(PCS IN Carton), @Qty(Delivery Qty),@TemplateName
            keyCollection[0] = "@Model";
            valueCollection[0] = generateArray(model);

            keyCollection[1] = "@CartonQty";
            valueCollection[1] = generateArray(pcsInQty);

            keyCollection[2] = "@Qty";
            valueCollection[2] = generateArray(dnQty);

            keyCollection[3] = "@TemplateName";
            valueCollection[3] = generateArray(templateName);

            //setPrintParam(lstPrtItem, "Pick Card", keyCollection, valueCollection);
            for (var jj = 0; jj < lstPrtItem.length; jj++) {
                lstPrtItem[jj].ParameterKeys = keyCollection;
                lstPrtItem[jj].ParameterValues = valueCollection;
            }
            printLabels(lstPrtItem, false);
            CleanInput();
            getAvailableData("processFun");
            inputObj.focus();
        }
        
        function onFail(result)
        {
            
            ShowInfo("Print Error");
            CleanInput();
            getAvailableData("processFun"); 
            endWaitingCoverDiv();
            inputObj.focus();
        }
        
        function clkSetting()
        {
            showPrintSetting(station, pCode);
        }

        function CleanInput() {
            firstInputFlage = false;
            model="";
            dnQty = 0;
            pcsInQty = 0;
            document.getElementById("<%=lblModelContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblMBContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblDescrContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblDeliveryQtyContent.ClientID %>").innerText = "";
            document.getElementById("<%=lblPCSInCartonContent.ClientID %>").innerText = "";
        }
    </script>
</asp:Content>

