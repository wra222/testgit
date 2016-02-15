<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FIXTure Input.aspx.cs" Inherits="SA_FIXTure_Input" %>
<%--<%@ MasterType VirtualPath="~/MasterPage.master" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="iMESContent" Runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Services>
            <asp:ServiceReference Path="~/SA/Service/WebServiceFIXTure.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="div1">
    <table style="width:100%;">
        <tr>
            <td style="width: 120px">
                <asp:Label ID="Label1" runat="server" Text="LOC"></asp:Label>
            </td>
            <td>
                <iMES:CmbConstValueType ID="cmbConstValueType1" runat="server"  Width="28" IsPercentage="true" IsSelectFirstNotNull="true" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="height: 32px; width: 120px">
                    <asp:Label ID="Label2" runat="server" Text="DataEntry"></asp:Label>
                </td>
            <td style="height: 32px">
                <iMES:Input ID="txtEntry" runat="server" ProcessQuickInput="true" CanUseKeyboard="true"
                        IsClear="true" IsPaste="true"/>
            </td>
            <td style="height: 32px">
            </td>
        </tr>
    </table>
    </div>
    <div id="div2">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  RenderMode="Inline">
        </asp:UpdatePanel>
    </div>
    
<script language="javascript" type="text/javascript">
        var loc;
        var editor;
        var dataEntryObj;
        var ct;


        window.onload = function() {
        editor = '<%=Editor %>';
            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                    dataEntryObj.focus();
                } catch (e) { }
            };

        function InputDataEntry(InputData) {
            if (document.getElementById("<%=cmbConstValueType1.InnerDropDownList.ClientID %>").value == "") {
                alert("Please Select Loc");
                getAvailableData("InputDataEntry");
                setConstValueTypeFocus();
                return;
            }
            if (InputData.length == 10) {
                ct = InputData;
                loc = document.getElementById("<%=cmbConstValueType1.InnerDropDownList.ClientID %>").value
                WebServiceFIXTure.Save(ct, loc, editor, SaveSucceed);
            }
            else {
                alert("Wrong Code,Please Input CT");
                dataEntryObj.focus();
                getAvailableData("InputDataEntry");
            }
        }
        
        function SaveSucceed(result) {
            if (result != null) {
                ShowSuccessfulInfo(true,"Save successfully!");
            }
            dataEntryObj.focus();
            getAvailableData("InputDataEntry");
        }

</script>
</asp:Content>

