<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HP_UpLoadFile.aspx.cs" Inherits="HP_UpLoadFile" Title="HP_UpLoadFile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Condition Table： &nbsp;<asp:FileUpload ID="FuCT" runat="server" />
    <asp:Button ID="btnCT" runat="server" OnClick="btnCT_Click" Text="UpLoad" /><br />
    <br />
    Reference Table：&nbsp;
    <asp:FileUpload ID="FuRT" runat="server" />
    <asp:Button ID="btnRT" runat="server" OnClick="btnRT_Click" Text="Upload" /><br />
    <br />
    Second RT Table：<asp:FileUpload ID="FuNSRT" runat="server" />
    <asp:Button ID="btnNSRT" runat="server" Text="Upload" OnClick="btnNSRT_Click" /><br />
</asp:Content>

