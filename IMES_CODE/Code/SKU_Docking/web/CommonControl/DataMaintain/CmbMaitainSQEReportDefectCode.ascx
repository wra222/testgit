<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaitainSQEReportDefectCode.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaitainSQEReport" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="SQAReportUserControl1" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">

    function getSQAReportDefectCodeCmbObj() {

        try {
            return document.getElementById("<%=SQAReportUserControl1.ClientID %>");

        } catch (e) {
            alert(e.description);
        }

    }

    function getSQAReportDefectCodeCmbText() {

        try {
            return document.all("<%=SQAReportUserControl1.ClientID %>").options[document.all("<%=SQAReportUserControl1.ClientID %>").selectedIndex].text.trim();

        } catch (e) {
            alert(e.description);
        }

    }
    function getSQAReportDefectCodeCmbValue() {

        try {
            return document.all("<%=SQAReportUserControl1.ClientID %>").options[document.all("<%=SQAReportUserControl1.ClientID %>").selectedIndex].value.trim();

        } catch (e) {
            alert(e.description);
        }

    }
    function checkSQAReportDefectCodeCmb() {

        try {
            var msgSelectNullCmb = "";
            if (getTypeDescCmbText() == "") {
                ShowMessage(msgSelectNullCmb);
                return false;
            }

            return true;

        } catch (e) {
            alert(e.description);
        }

    }

    function setSQAReportDefectCodeCmbFocus() {

        try {
            getSQAReportDefectCodeCmbObj().focus();

        } catch (e) {
            alert(e.description);
        }

    }              
</script>

