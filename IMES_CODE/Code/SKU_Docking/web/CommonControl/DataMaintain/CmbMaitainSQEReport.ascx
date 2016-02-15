<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbMaitainSQEReport.ascx.cs" Inherits="CommonControl_DataMaintain_CmbMaitainSQEReport" %>
<asp:UpdatePanel ID="up" runat="server" RenderMode="Inline" UpdateMode="Conditional" >
<ContentTemplate>
    <asp:DropDownList ID="SQAReportUserControl" runat="server" Width="326px" AutoPostBack="true">
    </asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
