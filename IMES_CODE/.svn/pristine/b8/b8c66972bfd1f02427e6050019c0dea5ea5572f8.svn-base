<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FIXTure Output.aspx.cs" Inherits="SA_FIXTure_Input" %>
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
                <asp:Label ID="Label1" runat="server" Text="PdLine"></asp:Label>
            </td>
            <td>
                <iMES:CmbPdLine ID="cmbPdLine" runat="server" Width="28"   IsPercentage="true" Stage="SA" />
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
        var pdline;
        var editor;
        var customer;
        var dataEntryObj;
        

        window.onload = function() {
        editor = '<%=Editor %>';
        customer = '<%=Customer %>';
            dataEntryObj = getCommonInputObject();
            getAvailableData("InputDataEntry");
            try {
                    dataEntryObj.focus();
                } catch (e) { }
            };

        function InputDataEntry(InputData) {
            if (getPdLineCmbValue() == "")
             {
                alert("please select Pdline");
                getAvailableData("InputDataEntry");
                setPdLineCmbFocus();
				return;
            }
            if (InputData.length == 10) {
                ct = InputData;
                pdline = getPdLineCmbValue();
                WebServiceFIXTure.OutSave(ct, pdline, editor, SaveSucceed);
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

