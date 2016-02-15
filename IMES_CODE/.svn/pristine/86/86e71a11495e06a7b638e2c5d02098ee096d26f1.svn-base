<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine Material and Lot
 * CI-MES12-SPEC-FA-UC Collection Material Lot.doc  
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2013-04-102  IEC000043             Create
 * Known issues:
 * TODO:
*/
--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPageMaintain.master" AutoEventWireup="true"
    CodeFile="CombineMaterialLot.aspx.cs" Inherits="FA_CombineMaterialLot" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
<bgsound  src="" autostart="true" id="bsoundInModal" loop="5"></bgsound>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
      <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  

    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function endRequest(sender, e) {
            EnableDro(isEndFirstInput);
        }
        var process;
        var collection;
        var editor;
        var customer;
        var pCode;
        var boxId;
        var specNo;
        var lotNo;
        var qty;
        var receving;
        var material;
        var materialType;
        var collectStage;
        var index = 1;
        var initPrdRowsCount;
        var grvProductClientID = "<%=grvProduct.ClientID%>";
        var msgInputLotNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputLotNo") %>';
        var msgInputCT = '<%=this.GetLocalResourceObject(Pre + "_msgInputCT") %>';
        var ctArray = [];
        var isEndFirstInput = false;
        var msgInputCollectionType = '<%=this.GetLocalResourceObject(Pre + "_msgInputCollectionType") %>';
        var msgInputCollectionStage = '<%=this.GetLocalResourceObject(Pre + "_msgInputCollectionStage") %>';
        var msgInputMaterial = '<%=this.GetLocalResourceObject(Pre + "_msgInputMaterial") %>';
     

        window.onload = function() {
            initPrdRowsCount = parseInt('<%=initProductTableRowsCount%>', 10) + 1;
            callNextInput();

        }
        window.onbeforeunload = function() {

        }

        function ExitPage() {

        }
        function IsNumber(src) {
            var regNum = /^[0-9]+[0-9]*]*$/;
            return regNum.test(src);
        }

        function AddRowInfo(RowArray) {
            try {
                if (index < initPrdRowsCount) {
                    eval("ChangeCvExtRowByIndex_" + grvProductClientID + "(RowArray,false, index)");
                }
                else {
                    eval("AddCvExtRowToBottom_" + grvProductClientID + "(RowArray,false)");
                }
                setSrollByIndex(index, false);
                setRowSelectedOrNotSelectedByIndex(index - 2, false, grvProductClientID);
                setRowSelectedOrNotSelectedByIndex(index - 1, true, grvProductClientID);     //设置当前高亮行
                index++;

            }
            catch (e) {
                ShowInfo2(e.description);
                //  PlaySound();
            }
        }
        function GetCollection() {
            var id = "<%=drpCollection.ClientID %>";
            collection = document.getElementById(id)[document.getElementById(id).selectedIndex].value;
            document.getElementById("<%=hidCollection.ClientID %>").value = collection;
        }
        function GetMaterialTypeValue() {
            var id = document.getElementById("<%=hidMaterialTypeID.ClientID %>").value;
            materialType = document.getElementById(id)[document.getElementById(id).selectedIndex].text;
            document.getElementById("<%=hidMaterialTypeValue.ClientID%>").value = materialType;
        }
        function GetCollectStageValue() {
            var id = document.getElementById("<%=hidCollectStageID.ClientID %>").value;
            collectStage = document.getElementById(id)[document.getElementById(id).selectedIndex].text;
            document.getElementById("<%=hidCollectStageValue.ClientID%>").value = collectStage;

        }
        function SetInfo() {

            document.getElementById("<%=txtQty.ClientID%>").value = document.getElementById("<%=hidQty.ClientID %>").value;
            document.getElementById("<%=txtCombinedQty.ClientID%>").value = document.getElementById("<%=hidComQty.ClientID %>").value;
            document.getElementById("<%=txtSpecNo.ClientID%>").value = document.getElementById("<%=hidSpecNo.ClientID %>").value;
            if (collection == "Output") {
                document.getElementById("<%=txtLotNo.ClientID%>").value = document.getElementById("<%=hidLotNo.ClientID%>").value;
                lotNo = document.getElementById("<%=hidLotNo.ClientID%>").value;
                AddTable();
            }
            else {
                document.getElementById("<%=txtLotNo.ClientID%>").value = lotNo;
            }
            isEndFirstInput = true;
            EnableDro(true);
            ShowInfo2("Please input Material CT!", "green");

        }
        function CheckSpecNo(data) {
            if (specNo == "") {
                if (data.length != 5)
                { return false; }
                document.getElementById("<%=txtSpecNo.ClientID%>").value = data;
                document.getElementById("<%=hidSpecNo.ClientID%>").value = data;
                ShowInfo2("Please input LotNo!", "green");
            }
            return true;

        }
        function CheckLotNo(data) {
            if (lotNo == "") {
                if (data.length != 8)
                { return false; }
                document.getElementById("<%=txtLotNo.ClientID%>").value = data;
                document.getElementById("<%=hidLotNo.ClientID%>").value = data;
                ShowInfo2("Please input Qty!", "green");
            }
            return true;
        }
        function CheckQty(data) {
            if (qty == "") {
                if (!CheckPositiveInteger(data))
                { return false; }
                document.getElementById("<%=txtQty.ClientID%>").value = data;
                document.getElementById("<%=hidQty.ClientID%>").value = data;
                ShowInfo2("Please input 9999!", "green");

            }
            return true;
        }
        function CheckPositiveInteger(data) {
            //var re = /^\d+$/;
            var re = /^[1-9]\d*$/;
            if (!re.test(data)) {
                return false;
            }
            return true;
        }
        function EndSave() {
            ResetValue();
            ShowInfo2("Success!", "green");
        }
		function PlaySound() {
         var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
         var obj = document.getElementById("bsoundInModal");
         obj.src = sUrl;
     }
        function ResetValue() {

            document.getElementById("<%=txtSpecNo.ClientID%>").value = "";
            document.getElementById("<%=txtLotNo.ClientID%>").value = "";
            document.getElementById("<%=txtQty.ClientID%>").value = "";
            document.getElementById("<%=txtCombinedQty.ClientID%>").value = "";
            document.getElementById("<%=txtScanQty.ClientID%>").value = "0";

            document.getElementById("<%=hidCtList.ClientID%>").value = "";
            lotNo = "";
            //hidCtList txtCombinedQty
            isEndFirstInput = false;
            index = 1;
            clearTable();
            ctArray = [];
        }
        function CheckExistData(data) {

            var gdObj = document.getElementById(grvProductClientID);
            var result = false;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (data.trim() != "" && gdObj.rows[i].cells[0].innerText.toUpperCase() == data) {
                    result = true;
                    break;
                }
            }
            return result;
        }
        function GetLastCT() {
            var last = "";
            var gdObj = document.getElementById(grvProductClientID);
            var result = false;
            for (var i = 1; i < gdObj.rows.length; i++) {
                if (gdObj.rows[i].cells[0].innerText.trim() != "")
                { last = gdObj.rows[i].cells[0].innerText; }

            }
            return last;
        }

        function AddTableByValue(data) {
            var rowInfo = new Array();
            var q = parseInt(document.getElementById("<%=txtScanQty.ClientID%>").value, 10);

            rowInfo.push(data);
            AddRowInfo(rowInfo);

        }
function PlaySound() {
         var sUrl = '../Sound/' + '<%=System.Configuration.ConfigurationManager.AppSettings["FailAudioFile"] %>';
         var obj = document.getElementById("bsoundInModal");
         obj.src = sUrl;
     }

	 function StopSound()
	 {
	    var obj = document.getElementById("bsoundInModal");
        obj.src = "";
	}
        function AddTable() {
            var rowInfo = new Array();
            var q = parseInt(document.getElementById("<%=txtScanQty.ClientID%>").value, 10);
            rowInfo.push(document.getElementById("<%=hidInputCT.ClientID%>").value);
            AddRowInfo(rowInfo);
            q++;
            document.getElementById("<%=txtScanQty.ClientID%>").value = q;
            document.getElementById("<%=hidCtList.ClientID%>").value = document.getElementById("<%=hidCtList.ClientID%>").value + rowInfo[0] + ",";
            ctArray.push(document.getElementById("<%=hidInputCT.ClientID%>").value);
        }
        function clearTable() {
            try {
                ClearGvExtTable(grvProductClientID, initPrdRowsCount); //grvDNClientID
                index = 1;
                setSrollByIndex(0, false);
            }
            catch (e) {
                alert(e.description);
            }
        }
        function EnableDro(enable) {
            var id1 = document.getElementById("<%=hidMaterialTypeID.ClientID %>").value;
            var id2 = document.getElementById("<%=hidCollectStageID.ClientID %>").value;
            var id3 = document.getElementById("<%=drpCollection.ClientID%>");
            document.getElementById(id1).disabled = enable;
            document.getElementById(id2).disabled = enable;
            id3.disabled = enable;

        }
        var ttt = 0;
        //****** Begin Input Function  *******
        function input(data) {
            ShowInfo2("");
			StopSound();
            if (data == "7777") {

                var lastCT = GetLastCT();
                if (lastCT == "" || ctArray.length == 0) {
                    ShowALL("Material CT 已清完，请刷新页面!");
                    document.getElementById("<%=txtScanQty.ClientID%>").value = 0;
                    return;
                }
                //setRowSelectedOrNotSelectedByIndex(index - 2, false, grvProductClientID);
                for (var i = 0; i < ctArray.length; i++) {
                    if (ctArray[i] == lastCT) {
                        ctArray.splice(i, 1);
                        setRowSelectedOrNotSelectedByIndex(index - 2, false, grvProductClientID);
                    }
                }
                clearTable();
                for (var i = 0; i < ctArray.length; i++) {
                    AddTableByValue(ctArray[i]);
                }
                document.getElementById("<%=txtScanQty.ClientID%>").value = ctArray.length;
                return;
            } // if (data == "7777")
            if (!isEndFirstInput) {
                GetCollection();
                GetCollectStageValue();
                GetMaterialTypeValue();
                if (collection == "") {
                    ShowALL(msgInputCollectionType);
                    //ShowInfo(msgInputCollectionType);
                    getAvailableData("input");
                    return;
                }
                if (collectStage == "") {
                    ShowALL(msgInputCollectionStage);
                 //   ShowInfo(msgInputCollectionStage);
                    getAvailableData("input");
                    return;
                }
                if (materialType == "") {
                    //ShowMessage(msgInputMaterial);
                    ShowALL(msgInputMaterial);
                    getAvailableData("input");
                    return;
                }
            } //  if (!isEndFirstInput) 
            specNo = document.getElementById("<%=txtSpecNo.ClientID%>").value;
            lotNo = document.getElementById("<%=txtLotNo.ClientID%>").value;
            qty = document.getElementById("<%=txtQty.ClientID%>").value;
            if (collection == "Input") {
                InputMode(data, materialType);
            }
            else {
                OutMode(data);
            }
           callNextInput();
            return;
        }
        //****** End Input Function  *******



        function InputMode(data, _materialType) {
            if (lotNo == "") {
                if (_materialType == "CPU") {
                    var regexLotNo = /^1T[A-Z0-9]*$/;
                    if (!regexLotNo.test(data)) {
                        ShowALL(msgInputLotNo);
                        getAvailableData("input");
                        return;
                    }
                    else {
                        lotNo = data.replace("1T", "");
                        beginWaitingCoverDiv();
                        document.getElementById("<%=hidLotNo.ClientID%>").value = lotNo;
                        document.getElementById("<%=btnInputFirstSN.ClientID%>").click();
                        callNextInput();
                        return;
                    }
                }
                else {
                    var regexLotNo = /^[A-Z0-9]{10}$/;
                    if (!regexLotNo.test(data)) {
                        ShowALL(msgInputLotNo);
                        getAvailableData("input");
                        return;
                    }
                    else {
                        lotNo = data;
                        beginWaitingCoverDiv();
                        document.getElementById("<%=hidLotNo.ClientID%>").value = lotNo;
                        document.getElementById("<%=btnInputFirstSN.ClientID%>").click();
                        callNextInput();
                        return;
                    }
                }

            }
            // 9999
            if (data == "9999") {
                if (document.getElementById("<%=txtScanQty.ClientID%>").value == 0)
                { ShowMessage("請刷入MaterialCT"); }
                else {
                    beginWaitingCoverDiv();
                    document.getElementById("<%=hidCtList.ClientID%>").value = ctArray.join(",");
                    document.getElementById("<%=btnSave.ClientID%>").click();
                }
                callNextInput();
                return;
            }

            // Add CT
            if (data.length <12 || (data.length>14 && data.length<90)) {
              //  ShowMessage(msgInputCT);
			    ShowALL(msgInputCT);
                callNextInput();
            }
            else {
                 if(data.length>89) {
                     if (_materialType == "CPU") { data = data.substr(76, 13) }
                     else { data = data.substr(76, 14) }
				 }
                if (CheckExistData(data)) {
                    ShowMessage("重復刷入");
                    ShowInfo2("重復刷入");
                    callNextInput();
                    return;
                }
                else {
                    ShowInfo2("");
                    var q = parseInt(document.getElementById("<%=txtScanQty.ClientID%>").value, 10);
                    var cqty = parseInt(document.getElementById("<%=txtCombinedQty.ClientID%>").value, 10);
                    var qty = parseInt(document.getElementById("<%=txtQty.ClientID%>").value, 10);
                    if ((cqty + q) >= qty) {
                        ShowALL("刷入的MaterialCT數與已收料數，超過LotQty總數量!");
                       // ShowInfo("刷入的MaterialCT數與已收料數，超過LotQty總數量!");
                        callNextInput();
                        return;
                    }

                    document.getElementById("<%=hidInputCT.ClientID%>").value = data;
                    document.getElementById("<%=btnCheckCT.ClientID%>").click();
                    callNextInput();
                    return;
                }
            }


        }

        function OutMode(data) {
            if (lotNo == "") {
                if (data.length < 12 || (data.length > 14 && data.length < 90)) {
                    ShowMessage(msgInputCT);
                    ShowInfo2(msgInputCT);
                    getAvailableData("input");
                    return;
                }
                else {
                    if (data.length > 89) {
                        if (materialType == "CPU") { data = data.substr(76, 13) }
                        else { data = data.substr(76, 14) }
                    }
                    beginWaitingCoverDiv();
                    document.getElementById("<%=hidInputCT.ClientID%>").value = data;
                    document.getElementById("<%=btnInputFirstSN.ClientID%>").click();
                    callNextInput();
                    return;
                }

            }
            // 9999
            if (data == "9999") {

                if (document.getElementById("<%=txtScanQty.ClientID%>").value == 0)
                { ShowMessage("請刷入MaterialCT"); }
                else {
                    beginWaitingCoverDiv();
                    document.getElementById("<%=hidCtList.ClientID%>").value = ctArray.join(",");
                    document.getElementById("<%=btnSaveByOut.ClientID%>").click();
                }
                callNextInput();
                return;
            }

            // Add CT
            if (data.length < 12 || (data.length > 14 && data.length < 90)) {
                ShowMessage(msgInputCT);
                ShowInfo2(msgInputCT);
                callNextInput();
            }
            else {
                if (data.length > 89) {
                    data = data.substr(76, 13)
                }
                if (CheckExistData(data)) {
                    ShowMessage("重復刷入");
                    ShowInfo2("重復刷入");
                    callNextInput();
                    return;
                }
                else {
                    ShowInfo2("");
                    document.getElementById("<%=hidInputCT.ClientID%>").value = data;
                    document.getElementById("<%=btnCheckCTandLot.ClientID%>").click();
                    callNextInput();
                    return;
                }
            }

        }
        function ShowALL(msg) {
            ShowMessage(msg);
            ShowInfo2(msg);
        }
        function callNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }
     
    </script>

    <div>
        <center>
            <table width="95%" border="0">
                <tr>
                    <td align="left" width="33%">
                        <asp:Label ID="Label3" Width="20%" runat="server" CssClass="iMes_label_13pt" Text="收集類別:"></asp:Label>
                        <asp:DropDownList ID="drpCollection" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Input">依照 LotNo 收集</asp:ListItem>
                            <asp:ListItem Value="Output">依照 Material CT 收集</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left" width="33%">
                        <asp:Label ID="lblReceving" runat="server" Width="20%" CssClass="iMes_label_13pt"
                            Text="製程:"></asp:Label>
                        <iMES:CmbCollectStage ID="cmbCollectStage" runat="server" Width="70" IsPercentage="true"
                            ValueType="CollectStage" />
                    </td>
                    <td align="left" width="33%">
                        <asp:Label ID="Label1" runat="server" Width="40%" CssClass="iMes_label_13pt" Text="Material Type:"></asp:Label>
                        <iMES:CmbCollectStage ID="cmbMaterialType" runat="server" Width="50" IsPercentage="true"
                            ValueType="MaterialType" />
                    </td>
                </tr>
            </table>
            <fieldset style="width: 95%" align="center">
                <legend id="lblProductInfo" runat="server" style="color: Blue" class="iMes_label_13pt">
                    <asp:Label ID="Label2" CssClass="iMes_label_13pt" runat="server" Text="Lot & Scan Information"></asp:Label>
                </legend>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr style="height: 30px" align="left">
                        <td>
                            <asp:Label ID="lblLotNo" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="LotNo"></asp:Label>
                            <asp:TextBox ID="txtLotNo" runat="server" Style="width: 22%" CssClass="iMes_textbox_input_Disabled"
                                IsClear="true" ReadOnly="True" />
                            <asp:Label ID="lblSpecNo" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="SpecNo"></asp:Label>
                            <asp:TextBox ID="txtSpecNo" runat="server" Style="width: 23%" CssClass="iMes_textbox_input_Disabled"
                                IsClear="true" ReadOnly="True" />
                        </td>
                    </tr>
                    <tr style="height: 30px" align="left">
                        <td>
                            <asp:Label ID="lblQty" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="Qty"></asp:Label>
                            <asp:TextBox ID="txtQty" runat="server" Style="width: 22%" CssClass="iMes_textbox_input_Disabled"
                                IsClear="true" ReadOnly="True" />
                            <asp:Label ID="Label4" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="Combined Qty"></asp:Label>
                            <asp:TextBox ID="txtCombinedQty" runat="server" Style="width: 23%" CssClass="iMes_textbox_input_Disabled"
                                IsClear="true" ReadOnly="True" />
                            <asp:Label ID="Label5" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="Scan Qty"></asp:Label>
                            <asp:TextBox ID="txtScanQty" runat="server" Style="width: 20%" CssClass="iMes_textbox_input_Disabled"
                                IsClear="true" ReadOnly="True" Text="0" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 95%">
                <legend id="Legend1" runat="server" style="color: Blue" class="iMes_label_13pt">
                    <asp:Label ID="Label6" CssClass="iMes_label_13pt" runat="server" Text="Material List"></asp:Label>
                </legend>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="gridViewUP" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iMES:GridViewExt ID="grvProduct" runat="server" AutoGenerateColumns="false" AutoHighlightScrollByValue="true"
                                        GvExtWidth="99%" GvExtHeight="240px" OnGvExtRowClick="" OnGvExtRowDblClick=""
                                        SetTemplateValueEnable="true" GetTemplateValueEnable="true" HighLightRowPosition="3"
                                        Style="top: 0px; left: 0px">
                                        <Columns>
                                            <asp:BoundField DataField="Material CT" />
                                        </Columns>
                                    </iMES:GridViewExt>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnInputFirstSN" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center" style="height: 50px">
                <tr valign="middle">
                    <td width="20%" align="left">
                        <asp:Label ID="lbDataEntry" runat="server" class="iMes_DataEntryLabel"> </asp:Label>
                    </td>
                    <td width="80%" align="left">
                        <iMES:Input ID="txtDataEntry" runat="server" ProcessQuickInput="true" Width="98%"
                            CanUseKeyboard="true" IsClear="true" IsPaste="true" />
                    </td>
                </tr>
            </table>
            <table width="95%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td>
                    </td>
                    <asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnInputFirstSN" runat="server" Text="Button" OnClick="btnInputFirstSN_Click"
                                Style="display: none" />
                            <asp:Button ID="btnSave" runat="server" Text="Button" OnClick="btnSave_Click" Style="display: none" />
                            <asp:Button ID="btnCheckCT" runat="server" Text="Button" OnClick="btnCheckCT_Click"
                                Style="display: none" />
                            <asp:Button ID="btnCheckCTandLot" runat="server" Text="Button" OnClick="btnCheckCTandLot_Click"
                                Style="display: none" />
                            <asp:Button ID="btnSaveByOut" runat="server" Text="Button" OnClick="btnSaveByOut_Click"
                                Style="display: none" />
                            <input type="hidden" runat="server" id="hidBoxId" />
                            <input type="hidden" runat="server" id="hidSpecNo" />
                            <input type="hidden" runat="server" id="hidLotNo" />
                            <input type="hidden" runat="server" id="hidQty" />
                            <input type="hidden" runat="server" id="hidComQty" />
                            <input type="hidden" runat="server" id="hidMaterialTypeID" />
                            <input type="hidden" runat="server" id="hidCollectStageID" />
                            <input type="hidden" runat="server" id="hidMaterialTypeValue" />
                            <input type="hidden" runat="server" id="hidCollectStageValue" />
                            <input type="hidden" runat="server" id="hidCtList" />
                            <input type="hidden" runat="server" id="hidInputCT" />
                            <input type="hidden" runat="server" id="hidCollection" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </tr>
            </table>
        </center>
    </div>
    <div align="center" >
        <textarea id="MessageTextArea" class="iMes_Master_msgbox" readonly="readOnly" tabindex="2" style="height:80px"
            runat="server"></textarea>
      <script language="javascript" type="text/javascript">
          function ShowInfo2(message, statusColor) {
              var obj = document.getElementById("<%=MessageTextArea.ClientID %>");

              if (statusColor) {
                  obj.style.color = statusColor;
              }
              else obj.style.color = "red";

              obj.value = message;
          }            
        </script>
         <input type="hidden" runat="server" id="hidMaterialID" />
         <input type="hidden" runat="server" id="hidRecevingID" />
    </div>
</asp:Content>
