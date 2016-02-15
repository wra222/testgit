<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbDBType.ascx.cs" Inherits="CommonControl_CmbDBType" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
<ContentTemplate>
   <asp:DropDownList ID="ddlDBType" runat="server" Width="150px"
    Font-Names="tahoma" AutoPostBack="True" 
    onselectedindexchanged="ddlDBType_SelectedIndexChanged">
    <asp:ListItem Value="OnlineDB" Text="OnlineDB"/>
    <asp:ListItem Value="HistoryDB" Text="HistoryDB"/>
    </asp:DropDownList>
    &nbsp;<asp:DropDownList ID="ddlDB" runat="server"  Width="150px">
    </asp:DropDownList>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlDBType"  EventName="SelectedIndexChanged" />                                  
    </Triggers>
</asp:UpdatePanel>