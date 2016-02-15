<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IDPrompt.ascx.cs" Inherits="IDPrompt" %>
<asp:Label ID="Label1" runat="server" Text="Internal ID: "></asp:Label>
<asp:TextBox
    ID="txtInternalID" runat="server" ValidationGroup="InternalID"></asp:TextBox><br />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="InternalID" ErrorMessage="You need to fill in Internal ID" ControlToValidate="txtInternalID"></asp:RequiredFieldValidator>