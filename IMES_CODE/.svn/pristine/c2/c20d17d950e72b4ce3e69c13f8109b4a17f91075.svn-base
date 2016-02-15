<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine COA and DN
 * CI-MES12-SPEC-PAK-Combine COA and DN.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="btcoolabel.aspx.cs" Inherits="DOCK_BTCooLabel" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
         <Services>
			<asp:ServiceReference Path="Service/CooLabelWebService.asmx" />
        </Services>
    </asp:ScriptManager>
    <script  type="text/javascript" language= "javascript ">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                //ShowMessage(args.get_error().message);
                //ShowInfo(args.get_error().message);
                if (args.get_error().message.substring(0, 51) == "Sys.WebForms.PageRequestManagerParserErrorException") {
                    window.location.reload();   //出现Session丢失时的错误处理，可以自己定义。 
                }
                else {
                    alert("发生错误!原因可能是数据不完整,或网络延迟。 ");   //其他错误的处理。 
                }
                args.set_errorHandled(true);
            }
        }   
    </script>
    <script type="text/javascript" language="javascript"  >
    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var pCode = '<%=code%>';
    var lastId = 1;
    var table = "";
    var inputObj;
    var username = '<%=UserName%>';
    var login = '<%=Login%>';
    var accountid = '<%=AccountId%>';
    var msgModelCheck = '<%=this.GetLocalResourceObject(Pre + "_msgModelCheck").ToString() %>';
    var msgModelNoFind = '<%=this.GetLocalResourceObject(Pre + "_msgModelNoFind").ToString() %>';
    var msgModeChange = '<%=this.GetLocalResourceObject(Pre + "_msgModeChange").ToString() %>';
    var ModeChange = "";
    var successTemp = "";
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgQualityCheck = '<%=this.GetLocalResourceObject(Pre + "_msgQualityCheck").ToString() %>';
    var msgModeWrong = '<%=this.GetLocalResourceObject(Pre + "_msgModeWrong").ToString() %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    var msgHaveBind = '<%=this.GetLocalResourceObject(Pre + "_msgHaveBind").ToString() %>'; 
    var index = 0;
    var cn = 1;
    window.onload = function() {
        if (document.getElementById("<%=lbJapan.ClientID%>").innerText == "Japan") {
            cn = 0;
        }
        document.getElementById("<%=HISCHECK.ClientID%>").value = "false";
        document.getElementById("<%=lbJapan.ClientID%>").innerText = "";
        callNextInput();
    }
    function drpOnChange() {
        if (document.getElementById("<%=HISCHECK.ClientID%>").value == "true") {
            document.getElementById("<%=HDN.ClientID%>").value = "";
            document.getElementById("<%=txtQTY.ClientID%>").value = "";
            getCommonInputObject().focus();
            return;
        }
        var obj = document.getElementById("<%=drpDN.ClientID%>");
        if (obj.value != "") {
            document.getElementById("<%=HDN.ClientID%>").value = obj.value;
            document.getElementById("<%=btnGetQty.ClientID%>").click();
        }
        else {
            document.getElementById("<%=HDN.ClientID%>").value = "";
            document.getElementById("<%=txtQTY.ClientID%>").value = "";
        }
        getCommonInputObject().focus();
    }
    function callNextInput() {
        getCommonInputObject().value = "";
        getCommonInputObject().focus();
        getAvailableData("input");
    }
    function DisplsyMsg(src) {
        ShowMessage(src);
        ShowInfo(src);
    }
    function input(data) {
        ShowInfo("");
        var inputTextBox1 = data.trim();
        inputTextBox1 = inputTextBox1.toUpperCase();
        document.getElementById("<%=HQCView.ClientID%>").value = "false";
        
        if (inputTextBox1 == "7777")
        {
            document.getElementById("<%=chkNotDN.ClientID%>").checked = false;
            document.getElementById("<%=HISCHECK.ClientID%>").value = "false";
            document.getElementById("<%=chkNot9999.ClientID%>").checked = false;
            getResetAll();
            ShowSuccessfulInfo(true, "Reset!");
            return;
        }
        if (inputTextBox1 == "9999"
        && !(document.getElementById("<%=HSN.ClientID%>").value == "" && document.getElementById("<%=HPROD.ClientID%>").value == "")
        ) {
            if (document.getElementById("<%=HISCHECK.ClientID%>").value == "true") {
                document.getElementById("<%=txtQTY.ClientID%>").value = "";
                
            }
            if (document.getElementById("<%=HISCHECK.ClientID%>").value != "true") {
                if (document.getElementById("<%=HDN.ClientID%>").value == "") {
                    DisplsyMsg("Delivery is empty!");
                    getResetValue();
                    callNextInput();
                    return;
                }
            } 
            document.getElementById("<%=btnSave.ClientID%>").click();
            return;
        }
        if (inputTextBox1 == "9999") {
            DisplsyMsg("Please input product ID or customer SN!");
            getResetAll();
            return;
        }
        if (document.getElementById("<%=HSN.ClientID%>").value == ""
        && document.getElementById("<%=HPROD.ClientID%>").value == "") {
            if (inputTextBox1.length == 9 || inputTextBox1.length == 10) {
                pattCustSN1 = /^CN.{7}$/;
                pattCustSN2 = /^CN.{8}$/;
                //if (pattCustSN1.exec(inputTextBox1) || pattCustSN2.exec(inputTextBox1)) {
				if (CheckCustomerSN(inputTextBox1)) {
                }
                else {
                    if (inputTextBox1.length == 10) {
                        document.getElementById("<%=HPROD.ClientID%>").value = inputTextBox1.substr(0, 9);
                    }
                    else {
                        document.getElementById("<%=HPROD.ClientID%>").value = inputTextBox1;
                    }
                }
            }
            if (inputTextBox1.length == 10) {
                pattCustSN1 = /^CN.{8}$/;
                //if (pattCustSN1.exec(inputTextBox1)) {
				if (CheckCustomerSN(inputTextBox1)) {
                    document.getElementById("<%=HSN.ClientID%>").value = inputTextBox1;
                }
            }
            if (document.getElementById("<%=HSN.ClientID%>").value == "" && document.getElementById("<%=HPROD.ClientID%>").value == "") {
                DisplsyMsg("Wrong Code!");
                getResetAll();
                return;
            }
            document.getElementById("<%=btnGetProduct.ClientID%>").click();
            callNextInput();
            return;
        }
        DisplsyMsg("Wrong Code!");
        callNextInput();
    }
    function getProduct() {
        document.getElementById("<%=txtCustomerSN.ClientID%>").value = document.getElementById("<%=HSN.ClientID%>").value;
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HPROD.ClientID%>").value;
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HMODE.ClientID%>").value;
        if (document.getElementById("<%=HJAPAN.ClientID%>").value == "japantrue") {
            if (cn == 1) {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "日本";
            }
            else {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "Japan";
            }
        }
        else {
            if (cn == 1) {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "非日本";
            }
            else {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "Not Japan";
            }
        }
        document.getElementById("<%=txtMO.ClientID%>").value = document.getElementById("<%=HMO.ClientID%>").value;
        document.getElementById("<%=txtTotal.ClientID%>").value = document.getElementById("<%=HTOTAL.ClientID%>").value;
        document.getElementById("<%=txtPass.ClientID%>").value = document.getElementById("<%=HPASS.ClientID%>").value;
        if (document.getElementById("<%=HISCHECK.ClientID%>").value == "false") {
            if (document.getElementById("<%=drpDN.ClientID%>").length < 2) {
                DisplsyMsg("Not Found PoData for " + document.getElementById("<%=HMODE.ClientID%>").value);
                document.getElementById("<%=txtQTY.ClientID%>").value = "";
                getResetValue();
                callNextInput();
                return;
            }
        } else {
            document.getElementById("<%=txtQTY.ClientID%>").value = "";
            var temp = document.getElementById("<%=HMODE.ClientID%>").value;
            
            document.getElementById("<%=btnSave.ClientID%>").click();
            return;
        }
        if (document.getElementById("<%=chkNot9999.ClientID%>").checked == true) {
            if(!(document.getElementById("<%=HSN.ClientID%>").value == "" && document.getElementById("<%=HPROD.ClientID%>").value == ""))
            {
                if (document.getElementById("<%=HDN.ClientID%>").value == "") {
                    DisplsyMsg("Delivery is empty!");
                    getResetValue();
                    callNextInput();
                    return;
                }
                document.getElementById("<%=btnSave.ClientID%>").click();
            }
        }
        getCommonInputObject().focus();
    }
    function getProductFresh(clear) {
        document.getElementById("<%=txtCustomerSN.ClientID%>").value = document.getElementById("<%=HSN.ClientID%>").value;
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=txtProductID.ClientID%>").value = document.getElementById("<%=HPROD.ClientID%>").value;
        document.getElementById("<%=HPROD.ClientID%>").value = "";
        document.getElementById("<%=txtModel.ClientID%>").value = document.getElementById("<%=HMODE.ClientID%>").value;
        
        if (document.getElementById("<%=HJAPAN.ClientID%>").value == "japantrue") {
            if (cn == 1) {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "日本";
            }
            else {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "Japan";
            }
        }
        else {
            if (cn == 1) {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "非日本";
            }
            else {
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "Not Japan";
            }
        }
        document.getElementById("<%=txtMO.ClientID%>").value = document.getElementById("<%=HMO.ClientID%>").value;
        document.getElementById("<%=txtTotal.ClientID%>").value = document.getElementById("<%=HTOTAL.ClientID%>").value;
        document.getElementById("<%=txtPass.ClientID%>").value = document.getElementById("<%=HPASS.ClientID%>").value;
        if (document.getElementById("<%=HISCHECK.ClientID%>").value == "false") {
            if (document.getElementById("<%=drpDN.ClientID%>").length < 2) {
                DisplsyMsg("Not Found PoData for " + document.getElementById("<%=HMODE.ClientID%>").value);
            }
        } else {
            var temp = document.getElementById("<%=HMODE.ClientID%>").value;
            
            return;
        }
        if (clear == "true") {
            //document.getElementById("<%=HMODE.ClientID%>").value = "";
            document.getElementById("<%=btnRefresh2.ClientID%>").click();
        }
        getCommonInputObject().focus();
    }
    function getQTY(qty) {
        if (document.getElementById("<%=HISCHECK.ClientID%>").value != "true") {
            document.getElementById("<%=txtQTY.ClientID%>").value = qty;
        }
        else {
            document.getElementById("<%=txtQTY.ClientID%>").value = "";
        }
        
        getCommonInputObject().focus();
    }
    function CheckClick9999(singleChk) {
        getCommonInputObject().focus();
        if (singleChk.checked) {
            if (document.getElementById("<%=HISCHECK.ClientID%>").value != "true") {
                if (!(document.getElementById("<%=HSN.ClientID%>").value == "" && document.getElementById("<%=HPROD.ClientID%>").value == "")) {
                    if (document.getElementById("<%=HDN.ClientID%>").value == "") {
                        DisplsyMsg("Delivery is empty!");
                        getResetValue();
                        callNextInput();
                        return;
                    }
                    document.getElementById("<%=btnSave.ClientID%>").click();
                }
            }
        }
    }
    
    function CheckClick(singleChk) {

        try {
            if (singleChk.checked) {
                var obj = document.getElementById("<%=drpDN.ClientID%>");
                obj.selectedIndex = 0;
                var len = obj.length;
                for (var iRow = 1; iRow < len; iRow++) {
                    obj.remove(1);
                }
                obj.selectedIndex = 0;
                document.getElementById("<%=txtQTY.ClientID%>").value = "";
                document.getElementById("<%=HISCHECK.ClientID%>").value = "true";
                document.getElementById("<%=chkNotDN.ClientID%>").checked = true;
                if (document.getElementById("<%=HSN.ClientID%>").value != ""
                    || document.getElementById("<%=HPROD.ClientID%>").value != "") {
                    var temp = document.getElementById("<%=HMODE.ClientID%>").value;
                    if (temp != "") {
                      
                        document.getElementById("<%=btnSave.ClientID%>").click();
                    }
                }
                else {
                    document.getElementById("<%=HDN.ClientID%>").value = "";
                    document.getElementById("<%=HPROD.ClientID%>").value = "";
                    document.getElementById("<%=lbJapan.ClientID%>").innerText = "";
                    document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
                    document.getElementById("<%=txtProductID.ClientID%>").value = "";
                    document.getElementById("<%=txtModel.ClientID%>").value = "";
                    document.getElementById("<%=txtQTY.ClientID%>").value = "";


                    document.getElementById("<%=txtMO.ClientID%>").value = "";
                    document.getElementById("<%=txtTotal.ClientID%>").value = "";
                    document.getElementById("<%=txtPass.ClientID%>").value = "";
                    ShowInfo("Please re-input product ID or customer SN!");
                }
            }
            else {
                /*ShowInfo("");
                document.getElementById("<%=HISCHECK.ClientID%>").value = "false";
                document.getElementById("<%=chkNotDN.ClientID%>").checked = false;
                document.getElementById("<%=HDN.ClientID%>").value = "";
                document.getElementById("<%=HMODE.ClientID%>").value = "";
                if (document.getElementById("<%=txtCustomerSN.ClientID%>").value != "" && document.getElementById("<%=HSN.ClientID%>").value != "") {
                    document.getElementById("<%=btnGetProduct.ClientID%>").click();
                }
                else {
                    document.getElementById("<%=HSN.ClientID%>").value = "";
                    ShowInfo("Please re-input product ID or customer SN!");
                }*/
                document.getElementById("<%=HISCHECK.ClientID%>").value = "false";
                document.getElementById("<%=chkNotDN.ClientID%>").checked = false;
                document.getElementById("<%=HDN.ClientID%>").value = "";
                document.getElementById("<%=HMODE.ClientID%>").value = "";
                document.getElementById("<%=HSN.ClientID%>").value = "";
                document.getElementById("<%=HPROD.ClientID%>").value = "";
                document.getElementById("<%=lbJapan.ClientID%>").innerText = "";
                document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
                document.getElementById("<%=txtProductID.ClientID%>").value = "";
                document.getElementById("<%=txtModel.ClientID%>").value = "";
                document.getElementById("<%=txtQTY.ClientID%>").value = "";


                document.getElementById("<%=txtMO.ClientID%>").value = "";
                document.getElementById("<%=txtTotal.ClientID%>").value = "";
                document.getElementById("<%=txtPass.ClientID%>").value = "";
                ShowInfo("Please re-input product ID or customer SN!");
            }
            getCommonInputObject().focus();
        } 
        catch (e) {
            alert(e.description);
        }
    }
    
    function getResetAndRefresh(display) {

        if (display == "true") {
            var temp = document.getElementById("<%=HSN.ClientID%>").value;
            if (temp == "") {
                temp = document.getElementById("<%=HPROD.ClientID%>").value;
            }
            var successTemp = "";
            if (temp != "") {
                successTemp = "[" + temp + "] " + msgSuccess;
            }
            if (document.getElementById("<%=HQCView.ClientID%>").value == "true") {
                successTemp = successTemp + "\n" + msgQualityCheck;
            }
            if (successTemp != "") {
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
        }
        else {
            ShowInfo("Please re-input product ID or customer SN!");
        }
        document.getElementById("<%=HPROD.ClientID%>").value = "";
        document.getElementById("<%=HMO.ClientID%>").value = "";
        document.getElementById("<%=HTOTAL.ClientID%>").value = "";
        document.getElementById("<%=HPASS.ClientID%>").value = "";
        document.getElementById("<%=HQCView.ClientID%>").value = "false";
        if (document.getElementById("<%=HISCHECK.ClientID%>").value == "true") {
            var obj = document.getElementById("<%=drpDN.ClientID%>");
            obj.selectedIndex = 0;
            var len = obj.length;
            for (var iRow = 1; iRow < len; iRow++) {
                obj.remove(1);
            }
            obj.selectedIndex = 0;
            document.getElementById("<%=txtQTY.ClientID%>").value = "";
        }
        //else {
        //document.getElementById("<%=btnRefresh.ClientID%>").click();
        if (document.getElementById("<%=HSN.ClientID%>").value != "") {
            document.getElementById("<%=btnGetQtyRefresh.ClientID%>").click();
        }
        // }
        //document.getElementById("<%=HSN.ClientID%>").value = "";
        callNextInput();
    }
    
    function getResetAll() {
        getResetText();
        getResetValue();
        callNextInput();
    }
    function getResetText()
    {
        var obj = document.getElementById("<%=drpDN.ClientID%>");
        obj.selectedIndex = 0;
        var len = obj.length;
        for (var iRow = 1; iRow < len; iRow++) {
            obj.remove(1);
        }
        obj.selectedIndex = 0;
        //document.getElementById("<%=chkNotDN.ClientID%>").checked = false;
        document.getElementById("<%=lbJapan.ClientID%>").innerText = "";
        document.getElementById("<%=txtCustomerSN.ClientID%>").value = "";
        document.getElementById("<%=txtProductID.ClientID%>").value = "";
        document.getElementById("<%=txtModel.ClientID%>").value = "";
        document.getElementById("<%=txtQTY.ClientID%>").value = "";


        document.getElementById("<%=txtMO.ClientID%>").value = "";
        document.getElementById("<%=txtTotal.ClientID%>").value = "";
        document.getElementById("<%=txtPass.ClientID%>").value = "";
    }
    function getResetValue()
    {
        //document.getElementById("<%=HISCHECK.ClientID%>").value = "false";
        document.getElementById("<%=HQCView.ClientID%>").value = "false";
        document.getElementById("<%=HSN.ClientID%>").value = "";
        document.getElementById("<%=HPROD.ClientID%>").value = "";
        document.getElementById("<%=HDN.ClientID%>").value = "";
        document.getElementById("<%=HMODE.ClientID%>").value = "";
        
        document.getElementById("<%=HMO.ClientID%>").value = "";
        document.getElementById("<%=HTOTAL.ClientID%>").value = "";
        document.getElementById("<%=HPASS.ClientID%>").value = "";
    }
    function getDNBind() {
        var temp = "";
        if (document.getElementById("<%=HSN.ClientID%>").value != "")
        {
            temp = document.getElementById("<%=HSN.ClientID%>").value;
        }
        else
        {
            temp = document.getElementById("<%=HPROD.ClientID%>").value;
        }
        temp = "[" + temp + "]";
        temp = temp + msgHaveBind;
        DisplsyMsg(temp);
        getResetAll();
    }
    
   
    </script>

 <div>
   <center >
   <table width="100%" border="0" >
    <tr>
        <td align="left" >
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Label ID="lbDN" Width = "11%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
                <asp:DropDownList ID="drpDN" runat="server"  Width="60%"   ></asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkNotDN" onclick="CheckClick(this)" runat="server" 
                        BackColor="#D2D2D2" BorderColor="#D2D2D2" BorderStyle="None" 
                        BorderWidth="0px" Visible="False"  />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        </tr>
         <tr>
         <td>
            <asp:Label ID="lbPackQty"  Width = "11%" runat="server" CssClass="iMes_label_13pt"></asp:Label>
            <asp:TextBox ID="txtQTY" runat="server"  style="width:20%"    CssClass="iMes_textbox_input_Disabled"
                    IsClear="true" ReadOnly="True" />
                    
        </td>
    </tr>
    <tr>
        <td align="left" >
        <hr>
        </td>
    </tr>
    <tr>
        <td align="left" >
        <fieldset style="width: auto">
            <legend>
             <asp:Label ID="lbProductInfo" runat="server"></asp:Label></legend>
             <table width="100%">
            <tr>
                <td style="width:10%">
                    <asp:Label ID="lbCustomerSN"   runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td style="width:32%">
                    <asp:TextBox ID="txtCustomerSN" runat="server"    Width="330px"  CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                            </td>
                    <td style="width:13%">
                    <asp:Label ID="lbProductID"  runat="server" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td  style="width:32%">
                    <asp:TextBox ID="txtProductID" runat="server"     Width="330px"  CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True"/>
                    </td>
             </tr>
           <tr>
                <td style="width:10%">
                    <asp:Label ID="lbModel"   runat="server" CssClass="iMes_label_13pt"></asp:Label>
                 </td>
                  <td style="width:32%">
                    <asp:TextBox ID="txtModel" runat="server"   Width="330px"  CssClass="iMes_textbox_input_Disabled"
                            IsClear="true" ReadOnly="True" />        
                </td>
                <td style="width:13%"> 
                </td>
                <td style="width:32%"> 
                <asp:Label ID="lbJapan"   runat="server" CssClass="iMes_label_30pt_Red"></asp:Label>
                </td>
            </tr>
           
            <tr>
                 <td>
                <br></br>
                </td>
            </tr>
             <tr>
             <td style="width:10%">
             <asp:Label ID="Label1"  runat="server" CssClass="iMes_label_13pt">Mo:</asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtMO" runat="server" Width="330px"  CssClass="iMes_textbox_input_Disabled"
            IsClear="true" ReadOnly="True"/>
            </td>
            <td>
             <asp:Label ID="Label2"  runat="server" CssClass="iMes_label_13pt">Total:</asp:Label>
            <asp:TextBox ID="txtTotal" runat="server" Width="100px"  CssClass="iMes_textbox_input_Disabled"
            IsClear="true" ReadOnly="True"/>
            </td>
             <td>
            <asp:Label ID="Label3"  runat="server" CssClass="iMes_label_13pt">Pass:</asp:Label>
             <asp:TextBox ID="txtPass" runat="server" Width="100px"  CssClass="iMes_textbox_input_Disabled"
            IsClear="true" ReadOnly="True"/>
            </td>
            </tr>
           </table>
         </fieldset>
        </td>
    </tr>
     <tr>
         <td>
            <br>
        </td>
      </tr>  
    <tr>
         <td>
            <br>
        </td>
      </tr>  
      <tr>
         <td>
            <br>
        </td>
      </tr>  
      <tr>
         <td>
            <br>
        </td>
      </tr>  
      <tr>
         <td>
            <br>
        </td>
      </tr>  
       <tr>
         <td>
            <br>
        </td>
      </tr>  
       <tr>
         <td>
            <br>
        </td>
      </tr>  
    <tr>
        <td align="left" colspan="1">
            <asp:Label ID="lbDataEntry" Width = "11%" runat="server" CssClass="iMes_DataEntryLabel"></asp:Label>
            <iMES:Input ID="TextBox1" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                                Width="45%" IsClear="true" IsPaste="true" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox 
                ID="chkNot9999" onclick="CheckClick9999(this)" runat="server" 
                        BackColor="#D2D2D2" BorderColor="#D2D2D2" BorderStyle="None" 
                        BorderWidth="0px" Text="Don't scan '9999'" /> 
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
    </tr>
    <tr>
         <td>
            <br>
        </td>
        <td>
            <br>
        </td>
        <td>
            <br>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="updatePanelAll" runat="server"  RenderMode="Inline" >
	            <ContentTemplate>
	                <button id="btnGetProduct" runat="server" type="button" style="display: none" />
	                <button id="btnGetQty" runat="server" type="button" style="display: none" />
	                <button id="btnGetQtyRefresh" runat="server" type="button" style="display: none" />
	                <button id="btnSave" runat="server" type="button" style="display: none" />
	                <button id="btnRefresh" runat="server" type="button" style="display: none" />
	                <button id="btnRefresh2" runat="server" type="button" style="display: none" />
	                <input type="hidden" runat="server" id="HSN" /> 
	                <input type="hidden" runat="server" id="HPROD" />
	                <input type="hidden" runat="server" id="HMODE" /> 
	                <input type="hidden" runat="server" id="HJAPAN" />
	                <input type="hidden" runat="server" id="HDN" />
	                <input type="hidden" runat="server" id="HISCHECK" />
	                <input type="hidden" runat="server" id="HMO" />
	                <input type="hidden" runat="server" id="HTOTAL" />
	                <input type="hidden" runat="server" id="HPASS" />
	                <input type="hidden" runat="server" id="HQCView" />
	            </ContentTemplate>   
            </asp:UpdatePanel> 
        </td>
    </tr>
    </table>
    </center>
</div>

</asp:Content>