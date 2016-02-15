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
    CodeFile="LotCollectionforStorage.aspx.cs" Inherits="FA_LotCollectionforStorage"    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPageMaintain.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" runat="Server">
  <script type="text/javascript" src="../CommonControl/jquery/js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>


    <script type="text/javascript" language="javascript">

        var editor;
        var customer;
        var pCode;
        var boxId;
        var specNo;
        var lotNo;
        var qty;
        var receving;
        var material;
        var msgInputMaterial = '<%=this.GetLocalResourceObject(Pre + "_msgInputMaterial") %>';
        var msgInputReceving = '<%=this.GetLocalResourceObject(Pre + "_msgInputReceving") %>';
        var msgInputBoxId = '<%=this.GetLocalResourceObject(Pre + "_msgInputBoxId") %>';
        var msgInputLotNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputLotNo") %>';
        var msgInputSpecNo = '<%=this.GetLocalResourceObject(Pre + "_msgInputSpecNo") %>';
        var msgInputQty = '<%=this.GetLocalResourceObject(Pre + "_msgInputQty") %>';
        window.onload = function() {
            CallNextInput();
            editor = "<%=userId %>";
        }
        window.onbeforeunload = function() {
        }
        function ExitPage() {
        }
        function IsNumber(src) {
            var regNum = /^[0-9]+[0-9]*]*$/;
            return regNum.test(src);
        }

        function CheckData(data, materialtype) {
            var txtId = "";
            if (materialtype == "CPU") {
                var regexBoxId = /^1B[A-Z0-9]{8}$/;
                //var regexLotNo = /^1T[A-Z0-9]{8}$/;
                var regexLotNo = /^1T[A-Z0-9]*$/;
                var regexQty = /^Q[0-9]*$/;
                var regxSpecNo = /^S/;
                if (regexBoxId.test(data)) { txtId = "BoxId"; }
                else if (regexLotNo.test(data)) { txtId = "LotNo"; }
                else if (regexQty.test(data)) { txtId = "Qty"; }
                else if (regxSpecNo.test(data)) { txtId = "SpecNo"; }
            }
            else {
                var regexBoxId = /^[A-Z0-9]{10}$/;
                var regexQty = /^Q[0-9]*$/;
                var regxSpecNo = /^[\S]{11}$/;

                if (regexBoxId.test(data)) { txtId = "BoxId"; }
              //  else if (regexLotNo.test(data)) { txtId = "LotNo"; }
                else if (regexQty.test(data)) { txtId = "Qty"; }
                // else if (regxSpecNo.test(data)) { txtId = "SpecNo"; }
               else if (data.trim().length==11||data.trim().length==13)
               {
                txtId = "SpecNo";
               }
            }
          
           
            return txtId;
        }

        function GetRecevingValue() {
            var id = document.getElementById("<%=hidRecevingID.ClientID %>").value;
            receving = document.getElementById(id)[document.getElementById(id).selectedIndex].text;
       
        }
        function GetMaterialValue() {
            var id = document.getElementById("<%=hidMaterialID.ClientID %>").value;
            material = document.getElementById(id)[document.getElementById(id).selectedIndex].text;
       
        }
       function CheckPositiveInteger(data) {
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
        function CheckAllInput() {
            var r = true;
             $('input[id^="txt"]').each(function() {
                if ($(this).val() == "")
                { r= false;return }
            });
            return r;
        }
        function ResetValue() {
            $('input[id^="txt"]').val('');
          //  $(':text').val('');
        }
        //****** Begin Input Function  *******
        function input(data) {
    
            ShowInfo2("");
            if (data == "7777") {
                ResetValue();
                CallNextInput();
                ShowInfo2("");
                return;
            }
         GetMaterialValue();
         GetRecevingValue();
            if (receving == "") {
                ShowMessage(msgInputReceving);
                ShowInfo2(msgInputReceving);
                getAvailableData("input");
                return;
            }
            if (material == "") {
                ShowMessage(msgInputMaterial);
                ShowInfo2(msgInputMaterial);
                getAvailableData("input");
                return;
            }
           
            if (CheckAllInput()) {
                if (data != "9999") {
                    ShowALL("Please input 9999!");
                    getAvailableData("input");
                }
                else {
                    beginWaitingCoverDiv();
                    PageMethods.Save($('#txtBoxId').val(), $('#txtSpecNo').val(), $('#txtLotNo').val(), $('#txtQty').val(), material, receving, editor, OnSuccess, OnError);
                     //public static  void Save(string boxId,string specNo,string lotNo,int qty,string material,string receving,string user)
                }
                return;
            }
            var txtId = CheckData(data, material);
            if (txtId == "") {
                ShowMessage("Wrong Input!");
                ShowInfo2("Wrong Input!");
                getAvailableData("input");
                return;
            }
            var idx = "#" + "txt" + txtId;
            if ($(idx).val() != "") {
                ShowALL(txtId + " 已輸入過!");
            }
            else {
                SetTxtBoxValue(txtId, data, material);
            }
            getAvailableData("input");
            return;

        }
        //****** End Input Function  *******
        function OnSuccess() {
            endWaitingCoverDiv();
            EndSave();
            CallNextInput();
        }
        function OnError(error) {
            endWaitingCoverDiv();
            ShowALL(error.get_message());
            CallNextInput();
        }
        function SetTxtBoxValue(item,data,_material) {
            var idx = "#" + "txt" + item;
            if (_material == "CPU") {
                switch (item) {
                    case "BoxId":
                        boxId = data.substr(2, 8);
                        PageMethods.CheckBoxId(boxId, OnSuccessForBoxId, OnErrorForBoxId);
                        break;
                    case "LotNo":
                        //    $(idx).val(data.substr(2, 8));
                        $(idx).val(data.replace("1T", ""));
                        break;
                    case "SpecNo":
                        //$(idx).val(data.substr(1, 1) + data.substr(3, 4));
                        $(idx).val(data.substr(1, data.length - 1).replace(/\s/g, ""));

                        break;
                    case "Qty":
                        $(idx).val(parseInt(data.substr(1, data.length - 1), 10));
                        break;
                    default:
                        break;
                }
            }
            else {
                switch (item) {
                    case "BoxId":
                        boxId = data;
                        PageMethods.CheckBoxId(boxId, OnSuccessForBoxId, OnErrorForBoxId);
                        $("#txtLotNo").val(data);
                        break;
                    case "LotNo":
                        //    $(idx).val(data.substr(2, 8));
                        $(idx).val(data);
                        break;
                    case "SpecNo":
                        //$(idx).val(data.substr(1, 1) + data.substr(3, 4));
                        $(idx).val(data);

                        break;
                    case "Qty":
                        $(idx).val(parseInt(data.substr(1, data.length - 1), 10));
                        break;
                    default:
                        break;
                }
            
            
            }
            
        }
        function OnSuccessForBoxId() {
            $("#txtBoxId").val(boxId);
        }
        function OnErrorForBoxId(error) {
            endWaitingCoverDiv();
            ShowALL(error.get_message());
            CallNextInput();
        }
        function CallNextInput() {
            getCommonInputObject().value = "";
            getCommonInputObject().focus();
            getAvailableData("input");
        }

        function ShowStage(obj) {
            var i = obj.selectedIndex;
            var s = obj[i].text;
            if (s != "") {
                ShowALL('選擇進料方式: ' + s);
            }

        }
        function ShowALL(msg) {
            ShowMessage(msg);
            ShowInfo2(msg);
        }
     
    </script>

    <div>
        <center>
            <table width="95%" border="0">
                <tr>
                    <td align="left" width="50%">
                        <asp:Label ID="lblReceving" runat="server" Width="15%" CssClass="iMes_label_13pt"
                            Text="進料方式:"></asp:Label>
                        <iMES:CmbCollectStage ID="cmbCollectStage" runat="server" Width="80" IsPercentage="true"
                            ValueType="FeedType" />
                    </td>
                    <td align="left" width="50%">
                        <asp:Label ID="Label1" runat="server" Width="15%" CssClass="iMes_label_13pt" Text="Material Type:"></asp:Label>
                        <iMES:CmbCollectStage ID="cmbCollectCPU" runat="server" Width="80" IsPercentage="true"
                            ValueType="MaterialType" />
                    </td>
                </tr>
            </table>
            <fieldset style="width: 95%" align="center">
                <legend id="lblProductInfo" runat="server" style="color: Blue" class="iMes_label_13pt">
                    <asp:Label ID="Label2" CssClass="iMes_label_13pt" runat="server" Text="Box Information"></asp:Label>
                </legend>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                    <tr style="height: 30px" align="left">
                        <td>
                            <asp:Label ID="lblBoxId" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="BoxId"></asp:Label>
                           
                            <input type="text" id="txtBoxId" readonly="readonly" style="width: 38%"  class="iMes_textbox_input_Disabled" value="" />
                            <asp:Label ID="lblSpecNo" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="SpecNo"></asp:Label>
                            <input type="text" id="txtSpecNo" readonly="readonly" style="width: 38%"  class="iMes_textbox_input_Disabled" value="" />
                            
                        </td>
                    </tr>
                    <tr style="height: 30px" align="left">
                        <td>
                            <asp:Label ID="lblLotNo" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="LotNo"></asp:Label>
                            <input type="text" id="txtLotNo" readonly="readonly" style="width: 38%"  class="iMes_textbox_input_Disabled" value="" />
                            <asp:Label ID="lblQty" Width="10%" runat="server" CssClass="iMes_label_13pt" Text="Qty"></asp:Label>
                            <input type="text" id="txtQty" readonly="readonly" style="width: 38%"  class="iMes_textbox_input_Disabled" value="" />
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
                
                </tr>
            </table>
        </center>
    </div>
     <div align="center" >
        <textarea id="MessageTextArea" class="iMes_Master_msgbox" readonly="readOnly" tabindex="2" style="height:100px"
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
