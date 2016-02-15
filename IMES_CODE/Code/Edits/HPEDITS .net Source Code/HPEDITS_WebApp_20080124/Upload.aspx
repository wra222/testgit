<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="Upload" Title="=== HP EDITS::Upload SAP flat files ===" %>

<%@ Register Src="UploadASPFlatFile.ascx" TagName="UploadASPFlatFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<table style="width:100%">
		<tr>
			<td>Comn.txt:
			</td>
			<td>
				<uc1:UploadASPFlatFile ID="fuComn" runat="server" FlatFile="Comn" />
			</td>
		</tr>
		<tr>
			<td>Paltno.txt:
			</td>
			<td>
				<uc1:UploadASPFlatFile ID="fuPaltno" runat="server" FlatFile="Paltno" />
			</td>
		</tr>
		<tr>
			<td>Edi850raw.txt
			</td>
			<td>
				<uc1:UploadASPFlatFile ID="fuEdi850raw" runat="server" FlatFile="Edi850raw" />
			</td>
		</tr>
		<tr>
			<td>Edi850rawINSTR.txt
			</td>
			<td>
				<uc1:UploadASPFlatFile ID="fuEdi850rawINSTR" runat="server" FlatFile="Edi850rawINSTR" />
			</td>
		</tr>				
	</table>
</asp:Content>

