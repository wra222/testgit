<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmbExportExcel.ascx.cs" Inherits="CommonControl_CmbExportExcel" %>
 <script type="text/javascript" src="../../CommonControl/JS/iMESCommonUse.js"></script>

<button id="btnEpExcel" runat="server" onserverclick="btnEpExcel_Click" click="SetGrID()"
    style="width: 100px">Export</button>
<asp:HiddenField ID="hidExcelID" runat="server" />

