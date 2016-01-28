<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UploadASPFlatFile.ascx.cs" Inherits="UploadASPFlatFile" %>
<asp:FileUpload ID="fuASPFlatFile" runat="server" />
<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />