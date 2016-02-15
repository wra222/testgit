<%--
/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:CNCardStatusChange
 * CI-MES12-SPEC-PAK-CN Card Status Change.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
 --%>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CNCardStatusChange.aspx.cs" Inherits="PAK_CNCardStatusChange" Title="Untitled Page" %>
<%@ MasterType VirtualPath ="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

    <script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    <script type="text/javascript" src="../UserManager/commoncontrol/function/commonFunction.js"></script>
    <script language="JavaScript" type="text/javascript" src="../CommonControl/JS/calendar.js"></script>
    <asp:ScriptManager runat="server" ID="SM"  EnablePartialRendering="true">

    </asp:ScriptManager>
    <script type="text/javascript" language="javascript"  >
    var editor = '<%=userId%>';
    var customer = '<%=customer%>';
    var station = '<%=station%>';
    var outStatus = "";
    var inStatus = "";
    var table;
    var indexDN = 1;
    var initRowsCount = 6;
    var strBegNO = "";
    var strEndNO = "";
    var numBegNO = 0;
    var numEndNO = 0;
    var currentStatus = "";
    var msgCN_InputFormat = '<%=this.GetLocalResourceObject(Pre + "_msgCN_InputFormat").ToString() %>';
    var msgErrorSameStatus = '<%=this.GetLocalResourceObject(Pre + "_msgErrorSameStatus").ToString() %>';
    var msgA1 = '<%=this.GetLocalResourceObject(Pre + "_msgA1").ToString() %>';
    var msgSame = '<%=this.GetLocalResourceObject(Pre + "_msgSame").ToString() %>';
    var msgSelectTaget = '<%=this.GetLocalResourceObject(Pre + "_msgSelectTaget").ToString() %>';
    var msgInputTxt = '<%=this.GetLocalResourceObject(Pre + "_msgInputTxt").ToString() %>';
    var msgSuccess = '<%=Resources.iMESGlobalMessage.ResourceManager.GetString(Pre + "_msgSuccess") %>';
    window.onload = function() {
    table = document.getElementById("<%=gridview.ClientID %>");
    document.getElementById("<%=TextBox1.ClientID%>").focus();
    }

    function IsNumber(src) {
        var regNum = /^[0-9]+$/;
        var aa = regNum.test(src);
        return regNum.test(src);
    }
    function DisplsyMsg(src) {
        ShowMessage(src);
        ShowInfo(src);
    }
    function onTextBox1KeyDown() {
        strBegNO = "";
        ShowInfo("");
        if (event.keyCode == 9 || event.keyCode == 13) {
            var inputTextBox1 = document.getElementById("<%=TextBox1.ClientID%>").value.trim();
            inputTextBox1 = inputTextBox1.toUpperCase();
            
            if (inputTextBox1.length < 7 || inputTextBox1.trim() == "" 
            || !IsNumber(inputTextBox1.substr(inputTextBox1.length - 7))) {
                DisplsyMsg(msgCN_InputFormat);
                document.getElementById("<%=TextBox1.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                return;
            }
            strBegNO = inputTextBox1;
            numBegNO = parseInt(inputTextBox1.substr(inputTextBox1.length - 7), 10);
            strEndNO = "";
            numEndNO = 0;
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").focus();
        }
    }
    function GetTextData() {
        ShowInfo("");
        var inputTextBox1 = document.getElementById("<%=TextBox1.ClientID%>").value.trim();
        if (inputTextBox1.length < 7 || inputTextBox1.trim() == ""
        || !IsNumber(inputTextBox1.substr(inputTextBox1.length - 7))) {
            DisplsyMsg(msgCN_InputFormat);
            ResetPage();
            /*document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            document.getElementById("<%=TextBox1.ClientID%>").focus();*/
            return false;
        } else {
            strBegNO = inputTextBox1;
            numBegNO = parseInt(inputTextBox1.substr(inputTextBox1.length - 7), 10);
        }
        var inputTextBox2 = document.getElementById("<%=TextBox2.ClientID%>").value.trim();
        inputTextBox2 = inputTextBox2.toUpperCase();
        if (inputTextBox2.length < 7 || inputTextBox2.trim() == ""
        || !IsNumber(inputTextBox2.substr(inputTextBox2.length - 7))) {
            DisplsyMsg(msgCN_InputFormat);
            ResetPage();
            /*document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";*/
            return false;
        }
        strEndNO = inputTextBox2;
        numEndNO = parseInt(inputTextBox2.substr(inputTextBox2.length - 7), 10);
        var rangeToDisplay = "";
        if (numEndNO - numBegNO < 0) {
            var numTemp = numEndNO;
            numEndNO = numBegNO;
            numBegNO = numTemp;
            var strTemp = strEndNO;
            strEndNO = strBegNO;
            strBegNO = strTemp;
        }
        /*document.getElementById("<%=TextBox1.ClientID%>").value = "";
        document.getElementById("<%=TextBox2.ClientID%>").value = "";
        document.getElementById("<%=TextBox1.ClientID%>").focus();*/
       
    }
    function onTextBox2KeyDown() {
        strEndNO = "";
        ShowInfo("");
       if (event.keyCode == 9 || event.keyCode == 13) {
           document.getElementById("<%=drpCNCardChange.ClientID%>").selectedIndex = 0;
           var ret = GetTextData();
           if (ret != false) {
               var rangeToDisplay = "Range:" + strBegNO + "~" + strEndNO;
               ShowInfo(rangeToDisplay);
           }
           //OnClick();
       }
       
    }
    function drpOnChange() {
        /*var obj = document.getElementById("<%=drpCNCardChange.ClientID%>");
        outStatus = obj.value;
        var id = obj.selectedIndex;
        inStatus = obj[id].innerText;*/
        var obj = document.getElementById("<%=drpCNCardChange.ClientID%>");
        inStatus= obj.value;
        var id = obj.selectedIndex;
        outStatus = obj[id].innerText;
    }

    function CheckStatus() {
        if (currentStatus == inStatus
            && currentStatus != "") {
            DisplsyMsg(msgErrorSameStatus);
            ResetPage();
        }
    }
    function AddRowInfoForDN(RowArray) {
        try {
            if (indexDN <= initRowsCount) {
                eval("ChangeCvExtRowByIndex_" + table.id + "(RowArray,false, indexDN)");
            } else {
            eval("AddCvExtRowToBottom_" + table.id + "(RowArray,false)");
            }
            setSrollByIndex(1, false);
            indexDN++;
            var i = indexDN - 1;
            for (; i > 1; i--) {
                table.moveRow(i, i - 1);
            }

        } catch (e) {
            ShowInfo(e.description);
        }
    }
    function ResetPage() {
        document.getElementById("<%=lbRangeValue.ClientID%>").innerText = "";
        document.getElementById("<%=lbCountValue.ClientID%>").innerText = "";
        document.getElementById("<%=lbPNValue.ClientID%>").innerText = "";
        document.getElementById("<%=lbPlaceValue.ClientID%>").innerText = "";
        document.getElementById("<%=TextBox1.ClientID%>").value = "";
        document.getElementById("<%=TextBox2.ClientID%>").value = "";
        currentStatus = "";
        strBegNO = "";
        strEndNO = "";
        numEndNO = 0;
        numBegNO = 0;
        outStatus = "";
        inStatus = "";
        document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
        document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
        document.getElementById("<%=range.ClientID%>").value = "";
        document.getElementById("<%=TextBox1.ClientID%>").focus();
        document.getElementById("<%=drpCNCardChange.ClientID%>").selectedIndex = 0;
    }
    function getQueryPno(pno) {
        document.getElementById("<%=lbPNValue.ClientID%>").innerText = pno;
        var  rangeToDisplay = strBegNO + "~" + strEndNO;
        document.getElementById("<%=lbRangeValue.ClientID%>").innerText = rangeToDisplay;
        document.getElementById("<%=lbCountValue.ClientID%>").innerText = numEndNO - numBegNO + 1;
    }
    function getQueryPlace(place) {
        document.getElementById("<%=lbPlaceValue.ClientID%>").innerText = place;
    }
    function getQueryStatus(status) {
        currentStatus = status;
        if (currentStatus == "A1") {
            ResetPage();
            DisplsyMsg(msgA1);
            return;
            //document.getElementById("<%=btnSave.ClientID%>").disabled = true;
        }
        else {
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
        }
    }
    function OnClick() {
        document.getElementById("<%=begNO.ClientID%>").value = strBegNO;
        document.getElementById("<%=endNO.ClientID%>").value = strEndNO;
        document.getElementById("<%=range.ClientID%>").value = numEndNO - numBegNO + 1;
        document.getElementById("<%=btnGetCNList.ClientID%>").click();
    }
    function btnQuery_onclick() {
        document.getElementById("<%=drpCNCardChange.ClientID%>").selectedIndex = 0;
        if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
        || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
            strBegNO = "";
            strEndNO = "";
            document.getElementById("<%=TextBox1.ClientID%>").value = "";
            document.getElementById("<%=TextBox2.ClientID%>").value = "";
            ResetPage();
            DisplsyMsg(msgInputTxt);
            return;
        }
        if (strBegNO == "" || strEndNO == "") {
            if (false == GetTextData()) {
                return;
            }
        }
        if (strBegNO == "" || strEndNO == "") {
            ResetPage();
            DisplsyMsg(msgInputTxt);
            return;
        }
        OnClick();
    }
    function btnSave_onclick() {

        if (document.getElementById("<%=lbCountValue.ClientID%>").innerText == "") {
            DisplsyMsg(msgInputTxt);
            return;
        }
        
        if (strBegNO == "" || strEndNO == "") {
            ResetPage();
            DisplsyMsg(msgInputTxt);
            return;
        }
        if (inStatus == "") {
            DisplsyMsg(msgSelectTaget);
            return;
        }
        CheckStatus();
        if (currentStatus != "") {
            document.getElementById("<%=status.ClientID%>").value = inStatus;
            document.getElementById("<%=btnUpdateCNList.ClientID%>").click();
            var rowInfo = new Array();
            rowInfo.push(document.getElementById("<%=lbPNValue.ClientID%>").innerText);
            rowInfo.push(strBegNO);
            rowInfo.push(strEndNO);
            rowInfo.push(document.getElementById("<%=lbCountValue.ClientID%>").innerText);
            AddRowInfoForDN(rowInfo);
            var successTemp = "";
            var temp = strBegNO + "~" + strEndNO;
            ResetPage();
            //ShowInfo("Save success");
            if (temp != "") {
                successTemp = "[" + temp + "]Save," + msgSuccess;
            }
            if (successTemp != "") {
                //ShowSuccessfulInfo(true);
                ShowSuccessfulInfo(true, successTemp);
            }
            else {
                ShowSuccessfulInfo(true);
            }
        }
    }
   /* function btnSave_onclick() {
    
    
    
        if (currentStatus == "") {
            if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
        || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                if (strBegNO != "" && strEndNO != "") {

                }
                else {
                    document.getElementById("<%=TextBox1.ClientID%>").value = "";
                    document.getElementById("<%=TextBox2.ClientID%>").value = "";
                    ResetPage();
                    DisplsyMsg(msgInputTxt);
                    return;
                }
            }
            if (strBegNO == "" || strEndNO == "") {
                if (false == GetTextData()) {
                    return;
                }
            }
            if (strBegNO == "" || strEndNO == "") {
                ResetPage();
                DisplsyMsg(msgInputTxt);
                return;
            }
            OnClick();
        }
        if (currentStatus == "") {
            return;
        }
        if (currentStatus == "same")
        {
            DisplsyMsg(msgSame);
            return;
        }
        if (document.getElementById("<%=TextBox1.ClientID%>").value == ""
        || document.getElementById("<%=TextBox2.ClientID%>").value == "") {
            if (strBegNO != "" && strEndNO != "") {

            }
            else {
                document.getElementById("<%=TextBox1.ClientID%>").value = "";
                document.getElementById("<%=TextBox2.ClientID%>").value = "";
                ResetPage();
                DisplsyMsg(msgInputTxt);
                return;
            }
        }
        if (strBegNO == "" || strEndNO == "") {
            DisplsyMsg(msgInputTxt);
            return;
        }
        if (inStatus == "") {
            DisplsyMsg(msgSelectTaget);
            return;
        }
        CheckStatus();
        if (currentStatus != "") {
            document.getElementById("<%=status.ClientID%>").value = inStatus;
            document.getElementById("<%=btnUpdateCNList.ClientID%>").click();
            var rowInfo = new Array();
            rowInfo.push(document.getElementById("<%=lbPNValue.ClientID%>").innerText);
            rowInfo.push(strBegNO);
            rowInfo.push(strEndNO);
            rowInfo.push(document.getElementById("<%=lbCountValue.ClientID%>").innerText);
            AddRowInfoForDN(rowInfo);
            ResetPage();
            currentStatus = "same";
            ShowInfo("Save success");
        }
    }*/
    </script>

 <div>
   <center >
   <table width="95%" style="vertical-align:middle; height:20%" cellpadding="0" cellspacing="0" >
    <tr>
        <td align="left" >
         <br>
            <asp:Label ID="lbCardNo" runat="server" CssClass="iMes_DataEntryLabel" ></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"   style="width:35%" />
            <asp:Label ID="lbto"   runat="server" CssClass="iMes_DataEntryLabel" > ~ </asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"  Height="25px" BackColor="#ffffa0" BorderColor="Brown" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"   style="width:35%" />
        <input id="btnQuery" type="button"  runat="server" 
                class="iMes_button"  onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True" onclick="return btnQuery_onclick()"  />
        <br><br>
        </td>
        
    </tr>
       <tr>
        <td align="left" >
        <asp:Label ID="lbRange" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
        <asp:Label ID="lbRangeValue"  runat="server" CssClass="iMes_label_13pt" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbCount" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
        <asp:Label ID="lbCountValue" runat="server" CssClass="iMes_label_13pt" />
        <br><br>
        </td>
        </tr>
        <tr>
        <td align="left" >
        <asp:Label ID="lbPN"        runat="server" CssClass="iMes_label_13pt" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbPNValue"   runat="server" CssClass="iMes_label_13pt" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbPlace"     runat="server" CssClass="iMes_label_13pt" ></asp:Label>
        <asp:Label ID="lbPlaceValue" runat="server"  CssClass="iMes_label_13pt" />
        <br><br>
        </td>
      </tr>
        
      <tr>
        <td style="width:85%" align="left">
        <asp:Label ID="lbTarget" runat="server" CssClass="iMes_label_13pt" ></asp:Label>
        <asp:DropDownList runat="server" ID="drpCNCardChange" Width="50%"  >
         </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <input id="btnSave" type="button"  runat="server" 
                class="iMes_button" 
                onmouseover="this.className='iMes_button_onmouseover'" 
                onmouseout="this.className='iMes_button_onmouseout'" visible="True" 
                onclick="btnSave_onclick()"/>
         <br><br>
        </td>   
    </tr>
    
    
    <tr>
    <td colspan="4" align="left">
        <asp:UpdatePanel ID="updatePanelGrid" runat="server" UpdateMode="Conditional" >
          <ContentTemplate>
	         <iMES:GridViewExt ID="gridview" runat="server" AutoGenerateColumns="False" AutoHighlightScrollByValue="true" Width="99.9%"
                GetTemplateValueEnable="False" GvExtHeight="220px" GvExtWidth="100%" OnGvExtRowClick="" Height="210px" 
                OnGvExtRowDblClick="" SetTemplateValueEnable="False" HighLightRowPosition="3"
                onrowdatabound="GridViewExt1_RowDataBound" >
                <Columns>
                    <asp:BoundField DataField="HP P/N"  />
                    <asp:BoundField DataField="Begin No"  />
                    <asp:BoundField DataField="End No" />
                    <asp:BoundField DataField="Qty"  />
                </Columns>
             </iMES:GridViewExt>
          </ContentTemplate>   
         </asp:UpdatePanel> 
    </td>
</tr>
    <tr>
	
	<td>
	<asp:UpdatePanel ID="updatePanelAll" runat="server" RenderMode="Inline" >
		<ContentTemplate>
		  <button id="btnGetCNList" runat="server" type="button" style="display:none" ></button>	
		  <button id="btnUpdateCNList" runat="server" type="button" style="display:none" ></button>	
		  <input type="hidden" runat="server" id="begNO" /> 
		  <input type="hidden" runat="server" id="endNO" />
		  <input type="hidden" runat="server" id="range" />
		  <input type="hidden" runat="server" id="status" />
		</ContentTemplate>   
	</asp:UpdatePanel> 
    </td>
</tr>
    </table>
    </center>
</div>

</asp:Content>