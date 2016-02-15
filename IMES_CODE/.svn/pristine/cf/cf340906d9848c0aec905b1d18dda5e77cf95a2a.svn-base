
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master"  ContentType="text/html;Charset=UTF-8" AutoEventWireup="true" CodeFile="~/PAK/WHInspection.aspx.cs"    Inherits="PAK_WHInspection" Title="Untitled Page" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">

<script type="text/javascript" src="../CommonControl/JS/iMESCommonUse.js"></script>
    
<style type="text/css">
</style>
 <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
            <Services>
                <asp:ServiceReference Path="Service/WebServiceWHInspection.asmx" />
            </Services>
        </asp:ScriptManager>
         
        <center>
            
            <table width="95%" style="height:200px; vertical-align:middle" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblMaterialType" Text="MaterialType:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="cmbMaterialType" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                </tr>    
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblInspectionResult" Text="InspectionResult:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="cmbInspectionResult" runat="server" Width="100%">
                            <asp:ListItem Text="Pass" Value="OK" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Fail" Value="NG"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblMaterialCT" Text="Material CT:" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label runat="server" ID="lblMaterialCTContent" CssClass="iMes_label_13pt"></asp:Label>
                    </td>
                </tr>
                <tr >
                    <td style="width:10%" align="left" >
                        <asp:Label ID="lblDataEntry" runat="server" class="iMes_DataEntryLabel"></asp:Label>
                    </td>
                    <td align="left" >
                        <iMES:Input ID="txt" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                        Width="99%" IsClear="true" IsPaste="true" />
                    </td>     
                </tr>
            </table>
        
        </center>
    </div>
    
    <script type="text/javascript">
        var msgDataEntryField = '<%=this.GetLocalResourceObject(Pre + "_msgDataEntryField").ToString() %>';
        var SUCCESSRET = "<%=WebConstant.SUCCESSRET%>";
        var inputObj = "";
        var inputData = "";
        var station = "";
        var materialct = "";
        var msgSystemError = '<%=Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_msgSystemError") %>';

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	onload
        //| Author		:	Jessica Liu
        //| Create Date	:	10/11/2011
        //| Description	:	加载接受输入数据事件
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        document.body.onload = function() {
            try {
                inputObj = getCommonInputObject();
                ShowInfo("");
                inputData = inputObj.value;
                inputObj.focus();
                station = '<%=Request["Station"] %>';
                getAvailableData("processDataEntry")

            } catch (e) {
                alert(e.description);
                inputObj.focus();
            }
        }
        
        function ShowALL(msg) {
            ShowMessage(msg);
            ShowInfo(msg);
        }

        function getMaterialTypeCmbValue() {
            return document.getElementById("<%=cmbMaterialType.ClientID %>").options[document.getElementById("<%=cmbMaterialType.ClientID %>").selectedIndex].value;
        }

        function getMaterialTypeCmbObj() {
            return document.getElementById("<%=cmbMaterialType.ClientID %>");
        }

        function getInspectionResultCmbValue() {
            return document.getElementById("<%=cmbInspectionResult.ClientID %>").options[document.getElementById("<%=cmbInspectionResult.ClientID %>").selectedIndex].value;
        }

        function getInspectionResultCmbObj() {
            return document.getElementById("<%=cmbInspectionResult.ClientID %>");
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //| Name		:	processDataEntry
        //| Author		:	Jessica Liu
        //| Create Date	:	10/12/2011
        //| Description	:	检测并进行数据检索及显示
        //| Input para.	:	
        //| Ret value	:	
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        function processDataEntry(data) {
            inputData = data;
            ShowInfo("");
            var materialtype = getMaterialTypeCmbValue();
            var inspectionresult = getInspectionResultCmbValue();

            if (materialtype == "") {
                alert("Please select MaterialType!");
                //ShowInfo("Please select MaterialType!");
                callNextInput();
                return;
            }

            if (inspectionresult == "") {
                alert("Please select InspectionResult!");
                //ShowInfo("Please select InspectionResult!");
                callNextInput();
                return;
            }


            if ((inputData.length < 12 || inputData.length > 14) && inputData.length != 4) {
                if (inputData.length >= 90) {
                    inputData = inputData.substr(76, 13);
                }
                else {
                    ShowInfo("Input Error");
                    callNextInput();
                    return;
                }
            }

            beginWaitingCoverDiv();
            materialct = inputData;
            WebServiceWHInspection.InputMaterialCT(inputData, materialtype, "", "<%=UserId%>", inspectionresult, "<%=Customer%>", onInputSucceed, onInputFail);
        }

        function onInputSucceed(result) {
            ShowInfo("");
            endWaitingCoverDiv();         
            try {
                if (result == null) {
                    var content = msgSystemError;
                    ShowMessage(content);
                    ShowInfo(content);
                }
                else if ((result.length == 2) && (result[0] == SUCCESSRET)) {
                    document.getElementById("<%=lblMaterialCTContent.ClientID %>").innerHTML = result[1];
                    materialct = "";
                    ShowSuccessfulInfo(true, "Success!");
                }
                else {
                    var content1 = result[0];
                    ShowMessage(content1);
                    ShowInfo(content1);
                }
                    callNextInput();
            } 
            catch (e) {
                alert(e.description);
            }
        }

        function onInputFail(error) {
            endWaitingCoverDiv();
            try {
                ShowInfo("");
                ShowMessage(error.get_message());
                ShowInfo(error.get_message());
                callNextInput();
            } catch (e) {
                alert(e.description);
            }
        }
     
        function callNextInput() 
        {
            inputObj.focus();
            getCommonInputObject().value = "";
            getAvailableData("processDataEntry");
        }

        window.onbeforeunload = function() {
            ExitPage();
        } 

        function ExitPage() 
        {
            if (materialct != "") {
                WebServiceWHInspection.Cancel(materialct);
            }
        }

        function resetAll() 
        {
            document.getElementById("<%=lblMaterialCTContent.ClientID %>").value = "";
            materialct = "";
            callNextInput();
        }

        function ResetPage() 
        {
            ExitPage();
            resetAll();
        }
    </script>
</asp:Content>
